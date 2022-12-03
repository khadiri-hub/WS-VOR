
using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class VisaModel : BaseModel<Visa, IVisaRepository>
    {
        public VisaModel(IVisaRepository visaRepository, IUnitOfWork unitOfWork)
            : base(visaRepository, unitOfWork)
        {
        }
    }
}