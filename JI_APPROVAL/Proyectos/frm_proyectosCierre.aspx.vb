Imports Telerik.Web.UI

Public Class frm_proyectosCierre
    Inherits System.Web.UI.Page

    Dim cl_user As New ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "SEL_PROY_CIERRE"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim db As New ly_SIME.dbRMS_HNEntities

    Sub fillGrid()
        Dim idPrograma = Me.Session("E_IDPrograma")
        Me.grd_cate.DataSource = db.vw_tme_ficha_proyecto.Where(Function(p) p.id_programa.Contains(idPrograma) And _
                                                                    (p.codigo_SAPME.Contains(Me.txt_doc.Text) Or p.nombre_proyecto.Contains(Me.txt_doc.Text))).ToList()
        Me.grd_cate.DataBind()
        'Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
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
            fillGrid()
        End If
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnkPrint As HyperLink = New HyperLink
            hlnkPrint = CType(e.Item.FindControl("hlk_Print"), HyperLink)
            hlnkPrint.NavigateUrl = "~/Proyectos/frm_proyectosRep?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()

            Dim hlnkActivar As HyperLink = New HyperLink
            hlnkActivar = CType(e.Item.FindControl("hlk_activar"), HyperLink)
            hlnkActivar.NavigateUrl = "~/Proyectos/frm_ProyectoCierre?id=" & DataBinder.Eval(e.Item.DataItem, "id_ficha_proyecto").ToString()
        End If
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid()
    End Sub

End Class