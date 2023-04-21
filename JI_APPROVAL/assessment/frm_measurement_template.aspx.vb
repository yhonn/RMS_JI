Imports System.IO
Imports ClosedXML.Excel
Imports ly_SIME
Imports System.Data.SqlClient

Public Class frm_measurement_template
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using dbEntities As New dbRMS_JIEntities

            Dim id_reporte = Convert.ToInt32(Me.Request.QueryString("id"))
            Dim id_survey = dbEntities.vw_ins_measurement.FirstOrDefault(Function(p) p.id_measurement = id_reporte).id_measurement_survey

            If id_survey = 6 Then ' New Aflatoon
                generarReporteAFLATON(id_reporte)
            Else
                generarReporte(id_reporte)
            End If



        End Using
    End Sub

    Sub generarReporteAFLATON(ByVal reporte As Integer)
        Dim Filter = reporte

        Using dbEntities As New dbRMS_JIEntities

            Dim sex_type = dbEntities.tme_sex_type.ToList()
            Dim class_level = Nothing ' dbEntities.tme_class_level.ToList()
            Dim school_status = dbEntities.tme_schooling_status.ToList()
            Dim answer_options = dbEntities.tme_measurement_answer_option.OrderBy(Function(p) p.id_measurement_answer_scale).ThenBy(Function(t) t.id_measurement_answer_option).ToList()

            Dim archivo As String

            Dim col() As String = {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD",
                                   "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD",
                                   "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD",
                                   "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD",
                                   "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED",
                                   "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ"}
            Dim wrkBook = New XLWorkbook()
            Dim wrkSheet = wrkBook.AddWorksheet("sheet1")

            Dim query = "SELECT DISTINCT id_measurement, participant_number, id_measurement_survey, name, moderator, answer_date, survey_name, codigo_SAPME, codigo_ficha_AID, nombre_proyecto, nombre_district, "
            query &= "order_number_title, order_number, percent_value_question, id_measurement_question, question_name, color, title_name, id_measurement_answer_scale, id_measurement_question_config, id_measurement_title, school_type "
            query &= " FROM vw_ins_measurement_detail_options where id_measurement ="

            Dim wrkSheet2 = wrkBook.AddWorksheet("sheet3")
            wrkBook.Worksheet(2).Name = "Listados"

            Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("dbCI_SAPConnectionString").ConnectionString
            Dim TotCols As Integer = 0
            Dim total_part = 0
            Dim total_filas = 0

            With wrkSheet
                .Range("A1:DD400").Style.Font.FontSize = 10
                .Range("A1:DD400").Style.Font.FontName = "Arial"

                .Range(col(2) & 1 & ":" & col(6) & 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Range(col(2) & 1 & ":" & col(6) & 3).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Range(col(2) & 1 & ":" & col(6) & 3).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                .Range(col(2) & 1 & ":" & col(6) & 3).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)

                .Range(col(2) & 1 & ":" & col(5) & 1).Row(1).Merge()
                .Cell(col(2) & 1).Value = "Is this a treatment school or a control school"
                .Cell(col(2) & 2).Value = "School Name"
                .Cell(col(2) & 3).Value = "Moderator"
                .Cell(col(5) & 2).Value = "Survey Date"
                .Cell(col(5) & 3).Value = "Number of Participants"

                .Cell(col(2) & 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#3498db")
                .Cell(col(2) & 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#3498db")
                .Cell(col(2) & 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#3498db")
                .Cell(col(5) & 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#3498db")
                .Cell(col(5) & 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#3498db")


                'wrkSheet.Cell(col(2) & 7).Value = "Sex Type"
                'wrkSheet.Cell(col(3) & 7).Value = "Schooling Status"
                'wrkSheet.Cell(col(4) & 7).Value = "Class Level"
                'wrkSheet.Cell(col(5) & 7).Value = "Age"

                .Range(col(2) & 6 & ":" & col(5) & 6).Row(1).Merge()
                wrkSheet.Cell(col(2) & 6).Value = "RESPONDENTS"
                .Cell(col(2) & 6).Style.Font.Bold = True
                '.Cell(col(2) & 6).Style.Font.FontColor = XLColor.White
                .Cell(col(2) & 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center

                wrkSheet.Cell(col(2) & 7).Value = "Students ID"
                wrkSheet.Cell(col(3) & 7).Value = "Gender/Sex"
                wrkSheet.Cell(col(4) & 7).Value = "Schooling Status"
                wrkSheet.Cell(col(5) & 7).Value = "Class"
                wrkSheet.Cell(col(6) & 7).Value = "Age"
                wrkSheet.SheetView.FreezeColumns(6)
                wrkSheet.SheetView.FreezeRows(7)



                Using conn As New SqlConnection(connectionString)
                    Dim cmdText As String = query & Filter & " order by order_number_title, order_number"

                    Dim cmd As New SqlCommand(cmdText, conn)


                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        'Columa inicial de las preguntas
                        Dim columna = 7
                        While reader.Read

                            Dim participant_number = Convert.ToInt32(reader("participant_number"))

                            .Range(col(3) & 2 & ":" & col(4) & 2).Row(1).Merge()
                            .Cell(col(3) & 2).Value = reader("name")
                            .Range(col(3) & 3 & ":" & col(4) & 3).Row(1).Merge()
                            .Cell(col(3) & 3).Value = reader("moderator")

                            .Cell(col(6) & 1).Value = reader("school_type").ToString()
                            .Cell(col(6) & 2).Value = reader("answer_date")
                            .Cell(col(6) & 3).Value = participant_number

                            .Cell(col(columna) & 5).Value = reader("id_measurement_question")
                            .Cell(col(columna) & 6).Value = reader("title_name")
                            .Row(5).Hide()

                            .Cell(col(columna) & 6).Style.Font.Bold = True
                            .Cell(col(columna) & 6).Style.Font.FontColor = XLColor.White
                            .Cell(col(columna) & 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center

                            .Range(col(columna) & 6 & ":" & col(columna) & (6 + participant_number + 1)).Style.Fill.BackgroundColor = XLColor.FromHtml(reader("color"))
                            .Cell(col(columna) & 7).Style.Alignment.WrapText = True
                            .Cell(col(columna) & 7).Value = reader("question_name")
                            '.Columns(columna).AdjustToContents(7)
                            .Columns(columna).Width = 60
                            '.Row(7).AdjustToContents()

                            'For item = 6 To Convert.ToInt32(reader("participant_number")) + 5
                            '    '.Cell(col(filas) & item).Value = reader("name")
                            '    Dim celda = wrkSheet.Cell(col(filas) & item)
                            '    celda.DataValidation.List(wrkSheet2.Range(celdasLista))
                            'Next

                            .Range(col(2) & 6 & ":" & col(columna) & (6 + participant_number + 1)).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                            .Range(col(2) & 6 & ":" & col(columna) & (6 + participant_number + 1)).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
                            .Range(col(2) & 6 & ":" & col(columna) & (6 + participant_number + 1)).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                            .Range(col(2) & 6 & ":" & col(columna) & (6 + participant_number + 1)).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)

                            total_filas = columna
                            columna = columna + 1
                            total_part = participant_number
                        End While

                        TotCols = columna - 1
                    End Using
                    conn.Close()
                End Using


                .Range(col(2) & 7, col(total_filas) & (7 + total_part)).Style.Protection.SetLocked(False)
                '.Columns.AdjustToContents()

            End With



            With wrkSheet2

                Using conn As New SqlConnection(connectionString)
                    Dim cmdText As String = query & Filter & " order by order_number_title, order_number"

                    Dim cmd As New SqlCommand(cmdText, conn)

                    Dim numero = 1
                    For Each item In sex_type
                        .Cell(col(1) & numero).Value = "'" + item.sex_type
                        .Cell(col(1) & numero).Style.NumberFormat.NumberFormatId = 0
                        numero = numero + 1
                    Next


                    numero = 1
                    For Each item In school_status
                        .Cell(col(2) & numero).Value = "'" + item.schooling_status
                        .Cell(col(2) & numero).Style.NumberFormat.NumberFormatId = 0
                        numero = numero + 1
                    Next


                    numero = 1
                    For Each item In class_level
                        .Cell(col(3) & numero).Value = "'" + item.class_level_name
                        .Cell(col(3) & numero).Style.NumberFormat.NumberFormatId = 0
                        numero = numero + 1
                    Next

                    numero = 1
                    For Each item In answer_options
                        .Cell(col(4) & numero).Value = item.id_measurement_answer_scale
                        .Cell(col(4) & numero).Style.NumberFormat.NumberFormatId = 0
                        .Cell(col(5) & numero).Value = "'" + item.option_name
                        .Cell(col(5) & numero).Style.NumberFormat.NumberFormatId = 0
                        numero = numero + 1
                    Next


                    Dim celdasLista = ""
                    celdasLista = col(1) & 1 & ":" & col(1) & (sex_type.Count())

                    Dim celdasSchooling = ""
                    celdasSchooling = col(2) & 1 & ":" & col(2) & (school_status.Count())

                    Dim celdasClass = ""
                    celdasClass = col(3) & 1 & ":" & col(3) & (class_level.Count())

                    Dim celdaRespuestas = ""


                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        'Columna inicial de las preguntas
                        Dim columna = 7
                        While reader.Read
                            Dim range_options As New List(Of Integer)
                            Dim id_scale = Convert.ToInt32(reader("id_measurement_answer_scale"))
                            For item = 1 To answer_options.Count()
                                If .Cell(col(4) & item).Value = id_scale Then
                                    range_options.Add(item)
                                End If
                            Next

                            celdaRespuestas = col(5) & range_options.FirstOrDefault() & ":" & col(5) & range_options.LastOrDefault()

                            For item = 8 To Convert.ToInt32(reader("participant_number")) + 7


                                '.Cell(col(filas) & item).Value = reader("name")
                                Dim celda = wrkSheet.Cell(col(3) & item)
                                celda.DataValidation.List(wrkSheet2.Range(celdasLista))

                                Dim celda2 = wrkSheet.Cell(col(4) & item)
                                celda2.DataValidation.List(wrkSheet2.Range(celdasSchooling))

                                Dim celda3 = wrkSheet.Cell(col(5) & item)
                                celda3.DataValidation.List(wrkSheet2.Range(celdasClass))

                                Dim celdaInt = wrkSheet.Cell(col(6) & item)
                                celdaInt.DataValidation.Decimal.GreaterThan(0)

                                Dim celdasR = wrkSheet.Cell(col(columna) & item)
                                celdasR.DataValidation.List(wrkSheet2.Range(celdaRespuestas))
                            Next
                            columna = columna + 1
                        End While

                    End Using
                    conn.Close()
                End Using
                .Columns.AdjustToContents()
            End With

            Dim range1 = wrkSheet.Range(col(2) & 7, col(TotCols) & (7 + total_part))
            range1.SetAutoFilter()

            wrkBook.Worksheet(1).Name = "TemplateSkills"
            wrkBook.Worksheet(2).Visibility = XLWorksheetVisibility.VeryHidden
            'wrkBook.Worksheet(1).Protect("RMSSystem2017")
            Dim protection = wrkBook.Worksheet(1).Protect("RMSSystem2017")

            'protection.FormatColumns = True
            'protection.FormatRows = True
            'protection.Sort = True
            'protection.AutoFilter = True


            Dim tip As String

            tip = "TemplateSkills"
            archivo = tip & String.Format("_ID{0}_D{1}{2}{3}_{4}{5}_{6}", reporte, Date.UtcNow.Day, Date.UtcNow.Month, Date.UtcNow.Year, Date.UtcNow.Hour, Date.UtcNow.Minute, Date.UtcNow.Second)

            Dim stream As MemoryStream = GetStream(wrkBook)

            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" + archivo + ".xlsx")
            Response.ContentType = "application/vnd.openxmlformats-officedocument." + "spreadsheetml.sheet"
            Response.BinaryWrite(stream.ToArray())
            Response.End()
        End Using
    End Sub

    Sub generarReporte(ByVal reporte As Integer)
        Dim Filter = reporte

        Using dbEntities As New dbRMS_JIEntities
            Dim sex_type = dbEntities.tme_sex_type.ToList()
            Dim class_level = Nothing 'dbEntities.tme_class_level.ToList()
            Dim school_status = dbEntities.tme_schooling_status.ToList()
            Dim answer_options = dbEntities.tme_measurement_answer_option.ToList()

            Dim archivo As String

            Dim col() As String = {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD",
                                   "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD",
                                   "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD",
                                   "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD",
                                   "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED",
                                   "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ"}
            Dim wrkBook = New XLWorkbook()
            Dim wrkSheet = wrkBook.AddWorksheet("sheet1")

            Dim query = "SELECT DISTINCT id_measurement, participant_number, id_measurement_survey, name, moderator, answer_date, survey_name, codigo_SAPME, codigo_ficha_AID, nombre_proyecto, nombre_district, "
            query &= "order_number, percent_value_question, id_measurement_question, question_name, color, title_name, id_measurement_answer_scale, id_measurement_question_config, id_measurement_title"
            query &= " FROM vw_ins_measurement_detail_options where id_measurement ="

            Dim wrkSheet2 = wrkBook.AddWorksheet("sheet3")
            wrkBook.Worksheet(2).Name = "Listados"

            Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("dbCI_SAPConnectionString").ConnectionString

            With wrkSheet
                .Range("A1:DD400").Style.Font.FontSize = 10
                .Range("A1:DD400").Style.Font.FontName = "Arial"

                .Range(col(2) & 2 & ":" & col(6) & 3).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Range(col(2) & 2 & ":" & col(6) & 3).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Range(col(2) & 2 & ":" & col(6) & 3).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                .Range(col(2) & 2 & ":" & col(6) & 3).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Cell(col(2) & 2).Value = "School Name"
                .Cell(col(2) & 3).Value = "Moderator"
                .Cell(col(5) & 2).Value = "Survey Date"
                .Cell(col(5) & 3).Value = "Number of Participants"

                .Cell(col(2) & 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#3498db")
                .Cell(col(2) & 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#3498db")
                .Cell(col(5) & 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#3498db")
                .Cell(col(5) & 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#3498db")


                wrkSheet.Cell(col(2) & 7).Value = "Sex Type"
                wrkSheet.Cell(col(3) & 7).Value = "Schooling Status"
                wrkSheet.Cell(col(4) & 7).Value = "Class Level"
                wrkSheet.Cell(col(5) & 7).Value = "Age"
                wrkSheet.SheetView.FreezeColumns(5)
                wrkSheet.SheetView.FreezeRows(7)
                Dim total_part = 0
                Dim total_filas = 0

                Using conn As New SqlConnection(connectionString)
                    Dim cmdText As String = query & Filter & " order by id_measurement_title, order_number"

                    Dim cmd As New SqlCommand(cmdText, conn)


                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        'Columa inicial de las preguntas
                        Dim columna = 6
                        While reader.Read
                            Dim participant_number = Convert.ToInt32(reader("participant_number"))
                            .Range(col(3) & 2 & ":" & col(4) & 2).Row(1).Merge()
                            .Cell(col(3) & 2).Value = reader("name")
                            .Range(col(3) & 3 & ":" & col(4) & 3).Row(1).Merge()
                            .Cell(col(3) & 3).Value = reader("moderator")
                            .Cell(col(6) & 2).Value = reader("answer_date")
                            .Cell(col(6) & 3).Value = participant_number



                            .Cell(col(columna) & 5).Value = reader("id_measurement_question")
                            .Cell(col(columna) & 6).Value = reader("title_name")
                            .Row(5).Hide()
                            .Cell(col(columna) & 6).Style.Font.Bold = True
                            .Cell(col(columna) & 6).Style.Font.FontColor = XLColor.White
                            .Cell(col(columna) & 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center

                            .Range(col(columna) & 6 & ":" & col(columna) & (6 + participant_number + 1)).Style.Fill.BackgroundColor = XLColor.FromHtml(reader("color"))

                            .Cell(col(columna) & 7).Value = reader("question_name")

                            'For item = 6 To Convert.ToInt32(reader("participant_number")) + 5
                            '    '.Cell(col(filas) & item).Value = reader("name")
                            '    Dim celda = wrkSheet.Cell(col(filas) & item)
                            '    celda.DataValidation.List(wrkSheet2.Range(celdasLista))
                            'Next

                            .Range(col(2) & 6 & ":" & col(columna) & (6 + participant_number + 1)).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin)
                            .Range(col(2) & 6 & ":" & col(columna) & (6 + participant_number + 1)).Style.Border.SetTopBorder(XLBorderStyleValues.Thin)
                            .Range(col(2) & 6 & ":" & col(columna) & (6 + participant_number + 1)).Style.Border.SetRightBorder(XLBorderStyleValues.Thin)
                            .Range(col(2) & 6 & ":" & col(columna) & (6 + participant_number + 1)).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)

                            total_filas = columna
                            columna = columna + 1
                            total_part = participant_number
                        End While

                    End Using
                    conn.Close()
                End Using


                .Range(col(2) & 7, col(total_filas) & (7 + total_part)).Style.Protection.SetLocked(False)
                .Columns.AdjustToContents()

            End With



            With wrkSheet2
                Using conn As New SqlConnection(connectionString)
                    Dim cmdText As String = query & Filter & " order by id_measurement_title, order_number"

                    Dim cmd As New SqlCommand(cmdText, conn)
                    
                    Dim numero = 1
                    For Each item In sex_type
                        .Cell(col(1) & numero).Value = "'" + item.sex_type
                        .Cell(col(1) & numero).Style.NumberFormat.NumberFormatId = 0
                        numero = numero + 1
                    Next

                    
                    numero = 1
                    For Each item In school_status
                        .Cell(col(2) & numero).Value = "'" + item.schooling_status
                        .Cell(col(2) & numero).Style.NumberFormat.NumberFormatId = 0
                        numero = numero + 1
                    Next


                    numero = 1
                    For Each item In class_level
                        .Cell(col(3) & numero).Value = "'" + item.class_level_name
                        .Cell(col(3) & numero).Style.NumberFormat.NumberFormatId = 0
                        numero = numero + 1
                    Next

                    numero = 1
                    For Each item In answer_options
                        .Cell(col(4) & numero).Value = item.id_measurement_answer_scale
                        .Cell(col(4) & numero).Style.NumberFormat.NumberFormatId = 0
                        .Cell(col(5) & numero).Value = "'" + item.option_name
                        .Cell(col(5) & numero).Style.NumberFormat.NumberFormatId = 0
                        numero = numero + 1
                    Next


                    Dim celdasLista = ""
                    celdasLista = col(1) & 1 & ":" & col(1) & (sex_type.Count())

                    Dim celdasSchooling = ""
                    celdasSchooling = col(2) & 1 & ":" & col(2) & (school_status.Count())

                    Dim celdasClass = ""
                    celdasClass = col(3) & 1 & ":" & col(3) & (class_level.Count())

                    Dim celdaRespuestas = ""


                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        'Columna inicial de las preguntas
                        Dim columna = 6

                        While reader.Read
                            Dim range_options As New List(Of Integer)
                            Dim id_scale = Convert.ToInt32(reader("id_measurement_answer_scale"))
                            For item = 1 To answer_options.Count()
                                If .Cell(col(4) & item).Value = id_scale Then
                                    range_options.Add(item)
                                End If
                            Next

                            celdaRespuestas = col(5) & range_options.FirstOrDefault() & ":" & col(5) & range_options.LastOrDefault()

                            For item = 8 To Convert.ToInt32(reader("participant_number")) + 7
                                '.Cell(col(filas) & item).Value = reader("name")
                                Dim celda = wrkSheet.Cell(col(2) & item)
                                celda.DataValidation.List(wrkSheet2.Range(celdasLista))

                                Dim celda2 = wrkSheet.Cell(col(3) & item)
                                celda2.DataValidation.List(wrkSheet2.Range(celdasSchooling))

                                Dim celda3 = wrkSheet.Cell(col(4) & item)
                                celda3.DataValidation.List(wrkSheet2.Range(celdasClass))

                                Dim celdasR = wrkSheet.Cell(col(columna) & item)
                                celdasR.DataValidation.List(wrkSheet2.Range(celdaRespuestas))
                            Next
                            columna = columna + 1
                        End While

                    End Using
                    conn.Close()
                End Using
                .Columns.AdjustToContents()
            End With

            wrkBook.Worksheet(1).Name = "TemplateSkills"
            wrkBook.Worksheet(2).Visibility = XLWorksheetVisibility.VeryHidden
            wrkBook.Worksheet(1).Protect("RMSSystem2017")


            Dim tip As String

            tip = "TemplateSkills"

            Dim aleatorio As String
            Dim rnd As New Random()
            aleatorio = rnd.Next(1, 999).ToString & Now.Date.ToString("ddMMyyyy")
            archivo = tip & aleatorio
            Dim stream As MemoryStream = GetStream(wrkBook)

            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" + archivo + ".xlsx")
            Response.ContentType = "application/vnd.openxmlformats-officedocument." + "spreadsheetml.sheet"
            Response.BinaryWrite(stream.ToArray())
            Response.End()
        End Using
    End Sub

    Public Function GetStream(excelWorkbook As XLWorkbook) As Stream
        Dim fs As Stream = New MemoryStream()
        excelWorkbook.SaveAs(fs)
        fs.Position = 0
        Return fs
    End Function

End Class