<%@ Page Language="C#" MasterPageFile="~/PopIn.Master" CodeBehind="GestionProgramme.aspx.cs" AutoEventWireup="true" Inherits="VOR.Front.Web.Pages.Evenement.Edit.GestionProgramme" %>

<%@ Register Src="~/UserControls/Evenement/HotelsUC.ascx" TagName="HotelsUC" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function validateFields() {
            $(".required").prev().removeClass("requiredField");
            $(".required:visible").prev().addClass("requiredField");
            $(".regExp").prev().prev().removeClass("requiredField");
            $(".regExp:visible").prev().prev().addClass("requiredField");
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content" style="background-color: #c7cde2; min-height: 100%;">
        <div class="headerSaveDelete">
            <div style="float: right">
                <asp:ImageButton ID="_btnSupprimer" ToolTip="Supprimer" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/pictoTrash.png" Width="40" Height="40"
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer ce programme?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnSupprimer_Click" />
                <asp:ImageButton ID="_btnValider" ToolTip="Enregistrer" runat="server" ImageUrl="~/Images/imagesBack/pictoSave.png" Width="40" Height="40"
                    OnClick="_btnValider_Click" OnClientClick="loading(true)" />
            </div>
        </div>
        <div class="formulaire" style="padding: 20px; margin-left: 55px; width: 499px; height: 483px">
            <div style="margin-bottom: 15px;">
                <label style="margin-bottom: 3px; display: inline-block;">Nom (*)</label>
                <div>
                    <asp:TextBox ID="_txtNom" runat="server" MaxLength="100" Width="450px" Height="27px" />
                    <asp:RequiredFieldValidator ID="_rfvNom" runat="server" ControlToValidate="_txtNom" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 15px;">
                <label style="margin-bottom: 3px; display: inline-block;">Prix à partir De (*)</label>
                <div>
                    <telerik:RadNumericTextBox ID="_txtPrix" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true"
                        Width="60px" Height="25px" Type="Number" Value="0" MinValue="0" ShowSpinButtons="false" />
                    DHS
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Evenement</label>
                <div>
                    <asp:DropDownList ID="_ddlEvenement" runat="server" Width="455px" Height="35px"
                        AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                    </asp:DropDownList>
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Type de programme</label>
                <div>
                    <asp:DropDownList ID="_ddlTypeProgramme" runat="server" Width="455px" Height="35px"
                        AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                    </asp:DropDownList>
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Vol</label>
                <div>
                    <asp:DropDownList ID="_ddlVol" runat="server" Width="455px" Height="35px"
                        AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                    </asp:DropDownList>
                </div>
            </div>

            <uc:HotelsUC id="_hotels" runat="server" />
            <telerik:RadNotification runat="server" ID="radNotif" Width="400" Height="100" EnableShadow="true" RenderMode="Classic" Position="Center" Animation="Fade" AutoCloseDelay="0"></telerik:RadNotification>
        </div>
    </div>

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
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>
