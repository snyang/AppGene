using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppGene.Model.EntityPerception
{
    public class SortPropertyGetter
    {
        /// <summary>
        /// Gets the sort columns.
        /// </summary>
        /// <param name="context">The entity analysis context.</param>
        /// <returns>The sort columns.</returns>
        public virtual IList<SortPropertyInfo> Get(EntityAnalysisContext context)
        {
            IList<SortPropertyInfo> sortProperties = new List<SortPropertyInfo>();

            // Find sort columns from DisplayColumnAttributes
            var displayColumnAttribute = context.EntityType.GetCustomAttribute<DisplayColumnAttribute>();
            if (displayColumnAttribute != null)
            {
                PropertyInfo sortProperty = context.EntityType.GetProperty(displayColumnAttribute.SortColumn);
                if (sortProperty != null)
                {
                    sortProperties.Add(new SortPropertyInfo
                    {
                        PropertyInfo = sortProperty,
                        SortDescending = displayColumnAttribute.SortDescending
                    });

                    // Return
                    return sortProperties;
                }
            }

            // Find sort columns by characteristic
            string[] sortCharacteristic = new string[] { "Order", "Name", "Code", "Id" };
            var properties = EntityAnalysisHelper.GetCharacteristicPropertyInfo(context.EntityType, sortCharacteristic);

            foreach (var character in sortCharacteristic)
            {
                PropertyInfo property;
                if (properties.TryGetValue(character, out property))
                {
                    sortProperties.Add(new SortPropertyInfo
                    {
                        PropertyInfo = property,
                    });
                }
            }

            return sortProperties;
        }
    }
}