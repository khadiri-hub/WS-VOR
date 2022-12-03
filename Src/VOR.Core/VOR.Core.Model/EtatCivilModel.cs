using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class EtatCivilModel : BaseModel<EtatCivil, IEtatCivilRepository>
    {
        public EtatCivilModel(IEtatCivilRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}