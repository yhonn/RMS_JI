Imports ly_SIME
Imports System.IO
Imports ClosedXML.Excel
Imports System.Data.SqlClient

Public Class xls
    Inherits System.Web.UI.Page
    Dim cl_user As New ly_SIME.CORE.cls_user

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim idReporte = Convert.ToInt32(Me.Request.QueryString("id"))
        Dim idSession = Convert.ToString(Me.Request.QueryString("idS"))
        Dim id_ficha = Convert.ToInt32(Me.Session("E_IdFicha"))
        cl_user = Session.Item("clUser")
        Dim filter As String = ""
        If idReporte > 0 Then
            reportes(idReporte)
        End If
        If idSession IsNot Nothing Then
            errorBeneficios(idSession)
        End If
    End Sub
    Sub errorBeneficios(ByVal id_session As String)

        Using dbEntities As New dbRMS_JIEntities
            Dim archivo As String

            Dim col() As String = {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ"}
            Dim wrkBook = New XLWorkbook()
            Dim wrkSheet = wrkBook.AddWorksheet("sheet1")

            With wrkSheet
                .Range("A1:DD400").Style.Font.FontSize = 10
                .Range("A1:DD400").Style.Font.FontName = "Arial"
                Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("dbCI_SAPConnectionString").ConnectionString

                Using conn As New SqlConnection(connectionString)
                    Dim cmdText As String = "select nombre_beneficiario 'Nombre beneficiario', cedula_beneficiario 'Cédula', error 'Error' from tmp_error_beneficios where id_session = '" & id_session & "'"

                    Dim cmd As New SqlCommand(cmdText, conn)
                    'Dim idParam As New SqlParameter("@ImageID", SqlDbType.Int)
                    'idParam.Value = ID

                    'cmd.Parameters.Add(idParam)
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        Dim filas = 2
                        While reader.Read

                            For i = 0 To reader.FieldCount - 1
                                .Cell(col(i + 1) & 1).Value = reader.GetName(i)
                                Dim tt = reader.GetFieldType(i)
                                If (reader.GetFieldType(0) Is GetType(Double)) Then
                                    .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 40
                                Else
                                    If (reader.GetFieldType(0) Is GetType(Date)) Then
                                        .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 15
                                    End If
                                End If

                                .Cell(col(i + 1) & filas).Value = reader(i)
                            Next
                            filas = filas + 1
                        End While

                    End Using
                End Using


                '.Cell("A2").Value = datasource
                .Columns.AdjustToContents()
            End With

            wrkBook.Worksheet(1).Name = "Listado"

            Dim tip As String

            tip = "Listado"

            Dim aleatorio As String
            Dim rnd As New Random()
            aleatorio = rnd.Next(1, 999).ToString & Now.Date.ToString("ddMMyyyy")
            archivo = tip & aleatorio & ".xlsx"
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
    Public Sub reportes(ByVal idReporte As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim reporte = dbEntities.tme_reportes.Find(idReporte)
            Dim archivo As String

            Dim id_usuario = Convert.ToInt32(Me.Session("E_IdUser").ToString())
            Dim id_filtro_busqueda_viajes = Convert.ToInt32(cl_user.getUsuarioField("id_filtro_busqueda_viajes", "id_usuario", Me.Session("E_IdUser")))
            Dim id_programa = Me.Session("E_IDPrograma").ToString()
            Dim id_tipo_usuario = cl_user.getUsuarioField("id_tipo_usuario", "id_usuario", Me.Session("E_IdUser"))
            Dim busqueda_programa = cl_user.getUsuarioField("busqueda_actividad", "id_usuario", Me.Session("E_IdUser"))
            Dim ListPro = cl_user.chk_accPRO()
            Dim id_ejecutor = 0
            If id_tipo_usuario = 2 Then
                Dim ejecutor = dbEntities.t_ejecutor_usuario.Where(Function(p) p.id_usuario = id_usuario)
                If ejecutor.Count() > 0 Then
                    id_ejecutor = ejecutor.FirstOrDefault().id_ejecutor
                End If
            End If

            Dim col() As String = {"", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ"}
            Dim wrkBook = New XLWorkbook()
            Dim wrkSheet = wrkBook.AddWorksheet(reporte.nombre_hoja)
            Dim wrkSheet2 = wrkBook.AddWorksheet("Sheet 2")
            Dim wrkSheet3 = wrkBook.AddWorksheet("Sheet 3")
            Dim wrkSheet4 = wrkBook.AddWorksheet("Sheet 4")
            Dim wrkSheet5 = wrkBook.AddWorksheet("Sheet 5")

            Dim queryFilter = " where 0 = 0"

            Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("dbCI_SAPConnectionString").ConnectionString
            Using conn As New SqlConnection(connectionString)
                Dim cmdText As String = reporte.query
                If idReporte = 4 Then
                    queryFilter = ""
                    Dim anio = 0
                    Dim estado = 0
                    Dim caso = 1
                    If Me.Request.QueryString("anio") IsNot Nothing Then
                        anio = Convert.ToInt32(Me.Request.QueryString("anio"))
                    End If
                    If Me.Request.QueryString("estado") IsNot Nothing Then
                        estado = Convert.ToInt32(Me.Request.QueryString("estado"))
                    End If
                    If anio > 0 And estado > 0 Then
                        caso = 4
                    ElseIf anio > 0 And estado = 0 Then
                        caso = 2
                    ElseIf anio = 0 And estado > 0 Then
                        caso = 3
                    Else
                        caso = 1
                    End If

                    If busqueda_programa = True Then
                        cmdText = cmdText & " and b.id_ficha_proyecto in (" & String.Join(",", ListPro.ToArray) & ")"
                    End If
                    If id_tipo_usuario = 2 Then
                        cmdText = cmdText & " and b.id_ejecutor = " & id_ejecutor
                    End If
                    Select Case caso
                        Case 2
                            cmdText = cmdText & " and (case when MONTH(b.fecha_inicio_proyecto) > 9 and MONTH(b.fecha_inicio_proyecto) <= 12 then YEAR(b.fecha_inicio_proyecto) + 1 else YEAR(b.fecha_inicio_proyecto) end) = " & anio
                        Case 3
                            cmdText = cmdText & " and a.id_estado_entregable = " & estado
                        Case 4
                            cmdText = cmdText & " and (case when MONTH(b.fecha_inicio_proyecto) > 9 and MONTH(b.fecha_inicio_proyecto) <= 12 then YEAR(b.fecha_inicio_proyecto) + 1 else YEAR(b.fecha_inicio_proyecto) end) = " & anio & " and a.id_estado_entregable = " & estado
                    End Select
                    cmdText = cmdText & " order by a.id_ficha_proyecto "
                End If
                If idReporte = 10 Then
                    cmdText = cmdText & " where id_ficha_proyecto = " & Convert.ToInt32(Me.Session("E_IdFicha"))
                End If

                Dim subR = dbEntities.t_usuario_subregion.Where(Function(p) p.id_usuario = id_usuario).Select(Function(p) p.id_subregion).ToList()
                Dim listSubR = String.Join(",", subR)

                If idReporte = 1 Or idReporte = 2 Then

                    Select Case id_filtro_busqueda_viajes
                        Case 1

                        Case 2
                            queryFilter &= " and id_sub_region in (" & listSubR & ")"
                        Case 3
                            queryFilter &= " and a.id_usuario = " & id_usuario
                    End Select

                ElseIf idReporte = 3 Then
                    queryFilter = ""
                    Dim vt = Me.Request.QueryString("vt")
                    If vt = "" Then
                        Select Case id_filtro_busqueda_viajes
                            Case 1
                                queryFilter &= " and id_usuario_crea = " & id_usuario
                            Case 2
                                queryFilter &= " and id_subregion in (" & listSubR & ")"
                            Case 3
                                queryFilter &= " and id_usuario_crea = " & id_usuario
                        End Select
                    Else

                    End If
                ElseIf idReporte = 5 Then

                    Dim fechaDesde = Convert.ToString(Me.Request.QueryString("desde"))
                    Dim fechaHasta = Convert.ToString(Me.Request.QueryString("hasta"))

                    If fechaDesde <> "" And fechaHasta <> "" Then
                        queryFilter &= " and fecha_radicado >= '" & fechaDesde & "' and fecha_radicado <= '" & fechaHasta & " 23:00:00'"
                    End If
                End If
                If idReporte = 1 Then
                    cmdText += queryFilter & " order by id_viaje"
                Else
                    cmdText += queryFilter
                End If
                Dim cmd As New SqlCommand(cmdText, conn)
                conn.Open()

                With wrkSheet
                    .Range("A1:DD400").Style.Font.FontSize = 10
                    .Range("A1:DD400").Style.Font.FontName = "Arial"



                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        Dim filas = 2
                        While reader.Read

                            For i = 0 To reader.FieldCount - 1
                                .Cell(col(i + 1) & 1).Value = reader.GetName(i)
                                Dim tt = reader.GetFieldType(i)
                                If (reader.GetFieldType(0) Is GetType(Double)) Then
                                    .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 40
                                Else
                                    If (reader.GetFieldType(0) Is GetType(Date)) Then
                                        .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 15
                                    End If
                                End If

                                .Cell(col(i + 1) & filas).Value = reader(i)
                            Next
                            filas = filas + 1
                        End While

                    End Using
                    .Columns.AdjustToContents()
                    If idReporte = 4 Then
                        .Column("C").Width = 120
                        .Range("C2:C1000").Style.Alignment.WrapText = True
                    End If
                End With
                If reporte.query2 IsNot Nothing Then
                    Dim cmdText2 As String = reporte.query2 & queryFilter

                    If idReporte = 4 Then
                        Dim anio = 0
                        Dim estado = 0
                        Dim caso = 1
                        If Me.Request.QueryString("anio") IsNot Nothing Then
                            anio = Convert.ToInt32(Me.Request.QueryString("anio"))
                        End If
                        If Me.Request.QueryString("estado") IsNot Nothing Then
                            estado = Convert.ToInt32(Me.Request.QueryString("estado"))
                        End If
                        If anio > 0 And estado > 0 Then
                            caso = 4
                        ElseIf anio > 0 And estado = 0 Then
                            caso = 2
                        ElseIf anio = 0 And estado > 0 Then
                            caso = 3
                        Else
                            caso = 1
                        End If
                        If busqueda_programa = True Then
                            cmdText2 = cmdText2 & " and b.id_ficha_proyecto in (" & String.Join(",", ListPro.ToArray) & ")"
                        End If
                        If id_tipo_usuario = 2 Then
                            cmdText2 = cmdText2 & " and b.id_ejecutor = " & id_ejecutor
                        End If
                        Select Case caso
                            Case 2
                                cmdText2 = cmdText2 & " and (case when MONTH(b.fecha_inicio_proyecto) > 9 and MONTH(b.fecha_inicio_proyecto) <= 12 then YEAR(b.fecha_inicio_proyecto) + 1 else YEAR(b.fecha_inicio_proyecto) end) = " & anio
                            Case 3
                                cmdText2 = cmdText2 & " and a.id_estado_entregable = " & estado
                            Case 4
                                cmdText2 = cmdText2 & " and (case when MONTH(b.fecha_inicio_proyecto) > 9 and MONTH(b.fecha_inicio_proyecto) <= 12 then YEAR(b.fecha_inicio_proyecto) + 1 else YEAR(b.fecha_inicio_proyecto) end) = " & anio & " and a.id_estado_entregable = " & estado
                        End Select
                        cmdText2 = cmdText2 & " order by a.id_ficha_proyecto asc "
                    End If


                    Dim cmd2 As New SqlCommand(cmdText2, conn)

                    With wrkSheet2
                        .Range("A1:DD400").Style.Font.FontSize = 10
                        .Range("A1:DD400").Style.Font.FontName = "Arial"



                        Using reader As SqlDataReader = cmd2.ExecuteReader()
                            Dim filas = 2
                            While reader.Read

                                For i = 0 To reader.FieldCount - 1
                                    .Cell(col(i + 1) & 1).Value = reader.GetName(i)
                                    Dim tt = reader.GetFieldType(i)
                                    If (reader.GetFieldType(0) Is GetType(Double)) Then
                                        .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 40
                                    Else
                                        If (reader.GetFieldType(0) Is GetType(Date)) Then
                                            .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 15
                                        End If
                                    End If

                                    .Cell(col(i + 1) & filas).Value = reader(i)
                                Next
                                filas = filas + 1
                            End While

                        End Using
                        .Columns.AdjustToContents()
                        If idReporte = 4 Then
                            .Column("C").Width = 120
                        End If
                    End With
                End If



                If reporte.query3 IsNot Nothing Then
                    Dim cmdText2 As String = reporte.query3 & queryFilter

                    If idReporte = 4 Then
                        Dim anio = 0
                        Dim estado = 0
                        Dim caso = 1
                        If Me.Request.QueryString("anio") IsNot Nothing Then
                            anio = Convert.ToInt32(Me.Request.QueryString("anio"))
                        End If
                        If Me.Request.QueryString("estado") IsNot Nothing Then
                            estado = Convert.ToInt32(Me.Request.QueryString("estado"))
                        End If
                        If anio > 0 And estado > 0 Then
                            caso = 4
                        ElseIf anio > 0 And estado = 0 Then
                            caso = 2
                        ElseIf anio = 0 And estado > 0 Then
                            caso = 3
                        Else
                            caso = 1
                        End If
                        If busqueda_programa = True Then
                            cmdText2 = cmdText2 & " and b.id_ficha_proyecto in (" & String.Join(",", ListPro.ToArray) & ")"
                        End If
                        If id_tipo_usuario = 2 Then
                            cmdText2 = cmdText2 & " and b.id_ejecutor = " & id_ejecutor
                        End If
                        Select Case caso
                            Case 2
                                cmdText2 = cmdText2 & " and (case when MONTH(b.fecha_inicio_proyecto) > 9 and MONTH(b.fecha_inicio_proyecto) <= 12 then YEAR(b.fecha_inicio_proyecto) + 1 else YEAR(b.fecha_inicio_proyecto) end) = " & anio
                            Case 3
                                cmdText2 = cmdText2 & " and a.id_estado_entregable = " & estado
                            Case 4
                                cmdText2 = cmdText2 & " and (case when MONTH(b.fecha_inicio_proyecto) > 9 and MONTH(b.fecha_inicio_proyecto) <= 12 then YEAR(b.fecha_inicio_proyecto) + 1 else YEAR(b.fecha_inicio_proyecto) end) = " & anio & " and a.id_estado_entregable = " & estado
                        End Select
                        cmdText2 = cmdText2 & " order by a.id_ficha_proyecto "
                    End If


                    Dim cmd2 As New SqlCommand(cmdText2, conn)

                    With wrkSheet3
                        .Range("A1:DD400").Style.Font.FontSize = 10
                        .Range("A1:DD400").Style.Font.FontName = "Arial"



                        Using reader As SqlDataReader = cmd2.ExecuteReader()
                            Dim filas = 2
                            While reader.Read

                                For i = 0 To reader.FieldCount - 1
                                    .Cell(col(i + 1) & 1).Value = reader.GetName(i)
                                    Dim tt = reader.GetFieldType(i)
                                    If (reader.GetFieldType(0) Is GetType(Double)) Then
                                        .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 40
                                    Else
                                        If (reader.GetFieldType(0) Is GetType(Date)) Then
                                            .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 15
                                        End If
                                    End If

                                    .Cell(col(i + 1) & filas).Value = reader(i)
                                Next
                                filas = filas + 1
                            End While

                        End Using
                        .Columns.AdjustToContents()
                        If idReporte = 4 Then
                            .Column("C").Width = 120
                        End If
                    End With
                End If


                If reporte.query4 IsNot Nothing Then
                    Dim cmdText2 As String = reporte.query4 & queryFilter

                    If idReporte = 4 Then
                        Dim anio = 0
                        Dim estado = 0
                        Dim caso = 1
                        If Me.Request.QueryString("anio") IsNot Nothing Then
                            anio = Convert.ToInt32(Me.Request.QueryString("anio"))
                        End If
                        If Me.Request.QueryString("estado") IsNot Nothing Then
                            estado = Convert.ToInt32(Me.Request.QueryString("estado"))
                        End If
                        If anio > 0 And estado > 0 Then
                            caso = 4
                        ElseIf anio > 0 And estado = 0 Then
                            caso = 2
                        ElseIf anio = 0 And estado > 0 Then
                            caso = 3
                        Else
                            caso = 1
                        End If
                        If busqueda_programa = True Then
                            cmdText2 = cmdText2 & " and b.id_ficha_proyecto in (" & String.Join(",", ListPro.ToArray) & ")"
                        End If
                        If id_tipo_usuario = 2 Then
                            cmdText2 = cmdText2 & " and b.id_ejecutor = " & id_ejecutor
                        End If
                        Select Case caso
                            Case 2
                                cmdText2 = cmdText2 & " and (case when MONTH(b.fecha_inicio_proyecto) > 9 and MONTH(b.fecha_inicio_proyecto) <= 12 then YEAR(b.fecha_inicio_proyecto) + 1 else YEAR(b.fecha_inicio_proyecto) end) = " & anio
                            Case 3
                                cmdText2 = cmdText2 & " and a.id_estado_entregable = " & estado
                            Case 4
                                cmdText2 = cmdText2 & " and (case when MONTH(b.fecha_inicio_proyecto) > 9 and MONTH(b.fecha_inicio_proyecto) <= 12 then YEAR(b.fecha_inicio_proyecto) + 1 else YEAR(b.fecha_inicio_proyecto) end) = " & anio & " and a.id_estado_entregable = " & estado
                        End Select
                        cmdText2 = cmdText2 & " order by a.id_ficha_proyecto "
                    End If


                    Dim cmd2 As New SqlCommand(cmdText2, conn)

                    With wrkSheet4
                        .Range("A1:DD400").Style.Font.FontSize = 10
                        .Range("A1:DD400").Style.Font.FontName = "Arial"



                        Using reader As SqlDataReader = cmd2.ExecuteReader()
                            Dim filas = 2
                            While reader.Read

                                For i = 0 To reader.FieldCount - 1
                                    .Cell(col(i + 1) & 1).Value = reader.GetName(i)
                                    Dim tt = reader.GetFieldType(i)
                                    If (reader.GetFieldType(0) Is GetType(Double)) Then
                                        .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 40
                                    Else
                                        If (reader.GetFieldType(0) Is GetType(Date)) Then
                                            .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 15
                                        End If
                                    End If

                                    .Cell(col(i + 1) & filas).Value = reader(i)
                                Next
                                filas = filas + 1
                            End While

                        End Using
                        .Columns.AdjustToContents()
                        If idReporte = 4 Then
                            .Column("C").Width = 120
                        End If
                    End With
                End If


                If reporte.query5 IsNot Nothing Then
                    Dim cmdText2 As String = reporte.query5 & queryFilter

                    If idReporte = 4 Then
                        Dim anio = 0
                        Dim estado = 0
                        Dim caso = 1
                        If Me.Request.QueryString("anio") IsNot Nothing Then
                            anio = Convert.ToInt32(Me.Request.QueryString("anio"))
                        End If
                        If Me.Request.QueryString("estado") IsNot Nothing Then
                            estado = Convert.ToInt32(Me.Request.QueryString("estado"))
                        End If
                        If anio > 0 And estado > 0 Then
                            caso = 4
                        ElseIf anio > 0 And estado = 0 Then
                            caso = 2
                        ElseIf anio = 0 And estado > 0 Then
                            caso = 3
                        Else
                            caso = 1
                        End If
                        If busqueda_programa = True Then
                            cmdText2 = cmdText2 & " and b.id_ficha_proyecto in (" & String.Join(",", ListPro.ToArray) & ")"
                        End If
                        If id_tipo_usuario = 2 Then
                            cmdText2 = cmdText2 & " and b.id_ejecutor = " & id_ejecutor
                        End If
                        Select Case caso
                            Case 2
                                cmdText2 = cmdText2 & " and (case when MONTH(b.fecha_inicio_proyecto) > 9 and MONTH(b.fecha_inicio_proyecto) <= 12 then YEAR(b.fecha_inicio_proyecto) + 1 else YEAR(b.fecha_inicio_proyecto) end) = " & anio
                            Case 3
                                cmdText2 = cmdText2 & " and a.id_estado_entregable = " & estado
                            Case 4
                                cmdText2 = cmdText2 & " and (case when MONTH(b.fecha_inicio_proyecto) > 9 and MONTH(b.fecha_inicio_proyecto) <= 12 then YEAR(b.fecha_inicio_proyecto) + 1 else YEAR(b.fecha_inicio_proyecto) end) = " & anio & " and a.id_estado_entregable = " & estado
                        End Select
                        cmdText2 = cmdText2 & " order by a.id_ficha_proyecto "
                    End If


                    Dim cmd2 As New SqlCommand(cmdText2, conn)

                    With wrkSheet5
                        .Range("A1:DD400").Style.Font.FontSize = 10
                        .Range("A1:DD400").Style.Font.FontName = "Arial"



                        Using reader As SqlDataReader = cmd2.ExecuteReader()
                            Dim filas = 2
                            While reader.Read

                                For i = 0 To reader.FieldCount - 1
                                    .Cell(col(i + 1) & 1).Value = reader.GetName(i)
                                    Dim tt = reader.GetFieldType(i)
                                    If (reader.GetFieldType(0) Is GetType(Double)) Then
                                        .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 40
                                    Else
                                        If (reader.GetFieldType(0) Is GetType(Date)) Then
                                            .Cell(col(i + 1)).Style.NumberFormat.NumberFormatId = 15
                                        End If
                                    End If

                                    .Cell(col(i + 1) & filas).Value = reader(i)
                                Next
                                filas = filas + 1
                            End While

                        End Using
                        .Columns.AdjustToContents()
                        If idReporte = 4 Then
                            .Column("C").Width = 120
                        End If
                    End With
                End If

            End Using
            wrkBook.Worksheet(1).Name = reporte.nombre_hoja
            If reporte.query2 IsNot Nothing Then
                wrkBook.Worksheet(2).Name = reporte.nombre_hoja2
            End If
            If reporte.query3 IsNot Nothing Then
                wrkBook.Worksheet(3).Name = reporte.nombre_hoja3
            End If
            If reporte.query4 IsNot Nothing Then
                wrkBook.Worksheet(4).Name = reporte.nombre_hoja4
            End If
            If reporte.query5 IsNot Nothing Then
                wrkBook.Worksheet(5).Name = reporte.nombre_hoja5
            End If

            Dim tip As String

            tip = reporte.nombre_reporte_export

            Dim firstCell = wrkBook.Worksheet(1).FirstCellUsed()
            Dim lastCell = wrkBook.Worksheet(1).LastCellUsed()
            Dim Range = wrkBook.Worksheet(1).Range(firstCell.Address, lastCell.Address)

            Range.Clear(XLClearOptions.AllFormats)
            Dim Table = Range.CreateTable()

            Dim columnWithHeaders = lastCell.Address.ColumnNumber
            Dim currentRow = Table.RangeAddress.FirstAddress.RowNumber
            Dim htFirstCell = wrkBook.Worksheet(1).Cell(Table.RangeAddress.FirstAddress.RowNumber, columnWithHeaders)
            Dim htLastCell = wrkBook.Worksheet(1).Cell(currentRow, columnWithHeaders)
            Table.ShowAutoFilter = True

            Dim lastCell2 = wrkBook.Worksheet(2).LastCellUsed()
            Dim firstCell2 = wrkBook.Worksheet(2).FirstCellUsed()
            If firstCell2 IsNot Nothing And lastCell2 IsNot Nothing Then
                Dim Range2 = wrkBook.Worksheet(2).Range(firstCell2.Address, lastCell2.Address)
                Range2.Clear(XLClearOptions.AllFormats)
                Dim Table2 = Range2.CreateTable()

                Dim columnWithHeaders2 = lastCell2.Address.ColumnNumber
                Dim currentRow2 = Table2.RangeAddress.FirstAddress.RowNumber
                Dim htFirstCell2 = wrkBook.Worksheet(2).Cell(Table2.RangeAddress.FirstAddress.RowNumber, columnWithHeaders2)
                Dim htLastCell2 = wrkBook.Worksheet(2).Cell(currentRow2, columnWithHeaders2)
                Table2.ShowAutoFilter = True
                If idReporte = 2 Then


                    Range2.Columns("C,T,V,W").Style.NumberFormat.NumberFormatId = 15
                    Range2.Columns("J,L").Style.NumberFormat.NumberFormatId = 4

                End If
            End If




            If idReporte = 1 Then
                Range.Columns("E,F,G,O,S,W,Z,AG,AN,AP,AS,AV,AY,AZ,BB,BC").Style.NumberFormat.NumberFormatId = 15
                Range.Columns("N,Q,W,Y,AD,AF,AK,AM,AQ,BD,BE,BF").Style.NumberFormat.NumberFormatId = 4
            End If

            If idReporte = 2 Then
                Range.Columns("F,G,H,AC,AE,AF").Style.NumberFormat.NumberFormatId = 15
                Range.Columns("Q,S").Style.NumberFormat.NumberFormatId = 4

            End If

            If idReporte = 3 Then
                Range.Columns("B,Q").Style.NumberFormat.NumberFormatId = 15
                Range.Columns("J").Style.NumberFormat.NumberFormatId = 4

            End If

            If idReporte = 5 Then
                Range.Columns("C,N,O,P").Style.NumberFormat.NumberFormatId = 15
                Range.Columns("J").Style.NumberFormat.NumberFormatId = 4

            End If
            Dim aleatorio As String
            Dim rnd As New Random()
            aleatorio = rnd.Next(1, 999).ToString & Now.Date.ToString("ddMMyyyy")
            archivo = tip & aleatorio & ".xlsx"

            wrkBook.SaveAs(Server.MapPath("~/Temp/") & archivo)



            Dim file As New FileInfo(Server.MapPath("~/Temp/") & archivo)
            Response.Clear()
            Response.ClearHeaders()
            Response.ClearContent()
            Response.AppendHeader("Content-Disposition", "attachment; filename = " & archivo)
            Response.AppendHeader("Content-Length", file.Length.ToString())
            Response.ContentType = "application/download"
            Response.WriteFile(file.FullName)
            Response.Flush()
            Response.Close()
            Response.End()
        End Using
    End Sub
End Class