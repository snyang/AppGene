using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
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
            if (EntityAnalysisHelper.IsCharacteristicProperty(context.EntityType,
                context.PropertyInfo,
                "id"))
            {
                property.IsHidden = true;
                return;
            }

            // TODO: refatory it as a plugin
            var databaseGeneratedAttribute = context.PropertyInfo.GetCustomAttribute<DatabaseGeneratedAttribute>();
            if (databaseGeneratedAttribute != null
                && databaseGeneratedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
            {
                property.IsHidden = true;
                return;
            }

            var timestampAttribute = context.PropertyInfo.GetCustomAttribute<TimestampAttribute>();
            if (timestampAttribute != null)
            {
                property.IsHidden = true;
                return;
            }
        }
    }
}
