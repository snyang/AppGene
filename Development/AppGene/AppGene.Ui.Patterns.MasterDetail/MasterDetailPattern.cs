using AppGene.Business.Infrastructure;
using AppGene.Ui.Patterns.GenericMvvmBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public class MasterDetailPattern<TEntity, TModel>
        : BaseGenericMvvmBusinessPattern<TEntity, TModel>
        where TEntity : class, new()
        where TModel : IGenericModel<TEntity>, new()
    {
       
    }
}
