using System;

namespace VOR.Front.Web.Base.Master
{
    public class BaseMasterPage : System.Web.UI.MasterPage
    {
        public virtual string Title { get; set; }

        public virtual string CurrentMenuTop { get; set; }

        private readonly string cookieName;
        public readonly Cookie CookieHelper;
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

        protected override void OnInit(EventArgs e)
        {
            this._userID = ExtractUserIdFromCookie();
            this._eventID = ExtractEventIdFromCookie();
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
            {
                return null;
            }
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

        protected BaseMasterPage()
        {
            cookieName = "coockieVOR";
            CookieHelper = new Cookie(cookieName);
        }
    }
}