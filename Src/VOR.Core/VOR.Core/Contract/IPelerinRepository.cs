using System.Collections.Generic;
using VOR.Core.Domain;
using VOR.Core.Domain.Vues;

namespace VOR.Core.Contract
{
    public interface IPelerinRepository : IRepository<Pelerin, int>
    {
        IList<VuePelerin> GetPelerinByEventIDAndAgenceID(int? eventID, int? agenceID, int? chambreID, int? statutPelerinID, int? motifStatutPelerinID);

        bool SetBadgeToDownload(bool toDownload, IList<int> lstIdPelerin);

        IList<Pelerin> GetPelerinToDownloadBadge(int agenceID);

        void RemovePelerinPelerin(int pelerinID1, int pelerinID2);

        bool isColorExist(List<int> lstIdPelerinInRelation, string color, int eventID);

        bool IsPelerinExistByYear(string nomFR, string prenomFR, int dateCreationYear);

        IList<Pelerin> GetPelerinsByChambreID(int eventID, int chambreID);

        IList<VuePelerin> GetPelerinSansMakkahChambreByEventID(int eventID, int? agenceID);

        IList<VuePelerin> GetPelerinSansMakkahChambreByEventEncours(List<int> lstEventIds, int? agenceID);

        IList<VuePelerin> GetPelerinSansMedineChambreByEventEncours(List<int> lstEventIds, int? agenceID);

        IList<VuePelerin> GetPelerinSansMedineChambreByEventID(int eventID, int? agenceID);

        void SetStatutPelerin(int? statutPelerinID, int? motifStatutPelerinID, IList<int> lstIdPelerin);

        IList<Pelerin> GetPelerinsByEventID(int? eventID);

        int GetNbrPelerinsByEventID(int? eventID);

        int GetNbrPelerinsByEventIDs(List<int> eventIds);

        IList<Pelerin> GetListPelerinsByEventIds(List<int> eventIds);
    }
}
