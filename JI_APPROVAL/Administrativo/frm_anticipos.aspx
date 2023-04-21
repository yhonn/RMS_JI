<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_anticipos.aspx.vb" Inherits="RMS_APPROVAL.frm_anticipos" %>
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
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Administración de anticipos</asp:Label>
                    </h3>                       
                </div>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-10">
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="Registrar anticipo">
                        </telerik:RadButton>
                    </div>
                    <div class="col-sm-2">
                        <%--<a id="A1" runat="server" href="~/reportes/xls?id=5" class="btn btn-primary btn-sm pull-right margin-r-5"><i class="fa fa-download"></i> Descargar datos</a>--%>
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

                   <div class="col-lg-12">
                       <div style="max-width:100%; overflow-x:auto">
                           <asp:HiddenField runat="server" ID="h_Filter" Value="" />  

                <%--          <telerik:RadGrid RenderMode="Lightweight" AutoGenerateColumns="false" ID="grd_cate"  PageSize="15"
                                AllowFilteringByColumn="True" AllowSorting="True" Culture = "French (France)" width="100%" 
                                ShowFooter="True" AllowPaging="True" runat="server">
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

                                         <telerik:GridTemplateColumn  UniqueName="colm_cambiar_estado" Visible="false" HeaderText="F1" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_estado" runat="server" ImageUrl="../Imagenes/iconos/Informacion2.png" ToolTip="Pendiente"/>
                                            </ItemTemplate>
                                           <HeaderStyle Width="30px" />
                                           <ItemStyle Width="30px" />                                           
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn  UniqueName="colm_estado_accion" Visible="false" HeaderText="F1" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:Image ID="col_img_estado" runat="server" ToolTip="Pendiente" ImageUrl="../Imagenes/iconos/Informacion2.png" Style="border-width: 0px;" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="30px" />
                                           <ItemStyle Width="30px" />                                           
                                        </telerik:GridTemplateColumn>


                                         <telerik:GridTemplateColumn UniqueName="colm_fecha_fact" Visible="false" HeaderText="F2"  AllowFiltering="false">
                                            <HeaderStyle Width="30" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="col_hlk_edit" runat="server" Width="10"
                                                    ImageUrl="../Imagenes/iconos/dollar.png" ToolTip="Fecha pago / contabilización"
                                                    OnClick="Editar_fp_Click">
                                                    <asp:Image ID="img_editar" runat="server" ImageUrl="../Imagenes/iconos/dollar.png" Style="border-width: 0px;" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                             <HeaderStyle Width="30px" />
                                           <ItemStyle Width="30px" />      
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_rechazar" Visible="false" HeaderText="Rechazar" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_reversar" runat="server" ImageUrl="../Imagenes/iconos/s_warn.png" ToolTip="Rechazar radicado"/>
                                            </ItemTemplate>
                                           <HeaderStyle Width="30px" />
                                           <ItemStyle Width="30px" />                                           
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn  UniqueName="colm_anular" Visible="false" HeaderText="Anular" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_anular" runat="server" ImageUrl="../Imagenes/iconos/arrow_left.png" ToolTip="Anular radicado"/>
                                            </ItemTemplate>
                                           <HeaderStyle Width="30px" />
                                           <ItemStyle Width="30px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_actualizar" Visible="false" HeaderText="" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="col_hlk_actualizar" runat="server" Width="10"
                                                    ImageUrl="../Imagenes/iconos/refresh.png" ToolTip="Actualizar GJ"
                                                    OnClick="Editar_gj_click">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/refresh.png" Style="border-width: 0px;" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                           <HeaderStyle Width="30px" />
                                           <ItemStyle Width="30px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="codigo"
                                            FilterControlAltText="Filter codigo column"
                                            HeaderText="# radicado" SortExpression="codigo"
                                            UniqueName="colm_codigo" >
                                            <HeaderStyle Width="120px" />                                            
                                            <ItemStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="codigo_GJ"
                                            FilterControlAltText="Filter codigo_GJ column"
                                            HeaderText="Código GJ" SortExpression="codigo_GJ"
                                            UniqueName="colm_codigo_gj" >
                                            <HeaderStyle Width="120px" />                                            
                                            <ItemStyle Width="120px" />
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
                                            <HeaderStyle Width="250px" />                                            
                                            <ItemStyle Width="250px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_responsable"
                                            FilterControlAltText="Filter nombre_responsable column"
                                            HeaderText="Responsable" SortExpression="nombre_responsable"
                                            UniqueName="colm_responsable" >
                                            <HeaderStyle Width="250px" />                                            
                                            <ItemStyle Width="250px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="identificacion_tercero_a_pagar"
                                            FilterControlAltText="Filter identificacion_tercero_a_pagar column" visible="false"
                                            HeaderText="# identificación" SortExpression="identificacion_tercero_a_pagar"
                                            UniqueName="colm_identificacion_tercero_a_pagar" >
                                            <HeaderStyle Width="140px" />                                            
                                            <ItemStyle Width="140px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="tercero_a_pagar"
                                            FilterControlAltText="Filter tercero_a_pagar column"
                                            HeaderText="Nombre tercero" SortExpression="tercero_a_pagar"
                                            UniqueName="colm_tercero_a_pagar" >
                                            <HeaderStyle Width="170px" />                                            
                                            <ItemStyle Width="170px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="detalle"
                                            FilterControlAltText="Filter detalle column"
                                            HeaderText="DOCUMENTO <small>(Factura, Cta Cobro, PTR, etc.)</small>" SortExpression="detalle"
                                            UniqueName="colm_detalle" >
                                            <HeaderStyle Width="250px" />                                            
                                            <ItemStyle Width="250px" />
                                        </telerik:GridBoundColumn>
                                        
                                        
                                        <telerik:GridNumericColumn HeaderStyle-Width="110px" FilterControlWidth="110px" DataField="valor_factura"
                                             ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            DataType="System.Decimal" HeaderText="Valor" Aggregate="Sum" DataFormatString="{0:n}">
                                            <FooterStyle Font-Bold="true"></FooterStyle>
                                            <HeaderStyle Width="170px" />                                            
                                            <ItemStyle Width="170px" />
                                        </telerik:GridNumericColumn>
                                        <telerik:GridBoundColumn DataField="caracter"
                                            FilterControlAltText="Filter caracter column"
                                            HeaderText="Caracter" SortExpression="caracter"
                                            UniqueName="colm_caracter" >
                                            <HeaderStyle Width="120px" />                                            
                                            <ItemStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="tipo_documento"
                                            FilterControlAltText="Filter tipo_documento column"
                                            HeaderText="Tipo de documento" SortExpression="tipo_documento"
                                            UniqueName="colm_tipo_documento" >
                                            <HeaderStyle Width="200px" />                                            
                                            <ItemStyle Width="200px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="estado_radicado"
                                            FilterControlAltText="Filter estado_radicado column"
                                            HeaderText="Estado" SortExpression="estado_radicado"
                                            UniqueName="colm_estado" >
                                            <HeaderStyle Width="120px" />                                            
                                            <ItemStyle Width="120px" />
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
                             
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>--%>
                        </div>

                    </div>

                      
            </div>
            <asp:HiddenField runat="server" ID="identity" Value="0" />
            

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