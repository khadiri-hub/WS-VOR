using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IIntervenantDroitRepository : IRepository<IntervenantDroit, int>
    {
        IntervenantDroit GetByPerId(int perId);
    }
}