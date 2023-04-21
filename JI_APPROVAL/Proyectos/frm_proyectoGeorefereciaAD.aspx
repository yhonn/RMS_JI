<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_proyectoGeorefereciaAD.aspx.vb" Inherits="ACS_SIME.frm_proyectoGeorefereciaAD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <script language="javascript" type="text/javascript">

        function ActualizarGO(formQ, nameQ) {
            var Lat = 0.00;
            var Long = 0.00;
            var LatNeg = 1;
            var LongNeg = 1;
            var sur = document.getElementById('<%=rbn_sur.ClientID%>');
            var oeste = document.getElementById('<%=rbn_oeste.ClientID%>');
            if (sur.checked == true) { LatNeg = -1; }
            if (oeste.checked == true) { LongNeg = -1; }

            try { formQ.MainContent_chkConfirmacion.checked = false; }
            catch (err) { }
            var glatitud = document.getElementById('<%=txt_gLatitud.ClientID%>');
            var mlatitud = document.getElementById('<%=txt_mLatitud.ClientID%>');
            var slatitud = document.getElementById('<%=txt_sLatitud.ClientID%>');
            Lat = (glatitud.value / 1) + (mlatitud.value / 60) + (slatitud.value / 3600);

            var glongitud = document.getElementById('<%=txt_gLongitud.ClientID%>');
            var mlongitud = document.getElementById('<%=txt_mLongitud.ClientID%>');
            var slongitud = document.getElementById('<%=txt_sLongitud.ClientID%>');

            Long = (glongitud.value / 1) + (mlongitud.value / 60) + (slongitud.value / 3600);

            var lblPuntoLat = document.getElementById('<%=lblPuntoLat.ClientID%>');
            var lblPuntoLong = document.getElementById('<%=lblPuntoLong.ClientID%>');
            lblPuntoLat.value = Math.round(LatNeg * Lat * 100000000) / 100000000;
            lblPuntoLong.value = Math.round(LongNeg * Long * 100000000) / 100000000;
        }

    </script>
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Ficha de Proyecto</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Ficha de georeferencia de la Ficha</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_codigo_ficha" CssClass="control-label text-bold">Código Ficha</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                            <asp:Label ID="lbl_id_proyecto" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lbl_codigo_ficha" runat="server"></asp:Label>
                            <asp:Label ID="lbl_guardado" runat="server" Visible="False"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_nombre" CssClass="control-label text-bold">Nombre de Proyecto</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_ficha" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_ejecutor" CssClass="control-label text-bold">Ejecutor</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_ejecutor" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_subregion" CssClass="control-label text-bold">Subregión</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_nombre_subregion" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_estado" CssClass="control-label text-bold">Estado</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_estado" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_fecha_actualizacion" CssClass="control-label text-bold">Fecha Actualización</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_fecha_actualizacion" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <hr />
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_precision" CssClass="control-label text-bold">Precisión</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadComboBox ID="cmb_precision" runat="server" DataSourceID="SqlDataSource1"
                                DataTextField="nombre_tipo_precision" DataValueField="id_tipo_precisionFicha" Width="350px">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                SelectCommand="SELECT [id_tipo_precisionFicha], [nombre_tipo_precision] FROM [tme_FichaUbicacionProyectoTipoPrecision]"></asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_codigo" CssClass="control-label text-bold">Código</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadComboBox ID="cmb_codigo" runat="server" DataSourceID="SqlDataSource2"
                                DataTextField="nombre_tipo_ubicacioncodigo" DataValueField="id_tipo_ubicacioncodigo" Width="350px">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                SelectCommand="SELECT [id_tipo_ubicacioncodigo], [nombre_tipo_ubicacioncodigo] FROM [tme_FichaUbicacionProyectoTipoCodigo]"></asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_msnm" CssClass="control-label text-bold">M.S.N.M</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadNumericTextBox ID="txt_msnm" runat="server" MaxLength="12" MaxValue="9999999999" MinValue="0" Value="0">
                                <NumberFormat ZeroPattern="n" />
                            </telerik:RadNumericTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                ControlToValidate="txt_msnm" CssClass="Error" Display="Dynamic"
                                ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_ubicacion" CssClass="control-label text-bold">Ubicación</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadTextBox ID="txt_ubicacion" runat="server" Rows="3" TextMode="MultiLine" Width="420px" ValidationGroup="1">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                ControlToValidate="txt_ubicacion" CssClass="Error" Display="Dynamic"
                                ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                        </div>
                        <div class="col-sm-8">
                            <hr />
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_latitud" CssClass="control-label text-bold">Latitud</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:RadioButton ID="rbn_norte" runat="server" Checked="True" GroupName="rrbNS" Text="Norte" />
                            <asp:RadioButton ID="rbn_sur" runat="server" GroupName="rrbNS" Text="Sur" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_grados" CssClass="control-label text-bold">Grados</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadNumericTextBox ID="txt_gLatitud" runat="server" MaxLength="3"
                                MaxValue="90" MinValue="0">
                                <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                            </telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_minutos" CssClass="control-label text-bold">Minutos</asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <telerik:RadNumericTextBox ID="txt_mLatitud" runat="server"
                                DataType="System.Single" MaxLength="2" MaxValue="60" MinValue="0">
                                <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                            </telerik:RadNumericTextBox>
                        </div>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblPuntoLat" runat="server" BackColor="#E8E8E0"
                                BorderColor="#E8E8E0" BorderStyle="None" Font-Bold="True" Font-Size="13pt"
                                ReadOnly="True">0.00</asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_segundos" CssClass="control-label text-bold">Segundos</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadNumericTextBox ID="txt_sLatitud" runat="server" MaxLength="5" MaxValue="60" MinValue="0">
                                <NumberFormat ZeroPattern="n" />
                            </telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                        </div>
                        <div class="col-sm-8">
                            <hr />
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_longitud" CssClass="control-label text-bold">Longitud</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:RadioButton ID="rbn_Este" runat="server" GroupName="rrbEO" Text="Este" />
                            <asp:RadioButton ID="rbn_Oeste" runat="server" GroupName="rrbEO" Text="Oeste" Checked="True" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_grados2" CssClass="control-label text-bold">Grados</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadNumericTextBox ID="txt_gLongitud" runat="server" MaxLength="3" MaxValue="90" MinValue="0">
                                <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                            </telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_minutos2" CssClass="control-label text-bold">Minutos</asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <telerik:RadNumericTextBox ID="txt_mLongitud" runat="server" MaxLength="2" MaxValue="60" MinValue="0">
                                <NumberFormat DecimalDigits="0" ZeroPattern="n" />
                            </telerik:RadNumericTextBox>
                        </div>
                        <div class="col-sm-5">
                            <asp:TextBox ID="lblPuntoLong" runat="server" BackColor="#E8E8E0"
                                BorderColor="#E8E8E0" BorderStyle="None" Font-Bold="True" Font-Size="13pt"
                                ReadOnly="True">0.00</asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_segundos2" CssClass="control-label text-bold">Segundos</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadNumericTextBox ID="txt_sLongitud" runat="server" MaxLength="5" MaxValue="60" MinValue="0">
                                <NumberFormat ZeroPattern="n" />
                            </telerik:RadNumericTextBox>
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" AutoPostBack="true" runat="server" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                            Width="100px" ValidationGroup="1">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar3" runat="server"  AutoPostBack="true" Text="Verificar en mapa" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false" CausesValidation="false" ValidationGroup="2">
                        </telerik:RadButton>
                    </div>
                </div>
                <!-- /.box-footer -->
            </div>
        </div>
    </section>
</asp:Content>

