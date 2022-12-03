using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class CompAerienneModel : BaseModel<CompAerienne, ICompAerienneRepository>
    {
        public CompAerienneModel(ICompAerienneRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public bool IsCompagnieSupprimable(decimal id)
        {
            return _repository.IsCompagnieSupprimable(id);
        }
    }
}