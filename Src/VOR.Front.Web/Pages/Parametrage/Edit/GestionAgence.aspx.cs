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

namespace VOR.Front.Web.Pages.Parametrage.Edit
{
    public partial class GestionAgence : BasePage
    {
        #region Properties

        public int? AgenceId
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
                BindDdlTypesAgence();
                InitAgence();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                VOR.Core.Domain.Agence agence = Global.Container.Resolve<AgenceModel>().GetByID(this.AgenceId.Value);
                bool isSupprimable = Global.Container.Resolve<AgenceModel>().IsAgenceSupprimable(this.AgenceId.Value);
                if (isSupprimable)
                {
                    Global.Container.Resolve<AgenceModel>().Delete(agence);
                    CloseAndRefresh("SUPPRESSION EFFECTUEE AVEC SUCCES.");
                }
                else
                {
                    ShowMessageBow("Suppression impossible. Agence Liée.", "delete");
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

            VOR.Core.Domain.Agence agence = null;
            if (this.AgenceId.HasValue)
                agence = Global.Container.Resolve<AgenceModel>().GetByID(this.AgenceId.Value);

            if (agence == null)
                agence = new Core.Domain.Agence();

            agence.Nom = this._txtNom.Text;
            agence.TypeAgence = Global.Container.Resolve<TypeAgenceModel>().LoadByID(int.Parse(this._ddlTypeAgence.SelectedValue));
            agence.Description = this._txtDescription.Text;
            agence.Adresse = this._txtAdresse.Text;
            agence.Telef = this._txtTelephone.Text;

            try
            {
                Global.Container.Resolve<AgenceModel>().InsertOrUpdate(agence);
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

        private void BindDdlTypesAgence()
        {
            this._ddlTypeAgence.DataSource = Global.Container.Resolve<TypeAgenceModel>().GetAll();
            this._ddlTypeAgence.DataTextField = "Description";
            this._ddlTypeAgence.DataValueField = "ID";
            this._ddlTypeAgence.DataBind();
        }

        private void InitAgence()
        {
            VOR.Core.Domain.Agence agence = null;

            if (this.AgenceId.HasValue)
                agence = Global.Container.Resolve<AgenceModel>().GetByID(this.AgenceId.Value);

            if (agence != null)
            {
                this._txtNom.Text = agence.Nom;
                this._ddlTypeAgence.SelectedValue = agence.TypeAgence.ID.ToString();
                this._txtDescription.Text = agence.Description;
                this._txtAdresse.Text = agence.Adresse;
                this._txtTelephone.Text = agence.Telef;

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