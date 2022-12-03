<%@ Page Language="C#" MasterPageFile="~/PopIn.Master" CodeBehind="GestionUtilisateur.aspx.cs" AutoEventWireup="true" Inherits="VOR.Front.Web.Pages.Agence.Edit.GestionUtilisateur" %>

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
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer cet utilisateur?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnSupprimer_Click" />
                <asp:ImageButton ID="_btnValider" ToolTip="Enregistrer" runat="server" ImageUrl="~/Images/imagesBack/pictoSave.png" Width="40" Height="40"
                    OnClick="_btnValider_Click" OnClientClick="loading(true)" />
            </div>
        </div>
        <div class="formulaire" style="padding: 20px 20px; margin: auto; width: 450px; height: 508px">
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Nom (*)</label>
                <div>
                    <asp:TextBox ID="_txtNom" runat="server" MaxLength="100" Width="420px" Height="27px" />
                    <asp:RequiredFieldValidator ID="_rfvNom" runat="server" ControlToValidate="_txtNom" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Prenom (*)</label>
                <div>
                    <asp:TextBox ID="_txtPrenom" runat="server" MaxLength="100" Width="420px" Height="27px" />
                    <asp:RequiredFieldValidator ID="_rfvPrenom" runat="server" ControlToValidate="_txtPrenom" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Téléphone</label>
                <div>
                    <asp:TextBox ID="_txtTelephone" runat="server" MaxLength="15" Width="420px" Height="27px" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Login (*)</label>
                <div>
                    <asp:TextBox ID="_txtLoginUser" runat="server" MaxLength="50" Width="420px" Height="27px" />
                    <asp:RequiredFieldValidator ID="_rfvLoginUser" runat="server" ControlToValidate="_txtLoginUser" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Mot de passe (*)</label>
                <div>
                    <asp:TextBox ID="_txtPwdUser" TextMode="Password" runat="server" MaxLength="50" Width="422px" Height="30px" />
                    <asp:RequiredFieldValidator ID="_rfvPwdUser" runat="server" ControlToValidate="_txtPwdUser" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Confirmer Mot de passe (*)</label>
                <div>
                    <asp:TextBox ID="_txtConfirmPwd" TextMode="Password" runat="server" MaxLength="50" Width="422px" Height="30px" />
                    <asp:RequiredFieldValidator ID="_rfvConfirmPwd" runat="server" ControlToValidate="_txtConfirmPwd" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                </div>
            </div>
            <div style="margin-bottom: 10px;">
                <label style="margin-bottom: 3px; display: inline-block;">Type utilisateur (*)</label>
                <div>
                    <asp:DropDownList ID="_ddlTypeUtilisateur" runat="server" Width="425px" Height="35px"
                        AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="_rfvTypeUtilisateur" CssClass="required" runat="server" ControlToValidate="_ddlTypeUtilisateur" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                </div>
            </div>
            <div>
                <label style="margin-bottom: 3px; display: inline-block;">Agence (*)</label>
                <div>
                    <asp:DropDownList ID="_ddlAgence" runat="server" Width="425px" Height="35px"
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
