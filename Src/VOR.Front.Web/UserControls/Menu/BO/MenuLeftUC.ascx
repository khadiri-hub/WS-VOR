<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuLeftUC.ascx.cs" Inherits="VOR.Front.Web.UserControls.Menu.BO.MenuLeftUC" %>

<div id="secondMenu">
    <div id="secondMenuLimitTop">
    </div>
    <div id="secondMenuLinks">
        <ul id="_menuParametrage" runat="server">
            <li id="_menuItemGestionAgences" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Pages/Parametrage/Agences.aspx">Gestion des Agences</asp:HyperLink>
            </li>
            <li id="_menuItemGestionUtilisateurs" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Pages/Parametrage/Utilisateurs.aspx">Gestion des Utilisateurs</asp:HyperLink>
            </li>
        </ul>
        <ul id="_menuPelerin" runat="server">
            <li id="_menuItemGestionPelerin" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink13" runat="server" NavigateUrl="~/Pages/Pelerin/Pelerins.aspx">Gestion des pelerins</asp:HyperLink>
            </li>
        </ul>
        <ul id="_menuEvenement" runat="server">
            <li id="_menuItemGestionEvenement" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Pages/Evenement/Evenements.aspx">Gestion des événements</asp:HyperLink>
            </li>
            <li id="_menuItemGestionProgramme" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Pages/Evenement/Programmes.aspx">Gestion des programmes</asp:HyperLink>
            </li>
            <li id="_menuItemGestionPnr" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink11" runat="server" NavigateUrl="~/Pages/Evenement/Pnr.aspx">Gestion des pnrs</asp:HyperLink>
            </li>
            <li id="_menuItemGestionCompAerienne" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/Pages/Evenement/Compagnies.aspx">Gestion des compagnies</asp:HyperLink>
            </li>
            <li id="_menuItemGestionVols" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/Pages/Evenement/Vols.aspx">Gestion des vols</asp:HyperLink>
            </li>
            <li id="_menuItemGestionVilles" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/Pages/Evenement/Villes.aspx">Gestion des villes</asp:HyperLink>
            </li>
            <li id="_menuItemGestionHotels" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/Pages/Evenement/Hotels.aspx">Gestion des hôtels</asp:HyperLink>
            </li>
            <li id="_menuItemGestionTypesChambre" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="~/Pages/Evenement/TypeChambres.aspx">Gestion de types <br />de chambre</asp:HyperLink>
            </li>
            <li id="_menuItemGestionChambres" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="~/Pages/Evenement/Chambres.aspx">Gestion de chambres</asp:HyperLink>
            </li>
        </ul>
        <ul id="_menuCollaborateur" runat="server">
            <li id="_menuItemGestionCollaborateur" visible="false" runat="server">
                <asp:HyperLink ID="HyperLink14" runat="server" NavigateUrl="~/Pages/Collaborateur/Collaborateurs.aspx">Gestion des collaborateurs</asp:HyperLink>
            </li>
        </ul>
    </div>
    <div id="secondMenuLimitBottom">
    </div>
</div>

<telerik:RadWindowManager ID="_rwmDdeBraceletSociete" runat="server" Modal="true"
    ShowContentDuringLoad="false" KeepInScreenBounds="true" IconUrl="null" EnableShadow="true"
    ReloadOnShow="true" VisibleStatusbar="false" VisibleOnPageLoad="false" Behaviors="Close">
    <Windows>
        <telerik:RadWindow ID="_rwDdeBraceletSociete" runat="server" Width="770px" Height="724px">
        </telerik:RadWindow>
    </Windows>
</telerik:RadWindowManager>
