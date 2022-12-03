
using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IRefVilleRepository : IRepository<RefVille, int>
    {
        IList<RefVille> GetVilleByRegionID(int regionID);
    }
}
