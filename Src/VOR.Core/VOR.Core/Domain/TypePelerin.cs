using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class TypePelerin : BaseObject<int>
    {
        public virtual string Libelle { get; set; }
    }
}
