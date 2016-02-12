using AppGene.Model.EntityPerception;
using System;
using System.Collections.Generic;

namespace AppGene.Ui.Infrastructure
{
    public class EntitySortComparer<TEntity>
        : IComparer<TEntity>
        where TEntity : class
    {
        private readonly IList<SortPropertyInfo> sortProperties;

        public EntitySortComparer(IList<SortPropertyInfo> sortProperties)
        {
            this.sortProperties = sortProperties;
        }

        public virtual int Compare(TEntity object1, TEntity object2)
        {
            int result = 0;
            foreach (var sortProperty in sortProperties)
            {
                result = Compare(sortProperty.PropertyInfo.PropertyType,
                            sortProperty.PropertyInfo.GetValue(object1),
                            sortProperty.PropertyInfo.GetValue(object2));
                if (sortProperty.SortDescending)
                {
                    result = 0 - result;
                }

                if (result != 0) return result;
            }
            return 0;
        }

        protected virtual int Compare(Type dataType, object value1, object value2)
        {
            if (dataType == typeof(string))
            {
                return String.Compare(value1 as string, value2 as string);
            }
            if (dataType == typeof(int))
            {
                return (int)value1 - (int)value2;
            }

            return 0;
        }
    }
}