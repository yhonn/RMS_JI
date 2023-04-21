<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UbicacionGeografica.ascx.vb" Inherits="RMS_APPROVAL.UbicacionGeograficaOLD" %>
<asp:UpdatePanel ID="UpdatePanelMunicipio" runat="server" RenderMode="Inline">
    <ContentTemplate>
        <div class="col-lg-12">
            <div class="form-group row">
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_county" CssClass="control-label text-bold">County</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="cmb_county" runat="server" Width="90%" AutoPostBack="true"
                        Filter="Contains"
                        CausesValidation="false"
                        AllowCustomText="true"
                        HighlightTemplatedItems="true"
                        EnableLoadOnDemand="True"
                        EmptyMessage="Escriba el elemento a buscar...">
                    </telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                        ControlToValidate="cmb_county" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                        ControlToValidate="cmb_county" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="99">*</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_subcounty" CssClass="control-label text-bold">SubCounty</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="cmb_subcounty" runat="server" Width="90%" AutoPostBack="true"
                        Filter="Contains"
                        CausesValidation="false"
                        AllowCustomText="true"
                        HighlightTemplatedItems="true"
                        EnableLoadOnDemand="True"
                        EmptyMessage="Escriba el elemento a buscar...">
                    </telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                        ControlToValidate="cmb_subcounty" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                        ControlToValidate="cmb_subcounty" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="99">*</asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_parish" CssClass="control-label text-bold">Parish</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="cmb_parish" runat="server" Width="90%" AutoPostBack="true"
                        Filter="Contains"
                        CausesValidation="false"
                        AllowCustomText="true"
                        HighlightTemplatedItems="true"
                        EnableLoadOnDemand="True"
                        EmptyMessage="Escriba el elemento a buscar...">
                    </telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                        ControlToValidate="cmb_parish" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="cmb_parish" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="99">*</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_village" CssClass="control-label text-bold">Village</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadComboBox ID="cmb_village" runat="server" Width="90%" AutoPostBack="true"
                        Filter="Contains"
                        CausesValidation="false"
                        AllowCustomText="true"
                        HighlightTemplatedItems="true"
                        EnableLoadOnDemand="True"
                        EmptyMessage="Escriba el elemento a buscar...">
                    </telerik:RadComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                        ControlToValidate="cmb_village" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ControlToValidate="cmb_village" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="99">*</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <%--        <table>
            <tr valign="top">
                <td style="white-space: nowrap">Departamento: 
                </td>
                <td style="white-space: nowrap">
                    <asp:DropDownList ID="ddlDepto" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                    &nbsp;
                </td>
                <td style="white-space: nowrap">&nbsp;</td>
            </tr>
            <tr valign="top">
                <td style="white-space: nowrap">Municipio:</td>
                <td style="white-space: nowrap">
                    <asp:DropDownList ID="ddlMunicipio" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="ddlMunicipio" Display="Dynamic" Enabled="false"
                        ErrorMessage="Seleccione un municipio" CssClass="error"></asp:RequiredFieldValidator>
                </td>
                <td style="white-space: nowrap">&nbsp;</td>
            </tr>
        </table>--%>
    </ContentTemplate>
</asp:UpdatePanel>
