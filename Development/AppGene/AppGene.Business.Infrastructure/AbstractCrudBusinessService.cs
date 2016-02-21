using AppGene.Data.Infrastructure;
using System.Collections.Generic;

namespace AppGene.Business.Infrastructure
{
    public abstract class AbstractCrudBusinessService<TEntity>
        where TEntity : class
    {
        public abstract ICrudDataService<TEntity> DataService { get; }

        public virtual void Delete(IList<TEntity> entities)
        {
            DataService.Delete(entities);
        }

        public virtual void Insert(TEntity entity)
        {
            DataService.Insert(entity);
        }

        public virtual IList<TEntity> Query()
        {
            return DataService.Query();
        }

        public virtual void Update(TEntity entity)
        {
            DataService.Update(entity);
        }
    }
}