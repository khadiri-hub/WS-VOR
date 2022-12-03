using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Contract
{
    public interface IChambreRepository : IRepository<Chambre, int>
    {
        IList<Chambre> GetChambreByEventID(int eventID);

        IList<Chambre> GetChambreByEventIDAndVilleID(int eventID, int villeID, int? agenceID);

        IList<Chambre> GetChambreByEventEnCoursAndVilleID(int villeID);

        IList<Chambre> GetChambreByListEventIDAndVilleID(List<int> eventIDs, int? villeID);

        bool IsChambreMakkahSupprimable(decimal id);

        bool IsChambreMedineSupprimable(decimal id);

        bool DeletePelerinsChambreMakkah(IList<int> lstIdPelerin);

        bool DeletePelerinsChambreMedine(IList<int> lstIdPelerin);

        bool isNumeroChambreExist(string numeroChambre, int chambreID);
    }
}