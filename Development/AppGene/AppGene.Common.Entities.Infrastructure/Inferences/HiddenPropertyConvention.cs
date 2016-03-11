using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
    public class HiddenPropertyConvention
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
            if (!EntityAnalysisHelper.IsCharacteristicProperty(context.EntityType,
                context.PropertyInfo,
                "id"))
            {
                return;
            }

            property.IsHidden = true;
        }
    }
}
