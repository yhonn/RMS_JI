Imports Telerik.Web.UI
Imports ly_SIME

Public Class frm_menu
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADMIN_MENU"
    Dim db As New dbRMS_JIEntities
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub fillGrid()
        'Me.grd_cate.DataBind()
        Me.grd_cate.DataSourceID = ""
        Me.grd_cate.DataSource = db.vw_t_menu.Where(Function(p) p.nombre_item_menu.Contains(Me.txt_doc.Text)).ToList()
        'Me.SqlDataSource2.SelectCommand = "SELECT * FROM t_programas WHERE nombre_programa LIKE '%" & Me.txt_doc.Text.Trim.Replace("'", "") & "%'"
        Me.grd_cate.DataBind()

        Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
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
            End Using
            fillGrid()
        End If
    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)
            hlnkEdit.NavigateUrl = "frm_menuEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_menu").ToString()
            hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")
        End If
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid()
    End Sub

End Class