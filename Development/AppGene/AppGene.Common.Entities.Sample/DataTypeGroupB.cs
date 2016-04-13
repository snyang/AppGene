using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGene.Common.Entities
{
    /// <summary>
    /// Data Type Group B : Advance Data Types
    /// </summary>
    public class DataTypeGroupB
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index(IsClustered = true, IsUnique = true)]
        public int DataTypeId { get; set; }

        public Boolean DataSByte { get; set; }

        public UInt16 DataUInt16 { get; set; }

        public UInt32 DataUInt32 { get; set; }

        public UInt64 DataUInt64 { get; set; }

        public TimeSpan DataTimeSpan { get; set; }

        [StringLength(1024)]
        [PasswordPropertyText]
        public String DataPassword { get; set; }

        [Column(TypeName = "timestamp")]
        public Byte[] DataTimestamp { get; set; }

        // Blob
        // Clob
        // Rich text
        // Html/xml
        // File
        // geo*
    }
}