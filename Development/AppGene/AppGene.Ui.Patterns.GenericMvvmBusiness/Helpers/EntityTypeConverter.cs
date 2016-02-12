using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Ui.Patterns.GenericMvvmBusiness.Helpers
{
    internal class EntityTypeConverter
        : TypeConverter
    {
        public static PropertyDescriptor CreatePropertyDescriptor<TComponent, TProperty>(
            string name,
            Func<TComponent, TProperty> getter,
            Action<TComponent, TProperty> setter)
        {
            return new EntityPropertyDescriptor<TComponent, TProperty>(name, getter, setter);
        }

        public static PropertyDescriptor CreatePropertyDescriptor<TComponent, TProperty>(
            string name, Func<TComponent, TProperty> getter)
        {
            return new EntityPropertyDescriptor<TComponent, TProperty>(name, getter);
        }

        public static PropertyDescriptor CreatePropertyDescriptor(string name,
            Type componentType,
            Type propertyType,
            Func<object, object> getter,
            Action<object, object> setter)
        {
            return new EntityPropertyDescriptor(name, componentType,
                       propertyType, getter, setter);
        }

        public static PropertyDescriptor CreatePropertyDescriptor(string name,
               Type componentType, Type propertyType, Func<object, object> getter)
        {
            return new EntityPropertyDescriptor(name, componentType,
                                                 propertyType, getter);
        }

        protected class EntityPropertyDescriptor<TComponent, TProperty>
            : SimplePropertyDescriptor
        {
            private Func<TComponent, TProperty> getter;
            private Action<TComponent, TProperty> setter;

            public EntityPropertyDescriptor(string name,
                Func<TComponent, TProperty> getter,
                Action<TComponent, TProperty> setter)
                 : base(typeof(TComponent), name, typeof(TProperty))
            {
                if (getter == null)
                {
                    throw new ArgumentNullException("getter");
                }
                if (setter == null)
                {
                    throw new ArgumentNullException("setter");
                }

                this.getter = getter;
                this.setter = setter;
            }

            public EntityPropertyDescriptor(string name,
                   Func<TComponent, TProperty> getter)
                 : base(typeof(TComponent), name, typeof(TProperty))
            {
                if (getter == null)
                {
                    throw new ArgumentNullException("getter");
                }

                this.getter = getter;
            }

            public override bool IsReadOnly
            {
                get
                {
                    return this.setter == null;
                }
            }

            public override object GetValue(object target)
            {
                TComponent component = (TComponent)target;
                TProperty value = this.getter(component);
                return value;
            }

            public override void SetValue(object target, object value)
            {
                if (!this.IsReadOnly)
                {
                    TComponent component = (TComponent)target;
                    TProperty newValue = (TProperty)value;
                    this.setter(component, newValue);
                }
            }
        }

        protected class EntityPropertyDescriptor :
                 TypeConverter.SimplePropertyDescriptor
        {
            Func<object, object> getter;
            Action<object, object> setter;

            public EntityPropertyDescriptor(string name,
                Type componentType,
                Type propertyType,
                Func<object, object> getter,
                Action<object, object> setter)
                : base(componentType, name, propertyType)
            {
                if (getter == null)
                {
                    throw new ArgumentNullException("getter");
                }
                if (setter == null)
                {
                    throw new ArgumentNullException("setter");
                }

                this.getter = getter;
                this.setter = setter;
            }

            public EntityPropertyDescriptor(string name, Type componentType,
                   Type propertyType, Func<object, object> getter)
                 : base(componentType, name, propertyType)
            {
                if (getter == null)
                {
                    throw new ArgumentNullException("getter");
                }

                this.getter = getter;
            }

            public override bool IsReadOnly
            {
                get
                {
                    return this.setter == null;
                }
            }

            public override object GetValue(object target)
            {
                object value = this.getter(target);
                return value;
            }

            public override void SetValue(object target, object value)
            {
                if (!this.IsReadOnly)
                {
                    object newValue = (object)value;
                    this.setter(target, newValue);
                }
            }
        }
    }
}
