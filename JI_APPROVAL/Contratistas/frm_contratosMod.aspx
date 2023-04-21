<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_contratosMod.aspx.vb" Inherits="RMS_APPROVAL.frm_contratosMod" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ReturnConfirm" Src="~/Controles/DeleteConfirm.ascx" %>


<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla_">Modificaciones del contrato</asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="col-lg-12">
                    <div class="box-body">
                       <div class ="row">
                           <asp:HiddenField runat="server" ID="idContrato" Value="0" />
                            <div class="col-sm-12 text-left ">
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_cod_Acti" runat="server"  CssClass="control-label text-bold" Text="Código del contrato"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                            <asp:Label ID="lbl_cod_contrato" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_actividad" runat="server"  CssClass="control-label text-bold" Text="Contratista"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_contratista" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_fecha_inicio" runat="server"  CssClass="control-label text-bold" Text="Fecha de inicio"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_inicio" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_fecha_fin" runat="server"  CssClass="control-label text-bold" Text="Fecha de finalización"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_fecha_fin" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="lblt_valor_contrato" runat="server"  CssClass="control-label text-bold" Text="Valor del contrato"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_valor" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="Label2" runat="server"  CssClass="control-label text-bold" Text="Supervisor"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_supervisor" runat="server" ></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-left">
                                        <asp:Label ID="Label4" runat="server"  CssClass="control-label text-bold" Text="Objeto del contrato"></asp:Label>
                                    </div>
                                    <div class="col-sm-9">
                                        <!--Control -->
                                        <asp:Label ID="lbl_objeto" runat="server" ></asp:Label>
                                    </div>
                                </div>
                            </div> 
                        </div>
                        <asp:Label ID="Label3" runat="server" Text="" Visible="false" />
                        <hr />
                        <div runat="server" id="modificacionesList">
                            <div class="row">
                                <div class="col-sm-12 text-right">
                                    <h4 class="text-center"><asp:Label runat="server" ID="Label5" CssClass="control-label text-bold">Modificaciones</asp:Label></h4>
                                    <hr class="box box-primary" />
                                </div>
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
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_modificacion_contrato" AllowAutomaticUpdates="True" >
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id_modificacion_contrato"
                                                    FilterControlAltText="Filter id_modificacion_contrato column"
                                                    SortExpression="id_modificacion_contrato" UniqueName="id_modificacion_contrato"
                                                    Visible="False" HeaderText="id_modificacion_contrato"
                                                    ReadOnly="True">
                                                </telerik:GridBoundColumn> 
                                                <telerik:GridBoundColumn DataField="fecha_modificacion"  DataFormatString="{0:MM/dd/yyyy}"
                                                    FilterControlAltText="Filter fecha_modificacion column" HeaderStyle-Width="17%"
                                                    HeaderText="Fecha de modificación" SortExpression="fecha_modificacion"
                                                    UniqueName="fecha_modificacion">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="objetivo_modificacion"
                                                    FilterControlAltText="Filter objetivo_modificacion column" HeaderStyle-Width="30%"
                                                    HeaderText="Objetivo de la modificación" SortExpression="objetivo_modificacion"
                                                    UniqueName="objetivo_modificacion">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="tipo_modificacion"
                                                    FilterControlAltText="Filter tipo_modificacion column" HeaderStyle-Width="8%"
                                                    HeaderText="Tipo de modificación" SortExpression="tipo_modificacion"
                                                    UniqueName="tipo_modificacion">
                                                    <HeaderStyle CssClass="wrapWord"  />
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </div>

                        </div>
                    </div>
                    <!-- /.box-body -->
                    <div class="box-footer">
                        <telerik:RadButton ID="btn_salir" runat="server" Text="Salir" Width="100px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_registrar_modifiacion" runat="server" Text="Registrar nueva modificación" Width="160px" CausesValidation="false" ValidationGroup="2" CssClass="btn btn-sm pull-right margin-r-5">
                        </telerik:RadButton>
                        <div class="alert-sm bg-blue col-sm-7" runat="server" id="div_mensaje" visible="false">
                            <asp:Label runat="server" ID="lblt_Error" CssClass="text-center text-bold">The value can´t be greater than the funding assign for the project</asp:Label>
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
    <script src="../Scripts/FileUploadTelerik.js"></script>
    
</asp:Content>

