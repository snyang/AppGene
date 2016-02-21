using AppGene.Common.EntityPerception;
using AppGene.Ui.Patterns.GenericMvvmBusiness;
using System;
using System.Globalization;
using System.Reflection;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public class MasterDetailModel<TEntity>
        : BaseGenericModel<TEntity>, IMasterDetailModel<TEntity>
        where TEntity : class, new()
    {
        private MasterDetailEntityPerception entityPerception = new MasterDetailEntityPerception(typeof(TEntity));

        public MasterDetailModel()
            : base()
        {
            (this as IMasterDetailModel<TEntity>).Entity = new TEntity();
            (this as IMasterDetailModel<TEntity>).IsNew = true;
            (this as IMasterDetailModel<TEntity>).SetDefault();
        }

        bool IMasterDetailModel<TEntity>.IsNew
        {
            get; set;
        }

        bool IMasterDetailModel<TEntity>.DoFilter(string keyword)
        {
            if (entityPerception.FilterProperties.Count == 0)
            {
                // Filter function not working.
                return true;
            }

            // Filtering
            foreach (var property in entityPerception.FilterProperties)
            {
                if (property.GetValue((this as IMasterDetailModel<TEntity>).Entity)
                    .ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
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
                object value = entityPerception.GetPropertyDefaultValue(property);
                if (value != null)
                {
                    property.SetValue((this as IMasterDetailModel<TEntity>).Entity, value);
                }
            }
        }

        string IMasterDetailModel<TEntity>.ToDisplayString()
        {
            string entityString = "item(s)";
            if (entityPerception.ReferenceProperties.Count == 0)
            {
                // Filter function not working.
                return entityString;
            }

            // Filtering
            object[] values = new object[entityPerception.ReferenceProperties.Count];
            string formatString = "";
            for (int i = 0; i < entityPerception.ReferenceProperties.Count; i++)
            {
                string propertyFormatString = entityPerception.GetPropertyFormatString(entityPerception.ReferenceProperties[i]);

                if (i != 0) formatString += " - ";
                formatString += string.IsNullOrEmpty(propertyFormatString)
                    ? "{" + i + "}"
                    : "{" + i + ":" + propertyFormatString + "}";

                values[i] = entityPerception.ReferenceProperties[i].GetValue((this as IMasterDetailModel<TEntity>).Entity);
            }

            entityString = string.Format(CultureInfo.CurrentCulture, formatString, values);

            return entityString;
        }
    }
}