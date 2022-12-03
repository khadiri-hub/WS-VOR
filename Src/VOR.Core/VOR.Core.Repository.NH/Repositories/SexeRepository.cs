using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class SexeRepository : Repository<Sexe, int>, ISexeRepository
    {
        public SexeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}