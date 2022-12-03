using System;

namespace VOR.Core.Domain
{
    public class Calendrier : BaseObject<int>
    {
        #region Properties

        public virtual DateTime DateJour { get; set; }

        public virtual int EvenementID { get; set; }

        public virtual string Obs { get; set; }

        [Obsolete("Ne pas utiliser cette propriété pour qualifier un jour tournoi. Donnée fausse !")]
        public virtual bool EstTournoi { get; set; }

        public virtual bool EstQualif { get; set; }

        public virtual bool EstAcces { get; set; }

        public virtual bool EstRestau { get; set; }

        public virtual bool EstTransport { get; set; }

        public virtual bool EstInvitation { get; set; }

        public virtual byte[] Image { get; set; }

        public virtual int? NumJournee { get; set; }

        #endregion Properties

        public Calendrier()
        {
        }

        public Calendrier(int id)
        {
            this.ID = id;
        }
    }
}