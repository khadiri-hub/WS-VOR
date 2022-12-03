using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class TProgrammeHotel
    {
        public virtual Programme Programme { get; set; }
        public virtual Hotel Hotel { get; set; }

        #region NHibernate Composite Key Requirements

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as TProgrammeHotel;
            if (t == null) return false;
            if (Programme.ID == t.Programme.ID
             && Hotel.ID == t.Hotel.ID)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ Programme.ID.GetHashCode();
            hash = (hash * 397) ^ Hotel.ID.GetHashCode();

            return hash;
        }

        #endregion
    }
}
