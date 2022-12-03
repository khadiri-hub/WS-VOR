using VOR.Front.Web.Base.BasePage;
using VOR.Utils;
using System;
using System.Web.UI;
using VOR.Core;
using VOR.Core.Domain;
using VOR.Core.Model;

namespace VOR.Front.Web.Pages.Evenement.Edit
{
    public partial class GestionCompagnie : BasePage
    {
        #region Properties

        public int? CompagnieId
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
                InitCompagnie();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                CompAerienne compagnie = Global.Container.Resolve<CompAerienneModel>().GetByID(this.CompagnieId.Value);

                bool isSupprimable = Global.Container.Resolve<CompAerienneModel>().IsCompagnieSupprimable(this.CompagnieId.Value);

                if (isSupprimable)
                {
                    Global.Container.Resolve<CompAerienneModel>().Delete(compagnie);
                    CloseAndRefresh("SUPPRESSION EFFECTUEE AVEC SUCCES.");
                }
                else
                {
                    ShowMessageBow("Suppression impossible. Compagnie Liée.", "delete");
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

            CompAerienne compagnie = null;
            if (this.CompagnieId.HasValue)
                compagnie = Global.Container.Resolve<CompAerienneModel>().GetByID(this.CompagnieId.Value);

            if (compagnie == null)
                compagnie = new CompAerienne();

            compagnie.Nom = this._txtNom.Text;

            try
            {
                Global.Container.Resolve<CompAerienneModel>().InsertOrUpdate(compagnie);
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
            
        }

        private void InitCompagnie()
        {
            CompAerienne compagnie = null;

            if (this.CompagnieId.HasValue)
                compagnie = Global.Container.Resolve<CompAerienneModel>().GetByID(this.CompagnieId.Value);

            if (compagnie != null)
            {
                this._txtNom.Text = compagnie.Nom;
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