using AppGene.Common.Core.Logging;
using AppGene.Common.Entities.Infrastructure.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
    /// <summary>
    /// The class provides functions to return a list of <ref="DisplayPropertyInfo">
    /// from a specific model class.
    /// </summary>
    public class DisplayPropertiesGetter
    {

        public EntityAnalysisContext Context { get; set; }

        /// <summary>
        /// Gets the display properties.
        /// </summary>
        /// <param name="context">The entity analysis context.</param>
        /// <returns>The display properties.</returns>
        public virtual IList<DisplayPropertyInfo> GetProperties(EntityAnalysisContext context)
        {
            Context = context;
            var displayProperties = new List<DisplayPropertyInfo>();
            var properties = Context.EntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                Context.PropertyInfo = property;
                var displayPropertyInfo = GetDisplayPropertyInfo(property);
                displayProperties.Add(displayPropertyInfo);
            }

            ConfigurePropertiesOrder(context, displayProperties);

            return displayProperties;
        }

        private void ConfigurePropertyInfo(DisplayPropertyInfo displayPropertyInfo,
            PropertyInfo property,
            DisplayAttribute displayAttribute)
        {
            if (displayAttribute == null)
            {
                displayPropertyInfo.Name = EntityAnalysisHelper.GetPropertyDisplayName(property.Name);
                displayPropertyInfo.ShortName = EntityAnalysisHelper.ConvertNameToShortName(property, displayPropertyInfo.Name);
                return;
            }
            else if (displayAttribute.ResourceType == null)
            {
                displayPropertyInfo.Name = displayAttribute.Name;
                displayPropertyInfo.ShortName = displayAttribute.ShortName;
                displayPropertyInfo.Description = displayAttribute.Description;
                displayPropertyInfo.Prompt = displayAttribute.Prompt;
            }
            else
            {
                displayPropertyInfo.Name = EntityAnalysisHelper.GetResourceString(displayAttribute.ResourceType, displayAttribute.Name);
                displayPropertyInfo.ShortName = EntityAnalysisHelper.GetResourceString(displayAttribute.ResourceType, displayAttribute.ShortName);
                if (string.IsNullOrEmpty(displayPropertyInfo.ShortName)) displayPropertyInfo.ShortName = displayPropertyInfo.Name;
                displayPropertyInfo.Description = EntityAnalysisHelper.GetResourceString(displayAttribute.ResourceType, displayAttribute.Description);
                displayPropertyInfo.Prompt = EntityAnalysisHelper.GetResourceString(displayAttribute.ResourceType, displayAttribute.Prompt);
            }

            if (displayAttribute.GetOrder().HasValue)
            {
                displayPropertyInfo.Order = displayAttribute.Order;
            }
        }

        private static void ConfigurePropertyInfo(DisplayPropertyInfo displayPropertyInfo, EditableAttribute editableAttribute)
        {
            if (editableAttribute == null) return;

            displayPropertyInfo.IsReadOnly = !editableAttribute.AllowEdit;
        }

        private static void ConfigurePropertyInfo(DisplayPropertyInfo displayPropertyInfo, StringLengthAttribute stringLengthAttribute)
        {
            if (stringLengthAttribute == null) return;
            displayPropertyInfo.ContentLength = stringLengthAttribute.MaximumLength;
        }

        private static void ConfigurePropertyInfo(DisplayPropertyInfo displayPropertyInfo, MaxLengthAttribute maxLengthAttribute)
        {
            if (maxLengthAttribute == null) return;
            displayPropertyInfo.ContentLength = maxLengthAttribute.Length;
        }

        private static void ConfigurePropertyInfo(DisplayPropertyInfo displayPropertyInfo, DisplayFormatAttribute displayFormatAttribute)
        {
            if (displayFormatAttribute == null) return;
            displayPropertyInfo.DisplayFormat = displayFormatAttribute.DataFormatString;
        }

        private void ConfigurePropertyInfo(DisplayPropertyInfo displayPropertyInfo, DependencyColumnAttribute dependencyColumnAttribute)
        {
            if (dependencyColumnAttribute == null)
            {
                new DependencyPropertyConvention().Apply(Context, displayPropertyInfo);
                return;
            }
            displayPropertyInfo.IsDependencyProperty = true;
            displayPropertyInfo.LogicalProperty = dependencyColumnAttribute.LogicalProperty;
            displayPropertyInfo.DependencyPropertyName = dependencyColumnAttribute.DependencyPropertyTypeName;
        }

        private void ConfigurePropertyInfo(DisplayPropertyInfo displayPropertyInfo, HiddenColumnAttribute hiddenColumnAttribute)
        {
            if (hiddenColumnAttribute == null)
            {
                new HiddenPropertyConvention().Apply(Context, displayPropertyInfo);
                return;
            }
            displayPropertyInfo.IsHidden = hiddenColumnAttribute.Hidden;
        }

        private void ConfigurePropertyInfo(DisplayPropertyInfo displayPropertyInfo, 
            ColumnTypeAttribute columnTypeAttribute)
        {
            if (columnTypeAttribute == null)
            {
                new DateTypeConvention().Apply(Context, displayPropertyInfo);
                return;
            }
            displayPropertyInfo.ConverterTypeName = columnTypeAttribute.ConverterTypeName;
            displayPropertyInfo.CustomType = columnTypeAttribute.CustomType;
            displayPropertyInfo.LogicalDataType = columnTypeAttribute.LogicalDataType;
            if (displayPropertyInfo.PropertyDataType.Equals(typeof(DateTime))
                && displayPropertyInfo.LogicalDataType== LogicalDataType.Default)
            {
                new DateTypeConvention().Apply(Context, displayPropertyInfo);
            }
        }

        private static void ConfigurePropertyInfo(DisplayPropertyInfo displayPropertyInfo, ComputeRelationshipAttribute computedColumnAttribute)
        {
            if (computedColumnAttribute == null) return;
            
            displayPropertyInfo.ComputeReferencePropertyNames = computedColumnAttribute.ReferenceColumns;
            if (computedColumnAttribute.ReferenceColumns.Length > 0)
            { 
                displayPropertyInfo.IsComputed = true;
            }
        }

        private static void ConfigurePropertyInfoHidden(DisplayPropertyInfo displayPropertyInfo, PropertyInfo property)
        {
            if (displayPropertyInfo.IsDependencyProperty)
            {
                displayPropertyInfo.IsHidden = true;
            }
        }

        private static void ConfigurePropertyInfoReadOnly(DisplayPropertyInfo displayPropertyInfo, PropertyInfo property)
        {
            if (!property.CanWrite)
            {
                displayPropertyInfo.IsReadOnly = true;
            }
        }

        private DisplayPropertyInfo GetDisplayPropertyInfo(PropertyInfo property)
        {
            var displayPropertyInfo = new DisplayPropertyInfo()
            {
                PropertyInfo = property
            };

            var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
            ConfigurePropertyInfo(displayPropertyInfo, property, displayAttribute);

            var maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>();
            ConfigurePropertyInfo(displayPropertyInfo, maxLengthAttribute);

            var stringLengthAttribute = property.GetCustomAttribute<StringLengthAttribute>();
            ConfigurePropertyInfo(displayPropertyInfo, stringLengthAttribute);

            var editableAttribute = property.GetCustomAttribute<EditableAttribute>();
            ConfigurePropertyInfo(displayPropertyInfo, editableAttribute);

            var displayFormatAttribute = property.GetCustomAttribute<DisplayFormatAttribute>();
            ConfigurePropertyInfo(displayPropertyInfo, displayFormatAttribute);

            var dependencyColumnAttribute = property.GetCustomAttribute<DependencyColumnAttribute>();
            ConfigurePropertyInfo(displayPropertyInfo, dependencyColumnAttribute);

            var computedColumnAttribute = property.GetCustomAttribute<ComputeRelationshipAttribute>();
            ConfigurePropertyInfo(displayPropertyInfo, computedColumnAttribute);

            var columnTypeAttribute = property.GetCustomAttribute<ColumnTypeAttribute>();
            ConfigurePropertyInfo(displayPropertyInfo, columnTypeAttribute);

            var hiddenColumnAttribute = property.GetCustomAttribute<HiddenColumnAttribute>();
            ConfigurePropertyInfo(displayPropertyInfo, hiddenColumnAttribute);

            ConfigurePropertyInfoReadOnly(displayPropertyInfo, property);
            ConfigurePropertyInfoHidden(displayPropertyInfo, property);

            return displayPropertyInfo;
        }

        /// <summary>
        /// # Order logical
        /// * If a <ref="ModelDisplayAttribut"> is applied, use the order in the attribute.
        /// * Otherwise, use the Order property value.
        /// * If the Order property value are same, the property in the model class overs the property in the entity class. 
        /// * If these 2 properties are in same class, use the declaration order.
        /// * If the Order property is not set and the property is a computed property, set the order after the last computing property.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="displayProperties"></param>
        private void ConfigurePropertiesOrder(EntityAnalysisContext context,
            List<DisplayPropertyInfo> displayProperties)
        {
            var modelDisplayAttribute = context.EntityType.GetCustomAttribute<ModelDisplayAttribute>();
            if (modelDisplayAttribute != null 
                && modelDisplayAttribute.DisplayedColumns != null 
                && modelDisplayAttribute.DisplayedColumns.Length > 0)
            { 
                // Configure order by using ModelDisplayAttribute
                ConfigurePropertiesOrder(displayProperties, modelDisplayAttribute);
                return;
            }

            // Configure order with order settings

            // Sort
            displayProperties.Sort(new PropertyOrderComparer());

            // put cumputed property (when order = 0) after the last reference property
            for (int i = 0; i < displayProperties.Count; i++)
            {
                DisplayPropertyInfo displayProperty = displayProperties[i];
                if (!displayProperty.HasOrder 
                    && displayProperty.IsComputed
                    && !displayProperty.IsDependencyProperty)
                {
                    int lastReferencePropertyIndex = GetLastReferencePropertyIndex(displayProperties, displayProperty.ComputeReferencePropertyNames);
                    if (lastReferencePropertyIndex == -1)
                    {
                        LoggerFactory.GetLogger().Warn(
                            "The setting of ComputeRelationship of property '{0}' is incorrect, cannot find reference property names.",
                            displayProperty.PropertyName);
                        continue;

                    }
                    if (lastReferencePropertyIndex < i)
                    {
                        // forward move
                        if (lastReferencePropertyIndex != i - 1)  // not current postition
                        {
                            displayProperties.Remove(displayProperty);
                            displayProperties.Insert(lastReferencePropertyIndex + 1, displayProperty);
                        }
                    }
                    else if (lastReferencePropertyIndex > i)
                    {
                        // backward move
                        displayProperties.Remove(displayProperty);
                        displayProperties.Insert(lastReferencePropertyIndex, displayProperty);
                        i--;
                    }
                }
            }

            // set order
            int order = 0;
            foreach (var displayProperty in displayProperties)
            {
                displayProperty.Order = order;
                order++;
            }

            // Sort
            displayProperties.Sort(new PropertyOrderComparer());
        }

        /// <summary>
        /// If the name of a property is defined in the ModelDisplayAttribute, set the order as the sequence in the attribute,
        /// otherwise, hidden the property.
        /// </summary>
        /// <param name="displayProperties"></param>
        /// <param name="modelDisplayAttribute"></param>
        private void ConfigurePropertiesOrder(List<DisplayPropertyInfo> displayProperties, ModelDisplayAttribute modelDisplayAttribute)
        {
            // clear all order settings
            foreach(var property in displayProperties)
            {
                property.IsHidden = true;
                if (property.HasOrder) { 
                    property.Order = int.MaxValue;
                }
            }

            int order = 0;
            foreach (var columnName in modelDisplayAttribute.DisplayedColumns)
            {
                foreach (var property in displayProperties)
                {
                    if (property.PropertyName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        property.IsHidden = false;
                        property.Order = order;
                        order++;
                        break;
                    }
                }
            }
        }

        private int GetLastReferencePropertyIndex(List<DisplayPropertyInfo> displayProperties, string[] computedReferencePropertyNames)
        {
            HashSet<string> searchedNames = new HashSet<string>(computedReferencePropertyNames);
            int lastIndex = -1;
            for (int i = 0; i < displayProperties.Count; i++)
            {
                DisplayPropertyInfo displayProperty = displayProperties[i];
                if (searchedNames.Contains(displayProperty.PropertyName))
                {
                    if (i > lastIndex) lastIndex = i;
                }
            }

            return lastIndex;
        }

        private class PropertyOrderComparer :
            IComparer<DisplayPropertyInfo>
        {
            /// <summary>
            /// The order is:
            /// * Order
            /// * IsDependencyProperty
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(DisplayPropertyInfo x, DisplayPropertyInfo y)
            {
                if (x.IsDependencyProperty != y.IsDependencyProperty)
                {
                    if (x.IsDependencyProperty) return 1;
                    if (y.IsDependencyProperty) return -1;
                }

                return x.Order - y.Order;
            }
        }
    }
}