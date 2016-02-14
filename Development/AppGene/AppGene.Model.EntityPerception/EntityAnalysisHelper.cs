using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppGene.Model.EntityPerception
{
    public class EntityAnalysisHelper
    {
        /// <summary>
        /// Convert name to short name.
        /// E.g.
        /// EmployeeName => Name
        /// Gender => Gender
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static string ConverNameToShortName(Type entityType, string name)
        {
            string shortName = name.StartsWith(entityType.Name)
                ? name.Substring(entityType.Name.Length)
                : name;

            return shortName;
        }

        /// <summary>
        /// Get the characteristic property info objects.
        /// For example:
        /// * Get the characteristic property info object for characteristic 'id' for entity Employee.
        /// The method will return property info of Id or EmployeeId if there is.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="characteristic">The array of characteristic.</param>
        /// <returns>The characteristic property info objects.</returns>
        public static IDictionary<string, PropertyInfo> GetCharacteristicPropertyInfo(Type entityType, string[] characteristic)
        {
            IDictionary<string, PropertyInfo> returnProperties = new Dictionary<string, PropertyInfo>();
            string entityName = entityType.Name.ToLowerInvariant();
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                foreach (var character in characteristic)
                {
                    if (property.Name.ToLowerInvariant() == character.ToLowerInvariant())
                    {
                        returnProperties.Remove(character);
                        returnProperties.Add(character, property);
                    }
                    else if (property.Name.Replace("_", "").ToLowerInvariant() == entityName + character.ToLowerInvariant()
                        && !returnProperties.ContainsKey(character))
                    {
                        returnProperties.Add(character, property);
                    }
                }
            }

            return returnProperties;
        }

        /// <summary>
        /// Get the characteristic property info object.
        /// For example:
        /// * Get the characteristic property info object for characteristic 'id' for entity Employee.
        /// The method will return property info of Id or EmployeeId if there is.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="characteristic">The array of characteristic.</param>
        /// <returns>The characteristic property info object.</returns>
        public static PropertyInfo GetCharacteristicPropertyInfo(Type entityType, string characteristic)
        {
            IDictionary<string, PropertyInfo> returnProperties = GetCharacteristicPropertyInfo(entityType, new string[] { characteristic });

            return returnProperties.Count > 0 ? returnProperties.ElementAt(0).Value : null;
        }

        /// <summary>
        /// Get display name of a property.
        /// For example:
        /// ID => ID
        /// EmployeeName => Employee Name
        /// EmployeeXMLName => Employee XML Name
        /// EmployeeXML => Employee XML
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Display name</returns>
        public static string GetPropertyDisplayName(string propertyName)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            if (string.IsNullOrEmpty(propertyName)) return "";

            StringBuilder nameBuilder = new StringBuilder();
            for (int i = 0; i < propertyName.Length; i++)
            {
                char letter = propertyName[i];
                if (i + 1 < propertyName.Length)
                {
                    char next = propertyName[i + 1];
                    if (Char.IsLower(next))
                    {
                        nameBuilder.Append(" ");
                    }
                }
                nameBuilder.Append(letter);
            }

            return nameBuilder.ToString();
        }

        /// <summary>
        /// Returns a resource property string value base on specific resource type and property name.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A string value of the property from the resource type.</returns>
        public static string GetResourceString(Type resourceType, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return "";

            if (resourceType == null) return propertyName;

            try
            {
                var resourceProp = resourceType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                return resourceProp.GetValue(null) as String;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return propertyName;
            }
        }

        /// <summary>
        /// Return if the property is characteristic property.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="property">The property.</param>
        /// <param name="characteristic">The characteristic names.</param>
        /// <returns>Return if the property is characteristic property.</returns>
        public static bool IsCharacteristicProperty(Type entityType,
            PropertyInfo property,
            string[] characteristic)
        {
            string entityName = entityType.Name.ToLowerInvariant();
            foreach (var character in characteristic)
            {
                if (property.Name.ToLowerInvariant() == character.ToLowerInvariant())
                {
                    return true;
                }
                else if (property.Name.Replace("_", "").ToLowerInvariant() == entityName + character.ToLowerInvariant())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return if the property is characteristic property.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="property">The property.</param>
        /// <param name="characteristic">The characteristic name.</param>
        /// <returns>Return if the property is characteristic property.</returns>
        public static bool IsCharacteristicProperty(Type entityType,
            PropertyInfo property,
            string characteristic)
        {
            return IsCharacteristicProperty(entityType, property, new string[] { characteristic });
        }
    }
}