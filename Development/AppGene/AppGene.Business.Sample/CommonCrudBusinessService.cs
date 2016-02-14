using AppGene.Business.Infrastructure;
using AppGene.Data.Infrastructure;
using AppGene.Data.Sample;

namespace AppGene.Business.Sample
{
    public class CommonCrudBusinessService<TEntity>
        : AbstractCrudBusinessService<TEntity>
        where TEntity : class
    {
        public override ICrudDataService<TEntity> GetDataService()
        {
            return new CommonCrudDataService<TEntity>();
        }
    }
}