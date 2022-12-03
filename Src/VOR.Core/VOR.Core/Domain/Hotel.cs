using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Hotel : BaseObject<int>
    {
        public virtual Ville Ville { get; set; }
        public virtual string Nom { get; set; }
        public virtual string NomFr { get; set; }
        public virtual int Categorie { get; set; }
        public virtual int DistanceToHaram { get; set; }

        public virtual string NomLong
        {
            get
            {
                return string.Format("{0} ( {1} - {2} m )", this.Nom, this.Ville.Nom, DistanceToHaram);
            }
        }
    }
}
