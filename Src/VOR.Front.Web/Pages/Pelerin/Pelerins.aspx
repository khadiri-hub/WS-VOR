<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pelerins.aspx.cs" Inherits="VOR.Front.Web.Pages.Pelerin.Pelerins" %>


<%@ Register TagPrefix="MyCtl" Namespace="VOR.Front.Web.Commun" Assembly="VOR.Front.Web" %>

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
                var masterTable = $find("<%= this.gridPelerin.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }

            function downloadBadge(eventID, agenceID) {
                var loadingPanel = $("#<%= RadAjaxLoadingPanel1.ClientID %>");
                loadingPanel.hide();
                window.location.href = "../../Handler/BadgePdfHandler.ashx?EventID=" + eventID + "&AgenceID=" + agenceID;
            }

            var blink_speed = 500;
            var t = setInterval(function () {
                var elements = document.getElementsByClassName("exclamation");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.visibility = (elements[i].style.visibility == 'hidden' ? '' : 'hidden');
                }
            }, blink_speed);


        </script>
        <style type="text/css">
            .rgDataDiv {
                height: 100% !important;
            }
        </style>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="clear"></div>
    <asp:Panel runat="server" ID="_pnlStats" CssClass="pnlFilter">
        <div style="float: left;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div style="float: left; position: relative; top: 6px;">
                        <div style="display: inline-block; margin-right: 20px;">
                            <asp:Label runat="server" ID="lblNbrPelerins" Text="Nombre de pelerins"></asp:Label>:
                            <span runat="server" id="nbrPelerin" style="color: #04399b; font-size: 18px"></span>
                        </div>
                        <div style="display: inline-block">
                            <asp:Label runat="server" ID="lblMaxPelerins" Text="Max pelerins"></asp:Label>:
                            <span runat="server" id="nbrMaxPelerins" style="color: #e51010; font-size: 18px"></span>
                        </div>
                        <div style="display: inline-block; margin-left: 30px">
                            <telerik:RadAjaxPanel ID="AjaxPanel1" runat="server">
                                <div class="">
                                    <telerik:RadSearchBox RenderMode="Lightweight" runat="server" ID="RadSearchBox1"
                                        CssClass="searchBox" Skin="Silk"
                                        Width="1000" DropDownSettings-Height="1000"
                                        DataSourceID="SqlDataSource1"
                                        DataTextField="NOM_PRENOM"
                                        DataValueField="ID_EVENEMENT"
                                        EmptyMessage="Search"
                                        Filter="StartsWith"
                                        MaxResultCount="20"
                                        OnDataSourceSelect="RadSearchBox1_DataSourceSelect"
                                        OnSearch="RadSearchBox1_Search">
                                        <DropDownSettings Height="400" Width="857">
                                            <ItemTemplate>
                                                <asp:Label ID="_lblNomFrancais" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_FRANCAIS") %>' runat="server" />
                                                <asp:Label ID="_lblPrenomFrancais" Text='<%# DataBinder.Eval(Container.DataItem, "PRENOM_FRANCAIS") %>' runat="server" />
                                                <asp:Label ID="_lblEvenementNom" Text='<%# "           ( " +  DataBinder.Eval(Container.DataItem, "NOM_EVENEMENT").ToString() + " )" %>' runat="server" />
                                                <asp:Label ID="_lblEvenementEtat" Text='<%# "    -    " +  DataBinder.Eval(Container.DataItem, "ETAT_EVENEMENT").ToString() %>' runat="server" />
                                            </ItemTemplate>
                                        </DropDownSettings>

                                        <SearchContext DataSourceID="SqlDataSource2" DataTextField="NOM" DataKeyField="ID_EVENEMENT" DropDownCssClass="contextDropDown"></SearchContext>
                                    </telerik:RadSearchBox>
                                </div>
                            </telerik:RadAjaxPanel>
                        </div>
                        <%--    <div style="display: inline-block; padding-left: 20px;  margin-right: 20px;">
                        <asp:Label runat="server" ID="lblPaye" Text="Montant payé"></asp:Label>:
                            <span runat="server" id="_montantPaye" style="color: brown; font-size: 12px"></span>
                    </div>
                    <div style="display: inline-block">
                        <asp:Label runat="server" ID="Label2" Text="Reste à payer"></asp:Label>:
                            <span runat="server" id="_resteAPayer" style="color: darksalmon; font-size: 12px"></span>
                    </div>--%>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>


        </div>
        <div class="clear"></div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            ConnectionString="<%$ ConnectionStrings:SAOASConnectionString %>"
            SelectCommand="SELECT * FROM View_Pelerin_Search
                            ORDER BY DATEDEBUT DESC"></asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
            ConnectionString="<%$ ConnectionStrings:SAOASConnectionString %>"
            SelectCommand="SELECT ID_EVENEMENT, DATE_DEBUT, NOM FROM EVENEMENT ORDER BY DATE_DEBUT DESC"></asp:SqlDataSource>
    </asp:Panel>


    <div class="clear"></div>
    <asp:Panel runat="server" ID="_pnlFiler" CssClass="pnlFilter" Style="margin-top: 2px;">
        <%--     <div style="display: inline-block">
            <asp:Label runat="server" ID="Label1" Text="T.payé RMD"></asp:Label>:
                            <span runat="server" id="_totalMontantPaye" style="color: cornflowerblue; font-size: 12px"></span>
        </div>
        <div style="display: inline-block; padding-left: 5px; margin-left: 5px">
            <asp:Label runat="server" ID="Label3" Text="T.R à payer RMD"></asp:Label>:
                            <span runat="server" id="_totalResteAPayer" style="color: darkmagenta; font-size: 12px"></span>
        </div>
           <div style="display: inline-block; padding-left: 5px; margin-left: 5px">
            <asp:Label runat="server" ID="Label4" Text="T.Pèlerins.RMD"></asp:Label>:
                            <span runat="server" id="_totalPelerins" style="color: cadetblue; font-size: 12px"></span>
        </div>
         <div style="display: inline-block">
            <asp:Label runat="server" ID="Label5" Text="T.payé MARS"></asp:Label>:
                            <span runat="server" id="_totalMontantPayeMawlid" style="color: cornflowerblue; font-size: 12px"></span>
        </div>
        <div style="display: inline-block; padding-left: 5px; margin-left: 5px">
            <asp:Label runat="server" ID="Label6" Text="T.R à payer MARS"></asp:Label>:
                            <span runat="server" id="_totalResteAPayerMawlid" style="color: darkmagenta; font-size: 12px"></span>
        </div>
           <div style="display: inline-block; padding-left: 5px; margin-left: 5px">
            <asp:Label runat="server" ID="Label7" Text="T.Pèlerins MARS"></asp:Label>:
                            <span runat="server" id="_totalPelerinsMawlid" style="color: cadetblue; font-size: 12px"></span>
        </div>
        <br />
         <br />--%>




        <div style="float: left; display: flex; margin: 5px; display: none">
            <asp:RadioButtonList ID="_rdoStatut" CssClass="rdoStaut" ClientIDMode="AutoID" Style="display: inline-block; position: relative; top: 5px;"
                AutoPostBack="true" OnSelectedIndexChanged="_rdoStatut_SelectedIndexChanged" runat="server" RepeatDirection="Horizontal" AppendDataBoundItems="true">
            </asp:RadioButtonList>&nbsp;
        <asp:DropDownList ID="_ddlMotifStatut" Width="200px" Height="35px" runat="server" Style="display: inline-block" AppendDataBoundItems="true" ClientIDMode="AutoID" Font-Size="13px">
            <asp:ListItem Text="---------------- Aucun ----------------" Value="0" />
        </asp:DropDownList>
            <asp:ImageButton runat="server" ID="_btnAfficher" ImageUrl="~/Images/imagesBack/pictoFilter.png" OnClick="_btnAfficher_Click" ClientIDMode="AutoID" Style="margin-left: 10px" Height="35" />
        </div>
        <div style="float: right; margin-right: 10px; position: relative; top: -10px;">
            <div style="float: left; position: relative; top: 9px; left: 15px; margin-right: 22px;">
                <asp:ImageButton runat="server" ID="_btnUpdate" ImageUrl="~/Images/imagesBack/pictoUpdate.png"
                    OnClientClick="if(confirm('Etes-vous sûr de vouloir éxecuter cette action ?')){ loading(true); return true; } else { return false; }"
                    OnClick="_btnUpdate_Click" ClientIDMode="AutoID" Style="height: 30px; margin-left: 10px; position: relative; top: 2px; display: none" />
            </div>
            <div style="float: left; position: relative; left: 15px; display: none">
                <asp:ImageButton runat="server" ID="btnPrintBadgesClick" ClientIDMode="AutoID" ImageUrl="~/Images/imagesBack/pictoBadge.png" OnClick="btnPrintBadges_Click" />
            </div>
            <div style="float: left; position: relative; top: 9px; left: 15px;">
                <asp:ImageButton runat="server" ID="btnExport" ImageUrl="~/Images/imagesBack/pictoExport.png" ClientIDMode="AutoID" OnClick="_btnExportToExcel_Click" />
            </div>
            <div style="float: left; position: relative; top: 7px; left: 15px; margin-left: 22px">
                <img id="btnNew" class="btn disabled " alt="" runat="server" src="~/Images/imagesBack/pictoAddPelerin.png" style="cursor: pointer" />
            </div>
        </div>
    </asp:Panel>

    <div style="padding: 5px;">
        <telerik:RadGrid ID="gridPelerin" runat="server"
            CellSpacing="0" AllowPaging="False" GridLines="None"
            OnItemDataBound="gridPelerin_ItemDataBound" AllowMultiRowSelection="True" AllowFilteringByColumn="true">
            <GroupingSettings CaseSensitive="false" />
            <ExportSettings HideStructureColumns="true" />
            <ClientSettings EnableRowHoverStyle="true" EnableAlternatingItems="false">
                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                <Selecting AllowRowSelect="True"></Selecting>
            </ClientSettings>
            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />
            <MasterTableView AutoGenerateColumns="False" Height="100%" EnableHeaderContextMenu="true" AllowSorting="True" NoMasterRecordsText="Aucun pelerin existant" DataKeyNames="ID" CommandItemDisplay="Top">
                <HeaderStyle HorizontalAlign="Center" />
                <CommandItemSettings ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Alerte" HeaderText="Alerte" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:ImageButton ID="_btnAlert" runat="server" Visible="false" CssClass="exclamation" ImageUrl="~/Images/imagesBack/exclamationMark.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="IdTypePelerin" SortExpression="IdTypePelerin" FilterControlWidth="100%" AllowFiltering="false" 
                        UniqueName="IdTypePelerin" HeaderText="Type pèlerin" 
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                        <ItemTemplate>
                            <asp:ImageButton ID="_imgTypePelerin" runat="server" Visible="true" ImageUrl="~/Images/imagesBack/sansType.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBinaryImageColumn UniqueName="BinaryImageColumn" Visible="false" AllowFiltering="false" HeaderStyle-Width="60px" ResizeMode="Fit" DataField="Photo" HeaderText="Photo"
                        ImageWidth="65" ImageHeight="65" />
                    <telerik:GridBoundColumn DataField="Agence" FilterControlWidth="100%" HeaderStyle-Width="100px" Exportable="true" Display="false" ItemStyle-ForeColor="Black" ItemStyle-BorderWidth="2" ItemStyle-Font-Bold="true" ItemStyle-BorderColor="Black" ItemStyle-BackColor="White"
                        SortExpression="Agence" UniqueName="Agence" HeaderText="Agence" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="Statut" FilterControlWidth="100%" HeaderStyle-Width="100px" Exportable="false" Display="false"
                        SortExpression="Statut" UniqueName="Statut" HeaderText="Statut" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                        <ItemTemplate>
                            <asp:Label ID="_lblStatut" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn DataField="MotifStatut" FilterControlWidth="100%" HeaderStyle-Width="100px" Exportable="false" Display="false"
                        SortExpression="MotifStatut" UniqueName="MotifStatut" HeaderText="Motif" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                        <ItemTemplate>
                            <asp:Label ID="_lblMotifStatut" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="PrenomNomFrancais" FilterControlWidth="100%" HeaderStyle-Width="0px" Exportable="true" Display="true"
                        SortExpression="PrenomNomFrancais" UniqueName="PrenomNomFrancais" HeaderText="Prénom Nom FR" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PrenomFrancais" FilterControlWidth="100%" HeaderStyle-Width="100px" Exportable="true"
                        SortExpression="PrenomFrancais" UniqueName="PrenomFrancais" HeaderText="Prénom FR" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="NomFrancais" FilterControlWidth="100%" HeaderStyle-Width="100px" Exportable="true"
                        SortExpression="NomFrancais" UniqueName="NomFrancais" HeaderText="Nom FR" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="NomVaccin" FilterControlWidth="100%" HeaderStyle-Width="100px" Exportable="true"
                        SortExpression="NomVaccin" UniqueName="NomVaccin" HeaderText="Vaccin" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="NbrVaccination" FilterControlWidth="100%" HeaderStyle-Width="100px" Exportable="true"
                        SortExpression="NbrVaccination" UniqueName="NbrVaccination" HeaderText="NBR Vaccin" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="NomArabe" FilterControlWidth="100%" HeaderStyle-Width="100px" Display="false" Exportable="true"
                        SortExpression="NomArabe" UniqueName="NomArabe" HeaderText="Nom AR" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PrenomArabe" FilterControlWidth="100%" HeaderStyle-Width="100px" Display="false" Exportable="true"
                        SortExpression="PrenomArabe" UniqueName="PrenomArabe" HeaderText="Prénom AR" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="DateNaissance" Display="false" HeaderText="Date de Naissance" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="130px" Exportable="true"
                        ShowFilterIcon="false" FilterControlWidth="120px" CurrentFilterFunction="LessThanOrEqualTo" DataFormatString="{0:dd/MM/yyyy}" AutoPostBackOnFilter="true">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="EtatCivil" FilterControlWidth="100%" Visible="false" Exportable="false"
                        SortExpression="EtatCivil" UniqueName="EtatCivil" HeaderText="Etat civil" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn DataField="Sexe" UniqueName="Sexe" Display="false" HeaderStyle-Width="110px" HeaderText="Sexe" ItemStyle-HorizontalAlign="Center" Exportable="true">
                        <ItemTemplate>
                            <asp:Label ID="_lblSexe" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="NumPassport" Display="false" FilterControlWidth="100%" HeaderStyle-Width="90px" Exportable="true"
                        SortExpression="NumPassport" UniqueName="NumPassport" HeaderText="N° Passeport" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn DataField="DateExpiration" Display="false" HeaderText="Date d'éxpiration" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" Exportable="true"
                        ShowFilterIcon="false" FilterControlWidth="120px" CurrentFilterFunction="LessThanOrEqualTo" DataFormatString="{0:dd/MM/yyyy}" AutoPostBackOnFilter="true">
                    </telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="Telephone" FilterControlWidth="100%" HeaderStyle-Width="90px" Exportable="true"
                        SortExpression="Telephone" UniqueName="Telephone1" HeaderText="Téléphone" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TransportPaye" FilterControlWidth="100%" HeaderStyle-Width="90px" Exportable="true"
                        SortExpression="TransportPaye" ItemStyle-Font-Bold="true" UniqueName="TransportPaye" HeaderText="Transport payé" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RepasPaye" FilterControlWidth="100%" HeaderStyle-Width="90px" Exportable="false"
                        SortExpression="RepasPaye" ItemStyle-Font-Bold="true" UniqueName="RepasPaye" HeaderText="Repas payé" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="AssuranceMaladie" FilterControlWidth="100%" HeaderStyle-Width="90px" Exportable="false"
                        SortExpression="AssuranceMaladie" ItemStyle-Font-Bold="true" UniqueName="AssuranceMaladie" HeaderText="Assurance maladie" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Stop" FilterControlWidth="100%" HeaderStyle-Width="90px" Exportable="true"
                        SortExpression="Stop" ItemStyle-Font-Bold="true" UniqueName="Stop" HeaderText="Stop" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <%-- <MyCtl:MyCustomFilteringColumn DataField="TransportPaye" HeaderText="Transport payé" DataType="System.String" AutoPostBackOnFilter="true"
                        Filtre="Boolean" ItemStyle-HorizontalAlign="Center" SortExpression="TransportPaye" CurrentFilterFunction="EqualTo"
                        UniqueName="TransportPaye" >
                        <ItemTemplate>
                            <asp:Label ID="lblTransportPaye" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TransportPaye") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="100px" />
                    </MyCtl:MyCustomFilteringColumn>

                    <MyCtl:MyCustomFilteringColumn DataField="VaccinPaye" HeaderText="Vaccin payé" DataType="System.String" AutoPostBackOnFilter="true"
                        Filtre="Boolean" ItemStyle-HorizontalAlign="Center" SortExpression="VaccinPaye" CurrentFilterFunction="EqualTo"
                        UniqueName="VaccinPaye" >
                        <ItemTemplate>
                            <asp:Label ID="lblVaccinPaye" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "VaccinPaye") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="100px" />
                    </MyCtl:MyCustomFilteringColumn>--%>
                    <telerik:GridBoundColumn DataField="ProgrammeNom" Visible="false" HeaderStyle-Width="170px" FilterControlWidth="100%" Exportable="false"
                        SortExpression="Programme" UniqueName="Programme" HeaderText="Programme" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TypeChambreNom" FilterControlWidth="100%" DataType="System.Int32" Visible="false" Exportable="false"
                        SortExpression="Code" UniqueName="Code" HeaderText="Chambre" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PrixVentePack" FilterControlWidth="100%" HeaderStyle-Width="70px" Exportable="false"
                        SortExpression="PrixVentePack" UniqueName="PrixVentePack" HeaderText="Prix V. Pack" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="LessThanOrEqualTo" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="MontantPaye" FilterControlWidth="100%" HeaderStyle-Width="70px" Exportable="false"
                        SortExpression="MontantPaye" UniqueName="MontantPaye" HeaderText="Payé" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="LessThanOrEqualTo" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RestApayer" FilterControlWidth="100%" HeaderStyle-Width="70px" Exportable="false"
                        SortExpression="RestApayer" UniqueName="RestApayer" HeaderText="Reste" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="LessThanOrEqualTo" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EvaluationVoyage" FilterControlWidth="100%" Exportable="false"
                        SortExpression="EvaluationVoyage" UniqueName="EvaluationVoyage" Display="false" HeaderText="Eval. Voyage" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EvaluationPelerin" Exportable="false"
                        SortExpression="EvaluationPelerin" UniqueName="EvaluationPelerin" Display="false" HeaderText="Eval. Pelerin" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Comentaire" FilterControlWidth="100%" Exportable="false"
                        SortExpression="Comentaire" UniqueName="Comentaire" Display="false" HeaderText="Comentaire" ItemStyle-HorizontalAlign="Center" CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Modifier" HeaderText="Modifier" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:HyperLink ID="_btnEdit" Text="Edit" runat="server" NavigateUrl="javascript:void(0)" Style="position: relative; top: 4px;" ImageUrl="~/Images/imagesBack/pictoEdit.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Recus" HeaderText="Reçus" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:HyperLink ID="_btnRecus" Text="Reçus" runat="server" NavigateUrl="javascript:void(0)" ImageUrl="~/Images/imagesBack/pictoMiniCheque.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Visa" HeaderText="Visa" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:HyperLink ID="_btnVisa" Text="Visa" runat="server" NavigateUrl="javascript:void(0)" ImageUrl="~/Images/imagesBack/visa.png" Style="position: relative; top: 2px" />
                            <asp:ImageButton ID="imgAlertVisa" runat="server" Visible="false" CssClass="exclamation" ImageUrl="~/Images/imagesBack/exclamationMark.png" Style="display: inline-block; position: relative; top: -3px;" />
                            <asp:ImageButton ID="imgOk" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/Ok.png" Style="display: inline-block; cursor: text" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Badge" ItemStyle-Width="20px"
                        AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle Width="60px" />
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="btnPrintBadge" OnClick="btnPrintBadge_Click" ImageUrl="~/Images/imagesBack/pictoMiniBadge.png"
                                CommandArgument='<%# DataBinder.Eval( Container, "DataItem.Id" ) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
    <telerik:RadNotification runat="server" ID="radNotif" Text="Veuillez sélectionner au moins un pelerin" TitleIcon="none" ContentIcon="warning" Width="400" Height="100" EnableShadow="true" RenderMode="Classic" Position="Center" Animation="Fade" AutoCloseDelay="0" />
    <telerik:RadNotification runat="server" ID="radNotifAlert" Text="" TitleIcon="none" ContentIcon="warning" Width="500" Height="120" EnableShadow="true" RenderMode="Classic" Position="Center" Animation="Fade" AutoCloseDelay="0" />
    <telerik:RadWindowManager ID="_rwmEdit" runat="server" Modal="true"
        ShowContentDuringLoad="false" KeepInScreenBounds="true" IconUrl="null" EnableShadow="true"
        ReloadOnShow="true" VisibleStatusbar="false" VisibleOnPageLoad="false" Behaviors="Close, Move, Minimize">
        <Windows>
            <telerik:RadWindow ID="_rwEdit" runat="server" Width="1275px" Height="785px" />
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadWindowManager ID="_rwmRecus" runat="server" Modal="true"
        ShowContentDuringLoad="false" KeepInScreenBounds="true" IconUrl="null" EnableShadow="true"
        ReloadOnShow="true" VisibleStatusbar="false" VisibleOnPageLoad="false" Behaviors="Close, Move, Minimize">
        <Windows>
            <telerik:RadWindow ID="_rwRecus" runat="server" Width="975px" Height="900px" />
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadWindowManager ID="_rwmVisa" runat="server" Modal="true"
        ShowContentDuringLoad="false" KeepInScreenBounds="true" IconUrl="null" EnableShadow="true"
        ReloadOnShow="true" VisibleStatusbar="false" VisibleOnPageLoad="false" Behaviors="Close, Move, Minimize">
        <Windows>
            <telerik:RadWindow ID="_rwVisa" runat="server" Width="700px" Height="700px" />
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxManagerProxy ID="AjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gridPelerin">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridPelerin" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnNew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnNew" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnPrintBadgesClick">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridPelerin" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_rdoStatut">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="_rdoStatut" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_rdoStatut">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="_ddlMotifStatut" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_btnAfficher">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridPelerin" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="_btnUpdate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridPelerin" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadSearchBox1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridPelerin" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>


        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>

