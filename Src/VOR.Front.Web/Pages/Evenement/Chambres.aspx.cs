using VOR.Core.Model;
using VOR.Front.Web.Base.BasePage;
using VOR.Front.Web.UserControls.Menu.BO;
using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VOR.Core.Domain;
using System.Drawing;
using VOR.Core.Enum;
using System.Collections.Generic;
using System.Linq;

namespace VOR.Front.Web.Pages.Evenement
{
    public partial class Chambres : BasePage
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
                this.gridChambre.Rebind();
        }

        protected void gridChambre_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            IList<Chambre> lstChambre = Global.Container.Resolve<ChambreModel>().GetChambreByEventID(this.EventID);
            if (lstChambre != null)
                this.gridChambre.DataSource = lstChambre.OrderBy(n => n.Hotel.Ville.Code).ToList();
        }

        protected void gridChambre_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                string url = string.Empty;
                string pageUrl = string.Empty;
                string popupTitle = string.Empty;
                string myRadWindow = string.Empty;

                var btnEdit = (HyperLink) e.Item.FindControl("_btnEdit");
                Chambre chambre = (Chambre) e.Item.DataItem;


                if(chambre.Hotel.Ville.ID == (int)EnumVille.MAKKAH) 
                {
                    if (chambre.PelerinsMakkah.Count == chambre.TypeChambre.Code)
                        e.Item.BackColor = ColorTranslator.FromHtml("#ff3f3f");
                    else if (chambre.PelerinsMakkah.Count == 0)
                        e.Item.BackColor = ColorTranslator.FromHtml("#56c605");
                    else
                        e.Item.BackColor = ColorTranslator.FromHtml("#ff9721");
                }
                else {
                    if (chambre.PelerinsMedine.Count == chambre.TypeChambre.Code)
                        e.Item.BackColor = ColorTranslator.FromHtml("#ff3f3f");
                    else if (chambre.PelerinsMedine.Count == 0)
                        e.Item.BackColor = ColorTranslator.FromHtml("#56c605");
                    else
                        e.Item.BackColor = ColorTranslator.FromHtml("#ff9721");
                }


                pageUrl = "~/Pages/Evenement/Edit/GestionChambre.aspx";
                url = ResolveUrl(string.Format("{0}?RenderMode=popin&Id={1}", pageUrl, chambre.ID));
                popupTitle = "Chambre";
                myRadWindow = string.Format("return OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);

                btnEdit.NavigateUrl = "#";
                btnEdit.Attributes["onclick"] = myRadWindow;
            }
        }

        protected void RadAjaxManager_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            this.gridChambre.Rebind();
        }

        #endregion

        #region Private 

        private void InitControls()
        {
            this.gridChambre.Skin = this.SkinTelerik;
            InitNewButton();
        }

        private void InitNewButton()
        {
            string pageUrl = string.Empty;
            string url = string.Empty;
            string function = string.Empty;
            string popupTitle = string.Empty;

            pageUrl = "~/Pages/Evenement/Edit/GestionChambre.aspx";
            url = ResolveUrl(string.Format("{0}?RenderMode=popin", pageUrl));
            popupTitle = "Chambre";

            function = string.Format("OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);
            btnNew.Attributes.Add("onClick", function);
        }

        #endregion
    }
}