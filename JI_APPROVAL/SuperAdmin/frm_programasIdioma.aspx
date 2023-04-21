<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="frm_programasIdioma.aspx.vb" Inherits="RMS_APPROVAL.frm_programasIdioma" %>

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
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Idiomas Programa</asp:Label></h3>
            </div>
            <div class="box-body">
                <asp:Label ID="lbl_id_mod" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="X-Small" ForeColor="#C00000"
                    Visible="False"></asp:Label>
                <telerik:RadComboBox runat="server" ID="cmb_id_idioma" DataSourceID="SqlDataSource1" EmptyMessage="Seleccione un Idioma"
                    DataTextField="descripcion_idioma" DataValueField="id_idioma" ValidationGroup="1" Width="300px">
                </telerik:RadComboBox>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                    SelectCommand="SELECT * FROM t_idiomas WHERE id_idioma NOT IN (SELECT id_idioma FROM t_programas_idioma WHERE id_programa = @id_programa)">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="id_programa" QueryStringField="Id" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <telerik:RadButton ID="btn_nuevo" runat="server"  AutoPostBack="true" Text="Asignar Idioma al Programa" ValidationGroup="1">
                </telerik:RadButton>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue=""
                    ControlToValidate="cmb_id_idioma" CssClass="Error" Display="Dynamic"
                    ErrorMessage="Registre un nombre el proyecto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                <hr />
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" CellSpacing="0"
                    GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="15" AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_programa_idioma" AllowAutomaticUpdates="True">
                        <Columns>

                            <telerik:GridBoundColumn DataField="id_programa_idioma"
                                FilterControlAltText="Filter id_programa_idioma column"
                                SortExpression="id_programa_idioma" UniqueName="id_programa_idioma"
                                Visible="False" DataType="System.Int32" HeaderText="id_programa_idioma"
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
                            <telerik:GridBoundColumn DataField="descripcion_idioma"
                                FilterControlAltText="Filter descripcion_idioma column" HeaderText="Idioma" UniqueName="colm_descripcion_idioma">
                                <HeaderStyle Width="150px" />
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </section>
</asp:Content>
