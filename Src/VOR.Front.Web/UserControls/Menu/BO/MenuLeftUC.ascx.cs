using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using VOR.Core;
using VOR.Core.Domain;
using VOR.Core.Enum;
using VOR.Front.Web;
using VOR.Front.Web.Helpers;

namespace VOR.Front.Web.UserControls.Menu.BO
{
    public partial class MenuLeftUC : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetMenuVisivility();
            }
        }

        private void SetMenuVisivility()
        {
            MenuTopUC.Tab activeTab = SessionHelper.Get<MenuTopUC.Tab>(Session, SessionKey.TabActive);

            this._menuParametrage.Visible = (activeTab == MenuTopUC.Tab.Parametrage);
            this._menuEvenement.Visible = (activeTab == MenuTopUC.Tab.Evenement);
            this._menuPelerin.Visible = (activeTab == MenuTopUC.Tab.Pelerin);
            this._menuCollaborateur.Visible = (activeTab == MenuTopUC.Tab.Collaborateur);

            foreach (HyperLink link in WebUtil.FindControlsOfType<HyperLink>(this))
            {
                ((HtmlGenericControl) link.Parent).Attributes["class"] = Request.FilePath.Contains(link.NavigateUrl.Replace("~", string.Empty))  ? "enCours" : string.Empty;
            }

            SetMenuItemsVisivility();
        }

        private void SetMenuItemsVisivility()
        {
            if (this._menuParametrage.Visible)
            {
                this._menuItemGestionAgences.Visible = true;
                this._menuItemGestionUtilisateurs.Visible = true;
            }
            else if(this._menuEvenement.Visible)
            {
                Utilisateur user = Global.Container.Resolve<UtilisateurModel>().GetByID(this._basePage.UserID);

                this._menuItemGestionEvenement.Visible = user.TypeUtilisateur.ID == (int) EnumTypeUtilisateur.Administrateur ? true : false;
                this._menuItemGestionProgramme.Visible = true;
                this._menuItemGestionVilles.Visible = true;
                this._menuItemGestionHotels.Visible = true;
                this._menuItemGestionVols.Visible = true;
                this._menuItemGestionCompAerienne.Visible = true;
                this._menuItemGestionTypesChambre.Visible = true;
                this._menuItemGestionChambres.Visible = true;
                this._menuItemGestionPnr.Visible = user.TypeUtilisateur.ID == (int) EnumTypeUtilisateur.Administrateur ? true : false;
            }
            else if (this._menuPelerin.Visible)
            {
                this._menuItemGestionPelerin.Visible = true;
            }
            else if (this._menuCollaborateur.Visible)
            {
                this._menuItemGestionCollaborateur.Visible = true;
            }
        }
    }
}