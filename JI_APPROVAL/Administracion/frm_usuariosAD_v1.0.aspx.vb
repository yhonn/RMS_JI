Imports System.Security.Cryptography
Imports CuteWebUI
Imports System.Data.SqlClient
Imports ly_SIME
Imports ly_APPROVAL

Public Class frm_usuariosAD_v10
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim clListado As New ly_SIME.CORE.cls_listados
    Dim frmCODE As String = "AP_NEW_USU"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_files As New ly_SIME.CORE.cls_files

    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Usuarios\"
    Dim sFileDirTemp As String = Server.MapPath("~") & "\Temp\"
    Dim sFileDirUbicacion As String = "~/FileUploads/Usuarios/"
    Dim CurrencyDecimalSeparator As String = ","
    Dim caracterizacion As Integer
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

        If Not Me.IsPostBack Then
            FillData()
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

            Using dbEntities As New dbRMS_HNEntities
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
                            New SqlParameter("@id_job_title", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_job_title.SelectedValue)},
                            New SqlParameter("@id_tipo_nivel_usuario", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_tipo_nivel.SelectedValue)},
                            New SqlParameter("@id_tipo_usuario", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_tipo_usuario.SelectedValue)},
                            New SqlParameter("@imagen", SqlDbType.VarChar) With {.Value = If(String.IsNullOrWhiteSpace(Me.lblarchivo.Text), "Logo_User.png", Me.lblarchivo.Text)},
                            New SqlParameter("@fecha_contrato", SqlDbType.Date) With {.Value = Me.dt_fecha_contrato.SelectedDate},
                            New SqlParameter("@id_usuario_upd", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.Session("E_IdUser").ToString())},
                            New SqlParameter("@id_estado_usr", SqlDbType.Int) With {.Value = 1},
                            New SqlParameter("@id_usuario_padre", SqlDbType.Int) With {.Value = If(Not Me.chk_SupervisorNA.Checked, Convert.ToInt32(Me.cmb_supervisor.SelectedValue), 0)},
                            New SqlParameter("@upddate", SqlDbType.DateTime) With {.Value = Date.UtcNow},
                            New SqlParameter("@id_idioma", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_idioma.SelectedValue)}
                        }

                query = "INSERT INTO t_usuarios (nombre_usuario, apellidos_usuario, email_usuario, usuario, clave, datecreated, id_job_title, id_tipo_nivel_usuario, id_tipo_usuario, " &
                    "imagen, fecha_contrato, id_usuario_upd, id_estado_usr, id_usuario_padre, upddate) VALUES (@nombre_usuario, @apellidos_usuario, @email_usuario, @usuario, " &
                    "HashBytes('SHA', @clave), @datecreated, @id_job_title, @id_tipo_nivel_usuario, @id_tipo_usuario, @imagen, @fecha_contrato, @id_usuario_upd, @id_estado_usr, @id_usuario_padre, @upddate) " &
                    " SELECT @@IDENTITY "
                dbEntities.Database.ExecuteSqlCommand(query, parameters)

                Dim idUsuario = dbEntities.t_usuarios.Max(Function(p) p.id_usuario)
                Dim parameters2() As SqlParameter = New SqlParameter() _
                        {
                            New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = idUsuario},
                            New SqlParameter("@id_programa", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())},
                            New SqlParameter("@id_idioma", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_idioma.SelectedValue)}
                        }
                Dim query2 = "INSERT INTO t_usuario_idioma values (@id_idioma,@id_programa,@id_usuario)"
                dbEntities.Database.ExecuteSqlCommand(query2, parameters2)

                If cmb_tipo_usuario.SelectedValue = 2 Then
                    query = String.Concat("INSERT INTO t_ejecutor_usuario VALUES (", Me.cmb_ejecutor.SelectedValue, ", ", idUsuario, ")")
                    dbEntities.Database.ExecuteSqlCommand(query)
                End If

                Dim parameters3() As SqlParameter = New SqlParameter() _
                        {
                            New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = idUsuario},
                            New SqlParameter("@id_programa", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())},
                            New SqlParameter("@usuario_completo", SqlDbType.Bit) With {.Value = 0},
                            New SqlParameter("@id_idioma", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_idioma.SelectedValue)},
                            New SqlParameter("@id_job_title", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_job_title.SelectedValue)}
                        }
                Dim query3 = "INSERT INTO t_usuario_programa (id_usuario, id_programa, usuario_completo, id_idioma, id_job_title) VALUES " &
                    "(@id_usuario, @id_programa, @usuario_completo, @id_idioma, @id_job_title)"
                dbEntities.Database.ExecuteSqlCommand(query3, parameters3)

                If Not Me.imgUser.ImageUrl.Contains(lbl_imgOriginal.Text) Then
                    CopyFile()
                End If


                If chk_Notify.Checked Then
                    '*********************************OPEN****************************************
                    Dim objEmail As New SIMEly.cls_notification(Me.Session("E_IDPrograma"), 7, cl_user.regionalizacionCulture, cl_user.idSys)
                    If (objEmail.Emailing_USER_UPD(idUsuario, Me.txt_clave_usuario.Text)) Then
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

    Sub FillData()
        Using dbEntities As New dbRMS_HNEntities
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Me.cmb_supervisor.DataSourceID = ""
            Me.cmb_supervisor.DataSource = clListado.get_t_usuarios()
            Me.cmb_supervisor.DataTextField = "nombre_usuario"
            Me.cmb_supervisor.DataValueField = "id_usuario"
            Me.cmb_supervisor.DataBind()

            'Me.cmb_tipo_nivel.DataSourceID = ""
            'Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa)
            'Me.cmb_tipo_nivel.DataTextField = "codigo_nivel_usuario"
            'Me.cmb_tipo_nivel.DataValueField = "id_tipo_nivel_usuario"
            'Me.cmb_tipo_nivel.DataBind()

            Me.cmb_tipo_usuario.DataSourceID = ""
            Me.cmb_tipo_usuario.DataSource = clListado.get_t_tipo_usuario()
            Me.cmb_tipo_usuario.DataTextField = "nombre_tipo_usuario"
            Me.cmb_tipo_usuario.DataValueField = "id_tipo_usuario"
            Me.cmb_tipo_usuario.DataBind()

            lbl_imgOriginal.Text = "Logo_User.png"

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


            Me.cmb_idioma.DataSource = dbEntities.vw_t_programa_idiomas.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_idioma.DataTextField = "descripcion_idioma"
            Me.cmb_idioma.DataValueField = "id_idioma"
            Me.cmb_idioma.DataBind()

            Me.cmb_job_title.DataSource = clListado.get_t_job_title(id_programa)
            Me.cmb_job_title.DataTextField = "job"
            Me.cmb_job_title.DataValueField = "id_job_title"
            Me.cmb_job_title.DataBind()


            Dim oUsuario = dbEntities.t_usuarios.Find(cl_user.id_usr)
            Dim nivel = oUsuario.t_tipo_nivel_usuario
            Me.cmb_tipo_nivel.DataSourceID = ""

            If nivel.codigo_nivel_usuario = "SYS_USER" Then

                Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa, "SYS_USER")
                Me.cmb_tipo_nivel.Enabled = False
                Me.div_nivel_usuario.Visible = False

            Else

                If nivel.codigo_nivel_usuario = "SYS_P_ADM" Then
                    Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa, "SYS_USER,SYS_P_ADM")
                Else
                    Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa, "SYS_USER,SYS_P_ADM,SYS_ADMIN")
                End If

                Me.cmb_tipo_nivel.DataTextField = "codigo_nivel_usuario"
                Me.cmb_tipo_nivel.DataValueField = "id_tipo_nivel_usuario"
                Me.cmb_tipo_nivel.DataBind()
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

            Using dbEntities As New dbRMS_HNEntities

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

    Protected Sub txt_usuarioSistema_TextChanged(sender As Object, e As EventArgs) Handles txt_usuarioSistema.TextChanged
        Using db As New dbRMS_HNEntities
            Dim existe = db.t_usuarios.Where(Function(p) p.usuario = Me.txt_usuarioSistema.Text).Count()
            If existe > 0 Then
                Me.lblErrorUsuario.Visible = True
                Me.lblErrorUsuario.Text = "**El Usuario ya existe, cambie el nombre de usuario"
            Else
                Me.lblErrorUsuario.Visible = False
            End If
        End Using
    End Sub

    Protected Sub cmb_tipo_usuario_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_tipo_usuario.SelectedIndexChanged
        If cmb_tipo_usuario.SelectedValue = 2 Then
            Me.divEjecutor.Visible = True
        Else
            Me.divEjecutor.Visible = False
        End If

    End Sub
End Class