<%@ Page Title="Log in" Language="VB" AutoEventWireup="true" CodeBehind="frm_login.aspx.vb" Inherits="RMS_APPROVAL.Login" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head id="Head1" runat="server">
    <title><%--[::. ARTISANAL GOLD MINING .::]--%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="~/Content/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/Content/dist/css/AdminLTE.min.css" />
    <link href="~/Content/Input.MetroTouch.css" rel="stylesheet" />
    <%--<script type="text/javascript" src="../Content/Jquery/jQuery-2.1.4.min.js"></script>
    <script type="text/javascript" src="../Content/bootstrap/js/bootstrap.min.js"></script>--%>
</head>

<body class="hold-transition login-page">
    <div class="cenform">
        <form id="form1" method="post" runat="server" enctype="multipart/form-data">
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"
                ToolTip="Cargando..." Transparency="50" />
            <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" LoadingPanelID="RadAjaxLoadingPanel1">
            </telerik:RadAjaxPanel>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            </telerik:RadScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="login-box">
                        <div class="login-logo">                          
                            <b>A</b>pprovals
                        </div>
                        <!-- /.login-logo -->
                        <div class="login-box-body">
                            <%--<p class="login-box-msg">Sign in to start your session</p>--%>
                            <div class="form-group has-feedback">
                                <input class="form-control" placeholder="User" runat="server" id="txt_usu" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txt_usu" ErrorMessage="Requerido" ForeColor="#C00000"></asp:RequiredFieldValidator>
                                <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
                            </div>
                            <div class="form-group has-feedback">
                                <%--<input type="password" class="form-control" placeholder="Contraseña" runat="server" id="txt_pass" />--%>
                                <%--<telerik:RadTextBox ID="txt_pass" runat="server" Width="100%" TextMode="Password">
                                    </telerik:RadTextBox>--%>
                                <input type="password" class="form-control" placeholder="Password" runat="server" id="txt_pass" />
                                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txt_pass" ErrorMessage="Requerido" ForeColor="#C00000"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group has-feedback">
                                <telerik:RadComboBox ID="rdC_Programas"
                                    runat="server"
                                    Width="320"
                                    DropDownWidth="320"
                                    EmptyMessage="Choose a Project"
                                    HighlightTemplatedItems="true"
                                    DataTextField="nombre_programa"
                                    DataValueField="id_programa" >
                                </telerik:RadComboBox>

                            </div>
                            <div class="row">
                                <div class="col-xs-8">
                                </div>
                                <!-- /.col -->
                                <div class="col-xs-5">
                                    <asp:Button type="submit" class="btn btn-primary btn-block btn-flat" runat="server" ID="btn_login_2" Text="Log in" data-dismiss="alert" aria-label="Close"></asp:Button>
                                </div>
                                <!-- /.col -->

                            </div>
                            <div class="form-group has-feedback">
                                <br />
                                <div class="alert alert-warning alert-dismissible fade in" role="alert" runat="server" id="alert" visible="false">
                                    <%--<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">×</span></button>--%>
                                    <asp:Label ID="lblerr_user" runat="server" Visible="False">[ Wrong User or Password ]</asp:Label>
                                </div>
                            </div>
                        </div>
                        <!-- /.login-box-body -->
                    </div>
                    <!-- /.login-box -->
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
