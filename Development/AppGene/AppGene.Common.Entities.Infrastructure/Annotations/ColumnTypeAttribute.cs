using System;

namespace AppGene.Common.Entities.Infrastructure.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ColumnTypeAttribute
           : Attribute
    {
        /// <summary>
        /// Gets or sets the fully qualified type name of the Type 
        /// to use as a converter for the object this attribute is bound to.
        /// </summary>
        public string ConverterTypeName { get; set; }

        public Type CustomType { get; set; }

        public LogicalDataType LogicalDataType { get; set; }
    }
}