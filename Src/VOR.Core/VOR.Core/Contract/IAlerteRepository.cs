using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IAlerteRepository : IRepository<Alerte, int>
    {
        IList<Alerte> GetAlerteByType(int typeAlerteID);
    }
}