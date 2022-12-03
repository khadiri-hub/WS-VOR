<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Chambres.aspx.cs" Inherits="VOR.Front.Web.Pages.Evenement.Chambres" %>

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
                var masterTable = $find("<%= this.gridChambre.ClientID %>").get_masterTableView();
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
    <div style="padding: 10px; height:610px">
        <telerik:RadGrid ID="gridChambre" runat="server" AllowFilteringByColumn="true"
            CellSpacing="0" AllowPaging="True" GridLines="None" OnNeedDataSource="gridChambre_NeedDataSource" OnItemDataBound="gridChambre_ItemDataBound">
            <ExportSettings HideStructureColumns="true" />
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />
            <ClientSettings EnableAlternatingItems="false">
            </ClientSettings>
            <MasterTableView PageSize="10" AutoGenerateColumns="False" Height="100%" AllowSorting="True" NoMasterRecordsText="Aucune chambre existante" DataKeyNames="ID" CommandItemDisplay="Top">
                <HeaderStyle HorizontalAlign="Center" />
                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridBoundColumn DataField="Nom" FilterControlWidth="100%"
                        SortExpression="Nom" UniqueName="Nom" HeaderText="Nom" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Agence.Alias" FilterControlWidth="100%"
                        SortExpression="Agence.Alias" UniqueName="Agence.Alias" HeaderText="Agence" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Programme.Nom" FilterControlWidth="100%"
                        SortExpression="Programme.Nom" UniqueName="Programme.Nom" HeaderText="Programme" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Hotel.Ville.Nom" FilterControlWidth="100%"
                        SortExpression="Hotel.Ville.Nom" UniqueName="Hotel.Ville.Nom" HeaderText="Ville" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Hotel.Nom" FilterControlWidth="100%"
                        SortExpression="Hotel.Nom" UniqueName="Hotel.Nom" HeaderText="Hotel" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TypeChambre.Code" FilterControlWidth="100%"
                        SortExpression="TypeChambre.Code" UniqueName="TypeChambre.Code" HeaderText="Type de Chambre" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TypeChambre.Nom" FilterControlWidth="100%"
                        SortExpression="TypeChambre.Nom" UniqueName="TypeChambre.Nom" HeaderText="Description" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
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
            <telerik:RadWindow ID="_rwEdit" runat="server" Width="600px" Height="632px" />
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxManagerProxy ID="AjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gridChambre">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridChambre" LoadingPanelID="RadAjaxLoadingPanel1" />
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

