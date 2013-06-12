using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace FakeDb
{
    public class InMemorySet<TEntity> : IDbSet<TEntity> where TEntity : class 
    {
        readonly IInternalSet _internalSet;

        public InMemorySet(IInternalSet internalSet)
        {
            if (internalSet == null) throw new ArgumentNullException("internalSet");
            _internalSet = internalSet;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _internalSet.Items.Cast<TEntity>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalSet.Items.GetEnumerator();
        }

        public Expression Expression { get; private set; }
        public Type ElementType { get { return typeof (TEntity); } }
        public IQueryProvider Provider { get; private set; }

        public TEntity Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public TEntity Add(TEntity entity)
        {
            _internalSet.Add(entity);
            return entity;
        }

        public TEntity Remove(TEntity entity)
        {
            _internalSet.Remove(entity);
            return entity;
        }

        public TEntity Attach(TEntity entity)
        {
            return entity;
        }

        public TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public ObservableCollection<TEntity> Local { get { throw new NotSupportedException(); } }

    }
}