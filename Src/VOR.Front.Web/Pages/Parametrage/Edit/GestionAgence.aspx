<%@ Page Language="C#" MasterPageFile="~/PopIn.Master" CodeBehind="GestionAgence.aspx.cs" AutoEventWireup="true" Inherits="VOR.Front.Web.Pages.Parametrage.Edit.GestionAgence" %>

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
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer cet agence?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnSupprimer_Click" />
                <asp:ImageButton ID="_btnValider" ToolTip="Enregistrer" runat="server" ImageUrl="~/Images/imagesBack/pictoSave.png" Width="40" Height="40"
                    OnClick="_btnValider_Click" OnClientClick="loading(true)" />
            </div>
        </div>
        <div class="formulaire" style="padding: 20px; margin-left: 55px; width: 400px; height: 318px">
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Nom (*)</label>
                <div>
                    <asp:TextBox ID="_txtNom" runat="server" MaxLength="100" Width="352px" Height="27px" />
                    <asp:RequiredFieldValidator ID="_rfvNom" runat="server" ControlToValidate="_txtNom" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Type d'agence (*)</label>
                <div>
                    <asp:DropDownList ID="_ddlTypeAgence" runat="server" Width="360px" Height="35px"
                        AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="_rfvTypeAgence" CssClass="required" runat="server" ControlToValidate="_ddlTypeAgence" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Description</label>
                <div>
                    <asp:TextBox ID="_txtDescription" runat="server" MaxLength="100" Width="352px" Height="27px" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Adresse</label>
                <div>
                    <asp:TextBox ID="_txtAdresse" runat="server" MaxLength="100" Width="352px" Height="27px" />
                </div>
            </div>
            <div>
                <label style="margin-bottom: 3px; display: inline-block;">Téléphone</label>
                <div>
                    <asp:TextBox ID="_txtTelephone" runat="server" MaxLength="100" Width="352px" Height="27px" />
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
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>
