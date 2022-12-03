using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Evenement : BaseObject<int>
    {
        public virtual Pnr Pnr { get; set; }
        public virtual string Nom { get; set; }
        public virtual DateTime DateDebut { get; set; }
        public virtual DateTime DateFin { get; set; }
        public virtual bool EnCours { get; set; }
        public virtual int? Duree { get; set; }
        public virtual string Couleur { get; set; }

        public virtual string TypeEvenement { get; set; }
    }
}
