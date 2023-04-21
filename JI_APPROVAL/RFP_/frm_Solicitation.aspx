﻿<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_Solicitation.aspx.vb" Inherits="RMS_APPROVAL.frm_Solicitation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
         <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Activity Management</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <div class="col-sm-6">   
                            <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Activities</asp:Label>
                            </h3>                       
                        </div>
                         <div class="col-sm-6 text-right">   
                            <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
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
        <%--        <div class="form-group row">
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
                </div>--%>

                 <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_solicitation_type" CssClass="control-label text-bold">Sub-Mecánismo</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_solicitation_type" runat="server" Width="300px" AutoPostBack="true">
                        </telerik:RadComboBox>                        
                        <asp:CheckBox ID="chk_allsolTYPES" runat="server" AutoPostBack="True" Font-Bold="True" Text="Seleccionar todos" />
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
                        <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Search" Width="100px">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_nuevo0" Visible="false" runat="server" AutoPostBack="true" Enabled="false" Text="Cuadro de mando"></telerik:RadButton>
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="New Solicitation">
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

                   <div class="col-lg-12" style="width:100%; margin: 0 auto; margin-top:10px;">
                       <div style="max-width:100%; overflow-x:auto">
                       
                                                
                            <telerik:RadGrid ID="grd_cate" 
                                             runat="server" 
                                             AllowAutomaticDeletes="True"
                                             AllowPaging="True" 
                                             AllowSorting="True"                                       
                                             PageSize="15" 
                                             AutoGenerateColumns="False"
                                            AllowFilteringByColumn="true"
                                            Width="100%" 
                                            Height="1000px">
                                <ClientSettings EnableRowHoverStyle="true">
                                   <Selecting AllowRowSelect="True"></Selecting>                                                                      
                                    <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                </ClientSettings>
                                 <GroupingSettings CaseSensitive="false" />
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_activity"
                                    AllowAutomaticUpdates="True"  ShowFooter="true"   >
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_activity"
                                            FilterControlAltText="Filter id_activity column"
                                            SortExpression="id_activity" UniqueName="id_activity"
                                            Visible="False" DataType="System.Int32" HeaderText="id_activity"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="ID_ACTIVITY_SOLICITATION"                                            
                                            SortExpression="ID_ACTIVITY_SOLICITATION" UniqueName="ID_ACTIVITY_SOLICITATION"
                                            Visible="False" DataType="System.Int32" HeaderText="ID_ACTIVITY_SOLICITATION"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                       
                                     
                                        <telerik:GridTemplateColumn  UniqueName="colm_Edit" Visible="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar proyecto" Target="_self" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>

                                         <telerik:GridBoundColumn DataField="nombre_region"
                                            FilterControlAltText="Filter nombre_region column" HeaderText="Cluster" UniqueName="colm_nombre_region">
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle Width="100px" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="SOLICITATION_TYPE"
                                            FilterControlAltText="Filter SOLICITATION_TYPE column" HeaderText="TP" UniqueName="colm_SOLICITATION_TYPE">
                                            <HeaderStyle Width="135px" />
                                            <ItemStyle Width="135px" />
                                        </telerik:GridBoundColumn>

                                         <%--<telerik:GridTemplateColumn 
                                              HeaderText="&nbsp;" UniqueName="colm_codes" >
                                             <ItemTemplate>                                     
                                                 <asp:Label runat="server" ID="lblt_rms_code" CssClass="control-label text-bold" Text="RMS Code: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_rms_code" CssClass="control-label" Text=""></asp:Label><br />
                                                 <asp:Label runat="server" ID="lblt_internal_code" CssClass="control-label text-bold" Text="Technical: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_internal_code" CssClass="control-label" Text=""></asp:Label><br />
                                                 <asp:Label runat="server" ID="lblt_monitor_code" CssClass="control-label text-bold" Text="Monitoring: "></asp:Label><br />&nbsp;<asp:Label runat="server" ID="lbl_monitor_code" CssClass="control-label" Text=""></asp:Label>
                                                 <br /><br />
                                             </ItemTemplate>
                                              <HeaderStyle Width="150px" />
                                              <ItemStyle Width="150px" />
                                        </telerik:GridTemplateColumn>--%>

                                        <telerik:GridBoundColumn DataField="SOLICITATION_CODE"
                                            FilterControlAltText="Filter SOLICITATION_CODE column" HeaderText="Solicitation Code" UniqueName="colm_SOLICITATION_CODE" Visible="true" Display="true">                                            
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle Width="100px" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="SOLICITATION_TITLE"
                                            FilterControlAltText="Filter SOLICITATION_TITLE column"
                                            HeaderText="TITLE" SortExpression="SOLICITATION_TITLE"
                                            UniqueName="colm_SOLICITATION_TITLE" >
                                        <HeaderStyle Width="150px" />                                            
                                        <ItemStyle Width="150px" />
                                        </telerik:GridBoundColumn>                                                                                

                                         <%--<telerik:GridBoundColumn DataField="ORGANIZATIONNAME"
                                            FilterControlAltText="Filter ORGANIZATIONNAME column"
                                            HeaderText="Grantee/Subcontractor" SortExpression="ORGANIZATIONNAME"
                                            UniqueName="colm_ORGANIZATIONNAME" >
                                        <HeaderStyle Width="150px" />                                            
                                        <ItemStyle Width="150px" />
                                        </telerik:GridBoundColumn>
                                 
                                        <telerik:GridBoundColumn DataField="OBLIGATED_AMOUNT_LOCAL" DataFormatString="{0:N0}"
                                            FilterControlAltText="Filter OBLIGATED_AMOUNT_LOCAL column"
                                            HeaderText="Amount" SortExpression="OBLIGATED_AMOUNT_LOCAL"
                                            UniqueName="colm_OBLIGATED_AMOUNT_LOCAL" Aggregate="Sum">
                                             <HeaderStyle Width="100px" />                                            
                                             <ItemStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="OBLIGATED_AMOUNT" DataFormatString="{0:N0}"
                                            FilterControlAltText="Filter OBLIGATED_AMOUNT column"
                                            HeaderText="Amount (USD)" SortExpression="OBLIGATED_AMOUNT"
                                            UniqueName="colm_OBLIGATED_AMOUNT" Aggregate="Sum" >
                                             <HeaderStyle Width="100px" />                                            
                                             <ItemStyle Width="100px" />
                                        </telerik:GridBoundColumn>--%>

                                         <telerik:GridBoundColumn DataField="start_date"  DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                            FilterControlAltText="Filter start_date column"
                                            HeaderText="Start Date" SortExpression="start_date"
                                            UniqueName="colm_start_date">
                                             <HeaderStyle Width="100px" />                                            
                                             <ItemStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="end_date"  DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                            FilterControlAltText="Filter end_date column"
                                            HeaderText="End Date" SortExpression="end_date"
                                            UniqueName="colm_end_date">
                                            <HeaderStyle Width="100px" />                                            
                                            <ItemStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="SOLICITATION_STATUS"
                                            FilterControlAltText="Filter SOLICITATION_STATUS column"
                                            HeaderText="STATUS" SortExpression="SOLICITATION_STATUS"
                                            UniqueName="colm_SOLICITATION_STATUS">
                                            <HeaderStyle Width="100px" />                                            
                                            <ItemStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                     <%--   <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="GeoreferenciaCMP" HeaderText="GIS" Visible="true">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_GeoreferenciaCMP" runat="server" ImageUrl="~/Imagenes/iconos/accept.png" ToolTip="Georeferencia completa" />
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                                            <HeaderStyle Width="30px" />                                                                                         
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="OrdenInicioCMP" HeaderText="OI" Visible="true">
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
                                        <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="print">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_Print" runat="server" ImageUrl="~/Imagenes/iconos/Informacion1.png" ToolTip="Imprimir ficha" Target="_blank" />
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" />
                                            <HeaderStyle Width="30px" />                                                                                         
                                        </telerik:GridTemplateColumn>
                                         --%>                                      
                                    </Columns>                                    
                                </MasterTableView>

                            </telerik:RadGrid>

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
    <!-- /.content -->
</asp:Content>

