<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_proyecto_ruta_aprobacion.aspx.vb" Inherits="RMS_APPROVAL.frm_proyecto_ruta_aprobacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <style>
        .RadUpload_Office2007 .ruStyled .ruFileInput{
            border-color: #e5e5e5;
        }
        .RadUpload_Office2007 input.ruFakeInput{
            border-color: #e5e5e5;
        }
        .RadUpload_Office2007 .ruButton{
            border: 1px solid #e5e5e5;
            color: #767676;
            background-color: #fff;
            background-image:none
        }
        div.qsf-right-content .qsf-col-wrap 
        {
            position: static;
        }
 
        .rgEditForm {
            width: auto !important;
        }
 
        * + html .rgEditForm.popUpEditForm {
            width: 800px !important;
        }
 
        .rgEditForm > div + div,
        .RadGrid .rgEditForm {
            height: auto !important;
        }
 
        .rgEditForm > div > table{
            height: 100%;
     
        }
 
        .rgEditForm > div > table > tbody > tr > td {
            padding: 4px 10px;
        }
 
        .rfdSelectBoxDropDown {
            z-index: 100011;
        }
        .rwContent
        {
            overflow-x: hidden !important;
        }
        #ctl00_MainContent_RadWindow2_C_soporte_adjuntoListContainer{
            float: left;
        }
    </style>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rbSubmitChanges">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" />
                    <telerik:AjaxUpdatedControl ControlID="RadWindow2" />
                    <telerik:AjaxUpdatedControl ControlID="btn_agregar_concepto" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Ficha de Actividades</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                 <div class="col-sm-11">   
                   <h3 class="box-title">
                        <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Proyecto - Aportes</asp:Label>
                        <asp:Label runat="server" ID="lbl_informacionproyecto"></asp:Label>
                        <asp:Label ID="lbl_id_sesion_temp" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
                    </h3>
                  </div>
                  <%--<div class="col-sm-1 text-right">   
                       <asp:LinkButton ID="lnk_help" Text="Try" Width="12%" class="btn btn-default btn-sm margin-r-5" data-toggle="Try" OnClick="showhelp();" ><i class="fa fa-question-circle fa-2x"></i>&nbsp;&nbsp;</asp:LinkButton>   
                  </div>--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="col-lg-12">
                            <asp:HiddenField ID="id_rol" runat="server" value="0" />
                            <asp:HiddenField ID="Hiddenindi" runat="server" />
                            <ul class="nav nav-tabs">
                                <li role="presentation"><a runat="server" id="alink_definicion" href="#">Información General</a></li>
                                <%--<li role="presentation"><a runat="server" id="alink_regionbeneficiada">Región Beneficiada</a></li>--%>
                                <%--<li role="presentation"><a runat="server" id="alink_value_chain">QASP</a></li>--%>
                                <%--<li role="presentation"><a runat="server" id="alink_areas">Información complementaria</a></li>
                                <li role="presentation" class="hidden"><a runat="server" id="alink_upm">UPM</a></li>
                                <li role="presentation" class="hidden"><a runat="server" id="alink_predios">Predios</a></li>
                                <li role="presentation"><a runat="server" id="alink_indicadores">Indicadores</a></li>--%>
                                <li role="presentation"><a runat="server" id="alink_aportes">Aportes</a></li>
                                 <li role="presentation" class="active"><a runat="server" class="primary" href="#" id="link_ruta">Ruta aprobación</a></li>
                               <%-- <li role="presentation"><a runat="server" id="alink_documentos">Documentos</a></li>
                                <li role="presentation"><a runat="server" id="alink_entregables">Entregables</a></li>
                                <li role="presentation"><a runat="server" id="alink_waiver">Waiver</a></li>
                                <li role="presentation"><a runat="server" id="alink_stos">STO</a></li>
                                <li role="presentation"><a runat="server" id="alink_po">PO</a></li>
                                <li role="presentation"><a runat="server" id="alink_Ik">IN KIND</a></li>--%>
                            </ul>
                        </div>
                        <div class="form-group row" style="margin-bottom: 0px;">
                        </div>
                        <asp:Label ID="Label1" runat="server" Text="" Visible="false" />

                        <div class="panel-body div-bordered">
                            <div class="form-group row">
                                <div class="col-sm-12 text-right">
                                 <telerik:RadButton ID="add_rol" runat="server" Text="Agregar rol" Enabled="true" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5">
                                    </telerik:RadButton>

                                </div>
                            </div>

                            <div class="form-group row">
                                    <div class="col-sm-12">
                                        <telerik:RadGrid ID="grd_roles" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                            ShowFooter="false" 
                                            ShowColumnFooters="false"
                                            ShowGroupFooters="false"
                                            ShowGroupPanel="false">
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Selecting AllowRowSelect="True"></Selecting>                                  
                                                <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_rol_aprobacioh_hito" AllowAutomaticUpdates="True" >
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="id_rol_aprobacioh_hito"
                                                        FilterControlAltText="Filter id_rol_aprobacioh_hito column"
                                                        SortExpression="id_rol_aprobacioh_hito" UniqueName="id_rol_aprobacioh_hito"
                                                        Visible="False" HeaderText="id_rol_aprobacioh_hito"
                                                        ReadOnly="True">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn UniqueName="colm_editar" Visible="true">
                                                        <HeaderStyle Width="32px" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="col_hlk_editar" runat="server" Width="32px"
                                                                ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Eliminar"
                                                                OnClick="editar_ruta">
                                                                <asp:Image ID="Image1" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" Style="border-width: 0px;" />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="descripcion"
                                                        FilterControlAltText="Filter descripcion column" HeaderStyle-Width="30%"
                                                        HeaderText="Rol" SortExpression="descripcion"
                                                        UniqueName="descripcion">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="orden"
                                                        FilterControlAltText="Filter orden column" HeaderStyle-Width="30%"
                                                        HeaderText="Orden" SortExpression="orden"
                                                        UniqueName="orden">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="usuarios"
                                                        FilterControlAltText="Filter usuarios column" HeaderStyle-Width="30%"
                                                        HeaderText="Usuarios" SortExpression="usuarios"
                                                        UniqueName="usuarios">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="tipo"
                                                        FilterControlAltText="Filter tipo column" HeaderStyle-Width="30%"
                                                        HeaderText="Tipo" SortExpression="tipo"
                                                        UniqueName="tipo">
                                                        <HeaderStyle CssClass="wrapWord"  />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </div>
                        </div>

                        <telerik:RadWindowManager runat="Server" ID="RadWindowManager1" EnableViewState="true"  Width="700" Height="400">
                                <Windows>
                                     <telerik:RadWindow RenderMode="Lightweight" runat="server" Width="700" Behaviors="Close, Pin, Move" Height="400" id="RadWindow2" Modal="true" EnableShadow="false" VisibleOnPageLoad="false" CssClass="windowcss">
                                         <ContentTemplate>
                                               <div class="form-group row">
                                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <asp:Label runat="server" ID="lbl_tipo_actividad2" CssClass="control-label text-bold">Rol</asp:Label>
                                                        <br />
                                                         <telerik:RadTextBox ID="txt_rol" runat="server"  Width="90%" MaxLength="1000">
                                                        </telerik:RadTextBox>
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator3ss" runat="server"
                                                                    ControlToValidate="txt_rol" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Orden</asp:Label>
                                                        <br />
                                                         <telerik:RadNumericTextBox ID="txt_orden" runat="server" decimal Width="90%" MinValue="0">
                                                             <NumberFormat AllowRounding="true" DecimalDigits="0" />
                                                        </telerik:RadNumericTextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                    ControlToValidate="txt_orden" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                   <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Tipo de aprobación</asp:Label>
                                                        <br />
                                                        <asp:RadioButtonList ID="rbn_tipo_aprobacion" runat="server" RepeatDirection="Horizontal" >
                                                            <asp:ListItem Text="Visto bueno" Value="Visto bueno"></asp:ListItem>
                                                            <asp:ListItem Text="Aprobación" Value="Aprobado"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                    ControlToValidate="rbn_tipo_aprobacion" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                   <div class="col-sm-6 col-md-6 col-lg-4">
                                                        <asp:Label runat="server" ID="Label4" CssClass="control-label text-bold">Aprobación ultimo entregable</asp:Label>
                                                        <br />
                                                        <asp:RadioButtonList ID="rbn_ultimo_entregable" runat="server" RepeatDirection="Horizontal" >
                                                            <asp:ListItem Text="Sí" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                    ControlToValidate="rbn_ultimo_entregable" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="2">*</asp:RequiredFieldValidator>
                                                    </div>
                                                   
                                                    
                                                </div>
                                                <div class="form-group row">
                                                    <hr />
                                                </div>
                                                 <div class="form-group row">
                                                     <div class="col-sm-6 col-md-6 col-lg-5">
                                                        <asp:Label runat="server" ID="Label31" CssClass="control-label text-bold">Personal asociado</asp:Label>
                                                        <br />

                                                         <telerik:RadComboBox ID="cmb_personal_asociado" emptymessage="Seleccione..." AutoPostBack="false" Filter="Contains" runat="server" Width="90%">
                                                        </telerik:RadComboBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2222" runat="server"
                                                                    ControlToValidate="cmb_personal_asociado" CssClass="Error" Display="Dynamic"
                                                                    ErrorMessage="Campo obligatorio" ValidationGroup="3">*</asp:RequiredFieldValidator>
                                                    </div>
                                                     <div class="col-sm-4">
                                                        <telerik:RadButton ID="btn_agregar_persona" runat="server" autopostback="true" CssClass="btn btn-sm" Text="Agregar persona" ValidationGroup="3" Width="100px" style="margin-top:10px;">
                                                        </telerik:RadButton>
                                                    </div>
                                                </div>
                                               <div class="form-group row">
                                                    <div class="col-sm-12">
                                                        <telerik:RadGrid ID="grd_conceptos" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                                            ShowFooter="false" 
                                                            ShowColumnFooters="false"
                                                            ShowGroupFooters="false"
                                                            ShowGroupPanel="false">
                                                            <ClientSettings EnableRowHoverStyle="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>                                  
                                                                <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_rol_usuario_aprobacion" AllowAutomaticUpdates="True" >
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="id_rol_usuario_aprobacion"
                                                                        FilterControlAltText="Filter id_rol_usuario_aprobacion column"
                                                                        SortExpression="id_rol_usuario_aprobacion" UniqueName="id_rol_usuario_aprobacion"
                                                                        Visible="False" HeaderText="id_rol_usuario_aprobacion"
                                                                        ReadOnly="True">
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridTemplateColumn UniqueName="colm_Eliminar" Visible="true">
                                                                        <HeaderStyle Width="4%" />
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="32px"
                                                                                ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                                                OnClick="delete_concepto">
                                                                                <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridBoundColumn DataField="nombre_usuario"
                                                                        FilterControlAltText="Filter nombre_usuario column" HeaderStyle-Width="30%"
                                                                        HeaderText="Usuario" SortExpression="nombre_usuario"
                                                                        UniqueName="nombre_usuario">
                                                                        <HeaderStyle CssClass="wrapWord"  />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="cargo"
                                                                        FilterControlAltText="Filter cargo column" HeaderStyle-Width="30%"
                                                                        HeaderText="Cargo" SortExpression="cargo"
                                                                        UniqueName="cargo">
                                                                        <HeaderStyle CssClass="wrapWord"  />
                                                                    </telerik:GridBoundColumn>
                                                                    <telerik:GridBoundColumn DataField="correo"
                                                                        FilterControlAltText="Filter correo column" HeaderStyle-Width="30%"
                                                                        HeaderText="Correo" SortExpression="correo"
                                                                        UniqueName="correo">
                                                                        <HeaderStyle CssClass="wrapWord"  />
                                                                    </telerik:GridBoundColumn>
                                                                   <telerik:GridBoundColumn  DataField="id_usuario"  FilterControlAltText="Filter id_usuario column"  SortExpression="id_usuario" UniqueName="id_usuario" Visible="true" Display="false"></telerik:GridBoundColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </div>
                                                 <div class="form-group row">
                                                    <hr />
                                                </div>
                                                <div class="form-group row">
                                                    <div class="col-sm-12">
                                                       
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <div class="col-sm-8"></div>
                                                    <div class="col-sm-4 text-right">
                                                       <telerik:RadButton ID="btn_agregar_rol" runat="server" ValidationGroup="2" AutoPostBack="true" Text="Guardar rol" Width="100px" CssClass="btn btn-sm btn-primary btn-ok import">
                                                    </telerik:RadButton>
                                                    </div>
                                                </div>
                                         </ContentTemplate>
                                     </telerik:RadWindow>

                                </Windows>
                            </telerik:RadWindowManager>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                            </telerik:RadButton>
                            <telerik:RadButton ID="btn_guardar_" runat="server" Text="Guardar" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5" Enabled="true"
                                Width="100px" ValidationGroup="1">
                            </telerik:RadButton>
                        </div>
                    </div>
                    <!-- /.box-footer -->

                </div>
            </div>
        </div>
     

        <script>

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

