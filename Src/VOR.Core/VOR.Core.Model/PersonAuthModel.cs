using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class PersonAuthModel : BaseModel<PersonAuth, IPersonAuthRepository>
    {
        public PersonAuthModel(IPersonAuthRepository personAuthRepository, IUnitOfWork unitOfWork)
            : base(personAuthRepository, unitOfWork)
        {
        }

        public PersonAuth GetPersonAuthByLoginByPwd(string login, string pwd)
        {
            return this._repository.GetPersonAuthByLoginByPwd(login, pwd);
        }
    }
}