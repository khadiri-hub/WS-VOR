using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOR.Core.Domain
{
    public class Recu : BaseObject<int>
    {
        public virtual string Numero { get; set; }

        public virtual int Montant { get; set; }

        public virtual Pelerin Pelerin { get; set; }

        public virtual Utilisateur UtilisateurCreation { get; set; }

        public virtual Utilisateur UtilisateurModification { get; set; }

        public virtual DateTime? DateCreation { get; set; }

        public virtual DateTime? DateModification { get; set; }

        public virtual byte[] Image { get; set; }
    }
}
