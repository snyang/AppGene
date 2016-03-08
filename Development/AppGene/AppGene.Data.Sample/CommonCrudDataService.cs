using AppGene.Data.Infrastructure;
using System.Data.Entity;

namespace AppGene.Data.Sample
{
    public class CommonCrudDataService<TEntity>
        : AbstractCrudDataService<AppGeneDbContext, TEntity>
        where TEntity : class
    {
        private DbSet<TEntity> dbSet;

        public override DbSet<TEntity> GetDbSet(AppGeneDbContext context)
        {
            if (dbSet == null)
            {
                dbSet = DataLayerHelper.FindDbSet<TEntity>(context);
            }
            return dbSet;
        }
    }
}