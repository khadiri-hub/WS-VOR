using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class EtatCivilRepository : Repository<EtatCivil, int>, IEtatCivilRepository
    {
        public EtatCivilRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}
