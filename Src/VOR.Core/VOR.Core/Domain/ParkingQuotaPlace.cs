using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class ParkingQuotaPlace : BaseObject<int>
    {
        public virtual ParkingQuota Quota { get; set; }
        public virtual int NumJournee { get; set; }
        public virtual int NbPlaces { get; set; }
    }
}