using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class TPelerinPelerin
    {
        public virtual Pelerin Pelerin1 { get; set; }
        public virtual Pelerin Pelerin2 { get; set; }
        public virtual string Couleur { get; set; }

        #region NHibernate Composite Key Requirements

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as TPelerinPelerin;
            if (t == null) return false;
            if (Pelerin1.ID == t.Pelerin1.ID
             && Pelerin2.ID == t.Pelerin2.ID)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ Pelerin1.ID.GetHashCode();
            hash = (hash * 397) ^ Pelerin2.ID.GetHashCode();

            return hash;
        }

        #endregion
    }
}
