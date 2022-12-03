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
    public class AlerteRepository : Repository<Alerte, int>, IAlerteRepository
    {
        public AlerteRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public IList<Alerte> GetAlerteByType(int typeAlerteID)
        {
            IList<Alerte> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                ICriteria criteriaQuery = session.CreateCriteria(typeof(Alerte));
                criteriaQuery.Add(Restrictions.Eq("TypeAlerte.ID", typeAlerteID));

                results = criteriaQuery.List<Alerte>();
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