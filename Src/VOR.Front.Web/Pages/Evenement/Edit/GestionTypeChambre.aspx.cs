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
    public partial class GestionTypeChambre : BasePage
    {
        #region Properties

        public int? ChambreId
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
                BindDdlProgramme();
                InitTypeChambre();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                TypeChambre TypeChambre = Global.Container.Resolve<TypeChambreModel>().GetByID(this.ChambreId.Value);
                bool isSupprimable = Global.Container.Resolve<TypeChambreModel>().IsTypeChambreSupprimable(this.ChambreId.Value);

                if (isSupprimable)
                {
                    Global.Container.Resolve<TypeChambreModel>().Delete(TypeChambre);
                    CloseAndRefresh("SUPPRESSION EFFECTUEE AVEC SUCCES.");
                }
                else
                {
                    ShowMessageBow("Suppression impossible. Type de Chambre Lié à un Pelerin.", "delete");
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

            TypeChambre typeChambre = null;
            if (this.ChambreId.HasValue)
                typeChambre = Global.Container.Resolve<TypeChambreModel>().GetByID(this.ChambreId.Value);

            if (typeChambre == null)
                typeChambre = new TypeChambre();

            typeChambre.Code = (int) this._txtCode.Value;
            typeChambre.Nom = this._txtNom.Text;
            typeChambre.Programme = Global.Container.Resolve<ProgrammeModel>().LoadByID(int.Parse(this._ddlProgramme.SelectedValue));
            typeChambre.PrixRs = this._txtPrix.Value.HasValue ? (int) this._txtPrix.Value : 0;

            try
            {
                Global.Container.Resolve<TypeChambreModel>().InsertOrUpdate(typeChambre);
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

        private void BindDdlProgramme()
        {
            this._ddlProgramme.DataSource = Global.Container.Resolve<ProgrammeModel>().GetAll();
            this._ddlProgramme.DataTextField = "Nom";
            this._ddlProgramme.DataValueField = "ID";
            this._ddlProgramme.DataBind();
        }

        private void InitTypeChambre()
        {
            TypeChambre typeChambre = null;

            if (this.ChambreId.HasValue)
                typeChambre = Global.Container.Resolve<TypeChambreModel>().GetByID(this.ChambreId.Value);

            if (typeChambre != null)
            {
                this._ddlProgramme.SelectedValue = typeChambre.Programme.ID.ToString();
                this._txtCode.Text = typeChambre.Code.ToString();
                this._txtNom.Text = typeChambre.Nom;
                this._txtPrix.Text = typeChambre.PrixRs.ToString();
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