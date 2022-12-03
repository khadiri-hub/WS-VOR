using System;
using System.Collections.Generic;
using VOR.Core.Contract;
using VOR.Core.Domain;
using VOR.Core.Domain.Vues;

namespace VOR.Core.Contract
{
    public interface IParkingRepository : IRepository<Parking, int>
    {
        IList<VueParking> GetVueParkingByEvent(int eventID);
    }
}
