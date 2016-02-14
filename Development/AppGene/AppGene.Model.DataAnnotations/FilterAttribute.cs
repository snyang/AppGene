using System;

namespace AppGene.Model.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FilterAttribute
           : Attribute
    {
    }
}