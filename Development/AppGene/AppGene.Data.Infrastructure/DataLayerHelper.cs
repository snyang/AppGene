using System.Data.Entity;
using System.Reflection;

namespace AppGene.Data.Infrastructure
{
    public static class DataLayerHelper
    {
        public static DbSet<TEntity> FindDbSet<TEntity>(DbContext context)
            where TEntity : class
        {
            DbSet<TEntity> dbSet = null;
            var properties = context.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(DbSet<TEntity>))
                {
                    dbSet = property.GetValue(context) as DbSet<TEntity>;
                    break;
                }
            }
            return dbSet;
        }
    }
}