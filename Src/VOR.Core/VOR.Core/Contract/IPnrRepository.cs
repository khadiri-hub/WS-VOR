using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IPnrRepository : IRepository<Pnr, int>
    {
        IList<Pnr> GetPnrByEventID(int eventID);
    }
}