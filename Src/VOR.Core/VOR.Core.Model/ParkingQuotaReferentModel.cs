using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class ParkingQuotaReferentModel : BaseModel<ParkingQuotaReferent, IParkingQuotaReferentRepository>
    {
        public ParkingQuotaReferentModel(IParkingQuotaReferentRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}