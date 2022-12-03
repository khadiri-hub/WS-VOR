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
    public class RefVilleRepository : Repository<RefVille, int>, IRefVilleRepository
    {
        public RefVilleRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public IList<RefVille> GetVilleByRegionID(int regionID)
        {
            IList<RefVille> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                ICriteria criteriaQuery = session.CreateCriteria(typeof(RefVille));
                criteriaQuery.Add(Restrictions.Eq("RefRegion.ID", regionID));

                results = criteriaQuery.List<RefVille>();
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