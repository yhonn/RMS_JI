<%@ Page Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" Inherits="RMS_APPROVAL.no_access2_App" CodeBehind="no_access2_app.aspx.vb" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <section class="content-header">
        <h1>Access Denied
            <%--<small>it all starts here</small>--%>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title"></h3>
            </div>
            <div class="box-body">
                <style type="text/css">
                    a:link {
                        color: #000066;
                        text-decoration: none;
                    }

                    a:visited {
                        color: #000066;
                        text-decoration: none;
                    }

                    a:hover {
                        color: #C1461C;
                        text-decoration: none;
                    }

                    .style2 {
                        height: 26px;
                    }

                    .fonformu {
                        width: 643px;
                    }

                    .style3 {
                        text-align: right;
                        width: 144px;
                    }

                    .style4 {
                        width: 144px;
                    }
                </style>
                <div style="vertical-align: middle; width: 100%;">
                    <table style="padding-left: 15px; padding-right: 15px; width: 100%">
                        <tr style="height: 45px;">
                            <td colspan="5">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 10%;"></td>
                            <td>
                                <img alt="" style="border: 0;" src="../imagenes/iconos/no_access.png" />
                            </td>
                            <td>
                                <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial"
                                    Font-Size="Small" ForeColor="#C00000">Sorry, you don't have access at this Approval Route, please contact your system administrator.</asp:Label>
                            </td>
                            <td class="style4">
                                <img alt="" style="border: 0;" src="../Images/activities/Chemonics-log150_nw.png" />
                            </td>
                            <td style="width: 10%;"></td>
                        </tr>
                        <tr style="height: 45px;">
                            <td colspan="5">&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </section>
</asp:Content>


