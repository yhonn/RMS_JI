<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_acceso_usr_mod.aspx.vb" Inherits="RMS_APPROVAL.frm_acceso_usr_mod" %>

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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Acceso Módulos</asp:Label>
                    <asp:Label runat="server" ID="lbl_subtitulo_aux"></asp:Label>
                </h3>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_palabra_clave" CssClass="control-label text-bold">Palabra Clave</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <asp:Label ID="lbl_id_usuario" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="X-Small" ForeColor="#C00000"
                            Visible="false"></asp:Label>
                        <telerik:RadTextBox ID="txt_doc" runat="server" EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="300px" ValidationGroup="1"></telerik:RadTextBox>
                        <telerik:RadButton ID="btn_buscar" runat="server"  AutoPostBack="true" Text="Buscar" Width="100px"></telerik:RadButton>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblt_sistema" CssClass="control-label text-bold">Sistema</asp:Label>
                    </div>
                    <div class="col-sm-10">
                        <telerik:RadComboBox ID="cmb_sistema" runat="server" Width="300px" AutoPostBack="true"></telerik:RadComboBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-2 text-right">
                    </div>
                   
                    <div class="col-sm-10">

                        <telerik:RadComboBox runat="server" ID="cmb_id_mod" DataSourceID="SqlDataSource1" EmptyMessage="Seleccione un Módulo"
                            Filter="Contains"
                            CausesValidation="false"
                            AllowCustomText="true"
                            HighlightTemplatedItems="true"
                            EnableLoadOnDemand="True"
                            DataTextField="mod_name" DataValueField="id_mod" ValidationGroup="1" Width="300px">
                        </telerik:RadComboBox>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                            SelectCommand="Select a.id_mod, a.mod_name as mod_name  
                                             FROM t_mod a
                                              INNER JOIN dbo.t_sys d ON a.id_sys = d.id_sys WHERE a.superadmin = 0 and a.id_sys = @id_sys and a.id_mod NOT IN (SELECT id_mod from VW_GR_ACC_USER_MOD where id_usuario = @id_usuario)">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="id_usuario" QueryStringField="Id" />
                                <asp:ControlParameter Name="id_sys" ControlID="cmb_sistema" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <telerik:RadButton ID="btn_nuevo" runat="server" Text="Asignar Acceso a Módulo" ValidationGroup="1" Enabled="false">
                        </telerik:RadButton>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue=""
                            ControlToValidate="cmb_id_mod" CssClass="Error" Display="Dynamic"
                            ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                
                
                <br />
                
                
                <hr />
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" CellSpacing="0"
                    GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_acc_usuario_mod" AllowAutomaticUpdates="True">
                        <Columns>
                            <telerik:GridBoundColumn DataField="id_acc_usuario_mod"
                                FilterControlAltText="Filter id_acc_usuario_mod column"
                                SortExpression="id_acc_usuario_mod" UniqueName="id_acc_usuario_mod"
                                Visible="False" DataType="System.Int32" HeaderText="id_acc_usuario_mod"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn DataField="sys_p"
                                FilterControlAltText="Filter sys_p column" HeaderText="SYS" UniqueName="colm_sys_p">
                                <HeaderStyle Width="50px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="mod_name"
                                FilterControlAltText="Filter mod_name column" HeaderText="Módulo" UniqueName="colm_mod_name">
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="mod_desc"
                                FilterControlAltText="Filter mod_desc column" HeaderText="Descripción" UniqueName="colm_mod_desc">
                                <HeaderStyle Width="250px" />
                            </telerik:GridBoundColumn>
                              <telerik:GridBoundColumn DataField="mod_url"
                                FilterControlAltText="Filter mod_url column" HeaderText="URL" UniqueName="colm_mod_url">
                                <HeaderStyle Width="100px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="acm_acc" Visible="false"
                                FilterControlAltText="Filter acm_acc column" HeaderText="Acceso" UniqueName="colm_acm_acc">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter estadoE column"
                                UniqueName="colm_estadoE" Visible="false">
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
                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                UniqueName="colm_visible" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_acceso_ctrl" runat="server" ImageUrl="../Imagenes/iconos/Family.png" ToolTip="Acceso controles" Target="_self" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
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
