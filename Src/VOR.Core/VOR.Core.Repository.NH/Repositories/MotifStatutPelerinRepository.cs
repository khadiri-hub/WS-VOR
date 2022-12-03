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
    public class MotifStatutPelerinRepository : Repository<MotifStatutPelerin, int>, IMotifStatutPelerinRepository
    {
        public MotifStatutPelerinRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IList<MotifStatutPelerin> GetByStatutPelerin(int statutID)
        {
            IList<MotifStatutPelerin> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                ICriteria criteriaQuery = session.CreateCriteria(typeof(MotifStatutPelerin));
                criteriaQuery.Add(Restrictions.Eq("StatutPelerin.ID", statutID));

                results = criteriaQuery.List<MotifStatutPelerin>();
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