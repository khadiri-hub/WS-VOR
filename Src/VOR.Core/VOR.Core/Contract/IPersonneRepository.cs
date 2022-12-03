using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IPersonneRepository : IRepository<Personne, int>
    {
        IList<Personne> GetPersonneByTypePersonne(int typePersonneId);
    }
}