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
    public partial class GestionProgramme : BasePage
    {
        #region Properties

        public int? ProgrammeId
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

        private Programme _currentProgramme = null;

        public Programme CurrentProgramme
        {
            get
            {
                if (_currentProgramme == null && this.ProgrammeId.HasValue)
                    _currentProgramme = Global.Container.Resolve<ProgrammeModel>().GetByID(this.ProgrammeId.Value);

                return _currentProgramme;
            }
            set
            {
                _currentProgramme = value;
            }

        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControls();
                InitProgramme();

                if(this.ProgrammeId != null)
                    this._hotels.ProgrammeId = this.CurrentProgramme.ID;

                this._hotels.GetHotels();
                this._hotels.Refresh();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                Programme programme = Global.Container.Resolve<ProgrammeModel>().GetByID(this.ProgrammeId.Value);
                bool isSupprimable = Global.Container.Resolve<ProgrammeModel>().IsProgrammeSupprimable(this.ProgrammeId.Value);

                if (isSupprimable)
                {
                    Global.Container.Resolve<ProgrammeModel>().Delete(programme);
                    CloseAndRefresh("SUPPRESSION EFFECTUEE AVEC SUCCES.");
                }
                else
                {
                    ShowMessageBow("Suppression impossible. Programme Lié.", "delete");
                    return;
                }

                Global.Container.Resolve<ProgrammeModel>().Delete(programme);
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

            if (!this.ProgrammeId.HasValue)
            {
                this.CurrentProgramme = new Programme();
                this.CurrentProgramme.Hotels = new List<TProgrammeHotel>();
            }

            this.CurrentProgramme.Nom = this._txtNom.Text;
            this.CurrentProgramme.PrixAPartirDe = this._txtPrix.Value.HasValue ? (int) this._txtPrix.Value : 0;

            int idEvent = 0;
            if (int.TryParse(this._ddlEvenement.SelectedValue, out idEvent))
            {
                this.CurrentProgramme.Evenement = Global.Container.Resolve<EvenementModel>().LoadByID(idEvent);
            }
            else
                this.CurrentProgramme.Evenement = null;

            int idTypeProgramme = 0;
            if (int.TryParse(this._ddlTypeProgramme.SelectedValue, out idTypeProgramme))
            {
                this.CurrentProgramme.TypeProgramme = Global.Container.Resolve<TypeProgrammeModel>().LoadByID(idTypeProgramme);
            }
            else
                this.CurrentProgramme.TypeProgramme = null;

            int idVol = 0;
            if (int.TryParse(this._ddlVol.SelectedValue, out idVol))
            {
                this.CurrentProgramme.Vol = Global.Container.Resolve<VolModel>().LoadByID(idVol);
            }
            else
                this.CurrentProgramme.Vol = null;

            #region Hotels

            foreach (TProgrammeHotel progHotel in this.CurrentProgramme.Hotels.ToArray())
            {
                if (!this._hotels.ListHotels.Where(q => q.Key == progHotel.Hotel.ID).Any())
                    this.CurrentProgramme.Hotels.Remove(progHotel);
            }

            foreach (var item in this._hotels.ListHotels)
            {
                Hotel hotel = Global.Container.Resolve<HotelModel>().GetByID(item.Key);

                if (!this.CurrentProgramme.Hotels.Where(q => q.Hotel.ID == item.Key).Any())
                {
                    this.CurrentProgramme.Hotels.Add(new TProgrammeHotel
                    {
                        Hotel = hotel,
                        Programme = this.CurrentProgramme
                    });
                }
            }

            #endregion

            try
            {
                Global.Container.Resolve<ProgrammeModel>().InsertOrUpdate(this.CurrentProgramme);
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
            BindDdlEvenement();
            BindDdlTypeProgramme();
            BindDdlVol();
        }

        private void BindDdlEvenement()
        {
            this._ddlEvenement.DataSource = Global.Container.Resolve<EvenementModel>().GetAll();
            this._ddlEvenement.DataTextField = "Nom";
            this._ddlEvenement.DataValueField = "ID";
            this._ddlEvenement.DataBind();
        }

        private void BindDdlTypeProgramme()
        {
            this._ddlTypeProgramme.DataSource = Global.Container.Resolve<TypeProgrammeModel>().GetAll();
            this._ddlTypeProgramme.DataTextField = "Description";
            this._ddlTypeProgramme.DataValueField = "ID";
            this._ddlTypeProgramme.DataBind();
        }

        private void BindDdlVol()
        {
            IList<Vol> vols = Global.Container.Resolve<VolModel>().GetAll();
            foreach(Vol vol in vols)
            {
                string text = string.Format("{0} ( {1} )", vol.Description, vol.CompAerienne.Nom);
                ListItem item = new ListItem(text, vol.ID.ToString());
                this._ddlVol.Items.Add(item);
            }
        }

        private void InitProgramme()
        {
            Programme programme = null;

            if (this.ProgrammeId.HasValue)
                programme = Global.Container.Resolve<ProgrammeModel>().GetByID(this.ProgrammeId.Value);

            if (programme != null)
            {
                this._txtNom.Text = programme.Nom;
                this._txtPrix.Text = programme.PrixAPartirDe.ToString();
                this._ddlEvenement.SelectedValue = programme.Evenement == null ? "" : programme.Evenement.ID.ToString();
                this._ddlTypeProgramme.SelectedValue = programme.TypeProgramme == null ? "" : programme.TypeProgramme.ID.ToString();
                this._ddlVol.SelectedValue = programme.Vol == null ? "" : programme.Vol.ID.ToString();
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