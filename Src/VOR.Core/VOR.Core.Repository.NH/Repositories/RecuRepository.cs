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
    public class RecuRepository : Repository<Recu, int>, IRecuRepository
    {
        public RecuRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public IList<Recu> GetRecuByPelerinID(int pelerinID)
        {
            IList<Recu> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                ICriteria criteriaQuery = session.CreateCriteria(typeof(Recu));
                criteriaQuery.Add(Restrictions.Eq("Pelerin.ID", pelerinID));

                results = criteriaQuery.List<Recu>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }

            return results;
        }
    }
}
