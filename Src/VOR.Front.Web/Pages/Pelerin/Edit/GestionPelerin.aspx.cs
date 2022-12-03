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
using System.Text;
using System.Net;
using System.Drawing;
using Telerik.Web.UI;
using System.Collections;
using VOR.Core.Domain.Vues;
using System.Web.UI.HtmlControls;

namespace VOR.Front.Web.Pages.Pelerin.Edit
{
    public partial class GestionPelerin : BasePage
    {
        #region constants

        private const int PERSON_PHOTO_WIDTH = 120;
        private const int PERSON_PHOTO_HEIGHT = 160;

        #endregion

        #region Properties

        public int? PelerinId
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
                if (this.PelerinId != null)
                {
                    this._hotels.PelerinId = this.PelerinId.Value;
                    this._accompagnants.PelerinId = this.PelerinId.Value;
                    this._commercial.PelerinId = this.PelerinId.Value;

                    this.grpPelerin.Visible = true;
                }

                InitControls();
                InitPelerin();

                this._hotels.Refresh();
                this._accompagnants.GetAccompagnants();
                this._accompagnants.Refresh();

                this._commercial.GetCommerciaux();
                this._commercial.Refresh();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                VOR.Core.Domain.Pelerin pelerin = Global.Container.Resolve<PelerinModel>().GetByID(this.PelerinId.Value);

                Global.Container.Resolve<PelerinModel>().Delete(pelerin);
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
                return;

            // Check if pelerin exist in this year
            bool isPelerinExist = false;
            if (!this.PelerinId.HasValue)
                isPelerinExist = Global.Container.Resolve<PelerinModel>().IsPelerinExistByYear(this._txtNomFR.Text, this._txtPrenomFR.Text, DateTime.Now.Year);

            if (isPelerinExist)
            {
                ShowMessageBow("Pelerin existant!", "warning");
                return;
            }

            VOR.Core.Domain.Pelerin pelerin = null;
            if (this.PelerinId.HasValue)
            {
                // Check if color exist for this event
                string couleur = string.Format("#{0}", this.RadColorPicker.SelectedColor.Name);
                IList<int> lstIdPelerinInRelation = GetRelationPelerin();
                lstIdPelerinInRelation.Add(this.PelerinId.Value);

                bool isColorExist = Global.Container.Resolve<PelerinModel>().isColorExist(lstIdPelerinInRelation.ToList(), couleur, int.Parse(this._ddlEvenement.SelectedValue));

                if (isColorExist)
                {
                    ShowMessageBow("Couleur Existante. Veuillez choisir une autre couleur.", "warning");
                    return;
                }

                pelerin = Global.Container.Resolve<PelerinModel>().GetByID(this.PelerinId.Value);
                pelerin.DateUpdate = DateTime.Now;
            }

            if (pelerin == null)
            {
                pelerin = new VOR.Core.Domain.Pelerin();
                pelerin.Hotels = new List<TPelerinHotel>();
                pelerin.Personnes = new List<TPersonnePelerin>();
                pelerin.DateCreation = DateTime.Now;
            }

            // Données personnelles
            pelerin.NomArabe = this._txtNomAR.Text;
            pelerin.NomFrancais = this._txtNomFR.Text.ToUpper();
            pelerin.PrenomArabe = this._txtPrenomAR.Text;
            pelerin.PrenomFrancais = this._txtPrenomFR.Text.ToUpper();

            int dayBirth = Convert.ToInt32(_ddlBirthDay.SelectedValue.ToString());
            int monthBirth = Convert.ToInt32(_ddlBirthMonth.SelectedValue.ToString());
            int yearBirth = Convert.ToInt32(_ddlBirthYear.SelectedValue.ToString());
            pelerin.DateNaissance = new DateTime(yearBirth, monthBirth, dayBirth);

