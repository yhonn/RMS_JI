<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Graficas.Master" CodeBehind="grp_GraphAvanceEjecucionInd.aspx.vb" Inherits="ACS_SIME.grp_GraphAvanceEjecucionInd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <%--<asp:ScriptManager runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Literal ID="ltrChart" runat="server"></asp:Literal>
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </div>
</asp:Content>
