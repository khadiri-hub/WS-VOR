
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class ParkingQuotaReferentRepository : Repository<ParkingQuotaReferent, int>, IParkingQuotaReferentRepository
    {
        public ParkingQuotaReferentRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
   }
}
