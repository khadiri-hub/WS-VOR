using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using VOR.Core.Domain;
using VOR.Core.Model;
using Telerik.Web.UI;
using System.Web.UI;
using System.Web.UI.WebControls;
using VOR.Core.Domain.Vues;
using System.Linq;

namespace VOR.Front.Web.UserControls.Evenement
{
    public partial class HotelsUC : BaseUserControl
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

        private Dictionary<int, string> _listHotels;

        public Dictionary<int, string> ListHotels
        {
            get
            {
                if (ViewState["ListHotels"] == null)
                {
                    _listHotels = new Dictionary<int, string>();
                    if (this.ProgrammeId != 0)
                    {
                        Programme programme = Global.Container.Resolve<ProgrammeModel>().GetByID(this.ProgrammeId);
                        if (programme != null)
                        {
                            foreach (TProgrammeHotel item in programme.Hotels)
                            {
                                _listHotels.Add(item.Hotel.ID, item.Hotel.NomLong);
                            }
                        }
                    }

                    else if (this.PelerinId != 0)
                    {
                        VOR.Core.Domain.Pelerin pelerin = Global.Container.Resolve<PelerinModel>().GetByID(this.PelerinId);
                        if (pelerin != null)
                        {
                            foreach (TPelerinHotel item in pelerin.Hotels)
                            {
                                _listHotels.Add(item.Hotel.ID, item.Hotel.NomLong);
                            }
                        }
                    }

                    ViewState["ListHotels"] = _listHotels;
                }
                return (Dictionary<int, string>) ViewState["ListHotels"];
            }
            set
            {
                ViewState["ListHotels"] = value;
            }
        }

        public void Refresh()
        {
            foreach (KeyValuePair<int, string> item in ListHotels)
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

        public void GetHotels()
        {
            IList<Hotel> lstHotels = Global.Container.Resolve<HotelModel>().GetAll();
            if (lstHotels != null)
            {
                _radListBoxSource.DataSource = lstHotels;
                _radListBoxSource.DataTextField = "NomLong";
                _radListBoxSource.DataValueField = "ID";
                _radListBoxSource.DataKeyField = "ID";
                _radListBoxSource.DataBind();
            }

            _radListBoxDestination.DataBind();
        }

        public void GetHotelsByProgrammeId(int programmeId)
        {
            Programme programme = Global.Container.Resolve<ProgrammeModel>().GetByID(programmeId);
            IList<Hotel> lstHotels = programme.Hotels.Select(n => n.Hotel).ToList();
            if (lstHotels != null)
            {
                _radListBoxSource.DataSource = lstHotels;
                _radListBoxSource.DataTextField = "NomLong";
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
                int hotelID;

                if (e.SourceListBox.ID == "_radListBoxSource" && int.TryParse(item.Value.ToString(), out hotelID))
                {
                    if (!ListHotels.ContainsKey(hotelID))
                        ListHotels.Add(hotelID, item.Text);
                }
                else if (e.SourceListBox.ID == "_radListBoxDestination" && int.TryParse(item.Value.ToString(), out hotelID))
                {
                    if (ListHotels.ContainsKey(hotelID))
                        ListHotels.Remove(hotelID);
                }
            }
        }

        public void Clear()
        {
            _radListBoxSource.Items.Clear();
            _radListBoxDestination.Items.Clear();
            ListHotels.Clear();
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