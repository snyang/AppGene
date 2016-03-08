using System;

namespace AppGene.Common.Entities.Infrastructure.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ComputeRelationshipAttribute
           : Attribute
    {
        private String[] computedColumns;
        public String ComputedColumn { get; set; }
        public String[] ComputedColumns
        {
            get
            {
                if (computedColumns != null)
                {
                    return computedColumns;
                }

                if (string.IsNullOrEmpty(ComputedColumn))
                {
                    return new string[] { };
                }

                return new string[] { ComputedColumn };
            }
            set
            {
                computedColumns = value;
            }
        }

        private String[] referenceColumns;
        public String ReferenceColumn { get; set; }
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