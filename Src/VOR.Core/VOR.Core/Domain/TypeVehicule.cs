using System;
using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class TypeVehicule : BaseObject<int>
    {
        public virtual string Nom { get; set; }
        public virtual string NomGb { get; set; }
    }
}