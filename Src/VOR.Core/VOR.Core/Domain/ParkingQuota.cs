using VOR.Core.Domain;
using System.Collections.Generic;

namespace VOR.Core.Domain
{
    public class ParkingQuota : BaseObject<int>
    {
        public virtual Parking Parking { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual string Nom { get; set; }
        public virtual string NomGb { get; set; }
        public virtual IList<ParkingQuotaPlace> Places { get; set; }
        public virtual IList<ParkingQuotaReferent> Referents { get; set; }
    }
}