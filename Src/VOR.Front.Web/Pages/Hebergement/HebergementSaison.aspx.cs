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
    public partial class HebergementSaison : BasePage
    {

        #region Properties

        public int? VilleID
        {
            get
            {
                int id;

                if (ViewState["Id"] != null && int.TryParse(ViewState["Id"].ToString(), out id))
                    return id;
                else if (int.TryParse(Request.QueryString["Id"], out id))
                    return id;
                else
                    return null;
            }
            set
            {
                ViewState["Id"] = value.ToString();
            }
        }

        #endregion

        #region private 

        private int _currentChambreID;

        List<int> lstEventIds = new List<int>();

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
                this.BinDdlEvenement();
                this.BindStatutChambre();
                this.BinDdlAgence();
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            BindAll();
        }

        protected void PelerinListView_NeedDataSource(object sender, RadListViewNeedDataSourceEventArgs e)
        {
            if (this._currentChambreID > 0)
                this.PelerinListView.DataSource = Global.Container.Resolve<PelerinModel>().GetPelerinByEventIDAndAgenceID(null, null, this._currentChambreID);
            else
            {
                BindPelerins();
            }

        }

        protected void PelerinListView_ItemDrop(object sender, RadListViewItemDragDropEventArgs e)
        {
            if (e.DestinationHtmlElement.IndexOf("ChambreLink") < 0)
            {
                return;
            }

            foreach (RepeaterItem item in ChambreRepeater.Items)
            {
                LinkButton chambreLink = item.FindControl("ChambreLink") as LinkButton;

                if (chambreLink != null && chambreLink.ClientID == e.DestinationHtmlElement)
                {
                    VOR.Core.Domain.Pelerin pelerin = Global.Container.Resolve<PelerinModel>().LoadByID((int)e.DraggedItem.GetDataKeyValue("ID"));

                    Chambre chambre = null;
                    if (int.Parse(chambreLink.CommandArgument.ToString()) != 0)
                        chambre = Global.Container.Resolve<ChambreModel>().LoadByID(int.Parse(chambreLink.CommandArgument.ToString()));

                    string script = string.Empty;

                    if (chambre != null)
                    {
                        IList<VOR.Core.Domain.Pelerin> lstPelerin = null;
                        if (this.VilleID == (int)EnumVille.MAKKAH)
                            lstPelerin = chambre.PelerinsMakkah;
                        else
                            lstPelerin = chambre.PelerinsMedine;

                        if (lstPelerin != null && chambre.TypeChambre.Code == lstPelerin.Count)
                        {
                            var msg = "Max de pelerins est atteint pour cette chambre. Veuillez choisir une autre chambre";
                            script = "DisplayMsgError('" + msg.ToJSFormat() + "');";
                            RunScript(script);
                            return;
                        }
                    }

                    if (this.VilleID == (int)EnumVille.MAKKAH)
                        pelerin.ChambreMakkah = chambre;
                    else if (this.VilleID == (int)EnumVille.MEDINE)
                        pelerin.ChambreMedine = chambre;

                    Global.Container.Resolve<PelerinModel>().Update(pelerin);
                    script = "ChambreClick('" + chambreLink.ClientID.ToJSFormat() + "');";
                    RunScript(script);



                    PelerinListView.Rebind();
                    BindStatutChambre();
                    BindAll();
                    break;
                }
            }
        }

        protected void PelerinListView_ItemDataBound(object sender, RadListViewItemEventArgs e)
        {
            if (e.Item.ItemType == RadListViewItemType.DataItem || e.Item.ItemType == RadListViewItemType.AlternatingItem)
            {
                RadListViewDataItem dataItem = (RadListViewDataItem)e.Item;
                VuePelerin vuePelerin = (VuePelerin)dataItem.DataItem;


                // Alerte
                ImageButton _btnAlert = (ImageButton)e.Item.FindControl("_btnAlert");
                if (vuePelerin.Alert != null)
                {
                    _btnAlert.Visible = true;
                    _btnAlert.ToolTip = vuePelerin.Alert;
                }
                else
                    _btnAlert.Visible = false;


                // Pelerin

                Panel pnlPelerin = (Panel)e.Item.FindControl("pnlPelerin");
                Label chambreDetail = (Label)pnlPelerin.FindControl("chambreDetail");
                Label vol = (Label)pnlPelerin.FindControl("vol");
                Label volDetail = (Label)pnlPelerin.FindControl("volDetail");
                Label infoPelerin = (Label)pnlPelerin.FindControl("infoPelerin");
                ImageButton imgRestaurant = (ImageButton)e.Item.FindControl("ImgRestaurant");

                // Chambre
                chambreDetail.Text = string.Format("{0}-{1}", vuePelerin.CodeTypeProgramme, vuePelerin.CodeChambre);

                // Vol
                vol.Text = string.Format("{0} - {1}", vuePelerin.LieuDepart, vuePelerin.LieuArrivee);

                // VolDetail
                volDetail.Text = string.Format("{0} - {1} | {2} - {3}",
                vuePelerin.HeureDepart.HasValue ? vuePelerin.HeureDepart.Value.ToString("dd/MM") : "dd/MM",
                vuePelerin.HeureDepart.HasValue ? vuePelerin.HeureDepart.Value.ToString("hh:mm") : "hh:mm",
                vuePelerin.HeureArrivee.HasValue ? vuePelerin.HeureArrivee.Value.ToString("dd/MM") : "dd/MM",
                vuePelerin.HeureArrivee.HasValue ? vuePelerin.HeureArrivee.Value.ToString("hh:mm") : "hh:mm");

                // InfoPelerin
                DateTime now = DateTime.Today;
                int age = now.Year - vuePelerin.DateNaissance.Year;
                if (now < vuePelerin.DateNaissance.AddYears(age)) age--;
                string nomPrenom = string.Format("{0} {1}", vuePelerin.NomArabe, vuePelerin.PrenomArabe);
                infoPelerin.Text = string.Format("{0} - {1}", nomPrenom, age.ToString());

                string imgSrc = "~/images/imagesCommunes/alien.png";
                if (vuePelerin.Photo != null)
                {
                    string imgData = Convert.ToBase64String(vuePelerin.Photo);
                    imgSrc = string.Format("data:image/png;base64,{0}", imgData);
                }
                HtmlImage _imgPhotoPerson = (HtmlImage)e.Item.FindControl("_imgPhotoPerson");
                _imgPhotoPerson.Src = imgSrc;

                Label pelerinLblAgence = (Label)pnlPelerin.FindControl("pelerinLblAgence");
                if (this.AgenceID == (int)EnumAgence.SALAALJADIDA)
                {
                    pelerinLblAgence.Text = vuePelerin.Agence;
                    pelerinLblAgence.Style.Add("display", "block");
                }
                else
                    pelerinLblAgence.Style.Add("display", "none");

                if (vuePelerin.RepasPaye.Equals("OUI"))
                    imgRestaurant.Visible = true;
                else
                    imgRestaurant.Visible = false;

                var panel = dataItem.FindControl("pnlPelerin") as Panel;
                panel.BackColor = ColorTranslator.FromHtml(vuePelerin.Couleur);
            }
        }

        protected void ChambreRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                IList<VOR.Core.Domain.Pelerin> lstPelerin = null;

                Chambre chambre = (Chambre)e.Item.DataItem;
                LinkButton genreLink = (LinkButton)e.Item.FindControl("ChambreLink");
                ImageButton btnSupprimer = (ImageButton)e.Item.FindControl("_btnSupprimer");
                Label chambreDetail = (Label)e.Item.FindControl("ChambreDetail");
                Label numeroChambre = (Label)e.Item.FindControl("numeroChambre");
                Panel header = (Panel)e.Item.FindControl("Header");
                Panel pnlEdit = (Panel)e.Item.FindControl("pnlEdit");

                if (chambre.Evenement != null)
                    header.BackColor = ColorTranslator.FromHtml(chambre.Evenement.Couleur);

                genreLink.Style.Add("color", "#ffffff");

                string ocuppants = string.Empty;
                if (this.VilleID == (int)EnumVille.MAKKAH)
                {
                    lstPelerin = chambre.PelerinsMakkah;
                    ocuppants = chambre.OccupantMakkah;

                    if (!chambre.Nom.Equals("SANS"))
                    {
                        if (chambre.PelerinsMakkah.Count == chambre.TypeChambre.Code)
                            genreLink.Style.Add("background-color", "#ff3f3f");
                        else if (chambre.PelerinsMakkah.Count == 0)
                            genreLink.Style.Add("background-color", "#56c605");
                        else
                            genreLink.Style.Add("background-color", "#ff9721");
                    }
                }
                else
                {
                    lstPelerin = chambre.PelerinsMedine;
                    ocuppants = chambre.OccupantMedine;

                    if (!chambre.Nom.Equals("SANS"))
                    {
                        if (chambre.PelerinsMedine.Count == chambre.TypeChambre.Code)
                            genreLink.Style.Add("background-color", "#ff3f3f");
                        else if (chambre.PelerinsMedine.Count == 0)
                            genreLink.Style.Add("background-color", "#56c605");
                        else
                            genreLink.Style.Add("background-color", "#ff9721");
                    }
                }


                if (lstPelerin != null)
                {
                    HtmlGenericControl br = new HtmlGenericControl();
                    br.TagName = "br";
                    genreLink.Controls.Add(br);
                    foreach (VOR.Core.Domain.Pelerin pelerin in lstPelerin)
                    {
                        string lblAlerte = string.Empty;

                        Panel pnl = new Panel();
                        pnl.Style.Add("text-align", "left");
                        pnl.Style.Add("margin", "2px");
                        pnl.Style.Add("margin-bottom", "7px");

                        Label lblPelerin = new Label();

                        string depart = string.Format("{0} ( {1} - {2} )", pelerin.Evenement.Pnr.LieuDepart.Code,
                          pelerin.Evenement.Pnr.HeureDepart.HasValue ? pelerin.Evenement.Pnr.HeureDepart.Value.ToString("dd/MM") : "dd/MM",
                          pelerin.Evenement.Pnr.HeureDepart.HasValue ? pelerin.Evenement.Pnr.HeureDepart.Value.ToString("hh:mm") : "hh:mm");

                        string arrivee = string.Format("{0} ( {1} - {2} )", pelerin.Evenement.Pnr.LieuArrivee.Code,
                          pelerin.Evenement.Pnr.HeureArrivee.HasValue ? pelerin.Evenement.Pnr.HeureArrivee.Value.ToString("dd/MM") : "dd/MM",
                          pelerin.Evenement.Pnr.HeureArrivee.HasValue ? pelerin.Evenement.Pnr.HeureArrivee.Value.ToString("hh:mm") : "hh:mm");

                        string volPelerin = string.Format("{0} | {1}", depart, arrivee);

                        if (this.AgenceID == (int)EnumAgence.SALAALJADIDA)
                        {
                            Label lblAgence = new Label();
                            lblAgence.Text = pelerin.Agence.Alias;
                            lblAgence.Style.Add("border", "1px solid");
                            lblAgence.Style.Add("padding", "2px");
                            lblAgence.Style.Add("background-color", "#fff");
                            lblAgence.Style.Add("color", "#000");
                            lblAgence.Style.Add("margin", "2px");
                            pnl.Controls.Add(lblAgence);
                        }

                        Label lblProgChambre = new Label();
                        lblProgChambre.Text = pelerin.Programme.TypeProgramme.Code + "-" + pelerin.TypeChambre.Code;
                        lblProgChambre.Style.Add("border", "1px solid");
                        lblProgChambre.Style.Add("padding", "2px");
                        lblProgChambre.Style.Add("background-color", "#fff");
                        lblProgChambre.Style.Add("color", "#000");
                        lblProgChambre.Style.Add("margin", "1px");
                        pnl.Controls.Add(lblProgChambre);

                        if (pelerin.RepasPaye)
                        {
                            ImageButton pictoRestaurant = new ImageButton();
                            pictoRestaurant.ImageUrl = "~/Images/imagesBack/pictoRestaurant.png";
                            pictoRestaurant.Width = Unit.Pixel(20);
                            pictoRestaurant.Height = Unit.Pixel(20);
                            pictoRestaurant.Style.Add("border", "1px solid");
                            pictoRestaurant.Style.Add("padding", "2px");
                            pictoRestaurant.Style.Add("background-color", "#fff");
                            pictoRestaurant.Style.Add("position", "relative");
                            pictoRestaurant.Style.Add("top", "7px");
                            pictoRestaurant.Style.Add("margin", "1px");
                            pnl.Controls.Add(pictoRestaurant);
                        }


                        if (chambre.Evenement.Pnr.ID != pelerin.Evenement.Pnr.ID)
                            lblPelerin.Text = "&nbsp;" + string.Format("{0} {1} {2}", volPelerin, pelerin.NomArabe, pelerin.PrenomArabe);
                        else
                            lblPelerin.Text = "&nbsp;" + string.Format("&nbsp;{0} {1}", pelerin.NomArabe, pelerin.PrenomArabe);

                        string hotels = string.Empty;
                        foreach (TPelerinHotel pHotel in pelerin.Hotels)
                        {
                            hotels += string.Format("{0} {1}", pHotel.Hotel.Nom, Environment.NewLine);
                        }

                        lblPelerin.Attributes.Add("data-tooltip", hotels);
                        lblPelerin.Style.Add("border", "1px solid");
                        lblPelerin.Style.Add("padding", "2px");
                        lblPelerin.Style.Add("background-color", "#fff");
                        lblPelerin.Style.Add("color", "#000");
                        lblPelerin.Style.Add("position", "relative");

                        pnl.Controls.Add(lblPelerin);
                        if (pelerin.Alerte != null)
                        {
                            ImageButton alert = new ImageButton();
                            alert.ImageUrl = "~/Images/imagesBack/exclamationMark.png";
                            alert.Width = Unit.Pixel(12);
                            alert.Height = Unit.Pixel(12);
                            alert.ToolTip = pelerin.Alerte.Libelle;
                            alert.Attributes.Add("class", "exclamation");
                            pnl.Controls.Add(alert);
                        }

                        genreLink.Controls.Add(pnl);
                    }
                }

                genreLink.BackColor = ColorTranslator.FromHtml(chambre.Couleur);

                if (!chambre.Nom.Equals("SANS"))
                {
                    Panel pnlVol = (Panel)e.Item.FindControl("Vol");
                    string lieuDepart = chambre.Evenement.Pnr.LieuDepart.Code;
                    string lieuArrivee = chambre.Evenement.Pnr.LieuArrivee.Code;
                    string heureDepart = string.Format("{0} - {1}", chambre.Evenement.Pnr.HeureDepart.HasValue ? chambre.Evenement.Pnr.HeureDepart.Value.ToString("dd/MM") : "dd/MM",
                        chambre.Evenement.Pnr.HeureDepart.HasValue ? chambre.Evenement.Pnr.HeureDepart.Value.ToString("hh:mm") : "hh:mm");
                    string heureArrivee = string.Format("{0} - {1}", chambre.Evenement.Pnr.HeureArrivee.HasValue ? chambre.Evenement.Pnr.HeureArrivee.Value.ToString("dd/MM") : "dd/MM",
                        chambre.Evenement.Pnr.HeureArrivee.HasValue ? chambre.Evenement.Pnr.HeureArrivee.Value.ToString("hh:mm") : "hh:mm");

                    HtmlTable ht = new HtmlTable();

                    HtmlTableRow tr1 = new HtmlTableRow();
                    HtmlTableCell td1 = new HtmlTableCell();
                    td1.InnerText = lieuDepart;
                    HtmlTableCell td2 = new HtmlTableCell();
                    td2.InnerText = " --> ";
                    HtmlTableCell td3 = new HtmlTableCell();
                    td3.InnerText = heureDepart;

                    tr1.Controls.Add(td1);
                    tr1.Controls.Add(td2);
                    tr1.Controls.Add(td3);

                    HtmlTableRow tr2 = new HtmlTableRow();
                    HtmlTableCell td4 = new HtmlTableCell();
                    td4.InnerText = lieuArrivee;
                    HtmlTableCell td5 = new HtmlTableCell();
                    td5.InnerText = " --> ";
                    HtmlTableCell td6 = new HtmlTableCell();
                    td6.InnerText = heureArrivee;
                    tr2.Controls.Add(td4);
                    tr2.Controls.Add(td5);
                    tr2.Controls.Add(td6);


                    ht.Controls.Add(tr1);
                    ht.Controls.Add(tr2);
                    ht.Style.Add("border", "1px solid");
                    ht.Style.Add("background-color", "#fff");
                    ht.Style.Add("width", "100%");
                    pnlVol.Controls.Add(ht);

                    chambreDetail.Text = string.Format("{0} ({1})", chambre.TypeChambre.Nom, ocuppants);
                }


                string pageUrl = "~/Pages/Evenement/Edit/GestionChambre.aspx";
                string url = ResolveUrl(string.Format("{0}?RenderMode=popin&Id={1}", pageUrl, chambre.ID));
                string popupTitle = "Chambre";
                string myRadWindow = string.Format("return OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);

                var btnEdit = (HyperLink)e.Item.FindControl("_btnEdit");
                btnEdit.NavigateUrl = "#";
                btnEdit.Attributes["onclick"] = myRadWindow;

                if (string.IsNullOrEmpty(chambre.Numero))
                    numeroChambre.Visible = false;
                else
                    numeroChambre.Visible = true;

                //var pnlOcuppied = (Panel)e.Item.FindControl("pnlOcuppied");

                //if(chambre.Occupe) { 
                //    pnlOcuppied.Style.Add("background-color", "#ff3f3f");
                //    pnlOcuppied.ToolTip = "Actuellement ocuppée";
                //}
                //else { 
                //    pnlOcuppied.Style.Add("background-color", "#56c605");
                //    pnlOcuppied.ToolTip = "Actuellement vide";
                //}


                if (chambre.Nom.Equals("SANS"))
                {
                    btnEdit.Visible = false;
                    //pnlOcuppied.Visible = false;
                    pnlEdit.Visible = false;
                    genreLink.Style.Add("top", "10px");
                    genreLink.Style.Add("padding-bottom", "36px");
                    genreLink.Style.Add("background-image", "/Images/imagesBack/room.png");
                    genreLink.Style.Add("background-position", "center");
                    genreLink.Style.Add("background-repeat", "no-repeat");
                    header.Style.Add("visibility", "hidden");

                    Panel pnlChLink = (Panel)e.Item.FindControl("pnlChLink");
                    pnlChLink.Style.Add("position", "relative");
                    pnlChLink.Style.Add("position", "relative");
                    pnlChLink.Style.Add("border", "4px solid");
                    pnlChLink.Style.Add("height", "154px");
                    pnlChLink.Style.Add("top", "-58px");
                }

                if (chambre.Agence != null)
                {
                    Label chLblAgence = (Label)genreLink.FindControl("chLblAgence");
                    if (this.AgenceID == (int)EnumAgence.SALAALJADIDA)
                    {
                        chLblAgence.Text = chambre.Agence.Alias;
                        chLblAgence.Style.Add("display", "block");
                    }
                    else
                        chLblAgence.Style.Add("display", "none");
                }
            }
        }

        protected void _btnSupprimer_Command(object sender, CommandEventArgs e)
        {
            try
            {

                IList<int> lstIdPelerin = null;

                int chambreID = int.Parse(e.CommandArgument.ToString());

                Chambre chambre = Global.Container.Resolve<ChambreModel>().GetByID(chambreID);

                if (this.VilleID == (int)EnumVille.MAKKAH)
                {
                    if (chambre.PelerinsMakkah != null && chambre.PelerinsMakkah.Count > 0)
                    {
                        lstIdPelerin = chambre.PelerinsMakkah.Select(n => n.ID).ToList();
                        Global.Container.Resolve<ChambreModel>().DeletePelerinsChambreMakkah(lstIdPelerin);
                    }
                }
                else
                {
                    if (chambre.PelerinsMedine != null && chambre.PelerinsMedine.Count > 0)
                    {
                        lstIdPelerin = chambre.PelerinsMedine.Select(n => n.ID).ToList();
                        Global.Container.Resolve<ChambreModel>().DeletePelerinsChambreMedine(lstIdPelerin);
                    }
                }

                Global.Container.Resolve<ChambreModel>().Delete(chambre);
                PelerinListView.Rebind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ChambreLink_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            this._currentChambreID = int.Parse(btn.CommandArgument);
            this.PelerinListView.Rebind();
        }

        #endregion

        #region Private 

        private void BindAll()
        {
            int? agenceID = null;

            List<Chambre> lstChambre = new List<Chambre>();
            lstChambre.Add(new Chambre
            {
                ID = 0,
                Nom = "SANS",
            });

            List<Chambre> lstChambreDB = Global.Container.Resolve<ChambreModel>().GetChambreByEventEnCoursAndVilleID(this.VilleID.Value).OrderBy(n => n.Evenement.DateDebut).ThenBy(n => n.TypeChambre.Code).ToList();
            List<Chambre> lstChambreForStats = lstChambreDB;
            IList<TypeChambre> lstTypeChambre = lstChambreDB.Select(n => n.TypeChambre).GroupBy(n => n.Nom).Select(grp => grp.First()).OrderBy(n => n.Code).ToList();
            IList<Hotel> lstHotel = lstChambreDB.Select(n => n.Hotel).GroupBy(n => n.Nom).Select(grp => grp.First()).OrderBy(n => n.NomFr).ToList();
            IList<Chambre> lstNumeroChambre = lstChambreDB.Select(n => n).GroupBy(n => n.Numero).Select(grp => grp.First()).OrderBy(n => n.Numero).ToList();

            if (!string.IsNullOrEmpty(this._ddlTypeChambre.SelectedValue))
                lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.TypeChambre.Code == int.Parse(this._ddlTypeChambre.SelectedValue)).ToList();

            if (this._ddlHotels.SelectedValue != "-1")
            {
                int hotelID = int.Parse(this._ddlHotels.SelectedValue);
                lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.Hotel.ID == hotelID).ToList();
                lstNumeroChambre = lstNumeroChambre.Select(n => n).Where(n => n.Hotel.ID == hotelID).ToList();
            }

            if (this._ddlNumeroChambre.SelectedValue != "-1")
                lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.Numero == this._ddlNumeroChambre.SelectedValue).ToList();

            if (this._ddlOccupe.SelectedValue != "-1")
            {
                if (this._ddlOccupe.SelectedValue == "0")
                    lstChambreDB = lstChambreDB.Select(n => n).Where(n => !n.Occupe).ToList();
                else
                    lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.Occupe).ToList();
            }


            if (!string.IsNullOrEmpty(this._ddlStatutChambre.SelectedValue))
            {
                if (this._ddlStatutChambre.SelectedValue == "1")
                {
                    if (this.VilleID == (int)EnumVille.MAKKAH)
                        lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.PelerinsMakkah.Count() == n.TypeChambre.Code).ToList();
                    else
                        lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.PelerinsMedine.Count() == n.TypeChambre.Code).ToList();
                }

                if (this._ddlStatutChambre.SelectedValue == "2")
                {
                    if (this.VilleID == (int)EnumVille.MAKKAH)
                        lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.PelerinsMakkah.Count() != 0 && n.PelerinsMakkah.Count() < n.TypeChambre.Code).ToList();
                    else
                        lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.PelerinsMedine.Count() != 0 && n.PelerinsMedine.Count() < n.TypeChambre.Code).ToList();
                }

                if (this._ddlStatutChambre.SelectedValue == "3")
                {
                    if (this.VilleID == (int)EnumVille.MAKKAH)
                        lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.PelerinsMakkah.Count() == 0).ToList();
                    else
                        lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.PelerinsMedine.Count() == 0).ToList();
                }
            }

            var events = this.ddlEvents.CheckedItems;

            foreach (var item in events)
            {
                lstEventIds.Add(int.Parse(item.Value));
            }

            if(lstEventIds.Any())
            {
                if (cbxFilterEventChambre.Checked)
                    lstChambreDB = lstChambreDB.Select(n => n).Where(n => lstEventIds.Contains(n.Evenement.ID)).ToList();

                lstEventIds = cbxFilterEventPelerin.Checked ? lstEventIds : null;
            }

            //if (!string.IsNullOrEmpty(this._ddlEvenement.SelectedValue))
            //{

            //    eventID = int.Parse(this._ddlEvenement.SelectedValue);

            //    if (cbxFilterEventChambre.Checked)
            //        lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.Evenement.ID == eventID).ToList();

            //    //VOR.Core.Domain.Evenement evenement = Global.Container.Resolve<EvenementModel>().LoadByID((int)eventID);
            //    //VOR.Core.Domain.Pnr pnr = Global.Container.Resolve<PnrModel>().LoadByID(evenement.Pnr.ID);
            //    //IList<VuePelerin> lstPelerin = Global.Container.Resolve<PelerinModel>().GetPelerinByEventIDAndAgenceID(evenement.ID, null);
            //    //int nbrPelerins = lstPelerin.Count;

            //    //this.lblNbrPelerins.Text = string.Format("{0} / {1}", nbrPelerins, pnr.NbrPassager);
            //    //this.lblNbrPelerins.Visible = true;

            //    eventID = cbxFilterEventPelerin.Checked ? eventID : null;
            //}
            //else
            //{
            //    this.lblNbrPelerins.Text = string.Empty;
            //    this.lblNbrPelerins.Visible = false;
            //}

            if (!string.IsNullOrEmpty(this._ddlAgence.SelectedValue))
            {
                agenceID = int.Parse(this._ddlAgence.SelectedValue);

                if(cbxFilterAgenceChambre.Checked)
                    lstChambreDB = lstChambreDB.Select(n => n).Where(n => n.Agence.ID == agenceID).ToList();

                agenceID = cbxFilterAgencePelerin.Checked ? agenceID : null;
            }

            lstChambre.AddRange(lstChambreDB);
            ChambreRepeater.DataSource = lstChambre;
            ChambreRepeater.DataBind();
            if (this._currentChambreID == 0)
                BindPelerins(lstEventIds, agenceID);

            BindDdlTypeChambre(lstTypeChambre, this._ddlTypeChambre.SelectedValue);
            BindDdlNumeroChambre(lstNumeroChambre, this._ddlNumeroChambre.SelectedValue);
            BinDdlHotels(lstHotel, this._ddlHotels.SelectedValue);

            Statistiques(lstChambreDB.Select(n => n).Where(n => n.Nom != "SANS").ToList());
        }

        private void Statistiques(List<Chambre> lstChambre)
        {
            IList<int> lstCodeTypeChambre = lstChambre.Select(n => n.TypeChambre.Code).Distinct().OrderBy(n => n).ToList();
            int nbrOcuppesA2 = 0;
            int nbrOuvertesA2 = 0;
            int nbrVidesA2 = 0;

            int nbrOcuppesA3 = 0;
            int nbrOuvertesA3 = 0;
            int nbrVidesA3 = 0;

            int nbrOcuppesA4 = 0;
            int nbrOuvertesA4 = 0;
            int nbrVidesA4 = 0;

            int nbrOcuppesA5 = 0;
            int nbrOuvertesA5 = 0;
            int nbrVidesA5 = 0;

            int nbrOcuppesA6 = 0;
            int nbrOuvertesA6 = 0;
            int nbrVidesA6 = 0;

            int nbrOcuppesA7 = 0;
            int nbrOuvertesA7 = 0;
            int nbrVidesA7 = 0;

            int nbrOcuppesA8 = 0;
            int nbrOuvertesA8 = 0;
            int nbrVidesA8 = 0;

            int nbrOcuppesA9 = 0;
            int nbrOuvertesA9 = 0;
            int nbrVidesA9 = 0;

            int nbrOcuppesA10 = 0;
            int nbrOuvertesA10 = 0;
            int nbrVidesA10 = 0;

            foreach (int code in lstCodeTypeChambre)
            {
                switch (code)
                {
                    case 2:
                        List<Chambre> lstChambreA2 = lstChambre.Select(n => n).Where(n => n.TypeChambre.Code == 2).ToList();
                        if (lstChambreA2.Count != 0)
                        {
                            foreach (Chambre chambre in lstChambreA2)
                            {
                                IList<Core.Domain.Pelerin> lstPelerin = this.VilleID.Value == 1 ? chambre.PelerinsMakkah : chambre.PelerinsMedine;
                                if (lstPelerin.Count == chambre.TypeChambre.Code)
                                    nbrOcuppesA2++;
                                else if (lstPelerin.Count == 0)
                                    nbrVidesA2++;
                                else
                                    nbrOuvertesA2++;
                            }

                            this.lblChambreA2.Text = string.Format("Ch. {0}", lstChambreA2.Select(n => n.TypeChambre).FirstOrDefault().Nom);
                            this.lblNbrOcuppesA2.Text = string.Format("Ocuppée: {0}", nbrOcuppesA2);
                            this.lblNbrOuvertesA2.Text = string.Format("Ouvertes: {0}", nbrOuvertesA2);
                            this.lblNbrVidesA2.Text = string.Format("Vides: {0}", nbrVidesA2);
                            this.lblNbrTotalA2.Text = (nbrOcuppesA2 + nbrOuvertesA2 + nbrVidesA2).ToString();
                            this.pnlChambreA2.Visible = true;
                        }
                        break;
                    case 3:
                        List<Chambre> lstChambreA3 = lstChambre.Select(n => n).Where(n => n.TypeChambre.Code == 3).ToList();
                        if (lstChambreA3.Count != 0)
                        {
                            foreach (Chambre chambre in lstChambreA3)
                            {
                                IList<Core.Domain.Pelerin> lstPelerin = this.VilleID.Value == 1 ? chambre.PelerinsMakkah : chambre.PelerinsMedine;
                                if (lstPelerin.Count == chambre.TypeChambre.Code)
                                    nbrOcuppesA3++;
                                else if (lstPelerin.Count == 0)
                                    nbrVidesA3++;
                                else
                                    nbrOuvertesA3++;
                            }

                            this.lblChambreA3.Text = string.Format("Ch. {0}", lstChambreA3.Select(n => n.TypeChambre).FirstOrDefault().Nom);
                            this.lblNbrOcuppesA3.Text = string.Format("Ocuppée: {0}", nbrOcuppesA3);
                            this.lblNbrOuvertesA3.Text = string.Format("Ouvertes: {0}", nbrOuvertesA3);
                            this.lblNbrVidesA3.Text = string.Format("Vides: {0}", nbrVidesA3);
                            this.lblNbrTotalA3.Text = (nbrOcuppesA3 + nbrOuvertesA3 + nbrVidesA3).ToString();
                            this.pnlChambreA3.Visible = true;
                        }

                        break;
                    case 4:
                        List<Chambre> lstChambreA4 = lstChambre.Select(n => n).Where(n => n.TypeChambre.Code == 4).ToList();
                        if (lstChambreA4.Count != 0)
                        {
                            foreach (Chambre chambre in lstChambreA4)
                            {
                                IList<Core.Domain.Pelerin> lstPelerin = this.VilleID.Value == 1 ? chambre.PelerinsMakkah : chambre.PelerinsMedine;
                                if (lstPelerin.Count == chambre.TypeChambre.Code)
                                    nbrOcuppesA4++;
                                else if (lstPelerin.Count == 0)
                                    nbrVidesA4++;
                                else
                                    nbrOuvertesA4++;
                            }

                            this.lblChambreA4.Text = string.Format("Ch. {0}", lstChambreA4.Select(n => n.TypeChambre).FirstOrDefault().Nom);
                            this.lblNbrOcuppesA4.Text = string.Format("Ocuppée: {0}", nbrOcuppesA4);
                            this.lblNbrOuvertesA4.Text = string.Format("Ouvertes: {0}", nbrOuvertesA4);
                            this.lblNbrVidesA4.Text = string.Format("Vides: {0}", nbrVidesA4);
                            this.lblNbrTotalA4.Text = (nbrOcuppesA4 + nbrOuvertesA4 + nbrVidesA4).ToString();
                            this.pnlChambreA4.Visible = true;
                        }
                        break;
                    case 5:
                        List<Chambre> lstChambreA5 = lstChambre.Select(n => n).Where(n => n.TypeChambre.Code == 5).ToList();
                        if (lstChambreA5.Count != 0)
                        {
                            foreach (Chambre chambre in lstChambreA5)
                            {
                                IList<Core.Domain.Pelerin> lstPelerin = this.VilleID.Value == 1 ? chambre.PelerinsMakkah : chambre.PelerinsMedine;
                                if (lstPelerin.Count == chambre.TypeChambre.Code)
                                    nbrOcuppesA5++;
                                else if (lstPelerin.Count == 0)
                                    nbrVidesA5++;
                                else
                                    nbrOuvertesA5++;
                            }

                            this.lblChambreA5.Text = string.Format("Ch. {0}", lstChambreA5.Select(n => n.TypeChambre).FirstOrDefault().Nom);
                            this.lblNbrOcuppesA5.Text = string.Format("Ocuppée: {0}", nbrOcuppesA5);
                            this.lblNbrOuvertesA5.Text = string.Format("Ouvertes: {0}", nbrOuvertesA5);
                            this.lblNbrVidesA5.Text = string.Format("Vides: {0}", nbrVidesA5);
                            this.lblNbrTotalA5.Text = (nbrOcuppesA5 + nbrOuvertesA5 + nbrVidesA5).ToString();
                            this.pnlChambreA5.Visible = true;
                        }
                        break;
                    case 6:
                        List<Chambre> lstChambreA6 = lstChambre.Select(n => n).Where(n => n.TypeChambre.Code == 6).ToList();
                        if (lstChambreA6.Count != 0)
                        {
                            foreach (Chambre chambre in lstChambreA6)
                            {
                                IList<Core.Domain.Pelerin> lstPelerin = this.VilleID.Value == 1 ? chambre.PelerinsMakkah : chambre.PelerinsMedine;
                                if (lstPelerin.Count == chambre.TypeChambre.Code)
                                    nbrOcuppesA6++;
                                else if (lstPelerin.Count == 0)
                                    nbrVidesA6++;
                                else
                                    nbrOuvertesA6++;
                            }
                            this.lblChambreA6.Text = string.Format("Ch. {0}", lstChambreA6.Select(n => n.TypeChambre).FirstOrDefault().Nom);
                            this.lblNbrOcuppesA6.Text = string.Format("Ocuppée: {0}", nbrOcuppesA6);
                            this.lblNbrOuvertesA6.Text = string.Format("Ouvertes: {0}", nbrOuvertesA6);
                            this.lblNbrVidesA6.Text = string.Format("Vides: {0}", nbrVidesA6);
                            this.lblNbrTotalA6.Text = (nbrOcuppesA6 + nbrOuvertesA6 + nbrVidesA6).ToString();
                            this.pnlChambreA6.Visible = true;
                        }
                        break;
                    case 7:
                        List<Chambre> lstChambreA7 = lstChambre.Select(n => n).Where(n => n.TypeChambre.Code == 7).ToList();
                        if (lstChambreA7.Count != 0)
                        {
                            foreach (Chambre chambre in lstChambreA7)
                            {
                                IList<Core.Domain.Pelerin> lstPelerin = this.VilleID.Value == 1 ? chambre.PelerinsMakkah : chambre.PelerinsMedine;
                                if (lstPelerin.Count == chambre.TypeChambre.Code)
                                    nbrOcuppesA7++;
                                else if (lstPelerin.Count == 0)
                                    nbrVidesA7++;
                                else
                                    nbrOuvertesA7++;
                            }
                            this.lblChambreA7.Text = string.Format("Ch. {0}", lstChambreA7.Select(n => n.TypeChambre).FirstOrDefault().Nom);
                            this.lblNbrOcuppesA7.Text = string.Format("Ocuppée: {0}", nbrOcuppesA7);
                            this.lblNbrOuvertesA7.Text = string.Format("Ouvertes: {0}", nbrOuvertesA7);
                            this.lblNbrVidesA7.Text = string.Format("Vides: {0}", nbrVidesA7);
                            this.lblNbrTotalA7.Text = (nbrOcuppesA7 + nbrOuvertesA7 + nbrVidesA7).ToString();
                            this.pnlChambreA7.Visible = true;
                        }
                        break;
                    case 8:
                        List<Chambre> lstChambreA8 = lstChambre.Select(n => n).Where(n => n.TypeChambre.Code == 8).ToList();
                        if (lstChambreA8.Count != 0)
                        {
                            foreach (Chambre chambre in lstChambreA8)
                            {
                                IList<Core.Domain.Pelerin> lstPelerin = this.VilleID.Value == 1 ? chambre.PelerinsMakkah : chambre.PelerinsMedine;
                                if (lstPelerin.Count == chambre.TypeChambre.Code)
                                    nbrOcuppesA8++;
                                else if (lstPelerin.Count == 0)
                                    nbrVidesA8++;
                                else
                                    nbrOuvertesA8++;
                            }
                            this.lblChambreA8.Text = string.Format("Ch. {0}", lstChambreA8.Select(n => n.TypeChambre).FirstOrDefault().Nom);
                            this.lblNbrOcuppesA8.Text = string.Format("Ocuppée: {0}", nbrOcuppesA8);
                            this.lblNbrOuvertesA8.Text = string.Format("Ouvertes: {0}", nbrOuvertesA8);
                            this.lblNbrVidesA8.Text = string.Format("Vides: {0}", nbrVidesA8);
                            this.lblNbrTotalA8.Text = (nbrOcuppesA8 + nbrOuvertesA8 + nbrVidesA8).ToString();
                            this.pnlChambreA8.Visible = true;
                        }
                        break;

                    case 9:
                        List<Chambre> lstChambreA9 = lstChambre.Select(n => n).Where(n => n.TypeChambre.Code == 9).ToList();
                        if (lstChambreA9.Count != 0)
                        {
                            foreach (Chambre chambre in lstChambreA9)
                            {
                                IList<Core.Domain.Pelerin> lstPelerin = this.VilleID.Value == 1 ? chambre.PelerinsMakkah : chambre.PelerinsMedine;
                                if (lstPelerin.Count == chambre.TypeChambre.Code)
                                    nbrOcuppesA9++;
                                else if (lstPelerin.Count == 0)
                                    nbrVidesA9++;
                                else
                                    nbrOuvertesA9++;
                            }
                            this.lblChambreA9.Text = string.Format("Ch. {0}", lstChambreA9.Select(n => n.TypeChambre).FirstOrDefault().Nom);
                            this.lblNbrOcuppesA9.Text = string.Format("Ocuppée: {0}", nbrOcuppesA9);
                            this.lblNbrOuvertesA9.Text = string.Format("Ouvertes: {0}", nbrOuvertesA9);
                            this.lblNbrVidesA9.Text = string.Format("Vides: {0}", nbrVidesA9);
                            this.lblNbrTotalA9.Text = (nbrOcuppesA9 + nbrOuvertesA9 + nbrVidesA9).ToString();
                            this.pnlChambreA9.Visible = true;
                        }
                        break;

                    case 10:
                        List<Chambre> lstChambreA10 = lstChambre.Select(n => n).Where(n => n.TypeChambre.Code == 10).ToList();
                        if (lstChambreA10.Count != 0)
                        {
                            foreach (Chambre chambre in lstChambreA10)
                            {
                                IList<Core.Domain.Pelerin> lstPelerin = this.VilleID.Value == 1 ? chambre.PelerinsMakkah : chambre.PelerinsMedine;
                                if (lstPelerin.Count == chambre.TypeChambre.Code)
                                    nbrOcuppesA10++;
                                else if (lstPelerin.Count == 0)
                                    nbrVidesA10++;
                                else
                                    nbrOuvertesA10++;
                            }
                            this.lblChambreA10.Text = string.Format("Ch. {0}", lstChambreA10.Select(n => n.TypeChambre).FirstOrDefault().Nom);
                            this.lblNbrOcuppesA10.Text = string.Format("Ocuppée: {0}", nbrOcuppesA10);
                            this.lblNbrOuvertesA10.Text = string.Format("Ouvertes: {0}", nbrOuvertesA10);
                            this.lblNbrVidesA10.Text = string.Format("Vides: {0}", nbrVidesA10);
                            this.lblNbrTotalA10.Text = (nbrOcuppesA10 + nbrOuvertesA10 + nbrVidesA10).ToString();
                            this.pnlChambreA10.Visible = true;
                        }
                        break;
                }
            }

            int nbrTotalOcuppes = nbrOcuppesA2 + nbrOcuppesA3 + nbrOcuppesA4 + nbrOcuppesA5 + nbrOcuppesA6 + nbrOcuppesA7 + nbrOcuppesA8 + nbrOcuppesA9 + +nbrOcuppesA10;
            int nbrTotalOuvertes = nbrOuvertesA2 + nbrOuvertesA3 + nbrOuvertesA4 + nbrOuvertesA5 + nbrOuvertesA6 + nbrOuvertesA7 + nbrOuvertesA8 + nbrOuvertesA9 + nbrOuvertesA10;
            int nbrTotalVides = nbrVidesA2 + nbrVidesA3 + nbrVidesA4 + nbrVidesA5 + nbrVidesA6 + nbrVidesA7 + nbrVidesA8 + nbrVidesA9 + nbrVidesA10;
            this.lblNbrTotalOcuppes.Text = string.Format("Ocuppée: {0}", nbrTotalOcuppes);
            this.lblNbrTotalOuvertes.Text = string.Format("Ouvertes: {0}", nbrTotalOuvertes);
            this.lblNbrTotalVides.Text = string.Format("Vides: {0}", nbrTotalVides);
            this.lblNbrTotal.Text = (nbrTotalOcuppes + nbrTotalOuvertes + nbrTotalVides).ToString();

            int countTotalPelerin = 0;
            if(lstEventIds.Any())
                countTotalPelerin = Global.Container.Resolve<PelerinModel>().GetNbrPelerinsByEventIDs(lstEventIds);

            lblTotalPelerin.Text = countTotalPelerin.ToString();
            pnlTotalPelerin.Visible =  countTotalPelerin > 0 ? true : false;
        }

        private void BindPelerins(List<int> lstEventIds = null, int? agenceID = null)
        {
            if (this.VilleID == (int)EnumVille.MAKKAH)
                this.PelerinListView.DataSource = Global.Container.Resolve<PelerinModel>().GetPelerinSansMakkahChambreByEventEncours(lstEventIds, agenceID);
            else if (this.VilleID == (int)EnumVille.MEDINE)
                this.PelerinListView.DataSource = Global.Container.Resolve<PelerinModel>().GetPelerinSansMedineChambreByEventEncours(lstEventIds, agenceID);
        }

        private void RunScript(string script)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        #endregion

        private void BindDdlNumeroChambre(IList<Chambre> lstNumeroChambre, string value)
        {
            this._ddlNumeroChambre.Items.Clear();
            this._ddlNumeroChambre.Items.Add(new ListItem("--- Tous ---", "-1"));
            this._ddlNumeroChambre.DataSource = lstNumeroChambre.OrderBy(n => n.Numero);
            this._ddlNumeroChambre.DataTextField = "Numero";
            this._ddlNumeroChambre.DataValueField = "Numero";
            this._ddlNumeroChambre.DataBind();

            this._ddlNumeroChambre.SelectedValue = value;
        }

        private void BindDdlTypeChambre(IList<TypeChambre> lstTypeChambre, string value)
        {
            this._ddlTypeChambre.Items.Clear();
            this._ddlTypeChambre.Items.Add(new ListItem("--- Tous ---", ""));
            this._ddlTypeChambre.DataSource = lstTypeChambre;
            this._ddlTypeChambre.DataTextField = "Nom";
            this._ddlTypeChambre.DataValueField = "Code";
            this._ddlTypeChambre.DataBind();

            this._ddlTypeChambre.SelectedValue = value;
        }

        private void BinDdlEvenement()
        {
            var lstEvenement = Global.Container.Resolve<EvenementModel>().GetEvenementsEnCours();

            RadComboBoxItem rcb;
            foreach (Core.Domain.Evenement evenement in lstEvenement)
            {
                rcb = new RadComboBoxItem();
                rcb.Text = evenement.Nom;
                rcb.Value = evenement.ID.ToString();
                rcb.Checked = evenement.ID == this.EventID ? true : false;
                this.ddlEvents.Items.Add(rcb);
            }
        }

        private void BinDdlAgence()
        {
            this._ddlAgence.DataSource = Global.Container.Resolve<AgenceModel>().GetAll();
            this._ddlAgence.DataTextField = "Nom";
            this._ddlAgence.DataValueField = "ID";
            this._ddlAgence.DataBind();
        }

        private void BinDdlHotels(IList<Hotel> lstHotel, string value)
        {
            this._ddlHotels.Items.Clear();
            this._ddlHotels.Items.Add(new ListItem("--- Tous ---", "-1"));
            this._ddlHotels.DataSource = lstHotel;
            this._ddlHotels.DataTextField = "NomFr";
            this._ddlHotels.DataValueField = "ID";
            this._ddlHotels.DataBind();

            this._ddlHotels.SelectedValue = value;
        }

        private void BindStatutChambre()
        {
            this._ddlStatutChambre.Items.Clear();
            this._ddlStatutChambre.Items.Add(new ListItem("--- Tous ---", "-1"));
            this._ddlStatutChambre.Items.Add(new ListItem("Ocuppée", "1"));
            this._ddlStatutChambre.Items.Add(new ListItem("Ouvertes", "2"));
            this._ddlStatutChambre.Items.Add(new ListItem("Vides", "3"));
        }

        protected void _btnFilter_Click(object sender, EventArgs e)
        {
            BindAll();
        }
    }
}