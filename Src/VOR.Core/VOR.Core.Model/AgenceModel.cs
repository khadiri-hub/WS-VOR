using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class AgenceModel : BaseModel<Agence, IAgenceRepository, int>
    {
        public AgenceModel(IAgenceRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public bool IsAgenceSupprimable(decimal id)
        {
            return _repository.IsAgenceSupprimable(id);
        }
    }
}