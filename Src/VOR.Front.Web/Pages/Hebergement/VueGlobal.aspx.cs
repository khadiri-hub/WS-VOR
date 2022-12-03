using VOR.Core.Model;
using VOR.Front.Web.Base.BasePage;
using VOR.Front.Web.UserControls.Menu.BO;
using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VOR.Core.Domain;
using VOR.Core.Enum;
using VOR.Core.Domain.Vues;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Drawing;
using System.Web.UI.HtmlControls;
using VOR.Utils;

namespace VOR.Front.Web.Pages.Hebergement
{
    public partial class VueGlobal : BasePage
    {

        #region private 

        private List<int> _eventsIds
        {
            get
            {
                IList<RadComboBoxItem> lstChecked = this.ddlEvents.CheckedItems;
                List<int> lstEventID = new List<int>();

                foreach (string item in lstChecked.Select(n => n.Value).ToList())
                {
                    lstEventID.Add(int.Parse(item));
                }
                return lstEventID;
            }
        }

        private List<int> _typesChambreIds
        {
            get
            {
                IList<RadComboBoxItem> lstChecked = this.ddlTypesChambre.CheckedItems;
                List<int> lstTypeChambreID = new List<int>();

                foreach (string item in lstChecked.Select(n => n.Value).ToList())
                {
                    lstTypeChambreID.Add(int.Parse(item));
                }
                return lstTypeChambreID;
            }
        }

        private List<int> _hotelIds
        {
            get
            {
                IList<RadComboBoxItem> lstChecked = this.ddlHotels.CheckedItems;
                List<int> lstHotelID = new List<int>();

                foreach (string item in lstChecked.Select(n => n.Value).ToList())
                {
                    lstHotelID.Add(int.Parse(item));
                }
                return lstHotelID;
            }
        }

        private List<Chambre> _chambres;
        private List<Chambre> chambres
        {
            get
            {
                if (this._chambres == null)
                    this._chambres = Global.Container.Resolve<ChambreModel>().GetChambreByListEventIDAndVilleID(this._eventsIds, null).ToList();
                return this._chambres;
            }
        }

        #endregion

        #region Events 

        protected void Page_Init(object sender, EventArgs e)
        {
            ((SiteMaster)this.Master).ActiveTab = MenuTopUC.Tab.Hebergement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager.GetCurrent(this.Page).ClientEvents.OnRequestStart = "onRequestStart";

            if (!IsPostBack)
            {
                BindEvents();
                BindTypeChambre();
                BindHotels();
                BindEventsRepeater();
            }
            else {
                EventRepeater.DataSource = new List<Core.Domain.Evenement>();
                EventRepeater.DataBind();
            }
        }

        protected void EventRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Core.Domain.Evenement evenement = (Core.Domain.Evenement)e.Item.DataItem;
                Label lblEvenement = (Label)e.Item.FindControl("Evenement");
                lblEvenement.Text = evenement.Nom;

                IList<Chambre> lstChambreMakkah = this.chambres.Select(n => n).Where(n => this._typesChambreIds.Contains(n.TypeChambre.ID) && this._hotelIds.Contains(n.Hotel.ID) && n.Hotel.Ville.ID == (int)EnumVille.MAKKAH).ToList();
                Repeater ChambreRepeaterMakkah = (Repeater)e.Item.FindControl("ChambreRepeaterMakkah");
                IList<Chambre> lstChambreMakkahEvent = lstChambreMakkah.Select(n => n).Where(n => n.Evenement.ID == evenement.ID).OrderBy(n => n.TypeChambre.Code).ToList();
                ChambreRepeaterMakkah.DataSource = lstChambreMakkahEvent;
                ChambreRepeaterMakkah.DataBind();

                IList<Chambre> lstChambreMedine = this.chambres.Select(n => n).Where(n => this._typesChambreIds.Contains(n.TypeChambre.ID) && this._hotelIds.Contains(n.Hotel.ID) && n.Hotel.Ville.ID == (int)EnumVille.MEDINE).ToList();
                Repeater ChambreRepeaterMedine = (Repeater)e.Item.FindControl("ChambreRepeaterMedine");
                IList<Chambre> lstChambreMedineEvent = lstChambreMedine.Select(n => n).Where(n => n.Evenement.ID == evenement.ID).OrderBy(n => n.TypeChambre.Code).ToList();
                ChambreRepeaterMedine.DataSource = lstChambreMedineEvent;
                ChambreRepeaterMedine.DataBind();


                Label countPelerinMakkah = (Label)e.Item.FindControl("CountPelerinMakkah");
                countPelerinMakkah.Text = string.Format("Total: {0}", lstChambreMakkahEvent.Select(n => n.PelerinsMakkah.Count).Sum()).ToString();
                this.TotalPelerinMakkah.Text = string.Format("Total: {0}", lstChambreMakkah.Select(n => n.PelerinsMakkah.Count).Sum()).ToString();

