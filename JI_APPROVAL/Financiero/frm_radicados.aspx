<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_radicados.aspx.vb" Inherits="RMS_APPROVAL.frm_radicados" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <section class="content-header">
         <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Financiero</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <div class="col-sm-6">   
                    <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Administración de radicados</asp:Label>
                    </h3>                       
                </div>
            </div>
             <asp:HiddenField runat="server" ID="h_Filter_fecha" Value="" />  
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-10">
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="Registrar radicado">
                        </telerik:RadButton>
                    </div>
                    
                </div>
                <hr />
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
                   
                   .RadGrid .rgDataDiv
                    {
                        height : auto !important;
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
                <div class="box-body">
                    <ul class="nav nav-tabs">
                        <li class="active"><a data-toggle="tab" href="#listadoRadicados">
                            <asp:Label runat="server" ID="lblt_listado_radicados" CssClass="control-label text-bold">Listado de radicados</asp:Label></a></li>
                        <li><a data-toggle="tab" href="#generarReporte">
                            <asp:Label runat="server" ID="lblt_reporte" CssClass="control-label text-bold">Generar reporte</asp:Label></a></li>
                    </ul>
                <div class="tab-content  div-bordered">
                    <div id="listadoRadicados" class="tab-pane fade active in">
                        <div class="col-12">
                             <div class="row">
                                                <div class="col-sm-12">
                                                    <hr />
                                                </div>
                                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div style="max-width:100%; overflow-x:auto">
                           <asp:HiddenField runat="server" ID="h_Filter" Value="" />  

                          <telerik:RadGrid RenderMode="Lightweight" AutoGenerateColumns="false" ID="grd_cate"  PageSize="15"
                                AllowFilteringByColumn="True" AllowSorting="True" Culture = "French (France)" width="100%" 
                                ShowFooter="True" AllowPaging="True" runat="server">
                                <ClientSettings EnableRowHoverStyle="true">
                                        <Selecting AllowRowSelect="True"></Selecting>                                  
                                        <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                    </ClientSettings>
                              <MasterTableView TableLayout="Fixed"></MasterTableView>
                                <HeaderStyle Width="200px" />
                                <PagerStyle Mode="NextPrevAndNumeric" />
                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True"  DataKeyNames="id_radicado">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_radicado"
                                            FilterControlAltText="Filter id_facturacion column"
                                            SortExpression="id_radicado" UniqueName="id_radicado"
                                            Visible="False" DataType="System.Int32" HeaderText="id_radicado"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                       <%-- <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="false" AllowFiltering="false">
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />      
                                            <ItemTemplate>
                                                <asp:LinkButton ID="col_hlk_eliminar" runat="server" 
                                                    ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                    OnClick="delete_detalle">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn  UniqueName="colm_Edit" Visible="false" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar proyecto" Target="_self" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>--%>

                                         <telerik:GridTemplateColumn  UniqueName="colm_cambiar_estado" Visible="false" HeaderText="F1" HeaderStyle-Width="35px" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_estado" runat="server" ImageUrl="../Imagenes/iconos/Informacion2.png" ToolTip="Pendiente"/>
                                            </ItemTemplate>
                                           <HeaderStyle Width="30px" />
                                           <ItemStyle Width="30px" />                                           
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn  UniqueName="colm_estado_accion" Visible="false" HeaderText="F1" HeaderStyle-Width="35px" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:Image ID="col_img_estado" runat="server" ToolTip="Pendiente" ImageUrl="../Imagenes/iconos/Informacion2.png" Style="border-width: 0px;" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="30px" />
                                           <ItemStyle Width="30px" />                                           
                                        </telerik:GridTemplateColumn>


                                         <telerik:GridTemplateColumn UniqueName="colm_fecha_fact" Visible="false" HeaderText="F2"  HeaderStyle-Width="35px" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="col_hlk_edit" runat="server" Width="10"
                                                    ImageUrl="../Imagenes/iconos/dollar.png" ToolTip="Fecha pago / contabilización"
                                                    OnClick="Editar_fp_Click">
                                                    <asp:Image ID="img_editar" runat="server" ImageUrl="../Imagenes/iconos/dollar.png" Style="border-width: 0px;" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                             <HeaderStyle Width="35px" />
                                           <ItemStyle Width="35px" />      
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_rechazar" Visible="false" HeaderText="Rechazar" HeaderStyle-Width="60px" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_reversar" runat="server" ImageUrl="../Imagenes/iconos/s_warn.png" ToolTip="Rechazar radicado"/>
                                            </ItemTemplate>
                                           <HeaderStyle Width="60px" />
                                           <ItemStyle Width="60px" />                                           
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn  UniqueName="colm_anular" Visible="false" HeaderText="Anular" HeaderStyle-Width="35px" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_anular" runat="server" ImageUrl="../Imagenes/iconos/arrow_left.png" ToolTip="Anular radicado"/>
                                            </ItemTemplate>
                                           <HeaderStyle Width="35px" />
                                           <ItemStyle Width="35px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_actualizar" Visible="false" HeaderText="" HeaderStyle-Width="87px" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="col_hlk_actualizar" runat="server" Width="10"
                                                    ImageUrl="../Imagenes/iconos/refresh.png" ToolTip="Actualizar GJ"
                                                    OnClick="Editar_gj_click">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/refresh.png" Style="border-width: 0px;" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                           <HeaderStyle Width="87px" />
                                           <ItemStyle Width="87px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="codigo" FilterControlWidth="77px" 
                                            FilterControlAltText="Filter codigo column"
                                            HeaderText="# radicado" SortExpression="codigo"
                                            UniqueName="colm_codigo" >
                                            <HeaderStyle Width="117px" /> 
                                            <ItemStyle Width="117px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="codigo_GJ"
                                            FilterControlAltText="Filter codigo_GJ column"
                                            HeaderText="Código GJ" SortExpression="codigo_GJ"
                                            UniqueName="colm_codigo_gj" >
                                            <HeaderStyle Width="117px" />                                            
                                            <ItemStyle Width="117px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridDateTimeColumn DataField="fecha_radicado" HeaderText="Fecha" FilterControlWidth="120px"
                                            SortExpression="fecha_radicado" PickerType="DatePicker" EnableTimeIndependentFiltering="true"
                                            DataFormatString="{0:MM/dd/yyyy hh:mm}">
                                            <HeaderStyle Width="125px" />                                            
                                            <ItemStyle Width="125px" />
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridDateTimeColumn DataField="fecha_pago_contabilizacion" HeaderText="Fecha de pago" FilterControlWidth="120px"
                                            SortExpression="fecha_pago_contabilizacion" PickerType="DatePicker" EnableTimeIndependentFiltering="true"
                                            DataFormatString="{0:MM/dd/yyyy}">
                                            <HeaderStyle Width="125px" />                                            
                                            <ItemStyle Width="125px" />
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridBoundColumn DataField="nombre_subregion"
                                            FilterControlAltText="Filter nombre_subregion column"
                                            HeaderText="Regional" SortExpression="nombre_subregion"
                                            UniqueName="colm_nombre_subregion" >
                                            <HeaderStyle Width="150px" />                                            
                                            <ItemStyle Width="150px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_responsable"
                                            FilterControlAltText="Filter nombre_responsable column"
                                            HeaderText="Responsable" SortExpression="nombre_responsable"
                                            UniqueName="colm_responsable" >
                                            <HeaderStyle Width="130px" />                                            
                                            <ItemStyle Width="130px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="identificacion_tercero_a_pagar"
                                            FilterControlAltText="Filter identificacion_tercero_a_pagar column" visible="false"
                                            HeaderText="# identificación" SortExpression="identificacion_tercero_a_pagar"
                                            UniqueName="colm_identificacion_tercero_a_pagar" >
                                            <HeaderStyle Width="130px" />                                            
                                            <ItemStyle Width="130px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="tercero_a_pagar"
                                            FilterControlAltText="Filter tercero_a_pagar column"
                                            HeaderText="Nombre tercero" SortExpression="tercero_a_pagar"
                                            UniqueName="colm_tercero_a_pagar" >
                                            <HeaderStyle Width="130px" />                                            
                                            <ItemStyle Width="130px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="detalle"
                                            FilterControlAltText="Filter detalle column"
                                            HeaderText="DOCUMENTO <small>(Factura, Cta Cobro, PTR, etc.)</small>" SortExpression="detalle"
                                            UniqueName="colm_detalle" >
                                            <HeaderStyle Width="133px" />                                            
                                            <ItemStyle Width="133px" />
                                        </telerik:GridBoundColumn>
                                        
                                        
                                        <telerik:GridNumericColumn HeaderStyle-Width="130px" FilterControlWidth="70px" DataField="valor_factura"
                                             ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            DataType="System.Decimal" HeaderText="Valor" Aggregate="Sum" DataFormatString="{0:n}">
                                            <FooterStyle Font-Bold="true"></FooterStyle>
                                            <HeaderStyle Width="108px" />                                            
                                            <ItemStyle Width="108px" />
                                        </telerik:GridNumericColumn>
                                        <telerik:GridBoundColumn DataField="caracter"
                                            FilterControlAltText="Filter caracter column"
                                            HeaderText="Caracter" SortExpression="caracter"
                                            UniqueName="colm_caracter" >
                                            <HeaderStyle Width="125px" />                                            
                                            <ItemStyle Width="125px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="tipo_documento"
                                            FilterControlAltText="Filter tipo_documento column"
                                            HeaderText="Tipo de documento" SortExpression="tipo_documento"
                                            UniqueName="colm_tipo_documento" >
                                            <HeaderStyle Width="150px" />                                            
                                            <ItemStyle Width="150px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="estado_radicado"
                                            FilterControlAltText="Filter estado_radicado column"
                                            HeaderText="Estado" SortExpression="estado_radicado"
                                            UniqueName="colm_estado" >
                                            <HeaderStyle Width="130px" />                                            
                                            <ItemStyle Width="130px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fuera_tiempo_text" visible="false"
                                            FilterControlAltText="Filter fuera_tiempo_text column"
                                            HeaderText="Fuera de tiempo" SortExpression="fuera_tiempo_text"
                                            UniqueName="colm_fuera_tiempo" >
                                            <HeaderStyle Width="120px" />                                            
                                            <ItemStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="id_estado"  FilterControlAltText="Filter id_estado column"  SortExpression="id_estado" UniqueName="id_estado" Visible="true" Display="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="id_usuario_crea"  FilterControlAltText="Filter id_usuario_crea column"  SortExpression="id_estado" UniqueName="id_usuario_crea" Visible="true" Display="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="id_usuario_aprobo"  FilterControlAltText="Filter id_usuario_aprobo column"  SortExpression="id_usuario_aprobo" UniqueName="id_usuario_aprobo" Visible="true" Display="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="radicado_pago_contabilizacion"  FilterControlAltText="Filter radicado_pago_contabilizacion column"  SortExpression="radicado_pago_contabilizacion" UniqueName="radicado_pago_contabilizacion" Visible="true" Display="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn  DataField="anulado"  FilterControlAltText="Filter anulado column"  SortExpression="anulado" UniqueName="anulado" Visible="true" Display="false"></telerik:GridBoundColumn>
                                       
                                      <%--<telerik:GridTemplateColumn  UniqueName="colm_detalle_3" visible="false" HeaderText="F3" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_detalle" runat="server" ImageUrl="../Imagenes/iconos/printer_off.png" ToolTip="Ver detalle"/>
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>--%>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>
                                </div>
                            </div>

                        </div>

                    </div>

                    <div id="generarReporte" class="tab-pane fade">
                        <div class="col-12">
                             <div class="row">
                                <div class="col-sm-12">
                                    <hr />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                            <div class="form-group row">
                                <div class="col-sm-6 col-md-6 col-lg-4">
                                    <asp:Label runat="server" ID="lbl_tipo_actividad" CssClass="control-label text-bold">Fecha de radicados desde</asp:Label>
                                    <br />
                                    <telerik:RadDatePicker ID="dt_fecha_desde" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                    </telerik:RadDatePicker>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-4">
                                    <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Fecha de radicados hasta</asp:Label>
                                    <br />
                                    <telerik:RadDatePicker ID="dt_fecha_hasta" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                    </telerik:RadDatePicker>
                                </div>
                                  
                                <div class="col-sm-6 col-md-6 col-lg-4">
                                    <div style=" margin-top: 15px;">
                                    <telerik:RadButton ID="btn_generar_reporte" runat="server" AutoPostBack="true" Text="Generar reporte" validationgroup="1" Width="200px"></telerik:RadButton>

                                    </div>
                                </div>
                         </div>
</div>

                        </div>

                        </div>
                       
                    </div>
                </div>
                </div>
                
                   

                      
            </div>
            <asp:HiddenField runat="server" ID="identity" Value="0" />
            <div class="modal fade bs-example-modal-sm" id="EditTC" data-backdrop="static" data-keyboard="false">
                <div class="vertical-alignment-helper">
                    <div class="modal-dialog modal-sm vertical-align-center">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color:#307095; color:#ffffff;">
                                <h4 class="modal-title" runat="server" id="H1">Fecha contabilización / pago</h4>
                            </div>
                            <div class="modal-body">
                                <telerik:RadDatePicker ID="dt_fecha_pago" AutoPostBack="false" Width="90%" Enabled="true" runat="server">
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                ControlToValidate="dt_fecha_pago" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="modal-footer">
                                <telerik:RadButton ID="btn_fecha_pago" runat="server"  ValidationGroup="3"  CssClass="btn btn-sm btn-primary btn-ok" AutoPostBack="true" Enabled="false" Text="Guardar">
                                </telerik:RadButton>
                                <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button2">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade bs-example-modal-sm" id="EditGJ" data-backdrop="static" data-keyboard="false">
                <div class="vertical-alignment-helper">
                    <div class="modal-dialog modal-sm vertical-align-center">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color:#307095; color:#ffffff;">
                                <h4 class="modal-title" runat="server" id="H2">Código GJ</h4>
                            </div>
                            <div class="modal-body">
                                 <asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Código GJ</asp:Label>
                                <br />
                                <telerik:RadTextBox CssClass="upper" ID="txt_codigo_gj" runat="server" Width="98%"  MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                ControlToValidate="txt_codigo_gj" CssClass="Error" Display="Dynamic"
                                ErrorMessage="Campo obligatorio" ValidationGroup="4">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="modal-footer">
                                <telerik:RadButton ID="btn_codigo_gj" runat="server"  ValidationGroup="4"  CssClass="btn btn-sm btn-primary btn-ok" AutoPostBack="true" Enabled="false" Text="Guardar">
                                </telerik:RadButton>
                                <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button1">Cancelar</button>
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



        function loadEditModal() {
            $('#EditTC').modal('show');
        }
        function hideDeleteModal() {
            $('#EditTC').modal('hide');
        }
        function loadGJModal() {
            $('#EditGJ').modal('show');
        }
            </script>
</asp:Content>