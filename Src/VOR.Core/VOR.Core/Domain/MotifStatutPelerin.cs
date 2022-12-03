using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOR.Core.Domain
{
    public class MotifStatutPelerin : BaseObject<int>
    {
        public virtual string Nom { get; set; }
        public virtual StatutPelerin StatutPelerin { get; set; }
    }
}
