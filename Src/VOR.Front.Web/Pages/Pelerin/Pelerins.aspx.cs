using VOR.Core.Model;
using VOR.Front.Web.Base.BasePage;
using VOR.Front.Web.UserControls.Menu.BO;
using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VOR.Core.Enum;
using VOR.Core.Domain.Vues;
using VOR.Core.Domain;
using System.Collections.Generic;
using VOR.Utils;
using System.Linq;
using System.Web.UI;
using System.Web;
using System.Drawing;
using System.Configuration;
using System.Data;
using DocumentFormat.OpenXml.Bibliography;
using ClosedXML.Excel;
using System.IO;
using System.Web.Hosting;
using Telerik.Web.UI.ExportInfrastructure;
using System.Globalization;

namespace VOR.Front.Web.Pages.Pelerin
{
    public partial class Pelerins : BasePage
    {

        #region Events 

        protected void Page_Init(object sender, EventArgs e)
        {
            ((SiteMaster)this.Master).ActiveTab = MenuTopUC.Tab.Pelerin;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager.GetCurrent(this.Page).ClientEvents.OnRequestStart = "onRequestStart";

            if (!IsPostBack)
            {
                InitControls();
                //InitCalculEvenementEnCours();
                GetDataSource();

            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Control ctrl = GetControlThatCausedPostBack(this.Page);
            if (ctrl != null && (ctrl.ID.Equals(this.gridPelerin.ID) || ctrl.ID.Equals("_ddlEvenement")))
                GetDataSource();
        }


        //private void InitCalculEvenementEnCours()
        //{
        //    IList<Core.Domain.Evenement> lstEvenement = Global.Container.Resolve<EvenementModel>().GetEvenementsEnCours();

        //    IList<Core.Domain.Evenement> lstEvenementRamandan = lstEvenement.Where(t=>t.TypeEvenement == "RAMADAN").ToList();
        //    IList<Core.Domain.Evenement> lstEvenementMawlid = lstEvenement.Where(t => t.TypeEvenement == "MARS").ToList();

        //    List<int> lstEventIdsRamadan = new List<int>();
        //    List<int> lstEventIdsMawlid = new List<int>();
        //    foreach (Core.Domain.Evenement even in lstEvenementRamandan)
        //    {

        //        lstEventIdsRamadan.Add(even.ID);
        //    }

        //    foreach (Core.Domain.Evenement even in lstEvenementMawlid)
        //    {

        //        lstEventIdsMawlid.Add(even.ID);
        //    }

        //    //int totalPayeEvenementEnCours = 0;
        //    //int resteAPayerEvenementEnCours = 0;

        //    //int totalPayeEvenementEnCoursMawlid = 0;
        //    //int resteAPayerEvenementEnCoursMawlid = 0;
        //    //IList<VOR.Core.Domain.Pelerin> pelerins = Global.Container.Resolve<PelerinModel>().GetListPelerinsByEventIds(lstEventIdsRamadan);

        //    //foreach (VOR.Core.Domain.Pelerin pelerin in pelerins)
        //    //{
        //    //    totalPayeEvenementEnCours += pelerin.MontantPaye;
        //    //    resteAPayerEvenementEnCours += pelerin.RestApayer;
        //    //}

        //    //this._totalMontantPaye.InnerText = String.Format("{0:n}", totalPayeEvenementEnCours) + " DHS";
        //    //this._totalResteAPayer.InnerText = String.Format("{0:n}", resteAPayerEvenementEnCours) + " DHS";

        //    //if (pelerins != null)
        //    //    _totalPelerins.InnerText = pelerins.Where(e=>e.EvenementID != 124).Count().ToString();

        //    //IList<VOR.Core.Domain.Pelerin> pelerinsMawlid = Global.Container.Resolve<PelerinModel>().GetListPelerinsByEventIds(lstEventIdsMawlid);
        //    //foreach (VOR.Core.Domain.Pelerin pelerin in pelerinsMawlid)
        //    //{
        //    //    totalPayeEvenementEnCoursMawlid += pelerin.MontantPaye;
        //    //    resteAPayerEvenementEnCoursMawlid += pelerin.RestApayer;
        //    //}

        //    //this._totalMontantPayeMawlid.InnerText = String.Format("{0:n}", totalPayeEvenementEnCoursMawlid) + " DHS";
        //    //this._totalResteAPayerMawlid.InnerText = String.Format("{0:n}", resteAPayerEvenementEnCoursMawlid) + " DHS";

        //    //if (pelerinsMawlid != null)
        //    //    _totalPelerinsMawlid.InnerText = pelerinsMawlid.Where(e => e.EvenementID != 124).Count().ToString();
        //}


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

        protected void RadSearchBox1_DataSourceSelect(object sender, SearchBoxDataSourceSelectEventArgs e)
        {
            SqlDataSource source = (SqlDataSource)e.DataSource;
            RadSearchBox searchBox = (RadSearchBox)sender;

            string likeCondition = string.Format("'{0}' + @filterString + '%'", searchBox.Filter == SearchBoxFilter.Contains ? "%" : "");
            string countCondition = e.ShowAllResults ? " " : " TOP " + searchBox.MaxResultCount + 1;

            if (e.SelectedContextItem != null)
            {
                likeCondition = string.Format("{0} AND ID_EVENEMENT={1}", likeCondition, e.SelectedContextItem.Key);
            }

            source.SelectCommand = string.Format("SELECT {0} * FROM [View_Pelerin_Search] WHERE [NOM_PRENOM] LIKE {1} ORDER BY ID_EVENEMENT DESC", countCondition, likeCondition);

            source.SelectParameters.Add("filterString", e.FilterString.Replace("%", "[%]").Replace("_", "[_]"));
        }

        protected void RadSearchBox1_Search(object sender, SearchBoxEventArgs e)
        {
            if (e.Value != null)
            {
                ((SiteMaster)this.Master).Menu.Evenement = e.Value;
                this.EventID = int.Parse(e.Value);
                this.CookieHelper.WriteCookieEntry("EventID", e.Value);
                //((SiteMaster)this.Master).Menu.DataBind();

                GetDataSource();
            }
        }

        private void GetDataSource(bool export = false)
        {
            VOR.Core.Domain.Agence agence = Global.Container.Resolve<AgenceModel>().LoadByID(this.AgenceID);

            int id;
            int? statutPelerinID = (!int.TryParse(this._rdoStatut.SelectedValue, out id) || this._rdoStatut.SelectedValue == "0") ? null : (int?)int.Parse(this._rdoStatut.SelectedValue);
            int? motifStatutPelerinID = (!int.TryParse(this._ddlMotifStatut.SelectedValue, out id) || this._ddlMotifStatut.SelectedValue == "0") ? null : (int?)int.Parse(this._ddlMotifStatut.SelectedValue);

            int? agenceID = agence.TypeAgence.ID == (int)EnumTypeAgence.Filial ? (int?)this.AgenceID : null;
            IList<VuePelerin> lstPelerin = Global.Container.Resolve<PelerinModel>().GetPelerinByEventIDAndAgenceID(this.EventID, agenceID, null, statutPelerinID, motifStatutPelerinID);
            //lstPelerin = lstPelerin.OrderBy(p => p.IdTypePelerin).ToList();
            this.gridPelerin.DataSource = lstPelerin;
            ViewState["lstPelerin"] = lstPelerin;

            if (!export)
                this.gridPelerin.DataBind();


            Pnr pnr = new Pnr();

            if (this.EventID != -1)
            {
                VOR.Core.Domain.Evenement EvenementEnCours = Global.Container.Resolve<EvenementModel>().LoadByID(this.EventID);
                pnr = Global.Container.Resolve<PnrModel>().LoadByID(EvenementEnCours.Pnr.ID);
            }

            int montantPaye = 0;
            int resteApayer = 0;
            foreach (VuePelerin vuePelerin in lstPelerin)
            {
                montantPaye += vuePelerin.MontantPaye;
                resteApayer += vuePelerin.RestApayer;
            }

            //this._montantPaye.InnerText = String.Format("{0:n}", montantPaye) + " DHS";
            //this._resteAPayer.InnerText = String.Format("{0:n}", resteApayer) + " DHS";



            int nbrPelerins = Global.Container.Resolve<PelerinModel>().GetNbrPelerinsByEventID(this.EventID);
            this.nbrPelerin.InnerText = nbrPelerins.ToString();

            int nbrPelerinsMax = pnr.NbrPassager;
            this.nbrMaxPelerins.InnerText = nbrPelerinsMax.ToString();


            if (nbrPelerins == nbrPelerinsMax)
            {
                var msg = "Impossible d'ajouter d'autre pelerins pour cet événement. Nbr de Pelerins max est atteint";

                btnNew.Attributes.Add("onClick", "DisplayMsgError('" + msg.ToJSFormat() + "');");
            }
            else
                InitNewButton();
        }

        protected void gridPelerin_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bool export = true;
            GetDataSource(export);
        }

