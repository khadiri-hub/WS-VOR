using System;
using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Alerte : BaseObject<int>
    {
        public virtual string Libelle { get; set; }
        public virtual TypeAlerte TypeAlerte { get; set; }
    }
}
