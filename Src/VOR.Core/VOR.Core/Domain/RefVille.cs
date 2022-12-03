using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class RefVille : BaseObject<int>
    {
        public virtual string Nom { get; set; }

        public virtual RefRegion RefRegion { get; set; }
    }
}
