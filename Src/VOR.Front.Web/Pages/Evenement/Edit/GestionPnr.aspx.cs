using VOR.Core.Model;
using VOR.Front.Web.Base.BasePage;
using VOR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VOR.Core.Enum;
using VOR.Core;
using VOR.Core.Domain;

namespace VOR.Front.Web.Pages.Evenement.Edit
{
    public partial class GestionPnr : BasePage
    {
        #region Properties

        public int? PnrId
        {
            get
            {
                int id;

                if (ViewState["Id"] != null && int.TryParse(ViewState["Id"].ToString(), out id))
                    return id;
                else if (int.TryParse(Request.QueryString["Id"], out id))
                    return id;
                else
                    return null;
            }
            set
            {
                ViewState["Id"] = value.ToString();
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControls();
                InitPnr();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                VOR.Core.Domain.Pnr pnr = Global.Container.Resolve<PnrModel>().GetByID(this.PnrId.Value);
                Global.Container.Resolve<PnrModel>().Delete(pnr);

                CloseAndRefresh("SUPPRESSION EFFECTUEE AVEC SUCCES.");
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                ShowMessageBow("Un problème a été rencontré lors de la suppression", "delete");
            }
        }

        protected void _btnValider_Click(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            bool isValid = ValidatePage(out errorMessage);

            if (!isValid)
            {
                ShowMessageBow(errorMessage, "warning");
                return;
            }

            VOR.Core.Domain.Pnr pnr = null;
            if (this.PnrId.HasValue)
                pnr = Global.Container.Resolve<PnrModel>().GetByID(this.PnrId.Value);

            if (pnr == null)
                pnr = new VOR.Core.Domain.Pnr();

            pnr.Nom = this._txtNom.Text;
            pnr.PrixTotalPnr = this._txtPrixTotal.Value.HasValue ? (int) this._txtPrixTotal.Value : 0;
            pnr.NbrPassager = this._txtNbrPassagers.Value.HasValue ? (int) this._txtNbrPassagers.Value : 0;
            pnr.CautionDepose = this._txtCaution.Value.HasValue ? (int) this._txtCaution.Value : 0;
            pnr.Vol = Global.Container.Resolve<VolModel>().LoadByID(int.Parse(this._ddlVol.SelectedValue));
            pnr.LieuDepart = Global.Container.Resolve<VilleModel>().LoadByID(int.Parse(this._ddlLieuDepart.SelectedValue));
            pnr.LieuArrivee = Global.Container.Resolve<VilleModel>().LoadByID(int.Parse(this._ddlLieuArrivee.SelectedValue));
            pnr.HeureDepart = this._txtHeureDepart.SelectedDate;
            pnr.HeureArrivee = this._txtHeureArrivee.SelectedDate;

            try
            {
                Global.Container.Resolve<PnrModel>().InsertOrUpdate(pnr);
                string message = "ENREGISTEMENT EFFECTUE AVEC SUCCES";
                CloseAndRefresh(message);
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                ShowMessageBow("Un problème a été rencontré lors de l'enregistrement", "delete");
            }
        }

        protected void _txtCaution_TextChanged(object sender, EventArgs e)
        {
            this._txtReste.Text = this._txtPrixTotal.Value.HasValue && this._txtCaution.Value.HasValue ? ((int) this._txtPrixTotal.Value - (int) this._txtCaution.Value).ToString() : "0";
        }

        #endregion

        #region Private

        private void InitControls()
        {
            BindDdlVol();
            BindLieuDepart();
            BindLieuArrivee();
        }

        private void BindDdlVol()
        {
            IList<Vol> vols = Global.Container.Resolve<VolModel>().GetAll();
            foreach (Vol vol in vols)
            {
                string text = string.Format("{0} ( {1} )", vol.Description, vol.CompAerienne.Nom);
                ListItem item = new ListItem(text, vol.ID.ToString());
                this._ddlVol.Items.Add(item);
            }
        }

        private void BindLieuDepart()
        {
            IList<Ville> villes = Global.Container.Resolve<VilleModel>().GetAll();
            foreach (Ville ville in villes)
            {
                string text = string.Format("{0} ( {1} )", ville.Nom, ville.Code);
                ListItem item = new ListItem(text, ville.ID.ToString());
                this._ddlLieuDepart.Items.Add(item);
            }
        }

        private void BindLieuArrivee()
        {
            IList<Ville> villes = Global.Container.Resolve<VilleModel>().GetAll();
            foreach (Ville ville in villes)
            {
                string text = string.Format("{0} ( {1} )", ville.Nom, ville.Code);
                ListItem item = new ListItem(text, ville.ID.ToString());
                this._ddlLieuArrivee.Items.Add(item);
            }
        }

        private void InitPnr()
        {
            VOR.Core.Domain.Pnr pnr = null;

            if (this.PnrId.HasValue)
                pnr = Global.Container.Resolve<PnrModel>().GetByID(this.PnrId.Value);

            if (pnr != null)
            {
                this._txtNom.Text = pnr.Nom;
                this._txtPrixTotal.Text = pnr.PrixTotalPnr.ToString();
                this._txtNbrPassagers.Text = pnr.NbrPassager.ToString();
                this._txtCaution.Text = pnr.CautionDepose.ToString();
                this._txtReste.Text = (pnr.PrixTotalPnr - pnr.CautionDepose).ToString();
                this._ddlVol.SelectedValue = pnr.Vol.ID.ToString();
                this._ddlLieuDepart.SelectedValue = pnr.LieuDepart.ID.ToString();
                this._ddlLieuArrivee.SelectedValue = pnr.LieuArrivee.ID.ToString();
                this._txtHeureDepart.SelectedDate = pnr.HeureDepart;
                this._txtHeureArrivee.SelectedDate = pnr.HeureArrivee;
                this._btnSupprimer.Visible = true;
            }
        }

        private void CloseAndRefresh(string msg)
        {
            RunScript(string.Format("CloseAndRebind('{0}');", msg.ToJSFormat()));
        }

        private void RunScript(string script)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        private bool ValidatePage(out string errorMessage)
        {
            errorMessage = string.Empty;
            Page.Validate();

            if (!Page.IsValid)
            {
                RunScript("validateFields();");
                errorMessage = "Vous devez remplir tous les champs obligatoires.";
                return false;
            }

            if (this._txtHeureDepart.SelectedDate.HasValue
              && this._txtHeureArrivee.SelectedDate.HasValue
              && DateTime.Compare(this._txtHeureDepart.SelectedDate.Value, this._txtHeureArrivee.SelectedDate.Value) > 0)
            {
                errorMessage = "La date de départ ne peut pas être supérieur à la date d'arrivée";
                return false;
            }


            return true;
        }

        private void ShowMessageBow(string msg, string type)
        {
            radNotif.Text = msg;
            radNotif.ContentIcon = type;
            radNotif.TitleIcon = "none";
            radNotif.Show();
            RunScript("loading(false);");
        }

        #endregion
    }
}