                Label countPelerinMedine = (Label)e.Item.FindControl("CountPelerinMedine");
                countPelerinMedine.Text = string.Format("Total: {0}", lstChambreMedineEvent.Select(n => n.PelerinsMedine.Count).Sum()).ToString();
                this.TotalPelerinMedine.Text = string.Format("Total: {0}", lstChambreMedine.Select(n => n.PelerinsMedine.Count).Sum()).ToString();
            }
        }

        protected void ChambreRepeaterMakkah_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string occupants = string.Empty;

                Chambre chambre = (Chambre)e.Item.DataItem;
                LinkButton chambreLink = (LinkButton)e.Item.FindControl("ChambreLink");

                chambreLink.Attributes.Add("onClick", "return false;");
                IList<VOR.Core.Domain.Pelerin> lstPelerin = null;
                Label label = (Label)chambreLink.FindControl("Capacite");

                lstPelerin = chambre.PelerinsMakkah;
                label.Text = chambre.OccupantMakkah;

                if (chambre.PelerinsMakkah.Count == chambre.TypeChambre.Code)
                {
                    chambreLink.BackColor = System.Drawing.ColorTranslator.FromHtml("#d10e0e");
                    label.ForeColor = Color.White;
                }
                else
                    chambreLink.BackColor = System.Drawing.ColorTranslator.FromHtml("#09ba12");

                if (lstPelerin != null)
                {
                    if (lstPelerin.Count == 0)
                        occupants = "فارغة";
                    else
                        foreach (VOR.Core.Domain.Pelerin pelerin in lstPelerin)
                            occupants += string.Format("{0} {1} {2}", pelerin.NomArabe, pelerin.PrenomArabe, Environment.NewLine);

                    //label.ToolTip = occupants;
                    chambreLink.Attributes.Add("data-tooltip", occupants);
                }
            }
        }

        protected void ChambreRepeaterMedine_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string occupants = string.Empty;

                Chambre chambre = (Chambre)e.Item.DataItem;
                LinkButton chambreLink = (LinkButton)e.Item.FindControl("ChambreLink");

                chambreLink.Attributes.Add("onClick", "return false;");
                IList<VOR.Core.Domain.Pelerin> lstPelerin = null;
                Label label = (Label)chambreLink.FindControl("Capacite");

                lstPelerin = chambre.PelerinsMedine;
                label.Text = chambre.OccupantMedine;
                if (chambre.PelerinsMedine.Count == chambre.TypeChambre.Code)
                {
                    chambreLink.BackColor = System.Drawing.ColorTranslator.FromHtml("#d10e0e");
                    label.ForeColor = Color.White;
                }
                else
                    chambreLink.BackColor = System.Drawing.ColorTranslator.FromHtml("#09ba12");

                if (lstPelerin != null)
                {
                    if (lstPelerin.Count == 0)
                        occupants = "فارغة";
                    else
                        foreach (VOR.Core.Domain.Pelerin pelerin in lstPelerin)
                            occupants += string.Format("{0} {1} {2}", pelerin.NomArabe, pelerin.PrenomArabe, Environment.NewLine);

                    //label.ToolTip = occupants;
                    chambreLink.Attributes.Add("data-tooltip", occupants);
                }
            }
        }

        protected void img_btn_search_Click(object sender, ImageClickEventArgs e)
        {
            BindEventsRepeater();
        }

        #endregion

        #region Private 

        private void BindEventsRepeater()
        {
            EventRepeater.DataSource = Global.Container.Resolve<EvenementModel>().GetEvenementsEnCours().OrderByDescending(n => n.DateDebut).Where(n => this._eventsIds.Contains(n.ID));
            EventRepeater.DataBind();
        }

        private void BindEvents()
        {
            this.ddlEvents.Items.Clear();

            RadComboBoxItem rcb;
            IList<VOR.Core.Domain.Evenement> lstEvents = Global.Container.Resolve<EvenementModel>().GetEvenementsEnCours();

            foreach (VOR.Core.Domain.Evenement eve in lstEvents)
            {
                rcb = new RadComboBoxItem();
                rcb.Text = eve.Nom;
                rcb.Value = eve.ID.ToString();
                rcb.Checked = true;
                this.ddlEvents.Items.Add(rcb);
            }
            BindTypeChambre();
        }

        private void BindTypeChambre()
        {
            this.ddlTypesChambre.Items.Clear();

            RadComboBoxItem rcb;
            IList<Chambre> lstChambre = Global.Container.Resolve<ChambreModel>().GetChambreByListEventIDAndVilleID(this._eventsIds, null).ToList();
            IList<TypeChambre> lstTypesChambre = lstChambre.Select(n => n.TypeChambre).Distinct().ToList();

            foreach (TypeChambre typeChambre in lstTypesChambre)
            {
                rcb = new RadComboBoxItem();
                rcb.Text = typeChambre.Nom;
                rcb.Value = typeChambre.ID.ToString();
                rcb.Checked = true;
                this.ddlTypesChambre.Items.Add(rcb);
            }
        }

        private void BindHotels()
        {
            this.ddlHotels.Items.Clear();

            RadComboBoxItem rcb;
            IList<Chambre> lstChambre = Global.Container.Resolve<ChambreModel>().GetChambreByListEventIDAndVilleID(this._eventsIds, null).ToList();
            IList<Hotel> lstHotels = lstChambre.Select(n => n.Hotel).Distinct().ToList();

            foreach (Hotel hotel in lstHotels)
            {
                rcb = new RadComboBoxItem();
                rcb.Text = hotel.NomFr;
                rcb.Value = hotel.ID.ToString();
                rcb.Checked = true;
                this.ddlHotels.Items.Add(rcb);
            }
        }

        private void RunScript(string script)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        #endregion
    }
}