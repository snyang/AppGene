using System.ComponentModel;

namespace AppGene.Ui.Patterns.GenericMvvmBusiness
{
    public interface IGenericModel<TEntity>
        : ICustomTypeDescriptor,
        IEditableObject,
        IDataErrorInfo,
        INotifyPropertyChanged
        where TEntity : class, new()
    {
        /// <summary>
        /// Gets and sets the entity object
        /// </summary>
        TEntity Entity { get; set; }

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
    }
}