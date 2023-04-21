<%@ Page Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_AdminTrimestres" CodeBehind="frm_AdminTrimestres.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
        <h1>
            <asp:Label runat="server" ID="lblt_titulo_pantalla">Administración</asp:Label>
        </h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Administración de indicadores y periodos activos</asp:Label></h3>
            </div>
            <div class="box-body">
                <div class="form-group row">
                    <div class="col-sm-4">
                        <asp:Label ID="lbltotal" runat="server" Font-Bold="True" Font-Size="12px"></asp:Label>
                    </div>
                    <div class="col-sm-8 text-right">
                        <telerik:RadButton ID="btn_crear" runat="server"  AutoPostBack="true" Enabled="false"
                            Text="Crear periodos" Width="130px">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_buscar" runat="server"  AutoPostBack="true" Enabled="false"
                            Text="Editar periodos" Width="130px">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_buscar0" runat="server"  AutoPostBack="true" Enabled="false"
                            Text="Consolidar datos" Width="130px">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btn_buscar1" runat="server"  AutoPostBack="true" Enabled="false"
                            Text="Depurar datos" Width="130px">
                        </telerik:RadButton>
                    </div>
                </div>

                <telerik:RadGrid ID="grd_cate" runat="server" DataSourceID="SqlDataSource2">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_periodo" DataSourceID="SqlDataSource2"
                        AllowAutomaticDeletes="False" AllowAutomaticUpdates="False">
                        <Columns>
                            <telerik:GridBoundColumn DataField="id_periodo"
                                FilterControlAltText="Filter id_periodo column"
                                SortExpression="id_periodo" UniqueName="id_periodo"
                                Visible="False" DataType="System.Int32" HeaderText="id_periodo"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="nombre_region"
                                FilterControlAltText="Filter nombre_region column"
                                HeaderText="Región" SortExpression="nombre_region"
                                UniqueName="colm_nombre_region">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="nombre_subregion"
                                FilterControlAltText="Filter nombre_subregion column"
                                HeaderText="Sub Región" SortExpression="nombre_subregion"
                                UniqueName="colm_nombre_subregion">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="anio"
                                FilterControlAltText="Filter anio column"
                                HeaderText="Año activo" SortExpression="anio"
                                UniqueName="colm_anio">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ciclo"
                                FilterControlAltText="Filter ciclo column" HeaderText="FY"
                                SortExpression="ciclo"
                                UniqueName="colm_ciclo">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="codigo_anio_fiscal"
                                FilterControlAltText="Filter codigo_anio_fiscal column"
                                HeaderText="Trimestre activo" SortExpression="codigo_anio_fiscal"
                                UniqueName="colm_codigo_anio_fiscal">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                UniqueName="visible" Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkActivo" runat="server" AutoPostBack="True"
                                        OnCheckedChanged="chkVisible_CheckedChanged" />
                                    <ajaxToolkit:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server"
                                        CheckedImageUrl="../Imagenes/iconos/Stock_IndexUp.png" ImageHeight="16" ImageWidth="16"
                                        TargetControlID="chkActivo" UncheckedImageUrl="../Imagenes/iconos/Stock_IndexDown.png">
                                    </ajaxToolkit:ToggleButtonExtender>
                                </ItemTemplate>
                                <HeaderStyle Width="10px" />
                                <ItemStyle Width="10px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn DataField="bloqueado"
                                FilterControlAltText="Filter CodPeriodo column" HeaderText="Bloqueado"
                                SortExpression="bloqueado"
                                UniqueName="bloqueado" Visible="False">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                    ConnectionString="<%$ ConnectionStrings:dbCI_SAPConnectionString %>"
                    SelectCommand="SELECT * FROM vw_t_periodos WHERE activo= 1"></asp:SqlDataSource>
                <asp:HiddenField runat="server" ID="h_Filter" Value="" />
            </div>
        </div>
    </section>
</asp:Content>

