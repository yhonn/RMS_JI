Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports System.Globalization
Imports ly_APPROVAL
Imports ly_RMS
Imports ly_SIME
Imports System.IO.Compression
Imports System.IO
Public Class frm_timesheetdownload
    Inherits System.Web.UI.Page
    Dim cls_TimeSheet As APPROVAL.clss_TimeSheet
    Dim clss_approval As APPROVAL.clss_approval
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
        generarReporte(Me.Request.QueryString("y"), Me.Request.QueryString("m"))
    End Sub
    Sub generarReporte(ByVal y As Integer, ByVal m As Integer)
        'Dim url As String = "frm_TimeSheetFollowingREP_pay?y=" & cmb_year.SelectedValue.ToString & "&m=" & cmb_Month.SelectedValue.ToString

        'Dim s As String = "window.open('" & url & "', '_blank');"
        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "TimeSheet Report", s, True)

        cls_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
        Dim tbl_table As DataTable = cls_TimeSheet.getTimeSheet_period_d(m, y)


        Using dbEntities As New dbRMS_JIEntities
            Dim id_ficha = Convert.ToInt32(Me.Session("E_IdFicha"))
            Dim id_Programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
            'Dim ficha = dbEntities.tme_Ficha_Proyecto.Find(id_ficha)
            Dim fecha = DateTime.Now
            Dim fechaStr = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond
            Dim oSys = dbEntities.t_sys.FirstOrDefault(Function(p) p.id_sys = 2)
            'Dim nombreArchivo = "doc_informe_avance_" & fechaStr & "_" & ficha.codigo_RFA & ".pdf"
            Try
                'Dim IdSubR = ficha.id_subregion
                'Dim subR = dbEntities.t_subregiones.Find(IdSubR)

                'Dim id_periodo = dbEntities.vw_t_periodos.FirstOrDefault(Function(p) p.id_programa = id_Programa And p.activo And p.id_region = subR.id_region).id_periodo()


                Dim Path As String
                Path = Server.MapPath("~/soportes/time_sheet/")

                Dim idTS As Integer

                For Each row In tbl_table.Rows
                    Dim numDoc = row.Item("numero_documento")
                    idTS = row.Item("id_timesheet")
                    Dim tipo = row.Item("TimeSheet_Type")
                    Dim id_timesheet = row.Item("id_timesheet")

                    If Not System.IO.Directory.Exists(Path & "\\" & y & "_" & m) Then
                        System.IO.Directory.CreateDirectory(Path & "\\" & y & "_" & m)
                    End If

                    If tipo <> "TimeSheet" Then
                        tipo = "Novedad"
                    End If

                    Dim startInfo = New ProcessStartInfo()
                    Dim myProcess As New Process
                    startInfo.UseShellExecute = False
                    startInfo.RedirectStandardOutput = True
                    startInfo.RedirectStandardInput = True
                    startInfo.RedirectStandardError = True
                    startInfo.FileName = "C:\\Program Files\\wkhtmltopdf\\bin\\wkhtmltopdf.exe"
                    startInfo.Arguments = "-O landscape " & oSys.sys_url & "/TimeSheet/frm_TimeSheetFollowingREPPrint.aspx?ID=" & idTS & " " & Path & "\\" & y & "_" & m & "\\" & tipo & "_" & id_timesheet & "_" & y & "_" & m & "_" & numDoc & ".pdf"
                    myProcess = Process.Start(startInfo)
                    myProcess.WaitForExit()
                    myProcess.Close()
                Next


                If Not System.IO.Directory.Exists(Path & "\\" & y & "_" & m) Then
                    System.IO.Directory.CreateDirectory(Path & "\\" & y & "_" & m)
                End If

                Dim fileZip As String

                fileZip = Path & "\\" & y & "_" & m & ".zip"

                If System.IO.File.Exists(fileZip) = True Then
                    System.IO.File.Delete(fileZip)
                End If


                ZipFile.CreateFromDirectory(Path & "\\" & y & "_" & m,
                                            fileZip,
                                            CompressionLevel.Optimal,
                                            False)

                Dim archivo = y & "_" & m & ".zip"
                Dim file As New FileInfo(fileZip)

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

                'Dim oInforme = New tme_proyecto_informe_avance()
                'oInforme.fecha_crea = DateTime.Now
                'oInforme.soporte = nombreArchivo
                'oInforme.id_periodo = id_periodo
                'oInforme.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                'oInforme.id_ficha_proyecto = id_ficha
                'oInforme.tipo = "Acta de seguimiento"

                'dbEntities.tme_proyecto_informe_avance.Add(oInforme)
                'dbEntities.SaveChanges()
                'Response.Clear()
                'Response.AddHeader("content-disposition", "attachment;filename=doc_hito_" & identity & ".pdf")
                'Response.ContentType = "application/pdf"
                ''Response.WriteFile("C:\\db\\doc_hito_" & identity & ".pdf")
                'Response.WriteFile("C:\\Users\\jhons\\Source\\Workspaces\\RMS_PB\\RMS_SIME_PB\\AA_SIME\\formatoentregables\\doc_hito_" & identity & ".pdf")
                'Response.End()
                'Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                'Me.MsgGuardar.Redireccion = "~/Instrumentos/frm_informe_avance"
                'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            Catch ex As Exception
                Dim errore = ex.Message
            End Try

        End Using
    End Sub

End Class