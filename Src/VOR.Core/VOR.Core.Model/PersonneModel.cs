using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class PersonneModel : BaseModel<Personne, IPersonneRepository>
    {
        public PersonneModel(IPersonneRepository personAuthRepository, IUnitOfWork unitOfWork)
            : base(personAuthRepository, unitOfWork)
        {
        }

        public IList<Personne> GetPersonneByTypePersonne(int typePersonneId)
        {
            return _repository.GetPersonneByTypePersonne(typePersonneId);
        }
    }
}