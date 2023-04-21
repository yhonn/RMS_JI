Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_SIME

Partial Class frm_Ejecutor
    Inherits System.Web.UI.Page
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As New ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_ADMIN_EJEC"
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub fillGrid()
        Dim db As New dbRMS_JIEntities
        Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
        Dim sqlS = "select * from t_ejecutores where (nombre_ejecutor like '%" & Me.txt_doc.Text & "%' or nombre_corto like '%" & Me.txt_doc.Text & "%'  or representante_legal like '%%') and id_programa = " & id_programa & ""
        Me.grd_cate.DataSourceID = ""
        Me.grd_cate.DataSource = db.t_ejecutores.SqlQuery(sqlS).ToList()
        Me.grd_cate.DataBind()
        'Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/proyectos/frm_EjecutorAD")
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
            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)
            hlnkEdit.NavigateUrl = "frm_EjecutorEdit.aspx?IdEjecME=" & DataBinder.Eval(e.Item.DataItem, "id_ejecutor").ToString()
            'hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")

            Dim hlnkPrint As HyperLink = New HyperLink
            hlnkPrint = CType(e.Item.FindControl("col_hlk_Print"), HyperLink)
            hlnkPrint.NavigateUrl = "frm_EjecutorPrint.aspx?IdEjecME=" & DataBinder.Eval(e.Item.DataItem, "id_ejecutor").ToString()
            'hlnkPrint.ToolTip = controles.iconosGrid("col_hlk_Print")

            Dim hlnkActivo As New CheckBox
            hlnkActivo = CType(e.Item.FindControl("chkActivo"), CheckBox)

            If DataBinder.Eval(e.Item.DataItem, "id_estado_EJ").ToString() = "1" Then
                hlnkActivo.ToolTip = controles.iconosGrid("col_hlk_activo")
                hlnkActivo.Checked = True
            Else
                hlnkActivo.Checked = False
                hlnkActivo.ToolTip = controles.iconosGrid("col_hlk_inactivo")
            End If

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_delete"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_ejecutor").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_ejecutor").ToString())
            'hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_delete")
        End If
    End Sub

    Protected Sub chkVisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim row As GridItem
        Dim sql = ""
        Dim Activo As String = ""
        cnnME.Open()
        For Each row In Me.grd_cate.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim idEjecutor As Integer = Convert.ToInt32(dataItem.GetDataKeyValue("id_ejecutor"))
                Dim chkvisible As CheckBox = CType(row.Cells(0).FindControl("chkActivo"), CheckBox)
                Activo = 1
                If chkvisible.Checked = False Then
                    Activo = 2
                End If
                sql = "UPDATE t_Ejecutores SET id_estado_EJ=" & Activo & " WHERE id_ejecutor=" & idEjecutor
                Dim dm As New SqlDataAdapter(sql, cnnME)
                Dim ds As New DataSet("IdActivo")
                dm.Fill(ds, "IdActvo")
            End If
        Next
        fillGrid()
        cnnME.Close()
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid()
    End Sub

    Protected Sub RadGrid1_SortCommand(ByVal source As Object, ByVal e As GridSortCommandEventArgs) Handles grd_cate.SortCommand
        Dim tableView As GridTableView = e.Item.OwnerTableView
        fillGrid()

        If e.CommandArgument = "nombre_ejecutor" Then
            Select Case e.OldSortOrder
                Case GridSortOrder.None
                    Dim expression As New GridSortExpression()
                    expression.FieldName = "nombre_ejecutor"
                    expression.SortOrder = GridSortOrder.Ascending
                    tableView.SortExpressions.AddSortExpression(expression)
                    tableView.Rebind()
                    Exit Select
                Case GridSortOrder.Ascending
                    Dim expression As New GridSortExpression()
                    expression.FieldName = "nombre_ejecutor"
                    expression.SortOrder = GridSortOrder.Descending
                    tableView.SortExpressions.AddSortExpression(expression)
                    tableView.Rebind()
                    Exit Select
                Case GridSortOrder.Descending
                    Dim expression As New GridSortExpression()
                    expression.FieldName = "nombre_ejecutor"
                    expression.SortOrder = GridSortOrder.None
                    tableView.SortExpressions.AddSortExpression(expression)
                    tableView.Rebind()
                    Exit Select
            End Select
        ElseIf e.CommandArgument = "representante_legal" Then
            Select Case e.OldSortOrder
                Case GridSortOrder.None
                    Dim expression As New GridSortExpression()
                    expression.FieldName = "representante_legal"
                    expression.SortOrder = GridSortOrder.Ascending
                    tableView.SortExpressions.AddSortExpression(expression)
                    tableView.Rebind()
                    Exit Select
                Case GridSortOrder.Ascending
                    Dim expression As New GridSortExpression()
                    expression.FieldName = "representante_legal"
                    expression.SortOrder = GridSortOrder.Descending
                    tableView.SortExpressions.AddSortExpression(expression)
                    tableView.Rebind()
                    Exit Select
                Case GridSortOrder.Descending
                    Dim expression As New GridSortExpression()
                    expression.FieldName = "representante_legal"
                    expression.SortOrder = GridSortOrder.None
                    tableView.SortExpressions.AddSortExpression(expression)
                    tableView.Rebind()
                    Exit Select
            End Select
        ElseIf e.CommandArgument = "telefono_ejecutor" Then
            Select Case e.OldSortOrder
                Case GridSortOrder.None
                    Dim expression As New GridSortExpression()
                    expression.FieldName = "telefono_ejecutor"
                    expression.SortOrder = GridSortOrder.Ascending
                    tableView.SortExpressions.AddSortExpression(expression)
                    tableView.Rebind()
                    Exit Select
                Case GridSortOrder.Ascending
                    Dim expression As New GridSortExpression()
                    expression.FieldName = "telefono_ejecutor"
                    expression.SortOrder = GridSortOrder.Descending
                    tableView.SortExpressions.AddSortExpression(expression)
                    tableView.Rebind()
                    Exit Select
                Case GridSortOrder.Descending
                    Dim expression As New GridSortExpression()
                    expression.FieldName = "telefono_ejecutor"
                    expression.SortOrder = GridSortOrder.None
                    tableView.SortExpressions.AddSortExpression(expression)
                    tableView.Rebind()
                    Exit Select
            End Select
        End If
    End Sub


    Protected Sub btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Try
                Dim Sql = "delete from t_ejecutores WHERE id_ejecutor = " & Me.identity.Text
                dbEntities.Database.ExecuteSqlCommand(Sql)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_Ejecutor"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    Protected Sub Elimina_Elemento(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

End Class
