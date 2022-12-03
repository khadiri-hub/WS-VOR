using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class TypeChambre : BaseObject<int>
    {
        public virtual Programme Programme { get; set; }
        public virtual string Nom { get; set; }
        public virtual int PrixRs { get; set; }
        public virtual int Code { get; set; }
    }
}
