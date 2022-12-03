using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class TPelerinHotel
    {
        public virtual Hotel Hotel { get; set; }
        public virtual Pelerin Pelerin { get; set; }

        #region NHibernate Composite Key Requirements

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as TPelerinHotel;
            if (t == null) return false;
            if (Pelerin.ID == t.Pelerin.ID
         && Hotel.ID == t.Hotel.ID)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ Pelerin.ID.GetHashCode();
            hash = (hash * 397) ^ Hotel.ID.GetHashCode();

            return hash;
        }
        #endregion
    }
}
