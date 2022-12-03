using System;
using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Utils
{

    public class PelerinBadge
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public byte[] Photo { get; set; }
        public string Accompagnant { get; set; }
        public string HotelMakkah { get; set; }
        public string HotelMedine { get; set; }
        public byte[] Visa { get; set; }
        public IList<Pelerin> PelerinsMakkah { get; set; }
        public IList<Pelerin> PelerinsMedine { get; set; }
        public int TypeChambre { get; set; }
    }
}
