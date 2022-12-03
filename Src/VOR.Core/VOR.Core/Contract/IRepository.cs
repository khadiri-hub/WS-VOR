using System;
using System.Collections.Generic;

namespace VOR.Core.Contract
{
    public interface IRepository<T, EntityKey>
    {
        T GetByID(EntityKey id);

        T LoadByID(EntityKey id);

        IList<T> GetAll();

        IList<T> GetAll(int index, int count);

        void Insert(T o);

        void Delete(T o);

        void Update(T o);

        void InsertOrUpdate(T o);
    }
}