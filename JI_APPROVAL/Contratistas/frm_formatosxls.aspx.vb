Imports ly_SIME
Imports System.IO
Imports ClosedXML.Excel
Imports System.Data.SqlClient
Public Class frm_formatosxls
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '1 = ficha de hogares, 2 = asistencia de capacitaciones
        Dim Formato = Convert.ToInt32(Me.Request.QueryString("id"))
        Dim wrkBook = New XLWorkbook()
        Dim hoja As String = ""
        Dim Sess As Integer

        'Dim id_top = Me.Request.QueryString("id")



        If Formato = 1 Then
            hoja = "Contratistas"
            generarReporte_Contratistas(Formato, hoja)
        End If

        Dim wrkSheet = wrkBook.AddWorksheet(hoja)

        wrkSheet.Row(1).Height = 20
        wrkSheet.Row(1).Style.Font.FontSize = 10
        wrkSheet.Row(1).Style.Font.FontColor = XLColor.White
        wrkSheet.Row(1).Style.Font.Bold = True
        Dim worksheet2 = wrkBook.Worksheets.Add("Data")
        Dim id_Programa = Convert.ToInt32(Me.Session("E_IDPrograma"))



    End Sub

    Sub generarReporte_Contratistas(ByVal idFormat As Integer, ByVal strHoja As String)
        Using dbEntities As New dbRMS_JIEntities
            Dim id_Programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim formatoBeneficiaryStr = Server.MapPath("~\Contratistas\Formatos\formato_contratistas.xlsx")

            Dim wrkBook = New XLWorkbook(formatoBeneficiaryStr)
            Dim wrkSheet = wrkBook.Worksheets.Worksheet(1)
            Dim worksheet2 = wrkBook.Worksheets.Worksheet(2)
            Dim worksheet3 = wrkBook.Worksheets.Worksheet(3)
            Dim worksheet4 = wrkBook.Worksheets.Worksheet(4)

            Dim id_ficha = Convert.ToInt32(Me.Session("E_IdFicha"))
            Dim usuarios = dbEntities.vw_t_usuarios.Where(Function(p) p.id_programa = id_Programa And p.id_estado_usr = 1).OrderBy(Function(p) p.nombre_usuario).ToList()


            Dim contratista = usuarios.Where(Function(p) p.id_tipo_usuario = 3).Select(Function(p) p.nombre_usuario).ToList()
            Dim supervisores = usuarios.Where(Function(p) p.id_tipo_usuario = 1).Select(Function(p) p.nombre_usuario).ToList()
            Dim compoentes = dbEntities.tme_estructura_marcologico.Where(Function(p) p.tme_programa_marco_logico.id_programa = id_Programa And p.id_tipo_marcologico = 15).Select(Function(p) New With
                                                                                                                                         {Key .descripcion_logica = p.codigo & " - " & p.descripcion_logica
                                                                                                                                         }).ToList()


            worksheet3.Cell("A1").Value = contratista
            worksheet3.Cell("B1").Value = supervisores
            worksheet3.Cell("C1").Value = compoentes

            Dim dv1 = wrkSheet.Range("E2:E1000").SetDataValidation()
            dv1.List(worksheet3.Range("A1:A" & (contratista.Count()) & ""))
            dv1.ErrorStyle = XLErrorStyle.Stop
            dv1.InputTitle = "Campo requerido"
            dv1.InputMessage = "*"

            Dim dv2 = wrkSheet.Range("G2:G1000").SetDataValidation()
            dv2.List(worksheet3.Range("B1:B" & (supervisores.Count()) & ""))
            dv2.ErrorStyle = XLErrorStyle.Stop
            dv2.InputTitle = "Campo requerido"
            dv2.InputMessage = "*"

            Dim dv3 = worksheet4.Range("B2:B1000").SetDataValidation()
            dv3.List(worksheet3.Range("C1:C" & (compoentes.Count()) & ""))
            dv3.ErrorStyle = XLErrorStyle.Stop
            dv3.InputTitle = "Campo requerido"
            dv3.InputMessage = "*"


            wrkBook.Worksheet(3).Visibility = XLWorksheetVisibility.VeryHidden

            Dim tip As String

            tip = strHoja
            Dim archivo = tip & String.Format("_D{0}{1}{2}_{3}{4}_{5}", Date.UtcNow.Day, Date.UtcNow.Month, Date.UtcNow.Year, Date.UtcNow.Hour, Date.UtcNow.Minute, Date.UtcNow.Second)
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