<%@ Page Language="C#" MasterPageFile="~/PopIn.Master" CodeBehind="GestionHotel.aspx.cs" AutoEventWireup="true" Inherits="VOR.Front.Web.Pages.Evenement.Edit.GestionHotel" %>

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
    <div id="content"  style="background-color: #c7cde2; min-height: 100%;">
        <div class="headerSaveDelete">
            <div style="float: right">
                <asp:ImageButton ID="_btnSupprimer" ToolTip="Supprimer" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/pictoTrash.png" Width="40" Height="40"
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer cet hôtel?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnSupprimer_Click" />
                <asp:ImageButton ID="_btnValider" ToolTip="Enregistrer" runat="server" ImageUrl="~/Images/imagesBack/pictoSave.png" Width="40" Height="40"
                    OnClick="_btnValider_Click" OnClientClick="loading(true)" />
            </div>
        </div>
        <div class="formulaire" style="padding: 20px; margin-left: 95px; width: 404px; height: 318px;">
            <div>
                <div style="margin-bottom: 15px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Nom AR (*)</label>
                    <div>
                        <asp:TextBox ID="_txtNomAR" runat="server" MaxLength="100" Width="352px" Height="27px" />
                        <asp:RequiredFieldValidator ID="_rfvNomAR" runat="server" ControlToValidate="_txtNomAR" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 15px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Nom FR (*)</label>
                    <div>
                        <asp:TextBox ID="_txtNomFR" runat="server" MaxLength="100" Width="352px" Height="27px" />
                        <asp:RequiredFieldValidator ID="_rfvNomFR" runat="server" ControlToValidate="_txtNomFR" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 15px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Categorie (*)</label>
                    <div>
                        <telerik:RadNumericTextBox ID="_txtCategorie" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true"
                            Width="60px" Height="25px" Type="Number" Value="3" MinValue="3" MaxValue="5" ShowSpinButtons="true" />
                    </div>
                </div>
                <div style="margin-bottom: 15px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Distance du Haram (*)</label>
                    <div>
                        <telerik:RadNumericTextBox ID="_txtDistance" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true"
                            Width="60px" Height="25px" Type="Number" Value="100" MinValue="0" ShowSpinButtons="false" />
                        Mêtres
                    </div>
                </div>
                <div>
                    <label style="margin-bottom: 3px; display: inline-block;">Ville (*)</label>
                    <div>
                        <asp:DropDownList ID="_ddlVille" runat="server" Width="360px" Height="35px"
                            AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                            <asp:ListItem Value="" Text="---Sélectionnez---" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="_rfvVille" CssClass="required" runat="server" ControlToValidate="_ddlVille" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                <telerik:RadNotification runat="server" ID="radNotif" Width="400" Height="100" EnableShadow="true" RenderMode="Classic" Position="Center" Animation="Fade" AutoCloseDelay="0"></telerik:RadNotification>
            </div>
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
