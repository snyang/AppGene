using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Model.EntityPerception
{
    public class DefaultValueGetter
    {
        public object Get(EntityAnalysisContext context)
        {
            //TODO support localization
            var defaultValueAttribute = context.PropertyInfo.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultValueAttribute == null) return null;

            return defaultValueAttribute.Value;
        }
    }
}
