using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Domain.Vues;
using VOR.Core.UnitOfWork;
using VOR.Utils;

namespace VOR.Core.Model
{
    public class PelerinModel : BaseModel<Pelerin, IPelerinRepository, int>
    {
        public PelerinModel(IPelerinRepository repository, IUnitOfWork unitOfWork)
           : base(repository, unitOfWork)
        {

        }

        public IList<VuePelerin> GetPelerinByEventIDAndAgenceID(int? eventID, int? agenceID, int? chambreID = null, int? statutPelerinID = null, int? motifStatutPelerinID = null)
        {
            return _repository.GetPelerinByEventIDAndAgenceID(eventID, agenceID, chambreID, statutPelerinID, motifStatutPelerinID);
        }

        public bool SetBadgeToDownload(bool toDownload, IList<int> lstIdPelerin)
        {
            return _repository.SetBadgeToDownload(toDownload, lstIdPelerin);
        }

        public IList<Pelerin> GetPelerinToDownloadBadge(int agenceID)
        {
            return _repository.GetPelerinToDownloadBadge(agenceID);
        }

        public string CrypteDemandeParkingID(int pelerinId)
        {
            string ret;
            SymmetricAlgorithm sa = new SymmetricAlgorithm();
            ret = System.Web.HttpUtility.UrlEncode(sa.Encrypt(pelerinId.ToString()));
            return ret;
        }

        public void RemovePelerinPelerin(int pelerinID1, int pelerinID2)
        {
            _repository.RemovePelerinPelerin(pelerinID1, pelerinID2);
        }

        public bool isColorExist(List<int> lstIdPelerinInRelation, string color, int eventID)
        {
            return _repository.isColorExist(lstIdPelerinInRelation, color, eventID);
        }

        public bool IsPelerinExistByYear(string nomFR, string prenomFR, int dateCreationYear)
        {
            return _repository.IsPelerinExistByYear(nomFR, prenomFR, dateCreationYear);
        }

        public IList<Pelerin> GetPelerinsByChambreID(int eventID, int chambreID)
        {
            return _repository.GetPelerinsByChambreID(eventID, chambreID);
        }

        public IList<VuePelerin> GetPelerinSansMakkahChambreByEventID(int eventID, int? agenceID)
        {
            return _repository.GetPelerinSansMakkahChambreByEventID(eventID, agenceID);
        }

        public IList<VuePelerin> GetPelerinSansMakkahChambreByEventEncours(List<int> lstEventIds, int? agenceID)
        {
            return _repository.GetPelerinSansMakkahChambreByEventEncours(lstEventIds, agenceID);
        }

        public IList<VuePelerin> GetPelerinSansMedineChambreByEventEncours(List<int> lstEventIds, int? agenceID)
        {
            return _repository.GetPelerinSansMedineChambreByEventEncours(lstEventIds, agenceID);
        }

        public IList<VuePelerin> GetPelerinSansMedineChambreByEventID(int eventID, int? agenceID)
        {
            return _repository.GetPelerinSansMedineChambreByEventID(eventID, agenceID);
        }

        public void SetStatutPelerin(int? statutPelerinID, int? motifStatutPelerinID, IList<int> lstIdPelerin)
        {
            _repository.SetStatutPelerin(statutPelerinID, motifStatutPelerinID, lstIdPelerin);
        }

        public IList<Pelerin> GetPelerinsByEventID(int? eventID)
        {
            return _repository.GetPelerinsByEventID(eventID);
        }

        public int GetNbrPelerinsByEventID(int? eventID)
        {
            return _repository.GetNbrPelerinsByEventID(eventID);
        }

        public int GetNbrPelerinsByEventIDs(List<int> eventIds)
        {
            return _repository.GetNbrPelerinsByEventIDs(eventIds);
        }

        public IList<Pelerin> GetListPelerinsByEventIds(List<int> eventIds)
        {
            return _repository.GetListPelerinsByEventIds(eventIds);
        }

    }
}