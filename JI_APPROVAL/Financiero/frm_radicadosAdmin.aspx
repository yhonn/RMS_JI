<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_radicadosAdmin.aspx.vb" Inherits="RMS_APPROVAL.frm_radicadosAdmin" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>
<%@ register tagprefix="uc" tagname="ReturnConfirm" src="~/Controles/DeleteConfirm.ascx" %>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">

    <uc:confirm runat="server" id="MsgGuardar" />
    <uc:returnconfirm runat="server" id="MsgReturn" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Financiero</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Permisos radicados</asp:Label></h3>
            </div>
            <div class="box-body">
                 <div class="form-group row">
                    <div class="col-sm-12">
                        <h4 class="text-center"><asp:Label runat="server" ID="Label1" CssClass="control-label text-bold">Radicados post cierre</asp:Label></h4>
                        <hr class="box box-primary" />
                    </div>
                </div>
                 <div class="form-group row">
                        <div class="col-sm-6 col-md-6 col-lg-4">
                            <asp:Label runat="server" ID="lbl_tipo_actividad" CssClass="control-label text-bold">Fecha limite registro radicados POST cierre</asp:Label>
                            <br />
                            <telerik:RadDatePicker ID="dt_fecha_post_cierre" AutoPostBack="true" Width="90%" Enabled="true" runat="server">
                                <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                            </telerik:RadDatePicker>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="dt_fecha_post_cierre" CssClass="Error" Display="Dynamic"
                                        ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </div>
                          <div class="col-sm-6 col-md-6 col-lg-4">
                            <asp:Label runat="server" ID="Label28" CssClass="control-label text-bold">Habilitar registro radicados:</asp:Label>
                            <br />
                            <asp:RadioButtonList ID="rbn_registro_radicados" runat="server"
                                RepeatColumns="2" Style="height: 26px" AutoPostBack="false">
                                <asp:ListItem Value="1">Sí &nbsp;</asp:ListItem>
                                <asp:ListItem Value="2">No &nbsp;</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="rv_gasto_evento_rbn" runat="server"
                            ControlToValidate="rbn_registro_radicados"  Visible="false" CssClass="Error" Display="Dynamic"
                            ErrorMessage="Campo obligatorio" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-4">
                            <telerik:RadButton ID="btn_guardar" runat="server" AutoPostBack="true" Text="Actualizar permisos" validationgroup="1"  Width="100px"></telerik:RadButton>
                        </div>
                 </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <h4 class="text-center"><asp:Label runat="server" ID="Label2" CssClass="control-label text-bold">Permisos usuarios</asp:Label></h4>
                        <hr class="box box-primary" />
                    </div>
                </div>
                <div class="form-group">
                                                    <telerik:RadTextBox ID="txt_doc" runat="server"
                                                        EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="395px"
                                                        ValidationGroup="1">
                                                    </telerik:RadTextBox>
                                                    <telerik:RadButton ID="btn_buscar" runat="server" AutoPostBack="true" Text="Buscar" Width="100px">
                                                    </telerik:RadButton>
                                                    <hr />

                                                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" AllowSorting="true"
                                                    CellSpacing="0" GridLines="None" PageSize="15" AllowPaging="true"
                                                    AutoGenerateColumns="False">
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_usuario" AllowAutomaticUpdates="True">

                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="id_usuario"
                                                                FilterControlAltText="Filter id_usuario column"
                                                                SortExpression="id_usuario" UniqueName="id_usuario"
                                                                Visible="False" DataType="System.Int32" HeaderText="id_usuario"
                                                                ReadOnly="True">
                                                            </telerik:GridBoundColumn>
                                                         
                                                            <telerik:GridBoundColumn DataField="usuario"
                                                                FilterControlAltText="Filter usuario column"
                                                                HeaderText="usuario" SortExpression="usuario"
                                                                UniqueName="colm_usuario">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="email_usuario"
                                                                FilterControlAltText="Filter email_usuario column" HeaderText="email_usuario"
                                                                SortExpression="email_usuario"
                                                                UniqueName="colm_email_usuario">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="job"
                                                                FilterControlAltText="Filter job column"
                                                                HeaderText="job" SortExpression="job"
                                                                UniqueName="colm_job">
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn DataField="registro_radicados_post_cierre" Visible="false"
                                                                FilterControlAltText="Filter registro_radicados_post_cierre column" HeaderText="Habilitar" UniqueName="registro_radicados_post_cierre">
                                                                <HeaderStyle Width="150px" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridTemplateColumn FilterControlAltText="Filter estadoE column"
                                                                UniqueName="estadoE" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkActivo" runat="server" AutoPostBack="True"
                                                                        OnCheckedChanged="chkVisible_CheckedChanged" />
                                                                    <cc1:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server"
                                                                        CheckedImageUrl="../Imagenes/iconos/hmenu-unlock.png" ImageHeight="16" ImageWidth="16"
                                                                        TargetControlID="chkActivo" UncheckedImageUrl="../Imagenes/iconos/hmenu-lock.png">
                                                                    </cc1:ToggleButtonExtender>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="10px" />
                                                                <ItemStyle Width="10px" />
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                        </div>
            </div>
        </div>
    </section>
  
</asp:Content>
