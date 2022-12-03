using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class TitreAccesSupportModel : BaseModel<TitreAccesSupport, ITitreAccesSupportRepository>
    {
        public TitreAccesSupportModel(ITitreAccesSupportRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}