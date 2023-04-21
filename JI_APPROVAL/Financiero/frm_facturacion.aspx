<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_facturacion.aspx.vb" Inherits="RMS_APPROVAL.frm_facturacion" %>

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
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Administración de facturas</asp:Label>
                    </h3>                       
                </div>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-10">
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="Registrar factura">
                        </telerik:RadButton>
                    </div>
                    <div class="col-sm-2">
                        <a id="A1" runat="server" href="~/reportes/xls?id=3" class="btn btn-primary btn-sm pull-right margin-r-5"><i class="fa fa-download"></i> Descargar datos</a>
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
                           <asp:HiddenField runat="server" ID="h_Filter" Value="" />  

                          <telerik:RadGrid RenderMode="Lightweight" AutoGenerateColumns="false" ID="grd_cate"
                                AllowFilteringByColumn="True" AllowSorting="True" Width="100%" Culture = "French (France)"
                                ShowFooter="True" AllowPaging="True" runat="server">
                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True"  DataKeyNames="id_facturacion">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_facturacion"
                                            FilterControlAltText="Filter id_facturacion column"
                                            SortExpression="id_facturacion" UniqueName="id_facturacion"
                                            Visible="False" DataType="System.Int32" HeaderText="id_facturacion"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="false" AllowFiltering="false">
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
                                        </telerik:GridTemplateColumn>

                                        
                                        <telerik:GridBoundColumn DataField="numero_factura"
                                            FilterControlAltText="Filter numero_factura column"
                                            HeaderText="# Factura" SortExpression="numero_factura"
                                            UniqueName="colm_numero_factura" >
                                            <HeaderStyle Width="150px" />                                            
                                            <ItemStyle Width="150px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridDateTimeColumn DataField="fecha_factura" HeaderText="Fecha" FilterControlWidth="90px"
                                            SortExpression="fecha_factura" PickerType="DatePicker" EnableTimeIndependentFiltering="true"
                                            DataFormatString="{0:MM/dd/yyyy}">
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridBoundColumn DataField="nombre_departamento"
                                            FilterControlAltText="Filter nombre_departamento column"
                                            HeaderText="Departamento" SortExpression="nombre_departamento"
                                            UniqueName="colm_nombre_departamento" >
                                            <HeaderStyle Width="250px" />                                            
                                            <ItemStyle Width="250px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_municipio"
                                            FilterControlAltText="Filter nombre_municipio column"
                                            HeaderText="Municipio" SortExpression="nombre_municipio"
                                            UniqueName="colm_nombre_municipio" >
                                            <HeaderStyle Width="250px" />                                            
                                            <ItemStyle Width="250px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="documento_identificacion"
                                            FilterControlAltText="Filter documento_identificacion column"
                                            HeaderText="Documento de identidad / NIT" SortExpression="documento_identificacion"
                                            UniqueName="colm_documento_identificacion" >
                                            <HeaderStyle Width="250px" />                                            
                                            <ItemStyle Width="250px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre"
                                            FilterControlAltText="Filter nombre column"
                                            HeaderText="Nombre" SortExpression="nombre"
                                            UniqueName="colm_nombre" >
                                            <HeaderStyle Width="250px" />                                            
                                            <ItemStyle Width="250px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="direccion"
                                            FilterControlAltText="Filter direccion column"
                                            HeaderText="Dirección" SortExpression="direccion"
                                            UniqueName="colm_direccion" >
                                            <HeaderStyle Width="250px" />                                            
                                            <ItemStyle Width="250px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="correo"
                                            FilterControlAltText="Filter correo column"
                                            HeaderText="Correo" SortExpression="correo"
                                            UniqueName="colm_correo" >
                                            <HeaderStyle Width="250px" />                                            
                                            <ItemStyle Width="250px" />
                                        </telerik:GridBoundColumn>
                                        
                                        <telerik:GridNumericColumn HeaderStyle-Width="110px" FilterControlWidth="110px" DataField="valor_total"
                                             ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            DataType="System.Decimal" HeaderText="Valor" Aggregate="Sum" DataFormatString="{0:n}">
                                            <FooterStyle Font-Bold="true"></FooterStyle>
                                        </telerik:GridNumericColumn>
                                        
                                        <telerik:GridTemplateColumn  UniqueName="colm_xls" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_download" runat="server" ImageUrl="../Imagenes/iconos/xlsx.png" ToolTip="Descargar" Target="_blank" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_detalle" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_detalle" runat="server" ImageUrl="../Imagenes/iconos/printer_off.png" ToolTip="Ver detalle" Target="_blank" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="anulada" Visible="false"></telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
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
</asp:Content>