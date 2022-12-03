<%@ Page Language="C#" MasterPageFile="~/PopIn.Master" CodeBehind="GestionEvenement.aspx.cs" AutoEventWireup="true" Inherits="VOR.Front.Web.Pages.Evenement.Edit.GestionEvenement" %>

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
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer cet evenement?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnSupprimer_Click" />
                <asp:ImageButton ID="_btnValider" ToolTip="Enregistrer" runat="server" ImageUrl="~/Images/imagesBack/pictoSave.png" Width="40" Height="40"
                    OnClick="_btnValider_Click" OnClientClick="loading(true)" />
            </div>
        </div>
        <div class="formulaire" style="padding: 20px; margin-left: 55px; width: 400px; height: 348px">
            <div style="margin-bottom: 15px;">
                <label style="margin-bottom: 3px; display: inline-block;">Nom (*)</label>
                <div>
                    <asp:TextBox ID="_txtNom" runat="server" MaxLength="100" Width="352px" Height="27px" />
                    <asp:RequiredFieldValidator ID="_rfvNom" runat="server" ControlToValidate="_txtNom" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 15px; float: left">
                <label style="margin-bottom: 3px; display: inline-block;">Date Début (*)</label>
                <div>
                    <telerik:RadDatePicker ID="_radDateDebut" runat="server" MinDate="01/01/1900" MaxDate="01/01/3000" Width="160" Height="27px"
                        Style="display: block" autocomplete="off" />
                    <asp:RequiredFieldValidator ID="_rfvDateDebut" CssClass="required" runat="server" ControlToValidate="_radDateDebut" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 15px; margin-right: 35px; float: right">
                <label style="margin-bottom: 3px; display: inline-block;">Date Fin (*)</label>
                <div>
                    <telerik:RadDatePicker ID="_radDateFin" runat="server" MinDate="01/01/1900" MaxDate="01/01/3000" Width="160" Height="27px"
                        Style="display: block" autocomplete="off" />
                    <asp:RequiredFieldValidator ID="_rfvDateFin" CssClass="required" runat="server" ControlToValidate="_radDateFin" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                </div>
            </div>
            <div class="clear"></div>
            <div style="margin-bottom: 15px;">
                <label style="margin-bottom: 3px; display: inline-block;">Durée (*)</label>
                <div>
                    <telerik:RadNumericTextBox ID="_txtNbrJour" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true"
                        Width="60px" Height="25px" Type="Number" Value="10" MinValue="0" ShowSpinButtons="true" />
                </div>
            </div>
            <div style="margin-bottom: 15px;">
                <label style="margin-bottom: 3px; display: inline-block;">En Cours</label>
                <asp:CheckBox ID="_cbEnCours" Style="position: relative; top: 3px" runat="server" />
            </div>
            <div style="margin-bottom: 15px;">
                <label style="margin-bottom: 3px; display: inline-block;">Pnr (*)</label>
                <div>
                    <asp:DropDownList ID="_ddlPnr" runat="server" Width="360px" Height="35px"
                        AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="_rfvPnr" CssClass="required" runat="server" ControlToValidate="_ddlPnr" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                </div>
            </div>
            <div>
                <label style="margin-bottom: 3px; display: inline-block;">Couleur</label>
                <div>
                    <telerik:RadColorPicker AutoPostBack="true" Enabled="true" ID="RadColorPicker" RenderMode="Lightweight" runat="server" ShowIcon="true" Skin="Metro" PaletteModes="All" />
                </div>
            </div>
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
            <telerik:AjaxSetting AjaxControlID="RadColorPicker">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>
