using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class ParkingQuotaReferent : BaseObject<int>
    {
        public virtual ParkingQuota Quota { get; set; }
        public virtual PersonRef Person { get; set; }

        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as ParkingQuotaReferent;
            if (t == null) return false;
            if (Quota.ID == t.Quota.ID
             && Person.ID == t.Person.ID)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ Quota.ID.GetHashCode();
            hash = (hash * 397) ^ Person.ID.GetHashCode();

            return hash;
        }
        #endregion
    }
}