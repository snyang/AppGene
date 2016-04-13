using AppGene.Common.Entities.Infrastructure.Annotations;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGene.Common.Entities.Infrastructure.Tests.TestData
{
    [ModelDisplay(DisplayedColumns = new string[] { "FieldA", "FieldB", "FieldC" })]
    public class EntityWithOrderAttribute
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int FieldA { get; set; }

        [Display(Order = 1)]
        public int FieldB { get; set; }

        [Display(Order = 3)]
        public Int16 FieldC { get; set; }

        [Display(Order = 2)]
        public Int32 FieldD { get; set; }

        [Timestamp]
        public Byte[] FieldE { get; set; }
    }
}
