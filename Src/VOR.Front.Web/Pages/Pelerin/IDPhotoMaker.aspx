<%@ Page Language="C#" MasterPageFile="~/Popin.Master" AutoEventWireup="true" CodeBehind="IDPhotoMaker.aspx.cs"
    Inherits="VOR.Front.Web.Pages.Pelerin.IDPhotoMaker" EnableEventValidation="true" %>

<%@ Register Src="~/UserControls/Pelerin/IDPhotoMakerUC.ascx" TagName="IDPhotoMakerUC" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        body {
            overflow: hidden;
        }
    </style>

    <uc1:IDPhotoMakerUC ID="IDPhotoMakerUC1" runat="server" />  
</asp:Content>