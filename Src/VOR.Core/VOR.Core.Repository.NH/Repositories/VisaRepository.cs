using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;
using VOR.Utils;

namespace VOR.Core.Repository.NH.Repositories
{
    public class VisaRepository : Repository<Visa, int>, IVisaRepository
    {
        public VisaRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}