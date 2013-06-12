namespace FakeDb
{
    public class FakeDb
    {
        readonly ICache _cache;

        public FakeDb(IIdGenerator idGenerator = null)
        {
            _cache = new Cache(idGenerator ?? new IdGenerator(), new ObjectGraph());
        }

        public IInMemorySet Set<TEntity>() where TEntity : class
        {
            return _cache.For(typeof(TEntity));
        }
    }
}