        protected void gridPelerin_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {

                VuePelerin vuePelerin = (VuePelerin)e.Item.DataItem;
                var _lblSexe = e.Item.FindControl("_lblSexe") as Label;
                _lblSexe.Text = vuePelerin.Sexe.Equals("HOMME") ? "H" : "F";

                var btnEdit = (HyperLink)e.Item.FindControl("_btnEdit");

                // Edit
                string url = string.Empty;
                string pageUrl = string.Empty;
                string popupTitle = string.Format("{0} {1}", vuePelerin.NomFrancais.ToJSFormat().ToUpper(), vuePelerin.PrenomFrancais.ToJSFormat().ToUpper());
                string myRadWindow = string.Empty;
                pageUrl = "~/Pages/Pelerin/Edit/GestionPelerin.aspx";
                url = ResolveUrl(string.Format("{0}?RenderMode=popin&Id={1}", pageUrl, vuePelerin.ID));
                myRadWindow = string.Format("return OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);

                btnEdit.NavigateUrl = "#";
                btnEdit.Attributes["onclick"] = myRadWindow;


                // Recu
                string titleRecu = string.Format("{0} de {1} {2}", "Recus", vuePelerin.NomFrancais.ToJSFormat().ToUpper(), vuePelerin.PrenomFrancais.ToJSFormat().ToUpper());
                var btnRecus = (HyperLink)e.Item.FindControl("_btnRecus");
                string pageRecus = "~/Pages/Pelerin/Edit/GestionRecu.aspx";
                string RecusUrl = ResolveUrl(string.Format("{0}?RenderMode=popin&PelerinId={1}", pageRecus, vuePelerin.ID));
                string radwindowEditRecu = string.Format("return OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", RecusUrl, this._rwmRecus.ClientID, "_rwRecus", titleRecu);

                btnRecus.NavigateUrl = "#";
                btnRecus.Attributes["onclick"] = radwindowEditRecu;

                // Visa
                string titleVisa = string.Format("{0} de {1} {2}", "Visa", vuePelerin.NomFrancais.ToJSFormat().ToUpper(), vuePelerin.PrenomFrancais.ToJSFormat().ToUpper());
                var btnVisa = (HyperLink)e.Item.FindControl("_btnVisa");
                string pageVisa = "~/Pages/Pelerin/Edit/GestionVisa.aspx";
                string VisaUrl = ResolveUrl(string.Format("{0}?RenderMode=popin&PelerinId={1}", pageVisa, vuePelerin.ID));
                string radwindowEditVisa = string.Format("return OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", VisaUrl, this._rwmVisa.ClientID, "_rwVisa", titleVisa);

                btnVisa.NavigateUrl = "#";
                btnVisa.Attributes["onclick"] = radwindowEditVisa;


                if (vuePelerin.Couleur != null)
                    e.Item.BackColor = ColorTranslator.FromHtml(vuePelerin.Couleur);

                var btnAlert = (ImageButton)e.Item.FindControl("_btnAlert");
                if (!string.IsNullOrEmpty(vuePelerin.Alert))
                {
                    btnAlert.Visible = true;
                    btnAlert.ToolTip = vuePelerin.Alert;
                }
                else
                    btnAlert.Visible = false;

                ImageButton imgTypePelrin = (ImageButton)e.Item.FindControl("_imgTypePelerin");

                if (vuePelerin.IdTypePelerin == 1)  // Nouveau
                    imgTypePelrin.ImageUrl = "~/Images/imagesBack/new.png";

                if (vuePelerin.IdTypePelerin == 2)  // Regle
                    imgTypePelrin.ImageUrl = "~/Images/imagesBack/updated.png";

                if (vuePelerin.IdTypePelerin == 3)  // En Cours
                    imgTypePelrin.ImageUrl = "~/Images/imagesBack/encours.png";



                var imgOk = (ImageButton)e.Item.FindControl("imgOk");
                var imgAlertVisa = (ImageButton)e.Item.FindControl("imgAlertVisa");

                if (vuePelerin.DateExpirationVisa == null)
                {
                    imgOk.Visible = false;
                    imgAlertVisa.Visible = false;
                }
                else
                {
                    bool alertVisa;
                    int daysBeforeExpiration = int.Parse(ConfigurationManager.AppSettings["DAYS_BEFORE_EXPIRATION_VISA"]);
                    DateTime daysLater = DateTime.Today.AddDays(daysBeforeExpiration);
                    alertVisa = vuePelerin.DateExpirationVisa <= daysLater ? true : false;

                    if (alertVisa)
                    {
                        imgAlertVisa.Visible = true;
                        imgAlertVisa.ToolTip = string.Format("Le Visa expire le {0}", vuePelerin.DateExpirationVisa.Value.ToString("dd/MM"));
                        imgOk.Visible = false;
                    }
                    else
                    {
                        imgOk.Visible = true;
                        imgAlertVisa.Visible = false;
                    }
                }

                var _lblStatut = e.Item.FindControl("_lblStatut") as Label;
                _lblStatut.Text = vuePelerin.Statut;
                _lblStatut.Font.Bold = true;

                var _lblMotifStatut = e.Item.FindControl("_lblMotifStatut") as Label;
                _lblMotifStatut.Text = vuePelerin.MotifStatut;
                _lblMotifStatut.Font.Bold = true;
            }
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            GetDataSource();
        }

