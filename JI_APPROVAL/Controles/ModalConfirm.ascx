<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ModalConfirm.ascx.vb" Inherits="RMS_APPROVAL.ModalConfirm" %>

<div class="modal fade bs-example-modal-sm" id="myModal" data-backdrop="static" data-keyboard="false">
    <div class="vertical-alignment-helper">
        <div class="modal-dialog modal-sm vertical-align-center">
            <div class="modal-content">
                <div class="modal-header modal-success">              
                    <h4 class="modal-title" runat="server" id="esp_ctrl_lbl_titulo_exitoso">Registro Guardado</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="esp_ctrl_lbl_mensaje_exitoso" runat="server" />
                    <asp:Label ID="lblRedireccion" runat="server" Visible="false" />
                </div>
                <div class="modal-footer">
                    <telerik:RadButton ID="esp_ctrl_btn_cerrar_mensaje" runat="server" Text="Aceptar" Width="100px" CssClass="btn btn-sm pull-right" data-dismiss="modal" UseSubmitBehavior="false">
                    </telerik:RadButton>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>
    <!-- /.modal-dialog -->
</div>
<!-- /.modal -->



<%--  <div class="modal fade" id="myModal"  data-backdrop="static" data-keyboard="false" >

        <div class="modal-dialog  modal-dialog-centered">
          <div class="modal-content modal-lg bg-success">
            <div class="modal-header">
               <h5 class="modal-title" runat="server" id="esp_ctrl_lbl_titulo_exitoso">Registro Guardado</h5>
              <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
               <asp:Label ID="esp_ctrl_lbl_mensaje_exitoso" runat="server" />
               <asp:Label ID="lblRedireccion" runat="server" Visible="false" />
            </div>
            <div class="modal-footer justify-content-between">
                <telerik:RadButton ID="esp_ctrl_btn_cerrar_mensaje" runat="server" Text="Aceptar" Width="100px" CssClass="btn btn-sm float-right" data-dismiss="modal" UseSubmitBehavior="false">
                </telerik:RadButton>
            </div>
          </div>
          <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
      </div>
      <!-- /.modal -->--%>


<script type="text/javascript">
    function Func() {
        $('#myModal').modal('show');
    }
    function FuncHide() {
        $(".modal-backdrop").remove();
        $('body').removeClass('modal-open');
        $('#myModal').modal('hide');
    }
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

    .bg-green-active, .modal-danger .modal-header, .modal-danger {
        background-color: #d9534f !important;
        color: white;
    }
</style>
