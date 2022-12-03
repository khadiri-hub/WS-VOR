using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Repository.NH.Repositories;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class ParkingQuotaPlaceRepository : Repository<ParkingQuotaPlace, int>, IParkingQuotaPlaceRepository
    {
        public ParkingQuotaPlaceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        
    }
}
