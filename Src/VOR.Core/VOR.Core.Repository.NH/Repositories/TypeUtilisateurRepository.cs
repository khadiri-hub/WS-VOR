using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class TypeUtilisateurRepository : Repository<TypeUtilisateur, int>, ITypeUtilisateurRepository
    {
        public TypeUtilisateurRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}