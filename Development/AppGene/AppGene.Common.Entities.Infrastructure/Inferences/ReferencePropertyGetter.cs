﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
    public class ReferencePropertyGetter
    {
        /// <summary>
        /// Gets the reference columns.
        /// </summary>
        /// <param name="context">The entity analysis context.</param>
        /// <returns>The reference columns.</returns>
        public virtual IList<PropertyInfo> GetProperties(EntityAnalysisContext context)
        {
            var displayProperties = new List<PropertyInfo>();

            // Find sort columns from DisplayColumnAttributes
            var displayColumnAttribute = context.EntityType.GetCustomAttribute<DisplayColumnAttribute>();
            if (displayColumnAttribute != null)
            {
                string[] displayColumns = displayColumnAttribute.DisplayColumn.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var column in displayColumns)
                {
                    var columnName = column.Trim();
                    PropertyInfo displayProperty = context.EntityType.GetProperty(columnName);
                    Debug.Assert(displayProperty != null);
                    displayProperties.Add(displayProperty);
                }

                if (displayProperties.Count > 0)
                {
                    // Return
                    return displayProperties;
                }
            }

            // Find display columns by characteristic
            string[] displayCharacteristic = new string[] { "Code", "Name" };
            var properties = EntityAnalysisHelper.GetCharacteristicPropertyInfo(context.EntityType, displayCharacteristic);

            foreach (var character in displayCharacteristic)
            {
                PropertyInfo property;
                if (properties.TryGetValue(character, out property))
                {
                    displayProperties.Add(property);
                }
            }

            return displayProperties;
        }
    }
}