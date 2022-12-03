using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IProgrammeRepository : IRepository<Programme, int>
    {
        bool IsProgrammeSupprimable(decimal id);

        IList<Programme> GetProgrammeByEventID(int eventID);
    }
}