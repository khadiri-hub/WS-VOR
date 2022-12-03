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
    public class ProgrammeRepository : Repository<Programme, int>, IProgrammeRepository
    {
        public ProgrammeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public bool IsProgrammeSupprimable(decimal id)
        {
            return SessionFactory.GetBooleanByNamedQuery("IsProgrammeSupprimable", id, "Id");
        }

        public IList<Programme> GetProgrammeByEventID(int eventID)
        {
            IList<Programme> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                ICriteria criteriaQuery = session.CreateCriteria(typeof(Programme));
                criteriaQuery.Add(Restrictions.Eq("Evenement.ID", eventID));

                results = criteriaQuery.List<Programme>();
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