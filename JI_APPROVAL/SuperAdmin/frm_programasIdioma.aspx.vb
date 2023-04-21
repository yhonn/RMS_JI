Imports ly_SIME
Imports Telerik.Web.UI

Public Class frm_programasIdioma
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADM_LANGP"
    Dim db As New dbRMS_JIEntities
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub fillGrid()
        Me.grd_cate.DataBind()
        Dim id_programa = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
        Me.grd_cate.DataSource = db.vw_t_programa_idiomas.Where(Function(p) p.id_programa = id_programa).ToList()
        'Me.lblt_subtitulo_pantalla.Text = "Acceso Controles - Rol: " + rol.rol_name + " - Módulo" + modulo.mod_name

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
            'Dim visible As New CheckBox
            'visible = CType(e.Item.FindControl("chkActivo"), CheckBox)
            'visible.Attributes.Add("idAcceso", DataBinder.Eval(e.Item.DataItem, "id_acc_ctrl").ToString())
        End If
    End Sub
    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        If Me.cmb_id_idioma.SelectedValue IsNot Nothing Then
            Using dbEntities As New dbRMS_JIEntities
                Dim oRolUsr As New t_programas_idioma

                oRolUsr.id_programa = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                oRolUsr.id_idioma = Convert.ToInt32(Me.cmb_id_idioma.SelectedValue)

                dbEntities.t_programas_idioma.Add(oRolUsr)
                dbEntities.SaveChanges()
            End Using
            'Me.MsgGuarda.Visible = True
            Me.Response.Redirect(String.Concat("~/SuperAdmin/frm_programasIdioma?id=", Me.Request.QueryString("Id")))
        End If
    End Sub

End Class