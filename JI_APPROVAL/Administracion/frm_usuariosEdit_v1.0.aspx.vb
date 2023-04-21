Imports CuteWebUI
Imports System.Data.SqlClient
Imports ly_SIME
Imports ly_APPROVAL


Public Class frm_usuariosEdit_v10
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim clListado As New ly_SIME.CORE.cls_listados
    Dim frmCODE As String = "AP_EDIT_USU"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_files As New ly_SIME.CORE.cls_files

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
                            New SqlParameter("@clave", SqlDbType.VarChar) With {.Value = Me.txt_clave_usuario.Text},
                            New SqlParameter("@datecreated", SqlDbType.DateTime) With {.Value = Date.UtcNow},
                            New SqlParameter("@id_job_title", SqlDbType.Int) With {.Value = Me.cmb_job_title.SelectedValue},
                            New SqlParameter("@id_tipo_nivel_usuario", SqlDbType.Int) With {.Value = Me.cmb_tipo_nivel.SelectedValue},
                            New SqlParameter("@id_tipo_usuario", SqlDbType.Int) With {.Value = Me.cmb_tipo_usuario.SelectedValue},
                            New SqlParameter("@imagen", SqlDbType.VarChar) With {.Value = If(Me.imgUser.ImageUrl.Contains(lbl_imgOriginal.Text), lbl_imgOriginal.Text, Me.lblarchivo.Text)},
                            New SqlParameter("@fecha_contrato", SqlDbType.Date) With {.Value = Me.dt_fecha_contrato.SelectedDate},
                            New SqlParameter("@id_usuario_upd", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.Session("E_IdUser").ToString())},
                            New SqlParameter("@id_estado_usr", SqlDbType.Int) With {.Value = Me.rb_estado.SelectedValue},
                            New SqlParameter("@id_usuario_padre", SqlDbType.Int) With {.Value = If(Not Me.chk_SupervisorNA.Checked, Convert.ToInt32(Me.cmb_supervisor.SelectedValue), 0)},
                            New SqlParameter("@upddate", SqlDbType.DateTime) With {.Value = Date.UtcNow},
                            New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = Me.lbl_id_usuario.Text}
                        }
                query = "UPDATE t_usuarios SET nombre_usuario = @nombre_usuario, apellidos_usuario = @apellidos_usuario, email_usuario = @email_usuario, usuario = @usuario, " &
                    "clave = HashBytes('SHA', @clave), id_job_title = @id_job_title, id_tipo_nivel_usuario = @id_tipo_nivel_usuario, id_tipo_usuario = @id_tipo_usuario, " &
                    "imagen = @imagen, fecha_contrato = @fecha_contrato, id_usuario_upd = @id_usuario_upd, id_estado_usr = @id_estado_usr, id_usuario_padre = @id_usuario_padre," &
                    "upddate = @upddate WHERE id_usuario = @id_usuario"
                dbEntities.Database.ExecuteSqlCommand(query, parameters)
                Dim parameters2() As SqlParameter = New SqlParameter() _
                        {
                            New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.lbl_id_usuario.Text)},
                            New SqlParameter("@id_programa", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())},
                            New SqlParameter("@id_idioma", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_idioma.SelectedValue)}
                        }
                Dim query2 = "UPDATE t_usuario_idioma SET id_idioma = @id_idioma WHERE id_usuario = @id_usuario and id_programa = @id_programa"
                dbEntities.Database.ExecuteSqlCommand(query2, parameters2)


                If Not Me.imgUser.ImageUrl.Contains(lbl_imgOriginal.Text) Then
                    CopyFile()
                End If

                Dim parameters3() As SqlParameter = New SqlParameter() _
                        {
                            New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.lbl_id_usuario.Text)},
                            New SqlParameter("@id_programa", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())},
                            New SqlParameter("@id_job_title", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_job_title.SelectedValue)}
                        }

                Dim query3 = "UPDATE t_usuario_programa SET id_job_title = @id_job_title WHERE id_usuario = @id_usuario and id_programa = @id_programa"
                dbEntities.Database.ExecuteSqlCommand(query3, parameters3)


                If chk_Notify.Checked Then
                    '*********************************OPEN****************************************
                    Dim objEmail As New SIMEly.cls_notification(Me.Session("E_IDPrograma"), 7, cl_user.regionalizacionCulture, cl_user.idSys)
                    If (objEmail.Emailing_USER_UPD(Me.lbl_id_usuario.Text, Me.txt_clave_usuario.Text)) Then
                    Else 'Error mandando Email
                    End If
                    '*********************************OPEN****************************************
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

            Me.dt_fecha_contrato.SelectedDate = oUsuario.fecha_contrato

            Dim user_route = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).url_user_images

            Me.imgUser.ImageUrl = user_route & oUsuario.imagen
            Me.lblarchivo.Text = oUsuario.imagen
            lbl_imgOriginal.Text = oUsuario.imagen

            Me.lbl_date_upd.Text = If(oUsuario.upddate.HasValue, CDate(oUsuario.upddate).ToShortDateString(), "")
            Me.lbl_id_usrupdate.Text = If(IsDBNull(oUsuario.id_usuario_upd), "", dbEntities.t_usuarios.FirstOrDefault(Function(p) p.id_usuario_upd = oUsuario.id_usuario_upd).usuario)

            Me.cmb_supervisor.DataSourceID = ""
            Me.cmb_supervisor.DataSource = clListado.get_t_usuarios()
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
            Me.cmb_ejecutor.DataSource = clListado.get_t_ejecutores()
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

            Dim oUsuarioActual = dbEntities.t_usuarios.Find(cl_user.id_usr)
            Dim nivel = oUsuarioActual.t_tipo_nivel_usuario
            If nivel.codigo_nivel_usuario = "SYS_USER" Then
                Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa, "SYS_USER")

                Me.cmb_tipo_nivel.Enabled = False
                Me.div_nivel_usuario.Visible = False
                If oUsuario.t_tipo_nivel_usuario.codigo_nivel_usuario = "SYS_ADMIN" Or oUsuario.t_tipo_nivel_usuario.codigo_nivel_usuario = "SYS_P_ADMIN" Then
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


        End Using
    End Sub
    Protected Overloads Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        AddHandler Uploader2.FileUploaded, AddressOf Uploader_FileUploaded
    End Sub

    Private Sub Uploader_FileUploaded(ByVal sender As Object, ByVal args As UploaderEventArgs)
        'Dim uploader As Uploader = DirectCast(sender, Uploader)
        Dim sFileDir As String = Server.MapPath("~") & "\Temp\"
        Try
            Dim Random As New Random()
            Dim Aleatorio As Double = Random.Next(1, 999999)
            args.CopyTo(sFileDir & "SIME_IMG_" & Aleatorio & args.FileName.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_"))
            Me.lblarchivo.Text = "SIME_IMG_" & Aleatorio & args.FileName.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_")
            Me.imgUser.ImageUrl = "~/Temp/" + Me.lblarchivo.Text
        Catch ex As Exception
            Me.Image1.ImageUrl = "../imagenes/s_warn.png"
            Me.lblarchivo.Text = "Error.."
        End Try
        Me.Panel1.Visible = True
    End Sub

    Sub DelFile()
        Dim sFileName As String = System.IO.Path.GetFileName(Me.lblarchivo.Text)
        Dim sFileDir As String = Server.MapPath("~") & "\Temp\"
        Dim file_info As New IO.FileInfo(sFileDir + sFileName)
        If (file_info.Exists) Then
            file_info.Delete()
            'Me.imgUser.ImageUrl = "~/Imagenes/Logo_User.png"
        End If
        Me.lblarchivo.Text = ""
        Me.Panel1.Visible = False
    End Sub

    Sub CopyFile()

        Dim sFileName As String = System.IO.Path.GetFileName(Me.lblarchivo.Text)
        Dim sFileDirTemp As String = Server.MapPath("~") & "\Temp\"
        Dim file_info As New IO.FileInfo(sFileDirTemp + sFileName)

        Try

            'Dim sFileNameNW As String = sFileName.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_")

            Using dbEntities As New dbRMS_JIEntities

                Dim id_programa = Convert.ToInt32(Session("E_IDprograma"))
                ' sFileDirTemp = sFileDirTemp & sFileName
                Dim user_route = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).url_user_images
                user_route = user_route.Substring(0, user_route.Trim.Length - 1)
                Dim idx1 As Integer = user_route.LastIndexOf("/") + 1
                Dim account As String = user_route.Substring(idx1, user_route.Length - idx1)

                cl_files.SaveFileCloud(account, sFileDirTemp, sFileName)
                ' file_info.CopyTo(sFileDir & sFileName.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_"))
                lbl_imgOriginal.Text = sFileName

            End Using

        Catch ex As Exception
        End Try
        DelFile()
        Me.Panel1.Visible = False
    End Sub

    Protected Sub imgEliminaImg_Click(sender As Object, e As ImageClickEventArgs) Handles imgEliminaImg.Click
        DelFile()
    End Sub

    Protected Sub cmb_tipo_usuario_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_tipo_usuario.SelectedIndexChanged
        If cmb_tipo_usuario.SelectedValue = 2 Then
            Me.divEjecutor.Visible = True
        Else
            Me.divEjecutor.Visible = False
        End If

    End Sub

End Class
