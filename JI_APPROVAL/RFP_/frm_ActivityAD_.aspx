<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ActivityAD_.aspx.vb" Inherits="RMS_APPROVAL.frm_ActivityAD_" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="UbicacionGeografica" Src="~/Controles/UbicacionGeografica.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    
    <style>
        .import{
            margin-top:40px;
            float:right;
            background-color:#3C8DBC;
        }
    </style>
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />

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
                                            <asp:Label runat="server" ID="lblt_subtitulo_pantalla">New Activity</asp:Label>
                                        </h3>                     
                                    </div>
                                     <div class="col-sm-3 pull-right box-tools">                    
                                         <telerik:RadButton ID="btn_approbals" runat="server" BackColor="#3c8dbc"  AutoPostBack="true" Text="Importar desde Approvals" CssClass="btn btn-primary btn-sm pull-right margin-r-5 hidden" CausesValidation="false" ValidationGroup="2" BorderColor="#3C8DBC" ForeColor="White">
                                         </telerik:RadButton>
                                        <%--<telerik:RadButton ID="bth_print" runat="server" Height="30" Width="40" OnClientClicked="">
                                            <ContentTemplate>
                                                <i class="fa fa-print"></i>Print
                                            </ContentTemplate>
                                        </telerik:RadButton>--%>
                                    </div>
                                    <div class="col-sm-3 text-right">   
                                        <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('Help_nuevo_proyecto');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                                    </div>               
                            <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                       </div>



                        <div class="box-body"> 
                            <div class="col-lg-12">
                                    <div class="box-body">
                                             <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">

                                                 <div class="panel panel-default">

                                                        <div class="panel-heading" role="tab" id="headingOne" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                                            <h4 class="panel-title">
                                                                <a role="button" data-toggle="collapse" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne"
                                                                    runat="server" id="alink_informacion">OVERVIEW
                                                                </a>
                                                            </h4>
                                                        </div>
                                                        <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">

                                                             <div class="form-group row hidden">
                                                                <div class="col-sm-3 text-right">
                                                                    <%--<asp:Label runat="server" ID="lblt_codigo_ficha" CssClass="control-label text-bold">Código de Proyecto</asp:Label>--%>
                                                                    <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                                                                    <%-- Para mantener el tab activo después de hacer un postback --%>
                                                                    <asp:Label ID="hfAccordionIndex" runat="server" />
                                                                </div>
                                                                <div class="col-sm-9">
                                                                    <telerik:RadTextBox ID="txt_codigoproyecto" runat="server" Rows="3" TextMode="SingleLine" Width="250px" MaxLength="250">
                                                                    </telerik:RadTextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                                        ControlToValidate="txt_codigoproyecto" CssClass="Error" Display="Dynamic"
                                                                        ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                    <asp:LinkButton ID="lnk_sugerir_codigo" runat="server">Sugerir código de proyecto</asp:LinkButton>
                                                                </div>
                                                            </div>

                                                            <div class="form-group row">
                                                                <div class="col-sm-3 text-right">
                                                                    <asp:HiddenField runat="server" ID="id_documento" />
                                                                    <asp:Label runat="server" ID="lblt_nombre" CssClass="control-label text-bold">Activity Name</asp:Label>
                                                                </div>
                                                                <div class="col-sm-9">
                                                                    <telerik:RadTextBox ID="txt_nombreproyecto" runat="server" Rows="5" TextMode="MultiLine" Width="500px">
                                                                    </telerik:RadTextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                        ControlToValidate="txt_nombreproyecto" CssClass="Error" Display="Dynamic"
                                                                        ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <div class="col-sm-3 text-right">
                                                                    <asp:Label runat="server" ID="lblt_descripcion" CssClass="control-label text-bold">Activity Description</asp:Label>
                                                                </div>
                                                                <div class="col-sm-9">
                                                                    <telerik:RadTextBox ID="txt_descripcion" runat="server" Rows="5" TextMode="MultiLine" Width="500px" MaxLength="5000">
                                                                    </telerik:RadTextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                                        ControlToValidate="txt_descripcion" CssClass="Error" Display="Dynamic"
                                                                        ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>

                                                             <div class="form-group row">
                                                                <div class="col-sm-3 text-right">
                                                                    <asp:Label runat="server" ID="lblt_persona_encargada" CssClass="control-label text-bold">Supervisor Lead</asp:Label>
                                                                </div>
                                                                <div class="col-sm-9">
                                                                    <telerik:RadComboBox ID="cmb_persona_encargada"
                                                                           EmptyMessage ="Select..."   
                                                                           AllowCustomText="true" 
                                                                           Filter="Contains"           
                                                                        runat="server" Width="500px">
                                                                    </telerik:RadComboBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group row">
                                                                <div class="col-sm-3 text-right">
                                                                    <asp:Label runat="server" ID="lblt_mecanismo_contratacion" CssClass="control-label text-bold">Contract Mechanism</asp:Label>
                                                                </div>
                                                                <div class="col-sm-9">
                                                                    <telerik:RadComboBox ID="cmb_mecanismo_contratacion" runat="server" Width="500px" AutoPostBack="true">
                                                                    </telerik:RadComboBox>
                                                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                        ControlToValidate="cmb_mecanismo_contratacion" CssClass="Error" Display="Dynamic"
                                                                        ErrorMessage="Registre el mecanismo" ValidationGroup="1">(*)</asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>

                                                            <div class="form-group row">
                                                               <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_sub_mecanismo" CssClass="control-label text-bold">Contract Sub-Mechanism</asp:Label>
                                                               </div>
                                                               <div class="col-sm-9">
                                                                        <telerik:RadComboBox ID="cmb_sub_mecanismo_contratacion" runat="server" Width="500px" AutoPostBack="true">
                                                                        </telerik:RadComboBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                                            ControlToValidate="cmb_sub_mecanismo_contratacion" CssClass="Error" Display="Dynamic"
                                                                            ErrorMessage="Registre el Sub-mecanismo" ValidationGroup="1">(*)</asp:RequiredFieldValidator>
                                                               </div>
                                                            </div>
                                                                        
                                                             <div class="form-group row" runat="server" id="ly_activity" visible="false" >
                                                                <div class="col-sm-3 text-right">
                                                                    <asp:Label runat="server" ID="lblt_actividad_padre" CssClass="control-label text-bold">Belongs to</asp:Label>
                                                                </div>
                                                                <div class="col-sm-9" style="border: 1px dashed black; ">
                                                                    <br />

                                                                    <telerik:RadComboBox  ID="cmb_activity_father" 
                                                                           runat ="server" 
                                                                           CausesValidation="False"                                                                     
                                                                           EmptyMessage="Select the activity father..."   
                                                                           AllowCustomText="true" 
                                                                           Filter="Contains"                                                  
                                                                           Height="200px"
                                                                           Width="80%"
                                                                           OnDataBound="cmb_activity_DataBound" 
                                                                           OnItemDataBound="cmb_activity_ItemDataBound"     
                                                                           OnSelectedIndexChanged="cmb_activity_father_SelectedIndexChanged" 
                                                                           OnClientItemsRequested="UpdateItemCountField" AutoPostBack="true"
                                                                          >                                                              
                                                                           <HeaderTemplate>
                                                                             <ul>
                                                                                <li style="font-weight:700;" >Sub Contract Code / Status</li>
                                                                                <li style="font-weight:100;" >Activity</li>                                                                        
                                                                                <li style="font-weight:500;" >Period</li>                                                                                                                                                                                                                                     
                                                                             </ul>
                                                                           </HeaderTemplate>
                                                                           <ItemTemplate>
                                                                               <ul>
                                                                                     <li style="font-weight:700;" >
                                                                                       <%# DataBinder.Eval(Container.DataItem, "codigo_SAPME")%> -- <%# DataBinder.Eval(Container.DataItem, "STATUS")%> 
                                                                                     </li>
                                                                                     <li style="font-weight:100;" >
                                                                                         <%# DataBinder.Eval(Container.DataItem, "nombre_proyecto")%>  
                                                                                     </li>
                                                                                     <li style="font-weight:500;" >
                                                                                      <span style="font-weight:700;"  >From</span> <%#  DataBinder.Eval(Container.DataItem, "fecha_inicio_proyecto", "{0:d}")%> <span style="font-weight:700;"  >to</span> <%# DataBinder.Eval(Container.DataItem, "fecha_fin_proyecto", "{0:d}")%>
                                                                                    </li>                                                                                    
                                                                                </ul>
                                                                           </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                A total of
                                                                               <asp:Literal runat="server" ID="RadComboItemsCount" />
                                                                                  items
                                                                               </FooterTemplate>
                                                                      </telerik:RadComboBox> 
                                                                     <asp:Label runat="server" ID="lbl_Activity_error" CssClass="control-label text-bold  text-red" Visible="false">(*)</asp:Label>   
                                                                     <br /><br />      
                                                                </div>
                                                            </div>

                                                            <%--HIDDEN--%>
                                                                <div class="form-group row hide">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_ejecutor" CssClass="control-label text-bold">Ejecutor</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-9">
                                                                        <telerik:RadComboBox ID="cmb_ejecutor" runat="server" Width="500px" Filter="Contains" EmptyMessage="Select">
                                                                        </telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row hidden">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_programa" CssClass="control-label text-bold">Programa</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <telerik:RadComboBox ID="cmb_programa" runat="server" Width="300px" Enabled="false"></telerik:RadComboBox>
                                                                    </div>
                                                                </div>

                                                            <%--***********************************************************REMOVE*********************************************************************--%>

                                                               <%-- <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_region" CssClass="control-label text-bold">Region</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <telerik:RadComboBox ID="cmb_region" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                                                        <asp:CheckBox runat="server" ID="chk_todos" Text="Muti-Region" AutoPostBack="true" />
                                                                    </div>
                                                                </div>
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <asp:Label ID="lblt_region_message" runat="server" CssClass="text-bold">This region is going to be in the code</asp:Label>
                                                                    </div>
                                                                </div>      
                                                                   
                                                                   
                                                                   --%>
                                                          
                                                                                                                                                                                   

                                                        <%--        <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_subregion" CssClass="control-label text-bold">Sub-Regions</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <telerik:RadComboBox ID="cmb_subregion" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                                                        <telerik:RadGrid ID="grd_subregion" runat="server" AutoGenerateColumns="False" Visible="false" Width="500px">
                                                                            <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_subregion">
                                                                                <Columns>
                                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                                                        HeaderText="" UniqueName="TemplateColumnAnual">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="ctrl_id" runat="server" AutoPostBack="true" OnCheckedChanged="ctrl_id_CheckedChanged" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="15px" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridBoundColumn DataField="id_subregion" DataType="System.Int32"
                                                                                        FilterControlAltText="Filter id_subregion column" HeaderText="id_subregion"
                                                                                        ReadOnly="True" SortExpression="id_subregion" UniqueName="id_subregion"
                                                                                        Display="False">
                                                                                    </telerik:GridBoundColumn>
                                                                                     <telerik:GridBoundColumn DataField="nombre_region"
                                                                                        FilterControlAltText="Filter nombre_region column"
                                                                                        HeaderText="Region" SortExpression="nombre_region"
                                                                                        UniqueName="colm_nombre_region" ItemStyle-Width="90%">
                                                                                         <ItemStyle Width="30%" />
                                                                                    </telerik:GridBoundColumn>
                                                                                    <telerik:GridBoundColumn DataField="nombre_subregion"
                                                                                        FilterControlAltText="Filter nombre_subregion column"
                                                                                        HeaderText="Sub-Region" SortExpression="nombre_subregion"
                                                                                        UniqueName="colm_nombre_subregion" ItemStyle-Width="90%">
                                                                                    </telerik:GridBoundColumn>
                                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter colm_nivel_cobertura column" Visible="false"
                                                                                        HeaderText="Nivel de Cobertura" UniqueName="colm_nivel_cobertura">
                                                                                        <ItemTemplate>
                                                                                            <telerik:RadNumericTextBox runat="server" ID="txt_nivel_cobertura" MinValue="0" MaxValue="100"
                                                                                                Enabled="false" OnTextChanged="txt_nivel_cobertura_TextChanged" AutoPostBack="true">
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="15px" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </div>
                                                                </div>--%>


                                                              <%--***********************************************************REMOVE*********************************************************************--%>


                                                              <div class="form-group row">
                                                                <div class="col-sm-3 text-right">
                                                                   <asp:Label runat="server" ID="lblt_regionII" CssClass="control-label text-bold">Region </asp:Label>
                                                                 </div>
                                                                 <div class="col-sm-8">
                                                                    <telerik:RadComboBox ID="cmb_regionII" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                                                  <asp:CheckBox runat="server" ID="chk_todosII" Text="Muti-Region" AutoPostBack="true" />
                                                                  </div>
                                                              </div>                                                               

                                                            
                                                                <div class="form-group row">
                                                                    <div class="col-sm-3 text-right">
                                                                        <asp:Label runat="server" ID="lblt_subregionII" CssClass="control-label text-bold">Sub-Regions</asp:Label>
                                                                    </div>
                                                                    <div class="col-sm-8">
                                                                        <telerik:RadComboBox ID="cmb_subregionII" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                                                        <telerik:RadGrid ID="grd_subregionII" runat="server" AutoGenerateColumns="False" Visible="false" Width="500px">
                                                                            <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_subregion">
                                                                                <Columns>
                                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                                                        HeaderText="" UniqueName="TemplateColumnAnual">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="ctrl_id" runat="server" AutoPostBack="true" OnCheckedChanged="ctrl_id_CheckedChanged" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="15px" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridBoundColumn DataField="id_subregion" DataType="System.Int32"
                                                                                        FilterControlAltText="Filter id_subregion column" HeaderText="id_subregion"
                                                                                        ReadOnly="True" SortExpression="id_subregion" UniqueName="id_subregion"
                                                                                        Display="False">
                                                                                    </telerik:GridBoundColumn>
                                                                                     <telerik:GridBoundColumn DataField="nombre_region"
                                                                                        FilterControlAltText="Filter nombre_region column"
                                                                                        HeaderText="Region" SortExpression="nombre_region"
                                                                                        UniqueName="colm_nombre_region" ItemStyle-Width="90%">
                                                                                         <ItemStyle Width="30%" />
                                                                                    </telerik:GridBoundColumn>
                                                                                    <telerik:GridBoundColumn DataField="nombre_subregion"
                                                                                        FilterControlAltText="Filter nombre_subregion column"
                                                                                        HeaderText="Sub-Region" SortExpression="nombre_subregion"
                                                                                        UniqueName="colm_nombre_subregion" ItemStyle-Width="90%">
                                                                                    </telerik:GridBoundColumn>
                                                                                    <telerik:GridTemplateColumn FilterControlAltText="Filter colm_nivel_cobertura column" Visible="false"
                                                                                        HeaderText="Nivel de Cobertura" UniqueName="colm_nivel_cobertura">
                                                                                        <ItemTemplate>
                                                                                            <telerik:RadNumericTextBox runat="server" ID="txt_nivel_cobertura" MinValue="0" MaxValue="100"
                                                                                                Enabled="false" OnTextChanged="txt_nivel_cobertura_TextChanged" AutoPostBack="true">
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="15px" />
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </div>
                                                                </div>

                                                                     <div class="form-group row hidden">
                                                                            <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_districts" CssClass="control-label text-bold">Municipios</asp:Label>
                                                                            </div>
                                                                            <div class="col-sm-8">
                                                                                <telerik:RadGrid ID="grd_district" runat="server" AutoGenerateColumns="False"  Width="500px">
                                                                                    <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_district">
                                                                                        <Columns>
                                                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                                                                HeaderText="" UniqueName="TemplateColumnAnual">
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="ctrl_id_municipio" runat="server" AutoPostBack="true" OnCheckedChanged="ctrl_id_municipio_CheckedChanged" />
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle Width="15px" />
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridBoundColumn DataField="id_municipio" DataType="System.Int32"
                                                                                                FilterControlAltText="Filter id_municipio column" HeaderText="id_municipio"
                                                                                                ReadOnly="True" SortExpression="id_municipio" UniqueName="id_municipio"
                                                                                                Display="False">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridBoundColumn DataField="nombre_municipio"
                                                                                                FilterControlAltText="Filter nombre_municipio column"
                                                                                                HeaderText="District" SortExpression="nombre_municipio"
                                                                                                UniqueName="colm_nombre_municipio" ItemStyle-Width="40%">
                                                                                            </telerik:GridBoundColumn>
                                                                                            <telerik:GridTemplateColumn FilterControlAltText="Filter colm_nivel_cobertura column"
                                                                                                HeaderText="Nivel de Cobertura" UniqueName="colm_nivel_cobertura">
                                                                                                <ItemTemplate>
                                                                                                    <telerik:RadNumericTextBox runat="server" ID="txt_nivel_cobertura" MinValue="0" MaxValue="100"
                                                                                                        Enabled="false" OnTextChanged="txt_nivel_cobertura_TextChanged" AutoPostBack="true">
                                                                                                    </telerik:RadNumericTextBox>
                                                                                                </ItemTemplate>
                                                                                                <HeaderStyle Width="15px" />
                                                                                            </telerik:GridTemplateColumn>
                                                                                        </Columns>
                                                                                    </MasterTableView>
                                                                                </telerik:RadGrid>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group row">
                                                                            <div class="col-sm-3 text-right">
                                                                            </div>
                                                                            <div class="col-sm-6">
                                                                                <div class="alert-sm bg-blue" runat="server" id="div_mensaje" visible="false" style="width: 500px;">
                                                                                    <asp:Label runat="server" ID="lblt_errorLOECero" Visible="false" CssClass="text-center text-bold" Font-Size="14px" Style="display: block; text-align: left">For each selected district the LEO must be greater than 0</asp:Label>
                                                                                    <asp:Label runat="server" ID="lblt_errorLOEHundred" Visible="false" CssClass="text-center text-bold" Font-Size="14px">The LOE must be 100%</asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group row hidden">
                                                                            <div class="col-sm-3 text-right">
                                                                                <asp:Label runat="server" ID="lblt_private_public" CssClass="control-label text-bold">Is this a Private-Public Partnership?</asp:Label>
                                                                            </div>
                                                                            <div class="col-sm-8">
                                                                                <asp:RadioButtonList ID="rbn_private_public" runat="server" RepeatDirection="Horizontal" CssClass="rbnStyle" AutoPostBack="true">
                                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </div>
                                                                        </div>


                                                                          <div class="form-group row hidden">
                                                                                <div class="col-sm-3 text-right" style="padding-top: 5px;">
                                                                                </div>
                                                                                <div class="col-sm-9" style="padding-top: 5px;">
                                                                                    <telerik:RadGrid ID="grd_partners" runat="server" AutoGenerateColumns="false" 
                                                                                        Width="500px" Enabled="false">
                                                                                        <%--OnItemCommand="grd_partners_ItemCommand" --%>
                                                                                        <%--OnNeedDataSource="grd_partners_NeedDataSource"--%>
                                                                                        <PagerStyle AlwaysVisible="true" />
                                                                                        <MasterTableView DataKeyNames="id_ficha_partner" CommandItemDisplay="Top" CommandItemSettings-ShowRefreshButton="false">
                                                                                            <Columns>
                                                                                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar">
                                                                                                    <HeaderStyle Width="10" />
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10" ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar" OnClick="Eliminar_Click">
                                                                                                            <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                                                        </asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridBoundColumn DataField="id_ficha_partner" UniqueName="id_ficha_partner" HeaderText="id_ficha_partner" Visible="false">
                                                                                                </telerik:GridBoundColumn>
                                                                                                <telerik:GridTemplateColumn UniqueName="colm_partner_name" HeaderText="Name of Partner">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txt_nombre_partner" runat="server" Text='<%# Eval("nombre_partner") %>'></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn UniqueName="colm_partnership_type" HeaderText="Type of Partnership">
                                                                                                    <ItemTemplate>
                                                                                                        <telerik:RadComboBox runat="server" ID="cmb_partner_type"></telerik:RadComboBox>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn UniqueName="colm_partnership_focus" HeaderText="Partnership Focus">
                                                                                                    <ItemTemplate>
                                                                                                        <telerik:RadComboBox runat="server" ID="cmb_partnership_focus"></telerik:RadComboBox>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridBoundColumn DataField="id_partner_type" UniqueName="id_partner_type" Visible="false"></telerik:GridBoundColumn>
                                                                                                <telerik:GridBoundColumn DataField="id_partnership_focus" UniqueName="id_partnership_focus" Visible="false"></telerik:GridBoundColumn>
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                </div>
                                                                            </div>

                                                                            <div class="form-group row hidden">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_componentes" CssClass="control-label text-bold">Componentes</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-8">
                                                                                    <telerik:RadComboBox ID="cmb_componente" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                                                                </div>
                                                                            </div>

                                                                           <div class="form-group row">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_fecha_inicio" CssClass="control-label text-bold">Start Date</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-9">
                                                                                    <telerik:RadDatePicker ID="dt_fecha_inicio" runat="server" AutoPostBack="true"></telerik:RadDatePicker>
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="Error"
                                                                                        ControlToValidate="dt_fecha_inicio" ErrorMessage="*"
                                                                                        ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group row">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_fecha_final" CssClass="control-label text-bold">End Date</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-9">
                                                                                    <telerik:RadDatePicker ID="dt_fecha_fin" runat="server"></telerik:RadDatePicker>
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="Error"
                                                                                        ControlToValidate="dt_fecha_fin" ErrorMessage="*"
                                                                                        ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </div>

                                                                             <div class="form-group row">
                                                                                  <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_exchange_rate" CssClass="control-label text-bold">Exchange Rate</asp:Label>
                                                                                  </div>
                                                                                <div class="col-sm-9">
                                                                                    <telerik:RadNumericTextBox ID="txt_exchange_rate" runat="server"  NumberFormat-DecimalDigits="4" >
                                                                                         <ClientEvents OnValueChanging="calc_Exchange" />
                                                                                    </telerik:RadNumericTextBox>
                                                                                </div>
                                                                            </div>

                                                                             <div class="form-group row">
                                                                                  <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_total_Amount" CssClass="control-label text-bold">Activity Amount (USD)</asp:Label>
                                                                                  </div>
                                                                                <div class="col-sm-9">
                                                                                    <telerik:RadNumericTextBox ID="txt_tot_amount" runat="server"  NumberFormat-DecimalDigits="2" >
                                                                                         <ClientEvents OnValueChanging="calc_Tot" />
                                                                                    </telerik:RadNumericTextBox>
                                                                                </div>
                                                                            </div>

                                                                             <div class="form-group row">
                                                                                  <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_total_Amount_local" CssClass="control-label text-bold">Activity Amount (COP)</asp:Label>
                                                                                  </div>
                                                                                <div class="col-sm-9">
                                                                                    <telerik:RadNumericTextBox ID="txt_tot_amount_Local" runat="server"  NumberFormat-DecimalDigits="2" >
                                                                                          <ClientEvents OnValueChanging="calc_Tot_LOC" />
                                                                                    </telerik:RadNumericTextBox>
                                                                                </div>
                                                                            </div>
                                    
                                                                            <div class="form-group row">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_trimestre" CssClass="control-label text-bold">Quarter</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-8">
                                                                                    <telerik:RadComboBox ID="cmb_periodo" runat="server" Width="300px"></telerik:RadComboBox>
                                                                                </div>
                                                                            </div>

                                                                            <div class="form-group row hidden">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_codigoME" CssClass="control-label text-bold">Código ME</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-9">
                                                                                    <telerik:RadTextBox ID="txt_codigo_SAPME" runat="server" Width="250px" MaxLength="250">
                                                                                    </telerik:RadTextBox>
                                                                                </div>
                                                                            </div>

                                                                             <div class="form-group row">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_estado" CssClass="control-label text-bold">Status</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-8">
                                                                                    <telerik:RadComboBox ID="cmb_estado" runat="server" Width="300px"></telerik:RadComboBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group row hidden">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_codigo_AID" CssClass="control-label text-bold">AID Code</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-9">
                                                                                    <telerik:RadTextBox ID="txt_codigoAID" runat="server" Width="250px" MaxLength="250">
                                                                                    </telerik:RadTextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group row">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_codigoRFA" CssClass="control-label text-bold">Technical Code</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-9">
                                                                                    <telerik:RadTextBox ID="txt_codigoRFA" runat="server" Width="300px" MaxLength="300">
                                                                                    </telerik:RadTextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group row">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_codigo_ficha" CssClass="control-label text-bold">Activity Code</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-9">
                                                                                    <div class="alert-sm bg-blue text-center" runat="server" id="divCodigo" visible="false" style="width: 300px;">
                                                                                        <asp:Label ID="lbl_mensaje" runat="server" CssClass="text-bold" Visible="False"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group row">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_codigo_monitor" CssClass="control-label text-bold">Monitoring Code</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-9">
                                                                                    <telerik:RadTextBox ID="txt_codigoMonitor" runat="server" Width="250px" MaxLength="250">
                                                                                    </telerik:RadTextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group row hidden">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_acta_aprobacion" CssClass="control-label text-bold">Acta de Aprobación</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-9">
                                                                                    <telerik:RadTextBox ID="txt_acta" runat="server" Width="250px" MaxLength="250">
                                                                                    </telerik:RadTextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group row hidden">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_codigo_convenio" CssClass="control-label text-bold">Código de Convenio</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-9">
                                                                                    <telerik:RadTextBox ID="txt_codigo_convenio" runat="server" Width="250px" MaxLength="250">
                                                                                    </telerik:RadTextBox>
                                                                                </div>
                                                                            </div>

                                                                         <div class="hidden">
                                                                            <div class="form-group row">
                                                                                <div class="col-sm-3 text-right">
                                                                                    <asp:Label runat="server" ID="lblt_imagen_proyecto" CssClass="control-label text-bold">Imagen de Proyecto</asp:Label>
                                                                                </div>
                                                                                <div class="col-sm-9">

                                                                                <%--    <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="AsyncUpload1"
                                                                                        TemporaryFolder="~/Temp" OnFileUploaded="RadAsyncUpload1_FileUploaded"
                                                                                        AllowedFileExtensions=".jpeg,.jpg,.png,.gif,.bmp" />--%>
                                            
                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                        </div>   <%--id="collapseOne" --%>

                                                 </div>  <%--class="panel panel-default"--%>

                                             </div>  <%--class="panel-group" --%>
                                         <!-- /.box-body -->
                                        <div class="box-footer">
                                            <telerik:RadButton ID="btn_salir" runat="server" Text="Exit" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                                            </telerik:RadButton>
                                            <telerik:RadButton ID="btn_guardar" runat="server" Text="Save" AutoPostBack="true" runat="server" SingleClick="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                                                Width="100px" ValidationGroup="1">
                                            </telerik:RadButton>
                                        </div>
                                    </div>
                                    <!-- /.box-footer -->
                            </div>  <%--<div class="col-lg-12">--%>
                        </div>   <%--<div class="box-body">--%>           
           
        
        </div>  <%-- <div class="box">--%>

        <script type="text/javascript">
                        
            var UpdatingValuesAC = 0;

            function calc_Tot(sender, eventArgs) {

                if (UpdatingValuesAC == 0) {

                    UpdatingValuesAC = 1;

                     var clientSideExchangeRate = $find("<%= txt_exchange_rate.ClientID  %>");
                     var vExchangeRATE = clientSideExchangeRate.get_value(); 
                     var currency_contributionUSD = eventArgs.get_newValue();
                     var Value_LOC = 0;
                    if (!isNaN(vExchangeRATE) && !isNaN(currency_contributionUSD)) {
                          Value_LOC = Math.round(currency_contributionUSD * vExchangeRATE);
                    }

                    var clientSide_value = $find("<%= txt_tot_amount_Local.ClientID  %>");                                                 
                        clientSide_value.set_value(Value_LOC);

                      UpdatingValuesAC = 0;

                }

            }

            function calc_Exchange(sender, eventArgs) {

                if (UpdatingValuesAC == 0) {

                    UpdatingValuesAC = 1;

                     var clientSide_valueLOC = $find("<%= txt_tot_amount_Local.ClientID  %>");
                     var currency_contributionLOC = clientSide_valueLOC.get_value(); 

                      var clientSide_valueUSD = $find("<%= txt_tot_amount.ClientID  %>");       
                      //var currency_contributionUSD = clientSide_valueUSD.get_value();                                       

                      var vExchangeRATE = eventArgs.get_newValue(); 
                      var Value_USD = 0;
                   
                      if (!isNaN(vExchangeRATE) && !isNaN(currency_contributionLOC)) {
                            Value_USD = Math.round(currency_contributionLOC / vExchangeRATE) ;
                      }
                                                                                
                        clientSide_valueUSD.set_value(Value_USD);

                      UpdatingValuesAC = 0;

                }

            }


            function calc_Tot_LOC(sender, eventArgs) {

                if (UpdatingValuesAC == 0) {

                    UpdatingValuesAC = 1;

                     var clientSideExchangeRate = $find("<%= txt_exchange_rate.ClientID  %>");
                     var vExchangeRATE = clientSideExchangeRate.get_value(); 
                     var currency_contributionLOC = eventArgs.get_newValue();
                     var Value_USD = 0;

                    if (!isNaN(vExchangeRATE) && !isNaN(currency_contributionLOC)) {
                          Value_USD = Math.round(currency_contributionLOC / vExchangeRATE) ;
                    }

                    var clientSide_valueUSD = $find("<%= txt_tot_amount.ClientID  %>");                                                 
                        clientSide_valueUSD.set_value(Value_USD);

                      UpdatingValuesAC = 0;

                }

            }

             


            function unCheckA(a) {
                $('input[type=checkbox]').each(function () {
                    if(a != this)
                        $(this).attr('checked', false);
                });
                
            }

          //  $(document).ready(function () {
                
                ////when a group is shown, save it as the active accordion group
                //$("#accordion").on('shown.bs.collapse', function () {
                //    var active = $("#accordion .in").attr('id');
                //    Cookies.set('activeAccordionGroup', active);
                //});
                //$("#accordion").on('hidden.bs.collapse', function () {
                //    Cookies.remove('activeAccordionGroup');
                //});

                <%--$("#<%= rbn_private_public.ClientID%>").on("change", function ()
                {
                    if ($(this).val() == 1) {
                        $("#div_private_public").removeClass("hidden");
                    }
                    else {
                        $("#div_private_public").addClass("hidden");
                    }
                });--%>
          //  });

            function UpdateItemCountField(sender, args) {
                //set the footer text
                sender.get_dropDownElement().lastChild.innerHTML = "Un total de " + sender.get_items().get_count() + " Activities";
            }
                        
            function FuncModatTrim() {
                $('#modalTasaCambio').modal('show');
            }
            
            function FuncApprobals() {
                $('#ApprovalsModal').modal('show');
            }


        </script>

                 

    </section>  <%--<section class="content">--%>
    
</asp:Content>