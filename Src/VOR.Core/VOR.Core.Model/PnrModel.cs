using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class PnrModel : BaseModel<Pnr, IPnrRepository>
    {
        public PnrModel(IPnrRepository personFFTRepository, IUnitOfWork unitOfWork)
            : base(personFFTRepository, unitOfWork)
        {
        }

        public IList<Pnr> GetPnrByEventID(int eventID)
        {
            return this._repository.GetPnrByEventID(eventID);
        }
    }
}