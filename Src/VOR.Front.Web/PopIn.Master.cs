using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using VOR.Front.Web.Base.Master;
using VOR.Core.Model;

namespace VOR.Front.Web
{
    public partial class PopinMaster : BaseMasterPage
    {
        public override string Title
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

            HtmlGenericControl scriptOutside2 = new HtmlGenericControl("script");
            scriptOutside2.Attributes.Add("type", "text/javascript");
            scriptOutside2.Attributes.Add("src", ResolveUrl("~/Scripts/custom.js"));

            this.DivAutocomplete.Controls.Add(scriptOutside2);

            HtmlGenericControl cssBalise = new HtmlGenericControl("link");
            cssBalise.Attributes.Add("rel", "stylesheet");
            cssBalise.Attributes.Add("type", "text/css");

            cssBalise.Attributes.Add("href", ResolveUrl("~/css/StyleBack.css"));
            this.head.Controls.Add(cssBalise);
        }
    }
}