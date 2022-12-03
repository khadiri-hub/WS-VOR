using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Agence : BaseObject<int>
    {
        public virtual string Nom { get; set; }
        public virtual string Alias { get; set; }
        public virtual string Description { get; set; }
        public virtual string Adresse { get; set; }
        public virtual string Telef { get; set; }
        public virtual TypeAgence TypeAgence { get; set; }
    }
}