            pelerin.EtatCivil = Global.Container.Resolve<EtatCivilModel>().LoadByID(int.Parse(this._ddlEtatCivil.SelectedValue));
            pelerin.Sexe = Global.Container.Resolve<SexeModel>().LoadByID(int.Parse(this._ddlSexe.SelectedValue));
            pelerin.Telephone1 = this._txtNumTelef1.Text;
            pelerin.Telephone2 = this._txtNumTelef2.Text;
            pelerin.NumPassport = this._txtNumPassport.Text;


            int dayExpire = Convert.ToInt32(_ddlExpireDay.SelectedValue.ToString());
            int monthExpire = Convert.ToInt32(_ddlExpireMonth.SelectedValue.ToString());
            int yearExpire = Convert.ToInt32(_ddlExpireYear.SelectedValue.ToString());
            pelerin.DateExpiration = new DateTime(yearExpire, monthExpire, dayExpire);

            if (!string.IsNullOrEmpty(this._hdnPhotoPerson.Value))
                pelerin.Photo = Convert.FromBase64String(_hdnPhotoPerson.Value);


            // Données voyage
            // Lors du changement de l'évenement on elimine la chambre du pelerin
            if (this.PelerinId.HasValue && int.Parse(this._ddlEvenement.SelectedValue) != pelerin.Evenement.ID)
            {
                pelerin.ChambreMakkah = null;
                pelerin.ChambreMedine = null;
            }
            pelerin.Evenement = Global.Container.Resolve<EvenementModel>().LoadByID(int.Parse(this._ddlEvenement.SelectedValue));
            pelerin.Programme = Global.Container.Resolve<ProgrammeModel>().LoadByID(int.Parse(this._ddlProgramme.SelectedValue));

            #region Hotels

            foreach (TPelerinHotel pelerinHotel in pelerin.Hotels.ToArray())
            {
                if (!this._hotels.ListHotels.Where(q => q.Key == pelerinHotel.Hotel.ID).Any())
                    pelerin.Hotels.Remove(pelerinHotel);
            }

            foreach (var item in this._hotels.ListHotels)
            {
                Hotel hotel = Global.Container.Resolve<HotelModel>().GetByID(item.Key);

                if (!pelerin.Hotels.Where(q => q.Hotel.ID == item.Key).Any())
                {
                    pelerin.Hotels.Add(new TPelerinHotel
                    {
                        Hotel = hotel,
                        Pelerin = pelerin
                    });
                }
            }

            #endregion

            #region Accompagnants

            foreach (TPersonnePelerin personnePelerin in pelerin.Personnes.Select(n => n).Where(n => n.Personne.TypePersonne.ID == (int)EnumTypePersonne.ACCOMPAGNANT).ToArray())
            {
                if (!this._accompagnants.ListPersonnes.Where(q => q.Key == personnePelerin.Personne.ID).Any())
                    pelerin.Personnes.Remove(personnePelerin);
            }

            foreach (var item in this._accompagnants.ListPersonnes)
            {
                Personne personne = Global.Container.Resolve<PersonneModel>().GetByID(item.Key);

                if (!pelerin.Personnes.Where(q => q.Personne.ID == item.Key).Any())
                {
                    pelerin.Personnes.Add(new TPersonnePelerin
                    {
                        Personne = personne,
                        Pelerin = pelerin
                    });
                }
            }

            #endregion

            #region Commerciaux

            foreach (TPersonnePelerin personnePelerin in pelerin.Personnes.Select(n => n).Where(n => n.Personne.TypePersonne.ID == (int)EnumTypePersonne.COMMERCIAL).ToArray())
            {
                if (!this._commercial.ListPersonnes.Where(q => q.Key == personnePelerin.Personne.ID).Any())
                    pelerin.Personnes.Remove(personnePelerin);
            }

            foreach (var item in this._commercial.ListPersonnes)
            {
                Personne personne = Global.Container.Resolve<PersonneModel>().GetByID(item.Key);

                if (!pelerin.Personnes.Where(q => q.Personne.ID == item.Key).Any())
                {
                    pelerin.Personnes.Add(new TPersonnePelerin
                    {
                        Personne = personne,
                        Pelerin = pelerin
                    });
                }
            }

