<%@ Page Title="" Language="VB"MasterPageFile="../MasterSIM.master" AutoEventWireup="false" Inherits="approval.Aprobaciones_frm_consultaContratos" Codebehind="frm_consultaContratos.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td style="width: 99px">
                &nbsp;
            </td>
            <td style="width: 13px">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 99px">
                &nbsp;
                        <asp:Label ID="Label18" runat="server" style="font-weight: 700" 
                            Text="Type approval"></asp:Label>
            </td>
            <td style="width: 13px">
                &nbsp;:</td>
            <td>
                <asp:RadioButtonList ID="rb_tipo" runat="server" AutoPostBack="True" 
                    RepeatColumns="3" Width="600px">
                    <asp:ListItem Selected="True" Value="1">Activities NOT GRANT&#39;s</asp:ListItem>
                    <asp:ListItem Value="2">Activities GRANT&#39;s (Aplicación)</asp:ListItem>
                    <asp:ListItem Value="3">Activities GRANT&#39;s  Compact</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="width: 99px">
                &nbsp;
                        <asp:Label ID="Label19" runat="server" style="font-weight: 700" 
                            Text="Name "></asp:Label>
            </td>
            <td style="width: 13px">
                &nbsp;:</td>
            <td>
                <telerik:RadTextBox ID="txtfind" Runat="server" 
                    EmptyMessage="Type text here..." LabelWidth="" Skin="Simple" Width="350px">
<PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                </telerik:RadTextBox>
&nbsp;<telerik:RadButton ID="btn_buscar" runat="server" Skin="Simple" 
                            Text="      Find     ">
                        </telerik:RadButton>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtfind" ErrorMessage="Required" Visible="False"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 99px">
                       &nbsp;  
                        <asp:Label ID="Label20" runat="server" style="font-weight: 700" 
                            Text="Total"></asp:Label>
            </td>
            <td style="width: 13px">
                &nbsp;:&nbsp;</td>
            <td>
                <asp:Label ID="lbl_total" runat="server" style="font-weight: 700"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 99px">
                &nbsp;</td>
            <td style="width: 13px">
                &nbsp;</td>
            <td>
                                         <telerik:RadGrid ID="grd_cate"  Skin="Office2010Blue"   runat="server" AllowAutomaticDeletes="True" 
                                             CellSpacing="0" Culture="Spanish (Spain)" 
                                             GridLines="None" Skin="Sunset" Width="823px" AllowAutomaticUpdates="True"  
                                             AutoGenerateColumns="False" DataSourceID="SqlDataSource2">
<HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
<WebServiceSettings>
<ODataSettings InitialContainerName=""></ODataSettings>
</WebServiceSettings>
</HeaderContextMenu>
<ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="True">
<Selecting AllowRowSelect="True"></Selecting>
        </ClientSettings>
<MasterTableView AutoGenerateColumns="False" DataKeyNames="Identificador" DataSourceID="SqlDataSource2">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
     
     <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" 
            UniqueName="select">
       <ItemTemplate>
       <asp:HyperLink ID="aprobar" runat="server" ImageUrl="~/Imagenes/accept.png" ToolTip="approvals" Target="_self" />
       </ItemTemplate>
       <ItemStyle Width="5px" />
   </telerik:GridTemplateColumn>
    
     
    <telerik:GridBoundColumn DataField="nombre_proceso" 
    FilterControlAltText="Filter descripcion_cat column" 
    HeaderText="Name of Process" SortExpression="nombre_proceso" 
    UniqueName="nombre_proceso">
    </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="code_process" 
            FilterControlAltText="Filter descripcion_aprobacion column" HeaderText="Code of process" 
            SortExpression="code_process" 
            UniqueName="code_process">
        </telerik:GridBoundColumn>
        
        <telerik:GridBoundColumn DataField="RegionalAprobaciones_Id" 
            FilterControlAltText="Filter RegionalAprobaciones_Id column" 
            HeaderText="Regional" SortExpression="RegionalAprobaciones_Id" 
            UniqueName="RegionalAprobaciones_Id">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="amount" 
            FilterControlAltText="Filter descripcion_aprobacion column" 
            HeaderText="Amount" SortExpression="amount" 
            UniqueName="amount">
        </telerik:GridBoundColumn>
        
        <telerik:GridBoundColumn DataField="num_instrumento" 
            FilterControlAltText="Filter num_instrumento column" HeaderText="Instrument #" 
            SortExpression="num_instrumento" UniqueName="num_instrumento">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="beneficiario" 
            FilterControlAltText="Filter Ejecutor_Nombre column" HeaderText="Beneficiary" 
            SortExpression="beneficiario" UniqueName="beneficiario">
        </telerik:GridBoundColumn>
        
<telerik:GridBoundColumn DataField="comentarios" HeaderText="Comments" 
            SortExpression="comentarios" UniqueName="comentarios" 
            FilterControlAltText="Filter comentarios column" Visible="False"></telerik:GridBoundColumn>
<telerik:GridBoundColumn DataField="id" SortExpression="id" UniqueName="id" 
            FilterControlAltText="Filter id column" Visible="False"></telerik:GridBoundColumn>
     
    <telerik:GridBoundColumn  DataField="Identificador"  
            FilterControlAltText="Filter id_documento column"  
            SortExpression="Identificador" UniqueName="Identificador" Visible="False">
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
                                                 ConnectionString="<%$ ConnectionStrings:Celins_TestConnectionString %>" 
                                                 SelectCommand="SELECT nombre_proceso, code_process, RegionalAprobaciones_Id, Regional_Id, Regional_Nombre, amount, num_instrumento, beneficiario, Comentarios, tipoContrato, tipoVista, Identificador, ID FROM vw_ConsultasPorAprobacionUSAID WHERE tipoVista=@tipovista" 
                    ProviderName="<%$ ConnectionStrings:Celins_TestConnectionString.ProviderName %>">
                                                 <SelectParameters>
                                                     <asp:ControlParameter ControlID="rb_tipo" DefaultValue="1" Name="tipovista" 
                                                         PropertyName="SelectedValue" />
                                                 </SelectParameters>
                                             </asp:SqlDataSource>
                        
                        
                        
                        
                        </td>
        </tr>
        <tr>
            <td style="width: 99px">
                &nbsp;</td>
            <td style="width: 13px">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 99px">
                &nbsp;</td>
            <td style="width: 13px">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

