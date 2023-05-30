
Imports System.Configuration.ConfigurationManager
Imports System.Linq.Expressions
Imports System.Configuration
Imports System.Data.SqlClient
Imports ly_SIME
Imports System.Globalization

Namespace APPROVAL

    Public Class clss_TimeSheet

        Public Property id_programa As Integer
        Public cl_utl As New CORE.cls_util
        Const cAction_ByProcess = 1
        Const cAction_ByMessage = 2
        Dim CNN_ As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

        Public Sub New(ByVal idP)
            id_programa = idP
        End Sub

        Public Function get_t_usuarios(Optional idUSr As Integer = 0) As DataTable

            Using db As New dbRMS_JIEntities

                Dim includUsr = If(idUSr = 0, 1, 0)

                Dim listado = db.vw_t_usuario_programa.Where(Function(p) p.id_programa = id_programa And (p.id_usuario = idUSr Or 1 = includUsr)).Select(Function(p) _
                                                                                                               New With {Key .id_usuario = p.id_usuario,
                                                                                                                                       Key .nombre_usuario = p.nombre_usuario,
                                                                                                                                       Key .job = p.job}).ToList()
                Return ConvertToDataTable(listado)

            End Using

        End Function



        Public Function get_t_usuarios(ByVal idUser As Integer, ByVal Users As Integer()) As DataTable

            Using db As New dbRMS_JIEntities

                Dim listado = db.vw_t_usuario_programa.Where(Function(p) p.id_programa = id_programa And (p.id_usuario = idUser Or Users.Contains(p.id_usuario))).Select(Function(p) _
                                                                                                               New With {Key .id_usuario = p.id_usuario,
                                                                                                                                       Key .nombre_usuario = p.nombre_usuario,
                                                                                                                                       Key .job = p.job}).ToList()
                Return ConvertToDataTable(listado)

            End Using

        End Function



        Public Function get_years() As DataTable

            Using db As New dbRMS_JIEntities

                Dim tbl_Year As New DataTable

                tbl_Year.Columns.Add("id_year", GetType(Integer))
                tbl_Year.Columns.Add("year", GetType(Integer))

                Dim DateStart As DateTime = db.t_programas.FirstOrDefault(Function(p) p.id_programa = id_programa).fecha_inicio
                Dim num_anios As Integer = db.t_programas.FirstOrDefault(Function(p) p.id_programa = id_programa).numero_anios
                Dim anio_start As Integer = DatePart(DateInterval.Year, DateStart)
                Dim i = 0

                For i = anio_start To anio_start + num_anios

                    tbl_Year.Rows.Add(i, i)

                Next


                Return tbl_Year

            End Using

        End Function


        Public Function get_months() As DataTable

            Using db As New dbRMS_JIEntities

                Dim tbl_months As New DataTable

                tbl_months.Columns.Add("id_month", GetType(Integer))
                tbl_months.Columns.Add("month", GetType(String))

                'Dim DateStart As DateTime = db.t_programas.FirstOrDefault(Function(p) p.id_programa = id_programa).fecha_inicio
                'Dim num_anios As Integer = db.t_programas.FirstOrDefault(Function(p) p.id_programa = id_programa).numero_anios
                'Dim DateStart = Date.UtcNow
                'Dim id_month_start As Integer = DatePart(DateInterval.Month, Date.UtcNow)

                Dim mont_start As Integer = 1
                Dim i = 0

                For i = 1 To 12

                    tbl_months.Rows.Add(i, Strings.UCase(MonthName(i)))

                Next


                Return tbl_months

            End Using

        End Function

        Public Function get_Table(ByVal vYear As Integer, ByVal vMonth As Integer, ByVal idTimeSheet As Integer, ByVal idEmployeeType As Integer, ByVal ts_leave As Integer) As DataTable

            Dim tbl_table As New DataTable
            Dim start_Date As Date = DateSerial(vYear, vMonth, 1)

            Dim DaysInMonth As Integer = Date.DaysInMonth(vYear, vMonth)
            Dim LastDayInMonthDate As Date = New Date(vYear, vMonth, DaysInMonth)

            tbl_table.Columns.Add("DATE", GetType(String)) '--First Column
            tbl_table.Columns.Add("billable_time_type", GetType(String))
            tbl_table.Columns.Add("id_billable_time", GetType(Integer))
            tbl_table.Columns.Add("visible", GetType(Byte))
            tbl_table.Columns.Add("id_billable_time_type", GetType(Integer))
            tbl_table.Columns.Add("ts_leave", GetType(Integer))


            '************CHANGE FOR OTHER KIND OF PERIOD***********
            For i = 1 To DaysInMonth 'Adding Columns

                tbl_table.Columns.Add(i.ToString, GetType(String))

            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********

            tbl_table.Columns.Add("Total", GetType(String))


            'First Row' Billable time
            Dim tblRow As DataRow = tbl_table.NewRow()
            tblRow.Item("DATE") = " "
            tblRow.Item("id_billable_time_type") = 1
            tblRow.Item("id_billable_time") = 1
            tblRow.Item("billable_time_type") = "0 -Días pagos"
            tblRow.Item("visible") = 0
            tblRow.Item("ts_leave") = 0

            '************CHANGE FOR OTHER KIND OF PERIOD***********
            Dim DateValue As Date
            For i = 1 To DaysInMonth 'Adding Row
                DateValue = New Date(vYear, vMonth, i)
                tblRow.Item(i + 5) = DateValue.ToString("ddd")
            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********

            tblRow.Item("Total") = " "

            tbl_table.Rows.Add(tblRow)

            'Dim strSQL As String = String.Format("select tab.billable_order, tab.id_billable_time_type, tab.billable_time_type, tab.id_billable_time, tab.billable_time, tab.visible 
            '                                       from
            '                                            (select a.billable_order, a.id_billable_time_type, convert(char(2),a.billable_order) + '-' + a.billable_time_type as billable_time_type, b.id_billable_time, b.billable_time, b.visible 
            '                                                from ta_billable_time_type a
            '                                                inner join ta_billable_time b on (a.id_billable_time_type = b.id_billable_time_type)
            '                                                inner join vw_ta_timesheet_template c on (c.id_billable_time = b.id_billable_time)
            '                                                where c.id_employee_type = {0}
            '                                                UNION
            '                                             select a.billable_order, a.id_billable_time_type, convert(char(2),a.billable_order) + '-' + a.billable_time_type as billable_time_type, b.id_billable_time, b.billable_time, b.visible 
            '                                               from ta_billable_time_type a
            '                                               inner join ta_billable_time b on (a.id_billable_time_type = b.id_billable_time_type)
            '                                               inner join (select distinct a.id_timesheet, b.id_billable_time
            '                                                    from ta_timesheet a
            '                                                  inner join ta_timesheet_detail b on (a.id_timesheet = b.id_timesheet)
            '                                                  where a.id_timesheet = {1}) as c on (b.id_billable_time = c.id_billable_time)) as tab", idEmployeeType, idTimeSheet)


            Dim strSQL As String = String.Format("select tab.billable_order, tab.id_billable_time_type, tab.billable_time_type, tab.id_billable_time, tab.billable_time, tab.visible, tab.ts_leave 
                                                     from
                                                        (select a.billable_order, a.id_billable_time_type, convert(char(2),a.billable_order) + '-' + a.billable_time_type as billable_time_type, b.id_billable_time, b.billable_time, b.visible, b.ts_leave 
                                                            from ta_billable_time_type a
                                                            inner join ta_billable_time b on (a.id_billable_time_type = b.id_billable_time_type)
                                                            inner join vw_ta_timesheet_template c on (c.id_billable_time = b.id_billable_time)
                                                            where c.id_employee_type =  {0}
                                                            UNION
                                                         select a.billable_order, a.id_billable_time_type, convert(char(2),a.billable_order) + '-' + a.billable_time_type as billable_time_type, b.id_billable_time, b.billable_time, b.visible, b.ts_leave 
                                                           from ta_billable_time_type a
                                                           inner join ta_billable_time b on (a.id_billable_time_type = b.id_billable_time_type)
                                                           inner join (select distinct a.id_timesheet, b.id_billable_time
	                                                                     from ta_timesheet a
	                                                                   inner join ta_timesheet_detail b on (a.id_timesheet = b.id_timesheet)
	                                                                   where a.id_timesheet = {1}) as c on (b.id_billable_time = c.id_billable_time)) as tab
	                                                    where tab.ts_leave = 1 or 0 = {2} ", idEmployeeType, idTimeSheet, ts_leave)

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_billable_time_type", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_billable_time_type") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            For Each dt As DataRow In tbl_result.Rows

                tblRow = tbl_table.NewRow()
                tblRow.Item("DATE") = dt("billable_time")
                tblRow.Item("id_billable_time") = dt("id_billable_time")
                tblRow.Item("billable_time_type") = dt("billable_time_type")
                tblRow.Item("id_billable_time_type") = dt("id_billable_time_type")
                tblRow.Item("visible") = dt("visible")
                tblRow.Item("ts_leave") = dt("ts_leave")

                tbl_table.Rows.Add(tblRow)

            Next


            Return tbl_table

        End Function


        Public Function get_Table(ByVal vYear As Integer, ByVal vMonth As Integer, ByVal idTimeSheet As Integer, ByVal idEmployeeType As Integer, ByVal ts_leave As Integer, ByVal AddBillID As List(Of Integer)) As DataTable

            Dim tbl_table As New DataTable
            Dim start_Date As Date = DateSerial(vYear, vMonth, 1)

            Dim DaysInMonth As Integer = Date.DaysInMonth(vYear, vMonth)
            Dim LastDayInMonthDate As Date = New Date(vYear, vMonth, DaysInMonth)

            tbl_table.Columns.Add("DATE", GetType(String)) '--First Column
            tbl_table.Columns.Add("billable_time_type", GetType(String))
            tbl_table.Columns.Add("id_billable_time", GetType(Integer))
            tbl_table.Columns.Add("visible", GetType(Byte))
            tbl_table.Columns.Add("id_billable_time_type", GetType(Integer))
            tbl_table.Columns.Add("ts_leave", GetType(Integer))

            '************CHANGE FOR OTHER KIND OF PERIOD***********
            For i = 1 To DaysInMonth 'Adding Columns

                tbl_table.Columns.Add(i.ToString, GetType(String))

            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********

            tbl_table.Columns.Add("Total", GetType(String))

            'First Row' Billable time
            Dim tblRow As DataRow = tbl_table.NewRow()
            tblRow.Item("DATE") = " "
            tblRow.Item("id_billable_time_type") = 1
            tblRow.Item("id_billable_time") = 1
            tblRow.Item("billable_time_type") = "0 -Días pagos"
            tblRow.Item("visible") = 0
            tblRow.Item("ts_leave") = 0

            Dim DateValue As Date
            '************CHANGE FOR OTHER KIND OF PERIOD***********
            For i = 1 To DaysInMonth 'Adding Row
                DateValue = New Date(vYear, vMonth, i)
                tblRow.Item(i + 5) = DateValue.ToString("ddd")
            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********

            tblRow.Item("Total") = " "

            tbl_table.Rows.Add(tblRow)

            If AddBillID.Count = 0 Then
                AddBillID.Add(0)
            End If
            Dim arrBill = AddBillID.ToArray()

            Dim strSQL As String = String.Format("select tab.billable_order, tab.id_billable_time_type, tab.billable_time_type, tab.id_billable_time, tab.billable_time, tab.visible, tab.ts_leave 
                                                    from
                                                        (select a.billable_order, a.id_billable_time_type, convert(char(2),a.billable_order) + '-' + a.billable_time_type as billable_time_type, b.id_billable_time, b.billable_time, b.visible, b.ts_leave 
                                                        from ta_billable_time_type a
                                                        inner join ta_billable_time b on (a.id_billable_time_type = b.id_billable_time_type)
                                                        inner join vw_ta_timesheet_template c on (c.id_billable_time = b.id_billable_time)
                                                        where c.id_employee_type = {0}
                                                        UNION
                                                        select a.billable_order, a.id_billable_time_type, convert(char(2),a.billable_order) + '-' + a.billable_time_type as billable_time_type, b.id_billable_time, b.billable_time, b.visible, b.ts_leave
                                                        from ta_billable_time_type a
                                                        inner join ta_billable_time b on (a.id_billable_time_type = b.id_billable_time_type)
                                                        inner join (select distinct a.id_timesheet, b.id_billable_time
			                                                          from ta_timesheet a
			                                                         inner join ta_timesheet_detail b on (a.id_timesheet = b.id_timesheet)
			                                                        where a.id_timesheet = {1}) as c on (b.id_billable_time = c.id_billable_time)
                                                        UNION
                                                        select a.billable_order, a.id_billable_time_type, convert(char(2),a.billable_order) + '-' + a.billable_time_type as billable_time_type, b.id_billable_time, b.billable_time, b.visible, b.ts_leave 
                                                        from ta_billable_time_type a
                                                        inner join ta_billable_time b on (a.id_billable_time_type = b.id_billable_time_type)
                                                        where b.id_billable_time in ({2})) as tab  
                                                  where tab.ts_leave = 1 or 0 = {3}", idEmployeeType, idTimeSheet, String.Join(",", arrBill), ts_leave)

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_billable_time_type", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_billable_time_type") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            For Each dt As DataRow In tbl_result.Rows

                tblRow = tbl_table.NewRow()
                tblRow.Item("DATE") = dt("billable_time")
                tblRow.Item("id_billable_time") = dt("id_billable_time")
                tblRow.Item("billable_time_type") = dt("billable_time_type")
                tblRow.Item("id_billable_time_type") = dt("id_billable_time_type")
                tblRow.Item("visible") = dt("visible")
                tblRow.Item("ts_leave") = dt("ts_leave")

                tbl_table.Rows.Add(tblRow)

            Next

            Return tbl_table

        End Function


        Public Shared Function ConvertToDataTable(Of t)(
                                                  ByVal list As IList(Of t)
                                               ) As DataTable
            Dim table As New DataTable()
            If Not list.Any Then
                'don't know schema ....
                Return table
            End If
            Dim fields() = list.First.GetType.GetProperties
            For Each field In fields

                If IsNullableType(field.PropertyType) Then
                    Dim UnderlyingType As Type = Nullable.GetUnderlyingType(field.PropertyType)
                    table.Columns.Add(field.Name, UnderlyingType)
                Else
                    table.Columns.Add(field.Name, field.PropertyType)
                End If


            Next
            For Each item In list
                Dim row As DataRow = table.NewRow()
                For Each field In fields

                    Dim p = item.GetType.GetProperty(field.Name)

                    If (Not IsNothing(p.GetValue(item, Nothing))) Then
                        row(field.Name) = p.GetValue(item, Nothing)
                    Else
                        row(field.Name) = DBNull.Value
                    End If

                    'If (Not p.GetValue(item, Nothing) Is Nothing) AndAlso IsNullableType(p.GetType) Then
                    '    Dim UnderlyingType As Type = Nullable.GetUnderlyingType(p.GetType)
                    '    row(field.Name) = p.GetValue(Convert.ChangeType(item, UnderlyingType), Nothing)
                    'Else
                    '    row(field.Name) = p.GetValue(item, Nothing)
                    'End If

                Next
                table.Rows.Add(row)
            Next
            Return table
        End Function

        Public Shared Function IsNullableType(ByVal myType As Type) As Boolean
            Return (myType.IsGenericType) AndAlso (myType.GetGenericTypeDefinition() Is GetType(Nullable(Of )))
        End Function

        Public Function get_TimeSheetTable(ByVal vYear As Integer, ByVal vMonth As Integer, ByVal idTimeSheet As Integer, ByVal idEmployeeType As Integer, ByVal ts_leave As Integer, Optional culture As CultureInfo = Nothing) As String

            Dim tbl_table As New DataTable
            Dim start_Date As Date = DateSerial(vYear, vMonth, 1)

            Dim DaysInMonth As Integer = Date.DaysInMonth(vYear, vMonth)
            Dim LastDayInMonthDate As Date = New Date(vYear, vMonth, DaysInMonth)

            tbl_table.Columns.Add("DATE", GetType(String)) '--First Column
            tbl_table.Columns.Add("billable_time_type", GetType(String))
            tbl_table.Columns.Add("id_billable_time", GetType(Integer))
            tbl_table.Columns.Add("visible", GetType(Byte))
            tbl_table.Columns.Add("id_billable_time_type", GetType(Integer))
            tbl_table.Columns.Add("ts_leave", GetType(Integer))

            '************CHANGE FOR OTHER KIND OF PERIOD***********
            For i = 1 To DaysInMonth 'Adding Columns

                tbl_table.Columns.Add(i.ToString, GetType(String))

            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********

            tbl_table.Columns.Add("Total", GetType(String))

            'First Row' Billable time
            Dim tblRow As DataRow = tbl_table.NewRow()
            tblRow.Item("DATE") = " "
            tblRow.Item("id_billable_time_type") = 1
            tblRow.Item("id_billable_time") = 1
            tblRow.Item("billable_time_type") = "0 -Días pagos"
            tblRow.Item("visible") = 0
            tblRow.Item("ts_leave") = 0


            '************CHANGE FOR OTHER KIND OF PERIOD***********
            Dim DateValue As Date
            For i = 1 To DaysInMonth 'Adding Row
                DateValue = New Date(vYear, vMonth, i)
                If culture Is Nothing Then
                    tblRow.Item(i + 5) = DateValue
                Else
                    tblRow.Item(i + 5) = DateValue.ToString("ddd", culture)
                End If
            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********

            tblRow.Item("Total") = " "

            tbl_table.Rows.Add(tblRow)

            Dim strSQL As String = String.Format("select tab.billable_order, tab.id_billable_time_type, tab.billable_time_type, tab.id_billable_time, tab.billable_time, tab.visible, tab.ts_leave  
                                                   from
                                                        (select a.billable_order, a.id_billable_time_type, convert(char(2),a.billable_order) + '-' + a.billable_time_type as billable_time_type, b.id_billable_time, b.billable_time, b.visible, b.ts_leave  
                                                            from ta_billable_time_type a
                                                            inner join ta_billable_time b on (a.id_billable_time_type = b.id_billable_time_type)
                                                            inner join vw_ta_timesheet_template c on (c.id_billable_time = b.id_billable_time)
                                                            where c.id_employee_type = {0}
                                                            UNION
                                                         select a.billable_order, a.id_billable_time_type, convert(char(2),a.billable_order) + '-' + a.billable_time_type as billable_time_type, b.id_billable_time, b.billable_time, b.visible, b.ts_leave  
                                                           from ta_billable_time_type a
                                                           inner join ta_billable_time b on (a.id_billable_time_type = b.id_billable_time_type)
                                                           inner join (select distinct a.id_timesheet, b.id_billable_time
			                                                             from ta_timesheet a
			                                                           inner join ta_timesheet_detail b on (a.id_timesheet = b.id_timesheet)
			                                                           where a.id_timesheet = {1}) as c on (b.id_billable_time = c.id_billable_time)) as tab
                                                     where tab.ts_leave = 1 or 0 = {2}  ", idEmployeeType, idTimeSheet, ts_leave)

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_billable_time_type", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_billable_time_type") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            For Each dt As DataRow In tbl_result.Rows

                tblRow = tbl_table.NewRow()
                tblRow.Item("DATE") = dt("billable_time")
                tblRow.Item("id_billable_time") = dt("id_billable_time")
                tblRow.Item("billable_time_type") = dt("billable_time_type")
                tblRow.Item("id_billable_time_type") = dt("id_billable_time_type")
                tblRow.Item("visible") = dt("visible")
                tblRow.Item("ts_leave") = dt("ts_leave")

                tbl_table.Rows.Add(tblRow)

            Next

            '***********************************************************************************
            '*************We have the table, now filling out all the values*********************
            '***********************************************************************************

            Dim registeredTS As DataTable = getTimeSheetDetail(idTimeSheet)
            Dim dia As Integer = 0
            Dim nameCol As String
            'Dim txtName As String
            'Dim txtRadNumeric As RadNumericTextBox

            If Not IsNothing(registeredTS) And registeredTS.Rows.Count > 0 Then

                For Each Drow As DataRow In tbl_table.Rows

                    Dim idBillableTime = Drow("id_billable_time")

                    For Each DVrow In registeredTS.Rows

                        If idBillableTime = DVrow("id_billable_time") Then

                            dia = DVrow("dia")
                            nameCol = "" & dia.ToString & ""
                            Drow(nameCol) = Convert.ToDecimal(DVrow("hours"))

                        End If

                    Next



                Next

            End If


            '***********************************************************************************
            '*************We have the table, now built the HTML5 table**************************
            '***********************************************************************************
            Dim strTable As String = ""
            Dim strTableHeader As String = ""
            Dim strCellHeader As String = "<th class='text-center padding-required ' style='width:3px!important;'>{0}</th>"
            Dim strTableBody As String = ""
            Dim strTableRows As String = ""
            Dim strCellBody As String = " <td {0} class=' {1} {2} padding-required '>{3}</td> "
            Dim strTHCellBody As String = "<th {0} scope='row' class=' {1} {2} padding-required '>{3}</th>"
            Dim strTableFooter As String = ""
            Dim strCellFooter As String = "<th class='{0} text-center padding-required ' style='width:3px!important;'>{1}</th>"

            Dim TOTrows(tbl_table.Rows.Count, DaysInMonth) As Decimal
            Dim rowIndex As Integer = 1
            Dim totRow As Decimal = 0
            Dim totATallRow As Decimal = 0
            Dim totCol As Decimal = 0
            Dim vBillableType As Integer = 0

            strTable = String.Format("<table class=' table-responsive table-hover table-sm table-bordered table-bordered-ts table-with-val'>")

            strTableHeader = String.Format("<thead ><tr class='bg-primary'>")

            strTableHeader &= String.Format("<th colspan='2' class='padding-required' style='width:10%!important;' >DATE</th>")
            '************CHANGE FOR OTHER KIND OF PERIOD***********
            For i = 1 To DaysInMonth 'Adding Columns
                strTableHeader &= String.Format(strCellHeader, i.ToString)
            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********
            strTableHeader &= String.Format(strCellHeader, "")
            strTableHeader &= String.Format("</tr></thead>")

            strTableBody = String.Format("<tbody>")
            Dim vIndex As Integer = 1
            Dim strDay As String
            Dim strSunDayClass As String = "bg-gray"
            Dim strValueRegistered As String = "bg-primary"
            Dim strValueRegistered_leave As String = "bg-warning"

            Dim strValueNOTallowed As String = "bg-red"

            strTableRows = ""
            vBillableType = tbl_table.Rows(0).Item("id_billable_time_type") 'Default Type
            For Each Drow As DataRow In tbl_table.Rows

                If vBillableType <> Drow("id_billable_time_type") Then 'New Billable Type
                    '<th {0} scope='row' class=' {1} {2}'>{3}</th>
                    strTableRows &= String.Format("<tr>" & strTHCellBody, "colspan='" & (DaysInMonth + 2).ToString & "'", "bg-info", "", Strings.Split(Drow("billable_time_type"), "-")(1).ToString) 'Open Row
                    strTableRows &= String.Format(strCellBody & "</tr>", "", "bg-primary", "text-center", "") 'Total Cell
                    vBillableType = Drow("id_billable_time_type")
                End If

                strTableRows &= String.Format("<tr>") 'Open Row
                totRow = 0

                'Dim idBillableTime = Drow("id_billable_time")
                If Drow("visible") = 0 Then
                    strTableRows &= String.Format(strTHCellBody, "colspan='2'", "bg-info", "", Strings.Split(Drow("billable_time_type"), "-")(1).ToString)
                Else
                    'strTableRows &= String.Format(strTHCellBody, "", "", "", vIndex) & String.Format(strCellBody, "", "", "", Drow("DATE"))
                    strTableRows &= String.Format(strCellBody, "colspan='2'", "", "", Drow("DATE"))
                End If

                If Drow("visible") = 0 Then
                    '************Fill Days**********************************
                    '************CHANGE FOR OTHER KIND OF PERIOD************
                    For i = 1 To DaysInMonth 'Adding Columns
                        nameCol = "" & i.ToString & ""
                        strTableRows &= String.Format(strCellBody, "", "bg-primary", "text-center", Left(Drow(nameCol), 3))
                    Next
                    '************CHANGE FOR OTHER KIND OF PERIOD************
                    '************Fill Days**********************************
                    strTableRows &= String.Format(strCellBody, "", "bg-primary", "text-center", "TOTAL") 'Total Cell

                Else

                    '************Fill Hours*********************************
                    '************CHANGE FOR OTHER KIND OF PERIOD************

                    For i = 1 To DaysInMonth 'Adding Columns
                        nameCol = "" & i.ToString & ""
                        strDay = tbl_table.Rows(0).Item(nameCol).ToString.Trim.ToLower.Substring(0, 3)

                        If Not IsDBNull(Drow(nameCol)) Then

                            If Val(Drow(nameCol)) > 8 Then 'Max Time Allowed
                                strTableRows &= String.Format(strCellBody, "", strValueNOTallowed, "text-center", Drow(nameCol))
                            ElseIf Val(Drow(nameCol)) = 0 Then

                                'What about sundays
                                'If strDay.Trim.ToLower = "dom." Or strDay.Trim.ToLower = "sun." Or strDay.Trim.ToLower = "sáb." Or strDay.Trim.ToLower = "sat." Then
                                If strDay = "sáb" Or strDay = "sat" Or strDay = "dom" Or strDay = "sun" Then
                                    strTableRows &= String.Format(strCellBody, "", strSunDayClass, "text-center", "") '
                                Else 'Normal
                                    strTableRows &= String.Format(strCellBody, "", "", "", "")
                                End If

                            Else
                                strTableRows &= String.Format(strCellBody, "", If(Drow("ts_leave") = 0, strValueRegistered, strValueRegistered_leave), "text-center", Drow(nameCol))
                            End If

                        Else
                            'If strDay.Trim.ToLower = "dom." Or strDay.Trim.ToLower = "sun." Or strDay.Trim.ToLower = "sáb." Or strDay.Trim.ToLower = "sat." Then
                            If strDay = "sáb" Or strDay = "sat" Or strDay = "dom" Or strDay = "sun" Then
                                strTableRows &= String.Format(strCellBody, "", strSunDayClass, "text-center", Drow(nameCol)) '
                            Else 'Normal
                                strTableRows &= String.Format(strCellBody, "", "", "text-center", Drow(nameCol)) '
                            End If
                        End If


                        If Not IsDBNull(Drow(nameCol)) Then
                            TOTrows(vIndex, i) = Val(Drow(nameCol))
                            totRow += Val(Drow(nameCol))
                        Else
                            TOTrows(vIndex, i) = 0
                            totRow += 0
                        End If

                    Next
                    '************CHANGE FOR OTHER KIND OF PERIOD************
                    '************Fill Hours*********************************
                    strTableRows &= String.Format(strCellBody, "", "bg-primary", "text-center", If(totRow > 0, totRow.ToString, "")) 'Total Cell
                    totATallRow += totRow
                    vIndex += 1

                End If

                strTableRows &= String.Format("</tr>")
                'vBillableType = tbl_table.Rows(0).Item("id_billable_time_type")

            Next

            strTableBody &= strTableRows.Trim & String.Format("</tbody>")


            strTableFooter = String.Format("<tfoot><tr>")

            strTableFooter &= String.Format("<th colspan='2'></th>")
            '************CHANGE FOR OTHER KIND OF PERIOD***********
            For i = 1 To DaysInMonth 'Adding Columns
                totCol = 0
                For j = 1 To tbl_table.Rows.Count
                    totCol += TOTrows(j, i)
                Next
                strTableFooter &= String.Format(strCellFooter, If(totCol > 8, strValueNOTallowed, "bg-primary"), If(totCol > 0, totCol.ToString, ""))
            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********
            strTableFooter &= String.Format(strCellFooter, "bg-primary", totATallRow)
            strTableFooter &= String.Format("</tr></tfoot>")

            strTable &= String.Format("{0}{1}{2}</table >", strTableHeader.Trim, strTableBody.Trim, strTableFooter.Trim)


            'Dim summaryTIMEsheet As vw_time_sheet_total = get_SummaryTimeSheet(idTimeSheet)

            'If Not IsNothing(summaryTIMEsheet) Then

            '    TOThrs = summaryTIMEsheet.TOThours
            '    TOTloe = summaryTIMEsheet.LOE

            'End If

            'Return "strTable"

            '***********************************************************************************
            '*************We have the table, now built the HTML5 table**************************
            '***********************************************************************************


            Return strTable


        End Function



        Public Function get_TimeSheetTableHTML(ByVal vYear As Integer, ByVal vMonth As Integer, ByVal idTimeSheet As Integer, ByVal idEmployeeType As Integer, ByVal ts_leave As Integer) As String

            Dim tbl_table As New DataTable
            Dim start_Date As Date = DateSerial(vYear, vMonth, 1)

            Dim DaysInMonth As Integer = Date.DaysInMonth(vYear, vMonth)
            Dim LastDayInMonthDate As Date = New Date(vYear, vMonth, DaysInMonth)

            tbl_table.Columns.Add("DATE", GetType(String)) '--First Column
            tbl_table.Columns.Add("billable_time_type", GetType(String))
            tbl_table.Columns.Add("id_billable_time", GetType(Integer))
            tbl_table.Columns.Add("visible", GetType(Byte))
            tbl_table.Columns.Add("id_billable_time_type", GetType(Integer))
            tbl_table.Columns.Add("ts_leave", GetType(Integer))

            '************CHANGE FOR OTHER KIND OF PERIOD***********
            For i = 1 To DaysInMonth 'Adding Columns

                tbl_table.Columns.Add(i.ToString, GetType(String))

            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********

            tbl_table.Columns.Add("Total", GetType(String))

            'First Row' Billable time
            Dim tblRow As DataRow = tbl_table.NewRow()
            tblRow.Item("DATE") = " "
            tblRow.Item("id_billable_time_type") = 1
            tblRow.Item("id_billable_time") = 1
            tblRow.Item("billable_time_type") = "0 -Días pagos"
            tblRow.Item("visible") = 0
            tblRow.Item("ts_leave") = 0

            '************CHANGE FOR OTHER KIND OF PERIOD***********
            Dim DateValue As Date
            For i = 1 To DaysInMonth 'Adding Row
                DateValue = New Date(vYear, vMonth, i)
                tblRow.Item(i + 5) = DateValue.ToString("ddd")
            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********

            tblRow.Item("Total") = " "

            tbl_table.Rows.Add(tblRow)

            Dim strSQL As String = String.Format("select tab.billable_order, tab.id_billable_time_type, tab.billable_time_type, tab.id_billable_time, tab.billable_time, tab.visible, tab.ts_leave 
                                                   from
                                                        (select a.billable_order, a.id_billable_time_type, convert(char(2),a.billable_order) + '-' + a.billable_time_type as billable_time_type, b.id_billable_time, b.billable_time, b.visible, b.ts_leave 
                                                            from ta_billable_time_type a
                                                            inner join ta_billable_time b on (a.id_billable_time_type = b.id_billable_time_type)
                                                            inner join vw_ta_timesheet_template c on (c.id_billable_time = b.id_billable_time)
                                                            where c.id_employee_type = {0}
                                                            UNION
                                                         select a.billable_order, a.id_billable_time_type, convert(char(2),a.billable_order) + '-' + a.billable_time_type as billable_time_type, b.id_billable_time, b.billable_time, b.visible, b.ts_leave 
                                                           from ta_billable_time_type a
                                                           inner join ta_billable_time b on (a.id_billable_time_type = b.id_billable_time_type)
                                                           inner join (select distinct a.id_timesheet, b.id_billable_time
			                                                             from ta_timesheet a
			                                                           inner join ta_timesheet_detail b on (a.id_timesheet = b.id_timesheet)
			                                                           where a.id_timesheet = {1}) as c on (b.id_billable_time = c.id_billable_time)) as tab
                                                    where tab.ts_leave = 1 or 0 = {2}  ", idEmployeeType, idTimeSheet, ts_leave)

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_billable_time_type", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_billable_time_type") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            For Each dt As DataRow In tbl_result.Rows

                tblRow = tbl_table.NewRow()
                tblRow.Item("DATE") = dt("billable_time")
                tblRow.Item("id_billable_time") = dt("id_billable_time")
                tblRow.Item("billable_time_type") = dt("billable_time_type")
                tblRow.Item("id_billable_time_type") = dt("id_billable_time_type")
                tblRow.Item("visible") = dt("visible")
                tblRow.Item("ts_leave") = dt("ts_leave")

                tbl_table.Rows.Add(tblRow)

            Next

            '***********************************************************************************
            '*************We have the table, now filling out all the values*********************
            '***********************************************************************************

            Dim registeredTS As DataTable = getTimeSheetDetail(idTimeSheet)
            Dim dia As Integer = 0
            Dim nameCol As String
            'Dim txtName As String
            'Dim txtRadNumeric As RadNumericTextBox

            If Not IsNothing(registeredTS) And registeredTS.Rows.Count > 0 Then

                For Each Drow As DataRow In tbl_table.Rows

                    Dim idBillableTime = Drow("id_billable_time")

                    For Each DVrow In registeredTS.Rows

                        If idBillableTime = DVrow("id_billable_time") Then

                            dia = DVrow("dia")
                            nameCol = "" & dia.ToString & ""
                            Drow(nameCol) = Convert.ToDecimal(DVrow("hours"))

                        End If

                    Next



                Next

            End If


            '***********************************************************************************
            '*************We have the table, now built the HTML5 table**************************
            '***********************************************************************************
            Dim strTable As String = ""
            Dim strTableHeader As String = ""
            Dim strCellHeader As String = "<th style='text-align:left;'>{0}</th>"
            Dim strTableBody As String = ""
            Dim strTableRows As String = ""
            Dim strCellBody As String = " <td {0} style='{1}{2}'>{3}</td> "
            Dim strTHCellBody As String = "<th {0} scope='row' style='{1}{2}'>{3}</th>"
            Dim strTableFooter As String = ""
            Dim strCellFooter As String = "<th style='{0}{2}'>{1}</th>"


            Dim cssText_Center As String = "text-align:center;"
            Dim cssText_Left As String = "text-align:left;"
            Dim cssBG_Primary As String = "color: #fff;background-color: #337ab7;"
            Dim cssBG_Gray As String = "color: #000;background-color: #d2d6de;"
            Dim cssBG_Red As String = "color: #000; background-color: #dd4b39 !important;"
            Dim cssBG_warning As String = "color: #000; background-color: #ffc300 !important;"
            Dim cssBG_Info As String = "background-color: #d9edf7;"
            Dim cssTable As String = "background-color: transparent; border: 1px solid #ddd;"
            Dim cssTable_Responsive As String = "min-height: .01%;overflow-x: auto;"
            Dim cssBoldFont As String = "font-weight: 500;font-family: Arial, Helvetica, sans-serif;font-size: small;"
            Dim cssNormalFont As String = "font-family:Verdana, Arial; font-size: x-small;"

            Dim TOTrows(tbl_table.Rows.Count, DaysInMonth) As Decimal
            Dim rowIndex As Integer = 1
            Dim totRow As Decimal = 0
            Dim totATallRow As Decimal = 0
            Dim totCol As Decimal = 0
            Dim vBillableType As Integer = 0

            strTable = String.Format("<table style='background-color: transparent; border: 1px solid #ddd;min-height: .01%;overflow-x: auto;font-family:Verdana, Arial; font-size: x-small;text-align:left; padding:2px 2px 2px 2px;' >")

            strTableHeader = String.Format("<thead ><tr style='color: #fff;background-color: #337ab7;'>")

            strTableHeader &= String.Format("<th colspan='2' style='{0}'>Fecha</th>", cssBoldFont)
            '************CHANGE FOR OTHER KIND OF PERIOD***********
            For i = 1 To DaysInMonth 'Adding Columns
                strTableHeader &= String.Format(strCellHeader, i.ToString)
            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********
            strTableHeader &= String.Format(strCellHeader, "")
            strTableHeader &= String.Format("</tr></thead>")

            strTableBody = String.Format("<tbody>")
            Dim vIndex As Integer = 1
            Dim strDay As String
            'Dim strSunDayClass As String = "bg-gray"
            'Dim strValueRegistered As String = "bg-primary"
            'Dim strValueNOTallowed As String = "bg-red"

            strTableRows = ""
            vBillableType = tbl_table.Rows(0).Item("id_billable_time_type") 'Default Type
            For Each Drow As DataRow In tbl_table.Rows

                If vBillableType <> Drow("id_billable_time_type") Then 'New Billable Type
                    '<th {0} scope='row' style='{1} {2}'>{3}</th>
                    strTableRows &= String.Format("<tr>" & strTHCellBody, "colspan='" & (DaysInMonth + 2).ToString & "'", cssBG_Info, cssText_Left, Strings.Split(Drow("billable_time_type"), "-")(1).ToString) 'Open Row
                    strTableRows &= String.Format(strCellBody & "</tr>", "", cssBG_Primary, cssText_Center, "") 'Total Cell
                    vBillableType = Drow("id_billable_time_type")
                End If

                strTableRows &= String.Format("<tr>") 'Open Row
                totRow = 0

                'Dim idBillableTime = Drow("id_billable_time")
                If Drow("visible") = 0 Then
                    strTableRows &= String.Format(strTHCellBody, "colspan='2'", cssBG_Info, cssText_Left, Strings.Split(Drow("billable_time_type"), "-")(1).ToString)
                Else
                    'strTableRows &= String.Format(strTHCellBody, "", "", "", vIndex) & String.Format(strCellBody, "", "", "", Drow("DATE"))
                    strTableRows &= String.Format(strCellBody, "colspan='2'", cssText_Left, "", Drow("DATE"))
                End If

                If Drow("visible") = 0 Then
                    '************Fill Days**********************************
                    '************CHANGE FOR OTHER KIND OF PERIOD************
                    For i = 1 To DaysInMonth 'Adding Columns
                        nameCol = "" & i.ToString & ""
                        strTableRows &= String.Format(strCellBody, "", cssBG_Primary, cssText_Center, Drow(nameCol))
                    Next
                    '************CHANGE FOR OTHER KIND OF PERIOD************
                    '************Fill Days**********************************
                    strTableRows &= String.Format(strCellBody, "", cssBG_Primary, cssText_Center, "TOTAL") 'Total Cell

                Else

                    '************Fill Hours*********************************
                    '************CHANGE FOR OTHER KIND OF PERIOD************

                    For i = 1 To DaysInMonth 'Adding Columns
                        nameCol = "" & i.ToString & ""
                        strDay = tbl_table.Rows(0).Item(nameCol).ToString.Trim.ToLower.Substring(0, 3)

                        If Not IsDBNull(Drow(nameCol)) Then

                            If Val(Drow(nameCol)) > 8 Then 'Max Time Allowed
                                strTableRows &= String.Format(strCellBody, "", cssBG_Red, cssText_Center, Drow(nameCol))
                            ElseIf Val(Drow(nameCol)) = 0 Then
                                'What about sundays
                                If strDay = "sáb" Or strDay = "sat" Or strDay = "dom" Or strDay = "sun" Then
                                    strTableRows &= String.Format(strCellBody, "", cssBG_Gray, cssText_Center, "") '
                                Else 'Normal
                                    strTableRows &= String.Format(strCellBody, "", "", "", "")
                                End If
                            Else
                                strTableRows &= String.Format(strCellBody, "", If(Drow("ts_leave") = 0, cssBG_Primary, cssBG_warning), cssText_Center, Drow(nameCol))
                            End If

                        Else
                            If strDay = "sáb" Or strDay = "sat" Or strDay = "dom" Or strDay = "sun" Then
                                strTableRows &= String.Format(strCellBody, "", cssBG_Gray, cssText_Center, Drow(nameCol)) '
                            Else 'Normal
                                strTableRows &= String.Format(strCellBody, "", "", cssText_Center, Drow(nameCol)) '
                            End If
                        End If


                        If Not IsDBNull(Drow(nameCol)) Then
                            TOTrows(vIndex, i) = Val(Drow(nameCol))
                            totRow += Val(Drow(nameCol))
                        Else
                            TOTrows(vIndex, i) = 0
                            totRow += 0
                        End If

                    Next
                    '************CHANGE FOR OTHER KIND OF PERIOD************
                    '************Fill Hours*********************************
                    strTableRows &= String.Format(strCellBody, "", cssBG_Primary, cssText_Center, If(totRow > 0, totRow.ToString, "")) 'Total Cell
                    totATallRow += totRow
                    vIndex += 1

                End If

                strTableRows &= String.Format("</tr>")
                'vBillableType = tbl_table.Rows(0).Item("id_billable_time_type")

            Next

            strTableBody &= strTableRows.Trim & String.Format("</tbody>")


            strTableFooter = String.Format("<tfoot><tr>")

            strTableFooter &= String.Format("<th colspan='2'></th>")
            '************CHANGE FOR OTHER KIND OF PERIOD***********
            For i = 1 To DaysInMonth 'Adding Columns
                totCol = 0
                For j = 1 To tbl_table.Rows.Count
                    totCol += TOTrows(j, i)
                Next
                strTableFooter &= String.Format(strCellFooter, If(totCol > 8, cssBG_Red, cssBG_Primary), If(totCol > 0, totCol.ToString, ""), cssText_Center)
            Next
            '************CHANGE FOR OTHER KIND OF PERIOD***********
            strTableFooter &= String.Format(strCellFooter, cssBG_Primary, totATallRow, cssText_Center)
            strTableFooter &= String.Format("</tr></tfoot>")

            strTable &= String.Format("{0}{1}{2}</table >", strTableHeader.Trim, strTableBody.Trim, strTableFooter.Trim)


            'Dim summaryTIMEsheet As vw_time_sheet_total = get_SummaryTimeSheet(idTimeSheet)

            'If Not IsNothing(summaryTIMEsheet) Then

            '    TOThrs = summaryTIMEsheet.TOThours
            '    TOTloe = summaryTIMEsheet.LOE

            'End If

            'Return "strTable"

            '***********************************************************************************
            '*************We have the table, now built the HTML5 table**************************
            '***********************************************************************************


            Return strTable

        End Function



        Public Function annual_leave_table(ByVal idUser As Integer) As String

            Dim ds As New DataSet
            Dim adapter As SqlDataAdapter

            CNN_.Open()

            Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("SP_HR_BILLABLE_TIME", CNN_)
            cmd.CommandType = CommandType.StoredProcedure
            'cmd.Parameters.Add("@tp_view", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            cmd.Parameters.AddWithValue("@tp_view", 4)
            cmd.Parameters.AddWithValue("@_id_usuario", idUser)
            cmd.Parameters.AddWithValue("@_anio", 0)

            adapter = New SqlDataAdapter(cmd)
            adapter.Fill(ds)

            '***********************************************************************************
            '*************We have the table, now built the HTML5 table**************************
            '***********************************************************************************
            Dim strTable As String = ""
            Dim strTableHeader As String = ""
            Dim strTableHeader_line1 As String = ""
            Dim strTableHeader_line2 As String = ""

            Dim strCellHeader As String = "<th style='text-align:left;{1}'>{0}</th>"
            Dim strTableBody As String = ""
            Dim strTableRows As String = ""
            Dim strCellBody As String = " <td {0} style='{1}{2}'>{3}</td> "
            Dim strTHCellBody As String = "<th {0} scope='row' style='{1}{2}'>{3}</th>"
            Dim strTableFooter As String = ""
            Dim strCellFooter As String = "<th style='{0}{2}' {3} >{1}</th>"

            Dim cssText_Center As String = "text-align:center;"
            Dim cssText_Left As String = "text-align:left;"
            Dim cssBG_Primary As String = "color: #fff;background-color: #337ab7;"
            Dim cssBG_Gray As String = "color: #000;background-color: #d2d6de;"
            Dim cssBG_Red As String = "color: #000; background-color: #dd4b39 !important;"
            Dim cssBG_warning As String = "color: #000; background-color: #ffc300 !important;"
            Dim cssBG_Info As String = "background-color: #d9edf7;"
            Dim cssTable As String = "background-color: transparent; border: 1px solid #ddd;"
            Dim cssTable_Responsive As String = "min-height: .01%;overflow-x: auto;"
            Dim cssBoldFont As String = "font-weight: 500;font-family: Arial, Helvetica, sans-serif;font-size: small;"
            Dim cssNormalFont As String = "font-family:Verdana, Arial; font-size: x-small;"
            Dim BadgeStyle As String = "display: inline-block; min-width: 10px; padding: 3px 7px; font-size: 12px; font-weight: bold; line-height: 1; color: #fff;  text-align: center;  white-space: nowrap;   vertical-align: middle; background-color: #777; border-radius: 10px;"



            If Not IsNothing(ds.Tables(0)) Then


                strTable = String.Format("<table cellpadding='4px' cellspacing='4px'  style='background-color: transparent; border: 1px solid #ddd;min-height: .01%;overflow-x: auto;font-family:Verdana, Arial; font-size: x-small;text-align:left; padding:2px 2px 2px 2px;' >")
                strTableHeader = String.Format("<thead>")


                strTableHeader_line2 = String.Format("<tr style ='color: #fff;background-color: #00796B;'>")

                '            strTableHeader &= String.Format("<th colspan='2' style='{0}'>DATE</th>", cssBoldFont)

                'Dim str_tbl_OPEN As String = "<table class='table table-striped'>"
                'Dim str_tbl_CLOSE As String = "</table>"

                'Dim str_tbl_HEADER As String = "<tr>"
                'Dim str_tbl_row1 As String = "<tr>"
                'Dim str_tbl_row2 As String = "<tr><td colspan='{0}'>{1}</td></tr>"

                Dim cols As Integer = 0
                Dim colsTot As Integer = ds.Tables(0).Columns.Count - 1

                'str_TABLE &= str_tbl_OPEN

                'For j = 0 To ds.Tables(0).Columns.Count - 1

                '    If j = 5 Then
                '        str_tbl_HEADER &= String.Format("<th>{0}</th>", "YEAR")
                '        cols += 1
                '    ElseIf j = 7 Then
                '        str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper) ' ds.Tables(0).Columns(j).ColumnName.ToString
                '        cols += 1
                '    ElseIf j = 8 Then
                '        'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                '        str_tbl_HEADER &= String.Format("<th>{0}</th>", "ANNUAL_LEAVE")
                '        cols += 1
                '    ElseIf j > 8 And j <= (colsTot - 2) Then
                '        str_tbl_HEADER &= String.Format("<th>{0}</th>", Replace(ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper, "ANNUAL_LEAVE", "AL"))
                '        cols += 1
                '        'ElseIf j > (colsTot - 2) Then
                '        '    str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                '        '    cols += 1
                '    ElseIf j = (colsTot - 1) Then
                '        'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                '        str_tbl_HEADER &= String.Format("<th>{0}</th>", "AL_TAKEN")
                '        cols += 1
                '    ElseIf j = (colsTot) Then
                '        'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                '        str_tbl_HEADER &= String.Format("<th>{0}</th>", "AL_BALANCE")
                '        cols += 1
                '    End If

                'Next

                strTableHeader_line2 &= String.Format("<th colspan='2' style='{0}'>YEAR</th>", cssBoldFont)
                strTableHeader_line2 &= String.Format("<th style='{0}'>WORKED DAYS</th>", cssBoldFont)
                strTableHeader_line2 &= String.Format("<th style='{0}'>ANNUAL LEAVE ACCRUED</th>", cssBoldFont)

                cols = 2

                For j = 9 To ds.Tables(0).Columns.Count - 3

                    strTableHeader_line2 &= String.Format(strCellHeader, "&nbsp;&nbsp;" & Right(ds.Tables(0).Columns(j).ColumnName.ToString.Trim.ToUpper, 3) & "&nbsp;&nbsp;", cssBoldFont)
                    cols += 1

                Next

                strTableHeader_line2 &= String.Format("<th style='{0}'>LEAVE DAYS TAKEN</th>", cssBoldFont)
                strTableHeader_line2 &= String.Format("<th style='{0}'>ANNUAL LEAVE BALANCE</th>", cssBoldFont)
                strTableHeader_line2 &= String.Format(strCellHeader, "", "")

                cols += 3


                strTableHeader_line2 &= "</tr>"


                strTableHeader_line1 = String.Format("<tr style ='color: #fff;background-color: #00796B;'>")
                strTableHeader_line1 &= String.Format("<th colspan='{0}' style='{1}'>ANNUAL LEAVE: {2}{4}{4}{4}{4}{4}{4}{4}{4}EMPLOYMENT DATE: {3:MM/dd/yyyy}</th>", (cols + 2).ToString, cssBoldFont & cssText_Center, String.Format("{0} {1}", ds.Tables(0).Rows.Item(0).Item("nombre_usuario"), ds.Tables(0).Rows.Item(0).Item("apellidos_usuario")), ds.Tables(0).Rows.Item(0).Item("contract_date"), "&nbsp;")
                strTableHeader_line1 &= String.Format("</tr>")

                strTableHeader &= strTableHeader_line1 & strTableHeader_line2
                strTableHeader &= String.Format("</thead>")



                strTableBody = String.Format("<tbody>")
                strTableRows = ""

                'str_tbl_HEADER &= String.Format("<th></th>")
                'cols += 1
                'str_tbl_HEADER &= "</tr>"
                'str_TABLE &= str_tbl_HEADER

                'str_tbl_row1 = ""
                Dim k As Integer = 0
                Dim percentVAC As Decimal = 0
                Dim TotWorked As Decimal = 0
                Dim TotYearly As Decimal = 0
                Dim TotVACATION As Decimal = 0
                Dim TotPEND As Decimal = 0

                For Each dtRow In ds.Tables(0).Rows

                    If dtRow("worked_days") > 0 Then

                        'str_tbl_row1 &= "<th style ='border:1px solid lightgray;' >" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</th>"

                        'strTableRows &= String.Format("<tr>" & strTHCellBody, "colspan='" & (DaysInMonth + 2).ToString & "'", cssBG_Info, cssText_Left, Strings.Split(Drow("billable_time_type"), "-")(1).ToString) 'Open Row
                        strTableRows &= String.Format("<tr>" & strTHCellBody, "colspan='2' rowspan='2'", "border:1px solid lightgray;", "", dtRow("anio")) 'Open Row
                        strTableRows &= String.Format(strCellBody, "", cssText_Left, "", dtRow("worked_days"))
                        TotWorked += dtRow("worked_days")
                        strTableRows &= String.Format(strCellBody, "", cssText_Left, "", dtRow("VACATION_YEARLY"))
                        TotYearly += dtRow("VACATION_YEARLY")

                        'str_tbl_row1 &= "<tr>"
                        For k = 9 To ds.Tables(0).Columns.Count - 3

                            'If k = 5 Then
                            '    str_tbl_row1 &= "<th style ='border:1px solid lightgray;' >" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</th>"
                            'ElseIf k = 7 Then
                            '    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                            '    TotWorked += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                            'ElseIf k = 8 Then
                            '    TotYearly += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                            '    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                            'ElseIf k > 8 And k <= (colsTot - 2) Then

                            '    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                            'ElseIf k = (colsTot - 1) Then
                            '    TotVACATION += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                            '    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                            'ElseIf k = (colsTot) Then
                            '    TotPEND += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                            '    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                            'End If

                            strTableRows &= String.Format(strCellBody, If(dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) > 0, cssBoldFont, ""), cssText_Left, "", dtRow(ds.Tables(0).Columns(k).ColumnName.ToString))

                        Next

                        strTableRows &= String.Format(strTHCellBody, "", "", "", dtRow("VACATION_TOT"))
                        TotVACATION += dtRow("VACATION_TOT")
                        strTableRows &= String.Format(strTHCellBody, "", "", "", dtRow("VACATION_PEND"))
                        TotPEND += dtRow("VACATION_PEND")

                        If dtRow("VACATION_YEARLY") = 0 Then
                            percentVAC = 0.0
                        Else
                            percentVAC = Math.Round(dtRow("VACATION_TOT") / dtRow("VACATION_YEARLY"), 2) * 100
                        End If

                        strTableRows &= String.Format(strCellBody, "", "", "", String.Format("<span style='{0}'> {1}% </span>", BadgeStyle & get_Tag_HTML(1, percentVAC), percentVAC.ToString))
                        strTableRows &= "</tr>"

                        'str_tbl_row1 &= String.Format("<td><span class='badge {0}'> {1}% </span></td>", get_Tag(1, percentVAC), percentVAC.ToString)
                        'str_tbl_row1 &= "</tr>"

                        '***********************************************************************************************
                        'strTableRows &= String.Format("<tr>" & strCellBody, "colspan='2'", "", "", "ANNUAL LEAVE %")
                        'Dim strBar As String = String.Format("<div style='-webkit-box-shadow: none;box-shadow: none;border-radius: 1px; height: 7px;'>
                        '                  <div style='border-radius: 1px; height: 7px;{0};width:{1}%'></div>
                        '               </div>", get_Tag_HTML(2, percentVAC), percentVAC)
                        'strTableRows &= String.Format(strCellBody, "colspan='" & (cols).ToString & "'", "", "", strBar)
                        'strTableRows &= "</tr>"
                        '***********************************************************************************************

                        Dim strBar As String = " <tr>
                                                <td colspan='{0}' style='-webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt;'>
                                                    <div style='height: 20px; margin-bottom: 20px; overflow: hidden; background-color: #f5f5f5; -webkit-box-shadow: none; box-shadow: none; border-radius: 1px; margin-top: 5px; margin: 0;'>
                                                        <div style='float: left; height: 100%; font-size: 12px; line-height: 20px; color: #fff; text-align: center; -webkit-transition: width .6s ease; -o-transition: width .6s ease; transition: width .6s ease; background-color: {1}; -webkit-box-shadow: none; box-shadow: none; border-radius: 1px; width: {2}%;'>
                                                            <span>{2}%</span>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>"


                        strTableRows &= String.Format(strBar, (cols).ToString, get_alert_ind(percentVAC), percentVAC.ToString)


                        'str_tbl_row1 &= String.Format("<tr><td colspan='1' >AL %</td><td colspan='{0}'>
                        '                         <div class='progress progress-xs'>
                        '                              <div class='progress-bar {1}' style='width:{2}%'></div>
                        '                          </div>
                        '                        </td></tr>", cols - 1, get_Tag(2, percentVAC), percentVAC)

                    End If ' If dtRow("worked_days") > 0 Then

                Next

                'str_TABLE &= str_tbl_row1


                strTableFooter = String.Format("<tfoot><tr style='border-bottom:1px solid lightgray;'>")
                'str_tbl_row2 = " <tr style='border-bottom:1px solid lightgray;'> "

                strTableFooter &= String.Format("<th colspan='2'></th>")

                'str_tbl_row2 &= "<th></th>"
                'str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotWorked.ToString() & "</th>"

                'If(TotWorked > 360, cssBG_Red, cssBG_Primary)
                strTableFooter &= String.Format(strCellFooter, "border-top:1px solid lightgray;", If(TotWorked > 0, TotWorked.ToString, ""), cssText_Left, "")
                strTableFooter &= String.Format(strCellFooter, "border-top:1px solid lightgray;", If(TotYearly > 0, TotYearly.ToString, ""), cssText_Left, "")
                strTableFooter &= String.Format(strCellFooter, "", "", cssText_Center, "colspan='" & (cols - 5).ToString & "'")

                'str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotYearly.ToString() & "</th><th colspan=" & (cols - 6) & " ></th>"


                strTableFooter &= String.Format(strCellFooter, "border-top:1px solid lightgray;", TotVACATION.ToString, cssText_Left, "")


                'str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotVACATION.ToString() & "</th>"

                strTableFooter &= String.Format(strCellFooter, "border-top:1px solid lightgray;", TotPEND.ToString, cssText_Left, "")



                'str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotPEND.ToString() & "</th>"
                Dim percentTOT As Decimal = 0
                If TotYearly = 0 Then
                    percentTOT = 100
                Else
                    percentTOT = (1 - Math.Round((TotPEND / TotYearly), 2)) * 100
                End If

                strTableFooter &= String.Format(strCellFooter, "border-top:1px solid lightgray;", String.Format("<span style='{0}'> {1}% </span>", BadgeStyle & get_Tag_HTML(1, percentTOT), percentTOT.ToString), cssText_Center, "")

                'str_tbl_row2 &= String.Format("<th style='border-top:1px solid lightgray;' ><span class='badge {0}'> {1} </span></th>", get_Tag(1, percentTOT), TotPEND)
                'str_tbl_row2 &= "<th></th></tr>"
                strTableFooter &= String.Format("</tr></tfoot>")

                strTableBody &= strTableRows.Trim & String.Format("</tbody>")

                strTable &= String.Format("{0}{1}{2}</table >", strTableHeader.Trim, strTableBody.Trim, strTableFooter.Trim)

                'str_TABLE &= str_tbl_row2
                'str_TABLE &= str_tbl_CLOSE

            Else
                strTable = ""
            End If


            CNN_.Close()


            annual_leave_table = strTable

        End Function



        Public Function get_Tag_HTML(ByVal idT As Integer, ByVal PercentV As Decimal) As String

            Dim strOutPut_bar As String = ""
            Dim strOutPut_color As String = ""

            If PercentV >= 0 And PercentV <= 25 Then
                strOutPut_bar = "background-color: #f39c12;"
                strOutPut_color = "background-color: #f39c12 !important;"
            ElseIf PercentV > 25 And PercentV <= 50 Then
                strOutPut_bar = "background-color: #3c8dbc;"
                strOutPut_color = "background-color: #3c8dbc !important;"
            ElseIf PercentV > 50 And PercentV <= 75 Then
                strOutPut_bar = "background-color: #00a65a;"
                strOutPut_color = "background-color: #00a65a !important;"
            Else
                strOutPut_bar = "background-color: #dd4b39;"
                strOutPut_color = "background-color: #dd4b39 !important;"
            End If

            If idT = 1 Then
                get_Tag_HTML = strOutPut_bar
            Else
                get_Tag_HTML = strOutPut_color
            End If

        End Function


        Public Function get_alert_ind(ByVal vValue As Double) As String

            '.progress-bar - yellow,
            '.progress - bar - warning {
            '    background-color:   #f39c12;
            '}
            ' background-color: #f39c12 !important;

            '.progress-bar - light - blue,
            '.progress - bar - primary {
            '    background-color:   #3c8dbc;
            '}

            '.progress-bar - green,
            '.progress - bar - success {
            '    background-color:   #00a65a;
            '}

            Dim strAlert_color As String = "#d2d6de"

            If (vValue >= 0 And vValue <= 25) Then
                strAlert_color = " #f39c12"
            ElseIf (vValue > 25 And vValue <= 50) Then
                strAlert_color = "#3c8dbc"
            ElseIf (vValue > 50 And vValue <= 75) Then
                strAlert_color = "#00a65a"
            Else '(vValue > 66.66 ) Then
                strAlert_color = "#dd4b39"
            End If

            'If (vValue >= 0 And vValue <= 33.33) Then
            '    strAlert_color = " #f39c12"
            'ElseIf (vValue > 33.33 And vValue <= 66.66) Then
            '    strAlert_color = "#3c8dbc"
            'Else '(vValue > 66.66 ) Then
            '    strAlert_color = "#00a65a"
            'End If

            get_alert_ind = strAlert_color

        End Function


        Public Function getTimeSheet(ByVal idTimeSheet As Integer) As vw_ta_timesheet

            Try

                Using db As New dbRMS_JIEntities

                    getTimeSheet = db.vw_ta_timesheet.Where(Function(p) p.id_timesheet = idTimeSheet).FirstOrDefault()

                End Using
            Catch ex As Exception

                getTimeSheet = Nothing

            End Try

        End Function

        Public Function getTimeSheetPending(ByVal idUsuario As Integer) As Integer

            Try

                Using db As New dbRMS_JIEntities

                    getTimeSheetPending = db.vw_ta_timesheet.Where(Function(p) (p.id_timesheet_estado = 1 Or p.id_timesheet_estado = 2 Or p.id_timesheet_estado = 5) And p.id_usuario = idUsuario).Count()

                End Using
            Catch ex As Exception

                getTimeSheetPending = 0

            End Try

        End Function


        Public Function getTimeSheet_period(ByVal idMonth As Integer, ByVal idYear As Integer) As List(Of vw_ta_timesheet)

            Try

                Using db As New dbRMS_JIEntities

                    getTimeSheet_period = db.vw_ta_timesheet.Where(Function(p) p.anio = idYear And p.mes = idMonth And p.ts_leave_update = False And p.id_timesheet_estado = 3).ToList

                End Using
            Catch ex As Exception

                getTimeSheet_period = Nothing

            End Try

        End Function


        Public Function getLeaveAPP(ByVal idUsuario As Integer, ByVal v_Month As Integer) As List(Of Integer)

            Try

                Using db As New dbRMS_JIEntities

                    getLeaveAPP = db.ta_timesheet.Where(Function(p) p.id_usuario = idUsuario And p.ts_leave_update = True And p.id_timesheet_estado = 3 And p.mes = v_Month).Select(Function(p) p.id_timesheet).ToList()

                End Using
            Catch ex As Exception

                getLeaveAPP = Nothing

            End Try

        End Function


        Public Function getTimeSheet(ByVal idUsuario As Integer, ByVal idRoles As Integer(), Optional bndALL As Integer = 0) As DataTable

            Try


                Using db As New dbRMS_JIEntities

                    'Dim Time_sheets As DataTable = get_TimeSheet_User(idUsuario)
                    'Dim IdValues = (From row In Time_sheets Select colB = Convert.ToInt32(row(0))).ToList
                    ' IdValues.Contains(p.id_timesheet) Or

                    'Dim idUsers As Object = (From us In db.vw_ta_roles_emplead Where idRoles.Contains(us.id_rol) And us.id_type_role = 1
                    '                         Select New With {Key .id_usuario = us.id_usuario}).ToList()

                    Dim idUsers = db.vw_ta_roles_emplead.Where(Function(p) idRoles.Contains(p.id_rol) And p.id_type_role = 1).Select(Function(s) s.id_usuario).ToList()
                    Dim arrUser As Integer() = idUsers.Select(Function(x) Convert.ToInt32(x)).ToArray()

                    Dim LIST_vw_ta_timesheet As Object = db.vw_ta_timesheet.Where(Function(p) p.id_programa = id_programa And
                                                                                      ((p.id_usuario = idUsuario Or arrUser.Contains(p.usuario_creo) Or bndALL = 1))).ToList().OrderByDescending(Function(p) p.anio).ThenByDescending(Function(p) p.month_name).ThenBy(Function(p) p.nombre_usuario).ToList()
                    getTimeSheet = ConvertToDataTable(LIST_vw_ta_timesheet)

                End Using
            Catch ex As Exception
                getTimeSheet = Nothing
            End Try

        End Function




        Public Function getTimeSheetPeriod(ByVal idUsuario As Integer, ByVal month As Integer, ByVal year As Integer) As DataTable

            Try


                Using db As New dbRMS_JIEntities

                    'Dim Time_sheets As DataTable = get_TimeSheet_User(idUsuario)
                    'Dim IdValues = (From row In Time_sheets Select colB = Convert.ToInt32(row(0))).ToList
                    ' IdValues.Contains(p.id_timesheet) Or

                    'Dim idUsers As Object = (From us In db.vw_ta_roles_emplead Where idRoles.Contains(us.id_rol) And us.id_type_role = 1
                    '                         Select New With {Key .id_usuario = us.id_usuario}).ToList()


                    Dim LIST_vw_ta_timesheet As Object = db.vw_ta_timesheet.Where(Function(p) p.id_programa = id_programa _
                                                                                        And p.id_usuario = idUsuario _
                                                                                        And p.mes = month _
                                                                                        And p.anio = year).ToList()
                    getTimeSheetPeriod = ConvertToDataTable(LIST_vw_ta_timesheet)

                End Using
            Catch ex As Exception
                getTimeSheetPeriod = Nothing
            End Try

        End Function


        Public Function getTimeSheetUSR(ByVal idUsuario As Integer) As DataTable

            Try


                Using db As New dbRMS_JIEntities

                    Dim Time_sheets As DataTable = get_TimeSheet_User(idUsuario)
                    Dim IdValues = (From row In Time_sheets Select colB = Convert.ToInt32(row(0))).ToList

                    Dim LIST_vw_ta_timesheet As Object = db.vw_ta_timesheet.Where(Function(p) p.id_programa = id_programa And
                                                                                      p.id_usuario = idUsuario).ToList()
                    getTimeSheetUSR = ConvertToDataTable(LIST_vw_ta_timesheet)

                End Using
            Catch ex As Exception
                getTimeSheetUSR = Nothing
            End Try

        End Function


        Public Function SaveTimeSheet(ByVal TimeSheet As ta_timesheet, Optional ID As Integer = 0) As String

            Try


                Using db As New dbRMS_JIEntities


                    Dim result As String = 0
                    Dim ta_timeSheet_upd As ta_timesheet

                    If ID = 0 Then

                        db.ta_timesheet.Add(TimeSheet)
                        'db.Entry(TimeSheet).GetDatabaseValues()
                        '

                    Else

                        ta_timeSheet_upd = db.ta_timesheet.Find(ID)

                        ta_timeSheet_upd.id_usuario = TimeSheet.id_usuario
                        ta_timeSheet_upd.anio = TimeSheet.anio
                        ta_timeSheet_upd.mes = TimeSheet.mes
                        ta_timeSheet_upd.description = TimeSheet.description
                        ta_timeSheet_upd.notes = TimeSheet.notes
                        ta_timeSheet_upd.id_programa = TimeSheet.id_programa
                        ta_timeSheet_upd.fecha_upd = TimeSheet.fecha_upd
                        ta_timeSheet_upd.usuario_upd = TimeSheet.usuario_upd
                        ta_timeSheet_upd.id_timesheet_estado = TimeSheet.id_timesheet_estado

                        db.Entry(ta_timeSheet_upd).State = Entity.EntityState.Modified


                    End If

                    If (db.SaveChanges()) Then

                        If ID = 0 Then
                            result = TimeSheet.id_timesheet
                        Else
                            result = ta_timeSheet_upd.id_timesheet
                        End If

                        SaveTimeSheet = result

                    Else
                        SaveTimeSheet = "-1"
                    End If

                End Using

            Catch ex As Exception

                SaveTimeSheet = ex.Message

            End Try

        End Function


        Public Function SaveDocumento_TimeSheet(ByVal DOCtimeSheet As ta_documento_timesheets, ByVal idDoc As Integer) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim ta_documento_timesheets_upd As ta_documento_timesheets

                    If idDoc = 0 Then

                        db.ta_documento_timesheets.Add(DOCtimeSheet)

                    Else

                        ta_documento_timesheets_upd = db.ta_documento_timesheets.Find(idDoc)
                        ta_documento_timesheets_upd.id_TipoDocumento = DOCtimeSheet.id_TipoDocumento
                        ta_documento_timesheets_upd.id_documento = DOCtimeSheet.id_documento

                        db.Entry(ta_documento_timesheets_upd).State = Entity.EntityState.Modified

                    End If


                    If (db.SaveChanges()) Then

                        If idDoc = 0 Then
                            result = DOCtimeSheet.id_documento_timesheet
                        Else
                            result = ta_documento_timesheets_upd.id_documento_timesheet
                        End If

                        SaveDocumento_TimeSheet = result

                    Else

                        SaveDocumento_TimeSheet = "-1"

                    End If


                End Using

            Catch ex As Exception

                SaveDocumento_TimeSheet = ex.Message

            End Try

        End Function



        Public Function SaveTimeSheetDetail(ByVal TimeSheetDetail As ta_timesheet_detail, ByVal idTimeSheet As Integer, ByVal idBillableTime As Integer) As String

            Try

                Dim result As String = 0
                Dim FindVal As Boolean = False
                Dim idDetail As Integer = 0
                Dim ta_timeSheet_detail_upd As ta_timesheet_detail

                Using db As New dbRMS_JIEntities

                    Dim ta_timesheetDetail As List(Of ta_timesheet_detail) = db.ta_timesheet_detail.Where(Function(p) p.id_timesheet = idTimeSheet And p.id_billable_time = idBillableTime).ToList

                    If ta_timesheetDetail.Count > 0 Then 'Has values for this billable item

                        FindVal = False

                        For Each itemTimeSheetDetail In ta_timesheetDetail

                            If itemTimeSheetDetail.dia = TimeSheetDetail.dia Then
                                FindVal = True
                                idDetail = itemTimeSheetDetail.id_timesheet_detail
                                Exit For
                            End If

                        Next

                    End If

                    If Not FindVal Then

                        db.ta_timesheet_detail.Add(TimeSheetDetail)

                    Else 'For update

                        ta_timeSheet_detail_upd = db.ta_timesheet_detail.Find(idDetail)
                        'ta_timeSheet_detail_upd.id_timesheet = TimeSheetDetail.id_timesheet
                        'ta_timeSheet_detail_upd.id_billable_time = TimeSheetDetail.id_billable_time
                        'ta_timeSheet_detail_upd.dia = TimeSheetDetail.dia
                        ta_timeSheet_detail_upd.hours = TimeSheetDetail.hours
                        ta_timeSheet_detail_upd.id_usuario_upd = TimeSheetDetail.id_usuario_creo 'its supposed to be created
                        ta_timeSheet_detail_upd.fecha_upd = Date.UtcNow
                        db.Entry(ta_timeSheet_detail_upd).State = Entity.EntityState.Modified


                    End If

                    If (db.SaveChanges()) Then

                        If Not FindVal Then
                            result = TimeSheetDetail.id_timesheet_detail
                        Else
                            result = ta_timeSheet_detail_upd.id_timesheet_detail
                        End If

                        SaveTimeSheetDetail = result

                    Else
                        SaveTimeSheetDetail = "-1"
                    End If

                End Using

            Catch ex As Exception

                SaveTimeSheetDetail = ex.Message

            End Try

        End Function


        Public Function DeleteTimeSheetDetail(ByVal dia As Integer, ByVal idTimeSheet As Integer, ByVal idBillableTime As Integer) As String

            Try

                Dim result As String = 0
                Dim FindVal As Boolean = False
                Dim idDetail As Integer = 0

                Using db As New dbRMS_JIEntities

                    Dim ta_timesheetDetail As List(Of ta_timesheet_detail) = db.ta_timesheet_detail.Where(Function(p) p.id_timesheet = idTimeSheet And p.id_billable_time = idBillableTime).ToList

                    If ta_timesheetDetail.Count > 0 Then 'Has values for this billable item

                        FindVal = False

                        For Each itemTimeSheetDetail In ta_timesheetDetail

                            If itemTimeSheetDetail.dia = dia Then

                                FindVal = True
                                idDetail = itemTimeSheetDetail.id_timesheet_detail
                                db.Database.ExecuteSqlCommand("DELETE FROM ta_timesheet_detail WHERE id_timesheet_detail = " + idDetail.ToString())
                                Exit For

                            End If

                        Next


                    End If


                    If (FindVal = True) Then
                        DeleteTimeSheetDetail = "0"
                    Else
                        DeleteTimeSheetDetail = "-1"
                    End If

                End Using

            Catch ex As Exception

                DeleteTimeSheetDetail = ex.Message

            End Try

        End Function


        Public Function DeleteTimeSheet(ByVal idTimeSheet As Integer) As String

            Try


                Using db As New dbRMS_JIEntities

                    db.Database.ExecuteSqlCommand("DELETE FROM ta_documento WHERE id_documento = (select id_documento from ta_documento_timesheets where id_timesheet = " & idTimeSheet.ToString() & " )")

                    db.Database.ExecuteSqlCommand("DELETE FROM ta_documento_timesheets WHERE id_timesheet = " + idTimeSheet.ToString())

                    db.Database.ExecuteSqlCommand("DELETE FROM ta_timesheet_detail WHERE id_timesheet = " + idTimeSheet.ToString())

                    db.Database.ExecuteSqlCommand("DELETE FROM ta_timesheet WHERE id_timesheet = " + idTimeSheet.ToString())

                    DeleteTimeSheet = "0"


                End Using

            Catch ex As Exception

                DeleteTimeSheet = ex.Message

            End Try

        End Function



        Public Function get_TimeSheet(ByVal idTimeSheet As Integer) As ta_timesheet

            Using db As New dbRMS_JIEntities

                get_TimeSheet = db.ta_timesheet.Find(idTimeSheet)

            End Using

        End Function


        Public Function get_ta_billable_type() As Object

            Using db As New dbRMS_JIEntities
                Dim listado = db.ta_billable_time_type.Select(Function(p) _
                                                           New With {Key .billable_time_type = p.billable_time_type,
                                                                     Key .id_billable_time_type = p.id_billable_time_type}).ToList()
                Return listado
            End Using
        End Function



        Public Function get_ta_billable_Option(ByVal idBillType As Integer, ByVal idTimeSheet As Integer, ByVal leave As Integer) As Object

            Using db As New dbRMS_JIEntities

                Dim t_idBillables = db.ta_timesheet_detail.Where(Function(p) p.id_timesheet = idTimeSheet).Select(Function(p) _
                                                                                                                      New With {Key .id_billable_time = p.id_billable_time}).Distinct()
                'Dim ArrBillable As Integer() = t_idBillables.ToArray
                ' t_idBillables.Contains(p.id_billable_time)



                Dim listado = db.ta_billable_time.Where(Function(p) p.id_billable_time_type = idBillType And p.visible = True And (p.ts_leave = True Or 0 = leave) And Not (t_idBillables.Any(Function(b) b.id_billable_time = p.id_billable_time))).Select(Function(p) _
                                                                                                                                                                                           New With {Key .Text = p.billable_time,
                                                                                                                                                                                           Key .Value = p.id_billable_time}).ToList()

                Return listado
                'Return t_idBillables

            End Using
        End Function


        Public Function getTimeSheetDetail(ByVal idTimeSheet As Integer) As DataTable

            Try

                Dim list_Result As Object

                Using db As New dbRMS_JIEntities

                    list_Result = db.ta_timesheet_detail.Where(Function(p) p.id_timesheet = idTimeSheet).ToList()

                    getTimeSheetDetail = ConvertToDataTable(list_Result)


                End Using

            Catch ex As Exception

                getTimeSheetDetail = Nothing

            End Try


        End Function


        Public Function get_type_employee() As Object

            Using db As New dbRMS_JIEntities
                Dim listado = db.t_employee_type.Where(Function(P) P.id_programa = id_programa).Select(Function(p) _
                                                           New With {Key .employee_type = p.employee_type,
                                                                     Key .id_employee_type = p.id_employee_type}).ToList()
                Return listado
            End Using
        End Function

        Public Function get_TimeSheet_User(ByVal idUser As Integer) As DataTable


            Dim tbl_table As New DataTable

            Dim strSQL As String = String.Format("Select distinct b.id_timesheet from ta_documento_timesheets b
	                                                inner join vw_ta_documentos a on (a.id_documento = b.id_documento)
	                                             where ({0}) in (select * from dbo.SDF_SplitString(a.IdUserPArticipate,',') )", idUser)

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_documento_timesheets", "id_timesheet", 0, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_timesheet") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            Return tbl_result


        End Function

        Public Function get_TimeSheet_Summary(ByVal idTimeSheet As Integer) As DataTable

            Dim tbl_table As New DataTable

            Dim strSQL As String = String.Format("select CONVERT(nvarchar(10), (ROW_NUMBER() OVER(ORDER BY  id_timesheet, billable_order, billable_time ASC))) + '.' AS numberITEM,  
                                                   billable_time_type,
	                                               id_billable_time_type,
	                                               id_billable_time,
	                                               billable_time,
	                                               billable_item,
	                                               dias,
	                                               TOThours,
	                                               LOE,
	                                               round(convert(numeric(15,0),progress_value),0) as progress_value,
	                                               case 
	                                                 when progress_value >= 0 and progress_value <= 33.33 then
		                                               'progress-bar-yellow' 
		                                             when progress_value > 33.33 and progress_value <= 66.66 then
		                                                'progress-bar-primary' 
		                                             else
		                                               'progress-bar-success'
		                                             end as progress_bar,
		                                             case 
	                                                 when progress_value >= 0 and progress_value <= 33.33 then
		                                               'bg-yellow'  
		                                             when progress_value > 33.33 and progress_value <= 66.66 then
		                                                'bg-light-blue' 
		                                             else
		                                               'bg-green'
		                                             end as bg_color
	                                                from vw_summary_time_sheet
                                             where id_timesheet = {0} ", idTimeSheet)

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_billable_time_type", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_billable_time_type") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            Return tbl_result

        End Function


        Public Function get_TimeSheet_Summary_rept(ByVal idTimeSheet As Integer) As DataTable

            Dim tbl_table As New DataTable

            Dim strSQL As String = String.Format("select CONVERT(nvarchar(10), (ROW_NUMBER() OVER(ORDER BY  id_timesheet, billable_order, billable_time ASC))) + '.' AS numberITEM, 
                                                   id_timesheet,
                                                   billable_time_type,
	                                               id_billable_time_type,
	                                               id_billable_time,
	                                               billable_time,
	                                               billable_item,
	                                               dias,
	                                               TOThours,
	                                               LOE,
	                                               round(convert(numeric(15,0),progress_value),0) as progress_value,
	                                               case 
	                                                 when progress_value >= 0 and progress_value <= 33.33 then
		                                               'progress-bar-yellow' 
		                                             when progress_value > 33.33 and progress_value <= 66.66 then
		                                                'progress-bar-primary' 
		                                             else
		                                               'progress-bar-success'
		                                             end as progress_bar,
		                                             case 
	                                                 when progress_value >= 0 and progress_value <= 33.33 then
		                                               'bg-yellow'  
		                                             when progress_value > 33.33 and progress_value <= 66.66 then
		                                                'bg-light-blue' 
		                                             else
		                                               'bg-green'
		                                             end as bg_color
	                                                from vw_summary_time_sheet
                                             where id_timesheet = {0} ", idTimeSheet)

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_billable_time_type", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_billable_time_type") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            Return tbl_result

        End Function

        Public Function get_SummaryTimeSheet(ByVal idTimeSheet As Integer) As Object

            Using db As New dbRMS_JIEntities
                Dim listado = db.vw_time_sheet_total.Where(Function(P) P.id_timesheet = idTimeSheet).FirstOrDefault()
                Return listado
            End Using
        End Function


        Public Function get_TimeSheet_User(ByVal idUser As Integer, ByVal strUsers As String) As DataTable


            Dim tbl_table As New DataTable



            Dim strSQL As String = String.Format("select id_timesheet,
                                                   id_usuario,
	                                               nombre_usuario,
	                                               email_usuario,
	                                               job,
	                                               anio,
	                                               mes,
	                                               description,
	                                               notes,
	                                               usuario_creo,
	                                               fecha_creo,
	                                               usuario_upd,
	                                               fecha_upd,
	                                               timesheet_estado,
	                                               id_programa,
	                                               id_employee_type,
	                                               employee_type
                                            from vw_ta_timesheet
                                            where (id_programa = {2} and  (id_usuario = {0} or usuario_creo in (1)) )", idUser, strUsers, id_programa)

            Dim tbl_result As DataTable = cl_utl.setObjeto("vw_ta_timesheet", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_programa") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            Return tbl_result

        End Function


        Public Function getTimeSheet_period_d(ByVal idMonth As Integer, ByVal idYear As Integer) As DataTable

            Try

                'Using db As New dbRMS_JIEntities

                '    getTimeSheet_period = db.vw_ta_timesheet.Where(Function(p) p.anio = idYear And p.mes = idMonth And p.ts_leave_update = False And p.id_timesheet_estado = 3).ToList

                'End Using
                Dim tbl_table As New DataTable

                Dim strSQL As String = String.Format(" select		
                                                                        ROW_NUMBER() OVER(ORDER BY nombre_usuario ASC) AS NR,
                                                                        id_timesheet,
			                                                            id_usuario,
			                                                            nombre_usuario,
			                                                            email_usuario,
			                                                            job,
			                                                            id_idioma,
			                                                            anio,
			                                                            mes,
			                                                            month_name,
			                                                            description,
			                                                            notes,
			                                                            fecha_creo,
			                                                            fecha_upd,
			                                                            usuario_creo,
			                                                            usuario_upd,
			                                                            ts_leave_update,
			                                                            TimeSheet_Type,
			                                                            id_timesheet_estado,
			                                                            timesheet_estado,
			                                                            id_programa,
			                                                            id_employee_type,
			                                                            employee_type, numero_documento from  vw_ta_timesheet
			                                                              where anio = {0} and mes = {1} 
			                                                               and ts_leave_update = 0 
				                                                            and id_timesheet_estado = 3
                                                                             and id_programa = {2}   
			                                                             order by nombre_usuario", idYear, idMonth, id_programa)

                Dim tbl_result As DataTable = cl_utl.setObjeto("vw_ta_timesheet", "id_programa", id_programa, strSQL)

                '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
                If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_programa") = 0) Then
                    tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
                End If
                '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

                getTimeSheet_period_d = tbl_result

            Catch ex As Exception

                getTimeSheet_period_d = Nothing

            End Try

        End Function


        Public Function get_app_tools(ByVal idcat As Integer) As Object

            Using db As New dbRMS_JIEntities



                Dim listado = db.vw_approval_tools_cat.Where(Function(p) p.id_categoria = idcat Or p.id_categoria = 0).Select(Function(p) _
                                                                                                              New With {Key .Text = p.approval_tool_name,
                                                                                                                         Key .Value = p.id_approval_tool}).ToList()
                Return listado

            End Using

        End Function


        Public Function get_CategoryUser(ByVal id_R As String) As DataTable

            Dim strSQL As String = String.Format("select distinct id_categoria, descripcion_cat from vw_roles_approvals " &
                                                 "   where id_programa = {0} and orden = 0 " &
                                                 "       and id_rol in ({1})", id_programa, id_R)

            get_CategoryUser = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_CategoryUser.Rows.Count = 1 And get_CategoryUser.Rows.Item(0).Item("id_categoria") = 0) Then
                get_CategoryUser.Rows.Remove(get_CategoryUser.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_TimeSheetApprovalUser(ByVal id_Usr As String) As DataTable

            Dim strSQL As String = String.Format("select * From vw_roles_approvals where id_usuario= {1} and id_programa = {0} and tool_code = 'TM-SHEET01' and orden = 0 and visible = 'SI' ", id_programa, id_Usr)

            get_TimeSheetApprovalUser = cl_utl.setObjeto("vw_roles_approvals", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_TimeSheetApprovalUser.Rows.Count = 1 And get_TimeSheetApprovalUser.Rows.Item(0).Item("id_tipoDocumento") = 0) Then
                get_TimeSheetApprovalUser.Rows.Remove(get_TimeSheetApprovalUser.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function




        Public Function get_TipoDOcumento(ByVal idTipoDocumento As Integer) As DataTable

            Try

                Dim list_Result As Object

                Using db As New dbRMS_JIEntities

                    list_Result = db.ta_tipoDocumento.Where(Function(p) p.id_tipoDocumento = idTipoDocumento).ToList()
                    get_TipoDOcumento = ConvertToDataTable(list_Result)


                End Using

            Catch ex As Exception

                get_TipoDOcumento = Nothing

            End Try


        End Function

        Public Function CrearCodigo(ByVal idCat As Integer) As String
            Dim codigoApp As String = ""


            Dim CorrrelativoStr As String = ""


            Dim strSQL As String = String.Format("SELECT id_categoria, cod_categoria, correlativos FROM ta_categoria WHERE id_categoria={0}", idCat)

            Dim TblCat As DataTable = cl_utl.setObjeto("ta_categoria", "id_programa", id_programa, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (TblCat.Rows.Count = 1 And TblCat.Rows.Item(0).Item("id_categoria") = 0) Then
                TblCat.Rows.Remove(TblCat.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************


            Dim Corrrelativo As Integer = 0

            If Not IsNothing(TblCat) Then
                Corrrelativo = TblCat.Rows(0).Item("correlativos") + 1
            Else
                Dim rnd As New Random()
                Corrrelativo = rnd.Next(1, 999999)
            End If


            If Corrrelativo < 10 Then
                CorrrelativoStr = "000"
            ElseIf Corrrelativo < 100 Then
                CorrrelativoStr = "00"
            ElseIf Corrrelativo < 1000 Then
                CorrrelativoStr = "0"
            Else
                CorrrelativoStr = ""
            End If

            If Not IsNothing(TblCat) Then
                Corrrelativo = TblCat.Rows(0).Item("correlativos") + 1
                codigoApp = TblCat.Rows(0).Item("cod_categoria") & "-" & CorrrelativoStr & Corrrelativo
            Else

                Corrrelativo += 1
                codigoApp = "CATNOCDE" & "-" & CorrrelativoStr & Corrrelativo

            End If

            Return codigoApp

        End Function




        Public Sub SaveComment(ByVal idApp As Integer, ByVal idEstadoDoc As Integer, ByVal Comment As String, ByVal idUser As Integer)

            Dim clss_approval As APPROVAL.clss_approval = New APPROVAL.clss_approval(id_programa)


            Dim strComment As String
            If Trim(Comment).Length = 0 Then
                strComment = "--No Comments--"
            Else
                strComment = Comment
            End If


            clss_approval.set_ta_comentariosDoc(0) 'New Record
            clss_approval.set_ta_comentariosDocFIELDS("id_App_Documento", idApp, "id_comment", 0)
            clss_approval.set_ta_comentariosDocFIELDS("id_estadoDoc", idEstadoDoc, "id_comment", 0)
            clss_approval.set_ta_comentariosDocFIELDS("id_tipoAccion", cAction_ByProcess, "id_comment", 0)
            clss_approval.set_ta_comentariosDocFIELDS("id_usuario", idUser, "id_comment", 0)
            clss_approval.set_ta_comentariosDocFIELDS("fecha_comentario", Date.UtcNow, "id_comment", 0)
            clss_approval.set_ta_comentariosDocFIELDS("comentario", strComment.Trim.Replace("  ", ""), "id_comment", 0)

            If clss_approval.save_ta_comentariosDoc() = -1 Then
                'Error do somenthing

            End If


        End Sub



        Public Function TimeSheet_Support_Docs(ByVal id_timesheet As Integer) As DataTable

            Dim strSQL As String = String.Format("select * from ta_timesheet_support_docs where id_timesheet = {0} ", id_timesheet)

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_timesheet_support_docs", "id_timesheet", id_timesheet, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_timesheet") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            TimeSheet_Support_Docs = tbl_result

        End Function




        Public Function TimeSheet_Support_Docs_del(ByVal id_timesheet_support_docs As Integer) As Boolean

            Dim strSQL As String = String.Format("delete from ta_timesheet_support_docs where id_timesheet_support_docs = {0} ", id_timesheet_support_docs)

            Try

                CNN_.Open()
                Dim dm As New SqlCommand(strSQL, CNN_)
                dm.ExecuteNonQuery()
                CNN_.Close()

                TimeSheet_Support_Docs_del = True

            Catch ex As Exception
                TimeSheet_Support_Docs_del = False
                CNN_.Close()
            End Try

        End Function




        Public Function Save_TimeSheet_support_docs(ByVal TimeSheet_support_docs As ta_timesheet_support_docs, Optional ID As Integer = 0) As String

            Try

                Using db As New dbRMS_JIEntities

                    Dim result As String = 0
                    Dim TimeSheet_support_docs_upd As ta_timesheet_support_docs

                    If ID = 0 Then

                        db.ta_timesheet_support_docs.Add(TimeSheet_support_docs)
                        'db.Entry(TimeSheet).GetDatabaseValues()

                    Else

                        TimeSheet_support_docs_upd = db.ta_timesheet_support_docs.Find(ID)

                        TimeSheet_support_docs_upd.archivo = TimeSheet_support_docs.archivo
                        TimeSheet_support_docs_upd.id_doc_soporte = TimeSheet_support_docs.id_doc_soporte
                        TimeSheet_support_docs_upd.ver = TimeSheet_support_docs.ver

                        db.Entry(TimeSheet_support_docs_upd).State = Entity.EntityState.Modified


                    End If

                    If (db.SaveChanges()) Then

                        If ID = 0 Then
                            result = TimeSheet_support_docs.id_timesheet_support_docs
                        Else
                            result = TimeSheet_support_docs_upd.id_timesheet_support_docs
                        End If

                        Save_TimeSheet_support_docs = result

                    Else
                        Save_TimeSheet_support_docs = "-1"
                    End If

                End Using

            Catch ex As Exception

                Save_TimeSheet_support_docs = ex.Message

            End Try

        End Function


        Public Function get_Doc_support_Route_TS(ByVal id_TipoDoc As Integer, Optional strDocs As String = "") As DataTable 'This function gave me the allowed files


            Dim bndDOCS As Integer = If(strDocs.Length > 0, 0, 1)

            If strDocs.Length = 0 Then
                strDocs = "0"
            End If

            Dim Sql As String = String.Format("SELECT ta_docs_soporte.id_doc_soporte, 
				                                    ta_docs_soporte.nombre_documento, 
				                                    ta_docs_soporte.id_programa, 
                                                    ta_docs_soporte.Template, 
                                                    ta_docs_soporte.extension, 
				                                    ta_aprobacion_docs.id_app_docs, 
				                                    ta_aprobacion_docs.id_tipoDocumento,
                                                    ta_aprobacion_docs.PermiteRepetir, 
                                                    ta_aprobacion_docs.RequeridoInicio, 
                                                    ta_aprobacion_docs.RequeridoFin,
                                                    ta_docs_soporte.environmental,
													ta_docs_soporte.deliverable,
													ta_docs_soporte.max_size	          
				                                    FROM ta_docs_soporte            
				                                    INNER JOIN ta_aprobacion_docs On ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte              
				                                    WHERE ( ta_docs_soporte.id_programa = {0} 
				                                     and ta_aprobacion_docs.id_tipoDocumento = {1} ) 
				                                     AND (  ( ta_aprobacion_docs.id_doc_soporte  NOT IN ({2}) OR 1={3} )
                                                          OR ta_aprobacion_docs.PermiteRepetir='SI' ) ", id_programa, id_TipoDoc, strDocs, bndDOCS)

            get_Doc_support_Route_TS = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_TipoDoc, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Doc_support_Route_TS.Rows.Count = 1 And get_Doc_support_Route_TS.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Doc_support_Route_TS.Rows.Remove(get_Doc_support_Route_TS.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_Doc_support_Route_TimeSheet(ByVal id_TipoDoc As Integer, Optional strDocs As String = "") As DataTable 'This function gave me the allowed files


            Dim bndDOCS As Integer = If(strDocs.Length > 0, 0, 1)

            If strDocs.Length = 0 Then
                strDocs = "0"
            End If

            Dim Sql As String = String.Format("SELECT ta_docs_soporte.id_doc_soporte, 
				                                    ta_docs_soporte.nombre_documento, 
				                                    ta_docs_soporte.id_programa, 
                                                    ta_docs_soporte.Template, 
                                                    ta_docs_soporte.extension, 
				                                    ta_aprobacion_docs.id_app_docs, 
				                                    ta_aprobacion_docs.id_tipoDocumento,
                                                    ta_aprobacion_docs.PermiteRepetir, 
                                                    ta_aprobacion_docs.RequeridoInicio, 
                                                    ta_aprobacion_docs.RequeridoFin,
                                                    ta_docs_soporte.environmental,
													ta_docs_soporte.deliverable,
													ta_docs_soporte.max_size	          
				                                    FROM ta_docs_soporte            
				                                    INNER JOIN ta_aprobacion_docs On ta_docs_soporte.id_doc_soporte = ta_aprobacion_docs.id_doc_soporte              
				                                    WHERE ( ta_docs_soporte.id_programa = {0} 
				                                     and ta_aprobacion_docs.id_tipoDocumento = {1} ) 
				                                     AND (  ( ta_aprobacion_docs.id_doc_soporte  NOT IN ({2}) OR 1={3} )
                                                          OR ta_aprobacion_docs.PermiteRepetir='SI' ) ", id_programa, id_TipoDoc, strDocs, bndDOCS)

            get_Doc_support_Route_TimeSheet = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_TipoDoc, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (get_Doc_support_Route_TimeSheet.Rows.Count = 1 And get_Doc_support_Route_TimeSheet.Rows.Item(0).Item("id_doc_soporte") = 0) Then
                get_Doc_support_Route_TimeSheet.Rows.Remove(get_Doc_support_Route_TimeSheet.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function TimeSheet_Document(ByVal id_timesheet As Integer, Optional idDoc As Integer = 0) As DataTable

            Dim strSQL As String = If(idDoc = 0, String.Format("select * from ta_documento_timesheets where id_timesheet = {0} ", id_timesheet), String.Format("select * from ta_documento_timesheets where id_documento = {0} ", idDoc))

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_documento_timesheets", "id_documento_timesheet", id_timesheet, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_timesheet") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            TimeSheet_Document = tbl_result

        End Function


        Public Function TimeSheet_Support_Documents_detail(ByVal id_timesheet As Integer) As DataTable

            Dim strSQL As String = String.Format("select  ROW_NUMBER() OVER(ORDER BY b.nombre_documento, a.archivo ASC) AS no,
                                                        a.id_timesheet_support_docs,
		                                                a.id_timesheet,
		                                                a.archivo,
		                                                a.id_doc_soporte,
		                                                b.nombre_documento,
		                                                a.ver,		
		                                                SUBSTRING(ltrim(rtrim(RIGHT(a.archivo,5))),CHARINDEX('.',ltrim(rtrim(RIGHT(a.archivo,5)))),LEN(ltrim(rtrim(RIGHT(a.archivo,5)))) - ((CHARINDEX('.',ltrim(rtrim(RIGHT(a.archivo,5))))) - 1)) as ext,
		                                                isnull(c.ext_icon,'fa fa-file') as ext_icon
                                                    from ta_timesheet_support_docs a
                                                      inner join ta_docs_soporte b on (a.id_doc_soporte = b.id_doc_soporte)
                                                       left join ta_catalogo_extensiones c on ( SUBSTRING(ltrim(rtrim(RIGHT(a.archivo,5))),CHARINDEX('.',ltrim(rtrim(RIGHT(a.archivo,5)))),LEN(ltrim(rtrim(RIGHT(a.archivo,5)))) - ((CHARINDEX('.',ltrim(rtrim(RIGHT(a.archivo,5))))) - 1)) = ltrim(rtrim(c.nom_ext)) )
                                                     where id_timesheet = {0} order by b.nombre_documento, a.archivo ", id_timesheet)

            Dim tbl_result As DataTable = cl_utl.setObjeto("ta_timesheet_support_docs", "id_timesheet_support_docs", id_timesheet, strSQL)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_result.Rows.Count = 1 And tbl_result.Rows.Item(0).Item("id_timesheet") = 0) Then
                tbl_result.Rows.Remove(tbl_result.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            TimeSheet_Support_Documents_detail = tbl_result

        End Function


    End Class






End Namespace