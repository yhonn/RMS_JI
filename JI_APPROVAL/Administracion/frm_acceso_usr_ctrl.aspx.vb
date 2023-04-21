Imports Telerik.Web.UI
Imports ly_SIME
Public Class frm_acceso_usr_ctrl
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADM_USRCTR"
    Dim db As New dbRMS_JIEntities
    Dim controles As New ly_SIME.CORE.cls_controles
    Sub fillGrid()
        Me.grd_cate.DataBind()
        Me.grd_cate.DataSourceID = ""
        Dim id_usuario = Convert.ToInt32(Me.Request.QueryString("IdUsuario").ToString)
        Dim id_mod = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
        Me.grd_cate.DataSource = db.VW_GR_USER_ACC_MOD_CTRLS.Where(Function(p) p.id_usuario = id_usuario And p.id_mod = id_mod And p.requiere_acceso = True).ToList()
        Dim usuario = db.t_usuarios.Find(id_usuario)
        Dim modulo = db.t_mod.Find(id_mod)
        Me.lblt_subtitulo_pantalla.Text = "Acceso Controles - Usuario: " + usuario.nombre_usuario + " " + usuario.apellidos_usuario + " - Módulo: " + modulo.mod_name

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
            Me.lbl_id_mod.Text = Me.Request.QueryString("Id").ToString
            fillGrid()
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
            Dim visible As New CheckBox
            visible = CType(e.Item.FindControl("chkActivo"), CheckBox)
            visible.Attributes.Add("idAcceso", DataBinder.Eval(e.Item.DataItem, "id_acc_usuario_ctrl").ToString())

            If DataBinder.Eval(e.Item.DataItem, "actrl_acc").ToString = "True" Then
                visible.ToolTip = controles.iconosGrid("col_hlk_activo")
                visible.Checked = True
            Else
                visible.Checked = False
                visible.ToolTip = controles.iconosGrid("col_hlk_inactivo")
            End If
        End If
    End Sub
    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        If Me.cmb_id_ctrl.SelectedValue IsNot Nothing Then
            Using dbEntities As New dbRMS_JIEntities
                Dim oRolUsr As New t_access_usuario_ctrl

                oRolUsr.id_usuario = Convert.ToInt32(Me.Request.QueryString("IdUsuario").ToString)
                oRolUsr.id_control = Convert.ToInt32(Me.cmb_id_ctrl.SelectedValue)
                oRolUsr.actrl_acc = True
                oRolUsr.fecha_crea = Date.UtcNow
                oRolUsr.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oRolUsr.id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                dbEntities.t_access_usuario_ctrl.Add(oRolUsr)
                dbEntities.SaveChanges()
            End Using
            'Me.MsgGuarda.Visible = True
            Me.Response.Redirect(String.Concat("~/Administracion/frm_acceso_usr_ctrl?id=", Me.lbl_id_mod.Text, "&IdUsuario=", Me.Request.QueryString("IdUsuario").ToString))
        End If
    End Sub
    Protected Sub chkVisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Activo As Boolean = False

        Dim chkvisible As CheckBox = CType(sender, CheckBox)
        If chkvisible.Checked Then
            Activo = True
        End If
        Using dbentities As New dbRMS_JIEntities
            Dim idAcceso = Convert.ToInt32(chkvisible.Attributes("idAcceso").ToString())
            Dim acceso_mod = dbentities.t_access_usuario_ctrl.Find(idAcceso)
            acceso_mod.actrl_acc = Activo
            acceso_mod.fecha_upd = Date.UtcNow
            acceso_mod.id_usuario_upd = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            dbentities.Entry(acceso_mod).State = Entity.EntityState.Modified
            dbentities.SaveChanges()
        End Using
        fillGrid()
    End Sub
End Class