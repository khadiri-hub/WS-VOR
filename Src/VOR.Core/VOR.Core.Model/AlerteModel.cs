
using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class AlerteModel : BaseModel<Alerte, IAlerteRepository>
    {
        public AlerteModel(IAlerteRepository alerteRefRepository, IUnitOfWork unitOfWork)
            : base(alerteRefRepository, unitOfWork)
        {
        }

        public IList<Alerte> GetAlerteByType(int typeAlerteID)
        {
            return _repository.GetAlerteByType(typeAlerteID);
        }
    }
}