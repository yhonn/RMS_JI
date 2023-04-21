<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_acceso_ctrl.aspx.vb" Inherits="RMS_APPROVAL.frm_acceso_ctrl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Acceso Controles</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lbl_id_mod" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="X-Small" ForeColor="#C00000"
                            Visible="False"></asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox runat="server" ID="cmb_id_ctrl" DataSourceID="SqlDataSource1" EmptyMessage="Seleccione un Control"
                            Filter="Contains"
                            CausesValidation="false"
                            AllowCustomText="true"
                            HighlightTemplatedItems="true"
                            EnableLoadOnDemand="True"
                            DataTextField="ctrl_name" DataValueField="id_control" ValidationGroup="1" Width="300px">
                        </telerik:RadComboBox>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                            SelectCommand="SELECT * FROM t_control WHERE id_control NOT IN (SELECT id_control FROM t_access_ctrl WHERE id_rol = @id_rol) and id_mod = @id_mod and requiere_acceso = 1">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="id_mod" QueryStringField="Id" />
                                <asp:QueryStringParameter Name="id_rol" QueryStringField="IdRol" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <telerik:RadButton ID="btn_nuevo" runat="server" AutoPostBack="true" Text="Asignar Acceso Control" ValidationGroup="1">
                        </telerik:RadButton>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue=""
                            ControlToValidate="cmb_id_ctrl" CssClass="Error" Display="Dynamic"
                            ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                
                <hr />
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" CellSpacing="0"
                    GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_acc_ctrl" AllowAutomaticUpdates="True">
                        <Columns>

                            <telerik:GridBoundColumn DataField="id_acc_ctrl"
                                FilterControlAltText="Filter id_acc_ctrl column"
                                SortExpression="id_acc_ctrl" UniqueName="id_acc_ctrl"
                                Visible="False" DataType="System.Int32" HeaderText="id_usuario"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>

                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                                ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow"
                                ConfirmDialogWidth="400px"
                                ConfirmText="Confirma que desea eliminar el registro?"
                                ConfirmTitle="Eliminar registro" ImageUrl="../Imagenes/iconos/b_drop.png"
                                UniqueName="Eliminar" FilterControlAltText="Filter Eliminar column" Visible="false">

                                <ItemStyle Width="5px" />
                            </telerik:GridButtonColumn>
                            <telerik:GridBoundColumn DataField="ctrl_name"
                                FilterControlAltText="Filter ctrl_name column" HeaderText="Control" UniqueName="colm_ctrl_name">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ctrl_code"
                                FilterControlAltText="Filter ctrl_code column" HeaderText="Código Control" UniqueName="colm_ctrl_code">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="actrl_acc" Visible="false"
                                FilterControlAltText="Filter actrl_acc column" HeaderText="Acceso" UniqueName="colm_actrl_acc">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter estadoE column"
                                UniqueName="estadoE" Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkActivo" runat="server" AutoPostBack="True"
                                        OnCheckedChanged="chkVisible_CheckedChanged" />
                                    <cc1:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server"
                                        CheckedImageUrl="../Imagenes/iconos/Stock_IndexUp.png" ImageHeight="16" ImageWidth="16"
                                        TargetControlID="chkActivo" UncheckedImageUrl="../Imagenes/iconos/Stock_IndexDown.png">
                                    </cc1:ToggleButtonExtender>
                                </ItemTemplate>
                                <HeaderStyle Width="10px" />
                                <ItemStyle Width="10px" />
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
            <%--<div class="box-footer">
                Footer
            </div>--%>
        </div>
    </section>
    <!-- /.content -->
</asp:Content>
