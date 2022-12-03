using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Utilisateur : BaseObject<int>
    {
        public virtual TypeUtilisateur TypeUtilisateur { get; set; }
        public virtual Agence Agence { get; set; }
        public virtual string Nom { get; set; }
        public virtual string Prenom { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string Telef { get; set; }
    }
}
