using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface ICompAerienneRepository : IRepository<CompAerienne, int>
    {
        bool IsCompagnieSupprimable(decimal id);
    }
}