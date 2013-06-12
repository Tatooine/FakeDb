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
        int _nextId;

        public object Identify(object instance)
        {
            var property = instance.GetType().GetProperty("Id",
                                                 BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (property == null)
                return instance;

            if (property.SetMethod == null)
                return instance;

            var nextId = Interlocked.Increment(ref _nextId);

            property.SetValue(instance, property.PropertyType == typeof(string) ? (object)nextId.ToString() : nextId);
            return instance;
        }
    }
}