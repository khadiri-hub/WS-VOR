using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IPersonAuthRepository : IRepository<PersonAuth, int>
    {
        PersonAuth GetPersonAuthByLoginByPwd(string login, string pwd);
    }
}