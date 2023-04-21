<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_proyectoOrdInicioAD.aspx.vb" Inherits="ACS_SIME.frm_proyectoOrdInicioAD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Ficha de Proyecto</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Instrumentos Relacionados</asp:Label></h3>
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
                            <asp:Label ID="lbl_id_ejecutor" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lbl_id_componente" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lbl_codigo_ficha" runat="server"></asp:Label>
                            <asp:Label ID="lbl_guardado" runat="server" Visible="False"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_codigo_temporal" CssClass="control-label text-bold">Código de Ficha Temporal</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_id_codigo" runat="server"></asp:Label>
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
                            <asp:Label runat="server" ID="lblt_fecha_orden" CssClass="control-label text-bold">Fecha de orden de inicio</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadDatePicker ID="dt_fecha_inicio" runat="server">
                                <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x">
                                </Calendar>
                            </telerik:RadDatePicker>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                ControlToValidate="dt_fecha_inicio" CssClass="Error" Display="Dynamic"
                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-4 text-right">
                            <asp:Label runat="server" ID="lblt_usuario" CssClass="control-label text-bold">Usuario Aprueba</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <asp:Label ID="lbl_usuario" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group row">
                        <hr />
                        <div class="col-sm-4 text-right">
                        </div>
                        <div class="col-sm-8">
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <label class="text-center cienporciento">
                                <asp:Label runat="server" ID="lblt_lista_instrumentos" CssClass="control-label text-bold">Lista de instrumentos relacionados con indicadores</asp:Label></label>
                            <telerik:RadGrid ID="grd_Instrumentos1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource11">
                                <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_instrumento" DataSourceID="SqlDataSource11">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_instrumento"
                                            FilterControlAltText="Filter  id_instrumento column" HeaderText="id_instrumento"
                                            SortExpression="id_instrumento" UniqueName="id_instrumento" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Number"
                                            FilterControlAltText="Filter Number column" HeaderText="No"
                                            SortExpression="Number" UniqueName="Number">
                                            <HeaderStyle Width="10px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_instrumento"
                                            FilterControlAltText="Filter nombre_instrumento column" HeaderText="Nombre del instrumento"
                                            SortExpression="nombre_instrumento" UniqueName="colm_nombre_instrumento">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="definicion_indicador"
                                            FilterControlAltText="Filter definicion_indicador column" HeaderText="Definición del indicador"
                                            SortExpression="definicion_indicador" UniqueName="colm_definicion_indicador">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_tipo_umedida"
                                            FilterControlAltText="Filter nombre_tipo_umedida column" HeaderText="Unidad Medida"
                                            SortExpression="nombre_tipo_umedida" UniqueName="colm_nombre_tipo_umedida">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <EditFormSettings>
                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                        </EditColumn>
                                    </EditFormSettings>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <asp:SqlDataSource ID="SqlDataSource11" runat="server"
                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                SelectCommand="SELECT ROW_NUMBER() OVER(ORDER BY id_InstrumentoIndicador ASC) as Number, definicion_indicador, nombre_tipo_umedida, nombre_instrumento, id_instrumento FROM vw_tme_Instrumento_Componente_Indicador">
                                <SelectParameters>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="form-group row">
                        <hr />
                        <div class="col-sm-12">
                            <label class="text-center cienporciento">
                                <asp:Label runat="server" ID="lblt_activar_instrumentos" CssClass="control-label text-bold">Activar instrumentos de seguimiento interno</asp:Label></label>
                            <telerik:RadGrid ID="grd_Instrumentos3" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource13">
                                <ClientSettings EnableRowHoverStyle="true">
                                    <Selecting AllowRowSelect="True" />
                                </ClientSettings>
                                <MasterTableView DataKeyNames="id_instrumento" DataSourceID="SqlDataSource13">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_instrumento"
                                            FilterControlAltText="Filter id_instrumento column" HeaderText="id_instrumento"
                                            SortExpression="id_instrumento" UniqueName="id_instrumento" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                            HeaderText="" UniqueName="TemplateColumnAnual">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkHRSeguimiento" runat="server" ToolTip="Activar instrumento" />
                                                <cc1:ToggleButtonExtender ID="ToggleButtonExtender3" runat="server"
                                                    CheckedImageUrl="../Imagenes/iconos/drop-yes.gif" ImageHeight="16" ImageWidth="16"
                                                    TargetControlID="ChkHRSeguimiento" UncheckedImageUrl="../Imagenes/iconos/Circle_Gray.png">
                                                </cc1:ToggleButtonExtender>
                                            </ItemTemplate>
                                            <HeaderStyle Width="15px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="Number"
                                            FilterControlAltText="Filter Number column" HeaderText="No"
                                            SortExpression="Number" UniqueName="Number">
                                            <HeaderStyle Width="10px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="codigo_instrumento"
                                            FilterControlAltText="Filter codigo_instrumento column" HeaderText="Código Instrumento"
                                            SortExpression="codigo_instrumento" UniqueName="colm_codigo_instrumento">
                                            <HeaderStyle Width="140px" />
                                            <ItemStyle Width="140px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_instrumento"
                                            FilterControlAltText="Filter nombre_instrumento column" HeaderText="Instrumento"
                                            SortExpression="nombre_instrumento" UniqueName="colm_nombre_instrumento">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <asp:SqlDataSource ID="SqlDataSource13" runat="server"
                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                SelectCommand="SELECT ROW_NUMBER() OVER(ORDER BY id_instrumento ASC) as Number, id_instrumento, codigo_instrumento, nombre_instrumento FROM tme_instrumentos WHERE (id_tipo_instrumento = 2)"></asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="form-group row">
                        <hr />
                        <label class="text-center cienporciento">
                            <asp:Label runat="server" ID="lblt_activar_informe" CssClass="control-label text-bold">Activar informe de avance de indicadores por reporte simple y permisos especiales que requieren validacion de M&E</asp:Label></label>
                        <div class="col-sm-12">
                            <telerik:RadGrid ID="grd_Instrumentos2" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource12">
                                <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_indicador" DataSourceID="SqlDataSource12">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_indicador"
                                            FilterControlAltText="Filter id_indicador column" HeaderText="id_indicador"
                                            SortExpression="id_indicador" UniqueName="id_indicador" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                            HeaderText="" UniqueName="TemplateColumnAnual">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkHerramientas" runat="server" ToolTip="Permitir avance por reporte simple" />
                                                <cc1:ToggleButtonExtender ID="ToggleButtonExtender2" runat="server"
                                                    CheckedImageUrl="../Imagenes/iconos/drop-yes.gif" ImageHeight="16" ImageWidth="16"
                                                    TargetControlID="ChkHerramientas" UncheckedImageUrl="../Imagenes/iconos/Circle_Gray.png">
                                                </cc1:ToggleButtonExtender>
                                            </ItemTemplate>
                                            <HeaderStyle Width="15px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                            UniqueName="visible">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActivo" runat="server" ToolTip="Requieren validacion de ME" />
                                                <cc1:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server"
                                                    CheckedImageUrl="../Imagenes/iconos/flag_blue.png" ImageHeight="16" ImageWidth="16"
                                                    TargetControlID="chkActivo" UncheckedImageUrl="../Imagenes/iconos/flag_gray.png">
                                                </cc1:ToggleButtonExtender>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10px" />
                                            <ItemStyle Width="10px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="Number"
                                            FilterControlAltText="Filter Number column" HeaderText="No"
                                            SortExpression="Number" UniqueName="Number">
                                            <HeaderStyle Width="10px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="definicion_indicador"
                                            FilterControlAltText="Filter definicion_indicador column" HeaderText="Definición del indicador"
                                            SortExpression="definicion_indicador" UniqueName="colm_definicion_indicador">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_tipo_umedida"
                                            FilterControlAltText="Filter nombre_tipo_umedida column" HeaderText="Unidad Medida"
                                            SortExpression="nombre_tipo_umedida" UniqueName="colm_nombre_tipo_umedida">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <asp:SqlDataSource ID="SqlDataSource12" runat="server"
                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                SelectCommand="SELECT ROW_NUMBER() OVER(ORDER BY id_indicador ASC) as Number, definicion_indicador, nombre_tipo_umedida, nombre_instrumento, id_indicador FROM vw_tme_Instrumento_Componente_Indicador">
                                <SelectParameters>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="form-group row">
                        <hr />
                        <label class="text-center cienporciento">
                            <asp:Label runat="server" ID="lblt_otras_herramientas" CssClass="control-label text-bold">Otras herramientas activas por defecto</asp:Label></label>
                        <div class="col-sm-12">
                            <telerik:RadGrid ID="grd_Instrumentos4" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource14">
                                <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_instrumento" DataSourceID="SqlDataSource14">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_instrumento"
                                            FilterControlAltText="Filter id_instrumento column" HeaderText="id_instrumento"
                                            SortExpression="id_instrumento" UniqueName="id_instrumento" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Number"
                                            FilterControlAltText="Filter Number column" HeaderText="No"
                                            SortExpression="Number" UniqueName="Number">
                                            <HeaderStyle Width="10px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="codigo_instrumento"
                                            FilterControlAltText="Filter codigo_instrumento column" HeaderText="Código Instrumento"
                                            SortExpression="codigo_instrumento" UniqueName="colm_codigo_instrumento">
                                            <HeaderStyle Width="125px" />
                                            <ItemStyle Width="125px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_instrumento"
                                            FilterControlAltText="Filter nombre_instrumento column" HeaderText="Instrumento"
                                            SortExpression="nombre_instrumento" UniqueName="colm_nombre_instrumento">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <asp:SqlDataSource ID="SqlDataSource14" runat="server"
                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                SelectCommand="SELECT ROW_NUMBER() OVER(ORDER BY id_instrumento ASC) as Number, id_instrumento, codigo_instrumento, nombre_instrumento FROM tme_instrumentos WHERE (id_tipo_instrumento = 3)"></asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" AutoPostBack="true" runat="server" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                            Width="100px" ValidationGroup="1">
                        </telerik:RadButton>
                    </div>
                </div>
                <!-- /.box-footer -->
            </div>
        </div>
    </section>

</asp:Content>
