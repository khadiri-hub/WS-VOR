using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VOR.Core.Domain
{
    public class Chambre : BaseObject<int>
    {
        public virtual string Nom { get; set; }
        public virtual string Couleur { get; set; }
        public virtual TypeChambre TypeChambre { get; set; }
        public virtual Evenement Evenement { get; set; }
        public virtual Programme Programme { get; set; }
        public virtual Hotel Hotel { get; set; }
        public virtual IList<Pelerin> PelerinsMakkah { get; set; }
        public virtual IList<Pelerin> PelerinsMedine { get; set; }
        public virtual string Numero { get; set; }

        public virtual float PrixChambre { get; set; }

        public virtual int NbrNuitees { get; set; }
        public virtual bool Occupe { get; set; }
        public virtual Agence Agence { get; set; }

        public virtual string OccupantMakkah
        {
            get {
                return this.PelerinsMakkah != null ? string.Format("{0}/{1}", PelerinsMakkah.Count, TypeChambre.Code) : "";
            }
        }

        public virtual string OccupantMedine
        {
            get
            {
                return this.PelerinsMedine != null ? string.Format("{0}/{1}", PelerinsMedine.Count, TypeChambre.Code) : "";
            }
        }

        public virtual bool isChambreMakkahOcuppe
        {
            get
            {
                return this.PelerinsMakkah.Count == TypeChambre.Code;
            }
        }

        public virtual bool isChambreMedineOcuppe
        {
            get
            {
                return this.PelerinsMedine.Count == TypeChambre.Code;
            }
        }
    }
}
