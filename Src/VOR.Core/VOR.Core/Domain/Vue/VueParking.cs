using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOR.Core.Domain.Vues
{
    public class VueParking
    {
        public virtual int ID { get; set; }
        public virtual string ParkingNom { get; set; }
        public virtual string TypeVehiculeNom { get; set; }
        public virtual int Jauge { get; set; }
        public virtual string SupportNom { get; set; }
        public virtual string TypeParking { get; set; }
        public DateTime JourneeDebut { get; set; }
        public DateTime JourneeFin { get; set; }
    }
}
