using System.Collections.Generic;
using System.Linq;

namespace FakeDb
{
    public class FakeDb
    {
        readonly ICache _cache;

        public FakeDb(IIdGenerator idGenerator = null, IEnumerable<IMaterializationHook> materializationHooks = null)
        {
            _cache = new Cache(idGenerator ?? new IdGenerator(), new ObjectGraph(),
                               materializationHooks ?? Enumerable.Empty<IMaterializationHook>());
        }

        public IInMemorySet Set<TEntity>() where TEntity : class
        {
            return _cache.For(typeof (TEntity));
        }
    }
}