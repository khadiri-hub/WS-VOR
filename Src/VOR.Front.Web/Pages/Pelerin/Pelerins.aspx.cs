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
    }
}