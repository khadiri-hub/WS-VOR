using System.Collections.Generic;

namespace VOR.Core.Domain
{
    public class PersonFFT : BaseObject<int>
    {
        #region Properties

        public virtual IntervenantDroit IntervenantDroit { get; set; }

        public virtual int RestaurationDroit { get; set; }

        public virtual int JoueurDroit { get; set; }

        public virtual PersonRef PersonRef { get; set; }

        public virtual int? ParkingDroit { get; set; }

        public virtual int? CompteProfilId { get; set; }

        #endregion Properties
    }
}