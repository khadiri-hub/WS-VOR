using NHibernate;
using NHibernate.Criterion;
using System;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;
using VOR.Utils;

namespace VOR.Core.Repository.NH.Repositories
{
    public class UtilisateurRepository : Repository<Utilisateur, int>, IUtilisateurRepository
    {
        public UtilisateurRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public Utilisateur GetUtilisateurByLoginAndPwd(string login, string pwd)
        {
            try
            {
                ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria<Utilisateur>()
                .Add(Restrictions.Eq("Login", login))
                .Add(Restrictions.Eq("Password", pwd));

                return CriteriaQuery.UniqueResult<Utilisateur>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                return null;
            }
        }
    }
}