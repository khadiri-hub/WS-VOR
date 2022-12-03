using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Model;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class TitreAccesTypeModel : BaseModel<TitreAccesType, ITitreAccesTypeRepository>
    {
        public TitreAccesTypeModel(ITitreAccesTypeRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}