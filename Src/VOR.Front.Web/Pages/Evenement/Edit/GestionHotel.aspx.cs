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
    public partial class GestionHotel : BasePage
    {
        #region Properties

        public int? HotelId
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
                InitHotel();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                Hotel hotel = Global.Container.Resolve<HotelModel>().GetByID(this.HotelId.Value);

                bool isSupprimable = Global.Container.Resolve<HotelModel>().IsHotelSupprimable(this.HotelId.Value);

                if (isSupprimable)
                {
                    Global.Container.Resolve<HotelModel>().Delete(hotel);
                    CloseAndRefresh("SUPPRESSION EFFECTUEE AVEC SUCCES.");
                }
                else
                {
                    ShowMessageBow("Suppression impossible. Hotel Lié.", "delete");
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

            Hotel hotel = null;
            if (this.HotelId.HasValue)
                hotel = Global.Container.Resolve<HotelModel>().GetByID(this.HotelId.Value);

            if (hotel == null)
                hotel = new Hotel();

            hotel.Nom = this._txtNomAR.Text;
            hotel.NomFr = this._txtNomFR.Text;
            hotel.DistanceToHaram = this._txtDistance.Value.HasValue ? (int) this._txtDistance.Value : 0;
            hotel.Categorie = this._txtCategorie.Value.HasValue ? (int) this._txtCategorie.Value : 0;
            hotel.Ville = Global.Container.Resolve<VilleModel>().LoadByID(int.Parse(this._ddlVille.SelectedValue));

            try
            {
                Global.Container.Resolve<HotelModel>().InsertOrUpdate(hotel);
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
            BindDdlVille();
        }

        private void BindDdlVille()
        {
            this._ddlVille.DataSource = Global.Container.Resolve<VilleModel>().GetAll();
            this._ddlVille.DataTextField = "Nom";
            this._ddlVille.DataValueField = "ID";
            this._ddlVille.DataBind();
        }

        private void InitHotel()
        {
            Hotel hotel = null;

            if (this.HotelId.HasValue)
                hotel = Global.Container.Resolve<HotelModel>().GetByID(this.HotelId.Value);

            if (hotel != null)
            {
                this._txtNomAR.Text = hotel.Nom;
                this._txtNomFR.Text = hotel.NomFr;
                this._txtCategorie.Text = hotel.Categorie.ToString();
                this._txtDistance.Text = hotel.DistanceToHaram.ToString();
                this._ddlVille.SelectedValue = hotel.Ville.ID.ToString();
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