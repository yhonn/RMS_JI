Imports System.Globalization
Imports System.Threading

Public Class PartnerMaster
    Inherits MasterPage

    Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Dim _antiXsrfTokenValue As String
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "MASTER_PAG"
    Dim controles As New ly_SIME.CORE.cls_controles

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
            Using db As New ly_SIME.dbRMS_JIEntities
                Dim periodo
                If db.vw_t_periodos.Where(Function(p) p.id_programa = id_programa And p.activo).Count() > 0 Then
                    periodo = db.vw_t_periodos.FirstOrDefault(Function(p) p.id_programa = id_programa And p.activo)
                    Me.lbl_periodo_activo.Text = Convert.ToString(periodo.anio) & " - " & periodo.codigo_anio_fiscal
                Else
                    Me.lbl_periodo_activo.Text = "--"
                End If
            End Using
            If Me.Session("E_Proyecto") IsNot Nothing Then
                Me.lbl_proyecto.Text = Me.Session("E_Proyecto").ToString()
            End If


            Using db As New ly_SIME.dbRMS_JIEntities
                Dim oPrograma = db.t_programas.Find(id_programa)

                If Not String.IsNullOrEmpty(oPrograma.imgName) Then
                    imgProgram.ImageUrl = ResolveUrl(oPrograma.imgName)
                Else
                    imgProgram.ImageUrl = ResolveUrl("images/activities/logo_Chemonics_nw.png")
                End If

                'Dim aa = db.vw_t_menu_idioma.Where(Function (p) Not p.parent_item_menu.HasValue).ToList()
                'Dim menuNormal = db.vw_t_menu_rol.Where(Function(p) Not p.parent_item_menu.HasValue And p.id_idioma = cl_user.id_idioma And p.id_sys = 1 _
                '                                   And (p.id_rol = cl_user.id_rolUsr Or Not p.id_rol.HasValue)) _
                '                               .OrderBy(Function(p) p.orden_item_menu) _
                '                               .Select(Function(p) _
                '                                           New With {Key .icono_clase = p.icono_clase, _
                '                                                     Key .valor = p.valor,
                '                                                     Key .id_menu = p.id_menu,
                '                                                     Key .URL_item_menu = p.URL_item_menu,
                '                                                     Key .type = 1}).ToList()

                'Nuevos comentarios'


                'Dim menuNormal = db.vw_menu.Where(Function(p) p.id_idioma = cl_user.id_idioma And p.id_sys = 1 And p.id_rol = cl_user.id_rolUsr) _
                '                 .OrderBy(Function(p) p.OrdenPadre) _
                '                 .Select(Function(p) _
                '                             New With {Key .icono_clase = p.IconoPadre, _
                '                                       Key .valor = p.ValorPadre,
                '                                       Key .id_menu = p.parent_item_menu.Value,
                '                                       Key .URL_item_menu = "",
                '                                       Key .type = 1,
                '                                       Key .ordenPadre = p.OrdenPadre.Value}).Distinct().OrderBy(Function(p) p.ordenPadre).ToList()

                Dim menuNormal = db.vw_menu.Where(Function(p) p.id_idioma = cl_user.id_idioma And p.id_sys = 1 _
                                                  And (cl_user.arr_roles_Usr.Contains(p.id_rol) Or (p.id_programa = id_programa And p.id_usuario = cl_user.id_usr))) _
                                 .OrderBy(Function(p) p.OrdenPadre) _
                                 .Select(Function(p) _
                                             New With {Key .icono_clase = p.IconoPadre,
                                                       Key .valor = p.ValorPadre,
                                                       Key .id_menu = p.parent_item_menu.Value,
                                                       Key .URL_item_menu = "",
                                                       Key .type = 1,
                                                       Key .ordenPadre = p.OrdenPadre.Value}).Distinct().OrderBy(Function(p) p.ordenPadre).ToList()


                Dim instrumentosProyecto = db.vw_tme_instrumentos_new.Where(Function(p) Not p.id_instrumento_padre.HasValue) _
                                   .Select(Function(p) _
                                               New With {Key .icono_clase = "fa-wrench",
                                               Key .valor = p.nombre_instrumento,
                                               Key .id_menu = p.id_instrumento,
                                               Key .URL_item_menu = "",
                                               Key .type = 2,
                                               Key .ordenPadre = 99}).ToList()
                If Not Me.Session("E_IdFicha") Is Nothing Then
                    rptFirstLevel.DataSource = menuNormal.Union(instrumentosProyecto)
                Else
                    rptFirstLevel.DataSource = menuNormal
                End If

                rptFirstLevel.DataBind()

                Dim idiomas = db.vw_t_programa_idiomas.Where(Function(p) p.id_programa = id_programa).Select(Function(p) _
                                               New With {Key .id_idioma = p.id_idioma,
                                               Key .bandera = p.bandera,
                                               Key .prefix = p.prefix}).ToList()

                For Each item In idiomas
                    Dim links As New HtmlAnchor
                    links.HRef = "~/Default?idLanguageSystem=" & item.id_idioma
                    links.Attributes.Add("class", "col-sm-6")
                    links.InnerHtml = String.Format("<span class='badge badge-light'>{0}</span>", item.prefix)

                    'Dim img = New Image
                    'img.ImageUrl = ResolveUrl(item.bandera)
                    'links.Controls.Add(img)

                    Me.divIdiomas.Controls.Add(links)
                Next


                Dim idiomaActual = idiomas.FirstOrDefault(Function(p) p.id_idioma = cl_user.id_idioma)
                If idiomaActual IsNot Nothing Then
                    'Dim img = New Image
                    'img.ImageUrl = ResolveUrl(idiomaActual.bandera)
                    'Me.div_bandera.Controls.Add(img)
                    Dim links As New HtmlAnchor
                    'links.HRef = "~/Default?idLanguageSystem=" & item.id_idioma
                    links.Attributes.Add("class", "col-sm-6 text-center")
                    links.InnerHtml = String.Format("<span class='badge badge-dark'>{0}</span>", idiomaActual.prefix)
                    Me.div_bandera.Controls.Add(links)
                End If

            End Using

        End If
        'Catch ex As Exception
        '    Me.Response.Redirect("~/frm_login")
        'End Try

    End Sub

    Protected Sub rptFirstLevel_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptFirstLevel.ItemDataBound
        Using db As New ly_SIME.dbRMS_JIEntities
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim padre = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_menu").ToString())
            Dim tipo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "type").ToString())
            'Dim submenu = db.vw_t_menu_rol.Where(Function(p) p.id_rol = cl_user.id_rolUsr And p.parent_item_menu.HasValue And p.parent_item_menu = padre _
            '                                      And p.id_idioma = cl_user.id_idioma And p.idiomaPadre = cl_user.id_idioma) _
            '                                  .OrderBy(Function(p) p.orden_item_menu) _
            '                                  .Select(Function(p) _
            '                                               New With { _
            '                                                         Key .valor = p.valor,
            '                                                         Key .URL_item_menu = p.URL_item_menu}).ToList()

            Dim submenu = db.vw_menu.Where(Function(p) p.parent_item_menu = padre And p.id_idioma = cl_user.id_idioma _
                                                   And (cl_user.arr_roles_Usr.Contains(p.id_rol) Or (p.id_programa = id_programa And p.id_usuario = cl_user.id_usr))) _
                                  .OrderBy(Function(p) p.orden_item_menu) _
                                  .Select(Function(p) _
                                           New With {
                                             Key .valor = p.ValorIdioma,
                                             Key .URL_item_menu = p.URL_item_menu,
                                             Key .orden_item_menu = p.orden_item_menu}).Distinct().OrderBy(Function(p) p.orden_item_menu).ToList()

            Dim id_ficha = 0
            If Not Me.Session("E_IdFicha") Is Nothing Then
                id_ficha = Convert.ToInt32(Me.Session("E_IdFicha"))
            End If
            Dim instrumentosProyecto = db.vw_tme_instrumentos_proyecto.Where(Function(p) p.id_instrumento_padre = padre And (p.id_ficha_proyecto = id_ficha Or id_ficha = 0)) _
                                   .Select(Function(p) _
                                               New With {
                                               Key .valor = p.nombre_instrumento,
                                               Key .URL_item_menu = p.URL,
                                               Key .orden_item_menu = 100}).ToList()
            Dim repeater = CType(e.Item.FindControl("rptSecondLevel"), Repeater)
            repeater.Visible = True
            If tipo = 1 Then
                repeater.DataSource = submenu
            Else
                repeater.DataSource = instrumentosProyecto
            End If


            repeater.DataBind()
        End Using
    End Sub
End Class