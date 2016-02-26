using System;
using AppGene.Business.Infrastructure;
using AppGene.Data.Infrastructure;
using AppGene.Data.Sample;

namespace AppGene.Business.Sample
{
    public class CommonCrudBusinessService<TEntity>
        : AbstractCrudBusinessService<TEntity>
        where TEntity : class
    {
        //private ICrudDataService<TEntity> dataService;
        public override ICrudDataService<TEntity> DataService
        {
            get
            {
                //if (dataService==null)
                //{
                //    dataService = new CommonCrudDataService<TEntity>();
                //}
                return new CommonCrudDataService<TEntity>();
            }
        }
    }
}