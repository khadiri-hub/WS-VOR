using System;
using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Visa : BaseObject<int>
    {
        public virtual string Commentaire { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int Validite { get; set; }
        public virtual DateTime? DateCreation { get; set; }
        public virtual DateTime? DateModification { get; set; }
        public virtual Utilisateur UtilisateurCreation { get; set; }
        public virtual Utilisateur UtilisateurModification { get; set; }
        public virtual byte[] Image { get; set; }
    }
}
