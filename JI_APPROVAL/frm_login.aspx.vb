Imports System
Imports System.Data
Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Globalization
Imports System.Threading
Imports ly_SIME

Public Class Login
    Inherits Page

    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As New ly_SIME.CORE.cls_user
    Const cAPPROVAL_SYS = 2
    Const cSIME_SYS = 1

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If String.IsNullOrEmpty(Me.Session("redirectURL")) And String.IsNullOrEmpty(Request.QueryString("tpRET")) Then 'It has already  getting
            Me.Session.Clear()
            Me.Session.RemoveAll()
        ElseIf String.IsNullOrEmpty(Request.QueryString("Idout")) Then
            If Request.QueryString("Idout") = 1 Then
                Me.Session.Clear()
                Me.Session.RemoveAll()
            End If
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'If Not String.IsNullOrEmpty(Request.QueryString("tpRET")) Then

        '    If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then 'If it has already loggued
        '        Me.Response.Redirect(String.Format("https://sime.justiciainclusiva.org/frm_login?tpRET={0}&id={1}", Request.QueryString("tpRET"), Request.QueryString("id")))



        '    Else
        '        Me.Response.Redirect(String.Format("https://sime.justiciainclusiva.org/frm_login?tpRET={0}&id={1}", Request.QueryString("tpRET"), Request.QueryString("id")))


        '    End If
        'Else
        '    Me.Session("redirectURL") = "fmr_login"
        'End If


        Me.txt_usu.Focus()

        'Try
        '    If String.IsNullOrEmpty(Request.QueryString("Idout")) Then
        '        If Request.QueryString("Idout") = 1 Then
        '            Me.Session.Clear()
        '            Me.Session.RemoveAll()
        '        End If
        '    End If
        'Catch ex As Exception
        'End Try

        If Not IsPostBack Then

            rdC_Programas.DataSource = cl_user.get_Programs(0, False, True)
            rdC_Programas.DataBind()
            rdC_Programas.SelectedIndex = 0

            'tpRET=DocAPP&id=

            If Not String.IsNullOrEmpty(Request.QueryString("tpRET")) Then

                If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then 'If it has already loggued

                    Select Case Request.QueryString("tpRET")
                        Case "DocAPP"
                            Me.Response.Redirect(String.Format("~/approvals/frm_DocAprobacion.aspx?IdDoc={0}", Request.QueryString("id"))) 'Redirect
                        Case "DocEnvAPP"
                            Me.Response.Redirect(String.Format("~/approvals/frm_EnvironmentalApprovals.aspx?IdDoc={0}", Request.QueryString("id"))) 'Redirect
                        Case "travelApp"
                            Me.Response.Redirect(String.Format("~/administrativo/frm_viajeSgmt.aspx?Id={0}", Request.QueryString("id"))) 'Redirect
                        Case "travelAppLeg"
                            Me.Response.Redirect(String.Format("~/administrativo/frm_viaje_legalizacionSgmt.aspx?Id={0}", Request.QueryString("id"))) 'Redirect
                        Case "travelAppInf"
                            Me.Response.Redirect(String.Format("~/administrativo/frm_viaje_informeSgmt.aspx?Id={0}", Request.QueryString("id"))) 'Redirect
                        Case "parApp"
                            Me.Response.Redirect(String.Format("~/administrativo/frm_parSgmt.aspx?Id={0}", Request.QueryString("id"))) 'Redirect

                    End Select

                Else

                    Select Case Request.QueryString("tpRET")
                        Case "DocAPP"
                            Me.Session("redirectURL") = String.Format("~/approvals/frm_DocAprobacion.aspx?IdDoc={0}", Request.QueryString("id"))
                        Case "DocEnvAPP"
                            Me.Session("redirectURL") = String.Format("~/approvals/frm_EnvironmentalApprovals.aspx?IdDoc={0}", Request.QueryString("id"))
                        Case "travelApp"
                            Me.Session("redirectURL") = String.Format("~/administrativo/frm_viajeSgmt.aspx?Id={0}", Request.QueryString("id"))
                        Case "travelAppLeg"
                            Me.Session("redirectURL") = String.Format("~/administrativo/frm_viaje_legalizacionSgmt.aspx?Id={0}", Request.QueryString("id"))
                        Case "travelAppInf"
                            Me.Session("redirectURL") = String.Format("~/administrativo/frm_viaje_informeSgmt.aspx?Id={0}", Request.QueryString("id"))
                        Case "parApp"
                            Me.Session("redirectURL") = String.Format("~/administrativo/frm_parSgmt.aspx?Id={0}", Request.QueryString("id"))

                    End Select

                End If
            Else
                Me.Session("redirectURL") = ""
            End If

                'cl_user.regionalizacion = DBConcurrencyException.


            End If

        'RegisterHyperLink.NavigateUrl = "Register"
        'OpenAuthLogin.ReturnUrl = Request.QueryString("ReturnUrl")

        'Dim returnUrl = HttpUtility.UrlEncode(Request.QueryString("ReturnUrl"))
        'If Not String.IsNullOrEmpty(returnUrl) Then
        '    RegisterHyperLink.NavigateUrl &= "?ReturnUrl=" & returnUrl
        'End If


    End Sub

    Private Function GET_IP() As String
        Dim DirAcceso As String = ""
        Dim reqActual As HttpRequest = HttpContext.Current.Request
        DirAcceso = reqActual.ServerVariables("REMOTE_HOST") & ":" & reqActual.ServerVariables("HTTP_USER_AGENT") & ":" & reqActual.ServerVariables("LOCAL_ADDR")
        Return DirAcceso
    End Function

    Private Function GET_HOST_DAT(ByVal idType As String) As String

        Dim vRequest As HttpRequest = HttpContext.Current.Request
        Dim strResult As String

        Select Case idType

            Case "HOST"
                strResult = vRequest.ServerVariables("REMOTE_HOST")
            Case "AGENT"
                strResult = vRequest.ServerVariables("HTTP_USER_AGENT")
            Case "LOCAL"
                strResult = vRequest.ServerVariables("LOCAL_ADDR")

        End Select

        GET_HOST_DAT = strResult

    End Function

    Private Sub btn_login_2_Click(sender As Object, e As EventArgs) Handles btn_login_2.Click
        Dim err = True

        Try


            'Dim id_usuario = cl_user.Test_LogUsr(Me.rdC_Programas.SelectedValue, Me.txt_usu.Value.ToString(), Me.txt_pass.Value.ToString())

            Dim id_usuario = cl_user.LogUsr(Me.rdC_Programas.SelectedValue, Me.txt_usu.Value.ToString(), Me.txt_pass.Value.ToString())


            If id_usuario > 0 Then

                'cl_user = New ly_SIME.CORE.cls_user_approval(1, id_usuario)
                cl_user = New ly_SIME.CORE.cls_user(cAPPROVAL_SYS, id_usuario, Me.rdC_Programas.SelectedValue) 'whitout Activity and System
                cl_user.setRemoteAddr(GET_HOST_DAT("HOST"))
                cl_user.setUsrAgent(GET_HOST_DAT("AGENT"))
                cl_user.setLocalAddr(GET_HOST_DAT("LOCAL"))
                err = False

                ''**********************This is a DEMO*********************************
                'Dim ci As New CultureInfo("en-US")
                'Thread.CurrentThread.CurrentCulture = ci
                ''**********************This is a DEMO*********************************

            Else
                Me.alert.Visible = True
                Me.lblerr_user.Text = "The username or password you entered doesn't match. Please try again.<br/><br/> if the issue repeats, please contact the system administrator."
                Me.lblerr_user.Visible = True
            End If

            If err = True Then
                Me.lblerr_user.Visible = True
            Else


                'Left(Me.Session("E_Nombre") & StrDup(24, " "), 25)

                '*************************************ESTABLECEMOS EL TOKEN PARA ESTA SESION*************************
                Dim vIdGuiToken As String = cl_user.CreateLoginToken()
                If vIdGuiToken.ToString <> "-1" Then

                    Me.Session("idGuiToken") = vIdGuiToken.ToString

                    Me.Session("E_IdUser") = cl_user.getUserField("id_usuario", "id_usuario", id_usuario)
                    Me.Session("E_Programa") = cl_user.getUsuarioField("nombre_programa", "id_usuario", id_usuario)
                    Me.Session("E_IDPrograma") = Me.rdC_Programas.SelectedValue
                    Me.Session("E_Nombre") = cl_user.getUsuarioField("nombre_usuario", "id_usuario", id_usuario)

                    ''Me.Session("E_Imagen") = cl_user.getUsuarioField("imagen", "id_usuario", id_usuario)
                    ''Me.Session("E_SubRegion") = cl_user.getUsuarioField("subregiones", "id_usuario", id_usuario)
                    ''Me.Session("E_Nombre") = cl_user.getUsuarioField("nombre_usuario", "id_usuario", id_usuario)

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

                        'Dim subregiones = db.vw_t_usuarios_subregiones.Where(Function(p) p.id_programa = id_programa And p.id_usuario = cl_user.id_usr).ToList().ToList
                        'Dim subr = ""
                        'For Each item In subregiones
                        '    subr += item.nombre_subregion & ", "
                        'Next
                        'subr = subr.TrimEnd(" ")
                        'subr = subr.TrimEnd(",")
                        'Me.Session("E_SubRegion") = subr

                        Me.Session("E_SubRegion") = cl_user.get_users_region(cl_user.id_idioma)

                        Dim user_uri As Uri
                        Dim user_route = db.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).url_user_images
                        Uri.TryCreate(user_route & cl_user.getUsuarioField("imagen", "id_usuario", id_usuario), UriKind.Absolute, user_uri)
                        Me.Session("E_Imagen") = user_uri.AbsoluteUri.ToString()

                        Thread.CurrentThread.CurrentCulture = cl_user.regionalizacionCulture
                        Thread.CurrentThread.CurrentUICulture = cl_user.regionalizacionCulture

                    End Using


                    cl_user.init_logUSR(cl_user.id_usr)
                    cl_user.setLOGfield("id_programa", cl_user.Id_Cprogram, "id_log_usr", cl_user.idLog)
                    cl_user.setLOGfield("id_token_usr", cl_user.idToken, "id_log_usr", cl_user.idLog)

                    If cl_user.save_LOG() <> -1 Then 'Save the user Logs system

                        HttpContext.Current.Session.Add("clUser", cl_user)

                        If String.IsNullOrEmpty(Me.Session("redirectURL")) Then
                            Me.Response.Redirect("~/default")
                        Else
                            Me.Response.Redirect(Me.Session("redirectURL"))
                        End If

                    End If


                Else

                    Me.alert.Visible = True
                    Me.lblerr_user.Text = "<Br>[ System error: Unable to create the session ] <Br><Br> Please contact Your System Administrator"
                    Me.lblerr_user.Visible = True

                End If

                '*************************************ESTABLECEMOS EL TOKEN PARA ESTA SESION*************************


            End If

        Catch ex As Exception

            Me.alert.Visible = True
            Me.lblerr_user.Text = "<Br>[ System error: " & ex.Message & " ] <Br><Br> Please contact Your System Administrator"
            Me.lblerr_user.Visible = True

        End Try

    End Sub
End Class