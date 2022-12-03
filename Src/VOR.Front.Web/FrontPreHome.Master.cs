using System;
using VOR.Front.Web.Base.Master;
using VOR.Core.Model;
using System.Web.UI.HtmlControls;
namespace VOR.Front.Web
{
    public partial class FrontPreHomeMaster : BaseMasterPage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                this.LBLINFO.Attributes.Add("style", "display:none;");
                this.LBLERROR.Attributes.Add("style", "display:none;");
            }
            HtmlGenericControl cssBalise = new HtmlGenericControl("link");
            cssBalise.Attributes.Add("rel", "stylesheet");
            cssBalise.Attributes.Add("type", "text/css");

            HtmlGenericControl faviconBalise = new HtmlGenericControl("link");
            faviconBalise.Attributes.Add("rel", "shortcut icon");
            faviconBalise.Attributes.Add("type", "image/x-icon");

            //if (evenementEnCours == "BERCY")
            //{
            cssBalise.Attributes.Add("href", ResolveUrl("~/css/StyleFrontBercy.css?x=" + DateTime.Today));
            logoFFT.Attributes.Add("src", "~/images/imagesBercy/logo-fft.png");
            faviconBalise.Attributes.Add("href", ResolveUrl("~/images/imagesBercy/faviconBercy.png?x=" + DateTime.Today));
            //}
            //else
            //{
            //cssBalise.Attributes.Add("href", ResolveUrl("~/css/StyleFrontRG.css?x=" + DateTime.Today));
            //logoFFT.Attributes.Add("src", "~/images/imagesRG/logo-fft.png");
            //faviconBalise.Attributes.Add("href", ResolveUrl("~/images/imagesRG/favicon.ico?x="+DateTime.Today));
            //}

            this.Head.Controls.Add(cssBalise);
            this.Head.Controls.Add(faviconBalise);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}