
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core
{
    public class UtilisateurModel : BaseModel<Utilisateur, IUtilisateurRepository, int>
    {
        public UtilisateurModel(IUtilisateurRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public Utilisateur GetUtilisateurByLoginAndPwd(string login, string pwd)
        {
            return this._repository.GetUtilisateurByLoginAndPwd(login, pwd);
        }
    }
}