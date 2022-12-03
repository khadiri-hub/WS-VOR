using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class VilleRepository : Repository<Ville, int>, IVilleRepository
    {
        public VilleRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public bool IsVilleSupprimable(decimal id)
        {
            return SessionFactory.GetBooleanByNamedQuery("IsVilleSupprimable", id, "Id");
        }
    }
}