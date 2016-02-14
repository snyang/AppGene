using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppGene.Model.EntityPerception
{
    public class DisplayFormatGetter
    {
        /// <summary>
        /// Gets the display columns.
        /// </summary>
        /// <param name="context">The entity analysis context.</param>
        /// <returns>The display columns.</returns>
        public virtual string Get(EntityAnalysisContext context)
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