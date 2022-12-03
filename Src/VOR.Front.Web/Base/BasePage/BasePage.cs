using System;
using System.Configuration;
using System.Web.UI;
using VOR.Core;
using VOR.Core.Domain;
using VOR.Core.Enum;
using VOR.Core.Model;

namespace VOR.Front.Web.Base.BasePage
{
    public class BasePage : Page
    {
        public readonly Cookie CookieHelper;
        public readonly QueryStringManager QueryString;
        private readonly string cookieName;
        private int? _userID;
        public int UserID
        {
            get
            {
                return _userID ?? -1;
            }
            set
            {
                _userID = value;
            }
        }



        private int? _eventID;
        public int EventID
        {
            get
            {
                return _eventID ?? -1;
            }
            set
            {
                _eventID = value;
            }
        }

        private int? _agenceID;
        public int AgenceID
        {
            get
            {
                return _agenceID ?? -1;
            }
            set
            {
                _agenceID = value;
            }
        }


        private Utilisateur _utilisateur;

        public Utilisateur Utilisateur
        {
            get
            {
                if (_utilisateur == null)
                {
                    _utilisateur = Global.Container.Resolve<UtilisateurModel>().GetByID(this.UserID);
                }
                return _utilisateur;
            }
            set
            {
                this._utilisateur = value;
            }
        }

        public EnumTypeUtilisateur TypePersonne
        {
            get
            {
                if (this.Utilisateur != null && this.Utilisateur.TypeUtilisateur != null)
                    return (EnumTypeUtilisateur) this.Utilisateur.TypeUtilisateur.ID;
                else
                    return EnumTypeUtilisateur.Aucun;
            }
        }

        public BasePage()
        {
            cookieName = "coockieVOR";
            CookieHelper = new Cookie(cookieName);
            QueryString = new QueryStringManager();
        }

        protected string SkinTelerik
        {
            get
            {
                return "Bootstrap";
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (!CanAccessPage())
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private bool CanAccessPage()
        {
            if (!ProcessUserInfo())
            {
                return false;
            }

            this._userID = ExtractUserIdFromCookie();
            this._eventID = ExtractEventIdFromCookie();
            this._agenceID = ExtractAgenceIdFromCookie();

            if (this._userID.HasValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ProcessUserInfo()
        {
            if (string.IsNullOrEmpty(CookieHelper.ReadCookieEntry("UserInfo")))
            {
                return false;
            }

            if (CookieHelper.ReadCookieEntry("UserInfo").Split(';').Length == 0)
            {
                return false;
            }

            return true;
        }

        private int? ExtractUserIdFromCookie()
        {
            string[] userInfo = CookieHelper.ReadCookieEntry("UserInfo").Split(';');

            int userID;
            if (int.TryParse(userInfo[0], out userID))
            {
                CookieHelper.WriteCookieEntry("UserID", userID.ToString());
                return userID;
            }
            else
                return null;
        }

        private int? ExtractEventIdFromCookie()
        {
            int eventID;

            if (int.TryParse(CookieHelper.ReadCookieEntry("EventID"), out eventID))
            {
                CookieHelper.WriteCookieEntry("EventID", eventID.ToString());
                return eventID;
            }
            else
                return null;
        }

        private int? ExtractAgenceIdFromCookie()
        {
            int agenceID;

            if (int.TryParse(CookieHelper.ReadCookieEntry("AgenceID"), out agenceID))
            {
                CookieHelper.WriteCookieEntry("AgenceID", agenceID.ToString());
                return agenceID;
            }
            else
                return null;
        }
    }
}