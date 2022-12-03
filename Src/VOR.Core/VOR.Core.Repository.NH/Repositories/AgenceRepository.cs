using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class AgenceRepository : Repository<Agence, int>, IAgenceRepository
    {
        public AgenceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public bool IsAgenceSupprimable(decimal id)
        {
            return SessionFactory.GetBooleanByNamedQuery("IsAgenceSupprimable", id, "Id");
        }
    }
}