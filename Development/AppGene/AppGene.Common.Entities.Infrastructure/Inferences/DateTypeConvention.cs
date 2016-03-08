using AppGene.Common.Entities.Infrastructure.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
    public class DateTypeConvention
    {
        /// <summary>
        /// Configure the specific property hidden value.
        /// * If the column name is "{EntityName}Id"
        /// * And this is self increment field.
        /// * Set the hidden as true.
        /// </summary>
        /// <param name="context"></param>
        public void Apply(EntityAnalysisContext context, DisplayPropertyInfo property)
        {
            string[] dateTypeColumnSuffixes = new string[] { "Date", "Day" };
            foreach (var suffix in dateTypeColumnSuffixes)
            {
                if (context.PropertyInfo.Name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    property.LogicalDataType = LogicalDataType.Date;
                    return;
                }
            }
        }
    }
}
