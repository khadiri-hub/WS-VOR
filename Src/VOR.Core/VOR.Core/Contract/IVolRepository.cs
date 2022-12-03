
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IVolRepository : IRepository<Vol, int>
    {
        bool IsVolSupprimable(decimal id);
    }
}