        #endregion

        #region Private 

        private void InitControls()
        {
            this.gridPelerin.Skin = this.SkinTelerik;
            InitNewButton();
            BindRdoStatut();
        }

        private void InitNewButton()
        {
            string pageUrl = string.Empty;
            string url = string.Empty;
            string function = string.Empty;
            string popupTitle = string.Empty;

            pageUrl = "~/Pages/Pelerin/Edit/GestionPelerin.aspx";
            url = ResolveUrl(string.Format("{0}?RenderMode=popin", pageUrl));
            popupTitle = "Pelerin";

            function = string.Format("OpenMyRadWindow('{0}', '{1}', '{2}', '{3}');", url, this._rwmEdit.ClientID, "_rwEdit", popupTitle);
            btnNew.Attributes.Add("onClick", function);
        }

        private void BindRdoStatut()
        {
            this._rdoStatut.Items.Clear();
            this._rdoStatut.Items.Add(new ListItem("Reset", "0"));

            this._rdoStatut.DataSource = Global.Container.Resolve<StatutPelerinModel>().GetAll().ToList();
            this._rdoStatut.DataValueField = "ID";
            this._rdoStatut.DataTextField = "Nom";
            this._rdoStatut.DataBind();


            this._rdoStatut.SelectedValue = "0";
        }

