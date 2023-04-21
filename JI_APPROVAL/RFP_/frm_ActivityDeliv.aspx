<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_ActivityDeliv.aspx.vb" Inherits="RMS_APPROVAL.frm_ActivityDeliv" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>
  
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <link rel="stylesheet" href="../Content/slider_style.css">

    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Proyectos</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <div class="col-sm-11">   
                    <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Proyecto - Entregables</asp:Label>
                        <asp:Label runat="server" ID="lbl_informacionproyecto"></asp:Label>
                        <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lbl_id_ficha_aw" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                    </h3>
                 </div>
                 <div class="col-sm-1 text-right">   
                     <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp('');" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                 </div

            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="col-lg-12">
                            <asp:HiddenField ID="Hiddenindi" runat="server" />
                            <ul class="nav nav-tabs">
                                <li role="presentation"><a runat="server" id="alink_definicion" class="hidden" href="#">ACTIVITY</a></li>                               
                                <li role="presentation"><a runat="server" id="alink_solicitation">SOLICITATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_prescreening">PRESCREENING</a></li>
                                <li role="presentation"><a runat="server" id="alink_submission">APPLICATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_evaluation">EVALUATION</a></li>
                                <li role="presentation"><a runat="server" id="alink_awarded">AWARDED</a></li>      
                                <li role="presentation"><a runat="server" id="alink_documentos">DOCUMENTS</a></li>
                                <li role="presentation"><a runat="server" id="alink_funding">FUNDING</a></li>    
                                <li role="presentation" class="active"><a class="primary" runat="server" id="alink_DELIVERABLES">DELIVERABLES</a></li> 
                                <li role="presentation"><a runat="server" id="alink_INDICATORS">INDICATORS</a></li>    
                            </ul>
                        </div>
                        <div class="form-group row" style="margin-bottom: 0px;">
                        </div>
                        <asp:Label ID="lbl_id_ficha" runat="server" Text="" Visible="false" />
                        <asp:Label ID="LBL_ID_AWARD" runat="server" Text="" Visible="false" />
                         
                        <div class="panel-body div-bordered">
                            
                            <%--**********************************************************************************CONTROL SELECTOR*************************************************************--%>

                                  <div class="form-group row">
                                                                     
                                     <div class="col-sm-12">                                         

                                                <div class="col-md-4 col-sm-6 col-xs-16">
                                                  <div class="info-box">
                                                      <span class="info-box-icon bg-orange-active"><i class="fas fa-sack-dollar"></i></span>
                                                      <div class="info-box-content">
                                                           <span class="info-box-text"><asp:Label runat="server" ID="lbl_totalACT2" Text ="0"></asp:Label><br /><asp:Label runat="server" ID="lbl_totalACT2_usd" Text ="0"></asp:Label></span>
                                                           <span class="text-bold">Total Solicitation</span>             
                                                      </div><!-- /.info-box-content -->
                                                  </div><!-- /.info-box -->
                                               </div><!-- /.col -->

                                               <div class="col-md-4 col-sm-6 col-xs-16 "  >                                                                                                                                                                                                                        
                                                  
                                                   <div style="width: 250px; height: 200px; margin: 0 auto">
                                                      <div id="container-money" style="width: 250px; height: 160px; margin: 0 auto; text-align: center;"></div>
                                                      <input style="display: none;" id="valor_avance" runat="server" />
                                                      <h3 runat="server" id="txt_avance" style="margin: 0 auto;text-align: center;font-family: Tahoma;font-weight: 600; font-size: 15px;"></h3>
                                                  </div>
                                                                                                              
                                               </div> 
                                                                                                      

                                               <div class="col-md-4 col-sm-6 col-xs-16 ">
                                                  <div class="info-box">
                                                      <span class="info-box-icon bg-orange-active"><i class="fa-solid fa-handshake-angle"></i></span>
                                                      <div class="info-box-content">
                                                           <span class="info-box-text"><asp:Label runat="server" ID="lbl_totalPerf2" Text ="0"></asp:Label><br /><asp:Label runat="server" ID="lbl_totalPerf2_usd" Text =""></asp:Label></span>
                                                           <span class="text-bold">Total Amount Awarded</span>             
                                                      </div><!-- /.info-box-content -->
                                                  </div><!-- /.info-box -->
                                               </div><!-- /.col -->                                      

                                     </div>                                                                       

                               </div>  


                           <%--**********************************************************************************CONTROL SELECTOR*************************************************************--%>

                            <div class="form-group row">   
                               
                               <div class="col-lg-1"></div>
                               <div class="col-lg-2">
                                   <asp:Label runat="server" ID="lblt_contract_selector" CssClass="control-label text-bold">CONTRACT</asp:Label>
                               </div>
                               <div class="col-lg-9">
                                   
                                               <!--Control -->
                                                             <telerik:RadComboBox  ID="cmb_awards" AutoPostBack="true" 
                                                                                   runat ="server" 
                                                                                    CausesValidation="False"                                                                     
                                                                                    EmptyMessage="Select an award..."   
                                                                                    AllowCustomText="true" 
                                                                                    Filter="Contains"                                                  
                                                                                    Height="200px"
                                                                                    Width="95%"
                                                                                    OnDataBound="cmb_awards_DataBound"                                                                                   
                                                                                    OnClientItemsRequested="UpdateItemCountField_aw"                                                                                                                                             
                                                                                     >                                                              
                                                                            <HeaderTemplate>
                                                                                    <ul>
                                                                                        <li style="font-weight:700;" >Contract Code / Status</li>
                                                                                        <li style="font-weight:100;" >Contract Mecanism</li> 
                                                                                        <li style="font-weight:100;" >Applicant</li>                                                                                                                                                                                                                                                                                                                       
                                                                                    </ul>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <ul>
                                                                                    <li style="font-weight:700;" >
                                                                                        <%# DataBinder.Eval(Container.DataItem, "AWARD_CODE")%> -- <%# DataBinder.Eval(Container.DataItem, "AWARD_STATUS")%> 
                                                                                    </li>
                                                                                    <li style="font-weight:100;" >
                                                                                        <span style="font-weight:400;" > <%# DataBinder.Eval(Container.DataItem, "nombre_proyecto")%> </span>
                                                                                    </li>
                                                                                    <li style="font-weight:700; color:#ED7620;" >
                                                                                        <%# DataBinder.Eval(Container.DataItem, "ORGANIZATIONNAME")%>  
                                                                                    </li>                                                                                                                                                                   
                                                                                </ul>
                                                                            </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            A total of
                                                                            <asp:Literal runat="server" ID="RadComboItemsCount_award" />
                                                                            items
                                                                        </FooterTemplate>
                                                            </telerik:RadComboBox>
                                                                              

                                       </div>

                            </div>
                                                      
                            <%--**********************************************************************************CONTROL SELECTOR*************************************************************--%>
                                                            
                               <div class="form-group row">                                                                     
                                     <div class="col-sm-12">
                                         <hr style="border-color: darkgrey;" />
                                     </div>
                                </div>

                                <div class="form-group row">

                                         <div class="col-sm-12">                                                                                       

                                               <div class="box-body">                                 
                                                    <div class="col-md-4 col-sm-8 col-xs-16">
                                                      <div class="info-box">
                                                        <span class="info-box-icon bg-gray"><i class="fa fa-university"></i></span>
                                                        <div class="info-box-content">
                                                             <span class="info-box-text"><asp:Label ID="lblt_implementer" runat="server">Implementer</asp:Label></span>
                                                             <span class="text-bold"><asp:Label ID="lbl_implementer" runat="server"></asp:Label></span>             
                                                        </div><!-- /.info-box-content -->
                                                      </div><!-- /.info-box -->
                                                    </div><!-- /.col -->
                                                     <div class="col-md-4 col-sm-8 col-xs-16">
                                                      <div class="info-box">
                                                         <span class="info-box-icon  bg-gray"><i class="fa fa-tasks"></i></span>
                                                        <div class="info-box-content">
                                                             <span class="info-box-text"><asp:Label ID="lbl_activity_name" runat="server"></asp:Label></span>                                                           
                                                             <span class="text-bold"><asp:Label ID="lbl_activity_Code" runat="server"></asp:Label></span>                                                                          
                                                        </div><!-- /.info-box-content -->
                                                      </div><!-- /.info-box -->
                                                    </div><!-- /.col -->
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                          <div class="info-box">
                                                              <span class="info-box-icon bg-gray"><i class="fas fa-calendar-check"></i></span>
                                                              <div class="info-box-content">
                                                                 <span class="info-box-text"><asp:Label ID="lbl_last_Deliverable" runat="server"></asp:Label></span>
                                                                 <span class="text-bold"><asp:Label ID="lbl_status_Deliverable" runat="server"></asp:Label>&nbsp;&nbsp;<i class="fa fa-calendar-o"></i>&nbsp;<asp:Label ID="lbl_period" runat="server"></asp:Label></span>             
                                                                 <%--&nbsp;--&nbsp;<asp:Label ID="lblt_date_status" runat="server"></asp:Label><br /><i class="fa fa-clock-o"></i><asp:Label ID="lblt_time_status" runat="server"></asp:Label>--%>
                                                              </div><!-- /.info-box-content -->
                                                          </div><!-- /.info-box -->
                                                      </div><!-- /.col -->                                                   
                                                </div>

                                           </div>

                                                  
                               </div>


                                 <div class="form-group row">                                                                     
                                     <div class="col-sm-12">
                                         <hr style="border-color: darkgrey;" />
                                     </div>
                                </div>

                             <%--**********************************************************************************CONTROL SELECTOR*************************************************************--%>




                            
                                 <div class="form-group row">                                         
                                    <br />
                                    <div class="col-sm-2 text-right">
                                         <asp:Label runat="server" ID="lblt_aporte" CssClass="control-label text-bold">Aporte Proyecto:</asp:Label>
                                    </div>                               
                                    <div class="col-sm-2 text-right">
                                           <asp:Label runat="server" ID="lbl_monto_aportes" CssClass="control-label text-bold"></asp:Label><br /><br />
                                           <asp:Label runat="server" ID="lbl_monto_aportesUSD" CssClass="control-label text-bold"></asp:Label>
                                           <asp:HiddenField runat="server" ID="monto_proyecto" />                                      
                                           <asp:HiddenField runat="server" ID="proy_tasa_cambio" Value ="0" />                                      
                                   </div>
                                     <div class="col-sm-2 text-right">
                                        <asp:Label runat="server" ID="lblt_total_entregable" CssClass="control-label text-bold">Total Entregables:</asp:Label>                         
                                    </div>
                                    <div class="col-sm-2">
                                       <asp:Label runat="server" ID="lbl_monto_entregables" CssClass="control-label text-bold"></asp:Label><br /><br />
                                       <asp:Label runat="server" ID="lbl_monto_entregablesUSD" CssClass="control-label text-bold"></asp:Label><br /><br />
                                       <asp:Label runat="server" ID="lbl_monto_entregablesPorc" CssClass="control-label text-bold"></asp:Label><br /><br />
                                    </div>
                               </div>
                            
                            <div class="form-group row"> 
                              <div class="col-sm-12">
                                  <hr style="border-color:black;" />
                              </div>   
                            </div>
                            
                            <div class="form-group row">      
                                    <div class="col-sm-2 text-right">
                                       <asp:Label runat="server" ID="lblt_currency_entry" CssClass="control-label text-bold">Currency data Entry:</asp:Label>
                                    </div>
                                    <div class="col-sm-2 text-right">   
                                        <br />
                                        <label class="switch">
                                          <input id="chk_data_in" runat="server" type="checkbox" onchange="Currency_input()">
                                          <span class="slider round"></span>
                                        </label>                                              
                                    </div>       
                                    <div class="col-sm-1 text-left" >                                        
                                      <h2><span id="currency_entry" class="label label-info"></span></h2>      
                                    </div>
                                    <div class="col-sm-3 text-right">
                                        <asp:HiddenField ID="curr_local" runat="server" Value="" />
                                        <asp:HiddenField ID="curr_International" runat="server" Value="" />
                                        <asp:Label runat="server" ID="lblt_exchange_rate" CssClass="control-label text-bold">Exchange Rate:</asp:Label>
                                    </div>
                                    <div class="col-sm-3 text-right">
                                       <telerik:RadNumericTextBox ID="txt_tasa_cambio" runat="server" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                ControlToValidate="txt_tasa_cambio" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                    </div>
                                 </div>

					             <div class="form-group row">
                                             <div style="width:99%;" >
                                                   <!-- TABLE: ACTIVITY DELIVERABLE RECORD -->
                                                      <div class="box box-info">
                                                        <div class="box-header with-border">
                                                          <h3 class="box-title"><asp:Label runat="server" ID="lblt_box1_label" >Registrar Entregable</asp:Label></h3>
                                                          <div class="box-tools pull-right">
                                                            <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>                                                         
                                                          </div>
                                                        </div><!-- /.box-header -->
                                                        <div class="box-body">
                                                          <div >
                                                                                                    
                                                             
                                                        

                            
                            <div class="form-group row">

                                 <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_numero_entregable" CssClass="control-label text-bold">Número</asp:Label>
                                </div>

                                 <div class="col-sm-2">
                                    <telerik:RadNumericTextBox ID="txt_numero_entregable" runat="server" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                ControlToValidate="txt_numero_entregable" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator><br /><br />
                                </div>

                            </div>
                            <div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_descripcion_entregable" CssClass="control-label text-bold">Entregable</asp:Label>
                                </div>
                                <div class="col-sm-6">
                                    <telerik:RadTextBox ID="txt_descripcion_entregable" runat="server" Rows="5" TextMode="MultiLine" Width="99%"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                ControlToValidate="txt_descripcion_entregable" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator><br /><br />
                                </div>
                            </div>
                             <div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_verification" CssClass="control-label text-bold">Verificación del entregable</asp:Label>
                                </div>
                                <div class="col-sm-6">
                                    <telerik:RadTextBox ID="txt_verification_" runat="server" Rows="7" TextMode="MultiLine" Width="99%"></telerik:RadTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                ControlToValidate="txt_verification_" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator><br /><br />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_fecha" CssClass="control-label text-bold">Fecha</asp:Label>
                                </div>
                                <div class="col-sm-2">
                                    <telerik:RadDatePicker ID="dt_fecha" runat="server">
                                        <Calendar runat="server" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x">
                                        </Calendar>
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                ControlToValidate="dt_fecha" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator><br /><br />
                                </div>
                            </div>


                            <div class="form-group row">

                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_valor" CssClass="control-label text-bold">Valor</asp:Label>
                                </div>
                                <div class="col-sm-2">
                                    <asp:HiddenField runat="server" ID="hd_updating" Value ="0" />                                      
                                    <telerik:RadNumericTextBox ID="txt_total_aporte"  MinValue="0"  runat="server">                                                                                                           
                                        <ClientEvents  OnValueChanged="PreventPostback" /> 
                                    </telerik:RadNumericTextBox><span id="span_curr_entry" style="font-weight:600" ></span>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="txt_total_aporte" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                                                        
                                </div>
                                <div class="col-sm-2 text-right">
                                    <asp:Label runat="server" ID="lblt_porcentaje" CssClass="control-label text-bold">Porcentaje</asp:Label>
                                </div>
                                <div class="col-sm-2">
                                    <telerik:RadNumericTextBox ID="txt_porcentaje" runat="server" MinValue="0"
                                        NumberFormat-DecimalDigits="2" DisabledStyle-BackColor="LightGray" DisabledStyle-ForeColor="White">
                                          <ClientEvents  OnValueChanged="porcent_Change" /> 
                                        <NumberFormat ZeroPattern="n" />
                                    </telerik:RadNumericTextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                ControlToValidate="txt_porcentaje" CssClass="Error" Display="Dynamic"
                                                ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator><br /><br />
                                </div>
                            </div>
                          

                            <div class="form-group row"  >
                                   <br />
                                   <div class="col-sm-2 text-right">
                                      <asp:Label ID="lblt_Approval" runat="server" CssClass="control-label text-bold" >Approval Flow</asp:Label>
                                      <asp:HiddenField ID="hd_dtDeliverable_Routes" runat="server" Value="" />
                                   </div>
                                   <div class="col-sm-8 text-left">
                                       <telerik:RadComboBox  ID="cmb_approvals" 
                                                             runat ="server"  
                                                             CausesValidation="False"                                                                     
                                                             EmptyMessage="Select the approval ..."   
                                                             AllowCustomText="true" 
                                                             Filter="Contains"                                         
                                                             Width="50%" >                                                                                               
                                      </telerik:RadComboBox>                                       
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Select the approval flow using for approving this deliverable" ForeColor="Red" ControlToValidate="cmb_approvals" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                                                                                                                    
                                                                                                              
                                   </div> 
                                   <br /><br />
                               </div>
                            
                            <div class="form-group row">
                                <div class="col-sm-2 text-right">
                                    <%--<asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Porcentaje</asp:Label>--%>
                                </div>
                                <div class="col-sm-9">                                   
                                    <telerik:RadButton ID="btn_guardarEntregable" runat="server" CssClass="btn btn-sm" Text="Agregar" ValidationGroup="1" Width="75px">                                    
                                    </telerik:RadButton>
                                    <div class="alert-sm bg-blue col-sm-7" id="div_mensaje" visible="false">
                                        <asp:Label runat="server" ID="lblt_Error" CssClass="text-center text-bold">The value can´t be greater than the funding assign for the project</asp:Label>
                                    </div>
                                </div>
                            </div>


                                                                </div><!-- /.table-responsive -->
                                                        </div><!-- /.box-body -->

                                                          <div class="box-footer clearfix">                                                

                                                                <div class="col-sm-12">
                                                                  <hr style="border-color:black;" />
                                                               </div>    
                                                   
                                                           </div> <!-- /.box-footer -->                                                                                            
                                         
                                                  </div><!-- /.box -->
						                        <!-- TABLE: ACTIVITY DELIVERABLE RECORD -->

                                            </div>
                            
                                        </div>    


                            
					            <div class="form-group row">
                                             <div style="width:99%;" >
                                                     <!-- TABLE: ACTIVITY DELIVERABLEs  -->
                                                      <div class="box box-info">
                                                        <div class="box-header with-border">
                                                          <h3 class="box-title"><asp:Label runat="server" ID="lblt_Box2_Title" >Detalle de Entregables</asp:Label></h3>
                                                          <div class="box-tools pull-right">
                                                            <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                                            <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                                                          </div>
                                                        </div><!-- /.box-header -->
                                                        <div class="box-body">
                                                          <div > <!-- /.table-responsive -->
                                                               
                                                              <%--OnNeedDataSource="grd_aportes_NeedDataSource"--%>
                                                              
                                                              <telerik:RadGrid ID="grd_aportes" runat="server" AllowAutomaticDeletes="True" PageSize="30"
                                                                AllowSorting="True" AutoGenerateColumns="False" >
                                                                <ClientSettings EnableRowHoverStyle="true">
                                                                    <Selecting AllowRowSelect="True"></Selecting>                                                                                                                                       
                                                                </ClientSettings>
                                                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID_ACTIVITY_AW_DELIVERABLES" AllowAutomaticUpdates="True">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="ID_ACTIVITY_AW_DELIVERABLES"
                                                                            DataType="System.Int32"
                                                                            FilterControlAltText="Filter ID_ACTIVITY_AW_DELIVERABLES column"
                                                                            HeaderText="id_aporteFicha" ReadOnly="True"
                                                                            SortExpression="ID_ACTIVITY_AW_DELIVERABLES" UniqueName="ID_ACTIVITY_AW_DELIVERABLES"
                                                                            Visible="False">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn UniqueName="colm_Eliminar">
                                                                            <HeaderStyle Width="10" />
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10"
                                                                                    ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                                    OnClick="EliminarAporte_Click">
                                                                                    <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridBoundColumn DataField="numero_entregable" FilterControlAltText="Filter numero_entregable column"
                                                                            HeaderText="numero_entregable" SortExpression="numero_entregable" UniqueName="numero_entregable" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn FilterControlAltText="Filter numero column"
                                                                            HeaderText="&nbsp;" UniqueName="colm_numero" ItemStyle-Width="40px">
                                                                            <ItemTemplate>
                                                                                <asp:Label runat="server" ID="lblt_No" CssClass="control-label text-bold">No</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                <telerik:RadNumericTextBox ID="txt_numero_ent" runat="server" Width="30px" MinValue="0" AutoPostBack="True" NumberFormat-DecimalDigits="0">
                                                                                </telerik:RadNumericTextBox><br /><br />
                                                                                <asp:Label runat="server" ID="lblt_Fecha2" CssClass="control-label text-bold">Due Date</asp:Label>
                                                                                <telerik:RadDatePicker ID="dt_col_fecha" runat="server">
                                                                                    <Calendar runat="server" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x">
                                                                                    </Calendar>
                                                                                </telerik:RadDatePicker>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                                        ControlToValidate="dt_col_fecha" CssClass="Error" Display="Dynamic"
                                                                                        ErrorMessage="the date is required" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                                                <br /><br />
                                                                                 <asp:Label runat="server" ID="lblt_Valor" CssClass="control-label text-bold">Amount</asp:Label>
                                                                                <telerik:RadNumericTextBox ID="txt_col_total_aporte" runat="server" MinValue="0" >
                                                                                   <ClientEvents  OnValueChanged="Grd_total_Change" /> 
                                                                                   <NumberFormat ZeroPattern="n" />
                                                                                </telerik:RadNumericTextBox>
                                                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                                    ControlToValidate="txt_col_total_aporte" CssClass="Error" Display="Dynamic"
                                                                                    ErrorMessage="the amount is required" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                                                <asp:Label runat="server" ID="lbl_curr_local" CssClass="control-label text-bold">&nbsp;</asp:Label><br /><br />
                                                                                <telerik:RadNumericTextBox ID="txt_col_total_aporteUSD" runat="server" MinValue="0" >
                                                                                   <ClientEvents  OnValueChanged="Grd_total_ChangeUSD" /> 
                                                                                   <NumberFormat ZeroPattern="n" />
                                                                                </telerik:RadNumericTextBox><asp:Label runat="server" ID="lbl_curr_international" CssClass="control-label text-bold">&nbsp;</asp:Label><br /><br />                                                                                
                                                                                <telerik:RadNumericTextBox ID="txt_tasa_cambio_" runat="server" MinValue="0" >
                                                                                   <NumberFormat ZeroPattern="n" />
                                                                                </telerik:RadNumericTextBox>
                                                                                <asp:Label runat="server" ID="lblt_Percent" CssClass="control-label text-bold">Percentage</asp:Label>
                                                                                 <telerik:RadNumericTextBox ID="txt_col_porcentaje" runat="server" MinValue="0" NumberFormat-DecimalDigits="2"
                                                                                    Enabled="false" DisabledStyle-BackColor="LightGray" DisabledStyle-ForeColor="White">
                                                                                    <NumberFormat ZeroPattern="n" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                                            HeaderText="Descripción entregable" UniqueName="colm_descripcion_entregable" >
                                                                            <ItemTemplate>
                                                                                <telerik:RadTextBox ID="txt_col_descripcion_entregable" runat="server" Rows="9" TextMode="MultiLine" Width="99%"></telerik:RadTextBox>
                                                                                 <br /><br />
                                                                                  <telerik:RadComboBox  ID="cmb_deliv_route"  runat ="server" Width="100%">                                                                                               
                                                                                </telerik:RadComboBox>    
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                         <telerik:GridTemplateColumn 
                                                                            HeaderText="Verificación" UniqueName="colm_verification_deliverable" >
                                                                            <ItemTemplate>

                                                                                <telerik:RadTextBox ID="txt_verification_deliverable" runat="server" Rows="9" TextMode="MultiLine" Width="99%"></telerik:RadTextBox>
                                                                                      
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>                                                                                                                                              

                                                                        <telerik:GridBoundColumn DataField="id_tipoDocumento" UniqueName="id_tipoDocumento" Visible="true" Display="false">
                                                                        </telerik:GridBoundColumn>

                                                                        <telerik:GridBoundColumn DataField="descripcion_entregable" FilterControlAltText="Filter descripcion_entregable column"
                                                                            HeaderText="descripcion_entregable" SortExpression="descripcion_entregable" UniqueName="descripcion_entregable" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                         <telerik:GridBoundColumn DataField="descripcion_entregable" FilterControlAltText="Filter descripcion_entregable column"
                                                                            HeaderText="descripcion_entregable" SortExpression="descripcion_entregable" UniqueName="descripcion_entregable" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                      <%--  <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                                            HeaderText="Fecha" UniqueName="colm_Template" ItemStyle-Width="100px">
                                                                            <ItemTemplate>
                                                                                <telerik:RadDatePicker ID="dt_col_fecha" runat="server">
                                                                                    <Calendar runat="server" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x">
                                                                                    </Calendar>
                                                                                </telerik:RadDatePicker>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="15px" />
                                                                        </telerik:GridTemplateColumn>--%>
                                                                        <telerik:GridBoundColumn DataField="fecha" FilterControlAltText="Filter fecha column"
                                                                            HeaderText="fecha" SortExpression="fecha" UniqueName="fecha" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                       <%-- <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumnMeta column"
                                                                            HeaderText="Porcentaje" UniqueName="colm_porcentaje" ItemStyle-Width="100px">
                                                                            <ItemTemplate>
                                                                                <telerik:RadNumericTextBox ID="txt_col_porcentaje" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="2"
                                                                                    AutoPostBack="True" Enabled="false" DisabledStyle-BackColor="LightGray" DisabledStyle-ForeColor="White">
                                                                                    <NumberFormat ZeroPattern="n" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>--%>
                                                                        <telerik:GridBoundColumn DataField="porcentaje" FilterControlAltText="Filter porcentaje column"
                                                                            HeaderText="porcentaje" SortExpression="porcentaje" UniqueName="porcentaje" Visible="false">
                                                                        </telerik:GridBoundColumn>                                              
                                                                        <telerik:GridBoundColumn DataField="deliverable_estado" FilterControlAltText="Filter deliverable_estado column"
                                                                            HeaderText="Estado" SortExpression="deliverable_estado" UniqueName="colm_deliverable_estado" Visible="true" >
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="valor" FilterControlAltText="Filter valor column"
                                                                            HeaderText="valor" SortExpression="valor" UniqueName="valor" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>


                                                                                                   
                                                             
                                                          </div><!-- /.table-responsive -->
                                                        </div><!-- /.box-body -->

                                                          <div class="box-footer clearfix">                                                

                                                                <div class="col-sm-12">
                                                                  <hr style="border-color:black;" />
                                                               </div>    
                                                   
                                                           </div> <!-- /.box-footer -->                                                                                            
                                         
                                                  </div><!-- /.box -->
						                    <!-- TABLE: ACTIVITY DELIVERABLEs  -->

                                            </div>
                            
                                        </div>    


                            <div class="form-group row">
                                <%--<div class="col-sm-2 text-right">
                                </div>--%>
                                <div class="col-sm-12">
                                    
                                </div>
                            </div>
                            <%--<div class="form-group row">
                                <div class="col-sm-12 text-right">
                                    <h4>
                                        <asp:Label runat="server" ID="lblt_total_aporte" CssClass="text-bold">Total Aporte:</asp:Label>
                                        <asp:Label runat="server" ID="lbl_total"></asp:Label>
                                    </h4>
                                    <h4>
                                        <asp:Label runat="server" ID="lblt_total_aporteUSD" CssClass="text-bold">Total Aporte USD:</asp:Label>
                                        <asp:Label runat="server" ID="lbl_totalUSD"></asp:Label>
                                    </h4>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar" CssClass="btn btn-sm pull-right margin-r-5" Enabled="false"
                            Width="100px" ValidationGroup="3">
                        </telerik:RadButton>   
                        <div class="alert-sm bg-blue col-sm-7" id="div_mensajeII" visible="false">
                            <asp:Label runat="server" ID="lblt_ErrorII" CssClass="text-center text-bold">The value can´t be greater than the funding assign for the project</asp:Label>
                        </div>
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
                                    <asp:Button runat="server" ID="btn_eliminarAportes" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" data-dismiss="modal" UseSubmitBehavior="false" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    
    <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts.js")%>"></script>
    <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/highcharts-more.js")%>"></script>
    <script src="<%=ResolveUrl("~/Scripts/Highcharts-4.0.1/js/modules/solid-gauge.js")%>"></script>     
    <script src="<%=ResolveUrl("~/Content/dist/js/aw_chart.js?time=0.0002")%>"></script>
   

    <script>

        $('#div_mensaje').css('display', 'none'); 
        $('#div_mensajeII').css('display', 'none'); 
        var bndUPD = 0;
              
                       
         $(document).ready(function () {
             $('#<%=chk_data_in.ClientID %>').prop('checked', false);//default value LOCA curr       
             Currency_input();
        }); 


        function UpdateItemCountField_aw(sender, args) {
                //set the footer text
                sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " contracts";
        }


        function Currency_input() {
           if ($('#<%=chk_data_in.ClientID %>').is(":checked")) {           
               //alert('Checkeado');               
               var currencySymbol =  $('input[id*=curr_International]');
               $("#currency_entry").html(currencySymbol.val());
               $("#span_curr_entry").html(currencySymbol.val());               
            } else {
               //alert('NO Checkeado');
               var currencySymbol = $('input[id*=curr_local]');
               $("#currency_entry").html(currencySymbol.val());
               $("#span_curr_entry").html(currencySymbol.val());
            }

        }

       function PreventPostback(sender, eventArgs)
       {
           if (bndUPD == 0) {            

               bndUPD = 1;

                   if (eventArgs.get_newValue() == "")
                       eventArgs.set_cancel(true);
                   else {

                       //alert(eventArgs.get_newValue());
                       var ValorEntregableACT = eventArgs.get_newValue().replace(',','.');

                       var masterTable = $find('<%= grd_aportes.ClientID %>');

                       if (masterTable) {
                            var tableView = masterTable.get_masterTableView();
                            var rows = tableView.get_dataItems();
                            var ColSel = 'colm_numero';
                            var ctrl_reported_value = 'monto_proyecto'
                            var valorEntregables = 0;
                            var valorTotal = $('input[id*=' + ctrl_reported_value + ']');

                           var ctrl_tasa_Cambio = $('input[id*=proy_tasa_cambio]');
                           var tasa_cambio = 0
                           var valor_entregable = 0

                           var ctrl_tasa_cambioII = $find('<%= txt_tasa_cambio.ClientID %>');
                           var tasa_cambioII = parseFloat(ctrl_tasa_cambioII.get_value());

                           if (tasa_cambioII == NaN) {
                              tasa_cambio = ctrl_tasa_Cambio.val().replace(',', '.');
                           } else {
                              tasa_cambio = tasa_cambioII
                           }

                          //alert('valor Actual: ' + ValorEntregableACT + ' ' + 'tasa_Cambio: ' + tasa_cambio );

                           //alert('Valor total: ' + parseFloat(ValorEntregableACT));
                           //alert('Tasa de Cambio: ' + tasa_Cambio.val().replace(',', '.'));
                           if ($('#<%=chk_data_in.ClientID %>').is(":checked")) {
                               valor_entregable = parseFloat(ValorEntregableACT) * tasa_cambio;
                           } else {
                               valor_entregable = parseFloat(ValorEntregableACT);
                           }

                           //alert(valor_entregable);

                            for (var i = 0; i < rows.length; i++) { //Row Total
                               // ControlText = $telerik.getElementByClassName(rows[i].get_cell(ColSel), "riTextBox");
                               // alert(ControlText.value);
                                Control1 = rows[i].findControl('txt_col_total_aporte');                                                 
                                valueCTRL = Control1.get_value();
                                //alert(Control1.get_value());
                                valorEntregables += parseFloat(valueCTRL);
                           }

                           //Globalize.parseFloat('0,04')
                           //alert('Actual Entregable :' + parseFloat(ValorEntregableACT));
                           var totEntregables = valorEntregables +  valor_entregable;
                           //alert('valorEntregables :' + totEntregables);
                           //alert('MontoTotal :' + valorTotal.val());
                           //alert(totEntregables + ' > ' + parseFloat(valorTotal.val()));

                           var ctrl_Porcent = $find('<%= txt_porcentaje.ClientID %>');
                           //txt_porcentaje.Value = txt_total_aporte.Value * 100 / valor_total
                           var Val_porcent = ((parseFloat(valor_entregable)/parseFloat(valorTotal.val().replace(',', '.'))) * 100);
                           //alert(Val_porcent);
                           ctrl_Porcent.set_value(Val_porcent);

                          var btn_save = $find("<%= btn_guardarEntregable.ClientID %>");
                          if (totEntregables > parseFloat(valorTotal.val().replace(',', '.'))) {
                               $('#div_mensaje').css('display', 'block');    
                              // $('#<%= btn_guardar.ClientID%>').addClass("disabled");
                              $('#<%= btn_guardarEntregable.ClientID%>').addClass("disabled");
                              btn_save.set_enabled(false);
                      
                           } else {
                              $('#div_mensaje').css('display', 'none');                              
                              // $('#<%= btn_guardar.ClientID%>').removeClass("disabled");
                              $('#<%= btn_guardarEntregable.ClientID%>').removeClass("disabled");
                              btn_save.set_enabled(true);
                           }


                       }               

               }

               bndUPD = 0;

           }//(bndUPD == 0) 

        }

        function porcent_Change(sender, eventArgs) {

            if (bndUPD == 0) {

                if (eventArgs.get_newValue() == "")
                    eventArgs.set_cancel(true);
                else {

                      var masterTable = $find('<%= grd_aportes.ClientID %>');

                    if (masterTable) {

                        bndUPD = 1;
                        var ctrl_reported_value = 'monto_proyecto'
                        var valPorcent = eventArgs.get_newValue().replace(',','.');
                        var valorTotal = $('input[id*=' + ctrl_reported_value + ']');
                        var Val_monto = (parseFloat(valorTotal.val().replace(',','.')) * (parseFloat(valPorcent) / 100));

                        var tableView = masterTable.get_masterTableView();
                        var rows = tableView.get_dataItems();

                        var valueMonto_fix = 0;
                        var ctrl_tasa_Cambio = $('input[id*=proy_tasa_cambio]');
                        var tasa_cambio = 0

                         var ctrl_tasa_cambioII = $find('<%= txt_tasa_cambio.ClientID %>');
                         var tasa_cambioII = parseFloat(ctrl_tasa_cambioII.get_value());

                           if (tasa_cambioII == NaN) {
                              tasa_cambio = ctrl_tasa_Cambio.val().replace(',', '.');
                           } else {
                              tasa_cambio = tasa_cambioII
                           }

                       
                          if ($('#<%=chk_data_in.ClientID %>').is(":checked")) {
                               valueMonto_fix = parseFloat(Val_monto) / tasa_cambio;
                           } else {
                               valueMonto_fix =  parseFloat(Val_monto);
                           }
                                               

                        //alert('Val monto: ' + Val_monto);
                        var ctrl_Value = $find('<%= txt_total_aporte.ClientID %>');
                        ctrl_Value.set_value(valueMonto_fix);
                        var valorEntregables = 0;

                        for (var i = 0; i < rows.length; i++) { //Row Total
                               // ControlText = $telerik.getElementByClassName(rows[i].get_cell(ColSel), "riTextBox");
                               // alert(ControlText.value);
                                var Control1 = rows[i].findControl('txt_col_total_aporte');                                                 
                                //alert(Control1.get_value());
                                var valueCTRL = Control1.get_value();
                                valorEntregables += parseFloat(valueCTRL);
                        }

                        var totEntregables = valorEntregables + Val_monto;

                        //alert('total actividad: ' + valorTotal.val().replace(',','.') + ' Valor agregar: ' + Val_monto + ' valor entregables:' +  valorEntregables  + ' Valor Ajustado:' + valueMonto_fix  + ' porcentaje:' + valPorcent +  'tasa_Cambio: ' + tasa_cambio );
                        
                         var btn_save = $find("<%= btn_guardarEntregable.ClientID %>");
                        if (totEntregables > parseFloat(valorTotal.val().replace(',','.'))) {
                            $('#div_mensaje').css('display', 'block');
                            // $('#<%= btn_guardar.ClientID%>').addClass("disabled");
                            $('#<%= btn_guardarEntregable.ClientID%>').addClass("disabled");                            
                            btn_save.set_enabled(false);

                        } else {
                            $('#div_mensaje').css('display', 'none');
                            // $('#<%= btn_guardar.ClientID%>').removeClass("disabled");
                            $('#<%= btn_guardarEntregable.ClientID%>').removeClass("disabled");
                            btn_save.set_enabled(true);
                        }

                        // Dim valor_total = Me.monto_proyecto.Value
                        // Me.txt_total_aporte.Value = valor_total * txt_porcentaje.Value / 100

                        bndUPD = 0;

                    } //if (masterTable)

                }

            }// bndUPD == 1;

        }


        function Grd_total_Change(sender, eventArgs) {

            if (bndUPD == 0) {

                if (eventArgs.get_newValue() == "")
                    eventArgs.set_cancel(true);
                else {

                     var masterTable = $find('<%= grd_aportes.ClientID %>');

                    if (masterTable) {

                        bndUPD = 1;
                        var tableView = masterTable.get_masterTableView();
                        var rows = tableView.get_dataItems();
                                     
                        var ctrl_reported_value = 'monto_proyecto'
                        var valorEntregables = 0;
                        var valorEntregablesUSD = 0;
                        var valueMonto_fix = 0;
                        var valorTotal = $('input[id*=' + ctrl_reported_value + ']');

                        //alert(valorTotal.val().replace(',','.'));

                            for (var i = 0; i < rows.length; i++) { //Row Total
                                   // ControlText = $telerik.getElementByClassName(rows[i].get_cell(ColSel), "riTextBox");
                                   // alert(ControlText.value);
                                var Control1 = rows[i].findControl('txt_col_total_aporte');  
                                var valueCTRL = Control1.get_value();

                                var Control2 = rows[i].findControl('txt_col_total_aporteUSD');  
                                var valueCTRL_usd = Control2.get_value();

                                var Control3 = rows[i].findControl('txt_tasa_cambio_');  
                                var valueCTRL_exchange = Control3.get_value();
                                                                
                                var Control4 = rows[i].findControl('txt_col_porcentaje');  
                                //var valueCTRL_porcentage = Control4.get_value();

                                valueMonto_fix = parseFloat(valueCTRL) / parseFloat(valueCTRL_exchange);
                                Control2.set_value(valueMonto_fix);

                                Control4.set_value((valueCTRL / parseFloat(valorTotal.val().replace(',', '.')))*100);
                                //alert(Control3.get_value());

                                valorEntregables += parseFloat(valueCTRL);
                                valorEntregablesUSD += parseFloat(valueCTRL_usd);

                            }

                         var btn_save = $find("<%= btn_guardar.ClientID %>");
                         if (valorEntregables > parseFloat(valorTotal.val().replace(',','.'))) {
                                    $('#div_mensajeII').css('display', 'block');    
                                    $('#<%= btn_guardar.ClientID%>').addClass("disabled");
                                    btn_save.set_enabled(false);
                                 <%--  $('#<%= btn_guardarEntregable.ClientID%>').addClass("disabled");--%>
                      
                          } else {
                                  $('#div_mensajeII').css('display', 'none');                              
                                  $('#<%= btn_guardar.ClientID%>').removeClass("disabled");
                                  btn_save.set_enabled(true);
                                 <%-- $('#<%= btn_guardarEntregable.ClientID%>').removeClass("disabled");--%>
                        }

                        bndUPD = 0;

                    }//if (masterTable) {

                 }

              }// bndUPD == 1;

        }


          function Grd_total_ChangeUSD(sender, eventArgs) {

              if (bndUPD == 0) {


                        if (eventArgs.get_newValue() == "")
                            eventArgs.set_cancel(true);
                        else {

                             var masterTable = $find('<%= grd_aportes.ClientID %>');

                            if (masterTable) {

                                bndUPD = 1;
                                var tableView = masterTable.get_masterTableView();
                                var rows = tableView.get_dataItems();
                                     
                                var ctrl_reported_value = 'monto_proyecto'
                                var valorEntregables = 0;
                                var valorEntregablesUSD = 0;
                                var valueMonto_fix = 0;
                                var valorTotal = $('input[id*=' + ctrl_reported_value + ']');

                                //alert(valorTotal.val().replace(',','.'));

                                    for (var i = 0; i < rows.length; i++) { //Row Total
                                           // ControlText = $telerik.getElementByClassName(rows[i].get_cell(ColSel), "riTextBox");
                                           // alert(ControlText.value);
                                        var Control1 = rows[i].findControl('txt_col_total_aporte');  
                                        var valueCTRL = Control1.get_value();

                                        var Control2 = rows[i].findControl('txt_col_total_aporteUSD');  
                                        var valueCTRL_usd = Control2.get_value();

                                        var Control3 = rows[i].findControl('txt_tasa_cambio_');  
                                        var valueCTRL_exchange = Control3.get_value();

                                         var Control4 = rows[i].findControl('txt_col_porcentaje');  
                                         //var valueCTRL_porcentage = Control4.get_value();
                                       
                                       // alert('Monto-USD:' + parseFloat(valueCTRL_usd) + ' Exchange:' + parseFloat(valueCTRL_exchange));
                                        valueMonto_fix = parseFloat(valueCTRL_usd) * parseFloat(valueCTRL_exchange);
                                        Control1.set_value(valueMonto_fix);
                                                                   
                                        Control4.set_value((valueMonto_fix / parseFloat(valorTotal.val().replace(',', '.'))) * 100);
                          

                                        valorEntregables += parseFloat(valueMonto_fix);
                                        valorEntregablesUSD += parseFloat(valueCTRL_usd);

                                    }

                                var btn_save = $find("<%= btn_guardar.ClientID %>");

                               // alert(valorEntregables + ' > ' + parseFloat(valorTotal.val()));

                                 if (valorEntregables > parseFloat(valorTotal.val().replace(',','.'))) {
                                            $('#div_mensajeII').css('display', 'block');    
                                            $('#<%= btn_guardar.ClientID%>').addClass("disabled");
                                            btn_save.set_enabled(false);
                                         <%--  $('#<%= btn_guardarEntregable.ClientID%>').addClass("disabled");--%>
                      
                                  } else {
                                          $('#div_mensajeII').css('display', 'none');                              
                                          $('#<%= btn_guardar.ClientID%>').removeClass("disabled");
                                          btn_save.set_enabled(true);
                                         <%-- $('#<%= btn_guardarEntregable.ClientID%>').removeClass("disabled");--%>
                                }


                             bndUPD = 0;

                            }//if (masterTable) {

                        }

                    
              }// bndUPD == 1;

        }
                 

     </script>                                             
</asp:Content>

