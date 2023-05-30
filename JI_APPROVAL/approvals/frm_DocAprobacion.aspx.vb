Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports CuteWebUI
Imports System.Net.Mail
Imports System.Net
Imports Subgurim.Controles
Imports System.Drawing
Imports System.Web.Script.Serialization
Imports ly_APPROVAL
Imports ly_SIME


Partial Class frm_DocAprobacion

    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    'Dim cnnContratos As New SqlConnection(ConnectionStrings("Celins_TestConnectionString").ConnectionString)
    'Dim bndUploadNW As Boolean
    'dbCelins_ConnectionString
    'Dim cnnContratos As New SqlConnection(ConnectionStrings("dbCelins_ConnectionString").ConnectionString)

    Dim frmCODE As String = "AP_APPROVAL_CON"
    Dim controles As New ly_SIME.CORE.cls_controles
    Public urlDir As String = ""
    Public idUrl As Integer = 0

    Const cPENDING = 1
    Const cAPPROVED = 2
    Const cnotAPPROVED = 3
    Const cCANCELLED = 4
    Const cOPEN = 5
    Const cSTANDby = 6
    Const cCOMPLETED = 7

    Const cAction_ByProcess = 1
    Const cAction_ByMessage = 2

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim clss_approval As APPROVAL.clss_approval


    '**************************FUNCIONES **************************************************



    Sub enviar_emailAprobacion(ByVal id_doc As Integer, ByVal id_appdoc As Integer, ByVal idEstado As Integer)
        '**************************DECLARACION DE VARIABLES***************************
        Dim sql = "SELECT * FROM t_config_email "
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
        '**********************HUBO UN CAMBIO ACÁ, SE INCLUYÓ A TODOS LOS DE LA RUTA********************************
        '******************INCLUYENDO LOS GRUPOS********************************
        sql = "  SELECT email, nombre_rol, id_rol FROM vw_ta_roles_emails "
        sql &= " WHERE (id_documento = " & id_doc & ") " '--AND (orden = 0 OR orden =(" & Orden & " - 1) OR  orden = " & Orden & ")"
        sql &= " UNION "
        sql &= " SELECT email, nombre_rol, id_rol FROM vw_ta_email_gruposRoles WHERE id_rol IN("
        sql &= " SELECT  id_rol FROM vw_ta_roles_emails AS A "
        sql &= " WHERE (id_documento = " & id_doc & ")) " '--AND (orden = 0 OR orden =(" & Orden & " - 1) OR  orden = " & Orden & "))"
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


        '***********************************CONTENIDO DEL EMAIL*********************************

        sql = "SELECT * FROM vw_ta_AppDocumento WHERE id_App_Documento=" & id_appdoc
        Dim dmap As New SqlDataAdapter(sql, cnnSAP)
        Dim dsap As New DataSet("appdoc")
        dmap.Fill(dsap, "appdoc")

        MensajeSend.Subject = String.Format("Approval process {0} has been completed", dsap.Tables("appdoc").Rows(0).Item("numero_instrumento"))

        ''*****************************************FIN******************************************

        '*****************************TABLA DE ESTADOS ******************************** 
        Dim dsP As New DataSet("dtPROCS")
        dsP.Tables.Add("dtPROCS")
        sql = "SELECT * FROM dbo.FN_Ta_RutaSeguimiento(" & id_doc & ") ORDER BY ORDEN"
        dm.SelectCommand.CommandText = sql
        dm.Fill(dsP, "dtPROCS")
        Dim strTable = makeTable(dsP.Tables("dtPROCS"), dsap.Tables("appdoc").Rows(0).Item("descripcion_estado"), id_appdoc)
        '*****************************TABLA DE ESTADOS ******************************** 

        Dim nullable As System.Text.Encoding
        Dim Mensaje = Make_Email2(dsap.Tables("appdoc"), url_aplicacion, strTable)
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


    End Sub  '**********************FUNCION PARA ENVIAR EMAILS**********

    Sub enviar_email(ByVal id_doc As Integer, ByVal id_appdoc As Integer, ByVal idEstado As Integer)

        '**************************DECLARACION DE VARIABLES***************************
        Dim sql = "SELECT * FROM t_config_email WHERE id_proyecto=" & Me.Session("E_IdProy")
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
        MensajeSend.Subject = "Document approval for next phase"

        Dim emails_cco() As String = ds.Tables("email").Rows(0).Item("BCO").ToString.Split(";")
        For i = 0 To emails_cco.Count() - 1
            If emails_cco(i).ToString <> "" Then
                MensajeSend.Bcc.Add(emails_cco(i).ToString)
            End If
        Next

        '**************************FIN********************************************************

        Select Case idEstado
            Case 2 ' APROBADO PASA A SIGUIENTE FASE

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
                MensajeSend.Subject = "Document approval for next phase"
            Case 3 'NOT APROBADO
                '********************AGREGANDO LOS DESTINATARIOS***************************************
                sql = "SELECT orden FROM  vw_ta_AppDocumento WHERE id_App_Documento=" & id_appdoc
                ds.Tables.Add("Orden")
                dm.SelectCommand.CommandText = sql
                dm.Fill(ds, "Orden")
                Dim Orden = ds.Tables("Orden").Rows(0).Item("orden")

                sql = "  SELECT email, nombre_rol, id_rol FROM vw_ta_roles_emails "
                sql &= " WHERE (id_documento = " & id_doc & ") AND (orden = 0 OR orden =(" & Orden & " - 1) OR  orden = " & Orden & ")"
                sql &= " UNION "
                sql &= " SELECT email, nombre_rol, id_rol FROM vw_ta_email_gruposRoles WHERE id_rol IN("
                sql &= " SELECT  id_rol FROM vw_ta_roles_emails AS A "
                sql &= " WHERE (id_documento = " & id_doc & ") AND (orden = 0 OR orden =(" & Orden & " - 1) OR  orden = " & Orden & "))"

                ds.Tables.Add("emailSend")
                dm.SelectCommand.CommandText = sql
                dm.Fill(ds, "emailSend")

                '***********************NOTIFICANDO A TODOS LOS QUE HASTA EL MOMENTO HAN PARTICIPADO.**
                For i = 0 To ds.Tables("emailSend").Rows.Count() - 1
                    MensajeSend.To.Add(ds.Tables("emailSend").Rows(i).Item("email"))
                Next

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
                MensajeSend.Subject = "Document classified as NOT approved"

                '**************************FIN**************************************** 
            Case 4 'CANCELADO
                '********************AGREGANDO LOS DESTINATARIOS***************************************
                sql = "SELECT orden FROM  vw_ta_AppDocumento WHERE id_App_Documento=" & id_appdoc
                ds.Tables.Add("Orden")
                dm.SelectCommand.CommandText = sql
                dm.Fill(ds, "Orden")
                Dim Orden = ds.Tables("Orden").Rows(0).Item("orden")

                sql = "  SELECT email, nombre_rol, id_rol FROM vw_ta_roles_emails "
                sql &= " WHERE (id_documento = " & id_doc & ") AND (orden = 0 OR orden =(" & Orden & " - 1) OR  orden = " & Orden & ")"
                sql &= " UNION "
                sql &= " SELECT email, nombre_rol, id_rol FROM vw_ta_email_gruposRoles WHERE id_rol IN("
                sql &= " SELECT  id_rol FROM vw_ta_roles_emails AS A "
                sql &= " WHERE (id_documento = " & id_doc & ") AND (orden = 0 OR orden =(" & Orden & " - 1) OR  orden = " & Orden & "))"

                ds.Tables.Add("emailSend")
                dm.SelectCommand.CommandText = sql
                dm.Fill(ds, "emailSend")

                '***********************NOTIFICANDO A TODOS LOS QUE HASTA EL MOMENTO HAN PARTICIPADO.**

                For i = 1 To ds.Tables("emailSend").Rows.Count() - 1
                    MensajeSend.To.Add(ds.Tables("emailSend").Rows(i).Item("email"))
                Next

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
                MensajeSend.Subject = "Document classified as Canceled"
                '**************************FIN**************************************** 
            Case 5 'ON FILE
                '*******************NO DEFINIDO********************************
                MensajeSend.Subject = "Document classified as On File"
            Case 6 'STAND BY
                '********************AGREGANDO LOS DESTINATARIOS***************************************
                sql = "SELECT orden FROM  vw_ta_AppDocumento WHERE id_App_Documento=" & id_appdoc
                ds.Tables.Add("Orden")
                dm.SelectCommand.CommandText = sql
                dm.Fill(ds, "Orden")
                Dim Orden = ds.Tables("Orden").Rows(0).Item("orden")

                '***********************NOTIFICANDO AL ORIGINADOR, AL QUE GENERA EL DOCUMENTO, AL ANTERIOR Y AL SIGUIENTE.**
                '******************INCLUYENDO LOS GRUPOS********************************
                sql = "  SELECT email, nombre_rol, id_rol FROM vw_ta_roles_emails "
                sql &= " WHERE (id_documento = " & id_doc & ") AND (orden = 0 OR orden =(" & Orden & " - 1) OR  orden = " & Orden & ")"
                sql &= " UNION "
                sql &= " SELECT email, nombre_rol, id_rol FROM vw_ta_email_gruposRoles WHERE id_rol IN("
                sql &= " SELECT  id_rol FROM vw_ta_roles_emails AS A "
                sql &= " WHERE (id_documento = " & id_doc & ") AND (orden = 0 OR orden =(" & Orden & " - 1) OR  orden = " & Orden & "))"

                Dim dm1 As New SqlDataAdapter(sql, cnnSAP)
                Dim ds1 As New DataSet("instrumento")
                dm1.Fill(ds1, "instrumento")

                '********************AGREGANDO LOS DESTINATARIOS************************
                For i = 0 To ds1.Tables("instrumento").Rows.Count() - 1
                    MensajeSend.To.Add(ds1.Tables("instrumento").Rows(i).Item("email"))
                Next
                '**************************FIN**************************************** 
                MensajeSend.Subject = "Document classified as Stand By"
        End Select

        '***********************************CONTENIDO DEL EMAIL*********************************
        sql = "SELECT * FROM vw_ta_AppDocumento WHERE id_App_Documento=" & id_appdoc
        Dim dmap As New SqlDataAdapter(sql, cnnSAP)
        Dim dsap As New DataSet("appdoc")
        dmap.Fill(dsap, "appdoc")

        'Dim Mensaje As String = "<html><body>"
        'Mensaje &= "<style type='text/css'><!--.Estilo1 {color: #0000CC}.Estilo2 {color: #0000FF}--></style>"
        'Mensaje &= "<p><font face='Arial' style='font-size: 13px'>Mail sent from the Project Management System CHEMONICS<br>"

        'Mensaje &= "<table border='1' cellpadding='0' cellspacing='0' bordercolor='#000033'>"

        'Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #FFFFFF' >"
        'Mensaje &= "<div align='righ'><b>Project: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF' >" & dsap.Tables("appdoc").Rows(0).Item("nombre_proyecto") & "</th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #000000' >"
        'Mensaje &= "<div align='righ'><b>Name of Category: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000' >" & dsap.Tables("appdoc").Rows(0).Item("descripcion_cat") & "</th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #FFFFFF' >"
        'Mensaje &= "<div align='righ'><b>Approval: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF' >" & dsap.Tables("appdoc").Rows(0).Item("descripcion_aprobacion") & "</th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #000000' >"
        'Mensaje &= "<div align='righ'><b>Instrument Number: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000' >" & dsap.Tables("appdoc").Rows(0).Item("numero_instrumento") & "</th></tr>"

        'Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #FFFFFF'>"
        'Mensaje &= "<div align='righ'><b>Beneficiary Name: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF'  >" & dsap.Tables("appdoc").Rows(0).Item("nom_beneficiario") & "</th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #000000' >"
        'Mensaje &= "<div align='righ'><b>Name of process: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000' >" & dsap.Tables("appdoc").Rows(0).Item("descripcion_doc") & "</th></tr>"

        'Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #FFFFFF'>"
        'Mensaje &= "<div align='righ'><b>Process Code: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF'  >" & dsap.Tables("appdoc").Rows(0).Item("codigo_AID") & "</th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #000000' >"
        'Mensaje &= "<div align='righ'><b>Code SAP/APPROVAL: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000' >" & dsap.Tables("appdoc").Rows(0).Item("codigo_SAP_APP") & "</th></tr>"

        'Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #FFFFFF' >"
        'Mensaje &= "<div align='righ'><b>Report create by: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF'  >" & dsap.Tables("appdoc").Rows(0).Item("nombre_empleado") & "</th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #000000' >"
        'Mensaje &= "<div align='righ'><b>Current phase of path: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000' >" & dsap.Tables("appdoc").Rows(0).Item("nombre_rol") & "</th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #FFFFFF' >"
        'Mensaje &= "<div align='righ'><b>Next phase of path: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF' >" & dsap.Tables("appdoc").Rows(0).Item("nextFase") & "</th></tr>"

        'Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #000000' >"
        'Mensaje &= "<div align='righ'><b>Status of process: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000'  >" & dsap.Tables("appdoc").Rows(0).Item("descripcion_estado") & "</th></tr>"

        'Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #FFFFFF' >"
        'Mensaje &= "<div align='righ'><b>Date of receipt  : </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF'  >" & dsap.Tables("appdoc").Rows(0).Item("fecha_recepcion") & "</th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #000000' >"
        'Mensaje &= "<div align='righ'><b>DeadLine: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000'>" & dsap.Tables("appdoc").Rows(0).Item("fecha_limite") & "</th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #FFFFFF' >"
        'Mensaje &= "<div align='righ'><b>Link: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF' ><a href='" & url_aplicacion & "/approvals/frm_DocAprobacion.aspx?IdDoc=" & dsap.Tables("appdoc").Rows(0).Item("id_documento") & "'> View link <a/></th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #000000'>"
        'Mensaje &= "<div align='righ'><b>Comments</b></div></th><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000'>" & Me.txtcoments.Text & "</th></tr></table>"

        'Mensaje &= " <p><strong>This is an automatically generated email, please do not reply</strong><br />"
        'Mensaje &= " <strong>WARNING</strong>: <em>This message contains  information legally protected. If you have received this message by  mistake, please avoid checking, distributing, copying, reproduction or misuse  of the information within it and reply to the sender informing about the  misunderstanding.</em></p>"

        'Mensaje &= "</body></html>"


        '*****************************************FIN******************************************

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


    End Sub  '**********************FUNCION PARA ENVIAR EMAILS**********


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
                '     If idAppDoc = dtR.Item("id_App_Documento") Then

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
                                dtData.Rows(0).Item("Observacion") &
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


    Public Function Make_Email2(ByVal dtData As DataTable, ByVal url_aplicacion As String, ByVal strTABLE As String) As String

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
                               "  <tr>" &
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'   >" &
                               "         PHASE OF PATH:</td>" &
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
                                dtData.Rows(0).Item("Observacion") &
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
    End Function '*****CALCULA DIAS HABILES**************


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

    'Sub DelFile()
    '    Dim sFileName As String = System.IO.Path.GetFileName(Me.lblarchivo.Text)
    '    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Temp\"
    '    Dim file_info As New IO.FileInfo(sFileDir + sFileName)
    '    If (file_info.Exists) Then
    '        file_info.Delete()
    '    End If
    '    ' Me.lblarchivo.Text = ""
    '    'Me.Panel1_firma.Visible = False
    'End Sub


    '******************************REV 0.0.0.2********************
    'Function DelFile(ByVal sFileName As String) As Boolean
    '    'Dim sFileName As String = System.IO.Path.GetFileName(Me.lblarchivo.Text)
    '    Dim sFileDir As String = Server.MapPath("~") & "\fileUploads\ApprovalProcc\"
    '    Dim file_info As New IO.FileInfo(sFileDir + sFileName)
    '    If (file_info.Exists) Then
    '        file_info.Delete()
    '        DelFile = True
    '    Else
    '        DelFile = False
    '    End If
    'End Function
    '******************************REV 0.0.0.2********************


    '******************************REV 0.0.0.1********************
    'Public Sub DelList()
    '    Dim li As ListItem
    '    Dim x As Integer = 0
    '    For Each li In ListBox_file.Items
    '        If Trim(li.Text) = Trim(lblarchivo.Text) Then
    '            Me.ListBox_file.Items.Remove(li)
    '            Exit For
    '        End If
    '    Next

    'End Sub
    '******************************REV 0.0.0.1********************
    Function GeTfIleName(ByVal fileName As String) As String

        Dim Random As New Random()
        Dim Aleatorio As Double = Random.Next(1, 999)
        Dim extension As String = System.IO.Path.GetExtension(fileName)
        Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(fileName)
        Dim File As String = "doc_" & Me.HiddenField1.Value & Me.Session("E_IdUser") & Aleatorio & fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension

        GeTfIleName = File

    End Function

    '******************************REV 0.0.0.2********************

    'Sub CopyFile()
    '    Dim sFileName As String = System.IO.Path.GetFileName(Me.lblarchivo.Text)
    '    Dim sFileDirTemp As String = Server.MapPath("~") & "\FileUploads\Temp\"
    '    Dim sFileDir As String = Server.MapPath("~") & "\fileUploads\ApprovalProcc\"
    '    Dim file_info As New IO.FileInfo(sFileDirTemp + sFileName)
    '    Dim extension As String = System.IO.Path.GetExtension(Me.lblarchivo.Text)
    '    Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(Me.lblarchivo.Text)
    '    Try
    '        file_info.CopyTo(sFileDir & fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension)
    '    Catch ex As Exception
    '    End Try
    '    DelFile()
    '    Me.Panel1_firma.Visible = False
    'End Sub


    Sub CopyFileParam(ByVal file As String)

        Dim sFileName As String = System.IO.Path.GetFileName(file)
        Dim sFileDirTemp As String = Server.MapPath("~") & "\FileUploads\Temp\"
        Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\ApprovalProcc\"
        Dim file_info As New IO.FileInfo(sFileDirTemp + sFileName)
        Dim extension As String = System.IO.Path.GetExtension(file)
        'Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(file)
        Try

            file_info.CopyTo(sFileDir & sFileName) 'fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension

        Catch ex As Exception
        End Try
        DelFileParam(file)
        'Me.Panel1_firma.Visible = False
    End Sub
    '*****************************FIN DEL BLOQUE DE FUNCIONES *********************************
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
                cl_user.chk_Rights(Page.Controls, 5, frmCODE, 0)
                'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        Dim approve As Boolean = True
        Dim fireExange_Rate As Boolean = False


        If Not IsPostBack Then

            Me.RadSync_NewFile.Enabled = True

            'RadSync_NewFile.AllowedFileExtensions = Strings.Split("xls,doc,pdf,xlsx,docx", ",")
            'RadSync_NewFile.MaxFileSize = (1024 * 1000) ' 1MG

            ' bndUploadNW = False

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
            Me.HiddenField1.Value = Me.Request.QueryString("IdDoc").ToString

            clss_approval.get_ta_DocumentosINFO(Me.HiddenField1.Value)

            '****************************************************************************************************************************

            Dim USerAllowed As String() = clss_approval.get_ta_DocumentosInfoFIELDS("IdUserParticipate", "id_documento", Me.HiddenField1.Value).ToString.Split(",")
            Dim indx As Integer = USerAllowed.ToList().IndexOf(Me.Session("E_IDUser"))
            Dim boolAcces As Boolean = Convert.ToBoolean(Val(Me.h_Filter.Value))

            If indx = -1 Then '--The User is not Allowed
                If Not boolAcces Then
                    Me.Response.Redirect("~/Proyectos/no_access2_app")
                End If
            End If

            Dim Tool_code As String = clss_approval.get_ta_DocumentosInfoFIELDS("tool_code", "id_documento", Me.HiddenField1.Value)
            Dim Tool_name As String = clss_approval.get_ta_DocumentosInfoFIELDS("approval_tool_name", "id_documento", Me.HiddenField1.Value)


            '***********************************TO REDIRECT TO THE MESSAGE REPORT*****************************************
            If (Not clss_approval.IS_User_StepMAX_Permitted(Me.HiddenField1.Value, Me.Session("E_IdUser"))) Then
                Me.Response.Redirect("~/approvals/frm_seguimientoAprobacionMessRep.aspx?IdDoc=" & Me.HiddenField1.Value & "&&IdRuta=" & clss_approval.get_ta_DocumentosInfoFIELDS("id_ruta", "id_documento", Me.HiddenField1.Value))
            Else

                If Tool_code = "DELIV-RMS01" And (clss_approval.get_ta_DocumentosInfoFIELDS("IdOriginador", "id_documento", Me.HiddenField1.Value) = Val(Me.Session("E_IdUser"))) Then 'It's on me and depend on the Deliv tools

                    Dim idDocument As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                    Dim cls_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
                    Dim tbl_result As DataTable = cls_Deliverable.Deliv_Document(0, idDocument)

                    If tbl_result.Rows.Count > 0 Then
                        Me.Response.Redirect("~/Deliverable/frm_DeliverableAdd.aspx?ID=" & tbl_result.Rows.Item(0).Item("id_deliverable"))
                    Else
                        Me.Response.Redirect("~/approvals/frm_seguimientoAprobacionMessRep.aspx?IdDoc=" & Me.HiddenField1.Value & "&&IdRuta=" & clss_approval.get_ta_DocumentosInfoFIELDS("id_ruta", "id_documento", Me.HiddenField1.Value))
                    End If


                ElseIf Tool_code = "TM-SHEET01" And (clss_approval.get_ta_DocumentosInfoFIELDS("IdOriginador", "id_documento", Me.HiddenField1.Value) = Val(Me.Session("E_IdUser"))) Then 'It's on me and depend on the Deliv tools Then

                    Using db As New dbRMS_JIEntities

                        Dim idDocument As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                        If db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDocument).Count > 0 Then
                            Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDocument).FirstOrDefault().id_timesheet
                            Me.Response.Redirect("~/TimeSheet/frm_TimeSheetAdd.aspx?ID=" & idTS.ToString)
                        Else
                            Me.Response.Redirect("~/approvals/frm_seguimientoAprobacionMessRep.aspx?IdDoc=" & Me.HiddenField1.Value & "&&IdRuta=" & clss_approval.get_ta_DocumentosInfoFIELDS("id_ruta", "id_documento", Me.HiddenField1.Value))
                        End If

                    End Using

                End If
            End If
            '***********************************TO REDIRECT TO THE MESSAGE REPORT*****************************************

            Me.lbl_categoria.Text = clss_approval.get_ta_DocumentosInfoFIELDS("descripcion_cat", "id_documento", Me.HiddenField1.Value)
            Me.lbl_aprobacion.Text = clss_approval.get_ta_DocumentosInfoFIELDS("descripcion_aprobacion", "id_documento", Me.HiddenField1.Value)
            Me.lbl_nivelaprobacion.Text = clss_approval.get_ta_DocumentosInfoFIELDS("nivel_aprobacion", "id_documento", Me.HiddenField1.Value)
            Me.lbl_condicion.Text = clss_approval.get_ta_DocumentosInfoFIELDS("condicion", "id_documento", Me.HiddenField1.Value)
            Me.lbl_proceso.Text = clss_approval.get_ta_DocumentosInfoFIELDS("descripcion_doc", "id_documento", Me.HiddenField1.Value)
            Me.lbl_codigo.Text = clss_approval.get_ta_DocumentosInfoFIELDS("codigo_AID", "id_documento", Me.HiddenField1.Value)
            Me.lbl_instrumento.Text = clss_approval.get_ta_DocumentosInfoFIELDS("numero_instrumento", "id_documento", Me.HiddenField1.Value)
            Me.lbl_beneficiario.Text = clss_approval.get_ta_DocumentosInfoFIELDS("nom_beneficiario", "id_documento", Me.HiddenField1.Value)
            Me.lbl_Comment.Text = clss_approval.get_ta_DocumentosInfoFIELDS("comentarios", "id_documento", Me.HiddenField1.Value)
            Me.lbl_IdCodigoAPP.Text = clss_approval.get_ta_DocumentosInfoFIELDS("codigo_Approval", "id_documento", Me.HiddenField1.Value)
            Me.lbl_IdCodigoSAP.Text = clss_approval.get_ta_DocumentosInfoFIELDS("codigo_SAP_APP", "id_documento", Me.HiddenField1.Value)


            Dim id_Programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Dim tbl_AppOrden As New DataTable
            tbl_AppOrden = clss_approval.get_ta_AppDocumentoAPP_MAX(Me.HiddenField1.Value) 'To get the info on the max step (id_app_doc)
            Dim idEstadoDoc As Integer = tbl_AppOrden.Rows(0).Item("id_estadoDoc")
            Dim idEstadoTipo As Integer = tbl_AppOrden.Rows(0).Item("id_estadoTipo")
            Dim EstadoTipoPrefijo As String = tbl_AppOrden.Rows(0).Item("estado_tipo_prefijo")

            '********************************************NOT ALLOWED************************************************************
            ''if the MAX order is in stand by get the lowest order and the last one
            'If idEstadoDoc = cSTANDby Then '************This is not necesary because another step is created********************
            '    tbl_AppOrden = clss_approval.get_ta_AppDocumentoOrden_MIN(Me.HiddenField1.Value) 'To get the info on the min step
            'End If '************This is not necesary because another step is created******************************************
            '********************************************NOT ALLOWED***********************************************************


            Me.lblIDocumento.Text = tbl_AppOrden.Rows(0).Item("id_app_Documento")
            Me.lblTipoDoc.Text = tbl_AppOrden.Rows(0).Item("id_tipoDocumento")
            Me.hd_TipoDoc.Value = tbl_AppOrden.Rows(0).Item("id_tipoDocumento")
            Me.lbl_aprueba.Text = tbl_AppOrden.Rows(0).Item("nextFase").ToString
            Me.lbl_owner.Text = tbl_AppOrden.Rows(0).Item("nombre_rol").ToString & " [ " & tbl_AppOrden.Rows(0).Item("nombre_empleado").ToString & " ]"
            Me.lblnextruta.Text = tbl_AppOrden.Rows(0)("id_ruta_next").ToString
            Me.lblNextRole.Text = tbl_AppOrden.Rows(0)("id_role_next").ToString
            Me.lblNextUserID.Text = tbl_AppOrden.Rows(0)("id_usuario_next").ToString

            hd_ROL.Value = tbl_AppOrden.Rows(0).Item("nombre_rol").ToString
            Dim idROl = tbl_AppOrden.Rows(0).Item("id_rol")

            '***************************************************************************************************************************************************
            '*********************************************************TOOL VERIFYING****************************************************************************
            '***************************************************************************************************************************************************

            If Tool_code = "TM-SHEET01" Then '--Time Tools

                Me.ToolsViewer.Visible = True

                Using db As New dbRMS_JIEntities

                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                    Dim idTimeSheet As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet
                    Me.lbl_tool_viewer.Text = Tool_name
                    'Me.hrefVIEWER.Attributes.Add("href", "~/TimeSheet/frm_TimeSheetFollowingREP.aspx?ID=" & idTimeSheet)
                    Me.hrefVIEWER.Attributes.Add("href", "javascript:OpenRadWindowTool('../TimeSheet/frm_TimeSheetFollowingREP.aspx?ID=" & idTimeSheet & "');")
                    'urlDir = "~/TimeSheet/frm_TimeSheetFollowingREP.aspx?ID=" & idTimeSheet
                End Using

            ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                Me.ToolsViewer.Visible = True

                Using db As New dbRMS_JIEntities

                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                    Dim idDeliverable As Integer = db.ta_documento_deliverable.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_deliverable
                    Me.hd_id_delivered.Value = idDeliverable
                    Me.lbl_tool_viewer.Text = Tool_name
                    'Me.hrefVIEWER.Attributes.Add("href", "~/Deliverable/frm_DeliverableFollowingREP.aspx?ID=" & idDeliverable)
                    Me.hrefVIEWER.Attributes.Add("href", "javascript:OpenRadWindowTool('../Deliverable/frm_DeliverableFollowingREP.aspx?ID=" & idDeliverable & "');")


                    Dim cls_Deliverable = New APPROVAL.clss_Deliverable(Convert.ToInt32(Me.Session("E_IDPrograma")), cl_user)
                    Current_Deliverable.InnerHtml = cls_Deliverable.GetDeliverables(0, idDeliverable)

                    Dim oPeriodo = db.vw_t_periodos.Where(Function(p) p.activo = True And p.id_programa = id_Programa).ToList()
                    Dim fechaReg = DateTime.Now
                    Dim oTasaCambio As List(Of t_trimestre_tasa_cambio)
                    Dim strCurrent_Period As String
                    If oPeriodo.Count() = 0 Then
                        oTasaCambio = db.t_trimestre_tasa_cambio.Where(Function(p) p.mes = Month(fechaReg) And p.t_trimestre.anio = Year(fechaReg) And p.id_programa = id_Programa).ToList()
                        strCurrent_Period = "--"
                    Else
                        Dim idTrimestre As Integer = Convert.ToInt32(oPeriodo.FirstOrDefault.id_trimestre)
                        oTasaCambio = db.t_trimestre_tasa_cambio.Where(Function(p) p.id_trimestre = idTrimestre And p.id_programa = id_Programa).ToList()
                        strCurrent_Period = oPeriodo.FirstOrDefault.FiscalYearNotation
                    End If

                    'String.Format(cl_user.regionalizacionCulture, "{1} {0:N2}",dtRow("valor"),cl_user.regionalizacionCulture.NumberFormat.CurrencySymbol)

                    If oTasaCambio.Count() = 0 Then
                        Me.hd_tasa_cambio.Value = 0
                        fireExange_Rate = True
                    Else
                        Dim valTasaCambio As Double = Math.Round(oTasaCambio.FirstOrDefault.tasa_cambio, 2, MidpointRounding.AwayFromZero)
                        Me.hd_tasa_cambio.Value = valTasaCambio
                    End If

                    Me.lbl_current_exchange_rate.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", Convert.ToDecimal(Me.hd_tasa_cambio.Value))
                    Me.lbl_period.Text = String.Format("{0}", strCurrent_Period)
                    Me.hd_is_tool.Value = tbl_AppOrden.Rows(0)("trigger_tool").ToString

                End Using
            ElseIf Tool_code = "TRAVEL-RM01" Then '--Time Tools

                Me.ToolsViewer.Visible = True

                Using db As New dbRMS_JIEntities

                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                    Dim tipoAprobacion = 0
                    Dim identity = 0
                    Dim urlTool = ""
                    If db.ta_documento_viaje.Where(Function(p) p.id_documento = idDoc).FirstOrDefault() IsNot Nothing Then
                        '1 = Solicitud de viaje
                        tipoAprobacion = 1
                        Tool_name = "Solicitud de " + Tool_name
                        identity = db.ta_documento_viaje.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_viaje
                        urlTool = "../administrativo/frm_viajePrint?Id=" & identity
                    ElseIf db.ta_documento_legalizacion_viaje.Where(Function(p) p.id_documento = idDoc).FirstOrDefault() IsNot Nothing Then
                        '2 = legalización de viaje
                        tipoAprobacion = 2
                        identity = db.ta_documento_legalizacion_viaje.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_viaje
                        Tool_name = "Legalización de " + Tool_name
                        urlTool = "../administrativo/frm_viaje_legalizacionPrint?Id=" & identity
                    ElseIf db.ta_documento_viaje_informe.Where(Function(p) p.id_documento = idDoc).FirstOrDefault() IsNot Nothing Then
                        '3 = informe de viaje
                        tipoAprobacion = 2
                        identity = db.ta_documento_viaje_informe.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_viaje
                        Tool_name = "Informe de " + Tool_name
                        urlTool = "../administrativo/frm_viaje_informePrint?Id=" & identity
                    End If
                    'Dim idTimeSheet As Integer = db.ta_documento_viaje.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet
                    Me.lbl_tool_viewer.Text = Tool_name
                    'Me.hrefVIEWER.Attributes.Add("href", "~/TimeSheet/frm_TimeSheetFollowingREP.aspx?ID=" & idTimeSheet)
                    Me.hrefVIEWER.Attributes.Add("href", "javascript:OpenRadWindowTool('" & urlTool & "');")
                    'urlDir = "~/TimeSheet/frm_TimeSheetFollowingREP.aspx?ID=" & idTimeSheet
                End Using
            ElseIf Tool_code = "PAR-RMS01" Then '--Time Tools

                Me.ToolsViewer.Visible = True

                Using db As New dbRMS_JIEntities

                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                    Dim tipoAprobacion = 0
                    Dim identity = db.ta_documento_par.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_par

                    'Dim idTimeSheet As Integer = db.ta_documento_viaje.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet
                    Me.lbl_tool_viewer.Text = Tool_name
                    Dim urlTool = "../administrativo/frm_parDetalle?Id=" & identity
                    'Me.hrefVIEWER.Attributes.Add("href", "~/TimeSheet/frm_TimeSheetFollowingREP.aspx?ID=" & idTimeSheet)
                    Me.hrefVIEWER.Attributes.Add("href", "javascript:OpenRadWindowTool('" & urlTool & "');")
                    'urlDir = "~/TimeSheet/frm_TimeSheetFollowingREP.aspx?ID=" & idTimeSheet
                End Using
            End If


            '***************************************************************************************************************************************************
            '*********************************************************TOOL VERIFYING****************************************************************************
            '***************************************************************************************************************************************************

            Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)

            Me.curr_local.Value = sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol
            Me.curr_International.Value = "USD"


            Me.grd_archivos.DataSource = clss_approval.get_ta_archivos_documento_ByApp(Me.HiddenField1.Value)
            Me.grd_archivos.DataBind()

            Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(Me.HiddenField1.Value)
            Me.grd_cate.DataBind()

            'Me.rb_tipoDoc.DataSource = clss_approval.get_Doc_support_Route_PendingALL(CType(Me.lblTipoDoc.Text, Integer), Me.HiddenField1.Value)
            'Me.rb_tipoDoc.DataBind()

            'grd_documentos.DataSource = clss_approval.get_Doc_support_Route_PendingALL(CType(Me.lblTipoDoc.Text, Integer), Me.HiddenField1.Value)
            grd_documentos.DataSource = clss_approval.get_Doc_support_By_Route_Pending(CType(Me.lblTipoDoc.Text, Integer), Me.HiddenField1.Value, clss_approval.get_ta_DocumentosInfoFIELDS("id_ruta", "id_documento", Me.HiddenField1.Value))
            grd_documentos.DataBind()

            '******************************REV 0.0.0.1********************
            'Me.rb_tipoDoc.SelectedIndex = 0
            '******************************REV 0.0.0.1********************

            Me.txtcoments.EmptyMessage = tbl_AppOrden.Rows(0).Item("observacion").ToString '.Replace("''", "'")
            '***********************************VALIDAR SI EL USUARIO PUEDE POSTEAR EN ESTE FORMULARIO*****************

            '*****************THE MAX isn´t necesary that woudl be tha max order*******************************************************
            '*****************And (tbl_AppOrden.Rows(0).Item("id_estadoDoc") <> cSTANDby)**********************************************
            Dim scripting As String = ""
            '*************************************************ROLES, GROUPS Y SHARED ROLES****************************************************
            '**********************************************************ENABLED THE BUTTONS*****************************************************
            '*********************************************CANCEL BUTTON***************************************************************************
            If clss_approval.get_ta_DocumentosInfoFIELDS("IdOriginador", "id_documento", Me.HiddenField1.Value) = Val(Me.Session("E_IdUser")) Then
                scripting &= stateButton("btn_Cancelled", True)
                btn_Cancelled.Enabled = True
                scripting &= stateButton("btn_NotApproved", False, idEstadoTipo)
                btn_NotApproved.Enabled = False
                scripting &= stateButton("btn_STandBy", False)
                btn_STandBy.Enabled = False
            Else
                scripting &= stateButton("btn_Cancelled", False)
                btn_Cancelled.Enabled = False
                scripting &= stateButton("btn_NotApproved", True, idEstadoTipo)
                btn_NotApproved.Enabled = True
                scripting &= stateButton("btn_STandBy", True)
                btn_STandBy.Enabled = True

            End If
            '*********************************************CANCEL BUTTON***************************************************************************

            'idEstadoTipo
            'EstadoTipoPrefijo

            '********************************COMPLETED BUTTON--APPROVED BUTTON******************************
            '**********************************THE LAST STEP***********************************************
            If Me.lblnextruta.Text <> "-1" Then
                btn_Completed.Visible = False
                btn_Approved.Visible = True
            Else
                btn_Completed.Visible = True
                btn_Approved.Visible = False
            End If
            '**********************************THE LAST STEP***********************************************
            '********************************COMPLETED BUTTON--APPROVED BUTTON******************************


            '***********************************************LAST PATCH******************************
            '***********************************************LAST PATCH******************************
            If idEstadoTipo = 1 Then 'VBO
                '    stateButton("btn_NotApproved", False)
                btn_NotApproved.Enabled = False
                Me.btn_Approved.Text = EstadoTipoPrefijo
                '$("#<%=delButton.ClientID%>").val('InActivate');
                scripting &= " $('#" + btn_Approved.ClientID + "').text('" + EstadoTipoPrefijo + "'); "
            End If
            '***********************************************LAST PATCH******************************
            '***********************************************LAST PATCH******************************

            Dim strALL_script As String = String.Format("function f(){{ {0}  Sys.Application.remove_load(f); }}  Sys.Application.add_load(f); ", scripting)


            '**********************************************************ENABLED THE BUTTONS*****************************************************

            If (Not clss_approval.IS_User_StepMAX_Permitted(Me.HiddenField1.Value, Me.Session("E_IdUser"))) Then
                    approve = False
                End If

                If tbl_AppOrden.Rows(0).Item("completo").ToString = "SI" Then ' The process has been completed
                    approve = False
                End If


                If approve = False Then

                    Me.btn_Approved.Enabled = False
                    Me.btn_NotApproved.Enabled = False
                    Me.btn_Cancelled.Enabled = False
                    Me.btn_STandBy.Enabled = False
                    Me.btn_Completed.Enabled = False
                    Me.txtcoments.Visible = False
                    Me.lblt_approval.Visible = False
                    Me.lblt_FileAtth.Visible = True

                    Me.rdListBox_files.Visible = False
                    Dim column As GridColumn = grd_archivos.MasterTableView.GetColumn("fileControl")
                    column.Visible = False
                    column.Display = False

                End If

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", strALL_script, True)

            HttpContext.Current.Session.Add("clss_approval", clss_approval)

            Else

                '**************************************************************TO EVALUATE THE ACTION TO DO***********************************************************
                'btn_Approved.Attributes.Add("OnClick", “this.text='PROCESSING..';this.disabled=true;" + ClientScript.GetPostBackEventReference(btn_Approved, "").ToString())
                'cmdSave.Attributes.Add(“onclick”, “this.disabled=true;document.getElementById('lblProgress’).innerText=’..PROCESSING..’;” + ClientScript.GetPostBackEventReference(cmdSave, “”).ToString());

                If HttpContext.Current.Session.Item("clss_approval") IsNot Nothing Then
                clss_approval = Me.Session.Item("clss_approval")
            End If

            'btn_Approved.Attributes.Add("CssClass", "CssClass")


        End If

        'If fireExange_Rate Then
        '    Dim funcion = "FuncModatTrimTasaCambio()"
        '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", funcion, True)
        'End If


    End Sub


    Public Function stateButton(ByVal strButton As String, ByVal booState As Boolean, Optional idEstadoTipo As Integer = 0) As String

        Dim script As String = ""
        Dim script_2 As String = ""
        Dim strButt As String = ""
        Dim strFunc As String = IIf(booState, ".removeClass('disabled');", ".addClass('disabled');")
        Dim strFuncCOLOR_remove As String = ""
        Dim strFuncCOLOR_add As String = ""
        Dim strALL_script As String = ""

        '$('#btn_Approved').removeClass('disabled');
        '$('#btn_Approved').addClass('disabled');
        'Dim script As String = "  function f(){$('#btn_Approved').addClass('disabled'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);"

        Select Case strButton
            Case "btn_Cancelled"
                'btn_Cancelled.Enabled = booState
                script = "  $('#" + btn_Cancelled.ClientID + "')"
                script_2 = "$('#" + btn_Cancelled.ClientID + "')"
                'script = "  function f(){ var btn = $find(""" + btn_Cancelled.ClientID + """); window.alert(btn); "
                strButt = "btn_Cancelled"
                strFuncCOLOR_remove = IIf(Not booState, ".removeClass('btn-danger');", "")
                strFuncCOLOR_add = IIf(Not booState, ".addClass('btn-default');", "")

            Case "btn_NotApproved"
                'btn_NotApproved.Enabled = booState
                'script = "  function f(){ var btn = $find(""" + btn_NotApproved.ClientID + """); window.alert(btn); "
                script = " $('#" + btn_NotApproved.ClientID + "')"
                script_2 = " $('#" + btn_NotApproved.ClientID + "')"
                strButt = "btn_NotApproved"

                If Not booState Then
                    strFuncCOLOR_remove = ".removeClass('btn-danger'); "
                    strFuncCOLOR_add = ".addClass('btn-default'); "
                Else
                    If idEstadoTipo = 1 Then 'VBO
                        strFuncCOLOR_remove = ".removeClass('btn-danger'); "
                        strFuncCOLOR_add = ".addClass('btn-default'); "
                        strFunc = ".addClass('disabled'); "
                        'btn_NotApproved.Enabled = False

                    End If
                End If


            Case "btn_STandBy"
                'btn_STandBy.Enabled = booState
                'script = "  function f(){ var btn = $find(""" + btn_STandBy.ClientID + """); window.alert(btn); "
                script = "  $('#" + btn_STandBy.ClientID + "')"
                script_2 = " $('#" + btn_STandBy.ClientID + "')"
                strFuncCOLOR_remove = IIf(Not booState, ".removeClass('btn-warning');", "")
                strFuncCOLOR_add = IIf(Not booState, ".addClass('btn-default');", "")
                strButt = "btn_STandBy"
        End Select

        'Script = "function f(){ $find(""" + rwAlert.ClientID + """)"  '".show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);"
        'script = "function f(){ $('#" & strButt & "')" & strFunc & ";} "
        'script &= "Sys.Application.remove_load(f);  Sys.Application.add_load(f); } "

        'script &= strFunc & "  Sys.Application.remove_load(f); } Sys.Application.add_load(f); "

        'strALL_script = "function f(){ "

        If strFuncCOLOR_remove <> "" Then
            strALL_script &= String.Format("{0}{1}", script_2, strFuncCOLOR_remove)
        End If

        If strFuncCOLOR_add <> "" Then
            strALL_script &= String.Format("{0}{1} ", script_2, strFuncCOLOR_add)
        End If

        strALL_script &= String.Format("{0}{1} ", script, strFunc)

        'strALL_script &= " Sys.Application.remove_load(f); }  Sys.Application.add_load(f); "

        stateButton = strALL_script
        'Select Case strButton
        '    Case "btn_Cancelled"
        '        btn_Cancelled.Enabled = booState
        '    Case "btn_NotApproved"
        '        btn_NotApproved.Enabled = booState
        '    Case "btn_STandBy"
        '        btn_STandBy.Enabled = booState
        'End Select

    End Function


    Protected Sub grd_archivos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_archivos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim ImageDownload As New HyperLink
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            'Visible = itemD("colm_select").FindControl("chkSelect")


            'Dim j = e.Item.Cells(6).Text.ToString
            Dim arregloExtension() As String = itemD("extension").Text.Split(",")
            'Dim strPrBvar As String = CType(e.Item, GridEditableItem).KeyValues

            Dim uploader As RadAsyncUpload = CType(itemD("fileControl").FindControl("RadAsyncUpload1"), RadAsyncUpload)
            'uploader.AllowedFileExtensions = arregloExtension
            'uploader.RenderMode = AsyncUpload.UploadedFilesRendering.BelowFileInput
            'uploader.MaxFileInputsCount = 1
            'uploader.MultipleFileSelection = AsyncUpload.MultipleFileSelection.Disabled

            ImageDownload = CType(e.Item.FindControl("ImageDownload"), HyperLink)
            ImageDownload.ImageUrl = "~/Imagenes/Iconos/adjunto.png"
            ImageDownload.NavigateUrl = "~/FileUploads/ApprovalProcc/" & itemD("ruta_archivos").Text ' e.Item.Cells(3).Text.ToString
            ImageDownload.Target = "_blank"

        End If
    End Sub


    Protected Sub EXECUTE_ACTION(ByVal vEsTatE As Integer)

        Using dbEntities As New dbRMS_JIEntities

            'Dim RadAsyncUpload1 As New RadAsyncUpload

            Dim duracion = 0
            Dim Err As Boolean = False

            Dim tbl_rutas_tipo_doc As New DataTable
            Dim strComment As String = ""


            Try

                Check_NewFile_FileUploaded() 'Uploading New Files here

                FileUploaded_UpdatedFiles() 'Uploading Files here

                '******This has to change for Stand BY app*********************
                Dim idRuta = Me.lblnextruta.Text
                Dim idNextRol = Me.lblNextRole.Text
                Dim idNextUserID = Me.lblNextUserID.Text
                Dim idRoute As Integer = 0
                idRoute = clss_approval.get_ta_AppDocumentoOrden_MAX(Me.HiddenField1.Value).Rows(0).Item("id_ruta").ToString 'current route

                lblerr_user.Text = ""

                If idRuta = -1 Then 'Just for the last step

                    '*****************SET DOCUMENT REQUIRED BY every approval**************************
                    Dim tbl_Doc As DataTable
                    Dim Bool_pendingDoc As Boolean = False
                    Dim str_pendingDoc As String = ""
                    Dim i As Integer = 0
                    Dim strPart1 As String
                    Dim strPart2 As String
                    Dim Bool_find As Boolean = False

                    tbl_Doc = clss_approval.get_Doc_support_Route_Pending(CType(Me.lblTipoDoc.Text, Integer), Me.HiddenField1.Value)

                    For Each dtRow As DataRow In tbl_Doc.Rows

                        If dtRow("requeridoFin") = "SI" Then


                            For Each item As RadListBoxItem In rdListBox_files.Items

                                If CType(item.Value, Integer) = dtRow("id_doc_soporte") Then
                                    Bool_find = True
                                End If

                            Next

                            'For i = 0 To Me.ListBox_file.Items.Count - 1
                            '    If CType(Me.ListBox_file.Items(i).Value, Integer) = dtRow("id_doc_soporte") Then
                            '        Bool_find = True
                            '    End If
                            'Next

                            If Not Bool_find Then
                                Bool_pendingDoc = True
                                str_pendingDoc = """" & dtRow("nombre_documento") & " """", "
                                i += 1
                            End If

                            Bool_find = False

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
                        lblerr_user.Text = String.Format("The document{0}: {1} {2} required. Please attached it before completed this approval proccess", strPart1, str_pendingDoc, strPart2)
                        Me.lblerr_user.Visible = True
                        Err = True
                    End If

                    '*****************SET DOCUMENT REQUIRED BY every approval**************************
                End If

                Dim Tool_code As String = clss_approval.get_ta_DocumentosInfoFIELDS("tool_code", "id_documento", Me.HiddenField1.Value)


                'vEsTatE = 1000

                If Err = False Then


                    Select Case vEsTatE


                        Case cAPPROVED '*****************DOCUMENTO APROBADO***************************
                            '****************************OBTENIENDO LA PROXIMA RUTA***********

                            Dim tbl_AppOrderO As New DataTable

                            'If came from StandBy State, We need to retur n to the ROL originator if is required**********************
                            If clss_approval.get_ta_DocumentosInfoFIELDS("id_estadoDoc", "id_documento", Me.HiddenField1.Value) = cSTANDby Then

                                tbl_AppOrderO = clss_approval.get_ta_AppDocumentoOrden_MAX(Me.HiddenField1.Value) ' To get the Max ORder values to make the same step again

                                If tbl_AppOrderO.Rows.Count > 0 Then

                                    '*******************************************check this one************************************************
                                    '****************Getting the values of the Max Order Again to return the approve**************************
                                    idRuta = tbl_AppOrderO.Rows(0).Item("id_ruta").ToString
                                    idNextRol = tbl_AppOrderO.Rows(0).Item("id_rol").ToString
                                    idNextUserID = tbl_AppOrderO.Rows(0).Item("id_usuario_app").ToString 'Return to de user that put the document in estand by status
                                    '*****************************Getting the new values of the Max Order Again*******************************

                                End If

                            End If


                            tbl_rutas_tipo_doc = clss_approval.get_Route_By_DocumentType(idRuta)

                            If tbl_rutas_tipo_doc.Rows.Count > 0 Then
                                duracion = tbl_rutas_tipo_doc.Rows(0).Item("duracion")
                            End If

                            Dim docState As Integer
                            If idRuta = -1 Then ' there is not more steps
                                docState = cCOMPLETED
                            Else
                                docState = cAPPROVED
                            End If

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(Me.lblIDocumento.Text, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", docState, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            'clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", , "id_app_documento", clss_approval.id_App_Documento) 'Add the Actual Role it is necesary?

                            If clss_approval.save_ta_AppDocumento() <> -1 Then

                                Save_NewFiles(CType(Me.lblIDocumento.Text, Integer), Tool_code)

                                SaveComment(Me.lblIDocumento.Text, docState, Me.txtcoments.Text.Trim) '.Replace("'", "''")

                                If idRuta = -1 Then ' there is not more steps
                                    'If you have to do something, do here....

                                    clss_approval.set_ta_documento(Me.HiddenField1.Value)
                                    clss_approval.set_ta_documentoFIELDS("completo", "SI", "id_documento", clss_approval.id_Documento)

                                    If clss_approval.save_ta_documento() <> -1 Then

                                        '***************Ver la categoria y ver si aplica registro ambiental**********************
                                        If clss_approval.get_enviromentalDoc(Me.HiddenField1.Value) = 1 Then

                                            clss_approval.set_ta_documento_ambiental(0)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_documento", Me.HiddenField1.Value, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_estado", cPENDING, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("observacion", clss_approval.get_ta_DocumentosInfoFIELDS("descripcion_doc", "id_documento", Me.HiddenField1.Value), "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            'clss_approval.set_ta_documento_ambientalFIELDS("fecha_aprobado", Date.UtcNow, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_usuario_creo", Me.Session("E_IdUser"), "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("fecha_creado", Date.UtcNow, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_tipoApp_Environmental", 0, "id_documento_ambiental", clss_approval.id_documento_ambiental)

                                            If clss_approval.save_ta_documento_ambiental() <> -1 Then
                                            End If

                                        End If

                                        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)


                                        If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                            '**************  Estatus 3 *********** Approved***********************************************************
                                            Dim result = clss_approval.TimeSheet_Update_Status(3, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                            '**************  Estatus 3 *********** Approved***********************************************************

                                            If result <> -1 Then
                                                '*********************************OPEN****************************************
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                                Else 'Error mandando Email
                                                End If
                                                '*********************************OPEN****************************************
                                            End If

                                        ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                            '**************  Estatus 3 *********** Approved***********************************************************
                                            Dim result = clss_approval.Deliverable_Update_Status(3, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                            '**************  Estatus 3 *********** Approved***********************************************************

                                            If result <> -1 Then

                                                '*********************************APPROVED****************************************
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                                Else 'Error mandando Email
                                                End If
                                                '*********************************APPROVED****************************************

                                            End If

                                        ElseIf Tool_code = "TRAVEL-RM01" Then '--Deliverable Tools

                                            'Dim id_documento = CType(Me.lblIDocumento.Text, Integer)
                                            'Dim viaje = dbEntities.ta_documento_viaje.Where(Function(p) p.id_documento = idDoc).FirstOrDefault()


                                            Dim viaje = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_documento = idDoc Or p.id_documento_legalizacion = idDoc Or p.id_documento_informe = idDoc).FirstOrDefault()
                                            If viaje IsNot Nothing Then

                                                If viaje.id_documento = idDoc Then
                                                    '**************  Estatus 2 *********** Approval in Process ***********************************************************
                                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1020, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                    If (objEmail.Emailing_APPROVAL_TRAVEL(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                    Else 'Error mandando Email
                                                    End If
                                                ElseIf viaje.id_documento_informe = idDoc Then
                                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1022, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                    If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                    Else 'Error mandando Email
                                                    End If
                                                ElseIf viaje.id_documento_legalizacion = idDoc Then
                                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1021, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                    If (objEmail.Emailing_APPROVAL_TRAVEL_LEGALIZATION(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                    Else 'Error mandando Email
                                                    End If
                                                End If


                                            End If
                                        ElseIf Tool_code = "PAR-RMS01" Then

                                            Dim par = dbEntities.vw_tme_par.Where(Function(p) p.id_documento = idDoc).FirstOrDefault()
                                            If par IsNot Nothing Then

                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1023, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_PAR(CType(Me.lblIDocumento.Text, Integer), par.id_par)) Then
                                                Else 'Error mandando Email
                                                End If


                                            End If

                                            'Else 'No tool related to this approval

                                            '    '***************Ver la categoria y ver si aplica registro ambiental*****************************************
                                            '    '********************************COMPLETED DOCUMENT*********************************************************
                                            '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                            '    Else 'Error mandando Email
                                            '    End If
                                            ' ********************************COMPLETED DOCUMENT*********************************************************

                                        End If



                                    Else 'Error Saving Docs


                                    End If


                                Else 'Yes there is more steps


                                    Dim fecha_recep As DateTime = Date.UtcNow 'DateAdd(DateInterval.Hour, 8, Date.UtcNow)
                                    Dim fecha_limit As DateTime = calculaDiaHabil(duracion, fecha_recep)

                                    strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                                    clss_approval.set_ta_AppDocumento(0) 'New Record
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_documento", Me.HiddenField1.Value, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_recep, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cPENDING, "id_app_documento", 0) 'Pending Step
                                    clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_app_documento", 0) 'Pending Step .Replace("'", "''")
                                    'clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_recep, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", idNextUserID, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) 'Add the next Role

                                    Dim id_appdocumento = clss_approval.save_ta_AppDocumento()

                                    If id_appdocumento <> -1 Then
                                        '********************************************************************************
                                        '***********************COPIANDO LOS ARCHIVOS A LA NUEVA VERSION***************
                                        '*************************NEW version change, just we goint to save the files required by the user***************************
                                        'sql = "INSERT INTO ta_archivos_documento SELECT " & id_appdocumento & " as id_App_Documento, archivo, id_doc_soporte FROM ta_archivos_documento WHERE id_App_Documento= " & Me.lblIDocumento.Text
                                        'dm.SelectCommand.CommandText = sql
                                        'dm.SelectCommand.ExecuteNonQuery()
                                    Else 'Error Saving
                                    End If


                                    '' 2 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then

                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))
                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 2 'In Approved Process

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                    '        If result <> -1 Then
                                    '            '*********************************OPEN****************************************
                                    '            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '            Else 'Error mandando Email
                                    '            End If
                                    '            '*********************************OPEN****************************************
                                    '        End If

                                    '    End Using

                                    'Else

                                    '    '*********************************APPROVED NEXT STEP****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************APPROVED NEXT STEP****************************************

                                    'End If
                                    '' 2 - *******************************TOOLS TIME SHEET**********************************

                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                        '**************  Estatus 2 *********** Approved In Process ***********************************************************
                                        Dim result = clss_approval.TimeSheet_Update_Status(2, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                        '**************  Estatus 2 ***********  Approved In Process***********************************************************

                                        If result <> -1 Then
                                            '*********************************NEXT STEP APP****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************NEXT STEP APP****************************************
                                        End If

                                    ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                        '**************  Estatus 2 *********** Approval in Process ***********************************************************
                                        Dim result = clss_approval.Deliverable_Update_Status(2, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                        '**************  Estatus 2 *********** Approval in Process ***********************************************************

                                        If result <> -1 Then
                                            '*********************************APPROVED****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************APPROVED****************************************

                                        End If

                                    ElseIf Tool_code = "TRAVEL-RM01" Then '--Deliverable Tools

                                        'Dim id_documento = CType(Me.lblIDocumento.Text, Integer)
                                        'Dim viaje = dbEntities.ta_documento_viaje.Where(Function(p) p.id_documento = idDoc).FirstOrDefault()


                                        Dim viaje = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_documento = idDoc Or p.id_documento_legalizacion = idDoc Or p.id_documento_informe = idDoc).FirstOrDefault()
                                        If viaje IsNot Nothing Then

                                            If viaje.id_documento = idDoc Then
                                                '**************  Estatus 2 *********** Approval in Process ***********************************************************
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1020, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            ElseIf viaje.id_documento_informe = idDoc Then
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1022, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            ElseIf viaje.id_documento_legalizacion = idDoc Then
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1021, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL_LEGALIZATION(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            End If


                                        End If
                                    ElseIf Tool_code = "PAR-RMS01" Then

                                        Dim par = dbEntities.vw_tme_par.Where(Function(p) p.id_documento = idDoc).FirstOrDefault()
                                        If par IsNot Nothing Then

                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1023, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_APPROVAL_PAR(CType(Me.lblIDocumento.Text, Integer), par.id_par)) Then
                                            Else 'Error mandando Email
                                            End If


                                        End If

                                        ''**************  Estatus 2 *********** Approval in Process ***********************************************************
                                        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1020, cl_user.regionalizacionCulture, id_appdocumento)
                                        'If (objEmail.Emailing_APPROVAL_TRAVEL(id_appdocumento, id_viaje)) Then
                                        'Else 'Error mandando Email
                                        'End If

                                        'Else 'No tool related to this approval

                                        '    '*********************************APPROVED NEXT STEP****************************************
                                        '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                        '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                        '    Else 'Error mandando Email
                                        '    End If
                                        '*********************************APPROVED NEXT STEP****************************************

                                    End If




                                End If

                            Else 'Error saving the estep



                            End If 'clss_approval.save_ta_AppDocumento()

                 '****************************************END APPROVED***************************************************

           '****************NO APROBADO ahí se termina el proceso**************************
           '**************************ACTUALIZAR EL REGISTRO QUE SE ESTA EDITANDO***********

                        Case cnotAPPROVED

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(Me.lblIDocumento.Text, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cnotAPPROVED, "id_App_documento", clss_approval.id_App_Documento)

                            If clss_approval.save_ta_AppDocumento() <> -1 Then

                                clss_approval.set_ta_documento(Me.HiddenField1.Value)
                                clss_approval.set_ta_documentoFIELDS("completo", "SI", "id_documento", clss_approval.id_Documento)

                                If clss_approval.save_ta_documento() <> -1 Then

                                    SaveComment(Me.lblIDocumento.Text, cnotAPPROVED, Me.txtcoments.Text.Trim) '.Replace("'", "''")

                                    Save_NewFiles(CType(Me.lblIDocumento.Text, Integer), Tool_code)


                                    ' 3 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then

                                    '    'Update Here the Time Sheet Status
                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 4 ' Not Approved

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                    '        If result <> -1 Then

                                    '            '*********************************OPEN****************************************
                                    '            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '            Else 'Error mandando Email
                                    '            End If
                                    '            '*********************************OPEN****************************************

                                    '        End If

                                    '    End Using

                                    'Else

                                    '    '*********************************NOT APPROVED ****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************NOT APPROVED ****************************************
                                    'End If
                                    ' 3 - *******************************TOOLS TIME SHEET**********************************

                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                        '**************  Estatus 4 *********** NOT Approved***********************************************************
                                        Dim result = clss_approval.TimeSheet_Update_Status(4, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                        '**************  Estatus 4 *********** NOT Approved***********************************************************

                                        If result <> -1 Then
                                            '*********************************NEXT STEP APP****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************NEXT STEP APP****************************************
                                        End If

                                    ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                        '**************  Estatus 4 *********** Not Approved***********************************************************
                                        Dim result = clss_approval.Deliverable_Update_Status(4, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                        '**************  Estatus 4 *********** Not Approved***********************************************************

                                        If result <> -1 Then
                                            '*********************************APPROVED****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************APPROVED****************************************

                                        End If

                                    ElseIf Tool_code = "TRAVEL-RM01" Then '--Deliverable Tools

                                        'Dim id_documento = CType(Me.lblIDocumento.Text, Integer)
                                        'Dim viaje = dbEntities.ta_documento_viaje.Where(Function(p) p.id_documento = idDoc).FirstOrDefault()


                                        Dim viaje = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_documento = idDoc Or p.id_documento_legalizacion = idDoc Or p.id_documento_informe = idDoc).FirstOrDefault()
                                        If viaje IsNot Nothing Then

                                            If viaje.id_documento = idDoc Then
                                                '**************  Estatus 2 *********** Approval in Process ***********************************************************
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1020, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            ElseIf viaje.id_documento_informe = idDoc Then
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1022, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            ElseIf viaje.id_documento_legalizacion = idDoc Then
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1021, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL_LEGALIZATION(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            End If


                                        End If
                                    ElseIf Tool_code = "PAR-RMS01" Then

                                        Dim par = dbEntities.vw_tme_par.Where(Function(p) p.id_documento = idDoc).FirstOrDefault()
                                        If par IsNot Nothing Then

                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1023, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_APPROVAL_PAR(CType(Me.lblIDocumento.Text, Integer), par.id_par)) Then
                                            Else 'Error mandando Email
                                            End If


                                        End If
                                        ''**************  Estatus 2 *********** Approval in Process ***********************************************************
                                        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1020, cl_user.regionalizacionCulture, id_appdocumento)
                                        'If (objEmail.Emailing_APPROVAL_TRAVEL(id_appdocumento, id_viaje)) Then
                                        'Else 'Error mandando Email
                                        'End If


                                        'Else 'No tool related to this approval

                                        '    '*********************************APPROVED NEXT STEP****************************************
                                        '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                        '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                        '    Else 'Error mandando Email
                                        '    End If
                                        '*********************************APPROVED NEXT STEP****************************************

                                    End If


                                Else 'Error

                                End If


                            Else 'Error happened
                            End If

                        '****************************************FIN***************************************************

                         '********************CANCELADO FIN DEL DOCUMENTO (ACTUALIZAR campo complet a SI EN ta_documentos )
                         '****************************ACTUALIZAR EL REGISTRO QUE SE ESTA EDITANDO***********
                        Case cCANCELLED

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(Me.lblIDocumento.Text, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cCANCELLED, "id_App_documento", clss_approval.id_App_Documento)

                            If clss_approval.save_ta_AppDocumento() <> -1 Then

                                clss_approval.set_ta_documento(Me.HiddenField1.Value)
                                clss_approval.set_ta_documentoFIELDS("completo", "SI", "id_documento", clss_approval.id_Documento)

                                If clss_approval.save_ta_documento() <> -1 Then

                                    SaveComment(Me.lblIDocumento.Text, cCANCELLED, Me.txtcoments.Text.Trim) '.Replace("'", "''")
                                    Save_NewFiles(CType(Me.lblIDocumento.Text, Integer), Tool_code)


                                    '' 4 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then
                                    '    'Update Here the Time Sheet Status

                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 4 ' Not Approved

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                    '        If result <> -1 Then

                                    '            '*********************************OPEN****************************************
                                    '            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '            Else 'Error mandando Email
                                    '            End If
                                    '            '*********************************OPEN****************************************

                                    '        End If

                                    '    End Using

                                    'Else

                                    '    '*********************************CANCELLED ****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************CANCELLED****************************************
                                    'End If

                                    '' 4 - *******************************TOOLS TIME SHEET**********************************


                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                        '**************  Estatus 6 *********** Cancelled  ***********************************************************
                                        Dim result = clss_approval.TimeSheet_Update_Status(6, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                        '**************  Estatus 6 ***********  Cancelled ***********************************************************

                                        If result <> -1 Then
                                            '*********************************NEXT STEP APP****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************NEXT STEP APP****************************************
                                        End If

                                    ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                        '**************  Estatus 6 *********** Cancelled ***********************************************************
                                        Dim result = clss_approval.Deliverable_Update_Status(6, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                        '**************  Estatus 6 *********** Cancelled ***********************************************************

                                        If result <> -1 Then
                                            '*********************************APPROVED****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************APPROVED****************************************

                                        End If

                                    ElseIf Tool_code = "TRAVEL-RM01" Then '--Deliverable Tools

                                        'Dim id_documento = CType(Me.lblIDocumento.Text, Integer)
                                        'Dim viaje = dbEntities.ta_documento_viaje.Where(Function(p) p.id_documento = idDoc).FirstOrDefault()


                                        Dim viaje = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_documento = idDoc Or p.id_documento_legalizacion = idDoc Or p.id_documento_informe = idDoc).FirstOrDefault()
                                        If viaje IsNot Nothing Then

                                            If viaje.id_documento = idDoc Then
                                                '**************  Estatus 2 *********** Approval in Process ***********************************************************
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1020, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            ElseIf viaje.id_documento_informe = idDoc Then
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1022, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            ElseIf viaje.id_documento_legalizacion = idDoc Then
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1021, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL_LEGALIZATION(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            End If


                                        End If
                                    ElseIf Tool_code = "PAR-RMS01" Then

                                        Dim par = dbEntities.vw_tme_par.Where(Function(p) p.id_documento = idDoc).FirstOrDefault()
                                        If par IsNot Nothing Then

                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1023, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_APPROVAL_PAR(CType(Me.lblIDocumento.Text, Integer), par.id_par)) Then
                                            Else 'Error mandando Email
                                            End If


                                        End If
                                        ''**************  Estatus 2 *********** Approval in Process ***********************************************************
                                        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1020, cl_user.regionalizacionCulture, id_appdocumento)
                                        'If (objEmail.Emailing_APPROVAL_TRAVEL(id_appdocumento, id_viaje)) Then
                                        'Else 'Error mandando Email
                                        'End If

                                        'Else 'No tool related to this approval

                                        '    '*********************************APPROVED NEXT STEP****************************************
                                        '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                        '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                        '    Else 'Error mandando Email
                                        '    End If
                                        '    '*********************************APPROVED NEXT STEP****************************************

                                    End If


                                Else 'Error
                                End If


                            Else  'Error
                            End If


                        Case cSTANDby
                            '*********************************************************************************

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(Me.lblIDocumento.Text, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cSTANDby, "id_App_documento", clss_approval.id_App_Documento)

                            If clss_approval.save_ta_AppDocumento() <> -1 Then


                                SaveComment(Me.lblIDocumento.Text, cSTANDby, Me.txtcoments.Text.Trim) '.Replace("'", "''")
                                Save_NewFiles(CType(Me.lblIDocumento.Text, Integer), Tool_code)


                                Dim tbl_AppOrderO As New DataTable
                                'tbl_AppOrderO = clss_approval.get_ta_AppDocumento_byOrden(Me.HiddenField1.Value, 0) ' To get the Rol originator Problem when repeat
                                tbl_AppOrderO = clss_approval.get_ta_AppDocumentoOrden_MIN(Me.HiddenField1.Value) 'To get the info on the min step
                                'To Create a New APP to the originator in Stand by state

                                '****************Getting the new values of the originator**************************
                                idRuta = tbl_AppOrderO.Rows(0).Item("id_ruta").ToString
                                idNextRol = tbl_AppOrderO.Rows(0).Item("id_rol").ToString
                                idNextUserID = tbl_AppOrderO.Rows(0).Item("id_usuario_app").ToString 'The user who applied as originator from this Approval procc
                                '****************Getting the new values of the originator**************************

                                tbl_rutas_tipo_doc = clss_approval.get_Route_By_DocumentType(idRuta)

                                If tbl_rutas_tipo_doc.Rows.Count > 0 Then
                                    duracion = tbl_rutas_tipo_doc.Rows(0).Item("duracion")
                                End If

                                Dim fecha_recep As DateTime = Date.UtcNow 'DateAdd(DateInterval.Hour, 8, Date.UtcNow)
                                Dim fecha_limit As DateTime = calculaDiaHabil(duracion, fecha_recep)

                                strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                                clss_approval.set_ta_AppDocumento(0) 'New Record in stanb By
                                clss_approval.set_ta_AppDocumentoFIELDS("id_documento", Me.HiddenField1.Value, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0) 'IdRutaOriginator
                                clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_recep, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cSTANDby, "id_app_documento", 0) 'Pending Step
                                clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_app_documento", 0) 'Pending Step '.Replace("'", "''")
                                'clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_recep, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", idNextUserID, "id_app_documento", 0) 'IdUSerORiginator
                                clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) ' idrolORiginator

                                Dim id_appdocumento = clss_approval.save_ta_AppDocumento()

                                If id_appdocumento <> -1 Then

                                    '' 5 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then


                                    '    'Update Here the Time Sheet Status
                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 5 'Observation Pending

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))


                                    '        'Update Here the Time Sheet Status
                                    '        '*********************************OPEN****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************OPEN****************************************

                                    '    End Using


                                    'Else

                                    '    '*********************************STAND BY****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************STAND BY****************************************

                                    'End If


                                    '' 5 - *******************************TOOLS TIME SHEET**********************************

                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                        '**************  Estatus 5 *********** 'Observation Pending ***********************************************************
                                        Dim result = clss_approval.TimeSheet_Update_Status(5, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                        '**************  Estatus 5 *********** 'Observation Pending ***********************************************************

                                        If result <> -1 Then
                                            '*********************************NEXT STEP APP****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************NEXT STEP APP****************************************
                                        End If

                                    ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                        '**************  Estatus 5 *********** 'Observation Pending ***********************************************************
                                        Dim result = clss_approval.Deliverable_Update_Status(5, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                        '**************  Estatus 5 *********** 'Observation Pending ***********************************************************

                                        If result <> -1 Then
                                            '*********************************APPROVED****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************APPROVED****************************************

                                        End If

                                    ElseIf Tool_code = "TRAVEL-RM01" Then '--Deliverable Tools

                                        'Dim id_documento = CType(Me.lblIDocumento.Text, Integer)
                                        'Dim viaje = dbEntities.ta_documento_viaje.Where(Function(p) p.id_documento = idDoc).FirstOrDefault()


                                        Dim viaje = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_documento = idDoc Or p.id_documento_legalizacion = idDoc Or p.id_documento_informe = idDoc).FirstOrDefault()
                                        If viaje IsNot Nothing Then

                                            If viaje.id_documento = idDoc Then
                                                '**************  Estatus 2 *********** Approval in Process ***********************************************************
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1020, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            ElseIf viaje.id_documento_informe = idDoc Then
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1022, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL_INFORME(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            ElseIf viaje.id_documento_legalizacion = idDoc Then
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1021, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_APPROVAL_TRAVEL_LEGALIZATION(CType(Me.lblIDocumento.Text, Integer), viaje.id_viaje)) Then
                                                Else 'Error mandando Email
                                                End If
                                            End If


                                        End If
                                    ElseIf Tool_code = "PAR-RMS01" Then

                                        Dim par = dbEntities.vw_tme_par.Where(Function(p) p.id_documento = idDoc).FirstOrDefault()
                                        If par IsNot Nothing Then

                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1023, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_APPROVAL_PAR(CType(Me.lblIDocumento.Text, Integer), par.id_par)) Then
                                            Else 'Error mandando Email
                                            End If


                                        End If
                                        ''**************  Estatus 2 *********** Approval in Process ***********************************************************
                                        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1020, cl_user.regionalizacionCulture, id_appdocumento)
                                        'If (objEmail.Emailing_APPROVAL_TRAVEL(id_appdocumento, id_viaje)) Then
                                        'Else 'Error mandando Email
                                        'End If

                                        'Else 'No tool related to this approval

                                        '    '*********************************APPROVED NEXT STEP****************************************
                                        '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                        '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                        '    Else 'Error mandando Email
                                        '    End If
                                        '    '*********************************APPROVED NEXT STEP****************************************

                                    End If

                                End If

                            Else 'Error

                            End If


                    End Select


                    Me.Response.Redirect("~/Approvals/frm_consulta_docsPending.aspx")


                End If

            Catch ex As Exception
                lblerr_user.Text = String.Format("An error was found in the action: {0} ", ex.Message)
                Me.lblerr_user.Visible = True
                Me.btn_Approved.Enabled = False
                Me.btn_NotApproved.Enabled = False
                Me.btn_Cancelled.Enabled = False
                Me.btn_STandBy.Enabled = False
                Me.btn_Completed.Enabled = False

            End Try


        End Using


    End Sub


    'Protected Sub btn_approved_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_approved.Click

    '    'Me.Response.Redirect("~/approvals/frm_consulta_docsPending.aspx")

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




    Public Sub SaveComment(ByVal idApp As Integer, ByVal idEstadoDoc As Integer, ByVal Comment As String)

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
        clss_approval.set_ta_comentariosDocFIELDS("id_usuario", Me.Session("E_IdUser"), "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("fecha_comentario", Date.UtcNow, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("comentario", strComment.Trim.Trim, "id_comment", 0) '.Replace("  ", "")

        If clss_approval.save_ta_comentariosDoc() = -1 Then
            'Error do somenthing

        End If


    End Sub





    Protected Sub FileUploaded_UpdatedFiles()

        Dim fullPath As String
        Dim fileName As String
        Dim fuLLfiLeName As String

        Dim file_Old As String
        Dim iTemD As GridDataItem
        Dim nAmEuPd As String
        Dim RadAsyncUpload1 As New RadAsyncUpload
        Dim i As Integer = 0
        Dim sql = ""

        'Watch it on the server do you need to have \FileUploads\Temp\, but on developer side you need to try with FileUploads\Temp\

        Dim sFileDirTemp As String = Server.MapPath("~") & "\FileUploads\Temp\"
        Dim sFileDir As String = Server.MapPath("~") & "\fileUploads\ApprovalProcc\"


        Try

            'fullPath = Server.MapPath("~\FileUploads\ApprovalProcc\")
            'fileName = GeTfIleName(e.File.GetName)

            Dim dmyhm As String = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
            Dim fileNameWE '= System.IO.Path.GetFileNameWithoutExtension(fileName)
            Dim extension As String '= System.IO.Path.GetExtension(fileName)

            Dim version As Integer = 0
            Dim strVer As String = ""

            '**********************CAMBIOS DE FONDO***********************************************
            For Each iTemD In Me.grd_archivos.Items

                RadAsyncUpload1 = CType(iTemD("fileControl").FindControl("RadAsyncUpload1"), RadAsyncUpload)

                Try


                    'New file name
                    nAmEuPd = RadAsyncUpload1.UploadedFiles.Item(0).FileName.ToString
                    extension = System.IO.Path.GetExtension(nAmEuPd)

                    file_Old = CType(iTemD("ruta_archivos").Text, String).Trim
                    file_Old = getClear_Name(file_Old)
                    fileNameWE = System.IO.Path.GetFileNameWithoutExtension(file_Old)

                    'If nAmEuPd.Length > 80 Then
                    '    fileNew_name = nAmEuPd.Substring(0, 80)
                    'Else
                    '    fileNew_name = nAmEuPd
                    'End If

                    clss_approval.set_ta_archivos_documento(Convert.ToInt32(iTemD("id_archivo").Text)) 'Load
                    version = clss_approval.get_ta_archivos_documentoFIELDS("ver", "id_archivo", Convert.ToInt32(iTemD("id_archivo").Text))
                    strVer = String.Format("_V1_{0}", version)
                    fileNameWE = fileNameWE.Replace(strVer, "")
                    strVer = String.Format("_V1.{0}", version)
                    fileNameWE = fileNameWE.Replace(strVer, "")

                    version += 1
                    fileNameWE = String.Format("doc{0}_0{1}_{2}_{3}_V1.{4}{5}", Me.HiddenField1.Value, Me.Session("E_IdUser"), dmyhm, fileNameWE, version, extension)
                    'fuLLfiLeName = System.IO.Path.Combine(fullPath, fileNameWE)

                    Dim file_info As New IO.FileInfo(sFileDirTemp & nAmEuPd)
                    file_info.CopyTo(sFileDir & fileNameWE)
                    DelFileParam(nAmEuPd)
                    'e.File.SaveAs(fuLLfiLeName)
                    'Me.lbl_fileUpload.Text &= "& " & fileName
                    '**************************************Actualizamos el Registro************************************************************
                    clss_approval.set_ta_archivos_documento(0)
                        clss_approval.set_ta_archivos_documentoFIELDS("id_App_Documento", Me.lblIDocumento.Text, "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("archivo", fileNameWE, "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("id_doc_soporte", iTemD("id_doc_soporte").Text, "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("ver", version, "id_archivo", 0)
                        If clss_approval.save_ta_archivos_documento() <> -1 Then 'Erro Happenned
                        lblerr_user.Text = ""
                    End If



                Catch ex As Exception
                    lblerr_user.Text = "UpdatedOnes: " & ex.Message
                End Try


            Next

        Catch ex As Exception
            lblerr_user.Text = "UpdatedOnes: " & ex.Message
        End Try

    End Sub


    Protected Sub FileUploaded_Chg_Name(ByVal sender As Object, ByVal e As FileUploadedEventArgs)

        Dim fullPath As String
        Dim fileName As String
        Dim fuLLfiLeName As String

        Dim file_Old As String
        Dim iTemD As GridDataItem
        Dim nAmEuPd As String
        Dim RadAsyncUpload1 As New RadAsyncUpload
        Dim i As Integer = 0
        Dim sql = ""

        'cnnSAP.Open()
        'Dim dm As New SqlDataAdapter(sql, cnnSAP)

        Try

            'e.File.SaveAs(System.Configuration.ConfigurationManager.AppSettings["ContractorDatabaseUploads"] + filenamenew + "_Certificate" + e.File.GetExtension());
            fullPath = Server.MapPath("~\FileUploads\ApprovalProcc\")
            fileName = GeTfIleName(e.File.GetName)

            Dim dmyhm As String = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
            Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(fileName)
            Dim extension As String = System.IO.Path.GetExtension(fileName)

            Dim version As Integer = 0
            Dim strVer As String = ""

            'fuLLfiLeName = System.IO.Path.Combine(fullPath, fileName)
            'e.File.SaveAs(fuLLfiLeName)

            '**********************CAMBIOS DE FONDO***********************************************
            For Each iTemD In Me.grd_archivos.Items


                ' Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(args.FileName) + "_v1.1"
                'Dim File As String '= "doc" & Me.HiddenField1.Value & "_0" & Me.Session("E_IdUser") & "_" & dmyhm & "_" & fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension
                'Me.HiddenField1.Value
                'File = String.Format("doc{0}_0{1}_{2}_{3}{4}", Me.HiddenField1.Value, Me.Session("E_IdUser"), dmyhm, fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-"), extension)
                'args.CopyTo(sFileDir & File)

                RadAsyncUpload1 = CType(iTemD("fileControl").FindControl("RadAsyncUpload1"), RadAsyncUpload)
                file_Old = CType(iTemD("ruta_archivos").Text, String).Trim
                file_Old = getClear_Name(file_Old)
                fileNameWE = System.IO.Path.GetFileNameWithoutExtension(file_Old)

                Try

                    'If String.IsNullOrEmpty(RadAsyncUpload1.UploadedFiles.Item(0).FileName.ToString) Then
                    nAmEuPd = RadAsyncUpload1.UploadedFiles.Item(0).FileName.ToString

                    '*********************REV 0.0.0.2********************************
                    If Trim(nAmEuPd) = Trim(e.File.GetName) Then 'Si es el mismo archivo que disparo el evento

                        clss_approval.set_ta_archivos_documento(Convert.ToInt32(iTemD("id_archivo").Text)) 'Load
                        version = clss_approval.get_ta_archivos_documentoFIELDS("ver", "id_archivo", Convert.ToInt32(iTemD("id_archivo").Text))
                        strVer = String.Format("_V1_{0}", version)
                        fileNameWE = fileNameWE.Replace(strVer, "")
                        strVer = String.Format("_V1.{0}", version)
                        fileNameWE = fileNameWE.Replace(strVer, "")

                        version += 1
                        fileNameWE = String.Format("doc{0}_0{1}_{2}_{3}_V1.{4}{5}", Me.HiddenField1.Value, Me.Session("E_IdUser"), dmyhm, fileNameWE, version, extension)
                        ' fileNameWE = fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & "_V1." & version.ToString & extension
                        fuLLfiLeName = System.IO.Path.Combine(fullPath, fileNameWE)
                        e.File.SaveAs(fuLLfiLeName)


                        Me.lbl_fileUpload.Text &= "& " & fileName
                        '**************************************Actualizamos el Registro************************************************************
                        'sql = "UPDATE ta_archivos_documento SET archivo ='" & fileName & "'  WHERE id_archivo=" & iTemD("id_archivo").Text
                        'dm.SelectCommand.CommandText = sql
                        'dm.SelectCommand.ExecuteNonQuery()
                        'DelFile(file_Old) '**********Borramos el anterior archivos
                        clss_approval.set_ta_archivos_documento(0)
                        clss_approval.set_ta_archivos_documentoFIELDS("id_App_Documento", Me.lblIDocumento.Text, "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("archivo", fileNameWE, "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("id_doc_soporte", iTemD("id_doc_soporte").Text, "id_archivo", 0)
                        clss_approval.set_ta_archivos_documentoFIELDS("ver", version, "id_archivo", 0)
                        If clss_approval.save_ta_archivos_documento() <> -1 Then 'Erro Happenned

                        End If

                    End If

                Catch ex As Exception
                    nAmEuPd = ex.Message
                End Try


            Next

        Catch ex As Exception
            'lblMsg.Text = ex.Message
        End Try

        cnnSAP.Close()

    End Sub


    Public Function getClear_Name(ByVal strName As String) As String

        Dim posC As Integer = 0
        Dim posF As Integer = 0
        Dim mark As Integer = 0
        Dim strNW_name As String

        'strNW_name = InStr(0, "_", strName)
        'posC = 1
        'For i As Integer = 1 To 3
        '    posC = InStr(posC, "_", strName)
        'Next

        'posF = posC
        'strNW_name = strName.Substring(posF, strName.Length - posF)

        For Each chrVal As Char In strName

            If chrVal = "_" Then
                mark += 1
            End If
            posC += 1

            If mark = 3 Then
                posF = posC
                Exit For
            End If

        Next

        strNW_name = strName.Substring(posF, strName.Length - (posF))

        getClear_Name = strNW_name

    End Function



    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnk As HyperLink = New HyperLink
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            hlnk = CType(itemD("completoC").FindControl("Completo"), HyperLink)
            hlnk.ToolTip = "Alert"

            hlnk.ImageUrl = "../Imagenes/Iconos/Circle_" & itemD("Alerta").Text & ".png" ' e.Item.Cells(7).Text.ToString
            If itemD("descripcion_estado").Text = "Pending" Then
                For i As Integer = 2 To 8
                    e.Item.Cells(i).BackColor = Color.FromArgb(227, 132, 67)
                Next
            End If

        End If
    End Sub

    'Protected Sub img_btn_agregar_temp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_btn_agregar_temp.Click

    '    '******************************REV 0.0.0.1********************
    '    If Not String.IsNullOrEmpty(Me.lblarchivo.Text) Then
    '        Me.lblMsg.Text = LTrim(RTrim(Me.lblarchivo.Text)) & ", ya fué registrado en la lista de archivos."
    '        ' Me.lblarchivo.Text = ""
    '    End If
    '    '******************************REV 0.0.0.1********************

    'End Sub

    'Protected Sub img_btn_borrar_temp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_btn_borrar_temp.Click
    '    DelList()
    '    DelFile()
    'End Sub

    'Protected Sub btn_remove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_remove.Click
    '    DelFileParam(Me.ListBox_file.SelectedItem.Text)
    '    Me.ListBox_file.Items.Remove(Me.ListBox_file.SelectedItem)
    'End Sub

    Protected Sub btn_Approved_Click(sender As Object, e As EventArgs) Handles btn_Approved.Click

        'If check_exchange_Rate() Then
        'EXECUTE_ACTION(cAPPROVED)
        'btn_Approved.Enabled = True
        'End If

        Dim IsTool As Int16 = Convert.ToInt16(Me.hd_is_tool.Value)
        Dim boolIS_deliverable As Boolean = Convert.ToBoolean(IsTool)

        If boolIS_deliverable Then
            check_allocated_amount()  'This is for confirming the allocated amount for Deliverable
        Else
            EXECUTE_ACTION(cAPPROVED)
        End If

    End Sub

    Protected Sub btn_Completed_Click(sender As Object, e As EventArgs) Handles btn_Completed.Click


        Dim IsTool As Int16 = Convert.ToInt16(Me.hd_is_tool.Value)
        Dim boolIS_deliverable As Boolean = Convert.ToBoolean(IsTool)

        If boolIS_deliverable Then
            check_allocated_amount()  'This is for confirming the allocated amount for Deliverable
        Else
            EXECUTE_ACTION(cAPPROVED)
        End If




    End Sub

    Protected Function check_exchange_Rate() As Boolean

        Using db As New dbRMS_JIEntities

            Dim strFuncion As String = ""
            Dim booPass As Boolean = True

            Dim fechaReg As Date = Date.UtcNow
            Dim oTasaCambio = db.t_trimestre_tasa_cambio.Where(Function(p) p.mes = Month(fechaReg) And p.t_trimestre.anio = Year(fechaReg)).ToList()

            If oTasaCambio.Count() > 0 Then
                Me.hd_tasa_cambio.Value = oTasaCambio.FirstOrDefault().id_trimestre_tasa_cambio
            Else
                Me.hd_tasa_cambio.Value = 0
                strFuncion = "FuncModatTrimTasaCambio()"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", strFuncion, True)
                booPass = False
            End If

            If booPass Then

                'strFuncion = "FuncModal_Monto()"
                'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", strFuncion, True)
                check_exchange_Rate = False

            Else

                check_exchange_Rate = True

            End If

        End Using


    End Function

    Protected Function check_allocated_amount() As Boolean

        Using db As New dbRMS_JIEntities

            Dim cls_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(Convert.ToInt32(Me.Session("E_IDprograma")))
            Dim idDeliverable As Integer = Convert.ToInt32(Me.hd_id_delivered.Value)
            Dim tbl_Deliverable As DataTable = cls_Deliverable.get_Deliverable_Activity(0, idDeliverable)

            Dim UpdatedBy As String = tbl_Deliverable.Rows.Item(0).Item("updatedBy")
            Dim EndValue As Double = Convert.ToDecimal(tbl_Deliverable.Rows.Item(0).Item("valor_final"))
            Dim EndexChangeRate As Double = Convert.ToDecimal(tbl_Deliverable.Rows.Item(0).Item("tasa_cambio_final"))

            If EndValue > 0 And EndexChangeRate > 0 Then 'It was Updated
                Me.lbl_updatedBy.Text = String.Format("Updated by {0}", UpdatedBy)
                EndexChangeRate = Convert.ToDecimal(tbl_Deliverable.Rows.Item(0).Item("tasa_cambio"))
            End If

            Me.txt_Local_Value.Value = If(EndValue = 0, Convert.ToDecimal(tbl_Deliverable.Rows.Item(0).Item("valor")), EndValue)
            Me.txt_total_tasa_cambio.Value = If(EndexChangeRate = 0, Convert.ToDecimal(Me.hd_tasa_cambio.Value), EndexChangeRate)
            Me.txt_USD_val.Value = If(Convert.ToDecimal(Me.txt_total_tasa_cambio.Value) = 0, 0, Me.txt_Local_Value.Value / Me.txt_total_tasa_cambio.Value)

            Dim funcion = "FuncModal_Monto()"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", funcion, True)

            If Me.txt_USD_val.Value > 0 Then
                check_allocated_amount = True
            Else
                check_allocated_amount = False
            End If

        End Using
    End Function

    Protected Sub btn_STandBy_Click(sender As Object, e As EventArgs) Handles btn_STandBy.Click
        EXECUTE_ACTION(cSTANDby)
    End Sub

    Protected Sub btn_NotApproved_Click(sender As Object, e As EventArgs) Handles btn_NotApproved.Click
        EXECUTE_ACTION(cnotAPPROVED)
    End Sub

    Protected Sub btn_Cancelled_Click(sender As Object, e As EventArgs) Handles btn_Cancelled.Click
        EXECUTE_ACTION(cCANCELLED)
    End Sub

    Protected Sub btn_return_Click(sender As Object, e As EventArgs) Handles btn_return.Click
        Me.Response.Redirect("~/Approvals/frm_consulta_docsPending.aspx")
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

    <Web.Services.WebMethod()>
    Public Shared Function get_DocTYPE(ByVal idProgram As Integer, ByVal id_TipoDoc As Integer, ByVal IdDoc As Integer) As Object


        Dim clss_approval As ly_APPROVAL.APPROVAL.clss_approval = New APPROVAL.clss_approval(idProgram)
        Dim tbl_DOCS As DataTable = New DataTable
        'tbl_DOCS = clss_approval.get_Doc_support_Route_PendingALL(CType(id_TipoDoc, Integer), CType(IdDoc, Integer))
        clss_approval.get_ta_DocumentosINFO(IdDoc)

        'tbl_DOCS = clss_approval.get_Doc_support_By_Route_Pending(CType(id_TipoDoc, Integer), CType(IdDoc, Integer), clss_approval.get_ta_DocumentosInfoFIELDS("id_ruta", "id_documento", IdDoc))
        tbl_DOCS = clss_approval.get_Doc_support_By_Route_Pending(CType(id_TipoDoc, Integer), CType(IdDoc, Integer), clss_approval.get_ta_DocumentosInfoFIELDS("id_ruta", "id_documento", IdDoc))

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

    Private Sub btn_registrar_tc_Click(sender As Object, e As EventArgs) Handles btn_registrar_tc.Click


        Using db As New dbRMS_JIEntities

            'Me.Response.Redirect("~/Administracion/frm_tasas_cambio")
            '******************************************
            'Here confirm the total amount
            '******************************************
            Dim bndError As Boolean = False
            Me.lblerr_user.Text = ""
            Me.lblerr_user.Visible = False

            If Val(Me.txt_Local_Value.Value) = 0 And Val(Me.txt_total_tasa_cambio.Value) = 0 And Val(Me.txt_USD_val.Value) = 0 Then
                bndError = True
            End If

            If bndError Then
                Me.lblerr_user.Text = "The allocated values, must to be higher than zero."
                Me.lblerr_user.Visible = True
            Else

                Dim coDeliverable As New ta_deliverable
                Dim idDeliverable As Integer = Convert.ToInt32(Me.hd_id_delivered.Value)

                coDeliverable = db.ta_deliverable.Find(Convert.ToInt32(idDeliverable))
                coDeliverable.valor_final = Me.txt_Local_Value.Value
                coDeliverable.tasa_cambio = Me.txt_total_tasa_cambio.Value
                'coDeliverable.fecha_aprobo = Date.UtcNow

                Dim id_programa As Integer = Convert.ToInt32(Me.Session("E_IDPrograma"))
                Dim cls_deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(id_programa)
                Dim result = cls_deliverable.Save_deliverable(coDeliverable, idDeliverable)

                If result <> -1 Then
                    EXECUTE_ACTION(cAPPROVED)
                Else
                    Me.lblerr_user.Text = "Error allocating values, please call the System Administrator."
                    Me.lblerr_user.Visible = True
                End If

            End If

        End Using

    End Sub

    Private Sub btn_add_exchange_Rate_Click(sender As Object, e As EventArgs) Handles btn_add_exchange_Rate.Click

        'Me.Response.Redirect("~/Administracion/frm_tasas_cambio")
        Dim strScript As String = "window.open('~/Administracion/frm_tasas_cambio' ,'_blank');"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Check_Exchange", strScript, True)

    End Sub


End Class
