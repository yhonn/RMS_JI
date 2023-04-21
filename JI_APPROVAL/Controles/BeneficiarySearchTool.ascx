<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BeneficiarySearchTool.ascx.vb" Inherits="ACS_SIME.BeneficiarySearchTool" %>

<asp:UpdatePanel ID="UpdatePanelMunicipio" runat="server" RenderMode="Inline">
    <ContentTemplate>
        <div class="col-lg-12">
            <div class="form-group row">
                <br />
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_name" CssClass="control-label text-bold">Surname</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadTextBox ID="txt_surname" runat="server" Width="90%" MaxLength="300" CssClass="form-control"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                        ControlToValidate="txt_surname" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                        ControlToValidate="txt_surname" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="99">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                        ControlToValidate="txt_surname" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="98">*</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_first_name" CssClass="control-label text-bold">First name</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadTextBox ID="txt_first_name" runat="server" Width="90%" MaxLength="300"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ControlToValidate="txt_first_name" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                        ControlToValidate="txt_first_name" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="99">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                        ControlToValidate="txt_first_name" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="98">*</asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_mother_surname" CssClass="control-label text-bold">Mother's Surname</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadTextBox ID="txt_mother_surname" runat="server" Width="90%" MaxLength="300"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="txt_mother_surname" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                        ControlToValidate="txt_mother_surname" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="99">*</asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2">
                    <asp:Label runat="server" ID="lblt_mother_firts_name" CssClass="control-label text-bold">Mother's First name</asp:Label>
                </div>
                <div class="col-sm-4">
                    <telerik:RadTextBox ID="txt_mother_firts_name" runat="server" Width="90%" MaxLength="300"></telerik:RadTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                        ControlToValidate="txt_mother_firts_name" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                        ControlToValidate="txt_mother_firts_name" CssClass="Error" Display="Dynamic"
                        ErrorMessage="registre una descripción" ValidationGroup="99">*</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
