using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VOR.Front.Web.Base.CustomControls
{
    public class RadioButtonUniqueID : System.Web.UI.WebControls.RadioButton
    {
        public string SpecificID { get; set; }

        public override string UniqueID
        {
            get
            {
                if (string.IsNullOrEmpty(SpecificID))
                    throw new ArgumentNullException("The property SpecificID has to be set");
                return SpecificID;
            }
        }

        public override string ClientID
        {
            get
            {
                if (string.IsNullOrEmpty(SpecificID))
                    throw new ArgumentNullException("The property SpecificID has to be set");
                return SpecificID;
            }
        }
    }
}