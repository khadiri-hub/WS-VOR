using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface ITypeChambreRepository : IRepository<TypeChambre, int>
    {
        bool IsTypeChambreSupprimable(decimal id);
    }
}