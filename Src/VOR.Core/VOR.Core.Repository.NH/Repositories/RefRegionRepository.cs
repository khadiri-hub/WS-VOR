using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class RefRegionRepository : Repository<RefRegion, int>, IRefRegionRepository
    {
        public RefRegionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}