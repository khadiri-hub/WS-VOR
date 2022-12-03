using VOR.Core.Domain;
using VOR.Core.Contract;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class DirectionRepository : Repository<Direction, int>, IDirectionRepository
    {
        public DirectionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        
    }
}