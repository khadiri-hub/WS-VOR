
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core
{
    public class RefRegionModel : BaseModel<RefRegion, IRefRegionRepository, int>
    {
        public RefRegionModel(IRefRegionRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}