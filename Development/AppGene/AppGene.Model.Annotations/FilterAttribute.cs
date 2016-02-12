using System;

namespace AppGene.Model.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FilterAttribute
        : Attribute
    {
    }
}
