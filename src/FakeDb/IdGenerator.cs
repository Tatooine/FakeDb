using System;
using System.Reflection;
using System.Threading;

namespace FakeDb
{
    public interface IIdGenerator
    {
        object Identify(object instance);
    }

    public class IdGenerator : IIdGenerator
    {
        readonly IIdPropertyFinder _idPropertyFinder;
        int _nextId;
       
        public IdGenerator(IIdPropertyFinder idPropertyFinder = null)
        {
            _idPropertyFinder = idPropertyFinder ?? new IdPropertyFinder();
        }

        public object Identify(object instance)
        {
            var property = _idPropertyFinder.Find(instance.GetType());

            if (property == null)
                return instance;

            if (property.GetSetMethod(true) == null)
                return instance;

            var pval = property.GetValue(instance, null);

            if (!IsInitialValue(pval))
                return instance;

            var nextId = Interlocked.Increment(ref _nextId);

            property.SetValue(instance, property.PropertyType == typeof(string) ? (object)nextId.ToString() : Convert.ChangeType(nextId, property.PropertyType), null);
            return instance;
        }
        
        static bool IsInitialValue(object obj)
        {
            if (obj == null)
                return true;

            Type type = obj.GetType();
            if (type.IsValueType && Activator.CreateInstance(type).Equals(obj))
            {
                return true;
            }

            return false;
        }
    }
}