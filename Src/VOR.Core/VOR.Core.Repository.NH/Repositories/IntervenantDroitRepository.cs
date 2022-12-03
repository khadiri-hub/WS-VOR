using System;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;
using VOR.Utils;
using NHibernate;

namespace VOR.Core.Repository.NH.Repositories
{
    public class IntervenantDroitRepository : Repository<IntervenantDroit, int>, IIntervenantDroitRepository
    {
        public IntervenantDroitRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public IntervenantDroit GetByPerId(int perId)
        {
            IntervenantDroit results = null;

            ISession s = SessionFactory.GetCurrentSession();

            try
            {
                string hql = "select IntervenantDroit from PersonFFT pfft where pfft.ID=:perId";
                IQuery query = s.CreateQuery(hql);
                query.SetInt32("perId", perId);
                results = query.UniqueResult<IntervenantDroit>();
            }
            catch (Exception exc)
            {
                Logger.Current.Error("Exception IntervenantDroit GetByPerId", exc);
                throw exc;
            }

            return results;
        }
    }
}