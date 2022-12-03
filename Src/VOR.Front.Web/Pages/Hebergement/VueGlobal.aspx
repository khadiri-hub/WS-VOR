<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VueGlobal.aspx.cs" Inherits="VOR.Front.Web.Pages.Hebergement.VueGlobal" %>

<asp:Content ID="content1" ContentPlaceHolderID="HeadContent" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            * {
                padding: 0;
                margin: 0;
            }

            #genreContainer {
                margin-top: 10px;
                padding: 15px;
                border-bottom: none;
                height: 100%; 
                float: right;
                width: 100%;
            }

                #genreContainer a,
                .result .genre {
                    display: block;
                    padding: 15px 15px 0px 0px;
                    font-size: 13px;
                    text-decoration: none;
                    border: 1px solid #E6E6E6;
                    text-align: center;
                }

            * > #genreContainer a,
            * > .result .genre {
                background-color: whitesmoke;
            }

            .clearFix {
                clear: both;
            }

            /************************  MENU ************************/

            #toggle-view {
                list-style: none;
                font-family: arial;
                font-size: 11px;
                margin: 0;
                padding: 0;
                width: 100%;
            }

                #toggle-view li {
                    margin: 10px;
                    border-bottom: 1px solid #ccc;
                    position: relative;
                    cursor: pointer;
                    list-style-type: none !important;
                }

                #toggle-view h3 {
                    margin: 0;
                    font-size: 14px;
                }

                #toggle-view span {
                    right: 5px;
                    top: 0;
                    font-size: 13px;
                }

                #toggle-view .panel {
                    margin: 5px 0;
                    display: none;
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

            $(document).ready(function () {

                var text = $("#toggle-view li").children('div.panel');
                text.slideDown('200');
                $("#toggle-view li").children('span').html('-');


                $("#toggle-view li").on('click', function (event) {

                    var text = $(this).children('div.panel');

                    if (text.is(':hidden')) {
                        text.slideDown('200');
                        $(this).children('span').html('-');
                    } else {
                        text.slideUp('200');
                        $(this).children('span').html('+');
                    }
                });
            });

            function onRequestStart(sender, args) {
                if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0)
                    args.set_enableAjax(false);
                else
                    args.set_enableAjax(true);
            }
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
    <div style="display: flex; flex-direction: row;">
        <div id="Events" style="width: 310px; float: left; margin-top: 5px; padding: 15px; border-right: 1px solid #ccc;">
            <label runat="server" style="margin-bottom: 3px; display: inline-block; font-size: 15px">Evenement</label>
            <div>
                <telerik:RadComboBox ID="ddlEvents" runat="server" CheckBoxes="true" ClientIDMode="AutoID" DropDownWidth="300" EmptyMessage="Sélectionner un évenement" Style="margin-bottom: 16px"
                    EnableCheckAllItemsCheckBox="true" AutoPostBack="false" Width="300" DropDownAutoWidth="Enabled" Filter="StartsWith" />
            </div>
            <label runat="server" style="margin-bottom: 3px; display: inline-block; font-size: 15px">Type de chambre</label>
            <div>
                <telerik:RadComboBox ID="ddlTypesChambre" runat="server" CheckBoxes="true" ClientIDMode="AutoID" DropDownWidth="300" EmptyMessage="Sélectionner un type de chambre" Style="margin-bottom: 16px"
                    EnableCheckAllItemsCheckBox="true" AutoPostBack="false" Width="300" DropDownAutoWidth="Enabled" Filter="StartsWith" />
            </div>
            <label runat="server" style="margin-bottom: 3px; display: inline-block; font-size: 15px">Hotêls</label>
            <div>
                <telerik:RadComboBox ID="ddlHotels" runat="server" CheckBoxes="true" ClientIDMode="AutoID" DropDownWidth="300" EmptyMessage="Sélectionner un hôtel" Style="margin-bottom: 16px"
                    EnableCheckAllItemsCheckBox="true" AutoPostBack="false" Width="300" DropDownAutoWidth="Enabled" Filter="StartsWith" />
            </div>
            <div style="float: right">
                <asp:ImageButton ID="img_btn_search" ClientIDMode="AutoID" runat="server" Style="position: relative; left: 7px;"
                    ImageUrl="~/Images/imagesBack/pictoSearch.png" OnClick="img_btn_search_Click" />
            </div>
        </div>
        <div id="genreContainer">
            <div class="clear"></div>
            <ul id="toggle-view">
                <asp:Repeater ID="EventRepeater" runat="server"
                    OnItemDataBound="EventRepeater_ItemDataBound">
                    <ItemTemplate>
                        <li>
                            <h3 style="display: inline-block">
                                <asp:Label runat="server" ID="Evenement" Font-Size="15"></asp:Label>
                            </h3>
                            <br />
                            <span style="display: inline-block; float: right">+</span>
                            <div class="panel">
                                <div style="width: 50%; float: left">
                                    <h2 style="display: inline-block">
                                        <asp:Label runat="server" ID="CountPelerinMakkah" Font-Size="15" style="float: right; color: red;"></asp:Label>
                                    </h2>
                                    <br /><br />
                                    <fieldset style="padding: 10px;">
                                        <legend>&nbsp;&nbsp;Makkah&nbsp;&nbsp;</legend>
                                        <br />
                                        <asp:Repeater ID="ChambreRepeaterMakkah" runat="server" OnItemDataBound="ChambreRepeaterMakkah_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:LinkButton Style="text-decoration: none !important; display: inline-table; width: 40px; height: 40px;" ClientIDMode="AutoID" ID="ChambreLink"
                                                    runat="server">
                                                    <asp:Label runat="server" ID="Capacite" Font-Size="10" Style="position: relative; top: 5px; left: 8px;"></asp:Label>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </fieldset>
                                </div>
                                <div style="width: 45%; float: right">
                                    <h2 style="display: inline-block">
                                        <asp:Label runat="server" ID="CountPelerinMedine" Font-Size="15" style="float: right; color: red;"></asp:Label>
                                    </h2>
                                    <br /><br />
                                    <fieldset style="padding: 10px;">
                                        <legend>&nbsp;&nbsp;Médine&nbsp;&nbsp;</legend>
                                        <br />
                                        <asp:Repeater ID="ChambreRepeaterMedine" runat="server" OnItemDataBound="ChambreRepeaterMedine_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:LinkButton Style="text-decoration: none !important; display: inline-table; width: 40px; height: 40px;" ClientIDMode="AutoID" ID="ChambreLink"
                                                    runat="server">
                                                    <asp:Label runat="server" ID="Capacite" Font-Size="10" Style="position: relative; top: 5px; left: 8px;"></asp:Label>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </fieldset>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </li>
                        <br />
                        <br />
                        <br />
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div style="width: 50%; float: left">
                <h3>
                    <asp:Label ID="TotalPelerinMakkah" runat="server" Font-Size="20" style="text-decoration: underline; color: red"></asp:Label>
                </h3>
            </div>
            <div style="width: 45%; float: right">
                <h3>
                    <asp:Label ID="TotalPelerinMedine" runat="server"  Font-Size="20" style="text-decoration: underline; color: red; float: left;"></asp:Label>
                </h3>
            </div>
        </div>
    </div>
    <telerik:RadAjaxManagerProxy ID="AjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="img_btn_search">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="genreContainer" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>

