<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_viajes.aspx.vb" Inherits="RMS_APPROVAL.frm_viajes" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
         <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administrativo</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <div class="col-sm-6">   
                    <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Administración de viajes</asp:Label>
                    </h3>                       
                </div>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-8">
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="Registrar viaje">
                        </telerik:RadButton>
                    </div>
                    <div class="col-sm-2">
                        <a id="A1" runat="server" href="~/reportes/xls?id=1" class="btn btn-primary btn-sm pull-right margin-r-5"><i class="fa fa-download"></i> Descargar datos</a>
                    </div>
                     <div class="col-sm-2 text-right">   
                        <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('viajes.mp4');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                    </div>
                </div>
                <hr />
                <div class="form-group row">
                    <div class="col-sm-2 text-left">
                        <!--Tittle -->
                            <asp:Label ID="lblt_groupBy" runat="server" CssClass="control-label text-bold"  Text="Filtrar por"></asp:Label> 
                    </div>
                    <div class="col-sm-8">
                        <asp:HiddenField runat="server" ID="idviaje" Value="0" />
                        <asp:RadioButton ID="RadioButton1" runat="server" AutoPostBack="True" GroupName="GrdPending"  Text="   Pending action by "  CssClass="labelRadiobutton"  /> <br />
                        <asp:RadioButton ID="RadioButton3" runat="server" AutoPostBack="True" GroupName="GrdPending" Text= "   Pending approvals that {0} participates" CssClass="labelRadiobutton" /><br />
                        <asp:RadioButton ID="RadioButton4" runat="server" AutoPostBack="True" GroupName="GrdPending" Text="   View all pending approvals"  CssClass="labelRadiobutton"   />
                    </div>
                        <asp:RadioButton ID="RadioButton2" runat="server" AutoPostBack="True" GroupName="GrdPending" Text="   Pending approvals initiated by " CssClass="labelRadiobutton" visible="false" /><br />
                </div>
                <asp:Label ID="lbltotal" runat="server" Font-Bold="True" Font-Size="12px"></asp:Label>

                <style type="text/css">
                    .rgHeaderDiv
                    {
                        height: 100% !important;
                    }
                    .rgDataDiv .rgMasterTable 
                    {
                        height: 100% !important;
                    }


		           .GridDataDiv_Default 
                            { 
                                overflow-y: hidden !important; 
                            } 

                </style>
              
                <script type="text/javascript">

               <%--  function pageLoad() {

                      //alert('Page Load');

                        var grid = $find("<%= grd_cate.ClientID %>");
                        var columns = grid.get_masterTableView().get_columns();

                     for (var i = 0; i < columns.length; i++) {

                             if (
                                 (columns[i].get_uniqueName() != 'colm_nombre_proyecto') &&
                                 (columns[i].get_uniqueName() != 'colm_nombre_ejecutor') &&
                                 (columns[i].get_uniqueName() != 'colm_codes') 
                             ) {
                                 //alert(columns[i].get_uniqueName());
                                 columns[i].resizeToFit(false, true);
                             }

                         }
                  }--%>

                    function ColumnResizing(sender, args)
                    {

                        //alert('Column Resizing');
                        //alert(args.get_gridColumn().get_element().cellIndex + ", width: ");
                        //alert(args.get_gridColumn().get_uniqueName() + ' ColumnIndex : ' + args.get_gridColumn().get_element().cellIndex + ", width: " + args.get_gridColumn().get_element().offsetWidth);
                        
                        var newWidth = args.get_gridColumn().get_element().offsetWidth;

                        //alert(newWidth);                        
                            if (newWidth > 200)
                            {
                                //alert('Cancel ' + args.get_gridColumn().get_element().cellIndex + ", width: " + args.get_gridColumn().get_element().offsetWidth); 
                                args.set_cancel(true);
                            }

                    }

                    
     
                 </script>

                   <div class="col-lg-12" style="width:100%; margin: 0 auto; margin-top:10px;">
                       <div style="max-width:100%; overflow-x:auto">
                           

                          <telerik:RadGrid RenderMode="Lightweight" AutoGenerateColumns="false" ID="grd_cate"
                                AllowFilteringByColumn="True" AllowSorting="True" Width="100%"  PageSize="15" 
                                ShowFooter="True" AllowPaging="True" runat="server">
                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True"  DataKeyNames="id_viaje">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_viaje"
                                            FilterControlAltText="Filter id_viaje column"
                                            SortExpression="id_viaje" UniqueName="id_viaje"
                                            Visible="False" DataType="System.Int32" HeaderText="id_viaje"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="id_estadoDoc"
                                            FilterControlAltText="Filter id_estadoDoc column"
                                            SortExpression="id_estadoDoc" UniqueName="id_estadoDoc"
                                            Visible="False" DataType="System.Int32" HeaderText="id_estadoDoc"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="id_documento"
                                            FilterControlAltText="Filter id_documento column"
                                            SortExpression="id_documento" UniqueName="id_documento"
                                            Visible="False" DataType="System.Int32" HeaderText="id_documento"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="id_usuario"
                                            FilterControlAltText="Filter id_usuario column"
                                            SortExpression="id_usuario" UniqueName="id_usuario"
                                            Visible="False" DataType="System.Int32" HeaderText="id_usuario"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="id_supervisor"
                                            FilterControlAltText="Filter id_supervisor column"
                                            SortExpression="id_usuario" UniqueName="id_supervisor"
                                            Visible="False" DataType="System.Int32" HeaderText="id_supervisor"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="id_estadoDoc_legalizacion"
                                            FilterControlAltText="Filter id_estadoDoc_legalizacion column"
                                            SortExpression="id_estadoDoc_legalizacion" UniqueName="id_estadoDoc_legalizacion"
                                            Visible="False" DataType="System.Int32" HeaderText="id_estadoDoc_legalizacion"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="id_documento_legalizacion"
                                            FilterControlAltText="Filter id_documento_legalizacion column"
                                            SortExpression="id_documento" UniqueName="id_documento_legalizacion"
                                            Visible="False" DataType="System.Int32" HeaderText="id_documento_legalizacion"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="id_estadoDoc_informe"
                                            FilterControlAltText="Filter id_estadoDoc_informe column"
                                            SortExpression="id_estadoDoc_informe" UniqueName="id_estadoDoc_informe"
                                            Visible="False" DataType="System.Int32" HeaderText="id_estadoDoc_informe"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_Edit" Visible="false" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar viaje" Target="_self" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="codigo_solicitud_viaje"
                                            FilterControlAltText="Filter codigo_solicitud_viaje column"
                                            HeaderText="Código" SortExpression="codigo_solicitud_viaje"
                                            UniqueName="colm_codigo_solicitud_viaje" >
                                            <HeaderStyle Width="50px" />                                            
                                            <ItemStyle Width="50px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_usuario"
                                            FilterControlAltText="Filter nombre_usuario column"
                                            HeaderText="Usuario" SortExpression="nombre_usuario"
                                            UniqueName="colm_nombre_usuario" >
                                            <HeaderStyle Width="200px" />                                            
                                            <ItemStyle Width="200px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="codigo_usuario"
                                            FilterControlAltText="Filter codigo_usuario column"
                                            HeaderText="Código usuario" SortExpression="codigo_usuario"
                                            UniqueName="colm_codigo_usuario" >
                                            <HeaderStyle Width="200px" />                                            
                                            <ItemStyle Width="200px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="numero_documento"
                                            FilterControlAltText="Filter numero_documento column"
                                            HeaderText="# Documento" SortExpression="numero_documento"
                                            UniqueName="colm_numero_documento" >
                                            <HeaderStyle Width="200px" />                                            
                                            <ItemStyle Width="200px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="tipo_viaje"
                                            FilterControlAltText="Filter tipo_viaje column"
                                            HeaderText="Tipo de viaje" SortExpression="tipo_viaje"
                                            UniqueName="colm_tipo_viaje" >
                                            <HeaderStyle Width="200px" />                                            
                                            <ItemStyle Width="200px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridDateTimeColumn DataField="fecha_crea" HeaderText="Fecha Solicitud" FilterControlWidth="110px"
                                            SortExpression="fecha_crea" PickerType="DatePicker" EnableTimeIndependentFiltering="true" FilterDateFormat="dd/MM/yyyy"
                                            DataFormatString="{0:dd/MM/yyyy}">
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridDateTimeColumn DataField="fecha_inicio_viaje" HeaderText="Fecha de inicio" FilterControlWidth="110px"
                                            SortExpression="fecha_inicio_viaje" PickerType="DatePicker" EnableTimeIndependentFiltering="true"
                                            DataFormatString="{0:dd/MM/yyyy}"  FilterDateFormat="dd/MM/yyyy">
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridDateTimeColumn DataField="fecha_fin_viaje" HeaderText="Fecha fin" FilterControlWidth="110px"
                                            SortExpression="fecha_fin_viaje" PickerType="DatePicker" EnableTimeIndependentFiltering="true"
                                            DataFormatString="{0:dd/MM/yyyy}" FilterDateFormat="dd/MM/yyyy">
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridBoundColumn DataField="motivo_viaje"
                                            FilterControlAltText="Filter motivo_viaje column" visible="false"
                                            HeaderText="Motivo" SortExpression="motivo_viaje"
                                            UniqueName="colm_motivo_viaje" >
                                            <HeaderStyle Width="350px" />                                            
                                            <ItemStyle Width="350px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_seguimiento" Visible="false" HeaderText="F1" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_seguimiento" runat="server" ImageUrl="../Imagenes/iconos/Informacion2.png" ToolTip="Pendiente de envío por aprobación" Target="_self" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_legalizazion_seguimiento" Visible="false" HeaderText="F2" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_seguimiento_legalizacion" runat="server" ImageUrl="../Imagenes/iconos/Informacion2.png" ToolTip="Pendiente de legalización" Target="_self" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_informe" Visible="false" HeaderText="F3" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_informe" runat="server" ImageUrl="../Imagenes/iconos/Informacion2.png" ToolTip="Informe" Target="_self" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_facturacion" Visible="false" HeaderText="F4" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_facturacion" runat="server" ImageUrl="../Imagenes/iconos/note_edit.png" ToolTip="Facturación" Target="_self" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn  DataField="editar_solicitud"  FilterControlAltText="Filter editar_solicitud column"  SortExpression="editar_solicitud" UniqueName="editar_solicitud" Visible="true" Display="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="editar_legalizacion"  FilterControlAltText="Filter editar_legalizacion column"  SortExpression="editar_legalizacion" UniqueName="editar_legalizacion" Visible="true" Display="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="editar_informe"  FilterControlAltText="Filter editar_informe column"  SortExpression="editar_informe" UniqueName="editar_informe" Visible="true" Display="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn  DataField="habilitar_facturacion"  FilterControlAltText="Filter habilitar_facturacion column"  SortExpression="habilitar_facturacion" UniqueName="habilitar_facturacion" Visible="true" Display="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn  DataField="id_usuario_app"  FilterControlAltText="Filter id_usuario_app column"  SortExpression="id_usuario_app" UniqueName="id_usuario_app" Visible="true" Display="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="id_usuario_app_legalizacion"  FilterControlAltText="Filter id_usuario_app_legalizacion column"  SortExpression="id_usuario_app_legalizacion" UniqueName="id_usuario_app_legalizacion" Visible="true" Display="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="requiere_servicio_aereo"  FilterControlAltText="Filter requiere_servicio_aereo column"  SortExpression="requiere_servicio_aereo" UniqueName="requiere_servicio_aereo" Visible="true" Display="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="id_usuario_app_informe"  FilterControlAltText="Filter id_usuario_app_informe column"  SortExpression="id_usuario_app_informe" UniqueName="id_usuario_app_informe" Visible="true" Display="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="id_usuario_radica"  FilterControlAltText="Filter id_usuario_radica column"  SortExpression="id_usuario_radica" UniqueName="id_usuario_radica" Visible="true" Display="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="numero_reserva"  FilterControlAltText="Filter numero_reserva column"  SortExpression="numero_reserva" UniqueName="numero_reserva" Visible="true" Display="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="id_usuario_radico"  FilterControlAltText="Filter id_usuario_radico column"  SortExpression="id_usuario_radico" UniqueName="id_usuario_radico" Visible="true" Display="false"></telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_anular" Visible="false" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_revertir" runat="server" ImageUrl="../Imagenes/iconos/arrow_left.png" ToolTip="Anular viaje" Target="_self" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn  UniqueName="colm_cod_reserva" Visible="false" HeaderText="CR" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="col_hlk_codigo_Reserva" runat="server" Width="10"
                                                    ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Código de reserva"
                                                    OnClick="Editar_codigo_reserva_Click">
                                                    <asp:Image ID="img_codigo_reserva" runat="server" ToolTip="Código de reserva" ImageUrl="../Imagenes/iconos/Informacion2.png" Style="border-width: 0px;" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="filtro Permisos column" UniqueName="colm_permisos" visible="false" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_mod" runat="server" ImageUrl="~/Imagenes/iconos/refresh.png" ToolTip="Permisos" />
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" />
                                            <HeaderStyle Width="30px" />                                                                                         
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_detalle" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_detalle" runat="server" ImageUrl="../Imagenes/iconos/printer_off.png" ToolTip="Ver detalle" Target="_blank" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="anulado" Visible="false"></telerik:GridBoundColumn>
                                        
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>

                    </div>

                      
            </div>
            <div class="modal fade bs-example-modal-sm" id="addCR" data-backdrop="static" data-keyboard="false">
                <div class="vertical-alignment-helper">
                    <div class="modal-dialog modal-sm vertical-align-center">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color:#307095; color:#ffffff;">
                                <h4 class="modal-title" runat="server" id="H1">Código de reserva</h4>
                            </div>
                            <div class="modal-body">
                                <telerik:RadTextBox ID="txt_Codigo_reserva" Width="90%" runat="server" MinValue="0">
                                </telerik:RadTextBox>
                            </div>
                            <div class="modal-footer">
                                <telerik:RadButton ID="btn_edit2" runat="server" CssClass="btn btn-sm btn-primary btn-ok" AutoPostBack="true" Enabled="true" Text="Guardar">
                                </telerik:RadButton>
                                <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button2">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
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
                                <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" />
                                <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
               

    </section>
    <script type="text/javascript">
        function loadCodigoReservaModal() {
            $('#addCR').modal('show');
        }
        function hideCodigoReservaModal() {
            $('#addCR').modal('hide');
        }
            </script>
</asp:Content>