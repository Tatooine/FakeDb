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

        public Db(IIdGenerator idGenerator = null, IEnumerable<IMaterializationHook> materializationHooks = null)
        {
            _idPropertyFinder = new IdPropertyFinder();
            _cache = new Cache(idGenerator ?? new IdGenerator(_idPropertyFinder), new ObjectGraph(),
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
            string idPropName;
            try
            {
                idPropName = ((MemberExpression) idPropExpr.Body).Member.Name;
            }
            catch(Exception ex)
            {
                throw new ArgumentException("Invalid property expression. Please use the form as 'object.objId'", ex);
            }

            _idPropertyFinder.RegisterIdName(typeof(TType), idPropName);

            return this;
        }
    }
}