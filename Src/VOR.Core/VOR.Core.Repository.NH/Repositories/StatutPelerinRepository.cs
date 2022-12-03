using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class StatutPelerinRepository : Repository<StatutPelerin, int>, IStatutPelerinRepository
    {
        public StatutPelerinRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}