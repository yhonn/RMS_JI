Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Net.Mail
Imports System.Text.RegularExpressions.Regex
Imports System.Web.UI.Page
Imports System.Net
Imports System.Text
Imports ly_SIME
Imports ly_RMS


'Imports System.Configuration.ConfigurationManager
'Imports System.Data.SqlClient
'Imports System.Globalization
'Imports ly_SIME
'Imports ly_RMS
'Imports System.Net.Mail
'

Namespace APPROVAL


    Public Class notification_proccess
        Inherits System.Web.UI.Page

        Dim cl_utl As New CORE.cls_util
        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
        Dim strResult As String

        Dim id_programa As Integer
        Dim id_notification As Integer
        Dim id_template As Integer
        'Dim id_documento As Integer
        Dim id_AppDocumento As Integer
        Dim id_comment As Integer


        Dim Sql As String
        Dim dateUtil As APPROVAL.cls_dUtil

        Dim userCulture As CultureInfo
        Dim ObJemail As UTILS.cls_email

        Dim tbl_t_notification As DataTable
        Dim Tbl_t_template As DataTable
        Dim Tbl_ta_AppDoumento As DataTable
        Dim Tbl_Document As DataTable
        Dim Tbl_Ta_RutaSeguimiento As DataTable
        Dim Tbl_ta_GrupoRoles_emails As DataTable
        Dim Tbl_ta_roles_emails As DataTable
        Dim tbl_ta_DocumentosINFO As DataTable
        Dim tbl_emails As DataTable
        Dim Tbl_ta_comentariosDoc As DataTable
        Dim tbl_t_usuarios As DataTable

        '***************Store the pending app with its respective emails************************
        Dim tbl_EmailIncluded As DataTable
        '***************Store the pending app with its respective emails************************

        Public Property timezoneUTC As Integer

        Const cPENDING = 1
        Const cAPPROVED = 2
        Const cnotAPPROVED = 3
        Const cCANCELLED = 4
        Const cOPEN = 5
        Const cSTANDby = 6
        Const cCOMPLETED = 7

        Const cAPP_sys As Integer = 2 '******************APPROVAL

        Public Sub New(ByVal id_prog As Integer)

            id_programa = id_prog

        End Sub

        Public Sub New(ByVal id_prog As Integer, ByVal id_noti As Integer, ByVal userC As CultureInfo)

            id_programa = id_prog
            id_notification = IIf(id_noti > 0, id_noti, id_notification)
            id_comment = 0
            userCulture = userC

        End Sub


        Public Function get_notificationSharedRoles(ByVal strState As String) As DataTable

            If strState = "StandBy" Then
                '--StandBy for Share Roles, send to the originator 
                Sql = String.Format("select idOriginador as ID , count(*) as N
                                from VW_GR_TA_DOCUMENTOS
                            where id_programa = {0}
                            and completo <> 'SI'
                            and idUserOwner = '0'
                            and idRolOriginator = rol_owner
                            group by idOriginador ", id_programa)
            Else 'Pending

                Sql = String.Format("Select rol_owner as ID, Propietario, count(*) as N
                                        from VW_GR_TA_DOCUMENTOS
                                       where completo <> 'SI'
                                     and idUserOwner = '0'
                                     and idRolOriginator <> rol_owner
                                     group by rol_owner, Propietario ", id_programa)

            End If



            get_notificationSharedRoles = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", 0, Sql)

            If (get_notificationSharedRoles.Rows.Count = 1 And get_notificationSharedRoles.Rows.Item(0).Item("ID") = 0) Then
                get_notificationSharedRoles.Rows.Remove(get_notificationSharedRoles.Rows.Item(0))
            End If

        End Function


        Public Function get_notificationPending() As DataTable

            '--Pending Notifications
            Sql = String.Format("select rol_owner as ID, count(*) as N
                                   from VW_GR_TA_DOCUMENTOS
                                  where completo <> 'SI'
                                 and idUserOwner <> '0'
                                 and id_programa = {0} 
                                group by rol_owner ", id_programa)

            get_notificationPending = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", 0, Sql)

            If (get_notificationPending.Rows.Count = 1 And get_notificationPending.Rows.Item(0).Item("ID") = 0) Then
                get_notificationPending.Rows.Remove(get_notificationPending.Rows.Item(0))
            End If

        End Function



        Public Function get_notificationPendingApp_ByOriginator(ByVal idOriginator As Integer) As DataTable

            Sql = String.Format(" select * from VW_GR_TA_DOCUMENTOS
                                     where id_programa = {0}
                                     and completo <> 'SI'
                                     and idUserOwner = '0'
                                     and idRolOriginator = rol_owner
                                     and IdOriginador = {1}", id_programa, idOriginator)

            get_notificationPendingApp_ByOriginator = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "idOriginador", 0, Sql)

            If (get_notificationPendingApp_ByOriginator.Rows.Count = 1 And get_notificationPendingApp_ByOriginator.Rows.Item(0).Item("idOriginador") = 0) Then
                get_notificationPendingApp_ByOriginator.Rows.Remove(get_notificationPendingApp_ByOriginator.Rows.Item(0))
            End If

        End Function


        Public Function get_notificationPendingApp_BySH_RolOwner(ByVal idRolOwner As Integer) As DataTable

            Sql = String.Format("select * from VW_GR_TA_DOCUMENTOS
                                    where id_programa = {0}
                                    and completo <> 'SI'
                                    and idUserOwner = '0'
                                    and idRolOriginator <> rol_owner
	                                and rol_owner = {1}", id_programa, idRolOwner)

            get_notificationPendingApp_BySH_RolOwner = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "rol_owner", 0, Sql)

            If (get_notificationPendingApp_BySH_RolOwner.Rows.Count = 1 And get_notificationPendingApp_BySH_RolOwner.Rows.Item(0).Item("id_documento") = 0) Then
                get_notificationPendingApp_BySH_RolOwner.Rows.Remove(get_notificationPendingApp_BySH_RolOwner.Rows.Item(0))
            End If

        End Function

        Public Function get_notificationPendingApp_By_RolOwner(ByVal idRolOwner As Integer) As DataTable

            Sql = String.Format("select * from VW_GR_TA_DOCUMENTOS
                                    where id_programa = {0}
                                    and completo <> 'SI'
                                    and idUserOwner <> '0'                                   
	                                and rol_owner = {1}", id_programa, idRolOwner)

            get_notificationPendingApp_By_RolOwner = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "rol_owner", 0, Sql)

            If (get_notificationPendingApp_By_RolOwner.Rows.Count = 1 And get_notificationPendingApp_By_RolOwner.Rows.Item(0).Item("id_documento") = 0) Then
                get_notificationPendingApp_By_RolOwner.Rows.Remove(get_notificationPendingApp_By_RolOwner.Rows.Item(0))
            End If

        End Function


        Public Function Notify_App_ByOriginator(ByVal idOriginator As Integer, Optional strBaseDir As String = "") As Boolean


            Dim tbl_approvalPending As DataTable = get_notificationPendingApp_ByOriginator(idOriginator) '--Getting the standbyNotification and sending to its respective originator
            Dim strMess As String

            '************************************SYSTEM INFO********************************************
            Dim cProgram As New RMS.cls_Program
            cProgram.get_Sys(0, True)
            cProgram.get_Programs(id_programa, True)
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
            dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
            '************************************SYSTEM INFO********************************************

            '*****************Creating the email Object**************************************
            ObJemail = New UTILS.cls_email(id_programa)
            '*****************Creating the email Object**************************************


            '*****************building the email from the templates objects**************************************
            set_t_notification(id_notification) 'Create the Noti_Object
            set_t_Template() 'Set Template object from the notification id
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

            '*****************FINDING the BCC destinataries **************************************
            Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

            For Each dtEmail As String In strBccEmails

                If Not String.IsNullOrEmpty(dtEmail) Then

                    If IsEmail(dtEmail) Then
                        ObJemail.AddBcc(dtEmail)
                    End If
                    'If dtEmail.IndexOf("@") > 1 And dtEmail.IndexOf(".") Then
                    '    ObJemail.AddBcc(dtEmail)
                    'End If
                End If

            Next
            '*****************FINDING the BCC destinataries **************************************

            '*****************ADDING THE SUBJECTS**********************
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

            '************************************CONTENT**************************************************************************
            Dim strContent As String = ""
            Dim strTable As String = ""
            '************************************CONTENT**************************************************************************
            Dim strContentALL As String = ""
            Dim boolSetEmailList As Boolean = True

            '--Fore each pending approval started to build the EMail
            '<!--##CATEGORY##-->;<!--##APPROVAL_NAME##-->;<!--##INSTRUMENT_NUMBER##-->;<!--##DESCRIPTION_DOC##-->;<!--##TABLE_STATUS##-->
            For Each dtR2 As DataRow In tbl_approvalPending.Rows

                '************************************CONTENT**************************************************************************
                strContent = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1)
                '************************************CONTENT**************************************************************************

                'set_Apps_Document(dtR2("id_documento")) 'Set all AppDoc
                '*****************FINDING the Destinataries **************************************

                strContent = strContent.Replace("<!--##CATEGORY##-->", dtR2("descripcion_cat"))
                strContent = strContent.Replace("<!--##APPROVAL_NAME##-->", dtR2("descripcion_aprobacion"))

                strContent = strContent.Replace("<!--##ID_DOC##-->", dtR2("id_documento"))
                strContent = strContent.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

                strContent = strContent.Replace("<!--##INSTRUMENT_NUMBER##-->", dtR2("numero_instrumento"))
                strContent = strContent.Replace("<!--##DESCRIPTION_DOC##-->", dtR2("descripcion_doc"))

                '************************************STATUS TABLE**************************************************************************
                '*****************<!--##TABLE_STATUS##-->'*****************
                set_Ta_RutaSeguimiento(dtR2("id_documento")) 'getting the complete route

                Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>
                Dim strTableContent As String = ""
                Dim strTableContentALL As String = ""
                '<!--##CONTENT##-->;<!--##ROLE_NAME##-->;<!--##EMPLOYEE_NAME##-->;<!--##STATE##-->;<!--##DATE_RECEIPT##-->;<!--##ALERT_TYPE##-->;<!--##STRONG_OPEN##-->;<!--##STRONG_OPEN##-->;
                strTable = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2)

                For Each dtRow In Tbl_Ta_RutaSeguimiento.Rows

                    strTableContent = get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 2)
                    strTableContent = strTableContent.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                    strTableContent = strTableContent.Replace("<!--##EMPLOYEE_NAME##-->", dtRow("nombre_empleado"))
                    strTableContent = strTableContent.Replace("<!--##STATE##-->", dtRow("descripcion_estado"))
                    strTableContent = strTableContent.Replace("<!--##DATE_RECEIPT##-->", dtRow("fecha_aprobacion"))
                    strTableContent = strTableContent.Replace("<!--##ALERT_TYPE##-->", dtRow("Alerta"))

                    'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                    If ((dtR2("id_ruta") = dtRow("id_ruta")) And (dtR2("id_estadoDoc") = dtRow("id_estadoDoc"))) Then
                        strTableContent = strTableContent.Replace("<tr>", strStyle)
                        strTableContent = strTableContent.Replace("<!--##STRONG_OPEN##-->", "<strong>")
                        strTableContent = strTableContent.Replace("<!--##STRONG_CLOSE##-->", "</strong>")
                    Else
                        strTableContent = strTableContent.Replace("<!--##STRONG_OPEN##-->", "")
                        strTableContent = strTableContent.Replace("<!--##STRONG_CLOSE##-->", "")
                    End If

                    strTableContentALL &= strTableContent

                Next

                strTable = strTable.Replace("<!--##CONTENT##-->", strTableContentALL)

                strContent = strContent.Replace("<!--##TABLE_STATUS##-->", strTable)
                strContentALL &= strContent
                strContentALL &= get_t_templateField("tp_script", "id_notification", id_notification, "tp_nwline", 1)

                '************************************STATUS TABLE**************************************************************************
                'Add_emails(dtR2("id_documento"), dtR2("id_estadoDoc"), idOriginator, boolSetEmailList) 'Adding the respective email according the state of the approval proccess
                Add_document_email_list(dtR2("id_documento"), boolSetEmailList)
                '*****************FINDING the Destinataries **************************************
                boolSetEmailList = False

            Next

            strMess = strMess.Replace("<!--##CONTENT##-->", strContentALL)

            Add_emails(idOriginator, "User") 'Adding the respective email according the user or the Role

            '*****TEMP*******
            If ObJemail.SilentMode Then
                'If Not ObJemail.SilentMode Then

                strSubject &= " (Silent MODE) "
                Dim strTO As String = ""
                Dim strCC As String = ""
                Dim strBCC As String = ""

                For Each dtRow In tbl_emails.Rows

                    Select Case dtRow("Tipo")
                        Case "TO"
                            strTO &= String.Format("{0};", dtRow("email").ToString.Trim)
                        Case "CC"
                            strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                        Case "BCC"
                            strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                    End Select

                Next

                Dim strEMAILS As String = String.Format("<br\><br\> TO:{0}<br\>CC:{1}<br\>BCC:{2} ", strTO, strCC, strBCC)
                strMess = strMess.Replace("<!--##MESS##-->", strEMAILS) 'Representavite emails for this  notification
            Else
                strMess = strMess.Replace("<!--##MESS##-->", "")
            End If

            'Adding the subjects according the state
            ObJemail.AddSubject(strSubject)

            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************

            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo

            Dim strPath As String
            If strBaseDir.Length > 1 Then
                strPath = strBaseDir
            Else
                strPath = Server.MapPath("~")
            End If


            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
            End If

            'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Circle_Red.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Circle_Green.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Notify_App_ByOriginator = True
            Else
                Notify_App_ByOriginator = False
            End If



        End Function




        Public Function Notify_App_ByRolOwner(ByVal idRolOwner As Integer, Optional boolSHARED As Boolean = False, Optional strBaseDir As String = "") As Boolean

            '**************JUST CHANGE HERE****************
            Dim tbl_approvalPending As DataTable
            If boolSHARED Then
                tbl_approvalPending = get_notificationPendingApp_BySH_RolOwner(idRolOwner)
            Else
                tbl_approvalPending = get_notificationPendingApp_By_RolOwner(idRolOwner)
            End If
            '**************JUST CHANGE HERE****************
            Dim strMess As String

            '************************************SYSTEM INFO********************************************
            Dim cProgram As New RMS.cls_Program
            cProgram.get_Sys(0, True)
            cProgram.get_Programs(id_programa, True)
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
            dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
            '************************************SYSTEM INFO********************************************

            '*****************Creating the email Object**************************************
            ObJemail = New UTILS.cls_email(id_programa)
            '*****************Creating the email Object**************************************


            '*****************building the email from the templates objects**************************************

            set_t_notification(id_notification) 'Create the Noti_Object
            set_t_Template() 'Set Template object from the notification id
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

            '*****************FINDING the BCC destinataries **************************************
            Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

            For Each dtEmail As String In strBccEmails

                If Not String.IsNullOrEmpty(dtEmail) Then
                    If dtEmail.IndexOf("@") > 1 And dtEmail.IndexOf(".") Then
                        ObJemail.AddBcc(dtEmail)
                    End If
                End If

            Next
            '*****************FINDING the BCC destinataries **************************************

            '*****************ADDING THE SUBJECTS**********************
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

            '************************************CONTENT**************************************************************************
            Dim strContent As String = ""
            Dim strTable As String = ""
            '************************************CONTENT**************************************************************************
            Dim strContentALL As String = ""
            Dim boolSetEmailList As Boolean = True

            '--Fore each pending approval started to build the EMail
            '<!--##CATEGORY##-->;<!--##APPROVAL_NAME##-->;<!--##INSTRUMENT_NUMBER##-->;<!--##DESCRIPTION_DOC##-->;<!--##TABLE_STATUS##-->
            For Each dtR2 As DataRow In tbl_approvalPending.Rows

                '************************************CONTENT**************************************************************************
                strContent = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1)
                '************************************CONTENT**************************************************************************

                'set_Apps_Document(dtR2("id_documento")) 'Set all AppDoc
                '*****************FINDING the Destinataries **************************************

                strContent = strContent.Replace("<!--##CATEGORY##-->", dtR2("descripcion_cat"))
                strContent = strContent.Replace("<!--##APPROVAL_NAME##-->", dtR2("descripcion_aprobacion"))

                strContent = strContent.Replace("<!--##ID_DOC##-->", dtR2("id_documento"))
                strContent = strContent.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

                strContent = strContent.Replace("<!--##INSTRUMENT_NUMBER##-->", dtR2("numero_instrumento"))
                strContent = strContent.Replace("<!--##DESCRIPTION_DOC##-->", dtR2("descripcion_doc"))

                '************************************STATUS TABLE**************************************************************************
                '*****************<!--##TABLE_STATUS##-->'*****************
                set_Ta_RutaSeguimiento(dtR2("id_documento")) 'getting the complete route

                Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>
                Dim strTableContent As String = ""
                Dim strTableContentALL As String = ""
                '<!--##CONTENT##-->;<!--##ROLE_NAME##-->;<!--##EMPLOYEE_NAME##-->;<!--##STATE##-->;<!--##DATE_RECEIPT##-->;<!--##ALERT_TYPE##-->;<!--##STRONG_OPEN##-->;<!--##STRONG_OPEN##-->;
                strTable = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2)


                For Each dtRow In Tbl_Ta_RutaSeguimiento.Rows

                    strTableContent = get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 2)
                    strTableContent = strTableContent.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                    strTableContent = strTableContent.Replace("<!--##EMPLOYEE_NAME##-->", dtRow("nombre_empleado"))
                    strTableContent = strTableContent.Replace("<!--##STATE##-->", dtRow("descripcion_estado"))
                    strTableContent = strTableContent.Replace("<!--##DATE_RECEIPT##-->", dtRow("fecha_aprobacion"))
                    strTableContent = strTableContent.Replace("<!--##ALERT_TYPE##-->", dtRow("Alerta"))

                    'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                    If ((dtR2("id_ruta") = dtRow("id_ruta")) And (dtR2("id_estadoDoc") = dtRow("id_estadoDoc"))) Then
                        strTableContent = strTableContent.Replace("<tr>", strStyle)
                        strTableContent = strTableContent.Replace("<!--##STRONG_OPEN##-->", "<strong>")
                        strTableContent = strTableContent.Replace("<!--##STRONG_CLOSE##-->", "</strong>")
                    Else
                        strTableContent = strTableContent.Replace("<!--##STRONG_OPEN##-->", "")
                        strTableContent = strTableContent.Replace("<!--##STRONG_CLOSE##-->", "")
                    End If

                    strTableContentALL &= strTableContent

                Next

                strTable = strTable.Replace("<!--##CONTENT##-->", strTableContentALL)

                strContent = strContent.Replace("<!--##TABLE_STATUS##-->", strTable)
                strContentALL &= strContent
                strContentALL &= get_t_templateField("tp_script", "id_notification", id_notification, "tp_nwline", 1)

                '************************************STATUS TABLE**************************************************************************
                'Add_emails(dtR2("id_documento"), dtR2("id_estadoDoc"), idOriginator, boolSetEmailList) 'Adding the respective email according the state of the approval proccess
                Add_document_email_list(dtR2("id_documento"), boolSetEmailList)
                '*****************FINDING the Destinataries **************************************
                boolSetEmailList = False

            Next

            strMess = strMess.Replace("<!--##CONTENT##-->", strContentALL)

            '**************JUST CHANGE HERE****************
            If boolSHARED Then
                Add_emailsSH_Roles(idRolOwner) 'Adding the respective email according the every Shared Role
            Else
                Add_emails(idRolOwner, "Role") 'Adding the respective email according the user or the Simple Role
            End If
            '**************JUST CHANGE HERE****************

            '*****TEMP*******
            If ObJemail.SilentMode Then
                'If Not ObJemail.SilentMode Then

                strSubject &= " (Silent MODE) "
                Dim strTO As String = ""
                Dim strCC As String = ""
                Dim strBCC As String = ""

                For Each dtRow In tbl_emails.Rows

                    Select Case dtRow("Tipo")
                        Case "TO"
                            strTO &= String.Format("{0};", dtRow("email").ToString.Trim)
                        Case "CC"
                            strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                        Case "BCC"
                            strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                    End Select

                Next

                Dim strEMAILS As String = String.Format("<br\><br\> TO:{0}<br\>CC:{1}<br\>BCC:{2} ", strTO, strCC, strBCC)
                strMess = strMess.Replace("<!--##MESS##-->", strEMAILS) 'Representavite emails for this  notification
            Else
                strMess = strMess.Replace("<!--##MESS##-->", "")
            End If

            'Adding the subjects according the state
            ObJemail.AddSubject(strSubject)

            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************

            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo

            Dim strPath As String
            If strBaseDir.Length > 1 Then
                strPath = strBaseDir
            Else
                strPath = Server.MapPath("~")
            End If

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
            End If

            'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Red_Time.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Blue_Time.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Orange_Time.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Cyan_Time.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Notify_App_ByRolOwner = True
            Else
                Notify_App_ByRolOwner = False
            End If



        End Function


        Public Function NOTIFIYING_SOLICITATION(ByVal id_notification_app As Integer, Optional strBaseDir As String = "") As Boolean

            'ByVal idOriginator As Integer, Optional strBaseDir As String = "", Optional tbl_docs As DataTable = Nothing

            '***  Dim tbl_approvalPending As DataTable = get_notificationPendingApp_ByOriginator(idOriginator) '--Getting the standbyNotification and sending to its respective originator
            ''Dim tbl_approvalPending As DataTable = get_notification_Pending_ByUser(idOriginator, "NNN", "--none--", 0) '--Getting the notifications and sending to its respective sender
            Dim strMess As StringBuilder = New StringBuilder

            ''Dim EmailOriginator As String = tbl_approvalPending.Rows(0).Item("email")

            '***************Store All the pending app with its respective emails************************
            ''tbl_EmailIncluded = tbl_docs.Copy
            'tbl_EmailIncluded = get_notification_Pending_ByUser(0, "NNN", "--none--", 0) '--to Getting the EmailList for the Documents
            '***************Store All the pending app with its respective emails************************

            Try

                Using dbEntities As New dbRMS_JIEntities

                    Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).ToList()
                    Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(oSolicitationAPP.FirstOrDefault.ID_ACTIVITY_SOLICITATION)

                    Dim EmailOriginator As String = oSolicitationAPP.FirstOrDefault.ORGANIZATIONEMAIL
                    Dim SecundaryEmail As String = oSolicitationAPP.FirstOrDefault.PRIMARYCONTACTEMAIL




                    If IsEmail(EmailOriginator) Then

                        'Dim tbl_approvalCC As DataTable = get_notification_Pending_ByUser(0, "CC", EmailOriginator.Trim, 0) '--Getting the notifications and sending to its respective sender
                        'Dim boolFound As Boolean = False

                        'For Each app_CC_dtRow As DataRow In tbl_EmailIncluded.Rows

                        '    If app_CC_dtRow("type_subject") = "CC" And app_CC_dtRow("email").ToString.Trim = EmailOriginator.Trim Then

                        '        For Each app_pend_dtRow As DataRow In tbl_approvalPending.Rows

                        '            If app_pend_dtRow("id_documento") = app_CC_dtRow("id_documento") Then
                        '                boolFound = True
                        '                Exit For
                        '            End If

                        '        Next

                        '        If Not boolFound Then
                        '            tbl_approvalPending.ImportRow(app_CC_dtRow)
                        '        End If

                        '        boolFound = False


                        '    End If

                        'Next

                        If Not IsEmail(SecundaryEmail) Then
                            SecundaryEmail = ""
                        Else
                            If EmailOriginator = SecundaryEmail Then
                                SecundaryEmail = ""
                            End If
                        End If

                        '************************************SYSTEM INFO********************************************
                        Dim cProgram As New RMS.cls_Program
                        cProgram.get_Sys(0, True)
                        cProgram.get_Programs(id_programa, True)
                        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
                        dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                        '************************************SYSTEM INFO********************************************

                        '*****************Creating the email Object**************************************
                        ObJemail = New UTILS.cls_email(id_programa)
                        '*****************Creating the email Object**************************************

                        '*****************building the email from the templates objects**************************************
                        set_t_notification(id_notification) 'Create the Noti_Object
                        set_t_Template() 'Set Template object from the notification id
                        strMess.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)) 'getting the first template

                        strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

                        '*****************FINDING the BCC destinataries **************************************
                        Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    ObJemail.AddBcc(dtEmail)
                                End If

                            End If

                        Next
                        ''*****************FINDING the BCC destinataries **************************************



                        '*****************ADDING THE SUBJECTS**********************
                        Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

                        strSubject = strSubject.Replace("<!--##SOLICITATION_NUMBER##-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)
                        strSubject = strSubject.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)



                        '<!--##PROJECT_NAME##-->)


                        '<!--##ORGANIZATION_NAME##-->
                        '<!--##DESTINATION_EMAIL##-->
                        '<!--#SOLICITATION##-->
                        '<!--#SOLICITATION_TITLE##-->
                        '<!--#SOLICITATION_PURPOSE##-->
                        '<!--##SOLICITATION_NUMBER##-->
                        '<!--#START_DATE##-->
                        '<!--END_DATE##-->
                        '<!--##SYS_PATH##-->
                        '<!--##ID_DOC##-->

                        strMess.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strMess.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        strMess.Replace("<!--##DESTINATION_EMAIL##-->", String.Format("{0};{1};", EmailOriginator, SecundaryEmail))
                        strMess.Replace("<!--#SOLICITATION##-->", oActivitySolicitation.SOLICITATION)
                        strMess.Replace("<!--#SOLICITATION_PURPOSE##-->", oActivitySolicitation.SOLICITATION_PURPOSE)
                        strMess.Replace("<!--##SOLICITATION_NUMBER##-->", oActivitySolicitation.SOLICITATION_CODE)

                        strMess.Replace("<!--#START_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.start_date, "f", timezoneUTC, True))
                        strMess.Replace("<!--END_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.end_date, "f", timezoneUTC, True))

                        strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys))


                        Dim URL_Keys As String = String.Format("ab={0}&ac={1}&ad={2}", oActivitySolicitation.SOLICITATION_TOKEN.ToString, oSolicitationAPP.FirstOrDefault.SOLICITATION_TOKEN, oSolicitationAPP.FirstOrDefault.USER_TOKEN_UPDATE.ToString)
                        strMess.Replace("<!--##ID_DOC##-->", URL_Keys)


                        ''''Dim strContentALL As StringBuilder = New StringBuilder()
                        ''''Dim boolSetEmailList As Boolean = True
                        ''''Dim tbl_document_rep As DataTable

                        ''''Dim NDays As Decimal
                        ''''Dim NHours As Decimal
                        ''''Dim SHours As Decimal

                        '''''--Fore each pending approval started to build the EMail
                        '''''<!--##CATEGORY##-->;<!--##APPROVAL_NAME##-->;<!--##INSTRUMENT_NUMBER##-->;<!--##DESCRIPTION_DOC##-->;<!--##TABLE_STATUS##-->

                        ''''For Each tbl_approvalPending_dtR As DataRow In tbl_approvalPending.Rows

                        ''''    '************************************CONTENT**************************************************************************
                        ''''    Dim strContent As StringBuilder = New StringBuilder()
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    tbl_document_rep = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", tbl_approvalPending_dtR("id_documento"))
                        ''''    'tbl_document_rep.Rows(0).Item(
                        ''''    '************************************CONTENT**************************************************************************
                        ''''    strContent.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1))
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    'set_Apps_Document(dtR2("id_documento")) 'Set all AppDoc
                        ''''    '*****************FINDING the Destinataries **************************************

                        ''''    strContent.Replace("<!--##CATEGORY##-->", tbl_document_rep.Rows(0).Item("descripcion_cat"))
                        ''''    strContent.Replace("<!--##APPROVAL_NAME##-->", tbl_document_rep.Rows(0).Item("descripcion_aprobacion"))

                        ''''    strContent.Replace("<!--##ID_DOC##-->", tbl_document_rep.Rows(0).Item("id_documento"))
                        ''''    strContent.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

                        ''''    strContent.Replace("<!--##INSTRUMENT_NUMBER##-->", tbl_document_rep.Rows(0).Item("numero_instrumento"))

                        ''''    Dim strDocumentDesc As String = tbl_document_rep.Rows(0).Item("descripcion_doc") & fn_get_EmailList(tbl_approvalPending_dtR("id_documento"))
                        ''''    strContent.Replace("<!--##DESCRIPTION_DOC##-->", strDocumentDesc)

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    '*****************<!--##TABLE_STATUS##-->'*****************
                        ''''    set_Ta_RutaSeguimiento(tbl_document_rep.Rows(0).Item("id_documento")) 'getting the complete route

                        ''''    Dim strStyle As String = "<tr style='background-color:#ee7108;border:1 dotted #FF0000; ' >" 'Putting inside <tr>
                        ''''    'style="background-color:#ee7108;border:1 dotted #FF0000;"
                        ''''    Dim strTableContent As StringBuilder = New StringBuilder()
                        ''''    Dim strTable_Cont As StringBuilder = New StringBuilder()
                        ''''    'Dim strTableContentALL As StringBuilder = New StringBuilder()
                        ''''    Dim strTable As StringBuilder = New StringBuilder()

                        ''''    '<!--##CONTENT##-->;<!--##ROLE_NAME##-->;<!--##EMPLOYEE_NAME##-->;<!--##STATE##-->;<!--##DATE_RECEIPT##-->;<!--##ALERT_TYPE##-->;<!--##STRONG_OPEN##-->;<!--##STRONG_OPEN##-->;
                        ''''    strTable.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2))

                        ''''    SHours = 0
                        ''''    For Each seg_dtRow In Tbl_Ta_RutaSeguimiento.Rows

                        ''''        strTable_Cont.Clear()
                        ''''        strTable_Cont.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 2))


                        ''''        'strTableContent.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                        ''''        strTable_Cont.Replace("<!--##ROLE_NAME##-->", "")
                        ''''        strTable_Cont.Replace("<!--##EMPLOYEE_NAME##-->", seg_dtRow("nombre_empleado"))
                        ''''        strTable_Cont.Replace("<!--##STATE##-->", seg_dtRow("descripcion_estado"))
                        ''''        strTable_Cont.Replace("<!--##DATE_RECEIPT##-->", seg_dtRow("fecha_aprobacion"))
                        ''''        strTable_Cont.Replace("<!--##ALERT_TYPE##-->", seg_dtRow("Alerta"))

                        ''''        'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                        ''''        If ((tbl_document_rep.Rows(0).Item("id_ruta") = seg_dtRow("id_ruta")) And (tbl_document_rep.Rows(0).Item("id_estadoDoc") = seg_dtRow("id_estadoDoc"))) Then
                        ''''            strTable_Cont.Replace("<tr>", strStyle)
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "<strong>")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "</strong>")
                        ''''        Else
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "")
                        ''''        End If

                        ''''        SHours += seg_dtRow("NHRs")
                        ''''        NDays = Math.Round((seg_dtRow("NHRs") / 24), 0, MidpointRounding.AwayFromZero)

                        ''''        strTable_Cont.Replace("<!--##DAYS##-->", NDays)

                        ''''        strTableContent.Append(strTable_Cont.ToString)


                        ''''    Next

                        ''''    NDays = Int((SHours / 24))
                        ''''    NHours = Math.Round((((SHours / 24) - NDays) * 24), 0, MidpointRounding.AwayFromZero)

                        ''''    'strTableContentALL.Append(strTableContent)

                        ''''    strTable.Replace("<!--##TOTAL_DAYS##-->", String.Format(" {0} Days {1} Hrs", NDays, NHours))
                        ''''    strTable.Replace("<!--##CONTENT##-->", strTableContent.ToString)

                        ''''    strContent.Replace("<!--##TABLE_STATUS##-->", strTable.ToString)
                        ''''    strContentALL.Append(strContent.ToString)
                        ''''    strContentALL.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_nwline", 1))

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    'Add_emails(dtR2("id_documento"), dtR2("id_estadoDoc"), idOriginator, boolSetEmailList) 'Adding the respective email according the state of the approval proccess
                        ''''    'Add_document_email_list(dtR2("id_documento"), boolSetEmailList)
                        ''''    '*****************FINDING the Destinataries **************************************
                        ''''    'boolSetEmailList = False

                        ''''    '**************TESTING PRUPORSES
                        ''''    'Exit For
                        ''''    '**************TESTING PRUPORSES

                        ''''Next

                        ''''strMess.Replace("<!--##CONTENT##-->", strContentALL.ToString)


                        'Add_emails(idOriginator, "User") 'Adding the respective email according the user or the Role

                        If ObJemail.SilentMode Then
                            'If Not ObJemail.SilentMode Then

                            strSubject &= " (Silent MODE) "
                            Dim strTO As StringBuilder = New StringBuilder
                            Dim strCC As String = ""
                            Dim strBCC As String = ""

                            '*********************************************************************************************************
                            '******************we can do this getting all the selection List......************************************
                            '*********************************************************************************************************

                            ' For Each dtRow In tbl_emails.Rows

                            'Select Case dtRow("Tipo")
                            '    Case "TO"
                            '        strTO &= String.Format("{0};", dtRow("email").ToString.Trim)
                            '    Case "CC"
                            '        strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                            '    Case "BCC"
                            '        strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                            'End Select

                            strTO.Append(String.Format("{0};", EmailOriginator.ToString.Trim))

                            If SecundaryEmail.Trim.Length > 1 Then
                                strTO.Append(String.Format("{0};", SecundaryEmail.ToString.Trim))
                            End If


                            ' Next

                            ' Dim strEMAILS As String = String.Format("<br /><br /><br /><br /> TO:{0}<br />CC:{1}<br />BCC:{2} ", strTO, strCC, strBCC)
                            Dim strEMAILS As StringBuilder = New StringBuilder
                            strEMAILS.Append("<br /><br /><br /><br />TO:" & strTO.ToString & "<br /><br />")

                            strMess.Replace("<!--##MESS##-->", strEMAILS.ToString) 'Representavite emails for this  notification

                        Else


                            strMess.Replace("<!--##MESS##-->", "")
                            '*********************************************************************************************************
                            '*******************THIS IS the Individual Email for every Notification***********************************
                            ObJemail.AddTo(EmailOriginator)

                            If SecundaryEmail.Trim.Length > 1 Then
                                ObJemail.AddTo(SecundaryEmail.ToString)
                            End If
                            '*******************THIS IS the Individual Email for every notification***********************************
                            '*********************************************************************************************************

                        End If


                        '*******************************************************************
                        '************************SUBJECTS***********************************
                        ObJemail.AddSubject(strSubject)
                        '************************SUBJECTS***********************************
                        '*******************************************************************

                        '************************* THE message it supposed Is here, need to proceed to test*****************
                        '*****************building the email from the templates objects**************************************

                        strMess.Replace("<!--##IMG_CID2##-->", "LogProgram") 'Name of the resource the Program Logo
                        strMess.Replace("<!--##IMG_CID1##-->", "LogChemo") 'Name of the resource the Company logo

                        Dim strPath As String
                        If strBaseDir.Length > 1 Then
                            strPath = strBaseDir
                        Else
                            strPath = Server.MapPath("~")
                        End If

                        Dim strTMP As String = strMess.ToString

                        ObJemail.setAlternativeVIEW(strMess.ToString) 'Setting alternative View

                        Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                        Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                        If Not String.IsNullOrEmpty(strProgramIMG) Then
                            ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
                        Else
                            ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
                        End If

                        'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                        ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")


                        'ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Red_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Blue_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Orange_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Cyan_Time.png", "image/png")

                        ObJemail.SetPriority(MailPriority.High)

                        If ObJemail.SendEmail(id_notification, oSolicitationAPP.FirstOrDefault.ID_SOLICITATION_APP, "REQUEST_EMAIL") Then
                            NOTIFIYING_SOLICITATION = True
                        Else
                            NOTIFIYING_SOLICITATION = False
                        End If

                    Else  '*********************Valid Email addres**********************************

                        NOTIFIYING_SOLICITATION = False

                    End If


                End Using

            Catch ex As Exception

                NOTIFIYING_SOLICITATION = False

            End Try


        End Function



        Public Function NOTIFIYING_SOLICITATION_MOD(ByVal id_notification_app As Integer, Optional strBaseDir As String = "") As Boolean

            'ByVal idOriginator As Integer, Optional strBaseDir As String = "", Optional tbl_docs As DataTable = Nothing

            '***  Dim tbl_approvalPending As DataTable = get_notificationPendingApp_ByOriginator(idOriginator) '--Getting the standbyNotification and sending to its respective originator
            ''Dim tbl_approvalPending As DataTable = get_notification_Pending_ByUser(idOriginator, "NNN", "--none--", 0) '--Getting the notifications and sending to its respective sender
            Dim strMess As StringBuilder = New StringBuilder

            ''Dim EmailOriginator As String = tbl_approvalPending.Rows(0).Item("email")

            '***************Store All the pending app with its respective emails************************
            ''tbl_EmailIncluded = tbl_docs.Copy
            'tbl_EmailIncluded = get_notification_Pending_ByUser(0, "NNN", "--none--", 0) '--to Getting the EmailList for the Documents
            '***************Store All the pending app with its respective emails************************

            Try

                Using dbEntities As New dbRMS_JIEntities

                    Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).ToList()
                    Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(oSolicitationAPP.FirstOrDefault.ID_ACTIVITY_SOLICITATION)

                    Dim EmailOriginator As String = oSolicitationAPP.FirstOrDefault.ORGANIZATIONEMAIL
                    Dim SecundaryEmail As String = oSolicitationAPP.FirstOrDefault.PRIMARYCONTACTEMAIL


                    If IsEmail(EmailOriginator) Then

                        'Dim tbl_approvalCC As DataTable = get_notification_Pending_ByUser(0, "CC", EmailOriginator.Trim, 0) '--Getting the notifications and sending to its respective sender
                        'Dim boolFound As Boolean = False

                        'For Each app_CC_dtRow As DataRow In tbl_EmailIncluded.Rows

                        '    If app_CC_dtRow("type_subject") = "CC" And app_CC_dtRow("email").ToString.Trim = EmailOriginator.Trim Then

                        '        For Each app_pend_dtRow As DataRow In tbl_approvalPending.Rows

                        '            If app_pend_dtRow("id_documento") = app_CC_dtRow("id_documento") Then
                        '                boolFound = True
                        '                Exit For
                        '            End If

                        '        Next

                        '        If Not boolFound Then
                        '            tbl_approvalPending.ImportRow(app_CC_dtRow)
                        '        End If

                        '        boolFound = False


                        '    End If

                        'Next

                        If Not IsEmail(SecundaryEmail) Then
                            SecundaryEmail = ""
                        Else
                            If EmailOriginator = SecundaryEmail Then
                                SecundaryEmail = ""
                            End If
                        End If

                        '************************************SYSTEM INFO********************************************
                        Dim cProgram As New RMS.cls_Program
                        cProgram.get_Sys(0, True)
                        cProgram.get_Programs(id_programa, True)
                        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
                        dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                        '************************************SYSTEM INFO********************************************

                        '*****************Creating the email Object**************************************
                        ObJemail = New UTILS.cls_email(id_programa)
                        '*****************Creating the email Object**************************************

                        '*****************building the email from the templates objects**************************************
                        set_t_notification(id_notification) 'Create the Noti_Object
                        set_t_Template() 'Set Template object from the notification id
                        strMess.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)) 'getting the first template

                        strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

                        '*****************FINDING the BCC destinataries **************************************
                        Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    ObJemail.AddBcc(dtEmail)
                                End If

                            End If

                        Next
                        ''*****************FINDING the BCC destinataries **************************************



                        '*****************ADDING THE SUBJECTS**********************
                        Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

                        strSubject = strSubject.Replace("<!--##SOLICITATION_NUMBER##-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)
                        strSubject = strSubject.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)



                        '<!--##PROJECT_NAME##-->)


                        '<!--##ORGANIZATION_NAME##-->
                        '<!--##DESTINATION_EMAIL##-->
                        '<!--#SOLICITATION##-->
                        '<!--#SOLICITATION_TITLE##-->
                        '<!--#SOLICITATION_PURPOSE##-->
                        '<!--##SOLICITATION_NUMBER##-->
                        '<!--#START_DATE##-->
                        '<!--END_DATE##-->
                        '<!--##SYS_PATH##-->
                        '<!--##ID_DOC##-->

                        strMess.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strMess.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        strMess.Replace("<!--##DESTINATION_EMAIL##-->", String.Format("{0};{1};", EmailOriginator, SecundaryEmail))
                        strMess.Replace("<!--#SOLICITATION##-->", oActivitySolicitation.SOLICITATION)
                        strMess.Replace("<!--#SOLICITATION_PURPOSE##-->", oActivitySolicitation.SOLICITATION_PURPOSE)
                        strMess.Replace("<!--##SOLICITATION_NUMBER##-->", oActivitySolicitation.SOLICITATION_CODE)

                        strMess.Replace("<!--#START_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.start_date, "f", timezoneUTC, True))
                        strMess.Replace("<!--END_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.end_date, "f", timezoneUTC, True))

                        strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys))

                        strMess.Replace("<!--##SOLICITATION_MODIFICATION##-->", oActivitySolicitation.MODIFICATIONS)

                        Dim URL_Keys As String = String.Format("ab={0}&ac={1}&ad={2}", oActivitySolicitation.SOLICITATION_TOKEN.ToString, oSolicitationAPP.FirstOrDefault.SOLICITATION_TOKEN, oSolicitationAPP.FirstOrDefault.USER_TOKEN_UPDATE.ToString)
                        strMess.Replace("<!--##ID_DOC##-->", URL_Keys)


                        ''''Dim strContentALL As StringBuilder = New StringBuilder()
                        ''''Dim boolSetEmailList As Boolean = True
                        ''''Dim tbl_document_rep As DataTable

                        ''''Dim NDays As Decimal
                        ''''Dim NHours As Decimal
                        ''''Dim SHours As Decimal

                        '''''--Fore each pending approval started to build the EMail
                        '''''<!--##CATEGORY##-->;<!--##APPROVAL_NAME##-->;<!--##INSTRUMENT_NUMBER##-->;<!--##DESCRIPTION_DOC##-->;<!--##TABLE_STATUS##-->

                        ''''For Each tbl_approvalPending_dtR As DataRow In tbl_approvalPending.Rows

                        ''''    '************************************CONTENT**************************************************************************
                        ''''    Dim strContent As StringBuilder = New StringBuilder()
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    tbl_document_rep = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", tbl_approvalPending_dtR("id_documento"))
                        ''''    'tbl_document_rep.Rows(0).Item(
                        ''''    '************************************CONTENT**************************************************************************
                        ''''    strContent.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1))
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    'set_Apps_Document(dtR2("id_documento")) 'Set all AppDoc
                        ''''    '*****************FINDING the Destinataries **************************************

                        ''''    strContent.Replace("<!--##CATEGORY##-->", tbl_document_rep.Rows(0).Item("descripcion_cat"))
                        ''''    strContent.Replace("<!--##APPROVAL_NAME##-->", tbl_document_rep.Rows(0).Item("descripcion_aprobacion"))

                        ''''    strContent.Replace("<!--##ID_DOC##-->", tbl_document_rep.Rows(0).Item("id_documento"))
                        ''''    strContent.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

                        ''''    strContent.Replace("<!--##INSTRUMENT_NUMBER##-->", tbl_document_rep.Rows(0).Item("numero_instrumento"))

                        ''''    Dim strDocumentDesc As String = tbl_document_rep.Rows(0).Item("descripcion_doc") & fn_get_EmailList(tbl_approvalPending_dtR("id_documento"))
                        ''''    strContent.Replace("<!--##DESCRIPTION_DOC##-->", strDocumentDesc)

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    '*****************<!--##TABLE_STATUS##-->'*****************
                        ''''    set_Ta_RutaSeguimiento(tbl_document_rep.Rows(0).Item("id_documento")) 'getting the complete route

                        ''''    Dim strStyle As String = "<tr style='background-color:#ee7108;border:1 dotted #FF0000; ' >" 'Putting inside <tr>
                        ''''    'style="background-color:#ee7108;border:1 dotted #FF0000;"
                        ''''    Dim strTableContent As StringBuilder = New StringBuilder()
                        ''''    Dim strTable_Cont As StringBuilder = New StringBuilder()
                        ''''    'Dim strTableContentALL As StringBuilder = New StringBuilder()
                        ''''    Dim strTable As StringBuilder = New StringBuilder()

                        ''''    '<!--##CONTENT##-->;<!--##ROLE_NAME##-->;<!--##EMPLOYEE_NAME##-->;<!--##STATE##-->;<!--##DATE_RECEIPT##-->;<!--##ALERT_TYPE##-->;<!--##STRONG_OPEN##-->;<!--##STRONG_OPEN##-->;
                        ''''    strTable.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2))

                        ''''    SHours = 0
                        ''''    For Each seg_dtRow In Tbl_Ta_RutaSeguimiento.Rows

                        ''''        strTable_Cont.Clear()
                        ''''        strTable_Cont.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 2))


                        ''''        'strTableContent.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                        ''''        strTable_Cont.Replace("<!--##ROLE_NAME##-->", "")
                        ''''        strTable_Cont.Replace("<!--##EMPLOYEE_NAME##-->", seg_dtRow("nombre_empleado"))
                        ''''        strTable_Cont.Replace("<!--##STATE##-->", seg_dtRow("descripcion_estado"))
                        ''''        strTable_Cont.Replace("<!--##DATE_RECEIPT##-->", seg_dtRow("fecha_aprobacion"))
                        ''''        strTable_Cont.Replace("<!--##ALERT_TYPE##-->", seg_dtRow("Alerta"))

                        ''''        'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                        ''''        If ((tbl_document_rep.Rows(0).Item("id_ruta") = seg_dtRow("id_ruta")) And (tbl_document_rep.Rows(0).Item("id_estadoDoc") = seg_dtRow("id_estadoDoc"))) Then
                        ''''            strTable_Cont.Replace("<tr>", strStyle)
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "<strong>")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "</strong>")
                        ''''        Else
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "")
                        ''''        End If

                        ''''        SHours += seg_dtRow("NHRs")
                        ''''        NDays = Math.Round((seg_dtRow("NHRs") / 24), 0, MidpointRounding.AwayFromZero)

                        ''''        strTable_Cont.Replace("<!--##DAYS##-->", NDays)

                        ''''        strTableContent.Append(strTable_Cont.ToString)


                        ''''    Next

                        ''''    NDays = Int((SHours / 24))
                        ''''    NHours = Math.Round((((SHours / 24) - NDays) * 24), 0, MidpointRounding.AwayFromZero)

                        ''''    'strTableContentALL.Append(strTableContent)

                        ''''    strTable.Replace("<!--##TOTAL_DAYS##-->", String.Format(" {0} Days {1} Hrs", NDays, NHours))
                        ''''    strTable.Replace("<!--##CONTENT##-->", strTableContent.ToString)

                        ''''    strContent.Replace("<!--##TABLE_STATUS##-->", strTable.ToString)
                        ''''    strContentALL.Append(strContent.ToString)
                        ''''    strContentALL.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_nwline", 1))

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    'Add_emails(dtR2("id_documento"), dtR2("id_estadoDoc"), idOriginator, boolSetEmailList) 'Adding the respective email according the state of the approval proccess
                        ''''    'Add_document_email_list(dtR2("id_documento"), boolSetEmailList)
                        ''''    '*****************FINDING the Destinataries **************************************
                        ''''    'boolSetEmailList = False

                        ''''    '**************TESTING PRUPORSES
                        ''''    'Exit For
                        ''''    '**************TESTING PRUPORSES

                        ''''Next

                        ''''strMess.Replace("<!--##CONTENT##-->", strContentALL.ToString)


                        'Add_emails(idOriginator, "User") 'Adding the respective email according the user or the Role

                        If ObJemail.SilentMode Then
                            'If Not ObJemail.SilentMode Then

                            strSubject &= " (Silent MODE) "
                            Dim strTO As StringBuilder = New StringBuilder
                            Dim strCC As String = ""
                            Dim strBCC As String = ""

                            '*********************************************************************************************************
                            '******************we can do this getting all the selection List......************************************
                            '*********************************************************************************************************

                            ' For Each dtRow In tbl_emails.Rows

                            'Select Case dtRow("Tipo")
                            '    Case "TO"
                            '        strTO &= String.Format("{0};", dtRow("email").ToString.Trim)
                            '    Case "CC"
                            '        strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                            '    Case "BCC"
                            '        strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                            'End Select

                            strTO.Append(String.Format("{0};", EmailOriginator.ToString.Trim))

                            If SecundaryEmail.Trim.Length > 1 Then
                                strTO.Append(String.Format("{0};", SecundaryEmail.ToString.Trim))
                            End If


                            ' Next

                            ' Dim strEMAILS As String = String.Format("<br /><br /><br /><br /> TO:{0}<br />CC:{1}<br />BCC:{2} ", strTO, strCC, strBCC)
                            Dim strEMAILS As StringBuilder = New StringBuilder
                            strEMAILS.Append("<br /><br /><br /><br />TO:" & strTO.ToString & "<br /><br />")

                            strMess.Replace("<!--##MESS##-->", strEMAILS.ToString) 'Representavite emails for this  notification

                        Else


                            strMess.Replace("<!--##MESS##-->", "")
                            '*********************************************************************************************************
                            '*******************THIS IS the Individual Email for every Notification***********************************
                            ObJemail.AddTo(EmailOriginator)

                            If SecundaryEmail.Trim.Length > 1 Then
                                ObJemail.AddTo(SecundaryEmail.ToString)
                            End If
                            '*******************THIS IS the Individual Email for every notification***********************************
                            '*********************************************************************************************************

                        End If


                        '*******************************************************************
                        '************************SUBJECTS***********************************
                        ObJemail.AddSubject(strSubject)
                        '************************SUBJECTS***********************************
                        '*******************************************************************

                        '************************* THE message it supposed Is here, need to proceed to test*****************
                        '*****************building the email from the templates objects**************************************

                        strMess.Replace("<!--##IMG_CID2##-->", "LogProgram") 'Name of the resource the Program Logo
                        strMess.Replace("<!--##IMG_CID1##-->", "LogChemo") 'Name of the resource the Company logo

                        Dim strPath As String
                        If strBaseDir.Length > 1 Then
                            strPath = strBaseDir
                        Else
                            strPath = Server.MapPath("~")
                        End If

                        Dim strTMP As String = strMess.ToString

                        ObJemail.setAlternativeVIEW(strMess.ToString) 'Setting alternative View

                        Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                        Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                        If Not String.IsNullOrEmpty(strProgramIMG) Then
                            ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
                        Else
                            ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
                        End If

                        'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                        ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")


                        'ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Red_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Blue_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Orange_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Cyan_Time.png", "image/png")

                        ObJemail.SetPriority(MailPriority.High)

                        If ObJemail.SendEmail(id_notification, oSolicitationAPP.FirstOrDefault.ID_SOLICITATION_APP, "SOLICITATION", "MODIFICATION") Then
                            NOTIFIYING_SOLICITATION_MOD = True
                        Else
                            NOTIFIYING_SOLICITATION_MOD = False
                        End If

                    Else  '*********************Valid Email addres**********************************

                        NOTIFIYING_SOLICITATION_MOD = False

                    End If


                End Using

            Catch ex As Exception

                NOTIFIYING_SOLICITATION_MOD = False

            End Try


        End Function



        Public Function NOTIFIYING_SOLICITATION_OP(ByVal id_notification_app As Integer, Optional strBaseDir As String = "") As Boolean

            Dim strMess As StringBuilder = New StringBuilder


            Try

                Using dbEntities As New dbRMS_JIEntities

                    Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).ToList()
                    Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(oSolicitationAPP.FirstOrDefault.ID_ACTIVITY_SOLICITATION)

                    ' Dim EmailOriginator As String = oSolicitationAPP.FirstOrDefault.ORGANIZATIONEMAIL
                    ' Dim SecundaryEmail As String = oSolicitationAPP.FirstOrDefault.PRIMARYCONTACTEMAIL

                    Dim EmailOriginator As String = ""
                    Dim SecundaryEmail As String = ""

                    Dim tbl_ As DataTable = get_t_notification_emails("CNTMNG")
                    Set_Emails_List()

                    If Not IsNothing(tbl_) Then

                        Add_Emails_List("TO", tbl_.Rows.Item(0).Item("email"))
                        EmailOriginator = tbl_.Rows.Item(0).Item("email")

                        Dim strBccEmails As String() = If(IsDBNull(tbl_.Rows.Item(0).Item("noti_cc")), "", tbl_.Rows.Item(0).Item("noti_cc")).Split(";")


                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    'ObJemail.AddBcc(dtEmail)
                                    Add_Emails_List("CC", dtEmail)

                                End If

                            End If

                        Next
                    End If


                    If IsEmail(EmailOriginator) Then

                        'Dim tbl_approvalCC As DataTable = get_notification_Pending_ByUser(0, "CC", EmailOriginator.Trim, 0) '--Getting the notifications and sending to its respective sender
                        'Dim boolFound As Boolean = False

                        'For Each app_CC_dtRow As DataRow In tbl_EmailIncluded.Rows

                        '    If app_CC_dtRow("type_subject") = "CC" And app_CC_dtRow("email").ToString.Trim = EmailOriginator.Trim Then

                        '        For Each app_pend_dtRow As DataRow In tbl_approvalPending.Rows

                        '            If app_pend_dtRow("id_documento") = app_CC_dtRow("id_documento") Then
                        '                boolFound = True
                        '                Exit For
                        '            End If

                        '        Next

                        '        If Not boolFound Then
                        '            tbl_approvalPending.ImportRow(app_CC_dtRow)
                        '        End If

                        '        boolFound = False


                        '    End If

                        'Next


                        '************************************SYSTEM INFO********************************************
                        Dim cProgram As New RMS.cls_Program
                        cProgram.get_Sys(0, True)
                        cProgram.get_Programs(id_programa, True)
                        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
                        dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                        '************************************SYSTEM INFO********************************************

                        '*****************Creating the email Object**************************************
                        ObJemail = New UTILS.cls_email(id_programa)
                        '*****************Creating the email Object**************************************

                        '*****************building the email from the templates objects**************************************
                        set_t_notification(id_notification) 'Create the Noti_Object
                        set_t_Template() 'Set Template object from the notification id
                        strMess.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)) 'getting the first template

                        strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

                        '*****************FINDING the BCC destinataries **************************************
                        Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    ObJemail.AddBcc(dtEmail)
                                End If

                            End If

                        Next
                        ''*****************FINDING the BCC destinataries **************************************



                        '*****************ADDING THE SUBJECTS**********************
                        Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

                        strSubject = strSubject.Replace("<!--##SOLICITATION_NUMBER##-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)
                        strSubject = strSubject.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strSubject = strSubject.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)

                        '<!--##PROJECT_NAME##-->)


                        '<!--##ORGANIZATION_NAME##-->
                        '<!--##DESTINATION_EMAIL##-->
                        '<!--#SOLICITATION##-->
                        '<!--#SOLICITATION_TITLE##-->
                        '<!--#SOLICITATION_PURPOSE##-->
                        '<!--##SOLICITATION_NUMBER##-->
                        '<!--#START_DATE##-->
                        '<!--END_DATE##-->
                        '<!--##SYS_PATH##-->
                        '<!--##ID_DOC##-->

                        strMess.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strMess.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        strMess.Replace("<!--##DESTINATION_EMAIL##-->", String.Format("{0};{1};", EmailOriginator, SecundaryEmail))
                        strMess.Replace("<!--#SOLICITATION##-->", oActivitySolicitation.SOLICITATION)
                        strMess.Replace("<!--#SOLICITATION_PURPOSE##-->", oActivitySolicitation.SOLICITATION_PURPOSE)
                        strMess.Replace("<!--##SOLICITATION_NUMBER##-->", oActivitySolicitation.SOLICITATION_CODE)

                        strMess.Replace("<!--#START_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.start_date, "f", timezoneUTC, True))
                        strMess.Replace("<!--END_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.end_date, "f", timezoneUTC, True))

                        strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys))

                        Dim URL_Keys As String = String.Format("id={3}&ab={0}&ac={1}&ad={2}", oActivitySolicitation.SOLICITATION_TOKEN.ToString, oSolicitationAPP.FirstOrDefault.SOLICITATION_TOKEN, oSolicitationAPP.FirstOrDefault.USER_TOKEN_UPDATE.ToString, oActivitySolicitation.ID_ACTIVITY)
                        strMess.Replace("<!--##ID_DOC##-->", URL_Keys)

                        strMess.Replace("<!--RECEIVED_DATE##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SENT_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--OPENED_BY##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        'strMess.Replace("<!--OPENED_DATE##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.RECEIVED_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--OPENED_DATE##-->", dateUtil.set_DateFormat(Date.UtcNow, "f", timezoneUTC, True))


                        ' ; 
                        ''''Dim strContentALL As StringBuilder = New StringBuilder()
                        ''''Dim boolSetEmailList As Boolean = True
                        ''''Dim tbl_document_rep As DataTable

                        ''''Dim NDays As Decimal
                        ''''Dim NHours As Decimal
                        ''''Dim SHours As Decimal

                        '''''--Fore each pending approval started to build the EMail
                        '''''<!--##CATEGORY##-->;<!--##APPROVAL_NAME##-->;<!--##INSTRUMENT_NUMBER##-->;<!--##DESCRIPTION_DOC##-->;<!--##TABLE_STATUS##-->

                        ''''For Each tbl_approvalPending_dtR As DataRow In tbl_approvalPending.Rows

                        ''''    '************************************CONTENT**************************************************************************
                        ''''    Dim strContent As StringBuilder = New StringBuilder()
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    tbl_document_rep = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", tbl_approvalPending_dtR("id_documento"))
                        ''''    'tbl_document_rep.Rows(0).Item(
                        ''''    '************************************CONTENT**************************************************************************
                        ''''    strContent.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1))
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    'set_Apps_Document(dtR2("id_documento")) 'Set all AppDoc
                        ''''    '*****************FINDING the Destinataries **************************************

                        ''''    strContent.Replace("<!--##CATEGORY##-->", tbl_document_rep.Rows(0).Item("descripcion_cat"))
                        ''''    strContent.Replace("<!--##APPROVAL_NAME##-->", tbl_document_rep.Rows(0).Item("descripcion_aprobacion"))

                        ''''    strContent.Replace("<!--##ID_DOC##-->", tbl_document_rep.Rows(0).Item("id_documento"))
                        ''''    strContent.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

                        ''''    strContent.Replace("<!--##INSTRUMENT_NUMBER##-->", tbl_document_rep.Rows(0).Item("numero_instrumento"))

                        ''''    Dim strDocumentDesc As String = tbl_document_rep.Rows(0).Item("descripcion_doc") & fn_get_EmailList(tbl_approvalPending_dtR("id_documento"))
                        ''''    strContent.Replace("<!--##DESCRIPTION_DOC##-->", strDocumentDesc)

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    '*****************<!--##TABLE_STATUS##-->'*****************
                        ''''    set_Ta_RutaSeguimiento(tbl_document_rep.Rows(0).Item("id_documento")) 'getting the complete route

                        ''''    Dim strStyle As String = "<tr style='background-color:#ee7108;border:1 dotted #FF0000; ' >" 'Putting inside <tr>
                        ''''    'style="background-color:#ee7108;border:1 dotted #FF0000;"
                        ''''    Dim strTableContent As StringBuilder = New StringBuilder()
                        ''''    Dim strTable_Cont As StringBuilder = New StringBuilder()
                        ''''    'Dim strTableContentALL As StringBuilder = New StringBuilder()
                        ''''    Dim strTable As StringBuilder = New StringBuilder()

                        ''''    '<!--##CONTENT##-->;<!--##ROLE_NAME##-->;<!--##EMPLOYEE_NAME##-->;<!--##STATE##-->;<!--##DATE_RECEIPT##-->;<!--##ALERT_TYPE##-->;<!--##STRONG_OPEN##-->;<!--##STRONG_OPEN##-->;
                        ''''    strTable.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2))

                        ''''    SHours = 0
                        ''''    For Each seg_dtRow In Tbl_Ta_RutaSeguimiento.Rows

                        ''''        strTable_Cont.Clear()
                        ''''        strTable_Cont.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 2))


                        ''''        'strTableContent.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                        ''''        strTable_Cont.Replace("<!--##ROLE_NAME##-->", "")
                        ''''        strTable_Cont.Replace("<!--##EMPLOYEE_NAME##-->", seg_dtRow("nombre_empleado"))
                        ''''        strTable_Cont.Replace("<!--##STATE##-->", seg_dtRow("descripcion_estado"))
                        ''''        strTable_Cont.Replace("<!--##DATE_RECEIPT##-->", seg_dtRow("fecha_aprobacion"))
                        ''''        strTable_Cont.Replace("<!--##ALERT_TYPE##-->", seg_dtRow("Alerta"))

                        ''''        'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                        ''''        If ((tbl_document_rep.Rows(0).Item("id_ruta") = seg_dtRow("id_ruta")) And (tbl_document_rep.Rows(0).Item("id_estadoDoc") = seg_dtRow("id_estadoDoc"))) Then
                        ''''            strTable_Cont.Replace("<tr>", strStyle)
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "<strong>")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "</strong>")
                        ''''        Else
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "")
                        ''''        End If

                        ''''        SHours += seg_dtRow("NHRs")
                        ''''        NDays = Math.Round((seg_dtRow("NHRs") / 24), 0, MidpointRounding.AwayFromZero)

                        ''''        strTable_Cont.Replace("<!--##DAYS##-->", NDays)

                        ''''        strTableContent.Append(strTable_Cont.ToString)


                        ''''    Next

                        ''''    NDays = Int((SHours / 24))
                        ''''    NHours = Math.Round((((SHours / 24) - NDays) * 24), 0, MidpointRounding.AwayFromZero)

                        ''''    'strTableContentALL.Append(strTableContent)

                        ''''    strTable.Replace("<!--##TOTAL_DAYS##-->", String.Format(" {0} Days {1} Hrs", NDays, NHours))
                        ''''    strTable.Replace("<!--##CONTENT##-->", strTableContent.ToString)

                        ''''    strContent.Replace("<!--##TABLE_STATUS##-->", strTable.ToString)
                        ''''    strContentALL.Append(strContent.ToString)
                        ''''    strContentALL.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_nwline", 1))

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    'Add_emails(dtR2("id_documento"), dtR2("id_estadoDoc"), idOriginator, boolSetEmailList) 'Adding the respective email according the state of the approval proccess
                        ''''    'Add_document_email_list(dtR2("id_documento"), boolSetEmailList)
                        ''''    '*****************FINDING the Destinataries **************************************
                        ''''    'boolSetEmailList = False

                        ''''    '**************TESTING PRUPORSES
                        ''''    'Exit For
                        ''''    '**************TESTING PRUPORSES

                        ''''Next

                        ''''strMess.Replace("<!--##CONTENT##-->", strContentALL.ToString)


                        'Add_emails(idOriginator, "User") 'Adding the respective email according the user or the Role

                        If ObJemail.SilentMode Then
                            'If Not ObJemail.SilentMode Then

                            strSubject &= " (Silent MODE) "
                            Dim strTO As StringBuilder = New StringBuilder
                            Dim strCC As String = ""
                            Dim strBCC As String = ""

                            '*********************************************************************************************************
                            '******************we can do this getting all the selection List......************************************
                            '*********************************************************************************************************

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        strTO.Append(String.Format("{0};", dtRow("email").ToString.Trim))
                                    Case "CC"
                                        strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                End Select

                                ' strTO.Append(String.Format("{0};", EmailOriginator.ToString.Trim))

                                'If SecundaryEmail.Trim.Length > 1 Then
                                '    strTO.Append(String.Format("{0};", SecundaryEmail.ToString.Trim))
                                'End If

                            Next

                            ' Dim strEMAILS As String = String.Format("<br /><br /><br /><br /> TO:{0}<br />CC:{1}<br />BCC:{2} ", strTO, strCC, strBCC)
                            Dim strEMAILS As StringBuilder = New StringBuilder
                            strEMAILS.Append("<br /><br /><br /><br />TO:" & strTO.ToString & "<br /><br />")

                            strMess.Replace("<!--##MESS##-->", strEMAILS.ToString) 'Representavite emails for this  notification

                        Else

                            strMess.Replace("<!--##MESS##-->", "")
                            '*********************************************************************************************************
                            '*******************THIS IS the Individual Email for every Notification***********************************
                            'ObJemail.AddTo(EmailOriginator)

                            'If SecundaryEmail.Trim.Length > 1 Then
                            '    ObJemail.AddTo(SecundaryEmail.ToString)
                            'End If

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        ObJemail.AddTo(dtRow("email").ToString.Trim)
                                    Case "CC"
                                        ObJemail.AddCC(dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        ObJemail.AddBcc(dtRow("email").ToString.Trim)
                                End Select

                            Next

                            '*******************THIS IS the Individual Email for every notification***********************************
                            '*********************************************************************************************************

                        End If

                        '*******************************************************************
                        '************************SUBJECTS***********************************
                        ObJemail.AddSubject(strSubject)
                        '************************SUBJECTS***********************************
                        '*******************************************************************

                        '************************* THE message it supposed Is here, need to proceed to test*****************
                        '*****************building the email from the templates objects**************************************

                        strMess.Replace("<!--##IMG_CID2##-->", "LogProgram") 'Name of the resource the Program Logo
                        strMess.Replace("<!--##IMG_CID1##-->", "LogChemo") 'Name of the resource the Company logo

                        Dim strPath As String
                        If strBaseDir.Length > 1 Then
                            strPath = strBaseDir
                        Else
                            strPath = Server.MapPath("~")
                        End If

                        Dim strTMP As String = strMess.ToString

                        ObJemail.setAlternativeVIEW(strMess.ToString) 'Setting alternative View

                        Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                        Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                        If Not String.IsNullOrEmpty(strProgramIMG) Then
                            ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
                        Else
                            ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
                        End If

                        'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                        ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

                        'ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Red_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Blue_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Orange_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Cyan_Time.png", "image/png")

                        ObJemail.SetPriority(MailPriority.High)

                        If ObJemail.SendEmail(id_notification, oSolicitationAPP.FirstOrDefault.ID_SOLICITATION_APP, "SOLICITATION_EMAIL", "SOLICITATION OPENED") Then
                            NOTIFIYING_SOLICITATION_OP = True
                        Else
                            NOTIFIYING_SOLICITATION_OP = False
                        End If

                    Else  '*********************Valid Email addres**********************************

                        NOTIFIYING_SOLICITATION_OP = False

                    End If


                End Using

            Catch ex As Exception

                NOTIFIYING_SOLICITATION_OP = False

            End Try


        End Function





        Public Function NOTIFIYING_SOLICITATION_STARTED(ByVal id_notification_app As Integer, Optional strBaseDir As String = "") As Boolean

            Dim strMess As StringBuilder = New StringBuilder


            Try

                Using dbEntities As New dbRMS_JIEntities

                    Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).ToList()
                    Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(oSolicitationAPP.FirstOrDefault.ID_ACTIVITY_SOLICITATION)

                    ' Dim EmailOriginator As String = oSolicitationAPP.FirstOrDefault.ORGANIZATIONEMAIL
                    ' Dim SecundaryEmail As String = oSolicitationAPP.FirstOrDefault.PRIMARYCONTACTEMAIL

                    Dim EmailOriginator As String = ""
                    Dim SecundaryEmail As String = ""

                    Dim tbl_ As DataTable = get_t_notification_emails("CNTMNG")
                    Set_Emails_List()

                    If Not IsNothing(tbl_) Then

                        Add_Emails_List("TO", tbl_.Rows.Item(0).Item("email"))
                        EmailOriginator = tbl_.Rows.Item(0).Item("email")

                        Dim strBccEmails As String() = If(IsDBNull(tbl_.Rows.Item(0).Item("noti_cc")), "", tbl_.Rows.Item(0).Item("noti_cc")).Split(";")


                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    'ObJemail.AddBcc(dtEmail)
                                    Add_Emails_List("CC", dtEmail)

                                End If

                            End If

                        Next
                    End If


                    If IsEmail(EmailOriginator) Then

                        'Dim tbl_approvalCC As DataTable = get_notification_Pending_ByUser(0, "CC", EmailOriginator.Trim, 0) '--Getting the notifications and sending to its respective sender
                        'Dim boolFound As Boolean = False

                        'For Each app_CC_dtRow As DataRow In tbl_EmailIncluded.Rows

                        '    If app_CC_dtRow("type_subject") = "CC" And app_CC_dtRow("email").ToString.Trim = EmailOriginator.Trim Then

                        '        For Each app_pend_dtRow As DataRow In tbl_approvalPending.Rows

                        '            If app_pend_dtRow("id_documento") = app_CC_dtRow("id_documento") Then
                        '                boolFound = True
                        '                Exit For
                        '            End If

                        '        Next

                        '        If Not boolFound Then
                        '            tbl_approvalPending.ImportRow(app_CC_dtRow)
                        '        End If

                        '        boolFound = False


                        '    End If

                        'Next



                        '************************************SYSTEM INFO********************************************
                        Dim cProgram As New RMS.cls_Program
                        cProgram.get_Sys(0, True)
                        cProgram.get_Programs(id_programa, True)
                        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
                        dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                        '************************************SYSTEM INFO********************************************

                        '*****************Creating the email Object**************************************
                        ObJemail = New UTILS.cls_email(id_programa)
                        '*****************Creating the email Object**************************************

                        '*****************building the email from the templates objects**************************************
                        set_t_notification(id_notification) 'Create the Noti_Object
                        set_t_Template() 'Set Template object from the notification id
                        strMess.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)) 'getting the first template

                        strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

                        '*****************FINDING the BCC destinataries **************************************
                        Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    ObJemail.AddBcc(dtEmail)
                                End If

                            End If

                        Next
                        ''*****************FINDING the BCC destinataries **************************************



                        '*****************ADDING THE SUBJECTS**********************
                        Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

                        strSubject = strSubject.Replace("<!--##APPLICATION_NUMBER##-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)
                        strSubject = strSubject.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strSubject = strSubject.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)

                        '<!--##PROJECT_NAME##-->)


                        '<!--##ORGANIZATION_NAME##-->
                        '<!--##DESTINATION_EMAIL##-->
                        '<!--#SOLICITATION##-->
                        '<!--#SOLICITATION_TITLE##-->
                        '<!--#SOLICITATION_PURPOSE##-->
                        '<!--##SOLICITATION_NUMBER##-->
                        '<!--#START_DATE##-->
                        '<!--END_DATE##-->
                        '<!--##SYS_PATH##-->
                        '<!--##ID_DOC##-->

                        strMess.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strMess.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        strMess.Replace("<!--##DESTINATION_EMAIL##-->", String.Format("{0};{1};", EmailOriginator, SecundaryEmail))
                        strMess.Replace("<!--#SOLICITATION##-->", oActivitySolicitation.SOLICITATION)
                        strMess.Replace("<!--#SOLICITATION_PURPOSE##-->", oActivitySolicitation.SOLICITATION_PURPOSE)
                        strMess.Replace("<!--##SOLICITATION_NUMBER##-->", oActivitySolicitation.SOLICITATION_CODE)

                        strMess.Replace("<!--#START_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.start_date, "f", timezoneUTC, True))
                        strMess.Replace("<!--END_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.end_date, "f", timezoneUTC, True))

                        strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys))

                        Dim URL_Keys As String = String.Format("id={3}&ab={0}&ac={1}&ad={2}", oActivitySolicitation.SOLICITATION_TOKEN.ToString, oSolicitationAPP.FirstOrDefault.SOLICITATION_TOKEN, oSolicitationAPP.FirstOrDefault.USER_TOKEN_UPDATE.ToString, oActivitySolicitation.ID_ACTIVITY)
                        strMess.Replace("<!--##ID_DOC##-->", URL_Keys)


                        strMess.Replace("<!--##APPLICATION_NUMBER##-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)

                        strMess.Replace("<!--##RECEIVED_DATE##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SENT_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--##STARTED_BY##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        'strMess.Replace("<!--OPENED_DATE##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.RECEIVED_DATE, "f", timezoneUTC, True))
                        ' strMess.Replace("<!--OPENED_DATE##-->", dateUtil.set_DateFormat(Date.UtcNow, "f", timezoneUTC, True))
                        strMess.Replace("<!--##STARTED_DATE##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.fecha_crea, "f", timezoneUTC, True))


                        ' ; 
                        ''''Dim strContentALL As StringBuilder = New StringBuilder()
                        ''''Dim boolSetEmailList As Boolean = True
                        ''''Dim tbl_document_rep As DataTable

                        ''''Dim NDays As Decimal
                        ''''Dim NHours As Decimal
                        ''''Dim SHours As Decimal

                        '''''--Fore each pending approval started to build the EMail
                        '''''<!--##CATEGORY##-->;<!--##APPROVAL_NAME##-->;<!--##INSTRUMENT_NUMBER##-->;<!--##DESCRIPTION_DOC##-->;<!--##TABLE_STATUS##-->

                        ''''For Each tbl_approvalPending_dtR As DataRow In tbl_approvalPending.Rows

                        ''''    '************************************CONTENT**************************************************************************
                        ''''    Dim strContent As StringBuilder = New StringBuilder()
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    tbl_document_rep = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", tbl_approvalPending_dtR("id_documento"))
                        ''''    'tbl_document_rep.Rows(0).Item(
                        ''''    '************************************CONTENT**************************************************************************
                        ''''    strContent.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1))
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    'set_Apps_Document(dtR2("id_documento")) 'Set all AppDoc
                        ''''    '*****************FINDING the Destinataries **************************************

                        ''''    strContent.Replace("<!--##CATEGORY##-->", tbl_document_rep.Rows(0).Item("descripcion_cat"))
                        ''''    strContent.Replace("<!--##APPROVAL_NAME##-->", tbl_document_rep.Rows(0).Item("descripcion_aprobacion"))

                        ''''    strContent.Replace("<!--##ID_DOC##-->", tbl_document_rep.Rows(0).Item("id_documento"))
                        ''''    strContent.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

                        ''''    strContent.Replace("<!--##INSTRUMENT_NUMBER##-->", tbl_document_rep.Rows(0).Item("numero_instrumento"))

                        ''''    Dim strDocumentDesc As String = tbl_document_rep.Rows(0).Item("descripcion_doc") & fn_get_EmailList(tbl_approvalPending_dtR("id_documento"))
                        ''''    strContent.Replace("<!--##DESCRIPTION_DOC##-->", strDocumentDesc)

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    '*****************<!--##TABLE_STATUS##-->'*****************
                        ''''    set_Ta_RutaSeguimiento(tbl_document_rep.Rows(0).Item("id_documento")) 'getting the complete route

                        ''''    Dim strStyle As String = "<tr style='background-color:#ee7108;border:1 dotted #FF0000; ' >" 'Putting inside <tr>
                        ''''    'style="background-color:#ee7108;border:1 dotted #FF0000;"
                        ''''    Dim strTableContent As StringBuilder = New StringBuilder()
                        ''''    Dim strTable_Cont As StringBuilder = New StringBuilder()
                        ''''    'Dim strTableContentALL As StringBuilder = New StringBuilder()
                        ''''    Dim strTable As StringBuilder = New StringBuilder()

                        ''''    '<!--##CONTENT##-->;<!--##ROLE_NAME##-->;<!--##EMPLOYEE_NAME##-->;<!--##STATE##-->;<!--##DATE_RECEIPT##-->;<!--##ALERT_TYPE##-->;<!--##STRONG_OPEN##-->;<!--##STRONG_OPEN##-->;
                        ''''    strTable.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2))

                        ''''    SHours = 0
                        ''''    For Each seg_dtRow In Tbl_Ta_RutaSeguimiento.Rows

                        ''''        strTable_Cont.Clear()
                        ''''        strTable_Cont.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 2))


                        ''''        'strTableContent.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                        ''''        strTable_Cont.Replace("<!--##ROLE_NAME##-->", "")
                        ''''        strTable_Cont.Replace("<!--##EMPLOYEE_NAME##-->", seg_dtRow("nombre_empleado"))
                        ''''        strTable_Cont.Replace("<!--##STATE##-->", seg_dtRow("descripcion_estado"))
                        ''''        strTable_Cont.Replace("<!--##DATE_RECEIPT##-->", seg_dtRow("fecha_aprobacion"))
                        ''''        strTable_Cont.Replace("<!--##ALERT_TYPE##-->", seg_dtRow("Alerta"))

                        ''''        'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                        ''''        If ((tbl_document_rep.Rows(0).Item("id_ruta") = seg_dtRow("id_ruta")) And (tbl_document_rep.Rows(0).Item("id_estadoDoc") = seg_dtRow("id_estadoDoc"))) Then
                        ''''            strTable_Cont.Replace("<tr>", strStyle)
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "<strong>")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "</strong>")
                        ''''        Else
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "")
                        ''''        End If

                        ''''        SHours += seg_dtRow("NHRs")
                        ''''        NDays = Math.Round((seg_dtRow("NHRs") / 24), 0, MidpointRounding.AwayFromZero)

                        ''''        strTable_Cont.Replace("<!--##DAYS##-->", NDays)

                        ''''        strTableContent.Append(strTable_Cont.ToString)


                        ''''    Next

                        ''''    NDays = Int((SHours / 24))
                        ''''    NHours = Math.Round((((SHours / 24) - NDays) * 24), 0, MidpointRounding.AwayFromZero)

                        ''''    'strTableContentALL.Append(strTableContent)

                        ''''    strTable.Replace("<!--##TOTAL_DAYS##-->", String.Format(" {0} Days {1} Hrs", NDays, NHours))
                        ''''    strTable.Replace("<!--##CONTENT##-->", strTableContent.ToString)

                        ''''    strContent.Replace("<!--##TABLE_STATUS##-->", strTable.ToString)
                        ''''    strContentALL.Append(strContent.ToString)
                        ''''    strContentALL.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_nwline", 1))

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    'Add_emails(dtR2("id_documento"), dtR2("id_estadoDoc"), idOriginator, boolSetEmailList) 'Adding the respective email according the state of the approval proccess
                        ''''    'Add_document_email_list(dtR2("id_documento"), boolSetEmailList)
                        ''''    '*****************FINDING the Destinataries **************************************
                        ''''    'boolSetEmailList = False

                        ''''    '**************TESTING PRUPORSES
                        ''''    'Exit For
                        ''''    '**************TESTING PRUPORSES

                        ''''Next

                        ''''strMess.Replace("<!--##CONTENT##-->", strContentALL.ToString)


                        'Add_emails(idOriginator, "User") 'Adding the respective email according the user or the Role

                        If ObJemail.SilentMode Then
                            'If Not ObJemail.SilentMode Then

                            strSubject &= " (Silent MODE) "
                            Dim strTO As StringBuilder = New StringBuilder
                            Dim strCC As String = ""
                            Dim strBCC As String = ""

                            '*********************************************************************************************************
                            '******************we can do this getting all the selection List......************************************
                            '*********************************************************************************************************

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        strTO.Append(String.Format("{0};", dtRow("email").ToString.Trim))
                                    Case "CC"
                                        strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                End Select

                            Next

                            ' Dim strEMAILS As String = String.Format("<br /><br /><br /><br /> TO:{0}<br />CC:{1}<br />BCC:{2} ", strTO, strCC, strBCC)
                            Dim strEMAILS As StringBuilder = New StringBuilder
                            strEMAILS.Append("<br /><br /><br /><br />TO:" & strTO.ToString & "<br /><br />")

                            strMess.Replace("<!--##MESS##-->", strEMAILS.ToString) 'Representavite emails for this  notification

                        Else

                            strMess.Replace("<!--##MESS##-->", "")
                            '*********************************************************************************************************
                            '*******************THIS IS the Individual Email for every Notification***********************************
                            'ObJemail.AddTo(EmailOriginator)

                            'If SecundaryEmail.Trim.Length > 1 Then
                            '    ObJemail.AddTo(SecundaryEmail.ToString)
                            'End If

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        ObJemail.AddTo(dtRow("email").ToString.Trim)
                                    Case "CC"
                                        ObJemail.AddCC(dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        ObJemail.AddBcc(dtRow("email").ToString.Trim)
                                End Select

                            Next

                            '*******************THIS IS the Individual Email for every notification***********************************
                            '*********************************************************************************************************

                        End If

                        '*******************************************************************
                        '************************SUBJECTS***********************************
                        ObJemail.AddSubject(strSubject)
                        '************************SUBJECTS***********************************
                        '*******************************************************************

                        '************************* THE message it supposed Is here, need to proceed to test*****************
                        '*****************building the email from the templates objects**************************************

                        strMess.Replace("<!--##IMG_CID2##-->", "LogProgram") 'Name of the resource the Program Logo
                        strMess.Replace("<!--##IMG_CID1##-->", "LogChemo") 'Name of the resource the Company logo

                        Dim strPath As String
                        If strBaseDir.Length > 1 Then
                            strPath = strBaseDir
                        Else
                            strPath = Server.MapPath("~")
                        End If

                        Dim strTMP As String = strMess.ToString

                        ObJemail.setAlternativeVIEW(strMess.ToString) 'Setting alternative View

                        Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                        Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                        If Not String.IsNullOrEmpty(strProgramIMG) Then
                            ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
                        Else
                            ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
                        End If

                        'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                        ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

                        'ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Red_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Blue_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Orange_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Cyan_Time.png", "image/png")

                        ObJemail.SetPriority(MailPriority.High)

                        If ObJemail.SendEmail(id_notification, oSolicitationAPP.FirstOrDefault.ID_SOLICITATION_APP, "SOLICITATION_EMAIL", "APPLICATION STARTED") Then
                            NOTIFIYING_SOLICITATION_STARTED = True
                        Else
                            NOTIFIYING_SOLICITATION_STARTED = False
                        End If

                    Else  '*********************Valid Email addres**********************************

                        NOTIFIYING_SOLICITATION_STARTED = False

                    End If


                End Using

            Catch ex As Exception

                NOTIFIYING_SOLICITATION_STARTED = False

            End Try


        End Function



        Public Function NOTIFIYING_SOLICITATION_RESPONSE(ByVal id_notification_app As Integer, ByVal idStatus As Integer, Optional strBaseDir As String = "") As Boolean

            Dim strMess As StringBuilder = New StringBuilder


            Try

                Using dbEntities As New dbRMS_JIEntities

                    Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).ToList()
                    Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(oSolicitationAPP.FirstOrDefault.ID_ACTIVITY_SOLICITATION)
                    Dim oSolicitationApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).FirstOrDefault()

                    Dim oApplyComm = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).FirstOrDefault()

                    Dim EmailOriginator As String = oSolicitationAPP.FirstOrDefault.ORGANIZATIONEMAIL
                    Dim SecundaryEmail As String = oSolicitationAPP.FirstOrDefault.PRIMARYCONTACTEMAIL

                    'Dim EmailOriginator As String = ""
                    'Dim SecundaryEmail As String = ""
                    Set_Emails_List()

                    Add_Emails_List("TO", EmailOriginator)
                    Add_Emails_List("CC", SecundaryEmail)

                    Dim tbl_ As DataTable = get_t_notification_emails("CNTMNG")

                    If Not IsNothing(tbl_) Then

                        Add_Emails_List("CC", tbl_.Rows.Item(0).Item("noti_email"))
                        ' EmailOriginator = tbl_.Rows.Item(0).Item("email")

                        Dim strBccEmails As String() = If(IsDBNull(tbl_.Rows.Item(0).Item("noti_cc")), "", tbl_.Rows.Item(0).Item("noti_cc")).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    'ObJemail.AddBcc(dtEmail)
                                    Add_Emails_List("CC", dtEmail)
                                    'SecundaryEmail &= String.Format("{0};", dtEmail)

                                End If

                            End If

                        Next

                    End If


                    If IsEmail(EmailOriginator) Then

                        'Dim tbl_approvalCC As DataTable = get_notification_Pending_ByUser(0, "CC", EmailOriginator.Trim, 0) '--Getting the notifications and sending to its respective sender
                        'Dim boolFound As Boolean = False

                        'For Each app_CC_dtRow As DataRow In tbl_EmailIncluded.Rows

                        '    If app_CC_dtRow("type_subject") = "CC" And app_CC_dtRow("email").ToString.Trim = EmailOriginator.Trim Then

                        '        For Each app_pend_dtRow As DataRow In tbl_approvalPending.Rows

                        '            If app_pend_dtRow("id_documento") = app_CC_dtRow("id_documento") Then
                        '                boolFound = True
                        '                Exit For
                        '            End If

                        '        Next

                        '        If Not boolFound Then
                        '            tbl_approvalPending.ImportRow(app_CC_dtRow)
                        '        End If

                        '        boolFound = False

                        '    End If

                        'Next

                        '************************************SYSTEM INFO********************************************
                        Dim cProgram As New RMS.cls_Program
                        cProgram.get_Sys(0, True)
                        cProgram.get_Programs(id_programa, True)
                        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
                        dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                        '************************************SYSTEM INFO********************************************

                        '*****************Creating the email Object**************************************
                        ObJemail = New UTILS.cls_email(id_programa)
                        '*****************Creating the email Object**************************************

                        '*****************building the email from the templates objects**************************************
                        set_t_notification(id_notification) 'Create the Noti_Object
                        set_t_Template() 'Set Template object from the notification id
                        strMess.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)) 'getting the first template

                        strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

                        '*****************FINDING the BCC destinataries **************************************
                        Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    ObJemail.AddBcc(dtEmail)
                                End If

                            End If

                        Next
                        ''*****************FINDING the BCC destinataries **************************************



                        '<!--##PROJECT_NAME##-->)


                        '<!--##ORGANIZATION_NAME##-->
                        '<!--##DESTINATION_EMAIL##-->
                        '<!--#SOLICITATION##-->
                        '<!--#SOLICITATION_TITLE##-->
                        '<!--#SOLICITATION_PURPOSE##-->
                        '<!--##SOLICITATION_NUMBER##-->
                        '<!--#START_DATE##-->
                        '<!--END_DATE##-->
                        '<!--##SYS_PATH##-->
                        '<!--##ID_DOC##-->

                        strMess.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strMess.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        strMess.Replace("<!--##DESTINATION_EMAIL##-->", String.Format("{0};{1};", EmailOriginator, SecundaryEmail))
                        strMess.Replace("<!--#SOLICITATION##-->", oActivitySolicitation.SOLICITATION)
                        strMess.Replace("<!--#SOLICITATION_PURPOSE##-->", oActivitySolicitation.SOLICITATION_PURPOSE)
                        strMess.Replace("<!--##SOLICITATION_NUMBER##-->", oActivitySolicitation.SOLICITATION_CODE)

                        strMess.Replace("<!--#START_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.start_date, "f", timezoneUTC, True))
                        strMess.Replace("<!--END_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.end_date, "f", timezoneUTC, True))

                        strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys))

                        Dim URL_Keys As String = String.Format("id={3}&ab={0}&ac={1}&ad={2}", oActivitySolicitation.SOLICITATION_TOKEN.ToString, oSolicitationAPP.FirstOrDefault.SOLICITATION_TOKEN, oSolicitationAPP.FirstOrDefault.USER_TOKEN_UPDATE.ToString, oActivitySolicitation.ID_ACTIVITY)
                        strMess.Replace("<!--##ID_DOC##-->", URL_Keys)

                        strMess.Replace("<!--RECEIVED_DATE##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SENT_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--OPENED_BY##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.RECEIVED_DATE, "f", timezoneUTC, True))

                        strMess.Replace("<!--#SUBMITTED_DATE#-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SUBMITTED_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--#APPLY_CODE#-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)
                        strMess.Replace("<!--#APPLY_DEFINITION#-->", oSolicitationApply.APPLY_DESCRIPTION)


                        Dim strSTATUS As String = ""
                        Dim strSUBJECTadd As String = ""
                        Dim strEMAIL_ACTION As String = "APPLICATION RESPONSE"

                        If idStatus = 4 Then
                            strSTATUS = "ACCEPTED"
                            strSUBJECTadd = " has been ACCEPTED, is passed to the evaluation stage."
                            strEMAIL_ACTION = "APPLICATION ACCEPTED"
                        ElseIf idStatus = 5 Then
                            strSTATUS = "REJECTED"
                            strSUBJECTadd = " did not PASS to the evaluation stage."
                            strEMAIL_ACTION = "APPLICATION REJECTED"
                        ElseIf idStatus = 3 Then
                            strSTATUS = "APPLIED"
                            strSUBJECTadd = " is submitted, the observation has been responded."
                            strEMAIL_ACTION = "APPLICATION APPLY AGAIN"
                        ElseIf idStatus = 6 Then
                            strSTATUS = "OBSERVED"
                            strSUBJECTadd = " is OBSERVED, please respond as soon as possible."
                            strEMAIL_ACTION = "APPLICATION OBSERVED"
                        Else
                            strSTATUS = "COMMENT"
                            strSUBJECTadd = " has been COMMENTED."
                            strEMAIL_ACTION = "APPLICATION COMMENTED"
                        End If

                        Dim strResponse As String = "--"
                        Dim strResponseBY As String = "--"
                        Dim idUSU As Integer = 0
                        Dim dateRESP As DateTime

                        'If oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).Count() > 0 Then

                        '    strResponse = oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).FirstOrDefault().APPLY_COMM
                        '    idUSU = oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).FirstOrDefault().ID_USUARIO_CREA
                        '    dateRESP = oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).FirstOrDefault().FECHA_CREA
                        '    Dim oUSER = dbEntities.vw_t_usuarios.Where(Function(P) P.id_programa = id_programa And P.id_usuario = idUSU).FirstOrDefault()
                        '    strResponseBY = String.Format("{0} ({1})", oUSER.nombre_usuario, oUSER.job)

                        'Else

                        strResponse = oApplyComm.TA_APPLY_COMM.OrderByDescending(Function(o) o.ID_APPLY_COMM).FirstOrDefault().APPLY_COMM
                            idUSU = oApplyComm.TA_APPLY_COMM.OrderByDescending(Function(o) o.ID_APPLY_COMM).FirstOrDefault().ID_USUARIO_CREA
                            dateRESP = oApplyComm.TA_APPLY_COMM.OrderByDescending(Function(o) o.ID_APPLY_COMM).FirstOrDefault().FECHA_CREA
                            Dim oUSER = dbEntities.vw_t_usuarios.Where(Function(P) P.id_programa = id_programa And P.id_usuario = idUSU).FirstOrDefault()
                            strResponseBY = String.Format("{0} ({1})", oUSER.nombre_usuario, oUSER.job)

                        ' End If

                        '*****************ADDING THE SUBJECTS**********************
                        Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

                        strSubject = strSubject.Replace("<!--##APPLICATION_NUMBER##-->", oApplyComm.TA_SOLICITATION_APP.SOLICITATION_APP_CODE)
                        strSubject = strSubject.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        strSubject &= strSUBJECTadd


                        strMess.Replace("<!--#REPLY STATUS#-->", strSTATUS)
                        strMess.Replace("<!--#REPLY RESPONSE#-->", strResponse)
                        strMess.Replace("<!--#REPLY BY#-->", strResponseBY)
                        strMess.Replace("<!--#REPLY DATE#-->", dateUtil.set_DateFormat(dateRESP, "f", timezoneUTC, True))


                        ' ; 
                        ''''Dim strContentALL As StringBuilder = New StringBuilder()
                        ''''Dim boolSetEmailList As Boolean = True
                        ''''Dim tbl_document_rep As DataTable

                        ''''Dim NDays As Decimal
                        ''''Dim NHours As Decimal
                        ''''Dim SHours As Decimal

                        '''''--Fore each pending approval started to build the EMail
                        '''''<!--##CATEGORY##-->;<!--##APPROVAL_NAME##-->;<!--##INSTRUMENT_NUMBER##-->;<!--##DESCRIPTION_DOC##-->;<!--##TABLE_STATUS##-->

                        ''''For Each tbl_approvalPending_dtR As DataRow In tbl_approvalPending.Rows

                        ''''    '************************************CONTENT**************************************************************************
                        ''''    Dim strContent As StringBuilder = New StringBuilder()
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    tbl_document_rep = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", tbl_approvalPending_dtR("id_documento"))
                        ''''    'tbl_document_rep.Rows(0).Item(
                        ''''    '************************************CONTENT**************************************************************************
                        ''''    strContent.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1))
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    'set_Apps_Document(dtR2("id_documento")) 'Set all AppDoc
                        ''''    '*****************FINDING the Destinataries **************************************

                        ''''    strContent.Replace("<!--##CATEGORY##-->", tbl_document_rep.Rows(0).Item("descripcion_cat"))
                        ''''    strContent.Replace("<!--##APPROVAL_NAME##-->", tbl_document_rep.Rows(0).Item("descripcion_aprobacion"))

                        ''''    strContent.Replace("<!--##ID_DOC##-->", tbl_document_rep.Rows(0).Item("id_documento"))
                        ''''    strContent.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

                        ''''    strContent.Replace("<!--##INSTRUMENT_NUMBER##-->", tbl_document_rep.Rows(0).Item("numero_instrumento"))

                        ''''    Dim strDocumentDesc As String = tbl_document_rep.Rows(0).Item("descripcion_doc") & fn_get_EmailList(tbl_approvalPending_dtR("id_documento"))
                        ''''    strContent.Replace("<!--##DESCRIPTION_DOC##-->", strDocumentDesc)

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    '*****************<!--##TABLE_STATUS##-->'*****************
                        ''''    set_Ta_RutaSeguimiento(tbl_document_rep.Rows(0).Item("id_documento")) 'getting the complete route

                        ''''    Dim strStyle As String = "<tr style='background-color:#ee7108;border:1 dotted #FF0000; ' >" 'Putting inside <tr>
                        ''''    'style="background-color:#ee7108;border:1 dotted #FF0000;"
                        ''''    Dim strTableContent As StringBuilder = New StringBuilder()
                        ''''    Dim strTable_Cont As StringBuilder = New StringBuilder()
                        ''''    'Dim strTableContentALL As StringBuilder = New StringBuilder()
                        ''''    Dim strTable As StringBuilder = New StringBuilder()

                        ''''    '<!--##CONTENT##-->;<!--##ROLE_NAME##-->;<!--##EMPLOYEE_NAME##-->;<!--##STATE##-->;<!--##DATE_RECEIPT##-->;<!--##ALERT_TYPE##-->;<!--##STRONG_OPEN##-->;<!--##STRONG_OPEN##-->;
                        ''''    strTable.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2))

                        ''''    SHours = 0
                        ''''    For Each seg_dtRow In Tbl_Ta_RutaSeguimiento.Rows

                        ''''        strTable_Cont.Clear()
                        ''''        strTable_Cont.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 2))


                        ''''        'strTableContent.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                        ''''        strTable_Cont.Replace("<!--##ROLE_NAME##-->", "")
                        ''''        strTable_Cont.Replace("<!--##EMPLOYEE_NAME##-->", seg_dtRow("nombre_empleado"))
                        ''''        strTable_Cont.Replace("<!--##STATE##-->", seg_dtRow("descripcion_estado"))
                        ''''        strTable_Cont.Replace("<!--##DATE_RECEIPT##-->", seg_dtRow("fecha_aprobacion"))
                        ''''        strTable_Cont.Replace("<!--##ALERT_TYPE##-->", seg_dtRow("Alerta"))

                        ''''        'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                        ''''        If ((tbl_document_rep.Rows(0).Item("id_ruta") = seg_dtRow("id_ruta")) And (tbl_document_rep.Rows(0).Item("id_estadoDoc") = seg_dtRow("id_estadoDoc"))) Then
                        ''''            strTable_Cont.Replace("<tr>", strStyle)
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "<strong>")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "</strong>")
                        ''''        Else
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "")
                        ''''        End If

                        ''''        SHours += seg_dtRow("NHRs")
                        ''''        NDays = Math.Round((seg_dtRow("NHRs") / 24), 0, MidpointRounding.AwayFromZero)

                        ''''        strTable_Cont.Replace("<!--##DAYS##-->", NDays)

                        ''''        strTableContent.Append(strTable_Cont.ToString)


                        ''''    Next

                        ''''    NDays = Int((SHours / 24))
                        ''''    NHours = Math.Round((((SHours / 24) - NDays) * 24), 0, MidpointRounding.AwayFromZero)

                        ''''    'strTableContentALL.Append(strTableContent)

                        ''''    strTable.Replace("<!--##TOTAL_DAYS##-->", String.Format(" {0} Days {1} Hrs", NDays, NHours))
                        ''''    strTable.Replace("<!--##CONTENT##-->", strTableContent.ToString)

                        ''''    strContent.Replace("<!--##TABLE_STATUS##-->", strTable.ToString)
                        ''''    strContentALL.Append(strContent.ToString)
                        ''''    strContentALL.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_nwline", 1))

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    'Add_emails(dtR2("id_documento"), dtR2("id_estadoDoc"), idOriginator, boolSetEmailList) 'Adding the respective email according the state of the approval proccess
                        ''''    'Add_document_email_list(dtR2("id_documento"), boolSetEmailList)
                        ''''    '*****************FINDING the Destinataries **************************************
                        ''''    'boolSetEmailList = False

                        ''''    '**************TESTING PRUPORSES
                        ''''    'Exit For
                        ''''    '**************TESTING PRUPORSES

                        ''''Next

                        ''''strMess.Replace("<!--##CONTENT##-->", strContentALL.ToString)


                        'Add_emails(idOriginator, "User") 'Adding the respective email according the user or the Role

                        If ObJemail.SilentMode Then
                            'If Not ObJemail.SilentMode Then

                            strSubject &= " (Silent MODE) "
                            Dim strTO As StringBuilder = New StringBuilder
                            Dim strCC As String = ""
                            Dim strBCC As String = ""

                            '*********************************************************************************************************
                            '******************we can do this getting all the selection List......************************************
                            '*********************************************************************************************************

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        strTO.Append(String.Format("{0};", dtRow("email").ToString.Trim))
                                    Case "CC"
                                        strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                End Select

                                ' strTO.Append(String.Format("{0};", EmailOriginator.ToString.Trim))

                                'If SecundaryEmail.Trim.Length > 1 Then
                                '    strTO.Append(String.Format("{0};", SecundaryEmail.ToString.Trim))
                                'End If

                            Next

                            ' Dim strEMAILS As String = String.Format("<br /><br /><br /><br /> TO:{0}<br />CC:{1}<br />BCC:{2} ", strTO, strCC, strBCC)
                            Dim strEMAILS As StringBuilder = New StringBuilder
                            strEMAILS.Append("<br /><br /><br /><br />TO:" & strTO.ToString & "<br /><br />")

                            strMess.Replace("<!--##MESS##-->", strEMAILS.ToString) 'Representavite emails for this  notification

                        Else

                            strMess.Replace("<!--##MESS##-->", "")
                            '*********************************************************************************************************
                            '*******************THIS IS the Individual Email for every Notification***********************************
                            'ObJemail.AddTo(EmailOriginator)

                            'If SecundaryEmail.Trim.Length > 1 Then
                            '    ObJemail.AddTo(SecundaryEmail.ToString)
                            'End If

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        ObJemail.AddTo(dtRow("email").ToString.Trim)
                                    Case "CC"
                                        ObJemail.AddCC(dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        ObJemail.AddBcc(dtRow("email").ToString.Trim)
                                End Select

                            Next

                            '*******************THIS IS the Individual Email for every notification***********************************
                            '*********************************************************************************************************

                        End If

                        '*******************************************************************
                        '************************SUBJECTS***********************************
                        ObJemail.AddSubject(strSubject)
                        '************************SUBJECTS***********************************
                        '*******************************************************************

                        '************************* THE message it supposed Is here, need to proceed to test*****************
                        '*****************building the email from the templates objects**************************************

                        strMess.Replace("<!--##IMG_CID2##-->", "LogProgram") 'Name of the resource the Program Logo
                        strMess.Replace("<!--##IMG_CID1##-->", "LogChemo") 'Name of the resource the Company logo

                        Dim strPath As String
                        If strBaseDir.Length > 1 Then
                            strPath = strBaseDir
                        Else
                            strPath = Server.MapPath("~")
                        End If

                        Dim strTMP As String = strMess.ToString

                        ObJemail.setAlternativeVIEW(strMess.ToString) 'Setting alternative View

                        Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                        Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                        If Not String.IsNullOrEmpty(strProgramIMG) Then
                            ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
                        Else
                            ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
                        End If

                        'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                        ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

                        'ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Red_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Blue_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Orange_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Cyan_Time.png", "image/png")

                        ObJemail.SetPriority(MailPriority.High)

                        If ObJemail.SendEmail(id_notification, oSolicitationAPP.FirstOrDefault.ID_SOLICITATION_APP, "APPLICATION RESPONSE", strEMAIL_ACTION) Then
                            NOTIFIYING_SOLICITATION_RESPONSE = True
                        Else
                            NOTIFIYING_SOLICITATION_RESPONSE = False
                        End If

                    Else  '*********************Valid Email addres**********************************

                        NOTIFIYING_SOLICITATION_RESPONSE = False

                    End If


                End Using

            Catch ex As Exception

                NOTIFIYING_SOLICITATION_RESPONSE = False

            End Try


        End Function



        Public Function NOTIFIYING_EVALUATION(ByVal id_notification_app As Integer, ByVal idStatus As Integer, Optional strBaseDir As String = "") As Boolean

            Dim strMess As StringBuilder = New StringBuilder


            Try

                Using dbEntities As New dbRMS_JIEntities

                    Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).ToList()
                    Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(oSolicitationAPP.FirstOrDefault.ID_ACTIVITY_SOLICITATION)
                    Dim oSolicitationApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).FirstOrDefault()
                    Dim oApplyComm = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).FirstOrDefault()

                    Dim idAPPLY_EVALUATION As Integer = oActivitySolicitation.TA_APPLY_EVALUATION.FirstOrDefault.ID_APPLY_EVALUATION
                    Dim oEvaluationTEAM = dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_APPLY_EVALUATION = idAPPLY_EVALUATION).ToList()

                    Dim EmailOriginator As String = ""
                    Dim SecundaryEmail As String = ""

                    'Dim EmailOriginator As String = oSolicitationAPP.FirstOrDefault.ORGANIZATIONEMAIL
                    'Dim SecundaryEmail As String = oSolicitationAPP.FirstOrDefault.PRIMARYCONTACTEMAIL

                    Set_Emails_List()

                    'Add_Emails_List("TO", EmailOriginator)
                    'Add_Emails_List("CC", SecundaryEmail)
                    For Each dtITEM In oEvaluationTEAM

                        Dim oUsuario = dbEntities.t_usuarios.Where(Function(p) p.id_usuario = dtITEM.ID_USER).FirstOrDefault
                        EmailOriginator = oUsuario.email_usuario.ToLower
                        Add_Emails_List("TO", oUsuario.email_usuario)

                    Next

                    Dim tbl_ As DataTable = get_t_notification_emails("CNTMNG")

                    If Not IsNothing(tbl_) Then

                        Add_Emails_List("CC", tbl_.Rows.Item(0).Item("noti_email"))

                        Dim strBccEmails As String() = If(IsDBNull(tbl_.Rows.Item(0).Item("noti_cc")), "", tbl_.Rows.Item(0).Item("noti_cc")).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then

                                    Add_Emails_List("CC", dtEmail)


                                End If

                            End If

                        Next

                    End If


                    If IsEmail(EmailOriginator) Then

                        '************************************SYSTEM INFO********************************************
                        Dim cProgram As New RMS.cls_Program
                        cProgram.get_Sys(0, True)
                        cProgram.get_Programs(id_programa, True)
                        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
                        dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                        '************************************SYSTEM INFO********************************************

                        '*****************Creating the email Object**************************************
                        ObJemail = New UTILS.cls_email(id_programa)
                        '*****************Creating the email Object**************************************

                        '*****************building the email from the templates objects**************************************
                        set_t_notification(id_notification) 'Create the Noti_Object
                        set_t_Template() 'Set Template object from the notification id
                        strMess.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)) 'getting the first template

                        strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

                        '*****************FINDING the BCC destinataries **************************************
                        Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    ObJemail.AddBcc(dtEmail)
                                End If

                            End If

                        Next
                        ''*****************FINDING the BCC destinataries **************************************

                        '<!--##PROJECT_NAME##-->)

                        '<!--##ORGANIZATION_NAME##-->
                        '<!--##DESTINATION_EMAIL##-->
                        '<!--#SOLICITATION##-->
                        '<!--#SOLICITATION_TITLE##-->
                        '<!--#SOLICITATION_PURPOSE##-->
                        '<!--##SOLICITATION_NUMBER##-->
                        '<!--#START_DATE##-->
                        '<!--END_DATE##-->
                        '<!--##SYS_PATH##-->
                        '<!--##ID_DOC##-->

                        strMess.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strMess.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        strMess.Replace("<!--##DESTINATION_EMAIL##-->", String.Format("{0};{1};", EmailOriginator, SecundaryEmail))
                        strMess.Replace("<!--#SOLICITATION##-->", oActivitySolicitation.SOLICITATION)
                        strMess.Replace("<!--#SOLICITATION_PURPOSE##-->", oActivitySolicitation.SOLICITATION_PURPOSE)
                        strMess.Replace("<!--##SOLICITATION_NUMBER##-->", oActivitySolicitation.SOLICITATION_CODE)

                        strMess.Replace("<!--#START_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.start_date, "f", timezoneUTC, True))
                        strMess.Replace("<!--END_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.end_date, "f", timezoneUTC, True))

                        strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys))

                        Dim URL_Keys As String = String.Format("id={3}&ab={0}&ac={1}&ad={2}", oActivitySolicitation.SOLICITATION_TOKEN.ToString, oSolicitationAPP.FirstOrDefault.SOLICITATION_TOKEN, oSolicitationAPP.FirstOrDefault.USER_TOKEN_UPDATE.ToString, oActivitySolicitation.ID_ACTIVITY)
                        strMess.Replace("<!--##ID_DOC##-->", URL_Keys)

                        strMess.Replace("<!--RECEIVED_DATE##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SENT_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--OPENED_BY##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.RECEIVED_DATE, "f", timezoneUTC, True))

                        strMess.Replace("<!--#SUBMITTED_DATE#-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SUBMITTED_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--#APPLY_CODE#-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)
                        strMess.Replace("<!--#APPLY_DEFINITION#-->", oSolicitationApply.APPLY_DESCRIPTION)


                        Dim strSTATUS As String = ""
                        'Dim strSUBJECTadd As String = ""
                        Dim strEMAIL_ACTION As String = "EVALUATION ALERT"

                        'If idStatus = 4 Then
                        strSTATUS = "ACCEPTED"
                        '    strSUBJECTadd = " has been ACCEPTED, is passed to the evaluation stage."
                        '    strEMAIL_ACTION = "APPLICATION ACCEPTED"
                        'ElseIf idStatus = 5 Then
                        '    strSTATUS = "REJECTED"
                        '    strSUBJECTadd = " did not PASS to the evaluation stage."
                        '    strEMAIL_ACTION = "APPLICATION REJECTED"
                        'ElseIf idStatus = 3 Then
                        '    strSTATUS = "APPLIED"
                        '    strSUBJECTadd = " is submitted, the observation has been responded."
                        '    strEMAIL_ACTION = "APPLICATION APPLY AGAIN"
                        'ElseIf idStatus = 6 Then
                        '    strSTATUS = "OBSERVED"
                        '    strSUBJECTadd = " is OBSERVED, please respond as soon as possible."
                        '    strEMAIL_ACTION = "APPLICATION OBSERVED"
                        'Else
                        '    strSTATUS = "COMMENT"
                        '    strSUBJECTadd = " has been COMMENTED."
                        '    strEMAIL_ACTION = "APPLICATION COMMENTED"
                        'End If

                        Dim strResponse As String = "--"
                        Dim strResponseBY As String = "--"
                        Dim idUSU As Integer = 0
                        Dim dateRESP As DateTime

                        'If oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).Count() > 0 Then

                        '    strResponse = oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).FirstOrDefault().APPLY_COMM
                        '    idUSU = oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).FirstOrDefault().ID_USUARIO_CREA
                        '    dateRESP = oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).FirstOrDefault().FECHA_CREA
                        '    Dim oUSER = dbEntities.vw_t_usuarios.Where(Function(P) P.id_programa = id_programa And P.id_usuario = idUSU).FirstOrDefault()
                        '    strResponseBY = String.Format("{0} ({1})", oUSER.nombre_usuario, oUSER.job)

                        'Else

                        strResponse = oApplyComm.TA_APPLY_COMM.OrderByDescending(Function(o) o.ID_APPLY_COMM).FirstOrDefault().APPLY_COMM
                        idUSU = oApplyComm.TA_APPLY_COMM.OrderByDescending(Function(o) o.ID_APPLY_COMM).FirstOrDefault().ID_USUARIO_CREA
                        dateRESP = oApplyComm.TA_APPLY_COMM.OrderByDescending(Function(o) o.ID_APPLY_COMM).FirstOrDefault().FECHA_CREA
                        Dim oUSER = dbEntities.vw_t_usuarios.Where(Function(P) P.id_programa = id_programa And P.id_usuario = idUSU).FirstOrDefault()
                        strResponseBY = String.Format("{0} ({1})", oUSER.nombre_usuario, oUSER.job)

                        ' End If

                        '*****************ADDING THE SUBJECTS**********************
                        Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

                        strSubject = strSubject.Replace("<!--##APPLICATION_NUMBER##-->", oApplyComm.TA_SOLICITATION_APP.SOLICITATION_APP_CODE)
                        strSubject = strSubject.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        'strSubject &= strSUBJECTadd


                        strMess.Replace("<!--#REPLY STATUS#-->", strSTATUS)
                        strMess.Replace("<!--#REPLY RESPONSE#-->", strResponse)
                        strMess.Replace("<!--#REPLY BY#-->", strResponseBY)
                        strMess.Replace("<!--#REPLY DATE#-->", dateUtil.set_DateFormat(dateRESP, "f", timezoneUTC, True))


                        If ObJemail.SilentMode Then
                            'If Not ObJemail.SilentMode Then

                            strSubject &= " (Silent MODE) "
                            Dim strTO As StringBuilder = New StringBuilder
                            Dim strCC As String = ""
                            Dim strBCC As String = ""

                            '*********************************************************************************************************
                            '******************we can do this getting all the selection List......************************************
                            '*********************************************************************************************************

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        strTO.Append(String.Format("{0};", dtRow("email").ToString.Trim))
                                    Case "CC"
                                        strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                End Select

                                ' strTO.Append(String.Format("{0};", EmailOriginator.ToString.Trim))

                                'If SecundaryEmail.Trim.Length > 1 Then
                                '    strTO.Append(String.Format("{0};", SecundaryEmail.ToString.Trim))
                                'End If

                            Next

                            ' Dim strEMAILS As String = String.Format("<br /><br /><br /><br /> TO:{0}<br />CC:{1}<br />BCC:{2} ", strTO, strCC, strBCC)
                            Dim strEMAILS As StringBuilder = New StringBuilder
                            strEMAILS.Append("<br /><br /><br /><br />TO:" & strTO.ToString & "<br /><br />")
                            strEMAILS.Append("CC:" & strCC.ToString & "<br /><br />")

                            strMess.Replace("<!--##MESS##-->", strEMAILS.ToString) 'Representavite emails for this  notification

                        Else

                            strMess.Replace("<!--##MESS##-->", "")
                            '*********************************************************************************************************
                            '*******************THIS IS the Individual Email for every Notification***********************************
                            'ObJemail.AddTo(EmailOriginator)

                            'If SecundaryEmail.Trim.Length > 1 Then
                            '    ObJemail.AddTo(SecundaryEmail.ToString)
                            'End If

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        ObJemail.AddTo(dtRow("email").ToString.Trim)
                                    Case "CC"
                                        ObJemail.AddCC(dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        ObJemail.AddBcc(dtRow("email").ToString.Trim)
                                End Select

                            Next

                            '*******************THIS IS the Individual Email for every notification***********************************
                            '*********************************************************************************************************

                        End If

                        '*******************************************************************
                        '************************SUBJECTS***********************************
                        ObJemail.AddSubject(strSubject)
                        '************************SUBJECTS***********************************
                        '*******************************************************************

                        '************************* THE message it supposed Is here, need to proceed to test*****************
                        '*****************building the email from the templates objects**************************************

                        strMess.Replace("<!--##IMG_CID2##-->", "LogProgram") 'Name of the resource the Program Logo
                        strMess.Replace("<!--##IMG_CID1##-->", "LogChemo") 'Name of the resource the Company logo

                        Dim strPath As String
                        If strBaseDir.Length > 1 Then
                            strPath = strBaseDir
                        Else
                            strPath = Server.MapPath("~")
                        End If

                        Dim strTMP As String = strMess.ToString

                        ObJemail.setAlternativeVIEW(strMess.ToString) 'Setting alternative View

                        Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                        Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                        If Not String.IsNullOrEmpty(strProgramIMG) Then
                            ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
                        Else
                            ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
                        End If

                        'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                        ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

                        'ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Red_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Blue_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Orange_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Cyan_Time.png", "image/png")

                        ObJemail.SetPriority(MailPriority.High)

                        If ObJemail.SendEmail(id_notification, oSolicitationAPP.FirstOrDefault.ID_SOLICITATION_APP, "APPLICATION RESPONSE", strEMAIL_ACTION) Then
                            NOTIFIYING_EVALUATION = True
                        Else
                            NOTIFIYING_EVALUATION = False
                        End If

                    Else  '*********************Valid Email addres**********************************

                        NOTIFIYING_EVALUATION = False

                    End If


                End Using

            Catch ex As Exception

                NOTIFIYING_EVALUATION = False

            End Try


        End Function





        Public Function NOTIFIYING_EVALUATION_ACCEPTED(ByVal id_notification_app As Integer, ByVal idStatus As Integer, ByVal idEVAL_Round As Integer, ByVal idEVAL_APP As Integer, Optional strBaseDir As String = "") As Boolean

            Dim strMess As StringBuilder = New StringBuilder


            Try

                Using dbEntities As New dbRMS_JIEntities

                    Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).ToList()
                    Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(oSolicitationAPP.FirstOrDefault.ID_ACTIVITY_SOLICITATION)
                    Dim oSolicitationApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).FirstOrDefault()
                    Dim oApplyComm = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).FirstOrDefault()


                    Dim oTA_EVALUATION_ROUNDS = dbEntities.TA_EVALUATION_ROUNDS.Find(idEVAL_Round)
                    'Dim idEvalSTATUS As Integer = Get_Evaluation_STATUS(2, oTA_EVALUATION_ROUNDS.ID_VOTING_TYPE, True) 'And p.ID_EVALUATION_APP_STATUS = idEvalSTATUS
                    Dim oEVAL_Comm = dbEntities.TA_EVALUATION_APP_COMM.Where(Function(p) p.ID_EVALUATION_APP = idEVAL_APP).OrderBy(Function(o) o.FECHA_CREA).ToList()

                    Dim idAPPLY_EVALUATION As Integer = oActivitySolicitation.TA_APPLY_EVALUATION.FirstOrDefault.ID_APPLY_EVALUATION
                    Dim oEvaluationTEAM = dbEntities.TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_APPLY_EVALUATION = idAPPLY_EVALUATION).ToList()

                    Dim EmailOriginator As String = ""
                    Dim SecundaryEmail As String = ""

                    'Dim EmailOriginator As String = oSolicitationAPP.FirstOrDefault.ORGANIZATIONEMAIL
                    'Dim SecundaryEmail As String = oSolicitationAPP.FirstOrDefault.PRIMARYCONTACTEMAIL

                    Set_Emails_List()

                    'Add_Emails_List("TO", EmailOriginator)
                    'Add_Emails_List("CC", SecundaryEmail)
                    For Each dtITEM In oEvaluationTEAM

                        Dim oUsuario = dbEntities.t_usuarios.Where(Function(p) p.id_usuario = dtITEM.ID_USER).FirstOrDefault
                        EmailOriginator = oUsuario.email_usuario
                        Add_Emails_List("TO", oUsuario.email_usuario)

                    Next

                    Dim tbl_ As DataTable = get_t_notification_emails("CNTMNG")

                    If Not IsNothing(tbl_) Then

                        Add_Emails_List("CC", tbl_.Rows.Item(0).Item("noti_email"))

                        Dim strBccEmails As String() = If(IsDBNull(tbl_.Rows.Item(0).Item("noti_cc")), "", tbl_.Rows.Item(0).Item("noti_cc")).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then

                                    Add_Emails_List("CC", dtEmail)


                                End If

                            End If

                        Next

                    End If


                    If IsEmail(EmailOriginator) Then

                        '************************************SYSTEM INFO********************************************
                        Dim cProgram As New RMS.cls_Program
                        cProgram.get_Sys(0, True)
                        cProgram.get_Programs(id_programa, True)
                        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
                        dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                        '************************************SYSTEM INFO********************************************

                        '*****************Creating the email Object**************************************
                        ObJemail = New UTILS.cls_email(id_programa)
                        '*****************Creating the email Object**************************************

                        '*****************building the email from the templates objects**************************************
                        set_t_notification(id_notification) 'Create the Noti_Object
                        set_t_Template() 'Set Template object from the notification id
                        strMess.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)) 'getting the first template

                        strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

                        '*****************FINDING the BCC destinataries **************************************
                        Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    ObJemail.AddBcc(dtEmail)
                                End If

                            End If

                        Next
                        ''*****************FINDING the BCC destinataries **************************************

                        '<!--##PROJECT_NAME##-->)

                        '<!--##ORGANIZATION_NAME##-->
                        '<!--##DESTINATION_EMAIL##-->
                        '<!--#SOLICITATION##-->
                        '<!--#SOLICITATION_TITLE##-->
                        '<!--#SOLICITATION_PURPOSE##-->
                        '<!--##SOLICITATION_NUMBER##-->
                        '<!--#START_DATE##-->
                        '<!--END_DATE##-->
                        '<!--##SYS_PATH##-->
                        '<!--##ID_DOC##-->

                        strMess.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strMess.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        strMess.Replace("<!--##DESTINATION_EMAIL##-->", String.Format("{0};{1};", EmailOriginator, SecundaryEmail))
                        strMess.Replace("<!--#SOLICITATION##-->", oActivitySolicitation.SOLICITATION)
                        strMess.Replace("<!--#SOLICITATION_PURPOSE##-->", oActivitySolicitation.SOLICITATION_PURPOSE)
                        strMess.Replace("<!--##SOLICITATION_NUMBER##-->", oActivitySolicitation.SOLICITATION_CODE)

                        strMess.Replace("<!--#START_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.start_date, "f", timezoneUTC, True))
                        strMess.Replace("<!--END_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.end_date, "f", timezoneUTC, True))

                        strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys))

                        Dim URL_Keys As String = String.Format("id={3}&ab={0}&ac={1}&ad={2}", oActivitySolicitation.SOLICITATION_TOKEN.ToString, oSolicitationAPP.FirstOrDefault.SOLICITATION_TOKEN, oSolicitationAPP.FirstOrDefault.USER_TOKEN_UPDATE.ToString, oActivitySolicitation.ID_ACTIVITY)
                        strMess.Replace("<!--##ID_DOC##-->", URL_Keys)

                        strMess.Replace("<!--RECEIVED_DATE##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SENT_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--OPENED_BY##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.RECEIVED_DATE, "f", timezoneUTC, True))

                        strMess.Replace("<!--#SUBMITTED_DATE#-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SUBMITTED_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--#APPLY_CODE#-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)
                        strMess.Replace("<!--#APPLY_DEFINITION#-->", oSolicitationApply.APPLY_DESCRIPTION)


                        Dim strSTATUS As String = ""
                        'Dim strSUBJECTadd As String = ""
                        Dim strEMAIL_ACTION As String = "EVALUATION ALERT"

                        'If idStatus = 4 Then
                        strSTATUS = "PASSED"
                        '    strSUBJECTadd = " has been ACCEPTED, is passed to the evaluation stage."
                        '    strEMAIL_ACTION = "APPLICATION ACCEPTED"
                        'ElseIf idStatus = 5 Then
                        '    strSTATUS = "REJECTED"
                        '    strSUBJECTadd = " did not PASS to the evaluation stage."
                        '    strEMAIL_ACTION = "APPLICATION REJECTED"
                        'ElseIf idStatus = 3 Then
                        '    strSTATUS = "APPLIED"
                        '    strSUBJECTadd = " is submitted, the observation has been responded."
                        '    strEMAIL_ACTION = "APPLICATION APPLY AGAIN"
                        'ElseIf idStatus = 6 Then
                        '    strSTATUS = "OBSERVED"
                        '    strSUBJECTadd = " is OBSERVED, please respond as soon as possible."
                        '    strEMAIL_ACTION = "APPLICATION OBSERVED"
                        'Else
                        '    strSTATUS = "COMMENT"
                        '    strSUBJECTadd = " has been COMMENTED."
                        '    strEMAIL_ACTION = "APPLICATION COMMENTED"
                        'End If

                        Dim strResponse As String = "--"
                        Dim strResponseBY As String = "--"
                        Dim idUSU As Integer = 0
                        Dim dateRESP As DateTime

                        Dim strCols As String = "<tr style='border-bottom-color:#ee7108;'>
                                                                <td style='font-weight500;font-family: Arial, Helvetica, sans - serif;font-size: small; padding-left:10px; '>                                                                   
                                                                        <div style='font-weight600;font-family:Arial,Helvetica,sans-serif;font-size:small;color:#035397;text-align:Left;float:Left;padding:10px;'>{0}</div>
                                                                        <div style='font-weight600;font-family:Arial,Helvetica,sans-serif;font-size:small;color:#035397;text-align:Right;float:Right;right:20px;padding:10px;'>{1}</div>                                                                       
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style='font-weight500;font-family: Arial, Helvetica,sans-serif;font-size:small;padding-left:30px;'>                                                                    
                                                                   {2}
                                                                    <br />
                                                                    <hr style='border-top 1px dashed #035397; vertical-align:bottom;' />
                                                                </td>
                                                            </tr>"

                        Dim strColsTOT As String = ""

                        'If oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).Count() > 0 Then

                        '    strResponse = oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).FirstOrDefault().APPLY_COMM
                        '    idUSU = oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).FirstOrDefault().ID_USUARIO_CREA
                        '    dateRESP = oApplyComm.TA_APPLY_COMM.Where(Function(p) p.ID_APPLY_STATUS = idStatus).FirstOrDefault().FECHA_CREA
                        '    Dim oUSER = dbEntities.vw_t_usuarios.Where(Function(P) P.id_programa = id_programa And P.id_usuario = idUSU).FirstOrDefault()
                        '    strResponseBY = String.Format("{0} ({1})", oUSER.nombre_usuario, oUSER.job)

                        'Else

                        If oEVAL_Comm.Count > 0 Then

                            For Each dtEVAL In oEVAL_Comm.ToList()

                                idUSU = dtEVAL.ID_USUARIO_CREA
                                dateRESP = dtEVAL.FECHA_CREA
                                Dim oUSER = dbEntities.vw_t_usuarios.Where(Function(P) P.id_programa = id_programa And P.id_usuario = idUSU).FirstOrDefault()
                                strResponseBY = String.Format("{0} ({1})", oUSER.nombre_usuario, oUSER.job)
                                strColsTOT &= String.Format(strCols, strResponseBY, dateUtil.set_DateFormat(dtEVAL.FECHA_CREA, "f", timezoneUTC, True), dtEVAL.EVALUATION_COMM.Trim)
                                'strResponse &= String.Format("<p>{0}</p> -- {1} ", dateUtil.set_DateFormat(dtEVAL.FECHA_CREA, "f", timezoneUTC, True), strResponseBY) & "<br/><br/>" & dtEVAL.EVALUATION_COMM.Trim & "<br/><br/>"


                            Next

                            strResponse = strColsTOT

                        Else

                            strResponse = ""

                        End If

                        ' End If

                        '*****************ADDING THE SUBJECTS**********************
                        Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

                        strSubject = strSubject.Replace("<!--##APPLICATION_NUMBER##-->", oApplyComm.TA_SOLICITATION_APP.SOLICITATION_APP_CODE)
                        strSubject = strSubject.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        'strSubject &= strSUBJECTadd


                        strMess.Replace("<!--#REPLY STATUS#-->", strSTATUS)
                        strMess.Replace("<!--#REPLY RESPONSE#-->", strResponse)
                        strMess.Replace("<!--#REPLY BY#-->", strResponseBY)
                        strMess.Replace("<!--#REPLY DATE#-->", dateUtil.set_DateFormat(dateRESP, "f", timezoneUTC, True))


                        If ObJemail.SilentMode Then
                            'If Not ObJemail.SilentMode Then

                            strSubject &= " (Silent MODE) "
                            Dim strTO As StringBuilder = New StringBuilder
                            Dim strCC As String = ""
                            Dim strBCC As String = ""

                            '*********************************************************************************************************
                            '******************we can do this getting all the selection List......************************************
                            '*********************************************************************************************************

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        strTO.Append(String.Format("{0};", dtRow("email").ToString.Trim))
                                    Case "CC"
                                        strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                End Select

                                ' strTO.Append(String.Format("{0};", EmailOriginator.ToString.Trim))

                                'If SecundaryEmail.Trim.Length > 1 Then
                                '    strTO.Append(String.Format("{0};", SecundaryEmail.ToString.Trim))
                                'End If

                            Next

                            ' Dim strEMAILS As String = String.Format("<br /><br /><br /><br /> TO:{0}<br />CC:{1}<br />BCC:{2} ", strTO, strCC, strBCC)
                            Dim strEMAILS As StringBuilder = New StringBuilder
                            strEMAILS.Append("<br /><br /><br /><br />TO:" & strTO.ToString & "<br /><br />")
                            strEMAILS.Append("CC:" & strCC.ToString & "<br /><br />")

                            strMess.Replace("<!--##MESS##-->", strEMAILS.ToString) 'Representavite emails for this  notification

                        Else

                            strMess.Replace("<!--##MESS##-->", "")
                            '*********************************************************************************************************
                            '*******************THIS IS the Individual Email for every Notification***********************************
                            'ObJemail.AddTo(EmailOriginator)

                            'If SecundaryEmail.Trim.Length > 1 Then
                            '    ObJemail.AddTo(SecundaryEmail.ToString)
                            'End If

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        ObJemail.AddTo(dtRow("email").ToString.Trim)
                                    Case "CC"
                                        ObJemail.AddCC(dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        ObJemail.AddBcc(dtRow("email").ToString.Trim)
                                End Select

                            Next

                            '*******************THIS IS the Individual Email for every notification***********************************
                            '*********************************************************************************************************

                        End If

                        '*******************************************************************
                        '************************SUBJECTS***********************************
                        ObJemail.AddSubject(strSubject)
                        '************************SUBJECTS***********************************
                        '*******************************************************************

                        '************************* THE message it supposed Is here, need to proceed to test*****************
                        '*****************building the email from the templates objects**************************************

                        strMess.Replace("<!--##IMG_CID2##-->", "LogProgram") 'Name of the resource the Program Logo
                        strMess.Replace("<!--##IMG_CID1##-->", "LogChemo") 'Name of the resource the Company logo

                        Dim strPath As String
                        If strBaseDir.Length > 1 Then
                            strPath = strBaseDir
                        Else
                            strPath = Server.MapPath("~")
                        End If

                        Dim strTMP As String = strMess.ToString

                        ObJemail.setAlternativeVIEW(strMess.ToString) 'Setting alternative View

                        Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                        Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                        If Not String.IsNullOrEmpty(strProgramIMG) Then
                            ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
                        Else
                            ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
                        End If

                        'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                        ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

                        'ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Red_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Blue_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Orange_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Cyan_Time.png", "image/png")

                        ObJemail.SetPriority(MailPriority.High)

                        If ObJemail.SendEmail(id_notification, oSolicitationAPP.FirstOrDefault.ID_SOLICITATION_APP, "EVALUATION PASSED", strEMAIL_ACTION) Then
                            NOTIFIYING_EVALUATION_ACCEPTED = True
                        Else
                            NOTIFIYING_EVALUATION_ACCEPTED = False
                        End If

                    Else  '*********************Valid Email addres**********************************

                        NOTIFIYING_EVALUATION_ACCEPTED = False

                    End If


                End Using

            Catch ex As Exception

                NOTIFIYING_EVALUATION_ACCEPTED = False

            End Try


        End Function

        Public Function get_t_notification_emails(ByVal strCode As String) As DataTable

            Dim tbl_emails As DataTable
            Dim strSql As String = String.Format("select a.id_notification_emails,
	                                                    a.noti_email_code,
		                                                a.noti_email,
		                                                b.email_usuario as email,
		                                                a.noti_cc,
		                                                b.id_usuario from t_notification_emails  a
                                                  inner join t_usuarios b on (a.id_usuario = b.id_usuario)
                                                where a.id_programa = {1}
                                                 and a.noti_email_code = '{0}' ", strCode, id_programa)

            tbl_emails = cl_utl.setObjeto("t_notification_emails", "id_notification_emails", 0, strSql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_emails.Rows.Count = 1 And tbl_emails.Rows.Item(0).Item("id_notification_emails") = 0) Then
                tbl_emails.Rows.Remove(tbl_emails.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_t_notification_emails = tbl_emails

        End Function


        Public Function NOTIFIYING_SOLICITATION_APPLY(ByVal id_notification_app As Integer, Optional strBaseDir As String = "") As Boolean

            Dim strMess As StringBuilder = New StringBuilder


            Try

                Using dbEntities As New dbRMS_JIEntities


                    Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).ToList()

                    Dim id_activity_solicitation As Integer = oSolicitationAPP.FirstOrDefault.ID_ACTIVITY_SOLICITATION

                    Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(id_activity_solicitation)
                    Dim oTeamEvaluation = dbEntities.VW_TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_activity_solicitation).ToList()

                    Dim oSolicitationApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_notification_app).FirstOrDefault()


                    'Dim EmailOriginator As String = oSolicitationAPP.FirstOrDefault.ORGANIZATIONEMAIL
                    'Dim SecundaryEmail As String = oSolicitationAPP.FirstOrDefault.PRIMARYCONTACTEMAIL

                    Dim EmailOriginator As String = ""
                    Dim SecundaryEmail As String = ""

                    Dim tbl_ As DataTable = get_t_notification_emails("CNTMNG")
                    Set_Emails_List()

                    If Not IsNothing(tbl_) Then

                        Add_Emails_List("TO", tbl_.Rows.Item(0).Item("email"))
                        EmailOriginator = tbl_.Rows.Item(0).Item("email")

                        Dim strBccEmails As String() = If(IsDBNull(tbl_.Rows.Item(0).Item("noti_cc")), "", tbl_.Rows.Item(0).Item("noti_cc")).Split(";")


                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    'ObJemail.AddBcc(dtEmail)
                                    Add_Emails_List("CC", dtEmail)

                                End If

                            End If

                        Next
                    End If


                    Dim oUSER = dbEntities.vw_t_usuarios.Where(Function(P) P.id_programa = id_programa And P.id_usuario = oActivitySolicitation.id_usuario_res).FirstOrDefault()
                    Add_Emails_List("TO", oUSER.email_usuario)

                    Dim strEmails_TO As String() = If(IsDBNull(oActivitySolicitation.email_to), "", oActivitySolicitation.email_to.Split(";"))

                    For Each dtEmail As String In strEmails_TO

                        If Not String.IsNullOrEmpty(dtEmail) Then

                            If IsEmail(dtEmail) Then
                                'ObJemail.AddBcc(dtEmail)
                                Add_Emails_List("TO", dtEmail)

                            End If

                        End If

                    Next

                    Dim strEmails_CC As String() = If(IsDBNull(oActivitySolicitation.email_cc), "", oActivitySolicitation.email_cc.Split(";"))

                    For Each dtEmail As String In strEmails_CC

                        If Not String.IsNullOrEmpty(dtEmail) Then

                            If IsEmail(dtEmail) Then
                                'ObJemail.AddBcc(dtEmail)
                                Add_Emails_List("CC", dtEmail)

                            End If

                        End If

                    Next


                    If IsEmail(EmailOriginator) Then

                        'Dim tbl_approvalCC As DataTable = get_notification_Pending_ByUser(0, "CC", EmailOriginator.Trim, 0) '--Getting the notifications and sending to its respective sender
                        'Dim boolFound As Boolean = False

                        'For Each app_CC_dtRow As DataRow In tbl_EmailIncluded.Rows

                        '    If app_CC_dtRow("type_subject") = "CC" And app_CC_dtRow("email").ToString.Trim = EmailOriginator.Trim Then

                        '        For Each app_pend_dtRow As DataRow In tbl_approvalPending.Rows

                        '            If app_pend_dtRow("id_documento") = app_CC_dtRow("id_documento") Then
                        '                boolFound = True
                        '                Exit For
                        '            End If

                        '        Next

                        '        If Not boolFound Then
                        '            tbl_approvalPending.ImportRow(app_CC_dtRow)
                        '        End If

                        '        boolFound = False


                        '    End If

                        'Next



                        '************************************SYSTEM INFO********************************************
                        Dim cProgram As New RMS.cls_Program
                        cProgram.get_Sys(0, True)
                        cProgram.get_Programs(id_programa, True)
                        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
                        dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                        '************************************SYSTEM INFO********************************************

                        '*****************Creating the email Object**************************************
                        ObJemail = New UTILS.cls_email(id_programa)
                        '*****************Creating the email Object**************************************

                        '*****************building the email from the templates objects**************************************
                        set_t_notification(id_notification) 'Create the Noti_Object
                        set_t_Template() 'Set Template object from the notification id
                        strMess.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)) 'getting the first template

                        strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

                        '*****************FINDING the BCC destinataries **************************************
                        Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    ObJemail.AddBcc(dtEmail)
                                End If

                            End If

                        Next
                        ''*****************FINDING the BCC destinataries **************************************



                        '*****************ADDING THE SUBJECTS**********************
                        Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

                        strSubject = strSubject.Replace("<!--##SOLICITATION_NUMBER##-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)
                        strSubject = strSubject.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strSubject = strSubject.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)

                        '<!--##PROJECT_NAME##-->)


                        '<!--##ORGANIZATION_NAME##-->
                        '<!--##DESTINATION_EMAIL##-->
                        '<!--#SOLICITATION##-->
                        '<!--#SOLICITATION_TITLE##-->
                        '<!--#SOLICITATION_PURPOSE##-->
                        '<!--##SOLICITATION_NUMBER##-->
                        '<!--#START_DATE##-->
                        '<!--END_DATE##-->
                        '<!--##SYS_PATH##-->
                        '<!--##ID_DOC##-->


                        strMess.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strMess.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        strMess.Replace("<!--##DESTINATION_EMAIL##-->", String.Format("{0};{1};", EmailOriginator, SecundaryEmail))
                        strMess.Replace("<!--#SOLICITATION##-->", oActivitySolicitation.SOLICITATION)
                        strMess.Replace("<!--#SOLICITATION_PURPOSE##-->", oActivitySolicitation.SOLICITATION_PURPOSE)
                        strMess.Replace("<!--##SOLICITATION_NUMBER##-->", oActivitySolicitation.SOLICITATION_CODE)

                        strMess.Replace("<!--#START_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.start_date, "f", timezoneUTC, True))
                        strMess.Replace("<!--END_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.end_date, "f", timezoneUTC, True))

                        strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys))


                        Dim URL_Keys As String = String.Format("ab={0}&ac={1}&ad={2}", oActivitySolicitation.SOLICITATION_TOKEN.ToString, oSolicitationAPP.FirstOrDefault.SOLICITATION_TOKEN, oSolicitationAPP.FirstOrDefault.USER_TOKEN_UPDATE.ToString)
                        strMess.Replace("<!--##ID_DOC##-->", URL_Keys)


                        strMess.Replace("<!--RECEIVED_DATE##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SENT_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--OPENED_BY##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.RECEIVED_DATE, "f", timezoneUTC, True))

                        strMess.Replace("<!--#SUBMITTED_DATE#-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SUBMITTED_DATE, "f", timezoneUTC, True))

                        strMess.Replace("<!--#APPLY_CODE#-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)

                        strMess.Replace("<!--#APPLY_DEFINITION#-->", oSolicitationApply.APPLY_DESCRIPTION)

                        ' ; 
                        ''''Dim strContentALL As StringBuilder = New StringBuilder()
                        ''''Dim boolSetEmailList As Boolean = True
                        ''''Dim tbl_document_rep As DataTable

                        ''''Dim NDays As Decimal
                        ''''Dim NHours As Decimal
                        ''''Dim SHours As Decimal

                        '''''--Fore each pending approval started to build the EMail
                        '''''<!--##CATEGORY##-->;<!--##APPROVAL_NAME##-->;<!--##INSTRUMENT_NUMBER##-->;<!--##DESCRIPTION_DOC##-->;<!--##TABLE_STATUS##-->

                        ''''For Each tbl_approvalPending_dtR As DataRow In tbl_approvalPending.Rows

                        ''''    '************************************CONTENT**************************************************************************
                        ''''    Dim strContent As StringBuilder = New StringBuilder()
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    tbl_document_rep = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", tbl_approvalPending_dtR("id_documento"))
                        ''''    'tbl_document_rep.Rows(0).Item(
                        ''''    '************************************CONTENT**************************************************************************
                        ''''    strContent.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1))
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    'set_Apps_Document(dtR2("id_documento")) 'Set all AppDoc
                        ''''    '*****************FINDING the Destinataries **************************************

                        ''''    strContent.Replace("<!--##CATEGORY##-->", tbl_document_rep.Rows(0).Item("descripcion_cat"))
                        ''''    strContent.Replace("<!--##APPROVAL_NAME##-->", tbl_document_rep.Rows(0).Item("descripcion_aprobacion"))

                        ''''    strContent.Replace("<!--##ID_DOC##-->", tbl_document_rep.Rows(0).Item("id_documento"))
                        ''''    strContent.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

                        ''''    strContent.Replace("<!--##INSTRUMENT_NUMBER##-->", tbl_document_rep.Rows(0).Item("numero_instrumento"))

                        ''''    Dim strDocumentDesc As String = tbl_document_rep.Rows(0).Item("descripcion_doc") & fn_get_EmailList(tbl_approvalPending_dtR("id_documento"))
                        ''''    strContent.Replace("<!--##DESCRIPTION_DOC##-->", strDocumentDesc)

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    '*****************<!--##TABLE_STATUS##-->'*****************
                        ''''    set_Ta_RutaSeguimiento(tbl_document_rep.Rows(0).Item("id_documento")) 'getting the complete route

                        ''''    Dim strStyle As String = "<tr style='background-color:#ee7108;border:1 dotted #FF0000; ' >" 'Putting inside <tr>
                        ''''    'style="background-color:#ee7108;border:1 dotted #FF0000;"
                        ''''    Dim strTableContent As StringBuilder = New StringBuilder()
                        ''''    Dim strTable_Cont As StringBuilder = New StringBuilder()
                        ''''    'Dim strTableContentALL As StringBuilder = New StringBuilder()
                        ''''    Dim strTable As StringBuilder = New StringBuilder()

                        ''''    '<!--##CONTENT##-->;<!--##ROLE_NAME##-->;<!--##EMPLOYEE_NAME##-->;<!--##STATE##-->;<!--##DATE_RECEIPT##-->;<!--##ALERT_TYPE##-->;<!--##STRONG_OPEN##-->;<!--##STRONG_OPEN##-->;
                        ''''    strTable.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2))

                        ''''    SHours = 0
                        ''''    For Each seg_dtRow In Tbl_Ta_RutaSeguimiento.Rows

                        ''''        strTable_Cont.Clear()
                        ''''        strTable_Cont.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 2))


                        ''''        'strTableContent.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                        ''''        strTable_Cont.Replace("<!--##ROLE_NAME##-->", "")
                        ''''        strTable_Cont.Replace("<!--##EMPLOYEE_NAME##-->", seg_dtRow("nombre_empleado"))
                        ''''        strTable_Cont.Replace("<!--##STATE##-->", seg_dtRow("descripcion_estado"))
                        ''''        strTable_Cont.Replace("<!--##DATE_RECEIPT##-->", seg_dtRow("fecha_aprobacion"))
                        ''''        strTable_Cont.Replace("<!--##ALERT_TYPE##-->", seg_dtRow("Alerta"))

                        ''''        'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                        ''''        If ((tbl_document_rep.Rows(0).Item("id_ruta") = seg_dtRow("id_ruta")) And (tbl_document_rep.Rows(0).Item("id_estadoDoc") = seg_dtRow("id_estadoDoc"))) Then
                        ''''            strTable_Cont.Replace("<tr>", strStyle)
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "<strong>")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "</strong>")
                        ''''        Else
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "")
                        ''''        End If

                        ''''        SHours += seg_dtRow("NHRs")
                        ''''        NDays = Math.Round((seg_dtRow("NHRs") / 24), 0, MidpointRounding.AwayFromZero)

                        ''''        strTable_Cont.Replace("<!--##DAYS##-->", NDays)

                        ''''        strTableContent.Append(strTable_Cont.ToString)


                        ''''    Next

                        ''''    NDays = Int((SHours / 24))
                        ''''    NHours = Math.Round((((SHours / 24) - NDays) * 24), 0, MidpointRounding.AwayFromZero)

                        ''''    'strTableContentALL.Append(strTableContent)

                        ''''    strTable.Replace("<!--##TOTAL_DAYS##-->", String.Format(" {0} Days {1} Hrs", NDays, NHours))
                        ''''    strTable.Replace("<!--##CONTENT##-->", strTableContent.ToString)

                        ''''    strContent.Replace("<!--##TABLE_STATUS##-->", strTable.ToString)
                        ''''    strContentALL.Append(strContent.ToString)
                        ''''    strContentALL.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_nwline", 1))

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    'Add_emails(dtR2("id_documento"), dtR2("id_estadoDoc"), idOriginator, boolSetEmailList) 'Adding the respective email according the state of the approval proccess
                        ''''    'Add_document_email_list(dtR2("id_documento"), boolSetEmailList)
                        ''''    '*****************FINDING the Destinataries **************************************
                        ''''    'boolSetEmailList = False

                        ''''    '**************TESTING PRUPORSES
                        ''''    'Exit For
                        ''''    '**************TESTING PRUPORSES

                        ''''Next

                        ''''strMess.Replace("<!--##CONTENT##-->", strContentALL.ToString)


                        'Add_emails(idOriginator, "User") 'Adding the respective email according the user or the Role

                        If ObJemail.SilentMode Then
                            'If Not ObJemail.SilentMode Then

                            strSubject &= " (Silent MODE) "
                            Dim strTO As StringBuilder = New StringBuilder
                            Dim strCC As String = ""
                            Dim strBCC As String = ""

                            '*********************************************************************************************************
                            '******************we can do this getting all the selection List......************************************
                            '*********************************************************************************************************

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        strTO.Append(String.Format("{0};", dtRow("email").ToString.Trim))
                                    Case "CC"
                                        strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                End Select

                                ' strTO.Append(String.Format("{0};", EmailOriginator.ToString.Trim))

                                'If SecundaryEmail.Trim.Length > 1 Then
                                '    strTO.Append(String.Format("{0};", SecundaryEmail.ToString.Trim))
                                'End If

                            Next

                            ' Dim strEMAILS As String = String.Format("<br /><br /><br /><br /> TO:{0}<br />CC:{1}<br />BCC:{2} ", strTO, strCC, strBCC)
                            Dim strEMAILS As StringBuilder = New StringBuilder
                            strEMAILS.Append("<br /><br /><br /><br />TO:" & strTO.ToString & "<br /><br />")

                            strMess.Replace("<!--##MESS##-->", strEMAILS.ToString) 'Representavite emails for this  notification

                        Else

                            strMess.Replace("<!--##MESS##-->", "")
                            '*********************************************************************************************************
                            '*******************THIS IS the Individual Email for every Notification***********************************
                            'ObJemail.AddTo(EmailOriginator)

                            'If SecundaryEmail.Trim.Length > 1 Then
                            '    ObJemail.AddTo(SecundaryEmail.ToString)
                            'End If

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        ObJemail.AddTo(dtRow("email").ToString.Trim)
                                    Case "CC"
                                        ObJemail.AddCC(dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        ObJemail.AddBcc(dtRow("email").ToString.Trim)
                                End Select

                            Next

                            '*******************THIS IS the Individual Email for every notification***********************************
                            '*********************************************************************************************************

                        End If


                        '*******************************************************************
                        '************************SUBJECTS***********************************
                        ObJemail.AddSubject(strSubject)
                        '************************SUBJECTS***********************************
                        '*******************************************************************

                        '************************* THE message it supposed Is here, need to proceed to test*****************
                        '*****************building the email from the templates objects**************************************

                        strMess.Replace("<!--##IMG_CID2##-->", "LogProgram") 'Name of the resource the Program Logo
                        strMess.Replace("<!--##IMG_CID1##-->", "LogChemo") 'Name of the resource the Company logo

                        Dim strPath As String
                        If strBaseDir.Length > 1 Then
                            strPath = strBaseDir
                        Else
                            strPath = Server.MapPath("~")
                        End If

                        Dim strTMP As String = strMess.ToString

                        ObJemail.setAlternativeVIEW(strMess.ToString) 'Setting alternative View

                        Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                        Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                        If Not String.IsNullOrEmpty(strProgramIMG) Then
                            ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
                        Else
                            ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
                        End If

                        'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                        ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")


                        'ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Red_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Blue_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Orange_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Cyan_Time.png", "image/png")

                        ObJemail.SetPriority(MailPriority.High)

                        If ObJemail.SendEmail(id_notification, oSolicitationAPP.FirstOrDefault.ID_SOLICITATION_APP, "APPLY_EMAIL", "APPLICATION_SUBMITTED") Then
                            NOTIFIYING_SOLICITATION_APPLY = True
                        Else
                            NOTIFIYING_SOLICITATION_APPLY = False
                        End If

                    Else  '*********************Valid Email addres**********************************

                        NOTIFIYING_SOLICITATION_APPLY = False

                    End If


                End Using

            Catch ex As Exception

                NOTIFIYING_SOLICITATION_APPLY = False

            End Try


        End Function


        Public Function NOTIFIYING_SOLICITATION_SCREENING(ByVal id_solicitation_app As Integer, Optional strBaseDir As String = "") As Boolean

            Dim strMess As StringBuilder = New StringBuilder


            Try


                Using dbEntities As New dbRMS_JIEntities

                    Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_solicitation_app).ToList()

                    Dim id_activity_solicitation As Integer = oSolicitationAPP.FirstOrDefault.ID_ACTIVITY_SOLICITATION

                    Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(id_activity_solicitation)
                    Dim oTeamEvaluation = dbEntities.VW_TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_activity_solicitation).ToList()

                    'Dim oSolicitationApply = dbEntities.TA_APPLY_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_solicitation_app).FirstOrDefault()

                    'Dim EmailOriginator As String = oSolicitationAPP.FirstOrDefault.ORGANIZATIONEMAIL
                    'Dim SecundaryEmail As String = oSolicitationAPP.FirstOrDefault.PRIMARYCONTACTEMAIL

                    Dim EmailOriginator As String = ""
                    Dim SecundaryEmail As String = ""

                    Dim tbl_ As DataTable = get_t_notification_emails("CNTMNG")
                    Set_Emails_List()

                    If Not IsNothing(tbl_) Then

                        Add_Emails_List("TO", tbl_.Rows.Item(0).Item("email"))
                        EmailOriginator = tbl_.Rows.Item(0).Item("email")

                        Dim strBccEmails As String() = If(IsDBNull(tbl_.Rows.Item(0).Item("noti_cc")), "", tbl_.Rows.Item(0).Item("noti_cc")).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    'ObJemail.AddBcc(dtEmail)
                                    Add_Emails_List("CC", dtEmail)

                                End If

                            End If

                        Next

                    End If


                    If IsEmail(EmailOriginator) Then

                        'Dim tbl_approvalCC As DataTable = get_notification_Pending_ByUser(0, "CC", EmailOriginator.Trim, 0) '--Getting the notifications and sending to its respective sender
                        'Dim boolFound As Boolean = False

                        'For Each app_CC_dtRow As DataRow In tbl_EmailIncluded.Rows

                        '    If app_CC_dtRow("type_subject") = "CC" And app_CC_dtRow("email").ToString.Trim = EmailOriginator.Trim Then

                        '        For Each app_pend_dtRow As DataRow In tbl_approvalPending.Rows

                        '            If app_pend_dtRow("id_documento") = app_CC_dtRow("id_documento") Then
                        '                boolFound = True
                        '                Exit For
                        '            End If

                        '        Next

                        '        If Not boolFound Then
                        '            tbl_approvalPending.ImportRow(app_CC_dtRow)
                        '        End If

                        '        boolFound = False


                        '    End If

                        'Next


                        '************************************SYSTEM INFO********************************************
                        Dim cProgram As New RMS.cls_Program
                        cProgram.get_Sys(0, True)
                        cProgram.get_Programs(id_programa, True)
                        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
                        dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                        '************************************SYSTEM INFO********************************************

                        '*****************Creating the email Object**************************************
                        ObJemail = New UTILS.cls_email(id_programa)
                        '*****************Creating the email Object**************************************

                        '*****************building the email from the templates objects**************************************
                        set_t_notification(id_notification) 'Create the Noti_Object
                        set_t_Template() 'Set Template object from the notification id
                        strMess.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)) 'getting the first template

                        strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

                        '*****************FINDING the BCC destinataries **************************************
                        Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    ObJemail.AddBcc(dtEmail)
                                End If

                            End If

                        Next
                        ''*****************FINDING the BCC destinataries **************************************



                        '*****************ADDING THE SUBJECTS**********************
                        Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

                        strSubject = strSubject.Replace("<!--##SOLICITATION_NUMBER##-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)
                        strSubject = strSubject.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strSubject = strSubject.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)

                        '<!--##PROJECT_NAME##-->)


                        '<!--##ORGANIZATION_NAME##-->
                        '<!--##DESTINATION_EMAIL##-->
                        '<!--#SOLICITATION##-->
                        '<!--#SOLICITATION_TITLE##-->
                        '<!--#SOLICITATION_PURPOSE##-->
                        '<!--##SOLICITATION_NUMBER##-->
                        '<!--#START_DATE##-->
                        '<!--END_DATE##-->
                        '<!--##SYS_PATH##-->
                        '<!--##ID_DOC##-->


                        strMess.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strMess.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        strMess.Replace("<!--##DESTINATION_EMAIL##-->", String.Format("{0};{1};", EmailOriginator, SecundaryEmail))
                        strMess.Replace("<!--#SOLICITATION##-->", oActivitySolicitation.SOLICITATION)
                        strMess.Replace("<!--#SOLICITATION_PURPOSE##-->", oActivitySolicitation.SOLICITATION_PURPOSE)
                        strMess.Replace("<!--##SOLICITATION_NUMBER##-->", oActivitySolicitation.SOLICITATION_CODE)

                        strMess.Replace("<!--#START_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.start_date, "f", timezoneUTC, True))
                        strMess.Replace("<!--END_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.end_date, "f", timezoneUTC, True))

                        strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys))


                        Dim URL_Keys As String = String.Format("ab={0}&ac={1}&ad={2}", oActivitySolicitation.SOLICITATION_TOKEN.ToString, oSolicitationAPP.FirstOrDefault.SOLICITATION_TOKEN, oSolicitationAPP.FirstOrDefault.USER_TOKEN_UPDATE.ToString)
                        strMess.Replace("<!--##ID_DOC##-->", URL_Keys)


                        strMess.Replace("<!--RECEIVED_DATE##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SENT_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--OPENED_BY##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)

                        strMess.Replace("<!--#APPLY_CODE#-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)

                        strMess.Replace("<!--#APPLY_DEFINITION#-->", "PRESCREENING OF " & oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)

                        Dim strPrescreening_Tbl As StringBuilder = New StringBuilder()

                        strPrescreening_Tbl.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1))

                        Dim strPreScreeningRow As StringBuilder = New StringBuilder()
                        strPreScreeningRow.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 1))

                        Dim strRow As StringBuilder = New StringBuilder()
                        Dim strRowTOT As StringBuilder = New StringBuilder()


                        Dim oVW_TA_APPLY_SCREENING_ANSWER = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = id_solicitation_app).OrderBy(Function(o) o.order_numberQC).ToList()
                        Dim totScored As Double = 0
                        Dim totPendingScored As Double = 0


                        Dim oTA_APPLY_SCREENING = dbEntities.TA_APPLY_SCREENING.Where(Function(p) p.ID_SOLICITATION_APP = Id_solicitation_app)

                        strMess.Replace("<!--#SUBMITTED_DATE#-->", dateUtil.set_DateFormat(oTA_APPLY_SCREENING.FirstOrDefault.DATE_ANSWERED, "f", timezoneUTC, True))


                        Dim idAnswerScale As Integer = 0
                        Dim remaining_Score As Double = 0
                        Dim remaining_Question As String = ""

                        For Each dtItem In oVW_TA_APPLY_SCREENING_ANSWER

                            strRow.Append(strPreScreeningRow)

                            strRow.Replace("<!--##PRESCREENING_NO-->", dtItem.order_numberQC)
                            strRow.Replace("<!--##PRESCREENING_QUESTION-->", dtItem.question_name)

                            Dim answCODE As String = dtItem.answer_type_code
                            Dim strAnswer As String = ""

                            If answCODE = "DROPDOWN" Then

                                strAnswer = dtItem.option_name

                            ElseIf answCODE = "TEXTENTRY" Then

                                strAnswer = dtItem.measurement_answer_text

                            ElseIf answCODE = "VALUEENTRY" Then

                                strAnswer = dtItem.measurement_answer_value.ToString

                            End If

                            strRow.Replace("<!--##PRESCREENING_ANSWER-->", strAnswer)

                            strRow.Replace("<!--##PRESCREENING_SCORE-->", dtItem.percent_valueAO)

                            totScored += dtItem.percent_valueAO

                            Dim oQUESTION = dbEntities.tme_measurement_question.Find(dtItem.id_measurement_question_eval)

                            If Not IsNothing(oQUESTION) Then
                                idAnswerScale = oQUESTION.tme_measurement_answer_scale.id_measurement_answer_scale
                                remaining_Score = dbEntities.tme_measurement_answer_option.Where(Function(p) p.id_measurement_answer_scale = idAnswerScale).Max(Function(f) f.percent_value)
                                remaining_Question = oQUESTION.question_name
                            Else
                                remaining_Score = 0
                                remaining_Question = ""
                            End If

                            totPendingScored += remaining_Score

                            strRow.Replace("<!--##REMAINING_QUESTION-->", remaining_Question)
                            strRow.Replace("<!--##PRESCREENING_REMAINING_SCORE-->", String.Format("{0:N4}", remaining_Score))

                            strRowTOT.Append(strRow)

                            strRow.Clear()

                        Next

                        strPrescreening_Tbl.Replace("<!--##PRESCREENING_CONTENT-->", strRowTOT.ToString())
                        strPrescreening_Tbl.Replace("<!--##TOTAL_SCORING-->", String.Format("{0:N4}", totScored))
                        strPrescreening_Tbl.Replace("<!--##REMAINING_SCORING-->", String.Format("{0:N4}", totPendingScored))


                        strMess.Replace("<!--#SCREENING_TABLE#-->", strPrescreening_Tbl.ToString())

                        ''''Dim strContentALL As StringBuilder = New StringBuilder()
                        ''''Dim boolSetEmailList As Boolean = True
                        ''''Dim tbl_document_rep As DataTable

                        ''''Dim NDays As Decimal
                        ''''Dim NHours As Decimal
                        ''''Dim SHours As Decimal

                        '''''--Fore each pending approval started to build the EMail
                        '''''<!--##CATEGORY##-->;<!--##APPROVAL_NAME##-->;<!--##INSTRUMENT_NUMBER##-->;<!--##DESCRIPTION_DOC##-->;<!--##TABLE_STATUS##-->

                        ''''For Each tbl_approvalPending_dtR As DataRow In tbl_approvalPending.Rows

                        ''''    '************************************CONTENT**************************************************************************
                        ''''    Dim strContent As StringBuilder = New StringBuilder()
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    tbl_document_rep = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", tbl_approvalPending_dtR("id_documento"))
                        ''''    'tbl_document_rep.Rows(0).Item(
                        ''''    '************************************CONTENT**************************************************************************
                        ''''    strContent.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1))
                        ''''    '************************************CONTENT**************************************************************************

                        ''''    'set_Apps_Document(dtR2("id_documento")) 'Set all AppDoc
                        ''''    '*****************FINDING the Destinataries **************************************

                        ''''    strContent.Replace("<!--##CATEGORY##-->", tbl_document_rep.Rows(0).Item("descripcion_cat"))
                        ''''    strContent.Replace("<!--##APPROVAL_NAME##-->", tbl_document_rep.Rows(0).Item("descripcion_aprobacion"))

                        ''''    strContent.Replace("<!--##ID_DOC##-->", tbl_document_rep.Rows(0).Item("id_documento"))
                        ''''    strContent.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

                        ''''    strContent.Replace("<!--##INSTRUMENT_NUMBER##-->", tbl_document_rep.Rows(0).Item("numero_instrumento"))

                        ''''    Dim strDocumentDesc As String = tbl_document_rep.Rows(0).Item("descripcion_doc") & fn_get_EmailList(tbl_approvalPending_dtR("id_documento"))
                        ''''    strContent.Replace("<!--##DESCRIPTION_DOC##-->", strDocumentDesc)

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    '*****************<!--##TABLE_STATUS##-->'*****************
                        ''''    set_Ta_RutaSeguimiento(tbl_document_rep.Rows(0).Item("id_documento")) 'getting the complete route

                        ''''    Dim strStyle As String = "<tr style='background-color:#ee7108;border:1 dotted #FF0000; ' >" 'Putting inside <tr>
                        ''''    'style="background-color:#ee7108;border:1 dotted #FF0000;"
                        ''''    Dim strTableContent As StringBuilder = New StringBuilder()
                        ''''    Dim strTable_Cont As StringBuilder = New StringBuilder()
                        ''''    'Dim strTableContentALL As StringBuilder = New StringBuilder()
                        ''''    Dim strTable As StringBuilder = New StringBuilder()

                        ''''    '<!--##CONTENT##-->;<!--##ROLE_NAME##-->;<!--##EMPLOYEE_NAME##-->;<!--##STATE##-->;<!--##DATE_RECEIPT##-->;<!--##ALERT_TYPE##-->;<!--##STRONG_OPEN##-->;<!--##STRONG_OPEN##-->;
                        ''''    strTable.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2))

                        ''''    SHours = 0
                        ''''    For Each seg_dtRow In Tbl_Ta_RutaSeguimiento.Rows

                        ''''        strTable_Cont.Clear()
                        ''''        strTable_Cont.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 2))


                        ''''        'strTableContent.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                        ''''        strTable_Cont.Replace("<!--##ROLE_NAME##-->", "")
                        ''''        strTable_Cont.Replace("<!--##EMPLOYEE_NAME##-->", seg_dtRow("nombre_empleado"))
                        ''''        strTable_Cont.Replace("<!--##STATE##-->", seg_dtRow("descripcion_estado"))
                        ''''        strTable_Cont.Replace("<!--##DATE_RECEIPT##-->", seg_dtRow("fecha_aprobacion"))
                        ''''        strTable_Cont.Replace("<!--##ALERT_TYPE##-->", seg_dtRow("Alerta"))

                        ''''        'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                        ''''        If ((tbl_document_rep.Rows(0).Item("id_ruta") = seg_dtRow("id_ruta")) And (tbl_document_rep.Rows(0).Item("id_estadoDoc") = seg_dtRow("id_estadoDoc"))) Then
                        ''''            strTable_Cont.Replace("<tr>", strStyle)
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "<strong>")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "</strong>")
                        ''''        Else
                        ''''            strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "")
                        ''''            strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "")
                        ''''        End If

                        ''''        SHours += seg_dtRow("NHRs")
                        ''''        NDays = Math.Round((seg_dtRow("NHRs") / 24), 0, MidpointRounding.AwayFromZero)

                        ''''        strTable_Cont.Replace("<!--##DAYS##-->", NDays)

                        ''''        strTableContent.Append(strTable_Cont.ToString)


                        ''''    Next

                        ''''    NDays = Int((SHours / 24))
                        ''''    NHours = Math.Round((((SHours / 24) - NDays) * 24), 0, MidpointRounding.AwayFromZero)

                        ''''    'strTableContentALL.Append(strTableContent)

                        ''''    strTable.Replace("<!--##TOTAL_DAYS##-->", String.Format(" {0} Days {1} Hrs", NDays, NHours))
                        ''''    strTable.Replace("<!--##CONTENT##-->", strTableContent.ToString)

                        ''''    strContent.Replace("<!--##TABLE_STATUS##-->", strTable.ToString)
                        ''''    strContentALL.Append(strContent.ToString)
                        ''''    strContentALL.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_nwline", 1))

                        ''''    '************************************STATUS TABLE**************************************************************************
                        ''''    'Add_emails(dtR2("id_documento"), dtR2("id_estadoDoc"), idOriginator, boolSetEmailList) 'Adding the respective email according the state of the approval proccess
                        ''''    'Add_document_email_list(dtR2("id_documento"), boolSetEmailList)
                        ''''    '*****************FINDING the Destinataries **************************************
                        ''''    'boolSetEmailList = False

                        ''''    '**************TESTING PRUPORSES
                        ''''    'Exit For
                        ''''    '**************TESTING PRUPORSES

                        ''''Next

                        ''''strMess.Replace("<!--##CONTENT##-->", strContentALL.ToString)


                        'Add_emails(idOriginator, "User") 'Adding the respective email according the user or the Role

                        If ObJemail.SilentMode Then
                            'If Not ObJemail.SilentMode Then

                            strSubject &= " (Silent MODE) "
                            Dim strTO As StringBuilder = New StringBuilder
                            Dim strCC As String = ""
                            Dim strBCC As String = ""

                            '*********************************************************************************************************
                            '******************we can do this getting all the selection List......************************************
                            '*********************************************************************************************************

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        strTO.Append(String.Format("{0};", dtRow("email").ToString.Trim))
                                    Case "CC"
                                        strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                End Select

                                ' strTO.Append(String.Format("{0};", EmailOriginator.ToString.Trim))

                                'If SecundaryEmail.Trim.Length > 1 Then
                                '    strTO.Append(String.Format("{0};", SecundaryEmail.ToString.Trim))
                                'End If

                            Next

                            ' Dim strEMAILS As String = String.Format("<br /><br /><br /><br /> TO:{0}<br />CC:{1}<br />BCC:{2} ", strTO, strCC, strBCC)
                            Dim strEMAILS As StringBuilder = New StringBuilder
                            strEMAILS.Append("<br /><br /><br /><br />TO:" & strTO.ToString & "<br /><br />")

                            strMess.Replace("<!--##MESS##-->", strEMAILS.ToString) 'Representavite emails for this  notification

                        Else

                            strMess.Replace("<!--##MESS##-->", "")
                            '*********************************************************************************************************
                            '*******************THIS IS the Individual Email for every Notification***********************************
                            'ObJemail.AddTo(EmailOriginator)

                            'If SecundaryEmail.Trim.Length > 1 Then
                            '    ObJemail.AddTo(SecundaryEmail.ToString)
                            'End If

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        ObJemail.AddTo(dtRow("email").ToString.Trim)
                                    Case "CC"
                                        ObJemail.AddCC(dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        ObJemail.AddBcc(dtRow("email").ToString.Trim)
                                End Select

                            Next

                            '*******************THIS IS the Individual Email for every notification***********************************
                            '*********************************************************************************************************

                        End If


                        '*******************************************************************
                        '************************SUBJECTS***********************************
                        ObJemail.AddSubject(strSubject)
                        '************************SUBJECTS***********************************
                        '*******************************************************************

                        '************************* THE message it supposed Is here, need to proceed to test*****************
                        '*****************building the email from the templates objects**************************************

                        strMess.Replace("<!--##IMG_CID2##-->", "LogProgram") 'Name of the resource the Program Logo
                        strMess.Replace("<!--##IMG_CID1##-->", "LogChemo") 'Name of the resource the Company logo

                        Dim strPath As String
                        If strBaseDir.Length > 1 Then
                            strPath = strBaseDir
                        Else
                            strPath = Server.MapPath("~")
                        End If

                        Dim strTMP As String = strMess.ToString

                        ObJemail.setAlternativeVIEW(strMess.ToString) 'Setting alternative View

                        Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                        Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                        If Not String.IsNullOrEmpty(strProgramIMG) Then
                            ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
                        Else
                            ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
                        End If

                        'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                        ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")


                        'ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Red_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Blue_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Orange_Time.png", "image/png")
                        'ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Cyan_Time.png", "image/png")

                        ObJemail.SetPriority(MailPriority.High)

                        If ObJemail.SendEmail(id_notification, oSolicitationAPP.FirstOrDefault.ID_SOLICITATION_APP, "SOLICITATION_EMAIL", "PRESCREENING_SUBMITTED") Then
                            NOTIFIYING_SOLICITATION_SCREENING = True
                        Else
                            NOTIFIYING_SOLICITATION_SCREENING = False
                        End If

                    Else  '*********************Valid Email addres**********************************

                        NOTIFIYING_SOLICITATION_SCREENING = False

                    End If


                End Using

            Catch ex As Exception

                NOTIFIYING_SOLICITATION_SCREENING = False

            End Try


        End Function



        Public Function NOTIFIYING_SOLICITATION_SCREENING_RESP(ByVal id_solicitation_app As Integer, Optional strBaseDir As String = "") As Boolean

            Dim strMess As StringBuilder = New StringBuilder


            Try


                Using dbEntities As New dbRMS_JIEntities

                    Dim oSolicitationAPP = dbEntities.VW_TA_SOLICITATION_APP.Where(Function(p) p.ID_SOLICITATION_APP = id_solicitation_app).ToList()

                    Dim id_activity_solicitation As Integer = oSolicitationAPP.FirstOrDefault.ID_ACTIVITY_SOLICITATION

                    Dim oActivitySolicitation = dbEntities.TA_ACTIVITY_SOLICITATION.Find(id_activity_solicitation)
                    Dim oTeamEvaluation = dbEntities.VW_TA_SOLICITATION_EVALUATION_TEAM.Where(Function(p) p.ID_ACTIVITY_SOLICITATION = id_activity_solicitation).ToList()


                    Dim EmailOriginator As String = oSolicitationAPP.FirstOrDefault.ORGANIZATIONEMAIL
                    Dim SecundaryEmail As String = oSolicitationAPP.FirstOrDefault.PRIMARYCONTACTEMAIL

                    Dim tbl_ As DataTable = get_t_notification_emails("CNTMNG")
                    Set_Emails_List()

                    If Not IsNothing(tbl_) Then

                        Add_Emails_List("TO", tbl_.Rows.Item(0).Item("email"))
                        'EmailOriginator = tbl_.Rows.Item(0).Item("email")

                        If IsEmail(EmailOriginator) Then
                            Add_Emails_List("TO", EmailOriginator)
                        End If
                        If IsEmail(SecundaryEmail) Then
                            Add_Emails_List("CC", SecundaryEmail)
                        End If

                        Dim strBccEmails As String() = If(IsDBNull(tbl_.Rows.Item(0).Item("noti_cc")), "", tbl_.Rows.Item(0).Item("noti_cc")).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    'ObJemail.AddBcc(dtEmail)
                                    Add_Emails_List("CC", dtEmail)
                                    'SecundaryEmail &= String.Format("{0};", dtEmail)

                                End If

                            End If

                        Next

                    End If


                    If IsEmail(EmailOriginator) Then


                        '************************************SYSTEM INFO********************************************
                        Dim cProgram As New RMS.cls_Program
                        cProgram.get_Sys(0, True)
                        cProgram.get_Programs(id_programa, True)
                        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
                        dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                        '************************************SYSTEM INFO********************************************

                        '*****************Creating the email Object**************************************
                        ObJemail = New UTILS.cls_email(id_programa)
                        '*****************Creating the email Object**************************************

                        '*****************building the email from the templates objects**************************************
                        set_t_notification(id_notification) 'Create the Noti_Object
                        set_t_Template() 'Set Template object from the notification id
                        strMess.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)) 'getting the first template

                        strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

                        '*****************FINDING the BCC destinataries **************************************
                        Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

                        For Each dtEmail As String In strBccEmails

                            If Not String.IsNullOrEmpty(dtEmail) Then

                                If IsEmail(dtEmail) Then
                                    ObJemail.AddBcc(dtEmail)
                                End If

                            End If

                        Next
                        ''*****************FINDING the BCC destinataries **************************************



                        '*****************ADDING THE SUBJECTS**********************
                        Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

                        strSubject = strSubject.Replace("<!--##SOLICITATION_NUMBER##-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)
                        strSubject = strSubject.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strSubject = strSubject.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)

                        '<!--##PROJECT_NAME##-->)


                        '<!--##ORGANIZATION_NAME##-->
                        '<!--##DESTINATION_EMAIL##-->
                        '<!--#SOLICITATION##-->
                        '<!--#SOLICITATION_TITLE##-->
                        '<!--#SOLICITATION_PURPOSE##-->
                        '<!--##SOLICITATION_NUMBER##-->
                        '<!--#START_DATE##-->
                        '<!--END_DATE##-->
                        '<!--##SYS_PATH##-->
                        '<!--##ID_DOC##-->


                        strMess.Replace("<!--#SOLICITATION_TITLE##-->", oActivitySolicitation.SOLICITATION_TITLE)
                        strMess.Replace("<!--##ORGANIZATION_NAME##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)
                        strMess.Replace("<!--##DESTINATION_EMAIL##-->", String.Format("{0};{1};", EmailOriginator, SecundaryEmail))
                        strMess.Replace("<!--#SOLICITATION##-->", oActivitySolicitation.SOLICITATION)
                        strMess.Replace("<!--#SOLICITATION_PURPOSE##-->", oActivitySolicitation.SOLICITATION_PURPOSE)
                        strMess.Replace("<!--##SOLICITATION_NUMBER##-->", oActivitySolicitation.SOLICITATION_CODE)

                        strMess.Replace("<!--#START_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.start_date, "f", timezoneUTC, True))
                        strMess.Replace("<!--END_DATE##-->", dateUtil.set_DateFormat(oActivitySolicitation.end_date, "f", timezoneUTC, True))

                        strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys))


                        Dim URL_Keys As String = String.Format("ab={0}&ac={1}&ad={2}", oActivitySolicitation.SOLICITATION_TOKEN.ToString, oSolicitationAPP.FirstOrDefault.SOLICITATION_TOKEN, oSolicitationAPP.FirstOrDefault.USER_TOKEN_UPDATE.ToString)
                        strMess.Replace("<!--##ID_DOC##-->", URL_Keys)


                        strMess.Replace("<!--RECEIVED_DATE##-->", dateUtil.set_DateFormat(oSolicitationAPP.FirstOrDefault.SENT_DATE, "f", timezoneUTC, True))
                        strMess.Replace("<!--OPENED_BY##-->", oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)

                        strMess.Replace("<!--#APPLY_CODE#-->", oSolicitationAPP.FirstOrDefault.SOLICITATION_APP_CODE)

                        strMess.Replace("<!--#APPLY_DEFINITION#-->", "PRESCREENING OF " & oSolicitationAPP.FirstOrDefault.ORGANIZATIONNAME)

                        Dim strPrescreening_Tbl As StringBuilder = New StringBuilder()

                        strPrescreening_Tbl.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1))

                        Dim strPreScreeningRow As StringBuilder = New StringBuilder()
                        strPreScreeningRow.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 1))

                        Dim strRow As StringBuilder = New StringBuilder()
                        Dim strRowTOT As StringBuilder = New StringBuilder()


                        Dim oVW_TA_APPLY_SCREENING_ANSWER = dbEntities.VW_TA_APPLY_SCREENING_ANSWER.Where(Function(p) p.ID_SOLICITATION_APP = id_solicitation_app).OrderBy(Function(o) o.order_numberQC).ToList()
                        Dim totScored As Double = 0
                        Dim totPendingScored As Double = 0


                        Dim oTA_APPLY_SCREENING = dbEntities.TA_APPLY_SCREENING.Where(Function(p) p.ID_SOLICITATION_APP = id_solicitation_app)

                        strMess.Replace("<!--#SUBMITTED_DATE#-->", dateUtil.set_DateFormat(oTA_APPLY_SCREENING.FirstOrDefault.DATE_ANSWERED, "f", timezoneUTC, True))


                        Dim idAnswerScale As Integer = 0
                        Dim remaining_Score As Double = 0
                        Dim remaining_Question As String = ""

                        For Each dtItem In oVW_TA_APPLY_SCREENING_ANSWER

                            strRow.Append(strPreScreeningRow)

                            strRow.Replace("<!--##PRESCREENING_NO-->", dtItem.order_numberQC)
                            strRow.Replace("<!--##PRESCREENING_QUESTION-->", dtItem.question_name)

                            Dim answCODE As String = dtItem.answer_type_code
                            Dim strAnswer As String = ""

                            If answCODE = "DROPDOWN" Then

                                strAnswer = dtItem.option_name

                            ElseIf answCODE = "TEXTENTRY" Then

                                strAnswer = dtItem.measurement_answer_text

                            ElseIf answCODE = "VALUEENTRY" Then

                                strAnswer = dtItem.measurement_answer_value.ToString

                            End If

                            strRow.Replace("<!--##PRESCREENING_ANSWER-->", strAnswer)

                            'strRow.Replace("<!--##PRESCREENING_SCORE-->", dtItem.percent_valueAO)

                            totScored += dtItem.percent_valueAO

                            Dim oQUESTION = dbEntities.tme_measurement_question.Find(dtItem.id_measurement_question_eval)

                            If Not IsNothing(oQUESTION) Then
                                idAnswerScale = oQUESTION.tme_measurement_answer_scale.id_measurement_answer_scale
                                remaining_Score = dbEntities.tme_measurement_answer_option.Where(Function(p) p.id_measurement_answer_option = dtItem.id_measurement_answer_option_eval).FirstOrDefault.percent_value
                                remaining_Question = oQUESTION.question_name
                            Else
                                remaining_Score = 0
                                remaining_Question = ""
                            End If

                            totPendingScored += remaining_Score

                            'strRow.Replace("<!--##REMAINING_QUESTION-->", remaining_Question)
                            'strRow.Replace("<!--##PRESCREENING_REMAINING_SCORE-->", String.Format("{0:N4}", remaining_Score))

                            strRowTOT.Append(strRow)

                            strRow.Clear()

                        Next

                        strPrescreening_Tbl.Replace("<!--##PRESCREENING_CONTENT-->", strRowTOT.ToString())
                        'strPrescreening_Tbl.Replace("<!--##TOTAL_SCORING-->", String.Format("{0:N4}", totScored))
                        'strPrescreening_Tbl.Replace("<!--##REMAINING_SCORING-->", String.Format("{0:N4}", totPendingScored))

                        strMess.Replace("<!--#SCREENING_TABLE#-->", strPrescreening_Tbl.ToString())

                        Dim idApplyScreening As Integer = oTA_APPLY_SCREENING.FirstOrDefault.ID_APPLY_SCREENING
                        Dim oTA_APPLY_SCREENING_COMM = dbEntities.TA_APPLY_SCREENING_COMM.Where(Function(p) p.ID_APPLY_SCREENING = idApplyScreening).OrderByDescending(Function(o) o.FECHA_CREA).FirstOrDefault

                        strMess.Replace("<!--##PREESCREENING_RESPONSE##-->", oTA_APPLY_SCREENING_COMM.SCREENING_COMM)


                        If ObJemail.SilentMode Then
                            'If Not ObJemail.SilentMode Then

                            strSubject &= " (Silent MODE) "
                            Dim strTO As StringBuilder = New StringBuilder
                            Dim strCC As String = ""
                            Dim strBCC As String = ""

                            '*********************************************************************************************************
                            '******************we can do this getting all the selection List......************************************
                            '*********************************************************************************************************

                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        strTO.Append(String.Format("{0};", dtRow("email").ToString.Trim))
                                    Case "CC"
                                        strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                                End Select

                            Next

                            Dim strEMAILS As StringBuilder = New StringBuilder
                            strEMAILS.Append("<br /><br /><br /><br />TO:" & strTO.ToString & "<br /><br />")

                            strMess.Replace("<!--##MESS##-->", strEMAILS.ToString) 'Representavite emails for this  notification

                        Else

                            strMess.Replace("<!--##MESS##-->", "")
                            '*********************************************************************************************************
                            '*******************THIS IS the Individual Email for every Notification***********************************


                            For Each dtRow In tbl_emails.Rows

                                Select Case dtRow("Tipo")
                                    Case "TO"
                                        ObJemail.AddTo(dtRow("email").ToString.Trim)
                                    Case "CC"
                                        ObJemail.AddCC(dtRow("email").ToString.Trim)
                                    Case "BCC"
                                        ObJemail.AddBcc(dtRow("email").ToString.Trim)
                                End Select

                            Next

                            '*******************THIS IS the Individual Email for every notification***********************************
                            '*********************************************************************************************************

                        End If


                        '*******************************************************************
                        '************************SUBJECTS***********************************
                        ObJemail.AddSubject(strSubject)
                        '************************SUBJECTS***********************************
                        '*******************************************************************

                        '************************* THE message it supposed Is here, need to proceed to test*****************
                        '*****************building the email from the templates objects**************************************

                        strMess.Replace("<!--##IMG_CID2##-->", "LogProgram") 'Name of the resource the Program Logo
                        strMess.Replace("<!--##IMG_CID1##-->", "LogChemo") 'Name of the resource the Company logo

                        Dim strPath As String
                        If strBaseDir.Length > 1 Then
                            strPath = strBaseDir
                        Else
                            strPath = Server.MapPath("~")
                        End If

                        Dim strTMP As String = strMess.ToString

                        ObJemail.setAlternativeVIEW(strMess.ToString) 'Setting alternative View

                        Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                        Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                        If Not String.IsNullOrEmpty(strProgramIMG) Then
                            ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
                        Else
                            ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
                        End If

                        ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

                        ObJemail.SetPriority(MailPriority.High)

                        If ObJemail.SendEmail(id_notification, oSolicitationAPP.FirstOrDefault.ID_SOLICITATION_APP, "SOLICITATION_EMAIL", "PRESCREENING_RESPONSE") Then
                            NOTIFIYING_SOLICITATION_SCREENING_RESP = True
                        Else
                            NOTIFIYING_SOLICITATION_SCREENING_RESP = False
                        End If

                    Else  '*********************Valid Email addres**********************************

                        NOTIFIYING_SOLICITATION_SCREENING_RESP = False

                    End If


                End Using

            Catch ex As Exception

                NOTIFIYING_SOLICITATION_SCREENING_RESP = False

            End Try


        End Function


        Public Function Notify_Reminder_Proc(ByVal idOriginator As Integer, Optional strBaseDir As String = "", Optional tbl_docs As DataTable = Nothing) As Boolean

            '  Dim tbl_approvalPending As DataTable = get_notificationPendingApp_ByOriginator(idOriginator) '--Getting the standbyNotification and sending to its respective originator
            Dim tbl_approvalPending As DataTable = get_notification_Pending_ByUser(idOriginator, "NNN", "--none--", 0) '--Getting the notifications and sending to its respective sender
            Dim strMess As StringBuilder = New StringBuilder

            Dim EmailOriginator As String = tbl_approvalPending.Rows(0).Item("email")

            '***************Store All the pending app with its respective emails************************
            tbl_EmailIncluded = tbl_docs.Copy
            'tbl_EmailIncluded = get_notification_Pending_ByUser(0, "NNN", "--none--", 0) '--to Getting the EmailList for the Documents
            '***************Store All the pending app with its respective emails************************

            Try



                If IsEmail(EmailOriginator) Then

                    'Dim tbl_approvalCC As DataTable = get_notification_Pending_ByUser(0, "CC", EmailOriginator.Trim, 0) '--Getting the notifications and sending to its respective sender
                    Dim boolFound As Boolean = False

                    For Each app_CC_dtRow As DataRow In tbl_EmailIncluded.Rows

                        If app_CC_dtRow("type_subject") = "CC" And app_CC_dtRow("email").ToString.Trim = EmailOriginator.Trim Then

                            For Each app_pend_dtRow As DataRow In tbl_approvalPending.Rows

                                If app_pend_dtRow("id_documento") = app_CC_dtRow("id_documento") Then
                                    boolFound = True
                                    Exit For
                                End If

                            Next

                            If Not boolFound Then
                                tbl_approvalPending.ImportRow(app_CC_dtRow)
                            End If

                            boolFound = False


                        End If

                    Next

                    '************************************SYSTEM INFO********************************************
                    Dim cProgram As New RMS.cls_Program
                    cProgram.get_Sys(0, True)
                    cProgram.get_Programs(id_programa, True)
                    timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
                    dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
                    '************************************SYSTEM INFO********************************************

                    '*****************Creating the email Object**************************************
                    ObJemail = New UTILS.cls_email(id_programa)
                    '*****************Creating the email Object**************************************

                    '*****************building the email from the templates objects**************************************
                    set_t_notification(id_notification) 'Create the Noti_Object
                    set_t_Template() 'Set Template object from the notification id
                    strMess.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)) 'getting the first template

                    strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))

                    '*****************FINDING the BCC destinataries **************************************
                    Dim strBccEmails As String() = get_t_notificationFIELDS("nt_BCC", "id_notification", id_notification).Split(";")

                    For Each dtEmail As String In strBccEmails

                        If Not String.IsNullOrEmpty(dtEmail) Then

                            If IsEmail(dtEmail) Then
                                ObJemail.AddBcc(dtEmail)
                            End If

                        End If

                    Next
                    ''*****************FINDING the BCC destinataries **************************************

                    '*****************ADDING THE SUBJECTS**********************
                    Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

                    Dim strContentALL As StringBuilder = New StringBuilder()
                    Dim boolSetEmailList As Boolean = True
                    Dim tbl_document_rep As DataTable

                    Dim NDays As Decimal
                    Dim NHours As Decimal
                    Dim SHours As Decimal

                    '--Fore each pending approval started to build the EMail
                    '<!--##CATEGORY##-->;<!--##APPROVAL_NAME##-->;<!--##INSTRUMENT_NUMBER##-->;<!--##DESCRIPTION_DOC##-->;<!--##TABLE_STATUS##-->

                    For Each tbl_approvalPending_dtR As DataRow In tbl_approvalPending.Rows

                        '************************************CONTENT**************************************************************************
                        Dim strContent As StringBuilder = New StringBuilder()
                        '************************************CONTENT**************************************************************************

                        tbl_document_rep = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", tbl_approvalPending_dtR("id_documento"))
                        'tbl_document_rep.Rows(0).Item(
                        '************************************CONTENT**************************************************************************
                        strContent.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1))
                        '************************************CONTENT**************************************************************************

                        'set_Apps_Document(dtR2("id_documento")) 'Set all AppDoc
                        '*****************FINDING the Destinataries **************************************

                        strContent.Replace("<!--##CATEGORY##-->", tbl_document_rep.Rows(0).Item("descripcion_cat"))
                        strContent.Replace("<!--##APPROVAL_NAME##-->", tbl_document_rep.Rows(0).Item("descripcion_aprobacion"))

                        strContent.Replace("<!--##ID_DOC##-->", tbl_document_rep.Rows(0).Item("id_documento"))
                        strContent.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

                        strContent.Replace("<!--##INSTRUMENT_NUMBER##-->", tbl_document_rep.Rows(0).Item("numero_instrumento"))

                        Dim strDocumentDesc As String = tbl_document_rep.Rows(0).Item("descripcion_doc") & fn_get_EmailList(tbl_approvalPending_dtR("id_documento"))
                        strContent.Replace("<!--##DESCRIPTION_DOC##-->", strDocumentDesc)

                        '************************************STATUS TABLE**************************************************************************
                        '*****************<!--##TABLE_STATUS##-->'*****************
                        set_Ta_RutaSeguimiento(tbl_document_rep.Rows(0).Item("id_documento")) 'getting the complete route

                        Dim strStyle As String = "<tr style='background-color:#ee7108;border:1 dotted #FF0000; ' >" 'Putting inside <tr>
                        'style="background-color:#ee7108;border:1 dotted #FF0000;"
                        Dim strTableContent As StringBuilder = New StringBuilder()
                        Dim strTable_Cont As StringBuilder = New StringBuilder()
                        'Dim strTableContentALL As StringBuilder = New StringBuilder()
                        Dim strTable As StringBuilder = New StringBuilder()

                        '<!--##CONTENT##-->;<!--##ROLE_NAME##-->;<!--##EMPLOYEE_NAME##-->;<!--##STATE##-->;<!--##DATE_RECEIPT##-->;<!--##ALERT_TYPE##-->;<!--##STRONG_OPEN##-->;<!--##STRONG_OPEN##-->;
                        strTable.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2))

                        SHours = 0
                        For Each seg_dtRow In Tbl_Ta_RutaSeguimiento.Rows

                            strTable_Cont.Clear()
                            strTable_Cont.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 2))


                            'strTableContent.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                            strTable_Cont.Replace("<!--##ROLE_NAME##-->", "")
                            strTable_Cont.Replace("<!--##EMPLOYEE_NAME##-->", seg_dtRow("nombre_empleado"))
                            strTable_Cont.Replace("<!--##STATE##-->", seg_dtRow("descripcion_estado"))
                            strTable_Cont.Replace("<!--##DATE_RECEIPT##-->", seg_dtRow("fecha_aprobacion"))
                            strTable_Cont.Replace("<!--##ALERT_TYPE##-->", seg_dtRow("Alerta"))

                            'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                            If ((tbl_document_rep.Rows(0).Item("id_ruta") = seg_dtRow("id_ruta")) And (tbl_document_rep.Rows(0).Item("id_estadoDoc") = seg_dtRow("id_estadoDoc"))) Then
                                strTable_Cont.Replace("<tr>", strStyle)
                                strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "<strong>")
                                strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "</strong>")
                            Else
                                strTable_Cont.Replace("<!--##STRONG_OPEN##-->", "")
                                strTable_Cont.Replace("<!--##STRONG_CLOSE##-->", "")
                            End If

                            SHours += seg_dtRow("NHRs")
                            NDays = Math.Round((seg_dtRow("NHRs") / 24), 0, MidpointRounding.AwayFromZero)

                            strTable_Cont.Replace("<!--##DAYS##-->", NDays)

                            strTableContent.Append(strTable_Cont.ToString)


                        Next

                        NDays = Int((SHours / 24))
                        NHours = Math.Round((((SHours / 24) - NDays) * 24), 0, MidpointRounding.AwayFromZero)

                        'strTableContentALL.Append(strTableContent)

                        strTable.Replace("<!--##TOTAL_DAYS##-->", String.Format(" {0} Days {1} Hrs", NDays, NHours))
                        strTable.Replace("<!--##CONTENT##-->", strTableContent.ToString)

                        strContent.Replace("<!--##TABLE_STATUS##-->", strTable.ToString)
                        strContentALL.Append(strContent.ToString)
                        strContentALL.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_nwline", 1))

                        '************************************STATUS TABLE**************************************************************************
                        'Add_emails(dtR2("id_documento"), dtR2("id_estadoDoc"), idOriginator, boolSetEmailList) 'Adding the respective email according the state of the approval proccess
                        'Add_document_email_list(dtR2("id_documento"), boolSetEmailList)
                        '*****************FINDING the Destinataries **************************************
                        'boolSetEmailList = False

                        '**************TESTING PRUPORSES
                        'Exit For
                        '**************TESTING PRUPORSES

                    Next

                    strMess.Replace("<!--##CONTENT##-->", strContentALL.ToString)


                    'Add_emails(idOriginator, "User") 'Adding the respective email according the user or the Role

                    If ObJemail.SilentMode Then
                        'If Not ObJemail.SilentMode Then

                        strSubject &= " (Silent MODE) "
                        Dim strTO As StringBuilder = New StringBuilder
                        Dim strCC As String = ""
                        Dim strBCC As String = ""

                        '*********************************************************************************************************
                        '******************we can do this getting all the selection List......************************************
                        '*********************************************************************************************************

                        ' For Each dtRow In tbl_emails.Rows

                        'Select Case dtRow("Tipo")
                        '    Case "TO"
                        '        strTO &= String.Format("{0};", dtRow("email").ToString.Trim)
                        '    Case "CC"
                        '        strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                        '    Case "BCC"
                        '        strBCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                        'End Select

                        strTO.Append(String.Format("{0};", EmailOriginator.ToString.Trim))

                        ' Next

                        ' Dim strEMAILS As String = String.Format("<br /><br /><br /><br /> TO:{0}<br />CC:{1}<br />BCC:{2} ", strTO, strCC, strBCC)
                        Dim strEMAILS As StringBuilder = New StringBuilder
                        strEMAILS.Append("<br /><br /><br /><br />TO:" & strTO.ToString & "<br /><br />")

                        strMess.Replace("<!--##MESS##-->", strEMAILS.ToString) 'Representavite emails for this  notification

                    Else


                        strMess.Replace("<!--##MESS##-->", "")
                        '*********************************************************************************************************
                        '*******************THIS IS the Individual Email for every Notification***********************************
                        ObJemail.AddTo(EmailOriginator)
                        '*******************THIS IS the Individual Email for every notification***********************************
                        '*********************************************************************************************************

                    End If


                    '*******************************************************************
                    '************************SUBJECTS***********************************
                    ObJemail.AddSubject(strSubject)
                    '************************SUBJECTS***********************************
                    '*******************************************************************

                    '************************* THE message it supposed Is here, need to proceed to test*****************
                    '*****************building the email from the templates objects**************************************

                    strMess.Replace("<!--##IMG_CID2##-->", "LogProgram") 'Name of the resource the Program Logo
                    strMess.Replace("<!--##IMG_CID1##-->", "LogChemo") 'Name of the resource the Company logo

                    Dim strPath As String
                    If strBaseDir.Length > 1 Then
                        strPath = strBaseDir
                    Else
                        strPath = Server.MapPath("~")
                    End If

                    Dim strTMP As String = strMess.ToString

                    ObJemail.setAlternativeVIEW(strMess.ToString) 'Setting alternative View

                    Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                    Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                    If Not String.IsNullOrEmpty(strProgramIMG) Then
                        ObJemail.Add_LinkResources("LogProgram", strPath + strProgramIMG, "image/png")
                    Else
                        ObJemail.Add_LinkResources("LogProgram", strPath + "\Images\Activities\accent.png", "image/png")
                    End If

                    'ObJemail.Add_LinkResources("LogChemo", strPath + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                    ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")


                    ObJemail.Add_LinkResources("EmmB_Red", strPath + "\Imagenes\iconos\Red_Time.png", "image/png")
                    ObJemail.Add_LinkResources("EmmB_Green", strPath + "\Imagenes\iconos\Blue_Time.png", "image/png")
                    ObJemail.Add_LinkResources("EmmB_Yellow", strPath + "\Imagenes\iconos\Orange_Time.png", "image/png")
                    ObJemail.Add_LinkResources("EmmB_Gray", strPath + "\Imagenes\iconos\Cyan_Time.png", "image/png")

                    ObJemail.SetPriority(MailPriority.High)

                    If ObJemail.SendEmail(id_notification) Then
                        Notify_Reminder_Proc = True
                    Else
                        Notify_Reminder_Proc = False
                    End If

                Else  '*********************Valid Email addres**********************************

                    Notify_Reminder_Proc = False

                End If

            Catch ex As Exception



            End Try


        End Function


        Public Function fn_get_EmailList(ByVal IdDoc) As String

            Dim TO_list As StringBuilder = New StringBuilder()
            Dim CC_List As StringBuilder = New StringBuilder()

            Dim Limit_ByLine As Integer
            Dim Email_CountingTO As Integer
            Dim Email_CountingCC As Integer

            Limit_ByLine = 3
            Email_CountingTO = 0
            Email_CountingCC = 0

            For Each dtRow As DataRow In tbl_EmailIncluded.Rows

                If dtRow("type_subject") = "TO" And dtRow("id_documento") = IdDoc Then

                    TO_list.Append(String.Format("{0};", dtRow("email")))

                    Email_CountingTO += 1
                    If Email_CountingTO > Limit_ByLine Then

                        Email_CountingTO = 0
                        TO_list.Append("<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")

                    End If

                ElseIf dtRow("type_subject") = "CC" And dtRow("id_documento") = IdDoc Then

                    CC_List.Append(String.Format("{0};", dtRow("email")))

                    Email_CountingCC += 1
                    If Email_CountingCC > Limit_ByLine Then
                        Email_CountingCC = 0
                        CC_List.Append("<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")

                    End If


                End If

            Next

            Dim strEmial_list As StringBuilder = New StringBuilder()

            strEmial_list.Append("<br />")
            strEmial_list.Append("<br />")
            'Dim strEMAILS As String = String.Format("<br /><br />")   ' TO:{0}<br\>CC:{1}<br\>BCC:{2} ", strTO, strCC, strBCC)

            If TO_list.Length > 1 Then
                strEmial_list.Append("<strong>TO:</strong>&nbsp;")
                strEmial_list.Append(TO_list.ToString())
                'strEMAILS &= String.Format("{0}", TO_list)
            End If

            If CC_List.Length > 1 Then
                strEmial_list.Append("<br /><br />")
                strEmial_list.Append("<strong>CC:</strong>&nbsp;")
                strEmial_list.Append(CC_List.ToString)
                'strEMAILS &= String.Format("<br /><br /><strong>CC:</strong>&nbsp;{0}", CC_List)
            End If

            fn_get_EmailList = strEmial_list.ToString


        End Function


        'Public Sub Add_emails(ByVal id_usr As Integer, ByVal strType As String) 'According the state

        '    Add_Emails_List("TO", set_vw_t_usuarios_email(id_usr)) 'Actual
        '    Add_Emails_List("CC", set_vw_ta_GrupoUsuario_emails(id_usr)) 'Actual step Group 0


        '    '**TEMP*****************If Not ObJemail.SilentMode Then
        '    If ObJemail.SilentMode Then

        '        '**************ADDING THE EMAILS FROM THE UNIQUE LIST*********************
        '        For Each dtRow In tbl_emails.Rows

        '            Select Case dtRow("Tipo")
        '                Case "TO"
        '                    ObJemail.AddTo(dtRow("email").ToString.Trim)
        '                Case "CC"
        '                    ObJemail.AddCC(dtRow("email").ToString.Trim)
        '                Case "BCC"
        '                    ObJemail.AddBcc(dtRow("email").ToString.Trim)
        '            End Select

        '        Next
        '        '**************ADDING THE EMAILS FROM THE UNIQUE LIST*********************

        '    End If

        'End Sub

        Public Sub Add_emails(ByVal ID As Integer, ByVal strType As String) 'For Roles

            Add_Emails_List("TO", set_vw_t_usuarios_email(ID, strType)) 'Actual
            Add_Emails_List("CC", set_vw_ta_GrupoUsuario_emails(ID, strType)) 'Actual step Group 0

            '**TEMP*****************
            If Not ObJemail.SilentMode Then
                'If ObJemail.SilentMode Then

                '**************ADDING THE EMAILS FROM THE UNIQUE LIST*********************
                For Each dtRow In tbl_emails.Rows

                    Select Case dtRow("Tipo")
                        Case "TO"
                            ObJemail.AddTo(dtRow("email").ToString.Trim)
                        Case "CC"
                            ObJemail.AddCC(dtRow("email").ToString.Trim)
                        Case "BCC"
                            ObJemail.AddBcc(dtRow("email").ToString.Trim)
                    End Select

                Next
                '**************ADDING THE EMAILS FROM THE UNIQUE LIST*********************

            End If

        End Sub


        Public Sub Add_emailsSH_Roles(ByVal idRolOwner As Integer) 'According the state


            Add_Emails_List("TO", set_vw_t_usuarios_roles_email(idRolOwner)) 'Actual

            '**TEMP*****************
            If Not ObJemail.SilentMode Then
                'If ObJemail.SilentMode Then

                '**************ADDING THE EMAILS FROM THE UNIQUE LIST*********************
                For Each dtRow In tbl_emails.Rows

                    Select Case dtRow("Tipo")
                        Case "TO"
                            ObJemail.AddTo(dtRow("email").ToString.Trim)
                        Case "CC"
                            ObJemail.AddCC(dtRow("email").ToString.Trim)
                        Case "BCC"
                            ObJemail.AddBcc(dtRow("email").ToString.Trim)
                    End Select

                Next
                '**************ADDING THE EMAILS FROM THE UNIQUE LIST*********************

            End If

        End Sub



        Public Sub Add_emails(ByVal vOrden As Integer, ByVal booInitialized As Boolean, ByVal boolADDIING As Boolean, ByVal id_documento As Integer) 'According the state

            If vOrden >= 0 Then 'Just a valid Order

                If booInitialized Then 'set the tbl_emails
                    Set_Emails_List() 'SE the Email List
                End If

                Add_Emails_List("TO", set_ta_roles_emails(id_documento, vOrden)) 'included all
                Add_Emails_List("CC", set_ta_GrupoRoles_emails(id_documento, vOrden)) 'included all groups

                '**************************************ADDING TO THE EMAIL***************************************
                If boolADDIING And Not ObJemail.SilentMode Then

                    Dim boolExist As Boolean = False

                    '**************ADDING THE EMAILS FROM THE UNIQUE LIST*********************
                    For Each dtRow In tbl_emails.Rows

                        Select Case dtRow("Tipo")
                            Case "TO"
                                ObJemail.AddTo(dtRow("email").ToString.Trim)
                            Case "CC"
                                ObJemail.AddCC(dtRow("email").ToString.Trim)
                            Case "BCC"
                                ObJemail.AddBcc(dtRow("email").ToString.Trim)
                        End Select

                    Next
                    '**************ADDING THE EMAILS FROM THE UNIQUE LIST*********************

                    '**************************************Independent Email list By Document ***************************************
                    set_ta_DocumentosINFO(id_documento)
                    Dim email As String() = get_ta_DocumentosINFO("email", "id_documento", id_documento).Split(";")

                    For Each dtEmail As String In email

                        If Not String.IsNullOrEmpty(dtEmail) Then
                            If dtEmail.IndexOf("@") > 1 And dtEmail.IndexOf(".") Then

                                For Each dtRow As DataRow In tbl_emails.Rows

                                    If dtEmail.Trim = dtRow("email").ToString.Trim Then
                                        boolExist = True
                                        Exit For
                                    End If

                                Next

                                If boolExist = False Then 'Add The Email
                                    ObJemail.AddCC(dtEmail.Trim)
                                Else
                                    boolExist = False
                                End If


                            End If
                        End If

                    Next
                    '**************************************Independent Email list By Document***************************************
                End If
                '**************************************ADDING TO THE EMAIL***************************************

            End If

        End Sub


        Public Sub Add_document_email_list(ByVal id_documento As Integer, Optional boolInitialized As Boolean = False)

            If boolInitialized Then 'set the tbl_emails
                Set_Emails_List() 'SE the Email List
            End If

            '**************************************Independent Email list By Document ***************************************
            set_ta_DocumentosINFO(id_documento)
            Dim email As String() = get_ta_DocumentosINFO("email", "id_documento", id_documento).Split(";")
            Dim boolExist As Boolean = False


            'Always store in the tbl_emails email list

            For Each dtEmail As String In email

                If Not String.IsNullOrEmpty(dtEmail) Then

                    'If dtEmail.IndexOf("@") > 1 And dtEmail.IndexOf(".") Then
                    If IsEmail(dtEmail) Then

                        For Each dtRow As DataRow In tbl_emails.Rows

                            If dtEmail.Trim = dtRow("email").ToString.Trim Then
                                boolExist = True
                                Exit For
                            End If

                        Next

                        If boolExist = False Then 'Add The Email
                            Add_Emails_List("CC", dtEmail) 'Actual step Group 0
                        Else
                            boolExist = False
                        End If

                    End If

                End If

            Next



        End Sub

        Public Sub Set_Emails_List()

            Sql = String.Format(" Select 'AAAA' as Tipo,  email_usuario as email from vw_t_usuarios  where id_usuario = 0")
            tbl_emails = cl_utl.setObjeto("vw_ta_roles_emails", "id_Documento", 0, Sql)

            If (tbl_emails.Rows.Count = 1 And Not String.IsNullOrEmpty(tbl_emails.Rows.Item(0).Item("email"))) Then
                tbl_emails.Rows.Remove(tbl_emails.Rows.Item(0))
            End If


        End Sub


        Public Sub Add_Emails_List(ByVal strType As String, ByVal tmp_Email As DataTable)

            Dim boolExist As Boolean = False

            For Each dtEmail As DataRow In tmp_Email.Rows

                For Each dtRow As DataRow In tbl_emails.Rows

                    If dtEmail("email").ToString.Trim = dtRow("email").ToString.Trim Then
                        boolExist = True
                        Exit For
                    End If

                Next

                If boolExist = False Then 'Add The Email
                    Dim NewRow As DataRow = tbl_emails.NewRow()
                    NewRow("Tipo") = strType
                    NewRow("email") = dtEmail("email").ToString.Trim
                    tbl_emails.Rows.Add(NewRow)
                Else
                    boolExist = False
                End If


            Next

            'Sql = String.Format(" Select 'AAAA' as Tipo,  email_usuario as email from vw_t_usuarios  where id_usuario = 0")
            'tbl_emails = cl_utl.setObjeto("vw_ta_roles_emails", "id_Documento", id_documento, Sql)


        End Sub


        Public Sub Add_Emails_List(ByVal strType As String, ByVal strEmail As String)

            Dim boolExist As Boolean = False


            For Each dtRow As DataRow In tbl_emails.Rows

                If dtRow("email").ToString.Trim = strEmail.ToString.Trim Then
                    boolExist = True
                    Exit For
                End If

            Next

            If boolExist = False Then 'Add The Email
                Dim NewRow As DataRow = tbl_emails.NewRow()
                NewRow("Tipo") = strType
                NewRow("email") = strEmail.ToString.Trim
                tbl_emails.Rows.Add(NewRow)
            Else
                boolExist = False
            End If




        End Sub


        Public Function set_ta_roles_emails(ByVal id_Documento As Integer, ByVal Orden As Integer) As DataTable

            Dim strOrden As String = " "
            If Orden > -1 Then
                strOrden = String.Format(" and orden = {0} ", Orden)
            End If

            Sql = String.Format("select * from vw_ta_roles_emails where id_documento = {0} {1} ", id_Documento, strOrden)
            Tbl_ta_roles_emails = cl_utl.setObjeto("vw_ta_AppDocumento", "id_Documento", id_Documento, Sql)


            If (Tbl_ta_roles_emails.Rows.Count = 1 And Tbl_ta_roles_emails.Rows.Item(0).Item("id_rol") = 0) Then
                Tbl_ta_roles_emails.Rows.Remove(Tbl_ta_roles_emails.Rows.Item(0))
            End If

            set_ta_roles_emails = Tbl_ta_roles_emails

        End Function

        Public Function set_vw_t_usuarios_email(ByVal ID As Integer, ByVal strType As String) As DataTable


            If strType = "User" Then
                Sql = String.Format("select a.id_usuario, a.nombre_usuario, a.email_usuario	as email
                                    from vw_t_usuarios a
	                            where id_programa = {0} and id_usuario = {1}", id_programa, ID)
            Else 'Roles
                Sql = String.Format("select a.*, b.id_usuario, c.nombre_usuario, c.email_usuario as email
                                      from ta_roles a
		                            inner join ta_role_user b on (a.id_rol = b.id_rol)
		                            inner join vw_t_usuarios c on (b.id_usuario = c.id_usuario and a.id_programa = c.id_programa)
		                             where a.id_type_role = 1
		                             and a.id_programa = {0} and a.id_rol = {1}", id_programa, ID)
            End If


            Tbl_ta_roles_emails = cl_utl.setObjeto("vw_t_usuarios", "id_usuario", 0, Sql)


            If (Tbl_ta_roles_emails.Rows.Count = 1 And Tbl_ta_roles_emails.Rows.Item(0).Item("id_usuario") = 0) Then
                Tbl_ta_roles_emails.Rows.Remove(Tbl_ta_roles_emails.Rows.Item(0))
            End If

            set_vw_t_usuarios_email = Tbl_ta_roles_emails

        End Function


        Public Function set_vw_t_usuarios_roles_email(ByVal idRolOwner As Integer) As DataTable

            Sql = String.Format("select a.*, b.id_usuario, c.nombre_usuario, c.email_usuario as email
			                        from ta_roles a
		                         inner join ta_role_user b on (a.id_rol = b.id_rol)
		                           inner join vw_t_usuarios c on (b.id_usuario = c.id_usuario and a.id_programa = c.id_programa)
		                            where a.id_type_role = 2
		                          and a.id_programa = {0} and a.id_rol = {1} ", id_programa, idRolOwner)

            Tbl_ta_roles_emails = cl_utl.setObjeto("vw_t_usuarios", "id_usuario", 0, Sql)


            If (Tbl_ta_roles_emails.Rows.Count = 1 And Tbl_ta_roles_emails.Rows.Item(0).Item("id_usuario") = 0) Then
                Tbl_ta_roles_emails.Rows.Remove(Tbl_ta_roles_emails.Rows.Item(0))
            End If

            set_vw_t_usuarios_roles_email = Tbl_ta_roles_emails

        End Function


        Public Function set_vw_ta_GrupoUsuario_emails(ByVal ID As Integer, ByVal strType As String) As DataTable


            If strType = "User" Then

                Sql = String.Format("select a.id_grupo, a.id_rol, tab1.id_usuario as id_usuarioOwner, a.id_usuario, b.nombre_usuario, b.email_usuario as email
	                                   from ta_gruposRoles a
	                                    inner join 
		                                   (select a.*, b.id_usuario
			                                   from ta_roles a
			                                 inner join ta_role_user b on (a.id_rol = b.id_rol)
			                                where a.id_type_role = 1
			                                  and a.id_programa = {0}
			                                  and b.id_usuario = {1}) as tab1 on ( a.id_rol = tab1.id_rol)
                                           inner join vw_t_usuarios b on (a.id_usuario = b.id_usuario and b.id_programa = tab1.id_programa )", id_programa, ID)


            Else

                Sql = String.Format("select a.id_grupo, a.id_rol, tab1.id_usuario as id_usuarioOwner, a.id_usuario, b.nombre_usuario, b.email_usuario as email
	                               from ta_gruposRoles a
	                                inner join 
		                               (select a.*, b.id_usuario
			                               from ta_roles a
			                             inner join ta_role_user b on (a.id_rol = b.id_rol)
			                            where a.id_type_role = 1
			                              and a.id_programa = {0}
			                              and a.id_rol = {1}) as tab1 on ( a.id_rol = tab1.id_rol)
                                       inner join vw_t_usuarios b on (a.id_usuario = b.id_usuario and b.id_programa = tab1.id_programa )", id_programa, ID)


            End If

            Tbl_ta_GrupoRoles_emails = cl_utl.setObjeto("vw_ta_roles_emails", "id_usuario", 0, Sql)

            If (Tbl_ta_GrupoRoles_emails.Rows.Count = 1 And Tbl_ta_GrupoRoles_emails.Rows.Item(0).Item("id_rol") = 0) Then
                Tbl_ta_GrupoRoles_emails.Rows.Remove(Tbl_ta_GrupoRoles_emails.Rows.Item(0))
            End If

            set_vw_ta_GrupoUsuario_emails = Tbl_ta_GrupoRoles_emails

        End Function


        Public Function set_ta_GrupoRoles_emails(ByVal id_documento As Integer, ByVal Orden As Integer) As DataTable

            Dim strOrden As String = " "
            If Orden > -1 Then
                strOrden = String.Format(" and orden = {0} ", Orden)
            End If

            Sql = String.Format("select * from vw_ta_email_gruposRoles where id_documento = {0} {1} ", id_documento, strOrden)
            Tbl_ta_GrupoRoles_emails = cl_utl.setObjeto("vw_ta_roles_emails", "id_Documento", id_documento, Sql)


            If (Tbl_ta_GrupoRoles_emails.Rows.Count = 1 And Tbl_ta_GrupoRoles_emails.Rows.Item(0).Item("id_rol") = 0) Then
                Tbl_ta_GrupoRoles_emails.Rows.Remove(Tbl_ta_GrupoRoles_emails.Rows.Item(0))
            End If

            set_ta_GrupoRoles_emails = Tbl_ta_GrupoRoles_emails

        End Function

        Public Function set_Ta_RutaSeguimiento(ByVal id_documento As Integer) As DataTable

            Sql = String.Format(" SELECT * FROM dbo.FN_Ta_RutaSeguimiento({0}) order by id_App_Documento ", id_documento)

            Tbl_Ta_RutaSeguimiento = cl_utl.setObjeto("FN_Ta_RutaSeguimiento", "id_documento", id_documento, Sql)
            set_Ta_RutaSeguimiento = Tbl_Ta_RutaSeguimiento

        End Function


        Public Function set_ta_DocumentosINFO(ByVal id_Documento As Integer) As DataTable

            Sql = String.Format("Select * FROM vw_ta_documentos WHERE id_documento = {0} ", id_Documento)

            tbl_ta_DocumentosINFO = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_Documento, Sql)

            set_ta_DocumentosINFO = tbl_ta_DocumentosINFO

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (set_ta_DocumentosINFO.Rows.Count = 1 And set_ta_DocumentosINFO.Rows.Item(0).Item("id_documento") = 0) Then
                set_ta_DocumentosINFO.Rows.Remove(set_ta_DocumentosINFO.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function


        Public Function get_ta_DocumentosINFO(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(tbl_ta_DocumentosINFO, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function



        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************

        Public Function Emailing_PENDING_NOTIFICATION(Optional id_AppDoc As Integer = 0) As Boolean



        End Function


        Public Function set_Apps_Document(ByVal idDoc As Integer) As DataTable

            Sql = String.Format("SELECT * FROM vw_ta_AppDocumento WHERE id_Documento= {0} ", idDoc)
            Tbl_ta_AppDoumento = cl_utl.setObjeto("vw_ta_AppDocumento", "id_Documento", idDoc, Sql)
            set_Apps_Document = Tbl_ta_AppDoumento

        End Function

        Public Function get_Apps_DocumentField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(Tbl_ta_AppDoumento, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function


        '********************************************************* t_template ENTITY ************************************************************************************
        '********************************************************* t_template  ENTITY ************************************************************************************

        Public Function set_t_template(ByVal id_templ As Integer) As DataTable

            id_template = IIf(id_templ > 0, id_templ, 0)
            Tbl_t_template = cl_utl.setObjeto("t_template", "id_template", id_template)
            set_t_template = Tbl_t_template

        End Function

        Public Function get_t_templateFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(Tbl_t_template, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_t_templateFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            Tbl_t_template = cl_utl.setDTval(Tbl_t_template, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_t_template() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("t_template", Tbl_t_template, "id_template", id_template)

            If RES <> -1 Then
                set_t_templateFIELDS("id_template", RES, "id_template", id_template)
                id_template = RES
                save_t_template = RES
            Else
                save_t_template = RES
            End If

        End Function

        Public Function del_t_template(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM t_template WHERE (id_template = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_t_template = True

                Catch ex As Exception
                    del_t_template = False
                End Try

            Else

                del_t_template = False

            End If

        End Function



        Public Function get_t_templateField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(Tbl_t_template, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function

        Public Function set_t_Template() As DataTable

            Tbl_t_template = cl_utl.setObjeto("t_template", "id_notification", id_notification).Copy

            set_t_Template = Tbl_t_template

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (set_t_Template.Rows.Count = 1 And set_t_Template.Rows.Item(0).Item("id_template") = 0) Then
                set_t_Template.Rows.Remove(set_t_Template.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        '********************************************************* t_template  ENTITY ************************************************************************************
        '********************************************************* t_template  ENTITY ************************************************************************************


        '********************************************************* t_notification ENTITY ************************************************************************************
        '********************************************************* t_notification  ENTITY ************************************************************************************

        Public Function set_t_notification(ByVal id_noti As Integer) As DataTable

            id_notification = IIf(id_noti > 0, id_noti, 0)
            tbl_t_notification = cl_utl.setObjeto("t_notification", "id_notification", id_notification)
            set_t_notification = tbl_t_notification

        End Function

        Public Function get_t_notificationFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_t_notification, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_t_notificationFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_t_notification = cl_utl.setDTval(tbl_t_notification, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_t_notification() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("t_notification", tbl_t_notification, "id_notification", id_notification)

            If RES <> -1 Then
                set_t_notificationFIELDS("id_notification", RES, "id_notification", id_notification)
                id_notification = RES
                save_t_notification = RES
            Else
                save_t_notification = RES
            End If

        End Function

        Public Function del_t_notification(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM t_notification WHERE (id_notification = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_t_notification = True

                Catch ex As Exception
                    del_t_notification = False
                End Try

            Else

                del_t_notification = False

            End If

        End Function


        '********************************************************* t_notification  ENTITY ************************************************************************************
        '********************************************************* t_notification  ENTITY ************************************************************************************

        Function IsEmail(ByVal email As String) As Boolean
            Static emailExpression As System.Text.RegularExpressions.Regex = New Text.RegularExpressions.Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")

            Return emailExpression.IsMatch(email)
        End Function

        Public Function get_notification_Pending_ByUser(ByVal id_user As Integer, ByVal subject_TP As String, ByVal email As String, ByVal idDoc As Integer) As DataTable

            Dim tblResult As New DataTable()

            Using dbEntities As New dbRMS_JIEntities

                Dim list_result = dbEntities.Pending_Approval_User(id_user, subject_TP, email, idDoc).ToList()

                If list_result.Count > 0 Then


                    tblResult = cl_utl.ConvertToDataTable(list_result)

                End If

            End Using


            get_notification_Pending_ByUser = tblResult

        End Function



        Public Function get_notification_Pending_Summary() As DataTable

            Dim tblResult As New DataTable()

            Using dbEntities As New dbRMS_JIEntities

                Dim list_result = dbEntities.Pending_Approval_Summary().ToList()

                If list_result.Count > 0 Then

                    '    Dim fields() = list_result.First.GetType.GetProperties
                    '    For Each field In fields
                    '        tblResult.Columns.Add(field.Name, field.PropertyType)
                    '    Next
                    '    For Each item In list_result
                    '        Dim row As DataRow = tblResult.NewRow
                    '        For Each field In fields
                    '            Dim p = item.GetType.GetProperty(field.Name)
                    '            row(field.Name) = p.GetValue(item, Nothing)
                    '        Next
                    '        tblResult.Rows.Add(row)
                    '    Next

                    tblResult = cl_utl.ConvertToDataTable(list_result)

                End If



            End Using


            get_notification_Pending_Summary = tblResult

        End Function


        Public Function Get_Evaluation_STATUS(ByVal Sorder As Integer, ByVal idTYPE As Integer, Optional STpostivie As Boolean = True) As Integer

            Using dbEntities As New dbRMS_JIEntities

                Dim oTA_EVALUATION_APP_STATUS = dbEntities.TA_EVALUATION_APP_STATUS.Where(Function(p) p.ID_VOTING_TYPE = idTYPE And p.STATUS_ORDER = Sorder).ToList()

                If oTA_EVALUATION_APP_STATUS.Count > 1 Then

                    If oTA_EVALUATION_APP_STATUS.Where(Function(p) p.STATUS_POSITIVE = STpostivie).Count() > 0 Then

                        Get_Evaluation_STATUS = oTA_EVALUATION_APP_STATUS.Where(Function(p) p.STATUS_POSITIVE = STpostivie).FirstOrDefault.ID_EVALUATION_APP_STATUS

                    Else

                        Get_Evaluation_STATUS = 0

                    End If

                Else

                    If oTA_EVALUATION_APP_STATUS.Count = 1 Then
                        Get_Evaluation_STATUS = oTA_EVALUATION_APP_STATUS.FirstOrDefault.ID_EVALUATION_APP_STATUS
                    Else
                        Get_Evaluation_STATUS = 0
                    End If

                End If

            End Using

        End Function



    End Class

End Namespace