using System.Collections.Generic;

namespace AppGene.Data.Infrastructure
{
    /// <summary>
    /// Interface for common Create/Retrieve/Update/Delete operations.
    /// Do not implement the interface directly, instead please inherit AbstractCrudDataService <see cref="AbstractCrudDataService"/>.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICrudDataService<TEntity>
        where TEntity : class
    {
        void Delete(IList<TEntity> entities);

        void Insert(TEntity entity);

        IList<TEntity> Query();

        void Update(TEntity entity);
    }
}