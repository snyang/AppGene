namespace AppGene.Ui.Infrastructure.Mvvm
{
    public class DefaultEditableModel<TModel, TEntity>
        : AbstractEditableModel<TModel, TEntity>
        where TModel : class, new()
        where TEntity : class, new()
    {
        public DefaultEditableModel()
            : base(true)
        { }

        public DefaultEditableModel(TEntity entity)
            : base(entity, true)
        { }
    }
}