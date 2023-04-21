﻿<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_proyectos.aspx.vb" Inherits="RMS_APPROVAL.frm_proyectos" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Ficha de Actividades</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <div class="col-sm-6">   
                            <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Actividades</asp:Label>
                            </h3>                       
                        </div>
                         <div class="col-sm-6 text-right">   
                            <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('registro_actividades.mp4');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                        </div>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_region" CssClass="control-label text-bold">Región</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_region" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                        <asp:CheckBox ID="chk_TodosR" runat="server" AutoPostBack="True" Font-Bold="True" Text="Seleccionar todos" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_subregion" CssClass="control-label text-bold">Sub Región</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_subregion" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                        <asp:CheckBox ID="chk_Todos" runat="server" AutoPostBack="True" Font-Bold="True" Text="Seleccionar todos" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_mecanism" CssClass="control-label text-bold">Mecánismo de contratación</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_mecanism" runat="server" Width="300px" AutoPostBack="true">
                        </telerik:RadComboBox>                        
                        <asp:CheckBox ID="chk_allMecanism" runat="server" AutoPostBack="True" Font-Bold="True" Text="Seleccionar todos" />
                    </div>
                </div>
                
                  <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_submecanism" CssClass="control-label text-bold">Sub-Mecánismo</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_Sub_mecanism" runat="server" Width="300px" AutoPostBack="true">
                        </telerik:RadComboBox>                        
                        <asp:CheckBox ID="chk_allSubmecanims" runat="server" AutoPostBack="True" Font-Bold="True" Text="Seleccionar todos" />
                    </div>
                </div>

                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadTextBox ID="txt_doc" runat="server" EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="300px" ValidationGroup="1">
                        </telerik:RadTextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_nuevo0" Visible="false" runat="server" AutoPostBack="true" Enabled="false" Text="Cuadro de mando"></telerik:RadButton>
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="Nuevo Proyecto">
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
                                                
                <div style="max-width:100%; overflow-x:auto">
                            <telerik:RadGrid ID="grd_cate" 
                                             runat="server" 
                                             AllowAutomaticDeletes="True"
                                             AllowPaging="True" 
                                             AllowSorting="True" 
                                             PageSize="25" 
                                             AutoGenerateColumns="False" Width="100%">
                                <ClientSettings EnableRowHoverStyle="true">
                                   <Selecting AllowRowSelect="True"></Selecting>                                  
                                   <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true" AllowResizeToFit="true" />
                                   <ClientEvents OnColumnResizing="ColumnResizing" />
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_ficha_proyecto"
                                    AllowAutomaticUpdates="True">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_ficha_proyecto"
                                            FilterControlAltText="Filter id_ficha_proyecto column"
                                            SortExpression="id_ficha_proyecto" UniqueName="id_ficha_proyecto"
                                            Visible="False" DataType="System.Int32" HeaderText="id_ficha_proyecto"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="false">
                                            
                                            <ItemTemplate>
                                                <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10" ToolTip="Eliminar">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="25px" />
                                            <ItemStyle Width="25px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_Edit" Visible="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar proyecto" Target="_self" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridBoundColumn DataField="nombre_mecanismo_contratacion"
                                            FilterControlAltText="Filter nombre_mecanismo_contratacion column" HeaderText="Mecánismo" UniqueName="colm_nombre_mecanismo_contratacion">
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="perfijo_sub_mecanismo"
                                            FilterControlAltText="Filter perfijo_sub_mecanismo column" HeaderText="TP" UniqueName="colm_prefijo_sub_mecanismo">
                                            <HeaderStyle Width="35px" />
                                            <ItemStyle Width="35px" />
                                        </telerik:GridBoundColumn>

                                         <telerik:GridTemplateColumn 
                                              HeaderText="&nbsp;" UniqueName="colm_codes" >
                                             <ItemTemplate>                                     
                                                 <%--<asp:Label runat="server" ID="lblt_rms_code" CssClass="control-label text-bold" Text="Código RMS: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_rms_code" CssClass="control-label" Text=""></asp:Label><br />--%>
                                                 <asp:Label runat="server" ID="lblt_internal_code" CssClass="control-label text-bold" Text="Código: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_internal_code" CssClass="control-label" Text=""></asp:Label><br />
                                                 <asp:Label runat="server" ID="lblt_monitor_code" CssClass="control-label text-bold" Text="Código Monitor: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_monitor_code" CssClass="control-label" Text=""></asp:Label>
                                                 <br /><br />
                                             </ItemTemplate>
                                              <HeaderStyle Width="150px" />
                                              <ItemStyle Width="150px" />
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="codigo_SAPME"
                                            FilterControlAltText="Filter codigo_SAPME column" HeaderText="Código RMS" UniqueName="colm_codigo_ficha_SAPME" Visible="true" Display="false">
                                            
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="codigo_RFA"
                                            FilterControlAltText="Filter codigo_RFA column" HeaderText="Código Revisión" UniqueName="colm_codigo_ficha_AID" Visible="true" Display="false">
                                            
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="codigo_MONITOR"
                                            FilterControlAltText="Filter codigo_MONITOR column" HeaderText="Código Monitor" UniqueName="colm_codigo_MONITOR" Visible="true" Display="false">
                                            
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_proyecto"
                                            FilterControlAltText="Filter nombre_proyecto column"
                                            HeaderText="Proyecto" SortExpression="nombre_proyecto"
                                            UniqueName="colm_nombre_proyecto" >
                                        <HeaderStyle Width="250px" />                                            
                                        <ItemStyle Width="250px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_ejecutor" 
                                            FilterControlAltText="Filter nombre_ejecutor column" HeaderText="Ejecutor"
                                            SortExpression="nombre_ejecutor"
                                            UniqueName="colm_nombre_ejecutor">
                                         <HeaderStyle Width="150px" />                                            
                                        <ItemStyle Width="150px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="c_Aportes_Proyecto" DataFormatString="{0:N0}"
                                            FilterControlAltText="Filter c_Aportes_Proyecto column"
                                            HeaderText="Costo Actividad" SortExpression="c_Aportes_Proyecto"
                                            UniqueName="colm_costo_total_proyecto">
                                             <HeaderStyle Width="100px" />                                            
                                             <ItemStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="c_Aportes_ProyectoUSD" DataFormatString="{0:N0}"
                                            FilterControlAltText="Filter c_Aportes_ProyectoUSD column"
                                            HeaderText="Costo Actividad (USD)" SortExpression="c_Aportes_Proyecto"
                                            UniqueName="colm_costo_total_proyectoUSD">
                                             <HeaderStyle Width="100px" />                                            
                                             <ItemStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="fecha_inicio_proyecto"  DataFormatString="{0:dd/MM/yyyy}"
                                            FilterControlAltText="Filter fecha_inicio_proyecto column"
                                            HeaderText="Inicio" SortExpression="fecha_inicio_proyecto"
                                            UniqueName="colm_fecha_inicio_proyecto">
                                             <HeaderStyle Width="80px" />                                            
                                             <ItemStyle Width="80px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fecha_fin_proyecto"  DataFormatString="{0:dd/MM/yyyy}"
                                            FilterControlAltText="Filter fecha_fin_proyecto column"
                                            HeaderText="Fin" SortExpression="fecha_fin_proyecto"
                                            UniqueName="colm_fecha_fin_proyecto">
                                            <HeaderStyle Width="80px" />                                            
                                            <ItemStyle Width="80px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="nombre_estado_ficha"
                                            FilterControlAltText="Filter nombre_estado_ficha column"
                                            HeaderText="Estado" SortExpression="nombre_estado_ficha"
                                            UniqueName="colm_nombre_estado_ficha">
                                            <HeaderStyle Width="80px" />                                            
                                            <ItemStyle Width="80px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="GeoreferenciaCMP" HeaderText="GIS" Visible="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_GeoreferenciaCMP" runat="server" ImageUrl="~/Imagenes/iconos/accept.png" ToolTip="Georeferencia completa" />
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                                            <HeaderStyle Width="30px" />                                                                                         
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="OrdenInicioCMP" HeaderText="OI" Visible="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_OrdenInicioCMP" runat="server" ImageUrl="~/Imagenes/iconos/accept.png" ToolTip="Ejecución" />
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                                            <HeaderStyle Width="30px" />                                                                                         
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Completo" Visible="false">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="col_hlk_Completo" runat="server" ImageUrl="~/Imagenes/iconos/alerta.png"
                                                        ToolTip="Activity Complete">
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                            <HeaderStyle Width="30px" />                                            
                                             <ItemStyle Width="30px" />
                                            </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="filtro Hitos column" HeaderText="Hitos" UniqueName="colm_hitos">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_hito" runat="server" ImageUrl="~/Imagenes/iconos/additem.png" ToolTip="Hitos" />
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" />
                                            <HeaderStyle Width="30px" />                                                                                         
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="filtro Modificaciones column" UniqueName="colm_mod" visible="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_mod" runat="server" ImageUrl="~/Imagenes/iconos/refresh.png" ToolTip="Modificaciones" />
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" />
                                            <HeaderStyle Width="30px" />                                                                                         
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="print" visible="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_Print" runat="server" ImageUrl="~/Imagenes/iconos/Informacion1.png" ToolTip="Imprimir ficha" Target="_blank" />
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" />
                                            <HeaderStyle Width="30px" />                                                                                         
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="c_Entregables" Visible="false"></telerik:GridBoundColumn>
                                        <%--<telerik:GridBoundColumn DataField="c_Aportes" Visible="false"></telerik:GridBoundColumn>--%>
                                        <telerik:GridBoundColumn DataField="c_Objectives" Visible="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="c_Key_issues" Visible="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="c_villages" Visible="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="c_support_services" Visible="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="c_indicadores" Visible="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="c_documentos" Visible="false"></telerik:GridBoundColumn>

                                    </Columns>                                    
                                </MasterTableView>

                                <FilterMenu EnableImageSprites="False">
                                    <WebServiceSettings>
                                        <ODataSettings InitialContainerName=""></ODataSettings>
                                    </WebServiceSettings>
                                </FilterMenu>
                            </telerik:RadGrid>
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
    <!-- /.content -->
</asp:Content>
