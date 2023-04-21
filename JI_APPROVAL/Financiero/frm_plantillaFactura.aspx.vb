Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Web
Imports System.IO
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports CuteWebUI
Imports System.Net
Imports System.Math
Imports Telerik.Web.UI
Imports System.Net.Mail
Imports System.Threading
Imports System.Globalization
Imports ClosedXML.Excel

Public Class frm_plantillaFactura
    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim FolderExcel As String = Server.MapPath("~/Financiero/Formato/")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        actividadProyecto("")
    End Sub
    Sub actividadProyecto(ByVal dirFile As String)
        Dim sql, id_solicitud As String
        'id_fichaproyecto = Me.Request("id_fichaProyecto")
        id_solicitud = Me.Request("id")

        Dim rnd As New Random()
        Dim aleatorio As String
        Dim Archivo, RutaCopia As String
        'ARCHIVO PLANTILLA
        Dim Ruta_plantilla = FolderExcel & "formato_doc_soporte.xlsx"
        Dim fecha = Date.Now().ToString
        'ARCHIVO A CREAR
        Dim textfecha = fecha.ToString.Replace("/", "").Replace(" ", "").Replace("a", "").Replace(".", "").Replace("m", "").Replace(":", "").Replace(";", "").Replace("p", "")
        aleatorio = rnd.Next(1, 999).ToString & textfecha.ToString
        Archivo = "FormatoFactura_" & aleatorio & ".xlsx"
        RutaCopia = FolderExcel & Archivo

        Dim oLibro As New XLWorkbook(Ruta_plantilla)
        Dim oHoja = oLibro.Worksheet(1)
        'oLibro.Worksheet(1).Name = "actividades_2"



        '************************************************LLENAMOS INFORMACION DE LOS DETALLES DEL SCORECARD
        sql = "select numero_factura, fecha_factura, nombre, direccion, concat(nombre_departamento, ' - ' , nombre_municipio) ciudad, telefono, documento_identificacion, celular, correo from tme_facturacion A 
                inner join t_municipios b on a.id_municipio = b.id_municipio
                inner join t_departamentos c on b.id_departamento = c.id_departamento "
        'sql &= " D.nombre_empleado as UsuarioVistoBueno, D.descripcion_nivel_empleado RolVistoBueno, "
        'sql &= " E.nombre_empleado as UsuarioAprueba, E.descripcion_nivel_empleado RolAprueba "

        'sql &= " INNER JOIN vw_empleados B ON A.id_usuario_solicita = B.id_empleado  "
        'sql &= " INNER JOIN t_tipo_nivel_empleado C ON B.id_nivel_empleado = C.id_nivel_empleado "
        'sql &= " LEFT JOIN vw_empleados D ON A.id_usuario_visto_bueno = D.id_empleado "
        'sql &= " LEFT JOIN vw_empleados E ON A.id_usuario_aprueba = E.id_empleado "
        sql &= " where id_facturacion = " & id_solicitud
        Dim detallesScordCard As New SqlDataAdapter(sql, cnn)
        Dim ds As New DataSet("detallesSC")
        detallesScordCard.Fill(ds, "detallesSC")

        Dim ddv = ds.Tables("detallesSC")(0)("numero_factura")
        Dim ddv2 = ds.Tables("detallesSC")(0)("fecha_factura")
        Dim ddv3 = ds.Tables("detallesSC")(0)("nombre")
        Dim ddv4 = ds.Tables("detallesSC")(0)("direccion")
        Dim ddv5 = ds.Tables("detallesSC")(0)("ciudad")
        Dim ddv6 = ds.Tables("detallesSC")(0)("telefono")
        Dim ddv7 = ds.Tables("detallesSC")(0)("celular")
        Dim ddv8 = ds.Tables("detallesSC")(0)("correo")
        Dim ddv9 = ds.Tables("detallesSC")(0)("documento_identificacion")


        oHoja.Cell("K3").Value = "NÚMERO:  " & ds.Tables("detallesSC")(0)("numero_factura")
        oHoja.Cell("B4").Value = CDate(ds.Tables("detallesSC")(0)("fecha_factura")).Day.ToString()
        oHoja.Cell("E4").Value = CDate(ds.Tables("detallesSC")(0)("fecha_factura")).Month.ToString()
        oHoja.Cell("G4").Value = CDate(ds.Tables("detallesSC")(0)("fecha_factura")).Year.ToString()
        oHoja.Cell("C5").Value = ds.Tables("detallesSC")(0)("nombre").ToString
        oHoja.Cell("D6").Value = ds.Tables("detallesSC")(0)("direccion").ToString
        oHoja.Cell("L6").Value = ds.Tables("detallesSC")(0)("ciudad").ToString
        oHoja.Cell("Q6").Value = ds.Tables("detallesSC")(0)("telefono").ToString
        oHoja.Cell("G7").Value = ds.Tables("detallesSC")(0)("documento_identificacion").ToString
        oHoja.Cell("P7").Value = ds.Tables("detallesSC")(0)("celular").ToString
        oHoja.Cell("F8").Value = ds.Tables("detallesSC")(0)("correo").ToString



        sql = "SELECT * FROM tme_facturacion_productos WHERE id_facturacion = " & id_solicitud

        Dim sqlcomando As New SqlCommand(sql, cnn)

        Dim adaptador As New SqlDataAdapter(sqlcomando)
        Dim dataset = New DataSet()
        adaptador.Fill(dataset, "tabla")

        Dim valortotal = 0.0
        Dim fila = 10
        For Each row As DataRow In dataset.Tables("tabla").Rows
            oHoja.Cell("A" & fila).Value = row("cantidad").ToString
            oHoja.Cell("D" & fila).Value = row("descripcion").ToString
            oHoja.Cell("R" & fila).Value = (row("valor")).ToString

            valortotal = valortotal + (row("valor"))
            fila = fila + 1
        Next
        'oHoja.Cell("R24").Value = valortotal

        Dim stream As MemoryStream = GetStream(oLibro)
        Archivo = "Documento_soporte_" & ds.Tables("detallesSC")(0)("numero_factura") & String.Format("_D{0}{1}{2}_{3}{4}_{5}", Date.UtcNow.Day, Date.UtcNow.Month, Date.UtcNow.Year, Date.UtcNow.Hour, Date.UtcNow.Minute, Date.UtcNow.Second)

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=" + Archivo + ".xlsx")
        Response.ContentType = "application/vnd.openxmlformats-officedocument." + "spreadsheetml.sheet"
        Response.BinaryWrite(stream.ToArray())
        Response.End()

        'oLibro.SaveAs("C:\excelSime\" & Archivo)
        'descargar("C:\excelSime\" & Archivo, Archivo)


    End Sub
    Public Function GetStream(excelWorkbook As XLWorkbook) As Stream
        Dim fs As Stream = New MemoryStream()
        excelWorkbook.SaveAs(fs)
        fs.Position = 0
        Return fs
    End Function
    'PARA DESCARGAR ARCHIVO
    Public Sub DownloadFile(ByVal FilePath As String, ByVal OriginalFileName As String)
        Dim fs As IO.FileStream = Nothing
        'obtenemos el archivo del servidor 
        fs = IO.File.Open(FilePath, IO.FileMode.Open, IO.FileAccess.Read)
        Dim byteBuffer(CInt(fs.Length - 1)) As Byte
        fs.Read(byteBuffer, 0, CInt(fs.Length))
        fs.Close()

        Using ms As New IO.MemoryStream(byteBuffer)
            'descargar con su nombre original 
            Response.AddHeader("Content-Disposition", "attachment; filename=" & OriginalFileName)
            ms.WriteTo(Response.OutputStream)
        End Using
    End Sub

    Public Sub descargar(ByVal FilePath As String, ByVal OriginalFileName As String)
        Dim dir, archivo As String
        dir = "C:\excelSime\"
        archivo = OriginalFileName
        Dim file As New FileInfo(dir & archivo)
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
    End Sub

    Function numeroaletra(ByVal numero As Integer) As String
        Dim letra As String
        letra = "A"
        Select Case numero
            Case 1
                letra = "A"
            Case 2
                letra = "B"
            Case 3
                letra = "C"
            Case 4
                letra = "D"
            Case 5
                letra = "E"
            Case 6
                letra = "F"
            Case 7
                letra = "G"
            Case 8
                letra = "H"
            Case 9
                letra = "I"
            Case 10
                letra = "J"
            Case 11
                letra = "K"
            Case Else
                letra = ""
        End Select
        Return letra
    End Function

End Class