            #endregion

            if (this.PelerinId.HasValue)
            {
                #region Relation Pelerin à Pelerin

                #region relation du pelerin en cours avec les pelerins séléctionnés

                IList<int> lstIdPelerinSelected = GetRelationPelerin();

                foreach (TPelerinPelerin pelerinPelerin in pelerin.Pelerins.ToArray())
                {
                    if (!lstIdPelerinSelected.Where(p => p == pelerinPelerin.Pelerin2.ID).Any())
                    {
                        pelerin.Pelerins.Remove(pelerinPelerin);

                        Global.Container.Resolve<PelerinModel>().RemovePelerinPelerin(pelerinPelerin.Pelerin1.ID, pelerinPelerin.Pelerin2.ID);
                    }
                }

                if (pelerin.Pelerins == null || pelerin.Pelerins.Count == 0)
                    Global.Container.Resolve<PelerinModel>().RemovePelerinPelerin(pelerin.ID, pelerin.ID);

                foreach (int idPelerin in lstIdPelerinSelected)
                {
                    VOR.Core.Domain.Pelerin pelerin2 = Global.Container.Resolve<PelerinModel>().GetByID(idPelerin);

                    if (!pelerin.Pelerins.Where(q => q.Pelerin2.ID == idPelerin).Any())
                    {
                        pelerin.Pelerins.Add(new TPelerinPelerin
                        {
                            Pelerin1 = pelerin,
                            Pelerin2 = pelerin2,
                            Couleur = string.Format("#{0}", this.RadColorPicker.SelectedColor.Name)
                        });
                    }
                    else
                    {
                        foreach (TPelerinPelerin tpp in pelerin.Pelerins)
                        {
                            tpp.Couleur = string.Format("#{0}", this.RadColorPicker.SelectedColor.Name);
                        }
                    }
                }

                IList<VOR.Core.Domain.Pelerin> lstPelerinEnRelation = pelerin.Pelerins.Select(n => n.Pelerin2).ToList();

                IList<TPelerinPelerin> lstPelerinPelerin = new List<TPelerinPelerin>();
                foreach (VOR.Core.Domain.Pelerin pelerinEnRelation in lstPelerinEnRelation)
                {
                    lstPelerinPelerin.Add(new TPelerinPelerin
                    {
                        Pelerin1 = pelerinEnRelation,
                        Pelerin2 = pelerin,
                        Couleur = string.Format("#{0}", this.RadColorPicker.SelectedColor.Name)
                    });

                    foreach (int idPelerin in lstIdPelerinSelected.Select(n => n).Where(n => n != pelerinEnRelation.ID))
                    {
                        VOR.Core.Domain.Pelerin pelerin2 = Global.Container.Resolve<PelerinModel>().GetByID(idPelerin);
                        lstPelerinPelerin.Add(new TPelerinPelerin
                        {
                            Pelerin1 = pelerinEnRelation,
                            Pelerin2 = pelerin2,
                            Couleur = string.Format("#{0}", this.RadColorPicker.SelectedColor.Name)
                        });
                    }

                    pelerinEnRelation.Pelerins = lstPelerinPelerin;

                    try
                    {
                        Global.Container.Resolve<PelerinModel>().InsertOrUpdate(pelerinEnRelation);
                    }
                    catch (Exception ex)
                    {
                        Logger.Current.Error(ex);
                        ShowMessageBow("Un problème a été rencontré lors de l'enregistrement", "delete");
                    }
                }


                #endregion

                #endregion
            }

