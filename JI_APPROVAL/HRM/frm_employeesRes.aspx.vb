Imports Telerik.Web.UI
Imports ly_SIME
Imports ly_APPROVAL
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Drawing

Public Class frm_employeeRes
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADMIN_EMPLOYEE_RES"
    Dim db As New dbRMS_JIEntities
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim vAccrued As Decimal = 0
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim totalC As Integer()

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
                'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate_employee)
                'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_users)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        Dim sManager = ScriptManager.GetCurrent(Me)
        sManager.RegisterPostBackControl(Me.export_button_annual)
        sManager.RegisterPostBackControl(Me.export_button_casual)
        sManager.RegisterPostBackControl(Me.export_button_sick)


        If Not Me.IsPostBack Then

            Dim Random As New Random()
            Dim Aleatorio As Double = Random.Next(1, 999)
            Me.hd_dtANNUAL.Value = String.Format("dtANNUAL{0}_{1}", Me.Session("E_IdUser"), Aleatorio)
            Me.hd_dtCASUAL.Value = String.Format("dtCASUAL{0}_{1}", Me.Session("E_IdUser"), Aleatorio)
            Me.hd_dtSICK.Value = String.Format("dtSICK{0}_{1}", Me.Session("E_IdUser"), Aleatorio)

            fill_Grid(True)
            fill_Grid_casual(True)
            fill_Grid_sick(True)


            Me.btnlk_Export.PostBackUrl = "~/HRM/frm_TemplateReport.aspx?idTR=10&vUs=0"
            Me.btnlk_Export_casual.PostBackUrl = "~/HRM/frm_TemplateReport.aspx?idTR=11&vUs=0"
            Me.btnlk_Export_sick.PostBackUrl = "~/HRM/frm_TemplateReport.aspx?idTR=12&vUs=0"

            'Me.radgrid_emp_reported.TotalsSettings.ColumnsSubTotalsPosition = TotalsPosition.None
            '******Me.radgrid_emp_reported.TotalsSettings.ColumnGrandTotalsPosition = TotalsPosition.None
            'Me.radgrid_emp_reported.TotalsSettings.RowGrandTotalsPosition = TotalsPosition.None
            'Me.radgrid_emp_reported.TotalsSettings.RowsSubTotalsPosition = TotalsPosition.None
            'Me.radgrid_emp_reported.Rebind()

        End If
    End Sub


    Public Sub fill_Grid(ByVal booREbind As Boolean)

        Dim ds As New DataSet
        Dim adapter As SqlDataAdapter

        Me.hd_tp.Value = 5

        If Session(Me.hd_dtANNUAL.Value) Is Nothing Then

            cnn.Open()

            Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("SP_HR_BILLABLE_TIME", cnn)
            cmd.CommandType = CommandType.StoredProcedure
            'cmd.Parameters.Add("@tp_view", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            cmd.Parameters.AddWithValue("@tp_view", Me.hd_tp.Value)
            cmd.Parameters.AddWithValue("@_id_usuario", 0)
            cmd.Parameters.AddWithValue("@_anio", 0)
            cmd.Parameters.AddWithValue("@_programa", Convert.ToInt32(Me.Session("E_IDPrograma")))

            adapter = New SqlDataAdapter(cmd)
            adapter.Fill(ds)

            Session(Me.hd_dtANNUAL.Value) = ds.Tables(0)

            cnn.Close()

        End If

        'Dim dataT As DataTable = ds.Tables(0)
        'radgrid_emp_reported.DataSource = ds.Tables(0)

        radgrid_emp_reported.DataSource = Session(Me.hd_dtANNUAL.Value)

        'grd_cate.DataBind()

        If booREbind Then
            radgrid_emp_reported.Rebind()
        End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("id_usuario"))) Then
        '    hideColumn("id_usuario")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("id_billable_time"))) Then
        '    hideColumn("id_billable_time")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("payable"))) Then
        '    hideColumn("payable")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("anio"))) Then
        '    hideColumn("anio")
        'End If


    End Sub





    Public Sub fill_Grid_casual(ByVal booREbind As Boolean)

        Dim ds As New DataSet
        Dim adapter As SqlDataAdapter



        Me.hd_tp.Value = 8

        If Session(Me.hd_dtCASUAL.Value) Is Nothing Then

            cnn.Open()

            Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("SP_HR_BILLABLE_TIME", cnn)
            cmd.CommandType = CommandType.StoredProcedure
            'cmd.Parameters.Add("@tp_view", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            cmd.Parameters.AddWithValue("@tp_view", Me.hd_tp.Value)
            cmd.Parameters.AddWithValue("@_id_usuario", 0)
            cmd.Parameters.AddWithValue("@_anio", 0)
            cmd.Parameters.AddWithValue("@_programa", Convert.ToInt32(Me.Session("E_IDPrograma")))

            adapter = New SqlDataAdapter(cmd)
            adapter.Fill(ds)

            Session(Me.hd_dtCASUAL.Value) = ds.Tables(0)

            cnn.Close()

        End If

        'Dim dataT As DataTable = ds.Tables(0)
        'radgrid_casual_reported.DataSource = ds.Tables(0)
        radgrid_casual_reported.DataSource = Session(Me.hd_dtCASUAL.Value)
        'grd_cate.DataBind()

        If booREbind Then
            radgrid_casual_reported.Rebind()
        End If




    End Sub


    Public Sub fill_Grid_sick(ByVal booREbind As Boolean)

        Dim ds As New DataSet
        Dim adapter As SqlDataAdapter

        Me.hd_tp.Value = 9

        If Session(Me.hd_dtSICK.Value) Is Nothing Then

            cnn.Open()

            Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("SP_HR_BILLABLE_TIME", cnn)
            cmd.CommandType = CommandType.StoredProcedure
            'cmd.Parameters.Add("@tp_view", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            cmd.Parameters.AddWithValue("@tp_view", Me.hd_tp.Value)
            cmd.Parameters.AddWithValue("@_id_usuario", 0)
            cmd.Parameters.AddWithValue("@_anio", 0)
            cmd.Parameters.AddWithValue("@_programa", Convert.ToInt32(Me.Session("E_IDPrograma")))

            adapter = New SqlDataAdapter(cmd)
            adapter.Fill(ds)

            Session(Me.hd_dtSICK.Value) = ds.Tables(0)

            cnn.Close()

        End If

        'Dim dataT As DataTable = ds.Tables(0)
        ' radgrid_sick_reported.DataSource = ds.Tables(0)

        radgrid_sick_reported.DataSource = Session(Me.hd_dtSICK.Value)

        'grd_cate.DataBind()

        If booREbind Then
            radgrid_sick_reported.Rebind()
        End If

    End Sub





    Public Function check_field(Optional tp As Integer = 0) As DataTable

        Dim id_type = If(tp = 0, Me.hd_tp.Value, tp)
        Dim cls_rh_employee As APPROVAL.cls_rh_employee = New APPROVAL.cls_rh_employee(Convert.ToInt32(Me.Session("E_IDprograma")))
        Dim tbl_fields As DataTable = cls_rh_employee.get_fields(id_type)
        check_field = tbl_fields

    End Function

    'Private Sub radgrid_emp_reported_PivotGridCellExporting(sender As Object, e As PivotGridCellExportingArgs) Handles radgrid_emp_reported.PivotGridCellExporting


    '    If Not IsNothing(e.ExportedCell.Value) Then

    '        If (e.PivotGridModelCell.CellType = PivotGridDataCellType.DataCell) Then

    '            Dim v As String = e.ExportedCell.Value.ToString()

    '            If (e.PivotGridModelCell.IsGrandTotalCell And e.ExportedCell.Value.ToString().Equals("Total Sum of accrued")) Then



    '            End If

    '        End If

    '    End If


    'End Sub

    Private Sub radgrid_emp_reported_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles radgrid_emp_reported.CellDataBound

        Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)

        If TypeOf e.Cell Is PivotGridDataCell Then

            If cell IsNot Nothing AndAlso cell.CellType = PivotGridDataCellType.DataCell Then

                'Dim a As String = cell.ParentColumnIndexes(0).ToString()
                'Dim b As String = cell.ParentColumnIndexes(1).ToString()

                If cell.ParentColumnIndexes(1).ToString() = "Annual Leave Balance" Then

                    cell.BackColor = Color.FromArgb(224, 224, 224)

                ElseIf cell.ParentColumnIndexes(1).ToString() = "Sum of Leave_taken" Then

                    Dim cell_taken As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                    If cell_taken > 0 Then

                        cell.BackColor = Color.FromArgb(174, 213, 129)

                    End If

                End If


                'ElseIf cell.CellType = PivotGridDataCellType.ColumnTotalDataCell OrElse cell.CellType = PivotGridDataCellType.RowTotalDataCell Then

            ElseIf cell.CellType = PivotGridDataCellType.RowGrandTotalDataCell OrElse cell.CellType = PivotGridDataCellType.ColumnGrandTotalDataCell Then

                Dim str As String = TryCast(cell.Field, PivotGridAggregateField).DataField

                Select Case TryCast(cell.Field, PivotGridAggregateField).DataField

                    Case "Employment_days"

                        Dim worked As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                        If worked = 0 Then
                            cell.BackColor = Color.FromArgb(245, 245, 245)
                        Else
                            cell.BackColor = Color.FromArgb(224, 224, 224)
                        End If

                    Case "leave_accrued"

                        Dim Accrued As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))
                        vAccrued = Accrued

                        If Accrued = 0 Then
                            cell.BackColor = Color.FromArgb(255, 204, 188)
                        Else
                            cell.BackColor = Color.FromArgb(255, 138, 101)
                        End If

                    Case "Leave_taken"

                        Dim taken As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                        If taken = 0 Then
                            cell.BackColor = Color.FromArgb(220, 237, 200)
                        Else
                            cell.BackColor = Color.FromArgb(174, 213, 129)
                        End If

                    Case "Annual Leave Balance"

                        Dim Balance As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                        If Balance = 0 Then
                            cell.BackColor = Color.FromArgb(255, 249, 196)
                        ElseIf Balance < 0 Then
                            cell.BackColor = Color.FromArgb(255, 111, 0)
                        ElseIf Balance = vAccrued Then
                            cell.BackColor = Color.FromArgb(253, 216, 53)
                        Else
                            cell.BackColor = Color.FromArgb(251, 192, 45)
                        End If

                End Select

            End If


        End If


    End Sub

    Private Sub radgrid_emp_reported_NeedDataSource(sender As Object, e As PivotGridNeedDataSourceEventArgs) Handles radgrid_emp_reported.NeedDataSource
        fill_Grid(False)
    End Sub

    Private Sub radgrid_casual_reported_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles radgrid_casual_reported.CellDataBound


        Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)

        If TypeOf e.Cell Is PivotGridDataCell Then

            If cell IsNot Nothing AndAlso cell.CellType = PivotGridDataCellType.DataCell Then

                If cell.ParentColumnIndexes(1).ToString() = "Annual Leave Balance" Then

                    cell.BackColor = Color.FromArgb(224, 224, 224)

                ElseIf cell.ParentColumnIndexes(1).ToString() = "Sum of Leave_taken" Then

                    Dim cell_taken As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                    If cell_taken > 0 Then

                        cell.BackColor = Color.FromArgb(174, 213, 129)

                    End If

                End If

            ElseIf cell.CellType = PivotGridDataCellType.RowGrandTotalDataCell OrElse cell.CellType = PivotGridDataCellType.ColumnGrandTotalDataCell Then

                Dim str As String = TryCast(cell.Field, PivotGridAggregateField).DataField

                Select Case TryCast(cell.Field, PivotGridAggregateField).DataField

                    Case "employment_days"

                        Dim worked As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                        If worked = 0 Then
                            cell.BackColor = Color.FromArgb(245, 245, 245)
                        Else
                            cell.BackColor = Color.FromArgb(224, 224, 224)
                        End If

                    Case "leave_accrued"

                        Dim Accrued As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))
                        vAccrued = Accrued

                        If Accrued = 0 Then
                            cell.BackColor = Color.FromArgb(255, 204, 188)
                        Else
                            cell.BackColor = Color.FromArgb(255, 138, 101)
                        End If

                    Case "Leave_taken"

                        Dim taken As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                        If taken = 0 Then
                            cell.BackColor = Color.FromArgb(220, 237, 200)
                        Else
                            cell.BackColor = Color.FromArgb(174, 213, 129)
                        End If

                    Case "Casual Leave Balance"

                        Dim Balance As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                        If Balance = 0 Then
                            cell.BackColor = Color.FromArgb(255, 249, 196)
                        ElseIf Balance < 0 Then
                            cell.BackColor = Color.FromArgb(255, 111, 0)
                        ElseIf Balance = vAccrued Then
                            cell.BackColor = Color.FromArgb(253, 216, 53)
                        Else
                            cell.BackColor = Color.FromArgb(251, 192, 45)
                        End If

                End Select

            End If


        End If


    End Sub

    Private Sub radgrid_casual_reported_NeedDataSource(sender As Object, e As PivotGridNeedDataSourceEventArgs) Handles radgrid_casual_reported.NeedDataSource
        fill_Grid_casual(False)
    End Sub

    Private Sub radgrid_sick_reported_CellDataBound(sender As Object, e As PivotGridCellDataBoundEventArgs) Handles radgrid_sick_reported.CellDataBound


        Dim cell As PivotGridDataCell = TryCast(e.Cell, PivotGridDataCell)

        If TypeOf e.Cell Is PivotGridDataCell Then

            If cell IsNot Nothing AndAlso cell.CellType = PivotGridDataCellType.DataCell Then

                If cell.ParentColumnIndexes(1).ToString() = "Annual Leave Balance" Then

                    cell.BackColor = Color.FromArgb(224, 224, 224)

                ElseIf cell.ParentColumnIndexes(1).ToString() = "Sum of Leave_taken" Then

                    Dim cell_taken As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                    If cell_taken > 0 Then

                        cell.BackColor = Color.FromArgb(174, 213, 129)

                    End If

                End If

            ElseIf cell.CellType = PivotGridDataCellType.RowGrandTotalDataCell OrElse cell.CellType = PivotGridDataCellType.ColumnGrandTotalDataCell Then

                Dim str As String = TryCast(cell.Field, PivotGridAggregateField).DataField

                Select Case TryCast(cell.Field, PivotGridAggregateField).DataField

                    Case "employment_days"

                        Dim worked As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                        If worked = 0 Then
                            cell.BackColor = Color.FromArgb(245, 245, 245)
                        Else
                            cell.BackColor = Color.FromArgb(224, 224, 224)
                        End If

                    Case "leave_accrued"

                        Dim Accrued As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))
                        vAccrued = Accrued

                        If Accrued = 0 Then
                            cell.BackColor = Color.FromArgb(255, 204, 188)
                        Else
                            cell.BackColor = Color.FromArgb(255, 138, 101)
                        End If

                    Case "Leave_taken"

                        Dim taken As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                        If taken = 0 Then
                            cell.BackColor = Color.FromArgb(220, 237, 200)
                        Else
                            cell.BackColor = Color.FromArgb(174, 213, 129)
                        End If

                    Case "Sick Leave Balance"

                        Dim Balance As Decimal = If(String.IsNullOrEmpty(cell.DataItem), 0, Convert.ToDecimal(cell.DataItem))

                        If Balance = 0 Then
                            cell.BackColor = Color.FromArgb(255, 249, 196)
                        ElseIf Balance < 0 Then
                            cell.BackColor = Color.FromArgb(255, 111, 0)
                        ElseIf Balance = vAccrued Then
                            cell.BackColor = Color.FromArgb(253, 216, 53)
                        Else
                            cell.BackColor = Color.FromArgb(251, 192, 45)
                        End If

                End Select

            End If


        End If

    End Sub

    Private Sub radgrid_sick_reported_NeedDataSource(sender As Object, e As PivotGridNeedDataSourceEventArgs) Handles radgrid_sick_reported.NeedDataSource
        fill_Grid_sick(False)
    End Sub


    'Private Sub grd_cate_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_cate.ItemDataBound

    '    If (TypeOf e.Item Is GridGroupFooterItem) Then
    '        Dim GroupfooterItem As GridGroupFooterItem = CType(e.Item, GridGroupFooterItem)
    '        For Each dtRow As DataRow In check_field().Rows
    '            GroupfooterItem(dtRow("Period")).Text = "Total: " + GroupfooterItem(dtRow("Period")).Text.ToString().Split(":")(1).ToString()
    '            GroupfooterItem(dtRow("Period")).Style.Add("font-weight", "bold")
    '        Next
    '    End If

    '    If (TypeOf e.Item Is GridFooterItem) Then

    '        Dim footerItem As GridFooterItem = CType(e.Item, GridFooterItem)
    '        For Each dtRow As DataRow In check_field().Rows
    '            footerItem(dtRow("Period")).Text = dtRow("Period") + " " + footerItem(dtRow("Period")).Text.ToString().Split(":")(1).ToString()
    '            footerItem(dtRow("Period")).Style.add("font-weight", "bold")
    '        Next

    '    End If

    'End Sub

    'Private Sub grd_cate_PreRender(sender As Object, e As EventArgs) Handles grd_cate.PreRender

    '    If grd_cate.MasterTableView.GroupByExpressions.Count > 0 Then

    '        For i = 0 To grd_cate.MasterTableView.GroupByExpressions.Count

    '            If Not IsNothing(grd_cate.MasterTableView.GroupByExpressions(i).GroupByFields) Then
    '                Dim strField As String = grd_cate.MasterTableView.GroupByExpressions(i).GroupByFields(0).FieldName
    '                If strField = "anio" Then
    '                    grd_cate.MasterTableView.GroupByExpressions.RemoveAt(i)
    '                End If
    '            End If
    '            i += 1
    '        Next

    '    End If

    '    Dim expression As GridGroupByExpression = New GridGroupByExpression
    '    Dim gridGroupByField As GridGroupByField = New GridGroupByField


    '    gridGroupByField = New GridGroupByField
    '    gridGroupByField.FieldName = "anio"
    '    gridGroupByField.HeaderText = "anio"

    '    expression.GroupByFields.Add(gridGroupByField)
    '    expression.SelectFields.Add(gridGroupByField)

    '    grd_cate.MasterTableView.GroupByExpressions.Add(expression)
    '    grd_cate.Rebind()

    'End Sub

    'Private Sub radgrid_emp_reported_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles radgrid_emp_reported.NeedDataSource

    '    fill_Grid(False)

    'End Sub

    'Private Sub grd_cate_ColumnCreated(sender As Object, e As GridColumnCreatedEventArgs) Handles grd_cate.ColumnCreated

    '    For Each dtRow As DataRow In check_field().Rows

    '        If e.Column.UniqueName = dtRow("Period") Then

    '            Dim boundColumn As New GridBoundColumn
    '            boundColumn = e.Column
    '            boundColumn.Aggregate = GridAggregateFunction.Sum

    '        End If

    '    Next


    '    If e.Column.UniqueName = "salary" Then
    '        Dim bndcol As GridBoundColumn = DirectCast(e.Column, GridBoundColumn)
    '        bndcol.DataFormatString = "{0:###,##0.00}"
    '    End If


    'End Sub



    Protected Sub export_button_annual_ServerClick(sender As Object, e As EventArgs)

        Try

            'Dim alternateText As String = "Xlsx"
            'Dim IND_code As String = "BHA-08"
            Dim DISSegre As String = "AnnualLeave"

            Dim archivo = "Export_" & DISSegre & String.Format("_{0}{1}{2}_{3}{4}_{5}", Date.UtcNow.Year, String.Format("{0:00}", Date.UtcNow.Month), String.Format("{0:00}", Date.UtcNow.Day), String.Format("{0:00}", Date.UtcNow.Hour), String.Format("{0:00}", Date.UtcNow.Minute), String.Format("{0:00}", Date.UtcNow.Second))
            'radgrid_ind_09_firm_values.ExportSettings.Excel.Format = DirectCast([Enum].Parse(GetType(PivotGridExcelFormat), alternateText), PivotGridExcelFormat)


            radgrid_emp_reported.ExportSettings.IgnorePaging = False
            radgrid_emp_reported.ExportSettings.OpenInNewWindow = True
            radgrid_emp_reported.ExportSettings.FileName = archivo
            radgrid_emp_reported.ExportToExcel()


        Catch ex As Exception

            Dim strMess As String = ex.Message

        End Try

    End Sub




    Private Sub radgrid_emp_reported_PivotGridCellExporting(sender As Object, e As PivotGridCellExportingArgs) Handles radgrid_emp_reported.PivotGridCellExporting

        Dim modelDataCell As PivotGridBaseModelCell = TryCast(e.PivotGridModelCell, PivotGridBaseModelCell)

        If modelDataCell IsNot Nothing Then

            Dim strAs As String = If(modelDataCell.Data Is Nothing, "Nothing", modelDataCell.Data.ToString.Trim())

            If modelDataCell.Field IsNot Nothing Then
                If modelDataCell.Field.Caption = "Contract Date" Then
                    AddStylesToDataCells(modelDataCell, e, "yyyy-MM-dd")
                ElseIf modelDataCell.Field.Caption = "Year" Then
                    AddStylesToDataCells(modelDataCell, e, "0")
                ElseIf modelDataCell.Field.Caption = "Leave Taken" Then
                    AddStylesToDataCells(modelDataCell, e, "0.00")
                ElseIf modelDataCell.Field.Caption = "Employment Days" Then
                    AddStylesToDataCells(modelDataCell, e, "0.00")
                ElseIf modelDataCell.Field.Caption = "Annual Leave Accrued" Then
                    AddStylesToDataCells(modelDataCell, e, "0.00")
                ElseIf modelDataCell.Field.Caption = "Annual Leave Balance" Then
                    AddStylesToDataCells(modelDataCell, e, "0.00")
                Else
                    AddStylesToDataCells(modelDataCell, e, "0.00")
                End If
            Else
                AddStylesToDataCells(modelDataCell, e, "0.00")
            End If

            'If modelDataCell.Field.Caption = "Contract Date" Then
            '    AddStylesToDataCells(modelDataCell, e, "yyyy-MM-dd")
            'Else
            '    AddStylesToDataCells(modelDataCell, e, "0.00")
            'End If

        End If



        If modelDataCell.TableCellType = PivotGridTableCellType.RowHeaderCell Then
            AddStylesToRowHeaderCells(modelDataCell, e)
        End If

        If modelDataCell.TableCellType = PivotGridTableCellType.ColumnHeaderCell Then
            AddStylesToColumnHeaderCells(modelDataCell, e)
        End If

        'If modelDataCell.IsGrandTotalCell Then
        '    e.ExportedCell.Style.BackColor = Color.FromArgb(150, 150, 150)
        '    e.ExportedCell.Style.Font.Bold = True
        'End If




        If modelDataCell.TableCellType = PivotGridTableCellType.DataCell And modelDataCell.CellType = PivotGridDataCellType.DataCell Then
            ' AddStylesToRowHeaderCells(modelDataCell, e)
            Dim cType_ As PivotGridDataCellType = modelDataCell.CellType



        End If




        If IsTotalDataCell(modelDataCell) Then
            e.ExportedCell.Style.BackColor = Color.FromArgb(230, 133, 154)
            e.ExportedCell.Style.Font.Bold = True
            e.ExportedCell.Style.HorizontalAlign = HorizontalAlign.Right
            AddBorders(e)
        End If

        If IsGrandTotalDataCell(modelDataCell) Then

            'If strAs = "Total Sum of Employment_days" Then
            'e.ExportedCell.Style.BackColor = Color.FromArgb(197, 225, 165)
            'ElseIf strAs = "Total Sum of leave_accrued" Then
            '    e.ExportedCell.Style.BackColor = Color.FromArgb(255, 204, 128)
            'ElseIf strAs = "Total Sum of Leave_taken" Then
            '    e.ExportedCell.Style.BackColor = Color.FromArgb(255, 171, 145)
            'ElseIf strAs = "Total Annual Leave Balance" Then
            '    e.ExportedCell.Style.BackColor = Color.FromArgb(128, 222, 234)
            'End If

            If modelDataCell.Field.Caption = "Employment Days" Then
                e.ExportedCell.Style.BackColor = Color.FromArgb(197, 225, 165)
            ElseIf modelDataCell.Field.Caption = "Annual Leave Accrued" Then
                e.ExportedCell.Style.BackColor = Color.FromArgb(255, 204, 128)
            ElseIf modelDataCell.Field.Caption = "Leave Taken" Then
                e.ExportedCell.Style.BackColor = Color.FromArgb(255, 171, 145)
            ElseIf modelDataCell.Field.Caption = "Annual Leave Balance" Then
                e.ExportedCell.Style.BackColor = Color.FromArgb(128, 222, 234)
            Else
                e.ExportedCell.Style.BackColor = Color.FromArgb(240, 240, 240)
            End If

            'e.ExportedCell.Style.BackColor = Color.FromArgb(197, 225, 165)
            e.ExportedCell.Style.Font.Bold = True
            e.ExportedCell.Style.HorizontalAlign = HorizontalAlign.Right
            AddBorders(e)

        End If

    End Sub



    Private Sub AddStylesToDataCells(modelDataCell As PivotGridBaseModelCell, e As PivotGridCellExportingArgs, strFormat As String)

        If modelDataCell.Data IsNot Nothing AndAlso TypeOf (modelDataCell.Data) Is Decimal Then

            Dim value As Decimal = Convert.ToDecimal(modelDataCell.Data)

            If value > 0.1 And value <= 5.0 Then
                e.ExportedCell.Style.BackColor = Color.FromArgb(238, 238, 228)
            ElseIf value > 5.0 And value <= 10.0 Then
                e.ExportedCell.Style.BackColor = Color.FromArgb(255, 255, 163)
            ElseIf value > 10.0 Then
                e.ExportedCell.Style.BackColor = Color.FromArgb(255, 255, 163)
            ElseIf value < 0.00 Then
                e.ExportedCell.Style.BackColor = Color.FromArgb(255, 87, 34)
            Else
                e.ExportedCell.Style.BackColor = Color.FromArgb(240, 240, 240)
            End If

            AddBorders(e)
            e.ExportedCell.Format = strFormat
            e.ExportedCell.Style.HorizontalAlign = HorizontalAlign.Right

        ElseIf modelDataCell.Data IsNot Nothing AndAlso TypeOf (modelDataCell.Data) Is Integer Then

            Dim value As Integer = Convert.ToInt32(modelDataCell.Data)

            If value > 0 And value <= 10 Then
                e.ExportedCell.Style.BackColor = Color.FromArgb(238, 238, 228)
            ElseIf value > 10 And value <= 20 Then
                e.ExportedCell.Style.BackColor = Color.FromArgb(255, 255, 163)
            ElseIf value >= 20 And value <= 31 Then
                e.ExportedCell.Style.BackColor = Color.FromArgb(255, 255, 163)
            Else
                e.ExportedCell.Style.BackColor = Color.FromArgb(240, 240, 240)
            End If

            AddBorders(e)
            e.ExportedCell.Format = strFormat
            e.ExportedCell.Style.HorizontalAlign = HorizontalAlign.Right

        ElseIf modelDataCell.Data IsNot Nothing AndAlso TypeOf (modelDataCell.Data) Is Date Then

            'Dim value As Date = Convert.ToDateTime(modelDataCell.Data)
            e.ExportedCell.Format = strFormat
            e.ExportedCell.Style.HorizontalAlign = HorizontalAlign.Left

        End If

    End Sub


    Private Shared Sub AddBorders(e As PivotGridCellExportingArgs)
        e.ExportedCell.Style.BorderBottomColor = Color.FromArgb(128, 128, 128)
        e.ExportedCell.Style.BorderBottomWidth = New Unit(1)
        e.ExportedCell.Style.BorderBottomStyle = BorderStyle.Solid

        e.ExportedCell.Style.BorderRightColor = Color.FromArgb(128, 128, 128)
        e.ExportedCell.Style.BorderRightWidth = New Unit(1)
        e.ExportedCell.Style.BorderRightStyle = BorderStyle.Solid

        e.ExportedCell.Style.BorderLeftColor = Color.FromArgb(128, 128, 128)
        e.ExportedCell.Style.BorderLeftWidth = New Unit(1)
        e.ExportedCell.Style.BorderLeftStyle = BorderStyle.Solid

        e.ExportedCell.Style.BorderTopColor = Color.FromArgb(128, 128, 128)
        e.ExportedCell.Style.BorderTopWidth = New Unit(1)
        e.ExportedCell.Style.BorderTopStyle = BorderStyle.Solid
    End Sub

    Private Sub AddStylesToColumnHeaderCells(modelDataCell As PivotGridBaseModelCell, e As PivotGridCellExportingArgs)

        If e.ExportedCell.Table.Columns(e.ExportedCell.ColIndex).Width = 0 Then
            e.ExportedCell.Table.Columns(e.ExportedCell.ColIndex).Width = 30.0
        End If

        'If modelDataCell.IsTotalCell Then
        '    e.ExportedCell.Style.BackColor = Color.FromArgb(150, 150, 150)
        '    e.ExportedCell.Style.Font.Bold = True
        'Else
        '    e.ExportedCell.Style.BackColor = Color.FromArgb(192, 192, 192)
        'End If

        If modelDataCell.IsTotalCell Then
            e.ExportedCell.Style.BackColor = Color.FromArgb(150, 150, 150)
            e.ExportedCell.Style.Font.Bold = True
            e.ExportedCell.Style.Font.Size = New FontUnit(16)
            e.ExportedCell.Style.HorizontalAlign = HorizontalAlign.Left
        Else
            e.ExportedCell.Style.BackColor = Color.FromArgb(255, 255, 255)
            e.ExportedCell.Style.HorizontalAlign = HorizontalAlign.Left
        End If


        AddBorders(e)
    End Sub

    Private Sub AddStylesToRowHeaderCells(modelDataCell As PivotGridBaseModelCell, e As PivotGridCellExportingArgs)
        If e.ExportedCell.Table.Columns(e.ExportedCell.ColIndex).Width = 0 Then
            e.ExportedCell.Table.Columns(e.ExportedCell.ColIndex).Width = 30.0
        End If
        If modelDataCell.IsTotalCell Then

            e.ExportedCell.Style.BackColor = Color.FromArgb(150, 150, 150)
            e.ExportedCell.Style.Font.Bold = True
            e.ExportedCell.Style.Font.Size = New FontUnit(16)
            e.ExportedCell.Style.HorizontalAlign = HorizontalAlign.Left

        Else

            e.ExportedCell.Style.BackColor = Color.FromArgb(255, 255, 255)
            e.ExportedCell.Style.HorizontalAlign = HorizontalAlign.Left

        End If

        AddBorders(e)
    End Sub

    Private Function IsTotalDataCell(modelDataCell As PivotGridBaseModelCell) As Boolean
        Return modelDataCell.TableCellType = PivotGridTableCellType.DataCell AndAlso (modelDataCell.CellType = PivotGridDataCellType.ColumnTotalDataCell OrElse modelDataCell.CellType = PivotGridDataCellType.RowTotalDataCell OrElse modelDataCell.CellType = PivotGridDataCellType.RowAndColumnTotal)
    End Function

    Private Function IsGrandTotalDataCell(modelDataCell As PivotGridBaseModelCell) As Boolean
        Return modelDataCell.TableCellType = PivotGridTableCellType.DataCell AndAlso (modelDataCell.CellType = PivotGridDataCellType.ColumnGrandTotalDataCell OrElse modelDataCell.CellType = PivotGridDataCellType.ColumnGrandTotalRowTotal OrElse modelDataCell.CellType = PivotGridDataCellType.RowGrandTotalColumnTotal OrElse modelDataCell.CellType = PivotGridDataCellType.RowGrandTotalDataCell OrElse modelDataCell.CellType = PivotGridDataCellType.RowAndColumnGrandTotal)
    End Function



    Protected Sub export_button_casual_ServerClick(sender As Object, e As EventArgs)

        Try

            'Dim alternateText As String = "Xlsx"
            'Dim IND_code As String = "BHA-08"
            Dim DISSegre As String = "CasualLeave"

            Dim archivo = "Export_" & DISSegre & String.Format("_{0}{1}{2}_{3}{4}_{5}", Date.UtcNow.Year, String.Format("{0:00}", Date.UtcNow.Month), String.Format("{0:00}", Date.UtcNow.Day), String.Format("{0:00}", Date.UtcNow.Hour), String.Format("{0:00}", Date.UtcNow.Minute), String.Format("{0:00}", Date.UtcNow.Second))
            'radgrid_ind_09_firm_values.ExportSettings.Excel.Format = DirectCast([Enum].Parse(GetType(PivotGridExcelFormat), alternateText), PivotGridExcelFormat)


            radgrid_casual_reported.ExportSettings.IgnorePaging = False
            radgrid_casual_reported.ExportSettings.OpenInNewWindow = True
            radgrid_casual_reported.ExportSettings.FileName = archivo
            radgrid_casual_reported.ExportToExcel()


        Catch ex As Exception

            Dim strMess As String = ex.Message

        End Try

    End Sub




    Private Sub radgrid_casual_reported_PivotGridCellExporting(sender As Object, e As PivotGridCellExportingArgs) Handles radgrid_casual_reported.PivotGridCellExporting


        Dim modelDataCell As PivotGridBaseModelCell = TryCast(e.PivotGridModelCell, PivotGridBaseModelCell)

        'modelDataCell.TableCellType.ColumnHeaderCell
        ' Dim strAs As String = modelDataCell.Data.ToString.Trim()

        If modelDataCell IsNot Nothing Then
            AddStylesToDataCells(modelDataCell, e, "0.00")
        End If

        If modelDataCell.TableCellType = PivotGridTableCellType.RowHeaderCell Then
            AddStylesToRowHeaderCells(modelDataCell, e)
        End If

        If modelDataCell.TableCellType = PivotGridTableCellType.ColumnHeaderCell Then
            AddStylesToColumnHeaderCells(modelDataCell, e)
        End If

        'If modelDataCell.IsGrandTotalCell Then
        '    e.ExportedCell.Style.BackColor = Color.FromArgb(150, 150, 150)
        '    e.ExportedCell.Style.Font.Bold = True
        'End If

        If IsTotalDataCell(modelDataCell) Then
            e.ExportedCell.Style.BackColor = Color.FromArgb(230, 133, 154)
            e.ExportedCell.Style.Font.Bold = True
            AddBorders(e)
        End If

        If IsGrandTotalDataCell(modelDataCell) Then

            'If strAs = "Total Sum of Employment_days" Then
            'e.ExportedCell.Style.BackColor = Color.FromArgb(197, 225, 165)
            'ElseIf strAs = "Total Sum of leave_accrued" Then
            '    e.ExportedCell.Style.BackColor = Color.FromArgb(255, 204, 128)
            'ElseIf strAs = "Total Sum of Leave_taken" Then
            '    e.ExportedCell.Style.BackColor = Color.FromArgb(255, 171, 145)
            'ElseIf strAs = "Total Annual Leave Balance" Then
            '    e.ExportedCell.Style.BackColor = Color.FromArgb(128, 222, 234)
            'End If

            e.ExportedCell.Style.BackColor = Color.FromArgb(197, 225, 165)
            e.ExportedCell.Style.Font.Bold = True
            AddBorders(e)

        End If

    End Sub




    Protected Sub export_button_sick_ServerClick(sender As Object, e As EventArgs)

        Try

            'Dim alternateText As String = "Xlsx"
            'Dim IND_code As String = "BHA-08"
            Dim DISSegre As String = "SickLeave"

            Dim archivo = "Export_" & DISSegre & String.Format("_{0}{1}{2}_{3}{4}_{5}", Date.UtcNow.Year, String.Format("{0:00}", Date.UtcNow.Month), String.Format("{0:00}", Date.UtcNow.Day), String.Format("{0:00}", Date.UtcNow.Hour), String.Format("{0:00}", Date.UtcNow.Minute), String.Format("{0:00}", Date.UtcNow.Second))
            'radgrid_ind_09_firm_values.ExportSettings.Excel.Format = DirectCast([Enum].Parse(GetType(PivotGridExcelFormat), alternateText), PivotGridExcelFormat)

            radgrid_sick_reported.ExportSettings.IgnorePaging = False
            radgrid_sick_reported.ExportSettings.OpenInNewWindow = True
            radgrid_sick_reported.ExportSettings.FileName = archivo
            radgrid_sick_reported.ExportToExcel()

        Catch ex As Exception

            Dim strMess As String = ex.Message

        End Try

    End Sub




    Private Sub radgrid_sick_reported_PivotGridCellExporting(sender As Object, e As PivotGridCellExportingArgs) Handles radgrid_sick_reported.PivotGridCellExporting


        Dim modelDataCell As PivotGridBaseModelCell = TryCast(e.PivotGridModelCell, PivotGridBaseModelCell)

        'modelDataCell.TableCellType.ColumnHeaderCell
        ' Dim strAs As String = modelDataCell.Data.ToString.Trim()

        If modelDataCell IsNot Nothing Then
            AddStylesToDataCells(modelDataCell, e, "0.00")
        End If

        If modelDataCell.TableCellType = PivotGridTableCellType.RowHeaderCell Then
            AddStylesToRowHeaderCells(modelDataCell, e)
        End If

        If modelDataCell.TableCellType = PivotGridTableCellType.ColumnHeaderCell Then
            AddStylesToColumnHeaderCells(modelDataCell, e)
        End If

        'If modelDataCell.IsGrandTotalCell Then
        '    e.ExportedCell.Style.BackColor = Color.FromArgb(150, 150, 150)
        '    e.ExportedCell.Style.Font.Bold = True
        'End If

        If IsTotalDataCell(modelDataCell) Then
            e.ExportedCell.Style.BackColor = Color.FromArgb(230, 133, 154)
            e.ExportedCell.Style.Font.Bold = True
            AddBorders(e)
        End If

        If IsGrandTotalDataCell(modelDataCell) Then

            'If strAs = "Total Sum of Employment_days" Then
            'e.ExportedCell.Style.BackColor = Color.FromArgb(197, 225, 165)
            'ElseIf strAs = "Total Sum of leave_accrued" Then
            '    e.ExportedCell.Style.BackColor = Color.FromArgb(255, 204, 128)
            'ElseIf strAs = "Total Sum of Leave_taken" Then
            '    e.ExportedCell.Style.BackColor = Color.FromArgb(255, 171, 145)
            'ElseIf strAs = "Total Annual Leave Balance" Then
            '    e.ExportedCell.Style.BackColor = Color.FromArgb(128, 222, 234)
            'End If

            e.ExportedCell.Style.BackColor = Color.FromArgb(197, 225, 165)
            e.ExportedCell.Style.Font.Bold = True
            AddBorders(e)

        End If

    End Sub


End Class