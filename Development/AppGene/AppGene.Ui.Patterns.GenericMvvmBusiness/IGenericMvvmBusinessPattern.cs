using AppGene.Common.Patterns.Core;
using AppGene.Common.Patterns.Core.Layers;

namespace AppGene.Ui.Patterns.GenericMvvmBusiness
{
    public interface IGenericMvvmBusinessPattern<TEntity, TModel>
        : IDesignPattern
        where TEntity : class, new()
        where TModel : IGenericModel<TEntity>, new()

    {
        IBusinessLayerService BusinessService { get; set; }
        IGenericLayoutDescriptor<TEntity> LayoutDescriptor { get; set; }
        //IGenericView<IGenericMvvmBusinessPattern<TEntity>> View { get; }
        IGenericViewConstructor<TEntity> ViewConstructor { get; }
        IGenericViewController<IGenericMvvmBusinessPattern<TEntity, TModel>> ViewController { get; }
        IGenericViewModel<TEntity> ViewModel { get; }
    }
}