<%@ Page Language="VB" MasterPageFile="~/Site.Master"   AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_DocAprobacionTest" Codebehind="frm_DocAprobacionTest.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
       
       <link rel="stylesheet" type="text/css" href="../Content/Upload_styles.css" />
        
       <style> 
           
         .FileWindows-modal .modal {
            position: absolute ;
            top: -30px;
            bottom: auto;
            right: auto;
            left: auto;
            display: block;
            z-index: 1;
          }

          .FileWindows-modal .modal {
            background: transparent !important;
          }

    </style>
                                      
             <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">APPROVAL</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Approval in process</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                       
                            <div class="col-lg-12">    
                                                                  
                                   <div class="box-body"> <!--div 0 lg-12-->
                                     
                                          <div class="form-group row">
                                                     <div class="col-sm-4 text-left">
                                                             <!--Tittle -->
                                                                <asp:Label ID="lblIDocumento" runat="server"   Visible="False"></asp:Label>
                                                                <asp:Label ID="lblTipoDoc"    runat  ="server" Visible="False"></asp:Label>
                                                                <asp:Label ID="lblnextruta"  runat ="server"   Visible="False"></asp:Label>
                                                                <asp:Label ID="lblNextRole"  runat ="server"    Visible="False"></asp:Label>
                                                                <asp:Label ID="lblNextUserID"  runat ="server"    Visible="False"></asp:Label>
                                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                <asp:HiddenField ID="sourceADRESS" runat="server" Value="0" />
                                                                <asp:HiddenField ID="destinationADRESS" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hd_ROL" runat="server" />
                                                     </div>
                                                     <div class="col-sm-8 text-right">
                                                               <!--Control -->
                                                                <asp:Label ID="lbl_IdCodigoAPP" runat="server" CssClass="control-label text-bold"></asp:Label>
                                                                <asp:Label ID="lbl_categoria1" runat="server"  CssClass="control-label text-bold">/</asp:Label>
                                                                <asp:Label ID="lbl_IdCodigoSAP" runat="server" CssClass="control-label text-bold"></asp:Label>
                                                     </div>
                                         </div>
                                      </div> <!--div 0 lg-12-->                                                                                                           
                                
                                     <div class="box-body"> <!--div 1 lg-12-->

                                        <div class="row">  <!--box Right --><!--box left -->
                                                                                        
                                          
                                                  <div class="col-sm-12 text-left">
                                                       <!--box top --><!--Grid-->

                                                           <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" 
                                                               AllowAutomaticUpdates ="True" AutoGenerateColumns="False" CellSpacing="0"
                                                                GridLines="None" Width="100%" ShowHeader="False">
                                                                      <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Sunset">
                                                                        <WebServiceSettings>
                                                                        <ODataSettings InitialContainerName=""></ODataSettings>
                                                                        </WebServiceSettings>
                                                                        </HeaderContextMenu>
                                                                    <MasterTableView >
                                                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                                        </RowIndicatorColumn>
                                                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                                        </ExpandCollapseColumn>
                                                                        <Columns>
                                                                                <telerik:GridBoundColumn DataField="orden" DataType="System.Int32" FilterControlAltText="Filter orden column"
                                                                                    HeaderText="Id" SortExpression="orden" UniqueName="orden">
                                                                                </telerik:GridBoundColumn>
                                                                                <telerik:GridBoundColumn DataField="nombre_rol" FilterControlAltText="Filter nombre_rol column"
                                                                                    HeaderText="Unit" SortExpression="nombre_rol" UniqueName="nombre_rol">
                                                                                </telerik:GridBoundColumn>
                                                                                <telerik:GridBoundColumn DataField="nombre_empleado" FilterControlAltText="Filter nombre_empleado column"
                                                                                    HeaderText="User" SortExpression="nombre_empleado" UniqueName="nombre_empleado">
                                                                                </telerik:GridBoundColumn>
                                                                                <telerik:GridBoundColumn DataField="descripcion_estado" FilterControlAltText="Filter descripcion_estado column"
                                                                                    HeaderText="State" SortExpression="descripcion_estado" UniqueName="descripcion_estado">
                                                                                </telerik:GridBoundColumn>
                                                                                <telerik:GridBoundColumn DataField="fecha_aprobacion" 
                                                                                        FilterControlAltText="Filter fecha_aprobacion column" 
                                                                                        HeaderText="Date of approval" SortExpression="fecha_aprobacion" 
                                                                                        UniqueName="fecha_aprobacion">
                                                                                    </telerik:GridBoundColumn>
        
                                                                                <telerik:GridBoundColumn DataField="Alerta" FilterControlAltText="Filter Alerta column"
                                                                                    HeaderText="Alert" SortExpression="Alerta" UniqueName="Alerta" Visible="true" Display="false">
                                                                                </telerik:GridBoundColumn>
                                                                                <telerik:GridTemplateColumn UniqueName="CompletoC" FilterControlAltText="Filter Completo column">
                                                                                    <ItemTemplate>
                                                                                        <asp:HyperLink ID="Completo" runat="server" ImageUrl="~/Imagenes/Iconos/adjunto.png"
                                                                                            ToolTip="Indicador Incompleto">
                                                                                    </asp:HyperLink>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>
                                                                            </Columns>
                                                                            <EditFormSettings>
                                                                                <EditColumn FilterControlAltText="Filter EditCommandColumn column" UniqueName="EditCommandColumn1">
                                                                                </EditColumn>
                                                                            </EditFormSettings>
                                                                        </MasterTableView>
                                                                        <ClientSettings AllowDragToGroup="True" EnableRowHoverStyle="True">
                                                                            <Selecting AllowRowSelect="True" />
                                                                        </ClientSettings>
                                                                        <FilterMenu EnableImageSprites="False">
                                                                            <WebServiceSettings>
                                                                            <ODataSettings InitialContainerName=""></ODataSettings>
                                                                            </WebServiceSettings>
                                                                           </FilterMenu>
                                                                       </telerik:RadGrid>

                                                                         <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                                                         ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                                         SelectCommand="SELECT * FROM dbo.FN_Ta_RutaSeguimiento(11) " 
                                                                         UpdateCommand="UPDATE ta_categoria SET descripcion_cat = @descripcion_cat WHERE (id_categoria = @id_categoria)">
                                                                         <UpdateParameters>
                                                                             <asp:Parameter Name="descripcion_cat" />
                                                                             <asp:Parameter Name="id_categoria" />
                                                                         </UpdateParameters>
                                                                     </asp:SqlDataSource>
                                                      

                                                       <!--box left --><!--Grid-->
                                                      <hr />
                                                   </div>        
                                                                      
                                             </div>  <!--class="row"--><!--box Right --><!--box left -->
                                                
                                         
                                         <div class =" row">

                                               <div class="col-sm-10 text-left ">
                                                    <!--box Right --><!--CONTROLS-->
                                            

                                                                 <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                         <asp:Label ID="lblt_category" runat="server"  CssClass="control-label text-bold" Text="Name of Category"></asp:Label>
                                                                    </div>
                                                                   <div class="col-sm-5">
                                                                       <!--Control -->
                                                                          <asp:Label ID="lbl_categoria" runat="server" ></asp:Label>
                                                                   </div>
                                                                </div>


                                                                 <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                     <asp:Label ID="lblt_approval" runat="server"  CssClass="control-label text-bold" Text="Approvals"></asp:Label>                                
                   
                                                                    </div>
                                                                   <div class="col-sm-5">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_aprobacion" runat="server" ></asp:Label>
                                                                   </div>
                                                                 </div>

                                                                 <div class=" row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lblt_Level" runat="server" CssClass="control-label text-bold" Text="Level required"></asp:Label>

                                                                    </div>
                                                                   <div class="col-sm-5">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_nivelaprobacion" runat="server" ></asp:Label>
                                                                   </div>
                                                                 </div>

                                            
                                                                 <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                          <asp:Label ID="lblt_owner" runat="server"  CssClass="control-label text-bold"  Text="Actual owner"></asp:Label></div>
                                                                   <div class="col-sm-5">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_owner" runat="server" ></asp:Label>
                                                                   </div>
                                                                 </div>

                                                                  <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                        <asp:Label ID="lblt_NextApp" runat="server"  CssClass="control-label text-bold" Text="Next Approver"></asp:Label>
                 
                                                                    </div>
                                                                   <div class="col-sm-5">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_aprueba" runat="server" ></asp:Label>
                                                                   </div>
                                                                 </div>



                                                                 <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->
                                                                       <asp:Label ID="lblt_condition" runat="server"  CssClass="control-label text-bold" Text="Condition"></asp:Label>                                                        
                                                                    </div>
                                                                   <div class="col-sm-5">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_condicion" runat="server" ></asp:Label>
                                                                   </div>
                                                                 </div>


                                                            <div class="row">
                                                                <div class="col-sm-4 text-left">
                                                                 <!--Tittle -->
                                                                  <asp:Label ID="lblt_proccess_name" runat="server" CssClass="control-label text-bold" Text="Name of Process"></asp:Label> 
                        
                                                                </div>
                                                               <div class="col-sm-5">
                                                                   <!--Control -->
                                                                   <asp:Label ID="lbl_proceso" runat="server" ></asp:Label>
                                                               </div>
                                                                <div class="col-sm-2">
                                                                   <!--Control -->
                                                                   <div runat="server" id="ToolsViewer" visible="false" ><a href="#" target="_blank" runat="server" id="hrefVIEWER" class="btn btn-sm btn-success">TimeSheet&nbsp;&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-search"></span></a></div>
                                                               </div>
                                                             </div>                                                                    

                                                            <div class="row">
                                                                <div class="col-sm-4 text-left">
                                                                 <!--Tittle -->                                                    
                                                                    <asp:Label ID="lblt_proccess_code" runat="server"   CssClass="control-label text-bold" Text="Sequence"></asp:Label>                    
                                                                </div>
                                                               <div class="col-sm-5">
                                                                   <!--Control -->
                                                                   <asp:Label ID="lbl_codigo" runat="server" ></asp:Label>
                                                               </div>
                                                             </div>                                                                      
                                                                                
                                        
                                                             <div class="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->  
                                                                          <asp:Label ID="lblt_instrument_number" runat="server"  CssClass="control-label text-bold"  Text="Internal Code"></asp:Label>                                                  
                                                                    
                                                                    </div>
                                                                   <div class="col-sm-5">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_instrumento" runat="server" ></asp:Label>
                                                   
                                                                   </div>
                                                                 </div>
                                          
                                        

                                                              <div cl
                                                                  ass="row">
                                                                    <div class="col-sm-4 text-left">
                                                                     <!--Tittle -->                                                    
                                                                           <asp:Label ID="lblt_benficiary_NAme" runat="server"  CssClass="control-label text-bold" Text="In Reference to"></asp:Label>                                                        
                                                                    
                                                                    </div>
                                                                   <div class="col-sm-5">
                                                                       <!--Control -->
                                                                       <asp:Label ID="lbl_beneficiario" runat="server" ></asp:Label>
                                                   
                                                                   </div>
                                                                 </div>
                                                                                      
                                                    <!--box Right -->
                                                  </div> 


                                         </div>                                
                                              
                                           


                                     
                                    </div> <!--class="box-body"-->  <!--div 1 lg-12-->
                                
                                   <div class="box-body">
                                    <div class=" row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->                                           
                                            <asp:Label ID="lblbt_comment" runat="server"  CssClass="control-label text-bold" Text="Comment"></asp:Label>
                                        </div>
                                       <div class="col-sm-10">
                                           <!--Control -->
                                           <asp:Label ID="lbl_Comment" runat="server" CssClass="text-justify "  Width="90%"></asp:Label>
                                       </div>
                                    </div>
                                  </div>
                                         
                               

                                 
                                   
                               <div class="box-body">
                                    <div class=" row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lbl_documentsAPP" runat="server" CssClass="control-label text-bold"   Text="Documents Approvals"></asp:Label>
                                        </div>
                                       <div class="col-sm-10">
                                           <!--Control -->

                                             <asp:ImageButton ID="ImageButton1" runat="server" 
                                                    ImageUrl="~/Imagenes/Iconos/updateico.png" style="width: 16px" />
                                             
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

                                              <asp:HiddenField ID="hd_id_doc" runat="server" Value="0" />
                                           
                                           <br />
                                           <asp:Label ID="lbl_errExtension" runat="server" Font-Names="Arial" Font-Size="Small" ForeColor="red" Visible="false" Text="Select a type of document"></asp:Label> 
                                           <br />
                        
                                       </div>
                                    </div>
                                  </div>




                                 <div class="box-body">
                                    <div class=" row">

                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_addFile" runat="server"  CssClass="control-label text-bold" Text="Other File"></asp:Label>

                                        </div>
                                       <div class="col-sm-10 text-left qsf-demo-canvas">
                                           <!--Control  -->
                                          <%--HttpHandlerUrl="~/FileUploadTelerik/UploadImageHandler.ashx"    --%>
                                           <%--OnClientFileUploadRemoving="APP_fileUploadRemoving"  --%>

                                               <telerik:RadAsyncUpload 
                                                    runat="server" 
                                                    RenderMode="Lightweight" 
                                                    ID="uploadFile"
                                                    Skin="Silk" 
                                                    TemporaryFolder="~/Temp" 
                                                    TargetFolder="~/Temp" 
                                                    HttpHandlerUrl="~/Controles/FileHandler/AppFileHandler.ashx"
                                                    OnClientValidationFailed="validationFailed"  
                                                    OnClientFileUploadRemoving="APP_fileUploadRemoving"                                                
                                                    OnClientFileUploaded="APP_fileUploaded"                                                                                                                                                                                                                                
                                                    MaxFileInputsCount="1"
                                                    MultipleFileSelection="Disabled" 
                                                    AllowedFileExtensions="jpeg,jpg,gif,png,bmp"    
                                                    MaxFileSize="3076000"                                              
                                                    PostbackTriggers="">
                                                </telerik:RadAsyncUpload>
                                                                                  
                                            <script src="../Scripts/FileUploadTelerik.js"></script>
                                            <script type="text/javascript">

                                                Sys.Application.add_load(function () {
                                                    FileHandler.initialize();
                                                });
                                        

                                                 function changeUpload(fileUploaded) {

                                                   document.getElementById("<%=lbl_archivo_uploaded.ClientID%>").value = fileUploaded;
                                                   <%--  $('#<%=lbl_archivo_uploaded.ClientID%>').val(fileUploaded);--%>
                                                   
                                                     <%-- var img = document.getElementById("<%= imgUser.ClientID%>");--%>
                                                      //img.className = "hidden";
                                                     <%-- document.getElementById("<%= imgUser.ClientID%>").src = "../Temp/" + fileUploaded;--%>

                                                     alert('stored:  ' + fileUploaded + ' as ' + document.getElementById("<%=lbl_archivo_uploaded.ClientID%>").value);

                                                 }


                                                    function showElements(boolShow){
                                               
                                                        pnFirma = "#<%= Panel1_firma.ClientID %>";
                                                        //alert(pnFirma);

                                                        if (boolShow) {
                                                            $(pnFirma).css('display','block');                                                
                                                        }else{
                                                            $(pnFirma).css('display','none');                                                
                                                        }                                              
                                               
                                                       //$(pnFirma).show();
                                        
                                                    }
                                                
                                                    function removeElement(Idx) {

                                                         var upload = $find("<%= uploadFile.ClientID %>");
                                                          //var inputs = upload.getUploadedFiles();
                                                          //for (i = inputs.length - 1; i >= 0;i--) {
                                                          //  if(!upload.isExtensionValid(inputs[i].value))
                                                          //     upload.deleteFileInputAt(i);
                                                        upload.deleteFileInputAt(0);
                                                        showElements(false);

                                                    }

                                                    function addElement(Idx){
                                                
                                                        var upload = $find("<%=uploadFile.ClientID %>");                                                        
                                                        var inputs = upload.getUploadedFiles();
                                                        var nwFile =   $("#<%=lbl_archivo_uploaded.ClientID%>").val();

                                                        alert(inputs[0] + " was  uploaded and renamed to " + nwFile);
                                                        //alert(upload.get_fileInfo().FileNameResult);
                                                        //for (i = inputs.length - 1; i >= 0; i--) {
                                                        //    if (!upload.isExtensionValid(inputs[i].value))
                                                        //        upload.deleteFileInputAt(i);
                                                        //}

                                                        alert('{ FileName:"' + nwFile + '", SourceAddr:"' + document.getElementById("<%=Me.sourceADRESS.ClientID%>").value  + '" , FinalAddr:"' + document.getElementById("<%=Me.destinationADRESS.ClientID%>").value + '" , TypeProcess:"Move" }');
                                                        <%--alert('{ FileName:"' + nwFile + '", ' + 'SourceAddr:"<%=Me.sourceADRESS.Value %>", ' + 'FinalAddr:"<%=Me.destinationADRESS.Value %>", TypeProcess:"Move" }');--%>
                                                        <%--data: '{ FileName:"' + inputs[0] + '", ' + 'idDoc:' + <%=Me.HiddenField1.Value%> +  ', '  +   'idUser:' + <%= Me.Session("E_IdUser")%> +  ', '  + 'idPrograma:"' + <%=Me.Session("E_IDPrograma")%> +'" }',--%>

                                                         $.ajax({
                                                                 type: "POST",
                                                                 url: "../Controles/FileHandler/AppFileProcess.ashx",
                                                                 data: '{ FileName:"' + nwFile + '", SourceAddr:"' + document.getElementById("<%=Me.sourceADRESS.ClientID%>").value  + '" , FinalAddr:"' + document.getElementById("<%=Me.destinationADRESS.ClientID%>").value + '" , TypeProcess:"Move" }',
                                                                 //contentType: "application/json; charset=utf-8",
                                                                 dataType: "json",
                                                                 success: function (data) {                                                       
                                                                     var items = data.d || data;
                                                                     alert(items);
                                                                     dataResult = jQuery.parseJSON(items);
                                                                     alert(dataResult);

                                                                     //for (var i = 0; i < data.length; i++) { }
                                                                     var itemR = dataResult[0];
                                                                     //item.Text
                                                                     //item.Mensaje
                                                                     alert('Successfully: ' + itemR.Mensaje);
                                                                 },
                                                                 failure: function (data) {
                                                                     var items = data.d || data;
                                                                     dataResult = jQuery.parseJSON(items);
                                                                     var itemR = dataResult[0];
                                                                     alert('Error: ' + itemR.Mensaje);
                                                                 }   
                                                         });
                                                        
                                                        showElements(false);
                                                                                                
                                                
                                                    }


                                                    function hasFiles(valor) {
                                                        document.getElementById("<%= lbl_hasFiles.ClientID%>").value = valor;
                                                    }

                                                    function print(input) {
                                                        alert(input);
                                                    }

                                    </script>
                                    <%-- <div class="qsf-results">
                                           
                                        <telerik:RadButton RenderMode="Lightweight" Skin="Silk" runat="server" ID="BtnSubmit" Text="Validate the uploaded files" ></telerik:RadButton>  
                                            <asp:Panel ID="ValidFiles" Visible="false" runat="server" CssClass="qsf-success">
                                                <h3>You successfully uploaded:</h3>
                                                <ul class="qsf-list" runat="server" id="ValidFilesList"></ul>
                                            </asp:Panel> 
                                            <asp:Panel ID="InvalidFiles" Visible="false" runat="server" CssClass="qsf-error">
                                                <h3>The Upload failed for:</h3>
                                                <ul class="qsf-list ruError" runat="server" id="InValidFilesList">
                                                    <li>
                                                        <p class="ruErrorMessage">The size of your overall upload exceeded the maximum of 1 MB</p>
                                                    </li>
                                                </ul> 
                                            </asp:Panel>
                                            <telerik:RadButton RenderMode="Lightweight" Skin="Silk" ID="RefreshButton" runat="server" OnClick="RefreshButton_Click" Visible="false" Text="Back" ></telerik:RadButton>
                                        </div>--%>
                                                                                  
                                         <div runat="server" ID="Panel1_firma"  class="FileWindows-modal" style="display:none;">
                                                
                                                 <asp:HiddenField runat="server" ID="lbl_archivo_uploaded" />
                                                 <asp:HiddenField runat="server" ID="lbl_hasFiles" />
                                                 <asp:HiddenField runat="server" ID="lbl_oldFile" />                                                                                 
                                     
                                                    <div class="modal modal-info">
                                                      <div class="modal-dialog">
                                                        <div class="modal-content">
                                                          <div class="modal-header">
                                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="removeElement(0);" ><span aria-hidden="true">&times;</span></button>
                                                            <h4 class="modal-title"><span class="fa fa-2x fa-cloud-upload"></span>&nbsp;&nbsp;Document Uploaded Successfully</h4>
                                                          </div>
                                                          <div class="modal-body">
                                                            <p class="text">Do you want attached this document to the approval process&hellip;<span class="fa fa-question-circle"></span></p>
                                                          </div>
                                                          <div class="modal-footer">
                                                            <button type="button" class="btn btn-outline pull-left" data-dismiss="modal" onclick="removeElement(0);" >Cancel</button>
                                                            <button type="button" class="btn btn-outline" onclick="addElement(0);">Attach</button>
                                                          </div>
                                                        </div><!-- /.modal-content -->
                                                      </div><!-- /.modal-dialog -->
                                                    </div><!-- /.modal -->

                                            </div><!-- /.FileWindows-modal -->                                           
                                          <div class="qsf-decoration"></div>

                                       </div>
                                    </div>
                                  </div>

                                 
                                   <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                             <asp:Label ID="lblt_listName" runat="server" CssClass="control-label text-bold"   Text="Files List"></asp:Label>
                                        </div>
                                       <div class="col-sm-10">
                                           <!--Control -->
                                           <asp:ListBox ID="ListBox_file" runat="server" Width="70%"></asp:ListBox>
                                            <telerik:RadButton ID="btn_remove" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Remove selected file" Width="150px"  CssClass="btn-sm btn-info"></telerik:RadButton>
                                       </div>
                                    </div>
                                  </div>

                                 
                                  <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                              <asp:Label ID="lblt_FileAtth" runat="server"  CssClass="control-label text-bold"   Text="File Attachments"></asp:Label>
                                        </div>
                                       <div class="col-sm-10">
                                           <!--Control -->
                                       </div>

                                         <div class="col-sm-12 "><!--Grid-->

                                               
                                                 <telerik:RadGrid ID="grd_archivos" runat="server" CellSpacing="0" GridLines="None" 
                                                     Width="95%" AllowAutomaticDeletes="True" AutoGenerateColumns="False" Skin="Office2010Blue" >
                                                   
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
                                                    DataKeyNames="id_archivo" >
                                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                                        Visible="True">
                                                    </RowIndicatorColumn>
                                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                                        Visible="True">
                                                    </ExpandCollapseColumn>
                                                    <Columns>
                                                       <telerik:GridTemplateColumn FilterControlAltText="Filter column1 column" UniqueName="ImageDownload" HeaderButtonType="PushButton" Visible="True" Display="True"  >
                                                           <ItemTemplate>
                                                               <asp:HyperLink ID="ImageDownload" runat="server">[ImageDownload]</asp:HyperLink>
                                                           </ItemTemplate>
                                                            <HeaderStyle Width="5px" />
                                                            <ItemStyle Width="5px"  />
                                                      </telerik:GridTemplateColumn>
                                                         <telerik:GridBoundColumn DataField="nombre_documento" 
                                                            FilterControlAltText="Filter nombre_documento column" 
                                                            HeaderText="Name of Document" 
                                                            SortExpression="nombre_documento" UniqueName="nombre_documento" Visible="True" Display="True">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="archivo" 
                                                            FilterControlAltText="Filter ruta_archivos column" 
                                                            HeaderText="Attachments" 
                                                            SortExpression="archivo" UniqueName="ruta_archivos" Visible="True" Display="True">
                                                        </telerik:GridBoundColumn>   
                                                         <telerik:GridBoundColumn DataField="ver" 
                                                            FilterControlAltText="Filter ver column" HeaderText="Rev" 
                                                            SortExpression="ver" UniqueName="ver"  Visible="True" Display="True">
                                                        </telerik:GridBoundColumn>                                                                                                             
                                                        <telerik:GridBoundColumn DataField="nombre_rol" 
                                                            FilterControlAltText="Filter nombre_rol column" HeaderText="Rol Rev" 
                                                            SortExpression="nombre_rol" UniqueName="nombre_rol" Visible="True" Display="True">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn FilterControlAltText="Filter fileControl column" 
                                                            UniqueName="fileControl" >
                                                            <ItemTemplate>
                                                                <telerik:RadAsyncUpload ID="RadAsyncUpload1" 
                                                                    runat="server"   
                                                                    OnFileUploaded="FileUploaded_Chg_Name"
                                                                    MaxFileInputsCount="1" 
                                                                    TargetFolder="~/FileUploads/ApprovalProcc"  
                                                                    CssClass=" btn-sm btn-default "
                                                                    Width="250px">
                                                                </telerik:RadAsyncUpload>                                                              
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="extension" 
                                                            FilterControlAltText="Filter extension column" SortExpression="extension" 
                                                            UniqueName="extension" Visible="True" Display="False" >
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="id_archivo" 
                                                            FilterControlAltText="Filter id_archivo column" SortExpression="id_archivo" 
                                                            UniqueName="id_archivo" Visible="True" Display="False" >
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="id_doc_soporte" 
                                                            FilterControlAltText="Filter id_doc_soporte column" 
                                                            SortExpression="id_doc_soporte" UniqueName="id_doc_soporte" Visible="True" Display="False">
                                                        </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn DataField="id_App_Documento" 
                                                            FilterControlAltText="Filter id_App_Documento column" 
                                                            SortExpression="id_App_Documento" UniqueName="id_App_Documento" Visible="True" Display="False">
                                                        </telerik:GridBoundColumn>
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

                                                <asp:SqlDataSource ID="archivos_temp" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                ConflictDetection="CompareAllValues" 
                                                OldValuesParameterFormatString="original_{0}" 
                                                
                                                    SelectCommand="SELECT ta_AppDocumento.id_documento, ta_AppDocumento.id_ruta, ta_AppDocumento.id_App_Documento, ta_docs_soporte.id_doc_soporte, ta_archivos_documento.id_archivo, ta_archivos_documento.archivo, ta_docs_soporte.nombre_documento, ta_docs_soporte.extension FROM ta_archivos_documento INNER JOIN ta_AppDocumento ON ta_archivos_documento.id_App_Documento = ta_AppDocumento.id_App_Documento INNER JOIN ta_docs_soporte ON ta_archivos_documento.id_doc_soporte = ta_docs_soporte.id_doc_soporte WHERE (ta_AppDocumento.id_documento = @id_doc) AND (ta_AppDocumento.id_App_Documento = @id_appDoc)" 
                                                    ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter DefaultValue="-1" Name="id_doc" 
                                                        QueryStringField="IdDoc" />
                                                    <asp:ControlParameter ControlID="lblIDocumento" DefaultValue="-1" 
                                                        Name="id_appDoc" PropertyName="Text" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>             
                                                                                      
                                       
                                        </div><!--Grid-->


                                    </div>
                                  </div>

                                   <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-4 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_writcomments" runat="server"  CssClass="control-label text-bold"  Text="Write your Comments"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                       </div>
                                        <div class="col-sm-12">
                                                <telerik:RadTextBox ID="txtcoments" Runat="server" Height="100px"  TextMode="MultiLine" Width="85%">
                                                </telerik:RadTextBox>                                                 
                                            <br />

                                         </div>
                                    </div>
                                  </div>


                                  <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-12 text-left form-group-sm  ">
                                          <!--Buttoms -->
                                              

                                       </div>
                                    </div>
                                  </div>
                                
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-12 text-center">                                              
                                                <asp:Label ID="lblerr_user" runat="server" Font-Names="Arial" ForeColor=Red Visible="true"></asp:Label>
                                            <br /><br />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-3 text-center  ">                                           
                                          <!--Buttoms -->
                                            <asp:Button ID="btn_Approved" runat="server" Text="APPROVED"  OnClick="btn_Approved_Click"  OnClientClick="  this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"  CssClass="btn-lg btn-primary" Width="65%" />
                                            <asp:Button ID="btn_Completed" runat="server" Text="APPROVED" OnClick="btn_Completed_Click"  OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-info" /> 
                                       </div>
                                        <div class="col-sm-3 text-center  ">
                                          <!--Buttoms -->
                                            <asp:Button ID="btn_STandBy" runat="server" Text="STAND BY" OnClick="btn_STandBy_Click"   OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-warning"  Width="65%" />                                            
                                       </div>
                                        <div class="col-sm-3 text-center  ">
                                          <!--Buttoms -->
                                              <asp:Button ID="btn_NotApproved" runat="server" Text="NOT APPROVED"  OnClick="btn_NotApproved_Click"   OnClientClick="this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"   CssClass="btn-lg btn-danger"  Width="65%" />                                                                                        
                                       </div>

                                        <div class="col-sm-3 text-center  ">
                                          <!--Buttoms -->
                                            <asp:Button ID="btn_Cancelled" runat="server" Text="CANCELLED"  OnClick="btn_Cancelled_Click"  OnClientClick=" this.disabled = true; this.value = 'Processing...';"    UseSubmitBehavior="false"   CssClass="btn-lg btn-danger" Width="65%" />                                                  
                                       </div>

                                    </div>
                                  </div>


                               </div> <!--col-lg-12-->
                                       
                        </div>

                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->

                        <div class="col-sm-12 text-center  ">
                             
                            <br />                            
                            <asp:Button ID="btn_return" runat="server" Text="RETURN WITHOUT SAVING"  OnClick="btn_return_Click"  OnClientClick="this.disabled = true; this.value = 'Processing...';"   UseSubmitBehavior="false"   CssClass="btn btn-default" />                                                  
                            
                            </div>
                           <asp:Label ID="lbl_fileUpload" runat="server"></asp:Label>   
                           
                                     
                   </div>

                </div>
           </section>    
         
      
    </asp:Content>

