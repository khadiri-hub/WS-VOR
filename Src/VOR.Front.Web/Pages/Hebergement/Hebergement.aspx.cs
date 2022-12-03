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
    public partial class Hebergement : BasePage
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

        public int? AgenceId
        {
            get
            {
                if (!_agenceID.HasValue)
                {
                    VOR.Core.Domain.Agence agence = Global.Container.Resolve<AgenceModel>().LoadByID(this.AgenceID);
                    _agenceID = agence.TypeAgence.ID == (int)EnumTypeAgence.Filial ? (int?)this.AgenceID : null;

                    return _agenceID;
                }
                else
                {
                    _agenceID = null;
                    return _agenceID;
                }
            }
        }
        private int? _agenceID;

        #endregion

        #region private 

        private int _currentChambreID;

        #endregion

        #region Events 

        protected void Page_Init(object sender, EventArgs e)
        {
            ((SiteMaster)this.Master).ActiveTab = MenuTopUC.Tab.Hebergement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager.GetCurrent(this.Page).ClientEvents.OnRequestStart = "onRequestStart";
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            BindChambresRepeater();
            Control ctrl = GetControlThatCausedPostBack(this.Page);
            if (ctrl != null && ctrl.ID.Equals("_ddlEvenement"))
                BindPelerins();
        }

        private Control GetControlThatCausedPostBack(Page page)
        {
            //initialize a control and set it to null
            Control ctrl = null;

            //get the event target name and find the control
            string ctrlName = page.Request.Params.Get("__EVENTTARGET");
            if (!String.IsNullOrEmpty(ctrlName))
                ctrl = page.FindControl(ctrlName);

            //return the control to the calling method
            return ctrl;
        }

        protected void PelerinListView_NeedDataSource(object sender, RadListViewNeedDataSourceEventArgs e)
        {
            if (this._currentChambreID > 0)
            {
                this.PelerinListView.DataSource = Global.Container.Resolve<PelerinModel>().GetPelerinByEventIDAndAgenceID(this.EventID, AgenceId, this._currentChambreID);
            }

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
                            var msg = "Nbr Max de pelerins est atteint pour cette chambre. Veuillez choisir une autre chambre";
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
                    BindChambresRepeater();
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

                Panel pnlPelerin = (Panel)e.Item.FindControl("pnlPelerin");
                Label chambreDetail = (Label)pnlPelerin.FindControl("chambreDetail");
                Label infoPelerin = (Label)pnlPelerin.FindControl("infoPelerin");
                ImageButton imgRestaurant = (ImageButton)e.Item.FindControl("ImgRestaurant");

                Label pelerinLblAgence = (Label)pnlPelerin.FindControl("pelerinLblAgence");
                if (this.AgenceID == (int)EnumAgence.SALAALJADIDA)
                {
                    pelerinLblAgence.Text = vuePelerin.Agence;
                    pelerinLblAgence.Style.Add("display", "block");
                }
                else
                    pelerinLblAgence.Style.Add("display", "none");


                // Chambre
                chambreDetail.Text = string.Format("{0}{1}", vuePelerin.CodeTypeProgramme, vuePelerin.CodeChambre);

                // InfoPelerin
                DateTime now = DateTime.Today;
                int age = now.Year - vuePelerin.DateNaissance.Year;
                if (now < vuePelerin.DateNaissance.AddYears(age)) age--;
                string nomPrenom = string.Format("{0} {1}", vuePelerin.NomArabe, vuePelerin.PrenomArabe);
                infoPelerin.Text = string.Format("{0} - {1}", nomPrenom, age.ToString());

                // Alerte
                ImageButton _btnAlert = (ImageButton)e.Item.FindControl("_btnAlert");
                if (vuePelerin.Alert != null)
                {
                    _btnAlert.Visible = true;
                    _btnAlert.ToolTip = vuePelerin.Alert;
                }
                else
                    _btnAlert.Visible = false;

                // Photo
                string imgSrc = "~/images/imagesCommunes/alien.png";
                if (vuePelerin.Photo != null)
                {
                    string imgData = Convert.ToBase64String(vuePelerin.Photo);
                    imgSrc = string.Format("data:image/png;base64,{0}", imgData);
                }
                HtmlImage _imgPhotoPerson = (HtmlImage)e.Item.FindControl("_imgPhotoPerson");
                _imgPhotoPerson.Src = imgSrc;


                var panel = dataItem.FindControl("pnlPelerin") as Panel;
                panel.BackColor = ColorTranslator.FromHtml(vuePelerin.Couleur);

                if (vuePelerin.RepasPaye.Equals("OUI"))
                    imgRestaurant.Visible = true;
                else
                    imgRestaurant.Visible = false;
            }
        }

        protected void ChambreRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Chambre chambre = (Chambre)e.Item.DataItem;
                LinkButton genreLink = (LinkButton)e.Item.FindControl("ChambreLink");
                ImageButton btnSupprimer = (ImageButton)e.Item.FindControl("_btnSupprimer");
                Panel header = (Panel)e.Item.FindControl("Header");
                Label numeroChambre = (Label)e.Item.FindControl("numeroChambre");
                Label chambreDetail = (Label)genreLink.FindControl("chambreDetail");

                IList<VOR.Core.Domain.Pelerin> lstPelerin = null;

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

                        if (chambre.Evenement.Pnr.ID != pelerin.Evenement.Pnr.ID)
                            lblPelerin.Text = "&nbsp;" + string.Format("{0} {1} {2}", volPelerin, pelerin.NomArabe, pelerin.PrenomArabe, pelerin.Programme.TypeProgramme.Code + "-" + pelerin.TypeChambre.Code);
                        else
                            lblPelerin.Text = "&nbsp;" + string.Format("{0} {1}", pelerin.NomArabe, pelerin.PrenomArabe, pelerin.Programme.TypeProgramme.Code + "-" + pelerin.TypeChambre.Code);

                        if (this.AgenceID == (int)EnumAgence.SALAALJADIDA)
                        {
                            Label lblAgence = new Label();
                            lblAgence.Text = pelerin.Agence.Alias;
                            lblAgence.Style.Add("border", "1px solid");
                            lblAgence.Style.Add("padding", "2px");
                            lblAgence.Style.Add("background-color", "#fff");
                            lblAgence.Style.Add("color", "#000");
                            lblAgence.Style.Add("margin", "1px");
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

                if (string.IsNullOrEmpty(chambre.Numero))
                    numeroChambre.Visible = false;
                else
                    numeroChambre.Visible = true;

                if (chambre.Nom.Equals("SANS"))
                {
                    btnSupprimer.Style.Add("visibility", "hidden");
                    genreLink.Style.Add("padding-bottom", "37px");
                    genreLink.Style.Add("background-image", "/Images/imagesBack/room.png");
                    genreLink.Style.Add("background-position", "center");
                    genreLink.Style.Add("background-repeat", "no-repeat");
                    genreLink.Style.Add("top", "-3px");
                    genreLink.Style.Add("height", "114px");
                    header.Style.Add("visibility", "hidden");

                    Panel pnlChLink = (Panel)e.Item.FindControl("pnlChLink");
                    pnlChLink.Style.Add("position", "relative");
                    pnlChLink.Style.Add("border", "4px solid");
                    pnlChLink.Style.Add("top", "123px");
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

        private void BindPelerins()
        {
            if (this.VilleID == (int)EnumVille.MAKKAH)
                this.PelerinListView.DataSource = Global.Container.Resolve<PelerinModel>().GetPelerinSansMakkahChambreByEventID(this.EventID, AgenceId);
            else if (this.VilleID == (int)EnumVille.MEDINE)
                this.PelerinListView.DataSource = Global.Container.Resolve<PelerinModel>().GetPelerinSansMedineChambreByEventID(this.EventID, AgenceId);
        }

        private void BindChambresRepeater()
        {
            List<Chambre> lstChambre = new List<Chambre>();
            lstChambre.Add(new Chambre
            {
                ID = 0,
                Nom = "SANS",
            });

            VOR.Core.Domain.Agence agence = Global.Container.Resolve<AgenceModel>().LoadByID(this.AgenceID);
            int? agenceID = agence.TypeAgence.ID == (int)EnumTypeAgence.Filial ? (int?)this.AgenceID : null;

            lstChambre.AddRange(Global.Container.Resolve<ChambreModel>().GetChambreByEventIDAndVilleID(this.EventID, this.VilleID.Value, agenceID).OrderBy(n => n.TypeChambre.Code).ToList());
            ChambreRepeater.DataSource = lstChambre;
            ChambreRepeater.DataBind();
        }

        private void RunScript(string script)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }


        #endregion
    }
}