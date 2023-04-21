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


'******************************************************REV****************************************************************************
'*************************************************************************************************************************************
'***********  Ver:    0.0.0.1                                                                                   **********************
'***********  Dated:  25/02/2014  11:00 a.m.                                                                    **********************
'***********  Author: Ing. Gustavo Rivera                                                                       **********************
'***********  Description: Se le quito la función de "No Replace", debido a que genera ciertas confusiones en   **********************
'***********  la forma de procesar y da na facilidad de no integridad y consistencia de información, debido a   **********************
'***********  que al seleccionarla, se sube un archivo de el mismo de nombre X, pero el registro de archivo no  **********************
'***********  no se actualiza en la base de datos, entonces no hay concistencia y da lugar a no tener la ultima **********************
'***********  version de los archivos.                                                                           **********************   
'***********  Se modifico totalmente el evento btn_approved_Click, de tal forma que no cambiamos ahi los registros**********************
'***********  de los archivos modificados y se hizo directamente desde la función  FileUploaded_Chg_Name que dispará**********************
'***********  en el momento que los archivos son subidos para su modificación, adicional se agrego la notación  **********************
'***********  de nombres unicos par alos archivos que se actualizan GeTfIleName igual que los nuevos, y también **********************
'***********  si se suben completamente se eliminan las versiones anteriores con la funcion Sub DelFile().      **********************
'*************************************************************************************************************************************
'*************************************************************************************************************************************


