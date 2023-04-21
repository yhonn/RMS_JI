<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_entregableSgmt.aspx.vb" Inherits="RMS_APPROVAL.frm_entregableSgmt" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla_">Contratos</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla_1">Seguimiento entregable</asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <asp:HiddenField runat="server" ID="idEntregable" Value="0" />
                        <telerik:RadGrid ID="grd_ruta" runat="server" AllowAutomaticDeletes="True"
                            AllowSorting="True" AutoGenerateColumns="False">
                            <ClientSettings EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="True"></Selecting>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_ruta_aprobacion_entregable" >
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id_ruta_aprobacion_entregable"
                                        DataType="System.Int32"
                                        FilterControlAltText="Filter id_ruta_aprobacion_entregable column"
                                        HeaderText="id_ruta_aprobacion_entregable" ReadOnly="True"
                                        SortExpression="id_ruta_aprobacion_entregable" UniqueName="id_ruta_aprobacion_entregable"
                                        Visible="False">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="responsable" FilterControlAltText="Filter responsable column"
                                        HeaderText="Responsable" SortExpression="responsable" UniqueName="responsable" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="usuario" FilterControlAltText="Filter usuario column"
                                        HeaderText="Usuario responsable" SortExpression="usuario" UniqueName="usuario" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="fecha_envio" FilterControlAltText="Filter fecha_envio column"
                                        HeaderText="Fecha" SortExpression="fecha_envio" UniqueName="fecha_envio" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="estado" FilterControlAltText="Filter estado column" 
                                        HeaderText="Estado" SortExpression="estado" UniqueName="estado" Visible="true">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <hr />
                       <div class ="row">
                            <div class="col-sm-12 text-left ">
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:HiddenField ID="id_hito" runat="server" />
                                        <asp:HiddenField ID="id_ruta_hito" runat="server" />
                                        <asp:Label ID="lblt_cod_Acti" runat="server"  CssClass="control-label text-bold" Text="Código del contrato"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                            <asp:Label ID="lbl_cod_contrato" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_actividad" runat="server"  CssClass="control-label text-bold" Text="Contratista"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_contratista" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_fecha_inicio" runat="server"  CssClass="control-label text-bold" Text="Número de entregable"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_nro_entregable" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_fecha_fin" runat="server"  CssClass="control-label text-bold" Text="Fecha esperada de entrega"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_entrega" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="lblt_ejecutor" runat="server"  CssClass="control-label text-bold" Text="Valor del entregable"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_valor" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold" Text="Nombre del entregable"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_nom_entregable" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="Label4" runat="server"  CssClass="control-label text-bold" Text="Productos"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:Label ID="lbl_num_producto" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4 text-left">
                                        <asp:Label ID="Label1" runat="server"  CssClass="control-label text-bold" Text="Soporte"></asp:Label>
                                    </div>
                                    <div class="col-sm-8">
                                        <!--Control -->
                                        <asp:HyperLink id="docs_admon" 
                                          NavigateUrl="#"
                                          Target="_blank"
                                          Text="Ver soporte"
                                          runat="server"/> 
                                    </div>
                                </div>
                            </div> 
                        </div>
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />
                        <div class="box-body">
                        <div class="row" runat="server" id="comentarios" >
                            <div class="col-sm-12 text-right">
                                <hr />
                                <h4 class="text-center"><asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Comentarios</asp:Label></h4>
                                <hr class="box box-primary" />
                            </div>
                            <div class="col-sm-12 text-left" style="overflow-x:scroll;">
                                 <telerik:RadGrid ID="grd_coment" runat="server" AllowAutomaticDeletes="True"
                                        AllowSorting="True" AutoGenerateColumns="False">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_ruta_aprobacion_entregable" AllowAutomaticUpdates="True">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_ruta_aprobacion_entregable"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter id_ruta_aprobacion_entregable column"
                                                    HeaderText="id_ruta_aprobacion_entregable" ReadOnly="True"
                                                    SortExpression="id_ruta_aprobacion_entregable" UniqueName="id_ruta_aprobacion_entregable"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn ItemStyle-Width="13%" DataField="fecha_envio" FilterControlAltText="Filter fecha_envio column" DataFormatString="{0:M/d/yyyy}"
                                                    HeaderText="Fecha Comentario" SortExpression="fecha_envio" UniqueName="fecha_envio" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn ItemStyle-Width="17%" DataField="usuario" FilterControlAltText="Filter usuario column"
                                                    HeaderText="Usuario" SortExpression="usuario" UniqueName="usuario" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn ItemStyle-Width="67%" DataField="comentarios" FilterControlAltText="Filter comentarios column"
                                                    HeaderText="Comentarios" SortExpression="comentarios" UniqueName="comentarios" Visible="true">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                   
                                <hr />
                            </div>
                        </div>
                    </div>

                        <hr />
                        <div class="row" visible="false" id="soporteURL" runat="server">
                            <div class="col-sm-4" >
                                <asp:Label ID="Label8" runat="server"  CssClass="control-label text-bold">Ingrese la URL del soporte *</asp:Label>
                                <br />
                                <telerik:RadTextBox ID="txt_url_docs_admin" runat="server" Width="90%"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="rv_docs_admin" runat="server"
                                    ControlToValidate="txt_url_docs_admin" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre el nombre de la UPM" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>

                         <div runat="server" id="addComentario" >
                          
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
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txtcoments" CssClass="Error" Display="Dynamic"
                                    ErrorMessage="Registre el nombre de la UPM" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    <br />

                                    </div>
                            </div>
                        
                            <div class="box-body">
                                <div class="row text-right" runat="server" id="accioEncar">
                                    <div class="col-sm-4 text-center  ">                                           
                                        <!--Buttoms -->
                                        <asp:Button ID="btn_aprueba" ValidationGroup="1" runat="server" OnClick="btn_aprueba_pro_Click" Text="Aprobar" OnClientClick="  this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"  CssClass="btn-lg btn-primary" Width="65%" />
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

