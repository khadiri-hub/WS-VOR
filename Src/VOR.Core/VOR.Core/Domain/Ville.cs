using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Ville : BaseObject<int>
    {
        public virtual string Nom { get; set; }
        public virtual string Code { get; set; }
    }
}
