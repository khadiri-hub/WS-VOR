using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Repository.NH.Repositories
{
    public class PersonFFTRepository : Repository<PersonFFT, int>, IPersonFFTRepository
    {
        public PersonFFTRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }        
    }
}