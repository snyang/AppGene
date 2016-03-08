using AppGene.Common.Entities.Infrastructure.Annotations;
using System;
using System.Reflection;
using System.Windows;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
    public class DisplayPropertyInfo
    {
        private bool isHidden;

        /// <summary>
        /// Gets or sets computed property names when the property is a computed property.
        /// </summary>
        // TODO refine the name
        public string[] ComputeComputedPropertyNames { get; set; }

        /// <summary>
        /// Gets or sets reference property names when the property is a computed property.
        /// </summary>
        public string[] ComputeReferencePropertyNames { get; set; }
        /// <summary>
        /// Gets or sets a value that is used to set the length for the input in the UI.
        /// </summary>
        public int ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the fully qualified type name of the Type 
        /// to use as a converter for the object this attribute is bound to.
        /// </summary>
        public string ConverterTypeName { get; set; }

        /// <summary>
        /// Gets or sets the date type expected to be used on UI.
        /// </summary>
        public Type CustomType { get; set; }

        /// <summary>
        /// Gets or sets the host property name when the property is a dependency property.
        /// </summary>
        public string DependencyHostPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the fully qualified type name of the dependency property type
        /// when the property is a dependency property.
        /// </summary>
        public string DependencyPropertyTypeName { get; set; }

        /// <summary>
        /// Gets or sets a value that is used to display a description in the UI.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///  Gets or sets a value that is used to set the display format for the input in the UI.
        /// </summary>
        public string DisplayFormat { get; set; }

        /// <summary>
        /// Gets or sets a value that is used to group fields in the UI.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets if the property is a computed property.
        /// </summary>
        public bool IsComputed { get; set; }

        /// <summary>
        /// Gets or sets if the property is a dependency property.
        /// </summary>
        public bool IsDependencyProperty { get; set; }

        /// <summary>
        /// Gets or sets if the property is hidden.
        /// </summary>
        public bool IsHidden
        {
            get
            {
                return isHidden
                    || IsDependencyProperty
                    || Order < 0;
            }
            set
            {
                isHidden = value;
            }
        }

        /// <summary>
        /// Gets or sets if the property is readonly.
        /// </summary>
        public bool IsReadOnly { get; set; }

        public LogicalDataType LogicalDataType { get; set; }

        /// <summary>
        /// Gets or sets the logical dependency property type
        /// when the property is a dependency property.
        /// </summary>
        public LogicalDependencyProperty LogicalDependencyProperty { get; set; }
        /// <summary>
        /// Gets or sets the logical dependency property when the property is a dependency property.
        /// </summary>
        public LogicalDependencyProperty LogicalProperty { get; set; }
        /// <summary>
        /// Gets or sets a value that is used for display in the UI.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the order weight of the property.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a value that will be used to set the watermark for prompts in the UI.
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// Gets the property data type that is used for UI.
        /// </summary>
        public Type PropertyDataType
        {
            get
            {
                return CustomType == null
                    ? PropertyInfo.PropertyType
                    : CustomType;
            }
        }

        /// <summary>
        /// Gets or sets the property type.
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public string PropertyName
        {
            get
            {
                if (PropertyInfo == null) return null;
                return PropertyInfo.Name;
            }
        }
        /// <summary>
        /// Gets or sets a value that is used for the grid column label.
        /// </summary>
        public string ShortName { get; set; }
    }
}