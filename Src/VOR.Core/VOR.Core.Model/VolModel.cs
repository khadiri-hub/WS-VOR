
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core
{
    public class VolModel : BaseModel<Vol, IVolRepository, int>
    {
        public VolModel(IVolRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public bool IsVolSupprimable(decimal id)
        {
            return _repository.IsVolSupprimable(id);
        }
    }
}