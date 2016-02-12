using AppGene.Model.Patterns.Core.Layers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AppGene.Data.Infrastructure
{
    public abstract class AbstractCrudDataService<TDbContext, TEntity>
        : ICrudDataService<TEntity>
        where TDbContext : DbContext, new()
        where TEntity : class
    {
        public virtual void Delete(IList<TEntity> entities)
        {
            using (var context = new TDbContext())
            {
                foreach (var entity in entities)
                {
                    GetDbSet(context).Attach(entity);
                    context.Entry(entity).State = EntityState.Deleted;
                }

                context.SaveChanges();
            }
        }

        public abstract DbSet<TEntity> GetDbSet(TDbContext context);
        public virtual void Insert(TEntity entity)
        {
            using (var context = new TDbContext())
            {
                GetDbSet(context).Add(entity);
                context.SaveChanges();
            }
        }

        public virtual IList<TEntity> Query()
        {
            using (var context = new TDbContext())
            {
                return GetDbSet(context).ToList<TEntity>();
            }
        }

        public virtual void Update(TEntity entity)
        {
            using (var context = new TDbContext())
            {
                GetDbSet(context).Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
