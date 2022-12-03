using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VOR.Front.Web.UserControls.Pelerin
{
    public partial class IDPhotoMakerUC : System.Web.UI.UserControl
    {
        protected string Lang
        {
            get
            {
                if (!String.IsNullOrEmpty(Request.QueryString["lang"]))
                    return Request.QueryString["lang"].ToString().ToLower();
                else
                    return "fr";
            }
        }
        protected int? CropWidth
        {
            get
            {
                int value;

                if (int.TryParse(Request.QueryString["cropwidth"], out value))
                    return value;
                else
                    return null;
            }
        }
        protected int? CropHeight
        {
            get
            {
                int value;

                if (int.TryParse(Request.QueryString["cropheight"], out value))
                    return value;
                else
                    return null;
            }
        }
        protected string RefWin
        {
            get
            {
                if (!String.IsNullOrEmpty(Request.QueryString["refWin"]))
                    return Request.QueryString["refWin"].ToString();
                else
                    return String.Empty;
            }
        }
        protected string RefWinMan
        {
            get
            {
                if (!String.IsNullOrEmpty(Request.QueryString["refWinMan"]))
                    return Request.QueryString["refWinMan"].ToString();
                else
                    return String.Empty;
            }
        }
    }
}