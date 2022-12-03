using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class EtatCivil : BaseObject<int>
    {
        public virtual string Etatcivil { get; set; }
    }
}
