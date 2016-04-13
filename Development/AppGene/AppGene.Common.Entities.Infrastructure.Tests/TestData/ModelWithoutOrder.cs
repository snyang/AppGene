using AppGene.Common.Entities.Infrastructure.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGene.Common.Entities.Infrastructure.Tests.TestData
{
    public class ModelWithoutOrder : EntityWithoutOrder
    {
        //public new Int16 FieldC { get; set; }

        [ComputeRelationship(ReferenceColumns = new string[] { "FieldC", "FieldB" })]
        public int FieldModelA
        {
            get
            {
                return FieldC + FieldB;
            }
        }

        [ComputeRelationship(ReferenceColumn = "FieldC")]
        public bool FieldB_IsReadOnly
        {
            get
            {
                return FieldC != 0;
            }
        }

    }
}

