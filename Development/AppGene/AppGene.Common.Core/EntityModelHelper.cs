using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Common.Core
{
    public static class EntityModelHelper
    {
        public static TEntity ToEntity<TModel, TEntity>(TModel model)
            where TModel : class
            where TEntity : class, new()
        {
            if (AreEntityAndModelSame<TModel, TEntity>())
            {
                return model as TEntity;
            }

            if (DoesModelInheritEntity<TModel, TEntity>())
            {
                return model as TEntity;
            }

            if (DoesModelImplementsIEntityModel<TModel, TEntity>())
            {
                return (model as IEntityModel<TEntity>).Entity;
            }

            TEntity entity = new TEntity();
            CopyObjectValue(model, entity);
            return entity;
        }

        public static TModel CreateModel<TModel, TEntity>(TEntity entity)
            where TModel : class, new()
            where TEntity : class
        {
            if (AreEntityAndModelSame<TModel, TEntity>())
            {
                return entity as TModel;
            }

            TModel model = TryDynamicCreateModel<TModel, TEntity>(entity);
            if (model != null)
            {
                return model;
            }

            if (DoesModelImplementsIEntityModel<TModel, TEntity>())
            {
                model = new TModel();
                (model as IEntityModel<TEntity>).Entity = entity;
                return model;
            }

            model = new TModel();
            CopyObjectValue(entity, model);
            return model;
        }

        public static void CopyObjectValue<T1, T2>(T1 from, T2 to)
            where T1 : class
            where T2 : class
        {
            // TODO: Should we support it?
            // It is a bug that need to copy the value back to model when entity is inserted (entity values perhaps changed.)
            var toProperties = typeof(T2).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var toProperty in toProperties)
            {
                if (!toProperty.CanWrite)
                {
                    continue;
                }

                var fromProperty = typeof(T1).GetProperty(toProperty.Name, BindingFlags.Public | BindingFlags.Instance);
                if (fromProperty.CanRead)
                {
                    toProperty.SetValue(to, fromProperty.GetValue(from));
                }
             }
        }

        public static bool AreEntityAndModelSame<TModel, TEntity>()
        {
            return typeof(TEntity) == typeof(TModel);
        }

        public static bool DoesModelInheritEntity<TModel, TEntity>()
        {
            return typeof(TModel).IsSubclassOf(typeof(TEntity));
        }

        public static bool DoesModelImplementsIEntityModel<TModel, TEntity>()
        {
            return typeof(IEntityModel<TEntity>).IsAssignableFrom(typeof(TModel));
        }

        public static TModel TryDynamicCreateModel<TModel, TEntity>(TEntity entity)
            where TModel : class, new()
        {
            TModel model = null;
            try
            {
                model = Activator.CreateInstance(typeof(TModel), entity) as TModel;
            }
            catch { }

            return model;

        }
    }
}
