<%@ Page Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" Inherits="RMS_APPROVAL.frm_Applicant" Title="APPROVAL SYSTEM" CodeBehind="frm_Applicant.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.DynamicData" TagPrefix="cc2" %>

<%@ Register TagPrefix="uc" TagName="Confirm" Src="~/Controles/ModalConfirm.ascx" %>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <uc:Confirm runat="server" ID="MsgGuardar" />
    <asp:Label ID="identity" runat="server" Text="" CssClass="deleteIdentity" data-id="" Visible="false" />
    <section class="content-header">
        <h1><asp:Label runat="server" ID="lblt_titulo_pantalla">REQUEST FOR PROPOSAL</asp:Label></h1>
    </section>
    <section class="content">
        <div class="box">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label runat="server" ID="lblt_subtitulo_pantalla">Potential Grantees</asp:Label></h3>
            </div>
            <div class="box-body">
                <telerik:RadTextBox ID="txt_doc" runat="server"
                    EmptyMessage="Ingrese nombre aquí..." LabelWidth="" Width="395px"
                    ValidationGroup="1">
                    <PasswordStrengthSettings IndicatorWidth="100px"></PasswordStrengthSettings>
                </telerik:RadTextBox>
                <telerik:RadButton ID="btn_buscar" runat="server"  AutoPostBack="true" Text="Buscar" Width="100px">
                </telerik:RadButton>
                <telerik:RadButton ID="btn_nuevo" runat="server"  AutoPostBack="true" Enabled="false" Text="New Applicant">
                </telerik:RadButton>
                <hr />
                <telerik:RadGrid ID="grd_cate" runat="server" AllowAutomaticDeletes="True" AllowSorting="true"
                    CellSpacing="0" GridLines="None" PageSize="15" AllowPaging="true"
                    AutoGenerateColumns="False">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID_ORGANIZATION_APP" AllowAutomaticUpdates="True">

                        <Columns>
                            <telerik:GridBoundColumn DataField="ID_ORGANIZATION_APP"
                                FilterControlAltText="Filter ID_ORGANIZATION_APP column"
                                SortExpression="ID_ORGANIZATION_APP" UniqueName="ID_ORGANIZATION_APP"
                                Visible="False" DataType="System.Int32" HeaderText="ID_ORGANIZATION_APP"
                                ReadOnly="True">
                            </telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn FilterControlAltText="Filter delete column" UniqueName="colm_delete" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="col_hlk_delete" runat="server" Width="10"
                                            ImageUrl="../Imagenes/iconos/b_drop.png" ToolTip="Editar"
                                            OnClick="Elimina_Elemento">
                                            <asp:Image ID="img_eliminar" runat="server" ImageUrl="../Imagenes/iconos/b_drop.png" Style="border-width: 0px;" />
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="5px" />
                                    <ItemStyle Width="10px" />
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="colm_Edit" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_edit" runat="server" ImageUrl="../Imagenes/iconos/b_edit.png" ToolTip="Editar ejecutor" Target="_self" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FilterControlAltText="filtro comments column" UniqueName="print" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="col_hlk_Print" runat="server" ImageUrl="~/Imagenes/iconos/Informacion1.png" ToolTip="Imprimir detalles" Target="_blank" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </telerik:GridTemplateColumn>
                             <telerik:GridBoundColumn DataField="NAMEALIAS"
                                FilterControlAltText="Filter NAMEALIAS column"
                                HeaderText="Acronyms" SortExpression="NAMEALIAS"
                                UniqueName="colm_NAMEALIAS">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ORGANIZATIONNAME"
                                FilterControlAltText="Filter ORGANIZATIONNAME column"
                                HeaderText="Name" SortExpression="ORGANIZATIONNAME"
                                UniqueName="colm_ORGANIZATIONNAME">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="PERSONNAME"
                                FilterControlAltText="Filter PERSONNAME column" HeaderText="Representative"
                                SortExpression="PERSONNAME"
                                UniqueName="colm_PERSONNAME">
                            </telerik:GridBoundColumn>

                             <telerik:GridBoundColumn DataField="ORGANIZATIONEMAIL"
                                FilterControlAltText="Filter ORGANIZATIONEMAIL column" HeaderText="Email"
                                SortExpression="ORGANIZATIONEMAIL"
                                UniqueName="colm_ORGANIZATIONEMAIL">
                            </telerik:GridBoundColumn>


                             <telerik:GridBoundColumn DataField="ORGANIZATION_TYPE"
                                FilterControlAltText="Filter ORGANIZATION_TYPE column" HeaderText="ORG TYPE"
                                SortExpression="ORGANIZATION_TYPE"
                                UniqueName="colm_ORGANIZATION_TYPE">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ADDRESSCOUNTRYREGIONID"
                                FilterControlAltText="Filter ADDRESSCOUNTRYREGIONID column"
                                HeaderText="Country" SortExpression="COUNTRY"
                                UniqueName="colm_ADDRESSCOUNTRYREGIONID">
                            </telerik:GridBoundColumn>
                              <telerik:GridBoundColumn DataField="ADDRESSCITY"
                                FilterControlAltText="Filter ADDRESSCITY column"
                                HeaderText="Cuty" SortExpression="ADDRESSCITY"
                                UniqueName="colm_ADDRESSCITY">
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="ORGANIZATIONSTATUS"
                                FilterControlAltText="Filter ORGANIZATIONSTATUS column" HeaderText="ORGANIZATIONSTATUS"
                                SortExpression="ORGANIZATIONSTATUS" UniqueName="colm_ORGANIZATIONSTATUS" Visible="false">
                            </telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn FilterControlAltText="Filter visible column"
                                UniqueName="colm_visible" Visible="false">
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
    </section>

</asp:Content>

