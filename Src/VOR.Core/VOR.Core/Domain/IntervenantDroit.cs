using System;
using System.Collections.Generic;

namespace VOR.Core.Domain
{
    public class IntervenantDroit : BaseObject<int>
    {
        public virtual string Nom { get; set; }

        public virtual int Level { get; set; }

        public virtual IntervenantType IntervenantType { get; set; }
    }
}