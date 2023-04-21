Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Data.SqlClient

Public Class frm_usuarioRoles
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADMIN_ROLU"
    Dim db As New dbRMS_JIEntities
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_error As New ly_SIME.CORE.ErrorHandler

    Sub fillGrid()
        Dim id_usuario = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
        'Anterior origen de datos de roles
        'Me.grd_cate.DataSource = db.vw_t_users_rol.Where(Function(p) p.id_usuario = id_usuario And p.id_programa = Me.cmb_programa.SelectedValue And p.id_rol > 0).ToList()

        'Nuevo origen de datos de roles'
        Me.grd_cate.DataSource = db.vw_t_user_roles_programa.Where(Function(p) p.id_usuario = id_usuario And p.id_programa = Me.cmb_programa.SelectedValue And p.id_rol > 0).ToList()

        Dim usuario = db.t_usuarios.Find(id_usuario)
        Me.lbl_rol_usuario.Text = " - " + usuario.nombre_usuario + " " + usuario.apellidos_usuario
        'Me.SqlDataSource2.SelectCommand = "SELECT * FROM t_programas WHERE nombre_programa LIKE '%" & Me.txt_doc.Text.Trim.Replace("'", "") & "%'"

        Me.grd_cate.DataBind()

        'Me.cmb_id_rol.DataSource = clListados.get_t_rol(Me.cmb_programa.SelectedValue)
        'Me.cmb_id_rol.DataTextField = "rol_name"
        'Me.cmb_id_rol.DataValueField = "id_rol"
        'Me.cmb_id_rol.DataBind()
        'Me.cmb_id_rol.Enabled = True

        'Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
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
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls

                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            Me.lbl_id_usuario.Text = Me.Request.QueryString("Id").ToString
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Me.cmb_programa.DataSourceID = ""
            Me.cmb_programa.DataSource = clListados.get_t_programas()
            Me.cmb_programa.DataTextField = "nombre_programa"
            Me.cmb_programa.DataValueField = "id_programa"
            Me.cmb_programa.DataBind()
            Me.cmb_programa.SelectedValue = idPrograma
            fillGrid()
            If cl_user.codigo_nivel_usuario = "SYS_ADMIN" Or cl_user.codigo_nivel_usuario = "SYS_P_ADM" Then
                Me.cmb_programa.Enabled = True
                If cl_user.codigo_nivel_usuario = "SYS_P_ADM" Then
                    Me.cmb_programa.DataSource = clListados.get_t_programas_usuario(cl_user.id_usr)
                    Me.cmb_programa.DataTextField = "nombre_programa"
                    Me.cmb_programa.DataValueField = "id_programa"
                    Me.cmb_programa.DataBind()
                End If
            End If
        End If
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)

            'Anterior Atributos Roles
            'hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_usuario_programa").ToString())
            'hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_usuario_programa").ToString())

            'Nuevo Tributo roles'
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_usuario_roles").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_usuario_roles").ToString())

            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")
        End If
    End Sub
    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        If Me.cmb_id_rol.SelectedValue IsNot Nothing Then
            Using dbEntities As New dbRMS_JIEntities
                Dim idUsuario = Convert.ToInt32(Me.lbl_id_usuario.Text)
                Dim oUsrPrograma = dbEntities.t_usuario_programa.FirstOrDefault(Function(p) p.id_programa = Me.cmb_programa.SelectedValue And p.id_usuario = idUsuario)
                oUsrPrograma.id_rol = Me.cmb_id_rol.SelectedValue
                Dim oUsr = dbEntities.t_usuarios.Find(idUsuario)

                If oUsr.t_usuario_subregion.Where(Function(p) p.t_subregiones.t_regiones.id_programa = Me.cmb_programa.SelectedValue).Count() > 0 Then
                    oUsrPrograma.usuario_completo = True
                End If
                dbEntities.Entry(oUsrPrograma).State = Entity.EntityState.Modified


                'Agrega un nuevo rol a el usuario con su respectivo programa'
                If dbEntities.t_usuario_roles.Where(Function(ur) ur.id_rol = Me.cmb_id_rol.SelectedValue And ur.id_usuario = idUsuario And ur.id_programa = Me.cmb_programa.SelectedValue).Count() = 0 Then
                    Dim oUsrRoles = New t_usuario_roles
                    oUsrRoles.id_rol = Me.cmb_id_rol.SelectedValue
                    oUsrRoles.id_usuario = idUsuario
                    oUsrRoles.id_rold = 0
                    oUsrRoles.id_programa = Me.cmb_programa.SelectedValue
                    oUsrRoles.fecha_crea = Date.UtcNow
                    oUsrRoles.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    dbEntities.t_usuario_roles.Add(oUsrRoles)
                End If
                'Fin agregar rol'

                'Dim oRolUsr As New t_rol_usr

                'oRolUsr.id_usuario = Convert.ToInt32(Me.lbl_id_usuario.Text)
                'oRolUsr.id_rol = Convert.ToInt32(Me.cmb_id_rol.SelectedValue)
                'oRolUsr.id_rold = 1

                'dbEntities.t_rol_usr.Add(oRolUsr)
                dbEntities.SaveChanges()
            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = String.Concat("~/Administracion/frm_usuarioRoles?id=", Me.lbl_id_usuario.Text)
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If
        fillGrid()
    End Sub


    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using db As New ly_SIME.dbRMS_JIEntities
            Try
                'Anterior Eliminar rol de los usuarios'
                db.Database.ExecuteSqlCommand("UPDATE t_usuario_programa SET id_rol = NULL, usuario_completo = 0 WHERE id_usuario_programa = " + Me.identity.Text)

                'Nuevo Eliminar rol de los usuarios'
                db.Database.ExecuteSqlCommand("DELETE t_usuario_roles where id_usuario_roles = " + Me.identity.Text)

                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            Me.MsgGuardar.Redireccion = String.Concat("~/Administracion/frm_usuarioRoles?id=", Me.lbl_id_usuario.Text)
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    Protected Sub Eliminar_Click(sender As Object, e As EventArgs)
        Dim eliminar = CType(sender, LinkButton)
        Me.identity.Text = eliminar.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub cmb_programa_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_programa.SelectedIndexChanged
        fillGrid()
        Me.cmb_id_rol.DataBind()
    End Sub
End Class