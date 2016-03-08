using System.ComponentModel;
using System.Reflection;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
    public class DefaultValueGetter
    {
        /// <summary>
        /// Gets the default value for specific property.
        /// </summary>
        /// <param name="context">The entity analysis context.</param>
        /// <returns>The default value. If there is no default value, return null.</returns>
        public object GetDefaultValue(EntityAnalysisContext context)
        {
            //TODO support localization
            var defaultValueAttribute = context.PropertyInfo.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultValueAttribute == null) return null;

            return defaultValueAttribute.Value;
        }
    }
}