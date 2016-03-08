using System;

namespace AppGene.Common.Entities.Infrastructure.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class DependencyColumnAttribute
           : Attribute
    {
        public String HostColumn { get; set; }
        public LogicalDependencyProperty LogicalProperty { get; set; }

        /// <summary>
        /// Gets or sets the fully qualified type name of the dependency property type.
        /// </summary>
        public string DependencyPropertyTypeName { get; set; }
    }
}