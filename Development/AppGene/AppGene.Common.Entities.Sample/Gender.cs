using System.ComponentModel;

namespace AppGene.Common.Entities
{
    public enum Gender : byte
    {
        //TODO: Localization
        [Description("Female")]
        Female,
        Male
    }
}