namespace AppGene.Ui.Infrastructure.Mvvm
{
    public class BaseEntityModel<TEntity>
        : AbstractEditableModel<TEntity, TEntity>
        where TEntity : class, new()
    {
        public BaseEntityModel()
            : base()
        {

        }


    }
}