using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using VOR.Front.Web.Base.Master;
using VOR.Front.Web.UserControls.Menu.BO;

namespace VOR.Front.Web
{
    public partial class SitePelerinMaster : BaseMasterPage
    {
        public new string Title
        {
            get
            {
                return this._pageTitle.Text;
            }
            set
            {
                this._pageTitle.Text = value;
            }
        }

        public MenuTopUC.Tab ActiveTab
        {
            get
            {
                return this._menuTopUC.ActiveTab;
            }
            set
            {
                this._menuTopUC.ActiveTab = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);         



            if (!Page.IsPostBack)
            {
                this.LBLINFO.Attributes.Add("style", "display:none;");
                this.LBLERROR.Attributes.Add("style", "display:none;");
            }
            this._lblTitle.Text = this.Title;


            HtmlGenericControl scriptOutside2 = new HtmlGenericControl("script");
            scriptOutside2.Attributes.Add("type", "text/javascript");
            scriptOutside2.Attributes.Add("src", ResolveUrl("~/Scripts/custom.js"));


            this.DivAutocomplete.Controls.Add(scriptOutside2);

            HtmlGenericControl faviconBalise = new HtmlGenericControl("link");
            faviconBalise.Attributes.Add("rel", "shortcut icon");
            faviconBalise.Attributes.Add("type", "image/x-icon");


            faviconBalise.Attributes.Add("href", ResolveUrl("~/images/imagesBack/favicon.ico?x=" + DateTime.Today));
            this.head.Controls.Add(faviconBalise);
        }
    }
}