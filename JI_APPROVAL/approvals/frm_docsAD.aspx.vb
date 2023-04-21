Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports CuteWebUI
Imports System.Net.Mail
Imports System.Net
Imports System.Web.Script.Serialization
'Imports Subgurim.Controles
Imports ly_SIME
Imports ly_APPROVAL


Partial Class frm_docsAD
    Inherits System.Web.UI.Page

    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim clss_approval As APPROVAL.clss_approval

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_OPEN_PROCC"
    Dim controles As New ly_SIME.CORE.cls_controles

    Const cPENDING = 1
    Const cAPPROVED = 2
    Const cnotAPPROVED = 3
    Const cCANCELLED = 4
    Const cOPEN = 5
    Const cSTANDby = 6
    Const cCOMPLETED = 7

    Const cAction_ByProcess = 1
    Const cAction_ByMessage = 2

    Public Sub New()

    End Sub

    Sub formatoNumero()
        Me.txt_tasacambio.NumberFormat.DecimalSeparator = ","
        Me.txt_tasacambio.NumberFormat.GroupSeparator = "."
        Me.txt_montoProyecto.NumberFormat.DecimalSeparator = ","
        Me.txt_montoProyecto.NumberFormat.GroupSeparator = "."
        Me.txt_montoTotal.NumberFormat.DecimalSeparator = ","
        Me.txt_montoTotal.NumberFormat.GroupSeparator = "."
    End Sub
    Sub enviar_email(ByVal id_doc As Integer, ByVal id_appdoc As Integer)

        '**************************DECLARACION DE VARIABLES***************************
        Dim sql = "SELECT * FROM t_config_email WHERE id_programa=" & Me.Session("E_IDPrograma")
        Dim dm As New SqlDataAdapter(sql, cnnSAP)
        Dim ds As New DataSet("email")
        dm.Fill(ds, "email")
        Dim i As Integer
        '**************************FIN****************************************

        '**************************PARAMETROS DE CONFIGURACION PARA ENVIAR EL EMAIL***********
        Dim destinatario = ""
        Dim destinatarioAdmin = ds.Tables("email").Rows(0).Item("email_admin").ToString 'DEBE SER A QUIEN INTERESE LA INFORMACION CONTENIDA EN LA FICHA

        Dim port = ds.Tables("email").Rows(0).Item("puerto_SMTP2").ToString
        Dim account = ds.Tables("email").Rows(0).Item("email2").ToString
        Dim pass = ds.Tables("email").Rows(0).Item("password2").ToString
        Dim smtp = ds.Tables("email").Rows(0).Item("SMTP2").ToString
        Dim tpConnCIF = Trim(ds.Tables("email").Rows(0).Item("tipo_conexion_cifrada2").ToString)

        Dim url_aplicacion = ds.Tables("email").Rows(0).Item("url_aplicacion").ToString

        Dim SendFrom As New MailAddress(ds.Tables("email").Rows(0).Item("email_noreplay").ToString, "System SAP-Approvals CHEMONICS")
        Dim SendTo As New MailAddress(ds.Tables("email").Rows(0).Item("BCC").ToString)
        Dim MensajeSend As New MailMessage(SendFrom, SendTo)
        MensajeSend.Subject = "Opening a new approval process"

        Dim emails_cco() As String = ds.Tables("email").Rows(0).Item("BCO").ToString.Split(";")
        For i = 0 To emails_cco.Count() - 1
            If emails_cco(i).ToString <> "" Then
                MensajeSend.Bcc.Add(emails_cco(i).ToString)
            End If
        Next
        'MensajeSend.Bcc.Add(destinatarioAdmin)

        '**************************FIN********************************************************

        '********************AGREGANDO LOS DESTINATARIOS***************************************
        sql = "SELECT orden FROM  vw_ta_AppDocumento WHERE id_App_Documento=" & id_appdoc
        ds.Tables.Add("Orden")
        dm.SelectCommand.CommandText = sql
        dm.Fill(ds, "Orden")
        Dim Orden = ds.Tables("Orden").Rows(0).Item("orden")

        '***********************NOTIFICANDO AL ORIGINADOR, AL QUE GENERA EL DOCUMENTO, AL ANTERIOR Y AL SIGUIENTE.**
        '******************INCLUYENDO LOS GRUPOS********************************
        sql = "  SELECT email, nombre_rol, id_rol FROM vw_ta_roles_emails "
        sql &= " WHERE (id_documento = " & id_doc & ") AND (orden = 0 OR orden = (" & Orden & " + 1) OR orden =(" & Orden & " - 1) OR  orden = " & Orden & ")"
        sql &= " UNION "
        sql &= " SELECT email, nombre_rol, id_rol FROM vw_ta_email_gruposRoles WHERE id_rol IN("
        sql &= " SELECT  id_rol FROM vw_ta_roles_emails AS A "
        sql &= " WHERE (id_documento = " & id_doc & ") AND (orden = 0 OR orden = (" & Orden & " + 1) OR orden =(" & Orden & " - 1) OR  orden = " & Orden & "))"
        Dim dm1 As New SqlDataAdapter(sql, cnnSAP)
        Dim ds1 As New DataSet("instrumento")
        dm1.Fill(ds1, "instrumento")

        '********************AGREGANDO LOS DESTINATARIOS************************
        For i = 0 To ds1.Tables("instrumento").Rows.Count() - 1
            MensajeSend.To.Add(ds1.Tables("instrumento").Rows(i).Item("email"))
        Next
        '**************************FIN**************************************** 

        '***************INCORPORANDO EMAILS ADICIONALES INTERESADOS EN EL PROCESO***********************
        sql = "SELECT email FROM vw_ta_documentos WHERE id_documento=" & id_doc
        Dim dmDoc As New SqlDataAdapter(sql, cnnSAP)
        Dim dsDoc As New DataSet("document")
        dmDoc.Fill(dsDoc, "document")
        Dim emails_cc() As String

        emails_cc = dsDoc.Tables("document").Rows(0).Item(0).ToString.Split(";")
        For i = 0 To emails_cc.Count() - 1
            If emails_cc(i).ToString <> "" Then
                MensajeSend.CC.Add(emails_cc(i).ToString)
            End If

        Next
        '*****************************************FIN******************************************

        '*****COLOCAR EN COPIA OCULTA AL ADMINISTRADOR********************
        MensajeSend.Bcc.Add(destinatarioAdmin)

        sql = "SELECT * FROM vw_ta_AppDocumento WHERE id_App_Documento=" & id_appdoc
        Dim dmap As New SqlDataAdapter(sql, cnnSAP)
        Dim dsap As New DataSet("appdoc")
        dmap.Fill(dsap, "appdoc")


        '  "  <img alt='Chermonics Inc.' src='http://www.colombiaresponde-ns.org/approval/Imagenes/logos/Chemonics-log150.png' style='width:150px; height:150px; border:0px;'   />" & _


        '*****************************TABLA DE ESTADOS ******************************** 
        Dim dsP As New DataSet("dtPROCS")
        dsP.Tables.Add("dtPROCS")
        sql = "SELECT * FROM dbo.FN_Ta_RutaSeguimiento(" & id_doc & ") ORDER BY ORDEN"
        dm.SelectCommand.CommandText = sql
        dm.Fill(dsP, "dtPROCS")
        Dim strTable = makeTable(dsP.Tables("dtPROCS"), dsap.Tables("appdoc").Rows(0).Item("descripcion_estado"), id_appdoc)
        '*****************************TABLA DE ESTADOS ******************************** 

        Dim nullable As System.Text.Encoding
        Dim Mensaje = Make_Email(dsap.Tables("appdoc"), url_aplicacion, strTable)
        Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(Mensaje, nullable, "text/html")
        ' Dim strPath = Server.MapPath("..") + "\Imagenes\logos\Chemonics-log150.png"

        Dim imageLink As LinkedResource = New LinkedResource(Server.MapPath("..") + "\Imagenes\logos\Chemonics-log150.png", "image/png")
        imageLink.ContentId = "LogCHERM"
        imageLink.TransferEncoding = System.Net.Mime.TransferEncoding.Base64
        htmlView.LinkedResources.Add(imageLink)

        Dim imgRED As LinkedResource = New LinkedResource(Server.MapPath("..") + "\Imagenes\Circle_Red.png", "image/png")
        imgRED.ContentId = "EmmB_Red"
        imgRED.TransferEncoding = System.Net.Mime.TransferEncoding.Base64
        htmlView.LinkedResources.Add(imgRED)

        Dim imgGRE As LinkedResource = New LinkedResource(Server.MapPath("..") + "\Imagenes\Circle_Green.png", "image/png")
        imgGRE.ContentId = "EmmB_Green"
        imgGRE.TransferEncoding = System.Net.Mime.TransferEncoding.Base64
        htmlView.LinkedResources.Add(imgGRE)

        Dim imgYELL As LinkedResource = New LinkedResource(Server.MapPath("..") + "\Imagenes\Circle_Yellow.png", "image/png")
        imgYELL.ContentId = "EmmB_Yellow"
        imgYELL.TransferEncoding = System.Net.Mime.TransferEncoding.Base64
        htmlView.LinkedResources.Add(imgYELL)

        Dim imgGRA As LinkedResource = New LinkedResource(Server.MapPath("..") + "\Imagenes\Circle_Gray.png", "image/png")
        imgGRA.ContentId = "EmmB_Gray"
        imgGRA.TransferEncoding = System.Net.Mime.TransferEncoding.Base64
        htmlView.LinkedResources.Add(imgGRA)

        '***EmmB_
        'EmmB_Red'  
        'EmmB_Green'  
        'EmmB_Yellow'  
        'EmmB_Gray'  
        '****.png

        MensajeSend.AlternateViews.Add(htmlView)
        MensajeSend.IsBodyHtml = True
        'MensajeSend.Body = Mensaje
        MensajeSend.Priority = MailPriority.High

        'Dim CorreoSend As New SmtpClient(smtp, port)
        'CorreoSend.Credentials = New NetworkCredential(account, pass)
        'CorreoSend.EnableSsl = False


        Dim CorreoSend As New SmtpClient(smtp, port)
        Dim basicAuth As New NetworkCredential(account, pass)

        CorreoSend.UseDefaultCredentials = True
        CorreoSend.Credentials = basicAuth

        If tpConnCIF = "TLS" Then
            CorreoSend.EnableSsl = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
        ElseIf tpConnCIF = "SSL" Then
            CorreoSend.EnableSsl = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
        Else
            CorreoSend.EnableSsl = False
        End If


        Try
            CorreoSend.Send(MensajeSend)
        Catch ex As Exception
            MsgBox("ERROR: " & ex.ToString, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Public Function makeTable(ByVal dtData As DataTable, ByVal desEstado As String, ByVal idAppDoc As Integer) As String

        Dim strTable As String = ""
        Dim trOP1 As String = "<tr"
        Dim trOP2 As String = ">"
        Dim trCL1 As String = "</tr>"
        Dim stlFOC As String = " style='background-color:#ED7620;border:1 dotted #FF0000; ' "
        Dim tdOP As String = "<td>"
        Dim tdCL As String = "</td>"
        Dim strOP As String = "<strong>"
        Dim strOP1 As String = ""
        Dim strCL As String = "</strong>"
        Dim strCL1 As String = ""
        'Dim Fecha As DateTime
        'Dim format As String = "ddd d MMMM yyyy HH:mm"
        Dim bndSTR As Boolean = False

        strTable = "<table border= '1' cellpadding='4' cellspacing='0'  style='border-color:#CCCCCC;padding: 5px; font-family:Verdana, Arial; font-size: x-small;'> "



        For Each dtR As DataRow In dtData.Rows

            If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) Then
                'If idAppDoc = dtR.Item("id_App_Documento") Then

                strOP1 = strOP
                strCL1 = strCL
                strTable &= trOP1 & stlFOC & trOP2

            Else

                strOP1 = ""
                strCL1 = ""
                strTable &= trOP1 & trOP2

            End If

            strTable &= tdOP & strOP1 & dtR.Item("nombre_rol") & strCL1 & tdCL
            strTable &= tdOP & strOP1 & dtR.Item("nombre_empleado") & strCL1 & tdCL
            strTable &= tdOP & strOP1 & dtR.Item("descripcion_estado") & strCL1 & tdCL
            strTable &= tdOP & strOP1 & dtR.Item("fecha_aprobacion") & strCL1 & tdCL
            'If Trim(dtR.Item("fecha_aprobacion")) = "-" Then
            '    strTable &= tdOP & strOP1 & dtR.Item("fecha_aprobacion") & strCL1 & tdCL
            'Else
            '    Fecha = CType(dtData.Rows(0).Item("fecha_recepcion"), DateTime)
            '    strTable &= tdOP & strOP1 & Fecha.ToString(format) & strCL1 & tdCL
            'End If

            strTable &= tdOP & "<img alt='\' hspace='0' src='cid:EmmB_" & Trim(dtR.Item("alerta")) & "' align='baseline' style='border:0px;' />" & tdCL

            strTable &= trCL1

        Next dtR

        strTable &= "</table>"

        Return strTable

    End Function

    Public Function Make_Email(ByVal dtData As DataTable, ByVal url_aplicacion As String, ByVal strTABLE As String) As String

        Dim strHTMLemail As String
        Dim FechaRec As Date
        Dim FechaLim As Date
        Dim format As String = "ddd d MMMM yyyy HH:mm"
        '' dtData.Rows(0).Item("descripcion_estado") & "   </td>" & _

        FechaRec = dtData.Rows(0).Item("fecha_recepcion")
        FechaLim = dtData.Rows(0).Item("fecha_limite")


        '  "  <img alt='Chermonics Inc.' src='http://www.colombiaresponde-ns.org/approval/Imagenes/logos/Chemonics-log150.png' style='width:150px; height:150px; border:0px;'   />" & _

        strHTMLemail = "<html xmlns='http://www.w3.org/1999/xhtml' >" &
        "<head>" &
                "</head>" &
                  "       <body>" &
                   "          <div>" &
                     "           <p style='font-family: Arial, Helvetica, sans-serif; font-size: small;'>" &
                      "            This message has been sent from the Results Management System CHEMONICS <br /><br />" &
                       "         </p>" &
                        "     </div>" &
                         "    <table border='1' cellpadding='0' cellspacing='0' style='width:100%; border-color:#FFFFFF;'>" &
                          "       <tr>" &
                           "          <td colspan ='2' >" &
                           "                 <table style='border-color: #000000; width:100%;border-color:#FFFFFF;'>" &
                           "                       <tr >" &
                            "                          <td  style='padding-left: 6px; width:10%; vertical-align:middle; text-align:center;width:120px; height:120px;' > " &
                            "                            <div id='logo1' style=' vertical-align:middle' >" &
                             "                              <img alt='\' hspace='0' src='cid:LogCHERM' align='baseline' style='width:120px; height:120px; border:0px;' />" &
                             "                           </div> " &
                             "                         </td>" &
                             "                          <td colspan='2' style='padding: 50px; text-align:left; font-family:Arial Unicode MS; font-size: medium; vertical-align:bottom; padding-bottom:5px; font-weight:bolder; width:90%; text-transform:uppercase '>" &
                             dtData.Rows(0).Item("nombre_proyecto") &
                             "                          </td>" &
                            "                              </tr>" &
                            "                  </table><br />" &
                           "           </td>" &
                          "       </tr>" &
                          "        <tr style=' height:10px;'>" &
                           "          <td colspan='2' style=' height:10px; background-color:#ED7620; border-color:#ED7620' >" &
                            "         </td>" &
                            "     </tr>" &
                            "     <tr>" &
                            "      <td colspan='2'>" &
                            "     <table style='width=100%;'>" &
                             "      <tr>" &
                             "       <td style='padding: 3px; border-color:#CECECE; background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" &
                               "          NAME OF CATEGORY: " &
                                "     </td>" &
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                                   dtData.Rows(0).Item("descripcion_cat") &
                                 "    </td>" &
                                " </tr>" &
                                " <tr>" &
                                 "    <td style='padding: 3px; border-color:#CECECE; background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" &
                                 "       APPROVAL:</td>" &
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                              dtData.Rows(0).Item("descripcion_aprobacion") & "  </td>" &
                                " </tr>" &
                                " <tr>" &
                                "     <td style='padding: 3px; border-color:#CECECE; background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" &
                                "        INSTRUMENT NUMBER:</td>" &
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                              dtData.Rows(0).Item("numero_instrumento") & "  </td>" &
                                " </tr>" &
                                " <tr>" &
                                "     <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" &
                                "        BENEFICIARY NAME:</td>" &
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                              dtData.Rows(0).Item("nom_beneficiario") & "  </td>" &
                                " </tr>" &
                               "  <tr>" &
                                "     <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" &
                                 "        NAME OF PROCESS:</td>" &
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                              dtData.Rows(0).Item("descripcion_doc") & "  </td>" &
                               "  </tr>" &
                                " <tr>" &
                                 "    <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" &
                                  "      PROCESS CODE:</td>" &
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                              dtData.Rows(0).Item("codigo_AID") & "   </td>" &
                               "  </tr>" &
                               "  <tr>" &
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" &
                               "          CODE SAP / APPROVAL:</td>" &
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                              dtData.Rows(0).Item("codigo_SAP_APP") &
                                "         </td>" &
                               "  </tr>" &
                               "   <tr>" &
                               "      <td style='padding: 3px;border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" &
                                "         REPORT CREATED BY:</td>" &
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                             dtData.Rows(0).Item("nombre_empleado") & "   </td>" &
                               "  </tr>" &
                               "   <tr>" &
                               "      <td style='padding: 3px;border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" &
                               "         POSITION:</td>" &
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                             dtData.Rows(0).Item("nombre_rol") & "   </td>" &
                               "  </tr>" &
                               "  <tr>" &
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'   >" &
                               "         NEXT PHASE OF PATH:</td>" &
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                              dtData.Rows(0).Item("nextFase") & "   </td>" &
                               "  </tr>" &
                               "  <tr>" &
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" &
                                "        DATE OF RECEIPT:</td>" &
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                                FechaRec.ToString(format) & "   </td>" &
                                " </tr>" &
                                  "  <tr>" &
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" &
                                "        DEADLINE:</td>" &
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                                FechaLim.ToString(format) & "   </td>" &
                                " </tr>" &
                               "  <tr>" &
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" &
                               "         STATUS PROCESS:</td>" &
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                              strTABLE & "   </td>" &
                               "  </tr>" &
                               "  <tr>" &
                                  "   <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" &
                                  "      LINK:</td>" &
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                                   "      <a href='" & url_aplicacion & "/approvals/frm_DocAprobacion.aspx?IdDoc=" &
                                dtData.Rows(0).Item("id_documento") & "' style=' text-decoration:none; font-family:Verdana, Arial;' > View link <a/> " &
                                   "      </td>" &
                                 "</tr>" &
                                " <tr>" &
                                 "    <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" &
                                 " COMMENTS:" &
                                  "   </td>" &
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " &
                                dtData.Rows(0).Item("Comentarios") &
                                  "   </td>" &
                               "  </tr>" &
                              " </table>" &
                               "  </td>" &
                              "   </tr>" &
                              "    <tr style=' height:10px;'>" &
                               "      <td colspan='2' style=' height:10px; background-color:#ED7620; border-color:#ED7620' >" &
                                "     </td>" &
                               "  </tr>" &
                                "  </table>" &
                                " <br /><br />" &
                               " <table>" &
                                " <tr>" &
                                "     <td colspan='2' style='border-color:#CCCCCC;background-color: #CCCCCC;'>" &
                                 "        <table border='1' cellpadding='0' cellspacing='0' style='mso-cellspacing: 0cm; mso-border-alt: outset #000033 .75pt; mso-yfti-tbllook: 1184; mso-padding-alt: 0cm 0cm 0cm 0cm; font-size: 10.0pt; font-family: 'Times New Roman', serif;border: 1.0pt outset #000033;'>" &
                                  "           <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;mso-yfti-lastrow:yes'>" &
                                   "              <td style='border: inset white 1.0pt; mso-border-alt: inset white .75pt;  padding: 0px 0px 0px 6px'>" &
                                    "                 <p>" &
                                      "                    &nbsp;</p>" &
                                      "               <p>" &
                                       "                  <b><strong>This is an automatically generated email, please do not reply.</strong></b></p>" &
                                        "             <p>" &
                                        "                 <br />" &
                                       "                  <strong>WARNING</strong>: <em>This message contains information legally " &
                                       "                  protected. If you have received this message by mistake, please avoid checking, " &
                                       "                  distributing, copying, reproduction or misuse of the information within it and " &
                                       "                  reply to the sender informing about the misunderstanding.</em></p>" &
                                       "              <p>" &
                                       "                  &nbsp;</p>" &
                                       "          </td>" &
                                       "      </tr>" &
                                       "  </table>" &
                                   "  </td>" &
                                " </tr>" &
                             "</table>" &
                           "  </body>" &
                      "   </html>"

        Return strHTMLemail

    End Function


    Private Function calculaDiaHabil(ByVal nDias As Integer, ByVal fechaPost As Date) As Date
        Dim hoy As Date = Date.UtcNow
        Dim weekend As Integer = 0
        Dim fecha_limite As Date
        Select Case fechaPost.DayOfWeek
            Case DayOfWeek.Sunday
                If nDias < 6 Then
                    weekend = 0
                ElseIf nDias < 11 Then
                    weekend = 2
                ElseIf nDias < 16 Then
                    weekend = 4
                End If
            Case DayOfWeek.Monday
                If nDias < 5 Then
                    weekend = 0
                ElseIf nDias < 10 Then
                    weekend = 2
                ElseIf nDias < 15 Then
                    weekend = 4
                End If
            Case DayOfWeek.Tuesday
                If nDias < 4 Then
                    weekend = 0
                ElseIf nDias < 9 Then
                    weekend = 2
                ElseIf nDias < 14 Then
                    weekend = 4
                End If
            Case DayOfWeek.Wednesday
                If nDias < 3 Then
                    weekend = 0
                ElseIf nDias < 8 Then
                    weekend = 2
                ElseIf nDias < 13 Then
                    weekend = 4
                End If
            Case DayOfWeek.Thursday
                If nDias < 2 Then
                    weekend = 0
                ElseIf nDias < 7 Then
                    weekend = 2
                ElseIf nDias < 12 Then
                    weekend = 4
                End If
            Case DayOfWeek.Friday
                If nDias < 1 Then
                    weekend = 0
                ElseIf nDias < 6 Then
                    weekend = 2
                ElseIf nDias < 11 Then
                    weekend = 4
                End If
            Case DayOfWeek.Saturday
                If nDias < 6 Then
                    weekend = 2
                ElseIf nDias < 10 Then
                    weekend = 4
                End If
        End Select
        Dim totaldias = weekend + nDias
        fecha_limite = DateAdd(DateInterval.DayOfYear, totaldias, fechaPost)
        Return fecha_limite
    End Function


    Sub Actualiza(ByVal typeACT As String)

        If (typeACT <> "ROL" And typeACT <> "CAT" And typeACT <> "APP") Then
            Me.cmb_rol.DataSource = clss_approval.get_UserRolesALL(Me.Session("E_IDuser"))
            Me.cmb_rol.DataBind()
        End If

        If (typeACT <> "CAT" And typeACT <> "APP") Then

            If Me.cmb_rol.Items.Count > 0 Then

                Me.cmb_cat.DataSource = clss_approval.get_CategoryUser(cmb_rol.SelectedValue)
                Me.cmb_cat.DataBind()
                generate_CODE()

                If Me.cmb_cat.Items.Count > 0 Then
                    Me.cmb_tipoDocumento.DataSource = clss_approval.get_TipoDoc(cmb_cat.SelectedValue)
                    Me.cmb_tipoDocumento.DataBind()
                    If Me.cmb_tipoDocumento.Items.Count = 0 Then
                        cmb_tipoDocumento.DataSource = clss_approval.get_TipoDoc(0) 'Default N/A
                        Me.cmb_tipoDocumento.DataBind()
                    End If

                End If

            End If

        End If

        If (typeACT = "CAT") Then

            If Me.cmb_rol.Items.Count > 0 Then

                If Me.cmb_cat.Items.Count > 0 Then
                    Me.cmb_tipoDocumento.DataSource = clss_approval.get_TipoDoc(cmb_cat.SelectedValue)
                    Me.cmb_tipoDocumento.DataBind()
                    If Me.cmb_tipoDocumento.Items.Count = 0 Then
                        cmb_tipoDocumento.DataSource = clss_approval.get_TipoDoc(0) 'Default N/A
                        Me.cmb_tipoDocumento.DataBind()
                    End If
                End If

            End If

        End If

        If (typeACT <> "APP") Then
            If (Me.cmb_rol.Items.Count > 0) And (Me.cmb_cat.Items.Count > 0) Then
                If Val(cmb_cat.SelectedValue) > 0 Then
                    Me.cmb_app.DataSource = clss_approval.get_ApprovalRole(cmb_rol.SelectedValue, cmb_cat.SelectedValue)
                    Me.cmb_app.DataBind()
                End If
            End If
        End If

        If (Me.cmb_app.Items.Count > 0) Then
            If Val(cmb_app.SelectedValue) > 0 Then
                'Me.rb_tipoDoc.DataSource = clss_approval.get_Approval_DocumentSupport(cmb_app.SelectedValue, lbl_id_sesion_temp.Text)
                'Me.rb_tipoDoc.DataBind()
                grd_documentos.DataSource = clss_approval.get_Approval_DocumentSupport(cmb_app.SelectedValue, lbl_id_sesion_temp.Text)
                grd_documentos.DataBind()
            End If
        End If

        'Me.rb_tipoDoc.DataBind()

        If Me.grd_documentos.Items.Count = 0 Then
            'Me.PanelFirma.Visible = False
            Me.ImageButton1.Visible = False
        Else
            Me.ImageButton1.Visible = True
            'Me.PanelFirma.Visible = True
            'Me.rb_tipoDoc.SelectedIndex = 0
        End If


        'Dim sql = ""
        'sql = "SELECT cod_actividad, condicion FROM vw_aprobaciones WHERE id_tipoDocumento=" & Me.cmb_app.SelectedValue

        Try

            Dim tbl_approval As New DataTable

            If (Me.cmb_app.Items.Count > 0) Then

                If Me.cmb_app.SelectedValue > 0 Then

                    tbl_approval = clss_approval.get_Approvals(Me.cmb_app.SelectedValue)

                    'Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
                    'Dim ds1 As New DataSet("cod_actividad")
                    'dm1.Fill(ds1, "extension")

                    If tbl_approval.Rows.Item(0).Item("cod_actividad") = "SI" Then '****SI ES FICHA DE ACTIVIDAD

                        '  If ds1.Tables("extension").Rows(0).Item(0).ToString = "SI" Then
                        Me.txt_codigoAID.Enabled = True
                        Me.cmb_region.Enabled = True
                        Me.txt_montoTotal.Enabled = True
                        Me.txt_montoProyecto.Enabled = True
                        Me.txt_tasacambio.Enabled = True

                        Me.cmb_region.AllowCustomText = True
                        Me.cmb_region.MarkFirstMatch = True


                    Else

                        Me.txt_codigoAID.Enabled = False
                        Me.cmb_region.Enabled = False
                        Me.txt_montoTotal.Enabled = False
                        Me.txt_montoProyecto.Enabled = False
                        Me.txt_tasacambio.Enabled = False

                    End If

                    Me.lbl_condition.Text = tbl_approval.Rows.Item(0).Item("condicion")  'ds1.Tables("extension").Rows(0).Item("condicion").ToString

                Else

                    Me.txt_codigoAID.Enabled = False
                    Me.lbl_condition.Text = ""

                End If

            End If

        Catch ex As Exception

            Me.txt_codigoAID.Enabled = False
            Me.lbl_condition.Text = ""

        End Try

    End Sub

    Function CrearCodigo() As String
        Dim codigoApp As String = ""

        If Me.cmb_cat.SelectedValue.Trim <> "" Then

            Dim CorrrelativoStr As String = ""
            Dim dm As New SqlDataAdapter("SELECT cod_categoria, correlativos FROM ta_categoria WHERE id_categoria=" & Me.cmb_cat.SelectedValue.Trim, cnnSAP)
            Dim ds As New DataSet("Codigos")
            dm.Fill(ds, "Codigos")
            Dim Corrrelativo As Integer = (Val(ds.Tables("Codigos")(0)("correlativos")) + 1)
            If Corrrelativo < 10 Then
                CorrrelativoStr = "000"
            ElseIf Corrrelativo < 100 Then
                CorrrelativoStr = "00"
            ElseIf Corrrelativo < 1000 Then
                CorrrelativoStr = "0"
            End If

            codigoApp = ds.Tables("Codigos")(0)("cod_categoria") & "-" & CorrrelativoStr & Corrrelativo
        End If

        Return codigoApp

    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login.aspx")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            Try
                Dim rnd As New Random()
                Dim fecha As DateTime = Date.Now
                Dim textfecha = fecha.ToString.Replace("/", "").Replace(" ", "").Replace("a", "").Replace(".", "").Replace("m", "").Replace(":", "").Replace(";", "").Replace("p", "")
                Me.lbl_id_sesion_temp.Text = rnd.Next(1, 999).ToString & textfecha.ToString
            Catch ex As Exception
                Me.lbl_id_sesion_temp.Text = "-1"
            End Try
            Dim Sql = ""

            Me.HiddenField1.Value = 0
            'Sql = String.Format("SELECT id_rol, nombre_rol, descripcion_rol, id_usuario, usuario, id_programa FROM vw_ta_roles_emplead " &
            '                     " WHERE id_programa = {0}  And id_usuario = {1} ", Me.Session("E_IDPrograma"), Me.Session("E_IdUser"))

            'cmb_rol.DataSource = clss_approval.get_UserRoles() 'All roles USer

            Dim tbl_user_role As New DataTable
            'tbl_user_role = clss_approval.get_UserRoles(Me.Session("E_IdUser"))
            tbl_user_role = clss_approval.get_UserRolesALL(Me.Session("E_IdUser"))

            'Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds1 As New DataSet("proyecto")
            'dm1.Fill(ds1, "proyecto")
            'If ds1.Tables("proyecto").Rows.Count > 0 Then 'The user has a Role 

            If tbl_user_role.Rows.Count > 0 Then 'The user has a Role, included th3 groups 

                Me.lblt_rolBegin.Visible = True
                Me.cmb_rol.Visible = True
                Me.lblt_rol_user.Visible = False

            Else 'lookin into a group with Approve rights

                ' *********************If if this is a memeber of group with a option to create************************************

                'Sql = "SELECT id_rol, id_usuario FROM  ta_Roles  WHERE (id_usuario = " & Me.Session("E_IdUser") & ")"
                '    Dim dm As New SqlDataAdapter(Sql, cnnSAP)
                '    Dim ds As New DataSet("role")
                '    dm.Fill(ds, "roles")

                '    Dim selected = " "
                '    Dim id_rol As Integer

                '    If ds.Tables("roles").Rows.Count = 0 Then

                'Sql = "SELECT id_rol, id_usuario FROM  ta_gruposRoles  WHERE aprueba='SI' AND (id_usuario = " & Me.Session("E_IdUser") & ")"
                '        ds.Tables.Add("condicion")
                '        dm.SelectCommand.CommandText = Sql
                '        dm.Fill(ds, "condicion")

                ''****************Dim tbl_groups_user As New DataTable
                ''****************tbl_groups_user = clss_approval.get_Groups_User(Me.Session("E_IdUser"))
                ''****************If tbl_groups_user.Rows.Count > 0 Then

                'If ds.Tables("condicion").Rows.Count > 0 Then

                'selected = " OR id_rol=" & ds.Tables("condicion").Rows(0).Item("id_rol") & " "
                'Sql = "SELECT id_rol, nombre_rol, descripcion_rol, id_usuario, usuario, id_programa FROM vw_ta_roles_emplead WHERE (id_programa = " & Me.Session("E_IDPrograma") & ") AND (id_usuario=" & Me.Session("E_IdUser") & ") " & selected

                'id_rol = ds.Tables("condicion").Rows(0).Item("id_rol")

                ''****************Dim roles As String = ""
                ''****************For Each dtR As DataRow In tbl_groups_user.Rows
                ''****************Roles &= dtR("id_rol") & ", "
                ''****************Next
                ''****************Roles = String.Format(" ( {0} ) ", roles.Substring(0, roles.Length - 1))

                'Sql = String.Format(" SELECT id_rol, nombre_rol, descripcion_rol, id_usuario, usuario, id_programa FROM vw_ta_roles_emplead  " &
                '                    "    WHERE id_rol= {0} ", id_rol)

                ' Me.lblt_rolBegin.Visible = True
                'Me.cmb_rol.Visible = False
                'Me.lblt_rol_user.Visible = True

                'Me.sql_rol.SelectCommand = Sql & " ORDER BY 2"
                ''****************Me.cmb_rol.DataSource = clss_approval.get_UserRoles(, roles)
                'Me.cmb_rol.DataBind()

                'Me.lblt_rol_user.Text = Me.cmb_rol.SelectedItem.Text
                'Else

                Me.lblt_rol_user.Text = "N/A"

            End If

            'Else
            '    Me.lblt_rol_user.Text = "N/A"
            'End If

            ' *********************If if this is a memeber of group with a option to create************************************

            'End If

            'Sql = "SELECT nombre_proyecto FROM vw_proyectos WHERE id_programa=" & Me.Session("E_IDPrograma")
            'Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds1 As New DataSet("proyecto")
            'dm1.Fill(ds1, "proyecto")
            'Me.lbl_proyecto.Text = ds1.Tables("proyecto").Rows(0).Item(0).ToString


            Actualiza("")


            'If cmb_cat.Items.Count > 0 Then

            '    Me.lbl_idDocumento.Text = clss_approval.Approval_CodeCreate(cmb_cat.SelectedValue)

            '    Dim strCode As String = Me.lbl_idDocumento.Text.Trim
            '    Dim strResultCode As String = ""
            '    Dim vTotal As Integer = strCode.Length
            '    Dim vSubstr As Integer = strCode.IndexOf("-")

            '    strResultCode = String.Format("{0}-0{1}-{2}", strCode.Substring(0, strCode.IndexOf("-") - 1), Date.Today.Year.ToString.Substring(2, 2), strCode.Substring(strCode.IndexOf("-") + 1, vTotal - (vSubstr + 1)))
            '    txt_Number.Text = strResultCode

            'End If

            formatoNumero()

            Me.txt_tasacambio.Text = clss_approval.tbl_t_Programa.Rows.Item(0).Item("tasa_cambio")

            '*************************************************************************************************

            If Me.grd_documentos.Items.Count = 0 Then
                'Me.PanelFirma.Visible = False
            Else
                'Me.PanelFirma.Visible = True
            End If

            HttpContext.Current.Session.Add("clss_approval", clss_approval)

        Else

            If HttpContext.Current.Session.Item("clss_approval") IsNot Nothing Then
                clss_approval = Me.Session.Item("clss_approval")
            End If

            formatoNumero()


        End If


    End Sub


    Public Sub generate_CODE()

        If cmb_cat.Items.Count > 0 Then

            Me.lbl_idDocumento.Text = clss_approval.Approval_CodeCreate(cmb_cat.SelectedValue)

            Dim strCode As String = Me.lbl_idDocumento.Text.Trim
            Dim strResultCode As String = ""
            Dim vTotal As Integer = strCode.Length
            Dim vSubstr As Integer = strCode.IndexOf("-")
            Dim Index As Integer = 0

            If vSubstr > 0 Then
                Index = strCode.IndexOf("-") - 1
            Else
                Index = 0
            End If

            'strCode.Substring(0, strCode.IndexOf("-") - 1)
            Dim p1 As String = strCode.Substring(0, Index)
            If p1.Length = 0 Then
                p1 = "APP"
            End If
            ' strCode.Substring(strCode.IndexOf("-") + 1, vTotal - (vSubstr + 1))
            Dim p2 As String = strCode.Substring(strCode.IndexOf("-") + 1, vTotal - (vSubstr + 1))

            strResultCode = String.Format("{0}-0{1}-{2}", p1, Date.Today.Year.ToString.Substring(2, 2), p2)
            txt_Number.Text = strResultCode
            txt_codigoAID.Text = strResultCode

        End If



    End Sub

    'Protected Sub rb_tipoDoc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_tipoDoc.SelectedIndexChanged

    '    'Dim sql = ""
    '    'sql = "SELECT extension FROM ta_docs_soporte WHERE id_doc_soporte=" & Me.rb_tipoDoc.SelectedValue
    '    'Dim dm1 As New SqlDataAdapter(sql, cnnSAP)
    '    'Dim ds1 As New DataSet("extension")
    '    'dm1.Fill(ds1, "extension")

    '    Me.Uploader3.ValidateOption.AllowedFileExtensions = clss_approval.get_DocumentSupport_Extension(Me.rb_tipoDoc.SelectedValue).Rows().Item(0).Item("extension") 'ds1.Tables("extension").Rows(0).Item(0).ToString

    'End Sub

    Protected Sub cmb_app_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_app.SelectedIndexChanged

        Actualiza("APP")

        '*********To import from Contract System Not in this fase****************
        'If Me.cmb_cat.SelectedItem.Text.Contains("Fund") Then
        '    Me.hlnk_import.Visible = True
        'Else
        '    Me.hlnk_import.Visible = False
        'End If
        '*********To import from Contract System Not in this fase****************


    End Sub

    Protected Sub cmb_cat_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_cat.SelectedIndexChanged



        Actualiza("CAT")

        Me.lbl_idDocumento.Text = clss_approval.Approval_CodeCreate(cmb_cat.SelectedValue)

        generate_CODE()

        '*********To import from Contract System Not in this fase****************
        'If Me.cmb_cat.SelectedItem.Text.Contains("Fund") Then
        '    Me.hlnk_import.Visible = True
        'Else
        '    Me.hlnk_import.Visible = False
        'End If
        '*********To import from Contract System Not in this fase****************

    End Sub


    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click


        If Val(cmb_app.SelectedValue) > 0 Then
            'Me.rb_tipoDoc.DataSource = clss_approval.get_Approval_DocumentSupport(cmb_app.SelectedValue, lbl_id_sesion_temp.Text)
            'Me.rb_tipoDoc.DataBind()

            grd_documentos.DataSource = clss_approval.get_Approval_DocumentSupport(cmb_app.SelectedValue, lbl_id_sesion_temp.Text)
            grd_documentos.DataBind()
        End If

        'If Me.rb_tipoDoc.Items.Count > 0 Then
        '    Me.rb_tipoDoc.SelectedIndex = 0
        'End If

    End Sub



    Protected Sub cmb_rol_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_rol.SelectedIndexChanged

        'Me.cmb_cat.DataBind()
        'cmb_app.DataBind()
        Actualiza("ROL")

    End Sub

    'Protected Overloads Overrides Sub OnInit(ByVal e As EventArgs)
    '    MyBase.OnInit(e)
    '    AddHandler Uploader3.FileUploaded, AddressOf Uploader_FileUploaded

    'End Sub




    Private Sub Save_NewFiles(ByVal id_app_doc As Integer, ByVal bndTool As String)

        '****************************************************************************************************
        '**************************DOCUMENTOS ADICIONALES EN EL VERSION ACTUAL******************************


        For Each item As RadListBoxItem In rdListBox_files.Items

            Dim a = item.Text
            Dim b = item.Value
            'item.Checked = True

            clss_approval.set_ta_archivos_documento(0) 'New Record
            clss_approval.set_ta_archivos_documentoFIELDS("id_App_Documento", id_app_doc, "id_archivo", 0)
            clss_approval.set_ta_archivos_documentoFIELDS("archivo", item.Text, "id_archivo", 0)
            clss_approval.set_ta_archivos_documentoFIELDS("id_doc_soporte", CType(item.Value, Integer), "id_archivo", 0)
            clss_approval.set_ta_archivos_documentoFIELDS("ver", 1, "id_archivo", 0)

            If bndTool = "DELIV-RMS01" Then '--Deliverable Tools

                Dim cls_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

                Dim tbl_Deliverable As DataTable = cls_Deliverable.Deliv_Document(0, CType(Me.HiddenField1.Value, Integer))
                Dim tbl_ta_documento_deliverable As New ta_deliverable_support_docs

                tbl_ta_documento_deliverable.id_deliverable = tbl_Deliverable.Rows.Item(0).Item("id_deliverable")
                tbl_ta_documento_deliverable.archivo = item.Text
                tbl_ta_documento_deliverable.id_doc_soporte = CType(item.Value, Integer)
                tbl_ta_documento_deliverable.ver = 1

                If cls_Deliverable.Save_deliverable_support_docs(tbl_ta_documento_deliverable, 0) <> -1 Then
                    'Error here
                End If


            End If

            If clss_approval.save_ta_archivos_documento() <> -1 Then 'Erro Happenned
                'If bndCopy Then
                '    CopyFileParam(item.Text) 'this has been done at the begining
                'End If
            End If

        Next

        'Dim listItemFile As New ListItem
        'For i = 0 To Me.ListBox_file.Items.Count - 1

        '    clss_approval.set_ta_archivos_documento(0) 'New Record
        '    clss_approval.set_ta_archivos_documentoFIELDS("id_App_Documento", id_app_doc, "id_archivo", 0)
        '    clss_approval.set_ta_archivos_documentoFIELDS("archivo", Me.ListBox_file.Items(i).Text, "id_archivo", 0)
        '    clss_approval.set_ta_archivos_documentoFIELDS("id_doc_soporte", CType(Me.ListBox_file.Items(i).Value, Integer), "id_archivo", 0)
        '    clss_approval.set_ta_archivos_documentoFIELDS("ver", 1, "id_archivo", 0)

        '    If clss_approval.save_ta_archivos_documento() <> -1 Then 'Erro Happenned
        '        CopyFileParam(Me.ListBox_file.Items(i).Text)
        '    End If

        'Next
        '****************************************************************************************************
        '**************************DOCUMENTOS ADICIONALES EN EL VERSION ACTUAL******************************

    End Sub

    'Private Sub Uploader_FileUploaded(ByVal sender As Object, ByVal args As UploaderEventArgs)
    '    Dim uploader As Uploader = DirectCast(sender, Uploader)
    '    'Dim sFileDir As String = Server.MapPath("~") & "~\FileUploads\Temp\"
    '    Dim sFileDir As String = "~\FileUploads\Temp\"


    '    Try

    '        If hd_id_doc.Value > 0 Then

    '            lbl_errExtension.Visible = False

    '            Dim Random As New Random()
    '            Dim extension As String = System.IO.Path.GetExtension(args.FileName)
    '            Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(args.FileName)
    '            Dim File As String = fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace("+", "_").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension

    '            args.CopyTo(sFileDir & File)
    '            Me.lblarchivo.Text = File
    '            Me.HnlkArchivo.NavigateUrl = "~\FileUploads\Temp\" & File
    '            Dim j = 0

    '            '******************************REV 0.0.0.1********************

    '            Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(Me.lblarchivo.Text)
    '            'Dim extension As String = System.IO.Path.GetExtension(Me.lblarchivo.Text)
    '            Dim archivo = sFileName.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace("+", "_").Replace(".", "-").Replace(",", "-").Replace("&", "-") + extension

    '            'Dim sql = " INSERT INTO ta_archivos_documento_temp(id_sesion_temp, archivo, id_doc_soporte)  
    '            'VALUES('" & Me.lbl_id_sesion_temp.Text & "','" & Me.lblarchivo.Text & "'," & Me.rb_tipoDoc.SelectedValue & ")"
    '            'Dim dm1 As New SqlDataAdapter(sql, cnnSAP)
    '            'Dim ds1 As New DataSet("extension")
    '            'dm1.Fill(ds1, "extension")

    '            clss_approval.set_ta_archivos_documento_temp(0) 'New Record
    '            clss_approval.set_ta_archivos_documento_tempFIELDS("id_sesion_temp", Me.lbl_id_sesion_temp.Text, "id_archivo_temp", 0)
    '            clss_approval.set_ta_archivos_documento_tempFIELDS("archivo", Me.lblarchivo.Text, "id_archivo_temp", 0)
    '            clss_approval.set_ta_archivos_documento_tempFIELDS("id_doc_soporte", hd_id_doc.Value, "id_archivo_temp", 0)

    '            If clss_approval.save_ta_archivos_documento_temp() <> -1 Then '***********Saving*****************

    '                'Me.rb_tipoDoc.Enabled = True

    '                Me.grd_archivos.DataBind()
    '                Me.lblarchivo.Text = ""

    '                'Me.rb_tipoDoc.DataSource = clss_approval.get_Approval_DocumentSupport(cmb_app.SelectedValue, lbl_id_sesion_temp.Text)
    '                'Me.rb_tipoDoc.DataBind()

    '                grd_documentos.DataSource = clss_approval.get_Approval_DocumentSupport(cmb_app.SelectedValue, lbl_id_sesion_temp.Text)
    '                grd_documentos.DataBind()


    '                Actualiza("APP") 'Let teh dataSets

    '                Me.Panel1_firma.Visible = False

    '                'If Me.rb_tipoDoc.Items.Count > 0 Then
    '                '    Me.rb_tipoDoc.SelectedIndex = 0
    '                'End If
    '                Me.lblMsg.Text = ""

    '            End If

    '        Else
    '            lbl_errExtension.Visible = True
    '        End If



    '    Catch ex As Exception

    '        Me.img_btn_borrar_temp.ImageUrl = "../imagenes/Iconos/s_warn.png"
    '        Me.lblarchivo.Text = "Error.."


    '    End Try

    '    'Me.Panel1_firma.Visible = True
    '    'Me.rb_tipoDoc.Enabled = False

    'End Sub


    '*********************************************+********************************************************************+**********************************
    '*********************************************+********************************************************************+**********************************
    '*********************************************+********************************************************************+**********************************

    Sub DelFileParam(ByVal archivo As String)
        Dim sFileName As String = System.IO.Path.GetFileName(archivo)
        Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Temp\"
        Dim file_info As New IO.FileInfo(sFileDir + sFileName)
        If (file_info.Exists) Then
            file_info.Delete()
        End If
        'Me.lblarchivo.Text = ""
        'Me.Panel1_firma.Visible = False
    End Sub

    'Sub DelFileParam(ByVal archivo As String)

    '    Dim sFileName As String = System.IO.Path.GetFileName(archivo)
    '    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Temp\" '"~\FileUploads\Temp\"
    '    Dim file_info As New IO.FileInfo(sFileDir + sFileName)
    '    If (file_info.Exists) Then
    '        file_info.Delete()
    '    End If
    '    Me.lblarchivo.Text = ""
    '    Me.Panel1_firma.Visible = False
    'End Sub


    'Sub DelFile()
    '    Dim sFileName As String = System.IO.Path.GetFileName(Me.lblarchivo.Text)
    '    Dim extension As String = System.IO.Path.GetExtension(Me.lblarchivo.Text)
    '    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Temp\" ' "~\FileUploads\Temp\"
    '    Dim file_info As New IO.FileInfo(sFileDir + sFileName)
    '    If (file_info.Exists) Then
    '        file_info.Delete()
    '    End If
    '    Me.lblarchivo.Text = ""
    '    Me.lblMsg.Text = ""
    '    Me.Panel1_firma.Visible = False
    'End Sub


    'Sub CopyFile()
    '    Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(Me.lblarchivo.Text)
    '    Dim extension As String = System.IO.Path.GetExtension(Me.lblarchivo.Text)
    '    Dim sFileDirTemp As String = Server.MapPath("~") & "\FileUploads\Temp\" '"~\FileUploads\Temp\" 
    '    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\ApprovalProcc\" ' "~\FileUploads\ApprovalProcc\" 
    '    Dim file_info As New IO.FileInfo(sFileDirTemp + sFileName + extension)
    '    Try
    '        file_info.CopyTo(sFileDir & sFileName.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", ".").Replace(",", "-") + extension)
    '    Catch ex As Exception
    '    End Try
    '    DelFile()
    '    ' Me.Panel1_firma.Visible = False
    'End Sub


    Sub CopyFileParam(ByVal file As String, ByVal nw_NameFile As String)

        'Dim dmyhm As String = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
        'Dim extension As String = System.IO.Path.GetExtension(file)
        'Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(file) + "_v1.1"
        'Dim strFile As String '= "doc" & idDoc & "_0" & Me.Session("E_IdUser") & "_" & dmyhm & "_" & fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension
        'strFile = String.Format("doc{0}_0{1}_{2}_{3}{4}", idDoc, Me.Session("E_IdUser"), dmyhm, fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-"), extension)

        'Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(file)
        Dim sFileDirTemp As String = Server.MapPath("~") & "\FileUploads\Temp\" ' "~\FileUploads\Temp\" 'Server.MapPath("~") & "\Temp\"
        Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\ApprovalProcc\" '"~\FileUploads\ApprovalProcc\" '
        Dim file_info As New IO.FileInfo(sFileDirTemp + file)
        Try
            file_info.CopyTo(sFileDir & nw_NameFile)
        Catch ex As Exception
        End Try
        DelFileParam(file)
        'Me.Panel1_firma.Visible = False
    End Sub


    'Protected Sub img_btn_agregar_temp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_btn_agregar_temp.Click

    '    'Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(Me.lblarchivo.Text)
    '    'Dim extension As String = System.IO.Path.GetExtension(Me.lblarchivo.Text)
    '    'Dim archivo = sFileName.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "-").Replace(",", "-").Replace("&", "-") + extension

    '    'Dim sql = " INSERT INTO ta_archivos_documento_temp(id_sesion_temp, archivo, id_doc_soporte)  VALUES('" & Me.lbl_id_sesion_temp.Text & "','" & Me.lblarchivo.Text & "'," & Me.rb_tipoDoc.SelectedValue & ")"
    '    'Dim dm1 As New SqlDataAdapter(sql, cnnSAP)
    '    'Dim ds1 As New DataSet("extension")
    '    'dm1.Fill(ds1, "extension")
    '    'Me.rb_tipoDoc.Enabled = True
    '    'Me.grd_archivos.DataBind()
    '    'Me.rb_tipoDoc.DataBind()
    '    'Me.lblarchivo.Text = ""
    '    'Me.Panel1_firma.Visible = False
    '    'If Me.rb_tipoDoc.Items.Count > 0 Then
    '    '    Me.rb_tipoDoc.SelectedIndex = 0
    '    'End If


    '    '******************************REV 0.0.0.1********************
    '    If Not String.IsNullOrEmpty(Me.lblarchivo.Text) Then
    '        Me.lblMsg.Text = LTrim(RTrim(Me.lblarchivo.Text)) & ", ya fué registrado en la lista de archivos."
    '        ' Me.lblarchivo.Text = ""
    '    End If
    '    '******************************REV 0.0.0.1********************


    'End Sub

    'Protected Sub img_btn_borrar_temp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_btn_borrar_temp.Click
    '    DelFile()
    '    'Me.rb_tipoDoc.Enabled = True
    'End Sub


    'Protected Sub grd_archivos_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_archivos.DeleteCommand

    '    Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
    '    Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_archivo_temp").ToString()

    '    'cnnSAP.Open()
    '    'Dim dm As New SqlCommand("DELETE FROM ta_archivos_documento_temp WHERE (id_archivo_temp = " & id_temp & ")", cnnSAP)
    '    'dm.ExecuteNonQuery()
    '    'cnnSAP.Close()

    '    If clss_approval.del_ta_archivos_documento_temp(id_temp) Then

    '        DelFileParam(itemD("ruta_archivos").Text) 'e.Item.Cells(4).Text.ToString)
    '        Me.grd_archivos.DataBind()

    '    End If


    '    'Me.rb_tipoDoc.DataSource = clss_approval.get_Approval_DocumentSupport(cmb_app.SelectedValue, lbl_id_sesion_temp.Text)
    '    'Me.rb_tipoDoc.DataBind()

    '    grd_documentos.DataSource = clss_approval.get_Approval_DocumentSupport(cmb_app.SelectedValue, lbl_id_sesion_temp.Text)
    '    grd_documentos.DataBind()

    '    'If Me.rb_tipoDoc.Items.Count > 0 Then
    '    '    Me.rb_tipoDoc.SelectedIndex = 0
    '    'End If


    'End Sub

    <Web.Services.WebMethod()>
    Public Shared Function get_DocTYPE(ByVal idProgram As Integer, ByVal id_TipoDoc As Integer, ByVal IdDoc As Integer) As Object


        Dim clss_approval As ly_APPROVAL.APPROVAL.clss_approval = New APPROVAL.clss_approval(idProgram)
        Dim tbl_DOCS As DataTable = New DataTable
        tbl_DOCS = clss_approval.get_Doc_support_Route_PendingALL(CType(id_TipoDoc, Integer), CType(IdDoc, Integer))

        Dim JsonResult As String
        Dim serializer As New JavaScriptSerializer()

        Dim list_TypeDOC As Object = (From dr In tbl_DOCS.AsEnumerable() Select (New With {
                                                Key .id_doc_soporte = dr.Field(Of Int32)("id_doc_soporte"),
                                                Key .nombre_documento = dr.Field(Of String)("nombre_documento"),
                                                Key .id_programa = dr.Field(Of Int32)("id_programa"),
                                                Key .Template = dr.Field(Of String)("Template"),
                                                Key .extension = dr.Field(Of String)("extension"),
                                                Key .max_size = dr.Field(Of Decimal)("max_size"),
                                                Key .id_app_docs = dr.Field(Of Int32)("id_app_docs"),
                                                Key .id_tipoDocumento = dr.Field(Of Int32)("id_tipoDocumento"),
                                                Key .PermiteRepetir = dr.Field(Of String)("PermiteRepetir"),
                                                Key .RequeridoInicio = dr.Field(Of String)("RequeridoInicio"),
                                                Key .RequeridoFin = dr.Field(Of String)("RequeridoFin")})).ToList()

        JsonResult = serializer.Serialize(list_TypeDOC)

        get_DocTYPE = JsonResult

    End Function



    Public Sub Check_NewFile_FileUploaded()

        'Watch it on the server do you need to have \FileUploads\Temp\, but on developer side you need to try with FileUploads\Temp\

        Dim sFileDirTemp As String = Server.MapPath("~") & "\FileUploads\Temp\"
        Dim sFileDir As String = Server.MapPath("~") & "\fileUploads\ApprovalProcc\"
        Dim dmyhm As String
        Dim extension As String
        Dim fileNameWE As String
        Dim fileNew_name As String
        Dim File As String


        Try

            If Convert.ToBoolean(Me.lbl_hasFiles.Value) Then 'It has a values

                lbl_errExtension.Visible = False

                Dim i As Integer = 0
                For Each item As RadListBoxItem In rdListBox_files.Items

                    'Dim a = item.Text
                    'Dim b = item.Value
                    'item.Checked = True

                    dmyhm = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
                    extension = System.IO.Path.GetExtension(item.Text.Trim)
                    fileNameWE = System.IO.Path.GetFileNameWithoutExtension(item.Text.Trim)


                    Dim FileOrginal As String
                    FileOrginal = If((fileNameWE.Length > 13), fileNameWE.Substring(13, fileNameWE.Length - 13), fileNameWE)


                    If FileOrginal.Length > 80 Then
                        fileNew_name = FileOrginal.Substring(0, 80)
                    Else
                        fileNew_name = FileOrginal
                    End If

                    File = String.Format("doc{0}_0{1}_{2}_{3}{4}{5}", Me.HiddenField1.Value, Me.Session("E_IdUser"), dmyhm, FileOrginal.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-").Replace("'", "").Replace("""", "").Replace("#", ""), "_v1.1", extension)

                    Dim file_info As New IO.FileInfo(sFileDirTemp & fileNameWE & extension)

                    Try

                        If (file_info.Exists) Then
                            file_info.CopyTo(sFileDir & File)
                            DelFileParam(fileNameWE & extension)
                        Else

                            file_info = New IO.FileInfo(sFileDirTemp & FileOrginal & extension)
                            file_info.CopyTo(sFileDir & File)
                            DelFileParam(FileOrginal & extension)

                        End If

                        rdListBox_files.Items.Item(i).Text = File

                        'Me.lblarchivo.Text = File

                        i += 1

                    Catch ex As Exception

                        lblerr_user.Text = ex.Message

                    End Try

                    '    grd_documentos.DataSource = clss_approval.get_Doc_support_Route_PendingALL(CType(Me.lblTipoDoc.Text, Integer), Me.HiddenField1.Value)
                    '    grd_documentos.DataBind()

                Next

            Else

                lbl_errExtension.Visible = True

            End If

            '    e.File.SaveAs(sFileDir & File)
            '    'args.CopyTo(sFileDir & File)
            '    Me.lblarchivo.Text = File

            '    Dim itemList As New ListItem
            '    itemList.Text = Me.lblarchivo.Text
            '    itemList.Value = hd_id_doc.Value
            '    Me.ListBox_file.Items.Add(itemList)
            '    Me.Panel1_firma.Visible = False
            '    Me.lblMsg.Text = ""


        Catch ex As Exception
            lblerr_user.Text = "NewFile: " & ex.Message
        End Try


    End Sub




    'Protected Sub grd_archivos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_archivos.ItemDataBound
    '    If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

    '        Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
    '        Dim ImageDownload As New HyperLink
    '        ImageDownload = CType(itemD("ImageDownloadC").FindControl("ImageDownload"), HyperLink)   'CType(e.Item.FindControl("ImageDownload"), HyperLink)
    '        ImageDownload.NavigateUrl = "~\FileUploads\Temp\" & itemD("ruta_archivos").Text 'e.Item.Cells(4).Text.ToString
    '        ImageDownload.Target = "_blank"

    '    End If
    'End Sub


    Public Sub SaveComment(ByVal idApp As Integer, ByVal idEstadoDoc As Integer, ByVal Comment As String)

        Dim strComment As String
        If Trim(Comment).Length = 0 Then
            strComment = "--No Comments--"
        Else
            strComment = Comment
        End If

        'cnnSAP.Open()
        'Dim SqlInsert As String = "INSERT INTO ta_comentariosDoc (id_App_Documento, id_estadoDoc, id_tipoAccion, id_usuario, comentario) 
        '      VALUES(" & idApp & ", " & idEstadoDoc & ", " & 1 & ", " & Me.Session("E_IdUser").ToString.Trim & ",'" & strComment.Trim & "') " '.Replace("'", "''").Replace("  ", "")
        'Dim dmt As New SqlDataAdapter(SqlInsert, cnnSAP)
        'Dim ds As New DataSet("IDComentario")
        'dmt.Fill(ds, "IDComentario")
        'cnnSAP.Close()

        clss_approval.set_ta_comentariosDoc(0) 'New Record
        clss_approval.set_ta_comentariosDocFIELDS("id_App_Documento", idApp, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("id_estadoDoc", idEstadoDoc, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("id_tipoAccion", cAction_ByProcess, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("id_usuario", Me.Session("E_IdUser"), "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("fecha_comentario", Date.UtcNow, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("comentario", strComment.Trim.Replace("  ", ""), "id_comment", 0) '.Replace("'", "''")

        If clss_approval.save_ta_comentariosDoc() = -1 Then
            'Error do somenthing

        End If


    End Sub


    Protected Sub txt_tasacambio_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_tasacambio.TextChanged
        Recualcualar()
    End Sub

    Protected Sub txt_montoProyecto_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_montoProyecto.TextChanged
        Recualcualar()
    End Sub

    Protected Sub txtmontoTotal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_montoTotal.TextChanged
        Recualcualar()
    End Sub
    Sub Recualcualar()
        If Val(Me.txt_tasacambio.Text) <> 0 Then
            Me.lblt_totalproyectoUS.Text = FormatNumber(Val(Me.txt_montoProyecto.Text) / Val(Me.txt_tasacambio.Text), 2, , TriState.True, TriState.True).ToString
            Me.lbl_totalUS.Text = FormatNumber(Val(Me.txt_montoTotal.Text) / Val(Me.txt_tasacambio.Text), 2, , TriState.True, TriState.True).ToString
        Else
            Me.lblt_totalproyectoUS.Text = "0.00"
            Me.lbl_totalUS.Text = "0.00"
        End If
    End Sub

    Protected Sub btn_Cancel_Click(sender As Object, e As EventArgs) Handles btn_Cancel.Click
        Me.Response.Redirect("~/Approvals/frm_consulta_docsPending.aspx")
    End Sub

    Protected Sub btn_Open_Click(sender As Object, e As EventArgs) Handles btn_Open.Click

        Dim err As Boolean = False
        lblerr_user.Text = ""
        'Dim row As GridItem
        Dim sql As String = ""
        Dim fecha As DateTime = Date.Now
        Dim textfecha = fecha.ToString.Replace("/", "").Replace(" ", "").Replace("a", "").Replace(".", "").Replace("m", "").Replace(":", "").Replace(";", "").Replace("p", "")
        Dim rnd As New Random()

        Dim codigo_sap As String = Me.Session("E_IDPrograma") & "-" & Me.cmb_cat.SelectedItem.Text.Substring(0, 3) & "-" & Me.cmb_app.SelectedItem.Text.Substring(0, 3) & "-" & textfecha.Substring(0, 8) & "-" & rnd.Next(1, 999).ToString
        Dim montoProyecto As Double = 0.0
        Dim montoTotal As Double = 0.0
        Dim TasaCambio As Double = 0.0

        Dim region As String = ""
        Dim cod_actividad = "NO"

        Dim id_appdocumento As Integer = 0
        Dim id_documento As Integer = 0

        'sql = "SELECT cod_actividad, condicion FROM vw_aprobaciones WHERE id_tipoDocumento=" & Me.cmb_app.SelectedValue
        'Dim dmFicha As New SqlDataAdapter(sql, cnnSAP)
        'Dim dsFicha As New DataSet("ficha")
        'dmFicha.Fill(dsFicha, "ficha")

        Dim tbl_Approvals As New DataTable

        tbl_Approvals = clss_approval.get_Approvals(Me.cmb_app.SelectedValue)

        If tbl_Approvals.Rows.Item(0).Item("cod_Actividad").ToString = "SI" Then
            'If dsFicha.Tables("ficha").Rows(0).Item("cod_actividad").ToString = "SI" Then
            cod_actividad = "SI"
            montoProyecto = Val(Me.txt_montoProyecto.Text)
            If montoProyecto = 0.0 Then
                'err = True
            End If
            montoTotal = Val(Me.txt_montoTotal.Text)
            If montoTotal = 0.0 Then
                err = True
            End If
            TasaCambio = Val(Me.txt_tasacambio.Text)
            If TasaCambio = 0.0 Then
                err = True
            End If

            Try
                region = Me.cmb_region.SelectedItem.Text
            Catch ex As Exception
                err = True
            End Try

        End If

        If Val(cmb_cat.SelectedValue) > 0 Then
            Dim tbl_Doc As DataTable
            Dim Bool_pendingDoc As Boolean = False
            Dim Bool_findDoc As Boolean = False
            Dim str_pendingDoc As String = ""
            Dim i As Integer = 0
            Dim strPart1 As String
            Dim strPart2 As String

            'tbl_Doc = clss_approval.get_Approval_DocumentSupportPending_ByTMP(cmb_app.SelectedValue, lbl_id_sesion_temp.Text)
            tbl_Doc = clss_approval.get_Approval_DocumentSupportSELECTED(cmb_app.SelectedValue)

            For Each dtRow As DataRow In tbl_Doc.Rows

                If dtRow("requeridoInicio") = "SI" Then

                    For Each item As RadListBoxItem In rdListBox_files.Items

                        If dtRow("id_doc_soporte") = item.Value Then
                            Bool_findDoc = True
                        End If

                    Next

                    If Not Bool_findDoc Then
                        Bool_pendingDoc = True
                        str_pendingDoc = """" & dtRow("nombre_documento") & """"", "
                        i += 1
                    End If

                    Bool_findDoc = False

                End If

            Next

            If Bool_pendingDoc Then
                If i > 1 Then
                    strPart1 = "s"
                    strPart2 = "are"
                Else
                    strPart1 = ""
                    strPart2 = "is"
                End If
                str_pendingDoc = str_pendingDoc.Substring(0, str_pendingDoc.Trim.Length - 1)
                lblerr_user.Text = String.Format("The document{0}: {1} {2} required. Please attached it before continue", strPart1, str_pendingDoc, strPart2)
                err = True
            End If

        End If



        If err = False Then

            Try

                'cnnSAP.Open()
                'sql = " INSERT INTO ta_documento(id_tipoDocumento, id_programa, numero_instrumento, descripcion_doc, nom_beneficiario, comentarios, codigo_AID, codigo_SAP_APP, 
                'ficha_actividad, monto_ficha, regional,codigo_Approval,id_tipoAprobacion,monto_total,tasa_cambio) "
                'sql &= "  VALUES(" & Me.cmb_app.SelectedValue & "," & Me.Session("E_IDPrograma") & ",'" & Me.txt_Number.Text & "','" & Me.txt_Doc.Text.trim & "','" & Me.txt_beneficiario.Text & "','" & Me.txt_coments.Text.Replace("'", "''") & "','" & Me.txt_codigoAID.Text & "','" & codigo_sap.ToString & "','" & 
                'cod_actividad.ToString & "'," & montoProyecto & ",'" & region & "','" & CrearCodigo() & "'," & Me.cmb_tipoDocumento.SelectedValue & "," & montoTotal & "," & TasaCambio & ")"
                'sql &= " SELECT @@IDENTITY"

                clss_approval.set_ta_documento(0) 'Set new Record
                clss_approval.set_ta_documentoFIELDS("id_tipoDocumento", Me.cmb_app.SelectedValue, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("id_programa", Me.Session("E_IDPrograma"), "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("numero_instrumento", Me.txt_Number.Text, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("descripcion_doc", Me.txt_Doc.Text.Trim, "id_documento", 0) '.Replace("'", "''")
                clss_approval.set_ta_documentoFIELDS("nom_beneficiario", Me.txt_beneficiario.Text, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("comentarios", Me.txt_coments.Text.Trim, "id_documento", 0) '.Replace("'", "''")
                clss_approval.set_ta_documentoFIELDS("codigo_AID", Me.txt_codigoAID.Text, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("codigo_SAP_APP", codigo_sap.ToString, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("ficha_actividad", cod_actividad.ToString, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("monto_ficha", montoProyecto, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("regional", region, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("codigo_Approval", CrearCodigo(), "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("id_tipoAprobacion", Me.cmb_tipoDocumento.SelectedValue, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("monto_total", montoTotal, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("tasa_cambio", TasaCambio, "id_documento", 0)
                clss_approval.set_ta_documentoFIELDS("datecreated", Date.UtcNow, "id_documento", 0)


                id_documento = clss_approval.save_ta_documento()

                If id_documento <> -1 Then

                    Me.HiddenField1.Value = id_documento

                    ' Check_NewFile_FileUploaded() 'Uploading New Files here
                    'Dim dm As New SqlDataAdapter(sql, cnnSAP)
                    'Dim ds As New DataSet("extension")
                    'dm.Fill(ds, "extension")
                    'Dim id_documento = ds.Tables("extension").Rows(0).Item(0)


                    'sql = "SELECT id_ruta FROM vw_ta_rutas_tipoDocumento WHERE (id_tipoDocumento = " & Me.cmb_app.SelectedValue & ") AND  (orden = 0)"
                    'ds.Tables.Add("RUTA")
                    'dm.SelectCommand.CommandText = sql
                    'dm.SelectCommand.ExecuteNonQuery()
                    'dm.Fill(ds, "RUTA")
                    'Dim idRuta = ds.Tables("RUTA").Rows(0).Item(0)

                    Dim tbl_Route_By_DOC As New DataTable
                    tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(Me.cmb_app.SelectedValue, 0) 'First Step


                    Dim idRuta = tbl_Route_By_DOC.Rows.Item(0).Item("id_ruta")
                    Dim Duracion As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("duracion")
                    '*Actualizar correlativos

                    'Dim dmm As New SqlCommand("UPDATE ta_categoria SET correlativos = (SELECT correlativos FROM ta_categoria WHERE id_categoria=" & Me.cmb_cat.SelectedValue.Trim & ") + 1 WHERE id_categoria=" & Me.cmb_cat.SelectedValue.Trim, cnnSAP)
                    'dmm.ExecuteNonQuery()

                    clss_approval.Approval_CategoryCode_UPD(Me.cmb_cat.SelectedValue.Trim)

                    '*****************************CREAMOS EL PRIMER REGISTRO DEL HISTORIAL PARA RUTA = 0
                    'Dim fecha_recep As DateTime = DateAdd(DateInterval.Hour, 8, Date.Today)

                    'Addin the hours number lapsed required for each step
                    Dim fecha_limit As DateTime = DateAdd(DateInterval.Day, Duracion, Date.UtcNow) 'UTC DATE
                    Dim fecha_Recep As DateTime = Date.UtcNow 'UTC DATE

                    'sql = "INSERT INTO ta_AppDocumento(id_documento, id_ruta, fecha_limite, fecha_recepcion, id_estadoDoc,id_usuario_app) "
                    'sql &= "VALUES (" & id_documento & "," & idRuta & ",'" & fecha_recep.ToString.Replace(" a.m.", "") & "',getdate(),2," & Me.Session("E_IdUser") & ")"
                    'sql &= " SELECT @@IDENTITY"
                    'Dim dm1 As New SqlDataAdapter(sql, cnnSAP)
                    'Dim ds1 As New DataSet("AppDoc")
                    'dm1.Fill(ds1, "AppDoc")

                    clss_approval.set_ta_AppDocumento(0) 'New Record
                    clss_approval.set_ta_AppDocumentoFIELDS("id_documento", id_documento, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_Recep, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_Recep, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cOPEN, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("observacion", Me.txt_coments.Text.Trim, "id_app_documento", 0) '.Replace("'", "''")
                    clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", Me.cmb_rol.SelectedValue, "id_app_documento", 0)
                    clss_approval.set_ta_AppDocumentoFIELDS("datecreated", Date.UtcNow, "id_app_documento", 0)

                    'Dim id_appdocumento = ds1.Tables("AppDoc").Rows(0).Item(0)
                    id_appdocumento = clss_approval.save_ta_AppDocumento()

                    If id_appdocumento <> -1 Then

                        Check_NewFile_FileUploaded()

                        Save_NewFiles(CType(id_appdocumento, Integer), "--")

                        '****************LOKSSS*************************************************
                        '****************LOKSSS*************************************************
                        '****************LOKSSS*************************************************
                        SaveComment(id_appdocumento, cAPPROVED, Me.txt_coments.Text.Trim) '.Replace("'", "''")
                        '****************LOKSSS************************************************
                        '****************LOKSSS************************************************
                        '****************LOKSSS************************************************

                        '********************************************************************************************************************
                        '*****************************GUARDAMOS LOS ARCHIVOS DE TEMP A LA CARPETA DESTINA Y GUARDAMOS EN LA BD **************
                        'sql = " INSERT INTO ta_archivos_documento SELECT " & id_appdocumento & "  as id_App_Documento, archivo, id_doc_soporte FROM ta_archivos_documento_temp WHERE id_sesion_temp='" & Me.lbl_id_sesion_temp.Text & "'"
                        'dm.SelectCommand.CommandText = sql
                        'dm.SelectCommand.ExecuteNonQuery()

                        'For Each row In Me.grd_archivos.Items
                        '    Dim file = row.Cells(4).Text
                        '    CopyFileParam(file)
                        'Next

                        Dim tbl_archivos_temp As New DataTable

                        tbl_archivos_temp = clss_approval.get_ta_archivos_documento_temp(Me.lbl_id_sesion_temp.Text)

                        Dim dmyhm As String
                        Dim extension As String
                        Dim fileNameWE
                        Dim strFile As String


                        For Each dRow In tbl_archivos_temp.Rows
                            If Not String.IsNullOrEmpty(dRow("archivo").ToString) Then

                                Dim idArch As Integer = 0

                                dmyhm = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
                                extension = System.IO.Path.GetExtension(dRow("archivo"))
                                fileNameWE = System.IO.Path.GetFileNameWithoutExtension(dRow("archivo"))
                                strFile = String.Format("doc{0}_0{1}_{2}_{3}{4}{5}", id_documento, Me.Session("E_IdUser"), dmyhm, fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-"), "_v1.1", extension)

                                clss_approval.set_ta_archivos_documento(0) 'New Record
                                clss_approval.set_ta_archivos_documentoFIELDS("id_App_Documento", id_appdocumento, "id_archivo", 0)
                                clss_approval.set_ta_archivos_documentoFIELDS("archivo", strFile, "id_archivo", 0)
                                clss_approval.set_ta_archivos_documentoFIELDS("id_doc_soporte", dRow("id_doc_soporte"), "id_archivo", 0)
                                clss_approval.set_ta_archivos_documentoFIELDS("ver", 1, "id_archivo", 0)
                                idArch = clss_approval.save_ta_archivos_documento()

                                If idArch = -1 Then 'Erro Happenned
                                    err = True
                                    Exit For
                                Else
                                    CopyFileParam(dRow("archivo"), strFile)
                                End If


                            End If
                        Next


                        If Not err Then


                            '*****************************CREAMOS EL SEGUNDO REGISTRO DEL HISTORIAL PARA RUTA = 1
                            'sql = "SELECT id_ruta, duracion FROM vw_ta_rutas_tipoDocumento WHERE (id_tipoDocumento = " & Me.cmb_app.SelectedValue & ") AND  (orden = 1)"
                            'ds.Tables.Add("RUTA2")
                            'dm.SelectCommand.CommandText = sql
                            'dm.SelectCommand.ExecuteNonQuery()
                            'dm.Fill(ds, "RUTA2")


                            'idRuta = ds.Tables("RUTA2").Rows(0).Item("id_ruta")
                            tbl_Route_By_DOC = clss_approval.get_Route_By_DocumentType(Me.cmb_app.SelectedValue, 1) 'Next Step
                            idRuta = tbl_Route_By_DOC.Rows.Item(0).Item("id_ruta")
                            Duracion = tbl_Route_By_DOC.Rows.Item(0).Item("duracion")
                            Dim NextUser As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("id_usuario")
                            Dim idNextRol As Integer = tbl_Route_By_DOC.Rows.Item(0).Item("id_usuario")
                            'Dim duracion = ds.Tables("RUTA2").Rows(0).Item("duracion")

                            fecha_Recep = Date.UtcNow 'UTC DATE
                            'fecha_limit =    DateAdd(DateInterval.Hour, Duracion, Date.UtcNow) 'UTC DATE
                            fecha_limit = calculaDiaHabil(Duracion, fecha_Recep)

                            'sql = "INSERT INTO ta_AppDocumento(id_documento, id_ruta, fecha_limite, fecha_recepcion, fecha_aprobacion, id_estadoDoc,id_empleado_app) "
                            'sql &= "VALUES (" & id_documento & "," & idRuta & ",'" & calculaDiaHabil(duracion, fecha_Recep).ToString.Replace(" a.m.", "") & "',getdate(),NULL,1," & Me.Session("E_IdUser") & ")"
                            'sql &= " SELECT @@IDENTITY"
                            'Dim dm2 As New SqlDataAdapter(sql, cnnSAP)
                            'Dim ds2 As New DataSet("AppDoc2")
                            'dm2.Fill(ds2, "AppDoc2")
                            'Dim id_appdocumento2 As String = ds2.Tables("AppDoc2").Rows(0).Item(0)
                            '************************************************************************************
                            '*****************************GUARDAMOS LOS ARCHIVOS DE TEMP A LA CARPETA DESTINA Y GUARDAMOS EN LA BD 

                            clss_approval.set_ta_AppDocumento(0) 'New Record
                            clss_approval.set_ta_AppDocumentoFIELDS("id_documento", id_documento, "id_app_documento", 0)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_Recep, "id_app_documento", 0)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cPENDING, "id_app_documento", 0) 'Pending Step
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", NextUser, "id_app_documento", 0)
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", Me.txt_coments.Text.Trim, "id_app_documento", 0) 'Pending Step '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) 'Add the next Role --NEW
                            clss_approval.set_ta_AppDocumentoFIELDS("datecreated", Date.UtcNow, "id_app_documento", 0)


                            'Dim id_appdocumento = ds1.Tables("AppDoc").Rows(0).Item(0)
                            Dim id_appdocumento2 = clss_approval.save_ta_AppDocumento()

                            If id_appdocumento2 <> -1 Then

                                ''*****************CHANGE WE ARE NOT SAVE The same files to the other Steps***************************
                                'sql = " INSERT INTO ta_archivos_documento SELECT " & id_appdocumento2 & "  as id_App_Documento, archivo, id_doc_soporte FROM ta_archivos_documento_temp WHERE id_sesion_temp='" & Me.lbl_id_sesion_temp.Text & "'"
                                'dm.SelectCommand.CommandText = sql
                                'dm.SelectCommand.ExecuteNonQuery()
                                ''*****************CHANGE WE ARE NOT SAVE The same files to the other Steps***************************

                            Else
                                err = True
                            End If  'app_documento 2


                        Else
                            err = True
                        End If 'Archivos_documento 

                    Else
                        err = True
                    End If 'app_documento 1



                Else
                    err = True
                End If 'documento



                If Not err Then

                    Try
                        '*********************************OPEN****************************************
                        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 5, cl_user.regionalizacionCulture, id_appdocumento)
                        If (objEmail.Emailing_APPROVAL_STEP(id_appdocumento)) Then
                        Else 'Error mandando Email
                        End If
                        '*********************************OPEN****************************************


                    Catch ex As Exception

                        lblerr_user.Text = String.Format("An error was found sending the email: {0} ", ex.Message)

                    End Try

                    Me.Response.Redirect("~/approvals/frm_consulta_docsPending.aspx")

                Else

                    lblerr_user.Text = String.Format("An error ocurred during saving the the approval ")

                End If

                'cnnSAP.Close()

            Catch ex As Exception
                btn_Open.Enabled = False
                lblerr_user.Text = String.Format("An error was found in the action: {0} ", ex.Message)
            End Try


        Else

            Me.lblerrRegion.Visible = True
            Me.lblerrMonto.Visible = True

        End If
    End Sub

    Private Sub grd_documentos_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_documentos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim visible As New CheckBox
            Dim hlk_ref As New HyperLink

            visible = itemD("colm_select").FindControl("chkSelect")
            visible.Checked = False
            visible.InputAttributes.Add("value", itemD("id_doc_soporte").Text)

            Dim str As String = visible.InputAttributes.Item("value")

            visible.ToolTip = "Select a document"

            hlk_ref = itemD("colm_template").FindControl("hlk_template")

            If Not itemD("Template").Text.Contains("--none--") Then
                hlk_ref.Text = itemD("Template").Text
                hlk_ref.NavigateUrl = "~/FileUploads/Templates/" & itemD("Template").Text
            Else
                hlk_ref.Text = itemD("Template").Text
                hlk_ref.NavigateUrl = "#"
            End If

        End If

    End Sub

    'Protected Sub chkVisible_CheckedChangedDOCS(ByVal sender As Object, ByVal e As System.EventArgs)

    '    Dim chkSelect As CheckBox = CType(sender, CheckBox)
    '    hd_id_doc.Value = Convert.ToInt32(chkSelect.InputAttributes.Item("value"))

    '    ActualizaDatosDOCS()

    'End Sub


    'Sub ActualizaDatosDOCS()

    '    For Each Irow As GridDataItem In Me.grd_documentos.Items

    '        Dim chkvisible As CheckBox = CType(Irow("colm_select").FindControl("chkSelect"), CheckBox)

    '        If chkvisible.Checked = True Then

    '            If Irow("id_doc_soporte").Text = hd_id_doc.Value Then
    '                ' Me.Uploader3.ValidateOption.AllowedFileExtensions = clss_approval.get_DocumentSupport_Extension(hd_id_doc.Value).Rows().Item(0).Item("extension")
    '                Me.Uploader3.ValidateOption.AllowedFileExtensions = Irow("extension").Text
    '            Else
    '                chkvisible.Checked = False
    '            End If

    '            'Dim Sql = " INSERT INTO ta_aprobacion_docs_temp (id_sesion_temp,  id_doc_soporte, id_programa) VALUES ('" & Me.lbl_id_sesion_temp.Text & "'," & Irow("id_doc_soporte").Text & ", " & Me.Session("E_IDPrograma") & ") "
    '            'cnnSAP.Open()
    '            'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
    '            'Dim ds As New DataSet("IdPlan")
    '            'dm.Fill(ds, "IdPlan")
    '            'cnnSAP.Close()

    '        End If
    '    Next

    '    'grd_documentos.DataSource = cl_AppDef.get_DocumentTypesFROM_tmp(Me.lbl_id_sesion_temp.Text)
    '    'grd_documentos.DataBind()
    '    'Me.grd_cate.DataBind()

    'End Sub
    '*********************************************+********************************************************************+**********************************
    '*********************************************+********************************************************************+**********************************
    '*********************************************+********************************************************************+**********************************

    Protected Sub chkVisible_CheckedChangedDOCS(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkSelect As CheckBox = CType(sender, CheckBox)
        hd_id_doc.Value = Convert.ToInt32(chkSelect.InputAttributes.Item("value"))

        ActualizaDatosDOCS()
        'Me.msg_document_type.Visible = False

        '  Me.dv_DOC_TYPE.Visible = False
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "div_Control(false)", True)

    End Sub

    Sub ActualizaDatosDOCS()

        For Each Irow As GridDataItem In Me.grd_documentos.Items

            Dim chkvisible As CheckBox = CType(Irow("colm_select").FindControl("chkSelect"), CheckBox)

            If chkvisible.Checked = True Then

                If Irow("id_doc_soporte").Text = hd_id_doc.Value Then

                    RadSync_NewFile.Enabled = True
                    'Me.Uploader3.ValidateOption.AllowedFileExtensions = Irow("extension").Text
                    RadSync_NewFile.AllowedFileExtensions = Irow("extension").Text.Trim.Replace(" ", "").Split(",")
                    'RadSync_NewFile.AllowedFileExtensions = Strings.Split("xls,doc,pdf,xlsx,docx", ",")
                    'RadSync_NewFile.MaxFileSize = (1024 * 1000) ' 1MG
                    RadSync_NewFile.MaxFileSize = (Convert.ToDecimal(Irow("max_size").Text) * 1024 * 1000)

                Else
                    chkvisible.Checked = False
                End If

            End If
        Next


    End Sub


End Class
