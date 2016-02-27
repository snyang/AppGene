using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Common.Entities.Infrastructure.EntityModels
{
    public abstract class AbstractEntityModel<TEntity>
        : IEntityModel<TEntity>,
        INotifyPropertyChanged
        where TEntity: class, new()
    {
        public AbstractEntityModel()
        {
            this.Entity = new TEntity();
        }

        public AbstractEntityModel(TEntity entity)
        {
            this.Entity = entity;
        }

        public virtual TEntity Entity
        {
            get; set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        

        protected virtual bool SetProperty<T>(ref T originalValue, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(originalValue, value)) return false;

            originalValue = value;
            this.OnPropertyChanged(propertyName);

            return true;
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
