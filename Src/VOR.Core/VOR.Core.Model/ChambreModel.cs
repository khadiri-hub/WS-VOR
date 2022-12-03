using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class ChambreModel : BaseModel<Chambre, IChambreRepository>
    {
        public ChambreModel(IChambreRepository repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public IList<Chambre> GetChambreByEventID(int eventID)
        {
            return _repository.GetChambreByEventID(eventID);
        }

        public IList<Chambre> GetChambreByEventIDAndVilleID(int eventID, int villeID, int? agenceID)
        {
            return _repository.GetChambreByEventIDAndVilleID(eventID, villeID, agenceID);
        }

        public IList<Chambre> GetChambreByEventEnCoursAndVilleID(int villeID)
        {
            return _repository.GetChambreByEventEnCoursAndVilleID(villeID);
        }

        public IList<Chambre> GetChambreByListEventIDAndVilleID(List<int> eventIDs, int? villeID)
        {
            return _repository.GetChambreByListEventIDAndVilleID(eventIDs, villeID);
        }

        public bool IsChambreMakkahSupprimable(decimal id)
        {
            return _repository.IsChambreMakkahSupprimable(id);
        }

        public bool IsChambreMedineSupprimable(decimal id)
        {
            return _repository.IsChambreMedineSupprimable(id);
        }

        public bool DeletePelerinsChambreMakkah(IList<int> lstIdPelerin)
        {
            return _repository.DeletePelerinsChambreMakkah(lstIdPelerin);
        }

        public bool DeletePelerinsChambreMedine(IList<int> lstIdPelerin)
        {
            return _repository.DeletePelerinsChambreMedine(lstIdPelerin);
        }

        public bool isNumeroChambreExist(string numeroChambre, int chambreID)
        {
            return _repository.isNumeroChambreExist(numeroChambre, chambreID);
        }
    }
}