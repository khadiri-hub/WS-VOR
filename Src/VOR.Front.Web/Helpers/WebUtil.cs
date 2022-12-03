using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VOR.Front.Web.Helpers
{
    public static class WebUtil
    {
        public static T FindControlByAttribute<T>(Control ctl, string attributeName, string attributeValue) where T : WebControl
        {
            foreach (Control c in ctl.Controls)
            {
                if (c.GetType() == typeof(T) && ((T) c).Attributes[attributeName] == attributeValue)
                    return (T) c;

                T cb = FindControlByAttribute<T>(c, attributeName, attributeValue);

                if (cb != null)
                    return cb;
            }

            return null;
        }

        public static IEnumerable<Control> FindControlsOfType<T>(Control control)
        {
            IEnumerable<Control> controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => FindControlsOfType<T>(ctrl)).Concat(controls).Where(c => c.GetType() == typeof(T));
        }
    }
}