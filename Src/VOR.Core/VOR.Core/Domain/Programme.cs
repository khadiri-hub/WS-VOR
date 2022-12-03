using System;
using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Programme : BaseObject<int>
    {
        public virtual Vol Vol { get; set; }
        public virtual Evenement Evenement { get; set; }
        public virtual string Nom { get; set; }
        public virtual int PrixAPartirDe { get; set; }
        public virtual IList<TProgrammeHotel> Hotels { get; set; }
        public virtual TypeProgramme TypeProgramme { get; set; }
    }
}
