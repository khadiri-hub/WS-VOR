using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Vol : BaseObject<int>
    { 
        public virtual CompAerienne CompAerienne { get; set; }
        public virtual string Description { get; set; }
    }
}
