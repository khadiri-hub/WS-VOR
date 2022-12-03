using System;
using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Parking : BaseObject<int>
    {
        public virtual TypeVehicule TypeVehicule { get; set; }
        public virtual TitreAccesType Type { get; set; }
        public virtual TitreAccesSupport Support { get; set; }
        public virtual string Nom { get; set; }
        public virtual int Jauge { get; set; }
        public virtual int JourneeDebut { get; set; }
        public virtual int JourneeFin { get; set; }
        public virtual string EvenementType { get; set; }
        public virtual IList<ParkingQuota> Quotas { get; set; }
    }
}