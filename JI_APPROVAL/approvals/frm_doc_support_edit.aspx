<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_doc_support_edit"  Codebehind="frm_doc_support_edit.aspx.vb" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">

           <section class="content-header">
                <h1>
                    <asp:Label runat="server" ID="lblt_titulo_pantalla">CATALOGS</asp:Label>
                </h1>
            </section>
            <section class="content">
                <div class="box">
                     <div class="box-header with-border">
                        <h3 class="box-title">
                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Edit Document</asp:Label>
                        </h3>
                    </div>
                    <div class="box-body">
                          <div class="box-body">
                            <div class="col-lg-12">
                               <div class="box-body">
                                    <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                              <asp:Label ID="lblt_Nombre_Documento" runat="server" CssClass="control-label text-bold" Text="Name of document"></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                              <telerik:RadTextBox ID="txt_cat" Runat="server" EmptyMessage="Type name here.." LabelWidth="" Width="450px"  ValidationGroup="1">
                                                <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                                              </telerik:RadTextBox>
                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                                                    ErrorMessage="Required" ControlToValidate="txt_cat" 
                                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                              <asp:HiddenField ID="HiddenField1" runat="server" />
                                       </div>
                                    </div>

                                   <div class="form-group row">
                                        <div class="col-sm-2 text-left">
                                         <!--Tittle -->
                                            <asp:Label ID="lblt_Extencion" runat="server" CssClass="control-label text-bold" Text="Permitted File Extensions" ></asp:Label>
                                        </div>
                                       <div class="col-sm-8">
                                           <!--Control -->
                                            <asp:CheckBoxList ID="chk_ext" runat="server" RepeatColumns="3" Width="185px" 
                                                DataSourceID="SqlDataSource2" DataTextField="nom_ext" DataValueField="id_ext">
                                            </asp:CheckBoxList>
                                            <asp:Label ID="lbl_errExt" runat="server" ForeColor="Red" Text="Required" 
                                                Visible="False"></asp:Label>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                                                ProviderName="<%$ ConnectionStrings:dbCI_SAPConnectionString.ProviderName %>" 
                                                SelectCommand="SELECT id_ext, nom_ext FROM ta_catalogo_extensiones">
                                            </asp:SqlDataSource>
                                       </div>
                                    </div>


                                  </div>


                                      <div class="box-body">
                                           <div class="form-group row">
                                            <div class="col-sm-2 text-left">
                                               <asp:Label ID="lblt_max_size" runat="server" CssClass="control-label text-bold" Text="Max Size (MB)"></asp:Label>
                                             </div>
                                            <div class="col-sm-4">
                                                 <telerik:RadNumericTextBox Value="2" ID="txt_max_size" Width="50%" runat="server" MinValue="2" MaxValue="50">                                    
                                                </telerik:RadNumericTextBox>
                                            </div>
                                          </div>
                                     </div> <!-- /.box-footer --> 
                                                               
                                            <div class="box-body">
                                                   <div class="form-group row">
                                                        <div class="col-sm-10 text-left">
                                                          <a runat="server" id="hlk_file" href="#">--none--</a>                                                         
                                                         </div>
                                                    </div>
                                           </div>

                                     <div class="box-body">
                                                   <div class="form-group row">
                                                    <div class="col-sm-2 text-left">
                                                       <asp:Label ID="lbl_template" runat="server" CssClass="control-label text-bold" Text="Template"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                                                                                                            
                                                         <telerik:RadAsyncUpload 
                                                            runat="server" 
                                                            RenderMode="Lightweight" 
                                                            ID="uploadFile"
                                                            Skin="Metro" 
                                                            TemporaryFolder="~/Temp" 
                                                            TargetFolder="~/Temp"                                                           
                                                            OnClientFileUploaded="changeUpload"                                                                                                                           
                                                            MaxFileInputsCount="1"
                                                            MultipleFileSelection="Disabled"                                    
                                                            PostbackTriggers="btn_save">
                                                        </telerik:RadAsyncUpload>
                                                       
                                                       
                                                       

                                                    </div>
                                                </div>
                                             </div> <!-- /.box-footer --> 

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

                                                                    function changeUpload(sender, args)
                                                                    {

                                                                        var fileUploaded = args.get_fileName();//args.get_fileInfo().fileNameResult;

                                                                        document.getElementById("<%= hlk_file.ClientID%>").innerHTML = fileUploaded;
                                                                        document.getElementById("<%= hlk_file.ClientID%>").href = '../Temp/' + fileUploaded

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
                                
                                      <div class="box-body">
                                           <div class="form-group row">
                                            <div class="col-sm-5 text-left">
                                               <asp:Label ID="lblt_environmental_docs" runat="server" CssClass="control-label text-bold" Text="Environmental Support"></asp:Label> &nbsp;&nbsp;
                                                <asp:CheckBox ID="chk_environmetal" runat="server" Width="185px" Text="Environmental" >
                                                </asp:CheckBox>  
                                            </div>
                                            <div class="col-sm-5 text-left">                                
                                                <asp:Label ID="lblt_deliverable_supp" runat="server" CssClass="control-label text-bold" Text="Deliverable Support"></asp:Label> &nbsp;&nbsp;              
                                                <asp:CheckBox ID="chk_deliverable" runat="server" Width="185px" Text="deliverable" >
                                                </asp:CheckBox>  
                                            </div>
                                        </div>
                                     </div> <!-- /.box-footer --> 



                               </div> 
              
                          </div>
                    </div>
                    <!-- /.box-footer --> 
                   <div class="box-footer">                            
                        <!--Controls -->
                        
                       <telerik:RadButton ID="btn_save" runat="server"  SingleClick="true" SingleClickText="Processing..." Text="Save" CssClass="btn btn-sm pull-left margin-r-5" Enabled="False" Width="100px"  ValidationGroup="1">
                       </telerik:RadButton>
                        <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                       <telerik:RadButton ID="btn_cancel" runat="server" SingleClick="true" SingleClickText="Processing..."  Text="Cancel" Width="100px" CssClass="btn btn-sm pull-left">
                       </telerik:RadButton>
                       <asp:Label ID="lblt_Error_Save" runat="server" Text="**  Field required" ForeColor="Red"  CssClass="btn btn-sm pull-left" Visible="false" ></asp:Label>
                       <asp:Label ID="lblt_Err" runat="server" Text="**  Field required" ForeColor="Red" Visible="false" CssClass="btn btn-sm pull-left"></asp:Label>
                       
                        
                   </div>

                </div>
           </section>
            
    </asp:Content>
