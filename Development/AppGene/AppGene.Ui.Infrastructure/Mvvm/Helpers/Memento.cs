using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppGene.Ui.Infrastructure.Mvvm.Helpers
{
    //http://www.codeproject.com/Articles/35066/Generic-implementation-of-IEditableObject-via-Type
    public class Memento<T>
    {
        public Dictionary<PropertyInfo, object> StoredProperties { get; } = new Dictionary<PropertyInfo, object>();

        public Memento(T originator)
        {
            var propertyInfos =
                typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       .Where(p => p.CanRead && p.CanWrite);

            foreach (var property in propertyInfos)
            {
                this.StoredProperties[property] = property.GetValue(originator, null);
            }
        }

        public void Restore(T originator)
        {
            foreach (var pair in this.StoredProperties)
            {
                pair.Key.SetValue(originator, pair.Value, null);
            }
        }
    }
}