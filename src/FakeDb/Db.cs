using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FakeDb.Builtin.MaterializationHooks;

namespace FakeDb
{
    public class Db
    {
        readonly ICache _cache;
        readonly IIdPropertyFinder _idPropertyFinder;
        readonly ForeignKeyFinderFinder _foreignKeyFinder;
        readonly IList<IMaterializationHook> _materializationHooks;

        public Db(IEnumerable<IMaterializationHook> materializationHooks = null)
        {
            _idPropertyFinder = new IdPropertyFinder();
            _foreignKeyFinder = new ForeignKeyFinderFinder();
            _materializationHooks = materializationHooks == null
                                        ? new List<IMaterializationHook>()
                                        : materializationHooks.ToList();

            _cache = new Cache(new IdGenerator(_idPropertyFinder), new ObjectGraph(), _materializationHooks);
        }

        public IInMemorySet Set<TEntity>() where TEntity : class
        {
            return _cache.For(typeof (TEntity));
        }

        public Db WithForeignKeyInitializer()
        {
            _materializationHooks.Add(new ForeignKeyInitializer(_idPropertyFinder, _foreignKeyFinder));
            return this;
        }

        public Db MapId<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> idPropExpr) where TEntity: class 
        {
            var idPropName = GetPropertyNameFromExpression(idPropExpr);
            
            _idPropertyFinder.RegisterIdName(typeof(TEntity), idPropName);

            return this;
        }

        public Db MapForeignKey<TEntity, TProperty, TForeignKey>(Expression<Func<TEntity, TProperty>> propExpr, Expression<Func<TEntity, TForeignKey>> foreignKeyPropExpr)
            where TEntity : class
            where TProperty : class
        {
            var propName = GetPropertyNameFromExpression(propExpr);
            var fkName = GetPropertyNameFromExpression(foreignKeyPropExpr);

            _foreignKeyFinder.RegisterForeignKey(typeof(TEntity), fkName, propName);

            return this;
        }

        static string GetPropertyNameFromExpression<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> expr)
        {
            try
            {
                return ((MemberExpression)expr.Body).Member.Name;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid property expression. Please use the form of 'object.property'", ex);
            }
        }
    }
}