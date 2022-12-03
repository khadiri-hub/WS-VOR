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
    public class PersonneRepository : Repository<Personne, int>, IPersonneRepository
    {
        public PersonneRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public IList<Personne> GetPersonneByTypePersonne(int typePersonneId)
        {
            IList<Personne> results = null;
            ISession session = SessionFactory.GetCurrentSession();

            try
            {
                ICriteria criteriaQuery = session.CreateCriteria(typeof(Personne));
                criteriaQuery.Add(Restrictions.Eq("TypePersonne.ID", typePersonneId));

                results = criteriaQuery.List<Personne>();
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