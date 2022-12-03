using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class VolRepository : Repository<Vol, int>, IVolRepository
    {
        public VolRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public bool IsVolSupprimable(decimal id)
        {
            return SessionFactory.GetBooleanByNamedQuery("IsVolSupprimable", id, "Id");
        }
    }
}