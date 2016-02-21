using System;

namespace AppGene.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class FilterAttribute
           : Attribute
    {
    }
}