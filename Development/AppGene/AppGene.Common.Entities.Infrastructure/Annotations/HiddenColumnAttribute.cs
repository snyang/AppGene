using System;

namespace AppGene.Common.Entities.Infrastructure.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class HiddenColumnAttribute
           : Attribute
    {
        public HiddenColumnAttribute(bool hidden)
        {
            Hidden = hidden;
        }

        public bool Hidden { get; set; }
    }
}