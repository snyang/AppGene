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
        : IGenericModel<TEntity>,
        ICustomTypeDescriptor
        where TEntity : class, new()
    {
        private Memento<TEntity> memento;

        public virtual event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                this.propertyChanged += value;
            }

            remove
            {
                this.propertyChanged -= value;
            }
        }

        private event PropertyChangedEventHandler propertyChanged;

        string IDataErrorInfo.Error
        {
            get
            {
                if (typeof(IDataErrorInfo).IsAssignableFrom(typeof(TEntity)))
                {
                    return ((this as IGenericModel<TEntity>).Entity as IDataErrorInfo).Error;
                }
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

        #region Implements IEditableObject
        public virtual void BeginEdit()
        {
            if (this.memento == null)
            {
                this.memento = new Memento<TEntity>((this as IGenericModel<TEntity>).Entity);
                (this as IGenericModel<TEntity>).TraceChanges = true;
            }
        }

        public virtual void CancelEdit()
        {
            if (this.memento != null)
            {
                (this as IGenericModel<TEntity>).TraceChanges = false;
                this.memento.Restore((this as IGenericModel<TEntity>).Entity);
                foreach (var pair in this.memento.StoredProperties)
                {
                    this.OnPropertyChanged(this, new PropertyChangedEventArgs(pair.Key.Name));
                }

                this.memento = null;
            }
        }

        public virtual void EndEdit()
        {
            if (this.memento != null)
            {
                (this as IGenericModel<TEntity>).TraceChanges = false;
                this.memento = null;
            }
        }
        #endregion

        private void HandleSet(Action setAction, [CallerMemberName] String propertyName = null)
        {
            setAction.Invoke();
            this.OnPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            if ((this as IGenericModel<TEntity>).TraceChanges) (this as IGenericModel<TEntity>).IsChanged = true;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.propertyChanged != null)
            {
                this.propertyChanged(this, e);
            }
        }

        private static bool IsInterfaceProperty(Type interfaceType, Type classType, PropertyInfo propertyInfo)
        {
            PropertyInfo interfaceProperty = interfaceType.GetProperty(propertyInfo.Name);
            MethodInfo getterMethod = propertyInfo.GetGetMethod();
            InterfaceMapping mapping = classType.GetInterfaceMap(interfaceType);

            for (int i = 0; i < mapping.InterfaceMethods.Length; i++)
            {
                if (mapping.InterfaceMethods[i] == getterMethod)
                {
                    return true;
                }
            }

            return false;
        }

        #region Implements ICustomTypeDescriptor
        public virtual AttributeCollection GetAttributes()
        {
            throw new NotImplementedException();
        }

        public virtual string GetClassName()
        {
            throw new NotImplementedException();
        }

        public virtual string GetComponentName()
        {
            throw new NotImplementedException();
        }

        public virtual TypeConverter GetConverter()
        {
            throw new NotImplementedException();
        }

        public virtual EventDescriptor GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        public virtual PropertyDescriptor GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        public virtual object GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        public virtual EventDescriptorCollection GetEvents()
        {
            throw new NotImplementedException();
        }

        public virtual EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public virtual PropertyDescriptorCollection GetProperties()
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

        public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public virtual object GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}