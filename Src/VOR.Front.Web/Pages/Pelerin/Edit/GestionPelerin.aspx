<%@ Page Language="C#" MasterPageFile="~/PopIn.Master" CodeBehind="GestionPelerin.aspx.cs" AutoEventWireup="true" Inherits="VOR.Front.Web.Pages.Pelerin.Edit.GestionPelerin" %>

<%@ Register Src="~/UserControls/Evenement/HotelsUC.ascx" TagName="HotelsUC" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Pelerin/AccompagnantsUC.ascx" TagName="AccompagnantsUC" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Pelerin/CommercialUC.ascx" TagName="CommercialUC" TagPrefix="uc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #content {
            width: 868px;
            height: 700px;
            background-color: #c7cde2;
        }

        #right, #left, #center {
            height: 632px;
            background-color: #e9e9e9;
            padding: 20px;
        }

        #right {
            float: right;
            width: 28%;
        }

        #left {
            float: left;
            width: 25%;
            margin-right: 22px;
        }

        #right, #center {
            border-left: 1px solid #fff;
        }

        #left {
            border-right: 1px solid #fff;
        }

        #center {
            margin-left: 5px;
        }

        .upper {
            text-transform: uppercase !important;
        }

        .rdoList label {
            display: inline;
        }
    </style>
    <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
        <script type="text/javascript">

            function validateFields() {
                $(".required").prev().removeClass("requiredField");
                $(".required:visible").prev().addClass("requiredField");
                $(".regExp").prev().prev().removeClass("requiredField");
                $(".regExp:visible").prev().prev().addClass("requiredField");
            }

            function updatePhoto(img) {
                var base64 = img.split(',')[1];
                $("#<%=_hdnPhotoPerson.ClientID%>").val(base64);
                $("#<%=_imgPhotoPerson.ClientID%>").attr("src", img);
            }

        </script>
    </telerik:RadScriptBlock>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content" runat="server" style="min-height: 100%;">
        <div class="headerSaveDelete">
            <div style="float: right">
                <asp:ImageButton ID="_btnSupprimer" ToolTip="Supprimer" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/pictoTrash.png" Width="40" Height="40" Style="cursor: pointer"
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer ce pelerin?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnSupprimer_Click" />
                <asp:ImageButton ID="_btnValider" ToolTip="Enregistrer" runat="server" ImageUrl="~/Images/imagesBack/pictoSave.png" Width="" Height="40" Style="cursor: pointer"
                    OnClick="_btnValider_Click" ClientIDMode="AutoID" />
            </div>
            <div style="margin: auto; width: 45%; position: relative; top: -5px;">
                <div style="display: inline">
                    <asp:Label ID="lblTransportPaye" runat="server" Style="display: inline; font-size: 16px; font-weight: bold; color: red">Transport payé?</asp:Label>
                    <asp:RadioButtonList ID="_rdoTransportList" Style="display: inline; position: relative; top: 3px" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="true" Text="Oui"></asp:ListItem>
                        <asp:ListItem Value="false" Selected="true" Text="Non"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div style="display: inline; margin-left: 50px">
                    <asp:Label ID="Label1" runat="server" Style="display: inline; font-size: 16px; font-weight: bold; color: red">Repas payé?</asp:Label>
                    <asp:RadioButtonList ID="_rdoRepasList" Style="display: inline; position: relative; top: 3px" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="true" Text="Oui"></asp:ListItem>
                        <asp:ListItem Value="false" Selected="true" Text="Non"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

            <div style="margin: auto; width: 45%; position: relative; top: -5px;">
                <div style="display: inline">
                    <asp:Label ID="lblAssuranceMaladie" runat="server" Style="display: inline; font-size: 16px; font-weight: bold; color: red">Assurance maladie?</asp:Label>
                    <asp:RadioButtonList ID="_rdoAssuranceMaladie" Style="display: inline; position: relative; top: 3px" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="true" Text="Oui"></asp:ListItem>
                        <asp:ListItem Value="false" Selected="true" Text="Non"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div style="display: inline; margin-left: 50px">
                    <asp:Label ID="lblStop" runat="server" Style="display: inline; font-size: 16px; font-weight: bold; color: red">Stop?</asp:Label>
                    <asp:RadioButtonList ID="_rdoStop" Style="display: inline; position: relative; top: 3px" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="true" Text="Oui"></asp:ListItem>
                        <asp:ListItem Value="false" Selected="true" Text="Non"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

        </div>
        <asp:Panel ID="_pnlFormumlaire" runat="server" CssClass="formulaire">
            <div id="right">
                <div style="text-align: center;">
                    <div class="edit-photo">
                        <asp:Image ID="_imgPhotoPerson" runat="server" class="imgborder" Width="180px" Height="210px" />
                        <asp:Panel ID="_pnlEditPhoto" runat="server" class="overlay">
                            <input type="button" id="_btnIDPhotoMaker" class="btnIDPhotoMaker" runat="server" />
                        </asp:Panel>
                    </div>
                    <asp:HiddenField ID="_hdnPhotoPerson" runat="server" />
                </div>
                <h4 style="background-color: #2972e8; color: #fff; text-align: center; padding: 4px;">PAIEMENT</h4>
                <table>
                    <tr>
                        <td style="width: 120px">
                            <div style="margin-bottom: 5px;">
                                <label style="margin-bottom: 3px; display: inline-block;">T.Prix (*)</label>
                                <div>
                                    <telerik:RadNumericTextBox ID="_txtPrix" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true"
                                        Width="60px" Height="25px" Type="Number" ShowSpinButtons="false" />
                                    <asp:RequiredFieldValidator ID="_rfvPrix" CssClass="required" runat="server" ControlToValidate="_txtPrix" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                                </div>
                            </div>
                        </td>
                        <td style="width: 120px">
                            <div style="margin-bottom: 5px;">
                                <label style="margin-bottom: 3px; display: inline-block;">Payé (*)</label>
                                <div>
                                    <telerik:RadNumericTextBox ID="_txtMontantPaye" AutoPostBack="true" ClientIDMode="AutoID" OnTextChanged="_txtMontantPaye_TextChanged" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true"
                                        Width="60px" Height="25px" Type="Number" ShowSpinButtons="false" />

                                    <asp:RequiredFieldValidator ID="_rfvMontantPaye" CssClass="required" runat="server" ControlToValidate="_txtMontantPaye" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                                </div>
                            </div>
                        </td>
                        <td style="width: 120px">
                            <div id="pnlResteApayer" style="margin-bottom: 5px;">
                                <label style="margin-bottom: 3px; display: inline-block;">Reste (*)</label>
                                <div>
                                    <telerik:RadNumericTextBox ID="_txtResteAPayer" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true"
                                        Width="60px" Height="25px" Type="Number" ShowSpinButtons="false" />

                                    <asp:RequiredFieldValidator ID="_rfvResteAPayer" CssClass="required" runat="server" ControlToValidate="_txtResteAPayer" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                                </div>
                            </div>
                        </td>
                        <td style="width: 120px">
                            <div style="margin-bottom: 5px;">
                                <label style="margin-bottom: 3px; display: inline-block;">Type pèlerin</label>
                                <div>
                                    <div style="display: inline-block; margin-left: 2px">
                                        <asp:DropDownList ID="_dllTypePelerin" runat="server" Height="30" Width="150px" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="required" runat="server" ControlToValidate="_txtMontantPaye" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                                </div>
                            </div>
                        </td>

                    </tr>
                </table>

                <h4 style="background-color: #65bc1e; color: #fff; text-align: center; padding: 4px;">EVALUATION</h4>
                <div style="display: inline-block">
                    <label style="margin-bottom: 3px; display: inline-block;">Evaluation Voyage</label>
                    <div>
                        <telerik:RadNumericTextBox ID="_txtEvaluationVoyage" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true"
                            Width="60px" Height="25px" Type="Number" Value="5" MinValue="0" MaxValue="10" ShowSpinButtons="true" />
                    </div>
                </div>
                <div style="display: inline-block; margin-left: 115px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Evaluation Pelerin</label>
                    <div>
                        <telerik:RadNumericTextBox ID="_txtEvaluationPelerin" runat="server" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="true"
                            Width="60px" Height="25px" Type="Number" Value="5" MinValue="0" MaxValue="10" ShowSpinButtons="true" />
                    </div>
                </div>
                <div style="margin-top: 15px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Commentaire</label>
                    <div>
                        <asp:TextBox ID="_txtCommentaire" runat="server" MaxLength="100" Width="338px" Height="27px" />
                    </div>
                </div>

                <h4 style="background-color: #e82929; color: #fff; text-align: center; padding: 4px;">ALERTE</h4>
                <div style="margin-top: 2px">
                    <div>
                        <asp:DropDownList ID="_ddlTypeAlerte" runat="server" Width="352px" Height="30" AutoPostBack="true"
                            OnSelectedIndexChanged="_ddlTypeAlerte_SelectedIndexChanged" AppendDataBoundItems="true" ClientIDMode="AutoID" Font-Size="13px">
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="margin-top: 2px">
                    <div>
                        <asp:DropDownList ID="_ddlAlerte" runat="server" Width="352px" Height="30" AppendDataBoundItems="true" ClientIDMode="AutoID" Font-Size="13px">
                            <asp:ListItem Value="" Text="---Alerte---" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="margin-top: 2px">
                    <div>
                        <asp:TextBox ID="_txtAlerteDescription" runat="server" Width="342px" Height="20" AppendDataBoundItems="true" ClientIDMode="AutoID" Font-Size="13px">
                      
                        </asp:TextBox>
                    </div>
                </div>
            </div>
            <div id="left">
                <div style="margin-bottom: 10px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Nom Francais (*)</label>
                    <div>
                        <asp:TextBox ID="_txtNomFR" runat="server" MaxLength="100" Width="300px" Height="27px" CssClass="upper" />
                        <asp:RequiredFieldValidator ID="_rfvNomFR" runat="server" ControlToValidate="_txtNomFR" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 10px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Prénom Francais (*)</label>
                    <div>
                        <asp:TextBox runat="server" ID="_txtPrenomFR" Text="" Width="300px" Height="25px" CssClass="upper"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvPrenomFR" CssClass="required" runat="server" ControlToValidate="_txtPrenomFR" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 10px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Nom Arabe (*)</label>
                    <div>
                        <asp:TextBox ID="_txtNomAR" runat="server" MaxLength="100" Width="300px" Height="27px" />
                        <asp:RequiredFieldValidator ID="_rfvNomAR" runat="server" ControlToValidate="_txtNomAR" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 10px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Prénom Arabe (*)</label>
                    <div>
                        <asp:TextBox runat="server" ID="_txtPrenomAR" Text="" Width="300px" Height="25px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="_rfvPrenomAR" CssClass="required" runat="server" ControlToValidate="_txtPrenomAR" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 10px;">
                    <label style="margin-bottom: 3px; display: inline-block;">Date de Naissance (*)</label><br />
                    <div style="display: inline-block;">
                        <asp:DropDownList ID="_ddlBirthDay" runat="server" Height="30" />
                        <asp:RequiredFieldValidator ID="_rfvBirthDay" CssClass="required" runat="server" ControlToValidate="_ddlBirthDay" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                    <div style="display: inline-block; margin-left: 16px">
                        <asp:DropDownList ID="_ddlBirthMonth" runat="server" Height="30" />
                        <asp:RequiredFieldValidator ID="_rfvBirthMonth" CssClass="required" runat="server" ControlToValidate="_ddlBirthMonth" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                    <div style="display: inline-block; margin-left: 16px">
                        <asp:DropDownList ID="_ddlBirthYear" runat="server" Height="30" />
                        <asp:RequiredFieldValidator ID="_rfvBirthYear" CssClass="required" runat="server" ControlToValidate="_ddlBirthYear" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-bottom: 10px;">
                    <table>
                        <tr>
                            <td>
                                <label style="margin-bottom: 3px; display: inline-block;">Etat Civil (*)</label>
                                <div>
                                    <asp:DropDownList ID="_ddlEtatCivil" runat="server" Width="150px" Height="30"
                                        AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="_rfvEtatCivil" CssClass="required" runat="server" ControlToValidate="_ddlEtatCivil" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                                </div>
                            </td>
                            <td>
                                <label style="margin-bottom: 3px; display: inline-block;">Sexe (*)</label>
                                <div>
                                    <asp:DropDownList ID="_ddlSexe" runat="server" Width="150px" Height="30"
                                        AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="_rfvEtatSexe" CssClass="required" runat="server" ControlToValidate="_ddlSexe" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin-bottom: 10px;">
                    <table>
                        <tr>
                            <td>
                                <label style="margin-bottom: 3px; display: inline-block;">Téléphone 1</label>
                                <div>
                                    <asp:TextBox ID="_txtNumTelef1" runat="server" MaxLength="100" Width="140px" Height="25px" />
                                </div>
                            </td>
                            <td>
                                <label style="margin-bottom: 3px; display: inline-block;">Téléphone 2</label>
                                <div>
                                    <asp:TextBox ID="_txtNumTelef2" runat="server" MaxLength="100" Width="140px" Height="25px" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin-bottom: 10px;">
                    <table>
                        <tr>
                            <td>
                                <label style="margin-bottom: 3px; display: inline-block;">Région (*)</label>
                                <div>
                                    <asp:DropDownList ID="_ddlRegion" runat="server" Width="150px" Height="30" OnSelectedIndexChanged="_ddlRegion_SelectedIndexChanged"
                                        AppendDataBoundItems="true" AutoPostBack="True" ClientIDMode="AutoID" Font-Size="13px">
                                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="_rfvRegion" CssClass="required" runat="server" ControlToValidate="_ddlRegion" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                                </div>
                            </td>
                            <td>
                                <label style="margin-bottom: 3px; display: inline-block;">Ville (*)</label>
                                <div>
                                    <asp:DropDownList ID="_ddlVille" runat="server" Width="150px" Height="30" AppendDataBoundItems="true" ClientIDMode="AutoID" Font-Size="13px">
                                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="_rfvVille" CssClass="required" runat="server" ControlToValidate="_ddlVille" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin-bottom: 15px;">
                    <label style="margin-bottom: 3px; display: inline-block;">N° Passeport (*)</label>
                    <label style="margin-left: 30px; margin-bottom: 3px; display: inline-block;">Type vaccin</label>
                    <br />
                    <asp:TextBox ID="_txtNumPassport" runat="server" MaxLength="60" Width="100px" Height="25px" />
                    <asp:RequiredFieldValidator ID="_rfvNumPassport" runat="server" ControlToValidate="_txtNumPassport" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                    <div style="display: inline-block; margin-left: 16px">
                        <asp:DropDownList ID="_dllTypesVaccins" runat="server" Height="30" />
                    </div>

                    <div style="display: inline-block; margin-left: 6px">
                        <asp:TextBox ID="_txtNbrVaccin" runat="server" MaxLength="60" Width="30px" Height="25px" />
                    </div>
                </div>
                <div>
                    <label style="margin-bottom: 3px; display: inline-block;">Date d'expiration (*)</label><br />
                    <div style="display: inline-block;">
                        <asp:DropDownList ID="_ddlExpireDay" runat="server" Height="30" />
                        <asp:RequiredFieldValidator ID="_rfvExpireDay" CssClass="required" runat="server" ControlToValidate="_ddlExpireDay" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                    <div style="display: inline-block; margin-left: 16px">
                        <asp:DropDownList ID="_ddlExpireMonth" runat="server" Height="30" />
                        <asp:RequiredFieldValidator ID="_rfvExpireMonth" CssClass="required" runat="server" ControlToValidate="_ddlExpireMonth" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                    <div style="display: inline-block; margin-left: 16px">
                        <asp:DropDownList ID="_ddlExpireYear" runat="server" Height="30" />
                        <asp:RequiredFieldValidator ID="_rfvExpireYear" CssClass="required" runat="server" ControlToValidate="_ddlExpireYear" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
            </div>
            <div id="center">
                <table>
                    <tr>
                        <td>
                            <div style="margin-bottom: 15px;">
                                <label style="margin-bottom: 3px; display: inline-block;">Evénement (*)</label>
                                <div>
                                    <asp:DropDownList ID="_ddlEvenement" runat="server" Width="212px" Height="30" OnSelectedIndexChanged="_ddlEvenement_SelectedIndexChanged"
                                        AppendDataBoundItems="true" AutoPostBack="True" ClientIDMode="AutoID" Font-Size="13px">
                                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="_rfvEvenement" CssClass="required" runat="server" ControlToValidate="_ddlEvenement" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                                </div>
                            </div>
                        </td>
                        <td style="width: 18px"></td>
                        <td>
                            <div style="margin-bottom: 15px;">
                                <label style="margin-bottom: 3px; display: inline-block;">Programme (*)</label>
                                <div>
                                    <asp:DropDownList ID="_ddlProgramme" runat="server" Width="212px" Height="30" OnSelectedIndexChanged="_ddlProgramme_SelectedIndexChanged"
                                        AppendDataBoundItems="true" AutoPostBack="True" ClientIDMode="AutoID" Font-Size="13px">
                                        <asp:ListItem Value="" Text="---Sélectionnez---" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="_rfvProgramme" CssClass="required" runat="server" ControlToValidate="_ddlProgramme" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="margin-bottom: 15px;">
                                <label style="margin-bottom: 3px; display: inline-block;">Chambre (*)</label>
                                <div>
                                    <asp:DropDownList ID="_ddlChambre" runat="server" Width="212px" Height="30"
                                        AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID" Font-Size="13px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="_rfvChambre" CssClass="required" runat="server" ControlToValidate="_ddlChambre" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                                </div>
                            </div>
                        </td>
                        <td style="width: 18px"></td>
                        <td id="grpPelerin" runat="server" visible="false">
                            <label style="margin-bottom: 3px; display: inline-block;">Relation</label>
                            <div>
                                <telerik:RadComboBox ID="ddlPelerin" runat="server" CheckBoxes="true" EmptyMessage="Sélectionner un pélerin" Style="margin-bottom: 16px" OnSelectedIndexChanged="ddlPelerin_SelectedIndexChanged"
                                    EnableCheckAllItemsCheckBox="true" AutoPostBack="True" Width="166px" DropDownAutoWidth="Enabled" Filter="StartsWith" />
                                <telerik:RadColorPicker Enabled="false" ID="RadColorPicker" RenderMode="Lightweight" runat="server" ShowIcon="true" Skin="Metro" PaletteModes="All"
                                    AutoPostBack="true" OnColorChanged="RadColorPicker_ColorChanged" />
                            </div>
                        </td>
                    </tr>
                </table>
                <div>
                    <uc:hotelsuc id="_hotels" runat="server" />
                    <uc:accompagnantsuc id="_accompagnants" runat="server" />
                    <uc:commercialuc id="_commercial" runat="server" />
                </div>
            </div>
            <div class="clear"></div>
            <telerik:RadNotification runat="server" ID="radNotif" Width="400" Height="100" EnableShadow="true" RenderMode="Classic" Position="Center" Animation="Fade" AutoCloseDelay="0" />
        </asp:Panel>
    </div>
    <telerik:RadAjaxManagerProxy ID="AjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="_ddlProgramme">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_ddlRegion">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_btnValider">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_txtMontantPaye">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlResteApayer" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadColorPicker">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlPelerin">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_ddlTypeAlerte">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>
