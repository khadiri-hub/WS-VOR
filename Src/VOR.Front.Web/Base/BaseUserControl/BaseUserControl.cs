using VOR.Front.Web.Base.BasePage;
using System;
using System.Web.UI;

namespace VOR.Front.Web
{
    public class BaseUserControl : UserControl
    {
        protected BasePage _basePage;

        protected string SkinTelerik
        {
            get
            {
                return "Office2010Silver";
            }
        }

        protected BaseUserControl()
        {
            this._basePage = new BasePage();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this._basePage = this.Page as BasePage;
        }
    }
}