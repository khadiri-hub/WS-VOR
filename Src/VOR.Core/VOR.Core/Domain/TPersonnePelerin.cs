using System;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class TPersonnePelerin
    {
        public virtual Personne Personne { get; set; }
        public virtual Pelerin Pelerin { get; set; }

        #region NHibernate Composite Key Requirements

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as TPersonnePelerin;
            if (t == null) return false;
            if (Personne.ID == t.Personne.ID
             && Pelerin.ID == t.Pelerin.ID)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ Personne.ID.GetHashCode();
            hash = (hash * 397) ^ Pelerin.ID.GetHashCode();

            return hash;
        }

        #endregion
    }
}
