Imports ly_SIME
Imports ly_APPROVAL
Imports ly_RMS
Imports System.Globalization

Public Class MasterPopUp
    Inherits System.Web.UI.MasterPage

    Dim cl_user As ly_SIME.CORE.cls_user
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            cl_user = HttpContext.Current.Session.Item("clUser")
            If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
                cl_user = Session.Item("clUser")
                System.Threading.Thread.CurrentThread.CurrentUICulture = cl_user.regionalizacionCulture
                System.Threading.Thread.CurrentThread.CurrentCulture = cl_user.regionalizacionCulture
            Else
                System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("es-CO")
                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-CO")
            End If
            Me.lbl_programa.Text = Me.Session("E_Programa").ToString()
            strFecha.InnerHtml = Me.Session("E_Nombre") & " - " & getFecha(Date.UtcNow, "f", True)
            'Date.Now.ToString("f", cl_user.regionalizacionCulture)

            If Not IsPostBack Then
                Using dbEntities As New dbRMS_JIEntities

                    Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                    Dim oPrograma = dbEntities.t_programas.Find(id_programa)

                    'logo_Chemonics_nw.png
                    If Not String.IsNullOrEmpty(oPrograma.imgName) And (oPrograma.imgName <> "images/activities/logo_Chemonics_nw.png") Then
                        imgProgram.ImageUrl = ResolveUrl(oPrograma.imgName)
                        'Else
                        '    imgProgram.ImageUrl =
                    End If
                    'imgProgram.ImageUrl = ResolveUrl(oPrograma.imgName)

                End Using
            End If



        Catch ex As Exception

        End Try
    End Sub

    Public Function getFecha(dateIN As DateTime, strFormat As String, boolUTC As Boolean) As String

        Dim clDate As APPROVAL.cls_dUtil
        '************************************SYSTEM INFO********************************************
        Dim cProgram As New RMS.cls_Program
        cProgram.get_Sys(0, True)
        cProgram.get_Programs(cl_user.Id_Cprogram, True)
        Dim userCulture As CultureInfo
        Dim timezoneUTC As Integer
        userCulture = cl_user.regionalizacionCulture
        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
        clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
        '************************************SYSTEM INFO********************************************

        Return clDate.set_DateFormat(dateIN, strFormat, timezoneUTC, boolUTC)
        'Return dateIN.ToShortDateString

    End Function



End Class