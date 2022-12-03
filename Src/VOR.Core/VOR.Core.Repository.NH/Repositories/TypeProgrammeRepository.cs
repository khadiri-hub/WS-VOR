using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class TypeProgrammeRepository : Repository<TypeProgramme, int>, ITypeProgrammeRepository
    {
        public TypeProgrammeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}