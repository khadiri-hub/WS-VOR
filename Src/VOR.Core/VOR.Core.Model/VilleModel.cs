
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core
{
    public class VilleModel : BaseModel<Ville, IVilleRepository, int>
    {
        public VilleModel(IVilleRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public bool IsVilleSupprimable(decimal id)
        {
            return _repository.IsVilleSupprimable(id);
        }
    }
}