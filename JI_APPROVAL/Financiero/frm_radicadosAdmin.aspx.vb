Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Data.SqlClient
Public Class frm_radicadosAdmin
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_ADMIN_RADICADOS"
    Dim db As New dbRMS_JIEntities
    Dim controles As New ly_SIME.CORE.cls_controles

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
            fillGrid(True)
            loadPermisos()
        End If
    End Sub
    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid(True)
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid(False)
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid(False)
    End Sub


    Private Sub grd_cate_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource
        fillGrid(False)
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then



            Dim visible As New CheckBox
            visible = CType(e.Item.FindControl("chkActivo"), CheckBox)
            visible.Attributes.Add("id_usuario", DataBinder.Eval(e.Item.DataItem, "id_usuario").ToString())

            If DataBinder.Eval(e.Item.DataItem, "registro_radicados_post_cierre").ToString = "True" Then
                visible.ToolTip = controles.iconosGrid("col_hlk_activo")
                visible.Checked = True
            Else
                visible.Checked = False
                visible.ToolTip = controles.iconosGrid("col_hlk_inactivo")
            End If

        End If
    End Sub

    Protected Sub chkVisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Activo As Boolean = False

        Dim chkvisible As CheckBox = CType(sender, CheckBox)
        If chkvisible.Checked Then
            Activo = True
        End If
        Using dbentities As New dbRMS_JIEntities
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim id_usuario = Convert.ToInt32(chkvisible.Attributes("id_usuario").ToString())
            Dim usuario = dbentities.t_usuarios_permisos.Where(Function(p) p.id_programa = idPrograma And p.id_usuario = id_usuario).FirstOrDefault()
            If usuario Is Nothing Then
                usuario = New t_usuarios_permisos
                usuario.id_programa = idPrograma
                usuario.id_usuario = id_usuario
                usuario.registro_radicados_post_cierre = Activo
                dbentities.t_usuarios_permisos.Add(usuario)
            Else
                usuario.registro_radicados_post_cierre = Activo
                dbentities.Entry(usuario).State = Entity.EntityState.Modified
            End If

            dbentities.SaveChanges()
        End Using
        fillGrid(False)
    End Sub

    Sub fillGrid(ByVal bndREBIND As Boolean)
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Me.grd_cate.DataSource = db.vw_t_usuarios_permisos.Where(Function(p) p.id_programa = idPrograma And (p.usuario.Contains(Me.txt_doc.Text) Or p.email_usuario.Contains(Me.txt_doc.Text) Or p.job.Contains(Me.txt_doc.Text) Or p.usuario.Contains(Me.txt_doc.Text))).OrderByDescending(Function(o) o.id_usuario).ToList()

        If bndREBIND Then
            Me.grd_cate.DataBind()
        End If

        'Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Using dbentities As New dbRMS_JIEntities
            Dim permisosRegistrados As Boolean = True
            Dim permisos = dbentities.tme_radicados_permisos.Where(Function(p) p.id_programa = idPrograma).FirstOrDefault()
            If permisos Is Nothing Then
                permisosRegistrados = False
                permisos = New tme_radicados_permisos
                permisos.id_programa = idPrograma
            End If
            permisos.fecha_cierre_final = Me.dt_fecha_post_cierre.SelectedDate
            permisos.habilitar_facturacion_post_cierre = If(Me.rbn_registro_radicados.SelectedValue = "1", True, False)

            If permisosRegistrados Then
                dbentities.Entry(permisos).State = Entity.EntityState.Modified
            Else
                dbentities.tme_radicados_permisos.Add(permisos)
            End If
            dbentities.SaveChanges()
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/financiero/frm_radicadosAdmin"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub
    Sub loadPermisos()
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Using dbentities As New dbRMS_JIEntities
            Dim permisos = dbentities.tme_radicados_permisos.Where(Function(p) p.id_programa = idPrograma).FirstOrDefault()
            If permisos IsNot Nothing Then
                Me.dt_fecha_post_cierre.SelectedDate = permisos.fecha_cierre_final
                Me.rbn_registro_radicados.SelectedValue = If(permisos.habilitar_facturacion_post_cierre, "1", "2")
            End If
        End Using
    End Sub
End Class