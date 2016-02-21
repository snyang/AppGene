using AppGene.Common.Patterns.Core.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Ui.Patterns.GenericMvvmBusiness
{
    public abstract class BaseGenericMvvmBusinessPattern<TEntity, TModel>
        : IGenericMvvmBusinessPattern<TEntity, TModel>
        where TEntity : class, new()
        where TModel : IGenericModel<TEntity>, new()
    {
        public virtual IBusinessLayerService BusinessService
        {
            get; set;
        }

        public virtual IGenericLayoutDescriptor<TEntity> LayoutDescriptor
        {
            get; set;
        }

        public virtual IGenericViewConstructor<TEntity> ViewConstructor
        {
            get; protected set;
        }

        public virtual IGenericViewController<IGenericMvvmBusinessPattern<TEntity, TModel>> ViewController
        {
            get; protected set;
        }

        public IGenericViewModel<TEntity> ViewModel
        {
            get; protected set;
        }
    }
}
