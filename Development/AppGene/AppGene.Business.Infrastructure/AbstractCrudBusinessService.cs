using AppGene.Data.Infrastructure;
using AppGene.Model.Patterns.Core.Layers;
using System.Collections.Generic;

namespace AppGene.Business.Infrastructure
{
    public abstract class AbstractCrudBusinessService<TEntity>
        : IBusinessLayerService
        where TEntity : class
    {
        public abstract ICrudDataService<TEntity> GetDataService();

        public virtual void Delete(IList<TEntity> entities)
        {
            GetDataService().Delete(entities);
        }

        public virtual void Insert(TEntity entity)
        {
            GetDataService().Insert(entity);
        }

        public virtual IList<TEntity> Query()
        {
            return GetDataService().Query();
        }

        public virtual void Update(TEntity entity)
        {
            GetDataService().Update(entity);
        }
    }
}