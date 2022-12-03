using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.UnitOfWork;
using NHibernate;
using System;

namespace VOR.Core.Repository.NH.Repositories
{
    public abstract class Repository<T, EntityKey> : IRepository<T, EntityKey> where T : IAggregateRoot
    {
        private IUnitOfWork _uow;
        protected TimeSpan _cacheDuration = new TimeSpan(0,1,0,0);

        public Repository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Insert(T entity)
        {
            try
            {
                SessionFactory.GetCurrentSession().BeginTransaction();
                SessionFactory.GetCurrentSession().Save(entity);
                SessionFactory.GetCurrentSession().Transaction.Commit();
            }
            catch
            {
                if (SessionFactory.GetCurrentSession().Transaction != null)
                {
                    SessionFactory.GetCurrentSession().Transaction.Rollback();
                }
                throw;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                SessionFactory.GetCurrentSession().BeginTransaction();
                SessionFactory.GetCurrentSession().Delete(entity);
                SessionFactory.GetCurrentSession().Transaction.Commit();
            }
            catch
            {
                if (SessionFactory.GetCurrentSession().Transaction != null)
                {
                    SessionFactory.GetCurrentSession().Transaction.Rollback();
                }
                throw;
            }
        }

        public void DeleteByID(EntityKey Id)
        {
            Delete(LoadByID(Id));
        }

        public void Update(T entity)
        {
            try
            {
                SessionFactory.GetCurrentSession().BeginTransaction();
                SessionFactory.GetCurrentSession().SaveOrUpdate(entity);
                SessionFactory.GetCurrentSession().Transaction.Commit();
            }
            catch
            {
                if (SessionFactory.GetCurrentSession().Transaction != null)
                {
                    SessionFactory.GetCurrentSession().Transaction.Rollback();
                }
                throw;
            }
        }

        public T GetByID(EntityKey id)
        {
            return SessionFactory.GetCurrentSession().Get<T>(id);
        }

        public T LoadByID(EntityKey id)
        {
            return SessionFactory.GetCurrentSession().Load<T>(id);
        }

        public IList<T> GetAll()
        {
            ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));

            return (List<T>)CriteriaQuery.List<T>();
        }

        public IList<T> GetAll(int index, int count)
        {
            ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));

            return (List<T>)CriteriaQuery.SetFetchSize(count).SetFirstResult(index).List<T>();
        }

        public virtual void InsertOrUpdate(T entity)
        {
            try
            {
                SessionFactory.GetCurrentSession().BeginTransaction();
                SessionFactory.GetCurrentSession().SaveOrUpdate(entity);
                SessionFactory.GetCurrentSession().Transaction.Commit();
            }
            catch
            {
                if (SessionFactory.GetCurrentSession().Transaction != null)
                {
                    SessionFactory.GetCurrentSession().Transaction.Rollback();
                }
                throw;
            }
        }

        public virtual void InsertOrUpdate(T entity, ITransaction transaction)
        {
            try
            {
                if (transaction == null)
                {
                    SessionFactory.GetCurrentSession().BeginTransaction();
                }
                SessionFactory.GetCurrentSession().SaveOrUpdate(entity);
                if (transaction == null)
                {
                    SessionFactory.GetCurrentSession().Transaction.Commit();
                }
            }
            catch
            {
                if (transaction == null && SessionFactory.GetCurrentSession().Transaction != null)
                {
                    SessionFactory.GetCurrentSession().Transaction.Rollback();
                }
                throw;
            }
        }
    }
}