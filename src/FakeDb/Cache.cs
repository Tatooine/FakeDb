using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FakeDb
{
    public interface ICache
    {
        IInMemorySet For(Type type);
    }

    public class Cache : ICache
    {
        readonly IIdGenerator _idGenerator;
        readonly IObjectGraph _objectGraph;
        readonly IEnumerable<IMaterializationHook> _materializationHooks;
        readonly ConcurrentDictionary<Type, IInMemorySet> _dictionary = new ConcurrentDictionary<Type, IInMemorySet>();

        public Cache(IIdGenerator idGenerator, IObjectGraph objectGraph, IEnumerable<IMaterializationHook> materializationHooks)
        {
            if (idGenerator == null) throw new ArgumentNullException("idGenerator");
            if (objectGraph == null) throw new ArgumentNullException("objectGraph");
            if (materializationHooks == null) throw new ArgumentNullException("materializationHooks");

            _idGenerator = idGenerator;
            _objectGraph = objectGraph;
            _materializationHooks = materializationHooks;
        }

        public IInMemorySet For(Type type)
        {
            return _dictionary.GetOrAdd(FindMostCommonBase(type), _ => new InMemorySet(_idGenerator, this, _objectGraph, _materializationHooks) );
        }

        static Type FindMostCommonBase(Type type)
        {
            return type.BaseType == null || type.BaseType == typeof(object) ? type : FindMostCommonBase(type.BaseType);
        }
    }
}