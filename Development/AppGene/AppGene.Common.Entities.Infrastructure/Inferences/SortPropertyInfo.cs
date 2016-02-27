using System.Reflection;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
    public class SortPropertyInfo
    {
        public PropertyInfo PropertyInfo { get; set; }

        public bool SortDescending { get; set; }
    }
}