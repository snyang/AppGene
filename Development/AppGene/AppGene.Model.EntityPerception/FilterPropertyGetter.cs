using AppGene.Model.DataAnnotations;
using System.Collections.Generic;
using System.Reflection;

namespace AppGene.Model.EntityPerception
{
    public class FilterPropertyGetter
    {
        /// <summary>
        /// Gets the sort columns.
        /// </summary>
        /// <param name="context">The entity analysis context.</param>
        /// <returns>The sort columns.</returns>
        public virtual IList<PropertyInfo> Get(EntityAnalysisContext context)
        {
            var filterProperties = new List<PropertyInfo>();

            // Find filter columns by FilterAttribute
            var allProperties = context.EntityType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in allProperties)
            {
                if (property.GetCustomAttribute<FilterAttribute>() != null)
                {
                    filterProperties.Add(property);
                }
            }
            if (filterProperties.Count > 0)
            {
                return filterProperties;
            }

            // Find filter columns by characteristic
            string[] filterCharacteristic = new string[] { "Name", "Code" };
            var properties = EntityAnalysisHelper.GetCharacteristicPropertyInfo(context.EntityType, filterCharacteristic);

            foreach (var character in filterCharacteristic)
            {
                PropertyInfo property;
                if (properties.TryGetValue(character, out property))
                {
                    filterProperties.Add(property);
                }
            }

            return filterProperties;
        }
    }
}