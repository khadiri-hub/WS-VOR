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
    public partial class Evenements : BasePage
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

        protected void gridEvenement_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.gridEvenement.DataSource = Global.Container.Resolve<EvenementModel>().GetAll();
        }

        protected void gridEvenement_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                string url = string.Empty;
                string pageUrl = string.Empty;
                string popupTitle = string.Empty;
                string myRadWindow = string.Empty;

                var btnEdit = (HyperLink) e.Item.FindControl("_btnEdit");
                VOR.Core.Domain.Evenement evenement = (VOR.Core.Domain.Evenement) e.Item.DataItem;

                var lblDateDebut = e.Item.FindControl("_lblDateDebut") as Label;
                var lblDateFin = e.Item.FindControl("_lblDateFin") as Label;
                var lblEnCours = e.Item.FindControl("_lblEnCours") as Label;
                var lblDuree = e.Item.FindControl("_lblDuree") as Label;

                lblDateDebut.Text = evenement.DateDebut.ToShortDateString();
                lblDateFin.Text = evenement.DateFin.ToShortDateString();
                lblEnCours.Text = evenement.EnCours ? "Oui" : "Non";
                lblDuree.Text = string.Format("{0} Jours", evenement.Duree);

                pageUrl = "~/Pages/Evenement/Edit/GestionEvenement.aspx";
                url = ResolveUrl(string.Format("{0}?RenderMode=popin&Id={1}", pageUrl, evenement.ID));
                popupTitle = "Evenement";
                myRadWindow = string.Format("return OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);

                btnEdit.NavigateUrl = "#";
                btnEdit.Attributes["onclick"] = myRadWindow;
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            this.gridEvenement.Rebind();
        }

        #endregion

        #region Private 

        private void InitControls()
        {
            this.gridEvenement.Skin = this.SkinTelerik;
            InitNewButton();
        }

        private void InitNewButton()
        {
            string pageUrl = string.Empty;
            string url = string.Empty;
            string function = string.Empty;
            string popupTitle = string.Empty;

            pageUrl = "~/Pages/Evenement/Edit/GestionEvenement.aspx";
            url = ResolveUrl(string.Format("{0}?RenderMode=popin", pageUrl));
            popupTitle = "Evenement";

            function = string.Format("OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);
            btnNew.Attributes.Add("onClick", function);
        }

        #endregion
    }
}