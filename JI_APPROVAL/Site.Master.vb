'Imports ly_RMS
Imports System.Globalization
Imports System.Threading

Public Class SiteMaster
    Inherits MasterPage

    Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Dim _antiXsrfTokenValue As String
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_MASTER_PAG"
    Dim controles As New ly_SIME.CORE.cls_controles

    Const cAPPROVAL_SYS = 2
    Const cSIME_SYS = 1

    'Dim cProgram As New RMS.cls_Program
    'Dim tbl_sys As New DataTable


    Public Property urlSys As String


    Protected Sub Page_Init(sender As Object, e As System.EventArgs)

        ' The code below helps to protect against XSRF attacks
        Dim requestCookie As HttpCookie = Request.Cookies(AntiXsrfTokenKey)
        Dim requestCookieGuidValue As Guid
        If ((Not requestCookie Is Nothing) AndAlso Guid.TryParse(requestCookie.Value, requestCookieGuidValue)) Then
            ' Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value
            Page.ViewStateUserKey = _antiXsrfTokenValue
        Else
            ' Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N")
            Page.ViewStateUserKey = _antiXsrfTokenValue

            Dim responseCookie As HttpCookie = New HttpCookie(AntiXsrfTokenKey) With {.HttpOnly = True, .Value = _antiXsrfTokenValue}
            If (FormsAuthentication.RequireSSL And Request.IsSecureConnection) Then
                responseCookie.Secure = True
            End If
            Response.Cookies.Set(responseCookie)
        End If

        Dim Sys_url As String = Request.Url.GetLeftPart(UriPartial.Authority)
        urlSys = Request.Url.GetLeftPart(UriPartial.Authority) ' Sys_url.Substring(0, Strings.InStr(Sys_url, "/"))

        AddHandler Page.PreLoad, AddressOf master_Page_PreLoad

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then

            cl_user = Session.Item("clUser")

            If Not Me.Request.QueryString("idLanguageSystem") Is Nothing Then

                cl_user.id_idioma = Me.Request.QueryString("idLanguageSystem")
                Me.Session("E_SubRegion") = cl_user.get_users_region(cl_user.id_idioma)

                Using db As New ly_SIME.dbRMS_JIEntities
                    cl_user.controles_otros = db.vw_control_otro_idioma.Where(Function(p) p.id_idioma = cl_user.id_idioma).ToList()
                End Using
            End If

        Else

            If Me.Request.QueryString("ab") IsNot Nothing And Me.Request.QueryString("ad") IsNot Nothing And Me.Request.QueryString("ad") IsNot Nothing Then

                Dim IDActivity_Solicitation As Guid = Guid.Parse(Me.Request.QueryString("ab").ToString())
                Dim IDSolicitation_AP As Guid = Guid.Parse(Me.Request.QueryString("ac").ToString())
                Dim IDSolicitation_AP_USER_TK As Guid = Guid.Parse(Me.Request.QueryString("ad").ToString())

                Using dbEntities As New ly_SIME.dbRMS_JIEntities

                    Dim oUserToken = dbEntities.VW_T_TOKEN_USERS.Where(Function(p) p.tk_Token = IDSolicitation_AP_USER_TK).FirstOrDefault()
                    Dim id_usuario = 2075 'Potential grantee user
                    'oUserToken.id_usr
                    Dim id_programa = oUserToken.id_programa

                    Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.SOLICITATION_TOKEN = IDSolicitation_AP).FirstOrDefault()

                    cl_user = New ly_SIME.CORE.cls_user(2, id_usuario, id_programa) 'whitout Activity and System
                    cl_user.setRemoteAddr(GET_HOST_DAT("HOST"))
                    cl_user.setUsrAgent(GET_HOST_DAT("AGENT"))
                    cl_user.setLocalAddr(GET_HOST_DAT("LOCAL"))

                    '*************************************ESTABLECEMOS EL TOKEN PARA ESTA SESION*************************
                    '***************************************************************************************************************

                    Dim vIdGuiToken As String = cl_user.CreateLoginToken()
                    If vIdGuiToken.ToString <> "-1" Then

                        Me.Session("idGuiToken") = vIdGuiToken.ToString 'New Token

                        Me.Session("E_IdUser") = cl_user.getUserField("id_usuario", "id_usuario", id_usuario)
                        Me.Session("E_Programa") = cl_user.getUsuarioField("nombre_programa", "id_usuario", id_usuario)
                        Me.Session("E_IDPrograma") = id_programa

                        '******************CHANGE THIS ONE
                        Me.Session("E_Nombre") = String.Format("{0} ({1})", oSolicitationAPP.ORGANIZATIONNAME, oSolicitationAPP.NAMEALIAS)
                        cl_user.id_idioma = cl_user.getUsuarioField("id_idioma", "id_usuario", id_usuario)
                        cl_user.codigo_nivel_usuario = cl_user.getUsuarioField("codigo_nivel_usuario", "id_usuario", id_usuario)

                        Using db As New ly_SIME.dbRMS_JIEntities

                            'Dim id_programa = Convert.ToInt32(cl_user.getUsuarioField("id_programa", "id_usuario", id_usuario))

                            cl_user.regionalizacion = db.t_programas.Find(id_programa).t_pais.t_regionalizaciones.codigo_regionalizacion
                            cl_user.regionalizacionCulture = New CultureInfo(cl_user.regionalizacion)

                            If Not String.IsNullOrEmpty(db.t_programas.Find(id_programa).t_pais.t_regionalizaciones.currency_symbol) Then
                                cl_user.set_CurrencySymbol(db.t_programas.Find(id_programa).t_pais.t_regionalizaciones.currency_symbol)
                            End If

                            cl_user.controles_otros = db.vw_control_otro_idioma.Where(Function(p) p.id_idioma = cl_user.id_idioma).ToList()
                            Dim string_programa = id_programa.ToString()

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

                        End If

                    Else

                        'Me.alert.Visible = True
                        'Me.lblerr_user.Text = "<Br>[ System error: Unable to create the session ] <Br><Br> Please contact Your System Administrator"
                        'Me.lblerr_user.Visible = True

                    End If

                End Using


            End If




        End If

    End Sub



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

    Private Sub master_Page_PreLoad(sender As Object, e As System.EventArgs)
        If (Not IsPostBack) Then
            ' Set Anti-XSRF token
            ViewState(AntiXsrfTokenKey) = Page.ViewStateUserKey
            ViewState(AntiXsrfUserNameKey) = If(Context.User.Identity.Name, String.Empty)
        Else
            ' Validate the Anti-XSRF token
            If (Not DirectCast(ViewState(AntiXsrfTokenKey), String) = _antiXsrfTokenValue _
                Or Not DirectCast(ViewState(AntiXsrfUserNameKey), String) = If(Context.User.Identity.Name, String.Empty)) Then
                Throw New InvalidOperationException("Validation of Anti-XSRF token failed.")
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        'Try
        If Not IsPostBack Then

            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControlsMaster(Control, cl_user.id_idioma)
            Next
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Me.lbl_programa.Text = Me.Session("E_Programa").ToString()
            Me.lbl_subregion.Text = Me.Session("E_subregion").ToString()


            'If id_programa <> 0 Then

            '    tbl_sys = cProgram.get_Sys(id_programa, True)
            '    If tbl_sys.Rows.Count = 1 And tbl_sys.Rows.Item(0).Item("id_sys") > 0 Then
            '        urlSys = tbl_sys.Rows.Item(0).Item("sys_url")
            '    End If

            'End If

            'Me.lbl_rol_id.Text = cl_user.id_rolUsr

            Using db As New ly_SIME.dbRMS_JIEntities

                Dim oPrograma = db.t_programas.Find(id_programa)

                If Not String.IsNullOrEmpty(oPrograma.imgName) Then
                    imgProgram.ImageUrl = ResolveUrl(oPrograma.imgName)
                Else
                    imgProgram.ImageUrl = ResolveUrl("images/activities/logo_Chemonics_nw.png")
                End If


                '**********Dim aa = db.vw_t_menu_idioma.Where(Function (p) Not p.parent_item_menu.HasValue).ToList()

                'Dim a = db.vw_t_menu_rol.Where(Function(p) Not p.parent_item_menu.HasValue And p.id_idioma = cl_user.id_idioma And p.id_sys = cAPPROVAL_SYS _
                '                                   And (p.id_rol = cl_user.id_rolUsr Or Not p.id_rol.HasValue)) _
                '                               .OrderBy(Function(p) p.orden_item_menu).ToList()

                Dim a = db.vw_menu.Where(Function(p) p.id_idioma = cl_user.id_idioma And p.id_sys = cAPPROVAL_SYS _
                                                  And (cl_user.arr_roles_Usr.Contains(p.id_rol) Or (p.id_programa = id_programa And p.id_usuario = cl_user.id_usr))) _
                                 .OrderBy(Function(p) p.OrdenPadre) _
                                 .Select(Function(p) _
                                             New With {Key .icono_clase = p.IconoPadre,
                                                       Key .valor = p.ValorPadre,
                                                       Key .id_menu = p.parent_item_menu.Value,
                                                       Key .URL_item_menu = "",
                                                       Key .type = 1,
                                                       Key .ordenPadre = p.OrdenPadre.Value}).Distinct().OrderBy(Function(p) p.ordenPadre).ToList()


                rptFirstLevel.DataSource = a
                rptFirstLevel.DataBind()

                Dim idiomas = db.vw_t_programa_idiomas.Where(Function(p) p.id_programa = id_programa).ToList()
                For Each item In idiomas
                    Dim links As New HtmlAnchor
                    links.HRef = "~/Default?idLanguageSystem=" & item.id_idioma
                    links.Attributes.Add("class", "col-sm-6")
                    Dim img = New Image
                    img.ImageUrl = ResolveUrl(item.bandera)
                    links.Controls.Add(img)
                    Me.divIdiomas.Controls.Add(links)
                Next
                Dim idiomaActual = idiomas.FirstOrDefault(Function(p) p.id_idioma = cl_user.id_idioma)
                If idiomaActual IsNot Nothing Then
                    Dim img = New Image
                    img.ImageUrl = ResolveUrl(idiomaActual.bandera)
                    Me.div_bandera.Controls.Add(img)
                End If


            End Using

        End If
        'Catch ex As Exception
        '    Me.Response.Redirect("~/frm_login")
        'End Try

    End Sub

    Protected Sub rptFirstLevel_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptFirstLevel.ItemDataBound

        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())

        Using db As New ly_SIME.dbRMS_JIEntities
            Dim padre = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_menu").ToString())

            Dim submenu = db.vw_menu.Where(Function(p) p.parent_item_menu = padre And p.id_idioma = cl_user.id_idioma _
                                                  And (cl_user.arr_roles_Usr.Contains(p.id_rol) Or (p.id_programa = id_programa And p.id_usuario = cl_user.id_usr))) _
                                 .OrderBy(Function(p) p.orden_item_menu) _
                                 .Select(Function(p) _
                                          New With {
                                            Key .valor = p.ValorIdioma,
                                            Key .URL_item_menu = p.URL_item_menu,
                                            Key .orden_item_menu = p.orden_item_menu}).Distinct().OrderBy(Function(p) p.orden_item_menu).ToList()


            Dim repeater = CType(e.Item.FindControl("rptSecondLevel"), Repeater)
            repeater.Visible = True
            repeater.DataSource = submenu
            repeater.DataBind()

        End Using
    End Sub
End Class