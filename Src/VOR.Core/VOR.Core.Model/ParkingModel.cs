using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Domain.Vues;
using VOR.Core.UnitOfWork;

namespace VOR.Core.Model
{
    public class ParkingModel : BaseModel<Parking, IParkingRepository, int>
    {
        public ParkingModel(IParkingRepository repository, IUnitOfWork unitOfWork)
           : base(repository, unitOfWork)
        {

        }

        public IList<VueParking> GetVueParkingByEvent(int eventID)
        {
            return _repository.GetVueParkingByEvent(eventID);
        }
    }
}