using AppGene.Common.Entities.Infrastructure.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
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

            ConfigureOrder(displayProperties);

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
            displayPropertyInfo.DependencyPropertyTypeName = dependencyColumnAttribute.DependencyPropertyTypeName;
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

            //displayPropertyInfo.ComputeComputedPropertyNames = computedColumnAttribute.ComputedColumns;
        }

        private static void ConfigurePropertyInfoHidden(DisplayPropertyInfo displayPropertyInfo, PropertyInfo property)
        {
            if (displayPropertyInfo.IsDependencyProperty)
            {
                displayPropertyInfo.IsHidden = true;
            }
            if (displayPropertyInfo.Order == -1)
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

        private void ConfigureOrder(List<DisplayPropertyInfo> displayProperties)
        {
            // Sort: the order is Order > 0; Order = 0; IsDependencyProperty
            displayProperties.Sort(new Comparer());

            // put cumputed property (when order = 0) after the last reference property
            for (int i = 0; i < displayProperties.Count; i++)
            {
                DisplayPropertyInfo displayProperty = displayProperties[i];
                if (displayProperty.Order == 0 && displayProperty.IsComputed)
                {
                    int lastReferencePropertyIndex = GetLastReferencePropertyIndex(displayProperties, displayProperty.ComputeReferencePropertyNames);
                    if (lastReferencePropertyIndex == -1)
                    {
                        Debug.WriteLine(string.Format("{0} computedProperty setting is incorrect, cannot find reference property names.",
                            displayProperty.PropertyName));
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
            int order = 1;
            foreach (var displayProperty in displayProperties)
            {
                if (displayProperty.IsDependencyProperty)
                {
                    displayProperty.Order = -1;
                }
                else if (displayProperty.Order >= 0)
                {
                    displayProperty.Order = order;
                    order++;
                }
            }

            // Sort: the order is Order > 0; IsDependencyProperty
            displayProperties.Sort(new Comparer());
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

        private class Comparer :
            IComparer<DisplayPropertyInfo>
        {
            /// <summary>
            /// The order is:
            /// * Order (order > 0)
            /// * Order == 0
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

                if ((x.Order < 0) != (y.Order < 0))
                {
                    if (x.Order < 0) return -1;
                    if (y.Order < 0) return 1;
                }

                if ((x.Order == 0) != (y.Order == 0))
                {
                    if (x.Order == 0) return -1;
                    if (y.Order == 0) return 1;
                }

                return x.Order - y.Order;
            }
        }
    }
}