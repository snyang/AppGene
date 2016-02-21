using AppGene.Ui.Patterns.GenericMvvmBusiness;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public interface IMasterDetailModel<TEntity>
        : IGenericModel<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Gets if the instance is a new instance which have not been stored.
        /// </summary>
        bool IsNew { get; set; }

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