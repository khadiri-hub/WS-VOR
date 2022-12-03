using VOR.Front.Web.Base.BasePage;
using VOR.Front.Web.UserControls.Menu.BO;
using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VOR.Core.Domain;
using VOR.Core;
using VOR.Core.Model;

namespace VOR.Front.Web.Pages.Evenement
{
    public partial class Hotels : BasePage
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

        protected void gridHotel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.gridHotel.DataSource = Global.Container.Resolve<HotelModel>().GetAll();
        }

        protected void gridHotel_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                string url = string.Empty;
                string pageUrl = string.Empty;
                string popupTitle = string.Empty;
                string myRadWindow = string.Empty;

                var btnEdit = (HyperLink) e.Item.FindControl("_btnEdit");
                Hotel hotel = (Hotel) e.Item.DataItem;

                var lblDistanceToHaram = e.Item.FindControl("_lblDistanceToHaram") as Label;
                lblDistanceToHaram.Text = string.Format("{0} Mêtres", hotel.DistanceToHaram);

                var categorie = e.Item.FindControl("_categorie") as Image;

                if(hotel.Categorie == 3)
                    categorie.ImageUrl = "~/Images/imagesBack/picto3stars.png";
                else if (hotel.Categorie == 4)
                    categorie.ImageUrl = "~/Images/imagesBack/picto4stars.png";
                else if (hotel.Categorie == 5)
                    categorie.ImageUrl = "~/Images/imagesBack/picto5stars.png";

                pageUrl = "~/Pages/Evenement/Edit/GestionHotel.aspx";
                url = ResolveUrl(string.Format("{0}?RenderMode=popin&Id={1}", pageUrl, hotel.ID));
                popupTitle = "Hotel";
                myRadWindow = string.Format("return OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);

                btnEdit.NavigateUrl = "#";
                btnEdit.Attributes["onclick"] = myRadWindow;
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            this.gridHotel.Rebind();
        }

        #endregion

        #region Private 

        private void InitControls()
        {
            this.gridHotel.Skin = this.SkinTelerik;
            InitNewButton();
        }

        private void InitNewButton()
        {
            string pageUrl = string.Empty;
            string url = string.Empty;
            string function = string.Empty;
            string popupTitle = string.Empty;

            pageUrl = "~/Pages/Evenement/Edit/GestionHotel.aspx";
            url = ResolveUrl(string.Format("{0}?RenderMode=popin", pageUrl));
            popupTitle = "Hotel";

            function = string.Format("OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);
            btnNew.Attributes.Add("onClick", function);
        }

        #endregion
    }
}