            pelerin.TypeChambre = Global.Container.Resolve<TypeChambreModel>().LoadByID(int.Parse(this._ddlChambre.SelectedValue));
            pelerin.PrixVentePack = this._txtPrix.Value.HasValue ? (int)this._txtPrix.Value : 0;
            pelerin.MontantPaye = this._txtMontantPaye.Value.HasValue ? (int)this._txtMontantPaye.Value : 0;
            pelerin.EvaluationVoyage = this._txtEvaluationVoyage.Value.HasValue ? (int)this._txtEvaluationVoyage.Value : (int?)null;
            pelerin.EvaluationPelerin = this._txtEvaluationPelerin.Value.HasValue ? (int)this._txtEvaluationPelerin.Value : (int?)null;
            pelerin.Commentaire = this._txtCommentaire.Text;
            if (!this.PelerinId.HasValue)
                pelerin.Agence = Global.Container.Resolve<AgenceModel>().LoadByID(this.AgenceID);
            pelerin.RefVille = Global.Container.Resolve<RefVilleModel>().LoadByID(int.Parse(this._ddlVille.SelectedValue));
            pelerin.Alerte = string.IsNullOrEmpty(this._ddlAlerte.SelectedValue) ? null : Global.Container.Resolve<AlerteModel>().LoadByID(int.Parse(this._ddlAlerte.SelectedValue));
            pelerin.TransportPaye = bool.Parse(this._rdoTransportList.SelectedValue);
            pelerin.RepasPaye = bool.Parse(this._rdoRepasList.SelectedValue);
            pelerin.AssuranceMaladie = bool.Parse(this._rdoAssuranceMaladie.SelectedValue);
            pelerin.Stop = bool.Parse(this._rdoStop.SelectedValue);

            #region Vaccin

            if (!string.IsNullOrEmpty(this._txtNbrVaccin.Text))
            {
                pelerin.NbrVaccin = int.Parse(this._txtNbrVaccin.Text);
            }


            pelerin.TypePelerinID = int.Parse(this._dllTypePelerin.Text);

            #region Alerte champ dans la table PELERIN
            pelerin.AlerteDescription = _txtAlerteDescription.Text;
            #endregion

            if (pelerin.Vaccins != null)
            {
                if (pelerin.Vaccins.FirstOrDefault() != null)
                    pelerin.Vaccins = null;

                IList<Vaccin> vaccins = new List<Vaccin>();
                Vaccin vaccin = new Vaccin();
                vaccin.Nom = _dllTypesVaccins.SelectedItem.Text;
                vaccin.ID = int.Parse(_dllTypesVaccins.SelectedValue);
                vaccins.Add(vaccin);
                pelerin.Vaccins = vaccins;
            }
            #endregion
            try
            {
                Global.Container.Resolve<PelerinModel>().InsertOrUpdate(pelerin);
                string message = "ENREGISTEMENT EFFECTUE AVEC SUCCES";
                CloseAndRefresh(message);
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                ShowMessageBow("Un problème a été rencontré lors de l'enregistrement", "delete");
            }
        }

