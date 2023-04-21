Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Public Class frm_contratos_consultores
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_CONTRATOS_CO"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim idUser As Integer = 0
    Dim varUSer As String
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)


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

        idUser = Convert.ToInt32(Me.Session("E_IdUser").ToString())
        If Not Me.IsPostBack Then
            fillGrid(True)

        End If

    End Sub
    Sub fillGrid(ByVal bndBind As Boolean)
        Using dbEntities As New dbRMS_JIEntities
            Dim id_usuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Me.grd_cate.DataSource = dbEntities.vw_tme_contratos.Where(Function(p) p.id_contratista = id_usuario Or p.id_supervisor = id_usuario).ToList()

            If bndBind Then
                Me.grd_cate.DataBind()
            End If
        End Using
    End Sub

    Private Sub grd_cate_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource
        fillGrid(False)
    End Sub
    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid(False)
    End Sub
    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid(False)
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            Dim hlnkActivar As HyperLink = New HyperLink
            hlnkActivar = CType(e.Item.FindControl("hlk_activar"), HyperLink)
            hlnkActivar.NavigateUrl = "frm_contratosSgmt?id=" & DataBinder.Eval(e.Item.DataItem, "id_contrato").ToString()

            Dim estado = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estado_contrato").ToString())
            If estado <> 1 Then
            ElseIf estado = 1 Then
                hlnkActivar.Visible = False
            ElseIf estado = 4 Then
                hlnkActivar.Visible = False
            End If
        End If
    End Sub
End Class