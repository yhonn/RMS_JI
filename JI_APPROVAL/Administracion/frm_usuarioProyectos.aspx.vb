Imports ly_SIME
Imports Telerik.Web.UI

Public Class frm_usuarioProyectos
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_ADMIN_USU_PRO"
    Dim db As New dbRMS_JIEntities
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    'Dim crudFunctions As New CRUD_SIME.CRUDNP.crud

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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_pro_rel)
                cl_user.chk_Rights(Page.Controls, 7, frmCODE, 0, grd_pro_rel)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls

                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            Dim id_usuario = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
            Me.id_usuario.Value = id_usuario
            loadListas(id_usuario)
            fillGrid()
        End If
    End Sub
    Sub fillGrid()
        Dim id_usuario = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
        Dim usuario = db.t_usuarios.Find(id_usuario)
        Me.lblt_subtitulo_pantalla.Text = "Sub Regiones Usuario - " + usuario.nombre_usuario + " " + usuario.apellidos_usuario
        'Me.SqlDataSource2.SelectCommand = "SELECT * FROM t_programas WHERE nombre_programa LIKE '%" & Me.txt_doc.Text.Trim.Replace("'", "") & "%'"
        Me.lblt_subtitulo_pantalla.Text = "Proyectos asignados a :- " + usuario.nombre_usuario + " " + usuario.apellidos_usuario
        'Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
    End Sub
    Protected Sub chkVisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Activo As Boolean = False

        Dim chkvisible As CheckBox = CType(sender, CheckBox)
        If chkvisible.Checked Then
            Activo = True
        End If
        Using dbentities As New dbRMS_JIEntities
            Dim idAcceso = Convert.ToInt32(chkvisible.Attributes("id_usuario_ficha_proyecto").ToString())
            Dim acceso_fich = dbentities.t_usuario_ficha_proyecto.Find(idAcceso)
            acceso_fich.acc_act = Activo
            acceso_fich.fecha_upd = Date.Now
            acceso_fich.id_usuario_upd = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            dbentities.Entry(acceso_fich).State = Entity.EntityState.Modified
            dbentities.SaveChanges()
        End Using
        fillGrid()
    End Sub
    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_pro_rel.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim visible As New CheckBox
            visible = CType(e.Item.FindControl("chkActivo"), CheckBox)
            visible.Attributes.Add("id_usuario_ficha_proyecto", DataBinder.Eval(e.Item.DataItem, "id_usuario_ficha_proyecto").ToString())

            If DataBinder.Eval(e.Item.DataItem, "acc_act").ToString = "True" Then
                visible.ToolTip = controles.iconosGrid("col_hlk_activo")
                visible.Checked = True
            Else
                visible.Checked = False
                visible.ToolTip = controles.iconosGrid("col_hlk_inactivo")
            End If
        End If
    End Sub
    Sub loadListas(ByVal id_usuario As Integer)

        Using dbEntities As New dbRMS_JIEntities

            Dim oPro = dbEntities.t_usuario_ficha_proyecto.Find(id_usuario)
            Dim idPrograma = Convert.ToString(Me.Session("E_IDPrograma"))
            Me.grd_pro_rel.DataSource = ""
            Me.grd_pro_rel.DataSource = dbEntities.vw_tme_ficha_usuarios.Where(Function(p) p.id_usuario = id_usuario).ToList()
            Me.grd_pro_rel.DataBind()

            Dim UpmsSelected = dbEntities.t_usuario_ficha_proyecto.Where(Function(p) p.id_usuario = id_usuario).Select(Function(p) p.id_ficha_proyecto).ToList()
            Dim UpmEnable = dbEntities.vw_tme_ficha_proyecto.Where(Function(x) x.id_programa.Contains(idPrograma) And Not UpmsSelected.Contains(x.id_ficha_proyecto)).ToList()

            Me.grd_upm_add.DataSource = UpmEnable.Where(Function(p) (p.nombre_proyecto.Contains(Me.txt_doc.Text) Or
                                                                                         p.codigo_RFA.Contains(Me.txt_doc.Text) Or
                                                                                         p.nombre_ejecutor.Contains(Me.txt_doc.Text)))
            Me.grd_upm_add.DataBind()
        End Using

    End Sub
    Protected Sub upm_check(sender As Object, e As EventArgs)
        Dim check As CheckBox = DirectCast(sender, CheckBox)
        Dim item As GridDataItem = DirectCast(check.NamingContainer, GridDataItem)
        Dim id_upm = Convert.ToInt32(item.GetDataKeyValue("id_ficha_proyecto").ToString())
        Dim id = Convert.ToInt32(Me.id_usuario.Value)
        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Using dbEntities As New dbRMS_JIEntities
            Dim oFichaUsu As New t_usuario_ficha_proyecto
            oFichaUsu.id_usuario = id
            oFichaUsu.id_ficha_proyecto = id_upm
            oFichaUsu.fecha_crea = Date.Now
            oFichaUsu.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            oFichaUsu.acc_act = True
            dbEntities.t_usuario_ficha_proyecto.Add(oFichaUsu)
            dbEntities.SaveChanges()
            Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = id)
            loadListas(id)
        End Using
    End Sub
    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_upm_add.PageIndexChanged
        Dim id = Convert.ToInt32(Me.id_usuario.Value)
        loadListas(id)
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_upm_add.PageSizeChanged
        Dim id = Convert.ToInt32(Me.id_usuario.Value)
        loadListas(id)
    End Sub
    Protected Sub grd_pro_rel_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_pro_rel.PageIndexChanged
        Dim id = Convert.ToInt32(Me.id_usuario.Value)
        loadListas(id)
    End Sub

    Protected Sub grd_pro_rel_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_pro_rel.PageSizeChanged
        Dim id = Convert.ToInt32(Me.id_usuario.Value)
        loadListas(id)
    End Sub

    Protected Sub btn_buscar_Click(sender As Object, e As EventArgs) Handles btn_buscar.Click
        Dim id = Convert.ToInt32(Me.id_usuario.Value)
        loadListas(id)
    End Sub
    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Administracion/frm_aportes"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub
End Class