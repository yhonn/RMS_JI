<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_tasa_cambioAD.aspx.vb" Inherits="RMS_APPROVAL.frm_tasa_cambioAD" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="UbicacionGeografica" Src="~/Controles/UbicacionGeografica.ascx" %>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Agregar tasa SER</asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_fiscal_year" CssClass="control-label text-bold">Año</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadComboBox ID="cmb_year" AutoPostBack="true" runat="server" Width="90%">
                                </telerik:RadComboBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_periodo" CssClass="control-label text-bold">Periodo</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadComboBox ID="cmb_periodo" AutoPostBack="true" runat="server" Width="90%">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_mes" CssClass="control-label text-bold">Mes</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadComboBox ID="cmb_mes" runat="server" Width="90%">
                                </telerik:RadComboBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label runat="server" ID="lblt_tasa_cambio" CssClass="control-label text-bold">Tasa SER</asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <telerik:RadNumericTextBox ID="txt_tasa_cambio" Width="90%" runat="server" MinValue="0">
                                    <NumberFormat ZeroPattern="n" />
                                </telerik:RadNumericTextBox>
                            </div>
                        </div>
                        
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                            Width="100px" ValidationGroup="1">
                        </telerik:RadButton>
                        <div class="alert-sm bg-blue col-sm-6" runat="server" id="div_mensaje" visible="false">
                            <asp:Label runat="server" ID="lbl_ErrorMapa" CssClass="text-center text-bold"></asp:Label>
                            <asp:Label runat="server" ID="lbl_MapaOK" Visible="false" CssClass="text-center text-bold" Font-Size="18px">The coordinates are correct</asp:Label>
                        </div>
                    </div>
                </div>
                <!-- /.box-footer -->
            </div>
        </div>
    </section>
</asp:Content>