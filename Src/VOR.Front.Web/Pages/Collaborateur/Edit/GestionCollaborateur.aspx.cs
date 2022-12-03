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

namespace VOR.Front.Web.Pages.Collaborateur.Edit
{
    public partial class GestionCollaborateur : BasePage
    {
        #region Properties

        public int? CollaborateurId
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
                InitCollaborateur();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                Personne personne = Global.Container.Resolve<PersonneModel>().GetByID(this.CollaborateurId.Value);

                Global.Container.Resolve<PersonneModel>().Delete(personne);
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

            Personne personne = null;
            if (this.CollaborateurId.HasValue)
                personne = Global.Container.Resolve<PersonneModel>().GetByID(this.CollaborateurId.Value);

            if (personne == null)
                personne = new Personne();

            personne.NomAR = this._txtNomAR.Text;
            personne.PrenomAR = this._txtPrenomAR.Text;
            personne.NomFR = this._txtNomFR.Text;
            personne.PrenomFR = this._txtPrenomFR.Text;
            personne.Telef = this._txtTelephone.Text;
            personne.TypePersonne = Global.Container.Resolve<TypePersonneModel>().LoadByID(int.Parse(this._ddlTypePersonne.SelectedValue));
            personne.Agence = Global.Container.Resolve<AgenceModel>().LoadByID(int.Parse(this._ddlAgence.SelectedValue));

            try
            {
                Global.Container.Resolve<PersonneModel>().InsertOrUpdate(personne);
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
            BindDdlTypesPersonne();
            BindDdlAgences();
        }

        private void BindDdlTypesPersonne()
        {
            this._ddlTypePersonne.DataSource = Global.Container.Resolve<TypePersonneModel>().GetAll();
            this._ddlTypePersonne.DataTextField = "Fonction";
            this._ddlTypePersonne.DataValueField = "ID";
            this._ddlTypePersonne.DataBind();
        }

        private void BindDdlAgences()
        {
            this._ddlAgence.DataSource = Global.Container.Resolve<AgenceModel>().GetAll();
            this._ddlAgence.DataTextField = "Nom";
            this._ddlAgence.DataValueField = "ID";
            this._ddlAgence.DataBind();
        }

        private void InitCollaborateur()
        {
            Personne personne = null;

            if (this.CollaborateurId.HasValue)
                personne = Global.Container.Resolve<PersonneModel>().GetByID(this.CollaborateurId.Value);

            if (personne != null)
            {
                this._txtNomAR.Text = personne.NomAR;
                this._txtPrenomAR.Text = personne.PrenomAR;
                this._txtNomFR.Text = personne.NomFR;
                this._txtPrenomFR.Text = personne.PrenomFR;
                this._txtTelephone.Text = personne.Telef;
                this._ddlTypePersonne.SelectedValue = personne.TypePersonne.ID.ToString();
                this._ddlAgence.SelectedValue = personne.Agence.ID.ToString();
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