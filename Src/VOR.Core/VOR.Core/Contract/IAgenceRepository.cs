using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IAgenceRepository : IRepository<Agence, int>
    {
        bool IsAgenceSupprimable(decimal Id);
    }
}