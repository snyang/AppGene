using AppGene.Ui.Infrastructure.Mvvm;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public interface IMasterDetailModel<TEntity>
        : IEditableModel<TEntity, TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Do filter.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        bool DoFilter(string keyword);

        /// <summary>
        /// Set default value to the instance.
        /// </summary>
        void SetDefault();

        /// <summary>
        /// Returns a string which is used to display the object in message box dialog.
        /// </summary>
        /// <returns></returns>
        string ToDisplayString();
    }
}