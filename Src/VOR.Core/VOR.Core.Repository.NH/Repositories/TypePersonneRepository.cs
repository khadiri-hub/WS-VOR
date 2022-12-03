using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Repository.NH.Repositories;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class TypePersonneRepository : Repository<TypePersonne, int>, ITypePersonneRepository
    {
        public TypePersonneRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}