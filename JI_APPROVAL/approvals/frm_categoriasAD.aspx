<%@ Page Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_categoriasAD" Codebehind="frm_categoriasAD.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Categories</asp:Label>
                </h3>
            </div>
               <div class="box-body">
                        <telerik:RadTextBox ID="txt_doc" runat="server"
                            EmptyMessage="Type name here..." LabelWidth="" Width="395px"
                            ValidationGroup="1">
                            <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                        </telerik:RadTextBox>
                        <telerik:RadButton ID="btn_buscar" runat="server" SingleClick="true" SingleClickText="Processing..."  Text="Search" Width="100px" Enabled="false" >
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_nuevo" runat="server"  SingleClick="true" SingleClickText="Processing..."  Text="New Type" Enabled="false" >
                        </telerik:RadButton>
                        <hr />

                      
                        <telerik:RadTextBox ID="txt_cat" Runat="server" 
                            EmptyMessage="Type new type here..." LabelWidth=""  Width="450px" 
                            ValidationGroup="1">
                            <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                            ErrorMessage="Required" ControlToValidate="txt_cat" 
                            ValidationGroup="1"></asp:RequiredFieldValidator>
                        <telerik:RadButton ID="btn_save" runat="server"  Text="Add Type " ValidationGroup="1" Enabled="false">
                        </telerik:RadButton>
                        <asp:Label ID="Label3" runat="server" Text="**  Field required"></asp:Label>

                        <hr />

                     <telerik:RadGrid ID="grd_cate"  
                         Skin="Office2010Blue"  
                         runat="server" 
                         AllowAutomaticDeletes="True" 
                         CellSpacing="0" 
                         DataSourceID="SqlDataSource2" 
                         GridLines="None" 
                         Width="720px" 
                         AllowAutomaticUpdates="True"  
                         AutoGenerateColumns="False">
        <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
        <WebServiceSettings>
        <ODataSettings InitialContainerName=""></ODataSettings>
        </WebServiceSettings>
        </HeaderContextMenu>
<ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
<Selecting AllowRowSelect="True"></Selecting>
        </ClientSettings>
<MasterTableView AutoGenerateColumns="False" DataKeyNames="id_categoria" DataSourceID="SqlDataSource2">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        
        <telerik:GridEditCommandColumn 
            ButtonType="ImageButton" 
            UniqueName ="colm_editar" 
            Visible="false" 
            EditImageUrl="~/Imagenes/iconos/b_edit.png" 
            FilterControlAltText="Filter EditCommandColumn column" 
            UpdateImageUrl="~/Imagenes/b_edit.png" 
            UpdateText="Save" >
            <ItemStyle Width="10px"  />
        </telerik:GridEditCommandColumn>
        
             <telerik:GridTemplateColumn 
                     FilterControlAltText="Filter edit2 column" 
                     UniqueName="colm_edit2" 
                     Visible="false">                                      
                     <ItemTemplate>
                        <asp:ImageButton 
                            ID="editar2" 
                            runat="server" 
                            ImageUrl  ="~/Imagenes/iconos/b_edit.png" 
                            ToolTip="Edit" />
                      </ItemTemplate>
                 <ItemStyle Width="10px" />
               </telerik:GridTemplateColumn>

        <telerik:GridBoundColumn  DataField="id_categoria" DataType="System.Int32" 
            FilterControlAltText="Filter id_categoria column" HeaderText="id_categoria" 
            ReadOnly="True" SortExpression="id_categoria" UniqueName="id_categoria" 
            Visible="true" Display="false">
        </telerik:GridBoundColumn>

        <telerik:GridBoundColumn DataField="descripcion_cat" 
            FilterControlAltText="Filter descripcion_cat column" 
            HeaderText="Name of Category" SortExpression="descripcion_cat" 
            UniqueName="descripcion_cat">
        </telerik:GridBoundColumn>
        
        <telerik:GridTemplateColumn FilterControlAltText="Filter visible column" 
            UniqueName="colm_visible" Visible="false">
            <ItemTemplate>
                <asp:CheckBox ID="chkVisible" runat="server" AutoPostBack="True" 
                    oncheckedchanged="chkVisible_CheckedChanged" />
                <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server" 
                    CheckedImageUrl="~/Imagenes/iconos/Stock_IndexUp.png" ImageHeight="16" ImageWidth="16" 
                    TargetControlID="chkVisible" UncheckedImageUrl="~/Imagenes/iconos/Stock_IndexDown.png">
                </ajaxToolkit:ToggleButtonExtender>
            </ItemTemplate>
            <HeaderStyle Width="10px" />
            <ItemStyle Width="10px" />
        </telerik:GridTemplateColumn>
        
         <telerik:GridBoundColumn DataField="visible" 
            FilterControlAltText="Filter descripcion_cat column" 
            HeaderText="Visible" SortExpression="visible" 
            UniqueName="Cvisible" Visible="true" Display="false">
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
                            
                                                 SelectCommand="SELECT * FROM ta_categoria WHERE id_programa=@id_program" 
                                                 
                                                 UpdateCommand="UPDATE ta_categoria SET descripcion_cat = @descripcion_cat, visible= @visible WHERE (id_categoria = @id_categoria)">

                                                 <SelectParameters>
                                                     <asp:SessionParameter DefaultValue="1" Name="id_program" 
                                                         SessionField="E_IDPrograma" />
                                                 </SelectParameters>

                                                 <UpdateParameters>
                                                     <asp:Parameter Name="descripcion_cat" />
                                                     <asp:Parameter Name="id_categoria" />
                                                     <asp:Parameter Name="visible" />
                                                 </UpdateParameters>

                                             </asp:SqlDataSource>
                    

               </div>

         </div>

     </section>
       
    </asp:Content>



