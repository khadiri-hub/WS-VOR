using System.Collections.Generic;

namespace VOR.Core.Domain
{
    public class Direction : BaseObject<int>
    {
        #region Properties

        public virtual string Nom { get; set; }

        #endregion Properties
    }
}