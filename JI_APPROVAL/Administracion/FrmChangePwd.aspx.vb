Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Drawing
Imports Telerik.Web.UI
Imports ly_SIME

Public Class FrmChangePwd
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_files As New ly_SIME.CORE.cls_files
    Dim frmCODE As String = "AP_EDIT_PASS"

    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Usuarios\"
    Dim sFileDirTemp As String = Server.MapPath("~") & "\Temp\"
    Dim sFileDirUbicacion As String = "~/FileUploads/Usuarios/"
    Dim CurrencyDecimalSeparator As String = ","
    Dim caracterizacion As Integer
    Dim extension, File As String

    Sub cargadatos()
        Dim id_usuario = Convert.ToInt32(Me.Session("E_IdUser"))
        Using dbEntities As New dbRMS_JIEntities
            Dim oUsuario = dbEntities.t_usuarios.Find(id_usuario)
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Me.txtnomb1.Text = oUsuario.nombre_usuario
            Me.txtapell.Text = oUsuario.apellidos_usuario
            Me.txt_email.Text = oUsuario.email_usuario
            Me.lbl_oldFile.Value = oUsuario.imagen
            Me.txtUsu.Text = oUsuario.usuario
            Dim user_uri As Uri
            Dim user_route = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).url_user_images
            Uri.TryCreate(user_route & oUsuario.imagen, UriKind.Absolute, user_uri)
            Me.imgUser.ImageUrl = user_uri.AbsoluteUri.ToString()


            Me.cmb_idioma.DataSource = dbEntities.vw_t_programa_idiomas.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_idioma.DataTextField = "descripcion_idioma"
            Me.cmb_idioma.DataValueField = "id_idioma"
            Me.cmb_idioma.DataBind()
            Me.cmb_idioma.SelectedValue = oUsuario.t_usuario_programa.FirstOrDefault(Function(p) p.id_programa = id_programa).id_idioma

        End Using

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login")
        End Try
        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls

                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If
        Dim config As FileAsyncUploadConfiguration = Me.uploadFile.CreateDefaultUploadConfiguration(Of FileAsyncUploadConfiguration)()
        config.UserID = 1
        Me.uploadFile.UploadConfiguration = config
        If Not IsPostBack Then
            cargadatos()
        End If
    End Sub

    Protected Sub btnguardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click
        Dim err = False
        If Me.txtpass.Text = "" And Me.chk_cambiar.Checked = True Then
            err = True
            Me.lblerr_pass.Visible = True
        End If

        If Me.txtpass.Text <> Me.txtpassConfirm.Text And Me.chk_cambiar.Checked = True Then
            err = True
            Me.lblerr_passconf.Visible = True
        End If
        If err = True Then
            Me.lblerrorG.Visible = True
        Else
            Me.lblerr_pass.Visible = False
            Me.lblerr_passconf.Visible = False
            Me.lblerrorG.Visible = False

            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim fileName = Me.lbl_oldFile.Value

            If lbl_hasFiles.Value = "true" Then
                fileName = Me.lbl_archivo_uploaded.Value
            End If

            Using dbEntities As New dbRMS_JIEntities
                Dim oUsuario As New t_usuarios
                Dim query = ""
                Dim query2 = ""
                Dim parameters() As SqlParameter = New SqlParameter() _
                        {
                            New SqlParameter("@clave", SqlDbType.VarChar) With {.Value = Me.txtpass.Text},
                            New SqlParameter("@id_usuario_upd", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.Session("E_IdUser").ToString())},
                            New SqlParameter("@upddate", SqlDbType.DateTime) With {.Value = Date.UtcNow},
                            New SqlParameter("@imagen", SqlDbType.VarChar) With {.Value = fileName},
                            New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.Session("E_IdUser").ToString())}
                        }
                'New SqlParameter("@imagen", SqlDbType.VarChar) With {.Value = If(Me.lblarchivo.Text = Me.imgUser.ImageUrl, Me.lblarchivo.Text, sFileDirUbicacion + Me.lblarchivo.Text)},
                Dim parameters2() As SqlParameter = New SqlParameter() _
                        {
                            New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.Session("E_IdUser").ToString())},
                            New SqlParameter("@id_programa", SqlDbType.Int) With {.Value = id_programa},
                            New SqlParameter("@id_idioma", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_idioma.SelectedValue)}
                        }

                If Me.chk_cambiar.Checked = True Then
                    query = "UPDATE t_usuarios SET clave = HashBytes('SHA', @clave), id_usuario_upd = @id_usuario_upd, upddate = @upddate, imagen = @imagen WHERE id_usuario = @id_usuario"
                    dbEntities.Database.ExecuteSqlCommand(query, parameters)
                    query2 = "UPDATE t_usuario_programa SET id_idioma = @id_idioma WHERE id_usuario = @id_usuario and id_programa = @id_programa"
                    dbEntities.Database.ExecuteSqlCommand(query2, parameters2)
                Else
                    query = "UPDATE t_usuarios SET imagen = @imagen WHERE id_usuario = @id_usuario"
                    dbEntities.Database.ExecuteSqlCommand(query, parameters)

                    query2 = "UPDATE t_usuario_programa SET id_idioma = @id_idioma WHERE id_usuario = @id_usuario and id_programa = @id_programa"
                    dbEntities.Database.ExecuteSqlCommand(query2, parameters2)

                End If
                cl_user.id_idioma = Me.cmb_idioma.SelectedValue
                cl_user.controles_otros = dbEntities.vw_control_otro_idioma.Where(Function(p) p.id_idioma = cl_user.id_idioma).ToList()

                If lbl_hasFiles.Value = "true" Then

                    Dim user_route = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).url_user_images

                    Dim user_path = user_route
                    user_path = user_path.Substring(0, user_path.Trim.Length - 1)
                    Dim idx1 As Integer = user_path.LastIndexOf("/") + 1
                    Dim account As String = user_path.Substring(idx1, user_path.Length - idx1)

                    cl_files.SaveFileCloud(account, Server.MapPath("~") & "\Temp\", Me.lbl_archivo_uploaded.Value)
                    Dim user_uri As Uri
                    Uri.TryCreate(user_route & Me.lbl_archivo_uploaded.Value, UriKind.Absolute, user_uri)
                    Me.Session("E_Imagen") = user_uri.AbsoluteUri.ToString()
                End If



            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Administracion/FrmChangePwd"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If

    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Default"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub chk_cambiar_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_cambiar.CheckedChanged
        If Me.chk_cambiar.Checked Then
            Me.txtpass.Enabled = True
            Me.txtpassConfirm.Enabled = True
        Else
            Me.txtpass.Enabled = False
            Me.txtpassConfirm.Enabled = False
        End If
    End Sub

End Class
