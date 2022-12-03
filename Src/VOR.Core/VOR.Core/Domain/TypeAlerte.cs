using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class TypeAlerte : BaseObject<int>
    {
        public virtual string Code { get; set; }
        public virtual string Libelle { get; set; }
    }
}
