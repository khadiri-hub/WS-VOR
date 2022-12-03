using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class CompAerienneRepository : Repository<CompAerienne, int>, ICompAerienneRepository
    {
        public CompAerienneRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public bool IsCompagnieSupprimable(decimal id)
        {
            return SessionFactory.GetBooleanByNamedQuery("IsCompagnieSupprimable", id, "Id");
        }
    }
}