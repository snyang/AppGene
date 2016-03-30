using System;

namespace AppGene.Common.Entities.Infrastructure.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ModelAttribute
           : Attribute
    {
        public String Name { get; set; }

        public Type ResourceType { get; set; }
    }
}