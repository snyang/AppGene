using System;

namespace AppGene.Common.Entities.Infrastructure.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ComputedColumnAttribute
           : Attribute
    {
        public String ReferenceColumn { get; set; }
        private String[] referenceColumns;
        public String[] ReferenceColumns
        {
            get
            {
                if (referenceColumns != null)
                {
                    return referenceColumns;
                }

                if (string.IsNullOrEmpty(ReferenceColumn))
                {
                    return new string[] { };
                }

                return new string[] { ReferenceColumn };

            }
            set
            {
                referenceColumns = value;
            }
        }
    }
}