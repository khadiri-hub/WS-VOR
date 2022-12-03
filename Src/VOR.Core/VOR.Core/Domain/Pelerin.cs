using System;
using System.Collections.Generic;
using VOR.Core.Domain;

namespace VOR.Core.Domain
{
    public class Pelerin : BaseObject<int>
    {   
        public virtual EtatCivil EtatCivil { get; set; }
        public virtual Sexe Sexe { get; set; }
        public virtual Evenement Evenement { get; set; }
        public virtual int EvenementID { get; set; }
        public virtual Programme Programme { get; set; }
        public virtual TypeChambre TypeChambre { get; set; }
        public virtual Chambre ChambreMakkah { get; set; }
        public virtual Chambre ChambreMedine { get; set; }
        public virtual Agence Agence { get; set; }
        public virtual IList<TPelerinHotel> Hotels { get; set; }
        public virtual IList<TPersonnePelerin> Personnes { get; set; }
        public virtual IList<TPelerinPelerin> Pelerins { get; set; }
        public virtual IList<Vaccin> Vaccins { get; set; }
        public virtual string NomFrancais { get; set; }
        public virtual string PrenomFrancais { get; set; }
        public virtual string PrenomNomFrancais
        {
            get
            {
                return string.Format("{0}   {1}", this.NomFrancais, this.PrenomFrancais);
            }
        }
        public virtual string NomArabe { get; set; }
        public virtual string PrenomArabe { get; set; }
        public virtual DateTime DateNaissance { get; set; }
        public virtual string NumPassport { get; set; }
        public virtual DateTime DateExpiration { get; set; }
        public virtual string Telephone { get; set; }
        public virtual string Telephone1 { get; set; }
        public virtual string Telephone2 { get; set; }
        public virtual int PrixVentePack { get; set; }
        public virtual int MontantPaye { get; set; }
        public virtual int RestApayer
        {
            get
            {
                return this.PrixVentePack - this.MontantPaye;
            }
        }
        public virtual int? IdPersonne { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual int? EvaluationVoyage { get; set; }
        public virtual int? EvaluationPelerin { get; set; }
        public virtual bool BadgeToDownload { get; set; }
        public virtual string Commentaire { get; set; }
        public virtual DateTime? DateCreation { get; set; }
        public virtual DateTime? DateUpdate { get; set; }
        public virtual RefVille RefVille { get; set; }      
        public virtual Alerte Alerte { get; set; }
        public virtual Visa Visa { get; set; }
        public virtual bool TransportPaye { get; set; }
        public virtual bool RepasPaye { get; set; }
        public virtual bool AssuranceMaladie { get; set; }
        public virtual bool Stop { get; set; }

        public virtual int NbrVaccin { get; set; }
        public virtual string AlerteDescription { get; set; }

        public virtual int TypePelerinID { get; set; }


    }
}