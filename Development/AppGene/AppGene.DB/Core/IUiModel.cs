using System.ComponentModel;

namespace AppGene.Db.Core
{
    /// <summary>
    /// The interface is uesd in UI operations.
    /// </summary>
    public interface IUiModel
        : IEditableObject,
            INotifyPropertyChanged,
            IDataErrorInfo
    {
        //TODO: If it is only used by MasterDetailView?

        /// <summary>
        /// Gets and sets if need to trace changes.
        /// The property is used with the property IsChanges.
        /// </summary>
        bool TraceChanges { get; set; }

        /// <summary>
        /// Gets and sets if the instance is changed.
        /// The property only works when TraceChanges is true.
        /// </summary>
        bool IsChanged { get; set; }

        /// <summary>
        /// Gets and sets if the instance is a new instance which is not stored in database.
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        /// Do filter.
        /// </summary>
        /// <param name="filterString"></param>
        /// <returns></returns>
        bool DoFilter(string filterString);

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
