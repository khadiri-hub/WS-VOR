using System;
using System.Configuration;
using System.Web;
using Castle.Windsor;
using VOR.Utils;
using VOR.Core.Model;

namespace VOR.Front.Web
{
    public class Global : System.Web.HttpApplication
    {
        public static readonly IWindsorContainer Container;

        static Global()
        {
            try
            {
                Container = Bootstrapper.Start();
            }
            catch (Exception ex)
            {
                Logger.Current.Error("Erreur initialisation Injection", ex);
                throw;
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["URL_ERROR"]))
            {
                Application["URL_ERROR"] = ConfigurationManager.AppSettings["URL_ERROR"];
            }
            else
            {
                Application["URL_ERROR"] = string.Empty;
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            System.Web.HttpContext context = HttpContext.Current;
            System.Exception ex = Context.Server.GetLastError();

            var code = (ex is HttpException) ? (ex as HttpException).GetHttpCode() : 500;

            if (code != 404)
            {
                Logger.Current.Error("Erreur remontée dans le Global.asax", ex);
#if !DEBUG
                context.Server.ClearError();
#endif
                if (!string.IsNullOrEmpty(Application["URL_ERROR"].ToString()))
                    Response.Redirect(Application["URL_ERROR"].ToString());
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}