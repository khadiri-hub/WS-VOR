using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Sexe : BaseObject<int>
    {
        public virtual int IdSexe { get; set; }
        public virtual string Type { get; set; }
    }
}
