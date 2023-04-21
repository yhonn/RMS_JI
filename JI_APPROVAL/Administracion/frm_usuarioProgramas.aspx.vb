Imports System.Data.SqlClient
Imports ly_SIME
Imports Telerik.Web.UI

Public Class frm_usuarioProgramas
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_USER_PROG"
    Dim db As New dbRMS_JIEntities
    Dim clListado As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_error As New ly_SIME.CORE.ErrorHandler

    Sub fillGrid()
        Dim id_usuario = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
        Me.grd_cate.DataSource = db.vw_t_usuario_programa.Where(Function(p) p.id_usuario = id_usuario).ToList()
        Dim usuario = db.t_usuarios.Find(id_usuario)
        Me.lbl_usuario.Text = " - " & usuario.nombre_usuario & " " & usuario.apellidos_usuario
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
            fillGrid()

            If cl_user.codigo_nivel_usuario = "SYS_ADMIN" Or cl_user.codigo_nivel_usuario = "SYS_P_ADM" Then
                Me.cmb_id_programa.Enabled = True
                If cl_user.codigo_nivel_usuario = "SYS_P_ADM" Then
                    Me.cmb_id_programa.DataSourceID = ""
                    Me.cmb_id_programa.DataSource = clListado.get_t_programas_usuario(cl_user.id_usr)
                    Me.cmb_id_programa.DataBind()
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
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_usuario_programa").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_usuario_programa").ToString())
        End If
    End Sub
    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        If Me.cmb_id_programa.SelectedValue IsNot Nothing Then
            Using dbEntities As New dbRMS_JIEntities
                Dim id = Convert.ToInt32(Me.lbl_id_usuario.Text)
                Dim oUsrProgram As New t_usuario_programa

                oUsrProgram.id_usuario = id
                oUsrProgram.id_programa = Convert.ToInt32(Me.cmb_id_programa.SelectedValue)
                oUsrProgram.usuario_completo = False
                oUsrProgram.id_idioma = Convert.ToInt32(Me.cmb_idioma.SelectedValue)
                oUsrProgram.id_job_title = Convert.ToInt32(Me.cmb_job_title.SelectedValue)
                dbEntities.t_usuario_programa.Add(oUsrProgram)
                dbEntities.SaveChanges()
            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = String.Concat("~/Administracion/frm_usuarioProgramas?id=", Me.lbl_id_usuario.Text)
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If
        fillGrid()
    End Sub


    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using db As New ly_SIME.dbRMS_JIEntities
            Try
                db.Database.ExecuteSqlCommand("DELETE FROM t_usuario_programa WHERE id_usuario_programa = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    Protected Sub Eliminar_Click(sender As Object, e As EventArgs)
        Dim eliminar = CType(sender, LinkButton)
        Me.identity.Text = eliminar.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub cmb_id_programa_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_id_programa.SelectedIndexChanged
        Me.cmb_idioma.DataSource = clListado.get_vw_t_programa_idiomas(Me.cmb_id_programa.SelectedValue)
        Me.cmb_idioma.DataTextField = "descripcion_idioma"
        Me.cmb_idioma.DataValueField = "id_idioma"
        Me.cmb_idioma.DataBind()
        Me.cmb_idioma.Enabled = True

        Me.cmb_job_title.DataSource = clListado.get_t_job_title(Me.cmb_id_programa.SelectedValue)
        Me.cmb_job_title.DataTextField = "job"
        Me.cmb_job_title.DataValueField = "id_job_title"
        Me.cmb_job_title.DataBind()
        Me.cmb_job_title.Enabled = True

    End Sub
End Class