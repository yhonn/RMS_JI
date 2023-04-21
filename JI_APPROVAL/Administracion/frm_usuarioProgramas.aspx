<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_usuarioProgramas.aspx.vb" Inherits="RMS_APPROVAL.frm_usuarioProgramas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">

    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Programas Usuario</asp:Label>
                    <asp:Label runat="server" ID="lbl_usuario"></asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lblt_programa" runat="server" CssClass="control-label text-bold" Text="Programa"></asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_id_programa" runat="server" DataSourceID="SqlDataSource1" EmptyMessage="Seleccione un Programa"
                            DataTextField="nombre_programa" DataValueField="id_programa" Width="300px" AutoPostBack="true" Enabled="false">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                            ControlToValidate="cmb_id_programa" CssClass="Error" Display="Dynamic"
                            ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lblt_idioma" runat="server" CssClass="control-label text-bold" Text="Idioma"></asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_idioma" runat="server" EmptyMessage="Seleccione un Idioma"
                            ValidationGroup="1" Width="300px" Enabled="false">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="cmb_idioma" CssClass="Error" Display="Dynamic"
                            ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lblt_cargo" runat="server" CssClass="control-label text-bold" Text="Cargo"></asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_job_title" runat="server" EmptyMessage="Seleccione un cargo"
                            ValidationGroup="1" Width="300px" Enabled="false">
                        </telerik:RadComboBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="cmb_job_title" CssClass="Error" Display="Dynamic"
                            ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" SingleClick="true" SingleClickText="Submitting..."
                            Text="Configurar permisos" ValidationGroup="1" CausesValidation="true">
                        </telerik:RadButton>
                    </div>
                </div>


                <asp:Label ID="lbl_id_usuario" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="X-Small" ForeColor="#C00000"
                    Visible="False"></asp:Label>
                <%--<telerik:RadComboBox runat="server" ID="cmb_id_programa" DataSourceID="SqlDataSource1" EmptyMessage="Seleccione un Programa"
                    DataTextField="nombre_programa" DataValueField="id_programa" ValidationGroup="1" Width="300px">
                </telerik:RadComboBox>--%>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                    SelectCommand="SELECT * FROM t_programas WHERE id_programa NOT IN (SELECT id_programa FROM t_usuario_programa WHERE id_usuario = @id_usuario) and visible=1">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="id_usuario" QueryStringField="Id" />
                    </SelectParameters>
                </asp:SqlDataSource>
                
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue=""
                    ControlToValidate="cmb_id_programa" CssClass="Error" Display="Dynamic"
                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>--%>
                <hr />

                <telerik:RadGrid ID="grd_cate" runat="server" GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" AllowAutomaticUpdates="True" DataKeyNames="id_usuario_programa">
                        <Columns>
                            <telerik:GridBoundColumn DataField="id_usuario_programa"
                                FilterControlAltText="Filter id_usuario_programa column"
                                SortExpression="id_usuario_programa" UniqueName="id_usuario_programa"
                                Visible="False" DataType="System.Int32" HeaderText="id_usuario_programa"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="Eliminar" Visible="false">
                                <HeaderStyle Width="10" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="col_hlk_eliminar" runat="server" Width="10" ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Eliminar" OnClick="Eliminar_Click">
                                        <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="nombre_programa"
                                FilterControlAltText="Filter nombre_programa column" HeaderText="Programa" UniqueName="colm_nombre_programa">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="descripcion_idioma"
                                FilterControlAltText="Filter descripcion_idioma column" HeaderText="Idioma" UniqueName="colm_descripcion_idioma">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="job"
                                FilterControlAltText="Filter job column" HeaderText="Cargo" UniqueName="colm_job">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
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
                                <asp:Button runat="server" ID="esp_ctrl_btn_eliminar" CssClass="btn btn-sm btn-danger btn-ok" Text="Eliminar" />
                                <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="esp_ctrl_btnh_CANCELAR">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- /.content -->
</asp:Content>

