using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class PersonFFTModel : BaseModel<PersonFFT, IPersonFFTRepository>
    {
        public PersonFFTModel(IPersonFFTRepository personFFTRepository, IUnitOfWork unitOfWork)
            : base(personFFTRepository, unitOfWork)
        {
        }
    }
}