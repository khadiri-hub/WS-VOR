using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class TypePersonne : BaseObject<int>
    {
        public virtual string Fonction { get; set; }
    }
}
