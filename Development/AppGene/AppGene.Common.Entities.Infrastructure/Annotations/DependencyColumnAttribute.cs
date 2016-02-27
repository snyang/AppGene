using System;
using System.Windows;

namespace AppGene.Common.Entities.Infrastructure.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class DependencyColumnAttribute
           : Attribute
    {
        public String HostColumn { get; set; }
        public LogicalDependencyProperty LogicalProperty { get; set; }
        // TODO:  Should move the class to another place, as it need windowsbase.dll
        public DependencyProperty DependencyProperty { get; set; }
    }
}