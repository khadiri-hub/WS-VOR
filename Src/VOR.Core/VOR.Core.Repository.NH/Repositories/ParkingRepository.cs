using System;
using System.Collections.Generic;
using System.Linq;
using VOR.Core.Repository.NH.Repositories;
using VOR.Core.UnitOfWork;
using VOR.Utils;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using NHibernate.Criterion;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Domain.Vues;

namespace VOR.Core.Repository.NH.Repositories
{
    public class ParkingRepository : Repository<Parking, int>, IParkingRepository
    {
        public ParkingRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)

        { }

        public IList<VueParking> GetVueParkingByEvent(int eventID)
        {
            ISession session = SessionFactory.GetCurrentSession();
            IList<VueParking> result = null;

            try
            {
                result = SessionFactory.GetCurrentSession().GetNamedQuery("GetVueParkingByEvent")
                    .SetInt32("eventID", eventID)
                    .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(VueParking))).List<VueParking>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error("ParkingRepository => GetVueParkingByEvent", ex);
                throw;
            }

            return result;
        }
    }
}
