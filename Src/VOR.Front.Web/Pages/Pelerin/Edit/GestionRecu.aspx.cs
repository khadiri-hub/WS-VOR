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
    public partial class GestionRecu : BasePage
    {
        #region constants

        private const int IMG_RECU_WIDTH = 456;
        private const int IMG_RECU_HEIGHT = 317;

        #endregion

        #region Properties

        public int? RecuId
        {
            get
            {
                int id;

                if (ViewState["RecuId"] != null && int.TryParse(ViewState["RecuId"].ToString(), out id))
                    return id;
                else if (int.TryParse(Request.QueryString["RecuId"], out id))
                    return id;
                else
                    return null;
            }
            set
            {
                ViewState["RecuId"] = value.ToString();
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
                InitRecu();
            }
        }

        protected void _btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                Recu recu = Global.Container.Resolve<RecuModel>().GetByID(this.RecuId.Value);
                Global.Container.Resolve<RecuModel>().Delete(recu);
                ClearData();
                this.gridRecu.Rebind();
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

            if (!isValid)
            {
                ShowMessageBow(errorMessage, "warning");
                return;
            }

            Recu recu = null;
            if (this.RecuId.HasValue) { 
                recu = Global.Container.Resolve<RecuModel>().GetByID(this.RecuId.Value);
                recu.DateModification = DateTime.Now;
                recu.UtilisateurModification = Global.Container.Resolve<UtilisateurModel>().GetByID(this.Utilisateur.ID);
            }

            if (recu == null) { 
                recu = new Recu();
                recu.DateCreation = DateTime.Now;
                recu.UtilisateurCreation = Global.Container.Resolve<UtilisateurModel>().GetByID(this.Utilisateur.ID);
            }

            recu.Pelerin = Global.Container.Resolve<PelerinModel>().GetByID(this.PelerinId.Value);
            recu.Numero = this._txtNumRecu.Text;
            recu.Montant = this._txtMontant.Value.HasValue ? (int)this._txtMontant.Value : 0;

            if (AsyncUpload.UploadedFiles.Count > 0)
            {
                UploadedFile file = AsyncUpload.UploadedFiles[0];
                byte[] fileData = new byte[file.InputStream.Length];
                file.InputStream.Read(fileData, 0, (int)file.InputStream.Length);

                System.Drawing.Image imgResized = ScaleImage(byteArrayToImage(fileData), IMG_RECU_WIDTH, IMG_RECU_HEIGHT);

                recu.Image = (byte[])(new ImageConverter()).ConvertTo(imgResized, typeof(byte[]));
            }

            try
            {
                Global.Container.Resolve<RecuModel>().InsertOrUpdate(recu);
                ClearData();
                this.gridRecu.Rebind();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex);
                ShowMessageBow("Un problème a été rencontré lors de l'enregistrement", "delete");
            }
        }

        protected void gridRecu_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                Recu recu = (Recu)e.Item.DataItem;

                if (recu != null)
                {
                    if (recu.DateCreation.HasValue)
                    {
                        Label lblCreation = (Label)e.Item.FindControl("lblCreation");
                        lblCreation.Text = string.Format("{0} par {1}", recu.DateCreation.Value.ToString("MM/dd/yyyy"), string.Format("{0} {1}", recu.UtilisateurCreation.Nom, recu.UtilisateurCreation.Prenom));
                    }
                    if (recu.DateModification.HasValue)
                    {
                        Label lblModification = (Label)e.Item.FindControl("lblModification");
                        lblModification.Text = string.Format("{0} par {1}", recu.DateModification.Value.ToString("MM/dd/yyyy"), string.Format("{0} {1}", recu.UtilisateurModification.Nom, recu.UtilisateurModification.Prenom));
                    }

                    Label lblMontant = (Label)e.Item.FindControl("lblMontant");
                    lblMontant.Text = string.Format("{0} DHS", recu.Montant);
                }
            }
        }

        protected void gridRecu_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "RowClick":
                    GridDataItem dataItem = e.Item as GridDataItem;
                    ViewState["RecuId"] = int.Parse(dataItem.GetDataKeyValue("ID").ToString());
                    InitRecu();
                    break;
                default:
                    break;
            }
        }

        protected void gridRecu_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            IList<Recu> lstRecu = new List<Recu>();

            if (this.PelerinId.HasValue)
                lstRecu = Global.Container.Resolve<RecuModel>().GetRecuByPelerinID(this.PelerinId.Value);

            this.gridRecu.DataSource = lstRecu;
        }

        protected void btnNew_Click(object sender, ImageClickEventArgs e)
        {
            ClearData();
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
                img.Width = IMG_RECU_WIDTH;
                img.Height = IMG_RECU_HEIGHT;
                lblRecu.Visible = false;
                img.Visible = true;
            }
        }

        #endregion

        #region Private

        private void InitControls()
        {
            this.gridRecu.Skin = this.SkinTelerik;
        }

        private void InitRecu()
        {
            Recu recu = null;

            if (this.RecuId.HasValue) { 
                recu = Global.Container.Resolve<RecuModel>().GetByID(this.RecuId.Value);
                this._txtNumRecu.Enabled = false;

                if (recu.Image != null)
                {
                    string base64String = Convert.ToBase64String(recu.Image, 0, recu.Image.Length);
                    img.ImageUrl = "data:image/png;base64," + base64String;
                    img.Visible = true;
                    lblRecu.Visible = false;
                }
                else
                {
                    img.Visible = false;
                    lblRecu.Visible = true;
                }
            }
            else {
                ClearData();
            }

            if (recu != null)
            {
                this._txtNumRecu.Text = recu.Numero;
                this._txtMontant.Text = recu.Montant.ToString();
                this._btnSupprimer.Visible = true;
            }
        }

        private void ClearData()
        {
            ViewState["RecuId"] = null;
            this._txtMontant.Text = string.Empty;
            this._txtNumRecu.Text = string.Empty;
            this._btnSupprimer.Visible = false;
            this._txtNumRecu.Enabled = true;

            img.Visible = false;
            lblRecu.Visible = true;
            this._btnSupprimer.Visible = false;
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


            if (AsyncUpload.UploadedFiles.Count < 1 && !this.RecuId.HasValue)
            {
                errorMessage = "Vous devez saisir une image du recu";
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

        protected void AsyncUpload_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            if (AsyncUpload.UploadedFiles.Count > 0)
            {
                UploadedFile file = AsyncUpload.UploadedFiles[0];
                byte[] buffer = new byte[file.InputStream.Length];
                file.InputStream.Read(buffer, 0, (int)file.InputStream.Length);
                string base64String = Convert.ToBase64String(buffer, 0, buffer.Length);
                img.ImageUrl = "data:image/png;base64," + base64String;
                img.Width = IMG_RECU_WIDTH;
                img.Height = IMG_RECU_HEIGHT;
                lblRecu.Visible = false;
                img.Visible = true;
            }
        }

        protected void RadAsyncUpload1_FileUploaded1(object sender, FileUploadedEventArgs e)
        {
            string path = e.File.GetName();
        }
    }
}