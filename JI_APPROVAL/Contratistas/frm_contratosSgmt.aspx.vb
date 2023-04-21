Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Public Class frm_contratosSgmt
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_CONT_SGMT"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtItinerario As New DataTable
    Dim dtAlojamiento As New DataTable
    Dim clss_approval As APPROVAL.clss_approval
    Const cPENDING = 1
    Const cAPPROVED = 2
    Const cnotAPPROVED = 3
    Const cCANCELLED = 4
    Const cOPEN = 5
    Const cSTANDby = 6
    Const cCOMPLETED = 7

    Const cAction_ByProcess = 1
    Const cAction_ByMessage = 2


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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_entregables)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            Me.idContrato.Value = Convert.ToInt32(Me.Request.QueryString("id"))
            loadData()
            fillGrid()
        End If

    End Sub
    Sub loadData()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_contrato = Convert.ToInt32(Me.idContrato.Value)
            Dim oContrato = dbEntities.vw_tme_contratos.Where(Function(p) p.id_contrato = id_contrato).FirstOrDefault()
            Me.lbl_cod_contrato.Text = oContrato.numero_contrato
            Me.lbl_contratista.Text = oContrato.contratista
            Me.lbl_fecha_inicio.Text = oContrato.fecha_inicio
            Me.lbl_fecha_fin.Text = oContrato.fecha_finalizacion
            Me.lbl_valor.Text = "$" & String.Format("{0:N}", oContrato.valor_contrato)
            Me.lbl_objeto.Text = oContrato.objeto_contrato
            Me.lbl_supervisor.Text = oContrato.supervisor
        End Using
    End Sub
    Sub fillGrid()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_contrato = Convert.ToInt32(Me.idContrato.Value)
            Me.grd_entregables.DataSource = dbEntities.vw_contratos_entregables.Where(Function(p) p.id_contrato = id_contrato).ToList()
            Me.grd_entregables.DataBind()
        End Using
    End Sub

    Protected Sub grd_entregables_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_entregables.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim hlnkActivar As HyperLink = New HyperLink
            hlnkActivar = CType(e.Item.FindControl("hlk_activar"), HyperLink)
            hlnkActivar.NavigateUrl = "frm_entregableSgmt?id=" & DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString()


            Dim hlnkDetalle As HyperLink = New HyperLink
            hlnkDetalle = CType(e.Item.FindControl("col_hlk_detalle"), HyperLink)
            hlnkDetalle.NavigateUrl = "frm_entregablePrint?Id=" & DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString()


            Dim hlnktrn As HyperLink = New HyperLink
            hlnktrn = CType(e.Item.FindControl("hlk_view"), HyperLink)
            If DataBinder.Eval(e.Item.DataItem, "url") IsNot Nothing Then
                Dim file_name = DataBinder.Eval(e.Item.DataItem, "url").ToString()
                hlnktrn.ToolTip = controles.iconosGrid("col_hlk_view")
                Dim adjunto = controles.iconosGrid("col_hlk_view")
                hlnktrn.NavigateUrl = file_name
                If file_name.Length < 4 Then
                    hlnktrn.Visible = False
                End If
            Else
                hlnktrn.Visible = False
            End If
        End If
    End Sub
End Class