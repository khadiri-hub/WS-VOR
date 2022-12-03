using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class PersonRefRepository : Repository<PersonRef, int>, IPersonRefRepository
    {
        public PersonRefRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}