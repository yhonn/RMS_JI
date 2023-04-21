Imports ly_SIME
Imports Telerik.Web.UI

Public Class frm_adminLanguage
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADM_LANGU"
    Dim db As New dbRMS_JIEntities
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub fillGrid()
        Me.grd_cate.DataSource = db.t_mod.Where(Function(p) (p.mod_name.Contains(Me.txt_doc.Text) _
                                                    Or p.mod_desc.Contains(Me.txt_doc.Text) _
                                                    Or p.mod_url.Contains(Me.txt_doc.Text)) And p.id_sys = cl_user.idSys).ToList()
        Me.grd_cate.DataBind()
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
            Using dbEntities As New dbRMS_JIEntities
                Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
                Me.cmb_idioma.DataSource = dbEntities.vw_t_programa_idiomas.Where(Function(p) p.id_programa = id_programa).ToList()
                Me.cmb_idioma.DataTextField = "descripcion_idioma"
                Me.cmb_idioma.DataValueField = "id_idioma"
                Me.cmb_idioma.DataBind()
                Me.cmb_idioma.SelectedValue = cl_user.id_idioma
                fillGrid()
            End Using
        End If
    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnkTraducir As HyperLink = New HyperLink
            hlnkTraducir = CType(e.Item.FindControl("col_hlk_traducir"), HyperLink)
            hlnkTraducir.NavigateUrl = "frm_lenguageAD?Id=" & DataBinder.Eval(e.Item.DataItem, "id_mod").ToString() & "&idI=" & Me.cmb_idioma.SelectedValue
            hlnkTraducir.ToolTip = controles.iconosGrid("col_hlk_traducir")
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
            Dim acceso_mod = dbentities.t_access_mod.Find(idAcceso)
            acceso_mod.acm_acc = Activo

            dbentities.Entry(acceso_mod).State = Entity.EntityState.Modified
            dbentities.SaveChanges()
        End Using
        fillGrid()
    End Sub

End Class