<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_sgtoEntregables.aspx.vb" Inherits="RMS_APPROVAL.frm_sgtoEntregables" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla_">Subactividad - Aprobación de Hitos</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla_1">Cadena de aprobación</asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <telerik:RadGrid ID="grd_ruta" runat="server" AllowAutomaticDeletes="True" DataSourceID="SqlDts_comentarios"
                            AllowSorting="True" AutoGenerateColumns="False">
                            <ClientSettings EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="True"></Selecting>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_ruta_hito" DataSourceID="SqlDts_comentarios" AllowAutomaticUpdates="True">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id_ruta_hito"
                                        DataType="System.Int32"
                                        FilterControlAltText="Filter id_ruta_hito column"
                                        HeaderText="id_ruta_hito" ReadOnly="True"
                                        SortExpression="id_ruta_hito" UniqueName="id_ruta_hito"
                                        Visible="False">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="responsable" FilterControlAltText="Filter responsable column"
                                        HeaderText="Área encargada" SortExpression="responsable" UniqueName="responsable" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="usuario_aprueba" FilterControlAltText="Filter usuario_aprueba column"
                                        HeaderText="Usuario responsable" SortExpression="usuario_aprueba" UniqueName="usuario_aprueba" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="fecha_envio" FilterControlAltText="Filter fecha_envio column" DataFormatString="{0:M/d/yyyy}"
                                        HeaderText="Fecha" SortExpression="fecha_envio" UniqueName="fecha_envio" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="estado_visible" FilterControlAltText="Filter estado_visible column" 
                                        HeaderText="Estado" SortExpression="estado_visible" UniqueName="estado_visible" Visible="true">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <asp:SqlDataSource ID="SqlDts_comentarios" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                            SelectCommand="">
                                        </asp:SqlDataSource>
                        <hr />
                       <div class ="row">
                            <div class="col-sm-12 text-left ">
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:HiddenField ID="id_hito" runat="server" />
                                        <asp:HiddenField ID="id_ruta_hito" runat="server" />
                                         <asp:HiddenField ID="orden" runat="server" />
                                        <asp:HiddenField ID="tiene_ruta" runat="server" />
                                        <asp:HiddenField ID="id_rol" runat="server" />
                                        <asp:HiddenField ID="ultimo_entregable" value="0" runat="server" />
                                        <asp:Label ID="lblt_cod_Acti" runat="server"  CssClass="control-label text-bold" Text="Código de la actividad"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                            <asp:Label ID="lbl_cod_acti" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_actividad" runat="server"  CssClass="control-label text-bold" Text="Nombre de la actividad"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_actividad" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_fecha_inicio" runat="server"  CssClass="control-label text-bold" Text="Fecha de inicio actividad"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_inicio" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_fecha_fin" runat="server"  CssClass="control-label text-bold" Text="Fecha de finalización actividad"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_fin" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_ejecutor" runat="server"  CssClass="control-label text-bold" Text="Nombre ejecutor"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_ejecutor" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold" Text="Nombre del hito"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_nom_producto" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="Label4" runat="server"  CssClass="control-label text-bold" Text="Número del hito"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_num_producto" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="Label3" runat="server"  CssClass="control-label text-bold" Text="Fecha esperada de entrega"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_entrega" runat="server" ></asp:Label>
                                    </div>
                                </div>

                                <div class="row" id="doc_admon_content" runat="server" visible="false">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="Label7" runat="server"  CssClass="control-label text-bold" Text="Documentos administrativos para el pago"></asp:Label>
                                    </div>
                                    <div class="col-sm-5">
                                        <!--Control -->
                                        <asp:HyperLink id="docs_admon" 
                                          NavigateUrl="#"
                                          Target="_blank"
                                          Text="Ver documentos"
                                          runat="server"/> 
                                    </div>
                                </div>
                            </div> 
                        </div>
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />
                        <div class="box-body">
                        <div class="row">
                            <div class="col-sm-12 text-right">
                                <hr />
                                <h4 class="text-center"><asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Comentarios</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                            <div class="col-sm-12 text-left" style="overflow-x:scroll;">
                                 <telerik:RadGrid ID="grd_coment" runat="server" AllowAutomaticDeletes="True" DataSourceID="SqlDts_comentarios2"
                                        AllowSorting="True" AutoGenerateColumns="False">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_ruta_hito" DataSourceID="SqlDts_comentarios2" AllowAutomaticUpdates="True">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_ruta_hito"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter id_ruta_hito column"
                                                    HeaderText="id_ruta_hito" ReadOnly="True"
                                                    SortExpression="id_ruta_hito" UniqueName="id_ruta_hito"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn ItemStyle-Width="13%" DataField="fecha_envio_text" FilterControlAltText="Filter fecha_envio_text column" DataFormatString="{0:M/d/yyyy}"
                                                    HeaderText="Fecha Comentario" SortExpression="fecha_envio_text" UniqueName="fecha_envio_text" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn ItemStyle-Width="17%" DataField="usuario_aprueba" FilterControlAltText="Filter usuario_aprueba column"
                                                    HeaderText="Usuario" SortExpression="usuario_aprueba" UniqueName="usuario_aprueba" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn ItemStyle-Width="67%" DataField="comentarios" FilterControlAltText="Filter comentarios column"
                                                    HeaderText="Comentarios" SortExpression="comentarios" UniqueName="comentarios" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="soporte" FilterControlAltText="Filter soporte column"
                                                    HeaderText="Soporte" SortExpression="soporte" UniqueName="soporte" Visible="true">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                    <asp:SqlDataSource ID="SqlDts_comentarios2" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                            SelectCommand="">
                                        </asp:SqlDataSource>
                                <hr />
                            </div>
                        </div>
                    </div>
                        <hr />
                        <div class="row">
                            <div class="col-sm-12 text-right">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Entregables</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                        </div>
                        
                        <div class="panel-body">
                            <div class="form-group row">
                                <%--<div class="col-sm-2 text-right">
                                </div>--%>
                                <div class="col-sm-12">
                                    <telerik:RadGrid ID="grd_aportes" runat="server" AllowAutomaticDeletes="True" DataSourceID="SqlDts_entregables"
                                        AllowSorting="True" AutoGenerateColumns="False">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_entregable_hito" DataSourceID="SqlDts_entregables" AllowAutomaticUpdates="True">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_entregable_hito"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter id_entregable_hito column"
                                                    HeaderText="id_entregable_hito" ReadOnly="True"
                                                    SortExpression="id_entregable_hito" UniqueName="id_entregable_hito"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>
                                               
                                                <telerik:GridBoundColumn DataField="nro_entregable" FilterControlAltText="Filter nro_entregable column" ItemStyle-Width="10"
                                                    HeaderText="Número" SortExpression="nro_entregable" UniqueName="nro_entregable" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="descripcion_entregable" FilterControlAltText="Filter descripcion_entregable column"
                                                    HeaderText="Entregable" SortExpression="descripcion_entregable" UniqueName="colm_sub_producto" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="unidad_medidad" FilterControlAltText="Filter unidad_medidad column"
                                                    HeaderText="Unidad de medida" SortExpression="unidad_medidad" UniqueName="unidad_medidad" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="cantidad" FilterControlAltText="Filter cantidad column"
                                                    HeaderText="Cantidad" SortExpression="cantidad" UniqueName="cantidad" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="observaciones_entregable" FilterControlAltText="Filter observaciones_entregable column"
                                                    HeaderText="Observaciones" SortExpression="observaciones_entregable" UniqueName="colm_observaciones_entregable" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="soporte" FilterControlAltText="Filter soporte column"
                                                    HeaderText="Soporte" SortExpression="soporte" UniqueName="colm_sub_producto" Visible="false">
                                                </telerik:GridBoundColumn>
                                                <%--<telerik:GridBoundColumn DataField="soporte_externo" FilterControlAltText="Filter soporte_externo column"
                                                    HeaderText="Comentarios" SortExpression="soporte_externo" UniqueName="soporte_externo" Visible="false">
                                                </telerik:GridBoundColumn>--%>
                                                <%--<telerik:GridBoundColumn DataField="soporte_inicio" FilterControlAltText="Filter soporte_inicio column"
                                                    HeaderText="Comentarios" SortExpression="soporte_inicio" UniqueName="soporte_inicio" Visible="false">
                                                </telerik:GridBoundColumn>--%>
                                                
                                                <telerik:GridTemplateColumn UniqueName="col_hlk_view">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlk_view" runat="server" ImageUrl="~/Imagenes/iconos/adjunto.png"
                                                            ToolTip="Soporte" Target="_new">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10px" />
                                                </telerik:GridTemplateColumn>  
                                                <telerik:GridTemplateColumn UniqueName="colm_productos" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlk_productos" Visible="false" runat="server" ImageUrl="~/Imagenes/iconos/observaciones.png"
                                                            ToolTip="Registrar entregable">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10px" />
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                    <asp:SqlDataSource ID="SqlDts_entregables" runat="server"
                                            ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                                            SelectCommand="">
                                        </asp:SqlDataSource>
                                </div>
                            </div>
                        </div>


                        <hr />
                        <div class="row" visible="false" id="soporteURL2" runat="server">
                            <div class="col-sm-12 text-right">
                                <h4 class="text-center"><asp:Label runat="server" ID="Label6" CssClass="control-label text-bold">Documentos administrativos</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                            <div class="col-sm-4" >
                                <asp:Label ID="Label8" runat="server"  CssClass="control-label text-bold">Ingrese la URL de los documentos administrativos *</asp:Label>
                                <br />
                                <telerik:RadTextBox ID="txt_url_docs_admin" runat="server" Width="90%"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rv_docs_admin" runat="server"
                                    ControlToValidate="txt_url_docs_admin" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre el nombre de la UPM" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        
                    </div>


                    <div runat="server" id="addComentario" >
                        <div class="box-body">
                            
                        </div>
                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-4 text-left">
                                    <!--Tittle -->
                                    <asp:Label ID="lblt_writcommen" runat="server"  CssClass="control-label text-bold">Comentarios</asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <!--Control -->
                                </div>
                                <div class="col-sm-12">
                                        <telerik:RadTextBox ID="txtcoments" Runat="server" Height="100px"  TextMode="MultiLine" Width="100%">
                                        </telerik:RadTextBox>                                                 
                                    <br />

                                    </div>
                            </div>
                        </div>
                        
                        <div class="box-body">
                            <div class="row text-right" runat="server" id="accioEncar">
                                <div class="col-sm-4 text-center  ">                                           
                                    <!--Buttoms -->
                                    <asp:Button ID="btn_aprueba" runat="server" OnClick="btn_aprueba_pro_Click" Text="Aprobar" OnClientClick="  this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"  CssClass="btn-lg btn-primary" Width="65%" />
                                </div>
                                <div class="col-sm-4 text-center  ">                                           
                                    <!--Buttoms -->
                                    <asp:Button ID="btn_ajustes" runat="server" OnClick="btn_ajustes_Click" Text="Solicitar ajustes"   OnClientClick="  this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"  CssClass="btn-lg btn-warning" Width="65%" />
                                </div>
                                <div class="col-sm-4 text-center  ">                                           
                                    <!--Buttoms -->
                                    <asp:Button ID="btn_no_aprobar" Visible="false" runat="server" Text="No Aprobar" OnClientClick="this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"  CssClass="btn-lg btn-danger" Width="65%" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                    </div>
                </div>
                <!-- /.box-footer -->
                <div class="modal fade bs-example-modal-sm" id="modalConfirm" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog modal-sm vertical-align-center">
                            <div class="modal-content">
                                <div class="modal-header modal-danger">
                                    <h4 class="modal-title" runat="server" id="esp_ctrl_h4_eliminar_titulo">Eliminar Registro</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="esp_ctrl_lbl_eliminar" runat="server" Text="Desea eliminar el registro?" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btn_eliminarAportes" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" data-dismiss="modal" UseSubmitBehavior="false" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
            </div>
        </div>
    </section>
    <script src="../Scripts/FileUploadTelerik.js"></script>
    
</asp:Content>

