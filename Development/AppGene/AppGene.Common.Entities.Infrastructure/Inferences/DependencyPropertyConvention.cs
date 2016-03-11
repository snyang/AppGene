using AppGene.Common.Entities.Infrastructure.Annotations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
    public class DependencyPropertyConvention
    {
        public void Apply(EntityAnalysisContext context, DisplayPropertyInfo property)
        {
            var propertyName = property.PropertyName;
            int underscorePosition = propertyName.LastIndexOf('_');
            if (underscorePosition <= 0) return;

            var prefix = propertyName.Substring(0, underscorePosition);
            var suffix = propertyName.Substring(underscorePosition + 1);
            LogicalDependencyProperty logicalDependencyProperty;
            if (string.Equals(suffix, 
                                LogicalDependencyProperty.IsEnabled.ToString(),
                                StringComparison.OrdinalIgnoreCase))
            {
                logicalDependencyProperty = LogicalDependencyProperty.IsEnabled;
            }
            else if (string.Equals(suffix,
                                LogicalDependencyProperty.IsVisible.ToString(),
                                StringComparison.OrdinalIgnoreCase))
            {
                logicalDependencyProperty = LogicalDependencyProperty.IsReadOnly;
            }
            else if (string.Equals(suffix,
                                LogicalDependencyProperty.IsReadOnly.ToString(),
                                StringComparison.OrdinalIgnoreCase))
            {
                logicalDependencyProperty = LogicalDependencyProperty.IsReadOnly;
            }
            else
            {
                return;
            }

            
            var findProperty = context.EntityType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .First<PropertyInfo>(
                    (p) => string.Equals(p.Name, prefix, StringComparison.OrdinalIgnoreCase));
            if (findProperty != null)
            {
                property.DependencyHostPropertyName = findProperty.Name;
                property.LogicalDependencyProperty = logicalDependencyProperty;
                property.IsDependencyProperty = true;
            }
        }
    }
}
