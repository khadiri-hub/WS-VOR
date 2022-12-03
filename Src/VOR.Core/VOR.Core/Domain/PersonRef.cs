using System;
using System.Collections.Generic;

namespace VOR.Core.Domain
{
    public class PersonRef : BaseObject<int>
    {
        public virtual string Sexe { get; set; }

        public virtual string Nom { get; set; }

        public virtual string Prenom { get; set; }

        public virtual string NomPrenom
        {
            get
            {
                return string.Format("{0} {1}", this.Nom, this.Prenom);
            }
        }

        public virtual DateTime? DateNaissance { get; set; }
    }
}
