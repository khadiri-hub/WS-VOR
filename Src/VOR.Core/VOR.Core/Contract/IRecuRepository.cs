using System.Collections.Generic;
using VOR.Core.Domain;


namespace VOR.Core.Contract
{
    public interface IRecuRepository : IRepository<Recu, int>
    {
        IList<Recu> GetRecuByPelerinID(int pelerinID);
    }
}
