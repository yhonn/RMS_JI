Public Class DeleteConfirm
    Inherits System.Web.UI.UserControl

    Dim cl_user As ly_SIME.CORE.cls_user
    Private mensaje As String
    Public Property NuevoMensaje() As String
        Get
            Return mensaje
        End Get
        Set(ByVal value As String)
            mensaje = value
            'Me.lblMessage.Text = mensaje
        End Set
    End Property

    Private urlRedireccion As String
    Public Property Redireccion() As String
        Get
            Return urlRedireccion
        End Get
        Set(ByVal value As String)
            urlRedireccion = value
            Me.lblRedireccion.Text = urlRedireccion
        End Set
    End Property

    Private tituloMensaje As String
    Public Property TituMensaje() As String
        Get
            Return tituloMensaje
        End Get
        Set(ByVal value As String)
            tituloMensaje = value
            If Not String.IsNullOrWhiteSpace(value) Then
                'Me.lblTitulo.InnerText = tituloMensaje
            End If
        End Set
    End Property

    Private cancelar As String
    Public Property btnCancelar() As String
        Get
            Return cancelar
        End Get
        Set(ByVal value As String)
            cancelar = Me.btn_cancelar.InnerText
        End Set
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        cl_user = Session.Item("clUser")
        Me.esp_ctrl_lbl_mensaje_cancelar.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "lbl_mensaje_cancelar").texto
        Me.esp_ctrl_lbl_titulo_cancelar.InnerText = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "lbl_titulo_cancelar").texto
        Me.btn_cancelar.InnerText = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btnh_CANCELAR").texto
        Me.esp_ctrl_btn_confirmar.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "CONFIRMAR").texto
    End Sub

    Protected Sub esp_ctrl_btn_confirmar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_confirmar.Click
        Dim b = Me.lblRedireccion.Text
        Page.Response.Redirect(b)
    End Sub
End Class