        private void BindMotifStatutPelerin()
        {
            BindDdlMotifsStatut();
        }

        private void BindDdlMotifsStatut()
        {
            int? statutId = this._rdoStatut.SelectedValue == "0" ? null : (int?)int.Parse(this._rdoStatut.SelectedValue);

            this._ddlMotifStatut.Items.Clear();

            if (statutId != null)
            {
                this._ddlMotifStatut.Items.Add(new ListItem("---------------- Aucun ----------------", "0"));
                this._ddlMotifStatut.DataSource = Global.Container.Resolve<MotifStatutPelerinModel>().GetByStatutPelerin((int)statutId);
                this._ddlMotifStatut.DataTextField = "Nom";
                this._ddlMotifStatut.DataValueField = "ID";
                this._ddlMotifStatut.DataBind();
            }
            else
                this._ddlMotifStatut.Items.Add(new ListItem("---------------- Aucun ----------------", "0"));
        }

        private void RunScript(string script)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        private void ShowMessageBow(string msg, string type)
        {
            radNotifAlert.Text = msg;
            radNotifAlert.ContentIcon = type;
            radNotifAlert.TitleIcon = "none";
            radNotifAlert.Show();
        }

        #endregion

        protected void btnPrintBadges_Click(object sender, EventArgs e)
        {
            try
            {
                IList<int> lstIdsPelerin = new List<int>();
                IList<int> lstIdsSelectedPelerin = new List<int>();
                foreach (GridDataItem Gdi in gridPelerin.Items)
                {
                    lstIdsPelerin.Add(int.Parse(Gdi.GetDataKeyValue("ID").ToString()));

                    if (Gdi.Selected)
                        lstIdsSelectedPelerin.Add(int.Parse(Gdi.GetDataKeyValue("ID").ToString()));
                }

                if (lstIdsPelerin.Count > 0 || lstIdsSelectedPelerin.Count > 0)
                {
                    if (lstIdsSelectedPelerin.Count == 0)
                        Global.Container.Resolve<PelerinModel>().SetBadgeToDownload(true, lstIdsPelerin);
                    else
                        Global.Container.Resolve<PelerinModel>().SetBadgeToDownload(true, lstIdsSelectedPelerin);

                    var url = string.Format("downloadBadge({0},{1});", this.EventID, this.AgenceID);
                    RunScript(url);
                }

            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
            }
        }

