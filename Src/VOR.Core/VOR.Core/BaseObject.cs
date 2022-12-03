using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOR.Core
{
    public abstract class BaseObject<TypeOfPrimaryKey> : IAggregateRoot
    {
        public virtual TypeOfPrimaryKey ID { get; set; }
    }
}