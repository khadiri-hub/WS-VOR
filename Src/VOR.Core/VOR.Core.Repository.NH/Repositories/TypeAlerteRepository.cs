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
    public class TypeAlerteRepository : Repository<TypeAlerte, int>, ITypeAlerteRepository
    {
        public TypeAlerteRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}