        protected void _ddlEvenement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._ddlEvenement.SelectedValue))
            {
                int evenementID = int.Parse(this._ddlEvenement.SelectedValue);
                BindDdlProgramme(evenementID);
                BindHotels(null);
            }
        }

        protected void _ddlProgramme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._ddlProgramme.SelectedValue))
            {
                int programmeId = int.Parse(this._ddlProgramme.SelectedValue);
                BindHotels(programmeId);
            }
            else
            {
                BindHotels(null);
            }
        }

        protected void _txtMontantPaye_TextChanged(object sender, EventArgs e)
        {
            this._txtResteAPayer.Text = this._txtPrix.Value.HasValue && this._txtMontantPaye.Value.HasValue ? ((int)this._txtPrix.Value - (int)this._txtMontantPaye.Value).ToString() : "0";
        }

        protected void RadColorPicker_ColorChanged(object sender, EventArgs e)
        {
            string couleur = string.Format("#{0}", this.RadColorPicker.SelectedColor.Name);
            IList<int> lstIdPelerinInRelation = GetRelationPelerin();
            lstIdPelerinInRelation.Add(this.PelerinId.Value);

            bool isColorExist = Global.Container.Resolve<PelerinModel>().isColorExist(lstIdPelerinInRelation.ToList(), couleur, this.EventID);

            if (isColorExist)
                ShowMessageBow("Couleur Existante. Veuillez choisir une autre couleur.", "warning");
        }

        protected void ddlPelerin_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var nbrChecked = this.ddlPelerin.CheckedItems;
            if (nbrChecked.Count > 0)
                this.RadColorPicker.Enabled = true;
            else
                this.RadColorPicker.Enabled = false;
        }


        protected void _ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._ddlRegion.SelectedValue))
            {
                BindDdlVille();
            }
        }


        protected void _ddlTypeAlerte_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDdlAlerte();
        }

        #endregion

        #region Private

        private void InitControls()
        {
            BindDdlEtatCivil();
            BindDdlSexe();
            BindDdlRegion();
            BinDdlEvenement();
            BindDdlProgramme(int.Parse(this._ddlEvenement.SelectedValue));
            BindDdlChambre();
            BindDdlsDate();
            BindDdlTypeAlerte();
            BindDblVaccins();
            BindDblTypePelerins();


            this._btnIDPhotoMaker.Attributes["onclick"] = OpenIDPhotoMaker();
        }

        private string OpenIDPhotoMaker()
        {
            string lang = CookieHelper.ReadCookieEntry("LANG");
            int cropWidth = PERSON_PHOTO_WIDTH;
            int cropHeight = PERSON_PHOTO_HEIGHT;

            return string.Format("openIDPhotoMaker('{0}', {1}, {2});", lang, cropWidth, cropHeight);
        }

        private void BindDdlEtatCivil()
        {
            this._ddlEtatCivil.DataSource = Global.Container.Resolve<EtatCivilModel>().GetAll();
            this._ddlEtatCivil.DataTextField = "Etatcivil";
            this._ddlEtatCivil.DataValueField = "ID";
            this._ddlEtatCivil.DataBind();
        }

        private void BindDdlSexe()
        {
            this._ddlSexe.DataSource = Global.Container.Resolve<SexeModel>().GetAll();
            this._ddlSexe.DataTextField = "Type";
            this._ddlSexe.DataValueField = "ID";
            this._ddlSexe.DataBind();
        }

        private void BindDdlRegion()
        {
            this._ddlRegion.Items.Clear();
            this._ddlRegion.Items.Add(new ListItem("---Sélectionnez---", ""));
            this._ddlRegion.DataSource = Global.Container.Resolve<RefRegionModel>().GetAll().OrderBy(n => n.Nom);
            this._ddlRegion.DataTextField = "Nom";
            this._ddlRegion.DataValueField = "ID";
            this._ddlRegion.DataBind();
        }

        private void BindDdlVille()
        {
            this._ddlVille.Items.Clear();
            this._ddlVille.Items.Add(new ListItem("---Sélectionnez---", ""));

            if (!string.IsNullOrEmpty(this._ddlRegion.SelectedValue))
            {
                int regionID = int.Parse(this._ddlRegion.SelectedValue);
                this._ddlVille.DataSource = Global.Container.Resolve<RefVilleModel>().GetVilleByRegionID(regionID).OrderBy(n => n.Nom);
            }

            this._ddlVille.DataTextField = "Nom";
            this._ddlVille.DataValueField = "ID";
            this._ddlVille.DataBind();
        }

        private void BinDdlEvenement()
        {
            this._ddlEvenement.DataSource = Global.Container.Resolve<EvenementModel>().GetEvenementsEnCours();
            this._ddlEvenement.DataTextField = "Nom";
            this._ddlEvenement.DataValueField = "ID";
            this._ddlEvenement.DataBind();

            this._ddlEvenement.SelectedValue = this.EventID.ToString();
        }

        private void BindDdlProgramme(int eventID)
        {
            this._ddlProgramme.Items.Clear();
            this._ddlProgramme.Items.Add(new ListItem("---Sélectionnez---", ""));
            this._ddlProgramme.DataSource = Global.Container.Resolve<ProgrammeModel>().GetProgrammeByEventID(eventID);
            this._ddlProgramme.DataTextField = "Nom";
            this._ddlProgramme.DataValueField = "ID";
            this._ddlProgramme.DataBind();
        }

        private void BindDdlChambre()
        {
            this._ddlChambre.Items.Add(new ListItem("---Sélectionnez---", ""));
            this._ddlChambre.DataSource = Global.Container.Resolve<TypeChambreModel>().GetAll();
            this._ddlChambre.DataTextField = "Nom";
            this._ddlChambre.DataValueField = "ID";
            this._ddlChambre.DataBind();
        }

        private void BindDblVaccins()
        {
            this._dllTypesVaccins.Items.Add(new ListItem("---Sélectionnez---", ""));
            this._dllTypesVaccins.DataSource = Global.Container.Resolve<VaccinModel>().GetAll();
            this._dllTypesVaccins.DataTextField = "Nom";
            this._dllTypesVaccins.DataValueField = "ID";
            this._dllTypesVaccins.DataBind();
        }

        private void BindDblTypePelerins()
        {
            this._dllTypePelerin.DataSource = Global.Container.Resolve<PelerinTypeModel>().GetAll();
            this._dllTypePelerin.DataTextField = "Libelle";
            this._dllTypePelerin.DataValueField = "ID";
            this._dllTypePelerin.DataBind();
        }
        
        private void BindDdlTypeAlerte()
        {
            this._ddlTypeAlerte.Items.Clear();
            this._ddlTypeAlerte.Items.Add(new ListItem("---Type D'alerte---", ""));
            this._ddlTypeAlerte.DataSource = Global.Container.Resolve<TypeAlerteModel>().GetAll();
            this._ddlTypeAlerte.DataTextField = "Libelle";
            this._ddlTypeAlerte.DataValueField = "ID";
            this._ddlTypeAlerte.DataBind();
        }

        private void BindDdlAlerte()
        {
            this._ddlAlerte.Items.Clear();
            this._ddlAlerte.Items.Add(new ListItem("---Alerte---", ""));

            if (!string.IsNullOrEmpty(this._ddlTypeAlerte.SelectedValue))
            {
                int typeAlerteId = int.Parse(this._ddlTypeAlerte.SelectedValue);
                this._ddlAlerte.DataSource = Global.Container.Resolve<AlerteModel>().GetAlerteByType(typeAlerteId);
                this._ddlAlerte.DataTextField = "Libelle";
                this._ddlAlerte.DataValueField = "ID";
                this._ddlAlerte.DataBind();
            }
        }

        private void BindHotels(int? programmeId)
        {
            if (programmeId.HasValue)
            {
                this._hotels.GetHotelsByProgrammeId(programmeId.Value);
                this._hotels.Refresh();
            }
            else
                this._hotels.Clear();
        }

        private void BindDdlsDate()
        {
            BindDdlDays();
            BinDdlMonths();
            BinDdlYears();
        }

        private void BindDdlDays()
        {
            this._ddlBirthDay.Items.Add(new ListItem("-- Jour --", ""));
            this._ddlExpireDay.Items.Add(new ListItem("-- Jour --", ""));

            for (int i = 1; i < 32; i++)
            {
                ListItem listBirthDay = new ListItem();
                listBirthDay.Text = i.ToString();
                listBirthDay.Value = i.ToString();
                _ddlBirthDay.Items.Add(listBirthDay);

                ListItem listExpireDay = new ListItem();
                listExpireDay.Text = i.ToString();
                listExpireDay.Value = i.ToString();
                _ddlExpireDay.Items.Add(listExpireDay);
            }
        }

        private void BinDdlMonths()
        {
            this._ddlBirthMonth.Items.Add(new ListItem("-- Mois --", ""));
            this._ddlExpireMonth.Items.Add(new ListItem("-- Mois --", ""));

            for (int i = 1; i < 13; i++)
            {
                ListItem listBirthMonth = new ListItem();
                listBirthMonth.Text = i.ToString();
                listBirthMonth.Value = i.ToString();
                _ddlBirthMonth.Items.Add(listBirthMonth);

                ListItem listExpireMonth = new ListItem();
                listExpireMonth.Text = i.ToString();
                listExpireMonth.Value = i.ToString();
                _ddlExpireMonth.Items.Add(listExpireMonth);
            }
        }

        private void BinDdlYears()
        {
            this._ddlBirthYear.Items.Add(new ListItem("-- Année --", ""));
            this._ddlExpireYear.Items.Add(new ListItem("-- Année --", ""));

            // Birth Year
            int yearLast = DateTime.Now.Year;
            int yearThen = yearLast - 90;
            for (int i = yearThen; i < yearLast; i++)
            {
                ListItem listBirthYear = new ListItem();
                listBirthYear.Text = yearThen.ToString();
                listBirthYear.Value = yearThen.ToString();
                _ddlBirthYear.Items.Add(listBirthYear);
                yearThen += 1;
            }

            // Expire Pass Year
            int yearNow = DateTime.Now.Year;
            int yearNext = yearNow + 12;
            for (int i = yearNow; i < yearNext; i++)
            {
                ListItem listBirthExpire = new ListItem();
                listBirthExpire.Text = yearNow.ToString();
                listBirthExpire.Value = yearNow.ToString();
                _ddlExpireYear.Items.Add(listBirthExpire);
                yearNow += 1;
            }
        }

        private IList<int> GetRelationPelerin()
        {
            IList<int> lstIdPelerin = new List<int>();

            var pelerin = this.ddlPelerin.CheckedItems;

            foreach (var item in pelerin)
            {
                lstIdPelerin.Add(int.Parse(item.Value));
            }

            return lstIdPelerin;
        }

        private void InitPelerin()
        {
            VOR.Core.Domain.Pelerin pelerin = null;

            if (this.PelerinId.HasValue)
                pelerin = Global.Container.Resolve<PelerinModel>().GetByID(this.PelerinId.Value);
            else
            {
                this._ddlRegion.SelectedValue = "10";
                this.BindDdlVille();
            }
            string imgSrc = "~/images/imagesCommunes/alien.png";

            if (pelerin != null)
            {
                // Données personnelles

                this._txtNomAR.Text = pelerin.NomArabe;
                this._txtNomFR.Text = pelerin.NomFrancais;
                this._txtPrenomAR.Text = pelerin.PrenomArabe;
                this._txtPrenomFR.Text = pelerin.PrenomFrancais;

                this._ddlBirthDay.SelectedValue = pelerin.DateNaissance.Day.ToString();
                this._ddlBirthMonth.SelectedValue = pelerin.DateNaissance.Month.ToString();
                this._ddlBirthYear.SelectedValue = pelerin.DateNaissance.Year.ToString();

                this._ddlEtatCivil.SelectedValue = pelerin.EtatCivil.ID.ToString();
                this._ddlSexe.SelectedValue = pelerin.Sexe.ID.ToString();
                this._txtNumTelef1.Text = pelerin.Telephone1;
                this._txtNumTelef2.Text = pelerin.Telephone2;
                this._txtNumPassport.Text = pelerin.NumPassport;

                this._dllTypePelerin.SelectedValue = pelerin.TypePelerinID.ToString();

                #region Vaccins
                Vaccin vaccin = pelerin.Vaccins.FirstOrDefault();
                if (vaccin != null)
                {
                    this._dllTypesVaccins.SelectedValue = vaccin.ID.ToString();
                    this._txtNbrVaccin.Text = pelerin.NbrVaccin.ToString();
                }
                #endregion

                #region Alerte champ dans la table PELERIN
                this._txtAlerteDescription.Text = pelerin.AlerteDescription;
                #endregion

                this._ddlExpireDay.SelectedValue = pelerin.DateExpiration.Day.ToString();
                this._ddlExpireMonth.SelectedValue = pelerin.DateExpiration.Month.ToString();
                this._ddlExpireYear.SelectedValue = pelerin.DateExpiration.Year.ToString();

                if (pelerin.Photo != null)
                {
                    string imgData = Convert.ToBase64String(pelerin.Photo);
                    imgSrc = string.Format("data:image/png;base64,{0}", imgData);
                    this._hdnPhotoPerson.Value = imgData;
                }

                // Données voyage
                this._ddlEvenement.SelectedValue = pelerin.Evenement.ID.ToString();
                this._ddlProgramme.SelectedValue = pelerin.Programme.ID.ToString();
                BindHotels(pelerin.Programme.ID);
                this._hotels.Refresh();
                this._ddlChambre.SelectedValue = pelerin.TypeChambre.ID.ToString();
                this._txtPrix.Text = pelerin.PrixVentePack.ToString();
                this._txtMontantPaye.Text = pelerin.MontantPaye.ToString();
                this._txtResteAPayer.Text = pelerin.RestApayer.ToString();
                this._txtEvaluationVoyage.Text = pelerin.EvaluationVoyage.HasValue ? pelerin.EvaluationVoyage.ToString() : null;
                this._txtEvaluationPelerin.Text = pelerin.EvaluationPelerin.HasValue ? pelerin.EvaluationPelerin.ToString() : null;
                this._txtCommentaire.Text = pelerin.Commentaire.ToString();
                this._ddlRegion.SelectedValue = pelerin.RefVille != null ? pelerin.RefVille.RefRegion.ID.ToString() : "";
                this.BindDdlVille();
                this._ddlVille.SelectedValue = pelerin.RefVille != null ? pelerin.RefVille.ID.ToString() : "";
                this._rdoTransportList.SelectedValue = pelerin.TransportPaye ? "true" : "false";
                this._rdoRepasList.SelectedValue = pelerin.RepasPaye ? "true" : "false";
                this._rdoAssuranceMaladie.SelectedValue = pelerin.AssuranceMaladie ? "true" : "false";
                this._rdoStop.SelectedValue = pelerin.Stop ? "true" : "false";

                if (pelerin.Alerte != null)
                {
                    this._ddlTypeAlerte.SelectedValue = pelerin.Alerte.TypeAlerte != null ? pelerin.Alerte.TypeAlerte.ID.ToString() : "";
                    this.BindDdlAlerte();
                    this._ddlAlerte.SelectedValue = pelerin.Alerte != null ? pelerin.Alerte.ID.ToString() : "";
                }

                // Relation Pelerin
                RadComboBoxItem rcb;
                VOR.Core.Domain.Agence agence = Global.Container.Resolve<AgenceModel>().LoadByID(this.AgenceID);

                int? agenceID = agence.TypeAgence.ID == (int)EnumTypeAgence.Filial ? (int?)this.AgenceID : null;
                IList<VuePelerin> lstPelerin = Global.Container.Resolve<PelerinModel>().GetPelerinByEventIDAndAgenceID(this.EventID, agenceID);

                foreach (VuePelerin p in lstPelerin.Where(p => p.ID != this.PelerinId).OrderBy(p => p.NomFrancais))
                {
                    rcb = new RadComboBoxItem();
                    rcb.Text = string.Format("{0}, {1}", p.NomFrancais, p.PrenomFrancais);
                    rcb.Value = p.ID.ToString();
                    rcb.Checked = pelerin.Pelerins.Select(n => n).Where(n => n.Pelerin2.ID == p.ID).ToList().Count > 0 ? true : false;
                    this.ddlPelerin.Items.Add(rcb);
                }

                if (pelerin.Pelerins.Count > 0)
                {
                    this.RadColorPicker.SelectedColor = ColorTranslator.FromHtml(pelerin.Pelerins.FirstOrDefault().Couleur);
                    this.RadColorPicker.Enabled = true;
                }

                this._btnSupprimer.Visible = true;
            }

            this._imgPhotoPerson.ImageUrl = imgSrc;
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