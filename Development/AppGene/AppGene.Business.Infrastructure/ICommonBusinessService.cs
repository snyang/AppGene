using System.Collections.Generic;

namespace AppGene.Business.Infrastructure
{
    public interface ICommonBusinessService<TEntity>
    {
        void Delete(IList<TEntity> items);

        void Insert(TEntity item);

        IList<TEntity> Query();

        void Update(TEntity item);
    }
}
