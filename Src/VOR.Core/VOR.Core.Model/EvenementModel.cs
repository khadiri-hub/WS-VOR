using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class EvenementModel : BaseModel<Evenement, IEvenementRepository>
    {
        public EvenementModel(IEvenementRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public bool IsEvenementSupprimable(decimal id)
        {
            return _repository.IsEvenementSupprimable(id);
        }

        public IList<Evenement> GetEvenementsEnCours()
        {
            return _repository.GetEvenementsEnCours();
        }

   
    }
}