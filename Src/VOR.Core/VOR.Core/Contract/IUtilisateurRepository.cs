
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IUtilisateurRepository : IRepository<Utilisateur, int>
    {
        Utilisateur GetUtilisateurByLoginAndPwd(string login, string pwd);
    }
}
