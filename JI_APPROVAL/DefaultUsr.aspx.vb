Imports ly_SIME
Imports System.Globalization
Imports System.Threading

Public Class DefaultUsr
    Inherits System.Web.UI.Page

    Dim cl_user As New CORE.cls_user

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Obtener todos los querystring o el HAsh y Obtener el codigo de usuario y Programa para crear un log de auditoria
        If Not (Request.QueryString("TkUsrVal") Is Nothing And Not (Request.QueryString("IdP") Is Nothing)) Then

            Dim Rw_usuario As DataRow = cl_user.findToken(Request.QueryString("TkUsrVal").ToString)
            Dim id_usuario As Integer
            Dim id_Tk As Integer

            Me.Session("idGuiToken") = Request.QueryString("TkUsrVal")

            If CType(Rw_usuario.Item("id_usr"), Integer) > 0 Then ' If there is a token an this token is activate set the user class and his default page, and insert the current log

                id_usuario = Rw_usuario.Item("id_usr")
                id_Tk = Rw_usuario.Item("id_token_usr")
                'Set User Class
                cl_user = New ly_SIME.CORE.cls_user(2, id_usuario, CType(Request.QueryString("IdP").ToString, Integer))
                'Set the User class whit ID Systems M&E and the respective activitie
                cl_user.SetTokenID(id_Tk)
                'Set the current Token

                Me.Session("E_IdUser") = cl_user.getUserField("id_usuario", "id_usuario", id_usuario)
                Me.Session("E_Programa") = cl_user.getUsuarioField("nombre_programa", "id_usuario", id_usuario)
                Me.Session("E_IDPrograma") = CType(Request.QueryString("IdP").ToString, Integer)

                'Me.Session("E_Imagen") = cl_user.getUsuarioField("imagen", "id_usuario", id_usuario)
                'Me.Session("E_SubRegion") = cl_user.getUsuarioField("subregiones", "id_usuario", id_usuario)
                Me.Session("E_Nombre") = cl_user.getUsuarioField("nombre_usuario", "id_usuario", id_usuario)
                cl_user.id_idioma = cl_user.getUsuarioField("id_idioma", "id_usuario", id_usuario)
                cl_user.codigo_nivel_usuario = cl_user.getUsuarioField("codigo_nivel_usuario", "id_usuario", id_usuario)

                Using db As New dbRMS_JIEntities
                    Dim id_programa = Convert.ToInt32(cl_user.getUsuarioField("id_programa", "id_usuario", id_usuario))
                    cl_user.regionalizacion = db.t_programas.Find(id_programa).t_pais.t_regionalizaciones.codigo_regionalizacion
                    cl_user.regionalizacionCulture = New CultureInfo(cl_user.regionalizacion)

                    If Not String.IsNullOrEmpty(db.t_programas.Find(id_programa).t_pais.t_regionalizaciones.currency_symbol) Then
                        cl_user.set_CurrencySymbol(db.t_programas.Find(id_programa).t_pais.t_regionalizaciones.currency_symbol)
                    End If

                    cl_user.controles_otros = db.vw_control_otro_idioma.Where(Function(p) p.id_idioma = cl_user.id_idioma).ToList()
                    Dim string_programa = id_programa.ToString()
                    Dim subregiones = db.vw_t_usuarios_subregiones.Where(Function(p) p.id_programa = id_programa And p.id_usuario = cl_user.id_usr).ToList().ToList
                    Dim subr = ""
                    For Each item In subregiones
                        subr += item.nombre_subregion & ", "
                    Next
                    subr = subr.TrimEnd(" ")
                    subr = subr.TrimEnd(",")
                    Me.Session("E_SubRegion") = subr

                    Dim user_uri As Uri
                    Dim user_route = db.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).url_user_images
                    Uri.TryCreate(user_route & cl_user.getUsuarioField("imagen", "id_usuario", id_usuario), UriKind.Absolute, user_uri)
                    Me.Session("E_Imagen") = user_uri.AbsoluteUri.ToString()

                    Thread.CurrentThread.CurrentCulture = cl_user.regionalizacionCulture
                    Thread.CurrentThread.CurrentUICulture = cl_user.regionalizacionCulture

                End Using

                'Set Log tran
                'The log user has tu be whit the respect Program
                cl_user.init_logUSR(cl_user.id_usr)
                cl_user.setLOGfield("id_programa", cl_user.Id_Cprogram, "id_log_usr", cl_user.idLog)
                cl_user.setLOGfield("id_token_usr", cl_user.idToken, "id_log_usr", cl_user.idLog)

                If cl_user.save_LOG() <> -1 Then
                    HttpContext.Current.Session.Add("clUser", cl_user) 'Save the session Class in the context
                    Me.Response.Redirect("~/Default")
                Else
                    Dim vUrlstr As String = Request.UrlReferrer.ToString() & "&ret=YES"
                    Me.Response.Redirect(vUrlstr)
                End If

            Else 'We need to redirect to his source page wheatever it's been

                Dim vUrlstr As String = Request.UrlReferrer.ToString() & "&ret=YES"
                Me.Response.Redirect(vUrlstr)

            End If

        Else
            Me.Response.Redirect("~/frm_login")
            'Me.Response.Redirect("www.google.com")
        End If

    End Sub

End Class