using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Db.Core
{
    public class BaseUiModel<TEntity> : IUiModel,
            ICustomTypeDescriptor,
            IComponent
        where TEntity: new()

    {
        /// <summary>
        /// The wrapped object.
        /// </summary>
        public TEntity Target { get; set; }

        public BaseUiModel()
        {
            this.Target = new TEntity();
            ((IUiModel)this).SetDefault();
        }

        public BaseUiModel(TEntity target)
        {
            this.Target = target;
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IUiModel.IsChanged
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        bool IUiModel.IsNew
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IUiModel.TraceChanges
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        ISite site;
        ISite IComponent.Site
        {
            get
            {
                return site;
            }

            set
            {
                site = value;
            }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler IComponent.Disposed
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        void IEditableObject.BeginEdit()
        {
            throw new NotImplementedException();
        }

        void IEditableObject.CancelEdit()
        {
            throw new NotImplementedException();
        }

        bool IUiModel.DoFilter(string filterString)
        {
            throw new NotImplementedException();
        }

        void IEditableObject.EndEdit()
        {
            throw new NotImplementedException();
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
            IList<PropertyDescriptor> propertyDescriptors =
                                        new List<PropertyDescriptor>();

            var readonlyPropertyInfos =
                typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         .Where(p => p.CanRead && !p.CanWrite);

            var writablePropertyInfos =
                typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         .Where(p => p.CanRead && p.CanWrite);

            foreach (var property in readonlyPropertyInfos)
            {
                var propertyCopy = property;
                // Need this copy of property for use in the closure

                var propertyDescriptor = PropertyDescriptorFactory.CreatePropertyDescriptor(
                    property.Name,
                    typeof(TEntity),
                    property.PropertyType,
                    (component) => propertyCopy.GetValue(
                                     ((BaseUiModel<TEntity>)component).Target, null));

                propertyDescriptors.Add(propertyDescriptor);
            }

            foreach (var property in writablePropertyInfos)
            {
                var propertyCopy = property;
                // Need this copy of property for use in the closure

                var propertyDescriptor = PropertyDescriptorFactory.CreatePropertyDescriptor(
                    property.Name,
                    typeof(TEntity),
                    property.PropertyType,
                    (component) => propertyCopy.GetValue(
                              ((BaseUiModel<TEntity>)component).Target, null),
                    (component, value) => propertyCopy.SetValue(
                              ((BaseUiModel<TEntity>)component).Target, value, null));

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

        void IUiModel.SetDefault()
        {
            throw new NotImplementedException();
        }

        string IUiModel.ToDisplayString()
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseUiModel() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
