
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core
{
    public class TypeUtilisateurModel : BaseModel<TypeUtilisateur, ITypeUtilisateurRepository, int>
    {
        public TypeUtilisateurModel(ITypeUtilisateurRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}