using System;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;
using NHibernate;
using VOR.Utils;

namespace VOR.Core.Repository.NH.Repositories
{
    public class PersonAuthRepository : Repository<PersonAuth, int>, IPersonAuthRepository
    {
        public PersonAuthRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

      
        public PersonAuth GetPersonAuthByLoginByPwd(string login, string pwd)
        {
            PersonAuth result = null;
            try
            {
                ISession session = SessionFactory.GetCurrentSession();
                IQuery query = session.CreateSQLQuery("exec PS_PRESTA_PERSON_GetAuth @Login=:Login, @Passwd=:Pwd");
                query = query.SetString("Login", login);
                query = query.SetString("Pwd", pwd);

                result = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(PersonAuth))).UniqueResult<PersonAuth>();
            }
            catch (Exception ex)
            {
                Logger.Current.Error("Erreur GetPersonAuthByLoginByPwd", ex);
            }
            return result;
        }
    }
}