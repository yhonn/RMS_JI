<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_EnvironmentalApprovals"  Codebehind="frm_EnvironmentalApprovals.aspx.vb" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

      <section class="content-header">
            <h1>
                <asp:Label runat="server" ID="lblt_titulo_pantalla">ENVIRONMENTAL APPROVAL</asp:Label>
            </h1>
        </section>

     <section class="content">
        
            <div class="box">
            
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Pending Approval</asp:Label>
                </h3>
            </div>

            <div class="box-body">
                  <div class="col-lg-12">
                    <div class="box-body">
                        
                         <div class="box-body">

                           <div class="form-group row">
                                <div class="col-sm-2 text-left">
                                   <asp:Label ID="lblb_InstrumentN" runat="server" CssClass="control-label text-bold" Text="Instrument Number"></asp:Label>
                           
                                </div>
                                <div class="col-sm-3">
                                <%--<telerik:RadTextBox ID="txt_InstrumentNumber" Runat="server" Width="20%" ReadOnly="true" >                                                              
                                 </telerik:RadTextBox>--%>
                                  <asp:Label ID="lbl_InstrumentNumber" runat="server" CssClass="control-label" Text=""></asp:Label>
                                  <asp:HiddenField runat="server" ID="hd_Id_doc" value="0" />
                                </div>
                            
                                 <div class="col-sm-2 text-left">
                                   <asp:Label ID="lbl_approvalName" runat="server" CssClass="control-label text-bold" Text="Approval Type"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                               <%-- <telerik:RadTextBox ID="txt_approvalName" Runat="server" Width="20%" ReadOnly="true" >                              
                                 </telerik:RadTextBox>--%>
                                     <asp:Label ID="lbl_approvalNameD" runat="server" CssClass="control-label" Text=""></asp:Label>
                                </div>                           
                           </div>

                           <div class="form-group row">
                            
                            </div>

                            <div class="form-group row">
                            <div class="col-sm-2 text-left">
                               <asp:Label ID="lbl_Beneficiary" runat="server" CssClass="control-label text-bold" Text="In reference To"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                            <%--    <telerik:RadTextBox ID="txt_beneficiary" Runat="server" Width="20%" ReadOnly="true" >                              
                                 </telerik:RadTextBox>--%>
                                <asp:Label ID="lbl_beneficiaryN" runat="server" CssClass="control-label" Text=""></asp:Label>
                            </div>
                            </div>
                            
                            <div class="form-group row">
                                <div class="col-sm-2 text-left">
                                   <asp:Label ID="lbl_dateCreated" runat="server" CssClass="control-label text-bold" Text="Created"></asp:Label>
                                </div>
                                <div class="col-sm-8">
                                     <asp:Label ID="lbl_created_user" runat="server" CssClass="control-label" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                     <asp:Label ID="lbl_created_Date" runat="server" CssClass="control-label" Text=""></asp:Label>
                                    
                                    <%--   <telerik:RadDatePicker ID="txt_finicio" Runat="server" DateInput-ReadOnly="true" >
                                           <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x" ></Calendar>                                   
                                           <DateInput DisplayDateFormat="dd/MM/yyyy" DateFormat="dd/MM/yyyy" LabelWidth=""></DateInput>
                                       </telerik:RadDatePicker>--%>

                               
                                </div>
                            </div>
                             

                          <div class="form-group row">
                            <div class="col-sm-2 text-left">
                               <asp:Label ID="lbl_dateUpdated" runat="server" CssClass="control-label text-bold" Text="Updated"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                  <%-- <telerik:RadDatePicker ID="txt_ffin" Runat="server" DateInput-ReadOnly="true"  >                                       
                                        <DateInput DisplayDateFormat="dd/MM/yyyy" DateFormat="dd/MM/yyyy" LabelWidth=""></DateInput>
                                   </telerik:RadDatePicker>--%>
                                       <asp:Label ID="lbl_updated_user" runat="server" CssClass="control-label" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                      
                                     <asp:Label ID="lbl_updated_Date" runat="server" CssClass="control-label" Text=""></asp:Label>

                            </div>
                            </div>

                             <hr />
                              
                         </div>

                        
                        <div class="box-body">
                          
                          
                          <div class="form-group row">
                            <div class="col-sm-2 text-left">
                                <asp:Label ID="lbl_Estado" runat="server" CssClass="control-label text-bold" Text="Estatus"></asp:Label>
                            </div>
                            <div class="col-sm-8">
                                  <span runat="server" id="spn_state" class="label label-danger" visible="false"></span>          
                                  <span runat="server" id="spn_state_approved" class="label label-success " visible="false"></span>      
                            </div>
                           </div>       
                            
                             <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                            <asp:Label ID="lbl_review_type" runat="server" CssClass="control-label text-bold" Text="Review Type"></asp:Label>
                                        </div>

                                        <div class="col-sm-8">
                                             <telerik:RadComboBox ID="cmb_rev_type" 
                                                 Runat="server" 
                                                 CausesValidation="False" 
                                                 DataSourceID="SqlDataSource1"                                      
                                                 DataTextField="nombre_tipo" 
                                                 DataValueField="id_tipoApp_Environmental"  
                                                 AllowCustomText="true" 
                                                 EmptyMessage="Select a type..." 
                                                 Width="35%">                                     
                                            </telerik:RadComboBox>
                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                                                ErrorMessage="Required" ControlToValidate="cmb_rev_type" 
                                                 ValidationGroup="1"></asp:RequiredFieldValidator>
                                        </div>                 

                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                                       ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                       ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>"                             
                                                       SelectCommand="select id_TipoApp_Environmental, nombre_tipo from tipo_AppAmbiental " >                                                    
                                        </asp:SqlDataSource>
                                       
                                 </div>                                       
                        

                            <div class="form-group row">
                                <div class="col-sm-2 text-left">
                                    <asp:Label ID="lblt_observation" runat="server" CssClass="control-label text-bold" Text="Observation"></asp:Label>
                                </div>
                                <div class="col-sm-8">                               

                                   <telerik:RadTextBox ID="txt_observation"  EmptyMessage="Type Observation here.." Runat="server" Height="50px"  TextMode="MultiLine" Width="80%">                               
                                   </telerik:RadTextBox>      
                                                                       
                                </div>
                            </div>


                         </div>
                                             

                          
                               <div class="box-body">
                                    <div class=" row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lbl_documentsAPP" runat="server" CssClass="control-label text-bold"   Text="Document Type "></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                               <asp:HiddenField ID="hd_id_tp_doc" runat="server" Value="0" />                                             
                                              <telerik:RadGrid ID="grd_documentos"  
                                                                               Skin="Office2010Blue"   
                                                                               runat="server"                                                                                
                                                                               CellSpacing="0" 
                                                                               DataSourceID="" 
                                                                               GridLines="None" 
                                                                               Width="85%"                                                                                
                                                                               EnableViewState="true"
                                                                               AllowPaging="True" 
                                                                               AllowSorting="True"
                                                                               PageSize="10" >
                                                                                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                                                    <WebServiceSettings>
                                                                                    <ODataSettings InitialContainerName=""></ODataSettings>
                                                                                    </WebServiceSettings>
                                                                                    </HeaderContextMenu>
                                                                                    <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                            </ClientSettings>
                                                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_doc_soporte" >
                                                                                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

                                                                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                                                    </RowIndicatorColumn>

                                                                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                                                    </ExpandCollapseColumn>

                                                                                    <Columns>

                                                                                             <telerik:GridTemplateColumn 
                                                                                                 FilterControlAltText="Filter select column" 
                                                                                                 UniqueName="colm_select" 
                                                                                                 Visible="true"
                                                                                                 HeaderText="Sel">
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True"  
                                                                                                        oncheckedchanged="chkVisible_CheckedChangedDOCS"  />
                                                                                                    <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server" 
                                                                                                        CheckedImageUrl="~/Imagenes/iconos/accept.png" ImageHeight="16" ImageWidth="16" 
                                                                                                        TargetControlID="chkSelect" UncheckedImageUrl="~/Imagenes/iconos/icon-warningAlert.png">
                                                                                                    </ajaxToolkit:ToggleButtonExtender>
                                                                                                </ItemTemplate>                                                                                                
                                                                                                <ItemStyle Width="5%" />
                                                                                            </telerik:GridTemplateColumn>

                                                                                       <telerik:GridBoundColumn  DataField="id_doc_soporte" DataType="System.Int32" 
                                                                                            FilterControlAltText="Filter id_categoria column" HeaderText="id_doc_soporte" 
                                                                                            SortExpression="id_doc_soporte" UniqueName="id_doc_soporte" 
                                                                                            Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>
                                                                                        <telerik:GridBoundColumn DataField="nombre_documento" 
                                                                                            FilterControlAltText="Filter descripcion_cat column" 
                                                                                            HeaderText="Name of document" SortExpression="nombre_documento" 
                                                                                            UniqueName="descripcion_cat">
                                                                                        </telerik:GridBoundColumn>

                                                                                        <telerik:GridBoundColumn DataField="extension" 
                                                                                            FilterControlAltText="Filter extension column" HeaderText="Allowed extension" 
                                                                                            SortExpression="extension" UniqueName="extension">
                                                                                        </telerik:GridBoundColumn>
                                    
                                                                                         <telerik:GridBoundColumn DataField="Template" 
                                                                                            FilterControlAltText="Filter Template column" HeaderText="Document Template" 
                                                                                            UniqueName="Template" Visible="true" Display="false">
                                                                                        </telerik:GridBoundColumn>
                                    
                                                                                     <telerik:GridTemplateColumn 
                                                                                          FilterControlAltText="Filter colm_template column"  HeaderText="Document Template" 
                                                                                          UniqueName="colm_template" >                                      
                                                                                         <ItemTemplate>                                       
                                                                                             <asp:HyperLink ID="hlk_Template" 
                                                                                                 runat="server" 
                                                                                                 Text="--none--"                                          
                                                                                                 navigateUrl="#"></asp:HyperLink>                                       
                                                                                         </ItemTemplate>
                                                                                          <ItemStyle Width="30%" />
                                                                                       </telerik:GridTemplateColumn>
                                                                                    </Columns>

                                                                                <EditFormSettings>
                                                                                <EditColumn FilterControlAltText="Filter EditCommandColumn column" 
                                                                                        UniqueName="EditCommandColumn1" ></EditColumn>
                                                                                </EditFormSettings>
                                                                                </MasterTableView>

                                                                                <FilterMenu EnableImageSprites="False">
                                                                                <WebServiceSettings>
                                                                                <ODataSettings InitialContainerName=""></ODataSettings>
                                                                                </WebServiceSettings>
                                                                                </FilterMenu>
                                                                          </telerik:RadGrid>  

                                            <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />                                                                                 
                                           <asp:Label ID="lbl_errExtension" runat="server" Font-Names="Arial" Font-Size="Small" ForeColor="red" Visible="false" Text="Select a type of document"></asp:Label> 
                                        
                        
                                       </div>
                                    </div>
                                  </div>
                        
                          
                     
                      <div class="box-body">
                           <div class="form-group row">
                            <div class="col-sm-2 text-left">
                               <asp:Label ID="lbl_docs" runat="server" CssClass="control-label text-bold" Text="Documents"></asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                               
                                   <telerik:RadAsyncUpload 
                                    runat="server" 
                                    RenderMode="Lightweight" 
                                    ID="uploadFile"
                                    Skin="Metro"                                                                          
                                    OnFileUploaded="file_uploaded_change"                                        
                                    MaxFileInputsCount="1"
                                    MultipleFileSelection="Disabled">
                                </telerik:RadAsyncUpload>                                  
                               
                            </div>
                            <div class="col-sm-4 text-left">
                                <telerik:RadButton ID="btn_addDoc" runat="server"  SingleClick="true" SingleClickText="Uploading..." Text="Add Document" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn-default" Enabled="false"> </telerik:RadButton>                                                          
                                 <asp:Label ID="lblt_ErrTypeDocFile" runat="server" Text="**  Field required" ForeColor="Red"  CssClass="btn btn-sm" Visible="false" ></asp:Label>
                            </div> 
                               
                        </div>
                     </div> <!-- /.box-footer --> 
                
                         <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                              <asp:Label ID="lblt_FileAtth" runat="server"  CssClass="control-label text-bold"   Text="File Attachments"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                       </div>

                                     


                                    </div>
                                  </div>   
                         

                            <div class="box-body">
                               <div class="row">
                                     <div class="col-sm-12 "><!--Grid-->

                                             <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Office2010Blue">
                                              </telerik:RadWindowManager>
                                               
                                                 <telerik:RadGrid ID="grd_archivos" 
                                                     runat="server" 
                                                     CellSpacing="0" GridLines="None" 
                                                     Width="98%" 
                                                     AllowAutomaticDeletes="True" 
                                                     AutoGenerateColumns="False" 
                                                     Skin="Office2010Blue" >
                                                   
                                                      <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                                                        <WebServiceSettings>
                                                            <ODataSettings InitialContainerName="">
                                                            </ODataSettings>
                                                        </WebServiceSettings>
                                                    </HeaderContextMenu>
                                                <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
                                                     <Selecting AllowRowSelect="True"></Selecting>
                                                 </ClientSettings>
                                                <MasterTableView
                                                    DataKeyNames="id_archivo_ambiental" >
                                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                                        Visible="True">
                                                    </RowIndicatorColumn>
                                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                                        Visible="True">
                                                    </ExpandCollapseColumn>
                                                    <Columns>
                                                       <telerik:GridTemplateColumn FilterControlAltText="Filter column1 column" UniqueName="colm_ImageDownload" HeaderButtonType="PushButton" Visible="false" >
                                                           <ItemTemplate>
                                                               <asp:HyperLink ID="ImageDownload" runat="server">[ImageDownload]</asp:HyperLink>
                                                           </ItemTemplate>
                                                            <HeaderStyle Width="5px" />
                                                            <ItemStyle Width="1%"  />
                                                      </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="archivo" 
                                                            FilterControlAltText="Filter archivo column" 
                                                            HeaderText="Attachments" 
                                                            SortExpression="archivo" UniqueName="colm_archivos">
                                                            <ItemStyle Width="30%"  />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="nombre_documento" 
                                                            FilterControlAltText="Filter nombre_documento column" HeaderText="File Type" 
                                                            SortExpression="nombre_documento" UniqueName="colm_nombre_documento">
                                                        </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn DataField="ver" 
                                                            FilterControlAltText="Filter ver column" HeaderText="Rev" 
                                                            SortExpression="ver" UniqueName="colm_ver" Visible="true" Display="false"> 
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="usuario" 
                                                            FilterControlAltText="Filter usuario column" HeaderText="User" 
                                                            SortExpression="usuario" UniqueName="colm_usuario">
                                                            <ItemStyle Width="20%"  />
                                                        </telerik:GridBoundColumn>  
                                                        <telerik:GridBoundColumn DataField="fecha_creado" 
                                                            FilterControlAltText="Filter fecha_creado column" HeaderText="Date Created" 
                                                            SortExpression="fecha_creado" UniqueName="colm_usuario">
                                                        </telerik:GridBoundColumn>                                                                                                               
                                                        <telerik:GridBoundColumn DataField="id_archivo_ambiental" 
                                                            FilterControlAltText="Filter id_archivo_ambiental column" SortExpression="id_archivo" 
                                                            UniqueName="id_archivo_ambiental" Visible="True" Display="False" >
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="id_doc_soporte" 
                                                            FilterControlAltText="Filter id_doc_soporte column" 
                                                            SortExpression="id_doc_soporte" UniqueName="id_doc_soporte" Visible="True" Display="False">
                                                        </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn DataField="id_documento_ambiental" 
                                                            FilterControlAltText="Filter id_documento_ambiental column" 
                                                            SortExpression="id_documento_ambiental" UniqueName="id_documento_ambiental" Visible="True" Display="False">
                                                        </telerik:GridBoundColumn>                                                       
                                                          <telerik:GridButtonColumn 
                                                              ConfirmText="Are you sure to remove this document?" 
                                                              ConfirmDialogType="RadWindow"
                                                              ConfirmTitle="Remove Document" ButtonType="ImageButton" 
                                                              CommandName="Delete" 
                                                              ConfirmDialogHeight="100px"
                                                              ConfirmDialogWidth="400px" 
                                                              UniqueName="colm_ELIMINAR" 
                                                              ImageUrl="../Imagenes/iconos/b_drop.png" Visible="false" >
                                                          <ItemStyle Width="10px"  />
                                                          </telerik:GridButtonColumn>
                                                     </Columns>
                                                    <EditFormSettings>
                                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                        </EditColumn>
                                                    </EditFormSettings>
                                                </MasterTableView>
                                                <FilterMenu EnableImageSprites="False">
                                                    <WebServiceSettings>
                                                        <ODataSettings InitialContainerName="">
                                                        </ODataSettings>
                                                    </WebServiceSettings>
                                                </FilterMenu>
                                            </telerik:RadGrid>
                                                                                     
                                       
                                        </div><!--Grid-->
                               </div>
                            </div>
                     
                     <div class="box-body">
                       <div class="form-group row">
                         <div class="col-sm-2 text-left">
                           
                                    </div>
                                    <div class="col-sm-8">
                                          <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:HiddenField runat="server" ID="lbl_archivo_uploaded" />
                                        <asp:HiddenField runat="server" ID="lbl_hasFiles" />
                                        <asp:HiddenField runat="server" ID="lbl_oldFile" />

                                        <script>

                                            function changeUpload(fileUploaded)
                                            {

                                                document.getElementById("<%= lbl_archivo_uploaded.ClientID%>").value = fileUploaded;
                                                //var img = document.getElementById("<= imgUser.ClientID%>");
                                                //document.getElementById("<= imgUser.ClientID%>").src = "../Temp/" + fileUploaded;
                                            }

                                            function hasFiles(valor)
                                            {
                                                document.getElementById("<%= lbl_hasFiles.ClientID%>").value = valor;
                                            }
                                        </script>                                        
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                 
                                    </div>
                                </div>
                             </div> <!-- /.box-footer --> 

                   </div>

                        <div class="box-footer">        

                              <div class="form-group row">
                                  
                                    <div class="col-sm-2 text-center  ">    
                                       <telerik:RadButton ID="btn_save" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Save" CssClass="btn btn-sm pull-left margin-r-5" Enabled="false"
                                       Width="100px" ValidationGroup="1">
                                       </telerik:RadButton>
                                    </div>
                                   <div class="col-sm-4 text-center  ">                                                                                     
                                       <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                       <telerik:RadButton ID="btn_cancel" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Cancel" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-left" Enabled="false">
                                       </telerik:RadButton>
                                       <asp:Label ID="lblt_Error_Save" runat="server" Text="**  Field required" ForeColor="Red"  CssClass="btn btn-sm pull-left" Visible="false" ></asp:Label>
                                   </div>
                                  <div class="col-sm-3 text-left">
                                        <asp:Button ID="btn_Approved" runat="server" Text="APPROVED"  OnClick="btn_Approved_Click"  OnClientClick="  this.disabled = true; this.value = 'Processing...';"   Width="100px" UseSubmitBehavior="false"  CssClass="btn-sm btn-primary" Enabled="false"   />                             
                                  </div>
                                                 
                               </div>

                        </div>

                  
            </div>
                
         </div>  

     </div>

  </section>            

        
    </asp:Content>