Partial Class frm_seguimientoAprobacion
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login.aspx")
        End Try

        If Not IsPostBack Then
            Me.lblIDocumento.Text = Me.Request.QueryString("IdDoc").ToString
            Dim Sql = "SELECT * FROM vw_ta_documentos WHERE id_documento=" & Me.lblIDocumento.Text
            Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds1 As New DataSet("proyecto")
            dm1.Fill(ds1, "proyecto")
            Me.lbl_categoria.Text = ds1.Tables("proyecto")(0)("descripcion_cat").ToString
            Me.lbl_aprobacion.Text = ds1.Tables("proyecto")(0)("descripcion_aprobacion").ToString
            Me.lbl_nivelaprobacion.Text = ds1.Tables("proyecto")(0)("nivel_aprobacion").ToString
            Me.lbl_condicion.Text = ds1.Tables("proyecto")(0)("condicion").ToString
            Me.lbl_proceso.Text = ds1.Tables("proyecto")(0)("descripcion_doc").ToString
            Me.lbl_codigo.Text = ds1.Tables("proyecto")(0)("codigo_AID").ToString
            Me.lbl_instrumento.Text = ds1.Tables("proyecto")(0)("numero_instrumento").ToString
            Me.lbl_beneficiario.Text = ds1.Tables("proyecto")(0)("nom_beneficiario").ToString
            Me.lbl_Comment.Text = ds1.Tables("proyecto")(0)("comentarios").ToString
            Me.SqlDataSource2.SelectCommand = "SELECT * FROM dbo.FN_Ta_RutaSeguimiento(" & Me.lblIDocumento.Text & ")"

            Me.lbl_datecreated.Text = ds1.Tables("proyecto")(0)("datecreated").ToString.Substring(0, 10)
            Me.lbl_IdCodigoAPP.Text = ds1.Tables("proyecto")(0)("codigo_Approval").ToString
            Me.lbl_IdCodigoSAP.Text = ds1.Tables("proyecto")(0)("codigo_SAP_APP").ToString
        End If
        fill(Me.lbl_datecreated.Text)
    End Sub

    Protected Sub grd_cate_EditCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.EditCommand

        ' If (e.Item.IsInEditMode) Then

        Dim item As GridEditableItem = e.Item
        'Dim id_Estado_Doc As Integer = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_EstadoDoc").ToString()
        'Dim id_App As Integer = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_App_Documento").ToString()
        Dim id_Estado_Doc As Integer = item("id_EstadoDoc").Text
        Dim id_App As Integer = item("id_App_Documento").Text
        Dim strOBserv As String = item("Observacion").Text
        ' lblMsg.Text = "Notificar Emailing enviarEmail(" & Me.lblIDocumento.Text & ", " & id_App.ToString & ", " & id_Estado_Doc.ToString & ")"

        'End If

        '***********************REV 0.0.0.1******************************************
       
        If id_Estado_Doc = 1 Then
            lblMsg.Text = " The notification cannot resend, invalid state ""PENDING"" "
        ElseIf id_Estado_Doc = 7 Then
            enviar_emailAprobacion(Me.lblIDocumento.Text, id_App, id_Estado_Doc, strOBserv)
        Else
            enviar_email(Me.lblIDocumento.Text, id_App, id_Estado_Doc, strOBserv)
        End If
        '***********************REV 0.0.0.1******************************************


    End Sub


    Sub enviar_emailAprobacion(ByVal id_doc As Integer, ByVal id_appdoc As Integer, ByVal idEstado As Integer, ByVal ObservacionTXT As String)
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
        MensajeSend.Subject = "Document approved"

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
        'Mensaje &= "<div align='righ'><b>Phase of path: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000' >" & dsap.Tables("appdoc").Rows(0).Item("nextFase") & "</th></tr>"

        'Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #FFFFFF' >"
        'Mensaje &= "<div align='righ'><b>Status of process: </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF'  >" & dsap.Tables("appdoc").Rows(0).Item("descripcion_estado") & "</th></tr>"

        'Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #000000' >"
        'Mensaje &= "<div align='righ'><b>Date of receipt  : </b></div></th><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000'  >" & dsap.Tables("appdoc").Rows(0).Item("fecha_recepcion") & "</th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #FFFFFF' >"
        'Mensaje &= "<div align='righ'><b>Link: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF'  ><a href='" & url_aplicacion & "/Aprobaciones/frm_DocAprobacion.aspx?IdDoc=" & dsap.Tables("appdoc").Rows(0).Item("id_documento") & "'> View link <a/></th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #000000'>"
        'Mensaje &= "<div align='righ'><b>Comments</b></div></th><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000'>" & Me.txtcoments.Text & "</th></tr></table>"

        'Mensaje &= " <p><strong>This is an automatically generated email, please do not reply</strong><br />"
        'Mensaje &= " <strong>WARNING</strong>: <em>This message contains  information legally protected. If you have received this message by  mistake, please avoid checking, distributing, copying, reproduction or misuse  of the information within it and reply to the sender informing about the  misunderstanding.</em></p>"

        'Mensaje &= "</body></html>"
        ''*****************************************FIN******************************************
        '*****************************TABLA DE ESTADOS ******************************** 
        Dim dsP As New DataSet("dtPROCS")
        dsP.Tables.Add("dtPROCS")
        sql = "SELECT * FROM dbo.FN_Ta_RutaSeguimiento(" & id_doc & ") ORDER BY ORDEN"
        dm.SelectCommand.CommandText = sql
        dm.Fill(dsP, "dtPROCS")
        Dim strTable = makeTable(dsP.Tables("dtPROCS"), dsap.Tables("appdoc").Rows(0).Item("descripcion_estado"), id_appdoc)
        '*****************************TABLA DE ESTADOS ******************************** 

        dsP.Tables.Add("dtUSER")
        sql = "select * from t_empleados where id_empleado = " & Me.Session("E_IdUser")
        dm.SelectCommand.CommandText = sql
        dm.Fill(dsP, "dtUSER")
        Dim UsuarioName As String = String.Format("{0} {1}", Trim(dsP.Tables("dtUSER").Rows(0).Item("empleado_nombre")), Trim(dsP.Tables("dtUSER").Rows(0).Item("apellidos")))

        Dim nullable As System.Text.Encoding
        Dim Mensaje = Make_Email2(dsap.Tables("appdoc"), url_aplicacion, strTable, UsuarioName)
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


    Sub enviar_email(ByVal id_doc As Integer, ByVal id_appdoc As Integer, ByVal idEstado As Integer, ByVal ObservacionTXT As String)

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

        'MensajeSend.Bcc.Add(destinatarioAdmin)
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
        'Mensaje &= "<div align='righ'><b>Link: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF' ><a href='" & url_aplicacion & "/Aprobaciones/frm_DocAprobacion.aspx?IdDoc=" & dsap.Tables("appdoc").Rows(0).Item("id_documento") & "'> View link <a/></th></tr>"

        'Mensaje &= "<tr><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #000000'>"
        'Mensaje &= "<div align='righ'><b>Comments</b></div></th><th  bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000'>" & Trim(ObservacionTXT) & "</th></tr></table>"

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

        dsP.Tables.Add("dtUSER")
        sql = "select * from t_empleados where id_empleado = " & Me.Session("E_IdUser")
        dm.SelectCommand.CommandText = sql
        dm.Fill(dsP, "dtUSER")
        Dim UsuarioName As String = String.Format("{0} {1}", Trim(dsP.Tables("dtUSER").Rows(0).Item("empleado_nombre")), Trim(dsP.Tables("dtUSER").Rows(0).Item("apellidos")))

        Dim strTable = makeTable(dsP.Tables("dtPROCS"), dsap.Tables("appdoc").Rows(0).Item("descripcion_estado"), id_appdoc)
        '*****************************TABLA DE ESTADOS ******************************** 

        Dim nullable As System.Text.Encoding
        Dim Mensaje = Make_Email(dsap.Tables("appdoc"), url_aplicacion, strTable, UsuarioName)
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





    Public Function Make_Email(ByVal dtData As DataTable, ByVal url_aplicacion As String, ByVal strTABLE As String, ByVal strUSer As String) As String

        Dim strHTMLemail As String
        Dim FechaRec As Date
        Dim FechaLim As Date
        Dim format As String = "ddd d MMMM yyyy HH:mm"
        '' dtData.Rows(0).Item("descripcion_estado") & "   </td>" & _

        FechaRec = dtData.Rows(0).Item("fecha_recepcion")

        FechaLim = dtData.Rows(0).Item("fecha_limite")


        '  "  <img alt='Chermonics Inc.' src='http://www.colombiaresponde-ns.org/approval/Imagenes/logos/Chemonics-log150.png' style='width:150px; height:150px; border:0px;'   />" & _

        strHTMLemail = "<html xmlns='http://www.w3.org/1999/xhtml' >" & _
        "<head>" & _
                "</head>" & _
                  "       <body>" & _
                   "          <div>" & _
                     "           <p style='font-family: Arial, Helvetica, sans-serif; font-size: small;'>" & _
                      "            This message has been sent from the Results Management System CHEMONICS  (email sent by " & strUSer & " )<br /><br />" & _
                       "         </p>" & _
                        "     </div>" & _
                         "    <table border='1' cellpadding='0' cellspacing='0' style='width:100%; border-color:#FFFFFF;'>" & _
                          "       <tr>" & _
                           "          <td colspan ='2' >" & _
                           "                 <table style='border-color: #000000; width:100%;border-color:#FFFFFF;'>" & _
                           "                       <tr >" & _
                            "                          <td  style='padding-left: 6px; width:10%; vertical-align:middle; text-align:center;width:120px; height:120px;' > " & _
                            "                            <div id='logo1' style=' vertical-align:middle' >" & _
                             "                              <img alt='\' hspace='0' src='cid:LogCHERM' align='baseline' style='width:120px; height:120px; border:0px;' />" & _
                             "                           </div> " & _
                             "                         </td>" & _
                             "                          <td colspan='2' style='padding: 50px; text-align:left; font-family:Arial Unicode MS; font-size: medium; vertical-align:bottom; padding-bottom:5px; font-weight:bolder; width:90%; text-transform:uppercase '>" & _
                             dtData.Rows(0).Item("nombre_proyecto") & _
                             "                          </td>" & _
                            "                              </tr>" & _
                            "                  </table><br />" & _
                           "           </td>" & _
                          "       </tr>" & _
                          "        <tr style=' height:10px;'>" & _
                           "          <td colspan='2' style=' height:10px; background-color:#ED7620; border-color:#ED7620' >" & _
                            "         </td>" & _
                            "     </tr>" & _
                            "     <tr>" & _
                            "      <td colspan='2'>" & _
                            "     <table style='width=100%;'>" & _
                             "      <tr>" & _
                             "       <td style='padding: 3px; border-color:#CECECE; background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                               "          NAME OF CATEGORY: " & _
                                "     </td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                                   dtData.Rows(0).Item("descripcion_cat") & _
                                 "    </td>" & _
                                " </tr>" & _
                                " <tr>" & _
                                 "    <td style='padding: 3px; border-color:#CECECE; background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                                 "       APPROVAL:</td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("descripcion_aprobacion") & "  </td>" & _
                                " </tr>" & _
                                " <tr>" & _
                                "     <td style='padding: 3px; border-color:#CECECE; background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                                "        INSTRUMENT NUMBER:</td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("numero_instrumento") & "  </td>" & _
                                " </tr>" & _
                                " <tr>" & _
                                "     <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                                "        BENEFICIARY NAME:</td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("nom_beneficiario") & "  </td>" & _
                                " </tr>" & _
                               "  <tr>" & _
                                "     <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                                 "        NAME OF PROCESS:</td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("descripcion_doc") & "  </td>" & _
                               "  </tr>" & _
                                " <tr>" & _
                                 "    <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                                  "      PROCESS CODE:</td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("codigo_AID") & "   </td>" & _
                               "  </tr>" & _
                               "  <tr>" & _
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                               "          CODE SAP / APPROVAL:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("codigo_SAP_APP") & _
                                "         </td>" & _
                               "  </tr>" & _
                               "   <tr>" & _
                               "      <td style='padding: 3px;border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                                "         REPORT CREATED BY:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                             dtData.Rows(0).Item("nombre_empleado") & "   </td>" & _
                               "  </tr>" & _
                               "   <tr>" & _
                               "      <td style='padding: 3px;border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                               "         POSITION:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                             dtData.Rows(0).Item("nombre_rol") & "   </td>" & _
                               "  </tr>" & _
                               "  <tr>" & _
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'   >" & _
                               "         NEXT PHASE OF PATH:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("nextFase") & "   </td>" & _
                               "  </tr>" & _
                               "  <tr>" & _
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                                "        DATE OF RECEIPT:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                                FechaRec.ToString(format) & "   </td>" & _
                                " </tr>" & _
                                  "  <tr>" & _
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                                "        DEADLINE:</td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                                FechaLim.ToString(format) & "   </td>" & _
                                " </tr>" & _
                               "  <tr>" & _
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                               "         STATUS PROCESS:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              strTABLE & "   </td>" & _
                               "  </tr>" & _
                               "  <tr>" & _
                                  "   <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                                  "      LINK:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                                   "      <a href='" & url_aplicacion & "/Aprobaciones/frm_DocAprobacion.aspx?IdDoc=" & _
                                dtData.Rows(0).Item("id_documento") & "' style=' text-decoration:none; font-family:Verdana, Arial;' > View link <a/> " & _
                                   "      </td>" & _
                                 "</tr>" & _
                                " <tr>" & _
                                 "    <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                                 " COMMENTS:" & _
                                  "   </td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                                dtData.Rows(0).Item("Observacion") & _
                                  "   </td>" & _
                               "  </tr>" & _
                              " </table>" & _
                               "  </td>" & _
                              "   </tr>" & _
                              "    <tr style=' height:10px;'>" & _
                               "      <td colspan='2' style=' height:10px; background-color:#ED7620; border-color:#ED7620' >" & _
                                "     </td>" & _
                               "  </tr>" & _
                                "  </table>" & _
                                " <br /><br />" & _
                               " <table>" & _
                                " <tr>" & _
                                "     <td colspan='2' style='border-color:#CCCCCC;background-color: #CCCCCC;'>" & _
                                 "        <table border='1' cellpadding='0' cellspacing='0' style='mso-cellspacing: 0cm; mso-border-alt: outset #000033 .75pt; mso-yfti-tbllook: 1184; mso-padding-alt: 0cm 0cm 0cm 0cm; font-size: 10.0pt; font-family: 'Times New Roman', serif;border: 1.0pt outset #000033;'>" & _
                                  "           <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;mso-yfti-lastrow:yes'>" & _
                                   "              <td style='border: inset white 1.0pt; mso-border-alt: inset white .75pt;  padding: 0px 0px 0px 6px'>" & _
                                    "                 <p>" & _
                                      "                    &nbsp;</p>" & _
                                      "               <p>" & _
                                       "                  <b><strong>This is an automatically generated email, please do not reply.</strong></b></p>" & _
                                        "             <p>" & _
                                        "                 <br />" & _
                                       "                  <strong>WARNING</strong>: <em>This message contains information legally " & _
                                       "                  protected. If you have received this message by mistake, please avoid checking, " & _
                                       "                  distributing, copying, reproduction or misuse of the information within it and " & _
                                       "                  reply to the sender informing about the misunderstanding.</em></p>" & _
                                       "              <p>" & _
                                       "                  &nbsp;</p>" & _
                                       "          </td>" & _
                                       "      </tr>" & _
                                       "  </table>" & _
                                   "  </td>" & _
                                " </tr>" & _
                             "</table>" & _
                           "  </body>" & _
                      "   </html>"




        Return strHTMLemail

    End Function



    Public Function Make_Email2(dtData As DataTable, ByVal url_aplicacion As String, ByVal strTABLE As String, ByVal strUSer As String) As String

        Dim strHTMLemail As String
        Dim FechaRec As Date
        Dim FechaLim As Date
        Dim format As String = "ddd d MMMM yyyy HH:mm"
        '' dtData.Rows(0).Item("descripcion_estado") & "   </td>" & _

        FechaRec = dtData.Rows(0).Item("fecha_recepcion")

        FechaLim = dtData.Rows(0).Item("fecha_limite")


        '  "  <img alt='Chermonics Inc.' src='http://www.colombiaresponde-ns.org/approval/Imagenes/logos/Chemonics-log150.png' style='width:150px; height:150px; border:0px;'   />" & _

        strHTMLemail = "<html xmlns='http://www.w3.org/1999/xhtml' >" & _
        "<head>" & _
                "</head>" & _
                  "       <body>" & _
                   "          <div>" & _
                     "           <p style='font-family: Arial, Helvetica, sans-serif; font-size: small;'>" & _
                      "            This message has been sent from the Results Management System CHEMONICS  (email sent by " & strUSer & " )<br /><br />" & _
                       "         </p>" & _
                        "     </div>" & _
                         "    <table border='1' cellpadding='0' cellspacing='0' style='width:100%; border-color:#FFFFFF;'>" & _
                          "       <tr>" & _
                           "          <td colspan ='2' >" & _
                           "                 <table style='border-color: #000000; width:100%;border-color:#FFFFFF;'>" & _
                           "                       <tr >" & _
                            "                          <td  style='padding-left: 6px; width:10%; vertical-align:middle; text-align:center;width:120px; height:120px;' > " & _
                            "                            <div id='logo1' style=' vertical-align:middle' >" & _
                             "                              <img alt='\' hspace='0' src='cid:LogCHERM' align='baseline' style='width:120px; height:120px; border:0px;' />" & _
                             "                           </div> " & _
                             "                         </td>" & _
                             "                          <td colspan='2' style='padding: 50px; text-align:left; font-family:Arial Unicode MS; font-size: medium; vertical-align:bottom; padding-bottom:5px; font-weight:bolder; width:90%; text-transform:uppercase '>" & _
                             dtData.Rows(0).Item("nombre_proyecto") & _
                             "                          </td>" & _
                            "                              </tr>" & _
                            "                  </table><br />" & _
                           "           </td>" & _
                          "       </tr>" & _
                          "        <tr style=' height:10px;'>" & _
                           "          <td colspan='2' style=' height:10px; background-color:#ED7620; border-color:#ED7620' >" & _
                            "         </td>" & _
                            "     </tr>" & _
                            "     <tr>" & _
                            "      <td colspan='2'>" & _
                            "     <table style='width=100%;'>" & _
                             "      <tr>" & _
                             "       <td style='padding: 3px; border-color:#CECECE; background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                               "          NAME OF CATEGORY: " & _
                                "     </td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                                   dtData.Rows(0).Item("descripcion_cat") & _
                                 "    </td>" & _
                                " </tr>" & _
                                " <tr>" & _
                                 "    <td style='padding: 3px; border-color:#CECECE; background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                                 "       APPROVAL:</td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("descripcion_aprobacion") & "  </td>" & _
                                " </tr>" & _
                                " <tr>" & _
                                "     <td style='padding: 3px; border-color:#CECECE; background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                                "        INSTRUMENT NUMBER:</td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("numero_instrumento") & "  </td>" & _
                                " </tr>" & _
                                " <tr>" & _
                                "     <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                                "        BENEFICIARY NAME:</td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("nom_beneficiario") & "  </td>" & _
                                " </tr>" & _
                               "  <tr>" & _
                                "     <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                                 "        NAME OF PROCESS:</td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("descripcion_doc") & "  </td>" & _
                               "  </tr>" & _
                                " <tr>" & _
                                 "    <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                                  "      PROCESS CODE:</td>" & _
                                " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("codigo_AID") & "   </td>" & _
                               "  </tr>" & _
                               "  <tr>" & _
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;' >" & _
                               "          CODE SAP / APPROVAL:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("codigo_SAP_APP") & _
                                "         </td>" & _
                               "  </tr>" & _
                               "   <tr>" & _
                               "      <td style='padding: 3px;border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                                "         REPORT CREATED BY:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                             dtData.Rows(0).Item("nombre_empleado") & "   </td>" & _
                               "  </tr>" & _
                               "  <tr>" & _
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'   >" & _
                               "         PHASE OF PATH:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              dtData.Rows(0).Item("nextFase") & "   </td>" & _
                               "  </tr>" & _
                               "  <tr>" & _
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                                "        DATE OF RECEIPT:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                                FechaRec.ToString(format) & "   </td>" & _
                                " </tr>" & _
                               "  <tr>" & _
                               "      <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                               "         STATUS PROCESS:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                              strTABLE & "   </td>" & _
                               "  </tr>" & _
                               "  <tr>" & _
                                  "   <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                                  "      LINK:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                                   "      <a href='" & url_aplicacion & "/Aprobaciones/frm_DocAprobacion.aspx?IdDoc=" & _
                                dtData.Rows(0).Item("id_documento") & "' style=' text-decoration:none; font-family:Verdana, Arial;' > View link <a/> " & _
                                   "      </td>" & _
                                 "</tr>" & _
                                " <tr>" & _
                                 "    <td style='padding: 3px;  border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                                 " COMMENTS:" & _
                                  "   </td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                                dtData.Rows(0).Item("Observacion") & _
                                  "   </td>" & _
                               "  </tr>" & _
                              " </table>" & _
                               "  </td>" & _
                              "   </tr>" & _
                              "    <tr style=' height:10px;'>" & _
                               "      <td colspan='2' style=' height:10px; background-color:#ED7620; border-color:#ED7620' >" & _
                                "     </td>" & _
                               "  </tr>" & _
                                "  </table>" & _
                                " <br /><br />" & _
                               " <table>" & _
                                " <tr>" & _
                                "     <td colspan='2' style='border-color:#CCCCCC;background-color: #CCCCCC;'>" & _
                                 "        <table border='1' cellpadding='0' cellspacing='0' style='mso-cellspacing: 0cm; mso-border-alt: outset #000033 .75pt; mso-yfti-tbllook: 1184; mso-padding-alt: 0cm 0cm 0cm 0cm; font-size: 10.0pt; font-family: 'Times New Roman', serif;border: 1.0pt outset #000033;'>" & _
                                  "           <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;mso-yfti-lastrow:yes'>" & _
                                   "              <td style='border: inset white 1.0pt; mso-border-alt: inset white .75pt;  padding: 0px 0px 0px 6px'>" & _
                                    "                 <p>" & _
                                      "                    &nbsp;</p>" & _
                                      "               <p>" & _
                                       "                  <b><strong>This is an automatically generated email, please do not reply.</strong></b></p>" & _
                                        "             <p>" & _
                                        "                 <br />" & _
                                       "                  <strong>WARNING</strong>: <em>This message contains information legally " & _
                                       "                  protected. If you have received this message by mistake, please avoid checking, " & _
                                       "                  distributing, copying, reproduction or misuse of the information within it and " & _
                                       "                  reply to the sender informing about the misunderstanding.</em></p>" & _
                                       "              <p>" & _
                                       "                  &nbsp;</p>" & _
                                       "          </td>" & _
                                       "      </tr>" & _
                                       "  </table>" & _
                                   "  </td>" & _
                                " </tr>" & _
                             "</table>" & _
                           "  </body>" & _
                      "   </html>"




        Return strHTMLemail

    End Function



    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnk As HyperLink = New HyperLink
            hlnk = CType(e.Item.FindControl("Completo"), HyperLink)
            hlnk.ToolTip = "Alert"
            hlnk.ImageUrl = "../Imagenes/Circle_" & e.Item.Cells(13).Text.ToString & ".png"
        End If
    End Sub

    Sub fill(ByVal Fecha As Date)
        Dim sql As String = ""
        '******************************************
        Me.RadScheduler2.TimeZoneOffset = TimeSpan.Parse("-06:00:00") ' Zona Horaria Centro América
        Me.RadScheduler2.SelectedDate = DateSerial(Year(Date.Now()), Month(Fecha), 1)
        '******************************************

        Me.RadScheduler2.Culture = System.Globalization.CultureInfo.CurrentCulture
        Me.RadScheduler2.ShowAllDayRow = False
        sql = "SELECT *, fecha_aprobacionDT as fecha_aprobacionDTPart FROM dbo.FN_Ta_RutaSeguimiento(" & Me.lblIDocumento.Text & ") WHERE fecha_aprobacionDT IS NOT NULL"
        sql &= " UNION SELECT *, fecha_recepcionDT as fecha_aprobacionDTPart FROM dbo.FN_Ta_RutaSeguimiento(" & Me.lblIDocumento.Text & ") WHERE fecha_aprobacionDT IS NULL"

        Me.SqlDataSource1.SelectCommand = sql

        Me.RadScheduler2.DataBind()
    End Sub

End Class