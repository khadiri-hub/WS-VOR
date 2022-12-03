
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IVilleRepository : IRepository<Ville, int>
    {
        bool IsVilleSupprimable(decimal id);
    }
}
