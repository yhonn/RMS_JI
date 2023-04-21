Imports Telerik.Web.UI
Imports ly_SIME

Public Class frm_acceso_ctrl
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADM_ROLCTR"
    Dim db As New dbRMS_JIEntities
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub fillGrid()
        Me.grd_cate.DataBind()
        Me.grd_cate.DataSourceID = ""
        Dim id_rol = Convert.ToInt32(Me.Request.QueryString("IdRol").ToString)
        Dim id_mod = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
        Me.grd_cate.DataSource = db.VW_GR_USER_CTRLS.Where(Function(p) p.id_rol = id_rol And p.id_mod = id_mod And p.requiere_acceso = True).ToList()
        Dim rol = db.t_rol.Find(id_rol)
        Dim modulo = db.t_mod.Find(id_mod)
        Me.lblt_subtitulo_pantalla.Text = "Acceso Controles - Rol: " + rol.rol_name + " - Módulo" + modulo.mod_name

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

    Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand
        Dim id_usuario = Convert.ToInt32(TryCast(e.Item, GridDataItem).GetDataKeyValue("id_rol_usr").ToString())
        Dim usuario = db.t_rol_usr.Find(id_usuario)
        db.t_rol_usr.Remove(usuario)
        db.SaveChanges()
        MsgBox("Eliminado: " & id_usuario)
        Me.Response.Redirect(String.Concat("~/SuperAdmin/frm_acceso_ctrl?id=", Me.lbl_id_mod.Text, "&IdRol=", Me.Request.QueryString("IdRol").ToString))
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
            visible.Attributes.Add("idAcceso", DataBinder.Eval(e.Item.DataItem, "id_acc_ctrl").ToString())

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
                Dim oRolUsr As New t_access_ctrl

                oRolUsr.id_rol = Convert.ToInt32(Me.Request.QueryString("IdRol").ToString)
                oRolUsr.id_control = Convert.ToInt32(Me.cmb_id_ctrl.SelectedValue)
                oRolUsr.actrl_acc = True

                dbEntities.t_access_ctrl.Add(oRolUsr)
                dbEntities.SaveChanges()
            End Using
            'Me.MsgGuarda.Visible = True
            Me.Response.Redirect(String.Concat("~/SuperAdmin/frm_acceso_ctrl?id=", Me.lbl_id_mod.Text, "&IdRol=", Me.Request.QueryString("IdRol").ToString))
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
            Dim acceso_mod = dbentities.t_access_ctrl.Find(idAcceso)
            acceso_mod.actrl_acc = Activo

            dbentities.Entry(acceso_mod).State = Entity.EntityState.Modified
            dbentities.SaveChanges()
        End Using
        fillGrid()
    End Sub

End Class