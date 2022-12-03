using System;
using System.Collections.Generic;
using System.Web.UI;
using VOR.Core;
using VOR.Core.Domain;
using VOR.Core.Model;
using System.Linq;
using System.Web;
using VOR.Core.Domain.Vues;
using System.Web.UI.WebControls;

namespace VOR.Front.Web.UserControls.Menu
{
    public partial class MenuBienvenuTopBackUC : BaseUserControl
    {

        public string Evenement
        {
            get
            {
                return this._ddlEvenement.SelectedValue;
            }
            set
            {
               this._ddlEvenement.SelectedValue = value;
            }

        }


        private Utilisateur _utilisateur;
        public Utilisateur Utilisateur
        {
            get
            {
                if (this._utilisateur == null && this._basePage != null && this._basePage.UserID != -1)
                {
                    this._utilisateur = Global.Container.Resolve<UtilisateurModel>().GetByID(this._basePage.UserID);
                }
                return this._utilisateur;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (this.Utilisateur != null)
                {
                    this._lblNomPrenom.Text = this.Utilisateur.Prenom + " " + this.Utilisateur.Nom;
                    BindDdlEvenenement();
                }
                else
                    this._menubienvenu.Visible = false;
            }
        }

        private void BindDdlEvenenement()
        {
            IList<Core.Domain.Evenement> lstEvenement = Global.Container.Resolve<EvenementModel>().GetEvenementsEnCours();

            foreach(Core.Domain.Evenement even in lstEvenement) {
                string lblEvent = string.Format("{0} - {1}", even.Nom, GetDisponibilite(even.ID).ToString());
                this._ddlEvenement.Items.Add(new ListItem(lblEvent, even.ID.ToString()));
            }

            int? eventID = ExtractEventIdFromCookie();
            this._ddlEvenement.SelectedValue = eventID == null ? "" : eventID.Value.ToString();
        }

        private string GetDisponibilite(int eventID)
        {
            int agenceID = ExtractAgenceIdFromCookie().Value;

            VOR.Core.Domain.Evenement EvenementEnCours = Global.Container.Resolve<EvenementModel>().LoadByID(eventID);
            Pnr pnr = Global.Container.Resolve<PnrModel>().LoadByID(EvenementEnCours.Pnr.ID);
            int nbrPelerins = Global.Container.Resolve<PelerinModel>().GetNbrPelerinsByEventID(eventID);

            int nbrPelerinsMax = pnr.NbrPassager;

            return string.Format("{0} | {1} Dispos", nbrPelerinsMax.ToString(), (nbrPelerinsMax - nbrPelerins).ToString());
        }

        private int? ExtractEventIdFromCookie()
        {
            int eventID;

            HttpCookie cookie = Request.Cookies["coockieVOR"];
            if (int.TryParse(this._basePage.CookieHelper.ReadCookieEntry("EventID"), out eventID))
            {
                this._basePage.CookieHelper.WriteCookieEntry("EventID", eventID.ToString());
                return eventID;
            }
            else
                return null;
        }

        private int? ExtractAgenceIdFromCookie()
        {
            int agenceID;

            HttpCookie cookie = Request.Cookies["coockieVOR"];
            if (int.TryParse(this._basePage.CookieHelper.ReadCookieEntry("AgenceID"), out agenceID))
            {
                this._basePage.CookieHelper.WriteCookieEntry("AgenceID", agenceID.ToString());
                return agenceID;
            }
            else
                return null;
        }

        private void SwitchLanguage(object sender, EventArgs e)
        {
        }

        protected void img_btn_logout_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        protected void _ddlEvenement_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._basePage.EventID = int.Parse(this._ddlEvenement.SelectedValue.ToString());
            this._basePage.CookieHelper.WriteCookieEntry("EventID", this._ddlEvenement.SelectedValue.ToString());
        }
    }
}