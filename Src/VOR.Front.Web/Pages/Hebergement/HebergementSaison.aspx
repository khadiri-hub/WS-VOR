<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HebergementSaison.aspx.cs" Inherits="VOR.Front.Web.Pages.Hebergement.HebergementSaison" %>

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
                margin-top: 60px;
                padding: 15px;
                border-bottom: none;
            }

                #genreContainer a,
                .result .genre {
                    display: block;
                    font-size: 12px;
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
                width: 230px;
                height: 140px;
                float: left;
                border: 1px solid #E6E6E6;
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
                padding-left: 5px;
                padding-right: 5px;
            }

            div.RadListView div.pelerin h3 {
                font-size: 12px;
            }

            div.RadListView div.pelerin .info {
                padding-top: 5px;
            }

            div.RadListView div.pelerin .rlvDrag {
                margin: 5px;
                float: left;
            }

            div.RadListView div.rlvDraggedItem {
                border: 0;
                margin: 0;
            }

            div.rlvDraggedItem div.pelerin.rlvI {
                border: none;
                width: 200px;
                height: 50px;
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

            .cadreRouge {
                width: 20px;
                height: 20px;
                border: 1px solid #E6E6E6;
                background-color: #ff3f3f;
                display: inline-block;
                position: relative;
                top: 5px;
            }

            .cadreOrange {
                width: 20px;
                height: 20px;
                border: 1px solid #E6E6E6;
                background-color: #ff9721;
                display: inline-block;
                position: relative;
                top: 5px;
            }

            .cadreVert {
                width: 20px;
                height: 20px;
                border: 1px solid #E6E6E6;
                background-color: #56c605;
                display: inline-block;
                position: relative;
                top: 5px;
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

            .button {
                font-family: "Trebuchet MS", Arial, Helvetica, sans-serif !important;
                text-transform: uppercase !important;
                outline: 0 !important;
                background: #edd902 !important;
                width: 352px !important;
                border: 0 !important;
                padding: 15px !important;
                color: #fff !important;
                font-size: 14px !important;
                -webkit-transition: all 0.3 ease !important;
                transition: all 0.3 ease !important;
                cursor: pointer !important;
                font-weight: bold !important;
                background: #5a569c !important;
            }

                .button:hover, .button:active, .button:focus {
                    background: #6a65c2 !important;
                }

            .cbxFilter input[type=checkbox] {
                display: none;
            }

                .cbxFilter input[type=checkbox] + label {
                    cursor: pointer;
                }

                    .cbxFilter input[type=checkbox] + label:before {
                        display: inline-block;
                        width: 12px;
                        height: 12px;
                        background: #FFF;
                        border: 1px solid #000;
                        margin-right: 5px;
                        margin-top: 9px;
                        content: "";
                    }

                .cbxFilter input[type=checkbox]:checked + label:before {
                    background: #d3390a;
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

            function OnClientClose(args) {
                DisplayMsgInfo(args);
                setTimeout(function () {
                    rebind();
                }, 1000);
            }
            function rebind() {
                __doPostBack();
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

    <asp:Panel ID="Statistic" runat="server" Style="min-height: 270px">
        <div style="float: right; width: 650px">
            <div style="float: left; margin-right: 20px;">
                <span style="font-size: 15px; font-weight: bold;">Evénement</span><br />
                <%--                <asp:DropDownList ID="_ddlEvenement" runat="server" Style="font-size: 15px; height: 35px; margin-top: 5px; width: 500px"
                    AppendDataBoundItems="true" AutoPostBack="false" ClientIDMode="AutoID" Font-Size="15px">
                    <asp:ListItem Value="" Text="--- Tous ---" />
                </asp:DropDownList>--%>
                <telerik:RadComboBox ID="ddlEvents" runat="server" CheckBoxes="true" EmptyMessage="Sélectionner un évenement" Style="font-size: 15px; height: 35px; margin-top: 5px; width: 500px"
                    EnableCheckAllItemsCheckBox="true" AutoPostBack="false" DropDownAutoWidth="Enabled" Filter="StartsWith" />
                <asp:Panel ID="pnlTotalPelerin" runat="server" Style="display: inline-block; padding: 10px; border: 5px solid #000; font-size: 20px; font-weight: bold; width: 50px; text-align: center;">
                    <asp:Label runat="server" ID="lblTotalPelerin"></asp:Label>
                </asp:Panel>
                <%--<asp:Label ID="lblNbrPelerins" runat="server" Style="margin-left: 20px; font-weight: bold; font-size: 20px" Visible="false"></asp:Label>--%>
                <div style="display: inline-flex; display: inline-flex; position: relative; top: -10px;">
                    <asp:CheckBox CssClass="cbxFilter" ID="cbxFilterEventChambre" Checked="true" Text="Chambre" ClientIDMode="AutoID" Style="position: relative; top: 5px;" runat="server" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:CheckBox CssClass="cbxFilter" ID="cbxFilterEventPelerin" Checked="true" Text="Pelerin" ClientIDMode="AutoID" Style="position: relative; top: 5px;" runat="server" />
                </div>
            </div>
            <div style="float: left; margin-right: 20px; margin-top: -10px;">
                <br />
                <span style="font-size: 15px; font-weight: bold;">Agence</span><br />
                <asp:DropDownList ID="_ddlAgence" runat="server" Style="font-size: 15px; height: 35px; margin-top: 5px; width: 500px"
                    AppendDataBoundItems="true" AutoPostBack="false" ClientIDMode="AutoID" Font-Size="15px">
                    <asp:ListItem Value="" Text="--- Tous ---" />
                </asp:DropDownList>
                <div style="display: inline-flex">
                    <asp:CheckBox CssClass="cbxFilter" ID="cbxFilterAgenceChambre" Checked="true" Text="Chambre" ClientIDMode="AutoID" Style="position: relative; top: 5px;" runat="server" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:CheckBox CssClass="cbxFilter" ID="cbxFilterAgencePelerin" Checked="true" Text="Pelerin" ClientIDMode="AutoID" Style="position: relative; top: 5px;" runat="server" />
                </div>
            </div>
            <div style="float: left; margin-right: 20px;">
                <br />
                <span style="font-size: 15px; font-weight: bold;">Hotels</span><br />
                <asp:DropDownList ID="_ddlHotels" runat="server" Style="font-size: 15px; height: 35px; margin-top: 5px;"
                    AppendDataBoundItems="true" AutoPostBack="false" ClientIDMode="AutoID" Font-Size="15px">
                    <asp:ListItem Value="-1" Text="--- Tous ---" />
                </asp:DropDownList>
            </div>
            <div style="float: left; margin-right: 20px;">
                <br />
                <span style="font-size: 15px; font-weight: bold;">Chambre</span><br />
                <asp:DropDownList ID="_ddlTypeChambre" runat="server" Style="font-size: 15px; height: 35px; margin-top: 5px;"
                    AppendDataBoundItems="true" AutoPostBack="false" ClientIDMode="AutoID" Font-Size="15px">
                    <asp:ListItem Value="" Text="--- Tous ---" />
                </asp:DropDownList>
            </div>
            <div style="float: left; margin-right: 20px;">
                <br />
                <span style="font-size: 15px; font-weight: bold;">Numero</span><br />
                <asp:DropDownList ID="_ddlNumeroChambre" runat="server" Style="font-size: 15px; height: 35px; margin-top: 5px;"
                    AppendDataBoundItems="true" AutoPostBack="false" ClientIDMode="AutoID" Font-Size="15px">
                    <asp:ListItem Value="-1" Text="--- Tous ---" />
                </asp:DropDownList>
            </div>
            <div style="float: left; margin-right: 20px;">
                <br />
                <span style="font-size: 15px; font-weight: bold;">Statut</span><br />
                <asp:DropDownList ID="_ddlStatutChambre" runat="server" Style="font-size: 15px; height: 35px; margin-top: 5px;"
                    AppendDataBoundItems="true" AutoPostBack="false" ClientIDMode="AutoID" Font-Size="15px">
                </asp:DropDownList>
            </div>
            <div style="float: left; display: none">
                <span style="font-size: 15px; font-weight: bold;">Act. occupées</span><br />
                <asp:DropDownList ID="_ddlOccupe" runat="server" Style="font-size: 15px; height: 35px; margin-top: 5px;"
                    AppendDataBoundItems="true" AutoPostBack="false" ClientIDMode="AutoID" Font-Size="15px">
                    <asp:ListItem Value="-1" Text="--- Tous ---" />
                    <asp:ListItem Value="1" Text="OUI" />
                    <asp:ListItem Value="0" Text="NON" />
                </asp:DropDownList>
            </div>
            <div style="float: left; width: 650px; margin-top: 20px">
                <asp:Button CssClass="button" ID="_btnFilter" runat="server" Text="Filtrer" OnClick="_btnFilter_Click" />
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="padding: 10px; background-color: #f2f2f2; margin: 10px; font-size: 15px; font-weight: bold; width: 650px">
                    <asp:Panel ID="pnlChambreA2" runat="server" Visible="false" Style="margin-bottom: 5px">
                        <asp:Label runat="server" ID="lblChambreA2"></asp:Label>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreRouge"></div>
                            <asp:Label runat="server" ID="lblNbrOcuppesA2"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreOrange"></div>
                            <asp:Label runat="server" ID="lblNbrOuvertesA2"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px">
                            <div class="cadreVert"></div>
                            <asp:Label runat="server" ID="lblNbrVidesA2"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 25px">
                            <span style="font-weight: bold">=</span>
                            <asp:Label runat="server" ID="lblNbrTotalA2" Style="margin-left: 15px"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlChambreA3" runat="server" Visible="false" Style="margin-bottom: 5px">
                        <asp:Label runat="server" ID="lblChambreA3"></asp:Label>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreRouge"></div>
                            <asp:Label runat="server" ID="lblNbrOcuppesA3"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreOrange"></div>
                            <asp:Label runat="server" ID="lblNbrOuvertesA3"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px">
                            <div class="cadreVert"></div>
                            <asp:Label runat="server" ID="lblNbrVidesA3"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 25px">
                            <span style="font-weight: bold">=</span>
                            <asp:Label runat="server" ID="lblNbrTotalA3" Style="margin-left: 15px"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlChambreA4" runat="server" Visible="false" Style="margin-bottom: 5px">
                        <asp:Label runat="server" ID="lblChambreA4"></asp:Label>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreRouge"></div>
                            <asp:Label runat="server" ID="lblNbrOcuppesA4"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreOrange"></div>
                            <asp:Label runat="server" ID="lblNbrOuvertesA4"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px">
                            <div class="cadreVert"></div>
                            <asp:Label runat="server" ID="lblNbrVidesA4"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 25px">
                            <span style="font-weight: bold">=</span>
                            <asp:Label runat="server" ID="lblNbrTotalA4" Style="margin-left: 15px"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlChambreA5" runat="server" Visible="false" Style="margin-bottom: 5px">
                        <asp:Label runat="server" ID="lblChambreA5"></asp:Label>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreRouge"></div>
                            <asp:Label runat="server" ID="lblNbrOcuppesA5"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreOrange"></div>
                            <asp:Label runat="server" ID="lblNbrOuvertesA5"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px">
                            <div class="cadreVert"></div>
                            <asp:Label runat="server" ID="lblNbrVidesA5"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 25px">
                            <span style="font-weight: bold">=</span>
                            <asp:Label runat="server" ID="lblNbrTotalA5" Style="margin-left: 15px"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlChambreA6" runat="server" Visible="false" Style="margin-bottom: 5px">
                        <asp:Label runat="server" ID="lblChambreA6"></asp:Label>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreRouge"></div>
                            <asp:Label runat="server" ID="lblNbrOcuppesA6"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreOrange"></div>
                            <asp:Label runat="server" ID="lblNbrOuvertesA6"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px">
                            <div class="cadreVert"></div>
                            <asp:Label runat="server" ID="lblNbrVidesA6"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 25px">
                            <span style="font-weight: bold">=</span>
                            <asp:Label runat="server" ID="lblNbrTotalA6" Style="margin-left: 15px"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlChambreA7" runat="server" Visible="false" Style="margin-bottom: 5px">
                        <asp:Label runat="server" ID="lblChambreA7"></asp:Label>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreRouge"></div>
                            <asp:Label runat="server" ID="lblNbrOcuppesA7"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreOrange"></div>
                            <asp:Label runat="server" ID="lblNbrOuvertesA7"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px">
                            <div class="cadreVert"></div>
                            <asp:Label runat="server" ID="lblNbrVidesA7"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 25px">
                            <span style="font-weight: bold">=</span>
                            <asp:Label runat="server" ID="lblNbrTotalA7" Style="margin-left: 15px"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlChambreA8" runat="server" Visible="false" Style="margin-bottom: 5px">
                        <asp:Label runat="server" ID="lblChambreA8"></asp:Label>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreRouge"></div>
                            <asp:Label runat="server" ID="lblNbrOcuppesA8"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreOrange"></div>
                            <asp:Label runat="server" ID="lblNbrOuvertesA8"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px">
                            <div class="cadreVert"></div>
                            <asp:Label runat="server" ID="lblNbrVidesA8"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 25px">
                            <span style="font-weight: bold">=</span>
                            <asp:Label runat="server" ID="lblNbrTotalA8" Style="margin-left: 15px"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlChambreA9" runat="server" Visible="false" Style="margin-bottom: 5px">
                        <asp:Label runat="server" ID="lblChambreA9"></asp:Label>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreRouge"></div>
                            <asp:Label runat="server" ID="lblNbrOcuppesA9"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreOrange"></div>
                            <asp:Label runat="server" ID="lblNbrOuvertesA9"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px">
                            <div class="cadreVert"></div>
                            <asp:Label runat="server" ID="lblNbrVidesA9"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 25px">
                            <span style="font-weight: bold">=</span>
                            <asp:Label runat="server" ID="lblNbrTotalA9" Style="margin-left: 15px"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlChambreA10" runat="server" Visible="false" Style="margin-bottom: 5px">
                        <asp:Label runat="server" ID="lblChambreA10"></asp:Label>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreRouge"></div>
                            <asp:Label runat="server" ID="lblNbrOcuppesA10"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreOrange"></div>
                            <asp:Label runat="server" ID="lblNbrOuvertesA10"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px">
                            <div class="cadreVert"></div>
                            <asp:Label runat="server" ID="lblNbrVidesA10"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 25px">
                            <span style="font-weight: bold">=</span>
                            <asp:Label runat="server" ID="lblNbrTotalA10" Style="margin-left: 15px"></asp:Label>
                        </div>
                    </asp:Panel>
                </div>
                <div style="padding: 10px; background-color: #f2f2f2; margin: 10px; padding: 10px; margin-top: -8px; font-size: 15px; font-weight: bold; width: 650px">
                    <asp:Panel ID="pnlTotal" runat="server">
                        <asp:Label runat="server" ID="Total" Text="Total" Style="font-size: 20px;"></asp:Label>
                        <div style="display: inline-block; margin-left: 16px; width: 155px;">
                            <div class="cadreRouge"></div>
                            <asp:Label runat="server" ID="lblNbrTotalOcuppes"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px; width: 155px;">
                            <div class="cadreOrange"></div>
                            <asp:Label runat="server" ID="lblNbrTotalOuvertes"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 20px">
                            <div class="cadreVert"></div>
                            <asp:Label runat="server" ID="lblNbrTotalVides"></asp:Label>
                        </div>
                        <div style="display: inline-block; margin-left: 25px">
                            <span style="font-weight: bold">=</span>
                            <asp:Label runat="server" ID="lblNbrTotal" Style="margin-left: 15px"></asp:Label>
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <div id="genreContainer">
        <asp:Repeater ID="ChambreRepeater" runat="server"
            OnItemDataBound="ChambreRepeater_ItemDataBound">
            <ItemTemplate>
                <b>
                    <asp:Panel ID="pnlch" runat="server" Style="min-width: 260px; display: inline-table;">
                        <div style="width: 100%">
                            <asp:Panel runat="server" ID="Header" Style="border: 1px solid #E6E6E6; text-align: center; font-size: 11px;">
                                <asp:Panel ID="pnlEdit" runat="server" Style="float: right;">
                                    <asp:HyperLink ID="_btnEdit" Text="Edit" runat="server" NavigateUrl="javascript:void(0)" ImageUrl="~/Images/imagesBack/pictoEdit.png" />
                                </asp:Panel>
                                <div>
                                    <asp:Label runat="server" ID="chLblAgence" Style="text-align: center; display: block; background-color: white; float: left; margin: 1px; border: 1px solid; padding: 3px;"></asp:Label>
                                    <asp:Label runat="server" ID="ChambreDetail" Style="text-align: center; float: left; border: 1px solid; margin: 1px; background-color: white; text-align: center; padding: 3px;"></asp:Label>
                                    <asp:Label runat="server" ID="numeroChambre" Text='<%# Eval("Numero") %>' Style="text-align: center; float: left; border: 1px solid; margin: 1px; background-color: white; text-align: center; padding: 3px;"></asp:Label>
                                    <div class="clear"></div>
                                    <asp:Panel runat="server" ID="Vol" Style="display: inline-block; width: 100%;"></asp:Panel>
                                </div>
                            </asp:Panel>
                        </div>
                        <asp:Panel runat="server" ID="pnlChLink">
                            <asp:LinkButton ID="ChambreLink" Style="text-decoration: none !important; position: relative; margin-bottom: 5px; text-align: right; padding-bottom: 5px; padding-right: 10px; min-height: 100px"
                                runat="server" OnClick="ChambreLink_Click" CommandArgument='<%# Eval("ID") %>'>                              
                            </asp:LinkButton>
                            <br />
                        </asp:Panel>
                    </asp:Panel>
                </b>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <asp:Panel runat="server" ID="pelerinContainer" Style="padding: 10px; height: 330px;">
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
                    <div style="width: 98%; margin: 0 auto;">
                        <telerik:RadListViewItemDragHandle ID="RadListViewItemDragHandle1" ClientIDMode="AutoID" runat="server" ToolTip="Drag to organize"></telerik:RadListViewItemDragHandle>
                        <asp:ImageButton ID="ImgRestaurant" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/pictoRestaurant.png" Style="display: inline-block; float: right; margin-top: 5px; border: 1px solid; font-weight: bold; padding: 2px; width: 19px; height: 19px; margin: 2px" />
                        <asp:Label runat="server" ID="chambreDetail" Style="text-align: center; display: inline-block; float: right; border: 1px solid; font-weight: bold; padding: 2px; margin: 2px"></asp:Label>
                        <asp:Label runat="server" ID="pelerinLblAgence" Style="text-align: center; display: inline-block; float: right; border: 1px solid; font-weight: bold; padding: 2px; margin: 2px"></asp:Label>
                        <asp:ImageButton ID="_btnAlert" CssClass="exclamation" runat="server" Visible="false" ImageUrl="~/Images/imagesBack/exclamationMark.png" Style="text-align: center; display: inline-block; float: right; border: 1px solid; font-weight: bold; padding: 2px; margin: 1px" />
                        <div class="clear"></div>
                        <div style="width: 180px; margin: 0 auto; font-size: 11px; font-weight: bold;">
                            <asp:Label runat="server" ID="vol" Style="text-align: center; display: block"></asp:Label>
                            <asp:Label runat="server" ID="volDetail" Style="text-align: center; display: block"></asp:Label>
                            <asp:Label runat="server" ID="infoPelerin"></asp:Label>
                        </div>
                        <div>
                            <img src="~/Images/imagesCommunes/alien.png" alt="" id="_imgPhotoPerson" runat="server" class="imgborder" style="border-radius: 50%; border: 3px solid #fff; width: 50px; height: 50px; position: relative;" />
                        </div>
                    </div>
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
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="_ddlOcupation">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="genreContainer" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="genreContainer">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="genreContainer" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2010Silver" />
</asp:Content>

