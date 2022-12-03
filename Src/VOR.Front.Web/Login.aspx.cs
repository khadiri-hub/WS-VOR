using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using VOR.Core;
using VOR.Core.Domain;
using VOR.Core.Model;

namespace VOR.Front.Web
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControls();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void _btnValid_Click(object sender, EventArgs e)
        {
            Utilisateur user = Global.Container.Resolve<UtilisateurModel>().GetUtilisateurByLoginAndPwd(this._txtLogin.Text, this._txtPwd.Text);
            if (user != null)
            {
                Cookie cookie = new Cookie("coockieVOR");
                cookie.WriteCookieEntry("UserInfo", user.ID + ";" + user.Nom + ";" + user.Prenom);
                cookie.WriteCookieEntry("EventID", this._ddlEvenement.SelectedValue.ToString());
                cookie.WriteCookieEntry("AgenceID", user.Agence.ID.ToString());

                Response.Redirect("~/Pages/Pelerin/Pelerins.aspx");
            }
        }

        private void InitControls()
        {
            this._ddlEvenement.DataSource = Global.Container.Resolve<EvenementModel>().GetEvenementsEnCours();
            this._ddlEvenement.DataTextField = "Nom";
            this._ddlEvenement.DataValueField = "ID";
            this._ddlEvenement.DataBind();

            this._ddlEvenement.SelectedIndex = 0;    
        }
    }
}