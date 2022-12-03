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
using System.Collections;

namespace VOR.Front.Web.Pages.Evenement.Edit
{
    public partial class GestionChambre : BasePage
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
                InitControls();
                InitChambre();
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

            try
            {
                if (this.ChambreId.HasValue)
                {
                    if (!string.IsNullOrEmpty(_txtNumero.Text))
                    {
                        bool isNumeroChambreExist = Global.Container.Resolve<ChambreModel>().isNumeroChambreExist(_txtNumero.Text, (int) this.ChambreId.Value);
                        if (isNumeroChambreExist)
                        {
                            ShowMessageBow("Chambre ocupée!", "warning");
                            return;
                        }
                    }

                    Chambre chambre = Global.Container.Resolve<ChambreModel>().GetByID(this.ChambreId.Value);
                    chambre.Nom = _txtNom.Text;
                    chambre.Numero = _txtNumero.Text;
                    chambre.PrixChambre = float.Parse(_txtPrixChambre.Text);
                    chambre.NbrNuitees = int.Parse(_txtNbrNuitees.Text);
                    chambre.Evenement = Global.Container.Resolve<EvenementModel>().LoadByID(int.Parse(this._ddlEvenement.SelectedValue));
                    chambre.Programme = Global.Container.Resolve<ProgrammeModel>().LoadByID(int.Parse(this._ddlProgramme.SelectedValue));
                    chambre.Hotel = Global.Container.Resolve<HotelModel>().LoadByID(int.Parse(this._ddlHotel.SelectedValue));
                    //chambre.Couleur = string.Format("#{0}", this.RadColorPicker.SelectedColor.Name);
                    chambre.Agence = Global.Container.Resolve<AgenceModel>().LoadByID(int.Parse(this._ddlAgence.SelectedValue));
                    Global.Container.Resolve<ChambreModel>().InsertOrUpdate(chambre);
                }
                else
                {
                    int count = this._txtNbrChambres.Value.HasValue ? (int) this._txtNbrChambres.Value : 0;

                    for (int i = 0; i < count; i++)
                    {
                        Chambre chambre = new Chambre();
                        chambre.TypeChambre = Global.Container.Resolve<TypeChambreModel>().LoadByID(int.Parse(this._ddlTypeChambre.SelectedValue));
                        chambre.Evenement = Global.Container.Resolve<EvenementModel>().LoadByID(this.EventID);
                        chambre.Programme = Global.Container.Resolve<ProgrammeModel>().LoadByID(int.Parse(this._ddlProgramme.SelectedValue));
                        chambre.Hotel = Global.Container.Resolve<HotelModel>().LoadByID(int.Parse(this._ddlHotel.SelectedValue));
                        //chambre.Couleur = string.Format("#{0}", this.RadColorPicker.SelectedColor.Name);
                        chambre.Agence = Global.Container.Resolve<AgenceModel>().LoadByID(int.Parse(this._ddlAgence.SelectedValue));
                        chambre.Nom = string.Format("CHAMBRE_{0}-{1}", chambre.Programme.TypeProgramme.Code, chambre.TypeChambre.Code);
                        chambre.PrixChambre = float.Parse(_txtPrixChambre.Text);
                        chambre.NbrNuitees = int.Parse(_txtNbrNuitees.Text);
                        Global.Container.Resolve<ChambreModel>().Insert(chambre);
                    }
                }
                string message = "ENREGISTEMENT EFFECTUE AVEC SUCCES";
                CloseAndRefresh(message);
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                ShowMessageBow("Un problème a été rencontré lors de l'enregistrement", "delete");
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                Chambre chambre = Global.Container.Resolve<ChambreModel>().GetByID(this.ChambreId.Value);

                bool isSupprimable = Global.Container.Resolve<ChambreModel>().IsChambreMakkahSupprimable(this.ChambreId.Value) 
                                    && Global.Container.Resolve<ChambreModel>().IsChambreMedineSupprimable(this.ChambreId.Value);

                if (isSupprimable)
                {
                    Global.Container.Resolve<ChambreModel>().Delete(chambre);
                    CloseAndRefresh("SUPPRESSION EFFECTUEE AVEC SUCCES.");
                }
                else
                {
                    ShowMessageBow("Suppression impossible. Chambre ocuppée par un pelerin.", "warning");
                    return;
                }

            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                ShowMessageBow("Un problème a été rencontré lors de la suppression", "delete");
            }
        }

        protected void _ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._ddlProgramme.SelectedValue))
            {
                int programmeId = int.Parse(this._ddlProgramme.SelectedValue);
                BindDdlHotel(programmeId);
            }
            else
            {
                BindDdlHotel(null);
            }
        }


        protected void _ddlEvenement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(this._ddlEvenement.SelectedValue))
            {
                int evenementId = int.Parse(this._ddlEvenement.SelectedValue);
                BindDdlProgramme(evenementId);
                BindDdlHotel(null);
            }
        }

        #endregion

        #region Private

        private void InitControls()
        {
            if (!this.ChambreId.HasValue)
            {
                BindDdlProgramme(null);
                BindDdlTypeChambre();
            }
            else {
                Chambre chambre = Global.Container.Resolve<ChambreModel>().LoadByID(this.ChambreId.Value);
                BinDdlEvenement(chambre.Evenement.ID);
                BindDdlProgramme(int.Parse(this._ddlEvenement.SelectedValue));
            }
            BindDdlAgences();
        }

        private void BindDdlAgences()
        {
            this._ddlAgence.DataSource = Global.Container.Resolve<AgenceModel>().GetAll();
            this._ddlAgence.DataTextField = "Nom";
            this._ddlAgence.DataValueField = "ID";
            this._ddlAgence.DataBind();
        }

        private void BindDdlProgramme(int? eventID)
        {
            int evId = eventID.HasValue ? (int)eventID : this.EventID;

            this._ddlProgramme.Items.Clear();
            this._ddlProgramme.Items.Add(new ListItem("---Sélectionnez---", ""));
            this._ddlProgramme.DataSource = Global.Container.Resolve<ProgrammeModel>().GetProgrammeByEventID(evId);
            this._ddlProgramme.DataTextField = "Nom";
            this._ddlProgramme.DataValueField = "ID";
            this._ddlProgramme.DataBind();
        }

        private void BindDdlHotel(int? programmeID)
        {
            if (!programmeID.HasValue) { 
                this._ddlHotel.Items.Clear();
                this._ddlHotel.Items.Add(new ListItem("---Sélectionnez---", ""));
            }
            else
            {
                this._ddlHotel.Items.Clear();
                this._ddlHotel.Items.Add(new ListItem("---Sélectionnez---", ""));
                this._ddlHotel.DataSource = Global.Container.Resolve<ProgrammeModel>().LoadByID(programmeID.Value).Hotels.Select(n => n.Hotel).ToList();
                this._ddlHotel.DataTextField = "Nom";
                this._ddlHotel.DataValueField = "ID";
                this._ddlHotel.DataBind();
            }
        }

        private void BindDdlTypeChambre()
        {
            this._ddlTypeChambre.Items.Add(new ListItem("---Sélectionnez---", ""));
            this._ddlTypeChambre.DataSource = Global.Container.Resolve<TypeChambreModel>().GetAll();
            this._ddlTypeChambre.DataTextField = "Nom";
            this._ddlTypeChambre.DataValueField = "ID";
            this._ddlTypeChambre.DataBind();
        }

        private void BinDdlEvenement(int eventID)
        {
            IList<VOR.Core.Domain.Evenement> evenements = Global.Container.Resolve<EvenementModel>().GetEvenementsEnCours();
            this._ddlEvenement.DataSource = evenements;
            this._ddlEvenement.DataTextField = "Nom";
            this._ddlEvenement.DataValueField = "ID";
            this._ddlEvenement.DataBind();

            this._ddlEvenement.SelectedValue = eventID.ToString();
        }

        private void InitChambre()
        {
            Chambre chambre = null;

            if (this.ChambreId.HasValue)
                chambre = Global.Container.Resolve<ChambreModel>().GetByID(this.ChambreId.Value);

            if (chambre != null)
            {
                ResizeWindow(543, 732);

                this.divNom.Visible = true;

                if((chambre.Hotel.Ville.ID == (int) EnumVille.MAKKAH && chambre.PelerinsMakkah.Count == 0)
                    || (chambre.Hotel.Ville.ID == (int)EnumVille.MEDINE && chambre.PelerinsMedine.Count == 0)) { 
                    this.divEvenement.Visible = true;
                    this.divDetailGeneration.Visible = true;
                }
                else { 
                    this.divEvenement.Visible = false;
                    this.divDetailGeneration.Visible = false;
                }

                //this.RadColorPicker.SelectedColor = ColorTranslator.FromHtml(chambre.Couleur);
                this.typeNbrChambre.Visible = false;
                this._txtNom.Text = chambre.Nom;
                this._txtNumero.Text = chambre.Numero;
                this._txtNbrNuitees.Text = chambre.NbrNuitees.ToString();
                this._txtPrixChambre.Text = chambre.PrixChambre.ToString();
                this._ddlEvenement.SelectedValue = chambre.Evenement.ID.ToString();
                this._ddlProgramme.SelectedValue = chambre.Programme.ID.ToString();

                BindDdlHotel(chambre.Programme.ID);
                this._ddlHotel.SelectedValue = chambre.Hotel.ID.ToString();
                this._ddlAgence.SelectedValue = chambre.Agence.ID.ToString();
                this._btnSupprimer.Visible = true;
            }
        }

        private void ResizeWindow(int? width, int? height)
        {
            string w = width.HasValue ? width.ToString() : "null";
            string h = height.HasValue ? height.ToString() : "null";

            RunScript("function f(){ setWindowSize(" + w + ", " + h + "); Sys.Application.remove_load(f); }; Sys.Application.add_load(f);");
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