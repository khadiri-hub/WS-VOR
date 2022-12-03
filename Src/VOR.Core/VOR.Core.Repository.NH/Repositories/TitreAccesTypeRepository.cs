using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class TitreAccesTypeRepository : Repository<TitreAccesType, int>, ITitreAccesTypeRepository
    {
        public TitreAccesTypeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}