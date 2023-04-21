<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_contratosModEntregables.aspx.vb" Inherits="RMS_APPROVAL.frm_contratosModEntregables" %>
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
    </style>
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <uc:ReturnConfirm runat="server" ID="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Contratistas</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Registrar entregable</asp:Label></h3>
                <asp:HiddenField runat="server" ID="idModificacion" Value="0" />
                <asp:Label ID="identity" runat="server" Text="0" CssClass="deleteIdentity" data-id="" Visible="false" />
                <%--<asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />--%>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                        <div class="stepwizard">
                            <div class="stepwizard-row setup-panel">
                                <div class="stepwizard-step" style="width:33%">
                                    <a href="#step-1" id="anchorMod" runat="server" type="button" class="btn btn-default btn-circle">1</a>
                                    <p>
                                        <asp:Label runat="server" ID="Label4">Información de la modificación</asp:Label>
                                    </p>
                                </div>
                                <div class="stepwizard-step" style="width:33%">
                                    <a href="#step-1" id="anchorInformation" runat="server" type="button" class="btn btn-default btn-circle">2</a>
                                    <p>
                                        <asp:Label runat="server" ID="lblt_informaciongeneral">Información general</asp:Label>
                                    </p>
                                </div>
                                <div class="stepwizard-step" style="width:33%">
                                    <a   href="#step-2" id="anchorBillable" runat="server" type="button" class="btn btn-primary btn-circle">3</a>
                                    <p>
                                        <asp:Label runat="server" ID="lblt_personal_status">Entregables</asp:Label>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <hr />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="info-box bg-gray-light">
                                    <span class="info-box-icon bg-gray-active"><i class="fa fa-money"></i></span>
                                    <div class="info-box-content">
                                        <span class="info-box-text">
                                            <span id="MainContent_lblt_total" class="control-label text-bold">Valor del contrato</span></span>
                                            <span class="info-box-text">
                                                <asp:Label runat="server" ID="lbl_total"></asp:Label>
                                            </span>
                                    </div>
                                    <!-- /.info-box-content -->
                                </div>
                                <!-- /.info-box -->
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="info-box bg-gray-light">
                                    <span class="info-box-icon bg-gray-active"><i class="fa fa-money"></i></span>
                                    <div class="info-box-content">
                                        <span class="info-box-text">
                                            <span id="MainContent_lblt_sumatoria" class="control-label text-bold">Sumatoria de los entregables</span></span>
                                        <span class="info-box-text">
                                            <asp:Label runat="server" ID="lbl_sumatoria"></asp:Label>
                                        </span>
                                    </div>
                                    <!-- /.info-box-content -->
                                </div>
                                <!-- /.info-box -->
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="info-box bg-gray-light">
                                    <span class="info-box-icon bg-gray-active"><i class="fa fa-money"></i></span>
                                    <div class="info-box-content">
                                        <span class="info-box-text">
                                            <span id="MainContent_lblt_diferencia" class="control-label text-bold">Diferencia</span></span>
                                        <span class="info-box-text">
                                            <asp:Label runat="server" ID="lbl_diferencia"></asp:Label>
                                        </span>
                                    </div>
                                    <!-- /.info-box-content -->
                                </div>
                                <!-- /.info-box -->
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label24" CssClass="control-label text-bold">Número de entregable</asp:Label>
                                <br />
                                <telerik:RadNumericTextBox ID="txt_numero_entregable" runat="server"  Width="90%" MinValue="1">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server"
                                            ControlToValidate="txt_numero_entregable" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Nombre del entregable</asp:Label>
                                <br />
                                <telerik:RadTextBox ID="txt_nombre_entregble" runat="server"  Width="90%" MinValue="1">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="txt_nombre_entregble" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>

                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label34" CssClass="control-label text-bold">Fecha esperada de entrega</asp:Label>
                                <br />
                                <telerik:RadDatePicker ID="dt_fecha_entrega" AutoPostBack="false" Width="90%" Enabled="true" runat="server" >
                                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"  ViewSelectorText="x"></Calendar>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server"
                                            ControlToValidate="dt_fecha_entrega" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3">
                                <asp:Label runat="server" ID="Label27" CssClass="control-label text-bold">Valor del entregable</asp:Label>
                                <br />
                                 <telerik:RadNumericTextBox ID="txt_valor_entregable" runat="server"  Width="90%" MaxLength="1000">
                                </telerik:RadNumericTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                                            ControlToValidate="txt_valor_entregable" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                          
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <asp:Label runat="server" ID="Label3" CssClass="control-label text-bold">Productos</asp:Label>
                                <br />
                                 <telerik:RadTextBox ID="txt_productos" runat="server" Rows="3" TextMode="MultiLine" Width="98%" MaxLength="1000">
                                </telerik:RadTextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                            ControlToValidate="txt_productos" CssClass="Error" Display="Dynamic"
                                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <telerik:RadButton ID="btn_agregar_entregable" runat="server" Text="Agregar entregable" Enabled="false" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5"
                            ValidationGroup="1">
                        </telerik:RadButton>
                        <asp:Label ID="lblerrorG" runat="server" CssClass="Error pull-right" Visible="False">* Complete campos</asp:Label>
                    </div>

                     <div class="form-group row">
                         <hr />
                         <div class="col-sm-12">
                                    <telerik:RadGrid ID="grd_entregables" runat="server" AllowAutomaticDeletes="True"  CellSpacing="0" AllowPaging="True" PageSize="100" AutoGenerateColumns="False" Width="100%"
                                        ShowFooter="true" 
                                        ShowColumnFooters="true"
                                        ShowGroupFooters="true"
                                        ShowGroupPanel="false">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="True"></Selecting>                                  
                                            <Resizing AllowColumnResize="true" AllowResizeToFit="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_contrato_entregable" AllowAutomaticUpdates="True" >
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_contrato_entregable"
                                                    FilterControlAltText="Filter id_contrato_entregable column"
                                                    SortExpression="id_contrato_entregable" UniqueName="id_contrato_entregable"
                                                    Visible="False" HeaderText="id_contrato_entregable"
                                                    ReadOnly="True">
                                                </telerik:GridBoundColumn>

                                                 <telerik:GridBoundColumn DataField="id_estado_aprobacion_entregable"
                                                    DataType="System.Int32"
                                                    FilterControlAltText="Filter id_estado_aprobacion_entregable column"
                                                    HeaderText="id_estado_aprobacion_entregable" ReadOnly="True"
                                                    SortExpression="id_estado_aprobacion_entregable" UniqueName="id_estado_aprobacion_entregable"
                                                    Visible="False">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridTemplateColumn UniqueName="colm_Eliminar"  HeaderStyle-Width="4%">
                                                    <HeaderStyle  />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10"
                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar"
                                                            >
                                                            <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="colm_editar"  HeaderStyle-Width="4%">
                                                    <HeaderStyle  />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="col_hlk_editar" runat="server" Width="10"
                                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Editar"
                                                            OnClick="Editar_Click">
                                                            <asp:Image ID="img_editar" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" Style="border-width: 0px;" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>


                                                <telerik:GridBoundColumn DataField="numero_entregable"
                                                    FilterControlAltText="Filter numero_entregable column" HeaderStyle-Width="12%"
                                                    HeaderText="Nro Entregable" SortExpression="numero_entregable"
                                                    UniqueName="colm_cantidad">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="nombre_entregable"
                                                    FilterControlAltText="Filter nombre_entregable column" HeaderStyle-Width="15%"
                                                    HeaderText="Entregable" SortExpression="nombre_entregable"
                                                    UniqueName="colm_descripcion">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                
                                                <telerik:GridBoundColumn DataField="fecha_esperada_entrega"  DataFormatString="{0:MM/dd/yyyy}"
                                                    FilterControlAltText="Filter fecha_esperada_entrega column" HeaderStyle-Width="17%"
                                                    HeaderText="Fecha esperada de entrega" SortExpression="fecha_esperada_entrega"
                                                    UniqueName="colm_fecha_esperada_entrega">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="dias_restantes"
                                                    FilterControlAltText="Filter dias_restantes column" HeaderStyle-Width="10%"
                                                    HeaderText="Dias faltantes" SortExpression="dias_restantes"
                                                    UniqueName="colm_dias_restantes">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="estado"
                                                    FilterControlAltText="Filter estado column" HeaderStyle-Width="8%"
                                                    HeaderText="Estado" SortExpression="estado"
                                                    UniqueName="colm_estado">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="productos"
                                                    FilterControlAltText="Filter productos column" HeaderStyle-Width="47%"
                                                    HeaderText="Productos" SortExpression="productos"
                                                    UniqueName="colm_productos">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="valor_entregable" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    Aggregate="Sum" DataFormatString="{0:n}" FooterAggregateFormatString="<b>Valor total: {0:n}</b>" FooterText="Valor total: "
                                                    FilterControlAltText="Filter valor_entregable column" HeaderStyle-Width="19%"
                                                    HeaderText="Valor total" SortExpression="valor_entregable"
                                                    UniqueName="colm_valor">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="alerta"
                                                    FilterControlAltText="Filter alerta column" HeaderStyle-Width="5%"
                                                    HeaderText="Alerta" SortExpression="alerta"
                                                    UniqueName="alerta">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                               <%-- <telerik:GridTemplateColumn UniqueName="colm_activar" Visible="true"  HeaderStyle-Width="4%">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlk_activar" runat="server" ImageUrl="~/Imagenes/iconos/additem.png"
                                                            ToolTip="Agregar entregables">
                                                        </asp:HyperLink>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>--%>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                               
                        
                     </div>
                     <div class="form-group row">
                         <hr />
                         <div class="col-sm-6 col-md-6 col-lg-6"></div>
                          <div class="col-sm-6 col-md-6 col-lg-6 text-right" visible="true" runat="server" id="errorTotalContrato">
                                <div class="alert alert-danger" role="alert">
                                    El valor total del contrato es diferente a la sumatoria del valor de los entregables
                                </div>
                            </div>

                     </div>
                     <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                         <telerik:RadButton ID="btn_guardar2" runat="server" Text="Guardar y finalizar modificación" AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_guardar" runat="server" Text="Guardar " AutoPostBack="true" CssClass="btn btn-sm pull-right margin-r-5">
                        </telerik:RadButton>
                        <asp:Label ID="Label2" runat="server" CssClass="Error pull-right" Visible="False">* Complete campos</asp:Label>
                    </div>
                </div>
                <div class="modal fade" id="modalConfirm" data-backdrop="static" data-keyboard="false">
                    <div class="vertical-alignment-helper">
                        <div class="modal-dialog  modal-dialog-centered">
                            <div class="modal-content modal-lg ">
                                <div class="modal-header modal-danger">
                                    <h4 class="modal-title" runat="server" id="esp_ctrl_h4_eliminar_titulo">Eliminar concepto</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="esp_ctrl_lbl_eliminar" runat="server" Text="Desea eliminar el registro?" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" Text="Delete" />
                                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancel</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script src="../Scripts/FileUploadTelerik.js"></script>
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function (s, e) {
        })
        window.onClientFileUploaded = function (radAsyncUpload, args) {
        }
    </script>
</asp:Content>