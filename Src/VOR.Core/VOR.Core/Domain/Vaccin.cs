using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Vaccin : BaseObject<int>
    {
        public virtual string Nom { get; set; }
    }
}
