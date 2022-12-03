using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class StatutPelerinModel : BaseModel<StatutPelerin, IStatutPelerinRepository, int>
    {
        public StatutPelerinModel(IStatutPelerinRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}