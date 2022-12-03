<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hebergement.aspx.cs" Inherits="VOR.Front.Web.Pages.Hebergement.Hebergement" %>

<asp:Content ID="content1" ContentPlaceHolderID="HeadContent" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            * {
                padding: 0;
                margin: 0;
            }

            #mainContentArea {
                width: 940px;
                background-color: White;
                margin: 100px auto;
                -moz-border-radius: 10px;
                padding: 30px;
            }


            #pelerinContainer {
                border: none;
                width: 100%;
                float: left;
            }

            div.RadListView {
                width: 100%;
                float: left;
                border: none;
                background-color: Transparent;
            }

            div.RadListView_Black,
            div.RadListView_Glow,
            div.RadListView_BlackMetroTouch,
            .qsfDark .result {
                color: #00156E;
            }

            #genreContainer {
                margin-top: 10px;
                padding: 15px;
                border-bottom: none;
            }

                #genreContainer a,
                .result .genre {
                    display: block;
                    font-size: 13px;
                    text-decoration: none;
                    text-align: center;
                }

            * > #genreContainer a,
            * > .result .genre {
            }


            .result .info {
                display: none;
                height: 145px;
            }

            .result {
                margin-top: 1.6em;
                float: right;
                width: 100px;
                font-size: 12px;
            }

                .result .msg {
                    border: 1px solid #E6E6E6;
                    background-color: #ECF4FF;
                    padding: 20px;
                }

                    .result .msg strong {
                        color: Red;
                    }

                .result div.track {
                    width: 150px;
                    height: 31px;
                    padding: 13px 5px 7px;
                    overflow: hidden;
                    text-align: center;
                    border: none;
                }

            * > .result div.track {
                background: transparent url('images/sprite.png') no-repeat scroll 0 -750px;
            }

            * html .result div.track {
                background: transparent url('images/sprite.gif') no-repeat scroll 0 -750px;
            }

            .result .arrow {
                width: 28px;
                height: 20px;
                margin-left: 170px;
                margin-top: 15px;
            }

            * > .result .arrow {
                background: transparent url('images/sprite.png') no-repeat scroll 0 -850px;
            }

            * html .result .arrow {
                background: transparent url('images/sprite.gif') no-repeat scroll 0 -850px;
            }


            .result .genre {
                width: 100px;
                font-size: 14px;
                border: 1px solid #E6E6E6;
                float: left;
            }

            .pelerin,
            div.RadListView div.pelerin {
                width: 180px;
                height: 135px;
                float: left;
                border: 1px solid #d7d7d7;
                padding: 0;
                margin: 5px;
            }

            .noPelerins {
                border-right: 1px solid #E6E6E6;
                border-bottom: 1px solid #E6E6E6;
                padding: 10px;
            }

            div.RadListView div.pelerin h3,
            div.RadListView div.pelerin div {
                text-align: center;
                line-height: 1.5em;
                color: black;
            }

            div.RadListView div.pelerin h3 {
                font-size: 12px;
            }

            div.RadListView div.pelerin .info {
                padding-top: 5px;
            }

            div.RadListView div.pelerin .rlvDrag {
                float: left;
            }

            div.RadListView div.rlvDraggedItem {
                border: 0;
                margin: 0;
            }

            div.rlvDraggedItem div.pelerin.rlvI {
                border: none;
            }

            * > div.rlvDraggedItem div.pelerin.rlvI {
                background: transparent url('images/sprite.png') no-repeat scroll 0 0;
            }

            * html div.rlvDraggedItem div.pelerin.rlvI {
                background: transparent url('images/sprite.gif') no-repeat scroll 0 0;
            }

                div.rlvDraggedItem div.pelerin.rlvI .info {
                    padding-top: 10px;
                    padding-left: 10px;
                }

                    div.rlvDraggedItem div.pelerin.rlvI .info div,
                    div.rlvDraggedItem div.pelerin.rlvI .info h3 {
                        font-size: 11px;
                        line-height: normal;
                    }

                div.rlvDraggedItem div.pelerin.rlvI .rlvDrag {
                    display: none;
                }

                div.rlvDraggedItem div.pelerin.rlvI .info .album {
                    display: none;
                }

                div.rlvDraggedItem div.pelerin.rlvI .info h3,
                div.rlvDraggedItem div.pelerin.rlvI .info div {
                    margin: 0 0 0 40px;
                    width: 130px;
                    height: 16px;
                    display: block;
                    text-align: left;
                    overflow: hidden;
                }

            .clearFix {
                clear: both;
            }

            .exclamation {
            }

            h3 {
                display: inline-block !important;
            }

            [data-tooltip] {
                position: relative;
                z-index: 2;
                cursor: pointer;
            }

                /* Hide the tooltip content by default */
                [data-tooltip]:before,
                [data-tooltip]:after {
                    visibility: hidden;
                    -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
                    filter: progid: DXImageTransform.Microsoft.Alpha(Opacity=0);
                    opacity: 0;
                    pointer-events: none;
                }

                /* Position tooltip above the element */
                [data-tooltip]:before {
                    position: absolute;
                    bottom: 100%;
                    left: 50%;
                    margin-bottom: 5px;
                    margin-left: -80px;
                    padding: 7px;
                    width: 160px;
                    -webkit-border-radius: 3px;
                    -moz-border-radius: 3px;
                    border-radius: 3px;
                    background-color: #000;
                    background-color: hsla(0, 0%, 20%, 0.9);
                    color: #fff;
                    content: attr(data-tooltip);
                    text-align: center;
                    font-size: 16px;
                    font-weight: bold;
                    line-height: 1.2;
                    white-space: pre-wrap;
                    z-index: 90000;
                }

                /* Triangle hack to make tooltip look like a speech bubble */
                [data-tooltip]:after {
                    position: absolute;
                    bottom: 100%;
                    left: 50%;
                    margin-left: -5px;
                    width: 0;
                    border-top: 5px solid #000;
                    border-top: 5px solid hsla(0, 0%, 20%, 0.9);
                    border-right: 5px solid transparent;
                    border-left: 5px solid transparent;
                    content: " ";
                    font-size: 0;
                    line-height: 0;
                }

                /* Show tooltip content on hover */
                [data-tooltip]:hover:before,
                [data-tooltip]:hover:after {
                    visibility: visible;
                    -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
                    filter: progid: DXImageTransform.Microsoft.Alpha(Opacity=100);
                    opacity: 1;
                }
        </style>
        <script type="text/javascript">
            function onRequestStart(sender, args) {
                if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0)
                    args.set_enableAjax(false);
                else
                    args.set_enableAjax(true);
            }

            var originalMsg = "";

            function ChambreClick(chambreLink) {
                __doPostBack('chambreLink', 'ShowPelerins');
            }

            var blink_speed = 500;
            var t = setInterval(function () {
                var elements = document.getElementsByClassName("exclamation");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].style.visibility = (elements[i].style.visibility == 'hidden' ? '' : 'hidden');
                }
            }, blink_speed);

        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() != undefined) {
                args.set_errorHandled(true);
            }
        }
    </script>
    <div class="clear"></div>
    <div class="clear"></div>
    <div class="clear"></div>
    <div id="genreContainer">
        <asp:Repeater ID="ChambreRepeater" runat="server" OnItemDataBound="ChambreRepeater_ItemDataBound">
            <ItemTemplate>
                <b>
                    <div style="min-width: 260px; display: inline-table;  top: -146px; position: relative;">
                        <asp:Panel runat="server" ID="Header" Style="border: 1px solid #E6E6E6; text-align: center; font-size: 11px;">
                            <div>
                                <asp:ImageButton CssClass="close" Style="float: right" ID="_btnSupprimer" ToolTip="Supprimer" runat="server" Visible="true" ImageUrl="~/Images/imagesBack/cross.png" CommandArgument='<%# Eval("ID") %>'
                                    OnClientClick="if(confirm('Etes-vous sûr de vouloir supprimer cette chambre?')){ loading(true); return true; } else { return false; }"
                                    OnCommand="_btnSupprimer_Command" />
                            </div>
                            <div>
                                <asp:Label runat="server" ID="chLblAgence" Style="text-align: center; display: block; background-color: white; float: left; margin: 2px; border: 1px solid; padding: 3px;"></asp:Label>
                                <asp:Label runat="server" ID="ChambreDetail" Style="text-align: center; float: left; border: 1px solid; margin: 2px; background-color: white; text-align: center; padding: 3px;"></asp:Label>
                                <asp:Label runat="server" ID="numeroChambre" Text='<%# Eval("Numero") %>' Style="text-align: center; display: block; background-color: white; float: left; margin: 2px; border: 1px solid; padding: 3px;"></asp:Label>
                                <div class="clear"></div>
                                <asp:Panel runat="server" ID="vol" Style="text-align: center"></asp:Panel>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlChLink">
                            <asp:LinkButton ID="ChambreLink" Style="padding-bottom: 5px; text-align: right; text-decoration: none !important; position: relative; top: 0px; padding-right: 10px; margin-bottom: 5px; color: #ffffff; min-height: 100px;"
                                runat="server" OnClick="ChambreLink_Click" ClientIDMode="AutoID" CommandArgument='<%# Eval("ID") %>'>                              
                            </asp:LinkButton>
                        </asp:Panel>
                    </div>
                </b>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <asp:Panel runat="server" ID="pelerinContainer" Style="padding: 10px; height: 330px; width: 98%">
        <telerik:RadListView ID="PelerinListView" Width="100%" runat="server" RenderMode="Lightweight" OnNeedDataSource="PelerinListView_NeedDataSource" Skin="Silk"
            OnItemDrop="PelerinListView_ItemDrop" OnItemDataBound="PelerinListView_ItemDataBound" ItemPlaceholderID="PelerinContainer" DataKeyNames="ID"
            ClientDataKeyNames="ID">
            <ClientSettings AllowItemsDragDrop="true">
            </ClientSettings>
            <LayoutTemplate>
                <div class="RadListView RadListView_Silk">
                    <asp:PlaceHolder ID="PelerinContainer" runat="server"></asp:PlaceHolder>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <asp:Panel ID="pnlPelerin" runat="server" ClientIDMode="AutoID" class="pelerin rlvI">
                    <div class="info">
                        <div style="font-size: 11px; font-weight: bold;">
                            <div style="width: 98%; margin: 0 auto;">
                                <telerik:RadListViewItemDragHandle ID="RadListViewItemDragHandle1" ClientIDMode="AutoID" runat="server" ToolTip="Drag to organize"></telerik:RadListViewItemDragHandle>
                                <asp:ImageButton ID="ImgRestaurant" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/pictoRestaurant.png" Style="display: inline-block; float: right; margin-top: 5px; border: 1px solid; font-weight: bold; padding: 2px; width: 18px; height: 16px; margin: 2px; position: relative; top: -4px;" />
                                <asp:Label runat="server" ID="pelerinLblAgence" Style="text-align: center; display: inline-block; float: right; border: 1px solid; font-weight: bold; padding: 2px; margin: 2px; position: relative; top: -4px;"></asp:Label>
                                <asp:Label runat="server" ID="chambreDetail" Style="text-align: center; display: inline-block; float: right; border: 1px solid; font-weight: bold; padding: 2px; margin: 2px; position: relative; top: -4px;"></asp:Label>
                                <asp:ImageButton ID="_btnAlert" CssClass="exclamation" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/exclamationMark.png" Style="text-align: center; display: inline-block; float: right; border: 1px solid; font-weight: bold; padding: 2px; margin: 1px" />
                                <div class="clear"></div>
                                <div style="width: 145px; margin: 0 auto">
                                    <asp:Label Style="text-align: center" runat="server" ID="infoPelerin"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <br />
                        <img src="~/Images/imagesCommunes/alien.png" alt="" id="_imgPhotoPerson" runat="server" class="imgborder" style="border-radius: 50%; border: 3px solid #fff; width: 70px; height: 70px; bottom: 20px; position: relative;" />
                    </div>
                    <%--<div class="info">
                        <asp:Panel ID="ddd" runat="server">
                            <h2 style="text-align: center; margin-right: 16px; display: inline-block">
                                <%# Eval("NomArabe") %>
                                <%# Eval("PrenomArabe") %>
                            </h2>
                            <asp:ImageButton ID="_btnAlert" runat="server" Visible="false"  CssClass="exclamation"  ImageUrl="~/Images/imagesBack/exclamationMark.png" style="display: inline-block"/>
                        </asp:Panel>
                        <h2 style="text-align: center; margin-right: 16px;" id="chambreAgeHtmlTag" runat="server"></h2>
                        <br />
                        <img src="~/Images/imagesCommunes/alien.png" alt="" id="_imgPhotoPerson" runat="server" class="imgborder" style="border-radius: 50%; border: 3px solid #fff; width: 100px; height: 100px; bottom: 20px; position: relative;" />
                    </div>--%>
                </asp:Panel>
            </ItemTemplate>
            <EmptyDataTemplate>
                <div class="noPelerins">
                    Aucun pelerin dans cette chambre
                </div>
            </EmptyDataTemplate>
        </telerik:RadListView>
        <div class="clearFix">
        </div>
    </asp:Panel>
    <telerik:RadWindowManager ID="_rwmEdit" runat="server" Modal="true"
        ShowContentDuringLoad="false" KeepInScreenBounds="true" IconUrl="null" EnableShadow="true"
        ReloadOnShow="true" VisibleStatusbar="false" VisibleOnPageLoad="false" Behaviors="Close, Move, Minimize">
        <Windows>
            <telerik:RadWindow ID="_rwEdit" runat="server" Width="550px" Height="290px" />
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxManagerProxy ID="AjaxManagerProxy1" runat="server">
    </telerik:RadAjaxManagerProxy>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>

