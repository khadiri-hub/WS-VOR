using System.Collections.Generic;

namespace VOR.Core.Domain
{
    public class PersonAuth : BaseObject<int>
    {
        #region Properties

        public virtual string Nom { get; set; }

        public virtual string Prenom { get; set; }

        public virtual string Mail { get; set; }

        public virtual int SocieteId { get; set; }

        #endregion Properties
    }
}