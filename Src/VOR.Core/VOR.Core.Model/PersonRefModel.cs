
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class PersonRefModel : BaseModel<PersonRef, IPersonRefRepository>
    {
        public PersonRefModel(IPersonRefRepository personRefRepository, IUnitOfWork unitOfWork)
            : base(personRefRepository, unitOfWork)
        {
        }
    }
}