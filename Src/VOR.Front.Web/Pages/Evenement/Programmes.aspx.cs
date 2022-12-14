using VOR.Core.Model;
using VOR.Front.Web.Base.BasePage;
using VOR.Front.Web.UserControls.Menu.BO;
using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VOR.Core.Enum;
using VOR.Core.Domain.Vues;
using VOR.Core.Domain;

namespace VOR.Front.Web.Pages.Evenement
{
    public partial class Programmes : BasePage
    {
        #region Events 

        protected void Page_Init(object sender, EventArgs e)
        {
            ((SiteMaster) this.Master).ActiveTab = MenuTopUC.Tab.Evenement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager.GetCurrent(this.Page).ClientEvents.OnRequestStart = "onRequestStart";

            if (!IsPostBack)
                InitControls();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (IsPostBack)
                this.gridProgramme.Rebind();
        }

        protected void gridProgramme_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.gridProgramme.DataSource = Global.Container.Resolve<ProgrammeModel>().GetProgrammeByEventID(this.EventID);
        }

        protected void gridProgramme_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                string url = string.Empty;
                string pageUrl = string.Empty;
                string popupTitle = string.Empty;
                string myRadWindow = string.Empty;

                var btnEdit = (HyperLink) e.Item.FindControl("_btnEdit");
                Programme programme = (Programme) e.Item.DataItem;

                var lblPrixApartirDe = e.Item.FindControl("_lblPrixApartirDe") as Label;
                lblPrixApartirDe.Text = string.Format("{0} DHS", programme.PrixAPartirDe);

                var lblVol = e.Item.FindControl("_lblVol") as Label;
                string progDesc = programme.Vol != null ? string.Format("{0} ( {1} )", programme.Vol.Description, programme.Vol.CompAerienne.Nom) : "";
                lblVol.Text = progDesc;

                var lblEvenement = e.Item.FindControl("_lblEvenement") as Label;
                string eventDesc = programme.Evenement != null ? programme.Evenement.Nom : "";
                lblEvenement.Text = eventDesc;


                pageUrl = "~/Pages/Evenement/Edit/GestionProgramme.aspx";
                url = ResolveUrl(string.Format("{0}?RenderMode=popin&Id={1}", pageUrl, programme.ID));
                popupTitle = "Programme";
                myRadWindow = string.Format("return OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);

                btnEdit.NavigateUrl = "#";
                btnEdit.Attributes["onclick"] = myRadWindow;
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            this.gridProgramme.Rebind();
        }

        #endregion

        #region Private 

        private void InitControls()
        {
            this.gridProgramme.Skin = this.SkinTelerik;
            InitNewButton();
        }

        private void InitNewButton()
        {
            string pageUrl = string.Empty;
            string url = string.Empty;
            string function = string.Empty;
            string popupTitle = string.Empty;

            pageUrl = "~/Pages/Evenement/Edit/GestionProgramme.aspx";
            url = ResolveUrl(string.Format("{0}?RenderMode=popin", pageUrl));
            popupTitle = "Programme";

            function = string.Format("OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);
            btnNew.Attributes.Add("onClick", function);
        }

        #endregion
    }
}