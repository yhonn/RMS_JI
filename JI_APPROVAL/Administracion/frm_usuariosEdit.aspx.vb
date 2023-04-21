Imports CuteWebUI
Imports System.Data.SqlClient
Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Configuration.ConfigurationManager
Public Class frm_usuariosEdit
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim clListado As New ly_SIME.CORE.cls_listados
    Dim frmCODE As String = "AP_EDIT_USU"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_files As New ly_SIME.CORE.cls_files
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Usuarios\"
    Dim sFileDirTemp As String = Server.MapPath("~") & "\Temp\"
    Dim sFileDirUbicacion As String = "~/FileUploads/Usuarios/"
    Dim extension, File As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls

                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then
            Me.lbl_id_usuario.Text = Me.Request.QueryString("Id").ToString
            FillData(Me.lbl_id_usuario.Text)
        End If
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click

        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False

        If Val(Me.cmb_job_title.SelectedValue) = 0 Then
            Me.lbl_JobErr.Visible = True
            errSave = True
        Else
            Me.lbl_JobErr.Visible = False
            errSave = False
        End If

        If Val(Me.cmb_supervisor.SelectedValue) = 0 Then
            Me.lbl_supervisorErr.Visible = True
            errSave = True
        Else
            Me.lbl_supervisorErr.Visible = False
            errSave = False
        End If

        Dim fileName = Me.lbl_oldFile.Value

        If lbl_hasFiles.Value = "true" Then
            fileName = Me.lbl_archivo_uploaded.Value
        End If

        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())

        If chk_PasswordChange.Checked Then
            Me.pass_validator.Validate()
            If Not Me.pass_validator.IsValid Then
                errSave = True
            End If
        End If



        If errSave = False Then

            Using dbEntities As New dbRMS_JIEntities
                Dim oUsuario As New t_usuarios
                Dim query = ""
                Dim parameters() As SqlParameter = New SqlParameter() _
                        {
                            New SqlParameter("@nombre_usuario", SqlDbType.VarChar) With {.Value = Me.txt_nombreUsuario.Text},
                            New SqlParameter("@apellidos_usuario", SqlDbType.VarChar) With {.Value = Me.txt_apellidos.Text},
                            New SqlParameter("@email_usuario", SqlDbType.VarChar) With {.Value = Me.txt_email_usuario.Text},
                            New SqlParameter("@usuario", SqlDbType.VarChar) With {.Value = Me.txt_usuarioSistema.Text},
                            New SqlParameter("@datecreated", SqlDbType.DateTime) With {.Value = Date.UtcNow},
                            New SqlParameter("@id_job_title", SqlDbType.Int) With {.Value = Me.cmb_job_title.SelectedValue},
                            New SqlParameter("@id_tipo_nivel_usuario", SqlDbType.Int) With {.Value = Me.cmb_tipo_nivel.SelectedValue},
                            New SqlParameter("@id_tipo_usuario", SqlDbType.Int) With {.Value = Me.cmb_tipo_usuario.SelectedValue},
                            New SqlParameter("@imagen", SqlDbType.VarChar) With {.Value = fileName},
                            New SqlParameter("@fecha_contrato", SqlDbType.Date) With {.Value = Me.dt_fecha_contrato.SelectedDate},
                            New SqlParameter("@id_usuario_upd", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.Session("E_IdUser").ToString())},
                            New SqlParameter("@id_estado_usr", SqlDbType.Int) With {.Value = Me.rb_estado.SelectedValue},
                            New SqlParameter("@id_usuario_padre", SqlDbType.Int) With {.Value = If(Not Me.chk_SupervisorNA.Checked, Convert.ToInt32(Me.cmb_supervisor.SelectedValue), 0)},
                            New SqlParameter("@upddate", SqlDbType.DateTime) With {.Value = Date.UtcNow},
                            New SqlParameter("@numero_documento", SqlDbType.BigInt) With {.Value = Me.txt_numero_documento.Value},
                            New SqlParameter("@codigo_usuario", SqlDbType.VarChar) With {.Value = Me.txt_codigo.Text},
                            New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = Me.lbl_id_usuario.Text}
                        }

                query = "UPDATE t_usuarios SET nombre_usuario = @nombre_usuario, apellidos_usuario = @apellidos_usuario, email_usuario = @email_usuario, usuario = @usuario, " &
                            "id_job_title = @id_job_title, id_tipo_nivel_usuario = @id_tipo_nivel_usuario, id_tipo_usuario = @id_tipo_usuario, " &
                            "imagen = @imagen, fecha_contrato = @fecha_contrato, id_usuario_upd = @id_usuario_upd, id_estado_usr = @id_estado_usr, 
                            id_usuario_padre = @id_usuario_padre, numero_documento = @numero_documento, codigo_usuario = @codigo_usuario, " &
                            "upddate = @upddate WHERE id_usuario = @id_usuario"
                dbEntities.Database.ExecuteSqlCommand(query, parameters)


                If Me.chk_PasswordChange.Checked Then

                    Dim parametersPSW() As SqlParameter = New SqlParameter() _
                        {
                            New SqlParameter("@clave", SqlDbType.VarChar) With {.Value = Me.txt_clave_usuario.Text},
                            New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = Me.lbl_id_usuario.Text}
                        }

                    query = "UPDATE t_usuarios SET clave = HashBytes('SHA', @clave) WHERE id_usuario = @id_usuario"
                    dbEntities.Database.ExecuteSqlCommand(query, parametersPSW)

                End If


                Dim parameters2() As SqlParameter = New SqlParameter() _
                        {
                            New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.lbl_id_usuario.Text)},
                            New SqlParameter("@id_programa", SqlDbType.Int) With {.Value = id_programa},
                            New SqlParameter("@id_idioma", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_idioma.SelectedValue)}
                        }
                Dim query2 = "UPDATE t_usuario_idioma SET id_idioma = @id_idioma WHERE id_usuario = @id_usuario and id_programa = @id_programa"
                dbEntities.Database.ExecuteSqlCommand(query2, parameters2)

                If lbl_hasFiles.Value = "true" Then

                    Dim user_route = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).url_user_images
                    Dim path As String = user_route
                    path = user_route.Substring(0, path.Trim.Length - 1)
                    Dim idx1 As Integer = path.LastIndexOf("/") + 1
                    Dim account As String = path.Substring(idx1, path.Length - idx1)
                    cl_files.SaveFileCloud(account, Server.MapPath("~") & "\Temp\", Me.lbl_archivo_uploaded.Value)
                    Dim user_uri As Uri
                    Uri.TryCreate(user_route & Me.lbl_archivo_uploaded.Value, UriKind.Absolute, user_uri)

                End If
                Dim busquedaActividad = If(Me.rbn_actividades_habilitadas.SelectedValue = 1, True, False)
                Dim id_filtro_busqueda_viajes = Convert.ToInt32(Me.cmb_filtro_viajes.SelectedValue)
                Dim parameters3() As SqlParameter = New SqlParameter() _
                        {
                            New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.lbl_id_usuario.Text)},
                            New SqlParameter("@id_programa", SqlDbType.Int) With {.Value = id_programa},
                            New SqlParameter("@id_job_title", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_job_title.SelectedValue)},
                            New SqlParameter("@busqueda_actividad", SqlDbType.Int) With {.Value = busquedaActividad},
                            New SqlParameter("@id_filtro_busqueda_viajes", SqlDbType.Int) With {.Value = id_filtro_busqueda_viajes}
                        }

                Dim query3 = "UPDATE t_usuario_programa SET id_job_title = @id_job_title, busqueda_actividad = @busqueda_actividad, id_filtro_busqueda_viajes = @id_filtro_busqueda_viajes WHERE id_usuario = @id_usuario and id_programa = @id_programa"
                dbEntities.Database.ExecuteSqlCommand(query3, parameters3)

                If cmb_tipo_usuario.SelectedValue = 2 Then

                    Dim idUSR As Integer = Convert.ToInt32(Me.lbl_id_usuario.Text)

                    If dbEntities.t_ejecutor_usuario.Where(Function(p) p.id_usuario = idUSR).Count() > 0 Then
                        query = String.Concat("UPDATE t_ejecutor_usuario SET id_ejecutor = " & Me.cmb_ejecutor.SelectedValue & " WHERE id_usuario = " & Me.lbl_id_usuario.Text)
                        dbEntities.Database.ExecuteSqlCommand(query)
                    Else
                        query = String.Concat("INSERT INTO t_ejecutor_usuario VALUES (", Me.cmb_ejecutor.SelectedValue, ", ", idUSR, ")")
                        dbEntities.Database.ExecuteSqlCommand(query)
                    End If


                End If

                If chk_Notify.Checked Then
                    '*********************************OPEN****************************************
                    Dim objEmail As New SIMEly.cls_notification(Me.Session("E_IDPrograma"), 7, cl_user.regionalizacionCulture, cl_user.idSys)

                    If (objEmail.Emailing_USER_UPD(Me.lbl_id_usuario.Text, If(chk_PasswordChange.Checked, Me.txt_clave_usuario.Text, ""))) Then
                    Else 'Error mandando Email
                    End If
                    '*********************************OPEN****************************************
                End If

                If Not Me.chk_SupervisorNA.Checked Then
                    Dim cmd As New SqlCommand("SP_asignar_rol_usuario", cnn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = Me.cmb_supervisor.SelectedValue
                    cmd.Parameters.Add("@idTipo", SqlDbType.Int).Value = 2
                    cmd.Connection.Open()
                    cmd.ExecuteNonQuery()
                End If

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Administracion/frm_usuarios"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If

    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Administracion/frm_usuarios"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Sub FillData(ByVal idUsuario As String)

        Dim id_usuario = Convert.ToInt32(idUsuario)
        Using dbEntities As New dbRMS_JIEntities
            Dim oUsuario = dbEntities.t_usuarios.Find(id_usuario)
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim oConfigUsuario = dbEntities.t_usuario_programa.FirstOrDefault(Function(p) p.id_programa = id_programa And p.id_usuario = id_usuario)

            Me.txt_nombreUsuario.Text = oUsuario.nombre_usuario
            Me.txt_apellidos.Text = oUsuario.apellidos_usuario
            Me.txt_email_usuario.Text = oUsuario.email_usuario
            Me.txt_usuarioSistema.Text = oUsuario.usuario
            Me.txt_clave_usuario.Text = "ThiIsmYPassWoR"
            Me.txt_clave_usuario.Enabled = False
            Me.chk_PasswordChange.Checked = False

            If oUsuario.tipo IsNot Nothing Then
                Me.rbn_tipo.SelectedValue = oUsuario.tipo
            End If
            Me.txt_codigo.Text = oUsuario.codigo_usuario
            Me.txt_numero_documento.Value = oUsuario.numero_documento

            Me.rbn_actividades_habilitadas.SelectedValue = If(oConfigUsuario.busqueda_actividad, 1, 2)
            Me.dt_fecha_contrato.SelectedDate = oUsuario.fecha_contrato

            Dim user_uri As Uri
            Dim user_route = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).url_user_images
            Uri.TryCreate(user_route & oUsuario.imagen, UriKind.Absolute, user_uri)
            Me.imgUser.ImageUrl = user_uri.AbsoluteUri.ToString()
            Me.lbl_oldFile.Value = oUsuario.imagen

            Me.lbl_date_upd.Text = If(oUsuario.upddate.HasValue, CDate(oUsuario.upddate).ToShortDateString(), "")
            Me.lbl_id_usrupdate.Text = If(IsDBNull(oUsuario.id_usuario_upd), "", dbEntities.t_usuarios.FirstOrDefault(Function(p) p.id_usuario_upd = oUsuario.id_usuario_upd).usuario)

            Me.cmb_supervisor.DataSourceID = ""
            Me.cmb_supervisor.DataSource = clListado.get_t_usuarios(id_programa)
            Me.cmb_supervisor.DataTextField = "nombre_usuario"
            Me.cmb_supervisor.DataValueField = "id_usuario"
            Me.cmb_supervisor.DataBind()
            If (oUsuario.id_usuario_padre.HasValue) Then
                Me.cmb_supervisor.SelectedValue = oUsuario.id_usuario_padre
            End If

            'Me.cmb_tipo_nivel.DataSourceID = ""
            'Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa)
            'Me.cmb_tipo_nivel.DataTextField = "codigo_nivel_usuario"
            'Me.cmb_tipo_nivel.DataValueField = "id_tipo_nivel_usuario"
            'Me.cmb_tipo_nivel.DataBind()
            'Me.cmb_tipo_nivel.SelectedValue = oUsuario.id_tipo_nivel_usuario

            Me.cmb_tipo_usuario.DataSourceID = ""
            Me.cmb_tipo_usuario.DataSource = clListado.get_t_tipo_usuario()
            Me.cmb_tipo_usuario.DataTextField = "nombre_tipo_usuario"
            Me.cmb_tipo_usuario.DataValueField = "id_tipo_usuario"
            Me.cmb_tipo_usuario.DataBind()
            Me.cmb_tipo_usuario.SelectedValue = oUsuario.id_tipo_usuario

            Me.cmb_ejecutor.DataSourceID = ""
            Me.cmb_ejecutor.DataSource = clListado.get_t_ejecutores(id_programa)
            Me.cmb_ejecutor.DataTextField = "nombre_ejecutor"
            Me.cmb_ejecutor.DataValueField = "id_ejecutor"
            Me.cmb_ejecutor.DataBind()

            Me.rb_estado.DataSource = clListado.get_t_estado_usuario()
            Me.rb_estado.DataTextField = "nombre_estado_usuario"
            Me.rb_estado.DataValueField = "id_estado_usuario"
            Me.rb_estado.RepeatColumns = 2
            Me.rb_estado.DataBind()
            Me.rb_estado.SelectedValue = oUsuario.id_estado_usr

            Me.cmb_idioma.DataSource = dbEntities.vw_t_programa_idiomas.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_idioma.DataTextField = "descripcion_idioma"
            Me.cmb_idioma.DataValueField = "id_idioma"
            Me.cmb_idioma.DataBind()
            Me.cmb_idioma.SelectedValue = oConfigUsuario.id_idioma

            Me.cmb_job_title.DataSource = clListado.get_t_job_title(id_programa)
            Me.cmb_job_title.DataTextField = "job"
            Me.cmb_job_title.DataValueField = "id_job_title"
            Me.cmb_job_title.DataBind()
            Me.cmb_job_title.SelectedValue = oConfigUsuario.id_job_title

            Me.cmb_filtro_viajes.DataSourceID = ""
            Me.cmb_filtro_viajes.DataSource = clListado.get_tme_filtro_busqueda_viajes(id_programa)
            Me.cmb_filtro_viajes.DataTextField = "filtro_busqueda"
            Me.cmb_filtro_viajes.DataValueField = "id_filtro_busqueda_viajes"
            Me.cmb_filtro_viajes.DataBind()
            Me.cmb_filtro_viajes.SelectedValue = oConfigUsuario.id_filtro_busqueda_viajes

            Dim oUsuarioActual = dbEntities.t_usuarios.Find(cl_user.id_usr)
            Dim nivel = oUsuarioActual.t_tipo_nivel_usuario
            'Dim t_levels_user = dbEntities.t_tipo_nivel_usuario.Where(Function(P) P.id_programa = id_programa).ToList()

            If nivel.codigo_nivel_usuario = "SYS_USER" Then

                Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa, "SYS_USER")
                Me.cmb_tipo_nivel.DataTextField = "codigo_nivel_usuario"
                Me.cmb_tipo_nivel.DataValueField = "id_tipo_nivel_usuario"
                Me.cmb_tipo_nivel.DataBind()
                ' Me.cmb_tipo_nivel.SelectedValue = t_levels_user.Where(Function(p) p.codigo_nivel_usuario = "SYS_USER").FirstOrDefault().id_tipo_nivel_usuario
                Me.cmb_tipo_nivel.SelectedValue = oUsuario.id_tipo_nivel_usuario
                Me.cmb_tipo_nivel.Enabled = False
                Me.div_nivel_usuario.Visible = False

                If oUsuario.t_tipo_nivel_usuario.codigo_nivel_usuario = "SYS_ADMIN" Or oUsuario.t_tipo_nivel_usuario.codigo_nivel_usuario = "SYS_P_ADM" Then
                    Me.btn_guardar.Enabled = False
                    Me.lbl_mensajeError.Text = "Access denied"
                End If

            Else

                If nivel.codigo_nivel_usuario = "SYS_P_ADM" Then
                    Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa, "SYS_USER,SYS_P_ADM")
                    If oUsuario.t_tipo_nivel_usuario.codigo_nivel_usuario = "SYS_ADMIN" Then
                        Me.btn_guardar.Enabled = False
                        Me.lbl_mensajeError.Text = "Access denied"
                    End If
                Else
                    Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa, "SYS_USER,SYS_P_ADM,SYS_ADMIN")
                End If

                Me.cmb_tipo_nivel.DataTextField = "codigo_nivel_usuario"
                Me.cmb_tipo_nivel.DataValueField = "id_tipo_nivel_usuario"
                Me.cmb_tipo_nivel.DataBind()
                Me.cmb_tipo_nivel.SelectedValue = oUsuario.id_tipo_nivel_usuario
                Me.div_nivel_usuario.Visible = True
                Me.cmb_tipo_nivel.Enabled = True

            End If

            If oUsuario.id_tipo_usuario = 2 Then
                Me.divEjecutor.Visible = True
                Me.cmb_ejecutor.SelectedValue = dbEntities.t_ejecutor_usuario.FirstOrDefault(Function(p) p.id_usuario = oUsuario.id_usuario).id_ejecutor
            End If

            'Me.grd_ejecutores.DataSource = ""
            'Me.grd_ejecutores.DataSource = dbEntities.t_ejecutores.Where(Function(p) p.id_programa = id_programa) _
            '    .Select(Function(p) _
            '                New With {Key .nombre_ejecutor = p.nombre_ejecutor, _
            '                          Key .id_ejecutor = p.id_ejecutor}).ToList()
            'Me.grd_ejecutores.DataBind()

            'For Each item In dbEntities.t_ejecutor_usuario.Where(Function(p) p.id_usuario = id_usuario)
            '    For Each row In Me.grd_ejecutores.Items
            '        If TypeOf row Is GridDataItem Then
            '            Dim dataItem As GridDataItem = CType(row, GridDataItem)
            '            Dim EjeS As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_ejecutor"), CheckBox)
            '            Dim id_ejecutor As Integer = dataItem.GetDataKeyValue("id_ejecutor")
            '            If item.id_ejecutor = id_ejecutor Then
            '                EjeS.Checked = True
            '            End If
            '        End If
            '    Next
            'Next



        End Using
    End Sub

    Protected Sub cmb_tipo_usuario_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_tipo_usuario.SelectedIndexChanged
        If cmb_tipo_usuario.SelectedValue = 2 Then
            Me.divEjecutor.Visible = True
        Else
            Me.divEjecutor.Visible = False
        End If

    End Sub

    Private Sub chk_PasswordChange_CheckedChanged(sender As Object, e As EventArgs) Handles chk_PasswordChange.CheckedChanged

        If Me.chk_PasswordChange.Checked = True Then
            Me.txt_clave_usuario.Text = ""
            Me.txt_clave_usuario.Enabled = True
        Else
            Me.txt_clave_usuario.Enabled = False
        End If

    End Sub
End Class
