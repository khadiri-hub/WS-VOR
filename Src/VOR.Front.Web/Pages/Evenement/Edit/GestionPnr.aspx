<%@ Page Language="C#" MasterPageFile="~/PopIn.Master" CodeBehind="GestionPnr.aspx.cs" AutoEventWireup="true" Inherits="VOR.Front.Web.Pages.Evenement.Edit.GestionPnr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function validateFields() {
                $(".required").prev().removeClass("requiredField");
                $(".required:visible").prev().addClass("requiredField");
                $(".regExp").prev().prev().removeClass("requiredField");
                $(".regExp:visible").prev().prev().addClass("requiredField");
            }
        </script>
        <style type="text/css">
            .header {
                height: 60px;
                line-height: 60px;
                padding: 0 20px;
                max-width: 400px;
                margin: auto;
                color: #fff;
                background: transparent url("Images/sprite.png") no-repeat 100% 9px;
                font-size: 18px;
            }

            #example .demo-container {
                margin-top: 0;
                padding: 20px;
                border: none;
                max-width: 400px;
            }

            .background-default .header {
                background-color: #1976d2;
            }

            .background-black .header {
                background-color: #9eda29;
                background-position: 100% -111px;
                color: black;
            }

            .background-blackmetrotouch .header {
                background-color: #0082cc;
            }

            .background-bootstrap .header {
                background-color: #337ab7;
            }

            .background-glow .header {
                background-color: #ffa915;
                background-position: 100% -51px;
                color: #202020;
            }

            .background-metro .header,
            .background-metrotouch .header {
                background-color: #25a0da;
            }

            .background-material .header {
                background-color: #03a9f4;
            }

            .background-office2007 .header {
                background-color: #00156e;
            }

            .background-office2010black .header {
                background-color: #f7c840;
            }

            .background-office2010blue .header {
                background-color: #3a71b7;
            }

            .background-office2010silver .header {
                background-color: #f6b700;
            }

            .background-outlook .header {
                background-color: #306ac5;
            }

            .background-silk .header {
                background-color: #2dabc1;
            }

            .background-simple .header {
                background-color: #882501;
            }

            .background-sunset .header {
                background-color: #982e00;
            }

            .background-telerik .header {
                background-color: #63ac39;
            }

            .background-vista .header {
                background-color: #3c7fb1;
            }

            .background-web20 .header {
                background-color: #12398a;
            }

            .background-webblue .header {
                background-color: #0e3d4f;
            }

            .background-windows7 .header {
                background-color: #4c93cc;
            }

            .demo-container hr {
                margin-bottom: 15px;
            }

            #timeslot {
                padding: 5px 0 15px 40px;
            }

                #timeslot .RadPicker {
                    margin-top: 10px;
                }

                #timeslot .riLabel {
                    padding-left: 26px;
                }

            .replyTextarea {
                resize: none;
                height: 100px;
                width: 396px;
                margin-bottom: 20px;
            }

            .background-bootstrap .replyTextarea {
                width: 374px;
            }

            #example .RadButton {
                margin-right: 10px;
            }
        </style>
    </telerik:RadCodeBlock>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content" style="background-color: #c7cde2; min-height: 100%;">
        <div class="headerSaveDelete">
            <div style="float: right">
                <asp:ImageButton ID="_btnSupprimer" ToolTip="Supprimer" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/pictoTrash.png" Width="40" Height="40"
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer ce pnr?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnSupprimer_Click" />
                <asp:ImageButton ID="_btnValider" ToolTip="Enregistrer" runat="server" ImageUrl="~/Images/imagesBack/pictoSave.png" Width="40" Height="40"
                    OnClick="_btnValider_Click" OnClientClick="loading(true)" />
            </div>
        </div>
        <asp:Panel ID="_pnlFormumlaire" runat="server" Style="padding: 20px; margin-left: 95px; width: 404px; height: 388px;">
            <div>
                <div style="margin-bottom: 15px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Nom (*)</label>
                    <div>
                        <asp:TextBox ID="_txtNom" runat="server" MaxLength="100" Width="352px" Height="27px" />
                        <asp:RequiredFieldValidator ID="_rfvNom" runat="server" ControlToValidate="_txtNom" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 15px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Vol (*)</label>
                    <div>
                        <asp:DropDownList ID="_ddlVol" runat="server" Width="360px" Height="35px"
                            AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                            <asp:ListItem Value="" Text="---Sélectionnez---" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="_rfvVol" CssClass="required" runat="server" ControlToValidate="_ddlVol" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 15px;">
                    <div style="display: inline-block;">
                        <label style="margin-bottom: 3px; display: inline-block;">Lieu de départ (*)</label><br />
                        <asp:DropDownList ID="_ddlLieuDepart" runat="server" Height="30">
                            <asp:ListItem Value="" Text="---Sélectionnez---" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="_rfvLieuDepart" CssClass="required" runat="server" ControlToValidate="_ddlLieuDepart" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>

                    <div style="display: inline-block; margin-left: 16px">
                        <label style="margin-bottom: 3px; display: inline-block;">Lieu d'arrivée (*)</label><br />
                        <asp:DropDownList ID="_ddlLieuArrivee" runat="server" Height="30">
                            <asp:ListItem Value="" Text="---Sélectionnez---" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="_rfvLieuArrivee" CssClass="required" runat="server" ControlToValidate="_ddlLieuArrivee" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 15px;">
                    <div style="display: inline-block;">
                        <label style="margin-bottom: 3px; display: inline-block;">Heure de départ</label><br />
                        <telerik:RadDateTimePicker ID="_txtHeureDepart" runat="server" Width="180px" />
                    </div>

                    <div style="display: inline-block; margin-left: 6px">
                        <label style="margin-bottom: 3px; display: inline-block;">Heure d'arrivée</label><br />
                        <telerik:RadDateTimePicker ID="_txtHeureArrivee" runat="server" Width="180px" />
                    </div>
                </div>
                <div style="margin-bottom: 20px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Nbr de passagers (*)</label>
                    <div>
                        <telerik:RadNumericTextBox ID="_txtNbrPassagers" runat="server" NumberFormat-DecimalDigits="0" AutoPostBack="true" ClientIDMode="AutoID" OnTextChanged="_txtCaution_TextChanged" IncrementSettings-InterceptArrowKeys="true"
                            Width="60px" Height="25px" Type="Number" Value="0" MinValue="0" ShowSpinButtons="true" />&nbsp;
                        <asp:RequiredFieldValidator ID="_rfvNbrPassagers" CssClass="required" runat="server" ControlToValidate="_txtNbrPassagers" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 5px;">
                    <div style="display: inline-block;">
                        <label style="margin-bottom: 3px; display: inline-block;">Prix Total (*)</label><br />
                        <telerik:RadNumericTextBox ID="_txtPrixTotal" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true"
                            Width="60px" Height="25px" Type="Number" Value="0" MinValue="0" ShowSpinButtons="false" />&nbsp;DHS
                        <asp:RequiredFieldValidator ID="_rfvPrixTotal" CssClass="required" runat="server" ControlToValidate="_txtPrixTotal" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                    <div style="display: inline-block; margin-left: 20px">
                        <label style="margin-bottom: 3px; display: inline-block;">Caution Déposée (*)</label><br />
                        <telerik:RadNumericTextBox ID="_txtCaution" runat="server" NumberFormat-DecimalDigits="0" AutoPostBack="true" ClientIDMode="AutoID" OnTextChanged="_txtCaution_TextChanged" IncrementSettings-InterceptArrowKeys="true"
                            Width="60px" Height="25px" Type="Number" Value="0" MinValue="0" ShowSpinButtons="false" />&nbsp;DHS
                        <asp:RequiredFieldValidator ID="_rfvCaution" CssClass="required" runat="server" ControlToValidate="_txtCaution" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                    <div style="display: inline-block; margin-left: 20px">
                        <label style="margin-bottom: 3px; display: inline-block;">Reste à payer (*)</label><br />
                        <telerik:RadNumericTextBox ID="_txtReste" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true"
                            Width="60px" Height="25px" Type="Number" Value="0" MinValue="0" ShowSpinButtons="false" />&nbsp;DHS
                        <asp:RequiredFieldValidator ID="_rfvReste" CssClass="required" runat="server" ControlToValidate="_txtReste" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                <telerik:RadNotification runat="server" ID="radNotif" Width="400" Height="100" EnableShadow="true" RenderMode="Classic" Position="Center" Animation="Fade" AutoCloseDelay="0"></telerik:RadNotification>
            </div>
        </asp:Panel>
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
            <telerik:AjaxSetting AjaxControlID="_txtCaution">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlResteApayer" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_txtNbrPassagers">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>
