<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ActivityE.aspx.vb" Inherits="RMS_APPROVAL.frm_ActivityE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="timeline" Src="~/Controles/ctrl_timeline_activity.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Activity Management</asp:Label></h1>
    </section>
    <section class="content">


        <div class="box">
            <div class="box-header with-border">
                 <div class="col-sm-11">   
                   <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Edit Activity</asp:Label>
                        <asp:Label runat="server" ID="lbl_informacionProyecto"></asp:Label>
                        <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />
                    </h3>
                  </div>
                  <div class="col-sm-1 text-right">   
                       <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp();" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                  </div>
            </div>
            <div class="box-body">

                <div class="col-lg-12">

                    <div class="panel panel-default">
                       <div class="panel-heading" role="tab" id="headingOne" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                          <h4 class="panel-title">
                            <a role="button" data-toggle="collapse" href="#collapseOne"
                                            aria-expanded="true" aria-controls="collapseOne" runat="server" id="alink_ActivityTIME">Activity Timeline&nbsp;&nbsp;<i class='fa  fa-caret-square-o-down'></i></a>
                          </h4>
                       </div>
                        <div id="collapseOne" class="panel-collapse collapse no-margin" role="tabpanel" aria-labelledby="headingOne">
                             <div class="panel-body no-margin no-padding">
                                <uc:timeline id="timeline_activity" runat="server" />
                             </div>
                        </div>                        
                   </div>                

                </div>

                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="col-lg-12">
                            <asp:HiddenField ID="Hiddenindi" runat="server" />
                            <ul class="nav nav-tabs">

                                <li role="presentation" class="active"><a class="primary" runat="server" id="alink_definicion" href="#">ACTIVITY</a></li>                               
                                <li role="presentation"><a runat="server" id="alink_solicitation">SOLICITATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_prescreening">PRESCREENING</a></li>
                                <li role="presentation"><a runat="server" id="alink_submission">APPLICATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_evaluation">EVALUATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_awarded">AWARDED</a></li>
                                <li role="presentation" ><a runat="server" id="alink_documentos">DOCUMENTS</a></li>
                                <li role="presentation"><a runat="server" id="alink_funding">FUNDING</a></li>    
                                <li role="presentation"><a runat="server" id="alink_DELIVERABLES">DELIVERABLES</a></li>   
                                <li role="presentation"><a runat="server" id="alink_INDICATORS">INDICATORS</a></li>   

                                <li role="presentation"><a  runat="server" id="alink_stos">_</a></li>
                                <li role="presentation"><a  runat="server" id="alink_po">_</a></li>
                                <li role="presentation"><a  runat="server" id="alink_Ik">_</a></li>

                            </ul>
                        </div>
                        <div class="form-group row" style="margin-bottom: 0px;">
                        </div>
                        <asp:Label ID="Label1" runat="server" Text="" Visible="false" />

                        <div class="panel-body div-bordered">
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
                                    <telerik:RadComboBox ID="cmb_persona_encargada" runat="server" Width="500px">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lblt_mecanismo_contratacion" CssClass="control-label text-bold">Contract Sub-Mechanism</asp:Label>
                                </div>
                                <div class="col-sm-9">
                                    <telerik:RadComboBox ID="cmb_mecanismo_contratacion" runat="server" Width="500px" Filter="Contains" EmptyMessage="Select" AutoPostBack="true">
                                    </telerik:RadComboBox>
                                </div>
                            </div>

                             <div class="form-group row">
                                 <div class="col-sm-3 text-right">
                                     <asp:Label runat="server" ID="lblt_sub_mecanismo" CssClass="control-label text-bold">Sub-Mecanismo de contratación</asp:Label>
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
                                    <telerik:RadComboBox ID="cmb_ejecutor" runat="server" Width="500px" Filter="Contains">
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
                            <div class="form-group row">
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lblt_region" CssClass="control-label text-bold">REgions</asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <telerik:RadComboBox ID="cmb_region" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                                    <asp:CheckBox runat="server" ID="chk_todos" Text="Todos" AutoPostBack="true" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-3 text-right">
                                </div>
                                <div class="col-sm-8">
                                    <asp:Label ID="lblt_region_message" runat="server" CssClass="text-bold">This region is going to be in the code</asp:Label>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lblt_subregion" CssClass="control-label text-bold">Sub-Region</asp:Label>
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
                                                <telerik:GridBoundColumn DataField="nombre_subregion"
                                                    FilterControlAltText="Filter nombre_subregion column"
                                                    HeaderText="Sub Región" SortExpression="nombre_subregion"
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
                                    <asp:Label runat="server" ID="lblt_districts" CssClass="control-label text-bold">Districts</asp:Label>
                                </div>
                                <div class="col-sm-8">
                                    <telerik:RadGrid ID="grd_district" runat="server" AutoGenerateColumns="False" Width="500px">
                                        <MasterTableView AllowAutomaticDeletes="True" DataKeyNames="id_municipio">
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
                                        <asp:Label runat="server" ID="lbl_errorLOECero" Visible="false" CssClass="text-center text-bold" Font-Size="14px" Style="display: block; text-align: left">The LOE of the selected regions must be greater than 0</asp:Label>
                                        <asp:Label runat="server" ID="lbl_errorLOE" Visible="false" CssClass="text-center text-bold" Font-Size="14px">The LOE must be 100%</asp:Label>
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
                                <div class="col-sm-3 text-right"></div>
                                <div class="col-sm-8">
                                   <%-- <telerik:RadGrid ID="grd_partners" runat="server" AutoGenerateColumns="false" OnNeedDataSource="grd_partners_NeedDataSource"
                                                OnItemCommand="grd_partners_ItemCommand" Width="500px" Enabled="false">
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
                                            </telerik:RadGrid>--%>
                                </div>
                            </div>
                            <div class="form-group row hidden">
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lblt_componente" CssClass="control-label text-bold">Componente</asp:Label>
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
                                    <telerik:RadTextBox ID="txt_codigoRFA" runat="server" Width="250px" MaxLength="250">
                                    </telerik:RadTextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-3 text-right">
                                    <asp:Label runat="server" ID="lblt_codigo_ficha" CssClass="control-label text-bold">Activity Code</asp:Label>
                                </div>
                                <div class="col-sm-9">
                                    <div class="alert-sm bg-blue text-center" runat="server" id="divCodigo" style="width: 300px;">
                                        <asp:Label ID="lbl_mensaje" runat="server" CssClass="text-bold"></asp:Label>
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

                                       <%-- <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" ID="AsyncUpload1"
                                            TemporaryFolder="~/Temp" AllowedFileExtensions=".jpeg,.jpg,.png,.gif,.bmp" />--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <telerik:RadButton ID="btn_salir" runat="server" Text="Exit" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                            </telerik:RadButton>
                            <telerik:RadButton ID="btn_guardar" runat="server" Text="Save" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="true"
                                Width="100px" ValidationGroup="1">
                            </telerik:RadButton>
                        </div>
                    </div>
                    <!-- /.box-footer -->

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
                                        <asp:Button runat="server" ID="btn_eliminarMunicipio" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" Visible="false" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <asp:Button runat="server" ID="btn_eliminarAporte" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" Visible="false" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <asp:Button runat="server" ID="btn_eliminarIndicador" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" Visible="false" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <asp:Button runat="server" ID="btn_eliminarPartner" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" Visible="false" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" UseSubmitBehavior="false" Text="Eliminar" data-dismiss="modal" />
                                        <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade bs-example-modal-sm" id="modalTasaCambio" data-backdrop="static" data-keyboard="false">
                        <div class="vertical-alignment-helper">
                            <div class="modal-dialog modal-sm vertical-align-center">
                                <div class="modal-content">
                                    <div class="modal-header modal-primary" style="background-color:#367fa9; color:#ffffff;">
                                        <h4 class="modal-title" runat="server" id="H1">Alerta</h4>
                                    </div>
                                    <div class="modal-body">
                                        <asp:Label ID="lblt_msn_tasa_cambio" runat="server" Text="Debe ingresar la tasa de cambio correspondiente al periodo" />
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button runat="server" ID="btn_registrar_tc" CssClass="btn btn-sm btn-primary btn-ok" Text="Registrar tasa de cambio" data-dismiss="modal" UseSubmitBehavior="false" />
                                        <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="Button2">Cancelar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
     

        <script>

              var UpdatingValues = 0;

            function calc_Tot(sender, eventArgs) {

                if (UpdatingValues == 0) {

                    UpdatingValues = 1;

                     var clientSideExchangeRate = $find("<%= txt_exchange_rate.ClientID  %>");
                     var vExchangeRATE = clientSideExchangeRate.get_value(); 
                     var currency_contributionUSD = eventArgs.get_newValue();
                     var Value_LOC = 0;
                    if (!isNaN(vExchangeRATE) && !isNaN(currency_contributionUSD)) {
                          Value_LOC = currency_contributionUSD * vExchangeRATE;
                    }

                    var clientSide_value = $find("<%= txt_tot_amount_Local.ClientID  %>");                                                 
                        clientSide_value.set_value(Value_LOC);

                      UpdatingValues = 0;

                }

            }

              function calc_Exchange(sender, eventArgs) {

                if (UpdatingValues == 0) {

                    UpdatingValues = 1;

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

                      UpdatingValues = 0;

                }

            }

            function calc_Tot_LOC(sender, eventArgs) {

                if (UpdatingValues == 0) {

                    UpdatingValues = 1;

                     var clientSideExchangeRate = $find("<%= txt_exchange_rate.ClientID  %>");
                     var vExchangeRATE = clientSideExchangeRate.get_value(); 
                     var currency_contributionLOC = eventArgs.get_newValue();
                     var Value_USD = 0;
                    if (!isNaN(vExchangeRATE) && !isNaN(currency_contributionLOC)) {
                          Value_USD = currency_contributionLOC / vExchangeRATE;
                    }

                    var clientSide_valueUSD = $find("<%= txt_tot_amount.ClientID  %>");                                                 
                        clientSide_valueUSD.set_value(Value_USD);

                      UpdatingValues = 0;

                }

            }
                         

            function FuncModatTrim() {
                $('#modalTasaCambio').modal('show');
            }

            function UpdateItemCountField(sender, args) {
                //set the footer text
                sender.get_dropDownElement().lastChild.innerHTML = "Un total de " + sender.get_items().get_count() + " Actividades";
            }
                     

        </script>


        <%--        <script src="../Scripts/js-cookie.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                //when a group is shown, save it as the active accordion group
                $("#accordion").on('shown.bs.collapse', function () {
                    var active = $("#accordion .in").attr('id');
                    Cookies.set('activeAccordionGroup', active);
                    console.log(active);
                });
                $("#accordion").on('hidden.bs.collapse', function () {
                    Cookies.remove('activeAccordionGroup');
                });

                //function cargar()
                //{
                //    var last = Cookies.get('activeAccordionGroup');
                //    if (last != null) {
                //        //remove default collapse settings
                //        $("#accordion .panel-collapse").removeClass('in');
                //        //show the account_last visible group
                //        $("#" + last).addClass("in");
                //    }
                //}
                //cargar();
            });
        </script>--%>
    </section>
</asp:Content>

