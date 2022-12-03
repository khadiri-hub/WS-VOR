using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.SessionState;

namespace VOR.Front.Web.Helpers
{
    public enum SessionKey
    {
        DemandeParkings,
        TabActive
    }

    public static class SessionHelper
    {
        public static void Set(HttpSessionState session, SessionKey key, object value)
        {
            session[Enum.GetName(typeof(SessionKey), key)] = value;
        }

        public static T Get<T>(HttpSessionState session, SessionKey key)
        {
            var dataValue = session[Enum.GetName(typeof(SessionKey), key)];
            if (dataValue is T)
            {
                return (T)dataValue;
            }
            return default(T);
        }

        public static IEnumerable<int> GetDemandeParkings(HttpSessionState session)
        {
            return Get<IEnumerable<int>>(session, SessionKey.DemandeParkings);
        }
    }
}