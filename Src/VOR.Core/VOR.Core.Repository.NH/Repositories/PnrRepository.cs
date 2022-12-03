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
    public class PnrRepository : Repository<Pnr, int>, IPnrRepository
    {
        public PnrRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public IList<Pnr> GetPnrByEventID(int eventID)
        {
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                return SessionFactory.GetCurrentSession().GetNamedQuery("GetPnrByEventID")
                    .SetInt32("eventID", eventID)
                    .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Pnr))).List<Pnr>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }
        }
    }
}