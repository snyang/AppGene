using System.ComponentModel;
using System.Reflection;

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