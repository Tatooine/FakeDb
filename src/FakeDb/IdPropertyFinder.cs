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
        Dictionary<Type, string> _idMap =new Dictionary<Type, string>();

        public PropertyInfo Find(Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            string idName;
            if (!_idMap.TryGetValue(type, out idName))
                idName = DefaultIdName;

            return type.GetProperty(idName, ReflectionSettings.AllInstance);
        }

        public void RegisterIdName(Type type, string name)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (name == null)
                throw new ArgumentNullException("name");

            _idMap.Add(type, name);
        }
    }
}
