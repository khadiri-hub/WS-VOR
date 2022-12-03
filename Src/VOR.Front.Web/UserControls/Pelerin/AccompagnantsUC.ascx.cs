using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using VOR.Core.Domain;
using VOR.Core.Model;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using VOR.Core.Enum;
using System.Linq;

namespace VOR.Front.Web.UserControls.Pelerin
{
    public partial class AccompagnantsUC : BaseUserControl
    {
        private JavaScriptSerializer serializer;

        public JavaScriptSerializer Serializer
        {
            get
            {
                if (serializer == null)
                {
                    serializer = new JavaScriptSerializer();
                }

                return serializer;
            }
        }

        public int ProgrammeId { get; set; }

        public int PelerinId { get; set; }

        private Dictionary<int, string> _listPersonnes;

        public Dictionary<int, string> ListPersonnes
        {
            get
            {
                if (ViewState["ListPersonnes"] == null)
                {
                    _listPersonnes = new Dictionary<int, string>();
                    if (this.PelerinId != 0)
                    {
                        VOR.Core.Domain.Pelerin pelerin = Global.Container.Resolve<PelerinModel>().GetByID(this.PelerinId);
                        if (pelerin != null)
                        {
                            foreach (TPersonnePelerin item in pelerin.Personnes.Select(n => n).Where(n => n.Personne.TypePersonne.ID == (int) EnumTypePersonne.ACCOMPAGNANT))
                            {
                                _listPersonnes.Add(item.Personne.ID, item.Personne.NomPrenom);
                            }
                        }
                    }

                    ViewState["ListPersonnes"] = _listPersonnes;
                }
                return (Dictionary<int, string>) ViewState["ListPersonnes"];
            }
            set
            {
                ViewState["ListPersonnes"] = value;
            }
        }

        public void Refresh()
        {
            foreach (KeyValuePair<int, string> item in ListPersonnes)
            {

                RadListBoxItem radItem = _radListBoxSource.FindItem(delegate (RadListBoxItem currentItem)
                {
                    if (currentItem != null && currentItem.DataKey != null && item.Key > 0)
                        return currentItem.DataKey.ToString() == item.Key.ToString();
                    else
                        return false;
                });

                if (radItem != null)
                    _radListBoxSource.Transfer(radItem, _radListBoxSource, _radListBoxDestination);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.ViewStateMode = System.Web.UI.ViewStateMode.Inherit;


            _radListBoxSource.Transferring += new RadListBoxTransferringEventHandler(RadListBoxSource_Transferring);

        }

        protected void Page_Load(object sender, EventArgs e) { }

        protected void RadListBoxSource_Transferred(object sender, RadListBoxTransferredEventArgs e)
        {
            foreach (RadListBoxItem item in e.Items)
            {
                item.DataBind();
            }
        }

        public void GetAccompagnants()
        {
            IList<Personne> lstAccompagnant = Global.Container.Resolve<PersonneModel>().GetPersonneByTypePersonne((int) EnumTypePersonne.ACCOMPAGNANT);
            if (lstAccompagnant != null)
            {
                _radListBoxSource.DataSource = lstAccompagnant;
                _radListBoxSource.DataTextField = "NomPrenom";
                _radListBoxSource.DataValueField = "ID";
                _radListBoxSource.DataKeyField = "ID";
                _radListBoxSource.DataBind();
            }

            _radListBoxDestination.DataBind();
        }

        protected void RadListBoxSource_Transferring(object sender, RadListBoxTransferringEventArgs e)
        {
            foreach (RadListBoxItem item in e.Items)
            {
                int personneID;

                if (e.SourceListBox.ID == "_radListBoxSource" && int.TryParse(item.Value.ToString(), out personneID))
                {
                    if (!ListPersonnes.ContainsKey(personneID))
                        ListPersonnes.Add(personneID, item.Text);
                }
                else if (e.SourceListBox.ID == "_radListBoxDestination" && int.TryParse(item.Value.ToString(), out personneID))
                {
                    if (ListPersonnes.ContainsKey(personneID))
                        ListPersonnes.Remove(personneID);
                }
            }
        }

        public void Clear()
        {
            _radListBoxSource.Items.Clear();
            _radListBoxDestination.Items.Clear();
            ListPersonnes.Clear();
        }

        public void Enable(bool enable)
        {
            this._radListBoxSource.AllowTransfer = enable;
            this._radListBoxSource.Width = new Unit(enable ? "100%" : "87%");
            this._radListBoxSource.Enabled = enable;
            this._radListBoxDestination.Enabled = enable;
        }
    }
}