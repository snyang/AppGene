using AppGene.Model.EntityPerception;
using AppGene.Ui.Patterns.GenericMvvmBusiness;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public class MasterDetailModel<TEntity>
        : BaseGenericModel<TEntity>, IMasterDetailModel<TEntity>
        where TEntity : class, new()
    {
        private IList<PropertyInfo> displayProperties;
        private IList<PropertyInfo> filterProperties;

        public MasterDetailModel()
            : base()
        {
            (this as IMasterDetailModel<TEntity>).Entity = new TEntity();
            (this as IMasterDetailModel<TEntity>).IsNew = true;
            (this as IMasterDetailModel<TEntity>).SetDefault();
        }

        public IList<PropertyInfo> DisplayProperties
        {
            get
            {
                if (displayProperties == null)
                {
                    displayProperties = new ReferencePropertyGetter().Get(new EntityAnalysisContext
                    {
                        EntityType = typeof(TEntity),
                        Source = this.GetType().FullName
                    });
                }

                return displayProperties;
            }
        }

        public IList<PropertyInfo> FilterProperties
        {
            get
            {
                if (filterProperties == null)
                {
                    filterProperties = new FilterPropertyGetter().Get(new EntityAnalysisContext
                    {
                        EntityType = typeof(TEntity),
                        Source = this.GetType().FullName
                    });
                }

                return filterProperties;
            }
        }
        bool IMasterDetailModel<TEntity>.IsNew
        {
            get; set;
        }

        bool IMasterDetailModel<TEntity>.DoFilter(string filterString)
        {
            if (FilterProperties.Count == 0)
            {
                // Filter function not working.
                return true;
            }

            // Filtering
            foreach (var property in FilterProperties)
            {
                if (property.GetValue((this as IMasterDetailModel<TEntity>).Entity)
                    .ToString().IndexOf(filterString, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        void IMasterDetailModel<TEntity>.SetDefault()
        {
            foreach (var property in typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                object value = new DefaultValueGetter().Get(new EntityAnalysisContext
                {
                    EntityType = typeof(TEntity),
                    PropertyInfo = property,
                    Source = this.GetType().FullName
                });
                if (value != null)
                {
                    property.SetValue((this as IMasterDetailModel<TEntity>).Entity, value);
                }
            }
        }

        string IMasterDetailModel<TEntity>.ToDisplayString()
        {
            string entityString = "item(s)";
            if (DisplayProperties.Count == 0)
            {
                // Filter function not working.
                return entityString;
            }

            // Filtering
            object[] values = new object[DisplayProperties.Count];
            string formatString = "";
            for (int i = 0; i < DisplayProperties.Count; i++)
            {
                string propertyFormatString = new DisplayFormatGetter().Get(new EntityAnalysisContext
                {
                    EntityType = typeof(TEntity),
                    PropertyInfo = DisplayProperties[i],
                    Source = this.GetType().FullName,
                });

                if (i != 0) formatString += " - ";
                formatString += string.IsNullOrEmpty(propertyFormatString)
                    ? "{" + i + "}"
                    : "{" + i + ":" + propertyFormatString + "}";

                values[i] = DisplayProperties[i].GetValue((this as IMasterDetailModel<TEntity>).Entity);
            }

            entityString = string.Format(formatString, values);

            return entityString;
        }
    }
}
