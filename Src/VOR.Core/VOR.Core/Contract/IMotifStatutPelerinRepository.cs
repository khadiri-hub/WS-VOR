using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IMotifStatutPelerinRepository : IRepository<MotifStatutPelerin, int>
    {
        IList<MotifStatutPelerin> GetByStatutPelerin(int statutID);
    }
}