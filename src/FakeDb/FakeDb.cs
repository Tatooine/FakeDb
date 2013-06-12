using System;
using System.Data.Entity;

namespace FakeDb
{
    public class FakeDb
    {
        readonly ICache _cache;

        public FakeDb(IIdGenerator idGenerator = null)
        {
            _cache = new Cache(idGenerator ?? new IdGenerator(), new ObjectGraph());
        }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return new InMemorySet<TEntity>(_cache.For(typeof(TEntity)));
        }
    }
}