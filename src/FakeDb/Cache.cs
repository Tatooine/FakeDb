using System;
using System.Collections.Concurrent;

namespace FakeDb
{
    public interface ICache
    {
        IInternalSet For(Type type);
    }

    public class Cache : ICache
    {
        readonly IIdGenerator _idGenerator;
        readonly IObjectGraph _objectGraph;
        readonly ConcurrentDictionary<Type, IInternalSet> _dictionary = new ConcurrentDictionary<Type, IInternalSet>();

        public Cache(IIdGenerator idGenerator, IObjectGraph objectGraph)
        {
            if (idGenerator == null) throw new ArgumentNullException("idGenerator");
            if (objectGraph == null) throw new ArgumentNullException("objectGraph");

            _idGenerator = idGenerator;
            _objectGraph = objectGraph;
        }

        public IInternalSet For(Type type)
        {
            return _dictionary.GetOrAdd(FindMostCommonBase(type), _ => new InternalSet(_idGenerator, this, _objectGraph) );
        }

        static Type FindMostCommonBase(Type type)
        {
            return type.BaseType == null || type.BaseType == typeof(object) ? type : FindMostCommonBase(type.BaseType);
        }
    }
}