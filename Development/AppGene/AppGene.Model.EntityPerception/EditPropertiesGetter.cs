﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppGene.Model.EntityPerception
{
    public class EditPropertiesGetter
    {
        /// <summary>
        /// Gets the edit columns.
        /// </summary>
        /// <param name="context">The entity analysis context.</param>
        /// <returns>The edit columns.</returns>
        public virtual IList<EditPropertyInfo> Get(EntityAnalysisContext context)
        {
            var editProperties = new List<EditPropertyInfo>();
            var properties = context.EntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (EntityAnalysisHelper.IsCharacteristicProperty(context.EntityType,
                    property,
                    "id"))
                {
                    continue;
                }

                var columnInfo = GetColumnInfo(context.EntityType, property);
                editProperties.Add(columnInfo);
            }

            return editProperties;
        }

        private static void ConfigureColumnInfo(EditPropertyInfo columnInfo,
            Type entityType,
            PropertyInfo property,
            DisplayAttribute displayAttribute)
        {
            if (displayAttribute == null)
            {
                columnInfo.Name = EntityAnalysisHelper.GetPropertyDisplayName(property.Name);
                columnInfo.ShortName = EntityAnalysisHelper.ConverNameToShortName(entityType, columnInfo.Name);
                return;
            }
            else if (displayAttribute.ResourceType == null)
            {
                columnInfo.Name = displayAttribute.Name;
                columnInfo.ShortName = displayAttribute.ShortName;
                columnInfo.Description = displayAttribute.Description;
                columnInfo.Prompt = displayAttribute.Prompt;
            }
            else
            {
                columnInfo.Name = EntityAnalysisHelper.GetResourceString(displayAttribute.ResourceType, displayAttribute.Name);
                columnInfo.ShortName = EntityAnalysisHelper.GetResourceString(displayAttribute.ResourceType, displayAttribute.ShortName);
                if (string.IsNullOrEmpty(columnInfo.ShortName)) columnInfo.ShortName = columnInfo.Name;
                columnInfo.Description = EntityAnalysisHelper.GetResourceString(displayAttribute.ResourceType, displayAttribute.Description);
                columnInfo.Prompt = EntityAnalysisHelper.GetResourceString(displayAttribute.ResourceType, displayAttribute.Prompt);
            }

            if (displayAttribute.GetOrder().HasValue)
            {
                columnInfo.Order = displayAttribute.Order;
            }
        }

        private static void ConfigureColumnInfo(EditPropertyInfo columnInfo, EditableAttribute editableAttribute)
        {
            if (editableAttribute == null) return;

            columnInfo.ReadOnly = !editableAttribute.AllowEdit;
        }

        private static void ConfigureColumnInfo(EditPropertyInfo columnInfo, StringLengthAttribute stringLengthAttribute)
        {
            if (stringLengthAttribute == null) return;
            columnInfo.ContentLength = stringLengthAttribute.MaximumLength;
        }

        private static void ConfigureColumnInfo(EditPropertyInfo columnInfo, MaxLengthAttribute maxLengthAttribute)
        {
            if (maxLengthAttribute == null) return;
            columnInfo.ContentLength = maxLengthAttribute.Length;
        }

        private static EditPropertyInfo GetColumnInfo(Type entityType, PropertyInfo property)
        {
            var columnInfo = new EditPropertyInfo()
            {
                PropertyInfo = property
            };

            var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
            ConfigureColumnInfo(columnInfo, entityType, property, displayAttribute);

            var maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>();
            ConfigureColumnInfo(columnInfo, maxLengthAttribute);

            var stringLengthAttribute = property.GetCustomAttribute<StringLengthAttribute>();
            ConfigureColumnInfo(columnInfo, stringLengthAttribute);

            var editableAttribute = property.GetCustomAttribute<EditableAttribute>();
            ConfigureColumnInfo(columnInfo, editableAttribute);

            var displayFormatAttribute = property.GetCustomAttribute<DisplayFormatAttribute>();
            ConfigureColumnInfo(columnInfo, displayFormatAttribute);
            return columnInfo;
        }

        private static void ConfigureColumnInfo(EditPropertyInfo columnInfo, DisplayFormatAttribute displayFormatAttribute)
        {
            if (displayFormatAttribute == null) return;
            columnInfo.DisplayFormat = displayFormatAttribute.DataFormatString;
        }
    }
}