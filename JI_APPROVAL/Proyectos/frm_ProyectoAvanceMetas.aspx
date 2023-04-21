<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ProyectoAvanceMetas.aspx.vb" Inherits="ACS_SIME.frm_ProyectoAvanceMetas" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Seguimiento a Proyectos</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Proyecto - Metas</asp:Label>
                    <asp:Label runat="server" ID="lbl_informacionindicador"></asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="lbl_id_ficha_proyecto" CssClass="hidden"></asp:Label>
                                <asp:Label runat="server" ID="lblt_codigo_proyecto" CssClass="control-label text-bold">Código de Proyecto</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_codigoSAPME" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="lblt_nombre_proyecto" CssClass="control-label text-bold">Nombre de Proyecto</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_proyecto" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="lblt_region" CssClass="control-label text-bold">Región</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_region" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="lblt_componente" CssClass="control-label text-bold">Componente</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_componente" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="lblt_ejecutor" CssClass="control-label text-bold">Ejecutor</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_ejecutor" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="lblt_fecha_inicio" CssClass="control-label text-bold">Fecha Inicio</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_fechainicio" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="lblt_fecha_fin" CssClass="control-label text-bold">Fecha Fin</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_fechafin" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="lblt_estado_proyecto" CssClass="control-label text-bold">Estado del Proyecto</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_estadoFicha" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="lblt_tasa_cambio" CssClass="control-label text-bold">Tasa de Cambio</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_tasaCambio" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <hr />
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="lblt_periodo" CssClass="control-label text-bold">Periodo</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_periodoActivo" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Activo</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_periodoActivoSN" runat="server">SI</asp:Label>
                                <asp:Image ID="img_periodoActivoSN" runat="server" ImageUrl="~/Imagenes/iconos/flag_green.png" ToolTip="Activo" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Estatus del periodo</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_periodoActivoEstatus" runat="server">Periodo ACTIVO. Permite registrar datos</asp:Label>
                                <asp:Image ID="img_periodoActivoEstatus" runat="server" ImageUrl="~/Imagenes/iconos/flag_green.png" ToolTip="Periodo ACTIVO. Permite registrar datos" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Sincronización MGR</asp:Label>
                            </div>
                            <div class="col-sm-9">
                                <asp:Label ID="lbl_periodoActivoEstatusSync" runat="server">Los datos se han actualizado correctamente. No requiere actualizar o precesar información.</asp:Label>
                                <asp:Image ID="img_periodoActivoEstatusSync" runat="server" ImageUrl="~/Imagenes/iconos/flag_green.png" ToolTip="Periodo ACTIVO. Permite registrar datos" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <hr />
                            <div class="col-sm-3 text-right">
                                <asp:Label runat="server" ID="lblt_avance_indicadores" CssClass="control-label text-bold">Avance Indicadores</asp:Label>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3 text-right">
                            </div>
                            <div class="col-sm-9">
                                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True"
                                    CellSpacing="0" AllowPaging="True" PageSize="15"
                                    AutoGenerateColumns="False">
                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_indicador" AllowAutomaticUpdates="True">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_indicador"
                                                FilterControlAltText="Filter id_indicador column"
                                                SortExpression="id_indicador" UniqueName="id_indicador"
                                                Visible="False" DataType="System.Int32" HeaderText="id_indicador"
                                                ReadOnly="True">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="codigo_indicador"
                                                FilterControlAltText="Filter codigo_indicador column" HeaderText="Código Indicador" UniqueName="colm_codigo_indicador">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_indicador_LB"
                                                FilterControlAltText="Filter nombre_indicador_LB column" HeaderText="Indicador" UniqueName="colm_nombre_indicador">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="nombre_tipo_umedida"
                                                FilterControlAltText="Filter nombre_tipo_umedida column"
                                                HeaderText="Unidad de Medida" SortExpression="nombre_tipo_umedida"
                                                UniqueName="colm_nombre_tipo_umedida">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="meta_total"
                                                FilterControlAltText="Filter meta_total column" HeaderText="Meta Total"
                                                SortExpression="meta_total"
                                                UniqueName="colm_meta_total">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ejecutado"
                                                FilterControlAltText="Filter ejecutado column"
                                                HeaderText="Ejecutado" SortExpression="ejecutado"
                                                UniqueName="colm_ejecutado">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="porcentajeAvance"
                                                FilterControlAltText="Filter porcentajeAvance column"
                                                HeaderText="% Avance" SortExpression="porcentajeAvance"
                                                UniqueName="colm_porcentajeAvance">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="alerta" HeaderText="Alerta" UniqueName="colm_alerta"></telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3"></div>
                            <div class="col-sm-9" runat="server" id="containerChart">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:Literal ID="ltrChart" runat="server"></asp:Literal>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts.js")%>"></script>
                                <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/exporting.js")%>"></script>
                                <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/data.js")%>"></script>
                                <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/drilldown.js")%>"></script>
                                <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/themes/sand-signika.js")%>"></script>

                            </div>
                        </div>
                        <div class="form-group row hidden">
                            <telerik:RadButton ID="btn_guardar" runat="server"
                                Text="Informe por reporte simple" Skin="Simple"
                                Width="160px" ValidationGroup="1" Enabled="false">
                            </telerik:RadButton>
                            <telerik:RadButton ID="btn_guardarSync" runat="server"
                                Text="Actualización de datos" Skin="Simple"
                                Width="160px" ValidationGroup="1" Enabled="false">
                            </telerik:RadButton>
                            <asp:Label ID="lblproyecto18" runat="server" Font-Bold="True"
                                Font-Strikeout="False" Font-Underline="False" Text="Avance de indicadores"></asp:Label>
                            &nbsp;<br />
                            <telerik:RadTreeView ID="TreeViewMetas" runat="server" Skin="Sitefinity"
                                Width="950px">
                            </telerik:RadTreeView>
                            <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                SelectCommand="SELECT ID_TRIMESTRE, NOMBRE_TRIMESTRE FROM VW_PLANIFICACION_TRIMESTRE WHERE ID_PROYECTO =-1"></asp:SqlDataSource>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

