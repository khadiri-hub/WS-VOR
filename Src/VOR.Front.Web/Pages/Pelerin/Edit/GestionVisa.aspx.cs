using VOR.Front.Web.Base.BasePage;
using VOR.Utils;
using System;
using System.Web.UI;
using VOR.Core;
using VOR.Core.Domain;
using VOR.Core.Model;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace VOR.Front.Web.Pages.Evenement.Edit
{
    public partial class GestionVisa : BasePage
    {
        #region constants

        private const int IMG_VISA_WIDTH = 456;
        private const int IMG_VISA_HEIGHT = 317;

        #endregion

        #region Properties

        public int? VisaId
        {
            get
            {
                int id;

                if (ViewState["VisaId"] != null && int.TryParse(ViewState["VisaId"].ToString(), out id))
                    return id;
                else if (int.TryParse(Request.QueryString["VisaId"], out id))
                    return id;
                else
                    return null;
            }
            set
            {
                ViewState["VisaId"] = value.ToString();
            }
        }

        public int? PelerinId
        {
            get
            {
                int id;

                if (ViewState["PelerinId"] != null && int.TryParse(ViewState["PelerinId"].ToString(), out id))
                    return id;
                else if (int.TryParse(Request.QueryString["PelerinId"], out id))
                    return id;
                else
                    return null;
            }
            set
            {
                ViewState["PelerinId"] = value.ToString();
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControls();
                InitVisa();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                Visa visa = Global.Container.Resolve<VisaModel>().GetByID(this.VisaId.Value);
                VOR.Core.Domain.Pelerin pelerin = Global.Container.Resolve<PelerinModel>().GetByID(this.PelerinId.Value);
                pelerin.Visa = null;

                Global.Container.Resolve<PelerinModel>().Update(pelerin);
                Global.Container.Resolve<VisaModel>().Delete(visa);

                string message = "ENREGISTEMENT EFFECTUE AVEC SUCCES";
                CloseAndRefresh(message);
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                ShowMessageBow("Un problème a été rencontré lors de la suppression", "delete");
            }
        }

        protected void _btnValider_Click(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            bool isValid = ValidatePage(out errorMessage);

            Visa visa = null;

            if (!isValid)
            {
                ShowMessageBow(errorMessage, "warning");
                return;
            }

            if (this.VisaId.HasValue)
            {
                visa = Global.Container.Resolve<VisaModel>().GetByID(this.VisaId.Value);
                visa.DateModification = DateTime.Now;
                visa.UtilisateurModification = Global.Container.Resolve<UtilisateurModel>().GetByID(this.Utilisateur.ID);
            }

            if (visa == null)
            {
                visa = new Visa();
                visa.DateCreation = DateTime.Now;
                visa.UtilisateurCreation = Global.Container.Resolve<UtilisateurModel>().GetByID(this.Utilisateur.ID);
            }

            int dayExpire = Convert.ToInt32(_ddlDay.SelectedValue.ToString());
            int monthExpire = Convert.ToInt32(_ddlMonth.SelectedValue.ToString());
            int yearExpire = Convert.ToInt32(_ddlYear.SelectedValue.ToString());

            DateTime date = new DateTime(yearExpire, monthExpire, dayExpire);
            visa.Date = date;
            visa.Validite = this._txtValidite.Value.HasValue ? (int)this._txtValidite.Value : 0;

            if (AsyncUpload.UploadedFiles.Count > 0)
            {
                UploadedFile file = AsyncUpload.UploadedFiles[0];
                byte[] fileData = new byte[file.InputStream.Length];
                file.InputStream.Read(fileData, 0, (int)file.InputStream.Length);

                System.Drawing.Image imgResized = ScaleImage(byteArrayToImage(fileData), IMG_VISA_WIDTH, IMG_VISA_HEIGHT);

                visa.Image = (byte[])(new ImageConverter()).ConvertTo(imgResized, typeof(byte[]));
            }

            try
            {
                VOR.Core.Domain.Pelerin pelerin = Global.Container.Resolve<PelerinModel>().GetByID(this.PelerinId.Value);

                Global.Container.Resolve<VisaModel>().InsertOrUpdate(visa);

                pelerin.Visa = visa;
                Global.Container.Resolve<PelerinModel>().Update(pelerin);

                string message = "ENREGISTEMENT EFFECTUE AVEC SUCCES";
                CloseAndRefresh(message);
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                ShowMessageBow("Un problème a été rencontré lors de l'enregistrement", "delete");
            }
        }

        protected void btnAttachFiles_Click(object sender, EventArgs e)
        {
            if (AsyncUpload.UploadedFiles.Count > 0)
            {
                UploadedFile file = AsyncUpload.UploadedFiles[0];
                byte[] buffer = new byte[file.InputStream.Length];
                file.InputStream.Read(buffer, 0, (int)file.InputStream.Length);
                string base64String = Convert.ToBase64String(buffer, 0, buffer.Length);
                img.ImageUrl = "data:image/png;base64," + base64String;
                img.Width = IMG_VISA_WIDTH;
                img.Height = IMG_VISA_HEIGHT;
                lblVisa.Visible = false;
                img.Visible = true;
            }
        }

        #endregion

        #region Private

        private void InitControls()
        {
            BindDdlsDate();
        }

        private void BindDdlsDate()
        {
            BindDdlDays();
            BinDdlMonths();
            BinDdlYears();
        }

        private void BindDdlDays()
        {
            this._ddlDay.Items.Add(new ListItem("-- Jour --", ""));
            for (int i = 1; i < 32; i++)
            {
                ListItem listExpireDay = new ListItem();
                listExpireDay.Text = i.ToString();
                listExpireDay.Value = i.ToString();
                _ddlDay.Items.Add(listExpireDay);
            }
        }

        private void BinDdlMonths()
        {
            this._ddlMonth.Items.Add(new ListItem("-- Mois --", ""));

            for (int i = 1; i < 13; i++)
            {
                ListItem listExpireMonth = new ListItem();
                listExpireMonth.Text = i.ToString();
                listExpireMonth.Value = i.ToString();
                _ddlMonth.Items.Add(listExpireMonth);
            }
        }

        private void BinDdlYears()
        {
            this._ddlYear.Items.Add(new ListItem("-- Année --", ""));

            // Expire Pass Year
            int yearNow = DateTime.Now.Year;
            int yearNext = yearNow + 12;
            for (int i = yearNow; i < yearNext; i++)
            {
                ListItem listBirthExpire = new ListItem();
                listBirthExpire.Text = yearNow.ToString();
                listBirthExpire.Value = yearNow.ToString();
                _ddlYear.Items.Add(listBirthExpire);
                yearNow += 1;
            }
        }

        private void InitVisa()
        {
            Visa visa = null;
            VOR.Core.Domain.Pelerin pelerin = Global.Container.Resolve<PelerinModel>().GetByID(this.PelerinId.Value);

            this.VisaId = pelerin.Visa != null ? pelerin.Visa.ID : (int?)null;

            if (this.VisaId.HasValue)
            {
                visa = Global.Container.Resolve<VisaModel>().GetByID(this.VisaId.Value);

                this._ddlDay.SelectedValue = visa.Date.Day.ToString();
                this._ddlMonth.SelectedValue = visa.Date.Month.ToString();
                this._ddlYear.SelectedValue = visa.Date.Year.ToString();
                this._txtValidite.Text = visa.Validite.ToString();
                this._btnSupprimer.Visible = true;

                if (visa.Image != null)
                {
                    string base64String = Convert.ToBase64String(visa.Image, 0, visa.Image.Length);
                    img.ImageUrl = "data:image/png;base64," + base64String;
                    img.Visible = true;
                    lblVisa.Visible = false;
                }
                else
                {
                    img.Visible = false;
                    lblVisa.Visible = true;
                }
            }
        }

        public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int newWidth, int newHeight)
        {
            var newImage = new System.Drawing.Bitmap(newWidth, newHeight);

            using (var graphics = System.Drawing.Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        private void CloseAndRefresh(string msg)
        {
            RunScript(string.Format("CloseAndRebind('{0}');", msg.ToJSFormat()));
        }

        private void RunScript(string script)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        private bool ValidatePage(out string errorMessage)
        {
            errorMessage = string.Empty;
            Page.Validate();

            if (!Page.IsValid)
            {
                RunScript("validateFields();");
                errorMessage = "Vous devez remplir tous les champs obligatoires.";
                return false;
            }

            if (AsyncUpload.UploadedFiles.Count < 1 && !this.VisaId.HasValue)
            {
                errorMessage = "Vous devez saisir une image du visa";
                return false;
            }

            return true;
        }

        private void ShowMessageBow(string msg, string type)
        {
            radNotif.Text = msg;
            radNotif.ContentIcon = type;
            radNotif.TitleIcon = "none";
            radNotif.Show();
            RunScript("loading(false);");
        }

        #endregion
    }
}