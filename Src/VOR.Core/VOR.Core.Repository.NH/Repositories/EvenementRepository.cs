
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
    public class EvenementRepository : Repository<Evenement, int>, IEvenementRepository
    {
        public EvenementRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public bool IsEvenementSupprimable(decimal id)
        {
            return SessionFactory.GetBooleanByNamedQuery("IsEvenementSupprimable", id, "Id");
        }

        public IList<Evenement> GetEvenementsEnCours()
        {
            try
            {
                ICriteria criteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria<Evenement>()
                .Add(Restrictions.Eq("EnCours", true));

                return criteriaQuery.List<Evenement>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }
        }

      

    }
}
