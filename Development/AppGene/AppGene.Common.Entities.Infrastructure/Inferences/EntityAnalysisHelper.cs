using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
    public static class EntityAnalysisHelper
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
        public static string ConvertNameToShortName(PropertyInfo property, string name)
        {
            string className = property.DeclaringType.Name;
            string shortName = name.StartsWith(className, StringComparison.OrdinalIgnoreCase)
                ? name.Substring(className.Length).Trim()
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
            string entityName = entityType.Name.ToUpperInvariant();
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                foreach (var character in characteristic)
                {
                    if (property.Name.ToUpperInvariant() == character.ToUpperInvariant())
                    {
                        returnProperties.Remove(character);
                        returnProperties.Add(character, property);
                    }
                    else if (property.Name.Replace("_", "").ToUpperInvariant() == entityName + character.ToUpperInvariant()
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
        /// Gets display name of a property.
        /// For example:
        /// ID => ID
        /// EmployeeName => Employee Name
        /// EmployeeXMLName => Employee XML Name
        /// EmployeeXML => Employee XML
        /// Employee_XML => Employee XML
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Display name</returns>
        public static string GetPropertyDisplayName(string propertyName)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            if (string.IsNullOrEmpty(propertyName)) return "";

            StringInfo propertyNameInfo = new StringInfo(propertyName);
            StringBuilder nameBuilder = new StringBuilder();
            string currentUpperCaseSection = "";
            for (int i = 0; i < propertyNameInfo.LengthInTextElements; i++)
            {
                bool hasContentBefore = nameBuilder.Length > 0;
                string current = propertyNameInfo.SubstringByTextElements(i, 1);
                bool currentIsUpper = Char.IsUpper(current, 0);
                if (currentIsUpper)
                {
                    if (hasContentBefore && currentUpperCaseSection.Length == 0)
                    {
                        currentUpperCaseSection = " ";
                    }
                    currentUpperCaseSection = currentUpperCaseSection + current;
                    continue;
                }
                else if (currentUpperCaseSection.Length > 0)
                {
                    currentUpperCaseSection = currentUpperCaseSection.TrimEnd();
                    if (currentUpperCaseSection.Length > 2)
                    {
                        nameBuilder.Append(currentUpperCaseSection.Substring(0, currentUpperCaseSection.Length - 1));
                        nameBuilder.Append(" ");
                        currentUpperCaseSection = currentUpperCaseSection.Substring(currentUpperCaseSection.Length - 1);
                    }
                    nameBuilder.Append(currentUpperCaseSection);
                    currentUpperCaseSection = "";
                }

                if (string.Equals(current, "_", StringComparison.OrdinalIgnoreCase))
                {
                    // Convert "_" as " "
                    if (hasContentBefore && currentUpperCaseSection.Length == 0)
                    {
                        currentUpperCaseSection = " ";
                    }
                    continue;
                }
                
                nameBuilder.Append(current);
            }

            if (currentUpperCaseSection.Length > 0)
            {
                nameBuilder.Append(currentUpperCaseSection.TrimEnd());
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
            string entityName = property.DeclaringType.Name.ToUpperInvariant();
            foreach (var character in characteristic)
            {
                if (property.Name.ToUpperInvariant() == character.ToUpperInvariant())
                {
                    return true;
                }
                else if (property.Name.Replace("_", "").ToUpperInvariant() == entityName + character.ToUpperInvariant())
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