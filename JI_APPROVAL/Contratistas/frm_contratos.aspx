<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_contratos.aspx.vb" Inherits="RMS_APPROVAL.frm_contratos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
         <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Contratistas</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <div class="col-sm-6">   
                    <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Contratos</asp:Label>
                    </h3>                       
                </div>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-8">
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="Registrar contrato">
                        </telerik:RadButton>
                    </div>
                    <%--<div class="col-sm-2">
                        <a id="A1" runat="server" href="~/reportes/xls?id=1" class="btn btn-primary btn-sm pull-right margin-r-5"><i class="fa fa-download"></i> Descargar datos</a>
                    </div>
                     <div class="col-sm-2 text-right">   
                        <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('viajes.mp4');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                    </div>--%>
                </div>
                <hr />
                <%--<div class="form-group row">
                    <div class="col-sm-2 text-left">
                        <!--Tittle -->
                            <asp:Label ID="lblt_groupBy" runat="server" CssClass="control-label text-bold"  Text="Filtrar por"></asp:Label> 
                    </div>
                    <div class="col-sm-8">
                        <asp:RadioButton ID="RadioButton1" runat="server" AutoPostBack="True" GroupName="GrdPending"  Text="   Pending action by "  CssClass="labelRadiobutton"  /> <br />
                        <asp:RadioButton ID="RadioButton3" runat="server" AutoPostBack="True" GroupName="GrdPending" Text= "   Pending approvals that {0} participates" CssClass="labelRadiobutton" /><br />
                        <asp:RadioButton ID="RadioButton4" runat="server" AutoPostBack="True" GroupName="GrdPending" Text="   View all pending approvals"  CssClass="labelRadiobutton"   />
                    </div>
                        <asp:RadioButton ID="RadioButton2" runat="server" AutoPostBack="True" GroupName="GrdPending" Text="   Pending approvals initiated by " CssClass="labelRadiobutton" visible="false" /><br />
                </div>--%>
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
                                ShowFooter="false" AllowPaging="True" runat="server">
                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True"  DataKeyNames="id_contrato">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id_contrato"
                                            FilterControlAltText="Filter id_contrato column"
                                            SortExpression="id_contrato" UniqueName="id_contrato"
                                            Visible="False" DataType="System.Int32" HeaderText="id_contrato"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="id_contratista"
                                            FilterControlAltText="Filter id_contratista column"
                                            SortExpression="id_contratista" UniqueName="id_contratista"
                                            Visible="False" DataType="System.Int32" HeaderText="id_contratista"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="id_supervisor"
                                            FilterControlAltText="Filter id_supervisor column"
                                            SortExpression="id_usuario" UniqueName="id_supervisor"
                                            Visible="False" DataType="System.Int32" HeaderText="id_supervisor"
                                            ReadOnly="True">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn  UniqueName="colm_Edit" Visible="false" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar viaje" Target="_self" />
                                            </ItemTemplate>
                                           <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />                                           
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="numero_contrato"
                                            FilterControlAltText="Filter numero_contrato column"
                                            HeaderText="Código Contrato" SortExpression="numero_contrato"
                                            UniqueName="numero_contrato" >
                                            <HeaderStyle Width="50px" />                                            
                                            <ItemStyle Width="50px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fecha_inicio"
                                            FilterControlAltText="Filter fecha_inicio column"
                                            HeaderText="Fecha de inicio" SortExpression="fecha_inicio" DataFormatString="{0:MM/dd/yyyy}"
                                            UniqueName="fecha_inicio" >
                                            <HeaderStyle Width="150px" />                                            
                                            <ItemStyle Width="150px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fecha_finalizacion"
                                            FilterControlAltText="Filter fecha_finalizacion column"
                                            HeaderText="Fecha de finalización" SortExpression="fecha_finalizacion" DataFormatString="{0:MM/dd/yyyy}"
                                            UniqueName="fecha_finalizacion" >
                                            <HeaderStyle Width="150px" />                                            
                                            <ItemStyle Width="150px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="contratista"
                                            FilterControlAltText="Filter contratista column"
                                            HeaderText="Contratista" SortExpression="contratista"
                                            UniqueName="contratista" >
                                            <HeaderStyle Width="230px" />                                            
                                            <ItemStyle Width="230px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="numero_documento"
                                            FilterControlAltText="Filter numero_documento column"
                                            HeaderText="Documento contratista" SortExpression="numero_documento"
                                            UniqueName="numero_documento" >
                                            <HeaderStyle Width="170px" />                                            
                                            <ItemStyle Width="170px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="supervisor"
                                            FilterControlAltText="Filter supervisor column"
                                            HeaderText="Supervisor" SortExpression="supervisor"
                                            UniqueName="supervisor" >
                                            <HeaderStyle Width="170px" />                                            
                                            <ItemStyle Width="170px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="valor_contrato"
                                            FilterControlAltText="Filter valor_contrato column" DataFormatString="{0:n}"
                                            HeaderText="Valor" SortExpression="supervisor"  ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            UniqueName="valor_contrato" >
                                            <HeaderStyle Width="120px" />                                            
                                            <ItemStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="estado_contrato"
                                            FilterControlAltText="Filter estado_contrato column" visible="true"
                                            HeaderText="Estado" SortExpression="estado_contrato"
                                            UniqueName="estado_contrato" >
                                            <HeaderStyle Width="550px" />                                            
                                            <ItemStyle Width="550px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="id_estado_contrato"
                                            FilterControlAltText="Filter id_estado_contrato column" visible="false"
                                            SortExpression="id_estado_contrato"
                                            UniqueName="id_estado_contrato" >
                                            <HeaderStyle Width="550px" />                                            
                                            <ItemStyle Width="550px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn FilterControlAltText="filtro Modificaciones column" UniqueName="colm_permisos" visible="false" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="col_hlk_mod" runat="server" ImageUrl="~/Imagenes/iconos/refresh.png" ToolTip="Permisos" />
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" />
                                            <HeaderStyle Width="30px" />                                                                                         
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="colm_activar" Visible="false"  AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlk_activar" runat="server" ImageUrl="~/Imagenes/iconos/additem.png"
                                                    ToolTip="Seguimiento de entregables">
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle Width="25px" />
                                           <ItemStyle Width="25px" />    
                                        </telerik:GridTemplateColumn>
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
</asp:Content>