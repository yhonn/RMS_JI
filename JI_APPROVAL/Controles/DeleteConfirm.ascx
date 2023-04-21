<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DeleteConfirm.ascx.vb" Inherits="RMS_APPROVAL.DeleteConfirm" %>

<div class="modal fade bs-example-modal-sm" id="cancelConfirm" data-backdrop="static" data-keyboard="false">
    <div class="vertical-alignment-helper">
        <div class="modal-dialog modal-sm vertical-align-center">
            <div class="modal-content">
                <div class="modal-header modal-warning">
                    <h4 class="modal-title" runat="server" id="esp_ctrl_lbl_titulo_cancelar">Cancelar Operación</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="esp_ctrl_lbl_mensaje_cancelar" runat="server" Text="Desea cancelar la operación?" />
                    <asp:Label ID="lblRedireccion" runat="server" CssClass="deleteRedireccion" Visible="false" />
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="esp_ctrl_btn_confirmar" CssClass="btn btn-sm btn-warning btn-ok" Text="OK" />
                    <button class="btn btn-sm" data-dismiss="modal" aria-hidden="true" causesvalidation="false" runat="server" id="btn_cancelar"></button>
                </div>
            </div>
        </div>
    </div>
</div>


<script type="text/javascript">
    function FuncCancel() {
        $('#cancelConfirm').modal('show');
    }
</script>
<script>
    $('#cancelConfirm').on('show.bs.modal', function (e) {
        $(this).find('.btn-ok').attr('href', $(e.relatedTarget).data('href'));
        $(this).find('.deleteRedireccion').html($(e.relatedTarget).data('redirect'));
    });
</script>

<style>
    .vertical-alignment-helper {
        display: table;
        height: 100%;
        width: 100%;
        pointer-events: none;
    }

    .vertical-align-center {
        /* To center vertically */
        display: table-cell;
        vertical-align: middle;
        pointer-events: none;
    }

    .modal-content {
        /* Bootstrap sets the size of the modal in the modal-dialog class, we need to inherit it */
        width: inherit;
        height: inherit;
        /* To center horizontally */
        margin: 0 auto;
        pointer-events: all;
    }

    .bg-green-active, .modal-success .modal-header, .modal-success {
        background-color: #5cb85c !important;
        color: white;
    }

    .bg-green-active, .modal-warning .modal-header, .modal-warning {
        background-color: #f39c12 !important;
        color: white;
    }
</style>
