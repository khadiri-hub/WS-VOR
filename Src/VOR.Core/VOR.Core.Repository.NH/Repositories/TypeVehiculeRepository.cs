using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Repository.NH.Repositories;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class TypeVehiculeRepository : Repository<TypeVehicule, int>, ITypeVehiculeRepository
    {
        public TypeVehiculeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}