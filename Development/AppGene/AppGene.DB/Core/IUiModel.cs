using System.ComponentModel;

namespace AppGene.Db.Core
{
    public interface IUiModel
        : IEditableObject,
            INotifyPropertyChanged,
            IDataErrorInfo
    {
        bool TraceChanges { get; set; }
        bool IsChanged { get; set; }
        bool IsNew { get; }
        bool DoFilter(string filterString);
        void SetDefault();
    }
}
