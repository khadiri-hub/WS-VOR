using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class TypeAgenceRepository : Repository<TypeAgence, int>, ITypeAgenceRepository
    {
        public TypeAgenceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}