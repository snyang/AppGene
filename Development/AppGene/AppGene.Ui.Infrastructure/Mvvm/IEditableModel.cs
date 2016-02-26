using AppGene.Common.Core;
using System.ComponentModel;

namespace AppGene.Ui.Infrastructure.Mvvm
{
    public interface IEditableModel<TModel, TEntity>
        : IEditableObject,
        IDataErrorInfo,
        INotifyPropertyChanged,
        IEntityModel<TEntity>
        where TModel : class, new()
        where TEntity : class, new()
    {
        /// <summary>
        /// Gets and sets if the instance is changed.
        /// The property only works when TraceChanges is true.
        /// </summary>
        bool IsChanged { get; set; }

        /// <summary>
        /// Gets and sets if the instance is a new instance that has not been stored.
        /// </summary>
        bool IsNew { get; set; }

        /// <summary>
        /// Gets and sets the model object
        /// </summary>
        TModel Model { get; set; }
        
        /// <summary>
        /// Gets and sets if need to trace changes.
        /// The property is used with the property IsChanges.
        /// </summary>
        bool TraceChanges { get; set; }

        /// <summary>
        /// Return IEditableModel instance.
        /// </summary>
        /// <returns>The IEditableModel instance.</returns>
        IEditableModel<TModel, TEntity> ToIEditableModel();
    }
}