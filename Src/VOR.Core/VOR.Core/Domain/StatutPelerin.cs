using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOR.Core.Domain
{
    public class StatutPelerin : BaseObject<int>
    {
        public virtual string Nom { get; set; }
        public virtual IList<MotifStatutPelerin> Motifs { get; set; }
    }
}
