using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class TitreAccesSupportRepository : Repository<TitreAccesSupport, int>, ITitreAccesSupportRepository
    {
        public TitreAccesSupportRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}