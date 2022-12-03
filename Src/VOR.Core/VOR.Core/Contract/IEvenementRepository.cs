
using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IEvenementRepository : IRepository<Evenement, int>
    {
        bool IsEvenementSupprimable(decimal id);

        IList<Evenement> GetEvenementsEnCours();
    }
}
