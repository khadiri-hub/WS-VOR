<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuBienvenuTopBackUC.ascx.cs"
    Inherits="VOR.Front.Web.UserControls.Menu.MenuBienvenuTopBackUC" %>
<div id="_menubienvenu" runat="server" style="background-color: #5a569c;">

    <div style="float: right; position: relative;">
        <div style="display: table-cell; vertical-align: middle; height: 20px;">

            <div class="disconnect-metro">
                <asp:ImageButton ID="img_btn_logout" ToolTip="Déconnection" runat="server" Style="cursor: pointer"
                    ImageUrl="~/Images/imagesBack/pictoDisconnect.png" OnClick="img_btn_logout_Click" />
            </div>
            <div class="userSpace-metro">
                <asp:Label ID="_lblNomPrenom" runat="server"></asp:Label>
            </div>
            <div class="profil-metro">
                <img id="Img1" src="~/Images/imagesBack/pictoProfil.png" style="filter: invert(100%); cursor:default" runat="server" />
            </div>

            <div class="profil-metro">
                <img id="Img2" src="~/Images/imagesBack/pictoEvenement.png" style="position: relative; bottom: 3px; cursor:default"
                    runat="server"/>
                <asp:DropDownList CssClass="inputText" Style="font-family: Roboto, sans-serif; background: #f2f2f2; border: 0; margin: 4px 0 0 15px; padding: 7px; box-sizing: border-box; font-size: 14px; top: -13px; position: relative; height: 45px;"
                    ID="_ddlEvenement" runat="server" OnSelectedIndexChanged="_ddlEvenement_SelectedIndexChanged"
                    AppendDataBoundItems="true" AutoPostBack="True">
                </asp:DropDownList>
            </div>
        </div>
    </div>

    <div class="clear">
    </div>

</div>
