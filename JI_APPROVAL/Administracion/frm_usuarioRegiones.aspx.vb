Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Data.SqlClient

Public Class frm_usuarioRegiones
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_ADMIN_USUR"
    Dim db As New dbRMS_JIEntities
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub fillGrid()
        Me.grd_cate.DataSourceID = ""
        Dim id_usuario = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
        Me.grd_cate.DataSource = db.vw_t_usuarios_subregiones.Where(Function(p) p.id_usuario = id_usuario And p.id_programa = Me.cmb_programa.SelectedValue).ToList()
        Dim usuario = db.t_usuarios.Find(id_usuario)
        Me.lblt_subtitulo_pantalla.Text = "Sub Regiones Usuario - " + usuario.nombre_usuario + " " + usuario.apellidos_usuario
        'Me.SqlDataSource2.SelectCommand = "SELECT * FROM t_programas WHERE nombre_programa LIKE '%" & Me.txt_doc.Text.Trim.Replace("'", "") & "%'"

        Me.grd_cate.DataBind()

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
            Me.cmb_region.DataSourceID = ""
            Me.cmb_region.DataSource = clListados.get_t_regiones(Convert.ToInt32(Me.cmb_programa.SelectedValue))
            Me.cmb_region.DataTextField = "nombre_region"
            Me.cmb_region.DataValueField = "id_region"
            Me.cmb_region.DataBind()
            fillGrid()

            If cl_user.codigo_nivel_usuario = "SYS_ADMIN" Or cl_user.codigo_nivel_usuario = "SYS_P_ADM" Then
                Me.cmb_programa.Enabled = True
                If cl_user.codigo_nivel_usuario = "SYS_P_ADM" Then
                    Me.cmb_programa.DataSource = clListados.get_t_programas_usuario(cl_user.id_usr)
                    Me.cmb_programa.DataBind()
                End If
            End If
        End If
    End Sub

    'Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
    '    fillGrid()
    'End Sub

    Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand
        Dim id_usuario = Convert.ToInt32(TryCast(e.Item, GridDataItem).GetDataKeyValue("id_subregion").ToString())
        Dim usuario = db.t_rol_usr.Find(id_usuario)
        db.t_rol_usr.Remove(usuario)
        db.SaveChanges()
        MsgBox("Eliminado: " & id_usuario)
        Me.Response.Redirect(String.Concat("~/Administracion/frm_regionesRoles?id=", Me.lbl_id_usuario.Text))
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
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_usuario_subregion").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_usuario_subregion").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")
        End If
    End Sub
    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        If Me.cmb_id_subregion.SelectedValue IsNot Nothing Then
            Using dbEntities As New dbRMS_JIEntities
                Dim oRegionUsr As New t_usuario_subregion
                Dim idUsuario = Convert.ToInt32(Me.lbl_id_usuario.Text)
                oRegionUsr.id_usuario = idUsuario
                oRegionUsr.id_subregion = Convert.ToInt32(Me.cmb_id_subregion.SelectedValue)

                Dim oUsrPrograma = dbEntities.t_usuario_programa.FirstOrDefault(Function(p) p.id_programa = Me.cmb_programa.SelectedValue And p.id_usuario = idUsuario)
                If oUsrPrograma.id_rol.HasValue Then
                    oUsrPrograma.usuario_completo = True
                Else
                    oUsrPrograma.usuario_completo = False
                End If
                dbEntities.Entry(oUsrPrograma).State = Entity.EntityState.Modified

                dbEntities.t_usuario_subregion.Add(oRegionUsr)

                dbEntities.SaveChanges()
            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = String.Concat("~/Administracion/frm_usuarioregiones?id=", Me.lbl_id_usuario.Text)
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If
    End Sub

    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using db As New ly_SIME.dbRMS_JIEntities
            Try
                db.Database.ExecuteSqlCommand("DELETE FROM t_usuario_subregion WHERE id_usuario_subregion = " + Me.identity.Text)
                Dim idUsuario = Convert.ToInt32(Me.lbl_id_usuario.Text)
                Dim oUsr = db.t_usuarios.Find(idUsuario)
                Dim oUsrPrograma = db.t_usuario_programa.FirstOrDefault(Function(p) p.id_programa = Me.cmb_programa.SelectedValue And p.id_usuario = idUsuario)
                If oUsr.t_usuario_subregion.Where(Function(p) p.t_subregiones.t_regiones.id_programa = Me.cmb_programa.SelectedValue).Count() > 0 Then
                    oUsrPrograma.usuario_completo = True
                Else
                    oUsrPrograma.usuario_completo = False
                End If
                db.Entry(oUsrPrograma).State = Entity.EntityState.Modified
                db.SaveChanges()
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    Protected Sub Eliminar_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub cmb_programa_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_programa.SelectedIndexChanged
        loadListas(Me.cmb_programa.SelectedValue)
        fillGrid()
    End Sub

    Protected Sub cmb_region_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_region.SelectedIndexChanged

    End Sub

    Sub loadListas(ByVal idPrograma As Integer)
        Me.cmb_region.DataSourceID = ""
        Me.cmb_region.DataSource = clListados.get_t_regiones(Convert.ToInt32(Me.cmb_programa.SelectedValue))
        Me.cmb_region.DataTextField = "nombre_region"
        Me.cmb_region.DataValueField = "id_region"
        Me.cmb_region.DataBind()
    End Sub
End Class