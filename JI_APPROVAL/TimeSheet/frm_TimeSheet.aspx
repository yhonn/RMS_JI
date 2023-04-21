<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_TimeSheet" Codebehind="frm_TimeSheet.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

              <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">TIME SHEET</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box">
                     <div class="box-header with-border" >
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Time sheets</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                                        

                   <div class="form-group row">

                        <div class="col-sm-10">
                         
                             <asp:Label ID="lbl_text_Error" ForeColor="Red" runat="server" text="" ></asp:Label>  

                             <asp:Label ID="lbl_GroupRolID" runat="server" Visible="false" ></asp:Label>  
                             <asp:HiddenField runat="server" ID="h_Filter" Value="" />  
                            <telerik:RadTextBox ID="txt_doc" Runat="server" EmptyMessage="Type a keyword here..."  Width="360px" ValidationGroup="1" >
                            </telerik:RadTextBox> 
                                           
                            <telerik:RadButton ID="btn_buscar" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Search" Width="12%"   CssClass="btn btn-sm" >
                            </telerik:RadButton>                              
                            <telerik:RadButton ID="btn_nuevo" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="New Time Sheet" Width="12%" Enabled ="false" CssClass="btn btn-sm">
                            </telerik:RadButton>

                            <telerik:RadButton ID="btn_leave_approval" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Leave Days Approval"  Width="15%" CssClass="btn btn-sm"></telerik:RadButton>

                          
                           <telerik:RadButton ID="RadButton1" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Send Email" Visible="False" ></telerik:RadButton>

                            <telerik:RadButton ID="Botom_testing" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Notify Pending APP" Visible="false">
                            </telerik:RadButton>

                          
                        </div>

                        <div class="col-sm-1 pull-right">  
                            <div class="dropdown pull-right">
                                 <button class="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="fa fa-question-circle fa-2x"></i>
                                 </button>
                                 <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                                      <a class="dropdown-item" href="javascript:showhelp('Time_sheet_01.mp4');")>Listado de Hojas de tiempo</a><br /><br />
                                      <a class="dropdown-item" href="javascript:showhelp('Time_sheet_02.mp4');">Nueva Hoja de Tiempo</a><br /><br />
                                      <a class="dropdown-item" href="javascript:showhelp('TimeShee_Edit_002.mp4');">Editar Hoja de tiempo</a><br /><br />
                                      <div class="dropdown-divider"></div>
                                      <a class="dropdown-item" href="javascript:showhelp('TimeShee_Edit_002.mp4');">Observación de Hoja de tiempo</a><br /><br />
                                 </div>
                              </div>
                       
                        </div>

                
                  </div>

                  <div class="form-group row">
                   
                      <div class="col-sm-1 text-left">
                        <!--Tittle -->
                          <asp:Label ID="lbl_anio" runat="server" CssClass="control-label text-bold" Text="Periodo (Año):"></asp:Label>
                       </div>
                      <div class="col-sm-2">                         
                        
                          <!--Control -->
                            <telerik:RadComboBox  ID="cmb_year" 
                                                  runat ="server" 
                                                  EmptyMessage="Select a year..."  
                                                  DataTextField ="year" 
                                                  DataValueField="id_year" 
                                                  AutoPostBack="true" 
                                                  Width="60%">
                                     
                           </telerik:RadComboBox>

                          <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                                                ControlToValidate="cmb_year" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                   
                      </div>

                        <div class="col-sm-1 text-left">
                         <!--Tittle -->
                          <asp:Label ID="lblMonth" runat="server" CssClass="control-label text-bold"   Text="Periodo (Mes):"></asp:Label>
                        </div>
                       <div class="col-sm-2">  
                           <!--Control -->
                             <telerik:RadComboBox  ID="cmb_Month" 
                                                   runat ="server" 
                                                   EmptyMessage="Select a year..."  
                                                   DataTextField ="month" 
                                                   DataValueField="id_month"    
                                                   AutoPostBack="true"                                               
                                                   Width="90%" >
                                      
                            </telerik:RadComboBox>  
                           
                               <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red"
                                                ControlToValidate="cmb_Month" ErrorMessage="Required" ValidationGroup="1"></asp:RequiredFieldValidator>


                       </div>

                      <div class="col-sm-4">                      
                            <telerik:RadButton ID="btn_Period_Report" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Time Sheet Reports"  Width="50%" CssClass="btn btn-sm" Enabled ="false" ValidationGroup="1"></telerik:RadButton>
                      </div>
                                      
                  </div>


                    
                    </div>

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

                    <div class="box-body">
                          <div class="box-body">
                               <div class="col-lg-12" style="width:100%; margin: 0 auto; margin-top:10px;">
                                     <div style="max-width:100%; overflow-x:auto">
                                         
                                      <telerik:RadGrid ID="grd_cate"  
                                          Skin="Office2010Blue"   
                                          runat="server" 
                                          AllowAutomaticDeletes="True" 
                                          CellSpacing="0"  
                                          GridLines ="None" 
                                          Width="100%" 
                                          AllowAutomaticUpdates="True"  
                                          AutoGenerateColumns="False"
                                          AllowPaging="True" 
                                          PageSize="25"  
                                          AllowSorting="True" AllowFilteringByColumn="true" >

                                             <ClientSettings EnableRowHoverStyle="true" >
                                                <Selecting AllowRowSelect="True"></Selecting>                                                    
                                                <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                             </ClientSettings>
                                             <GroupingSettings CaseSensitive="false" />

                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_timesheet" AllowAutomaticUpdates="True" >
                                            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                                                                         

                                             <Columns>           
                                                 

                                                         <telerik:GridTemplateColumn UniqueName="colm_edit" Visible="false" ShowFilterIcon="false" AllowFiltering="false">
                                                              <ItemTemplate>
                                                                <asp:ImageButton ID="editar" runat="server" 
                                                                 ImageUrl   ="~/Imagenes/iconos/b_edit.png" ToolTip="Edit" />
                                                              </ItemTemplate>
                                                              <ItemStyle Width="25px" />
                                                              <HeaderStyle Width="25px"  />
                                                          </telerik:GridTemplateColumn>
                                                          
                                                        <telerik:GridBoundColumn  
                                                                    DataField="id_timesheet" 
                                                                    UniqueName="id_timesheet" 
                                                                    Visible="true" Display="false">                                                               
                                                             </telerik:GridBoundColumn>                                                           

                                                             <telerik:GridBoundColumn  
                                                                    DataField="TimeSheet_Type" 
                                                                    FilterControlAltText="Filter TimeSheet_Type column" 
                                                                    HeaderText="Tipo de aprobación" SortExpression="TimeSheet_Type"  
                                                                    UniqueName="TimeSheet_Type">    
                                                                 <ItemStyle Width="125px" />
                                                                 <HeaderStyle Width="125px"  />
                                                             </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn  
                                                                    DataField="usuario_creo" 
                                                                    UniqueName="usuario_creo" 
                                                                    Visible="true" Display="false">  
                                                                <ItemStyle Width="125px" />
                                                                <HeaderStyle Width="125px"  />
                                                             </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn  
                                                                    DataField="id_usuario" 
                                                                    UniqueName="id_usuario" 
                                                                    Visible="true" Display="false">                                                               
                                                             </telerik:GridBoundColumn> 
                                                             
                                                            <telerik:GridBoundColumn DataField="nombre_usuario" 
                                                                    FilterControlAltText="Filter nombre_usuario column" 
                                                                    HeaderText="Usuario" SortExpression="nombre_usuario"  
                                                                    UniqueName="nombre_usuario">
                                                                     <ItemStyle Width="150px" />
                                                                    <HeaderStyle Width="150px"  />
                                                             </telerik:GridBoundColumn>
                                                                
                                                            <telerik:GridBoundColumn DataField="job" 
                                                                    FilterControlAltText="Filter job column" HeaderText="Cargo" 
                                                                    SortExpression="job" UniqueName="job" >
                                                                      <ItemStyle Width="150px" />
                                                                    <HeaderStyle Width="150px"  />
                                                                </telerik:GridBoundColumn>    
                                                 
                                                                <telerik:GridBoundColumn DataField="anio" 
                                                                    FilterControlAltText="Filter anio column" HeaderText="Año" 
                                                                    SortExpression="anio" UniqueName="anio">
                                                                     <ItemStyle Width="125px" />
                                                                     <HeaderStyle Width="125px"  />
                                                                </telerik:GridBoundColumn> 
                                                 
                                                                <telerik:GridBoundColumn DataField="mes" 
                                                                    FilterControlAltText="Filter mes column" HeaderText="Month" 
                                                                    SortExpression="mes" UniqueName="mes"  Visible="true" Display="false">
                                                                    <ItemStyle Width="125px" />
                                                                    <HeaderStyle Width="125px"  />
                                                                </telerik:GridBoundColumn>   
                                                          
                                                              <telerik:GridBoundColumn DataField="month_name" 
                                                                    FilterControlAltText="Filter month_name column" HeaderText="Mes" 
                                                                    SortExpression="month_name" UniqueName="month_name"  Visible="true" Display="true">
                                                                   
                                                                  <ItemStyle Width="125px" />
                                                                  <HeaderStyle Width="125px"  />
                                                               
                                                              </telerik:GridBoundColumn>   

                                                               <telerik:GridBoundColumn DataField="description" 
                                                                    FilterControlAltText="Filter description column" 
                                                                    HeaderText="Descripción" SortExpression="description" 
                                                                    UniqueName="description">  
                                                                   <ItemStyle Width="150px" />
                                                                   <HeaderStyle Width="150px" />
                                                               </telerik:GridBoundColumn>

                                                               <telerik:GridBoundColumn DataField="timesheet_estado" 
                                                                    FilterControlAltText="Filter timesheet_estado column" HeaderText="Estado" 
                                                                    SortExpression="timesheet_estado" UniqueName="timesheet_estado">
                                                                     <ItemStyle Width="125px" />
                                                                     <HeaderStyle Width="125px"  />
                                                                </telerik:GridBoundColumn>
                                                 
                                                               <telerik:GridBoundColumn DataField="id_timesheet_estado"  Visible="true" Display="false"
                                                                    FilterControlAltText="Filter id_timesheet_estado column" HeaderText="id_Estado" 
                                                                    SortExpression="id_timesheet_estado" UniqueName="id_timesheet_estado">
                                                                </telerik:GridBoundColumn>     
                                                          
                                                              <telerik:GridBoundColumn DataField="ts_leave_update"  Visible="true" Display="false"
                                                                   UniqueName="ts_leave_update">
                                                                </telerik:GridBoundColumn>     
                                                                                                                                                                                    
                                                                <telerik:GridBoundColumn DataField="employee_type"  HeaderText="Tipo de empleado" 
                                                                    FilterControlAltText="Filter employee_type column" SortExpression="employee_type" 
                                                                    UniqueName="employee_type" Visible="true" Display="true">
                                                                     
                                                                    <ItemStyle Width="125px" />
                                                                    <HeaderStyle Width="125px"  />

                                                                </telerik:GridBoundColumn>   
                                                 
                                                                <telerik:GridTemplateColumn UniqueName="colm_open" Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                      <ItemTemplate>
                                                                           <asp:HyperLink ID="hlk_timesheet" runat="server" ImageUrl="~/Imagenes/iconos/Preview.png" 
                                                                            ToolTip="View TimeSheet" Target="_new">
                                                                            </asp:HyperLink>
                                                                       </ItemTemplate>

                                                                    <ItemStyle Width="30px" />
                                                                    <HeaderStyle Width="30px"  />

                                                                </telerik:GridTemplateColumn>
                                                                  
                                                               
                                                                <telerik:GridButtonColumn 
                                                                      Visible="false"
                                                                      ConfirmText="Do you want to delete the TimeSheet?" 
                                                                      ConfirmDialogType="RadWindow"
                                                                      ConfirmTitle="TimeSheet" 
                                                                      ButtonType="ImageButton"  
                                                                      CommandName="Delete" 
                                                                      ConfirmDialogHeight="100px"                                                                                    
                                                                      ConfirmDialogWidth="400px" 
                                                                      UniqueName="colm_delete" 
                                                                      ImageUrl="../Imagenes/Iconos/b_drop.png" >
                                                                     
                                                                    <ItemStyle Width="30px" />
                                                                    <HeaderStyle Width="30px"  />

                                                                </telerik:GridButtonColumn>


                                                                    <telerik:GridTemplateColumn UniqueName="colm_edit_app" Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                      <ItemTemplate>
                                                                           <asp:HyperLink ID="hlk_edit_app" runat="server" ImageUrl="~/Imagenes/iconos/note_edit.png" 
                                                                            ToolTip="Edit TimeSheet Approval" Target="_new">
                                                                            </asp:HyperLink>
                                                                       </ItemTemplate>
                                                                    
                                                                        <ItemStyle Width="30px" />
                                                                       <HeaderStyle Width="30px"  />

                                                                 </telerik:GridTemplateColumn>



                                                            </Columns>

                                                            <GroupByExpressions>
                                                                <telerik:GridGroupByExpression>
                                                                    <SelectFields>
                                                                        <telerik:GridGroupByField FieldAlias="anio" FieldName="anio" FormatString="" HeaderText="" />
                                                                    </SelectFields>
                                                                    <GroupByFields>
                                                                        <telerik:GridGroupByField FieldName="anio" />
                                                                    </GroupByFields>
                                                                </telerik:GridGroupByExpression>
                                                                 <telerik:GridGroupByExpression>
                                                                    <SelectFields>
                                                                        <telerik:GridGroupByField FieldAlias="-" FieldName="month_name" FormatString="" />
                                                                    </SelectFields>
                                                                    <GroupByFields>
                                                                        <telerik:GridGroupByField FieldName="month_name" />
                                                                    </GroupByFields>
                                                                </telerik:GridGroupByExpression>
                                                            </GroupByExpressions>
                                                           
                                                        </MasterTableView>
                                                                                                
                                         </telerik:RadGrid>
                                         
                                      </div>

                                         <asp:SqlDataSource ID="SqlDataSource2" runat="server"  ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                 SelectCommand="SELECT id_tipoDocumento, id_categoria, descripcion_aprobacion, condicion, nivel_aprobacion, email_notificacion, descripcion_cat, id_programa, nombre_proyecto, nombre_actividad, nombre_cliente, visible 
			                                                     FROM vw_aprobaciones 
			                                                      WHERE (id_programa = @id_program)">
                                                 <SelectParameters>
                                                     <asp:SessionParameter DefaultValue="-1" Name="id_program" 
                                                         SessionField="E_IDPrograma" />
                                                 </SelectParameters>
                                             </asp:SqlDataSource>
             
                               </div>
                           </div>
                    </div>

    
                 </div>                

             </section>

    

    </asp:Content>

