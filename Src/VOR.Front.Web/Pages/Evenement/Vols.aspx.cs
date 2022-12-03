using VOR.Core.Model;
using VOR.Front.Web.Base.BasePage;
using VOR.Front.Web.UserControls.Menu.BO;
using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VOR.Core.Enum;
using VOR.Core.Domain.Vues;
using VOR.Core.Domain;
using VOR.Core;

namespace VOR.Front.Web.Pages.Evenement
{
    public partial class Vols : BasePage
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

        protected void gridVol_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.gridVol.DataSource = Global.Container.Resolve<VolModel>().GetAll();
        }

        protected void gridVol_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                string url = string.Empty;
                string pageUrl = string.Empty;
                string popupTitle = string.Empty;
                string myRadWindow = string.Empty;

                var btnEdit = (HyperLink) e.Item.FindControl("_btnEdit");
                Vol vol = (Vol) e.Item.DataItem;

                pageUrl = "~/Pages/Evenement/Edit/GestionVol.aspx";
                url = ResolveUrl(string.Format("{0}?RenderMode=popin&Id={1}", pageUrl, vol.ID));
                popupTitle = "Vol";
                myRadWindow = string.Format("return OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);

                btnEdit.NavigateUrl = "#";
                btnEdit.Attributes["onclick"] = myRadWindow;
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            this.gridVol.Rebind();
        }

        #endregion

        #region Private 

        private void InitControls()
        {
            this.gridVol.Skin = this.SkinTelerik;
            InitNewButton();
        }

        private void InitNewButton()
        {
            string pageUrl = string.Empty;
            string url = string.Empty;
            string function = string.Empty;
            string popupTitle = string.Empty;

            pageUrl = "~/Pages/Evenement/Edit/GestionVol.aspx";
            url = ResolveUrl(string.Format("{0}?RenderMode=popin", pageUrl));
            popupTitle = "Vol";

            function = string.Format("OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);
            btnNew.Attributes.Add("onClick", function);
        }

        #endregion
    }
}