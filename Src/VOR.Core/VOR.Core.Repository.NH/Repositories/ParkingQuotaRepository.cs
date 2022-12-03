using VOR.Core.Domain;
using VOR.Core.Contract;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class ParkingQuotaRepository : Repository<ParkingQuota, int>, IParkingQuotaRepository
    {
        public ParkingQuotaRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }        
    }
}