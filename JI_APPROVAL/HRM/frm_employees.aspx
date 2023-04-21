<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_employees.aspx.vb" Inherits="RMS_APPROVAL.frm_employee" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración de Empleados</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Empleados</asp:Label>
                </h3>
            </div>
            <div class="box-body">

                 <ul class="nav nav-tabs">
                     <li class="active"><a data-toggle="tab" href="#Employees"><asp:Label runat="server" ID="lblt_Employees" CssClass="control-label text-bold">Empleados</asp:Label></a></li>                                    
                     <li><a data-toggle="tab" href="#Users"><asp:Label runat="server" ID="lblt_users" CssClass="control-label text-bold">Empleados por Registrar</asp:Label></a></li>
                 </ul>

             <div class="tab-content">
                 
                  <div id="Employees" class="tab-pane fade in active">

                          <div class="form-group row" style="width:100%; margin: 0 auto">

			                    <%--***********REGISTER EMPLOYEES***********--%>

                                   <div class="col-lg-12">
                                            <br /><br />
                                            <telerik:RadTextBox ID="txt_doc" runat="server"
                                                EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="395px"
                                                ValidationGroup="1">
                                            </telerik:RadTextBox>
                                            <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                                            </telerik:RadButton>
                                            <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Enabled="false" Text="Nuevo Empleado">
                                            </telerik:RadButton>
                                            <hr />

                                                <telerik:RadGrid ID="grd_cate_employee" runat="server" AllowAutomaticDeletes="True"
                                                    CellSpacing="0" AllowPaging="True" PageSize="15"
                                                    AutoGenerateColumns="False">
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_usuario" AllowAutomaticUpdates="True">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="id_usuario"
                                                                FilterControlAltText="Filter id_usuario column"
                                                                SortExpression="id_usuario" UniqueName="id_usuario"
                                                                Visible="False" DataType="System.Int32" HeaderText="id_usuario"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>        
                                                            <telerik:GridBoundColumn DataField="id_rh_employee"
                                                                FilterControlAltText="Filter id_rh_employee column"
                                                                SortExpression="id_usuario" UniqueName="id_rh_employee"
                                                                Visible="False" DataType="System.Int32" HeaderText="id_rh_employee"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>        
                                                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_edit" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar usuario" Target="_self" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="5px" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="job"
                                                                FilterControlAltText="Filter job column" HeaderText="Cargo" UniqueName="colm_codigo_usuario">
                                                                <HeaderStyle Width="150px" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="nombre_usuario"
                                                                FilterControlAltText="Filter nombre_usuario column"
                                                                HeaderText="Nombres" SortExpression="nombre_usuario"
                                                                UniqueName="colm_nombre_usuario">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="apellidos_usuario"
                                                                FilterControlAltText="Filter apellidos_usuario column" HeaderText="Apellidos"
                                                                SortExpression="apellidos_usuario"
                                                                UniqueName="colm_apellidos_usuario">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="email_usuario"
                                                                FilterControlAltText="Filter email_usuario column"
                                                                HeaderText="Email Usuario" SortExpression="email_usuario"
                                                                UniqueName="colm_email_usuario">
                                                            </telerik:GridBoundColumn>                           
                                                             <telerik:GridBoundColumn DataField="contract_date"
                                                                FilterControlAltText="Filter contract_date column"
                                                                HeaderText="Fecha de ingreso" SortExpression="usuario"
                                                                UniqueName="colm_contract_date" DataType="System.DateTime" DataFormatString="{0:dd/MM/yyyy}">
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn DataField="expired_date"
                                                                FilterControlAltText="Filter expired_date column"
                                                                HeaderText="Fecha vencimiento" SortExpression="expired_date"
                                                                UniqueName="colm_expired_date" DataType="System.DateTime" DataFormatString="{0:dd/MM/yyyy}">
                                                            </telerik:GridBoundColumn>
                                                          <%--  
                                                                <telerik:GridBoundColumn DataField="usuario"
                                                                    FilterControlAltText="Filter usuario column"
                                                                    HeaderText="Usuario" SortExpression="usuario"
                                                                    UniqueName="colm_usuario">
                                                                </telerik:GridBoundColumn>
                              
                                                               <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                UniqueName="visibleProgramas" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_programa" runat="server" ImageUrl="../Imagenes/iconos/view_tree.png" ToolTip="Asignar Programa" Target="_self" Width="20" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                UniqueName="visibleRoles" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_roles" runat="server" ImageUrl="../Imagenes/iconos/Family.png" ToolTip="Asignar Rol" Target="_self" Width="20" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                UniqueName="visibleSubRegiones" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_regiones" runat="server" ImageUrl="../Imagenes/iconos/MapPoint.png" ToolTip="Asignar Sub Regiones" Target="_self" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>--%>
                                                            <telerik:GridTemplateColumn UniqueName="Completo">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_Completo" runat="server" ImageUrl="~/Imagenes/iconos/alerta.png"
                                                                        ToolTip="Empleado Incompleto">
                                                                    </asp:HyperLink>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>
                                                          <%--  <telerik:GridBoundColumn DataField="usuario_completo"
                                                                FilterControlAltText="Filter descripcion_riesgo column" HeaderText="usuario_completo"
                                                                SortExpression="usuario_completo"
                                                                UniqueName="usuario_completo" Visible="False">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                UniqueName="colm_usrAccMod" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_usrAccMod" runat="server" ImageUrl="../Imagenes/iconos/arbol.png" ToolTip="Asignar Modulos" Target="_self" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                UniqueName="colm_ficha_proyecto" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_ficha_proyecto" runat="server" ImageUrl="../Imagenes/iconos/flag_green.png" ToolTip="Asignar fichas de proyectos" Target="_self" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>--%>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                        </div> <%--<div class="col-lg-12">--%>

                                     <%--***********REGISTER EMPLOYEES***********--%>

                          </div>    <%--<div class="form-group row">   --%> 
       
                     </div> <%--id=Employees"--%>
              
                 <div id="Users" class="tab-pane fade" >

                          <div class="form-group row" style="width:100%; margin: 0 auto">
                          <br /><br />
                             
                                  <%--***********Users by Register***********--%>

                                <telerik:RadGrid ID="grd_users" runat="server" AllowAutomaticDeletes="True"
                                                    CellSpacing="0" AllowPaging="false"
                                                    AutoGenerateColumns="False">
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_usuario" AllowAutomaticUpdates="True">
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="id_usuario"
                                                                FilterControlAltText="Filter id_usuario column"
                                                                SortExpression="id_usuario" UniqueName="id_usuario"
                                                                Visible="False" DataType="System.Int32" HeaderText="id_usuario"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>        
                                                            <telerik:GridBoundColumn DataField="id_rh_employee"
                                                                FilterControlAltText="Filter id_rh_employee column"
                                                                SortExpression="id_usuario" UniqueName="id_rh_employee"
                                                                Visible="False" DataType="System.Int32" HeaderText="id_rh_employee"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>        
                                                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_add" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_add" runat="server" ImageUrl="../Imagenes/iconos/drop-add.gif" ToolTip="Agregar Empleado" Target="_self" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="5px" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="job"
                                                                FilterControlAltText="Filter job column" HeaderText="Cargo" UniqueName="colm_codigo_usuario">
                                                                <HeaderStyle Width="150px" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn DataField="nombre_usuario"
                                                                FilterControlAltText="Filter nombre_usuario column"
                                                                HeaderText="Nombres" SortExpression="nombre_usuario"
                                                                UniqueName="colm_nombre_usuario">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="apellidos_usuario"
                                                                FilterControlAltText="Filter apellidos_usuario column" HeaderText="Apellidos"
                                                                SortExpression="apellidos_usuario"
                                                                UniqueName="colm_apellidos_usuario">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="email_usuario"
                                                                FilterControlAltText="Filter email_usuario column"
                                                                HeaderText="Email Usuario" SortExpression="email_usuario"
                                                                UniqueName="colm_email_usuario">
                                                            </telerik:GridBoundColumn>                           
                                                             <telerik:GridBoundColumn DataField="date_started"
                                                                FilterControlAltText="Filter date_started column"
                                                                HeaderText="Fecha de ingreso" SortExpression="usuario"
                                                                UniqueName="colm_date_started" DataType="System.DateTime" DataFormatString="{0:dd/MM/yyyy}">
                                                            </telerik:GridBoundColumn>                                                            
                                                          <%--  
                                                                <telerik:GridBoundColumn DataField="usuario"
                                                                    FilterControlAltText="Filter usuario column"
                                                                    HeaderText="Usuario" SortExpression="usuario"
                                                                    UniqueName="colm_usuario">
                                                                </telerik:GridBoundColumn>
                              
                                                               <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                UniqueName="visibleProgramas" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_programa" runat="server" ImageUrl="../Imagenes/iconos/view_tree.png" ToolTip="Asignar Programa" Target="_self" Width="20" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                UniqueName="visibleRoles" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_roles" runat="server" ImageUrl="../Imagenes/iconos/Family.png" ToolTip="Asignar Rol" Target="_self" Width="20" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                UniqueName="visibleSubRegiones" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_regiones" runat="server" ImageUrl="../Imagenes/iconos/MapPoint.png" ToolTip="Asignar Sub Regiones" Target="_self" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>--%>
                                                            <telerik:GridTemplateColumn UniqueName="Completo">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_Completo" runat="server" ImageUrl="~/Imagenes/iconos/alerta.png"
                                                                        ToolTip="Empleado Incompleto">
                                                                    </asp:HyperLink>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>
                                                          <%--  <telerik:GridBoundColumn DataField="usuario_completo"
                                                                FilterControlAltText="Filter descripcion_riesgo column" HeaderText="usuario_completo"
                                                                SortExpression="usuario_completo"
                                                                UniqueName="usuario_completo" Visible="False">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                UniqueName="colm_usrAccMod" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_usrAccMod" runat="server" ImageUrl="../Imagenes/iconos/arbol.png" ToolTip="Asignar Modulos" Target="_self" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                                                UniqueName="colm_ficha_proyecto" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="col_hlk_ficha_proyecto" runat="server" ImageUrl="../Imagenes/iconos/flag_green.png" ToolTip="Asignar fichas de proyectos" Target="_self" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>--%>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>




  	                              <%--***********Users by Register***********--%>


                          </div>    <%--<div class="form-group row">   --%>                             

                     </div> <%--id="Users"--%>


   </div> <%--<div class="tab-content">--%>
                   
  </div>  <%--<div class="box-body">--%>



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

