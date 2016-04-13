using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGene.Common.Entities.Infrastructure.Tests.TestData
{
    public class EntityWithoutOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int FieldA { get; set; }

        public int FieldB { get; set; }

        public Int16 FieldC { get; set; }

        public Int32 FieldD { get; set; }

        [Timestamp]
        public Byte[] FieldE { get; set; }
    }
}
