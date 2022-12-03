using VOR.Core.Model;
using VOR.Front.Web.Base.BasePage;
using VOR.Utils;
using System;
using System.Web.UI;
using VOR.Core;
using VOR.Core.Domain;

namespace VOR.Front.Web.Pages.Agence.Edit
{
    public partial class GestionUtilisateur : BasePage
    {
        #region Properties

        public int? UtilisateurId
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
                InitUtilisateur();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                Utilisateur utilisateur = Global.Container.Resolve<UtilisateurModel>().GetByID(this.UtilisateurId.Value);

                Global.Container.Resolve<UtilisateurModel>().Delete(utilisateur);
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

            Utilisateur utilisateur = null;
            if (this.UtilisateurId.HasValue)
                utilisateur = Global.Container.Resolve<UtilisateurModel>().GetByID(this.UtilisateurId.Value);

            if (utilisateur == null)
                utilisateur = new Utilisateur();

            utilisateur.Nom = this._txtNom.Text;
            utilisateur.Prenom = this._txtPrenom.Text;
            utilisateur.Telef = this._txtTelephone.Text;
            utilisateur.Login = this._txtLoginUser.Text;
            utilisateur.Password = this._txtPwdUser.Text;
            utilisateur.TypeUtilisateur = Global.Container.Resolve<TypeUtilisateurModel>().LoadByID(int.Parse(this._ddlTypeUtilisateur.SelectedValue));
            utilisateur.Agence = Global.Container.Resolve<AgenceModel>().LoadByID(int.Parse(this._ddlAgence.SelectedValue));

            try
            {
                Global.Container.Resolve<UtilisateurModel>().InsertOrUpdate(utilisateur);
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
            BindDdlTypesUtilisateur();
            BindDdlAgences();
        }

        private void BindDdlTypesUtilisateur()
        {
            this._ddlTypeUtilisateur.DataSource = Global.Container.Resolve<TypeUtilisateurModel>().GetAll();
            this._ddlTypeUtilisateur.DataTextField = "Fonction";
            this._ddlTypeUtilisateur.DataValueField = "ID";
            this._ddlTypeUtilisateur.DataBind();
        }

        private void BindDdlAgences()
        {
            this._ddlAgence.DataSource = Global.Container.Resolve<AgenceModel>().GetAll();
            this._ddlAgence.DataTextField = "Nom";
            this._ddlAgence.DataValueField = "ID";
            this._ddlAgence.DataBind();
        }

        private void InitUtilisateur()
        {
            VOR.Core.Domain.Utilisateur utilisateur = null;

            if (this.UtilisateurId.HasValue)
                utilisateur = Global.Container.Resolve<UtilisateurModel>().GetByID(this.UtilisateurId.Value);

            if (utilisateur != null)
            {
                this._txtNom.Text = utilisateur.Nom;
                this._txtPrenom.Text = utilisateur.Prenom;
                this._txtLoginUser.Text = utilisateur.Login;
                this._txtPwdUser.Attributes["value"] = utilisateur.Password;
                this._txtConfirmPwd.Attributes["value"] = utilisateur.Password;
                this._txtTelephone.Text = utilisateur.Telef;
                this._ddlTypeUtilisateur.SelectedValue = utilisateur.TypeUtilisateur.ID.ToString();
                this._ddlAgence.SelectedValue = utilisateur.Agence.ID.ToString();
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

            if (!this._txtPwdUser.Text.Equals(this._txtConfirmPwd.Text))
            {
                errorMessage = "Confirmation de password incorrecte.";
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