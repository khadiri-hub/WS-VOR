using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Pnr : BaseObject<int>
    {
        public virtual string Nom { get; set; }
        public virtual int PrixTotalPnr { get; set; }
        public virtual int CautionDepose { get; set; }
        public virtual int NbrPassager { get; set; }
        public virtual Vol Vol { get; set; }
        public virtual Ville LieuDepart { get; set; }
        public virtual Ville LieuArrivee { get; set; }
        public virtual DateTime? HeureDepart { get; set; }
        public virtual DateTime? HeureArrivee { get; set; }
    }
}
