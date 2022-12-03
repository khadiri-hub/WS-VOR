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
using System.Drawing;

namespace VOR.Front.Web.Pages.Evenement.Edit
{
    public partial class GestionEvenement : BasePage
    {
        #region Properties

        public int? EvenementId
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
                InitEvenement();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                VOR.Core.Domain.Evenement evenement = Global.Container.Resolve<EvenementModel>().GetByID(this.EvenementId.Value);
                bool isSupprimable = Global.Container.Resolve<EvenementModel>().IsEvenementSupprimable(this.EvenementId.Value);

                if (isSupprimable)
                {
                    Global.Container.Resolve<EvenementModel>().Delete(evenement);
                    CloseAndRefresh("SUPPRESSION EFFECTUEE AVEC SUCCES.");
                }
                else
                {
                    ShowMessageBow("Suppression impossible. Evenement Lié.", "delete");
                    return;
                }
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

            VOR.Core.Domain.Evenement evenement = null;
            if (this.EvenementId.HasValue)
                evenement = Global.Container.Resolve<EvenementModel>().GetByID(this.EvenementId.Value);

            if (evenement == null)
                evenement = new VOR.Core.Domain.Evenement();

            evenement.Nom = this._txtNom.Text;
            evenement.DateDebut = this._radDateDebut.SelectedDate.Value;
            evenement.DateFin = this._radDateFin.SelectedDate.Value;
            evenement.Pnr = Global.Container.Resolve<PnrModel>().LoadByID(int.Parse(this._ddlPnr.SelectedValue));
            evenement.Duree = this._txtNbrJour.Value.HasValue ? (int) this._txtNbrJour.Value : 0;
            evenement.EnCours = _cbEnCours.Checked;
            evenement.Couleur = string.Format("#{0}", this.RadColorPicker.SelectedColor.Name);

            try
            {
                Global.Container.Resolve<EvenementModel>().InsertOrUpdate(evenement);
                string message = "ENREGISTEMENT EFFECTUE AVEC SUCCES";
                CloseAndRefresh(message);
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                ShowMessageBow("Un problème a été rencontré lors de l'enregistrement", "delete");
            }
        }

        #endregion

        #region Private

        private void InitControls()
        {
            BindDdlPnr();
        }

        private void BindDdlPnr()
        {
            this._ddlPnr.DataSource = Global.Container.Resolve<PnrModel>().GetAll();
            this._ddlPnr.DataTextField = "Nom";
            this._ddlPnr.DataValueField = "ID";
            this._ddlPnr.DataBind();
        }

        private void InitEvenement()
        {
            VOR.Core.Domain.Evenement evenement = null;

            if (this.EvenementId.HasValue)
                evenement = Global.Container.Resolve<EvenementModel>().GetByID(this.EvenementId.Value);

            if (evenement != null)
            {
                this._txtNom.Text = evenement.Nom;
                this._radDateDebut.SelectedDate = evenement.DateDebut;
                this._radDateFin.SelectedDate = evenement.DateFin;
                this._txtNbrJour.Text = evenement.Duree.HasValue ? evenement.Duree.Value.ToString() : "0";
                this._ddlPnr.SelectedValue = evenement.Pnr.ID.ToString();
                this._cbEnCours.Checked = evenement.EnCours;
                this.RadColorPicker.SelectedColor = ColorTranslator.FromHtml(evenement.Couleur);
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

            DateTime dateDebut = this._radDateDebut.SelectedDate.Value;
            DateTime dateFin = this._radDateFin.SelectedDate.Value;

            if (dateDebut > dateFin)
            {
                errorMessage = "La date Début doit être inférieur à la date Fin.";
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