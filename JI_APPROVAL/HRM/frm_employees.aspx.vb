Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Data.SqlClient

Public Class frm_employee
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADMIN_EMPLOYEE"
    Dim db As New dbRMS_JIEntities
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub fillGrid()

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

        Me.grd_cate_employee.DataSource = db.vw_rh_employee.Where(Function(p) p.id_programa = idPrograma And p.nombre_usuario.Contains(Me.txt_doc.Text) And p.id_rh_office > 0).OrderBy(Function(p) p.apellidos_usuario).ThenBy(Function(o) o.nombre_usuario).ToList()
        Me.grd_cate_employee.DataBind()

        Me.grd_users.DataSource = db.vw_rh_employee.Where(Function(p) p.id_programa = idPrograma And p.id_rh_employee = 0).OrderBy(Function(p) p.apellidos_usuario).ThenBy(Function(o) o.nombre_usuario).ToList()
        Me.grd_users.DataBind()

        'Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate_employee.Items.Count & " ]"
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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate_employee)
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_users)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            fillGrid()
        End If
    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Protected Sub grd_cate_employee_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate_employee.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_employee_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate_employee.PageSizeChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_employee_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate_employee.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)
            hlnkEdit.NavigateUrl = "frm_employee_profile?Id=" & DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString()
            'hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")

            'Dim hlnkRoles As HyperLink = New HyperLink
            'hlnkRoles = CType(e.Item.FindControl("col_hlk_roles"), HyperLink)
            'hlnkRoles.NavigateUrl = "frm_usuarioRoles?Id=" & DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString()
            ''hlnkRoles.ToolTip = controles.iconosGrid("col_hlk_roles")

            'Dim hlnkRegiones As HyperLink = New HyperLink
            'hlnkRegiones = CType(e.Item.FindControl("col_hlk_regiones"), HyperLink)
            'hlnkRegiones.NavigateUrl = "frm_usuarioRegiones?Id=" & DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString()
            ''hlnkRegiones.ToolTip = controles.iconosGrid("col_hlk_regiones")

            'Dim hlnkProyectos As HyperLink = New HyperLink
            'hlnkProyectos = CType(e.Item.FindControl("col_hlk_ficha_proyecto"), HyperLink)
            'hlnkProyectos.NavigateUrl = "frm_usuarioProyectos?Id=" & DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString()
            ''hlnkProyectos.ToolTip = controles.iconosGrid("col_hlk_ficha_proyecto")

            'Dim hlnkModulos As HyperLink = New HyperLink
            'hlnkModulos = CType(e.Item.FindControl("col_hlk_usrAccMod"), HyperLink)
            'hlnkModulos.NavigateUrl = "frm_acceso_usr_mod?Id=" & DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString()
            ''hlnkModulos.ToolTip = controles.iconosGrid("col_hlk_usrAccMod")

            'Dim hlnkProgramas As HyperLink = New HyperLink
            'hlnkProgramas = CType(e.Item.FindControl("col_hlk_programa"), HyperLink)
            'hlnkProgramas.NavigateUrl = "frm_usuarioProgramas?Id=" & DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString()
            ''hlnkProgramas.ToolTip = controles.iconosGrid("col_hlk_programa")

            'Dim hlnkDelete As LinkButton = New LinkButton
            'hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            'hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString())
            'hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString())
            ''hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

            Dim hlnkCompleto As HyperLink = New HyperLink
            hlnkCompleto = CType(e.Item.FindControl("col_hlk_Completo"), HyperLink)

            If DataBinder.Eval(e.Item.DataItem, "id_rh_employee").ToString().Equals("0") Then
                hlnkCompleto.ToolTip = "No posee registro de empleado"
            Else
                hlnkCompleto.ToolTip = "Registro de empleado completo"
                hlnkCompleto.ImageUrl = "../Imagenes/iconos/accept.png"
            End If

        End If
    End Sub
    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/HRM/frm_employee_profile")
    End Sub

    Private Sub grd_users_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_users.ItemDataBound


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("col_hlk_add"), HyperLink)
            hlnkEdit.NavigateUrl = "frm_employee_profile?Id=" & DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString()
            'hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")

            Dim hlnkCompleto As HyperLink = New HyperLink
            hlnkCompleto = CType(e.Item.FindControl("col_hlk_Completo"), HyperLink)

            If DataBinder.Eval(e.Item.DataItem, "id_rh_employee").ToString().Equals("0") Then
                hlnkCompleto.ToolTip = "No posee registro de empleado"
            Else
                hlnkCompleto.ToolTip = "Registro de empleado realizado"
                hlnkCompleto.ImageUrl = "../Imagenes/iconos/accept.png"
            End If

        End If

    End Sub

    'Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
    '    Using db As New ly_SIME.dbRMS_JIEntities
    '        Try
    '            db.Database.ExecuteSqlCommand("DELETE FROM t_usuarios WHERE id_usuario = " + Me.identity.Text)
    '            Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
    '        Catch ex As SqlException
    '            Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
    '            Me.MsgGuardar.TituMensaje = "Error al eliminar"
    '        End Try
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    '    End Using
    'End Sub

    'Protected Sub Eliminar_Click(sender As Object, e As EventArgs)
    '    Dim eliminar = CType(sender, LinkButton)
    '    Me.identity.Text = eliminar.Attributes.Item("data-identity").ToString()
    '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    'End Sub


End Class