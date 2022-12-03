using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class RefRegion : BaseObject<int>
    {
        public virtual string Nom { get; set; }
    }
}
