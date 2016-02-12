using AppGene.Ui.Patterns.GenericMvvmBusiness.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AppGene.Ui.Patterns.GenericMvvmBusiness
{
    public class BaseGenericModel<TEntity>
        : IGenericModel<TEntity>
        where TEntity : class, new()
    {
        private Memento<TEntity> memento;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                if (this is INotifyPropertyChanged)
                {
                    this.PropertyChanged += value;
                    //((INotifyPropertyChanged)this).PropertyChanged +=
                    //                              this.NotifyPropertyChanged;
                }
            }

            remove
            {
                if (this is INotifyPropertyChanged)
                {
                    this.PropertyChanged -= value;
                    //((INotifyPropertyChanged)this).PropertyChanged -=
                    //                             this.NotifyPropertyChanged;
                }
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                return ValidationHelper.ValidateObject((this as IGenericModel<TEntity>).Entity);
            }
        }

        TEntity IGenericModel<TEntity>.Entity { get; set; }

        bool IGenericModel<TEntity>.IsChanged { get; set; }

        bool IGenericModel<TEntity>.TraceChanges { get; set; }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                return ValidationHelper.ValidateProperty((this as IGenericModel<TEntity>).Entity, columnName);
            }
        }

        void IEditableObject.BeginEdit()
        {
            if (this.memento == null)
            {
                this.memento = new Memento<TEntity>((this as IGenericModel<TEntity>).Entity);
                (this as IGenericModel<TEntity>).TraceChanges = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (this.memento != null)
            {
                (this as IGenericModel<TEntity>).TraceChanges = false;
                this.memento.Restore((this as IGenericModel<TEntity>).Entity);
                foreach (var pair in this.memento.StoredProperties)
                {
                    this.NotifyPropertyChanged(this, new PropertyChangedEventArgs(pair.Key.Name));
                }

                this.memento = null;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (this.memento != null)
            {
                (this as IGenericModel<TEntity>).TraceChanges = false;
                this.memento = null;
            }
        }

        private void HandleSet(Action setAction, [CallerMemberName] String propertyName = null)
        {
            setAction.Invoke();
            this.NotifyPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            if ((this as IGenericModel<TEntity>).TraceChanges) (this as IGenericModel<TEntity>).IsChanged = true;
        }

        private void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            throw new NotImplementedException();
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            throw new NotImplementedException();
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            throw new NotImplementedException();
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            throw new NotImplementedException();
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            throw new NotImplementedException();
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {

            IList<PropertyDescriptor> propertyDescriptors = new List<PropertyDescriptor>();

            var readonlyProperties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         .Where(p => p.CanRead && !p.CanWrite);

            var writableProperties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         .Where(p => p.CanRead && p.CanWrite);

            foreach (var property in readonlyProperties)
            {
                var propertyCopy = property;
                // Need this copy of property for use in the closure

                var propertyDescriptor = EntityTypeConverter.CreatePropertyDescriptor(
                    property.Name,
                    typeof(TEntity),
                    property.PropertyType,
                    (component) => propertyCopy.GetValue(((IGenericModel<TEntity>)component).Entity, null));

                propertyDescriptors.Add(propertyDescriptor);
            }

            foreach (var property in writableProperties)
            {
                var propertyCopy = property;
                // Need this copy of property for use in the closure

                var propertyDescriptor = EntityTypeConverter.CreatePropertyDescriptor(
                    property.Name,
                    typeof(TEntity),
                    property.PropertyType,
                    (component) => propertyCopy.GetValue(((IGenericModel<TEntity>)component).Entity, null),
                    (component, value) =>
                    {
                        HandleSet(() =>
                        {
                            propertyCopy.SetValue(((IGenericModel<TEntity>)component).Entity, value, null);
                        }, property.Name);
                    });

                propertyDescriptors.Add(propertyDescriptor);
            }

            return new PropertyDescriptorCollection(propertyDescriptors.ToArray());
        }

        

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }

        private event PropertyChangedEventHandler PropertyChanged;
    }
}