        protected void btnPrintBadge_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            VOR.Core.Domain.Pelerin pelerin = Global.Container.Resolve<PelerinModel>().LoadByID(int.Parse(((ImageButton)sender).CommandArgument.ToString()));

            try
            {
                pelerin.BadgeToDownload = true;
                Global.Container.Resolve<PelerinModel>().Update(pelerin);
                var url = string.Format("downloadBadge({0},{1});", this.EventID, this.AgenceID);
                RunScript(url);
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
            }
        }

        protected void _btnExportToExcel_Click(object sender, EventArgs e)
        {
            this.gridPelerin.NeedDataSource += new GridNeedDataSourceEventHandler(gridPelerin_NeedDataSource);

            foreach (GridDataItem Gdi in gridPelerin.Items)
            {
                int pelerinID = int.Parse(Gdi.GetDataKeyValue("ID").ToString());
                VOR.Core.Domain.Pelerin pelerin = Global.Container.Resolve<PelerinModel>().GetByID(pelerinID);

                if (pelerin.Alerte != null && EnumTypeAlerte.BIL.ToString() == pelerin.Alerte.TypeAlerte.Code)
                {
                    string msg = string.Format("Vueillez désactiver l'alerte '{0}' du pelerin {1} {2} afin d'exporter la liste", pelerin.Alerte.Libelle, pelerin.NomFrancais, pelerin.PrenomFrancais);
                    ShowMessageBow(msg, "warning");
                    return;
                }
            }

            VOR.Core.Domain.Evenement evenement = Global.Container.Resolve<EvenementModel>().LoadByID(this.EventID);

            this.gridPelerin.ExportSettings.ExportOnlyData = true;
            this.gridPelerin.ExportSettings.IgnorePaging = true;
            this.gridPelerin.ExportSettings.OpenInNewWindow = true;
            this.gridPelerin.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
            this.gridPelerin.ExportSettings.FileName = string.Format("EXPORT-PELERIN-{0}-{1}", evenement.Nom, DateTime.Now.ToString("dd.MM.yyyy"));
            this.gridPelerin.MasterTableView.ExportToExcel();
        }

