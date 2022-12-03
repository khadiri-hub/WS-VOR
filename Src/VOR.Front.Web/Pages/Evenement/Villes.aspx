<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Villes.aspx.cs" Inherits="VOR.Front.Web.Pages.Evenement.Villes" %>

<asp:Content ID="content1" ContentPlaceHolderID="HeadContent" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function onRequestStart(sender, args) {
                if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0)
                    args.set_enableAjax(false);
                else
                    args.set_enableAjax(true);
            }
            function OnClientClose(args) {
                DisplayMsgInfo(args);
                setTimeout(function () {
                    rebindGrid();
                }, 1000);
            }
            function rebindGrid() {
                var masterTable = $find("<%= this.gridVille.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bottons">
        <img id="btnNew" alt="" runat="server" src="~/Images/imagesBack/pictoAdd.png" style="cursor: pointer" width="30" height="30" />
    </div>
    <div class="clear"></div>
    <div style="padding: 10px;height:610px">
        <telerik:RadGrid ID="gridVille" runat="server"
            CellSpacing="0" AllowPaging="True" GridLines="None" OnNeedDataSource="gridVille_NeedDataSource" OnItemDataBound="gridVille_ItemDataBound">
            <ExportSettings HideStructureColumns="true" />
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />
            <MasterTableView PageSize="10" AutoGenerateColumns="False" Height="100%" AllowSorting="True" NoMasterRecordsText="Aucune ville existante" DataKeyNames="ID" CommandItemDisplay="Top">
                <HeaderStyle HorizontalAlign="Center" />
                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridBoundColumn DataField="Nom"
                        SortExpression="Nom" UniqueName="Nom" HeaderText="Nom" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                      <telerik:GridBoundColumn DataField="Code"
                        SortExpression="Code" UniqueName="Code" HeaderText="Code" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Modifier" HeaderText="Modifier" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle Width="45px" />
                        <ItemTemplate>
                            <asp:HyperLink ID="_btnEdit" Text="Edit" runat="server" NavigateUrl="javascript:void(0)" ImageUrl="~/Images/imagesBack/pictoEdit.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
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
            <telerik:AjaxSetting AjaxControlID="gridVille">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridVille" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnNew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnNew" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>

