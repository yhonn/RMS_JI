
Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports CuteWebUI
Imports System.Net.Mail
Imports System.Net
'Imports Subgurim.Controles
'Imports System.Drawing


Partial Class Aprobaciones_process_day_by_day
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim strResult As String


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login.aspx")
        End Try


        Try

            strResult = ""

            If Not IsPostBack Then


                If Me.Request.QueryString("state_begining").ToString() = "Began" Then

                    'Me.Request.QueryString("state_begining").ToString() = "Began"
                    strResult &= String.Format("{0}Iniciando el proceso de envio de correo de procesos pendientes... {0}{0}", "<br />")

                    '*********************************************COMENZAMOS CON DOCUMENTOS PENDIENTES******************************

                    Dim idEstadoDoc As Integer
                    idEstadoDoc = 1 '*************PENDIENTES

                    Dim sql = String.Format("SELECT  distinct(a.id_rol)  FROM vw_ta_AppDocumento a where a.completo = 'NO' and a.id_estadoDoc in ({0})  order by id_rol", idEstadoDoc.ToString)
                    Dim dm As New SqlDataAdapter(sql, cnnSAP)
                    Dim dtSET As New DataSet("dtROL")
                    dm.Fill(dtSET, "dtROL")
                    dtSET.Tables.Add("dtDOCS")

                    For Each dtR As DataRow In dtSET.Tables("dtROL").Rows


                        '*********************PARA CADA ROL**********************
                        sql = String.Format("   SELECT id_documento,  " & _
                                        "  id_proyecto, " & _
                                        "  nombre_proyecto, " & _
                                        "  id_empleado_app," & _
                                        "  id_empleado, " & _
                                        "  id_rol, " & _
                                        "  id_App_Documento, " & _
                                        "  nombre_rol," & _
                                        "  descripcion_rol," & _
                                        "  id_estadoDoc," & _
                                        "  descripcion_estado," & _
                                        "  descripcion_cat," & _
                                        "  descripcion_aprobacion," & _
                                        "  numero_instrumento," & _
                                        "  descripcion_doc," & _
                                        "  id_empleado_app," & _
                                        "  nombre_empleado_app" & _
                                        "    FROM vw_ta_AppDocumento  " & _
                                        "  where completo = 'NO'" & _
                                        "   and id_estadoDoc in ({0})" & _
                                        "    and id_rol ={1} ", idEstadoDoc.ToString, dtR("id_rol"))

                        dtSET.Tables("dtDOCS").Rows.Clear()
                        dm.SelectCommand.CommandText = sql
                        dm.Fill(dtSET, "dtDOCS")


                        '*************ENVIAMOS EL EMAIL
                        enviar_email(dtSET.Tables("dtDOCS"), dtSET.Tables("dtDOCS").Rows(0).Item("id_rol"), idEstadoDoc)

                        'For Each dtROW_2 As DataRow In dtSET.Tables("dtDOCS").Rows '************PARA CADA DOCUMENTO
                        'Next

                    Next




                    '*********************************************CONTINUAMOS CON DOCUMENTOS STAND by******************************

                    idEstadoDoc = 6 '*************STAND BY

                    sql = String.Format("SELECT  distinct(a.id_rol)  FROM vw_ta_AppDocumento a where a.completo = 'NO' and a.id_estadoDoc in ({0})  order by id_rol", idEstadoDoc.ToString)
                    dtSET.Tables.Add("dtROL_StandBy")
                    dm.SelectCommand.CommandText = sql
                    dm.Fill(dtSET, "dtROL_StandBy")
                    dtSET.Tables.Add("dtDOCS_StandBy")

                    For Each dtR As DataRow In dtSET.Tables("dtROL_StandBy").Rows


                        '*********************PARA CADA ROL**********************
                        sql = String.Format("   SELECT id_documento,  " & _
                                        "  id_proyecto, " & _
                                        "  nombre_proyecto, " & _
                                        "  id_empleado_app," & _
                                        "  id_empleado, " & _
                                        "  id_rol, " & _
                                        "  id_App_Documento, " & _
                                        "  nombre_rol," & _
                                        "  descripcion_rol," & _
                                        "  id_estadoDoc," & _
                                        "  descripcion_estado," & _
                                        "  descripcion_cat," & _
                                        "  descripcion_aprobacion," & _
                                        "  numero_instrumento," & _
                                        "  descripcion_doc," & _
                                        "  id_empleado_app," & _
                                        "  nombre_empleado_app" & _
                                        "    FROM vw_ta_AppDocumento  " & _
                                        "  where completo = 'NO'" & _
                                        "   and id_estadoDoc in ({0})" & _
                                        "    and id_rol ={1} ", idEstadoDoc.ToString, dtR("id_rol"))


                        dtSET.Tables("dtDOCS_StandBy").Rows.Clear()
                        dm.SelectCommand.CommandText = sql
                        dm.Fill(dtSET, "dtDOCS_StandBy")


                        '*************ENVIAMOS EL EMAIL
                        enviar_email(dtSET.Tables("dtDOCS_StandBy"), dtSET.Tables("dtDOCS_StandBy").Rows(0).Item("id_rol"), idEstadoDoc)

                        'For Each dtROW_2 As DataRow In dtSET.Tables("dtDOCS").Rows '************PARA CADA DOCUMENTO
                        'Next

                    Next


                    strResult &= String.Format("{0}{0}{0}  {1}Correo Enviado Satisfactoriamente!!{2} {0}", "<br />", "<strong>", "</strong>")


                End If


                ' strResult &= String.Format("{0}{0}{0}  {1}Correo Enviado Satisfactoriamente!!{2} {0}", "<br />", "<strong>", "</strong>")

            End If



        Catch ex As Exception

            strResult &= String.Format("{0}{0}{0}  {1}!!Error Generado Correos!!{2}: {3}", "<br />", "<strong>", "</strong>", ex.Message)

        End Try

        Dim dvResult As System.Web.UI.HtmlControls.HtmlGenericControl = New System.Web.UI.HtmlControls.HtmlGenericControl
        dvResult.InnerHtml = strResult
        pnResult.Controls.Add(dvResult)

    End Sub



    Sub enviar_email(ByVal dtDATA As DataTable, ByVal idRol As Integer, ByVal idEstadoDoc As Integer)


        '**************************DECLARACION DE VARIABLES***************************
        Dim sql = "SELECT * FROM t_config_email WHERE id_proyecto=" & dtDATA.Rows(0).Item("id_proyecto")
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

        MensajeSend.Subject = "Notification: Documents pending for approval"

        strResult &= String.Format("Obteniendo CCO... {0}", "<br /><br />")
        Dim emails_cco() As String = ds.Tables("email").Rows(0).Item("BCO").ToString.Split(";")
        For i = 0 To emails_cco.Count() - 1
            If emails_cco(i).ToString <> "" Then
                MensajeSend.Bcc.Add(emails_cco(i).ToString)
                strResult &= String.Format("  -{0}{1}", emails_cco(i).ToString, "<br />")
            End If
        Next

        '****************************************AGREGAMOS AL BIG BOSS*********************************
        ds.Tables.Add("COP_EMAIL")
        sql = " select b.email, a.nombre_rol, a.id_rol  from ta_roles a " &
              "    inner join t_empleados b on (a.id_empleado = b.id_empleado)" &
              "      where a.nombre_rol = 'COP' and (estado = 'ACTIVE' OR estado = 'ACTIVO')"
        dm.SelectCommand.CommandText = sql
        dm.Fill(ds, "COP_EMAIL")
        If ds.Tables("COP_EMAIL").Rows.Count > 0 Then
            MensajeSend.Bcc.Add(ds.Tables("COP_EMAIL").Rows(0).Item("email").ToString)
            strResult &= String.Format("  -{0}{1}", ds.Tables("COP_EMAIL").Rows(0).Item("email").ToString, "<br />")
        End If
        '****************************************AGREGAMOS AL BIG BOSS***********************************

        '************************************SACAMOS EL NUMERO DE DOCUMENTOS PENDIENTE QUE TIENE El ROL****************
        Dim strDocs As String = "("
        For Each dtRw As DataRow In dtDATA.Rows
            strDocs &= dtRw("id_documento") & ","
        Next

        strDocs = strDocs.Substring(0, strDocs.Length - 1) '****Quitamos la última comma
        strDocs &= ")"


        If idEstadoDoc = 1 Then

            sql = String.Format(" select distinct tab.email, tab.nombre_rol, tab.id_rol " & _
            "      from" & _
            "      ( select email, nombre_rol, id_rol from vw_ta_roles_emails" & _
            "         where id_rol = {0}" & _
            "          and id_documento in {1}" & _
            "      UNION   " & _
            "       select email, nombre_rol, id_rol  FROM vw_ta_email_gruposRoles " & _
            "         WHERE id_rol = {0} ) as TAB ", idRol.ToString, strDocs)
        Else

            'sql = String.Format("  select distinct tab.email, tab.nombre_rol, tab.id_rol " & _
            '  "   from" & _
            '  "    (select distinct email, nombre_rol, id_rol " & _
            '  "     from vw_ta_roles_emails" & _
            '  "      where id_documento in {1} and orden = 0" & _
            '  "  union " & _
            '  "    select distinct email, nombre_rol, id_rol  FROM vw_ta_email_gruposRoles " & _
            '  "     WHERE id_rol IN ({0})  ) as tab", strDocs)

            sql = String.Format("  select distinct tab.email, tab.nombre_rol, tab.id_rol " & _
                  "   from " & _
                  "    (select email, nombre_rol, id_rol  " & _
                  "  from vw_ta_roles_emails " & _
                  "        where id_documento in {0} and orden = 0 " & _
                  " union  " & _
                  "  select email, nombre_rol, id_rol  FROM vw_ta_email_gruposRoles   " & _
                  "     WHERE id_rol IN (select distinct id_rol from vw_ta_roles_emails where id_documento  in {0} and orden = 0)) as tab ", strDocs)

        End If


        dm.SelectCommand.CommandText = sql
        ds.Tables.Add("dtRoles")
        dm.Fill(ds, "dtRoles")


        strResult &= String.Format("Correos generados para procesos... {0}", "<br /><br />")
        For Each dtROW As DataRow In dtDATA.Rows
            strResult &= String.Format("        {3} - {1} - {2} {4}{0} ", "<br />", dtROW("numero_instrumento"), dtROW("descripcion_doc"), "<strong>", "</strong>")
        Next


        strResult &= String.Format(" ...destinatarios {0} {0}", "<br />")
        '********************AGREGANDO LOS DESTINATARIOS es ESTE EMAIL************************
        For i = 0 To ds.Tables("dtRoles").Rows.Count() - 1
            MensajeSend.To.Add(ds.Tables("dtRoles").Rows(i).Item("email"))
            strResult &= String.Format("  -{0}{1}", ds.Tables("dtRoles").Rows(i).Item("email"), "<br />")
        Next
        '**************************FIN********************************************************

        strResult &= String.Format("  {0}...{0}{0}", "<br />")

        Dim nullable As System.Text.Encoding
        Dim Mensaje = Make_Email(dtDATA)
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

            '************VALIDAR ACA QUE SI NO ES PRUEBA************************
            If Me.Request.QueryString("testing").ToString() = "NO" Then
                CorreoSend.Send(MensajeSend)
            End If
            '************VALIDAR ACA QUE SI NO ES PRUEBA************************

        Catch ex As Exception
            lblResult2.Text &= String.Format("{0}{0}{0}{0}  !!Error Generado Correos!! {0}{1}", Chr(13), ex.Message)
            MsgBox("ERROR: " & ex.ToString, MsgBoxStyle.Critical, "Error")
        End Try


    End Sub  '**********************FUNCION PARA ENVIAR EMAILS**********

    Public Function Make_Email(ByVal dtData As DataTable) As String


        '*****************************TABLA DE ESTADOS ******************************** 
        'Dim dsP As New DataSet("dtPROCS")
        'dsP.Tables.Add("dtPROCS")
        'Dim Sql As String
        'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
        Dim strTable As String 

        
        '*****************************TABLA DE ESTADOS ******************************** 
        Dim strHTMLemail As String = "<html xmlns='http://www.w3.org/1999/xhtml' >" & _
        "<head>" & _
                "</head>" & _
                  "       <body>" & _
                   "          <div>" & _
                     "           <p style='font-family: Arial, Helvetica, sans-serif; font-size: small;'>" & _
                      "            This message has been sent from the Results Management System CHEMONICS <br /><br />" & _
                       "         </p>" & _
                        "     </div>"


        strHTMLemail &= "<table border='1' cellpadding='0' cellspacing='0' style='width:100%; border-color:#FFFFFF;'>" & _
                       "         <tr>" & _
                       "             <td colspan ='2' >" & _
                       "                   <table style='border-color: #000000; width:100%;border-color:#FFFFFF;'>" & _
                       "                          <tr >" & _
                       "                              <td  style='padding-left: 6px; width:10%; vertical-align:middle; text-align:center' > " & _
                       "                                 <div id='logo1' style=' vertical-align:middle' >" & _
                       "                                     <img alt='\' hspace='0' src='cid:LogCHERM' align='baseline' style='width:120px; height:120px; border:0px;' />" & _
                       "                                  </div> " & _
                       "                              </td>" & _
                       "                               <td colspan='2' style='padding: 50px; text-align:left; font-family:Arial Unicode MS; font-size: medium; vertical-align:bottom; padding-bottom:5px; font-weight:bolder; width:60%; text-transform:uppercase '>" & _
                       "                                       CELI-Norte/Sur o Colombia Responde Norte/Sur" & _
                       "                               </td>" & _
                       "                             </tr>" & _
                       "                      </table><br />" & _
                       "              </td>" & _
                       "         </tr>" & _
                       "          <tr style=' height:10px;'>" & _
                       "             <td colspan='2' style=' height:10px; background-color:#ED7620; border-color:#ED7620' >" & _
                       "             </td>" & _
                       "         </tr>" & _
                       "         <tr>" & _
                       "          <td colspan='2'>" & _
                       "         <!-- OJO -->" & _
                       "            <table width='100%'>" & _
                       "             <tr>" & _
                       "              <td colspan='5'></td>" & _
                       "             </tr>" & _
                       "              <tr style = 'padding: 3px; border-color:#CECECE; background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size:small; width:20%; text-align:center;' >" & _
                       "                 <td>CATEGORY</td>" & _
                       "                 <td>APPROVAL</td>" & _
                       "                 <td>INSTRUMENT NUMBER</td>" & _
                       "                 <td>NAME OF PROCESS</td>" & _
                       "                 <td>STATUS</td>" & _
                       "              </tr>" & _
                       "             <tr>" & _
                       "              <td colspan='5'></td>" & _
                       "             </tr>"

       
        '*********************************AGREGAMOS TODOS LOS DOCUMENTOS PENDIENTES************************************************
        For Each dtROW As DataRow In dtData.Rows

            strHTMLemail &= "<tr>" & _
                            "          <td style='padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;' >" & _
                            dtROW("descripcion_cat") & _
                            "            </td>" & _
                            "          <td style='padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;' >" & _
                             dtROW("descripcion_aprobacion") & _
                            "           </td>" & _
                            "          <td style='padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;' >" & _
                             dtROW("numero_instrumento") & _
                            "          </td>" & _
                            "          <td style='padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;' >" & _
                            dtROW("descripcion_doc") & _
                            "          </td>" & _
                            "          <td style='padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;' >"

            '***********************GENERAMOS LA TABLA DE STATUS**********************************

            strHTMLemail &= "<!-- TABLE STATUS --> "

            strHTMLemail &= makeTable(dtROW("id_documento"), dtROW("id_App_Documento"))

            strHTMLemail &= "            </td>" & _
                            "        </tr>"

            strHTMLemail &= "    <tr>" & _
                            "         <td colspan='5'></td>" & _
                            "        </tr>"

        Next
        '*********************************AGREGAMOS TODOS LOS DOCUMENTOS PENDIENTES************************************************


        strHTMLemail &= "   </table>" & _
                        "         <!-- OJO -->" & _
                        "        </td>" & _
                        "        </tr>" & _
                        "         <tr style=' height:10px;'>" & _
                        "            <td colspan='2' style=' height:10px; background-color:#ED7620; border-color:#ED7620' >" & _
                        "            </td>" & _
                        "        </tr>" & _
                        "         </table>" & _
                        "        <br /><br />" & _
                        "        <table>" & _
                        "        <tr>" & _
                        "            <td colspan='2' style='border-color:#CCCCCC;background-color: #CCCCCC;'>" & _
                        "                <table border='1' cellpadding='0' cellspacing='0' style='mso-cellspacing: 0cm; mso-border-alt: outset #000033 .75pt; mso-yfti-tbllook: 1184; mso-padding-alt: 0cm 0cm 0cm 0cm; font-size: 10.0pt; font-family: 'Times New Roman', serif;border: 1.0pt outset #000033;'>" & _
                        "                    <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;mso-yfti-lastrow:yes'>" & _
                        "                        <td style='border: inset white 1.0pt; mso-border-alt: inset white .75pt;  padding: 0px 0px 0px 6px'>" & _
                        "                            <p>" & _
                        "                                &nbsp;</p>" & _
                        "                            <p>" & _
                        "                                <b><strong>This is an automatically generated email, please do not reply</strong></b></p>" & _
                        "                            <p>" & _
                        "                                <br />" & _
                        "                                <strong>WARNING</strong>: <em>This message contains information legally " & _
                        "                                protected. If you have received this message by mistake, please avoid checking, " & _
                        "                                distributing, copying, reproduction or misuse of the information within it and " & _
                        "                                reply to the sender informing about the misunderstanding.</em></p>" & _
                        "                            <p>" & _
                        "                                &nbsp;</p>" & _
                        "                        </td>" & _
                        "                    </tr>" & _
                        "                </table>" & _
                        "            </td>" & _
                        "       </tr>" & _
                        "    </table>" & _
                        "</body>" & _
                        "</html>"



        Return strHTMLemail

    End Function


    Public Function makeTable(ByVal id_doc As Integer, ByVal idAppDoc As Integer) As String

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
        Dim bndSTR As Boolean = False

        Dim Sql = "SELECT * FROM dbo.FN_Ta_RutaSeguimiento(" & id_doc & ") ORDER BY ORDEN"
        Dim dm As New SqlDataAdapter(Sql, cnnSAP)
        Dim ds As New DataSet("dtPROCS")
        dm.Fill(ds, "dtPROCS")

        strTable = "<table border= '1' cellpadding='4' cellspacing='0'  style='border-color:#CCCCCC;padding: 5px; font-family:Verdana, Arial; font-size: x-small;'> "

        For Each dtR As DataRow In ds.Tables("dtPROCS").Rows

            If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) Then

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

            strTable &= tdOP & "<img alt='\' hspace='0' src='cid:EmmB_" & Trim(dtR.Item("alerta")) & "' align='baseline' style='border:0px;' />" & tdCL

            strTable &= trCL1

        Next dtR

        strTable &= "</table>"

        Return strTable

    End Function


End Class
