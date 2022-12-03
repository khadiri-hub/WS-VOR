using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Personne : BaseObject<int>
    {
        public virtual TypePersonne TypePersonne { get; set; }
        public virtual Agence Agence { get; set; }
        public virtual string NomFR { get; set; }
        public virtual string PrenomFR { get; set; }
        public virtual string NomAR { get; set; }
        public virtual string PrenomAR { get; set; }
        public virtual string Telef { get; set; }

        public virtual string NomPrenom
        {
            get
            {
                return string.Format("{0} {1}", this.PrenomAR, this.NomAR);
            }
        }

    }
}
