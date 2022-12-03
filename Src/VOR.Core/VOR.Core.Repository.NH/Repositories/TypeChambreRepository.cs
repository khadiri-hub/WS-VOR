using VOR.Core.Domain;
using VOR.Core.Contract;
using VOR.Core.UnitOfWork;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Criterion;
using System;
using VOR.Utils;

namespace VOR.Core.Repository.NH.Repositories
{
    public class TypeChambreRepository : Repository<TypeChambre, int>, ITypeChambreRepository
    {
        public TypeChambreRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public bool IsTypeChambreSupprimable(decimal id)
        {
            return SessionFactory.GetBooleanByNamedQuery("IsTypeChambreSupprimable", id, "Id");
        }
    }
}