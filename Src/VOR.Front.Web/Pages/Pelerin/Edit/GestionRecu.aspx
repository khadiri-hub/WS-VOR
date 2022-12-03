<%@ Page Language="C#" MasterPageFile="~/PopIn.Master" CodeBehind="GestionRecu.aspx.cs" AutoEventWireup="true"
    Inherits="VOR.Front.Web.Pages.Evenement.Edit.GestionRecu" %>

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
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer ce recu?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnSupprimer_Click" />
                <asp:ImageButton CssClass="btnSave" ID="_btnValider" ToolTip="Enregistrer" runat="server" ImageUrl="~/Images/imagesBack/pictoSave.png" Width="40" Height="40"
                    OnClick="_btnValider_Click" OnClientClick="loading(true)" />
            </div>
        </div>
        <div class="formulaire" style="margin-left: 55px; width: 830px; height: 788px">
            <div style="float: left; margin-right: 20px;">
                <div>
                    <label style="margin-bottom: 3px; display: inline-block;">Numero du recu (*)</label>
                    <div>
                        <asp:TextBox ID="_txtNumRecu" runat="server" MaxLength="100" Width="190px" Height="27px" />
                        <asp:RequiredFieldValidator ID="_rfvNumRecu" runat="server" ControlToValidate="_txtNumRecu" CssClass="required" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="false" Style="position: absolute;" />
                    </div>
                </div>
                <div style="margin-top: 20px">
                    <label style="margin-bottom: 3px; display: inline-block;">Montant du recu (*)</label>
                    <div>
                        <telerik:RadNumericTextBox ID="_txtMontant" runat="server" Width="200px" Height="35px" Type="Number" ShowSpinButtons="false" />&nbsp;DHS
                    <asp:RequiredFieldValidator ID="_rfvMontantRecu" CssClass="required" runat="server" ControlToValidate="_txtMontant" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                    </div>
                </div>
            </div>
            <div style="float: right; background-color: #f9f9f9; padding: 8px 10px; height: 425px; width: 480px;">
                <label style="display: inline-block;">Recu<span style="font-weight: normal; font-style: italic;">&nbsp;(au format image .jpeg,.jpg,.png)</span></label>
                <div class="attachment-container">
                    <telerik:RadAsyncUpload ID="AsyncUpload" OnClientFileUploaded="OnClientFileUploaded" MaxFileInputsCount="1" RenderMode="Lightweight" runat="server" CssClass="async-attachment"
                        HideFileInput="true" AllowedFileExtensions=".jpeg,.jpg,.png" />
                </div>
                <asp:ImageButton ID="btnAttachFiles" runat="server" AutoPostBack="true" ClientIDMode="AutoID" OnClick="btnAttachFiles_Click" Style="visibility: hidden;" />
                <asp:Panel ID="pnlImage" runat="server" Style="border: 3px solid #bec1c4; width: 455px; height: 318px; padding: 5px;">
                    <asp:Image ID="img" Visible="false" runat="server" />
                    <asp:Label ID="lblRecu" runat="server" Text="IMAGE RECU" Style="top: 130px; left: 170px; font-size: 22px; font-weight: bold; color: #bec1c4; position: relative;"></asp:Label>
                </asp:Panel>
            </div>

            <div class="clear"></div>
            <div style="float: right; margin-top: 25px;">
                <asp:ImageButton ID="btnNew" runat="server" ImageUrl="~/Images/imagesBack/Add.png" Style="cursor: pointer" ClientIDMode="AutoID" OnClick="btnNew_Click" />
            </div>
            <div style="margin-top: 50px">
                <telerik:RadGrid ID="gridRecu" runat="server"
                    CellSpacing="0" AllowPaging="True" GridLines="None" OnNeedDataSource="gridRecu_NeedDataSource" OnItemCommand="gridRecu_ItemCommand" OnItemDataBound="gridRecu_ItemDataBound">
                    <ExportSettings HideStructureColumns="true" />
                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="false" />
                    <ClientSettings EnablePostBackOnRowClick="true">
                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" ScrollHeight="200px" />
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
                    <MasterTableView PageSize="10" AutoGenerateColumns="False" Height="100%" AllowSorting="True" NoMasterRecordsText="Aucun recu existant" DataKeyNames="ID" CommandItemDisplay="Top">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="Numero" SortExpression="Numero" UniqueName="Numero" HeaderText="Recu N°" HeaderStyle-Width="100px"
                                ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn HeaderText="Montant" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblMontant" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Crée le" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCreation" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Modifié le" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblModification" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
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
            <telerik:AjaxSetting AjaxControlID="gridRecu">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnNew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="content" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
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
            <telerik:AjaxSetting AjaxControlID="btnAttachFiles">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlImage" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>
