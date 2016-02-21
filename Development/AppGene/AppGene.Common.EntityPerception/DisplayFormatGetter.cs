using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppGene.Common.EntityPerception
{
    public class DisplayFormatGetter
    {
        /// <summary>
        /// Gets format string for specific property.
        /// </summary>
        /// <param name="context">The entity analysis context.</param>
        /// <returns>The format string.</returns>
        public string GetFormatString(EntityAnalysisContext context)
        {
            string displayFormat = null;

            // Find sort columns from DisplayColumnAttributes
            var displayFormatAttribue = context.PropertyInfo.GetCustomAttribute<DisplayFormatAttribute>();
            if (displayFormatAttribue != null)
            {
                return displayFormatAttribue.DataFormatString;
            }

            return displayFormat;
        }
    }
}