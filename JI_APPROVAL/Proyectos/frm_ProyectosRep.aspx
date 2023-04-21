<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPopUp.Master" CodeBehind="frm_ProyectosRep.aspx.vb" Inherits="ACS_SIME.frm_ProyectosRep" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row invoice-info">
        <div class="col-md-12">
            <table class="table table-responsive table-condensed box box-primary">
                <tr>
                    <td>
                        <asp:Label ID="lblt_codigo_proyecto" runat="server" CssClass="control-label text-bold">Código de Proyecto:</asp:Label>
                        <asp:Label ID="lbl_codigo_proyecto" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_estado_proyecto" runat="server" CssClass="control-label text-bold">Estado del Proyecto:</asp:Label>
                        <asp:Label ID="lbl_estado_proyecto" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_nombre_proyecto" runat="server" CssClass="control-label text-bold">Nombre del Proyecto:</asp:Label>
                        <asp:Label ID="lbl_nombre_proyecto" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_descripcion" runat="server" CssClass="control-label text-bold">Descripción del Proyecto:</asp:Label>
                        <asp:Label ID="lbl_descripcion" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_fecha_inicio" runat="server" CssClass="control-label text-bold">Fecha Inicio:</asp:Label>
                        <asp:Label ID="lbl_fecha_inicio" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_fecha_fin" runat="server" CssClass="control-label text-bold">Fecha Fin:</asp:Label>
                        <asp:Label ID="lbl_fecha_fin" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_region" runat="server" CssClass="control-label text-bold">Región:</asp:Label>
                        <asp:Label ID="lbl_region" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_subregion" runat="server" CssClass="control-label text-bold">Sub Región:</asp:Label>
                        <asp:Label ID="lbl_subregion" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="hidden">
                    <td>
                        <asp:Label ID="lblt_componente" runat="server" CssClass="control-label text-bold">Componente:</asp:Label>
                        <asp:Label ID="lbl_componente" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_ejecutor" runat="server" CssClass="control-label text-bold">Ejecutor:</asp:Label>
                        <asp:Label ID="lbl_ejecutor" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblt_codigoAID" runat="server" CssClass="control-label text-bold">Código Ficha AID:</asp:Label>
                        <asp:Label ID="lbl_codigoAID" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table class="table table-responsive table-condensed">
                <tr class="tituloExitoso text-center">
                    <td>
                        <asp:Label ID="lblt_municipios_atendidos" runat="server" CssClass="control-label text-bold">Municipios Atendidos</asp:Label></td>
                </tr>
                <tr class="box box-success">
                    <td>
                        <telerik:RadGrid runat="server" ID="grd_municipios" AutoGenerateColumns="False">
                            <MasterTableView>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="nombre_municipio"
                                        FilterControlAltText="Filter nombre_municipio column" HeaderText="Municipio"
                                        SortExpression="nombre_municipio" UniqueName="colm_nombre_municipio">
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField FieldAlias="nombre_departamento" FieldName="nombre_departamento"
                                                HeaderText=" " HeaderValueSeparator="" />
                                        </SelectFields>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldAlias="nombre_departamento" FieldName="nombre_departamento"
                                                FormatString="" HeaderText="" />
                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <table class="table table-responsive table-condensed">
                <tr class="tituloExitoso text-center">
                    <td>
                        <asp:Label ID="lblt_sectores" runat="server" CssClass="control-label text-bold">Sectores Asociados</asp:Label></td>
                </tr>
                <tr class="box box-success">
                    <td>
                        <telerik:RadGrid runat="server" ID="grd_sectores" AutoGenerateColumns="False">
                            <MasterTableView>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="nombre_sector_me"
                                        FilterControlAltText="Filter nombre_sector_me column" HeaderText="Sector"
                                        SortExpression="nombre_sector_me" UniqueName="colm_nombre_sector_me">
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <GroupByExpressions>
                                    <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField FieldAlias="nombre_tipo_sector_me"
                                                FieldName="nombre_tipo_sector_me" HeaderText=" " HeaderValueSeparator=" " />
                                        </SelectFields>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldAlias="nombre_tipo_sector_me"
                                                FieldName="nombre_tipo_sector_me" FormatString="" HeaderText="" />
                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                </GroupByExpressions>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <table class="table table-responsive table-condensed">
                <tr class="tituloExitoso text-center">
                    <td>
                        <asp:Label ID="lblt_indicadores_meta" runat="server" CssClass="control-label text-bold">Indicadores Meta</asp:Label></td>
                </tr>
                <tr class="box box-success">
                    <td>
                        <telerik:RadGrid runat="server" ID="grd_indicadores" AutoGenerateColumns="False">
                            <MasterTableView>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="codigo_indicador"
                                        FilterControlAltText="Filter codigo_indicador column" HeaderText="Código"
                                        SortExpression="codigo_indicador" UniqueName="codigo_indicador">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="definicion_indicador"
                                        FilterControlAltText="Filter definicion_indicador column" HeaderText="Indicador"
                                        SortExpression="definicion_indicador" UniqueName="colm_definicion_indicador">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="nombre_metodo_operacion"
                                        FilterControlAltText="Filter nombre_metodo_operacion column" HeaderText="Método Operación"
                                        SortExpression="nombre_metodo_operacion" UniqueName="colm_nombre_metodo_operacion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="nombre_tipo_umedida"
                                        FilterControlAltText="Filter nombre_tipo_umedida column" HeaderText="Método Operación"
                                        SortExpression="nombre_tipo_umedida" UniqueName="colm_nombre_tipo_umedida">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="meta_total"
                                        FilterControlAltText="Filter meta_total column" HeaderText="Meta Total"
                                        SortExpression="meta_total" UniqueName="colm_meta_total">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <table class="table table-responsive table-condensed">
                <tr class="tituloExitoso text-center">
                    <td>
                        <asp:Label ID="lblt_aportes" runat="server" CssClass="control-label text-bold">Aportes al proyecto</asp:Label></td>
                </tr>
                <tr class="box box-success">
                    <td>
                        <telerik:RadGrid runat="server" ID="grd_aportes" AutoGenerateColumns="False">
                            <MasterTableView>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="nombre_aporte"
                                        FilterControlAltText="Filter nombre_aporte column" HeaderText="Origen del aporte"
                                        SortExpression="nombre_aporte" UniqueName="colm_nombre_aporte">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="monto_aporte"
                                        FilterControlAltText="Filter monto_aporte column" HeaderText="Monto Estimado"
                                        SortExpression="monto_aporte" UniqueName="colm_monto_aporte">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="monto_aporte_obligado"
                                        FilterControlAltText="Filter monto_aporte_obligado column" HeaderText="Monto total Obligado"
                                        SortExpression="monto_aporte_obligado" UniqueName="colm_monto_aporte_obligado">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TotalUSD" DataFormatString="{0:N}"
                                        FilterControlAltText="Filter TotalUSD column" HeaderText="Valor USD"
                                        SortExpression="TotalUSD" UniqueName="colm_TotalUSD">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td class="text-right">
                        <h4>
                            <asp:Label runat="server" ID="lblt_total_aporte" CssClass="text-bold">Total Aporte:</asp:Label>
                            <asp:Label runat="server" ID="lbl_total"></asp:Label>
                        </h4>
                        <h4>
                            <asp:Label runat="server" ID="lblt_total_aporteUSD" CssClass="text-bold">Total Aporte USD:</asp:Label>
                            <asp:Label runat="server" ID="lbl_totalUSD"></asp:Label>
                        </h4>
                    </td>
                </tr>
                <tr>
                    <td class="text-right"></td>
                </tr>
            </table>
            <table class="table table-responsive table-condensed">
                <tr class="tituloExitoso text-center">
                    <td>
                        <asp:Label ID="lblt_documentos" runat="server" CssClass="control-label text-bold">Documentos Adjuntos</asp:Label></td>
                </tr>
                <tr class="box box-success">
                    <td>
                        <telerik:RadGrid ID="GrdArchivos" runat="server" AllowAutomaticDeletes="True"
                            AllowAutomaticUpdates="True" AutoGenerateColumns="False" CellSpacing="0"
                            DataSourceID="SqlDataSource7" GridLines="None" ShowHeader="False">
                            <MasterTableView DataSourceID="SqlDataSource7">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Number"
                                        FilterControlAltText="Filter archivo column" HeaderText="Number"
                                        SortExpression="Number" UniqueName="Number">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="nombre_documento"
                                        FilterControlAltText="Filter archivo column" HeaderText="nombre_documento"
                                        SortExpression="nombre_documento" UniqueName="colm_nombre_documento">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="id_anexo_ficha"
                                        FilterControlAltText="Filter nombre_documento column" HeaderText="Type"
                                        SortExpression="id_anexo_ficha" UniqueName="id_anexo_ficha">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn FilterControlAltText="Filter Completo column"
                                        UniqueName="AttachFile">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="Attach" runat="server" ImageUrl="~/Imagenes/iconos/adjunto.png"
                                                ToolTip="Descargar">
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Width="5px" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                            ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                            SelectCommand="SELECT * FROM tme_Anexos_ficha WHERE id_ficha_proyecto = @id">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="id" QueryStringField="id" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
            <table class="table table-responsive table-condensed">
                <tr class="tituloExitoso text-center">
                    <td>
                        <asp:Label ID="lblt_imagenes" runat="server" CssClass="control-label text-bold">Imágenes Adjuntas</asp:Label></td>
                </tr>
                <tr class="box box-success">
                    <td>
                        <telerik:RadGrid ID="GrdArchivosImg" runat="server" AllowAutomaticDeletes="True"
                            AllowAutomaticUpdates="True" AutoGenerateColumns="False" CellSpacing="0"
                            DataSourceID="SqlDataSource8" GridLines="None"
                            ShowHeader="False">
                            <MasterTableView DataSourceID="SqlDataSource8">
                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"
                                    Visible="True">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"
                                    Visible="True">
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Number"
                                        FilterControlAltText="Filter archivo column" HeaderText="Number"
                                        SortExpression="Number" UniqueName="Number">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="nombre_archivo_proyecto"
                                        FilterControlAltText="Filter nombre_archivo_proyecto column" HeaderText="nombre_archivo_proyecto"
                                        SortExpression="nombre_archivo_proyecto" UniqueName="colm_nombre_archivo_proyecto">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="nombre_tipo_proyecto_imagen"
                                        FilterControlAltText="Filter nombre_tipo_proyecto_imagen column" HeaderText="nombre_tipo_proyecto_imagen"
                                        SortExpression="nombre_tipo_proyecto_imagen" UniqueName="colm_nombre_tipo_proyecto_imagen">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn FilterControlAltText="Filter Completo column"
                                        UniqueName="AttachFile">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="AttachImg" runat="server" ImageUrl="~/Imagenes/iconos/photo16x16.png"
                                                ToolTip="Descargar">
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Width="5px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Descripcion_Imagen"
                                        FilterControlAltText="Filter column column"
                                        SortExpression=" Descripcion_Imagen" UniqueName="column">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <asp:SqlDataSource ID="SqlDataSource8" runat="server"
                            ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                            SelectCommand="SELECT * FROM tme_FichaProyectoImagen where id_ficha_proyecto = @id">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="id" QueryStringField="id" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>



