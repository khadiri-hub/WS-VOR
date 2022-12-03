<%@ Page Language="C#" MasterPageFile="~/PopIn.Master" CodeBehind="GestionChambre.aspx.cs" AutoEventWireup="true" Inherits="VOR.Front.Web.Pages.Evenement.Edit.GestionChambre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            function validateFields() {
                $(".required").prev().removeClass("requiredField");
                $(".required:visible").prev().addClass("requiredField");
                $(".regExp").prev().prev().removeClass("requiredField");
                $(".regExp:visible").prev().prev().addClass("requiredField");
            }

            function setWindowSize(width, height) {
                var wnd = GetRadWindow();
                if (width) wnd.set_width(width);
                if (height) wnd.set_height(height);
                wnd.Center();
            }

        </script>
    </telerik:RadScriptBlock>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server" ID="content" Style="background-color: #c7cde2; min-height: 100%;">
        <div class="headerSaveDelete">
            <div style="float: right">
                <asp:ImageButton ID="_btnSupprimer" ToolTip="Supprimer" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/pictoTrash.png" Width="40" Height="40"
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer cette chambre?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnSupprimer_Click" />
                <asp:ImageButton ID="_btnValider" ToolTip="Enregistrer" runat="server" ImageUrl="~/Images/imagesBack/pictoSave.png" Width="40" Height="40"
                    OnClick="_btnValider_Click" OnClientClick="loading(true)" />
            </div>
        </div>
        <asp:Panel ID="_pnlFormumlaire" runat="server" Style="padding: 20px; margin-left: 95px; width: 300px; height: 480px">
            <div>
                <div runat="server" id="divNom" visible="false" style="margin-bottom: 15px;">
                    <div style="margin-bottom: 15px;">
                        <label style="margin-bottom: 3px; display: inline-block;">Nom (*)</label>
                        <div>
                            <asp:TextBox ID="_txtNom" runat="server" MaxLength="100" Width="290px" Height="27px" />
                            <asp:RequiredFieldValidator ID="_rfvNom" runat="server" ControlToValidate="_txtNom" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                        </div>
                    </div>
                    <div style="margin-bottom: 15px;">
                        <label style="margin-bottom: 3px; display: inline-block;">Numero</label>
                        <div>
                            <asp:TextBox ID="_txtNumero" runat="server" MaxLength="100" Width="290px" Height="27px" />
                        </div>
                    </div>
                </div>
                <div runat="server" id="divEvenement" visible="false" style="margin-bottom: 15px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Evenement</label>
                    <asp:DropDownList ID="_ddlEvenement" runat="server" Width="300px" Height="30" OnSelectedIndexChanged="_ddlEvenement_SelectedIndexChanged"
                        AppendDataBoundItems="true" AutoPostBack="True" ClientIDMode="AutoID" Font-Size="13px">
                    </asp:DropDownList>
                </div>
                <div id="divDetailGeneration" runat="server" style="margin-bottom: 15px;">
                    <div style="margin-bottom: 15px;">
                        <label style="margin-bottom: 3px; display: inline-block;">Programme (*)</label>
                        <div>
                            <asp:DropDownList ID="_ddlProgramme" runat="server" Width="300px" Height="30" OnSelectedIndexChanged="_ddlProgramme_SelectedIndexChanged"
                                AppendDataBoundItems="true" AutoPostBack="True" ClientIDMode="AutoID" Font-Size="13px">
                                <asp:ListItem Value="" Text="---Sélectionnez---" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="_rfvProgramme" CssClass="required" runat="server" ControlToValidate="_ddlProgramme" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                        </div>
                    </div>
                    <div style="margin-bottom: 15px;">
                        <label style="margin-bottom: 3px; display: inline-block;">Hotel (*)</label>
                        <div>
                            <asp:DropDownList ID="_ddlHotel" runat="server" Width="300px" Height="30"
                                AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                                <asp:ListItem Value="" Text="---Sélectionnez---" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="_rfvHotel" CssClass="required" runat="server" ControlToValidate="_ddlHotel" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                        </div>
                    </div>
                </div>
                <div style="margin-bottom: 20px;" runat="server" id="typeNbrChambre">
                    <label style="margin-bottom: 3px; display: inline-block;">Type de Chambre (*)</label>
                    <div>
                        <asp:DropDownList ID="_ddlTypeChambre" runat="server" Width="300px" Height="30"
                            AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="_rfvChambre" CssClass="required" runat="server" ControlToValidate="_ddlTypeChambre" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 20px;" runat="server" id="prixChambre">
                    <label style="margin-bottom: 3px; display: inline-block;">Prix Chambre RS (*)</label>
                    <div>
                       <div>
                            <asp:TextBox ID="_txtPrixChambre" runat="server" MaxLength="100" Width="290px" Height="27px" />
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="required" runat="server" ControlToValidate="_txtPrixChambre" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                 <div style="margin-bottom: 20px;" runat="server" id="nbrNuitees">
                    <label style="margin-bottom: 3px; display: inline-block;">Nombre de nuitées (*)</label>
                    <div>
                       <div>
                            <asp:TextBox ID="_txtNbrNuitees" runat="server" MaxLength="100" Width="290px" Height="27px" />
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="required" runat="server" ControlToValidate="_txtNbrNuitees" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 15px;">
                    <div style="margin-bottom: 15px; float: left">
                        <label style="margin-bottom: 3px; display: inline-block;">Nbr de chambre (*)</label>
                        <div>
                            <telerik:RadNumericTextBox ID="_txtNbrChambres" runat="server" NumberFormat-DecimalDigits="0" AutoPostBack="true" ClientIDMode="AutoID" IncrementSettings-InterceptArrowKeys="true"
                                Width="60px" Height="25px" Type="Number" Value="0" MinValue="0" ShowSpinButtons="true" />&nbsp;
                                <asp:RequiredFieldValidator ID="_rfvNbrChambres" CssClass="required" runat="server" ControlToValidate="_txtNbrChambres" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                        </div>
                    </div>
                    <%--                    <div style="margin-bottom: 15px; float: right">
                        <label style="margin-bottom: 3px; display: inline-block;">Couleur</label><br />
                        <telerik:RadColorPicker ID="RadColorPicker" RenderMode="Lightweight" AutoPostBack="True" runat="server" ShowIcon="true" Skin="Metro" PaletteModes="All" />
                    </div>--%>
                </div>
                <div class="clear"></div>
                <div style="margin-bottom: 20px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Agence (*)</label>
                    <div>
                        <asp:DropDownList ID="_ddlAgence" runat="server" Width="300px" Height="30"
                            AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                            <asp:ListItem Value="" Text="---Sélectionnez---" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="_rfvAgence" CssClass="required" runat="server" ControlToValidate="_ddlAgence" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                <telerik:RadNotification runat="server" ID="radNotif" Width="400" Height="100" EnableShadow="true" RenderMode="Classic" Position="Center" Animation="Fade" AutoCloseDelay="0"></telerik:RadNotification>
            </div>
        </asp:Panel>
    </asp:Panel>
    <telerik:RadAjaxManagerProxy ID="AjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="_btnValider">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_ddlProgramme">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_ddlEvenement">
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
