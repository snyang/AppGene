using AppGene.Common.Entities.Infrastructure.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGene.Common.Entities.Infrastructure.Tests.TestData
{
    [ModelDisplay(DisplayedColumns = new string[] { "ModelFieldA", "FieldA", "FieldB", "FieldC" })]
    public class ModelWithOrderAttribute : EntityWithOrderAttribute
    {
        [Display(Order = 1)]
        public int ModelFieldA { get; set; }

        [Display(Order = 2)]
        public Int32 ModelFieldB { get; set; }
    }
}
