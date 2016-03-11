using AppGene.Common.Entities.Infrastructure.Inferences;
using AppGene.Ui.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public class MasterDetailUiService<TModel, TEntity>
        where TModel : class, new()
        where TEntity : class, new()
    {
        private MasterDetailModelInference modelInference = new MasterDetailModelInference(typeof(TModel));
        private MasterDetailModelInference entityInference = new MasterDetailModelInference(typeof(TEntity));

        public virtual bool DoFilter(TModel model, string keyword)
        {
            if (modelInference.FilterProperties.Count == 0)
            {
                // Filter function not working.
                return true;
            }

            // Filtering
            foreach (var property in modelInference.FilterProperties)
            {
                if (property.GetValue(model)
                    .ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual void SetDefault(TModel model)
        {
            foreach (var property in typeof(TModel).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                object value = modelInference.GetPropertyDefaultValue(property);
                if (value != null)
                {
                    property.SetValue(model, value);
                }
            }

        }

        /// <summary>
        /// Returns a string which is used to display the object in message box dialog.
        /// </summary>
        /// <returns></returns>
        public virtual string ToDisplayString(TModel model)
        {
            string entityString = "item(s)";
            if (modelInference.ReferenceProperties.Count == 0)
            {
                // Filter function not working.
                return entityString;
            }

            // Filtering
            object[] values = new object[modelInference.ReferenceProperties.Count];
            string formatString = "";
            for (int i = 0; i < modelInference.ReferenceProperties.Count; i++)
            {
                string propertyFormatString = modelInference.GetPropertyFormatString(modelInference.ReferenceProperties[i]);

                if (i != 0) formatString += " - ";
                formatString += string.IsNullOrEmpty(propertyFormatString)
                    ? "{" + i + "}"
                    : "{" + i + ":" + propertyFormatString + "}";

                values[i] = modelInference.ReferenceProperties[i].GetValue(model);
            }

            entityString = string.Format(CultureInfo.CurrentCulture, formatString, values);

            return entityString;
        }


        public virtual void Sort(IList<TEntity> entities)
        {
            if (entityInference.SortProperties.Count == 0) return;

            EntitySortComparer<TEntity> comparer = new EntitySortComparer<TEntity>(entityInference.SortProperties);
            (entities as List<TEntity>).Sort(comparer);
        }

        public virtual IList<DisplayPropertyInfo> GetDisplayProperties()
        {
            return modelInference.DisplayProperties;
        }

        public virtual IList<DisplayPropertyInfo> GetGridDisplayProperties()
        {
            return modelInference.GridDisplayProperties;
        }
    }
}