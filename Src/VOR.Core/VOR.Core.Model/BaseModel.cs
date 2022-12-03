using System;
using System.Collections.Generic;
using System.Linq;
using VOR.Core.Contract;
using VOR.Core.UnitOfWork;
using System.Web;
using System.Web.Caching;

namespace VOR.Core.Model
{
    public abstract class BaseModel<T, R> : BaseModel<T, R, int> where R : IRepository<T, int>
    {
        public BaseModel(R repository, IUnitOfWork unitOfWork)
            : this(repository, unitOfWork, TimeSpan.Zero, string.Empty)
        {

        }

        public BaseModel(R repository, IUnitOfWork unitOfWork, TimeSpan cacheDuration, string cacheKey)
            : base(repository, unitOfWork, cacheDuration, cacheKey)
        {
        }
    }

    public abstract class CacheableModel<T, R, EntityKey> : BaseModel<T, R, EntityKey> where R : IRepository<T, EntityKey>
    {

        public CacheableModel(R repository, IUnitOfWork unitOfWork, TimeSpan cacheDuration, string cacheKey)
            : base(repository, unitOfWork, cacheDuration, cacheKey)
        {
        }

        public virtual T FromCache_GetByID(EntityKey id)
        {
            var key = _cacheKey + "::" + id;
            var cache = HttpRuntime.Cache;
            T result = (T)cache[key];
            if (result != null)
                return result;
            else
            {
                result = _repository.GetByID(id);
                cache.Insert(key, result, null, DateTime.Now.Add(_cacheDuration), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
            return result;
        }

        public virtual IList<T> FromCache_GetAll()
        {
            var key = "ListOf" + _cacheKey + "::ALL";
            var cache = HttpRuntime.Cache;
            IList<T> results = (IList<T>)cache[key];
            if (results != null)
                return results;
            else
            {
                results = _repository.GetAll().ToList();
                cache.Insert(key, results, null, DateTime.Now.Add(_cacheDuration), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
            return results;
        }
    }

    public abstract class BaseModel<T, R, EntityKey> where R : IRepository<T, EntityKey>
    {
        protected IUnitOfWork _uow;
        protected R _repository;
        protected readonly TimeSpan _cacheDuration;
        protected readonly string _cacheKey;

        public BaseModel(R repository, IUnitOfWork unitOfWork)
            : this(repository, unitOfWork, TimeSpan.Zero, string.Empty)
        {

        }

        public BaseModel(R repository, IUnitOfWork unitOfWork, TimeSpan cacheDuration, string cacheKey)
        {
            this._repository = repository;
            this._uow = unitOfWork;
            this._cacheDuration = cacheDuration;
            this._cacheKey = cacheKey;
        }

        public virtual T GetByID(EntityKey id)
        {
            return _repository.GetByID(id);
        }

        public virtual T LoadByID(EntityKey id)
        {
            return _repository.LoadByID(id);
        }

        public virtual IList<T> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public virtual IList<T> GetAll(int index, int count)
        {
            return _repository.GetAll(index, count);
        }

        public virtual void Delete(T o)
        {
            _repository.Delete(o);
        }

        public virtual void Update(T o)
        {
            _repository.Update(o);
        }

        public virtual void Insert(T o)
        {
            _repository.Insert(o);
        }

        public virtual void InsertOrUpdate(T o)
        {
            _repository.InsertOrUpdate(o);
        }
    }
}