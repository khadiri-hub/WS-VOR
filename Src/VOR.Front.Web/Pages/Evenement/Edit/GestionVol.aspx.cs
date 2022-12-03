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
    public partial class GestionVol : BasePage
    {
        #region Properties

        public int? VolId
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
                InitVol();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                Vol vol = Global.Container.Resolve<VolModel>().GetByID(this.VolId.Value);

                bool isSupprimable = Global.Container.Resolve<VolModel>().IsVolSupprimable(this.VolId.Value);

                if (isSupprimable)
                {
                    Global.Container.Resolve<VolModel>().Delete(vol);
                    CloseAndRefresh("SUPPRESSION EFFECTUEE AVEC SUCCES.");
                }
                else
                {
                    ShowMessageBow("Suppression impossible. Vol Lié.", "delete");
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

            Vol vol = null;
            if (this.VolId.HasValue)
                vol = Global.Container.Resolve<VolModel>().GetByID(this.VolId.Value);

            if (vol == null)
                vol = new Vol();

            vol.Description = this._txtDescription.Text;
            vol.CompAerienne = Global.Container.Resolve<CompAerienneModel>().LoadByID(int.Parse(this._ddlCA.SelectedValue));

            try
            {
                Global.Container.Resolve<VolModel>().InsertOrUpdate(vol);
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
            BindDdlCA();
        }

        private void BindDdlCA()
        {
            this._ddlCA.DataSource = Global.Container.Resolve<CompAerienneModel>().GetAll();
            this._ddlCA.DataTextField = "Nom";
            this._ddlCA.DataValueField = "ID";
            this._ddlCA.DataBind();
        }

        private void InitVol()
        {
            Vol vol = null;

            if (this.VolId.HasValue)
                vol = Global.Container.Resolve<VolModel>().GetByID(this.VolId.Value);

            if (vol != null)
            {
                this._txtDescription.Text = vol.Description;
                this._ddlCA.SelectedValue = vol.CompAerienne.ID.ToString();
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