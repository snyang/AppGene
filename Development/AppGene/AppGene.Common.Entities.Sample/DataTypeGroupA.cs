using AppGene.Common.Entities.Infrastructure.Annotations;
using AppGene.Common.Entities.Sample;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGene.Common.Entities
{
    /// <summary>
    /// Data Type Group A : basic data types
    /// </summary>
    public class DataTypeGroupA
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int DataTypeId { get; set; }

        public Boolean DataBoolean { get; set; }

        public Byte DataByte { get; set; }

        public System.Char DataChar { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DataDateTime { get; set; }

        public Decimal DataDecimal { get; set; }

        public Double DataDouble { get; set; }

        public ShortEnum DataEnumShort { get; set; }

        public LongEnum DataEnumLong { get; set; }

        public Guid DataGuid { get; set; }

        [DefaultValue(1)]
        public Int16 DataInt16 { get; set; }

        public Int32 DataInt32 { get; set; }

        public Int32? DataInt32Nullable { get; set; }

        public Int64 DataInt64 { get; set; }

        public Single DataSingle { get; set; }

        [StringLength(255)]
        public String DataString { get; set; }

        [StringLength(1024)]
        [PasswordPropertyText]
        public String DataPassword { get; set; }

        [Timestamp]
        public Byte[] DataTimestamp { get; set; }
    }
}