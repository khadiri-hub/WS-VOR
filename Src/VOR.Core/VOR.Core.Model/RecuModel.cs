using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class RecuModel : BaseModel<Recu, IRecuRepository, int>
    {
        public RecuModel(IRecuRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public IList<Recu> GetRecuByPelerinID(int pelerinID)
        {
            return _repository.GetRecuByPelerinID(pelerinID);
        }
    }
}
