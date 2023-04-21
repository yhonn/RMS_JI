Public Class ModalConfirm
    Inherits System.Web.UI.UserControl

    Dim cl_user As ly_SIME.CORE.cls_user
    Private mensaje As String
    Public Property NuevoMensaje() As String
        Get
            Return mensaje
        End Get
        Set(ByVal value As String)
            mensaje = value
            Me.esp_ctrl_lbl_mensaje_exitoso.Text = mensaje
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
                Me.esp_ctrl_lbl_titulo_exitoso.InnerText = tituloMensaje
            End If
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cl_user = Session.Item("clUser")
        Me.esp_ctrl_lbl_mensaje_exitoso.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.esp_ctrl_lbl_titulo_exitoso.InnerText = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "TITULOGUARDADO").texto
        Me.esp_ctrl_btn_cerrar_mensaje.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btnh_aceptar").texto
    End Sub

    Protected Sub btn_cerrar_mensaje_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_cerrar_mensaje.Click
        If Me.lblRedireccion.Text.Length > 0 Then
            Page.Response.Redirect(Me.lblRedireccion.Text)
        Else
            'Solo para guardar y mantenerse en la misma pantalla
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncHide()", True)
        End If
    End Sub
End Class