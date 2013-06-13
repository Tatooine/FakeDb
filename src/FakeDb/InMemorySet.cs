using System;
using System.Collections.Generic;

namespace FakeDb
{
    public interface IInMemorySet
    {
        object Add(object item);
        object Remove(object item);
        ISet<object> Items { get; }
    }

    public class InMemorySet : IInMemorySet
    {
        readonly IIdGenerator _idGenerator;
        readonly ICache _cache;
        readonly IObjectGraph _objectGraph;
        readonly HashSet<object> _set = new HashSet<object>();

        public InMemorySet(IIdGenerator idGenerator, ICache cache, IObjectGraph objectGraph)
        {
            if (idGenerator == null) throw new ArgumentNullException("idGenerator");
            if (cache == null) throw new ArgumentNullException("cache");
            if (objectGraph == null) throw new ArgumentNullException("objectGraph");

            _idGenerator = idGenerator;
            _cache = cache;
            _objectGraph = objectGraph;
        }

        public object Add(object item)
        {
            if(item == null)
                throw new ArgumentNullException("item");

            if (_set.Contains(item))
                return item;

            var identified = _idGenerator.Identify(item);

            _set.Add(identified);

            Meterialize(identified);

            return item;
        }

        public object Remove(object item)
        {
            _set.Remove(item);
            return item;
        }

        public ISet<object> Items { get { return _set; } }

        void Meterialize(object item)
        {
            foreach (var obj in _objectGraph.Traverse(item))
            {
                if(obj == item)
                    continue;

                var s = (InMemorySet) _cache.For(obj.GetType());
                s.Add(obj);
            }
        }
    }
}