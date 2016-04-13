using System;

namespace AppGene.Common.Entities.Infrastructure.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ModelDisplayAttribute
           : Attribute
    {
        public String[] DisplayedColumns { get; set; }
    }
}