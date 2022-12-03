using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class ParkingQuotaModel : BaseModel<ParkingQuota, IParkingQuotaRepository>
    {
        public ParkingQuotaModel(IParkingQuotaRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}