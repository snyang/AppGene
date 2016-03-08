using AppGene.Common.Entities.Infrastructure.Annotations;
using AppGene.Common.Entities.Infrastructure.EntityModels;
using AppGene.Ui.Infrastructure.Mvvm.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AppGene.Ui.Infrastructure.Mvvm
{
    /// <summary>
    /// Provide editable model functions:
    /// Implments
    ///    INotifyPropertyChanged : for data binding.
    ///    IDataErrorInfo : for validation.
    ///    IEditableObject: support cancelable editing.
    ///    IEntityModel: for convert model to entity.
    /// </summary>
    /// <typeparam name="TModel">The model class</typeparam>
    /// <typeparam name="TEntity">The entity class</typeparam>
    /// <remarks>
    /// <typeparamref name="TModel"/> :
    ///   is the class that is used in the presentation layer.
    /// <typeparamref name="TEntity"/> :
    ///   is the class that is used in the business layer.
    /// The relationships of <typeparamref name="TModel"/> and <typeparamref name="TEntity"/>
    /// * They are same, it means that the same class be used among the presentation layer and the business layer.
    /// * They are different, <typeparamref name="TModel"/> inherits <typeparamref name="TEntity"/>.
    /// * They are different, <typeparamref name="TModel"/> implements <see cref="Common.Entities.Infrastructure.EntityModels.IEntityModel"/>.
    /// * They are different, no relationship between <typeparamref name="TModel"/> and <typeparamref name="TEntity"/>.
    /// </remarks>
    public abstract class AbstractEditableModel<TModel, TEntity>
        : IEditableModel<TModel, TEntity>,
        ICustomTypeDescriptor
        where TModel : class, new()
        where TEntity : class, new()
    {
        private bool? doesModelImplementIDataErrorInfo;
        private bool? doesModelImplementINotifyPropertyChanged;
        private TEntity entity;
        private bool generateModelCustomProperties = false;
        private bool isChanged;
        private Memento<TModel> memento;
        private TModel model;
        private Dictionary<string, List<string>> computedRelationShip = new Dictionary<string, List<string>>();

        public AbstractEditableModel()
            : this(null, false)
        { }

        public AbstractEditableModel(bool generateModelCustomProperties)
            : this(null, generateModelCustomProperties)
        { }

        public AbstractEditableModel(TEntity entity, bool generateModelCustomProperties)
        {
            this.generateModelCustomProperties = generateModelCustomProperties;
            if (entity != null)
            {
                this.ToIEditableModel().Model = EntityModelHelper.CreateModel<TModel, TEntity>(entity);
            }
            else
            {
                this.ToIEditableModel().Model = new TModel();
            }
        }

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

        public virtual TEntity Entity
        {
            get
            {
                return EntityModelHelper.ToEntity<TModel, TEntity>(this.ToIEditableModel().Model);
            }
            set
            {
                if (AreEntityAndModelSame)
                {
                    this.ToIEditableModel().Model = value as TModel;
                    return;
                }
                entity = value;
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (DoesModelImplementIDataErrorInfo)
                {
                    return (this.ToIEditableModel().Model as IDataErrorInfo).Error;
                }
                return ValidationHelper.ValidateObject(this.ToIEditableModel().Model);
            }
        }
        bool IEditableModel<TModel, TEntity>.IsChanged
        {
            get
            {
                return isChanged || this.ToIEditableModel().IsNew;
            }
            set
            {
                isChanged = value;
            }
        }

        bool IEditableModel<TModel, TEntity>.IsNew { get; set; }

        TModel IEditableModel<TModel, TEntity>.Model
        {
            get
            {
                return model;
            }
            set
            {
                if (model != value)
                {
                    model = value;
                    if (model != null && DoesModelImplementINotifyPropertyChanged)
                    {
                        (model as INotifyPropertyChanged).PropertyChanged += HandleModelPropertyChangedEvent;
                    }

                }
            }
        }

        bool IEditableModel<TModel, TEntity>.TraceChanges { get; set; }

        bool AreEntityAndModelSame
        {
            get { 
                return typeof(TEntity) == typeof(TModel);
            }
        } 
        private bool DoesModelImplementIDataErrorInfo
        {
            get
            {
                if (!doesModelImplementIDataErrorInfo.HasValue)
                {
                    doesModelImplementIDataErrorInfo = typeof(IDataErrorInfo).IsAssignableFrom(typeof(TModel));
                }
                return doesModelImplementIDataErrorInfo.Value;
            }
        }

        private bool DoesModelImplementINotifyPropertyChanged
        {
            get
            {
                if (!doesModelImplementINotifyPropertyChanged.HasValue)
                {
                    doesModelImplementINotifyPropertyChanged = typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(TModel));
                }
                return doesModelImplementINotifyPropertyChanged.Value;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (DoesModelImplementIDataErrorInfo)
                {
                    return (this.ToIEditableModel().Model as IDataErrorInfo)[columnName];
                }
                return ValidationHelper.ValidateProperty(this.ToIEditableModel().Model, columnName);
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

        private void HandleModelPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(sender, e);
        }
        #region Implements IEditableObject
        private bool? doesModelImplementIEditableObject;

        private bool DoesModelImplementIEditableObject
        {
            get
            {
                if (!doesModelImplementIEditableObject.HasValue)
                {
                    doesModelImplementIEditableObject = typeof(IEditableObject).IsAssignableFrom(typeof(TModel));
                }
                return doesModelImplementIEditableObject.Value;
            }
        }

        public virtual void BeginEdit()
        {
            if (DoesModelImplementIEditableObject)
            {
                (this.ToIEditableModel().Model as IEditableObject).BeginEdit();
                this.ToIEditableModel().TraceChanges = true;
                return;
            }

            if (this.memento == null)
            {
                this.memento = new Memento<TModel>(this.ToIEditableModel().Model);
                this.ToIEditableModel().TraceChanges = true;
            }
        }
        public virtual void CancelEdit()
        {
            if (DoesModelImplementIEditableObject)
            {
                (this.ToIEditableModel().Model as IEditableObject).CancelEdit();
                this.ToIEditableModel().TraceChanges = false;
                this.ToIEditableModel().IsChanged = false;
                return;
            }

            if (this.memento != null)
            {
                this.ToIEditableModel().TraceChanges = false;
                this.memento.Restore(this.ToIEditableModel().Model);
                foreach (var pair in this.memento.StoredProperties)
                {
                    this.OnPropertyChanged(this, new PropertyChangedEventArgs(pair.Key.Name));
                }
                this.ToIEditableModel().IsChanged = false;
                this.memento = null;
            }
        }

        public virtual void EndEdit()
        {
            if (DoesModelImplementIEditableObject)
            {
                (this.ToIEditableModel().Model as IEditableObject).EndEdit();
                this.ToIEditableModel().TraceChanges = false;
                this.ToIEditableModel().IsChanged = false;
                return;
            }

            if (this.memento != null)
            {
                this.ToIEditableModel().TraceChanges = false;
                this.ToIEditableModel().IsChanged = false;
                this.memento = null;
            }
        }
        #endregion
        private void HandleSetPropertyValue(PropertyInfo property, object component, object value)
        {
            if (property.CanRead && object.Equals(value, (property.GetValue(AsIEditableModel(component).Model))))
            {
                return;
            }
            property.SetValue(AsIEditableModel(component).Model, value, null);
            this.OnPropertyChanged(this, new PropertyChangedEventArgs(property.Name));

            // Raise event for related computed properties.
            List<string> computedProperties;
            if (computedRelationShip.TryGetValue(property.Name, out computedProperties))
            {
                foreach (var computedProperty in computedProperties)
                {
                    this.OnPropertyChanged(this, new PropertyChangedEventArgs(computedProperty));
                }
            }
            if (this.ToIEditableModel().TraceChanges) this.ToIEditableModel().IsChanged = true;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.propertyChanged != null)
            {
                this.propertyChanged(this, e);
            }
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
            if (!generateModelCustomProperties)
            {
                throw new NotImplementedException();
            }
            IList<PropertyDescriptor> propertyDescriptors = new List<PropertyDescriptor>();

            var properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                // Need this copy of property for use in the closure
                var propertyCopy = property;

                Func<object, object> getter = null;
                if (property.CanRead)
                {
                    getter = (component) =>
                    {
                        return propertyCopy.GetValue(AsIEditableModel(component).Model, null);
                    };
                }

                Action<object, object> setter = null;
                if (property.CanWrite)
                {
                    setter = (component, value) =>
                    {
                        HandleSetPropertyValue(propertyCopy, component, value);
                    };
                }

                var propertyDescriptor = EntityTypeConverter.CreatePropertyDescriptor(
                    property.Name,
                    typeof(TModel),
                    property.PropertyType,
                    getter,
                    setter);

                propertyDescriptors.Add(propertyDescriptor);
            }

            // Build Computed relationships
            BuildComputeRelationship(properties);

            return new PropertyDescriptorCollection(propertyDescriptors.ToArray());
        }

        private void BuildComputeRelationship(PropertyInfo[] properties)
        {
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<ComputeRelationshipAttribute>();
                if (attribute == null) continue;

                string[] referenceColumns = attribute.ReferenceColumns;
                foreach (var referenceColumn in referenceColumns)
                {
                    List<string> referenceComputedColumns;
                    if (computedRelationShip.TryGetValue(referenceColumn, out referenceComputedColumns))
                    {
                        referenceComputedColumns.Add(property.Name);
                    }
                    else
                    {
                        referenceComputedColumns = new List<string>();
                        referenceComputedColumns.Add(property.Name);
                        computedRelationShip.Add(referenceColumn, referenceComputedColumns);
                    }
                }

                // 
                string[] arrtributCcomputedColumns = attribute.ComputedColumns;
                foreach (var computedColumn in arrtributCcomputedColumns)
                {
                    List<string> computedColumns;
                    if (computedRelationShip.TryGetValue(property.Name, out computedColumns))
                    {
                        computedColumns.Add(computedColumn);
                    }
                    else
                    {
                        computedColumns = new List<string>();
                        computedColumns.Add(computedColumn);
                        computedRelationShip.Add(property.Name, computedColumns);
                    }
                }
            }
        }

        public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public virtual object GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }

        public IEditableModel<TModel, TEntity> ToIEditableModel()
        {
            return this as IEditableModel<TModel, TEntity>;
        }

        private static IEditableModel<TModel, TEntity> AsIEditableModel(object component)
        {
            return component as IEditableModel<TModel, TEntity>;
        }
        #endregion
    }
}