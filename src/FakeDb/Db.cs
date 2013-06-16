using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FakeDb
{
    public class Db
    {
        readonly ICache _cache;
        readonly IIdPropertyFinder _idPropertyFinder;

        public Db(IEnumerable<IMaterializationHook> materializationHooks = null)
        {
            _idPropertyFinder = new IdPropertyFinder();

            _cache = new Cache(new IdGenerator(_idPropertyFinder), new ObjectGraph(),
                               materializationHooks ?? Enumerable.Empty<IMaterializationHook>());
        }

        public IInMemorySet Set<TEntity>() where TEntity : class
        {
            return _cache.For(typeof (TEntity));
        }

        public Db MapId<TType, TProperty>(Expression<Func<TType, TProperty>> idPropExpr) 
            where TType: class 
            where TProperty : struct
        {
            var idPropName = ((MemberExpression) idPropExpr.Body).Member.Name;

            _idPropertyFinder.RegisterIdName(typeof(TType), idPropName);

            return this;
        }
    }
}