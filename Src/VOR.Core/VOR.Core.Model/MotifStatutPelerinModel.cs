using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class MotifStatutPelerinModel : BaseModel<MotifStatutPelerin, IMotifStatutPelerinRepository, int>
    {
        public MotifStatutPelerinModel(IMotifStatutPelerinRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public IList<MotifStatutPelerin> GetByStatutPelerin(int statutID)
        {
            return _repository.GetByStatutPelerin(statutID);
        }
    }
}