        protected void _rdoStatut_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMotifStatutPelerin();
        }

        protected void _btnAfficher_Click(object sender, EventArgs e)
        {
            GetDataSource();
        }

        protected void _btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                IList<int> lstIdsPelerin = new List<int>();
                IList<int> lstIdsSelectedPelerin = new List<int>();
                foreach (GridDataItem Gdi in gridPelerin.Items)
                {
                    lstIdsPelerin.Add(int.Parse(Gdi.GetDataKeyValue("ID").ToString()));

                    if (Gdi.Selected)
                        lstIdsSelectedPelerin.Add(int.Parse(Gdi.GetDataKeyValue("ID").ToString()));
                }

                if (lstIdsPelerin.Count > 0 || lstIdsSelectedPelerin.Count > 0)
                {
                    int id;
                    int? statutPelerinID = (!int.TryParse(this._rdoStatut.SelectedValue, out id) || this._rdoStatut.SelectedValue == "0") ? null : (int?)int.Parse(this._rdoStatut.SelectedValue);
                    int? motifStatutPelerinID = (!int.TryParse(this._ddlMotifStatut.SelectedValue, out id) || this._ddlMotifStatut.SelectedValue == "0") ? null : (int?)int.Parse(this._ddlMotifStatut.SelectedValue);

                    if (lstIdsSelectedPelerin.Count == 0)
                        Global.Container.Resolve<PelerinModel>().SetStatutPelerin(statutPelerinID, motifStatutPelerinID, lstIdsPelerin);
                    else
                        Global.Container.Resolve<PelerinModel>().SetStatutPelerin(statutPelerinID, motifStatutPelerinID, lstIdsSelectedPelerin);
                    GetDataSource();
                }
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
            }
        }

        protected void btnExportTurkish_Click(object sender, ImageClickEventArgs e)
        {
            int CELL_NO = 1;
            int CELL_DOB = 2;
            int CELL_GENDER = 3;
            int CELL_LAST_NAME = 4;
            int CELL_FIRST_NAME = 5;
            int CELL_TITLE = 6;

            Core.Domain.Evenement evenement = Global.Container.Resolve<EvenementModel>().LoadByID(this.EventID);
            string pnrDtDepart = evenement.Pnr.HeureDepart.HasValue ? evenement.Pnr.HeureDepart.Value.ToString("yyyyMMdd") : "YYYYMMDD";

            string fileName = $"VoyageOr_{pnrDtDepart}_TK.xlsx";

            var templatePath = HostingEnvironment.MapPath("~/Ressources/Exports/export_TK.xlsx");
            IList<VuePelerin> lstPelerin = (List<VuePelerin>)ViewState["lstPelerin"];

            using (XLWorkbook wb = new XLWorkbook(templatePath))
            {
                if (lstPelerin != null && lstPelerin.Any())
                {
                    DataTable dt = new DataTable();
                    IXLWorksheet workSheet = wb.Worksheet(1);

                    int rowsHeader = 1;
                    int startRow = rowsHeader + 1;

                    workSheet.Row(startRow).InsertRowsBelow(lstPelerin.Count - 1);

                    int i = 0;
                   foreach (IXLRow row in workSheet.Rows().Skip(rowsHeader))
                    {
                        if (i < lstPelerin.Count)
                        {
                            VuePelerin pelerin = lstPelerin[i];
                            row.Style.Font.Bold = false;

                            IXLCell cellNo = row.Cell(CELL_NO);
                            cellNo.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellNo.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellNo.Value = i + 1;

                            IXLCell cellDoB = row.Cell(CELL_DOB);
                            cellDoB.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellDoB.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            string day = pelerin.DateNaissance.Day.ToString("d");
                            string month = formatMonth(pelerin.DateNaissance.Month);
                            string year = pelerin.DateNaissance.Year.ToString();
                            cellDoB.DataType = XLDataType.Text;
                            cellDoB.SetValue<string>(Convert.ToString($"{day}-{month}-{year}"));

                            IXLCell cellGender = row.Cell(CELL_GENDER);
                            cellGender.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellGender.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellGender.Value = pelerin.Sexe.Equals("HOMME") ? "M" : "F";

                            IXLCell cellLastName = row.Cell(CELL_LAST_NAME);
                            cellLastName.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellLastName.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellLastName.Value = pelerin.NomFrancais.ToUpper();

                            IXLCell cellFirstName = row.Cell(CELL_FIRST_NAME);
                            cellFirstName.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellFirstName.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellFirstName.Value = pelerin.PrenomFrancais.ToUpper();

                            IXLCell cellTitle = row.Cell(CELL_TITLE);
                            cellTitle.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellTitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellTitle.Value = pelerin.Sexe.Equals("HOMME") ? "MR" : "MRS";

                            i++;
                        }
                    }
                }

                Response.Clear();
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                Response.ContentType = "application/vnd.ms-excel";
                if (HttpContext.Current.Request.UserAgent.Contains("Trident/7") || HttpContext.Current.Request.UserAgent.Contains("Edge/"))
                {
                    Response.Headers.Add("X-UA-Compatible", "IE=EmulateIE9,chrome=1");
                    Response.Headers.Add("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                }
                else
                {
                    Response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                }
                Response.Cookies.Add(new HttpCookie("downloadExport", "true") { HttpOnly = false, Path = "/" });

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    ms.WriteTo(HttpContext.Current.Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    Response.BinaryWrite(ms.ToArray());
                }
            }
        }

        protected void btnExportSaudia_Click(object sender, ImageClickEventArgs e)
        {
            int CELL_CODE = 1;
            int CELL_PASS = 2;
            int CELL_SEPARATOR_1 = 3;
            int CELL_NAT = 4;
            int CELL_SEPARATOR_2 = 5;
            int CELL_DOB = 6;
            int CELL_SEPARATOR_3 = 7;
            int CELL_GENDER = 8;
            int CELL_SEPARATOR_4 = 9;
            int CELL_PASS_EXP = 10;
            int CELL_SEPARATOR_5 = 11;
            int CELL_NAME = 12;
            int CELL_P = 13;
            int CELL_NO = 14;
            int CELL_POINT_VIRGULE = 15;



            Core.Domain.Evenement evenement = Global.Container.Resolve<EvenementModel>().LoadByID(this.EventID);
            string pnrDtDepart = evenement.Pnr.HeureDepart.HasValue ? evenement.Pnr.HeureDepart.Value.ToString("yyyyMMdd") : "YYYYMMDD";

            string fileName = $"VoyageOr_{pnrDtDepart}_SV.xlsx";

            var templatePath = HostingEnvironment.MapPath("~/Ressources/Exports/export_SV.xlsx");
            IList<VuePelerin> lstPelerin = (List<VuePelerin>)ViewState["lstPelerin"];

            using (XLWorkbook wb = new XLWorkbook(templatePath))
            {
                if (lstPelerin != null && lstPelerin.Any())
                {
                    DataTable dt = new DataTable();
                    IXLWorksheet workSheet = wb.Worksheet(1);

                    int rowsHeader = 1;
                    int startRow = rowsHeader + 1;

                    workSheet.Row(startRow).InsertRowsBelow(lstPelerin.Count - 1);

                    int i = 0;
                    foreach (IXLRow row in workSheet.Rows().Skip(rowsHeader))
                    {
                        if (i < lstPelerin.Count)
                        {
                            VuePelerin pelerin = lstPelerin[i];
                            row.Style.Font.Bold = false;

                            IXLCell cellCode = row.Cell(CELL_CODE);
                            cellCode.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellCode.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellCode.Style.Font.Bold = true;
                            cellCode.Value = "SRDOCSSVHK1-P-MAR-";

                            IXLCell cellPass = row.Cell(CELL_PASS);
                            cellPass.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellPass.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellPass.Value = pelerin.NumPassport.ToUpper();

                            IXLCell cellSeparator1 = row.Cell(CELL_SEPARATOR_1);
                            cellSeparator1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellSeparator1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellSeparator1.Value = "-";

                            IXLCell cellNationality = row.Cell(CELL_NAT);
                            cellNationality.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellNationality.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellNationality.Style.Font.Bold = true;
                            cellNationality.Value = "MAR";

                            IXLCell cellSeparator2 = row.Cell(CELL_SEPARATOR_2);
                            cellSeparator2.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellSeparator2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellSeparator2.Value = "-";

                            IXLCell cellDoB = row.Cell(CELL_DOB);
                            cellDoB.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellDoB.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellDoB.Value = pelerin.DateNaissance;

                            IXLCell cellSeparator3 = row.Cell(CELL_SEPARATOR_3);
                            cellSeparator3.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellSeparator3.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellSeparator3.Value = "-";

                            IXLCell cellGender = row.Cell(CELL_GENDER);
                            cellGender.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellGender.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellGender.Value = pelerin.Sexe.Equals("HOMME") ? "M" : "F";

                            IXLCell cellSeparator4 = row.Cell(CELL_SEPARATOR_4);
                            cellSeparator4.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellSeparator4.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellSeparator4.Value = "-";

                            IXLCell cellPassExp = row.Cell(CELL_PASS_EXP);
                            cellPassExp.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellPassExp.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellPassExp.Value = pelerin.DateExpiration;

                            IXLCell cellSeparator5 = row.Cell(CELL_SEPARATOR_5);
                            cellSeparator5.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellSeparator5.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellSeparator5.Value = "-";

                            IXLCell cellName = row.Cell(CELL_NAME);
                            cellName.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellName.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            var title = pelerin.Sexe.Equals("HOMME") ? "MR" : "MRS";
                            cellName.Value = $"{pelerin.PrenomFrancais.ToUpper()}/{pelerin.NomFrancais.ToUpper()} {title}";

                            IXLCell cellP = row.Cell(CELL_P);
                            cellP.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellP.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellP.Style.Font.Bold = true;
                            cellP.Value = "/P";

                            IXLCell cellNo = row.Cell(CELL_NO);
                            cellNo.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellNo.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellNo.Style.Font.Bold = true;
                            cellNo.Value = i + 1;

                            IXLCell cellSep = row.Cell(CELL_POINT_VIRGULE);
                            cellSep.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            cellSep.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cellSep.Value = ";";

                            i++;
                        }
                    }
                }

                Response.Clear();
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                Response.ContentType = "application/vnd.ms-excel";
                if (HttpContext.Current.Request.UserAgent.Contains("Trident/7") || HttpContext.Current.Request.UserAgent.Contains("Edge/"))
                {
                    Response.Headers.Add("X-UA-Compatible", "IE=EmulateIE9,chrome=1");
                    Response.Headers.Add("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                }
                else
                {
                    Response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                }
                Response.Cookies.Add(new HttpCookie("downloadExport", "true") { HttpOnly = false, Path = "/" });

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    ms.WriteTo(HttpContext.Current.Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    Response.BinaryWrite(ms.ToArray());
                }
            }
        }

        private string formatMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "janv.";
                case 2:
                    return "févr.";
                case 3:
                    return "mars";
                case 4:
                    return "avr.";
                case 5:
                    return "mai";
                case 6:
                    return "juin";
                case 7:
                    return "juil.";
                case 8:
                    return "août";
                case 9:
                    return "sept.";
                case 10:
                    return "oct.";
                case 11:
                    return "nov.";
                case 12:
                    return "déc.";
                default:
                    return string.Empty;
            }
        } 
    }
}