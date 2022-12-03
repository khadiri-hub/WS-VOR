using System;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class IntervenantDroitModel : BaseModel<IntervenantDroit, IIntervenantDroitRepository>
    {
        public IntervenantDroitModel(IIntervenantDroitRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public IntervenantDroit GetByPerId(int perId)
        {
            return this._repository.GetByPerId(perId);
        }
    }
}