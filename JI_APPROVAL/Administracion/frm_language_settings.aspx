<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_language_settings.aspx.vb" Inherits="RMS_APPROVAL.frm_language_settings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Configuración Lenguaje</asp:Label></h3>
                <asp:HiddenField runat="server" ID="lenguajeID" />
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <i class="fa fa-question-circle" style="font-size:40px; color: blue;"></i>
                    </div>
                    <div class="col-sm-10">
                        <asp:Label runat="server" ID="lblt_explicacion" CssClass="control-label page-header">De clic al icono de lenguaje para traducir el tema seleccionado</asp:Label>
                    </div>
                </div>
                <div class="row">
                    <hr />
                    <div class="col-md-4 col-sm-6 col-xs-12">
                        <a href="<%= ResolveUrl("~/Administracion/frm_AdminLanguage")%>">
                            <div class="info-box bg-gray box-shadow">
                                <span class="info-box-icon bg-aqua"><i class="fa fa-language"></i></span>
                                <div class="info-box-content">
                                    <span class="info-box-text">Módulos</span>
                                    <span class="info-box-number"><small>Titulos, Textos y controles </small></span>
                                </div>
                                <!-- /.info-box-content -->
                            </div>
                        </a>
                        <!-- /.info-box -->
                    </div>
                    <%--</div>
                <div class="row">--%>
                    <!-- /.col -->
                    <div class="col-md-4 col-sm-6 col-xs-12">
                        <a href="<%= ResolveUrl("~/Administracion/frm_AdminLanguageControles?IdI=" & Me.lenguajeID.Value)%>">
                        <div class="info-box bg-gray box-shadow">
                            <span class="info-box-icon bg-red"><i class="fa fa-language"></i></span>
                            <div class="info-box-content">
                                <span class="info-box-text">Controles Generales</span>
                                <span class="info-box-number"><small>Alertas y mensajes</small></span>
                            </div>
                            <!-- /.info-box-content -->
                        </div>
                            </a>
                        <!-- /.info-box -->
                    </div>
                    <!-- /.col -->
                    <%--                </div>
                <div class="row">--%>
                    <div class="col-md-4 col-sm-6 col-xs-12">
                        <a href="<%= ResolveUrl("~/SuperAdmin/frm_menulenguageAD?IdI=" & Me.lenguajeID.Value)%>">
                            <div class="info-box bg-gray box-shadow">
                                <span class="info-box-icon bg-green"><i class="fa fa-language"></i></span>
                                <div class="info-box-content">
                                    <span class="info-box-text">Menú</span>
                                    <span class="info-box-number"></span>
                                </div>
                                <!-- /.info-box-content -->
                            </div>
                        </a>
                        <!-- /.info-box -->
                    </div>
                    <!-- /.col -->
                </div>
                <div class="box-footer">
                    <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                    </telerik:RadButton>
                </div>
            </div>
        </div>
    </section>
</asp:Content>


