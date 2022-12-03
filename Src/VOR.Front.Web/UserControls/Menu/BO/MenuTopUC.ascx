<%@ control language="C#" autoeventwireup="true" codebehind="MenuTopUC.ascx.cs" inherits="VOR.Front.Web.UserControls.Menu.BO.MenuTopUC" %>

<%@ register tagprefix="telerik" namespace="Telerik.Web.UI" assembly="Telerik.Web.UI" %>
<style type="text/css">
    .dropbtn {
        background-color: #5a569c;
        color: white;
        padding: 21px;
        height: 49px;
        border: none;
        cursor: pointer;
        font-weight: bold;
        font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
    }

    .dropdown {
        position: relative;
        display: inline-block;
    }

    .dropdown-content {
        display: none;
        position: absolute;
        top: 49px;
        background-color: #5a569c;
        width: 100%;
        box-shadow: 0px 10px 16px 0px rgba(0,0,0,0.2);
        z-index: 1;
    }

        .dropdown-content a {
            line-height: 25px;
            min-height: 25px;
            display: block;
            list-style: none;
            width: 100%;
            border-bottom: 1px solid;
            border-bottom-color: #6a65c2;
            color: #fff;
            padding-top: 5px;
            text-align: center;
            text-decoration: none !important;
        }

            .dropdown-content a:hover {
                background-color: #6a65c2;
            }

    .dropdown:hover .dropdown-content {
        display: block;
    }

    .dropdown:hover .dropbtn {
        background-color: #6a65c2;
    }
</style>
<div id="tabs">

    <asp:Panel ID="_pnlMenuParametrage" Visible="false" CssClass="dropdown" runat="server" data-index="Parametrage">
        <button runat="server" id="btnParametrage" class="dropbtn">Paramétrage</button>
        <div class="dropdown-content" style="left: 0;">
            <asp:HyperLink ID="lnkGestionAgence" Visible="false" runat="server" Text="Agences" NavigateUrl="~/Pages/Parametrage/Agences.aspx" />
            <asp:HyperLink ID="lnkGestionUtilisateurs" Visible="false" runat="server" Text="Utilisateurs" NavigateUrl="~/Pages/Parametrage/Utilisateurs.aspx" />
        </div>
    </asp:Panel>
    <asp:Panel ID="_pnlMenuPelerin" Visible="false" CssClass="dropdown" runat="server" data-index="Pelerin">
        <button runat="server" id="btnPelerin" class="dropbtn">Pelerin</button>
        <div class="dropdown-content" style="left: 0;">
            <asp:HyperLink ID="lnkGestionPelerin" Visible="false" runat="server" Text="Pelerins" NavigateUrl="~/Pages/Pelerin/Pelerins.aspx" Enabled="true" />
        </div>
    </asp:Panel>
    <asp:Panel ID="_pnlHebergement" Visible="false" runat="server" CssClass="dropdown" data-index="Hebergement">
        <button runat="server" id="btnHebergement" class="dropbtn">Hebergement</button>
        <div class="dropdown-content" style="left: 0;">
            <asp:HyperLink ID="lnkVueGlobal" Visible="false" runat="server" Text="Vue Global" NavigateUrl="~/Pages/Hebergement/VueGlobal.aspx" Enabled="true" />
            <asp:HyperLink ID="lnkHebergementMakkahSaison" Visible="false" runat="server" Text="Makkah par saison" NavigateUrl="~/Pages/Hebergement/HebergementSaison.aspx?Id=1" Enabled="true" />
            <asp:HyperLink ID="lnkHebergementMedineSaison" Visible="false" runat="server" Text="Médine par saison" NavigateUrl="~/Pages/Hebergement/HebergementSaison.aspx?Id=2" Enabled="true" />
            <asp:HyperLink ID="lnkHebergementMakkah" Visible="false" runat="server" Text="Makkah" NavigateUrl="~/Pages/Hebergement/Hebergement.aspx?Id=1" Enabled="true" />
            <asp:HyperLink ID="lnkHebergementMedine" Visible="false" runat="server" Text="Médine" NavigateUrl="~/Pages/Hebergement/Hebergement.aspx?Id=2" Enabled="true" />
        </div>
    </asp:Panel>
    <asp:Panel ID="_pnlMenuEvenement" Visible="false" runat="server" CssClass="dropdown" data-index="Evenement">
        <button runat="server" id="btnEvenement" class="dropbtn">Evénement</button>
        <div class="dropdown-content" style="left: 0;">
            <asp:HyperLink ID="lnkEvenement" runat="server" Text="Evénements" NavigateUrl="~/Pages/Evenement/Evenements.aspx" Enabled="true" />
            <asp:HyperLink ID="lnkProgramme" Visible="false" runat="server" Text="Programmes" NavigateUrl="~/Pages/Evenement/Programmes.aspx" Enabled="true" />
            <asp:HyperLink ID="lnkPnr" Visible="false" runat="server" Text="Pnrs" NavigateUrl="~/Pages/Evenement/Pnr.aspx" />
            <asp:HyperLink ID="lnkCompagnies" Visible="false" runat="server" Text="Compagnies" NavigateUrl="~/Pages/Evenement/Compagnies.aspx" />
            <asp:HyperLink ID="lnkVols" Visible="false" runat="server" Text="Vols" NavigateUrl="~/Pages/Evenement/Vols.aspx" />
            <asp:HyperLink ID="lnkVilles" Visible="false" runat="server" Text="Villes" NavigateUrl="~/Pages/Evenement/Villes.aspx" />
            <asp:HyperLink ID="lnkHotels" Visible="false" runat="server" Text="Hôtels" NavigateUrl="~/Pages/Evenement/Hotels.aspx" />
            <asp:HyperLink ID="lnkTypeChambre" Visible="false" runat="server" Text="Types de chambre" NavigateUrl="~/Pages/Evenement/TypeChambres.aspx" />
            <asp:HyperLink ID="lnkChambres" Visible="false" runat="server" Text="Chambres" NavigateUrl="~/Pages/Evenement/Chambres.aspx" />
        </div>
    </asp:Panel>
    <asp:Panel ID="_pnlMenuCollaborateur" Visible="false" runat="server" CssClass="dropdown" data-index="Collaborateur">
        <button runat="server" id="btnCollaborateur" class="dropbtn">Collaborateurs</button>
        <div class="dropdown-content" style="left: 0;">
            <asp:HyperLink ID="lnkCollaborateurs" Visible="false" runat="server" Text="Collaborateurs" NavigateUrl="~/Pages/Collaborateur/Collaborateurs.aspx" Enabled="true" />
        </div>
    </asp:Panel>
</div>
