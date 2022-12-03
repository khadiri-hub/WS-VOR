<%@ Page Language="C#" MasterPageFile="~/PopIn.Master" CodeBehind="GestionCollaborateur.aspx.cs" AutoEventWireup="true" Inherits="VOR.Front.Web.Pages.Collaborateur.Edit.GestionCollaborateur" %>

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
    <div id="content" style="background-color: #c7cde2">
        <div class="headerSaveDelete">
            <div style="float: right">
                <asp:ImageButton ID="_btnSupprimer" ToolTip="Supprimer" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/pictoTrash.png" Width="40" Height="40"
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer ce collaborateur?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnSupprimer_Click" />
                <asp:ImageButton ID="_btnValider" ToolTip="Enregistrer" runat="server" ImageUrl="~/Images/imagesBack/pictoSave.png" Width="40" Height="40"
                    OnClick="_btnValider_Click" OnClientClick="loading(true)" />
            </div>
        </div>
        <div class="formulaire" style="padding: 20px; margin-left: 55px; width: 400px; height: 448px">
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Nom Arabe (*)</label>
                <div>
                    <asp:TextBox ID="_txtNomAR" runat="server" MaxLength="100" Width="352px" Height="27px" />
                    <asp:RequiredFieldValidator ID="_rfvNomAR" runat="server" ControlToValidate="_txtNomAR" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Prenom Arabe (*)</label>
                <div>
                    <asp:TextBox ID="_txtPrenomAR" runat="server" MaxLength="100" Width="352px" Height="27px" />
                    <asp:RequiredFieldValidator ID="_rfvPrenomAR" runat="server" ControlToValidate="_txtPrenomAR" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Nom Francais (*)</label>
                <div>
                    <asp:TextBox ID="_txtNomFR" runat="server" MaxLength="100" Width="352px" Height="27px" />
                    <asp:RequiredFieldValidator ID="_rfvNomFR" runat="server" ControlToValidate="_txtNomFR" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Prenom Francais (*)</label>
                <div>
                    <asp:TextBox ID="_txtPrenomFR" runat="server" MaxLength="100" Width="352px" Height="27px" />
                    <asp:RequiredFieldValidator ID="_rfvPrenomFR" runat="server" ControlToValidate="_txtPrenomFR" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Téléphone</label>
                <div>
                    <asp:TextBox ID="_txtTelephone" runat="server" MaxLength="15" Width="352px" Height="27px" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Type personne (*)</label>
                <div>
                    <asp:DropDownList ID="_ddlTypePersonne" runat="server" Width="360px" Height="35px"
                        AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="_rfvTypePersonne" CssClass="required" runat="server" ControlToValidate="_ddlTypePersonne" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                </div>
            </div>
            <div>
                <label style="margin-bottom: 3px; display: inline-block;">Agence (*)</label>
                <div>
                    <asp:DropDownList ID="_ddlAgence" runat="server" Width="360px" Height="35px"
                        AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="_rfvAgence" CssClass="required" runat="server" ControlToValidate="_ddlAgence" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
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
