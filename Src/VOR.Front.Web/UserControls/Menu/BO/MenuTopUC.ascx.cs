using VOR.Core.Enum;
using System;
using System.Web.UI.WebControls;
using VOR.Front.Web.Helpers;
using Telerik.Web.UI;
using System.Collections.Generic;
using VOR.Core.Model;

namespace VOR.Front.Web.UserControls.Menu.BO
{
    public partial class MenuTopUC : BaseUserControl
    {

        public enum Tab
        {
            None,
            Parametrage,
            Pelerin,
            Hebergement,
            Evenement,
            Collaborateur
        }

        public Tab ActiveTab
        {
            get
            {
                return SessionHelper.Get<Tab>(Session, SessionKey.TabActive);
            }
            set
            {
                if (value != Tab.None)
                {
                    Panel menu = WebUtil.FindControlByAttribute<Panel>(this.Page, "data-index", value.ToString());


                    if (menu != null)
                    {
                        switch (value.ToString())
                        {
                            case "Parametrage":
                                btnParametrage.Attributes["class"] = "dropbtn active";
                                break;
                            case "Pelerin":
                                btnPelerin.Attributes["class"] = "dropbtn active";
                                break;
                            case "Hebergement":
                                btnHebergement.Attributes["class"] = "dropbtn active";
                                break;
                            case "Evenement":
                                btnEvenement.Attributes["class"] = "dropbtn active";
                                break;
                            case "Collaborateur":
                                btnCollaborateur.Attributes["class"] = "dropbtn active";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                        btnPelerin.Attributes["class"] = "dropbtn active";
                }
                else
                {
                    btnPelerin.Attributes["class"] = "dropbtn active";
                }

                SessionHelper.Set(Session, SessionKey.TabActive, value);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetMenuItemsVisivility();
                SetSousMenuItemsVisivility();
            }
        }

        private void SetMenuItemsVisivility()
        {
            switch (this._basePage.TypePersonne)
            {
                case EnumTypeUtilisateur.Administrateur:
                    
                    this._pnlMenuParametrage.Visible = true;
                    this._pnlMenuPelerin.Visible = true;
                    this._pnlHebergement.Visible = true;
                    this._pnlMenuEvenement.Visible = true;
                    this._pnlMenuCollaborateur.Visible = true;
                    break;
                case EnumTypeUtilisateur.Utilisateur:
                    this._pnlMenuParametrage.Visible = false;
                    this._pnlMenuPelerin.Visible = true;
                    this._pnlHebergement.Visible = true;
                    this._pnlMenuEvenement.Visible = false;
                    this._pnlMenuCollaborateur.Visible = false;
                    break;
                default:
                    this._pnlMenuParametrage.Visible = false;
                    this._pnlMenuPelerin.Visible = false;
                    this._pnlHebergement.Visible = false;
                    this._pnlMenuEvenement.Visible = false;
                    this._pnlMenuCollaborateur.Visible = false;
                    break;
            }
        }

        private void SetSousMenuItemsVisivility()
        {
            switch (this._basePage.TypePersonne)
            {
                case EnumTypeUtilisateur.Administrateur:
                    this.lnkVueGlobal.Visible = true;
                    this.lnkHebergementMakkah.Visible = true;
                    this.lnkHebergementMedine.Visible = true;
                    this.lnkHebergementMakkahSaison.Visible = true;
                    this.lnkHebergementMedineSaison.Visible = true;
                    this.lnkGestionPelerin.Visible = true;
                    this.lnkGestionAgence.Visible = true;
                    this.lnkGestionUtilisateurs.Visible = true;
                    this.lnkEvenement.Visible = true;
                    this.lnkPnr.Visible = true;
                    this.lnkProgramme.Visible = true;
                    this.lnkVilles.Visible = true;
                    this.lnkHotels.Visible = true;
                    this.lnkVols.Visible = true;
                    this.lnkCompagnies.Visible = true;
                    this.lnkTypeChambre.Visible = true;
                    this.lnkChambres.Visible = true;
                    this.lnkCollaborateurs.Visible = true;
                    break;
                default:
                    this.lnkVueGlobal.Visible = false;
                    this.lnkHebergementMakkah.Visible = true;
                    this.lnkHebergementMedine.Visible = true;
                    this.lnkHebergementMakkahSaison.Visible = false;
                    this.lnkHebergementMedineSaison.Visible = false;
                    this.lnkGestionAgence.Visible = false;
                    this.lnkGestionUtilisateurs.Visible = false;
                    this.lnkEvenement.Visible = false;
                    this.lnkPnr.Visible = false;
                    this.lnkCollaborateurs.Visible = false;
                    this.lnkGestionPelerin.Visible = true;
                    this.lnkProgramme.Visible = true;
                    this.lnkVilles.Visible = true;
                    this.lnkHotels.Visible = true;
                    this.lnkVols.Visible = true;
                    this.lnkCompagnies.Visible = true;
                    this.lnkTypeChambre.Visible = true;
                    this.lnkChambres.Visible = true;
                    break;
            }
        }

     
    }
}