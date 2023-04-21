
Imports System.IO
Imports ClosedXML.Excel
Imports ly_SIME
Imports ly_APPROVAL
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Public Class frm_TemplateReportDeliv
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim idTpReport As Integer = Me.Request.QueryString("idTR")
        Dim idUser As Integer = Me.Request.QueryString("vUs")
        Dim idMec As Integer = Me.Request.QueryString("vMec")
        Dim idSubMec As Integer = Me.Request.QueryString("vSubMec")

        'Dim vYR As Integer = Me.Request.QueryString("vYR")
        'Dim idP As Integer = Me.Request.QueryString("idP")
        'Dim idA As Integer = Me.Request.QueryString("idA")
        'Dim idI As Integer = Me.Request.QueryString("idI")
        'Dim idInd As Integer = Me.Request.QueryString("idInd")
        'Dim PgNB As Integer = Me.Request.QueryString("PgNumb")

        Dim ds As Object = Nothing
        Dim ReportTittle As String = ""

        If idTpReport = 1 Then
            ds = get_DataSet_Deliv(idTpReport, idMec, idSubMec)
            ReportTittle = "Deliverable"

        ElseIf idTpReport = 2 Then
            ds = get_DataSet_(idTpReport, idUser)
            ReportTittle = "TimeReported_"
        End If

        GenerateReport_(idTpReport, idUser, ds, ReportTittle)

        'Dim _DataSet As Object = get_Indicator_Period_Result(vYR, idP, idA, idI)

        'If idTpReport = 1 Then 'Report By user
        '    'GenerateReport_(vYR, idP, idA, idI, get_Indicator_Period_Result(vYR, idP, idA, idI), "Implementer_Detail", ReportHeader(vYR, idP, idA, idI, -1))
        '    GenerateReport_(vYR, idP, idA, idI, get_ToolDetail_(vYR, idP, idA, idI, idInd, PgNB), "Indicator_Detail_Part" & PgNB, ReportHeader(vYR, idP, idA, idI, idInd))
        'ElseIf idTpReport = 2 Then 'Implementer Source of information
        '    ' GenerateReport_(vYR, idP, idA, idI, get_ToolDetail_(vYR, idP, idA, idI, idInd, PgNB), "Indicator_Detail_Part" & PgNB, ReportHeader(vYR, idP, idA, idI, idInd))
        'End If


    End Sub

    Public Function get_ToolDetail_(ByVal vYear As Integer, ByVal idProgram As Integer, ByVal vActivity As Integer, ByVal vImplementer As Integer, ByVal idIndicator As Integer, ByVal pgNumber As Integer) As DataTable

        Dim bndYEar As Integer = If(vYear = -1, 1, 0)
        Dim bndImplementer As Integer = If(vImplementer = -1, 1, 0)
        Dim bndActivity As Integer = If(vActivity = -1, 1, 0)
        Dim indicadores As Object

        Using dbEntities As New dbRMS_JIEntities

            Dim cl_Util As CORE.cl_RMS_Results = New CORE.cl_RMS_Results(idProgram)

            indicadores = cl_Util.get_Result_Detail(idIndicator, vYear, vActivity, vImplementer, "Detail", pgNumber)


        End Using

        get_ToolDetail_ = indicadores

    End Function


    Public Function get_DataSet_(ByVal idTP As Integer, ByVal idUser As Integer) As DataTable

        Dim ds As New DataSet
        Dim adapter As SqlDataAdapter

        cnn.Open()

        'Me.hd_tp.Value = 1

        Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("SP_HR_BILLABLE_TIME", cnn)
        cmd.CommandType = CommandType.StoredProcedure
        'cmd.Parameters.Add("@tp_view", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
        cmd.Parameters.AddWithValue("@tp_view", idTP)
        cmd.Parameters.AddWithValue("@_id_usuario", idUser)
        cmd.Parameters.AddWithValue("@_anio", 0)

        adapter = New SqlDataAdapter(cmd)
        adapter.Fill(ds)

        'Dim dataT As DataTable = ds.Tables(0)
        get_DataSet_ = ds.Tables(0)

    End Function


    Public Function get_DataSet_Deliv(ByVal idTP As Integer, ByVal idMec As Integer, ByVal idSubMec As Integer) As DataTable

        Dim idPrograma As Integer = CType(Session.Item("E_IDPrograma"), Integer)
        Dim cls_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(idPrograma)

        Dim tbl_Data As DataTable = cls_Deliverable.get_Deliverable_Det(idMec, idSubMec)

        get_DataSet_Deliv = tbl_Data

    End Function




    Public Function ExportDataTable(table As DataTable) As List(Of List(Of Object))
        Dim result As New List(Of List(Of Object))
        For Each row As DataRow In table.Rows
            Dim values As New List(Of Object)
            For Each column As DataColumn In table.Columns
                If row.IsNull(column) Then
                    values.Add(Nothing)
                Else
                    values.Add(row.Item(column))
                End If
            Next
            result.Add(values)
        Next
        Return result
    End Function

    Public Function get_Indicator_Period_Result(ByVal vYear As Integer, ByVal idProgram As Integer, ByVal vActivity As Integer, ByVal vImplementer As Integer) As Object

        Dim bndYEar As Integer = If(vYear = -1, 1, 0)
        Dim bndImplementer As Integer = If(vImplementer = -1, 1, 0)
        Dim bndActivity As Integer = If(vActivity = -1, 1, 0)
        Dim indicadores As Object

        Using dbEntities As New dbRMS_JIEntities

            'And (p.Total_Progress > 0 Or p.meta_total > 0)
            indicadores = dbEntities.vw_tme_ficha_meta_indicador_Year_Period.Where(Function(p) p.id_programa = idProgram _
                                                                                            And (p.anio = vYear Or 1 = bndYEar) _
                                                                                            And (p.id_ficha_proyecto = vActivity Or 1 = bndActivity) _
                                                                                            And (p.id_ejecutor = vImplementer Or 1 = bndImplementer)) _
                                                                                   .GroupBy(Function(g) New With {g.Implementer,
                                                                                                                  g.codigo_SAPME,
                                                                                                                  g.nombre_proyecto,
                                                                                                                  g.meta_total,
                                                                                                                  g.orden_matriz_LB,
                                                                                                                  g.FY,
                                                                                                                  g.anio,
                                                                                                                  g.id_indicador,
                                                                                                                  g.codigo_indicador,
                                                                                                                  g.nombre_indicador_LB,
                                                                                                                  g.nombre_metodo_operacion}) _
                                                                                   .Select(Function(s) New With {.Implementer = s.FirstOrDefault.Implementer,
                                                                                                                  .Activity_Code = s.FirstOrDefault.codigo_SAPME,
                                                                                                                  .Project_Code = s.FirstOrDefault.nombre_proyecto,
                                                                                                                  .Project_Target = s.FirstOrDefault.meta_total,
                                                                                                                  .Indicator_Order = s.FirstOrDefault.orden_matriz_LB,
                                                                                                                  .FY = s.FirstOrDefault.FY,
                                                                                                                  .YEAR = s.FirstOrDefault.anio,
                                                                                                                  .id_indicator = s.FirstOrDefault.id_indicador,
                                                                                                                  .indicador_code = s.FirstOrDefault.codigo_indicador,
                                                                                                                  .indicator_name = s.FirstOrDefault.nombre_indicador_LB,
                                                                                                                  .calculation = s.FirstOrDefault.nombre_metodo_operacion,
                                                                                                                  .Q1 = s.Sum(Function(m) m.Q1),
                                                                                                                  .Q2 = s.Sum(Function(m) m.Q2),
                                                                                                                  .Q3 = s.Sum(Function(m) m.Q3),
                                                                                                                  .Q4 = s.Sum(Function(m) m.Q4),
                                                                                                                  .Total_Progress = s.Sum(Function(m) m.Total_Progress),
                                                                                                                  .Porcen_Progress = s.Sum(Function(por) por.Porcen_Progress)}) _
                                                                                   .OrderBy(Function(o) o.Implementer).ThenBy(Function(o2) o2.Project_Code).ThenBy(Function(o3) o3.FY).ThenBy(Function(o4) o4.Indicator_Order).ToList()


        End Using

        get_Indicator_Period_Result = indicadores

    End Function


    Sub GenerateReport_(ByVal idTypeReport As Integer, ByVal idUsr As Integer, _DataSet As DataTable, strTimeSheet_name As String)

        Dim col() As String = {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD",
                                   "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD",
                                   "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD",
                                   "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD",
                                   "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED",
                                   "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ"}

        Dim wrkBook = New XLWorkbook()
        Dim wrkSheet = wrkBook.AddWorksheet("sheet1")

        With wrkSheet

            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            .Range("A1:DD400").Style.Font.FontSize = 10
            .Range("A1:DD400").Style.Font.FontName = "Arial"

            ''.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            ''.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            ''.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            ''.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)

            '.Range("B2:E4").Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")
            '.Range("B2:E4").Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")

            'Dim strValues() As String = strHeader.Split("||")

            '--Implementer, progress, activity, year

            '.Cell(col(2) & 2).Value = strValues(2) '"<<--Achieved-->>"
            '.Range(col(2) & 2 & ":" & col(3) & 4).Merge()
            '.Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            '.Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            '.Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            '.Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            '.Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            '.Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            '.Range(col(2) & 2 & ":" & col(3) & 4).Style.Font.FontSize = 20
            '.Range(col(2) & 2 & ":" & col(3) & 4).Style.Font.Bold = True
            '.Range(col(2) & 2 & ":" & col(3) & 4).Style.Alignment.SetVertical(XLDrawingVerticalAlignment.Center)
            '.Range(col(2) & 2 & ":" & col(3) & 4).Style.Alignment.SetHorizontal(XLDrawingHorizontalAlignment.Center)


            '.Cell(col(4) & 2).Value = strValues(0)  '"<<--Implementer-->>"
            '.Range(col(4) & 2 & ":" & col(5) & 2).Merge()
            '.Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            '.Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            '.Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 2 & ":" & col(5) & 2).Style.Font.FontSize = 13
            '.Range(col(4) & 2 & ":" & col(5) & 2).Style.Font.Bold = True


            '.Cell(col(4) & 3).Value = strValues(4) '"<<--Activities-->>"
            '.Range(col(4) & 3 & ":" & col(5) & 3).Merge()
            '.Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            '.Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            '.Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 3 & ":" & col(5) & 3).Style.Font.FontSize = 10
            '.Range(col(4) & 3 & ":" & col(5) & 3).Style.Font.Bold = True


            '.Cell(col(4) & 4).Value = "Achieved - " & strValues(6) '"<<--Period of year-->>"
            '.Range(col(4) & 4 & ":" & col(5) & 4).Merge()
            '.Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            '.Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            '.Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            '.Range(col(4) & 4 & ":" & col(5) & 4).Style.Font.FontSize = 10
            '.Range(col(4) & 4 & ":" & col(5) & 4).Style.Font.Bold = True

            ''.Cell(col(2) & 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            ''.Cell(col(2) & 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            ''.Cell(col(2) & 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            ''.Cell(col(2) & 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")

            ''.Cell(col(5) & 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            ''.Cell(col(5) & 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            ''.Cell(col(5) & 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")

            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            'Dim fields() As Reflection.PropertyInfo = GetFields_Properties(_DataSet)

            Dim i As Integer = 3
            Dim j = 0

            For j = 0 To _DataSet.Columns.Count - 1

                wrkSheet.Cell(col(i) & 4).Value = _DataSet.Columns(j).ColumnName.ToString
                i += 1

            Next

            wrkSheet.SheetView.FreezeRows(4)

            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            Dim rows_ = 5
            For Each Drow As DataRow In _DataSet.Rows

                i = 3

                For j = 0 To _DataSet.Columns.Count - 1

                    If (Not IsNothing(Drow(_DataSet.Columns(j).ColumnName.ToString))) Then
                        wrkSheet.Cell(col(i) & rows_).Value = Drow(_DataSet.Columns(j).ColumnName.ToString)
                    Else
                        wrkSheet.Cell(col(i) & rows_).Value = DBNull.Value
                    End If

                    i += 1

                Next

                rows_ = rows_ + 1

            Next

            .Columns.AdjustToContents()

            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            Dim range1 = wrkSheet.Range(col(3) & 4, col(i - 1) & (rows_ - 1))
            range1.SetAutoFilter()

            wrkBook.Worksheet(1).Name = strTimeSheet_name
            'wrkBook.Worksheet(2).Visibility = XLWorksheetVisibility.VeryHidden

            Dim dateNow As DateTime = Date.UtcNow
            Dim strFile_Name As String = String.Format("{7}_U{0}_D{1}{2}{3}_T{4}{5}{6}", Me.Session("E_IdUser"), Right("0" & dateNow.Month, 2), Right("0" & dateNow.Day, 2), dateNow.Year, Right("0" & dateNow.Hour, 2), Right("0" & dateNow.Minute, 2), Right("0" & dateNow.Second, 2), strTimeSheet_name)

            Dim stream As MemoryStream = GetStream(wrkBook)

            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" + strFile_Name + ".xlsx")
            Response.ContentType = "application/vnd.openxmlformats-officedocument." + "spreadsheetml.sheet"
            Response.BinaryWrite(stream.ToArray())
            Response.End()


        End With



    End Sub



    Sub GenerateReport_(ByVal vYear As Integer, ByVal idProgram As Integer, ByVal vActivity As Integer, ByVal vImplementer As Integer, _DataSet As Object, strTimeSheet_name As String, strHeader As String)

        Dim col() As String = {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD",
                                   "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD",
                                   "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD",
                                   "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD",
                                   "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED",
                                   "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ"}

        Dim wrkBook = New XLWorkbook()
        Dim wrkSheet = wrkBook.AddWorksheet("sheet1")

        Dim bndYEar As Integer = If(vYear = -1, 1, 0)
        Dim bndImplementer As Integer = If(vImplementer = -1, 1, 0)
        Dim bndActivity As Integer = If(vActivity = -1, 1, 0)

        With wrkSheet

            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            .Range("A1:DD400").Style.Font.FontSize = 10
            .Range("A1:DD400").Style.Font.FontName = "Arial"

            '.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            '.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            '.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            '.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)

            .Range("B2:E4").Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")
            .Range("B2:E4").Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")

            Dim strValues() As String = strHeader.Split("||")

            ''Implementer, progress, activity, year

            .Cell(col(2) & 2).Value = strValues(2) '"<<--Achieved-->>"
            .Range(col(2) & 2 & ":" & col(3) & 4).Merge()
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Font.FontSize = 20
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Font.Bold = True
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Alignment.SetVertical(XLDrawingVerticalAlignment.Center)
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Alignment.SetHorizontal(XLDrawingHorizontalAlignment.Center)


            .Cell(col(4) & 2).Value = strValues(0)  '"<<--Implementer-->>"
            .Range(col(4) & 2 & ":" & col(5) & 2).Merge()
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Font.FontSize = 13
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Font.Bold = True


            .Cell(col(4) & 3).Value = strValues(4) '"<<--Activities-->>"
            .Range(col(4) & 3 & ":" & col(5) & 3).Merge()
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Font.FontSize = 10
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Font.Bold = True


            .Cell(col(4) & 4).Value = "Achieved - " & strValues(6) '"<<--Period of year-->>"
            .Range(col(4) & 4 & ":" & col(5) & 4).Merge()
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Font.FontSize = 10
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Font.Bold = True

            '.Cell(col(2) & 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            '.Cell(col(2) & 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            '.Cell(col(2) & 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            '.Cell(col(2) & 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")

            '.Cell(col(5) & 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            '.Cell(col(5) & 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            '.Cell(col(5) & 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")

            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            Dim fields() As Reflection.PropertyInfo = GetFields_Properties(_DataSet)
            Dim i As Integer = 3

            For Each field In fields
                wrkSheet.Cell(col(i) & 4).Value = field.Name
                i += 1
            Next

            wrkSheet.SheetView.FreezeRows(4)

            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            Dim rows_ = 8
            For Each item In _DataSet

                i = 3
                For Each field In fields

                    Dim p = item.GetType.GetProperty(field.Name)

                    If (Not IsNothing(p.GetValue(item, Nothing))) Then
                        wrkSheet.Cell(col(i) & rows_).Value = p.GetValue(item, Nothing)
                    Else
                        wrkSheet.Cell(col(i) & rows_).Value = DBNull.Value
                    End If

                    i += 1

                Next
                rows_ = rows_ + 1

            Next
            .Columns.AdjustToContents()
            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            Dim range1 = wrkSheet.Range(col(3) & 4, col(i - 1) & (rows_ - 1))
            range1.SetAutoFilter()

            wrkBook.Worksheet(1).Name = strTimeSheet_name
            'wrkBook.Worksheet(2).Visibility = XLWorksheetVisibility.VeryHidden

            Dim dateNow As DateTime = Date.UtcNow
            Dim strFile_Name As String = String.Format("{7}_U{0}_D{1}{2}{3}_T{4}{5}{6}", Me.Session("E_IdUser"), Right("0" & dateNow.Month, 2), Right("0" & dateNow.Day, 2), dateNow.Year, Right("0" & dateNow.Hour, 2), Right("0" & dateNow.Minute, 2), Right("0" & dateNow.Second, 2), strTimeSheet_name)

            Dim stream As MemoryStream = GetStream(wrkBook)

            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" + strFile_Name + ".xlsx")
            Response.ContentType = "application/vnd.openxmlformats-officedocument." + "spreadsheetml.sheet"
            Response.BinaryWrite(stream.ToArray())
            Response.End()


        End With



    End Sub

    Public Function GetStream(excelWorkbook As XLWorkbook) As Stream
        Dim fs As Stream = New MemoryStream()
        excelWorkbook.SaveAs(fs)
        fs.Position = 0
        Return fs
    End Function


    Public Function GetFields_Properties(Of t)(
                                              ByVal list As IList(Of t)
                                           ) As Reflection.PropertyInfo()

        Dim fields() = list.First.GetType.GetProperties

        'For Each field In fields

        '    If IsNullableType(field.PropertyType) Then
        '        Dim UnderlyingType As Type = Nullable.GetUnderlyingType(field.PropertyType)
        '        table.Columns.Add(field.Name, UnderlyingType)
        '    Else
        '        table.Columns.Add(field.Name, field.PropertyType)
        '    End If

        'Next
        'For Each item In list
        '    Dim row As DataRow = table.NewRow()
        '    For Each field In fields

        '        Dim p = item.GetType.GetProperty(field.Name)

        '        If (Not IsNothing(p.GetValue(item, Nothing))) Then
        '            row(field.Name) = p.GetValue(item, Nothing)
        '        Else
        '            row(field.Name) = DBNull.Value
        '        End If

        '        'If (Not p.GetValue(item, Nothing) Is Nothing) AndAlso IsNullableType(p.GetType) Then
        '        '    Dim UnderlyingType As Type = Nullable.GetUnderlyingType(p.GetType)
        '        '    row(field.Name) = p.GetValue(Convert.ChangeType(item, UnderlyingType), Nothing)
        '        'Else
        '        '    row(field.Name) = p.GetValue(item, Nothing)
        '        'End If

        '    Next
        '    table.Rows.Add(row)
        'Next
        'Return table

        Return fields

    End Function



    Sub GenerateReport_(ByVal vYear As Integer, ByVal idProgram As Integer, ByVal vActivity As Integer, ByVal vImplementer As Integer, _DataSet As DataTable, strTimeSheet_name As String, strHeader As String)

        Dim col() As String = {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD",
                                   "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD",
                                   "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD",
                                   "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD",
                                   "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED",
                                   "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ"}

        Dim wrkBook = New XLWorkbook()
        Dim wrkSheet = wrkBook.AddWorksheet("sheet1")

        Dim bndYEar As Integer = If(vYear = -1, 1, 0)
        Dim bndImplementer As Integer = If(vImplementer = -1, 1, 0)
        Dim bndActivity As Integer = If(vActivity = -1, 1, 0)

        With wrkSheet

            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            .Range("A1:DD400").Style.Font.FontSize = 10
            .Range("A1:DD400").Style.Font.FontName = "Arial"

            '.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            '.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            '.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            '.Range(col(2) & 2 & ":" & col(6) & 5).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)

            .Range("B2:E4").Style.Font.FontColor = XLColor.FromHtml("#FFFFFF")
            .Range("B2:E4").Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")

            Dim strValues() As String = strHeader.Split("||")

            ''Implementer, progress, activity, year

            .Cell(col(2) & 2).Value = strValues(2) '"<<--Achieved-->>"
            .Range(col(2) & 2 & ":" & col(3) & 4).Merge()
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Font.FontSize = 20
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Font.Bold = True
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Alignment.SetVertical(XLDrawingVerticalAlignment.Center)
            .Range(col(2) & 2 & ":" & col(3) & 4).Style.Alignment.SetHorizontal(XLDrawingHorizontalAlignment.Center)


            .Cell(col(4) & 2).Value = strValues(0)  '"<<--Implementer-->>"
            .Range(col(4) & 2 & ":" & col(5) & 2).Merge()
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Font.FontSize = 13
            .Range(col(4) & 2 & ":" & col(5) & 2).Style.Font.Bold = True


            .Cell(col(4) & 3).Value = strValues(4) '"<<--Activities-->>"
            .Range(col(4) & 3 & ":" & col(5) & 3).Merge()
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Font.FontSize = 10
            .Range(col(4) & 3 & ":" & col(5) & 3).Style.Font.Bold = True


            .Cell(col(4) & 4).Value = "Achieved - " & strValues(6) '"<<--Period of year-->>"
            .Range(col(4) & 4 & ":" & col(5) & 4).Merge()
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.OutsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.InsideBorderColor = XLColor.FromHtml("#FFFFFF")
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Font.FontSize = 10
            .Range(col(4) & 4 & ":" & col(5) & 4).Style.Font.Bold = True

            '.Cell(col(2) & 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            '.Cell(col(2) & 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            '.Cell(col(2) & 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            '.Cell(col(2) & 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")

            '.Cell(col(5) & 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            '.Cell(col(5) & 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")
            '.Cell(col(5) & 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#00a65a")

            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            'Dim fields() As Reflection.PropertyInfo = GetFields_Properties(_DataSet)

            Dim i As Integer = 3
            Dim j = 0

            For j = 0 To _DataSet.Columns.Count - 1

                wrkSheet.Cell(col(i) & 4).Value = _DataSet.Columns(j).ColumnName.ToString
                i += 1

            Next

            wrkSheet.SheetView.FreezeRows(4)

            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            Dim rows_ = 5
            For Each Drow As DataRow In _DataSet.Rows

                i = 3

                For j = 0 To _DataSet.Columns.Count - 1

                    If (Not IsNothing(Drow(_DataSet.Columns(j).ColumnName.ToString))) Then
                        wrkSheet.Cell(col(i) & rows_).Value = Drow(_DataSet.Columns(j).ColumnName.ToString)
                    Else
                        wrkSheet.Cell(col(i) & rows_).Value = DBNull.Value
                    End If

                    i += 1

                Next

                rows_ = rows_ + 1

            Next

            .Columns.AdjustToContents()

            '--****************************************************************************************************************
            '--****************************************************************************************************************
            '--****************************************************************************************************************

            Dim range1 = wrkSheet.Range(col(3) & 4, col(i - 1) & (rows_ - 1))
            range1.SetAutoFilter()

            wrkBook.Worksheet(1).Name = strTimeSheet_name
            'wrkBook.Worksheet(2).Visibility = XLWorksheetVisibility.VeryHidden

            Dim dateNow As DateTime = Date.UtcNow
            Dim strFile_Name As String = String.Format("{7}_U{0}_D{1}{2}{3}_T{4}{5}{6}", Me.Session("E_IdUser"), Right("0" & dateNow.Month, 2), Right("0" & dateNow.Day, 2), dateNow.Year, Right("0" & dateNow.Hour, 2), Right("0" & dateNow.Minute, 2), Right("0" & dateNow.Second, 2), strTimeSheet_name)

            Dim stream As MemoryStream = GetStream(wrkBook)

            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" + strFile_Name + ".xlsx")
            Response.ContentType = "application/vnd.openxmlformats-officedocument." + "spreadsheetml.sheet"
            Response.BinaryWrite(stream.ToArray())
            Response.End()


        End With



    End Sub


    Public Function ReportHeader(ByVal vYear As Integer, ByVal idProgram As Integer, ByVal vActivity As Integer, ByVal vImplementer As Integer, ByVal id_indicator As Integer) As String

        Dim bndYEar As Integer = If(vYear = -1, 1, 0)
        Dim bndImplementer As Integer = If(vImplementer = -1, 1, 0)
        Dim bndActivity As Integer = If(vActivity = -1, 1, 0)

        Dim cl_Util As CORE.cl_RMS_Results = New CORE.cl_RMS_Results(idProgram)

        Dim indicadores As Object = cl_Util.get_Achieved_Result(vYear, vActivity, vImplementer, id_indicator)

        'Using dbEntities As New dbRMS_JIEntities


        '    indicadores = dbEntities.vw_tme_ficha_meta_indicador_Year_Period.Where(Function(p) p.id_programa = idProgram And p.id_ficha_estado <> 3 _
        '                                                                                    And (p.anio = vYear Or 1 = bndYEar) _
        '                                                                                    And (p.id_ficha_proyecto = vActivity Or 1 = bndActivity) _
        '                                                                                    And (p.id_ejecutor = vImplementer Or 1 = bndImplementer)) _
        '                                                                           .GroupBy(Function(g) New With {g.orden_matriz_LB,
        '                                                                                                           g.id_indicador,
        '                                                                                                           g.codigo_indicador,
        '                                                                                                           g.nombre_indicador_LB,
        '                                                                                                           g.nombre_metodo_operacion}) _
        '                                                                          .Select(Function(s) New With {.orden_matriz_LB = s.FirstOrDefault.orden_matriz_LB,
        '                                                                                                           .id_indicador = s.FirstOrDefault.id_indicador,
        '                                                                                                           .codigo_indicador = s.FirstOrDefault.codigo_indicador,
        '                                                                                                           .nombre_indicador_LB = s.FirstOrDefault.nombre_indicador_LB,
        '                                                                                                           .nombre_metodo_operacion = s.FirstOrDefault.nombre_metodo_operacion,
        '                                                                                                           .Q1 = s.Sum(Function(m) m.Q1),
        '                                                                                                           .Q2 = s.Sum(Function(m) m.Q2),
        '                                                                                                           .Q3 = s.Sum(Function(m) m.Q3),
        '                                                                                                           .Q4 = s.Sum(Function(m) m.Q4),
        '                                                                                                           .meta_total = s.Sum(Function(m) m.meta_total),
        '                                                                                                           .Total_Progress = s.Sum(Function(m) m.Total_Progress),
        '                                                                                                           .Porcen_Progress = If(s.Sum(Function(m) m.meta_total) = 0, 0, ((s.Sum(Function(m) m.Total_Progress) / s.Sum(Function(m) m.meta_total)) * 100))}) _
        '                                                                           .OrderBy(Function(o) o.orden_matriz_LB).ToList()



        'End Using

        Dim i = 0
        Dim TotProgress As Decimal = 0


        If indicadores.count > 0 Then

            For Each item In indicadores

                TotProgress += If(item.Porcen_Progress > 100, 100, item.Porcen_Progress)
                i = i + 1

            Next

        End If

        Dim strOverAll_progress As Double
        Dim strImplementer As String = ""
        Dim strActivity As String = ""
        Dim strYear As String = ""

        strOverAll_progress = If(i = 0, 0, Math.Round((TotProgress / i), 2, MidpointRounding.AwayFromZero))

        Dim listActivity As Object
        Dim nActivities As Integer = 0
        Dim listImplementer As Object
        Dim nImplementer As Integer = 0

        Using dbEntities As New dbRMS_JIEntities

            listImplementer = (From IM In dbEntities.t_ejecutores
                               Join AC In dbEntities.tme_Ficha_Proyecto On IM.id_ejecutor Equals AC.id_ejecutor
                               Join TP In dbEntities.tme_organization_type On IM.id_organization_type Equals TP.id_organization_type
                               Where ((AC.id_ficha_estado <> 3) And (IM.id_ejecutor = vImplementer Or bndImplementer = 1))
                               Select New With {.id_ejecutor = IM.id_ejecutor,
                                                    .Implementer = IM.nombre_ejecutor,
                                                    .organization_type = TP.organization_type} Distinct).ToList()


            If listImplementer.Count = 1 Then
                strImplementer = listImplementer.Item(0).Implementer
            ElseIf listImplementer.Count > 1 Then
                strImplementer = listImplementer.Count & " Implementers"
            Else
                strImplementer = "No Implementer"
            End If

            '*************************************************************************************************************************************
            '*************************************************************************************************************************************
            '*************************************************************************************************************************************

            listActivity = dbEntities.vw_tme_ficha_proyecto.Where(Function(f) (f.id_ejecutor = vImplementer Or bndImplementer = 1) _
                                                                                And (f.id_ficha_proyecto = vActivity Or bndActivity = 1) _
                                                                                And f.id_ficha_estado <> 3) _
                                             .Select(Function(s) New With {Key .codigo_SAPME = s.codigo_SAPME,
                                                                               .nombre_proyecto = s.nombre_proyecto}).ToList()

            If listActivity.Count = 1 Then
                strActivity = "In " & listActivity.Item(0).codigo_SAPME & " - " & listActivity.Item(0).nombre_proyecto
            ElseIf (listActivity.count > 1 And bndImplementer = 0) Then
                strActivity = "In "
                For Each item In listActivity
                    strActivity &= item.codigo_SAPME & " - "
                Next
                strActivity = strActivity.Trim.TrimEnd("-")
            ElseIf (listActivity.count > 1 And bndImplementer = 1) Then
                strActivity = "In " & listActivity.Count & " Activities"
            Else
                strActivity = "In 1 Actitivie"
            End If

            If (bndYEar = 1) Then
                strYear = String.Format(" Overall life of project{0} ", If(listActivity.Count > 1, "s", ""))
            Else
                strYear = String.Format(" {0} ", vYear)
            End If

            '*************************************************************************************************************************************
            '*************************************************************************************************************************************
            '*************************************************************************************************************************************

            Dim strResult As String = strImplementer.Trim & "||" & String.Format("{0:N2}%", strOverAll_progress).Trim & "||" & strActivity.Trim & "||" & strYear.Trim

            Return strResult

        End Using



    End Function



End Class