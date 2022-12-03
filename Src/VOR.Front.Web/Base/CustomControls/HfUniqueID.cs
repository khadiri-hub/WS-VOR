using System;

namespace VOR.Front.Web.Base.CustomControls
{
    public class HfUniqueID : System.Web.UI.WebControls.HiddenField
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