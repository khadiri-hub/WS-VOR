<%@ Page Language="C#" MasterPageFile="~/PopIn.Master" CodeBehind="GestionVisa.aspx.cs" AutoEventWireup="true"
    Inherits="VOR.Front.Web.Pages.Evenement.Edit.GestionVisa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            function validateFields() {
                $(".required").prev().removeClass("requiredField");
                $(".required:visible").prev().addClass("requiredField");
                $(".regExp").prev().prev().removeClass("requiredField");
                $(".regExp:visible").prev().prev().addClass("requiredField");
            }

            function OnClientFileUploaded(sender, args) {
                document.getElementById('<%= btnAttachFiles.ClientID %>').click();
            }
        </script>
    </telerik:RadScriptBlock>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content" style="background-color: #c7cde2; min-height: 100%;">
        <div class="headerSaveDelete">
            <div style="float: right">
                <asp:ImageButton CssClass="btnDelete" ID="_btnSupprimer" ToolTip="Supprimer" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/pictoTrash.png" Width="40" Height="40"
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer cette visa?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnSupprimer_Click" />
                <asp:ImageButton CssClass="btnSave" ID="_btnValider" ToolTip="Enregistrer" runat="server" ImageUrl="~/Images/imagesBack/pictoSave.png" Width="40" Height="40"
                    OnClick="_btnValider_Click" OnClientClick="loading(true)" />
            </div>
        </div>
        <div class="formulaire" style="margin-left: 55px; width: 430px; height: 588px">
            <div>
                <label style="margin-bottom: 3px; display: inline-block;">Date *</label><br />
                <div style="display: inline-block;">
                    <asp:DropDownList ID="_ddlDay" runat="server" Height="30" />
                    <asp:RequiredFieldValidator ID="_rfvDay" CssClass="required" runat="server" ControlToValidate="_ddlDay" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                </div>
                <div style="display: inline-block; margin-left: 16px">
                    <asp:DropDownList ID="_ddlMonth" runat="server" Height="30" />
                    <asp:RequiredFieldValidator ID="_rfvMonth" CssClass="required" runat="server" ControlToValidate="_ddlMonth" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                </div>
                <div style="display: inline-block; margin-left: 16px">
                    <asp:DropDownList ID="_ddlYear" runat="server" Height="30" />
                    <asp:RequiredFieldValidator ID="_rfvYear" CssClass="required" runat="server" ControlToValidate="_ddlYear" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                </div>
                <div style="margin-top: 10px">
                    <label style="margin-bottom: 3px; display: inline-block;">Validité *</label>
                    <div>
                        <telerik:RadNumericTextBox ID="_txtValidite" runat="server" NumberFormat-DecimalDigits="0" MinValue="1"
                            Width="60px" Height="25px" Type="Number" ShowSpinButtons="false" />&nbsp;Jours
                            <asp:RequiredFieldValidator ID="_rfvValidite" CssClass="required" runat="server" ControlToValidate="_txtValidite" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
            </div>
            <br />
            <div style="margin-bottom: 2px; background-color: #f9f9f9; padding: 8px 10px; height: 425px; width: 480px;">
                <label style="margin-bottom: 3px; display: inline-block;">Visa<span style="font-weight: normal; font-style: italic;">(au format image .jpeg,.jpg,.png)</span></label>
                <div class="attachment-container">
                    <telerik:RadAsyncUpload ID="AsyncUpload" OnClientFileUploaded="OnClientFileUploaded" MaxFileInputsCount="1" RenderMode="Lightweight" runat="server" CssClass="async-attachment"
                        HideFileInput="true" AllowedFileExtensions=".jpeg,.jpg,.png" />
                </div>
                <asp:ImageButton ID="btnAttachFiles" runat="server" AutoPostBack="true" ClientIDMode="AutoID" OnClick="btnAttachFiles_Click" Style="visibility: hidden" />
                <asp:Panel ID="pnlImage" runat="server" Style="margin-top: 10px; border: 3px solid #bec1c4; width: 455px; height: 318px; padding: 5px;">
                    <asp:Image ID="img" Visible="false" runat="server" />
                    <asp:Label ID="lblVisa" runat="server" Text="IMAGE VISA" Style="top: 130px; left: 170px; font-size: 22px; font-weight: bold; color: #bec1c4; position: relative;"></asp:Label>
                </asp:Panel>
            </div>
            <telerik:RadNotification runat="server" ID="radNotif" Width="400" Height="100" EnableShadow="true" RenderMode="Classic" Position="Center" Animation="Fade" AutoCloseDelay="0"></telerik:RadNotification>
        </div>
    </div>
    <telerik:RadWindowManager ID="_rwmEdit" runat="server" Modal="true"
        ShowContentDuringLoad="false" KeepInScreenBounds="true" IconUrl="null" EnableShadow="true"
        ReloadOnShow="true" VisibleStatusbar="false" VisibleOnPageLoad="false" Behaviors="Close, Move, Minimize">
        <Windows>
            <telerik:RadWindow ID="_rwEdit" runat="server" Width="550px" Height="290px" />
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadAjaxManagerProxy ID="AjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="_btnValider">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_btnSupprimer">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_btnVisualize">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAttachFiles">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlImage" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>
