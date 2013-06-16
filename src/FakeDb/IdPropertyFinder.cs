using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FakeDb
{
    public interface IIdPropertyFinder
    {
        PropertyInfo Find(Type type);
        void RegisterIdName(Type type, string name);
    }

    class IdPropertyFinder : IIdPropertyFinder
    {
        public const string DefaultIdName = "Id";
        Dictionary<Type, PropertyInfo> _idPropMap = new Dictionary<Type, PropertyInfo>();

        public PropertyInfo Find(Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            PropertyInfo idProp;
            if (!_idPropMap.TryGetValue(type, out idProp))
            {
                idProp = type.GetProperty(DefaultIdName, ReflectionSettings.AllInstance);
                _idPropMap.Add(type, idProp);
            }

            return idProp;
        }

        public void RegisterIdName(Type type, string idName)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (idName == null)
                throw new ArgumentNullException("idName");

            var prop = type.GetProperty(idName, ReflectionSettings.AllInstance);

            if(prop != null)
                _idPropMap.Add(type, prop);
        }
    }
}
