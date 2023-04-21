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
Imports ly_APPROVAL
Imports ly_SIME
Imports System.Web.Script.Serialization


Partial Class frm_DocAprobacionTest

    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    'Dim cnnContratos As New SqlConnection(ConnectionStrings("Celins_TestConnectionString").ConnectionString)
    'Dim bndUploadNW As Boolean
    'dbCelins_ConnectionString
    'Dim cnnContratos As New SqlConnection(ConnectionStrings("dbCelins_ConnectionString").ConnectionString)

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

    'Protected Overloads Overrides Sub OnInit(ByVal e As EventArgs)
    '    MyBase.OnInit(e)
    '    AddHandler Uploader3.FileUploaded, AddressOf Uploader_FileUploaded
    'End Sub

    'Private Sub Uploader_FileUploaded(ByVal sender As Object, ByVal args As UploaderEventArgs)
    '    Dim uploader As Uploader = DirectCast(sender, Uploader)
    '    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Temp\"
    '    Try

    '        If hd_id_doc.Value > 0 Then

    '            lbl_errExtension.Visible = False

    '            'Dim Random As New Random()
    '            'Dim Aleatorio As Double = Random.Next(1, 999)
    '            Dim dmyhm As String = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
    '            Dim extension As String = System.IO.Path.GetExtension(args.FileName)
    '            Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(args.FileName)
    '            Dim File As String '= "doc" & Me.HiddenField1.Value & "_0" & Me.Session("E_IdUser") & "_" & dmyhm & "_" & fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension
    '            File = String.Format("doc{0}_0{1}_{2}_{3}{4}{5}", Me.HiddenField1.Value, Me.Session("E_IdUser"), dmyhm, fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-"), "_v1.1", extension)
    '            args.CopyTo(sFileDir & File)
    '            Me.lblarchivo.Text = File

    '            '******************************REV 0.0.0.1********************
    '            Dim itemList As New ListItem
    '            itemList.Text = Me.lblarchivo.Text
    '            'itemList.Value = Me.rb_tipoDoc.SelectedValue
    '            itemList.Value = hd_id_doc.Value
    '            Me.ListBox_file.Items.Add(itemList)
    '            'Me.lblarchivo.Text = ""
    '            Me.Panel1_firma.Visible = False
    '            'Me.rb_tipoDoc.Enabled = True
    '            Me.lblMsg.Text = ""
    '            '******************************REV 0.0.0.1*********************

    '            'Me.rb_tipoDoc.DataSource = clss_approval.get_Doc_support_Route_PendingALL(CType(Me.lblTipoDoc.Text, Integer), Me.HiddenField1.Value)
    '            'Me.rb_tipoDoc.DataBind()

    '            grd_documentos.DataSource = clss_approval.get_Doc_support_Route_PendingALL(CType(Me.lblTipoDoc.Text, Integer), Me.HiddenField1.Value)
    '            grd_documentos.DataBind()

    '        Else
    '            lbl_errExtension.Visible = True
    '        End If

    '    Catch ex As Exception
    '        Me.img_btn_borrar_temp.ImageUrl = "../imagenes/Iconos/s_warn.png"
    '        Me.lblarchivo.Text = "Error.."
    '    End Try
    '    Me.Panel1_firma.Visible = True

    'End Sub


    Private Sub uploadFile_FileUploaded(sender As Object, e As FileUploadedEventArgs) Handles uploadFile.FileUploaded

        Dim uploader As Uploader = DirectCast(sender, Uploader)
        Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Temp\"

        Try

            If CType(lbl_hasFiles.Value, Boolean) = True Then

                lbl_errExtension.Visible = False

                Dim dmyhm As String = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
                Dim extension As String = System.IO.Path.GetExtension(e.File.FileName)
                Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(e.File.FileName)
                Dim File As String '= "doc" & Me.HiddenField1.Value & "_0" & Me.Session("E_IdUser") & "_" & dmyhm & "_" & fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension
                File = String.Format("doc{0}_0{1}_{2}_{3}{4}{5}", Me.HiddenField1.Value, Me.Session("E_IdUser"), dmyhm, fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-"), "_v1.1", extension)

            End If

        Catch ex As Exception
            '   Me.img_btn_borrar_temp.ImageUrl = "../imagenes/Iconos/s_warn.png"
            '    Me.lblarchivo.Text = "Error.."
        End Try
        ' Me.Panel1_firma.Visible = True

    End Sub



    <Web.Services.WebMethod()>
    Public Shared Function AddFile(ByVal FileName As String, ByVal idDoc As Integer, ByVal idUser As Integer, ByVal idPrograma As Integer) As Object

        Dim jsonITEMS As String = "[]"

        'Dim uploader As Uploader = DirectCast(sender, Uploader)
        'Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Temp\"


        Dim myArray(1) As Object

        Dim dmyhm As String = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
        Dim extension As String = System.IO.Path.GetExtension(FileName)
        Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(FileName)
        Dim File As String '= "doc" & Me.HiddenField1.Value & "_0" & Me.Session("E_IdUser") & "_" & dmyhm & "_" & fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension
        File = String.Format("doc{0}_0{1}_{2}_{3}{4}{5}", idDoc, idUser, dmyhm, fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-"), "_v1.1", extension)



        '            args.CopyTo(sFileDir & File)
        '            Me.lblarchivo.Text = File

        '            '******************************REV 0.0.0.1********************
        '            Dim itemList As New ListItem
        '            itemList.Text = Me.lblarchivo.Text
        '            'itemList.Value = Me.rb_tipoDoc.SelectedValue
        '            itemList.Value = hd_id_doc.Value
        '            Me.ListBox_file.Items.Add(itemList)
        '            'Me.lblarchivo.Text = ""
        '            Me.Panel1_firma.Visible = False
        '            'Me.rb_tipoDoc.Enabled = True
        '            Me.lblMsg.Text = ""
        '            '******************************REV 0.0.0.1*********************

        '            'Me.rb_tipoDoc.DataSource = clss_approval.get_Doc_support_Route_PendingALL(CType(Me.lblTipoDoc.Text, Integer), Me.HiddenField1.Value)
        '            'Me.rb_tipoDoc.DataBind()

        '            grd_documentos.DataSource = clss_approval.get_Doc_support_Route_PendingALL(CType(Me.lblTipoDoc.Text, Integer), Me.HiddenField1.Value)
        '            grd_documentos.DataBind()



        myArray(0) = New With {Key .Estado = 1, .Mensaje = File}

        Dim serializer As New JavaScriptSerializer()
        jsonITEMS = serializer.Serialize(myArray)

        Return jsonITEMS

    End Function

    'Sub DelFileParam(ByVal archivo As String)
    '    Dim sFileName As String = System.IO.Path.GetFileName(archivo)
    '    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Temp\"
    '    Dim file_info As New IO.FileInfo(sFileDir + sFileName)
    '    If (file_info.Exists) Then
    '        file_info.Delete()
    '    End If
    '    Me.lblarchivo.Text = ""
    '    Me.Panel1_firma.Visible = False
    'End Sub

    'Sub DelFile()
    '    Dim sFileName As String = System.IO.Path.GetFileName(Me.lblarchivo.Text)
    '    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Temp\"
    '    Dim file_info As New IO.FileInfo(sFileDir + sFileName)
    '    If (file_info.Exists) Then
    '        file_info.Delete()
    '    End If
    '    Me.lblarchivo.Text = ""
    '    Me.Panel1_firma.Visible = False
    'End Sub


    '******************************REV 0.0.0.2********************
    Function DelFile(ByVal sFileName As String) As Boolean
        'Dim sFileName As String = System.IO.Path.GetFileName(Me.lblarchivo.Text)
        Dim sFileDir As String = Server.MapPath("~") & "\fileUploads\ApprovalProcc\"
        Dim file_info As New IO.FileInfo(sFileDir + sFileName)
        If (file_info.Exists) Then
            file_info.Delete()
            DelFile = True
        Else
            DelFile = False
        End If
    End Function
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


    'Sub CopyFileParam(ByVal file As String)

    '    Dim sFileName As String = System.IO.Path.GetFileName(file)
    '    Dim sFileDirTemp As String = Server.MapPath("~") & "\FileUploads\Temp\"
    '    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\ApprovalProcc\"
    '    Dim file_info As New IO.FileInfo(sFileDirTemp + sFileName)
    '    Dim extension As String = System.IO.Path.GetExtension(file)
    '    'Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(file)
    '    Try

    '        file_info.CopyTo(sFileDir & sFileName) 'fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension

    '    Catch ex As Exception
    '    End Try
    '    DelFileParam(file)
    '    Me.Panel1_firma.Visible = False
    'End Sub
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
            'If Not cl_user.chk_accessMOD(0, frmCODE) Then
            '    Me.Response.Redirect("~/Administracion/no_access2")
            'Else
            '    cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            '    cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_catalogos)
            'End If
            'controles.code_mod = frmCODE
            'For Each Control As Control In Page.Controls

            '    controles.checkControls(Control, cl_user.id_idioma)
            'Next
        End If

        Dim approve As Boolean = True

        If Not IsPostBack Then

            ' bndUploadNW = False

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            Me.HiddenField1.Value = Me.Request.QueryString("IdDoc").ToString

            clss_approval.get_ta_DocumentosINFO(Me.HiddenField1.Value)

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

            Dim idAppTool As Integer = clss_approval.get_ta_DocumentosInfoFIELDS("id_approval_tool", "id_documento", Me.HiddenField1.Value)

            If idAppTool > 1 Then

                Me.ToolsViewer.Visible = True

                Using db As New dbRMS_JIEntities

                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                    Dim idTimeSheet As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet
                    Me.hrefVIEWER.Attributes.Add("href", "~/TimeSheet/frm_TimeSheetFollowingREP.aspx?ID=" & idTimeSheet)

                End Using

            End If


            Dim tbl_AppOrden As New DataTable

            tbl_AppOrden = clss_approval.get_ta_AppDocumentoAPP_MAX(Me.HiddenField1.Value) 'To get the info on the max step (id_app_doc)

            Dim idEstadoDoc As Integer = tbl_AppOrden.Rows(0).Item("id_estadoDoc")

            '********************************************NOT ALLOWED************************************************************
            ''if the MAX order is in stand by get the lowest order and the last one
            'If idEstadoDoc = cSTANDby Then '************This is not necesary because another step is created********************
            '    tbl_AppOrden = clss_approval.get_ta_AppDocumentoOrden_MIN(Me.HiddenField1.Value) 'To get the info on the min step
            'End If '************This is not necesary because another step is created******************************************
            '********************************************NOT ALLOWED***********************************************************

            Me.lblIDocumento.Text = tbl_AppOrden.Rows(0).Item("id_app_Documento")
            Me.lblTipoDoc.Text = tbl_AppOrden.Rows(0).Item("id_tipoDocumento")
            Me.lbl_aprueba.Text = tbl_AppOrden.Rows(0).Item("nextFase").ToString
            Me.lbl_owner.Text = tbl_AppOrden.Rows(0).Item("nombre_rol").ToString & " [ " & tbl_AppOrden.Rows(0).Item("nombre_empleado").ToString & " ]"
            Me.lblnextruta.Text = tbl_AppOrden.Rows(0)("id_ruta_next").ToString
            Me.lblNextRole.Text = tbl_AppOrden.Rows(0)("id_role_next").ToString
            Me.lblNextUserID.Text = tbl_AppOrden.Rows(0)("id_usuario_next").ToString

            hd_ROL.Value = tbl_AppOrden.Rows(0).Item("nombre_rol").ToString
            Dim idROl = tbl_AppOrden.Rows(0).Item("id_rol")

            Me.grd_archivos.DataSource = clss_approval.get_ta_archivos_documento_ByApp(Me.HiddenField1.Value)
            Me.grd_archivos.DataBind()

            Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(Me.HiddenField1.Value)
            Me.grd_cate.DataBind()

            'Me.rb_tipoDoc.DataSource = clss_approval.get_Doc_support_Route_PendingALL(CType(Me.lblTipoDoc.Text, Integer), Me.HiddenField1.Value)
            'Me.rb_tipoDoc.DataBind()

            grd_documentos.DataSource = clss_approval.get_Doc_support_Route_PendingALL(CType(Me.lblTipoDoc.Text, Integer), Me.HiddenField1.Value)
            grd_documentos.DataBind()

            '******************************REV 0.0.0.1********************
            'Me.rb_tipoDoc.SelectedIndex = 0
            '******************************REV 0.0.0.1********************


            Me.txtcoments.EmptyMessage = tbl_AppOrden.Rows(0).Item("observacion").ToString '.Replace("''", "'")
            '***********************************VALIDAR SI EL USUARIO PUEDE POSTEAR EN ESTE FORMULARIO*****************

            '*****************THE MAX isn´t necesary that woudl be tha max order*******************************************************
            '*****************And (tbl_AppOrden.Rows(0).Item("id_estadoDoc") <> cSTANDby)**********************************************

            '*************************************************ROLES, GROUPS Y SHARED ROLES****************************************************
            '**********************************************************ENABLED THE BUTTONS*****************************************************
            '*********************************************CANCEL BUTTON***************************************************************************
            If clss_approval.get_ta_DocumentosInfoFIELDS("IdOriginador", "id_documento", Me.HiddenField1.Value) = Val(Me.Session("E_IdUser")) Then
                stateButton("btn_Cancelled", True)
                btn_Cancelled.Enabled = True
                stateButton("btn_NotApproved", False)
                btn_NotApproved.Enabled = False
                stateButton("btn_STandBy", False)
                btn_STandBy.Enabled = False
            Else
                stateButton("btn_Cancelled", False)
                btn_Cancelled.Enabled = False
                stateButton("btn_NotApproved", True)
                btn_NotApproved.Enabled = True
                stateButton("btn_STandBy", True)
                btn_STandBy.Enabled = True
            End If
            '*********************************************CANCEL BUTTON***************************************************************************

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
                Me.lblt_addFile.Visible = False

                'Me.Panel1.Visible = False

                Me.btn_remove.Visible = False
                Me.ListBox_file.Visible = False
                Dim column As GridColumn = grd_archivos.MasterTableView.GetColumn("fileControl")
                column.Visible = False
                column.Display = False

                'Me.grd_archivos.Columns(2).Visible = False
                'Me.grd_archivos.Columns(3).Visible = False

            End If

            HttpContext.Current.Session.Add("clss_approval", clss_approval)


            '***********************************************************FILE CONTROL SETTING UP************************************************
            ' Populate the default (base) upload configuration into an object of type SampleAsyncUploadConfiguration
            Dim config As AppFileConfiguration = uploadFile.CreateDefaultUploadConfiguration(Of AppFileConfiguration)()
            ' Populate any additional fields
            config.IDuser = Convert.ToInt32(Me.Session("E_IdUser"))
            config.IDdocument = Convert.ToInt32(Me.HiddenField1.Value)
            config.finalPath = Server.MapPath("~") & "\FileUploads\Temp\"
            ' The upload configuration will be available in the handler
            uploadFile.UploadConfiguration = config
            '***********************************************************FILE CONTROL SETTING UP************************************************

            Me.sourceADRESS.Value = "\FileUploads\Temp\"
            Me.destinationADRESS.Value = "\FileUploads\ApprovalProcc\"

        Else

            '**************************************************************TO EVALUATE THE ACTION TO DO***********************************************************
            'btn_Approved.Attributes.Add("OnClick", “this.text='PROCESSING..';this.disabled=true;" + ClientScript.GetPostBackEventReference(btn_Approved, "").ToString())
            'cmdSave.Attributes.Add(“onclick”, “this.disabled=true;document.getElementById('lblProgress’).innerText=’..PROCESSING..’;” + ClientScript.GetPostBackEventReference(cmdSave, “”).ToString());

            If HttpContext.Current.Session.Item("clss_approval") IsNot Nothing Then
                clss_approval = Me.Session.Item("clss_approval")
            End If

            'btn_Approved.Attributes.Add("CssClass", "CssClass")


        End If


    End Sub


    Public Sub stateButton(ByVal strButton As String, ByVal booState As Boolean)

        Dim script As String = ""
        Dim strButt As String = ""
        Dim strFunc As String = IIf(booState, ".removeClass('disabled')", ".addClass('disabled')")

        '$('#btn_Approved').removeClass('disabled');
        '$('#btn_Approved').addClass('disabled');
        'Dim script As String = "  function f(){$('#btn_Approved').addClass('disabled'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);"

        Select Case strButton
            Case "btn_Cancelled"
                'btn_Cancelled.Enabled = booState
                script = "  function f(){ $('#" + btn_Cancelled.ClientID + "')"
                'script = "  function f(){ var btn = $find(""" + btn_Cancelled.ClientID + """); window.alert(btn); "
                strButt = "btn_Cancelled"
            Case "btn_NotApproved"
                'btn_NotApproved.Enabled = booState
                'script = "  function f(){ var btn = $find(""" + btn_NotApproved.ClientID + """); window.alert(btn); "
                script = "  function f(){ $('#" + btn_NotApproved.ClientID + "')"
                strButt = "btn_NotApproved"
            Case "btn_STandBy"
                'btn_STandBy.Enabled = booState
                'script = "  function f(){ var btn = $find(""" + btn_STandBy.ClientID + """); window.alert(btn); "
                script = "  function f(){ $('#" + btn_STandBy.ClientID + "')"
                strButt = "btn_STandBy"
        End Select

        'Script = "function f(){ $find(""" + rwAlert.ClientID + """)"  '".show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);"
        'script = "function f(){ $('#" & strButt & "')" & strFunc & ";} "
        'script &= "Sys.Application.remove_load(f);  Sys.Application.add_load(f); } "

        script &= strFunc & ";  Sys.Application.remove_load(f); } Sys.Application.add_load(f); "

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, True)

        'Select Case strButton
        '    Case "btn_Cancelled"
        '        btn_Cancelled.Enabled = booState
        '    Case "btn_NotApproved"
        '        btn_NotApproved.Enabled = booState
        '    Case "btn_STandBy"
        '        btn_STandBy.Enabled = booState
        'End Select

    End Sub


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


        'Dim RadAsyncUpload1 As New RadAsyncUpload

        Dim duracion = 0
        Dim Err As Boolean = False

        Dim tbl_rutas_tipo_doc As New DataTable

        Try


            lblerr_user.Text = ""
            '******This has to change for Stand BY app*********************
            Dim idRuta = Me.lblnextruta.Text
            Dim idNextRol = Me.lblNextRole.Text
            Dim idNextUserID = Me.lblNextUserID.Text

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

                        For i = 0 To Me.ListBox_file.Items.Count - 1
                            If CType(Me.ListBox_file.Items(i).Value, Integer) = dtRow("id_doc_soporte") Then
                                Bool_find = True
                            End If
                        Next

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
                    Err = True
                End If

                '*****************SET DOCUMENT REQUIRED BY every approval**************************

            End If



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

                        clss_approval.set_ta_AppDocumento(CType(Me.lblIDocumento.Text, Integer))
                        clss_approval.set_ta_AppDocumentoFIELDS("observacion", Me.txtcoments.Text.Trim, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                        clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                        clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", docState, "id_App_documento", clss_approval.id_App_Documento)
                        clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                        'clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", , "id_app_documento", clss_approval.id_App_Documento) 'Add the Actual Role it is necesary?


                        If clss_approval.save_ta_AppDocumento() <> -1 Then


                            Save_NewFiles(CType(Me.lblIDocumento.Text, Integer))


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

                                    '*******************************TOOLS TIME SHEET**********************************

                                    If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then

                                        'Update Here the Time Sheet Status

                                        Using db As New dbRMS_JIEntities

                                            Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                            Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                            Dim TimeSheet As New ta_timesheet

                                            TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                            TimeSheet.fecha_upd = Date.UtcNow
                                            TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                            TimeSheet.id_timesheet_estado = 3 'Approved

                                            Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                            Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                            If result <> -1 Then

                                                '*********************************OPEN****************************************
                                                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                                If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                                Else 'Error mandando Email
                                                End If
                                                '*********************************OPEN****************************************

                                            End If

                                        End Using

                                    Else

                                        '***************Ver la categoria y ver si aplica registro ambiental**********************

                                        '********************************COMPLETED DOCUMENT*********************************************************
                                        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                        If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                        Else 'Error mandando Email
                                        End If
                                        ' ********************************COMPLETED DOCUMENT*********************************************************


                                    End If

                                    '*******************************TOOLS TIME SHEET**********************************





                                Else 'Error Saving Docs


                                End If

                            Else 'Yes there is more steps


                                Dim fecha_recep As DateTime = Date.UtcNow 'DateAdd(DateInterval.Hour, 8, Date.UtcNow)
                                Dim fecha_limit As DateTime = calculaDiaHabil(duracion, fecha_recep)

                                clss_approval.set_ta_AppDocumento(0) 'New Record
                                clss_approval.set_ta_AppDocumentoFIELDS("id_documento", Me.HiddenField1.Value, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_recep, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cPENDING, "id_app_documento", 0) 'Pending Step
                                clss_approval.set_ta_AppDocumentoFIELDS("observacion", Me.txtcoments.Text.Trim, "id_app_documento", 0) 'Pending Step .Replace("'", "''")
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


                                '*******************************TOOLS TIME SHEET**********************************

                                If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then

                                    '*********************************OPEN****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    Else 'Error mandando Email
                                    End If
                                    '*********************************OPEN****************************************

                                Else

                                    '*********************************APPROVED NEXT STEP****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then

                                    Else 'Error mandando Email
                                    End If
                                    '*********************************APPROVED NEXT STEP****************************************

                                End If

                            End If

                        Else 'Error saving the estep
                        End If 'clss_approval.save_ta_AppDocumento()

                 '****************************************END APPROVED***************************************************

           '****************NO APROBADO ahí se termina el proceso**************************
           '**************************ACTUALIZAR EL REGISTRO QUE SE ESTA EDITANDO***********

                    Case cnotAPPROVED


                        clss_approval.set_ta_AppDocumento(CType(Me.lblIDocumento.Text, Integer))
                        clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                        clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                        clss_approval.set_ta_AppDocumentoFIELDS("observacion", Me.txtcoments.Text.Trim, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                        clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cnotAPPROVED, "id_App_documento", clss_approval.id_App_Documento)

                        If clss_approval.save_ta_AppDocumento() <> -1 Then

                            clss_approval.set_ta_documento(Me.HiddenField1.Value)
                            clss_approval.set_ta_documentoFIELDS("completo", "SI", "id_documento", clss_approval.id_Documento)

                            If clss_approval.save_ta_documento() <> -1 Then

                                SaveComment(Me.lblIDocumento.Text, cnotAPPROVED, Me.txtcoments.Text.Trim) '.Replace("'", "''")

                                Save_NewFiles(CType(Me.lblIDocumento.Text, Integer))

                                If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then

                                    'Update Here the Time Sheet Status
                                    Using db As New dbRMS_JIEntities

                                        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                        Dim TimeSheet As New ta_timesheet

                                        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                        TimeSheet.fecha_upd = Date.UtcNow
                                        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                        TimeSheet.id_timesheet_estado = 4 ' Not Approved

                                        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                        If result <> -1 Then

                                            '*********************************OPEN****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************OPEN****************************************

                                        End If

                                    End Using

                                Else

                                    '*********************************NOT APPROVED ****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    Else 'Error mandando Email
                                    End If
                                    '*********************************NOT APPROVED ****************************************
                                End If



                            Else 'Error
                            End If


                        Else 'Error happened
                        End If

                        '****************************************FIN***************************************************

                         '********************CANCELADO FIN DEL DOCUMENTO (ACTUALIZAR campo complet a SI EN ta_documentos )
                         '****************************ACTUALIZAR EL REGISTRO QUE SE ESTA EDITANDO***********
                    Case cCANCELLED

                        clss_approval.set_ta_AppDocumento(CType(Me.lblIDocumento.Text, Integer))
                        clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                        clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                        clss_approval.set_ta_AppDocumentoFIELDS("observacion", Me.txtcoments.Text.Trim, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                        clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cCANCELLED, "id_App_documento", clss_approval.id_App_Documento)

                        If clss_approval.save_ta_AppDocumento() <> -1 Then

                            clss_approval.set_ta_documento(Me.HiddenField1.Value)
                            clss_approval.set_ta_documentoFIELDS("completo", "SI", "id_documento", clss_approval.id_Documento)

                            If clss_approval.save_ta_documento() <> -1 Then

                                SaveComment(Me.lblIDocumento.Text, cCANCELLED, Me.txtcoments.Text.Trim) '.Replace("'", "''")
                                Save_NewFiles(CType(Me.lblIDocumento.Text, Integer))


                                If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then
                                    'Update Here the Time Sheet Status

                                    Using db As New dbRMS_JIEntities

                                        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                        Dim TimeSheet As New ta_timesheet

                                        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                        TimeSheet.fecha_upd = Date.UtcNow
                                        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                        TimeSheet.id_timesheet_estado = 4 ' Not Approved

                                        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                        If result <> -1 Then

                                            '*********************************OPEN****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************OPEN****************************************

                                        End If

                                    End Using

                                Else

                                    '*********************************CANCELLED ****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    Else 'Error mandando Email
                                    End If
                                    '*********************************CANCELLED****************************************
                                End If


                            Else 'Error
                            End If


                        Else  'Error
                        End If


                    Case cSTANDby
                        '*********************************************************************************

                        clss_approval.set_ta_AppDocumento(CType(Me.lblIDocumento.Text, Integer))
                        clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                        clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                        clss_approval.set_ta_AppDocumentoFIELDS("observacion", Me.txtcoments.Text.Trim, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                        clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cSTANDby, "id_App_documento", clss_approval.id_App_Documento)

                        If clss_approval.save_ta_AppDocumento() <> -1 Then


                            SaveComment(Me.lblIDocumento.Text, cSTANDby, Me.txtcoments.Text.Trim) '.Replace("'", "''")
                            Save_NewFiles(CType(Me.lblIDocumento.Text, Integer))


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

                            clss_approval.set_ta_AppDocumento(0) 'New Record in stanb By
                            clss_approval.set_ta_AppDocumentoFIELDS("id_documento", Me.HiddenField1.Value, "id_app_documento", 0)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0) 'IdRutaOriginator
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_recep, "id_app_documento", 0)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cSTANDby, "id_app_documento", 0) 'Pending Step
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", Me.txtcoments.Text.Trim, "id_app_documento", 0) 'Pending Step '.Replace("'", "''")
                            'clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_recep, "id_app_documento", 0)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", idNextUserID, "id_app_documento", 0) 'IdUSerORiginator
                            clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) ' idrolORiginator

                            Dim id_appdocumento = clss_approval.save_ta_AppDocumento()

                            If id_appdocumento <> -1 Then

                                If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then

                                    'Update Here the Time Sheet Status
                                    '*********************************OPEN****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    Else 'Error mandando Email
                                    End If
                                    '*********************************OPEN****************************************

                                Else

                                    '*********************************STAND BY****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    Else 'Error mandando Email
                                    End If
                                    '*********************************STAND BY****************************************

                                End If

                            End If

                        Else 'Error
                        End If


                End Select

                Me.Response.Redirect("~/Approvals/frm_consulta_docsPending.aspx")


            End If

        Catch ex As Exception
            lblerr_user.Text = String.Format("An error was found in the action: {0} ", ex.Message)
            Me.btn_Approved.Enabled = False
            Me.btn_NotApproved.Enabled = False
            Me.btn_Cancelled.Enabled = False
            Me.btn_STandBy.Enabled = False
            Me.btn_Completed.Enabled = False

        End Try



    End Sub


    'Protected Sub btn_approved_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_approved.Click

    '    'Me.Response.Redirect("~/approvals/frm_consulta_docsPending.aspx")

    'End Sub


    Private Sub Save_NewFiles(ByVal id_app_doc As Integer)

        '****************************************************************************************************
        '**************************DOCUMENTOS ADICIONALES EN EL VERSION ACTUAL******************************
        Dim listItemFile As New ListItem
        For i = 0 To Me.ListBox_file.Items.Count - 1

            clss_approval.set_ta_archivos_documento(0) 'New Record
            clss_approval.set_ta_archivos_documentoFIELDS("id_App_Documento", id_app_doc, "id_archivo", 0)
            clss_approval.set_ta_archivos_documentoFIELDS("archivo", Me.ListBox_file.Items(i).Text, "id_archivo", 0)
            clss_approval.set_ta_archivos_documentoFIELDS("id_doc_soporte", CType(Me.ListBox_file.Items(i).Value, Integer), "id_archivo", 0)
            clss_approval.set_ta_archivos_documentoFIELDS("ver", 1, "id_archivo", 0)

            If clss_approval.save_ta_archivos_documento() <> -1 Then 'Erro Happenned
                '*******************HERE THE CHANGE********************************
                ' CopyFileParam(Me.ListBox_file.Items(i).Text)
                '*******************HERE THE CHANGE********************************
            End If

        Next
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

        strNW_name = strName.Substring(posF + 1, strName.Length - posF - 1)

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

    Protected Sub btn_remove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_remove.Click
        '**********************HERE THE CHANGE*********************
        ' DelFileParam(Me.ListBox_file.SelectedItem.Text)
        '**********************HERE THE CHANGE*********************
        Me.ListBox_file.Items.Remove(Me.ListBox_file.SelectedItem)
    End Sub

    Protected Sub btn_Approved_Click(sender As Object, e As EventArgs) Handles btn_Approved.Click
        EXECUTE_ACTION(cAPPROVED)
        'btn_Approved.Enabled = True
    End Sub

    Protected Sub btn_Completed_Click(sender As Object, e As EventArgs) Handles btn_Completed.Click
        EXECUTE_ACTION(cAPPROVED)
    End Sub

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

    End Sub

    Sub ActualizaDatosDOCS()

        For Each Irow As GridDataItem In Me.grd_documentos.Items

            Dim chkvisible As CheckBox = CType(Irow("colm_select").FindControl("chkSelect"), CheckBox)

            If chkvisible.Checked = True Then

                If Irow("id_doc_soporte").Text = hd_id_doc.Value Then
                    'Me.Uploader3.ValidateOption.AllowedFileExtensions = Irow("extension").Text
                Else
                    chkvisible.Checked = False
                End If

                'Dim Sql = " INSERT INTO ta_aprobacion_docs_temp (id_sesion_temp,  id_doc_soporte, id_programa) VALUES ('" & Me.lbl_id_sesion_temp.Text & "'," & Irow("id_doc_soporte").Text & ", " & Me.Session("E_IDPrograma") & ") "
                'cnnSAP.Open()
                'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
                'Dim ds As New DataSet("IdPlan")
                'dm.Fill(ds, "IdPlan")
                'cnnSAP.Close()

            End If
        Next

        'grd_documentos.DataSource = cl_AppDef.get_DocumentTypesFROM_tmp(Me.lbl_id_sesion_temp.Text)
        'grd_documentos.DataBind()
        'Me.grd_cate.DataBind()

    End Sub


End Class
