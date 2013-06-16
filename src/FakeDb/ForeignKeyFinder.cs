using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FakeDb
{
    public interface IForeignKeyFinder
    {
        PropertyInfo Find(PropertyInfo property);
        void RegisterForeignKey(Type type, string propertyName, string foreignKeyName);
    }

    class ForeignKeyFinderFinder : IForeignKeyFinder
    {
        readonly Dictionary<PropertyInfo, PropertyInfo> _prop2FkMap = new Dictionary<PropertyInfo, PropertyInfo>(); 
        public PropertyInfo Find(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            PropertyInfo fkProp;
            if (!_prop2FkMap.TryGetValue(property, out fkProp))
            {
                fkProp = property.DeclaringType.GetProperty(GetDefaultFkName(property), ReflectionSettings.AllInstance);
                _prop2FkMap.Add(property, fkProp);
            }

            return fkProp;
        }

        public void RegisterForeignKey(Type type, string foreignKeyName, string propertyName)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            if (foreignKeyName == null)
                throw new ArgumentNullException("foreignKeyName");

            var prop = type.GetProperty(propertyName, ReflectionSettings.AllInstance);
            var fk = type.GetProperty(foreignKeyName, ReflectionSettings.AllInstance);
            if (prop != null && fk != null)
                _prop2FkMap.Add(prop, fk);
        }

        string GetDefaultFkName(PropertyInfo property)
        {
            return property.Name + "Id";
        }
    }
}
