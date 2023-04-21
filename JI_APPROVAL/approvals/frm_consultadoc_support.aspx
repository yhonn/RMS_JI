<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_consultadoc_support"  Codebehind="frm_consultadoc_support.aspx.vb" %>



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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Documents</asp:Label>
                </h3>
            </div>
            <div class="box-body">
                        <telerik:RadTextBox ID="txt_doc" runat="server"
                            EmptyMessage="Type document name here..." LabelWidth="" Width="395px"
                            ValidationGroup="1">
                               <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                        </telerik:RadTextBox>
                        <telerik:RadButton ID="btn_buscar" runat="server" SingleClick="true" SingleClickText="Processing..."  Text="Search" Width="100px" >
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_nuevo" runat="server" SingleClick="true" SingleClickText="Processing..."  Enabled="false" Text="New Document Type" >
                        </telerik:RadButton>
                        <hr />

       
                <telerik:RadGrid ID="grd_cate"  Skin="Office2010Blue"   
                                 runat="server" AllowAutomaticDeletes="True" 
                                 CellSpacing="0" DataSourceID="SqlDataSource2" 
                                 GridLines="None" Width="95%" AllowAutomaticUpdates="True"  
                                 AutoGenerateColumns="False"  EnableViewState="true" >
                          
                            <ClientSettings EnableRowHoverStyle="true" >
                               <Selecting AllowRowSelect="True"></Selecting>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_doc_soporte" DataSourceID="SqlDataSource2">                        

                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">                          
                            </RowIndicatorColumn>

                     

                                <Columns>

                                 <telerik:GridTemplateColumn 
                                      FilterControlAltText="Filter edit column" 
                                      UniqueName="colm_edit" Visible="false">                                      
                                     <ItemTemplate>
                                       <asp:ImageButton 
                                           ID="editar" 
                                           runat="server" 
                                           ImageUrl  ="~/Imagenes/iconos/b_edit.png" 
                                           ToolTip="Edit" />
                                     </ItemTemplate>
                                      <ItemStyle Width="5%"  />
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
                                        <ItemStyle Width="20%" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="extension" 
                                        FilterControlAltText="Filter extension column" HeaderText="Extension" 
                                        SortExpression="extension" UniqueName="extension">
                                        <ItemStyle Width="20%" />
                                    </telerik:GridBoundColumn>

                                     <telerik:GridBoundColumn DataField="max_size" 
                                        FilterControlAltText="Filter max_size column" HeaderText="Max Size (MB)" 
                                        SortExpression="max_size" UniqueName="max_size">                                         
                                         <ItemStyle Width="10%" VerticalAlign="Middle" HorizontalAlign="Center" />
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
                                      <ItemStyle Width="20%"  />
                                   </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn 
                                      FilterControlAltText="Filter colm_env column"  HeaderText="ENVIR" 
                                      UniqueName="colm_env" >                                      
                                     <ItemTemplate>     
                                          <asp:CheckBox ID="chkENVIR" runat="server" Enabled="false" />                                                                                                             
                                     </ItemTemplate>
                                      <ItemStyle Width="2%" VerticalAlign="Middle" HorizontalAlign="Center" />
                                   </telerik:GridTemplateColumn>

                                      <telerik:GridBoundColumn  DataField="environmental" UniqueName="environmental" Visible="true" Display="false">
                                      </telerik:GridBoundColumn>


                                     <telerik:GridTemplateColumn 
                                      FilterControlAltText="Filter colm_env column"  HeaderText="DELIV" 
                                      UniqueName="colm_deliv" >                                      
                                     <ItemTemplate>                                       
                                          <asp:CheckBox ID="chkDELIV" runat="server" Enabled="false" />                                   
                                     </ItemTemplate>
                                       <ItemStyle Width="2%" VerticalAlign="Middle" HorizontalAlign="Center" />
                                   </telerik:GridTemplateColumn>

                                    <telerik:GridBoundColumn  DataField="deliverable" UniqueName="deliverable" Visible="true" Display="false">
                                      </telerik:GridBoundColumn>

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

                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                         ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>" 
                              SelectCommand="SELECT id_doc_soporte, nombre_documento, extension, Template, environmental, deliverable, max_size FROM ta_docs_soporte WHERE id_programa=@id_program">
                                                 <SelectParameters>
                                                     <asp:SessionParameter DefaultValue="-1" Name="id_program" 
                                                         SessionField="E_IDPrograma" />
                                                 </SelectParameters>
                                             </asp:SqlDataSource>
                        
                        


            </div>

         </div>

     </section>
               
    </asp:Content>



