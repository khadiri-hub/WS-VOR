using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class VaccinModel : BaseModel<Vaccin, IVaccinRepository>
    {
        public VaccinModel(IVaccinRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}