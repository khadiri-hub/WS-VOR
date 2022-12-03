<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccompagnantsUC.ascx.cs"
    Inherits="VOR.Front.Web.UserControls.Pelerin.AccompagnantsUC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    div.RadListBox .rlbTransferAllFromDisabled, div.RadListBox .rlbTransferAllFrom, div.RadListBox .rlbTransferAllToDisabled, div.RadListBox .rlbTransferAllTo {
        display: none;
    }
    .rlbButtonAreaRight {
        margin-top: 38px;
    }
</style>


<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        function clientTransferred(sender, args) {
            $("#<%= RadAjaxPanel1.ClientID %>").setDirty();
    }
    </script>
</telerik:RadCodeBlock>
<div>
    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
        <div style="float: left; height: 152px; width: 240px;">
            <h4 style="background-color: #e1e1e1; padding: 4px; width: 205px;">Accompagnants</h4>

            <telerik:RadListBox runat="server" ID="_radListBoxSource" height="110px" Width="100%" style="top: -10px"
                AllowTransfer="true" TransferToID="_radListBoxDestination" AutoPostBackOnTransfer="true"
                SelectionMode="Multiple" AllowTransferOnDoubleClick="true" OnTransferred="RadListBoxSource_Transferred" OnClientTransferred="clientTransferred"
                AutoPostBack="true" DataKeyField="ID">
                <itemtemplate>
                    <asp:Label ID="_lblPersonneSource" runat="server"  ><%# DataBinder.Eval(Container, "Text")%></asp:Label>                                
                </itemtemplate>
            </telerik:RadListBox>
        </div>
        <div style="float: left; height: 152px; width: 212px; margin-left: 4px;">
            <h4 style="background-color: #e1e1e1; padding: 4px;">Accompagnants sélectionnés</h4>
            <div style="height: 110px; width: 100%;">
                <telerik:RadListBox ID="_radListBoxDestination" runat="server" Height="100%" Width="100%" style="top: -10px"
                    DataKeyField="ID" SelectionMode="Multiple" AllowTransferOnDoubleClick="true" OnClientTransferred="clientTransferred">
                    <itemtemplate>
                        <asp:Label ID="_lblPersonneDestination" runat="server"><%# DataBinder.Eval(Container, "Text")%></asp:Label>
                    </itemtemplate>
                </telerik:RadListBox>
            </div>
        </div>
    </telerik:RadAjaxPanel>

</div>
