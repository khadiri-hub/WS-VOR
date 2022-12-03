using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOR.Core.Domain.Vues
{
    public class VuePelerin
    {
        public virtual int ID { get; set; }
        public virtual int IdTypePelerin { get; set; }
        public virtual string NomFrancais { get; set; }
        public virtual string PrenomFrancais { get; set; }
        public virtual string NomArabe { get; set; }
        public virtual string PrenomArabe { get; set; }
        public virtual string NomVaccin { get; set; }
        public virtual int NbrVaccination { get; set; }
        public virtual DateTime DateNaissance { get; set; }
        public virtual string NumPassport { get; set; }
        public virtual DateTime DateExpiration { get; set; }
        public virtual string Telephone { get; set; }
        public virtual int PrixVentePack { get; set; }
        public virtual int MontantPaye { get; set; }
        public virtual int EvaluationVoyage { get; set; }
        public virtual int EvaluationPelerin { get; set; }
        public virtual string Commentaire { get; set; }
        public virtual string EtatCivil { get; set; }
        public virtual string Sexe { get; set; }
        public virtual string EvenementNom { get; set; }
        public virtual string ProgrammeNom { get; set; }
        public virtual string TypeChambreNom { get; set; }
        public virtual string Couleur { get; set; }
        public virtual DateTime? DateCreation { get; set; }
        public virtual int CodeChambre { get; set; }
        public virtual string CodeTypeProgramme { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual string Alert { get; set; }
        public virtual DateTime? DateExpirationVisa { get; set; }
        public virtual string Statut { get; set; }
        public virtual string MotifStatut { get; set; }
        public virtual int RestApayer
        {
            get
            {
                return this.PrixVentePack - this.MontantPaye;
            }
        }
        public virtual DateTime? HeureDepart { get; set; }
        public virtual DateTime? HeureArrivee { get; set; }
        public virtual string LieuDepart { get; set; }
        public virtual string LieuArrivee { get; set; }
        public virtual DateTime? DateDebutEvenement { get; set; }
        public virtual string TransportPaye { get; set; }
        public virtual string RepasPaye { get; set; }
        public virtual string AssuranceMaladie { get; set; }
        public virtual string Stop { get; set; }
        public virtual string PrenomNomFrancais
        {
            get
            {
                return string.Format("NM1{0}/{1}", this.NomFrancais, this.PrenomFrancais);
            }
        }

        public virtual string Agence { get; set; }
    }
}
