
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Net.Mail
Imports System.Text
Imports ly_RMS
Imports ly_SIME

Namespace APPROVAL

    Public Class cls_notification
        Inherits System.Web.UI.Page

        Dim Sql As String
        Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

        Dim cl_utl As New CORE.cls_util
        Dim dateUtil As APPROVAL.cls_dUtil

        Dim Tbl_EquipoCentral As DataTable
        Dim Tbl_EquipoRegional As DataTable
        Dim Tbl_EquipoOrganizacion As DataTable
        Dim tbl_t_programa As DataTable
        Dim tbl_t_entregable As DataTable
        Dim tbl_t_entregableHito As DataTable
        Dim tbl_t_ruta_entregable As DataTable
        Dim tbl_t_comentarios_entregable As DataTable
        Dim tbl_t_comentarios_entregableHito As DataTable

        Dim tbl_t_notification As DataTable
        Dim Tbl_t_template As DataTable
        Dim Tbl_ta_AppDoumento As DataTable
        Dim Tbl_Document As DataTable
        Dim Tbl_Ta_RutaSeguimiento As DataTable
        Dim Tbl_ta_GrupoRoles_emails As DataTable
        Dim Tbl_ta_roles_emails As DataTable
        Dim Tbl_ta_viajes_email As DataTable
        Dim Tbl_Ta_ViajeItinerario As DataTable
        Dim Tbl_Ta_ViajeHotel As DataTable

        Dim Tbl_Ta_parDetalle As DataTable

        Dim Tbl_Ta_anticipoDetalle_compras As DataTable
        Dim Tbl_Ta_anticipoDetalle_eventos As DataTable

        Dim Tbl_Ta_ViajePasaje As DataTable
        Dim Tbl_Ta_ViajeReuniones As DataTable
        Dim Tbl_Ta_ViajeMiscelaneos As DataTable
        Dim Tbl_Ta_ViajeAlonamiento As DataTable

        Dim tbl_ta_DocumentosINFO As DataTable
        Dim tbl_emails As DataTable
        Dim Tbl_ta_comentariosDoc As DataTable
        Dim tbl_t_usuarios As DataTable

        Dim Tbl_ta_viaje As DataTable

        Dim Tbl_ta_par As DataTable
        Dim Tbl_ta_anticipo As DataTable

        Dim tbl_t_entregables_hito As DataTable
        Dim Tbl_ta_par_email As DataTable

        Dim id_programa As Integer
        Dim id_notification As Integer
        Dim id_template As Integer
        Dim id_documento As Integer
        Dim id_documento_ambiental As Integer
        Dim id_AppDocumento As Integer
        Dim id_comment As Integer
        Dim userCulture As CultureInfo
        Dim ObJemail As UTILS.cls_email

        Dim id_usuario As Integer
        Dim id_ficha_entHito As Integer
        Const cAPP_sys As Integer = 2
        Dim id_ficha_ent As Integer
        Public Property id_proyecto As Integer
        Public Property timezoneUTC As Integer

        Const cPENDING = 1
        Const cAPPROVED = 2
        Const cnotAPPROVED = 3
        Const cCANCELLED = 4
        Const cOPEN = 5
        Const cSTANDby = 6
        Const cCOMPLETED = 7

        Public Sub New(ByVal id_prog As Integer, ByVal id_doc As Integer, ByVal id_noti As Integer, ByVal userC As CultureInfo, Optional ByVal id_AppDoc As Integer = 0)

            id_programa = id_prog
            id_notification = IIf(id_noti > 0, id_noti, id_notification)
            id_documento = id_doc
            id_AppDocumento = IIf(id_AppDoc > 0, id_AppDoc, id_AppDocumento)
            id_comment = 0
            userCulture = userC

        End Sub



        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, COMMENT STEP**************************************************
        '*****************************************************************************************************************

        Public Function Emailing_COMMENT_APPROVAL(ByVal strComment As String, Optional id_AppDoc As Integer = 0, Optional ByVal idComm As Integer = 0, Optional vAuth As Boolean = True) As Boolean

            Dim strMess As String
            id_AppDocumento = IIf(id_AppDoc = 0, id_AppDocumento, id_AppDoc)
            id_comment = IIf(idComm > 0, idComm, id_comment)

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
            set_Apps_Document() 'Set all AppDoc
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            '*****************ADDING THE SUBJECTS**********************

            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->

            'Change this part temporaly
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)
            set_Comment(id_comment)

            'Dim strCommentUser As String = String.Format("{0} ({1})", get_CommentField("empleado", "id_comment", id_comment), get_CommentField("nombre_rol", "id_comment", id_comment))

            strSubject = strSubject.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strSubject = strSubject.Replace("<!--##ROL_NAME##-->", get_CommentField("nombre_rol", "id_comment", id_comment))
            strSubject = strSubject.Replace("<!--##USER_NAME##-->", get_CommentField("empleado", "id_comment", id_comment))

            If ObJemail.SilentMode Then
                strSubject &= " (Silent MODE) "
            End If

            ObJemail.AddSubject(strSubject)

            '*****************FINDING the destinataries **************************************
            Dim Order As Integer = get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)

            '*****Adding the respective email according the APP Order*************
            Add_emails(0, True, False) 'Set the email List and Adding
            Add_emails(Order - 1, False, False) 'Adding to the email List
            Add_emails(Order, False, False) 'Adding to the email List
            Add_emails(Order + 1, False, True) 'Adding to the email List and Add to the email
            '*****Adding the respective email according the APP Order*************

            '*****Adding the respective email according the APP Order*************
            'ObJemail.AddBcc("grivera@colombiaresponde-ns.org") 'Just for testing
            'ObJemail.AddCC("rbyarugaba@ftfcpm.com") 'Just for testing
            '*****************FINDING the destinataries **************************************

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



            '**************At this part we neeed to start to replace the Documetn info for this email*******************

            '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_Apps_DocumentField("nombre_programa", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DESCRIPTION_CAT##-->", get_Apps_DocumentField("descripcion_cat", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DESCRIPTION_APPROVAL##-->", get_Apps_DocumentField("descripcion_aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##BENEFICIARY_NAME##-->", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DOCUMENT_DESCRIPTION##-->", get_Apps_DocumentField("descripcion_doc", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'strMess = strMess.Replace("<!--##CODE_PROCESS##-->", get_Apps_DocumentField("codigo_SAP_APP", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            strMess = strMess.Replace("<!--##APPROVAL_CODE##-->", get_Apps_DocumentField("codigo_Approval", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##USER_NAME##-->", get_Apps_DocumentField("nombre_usuario_app", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##NEXT_FASE##-->", get_Apps_DocumentField("nextFase", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento)), "f")
            strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", vFecha)
            'dateUtil.set_DateFormat(CDate( ), "f")
            'strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            strMess = strMess.Replace("<!--##ID_DOC##-->", id_documento)
            strMess = strMess.Replace("<!--##COMMENTS##-->", strComment.Replace("''", "'"))

            strMess = strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

            '**************At this part we neeed to start to replace the Documetn info for this email*******************

            '******************************FOLLOWING TABLE**********************************************
            Dim strTablePath As String = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1) 'getting the second Element to fill
            Dim strALL_Rows As String = ""
            Dim strRow As String = ""

            Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>

            set_Ta_RutaSeguimiento() 'getting the complete route

            For Each dtRow In Tbl_Ta_RutaSeguimiento.Rows

                strRow = get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 1)

                strRow = strRow.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                strRow = strRow.Replace("<!--##EMPLOYEE_NAME##-->", dtRow("nombre_empleado"))
                strRow = strRow.Replace("<!--##STATE##-->", dtRow("descripcion_estado"))
                strRow = strRow.Replace("<!--##DATE_RECEIPT##-->", dtRow("fecha_aprobacion"))
                strRow = strRow.Replace("<!--##ALERT_TYPE##-->", dtRow("Alerta"))

                'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                If id_AppDocumento = dtRow("id_app_documento") Then
                    strRow = strRow.Replace("<tr>", strStyle)
                End If

                strALL_Rows &= strRow

            Next

            strTablePath = strTablePath.Replace("<!--##CONTENT##-->", strALL_Rows)

            strMess = strMess.Replace("<!--##STEP_TABLE##-->", strTablePath) 'this would be by template

            '******************************FOLLOWING TABLE**********************************************

            '***********************************adding the resources an sending************************************************

            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View


            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
            End If

            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.SetPriority(MailPriority.Normal)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_COMMENT_APPROVAL = True
            Else
                Emailing_COMMENT_APPROVAL = False
            End If

            '***********************************adding the resources an sending************************************************

        End Function

        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, COMMENT STEP**************************************************
        '*****************************************************************************************************************


        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************






        Public Function Emailing_APPROVAL_STEP(Optional id_AppDoc As Integer = 0) As Boolean

            Dim strMess As String
            id_AppDocumento = IIf(id_AppDoc = 0, id_AppDocumento, id_AppDoc)

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
            set_Apps_Document() 'Set all AppDoc
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            '*****************ADDING THE SUBJECTS**********************


            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->

            'Change this part temporaly
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

            'strSubject = strSubject.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", get_Apps_DocumentField("descripcion_estado", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            ''ObJemail.AddSubject(strSubject) Move to Email find

            '*****************FINDING the destinataries **************************************
            'ObJemail.AddTo("grivera@colombiaresponde-ns.org") 'Just for testing
            Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            'ObJemail.AddBcc("grivera@colombiaresponde-ns.org") 'Just for testing
            '*****************FINDING the destinataries **************************************

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




            '**************At this part we neeed to start to replace the Documetn info for this email*******************

            '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_Apps_DocumentField("nombre_programa", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DESCRIPTION_CAT##-->", get_Apps_DocumentField("descripcion_cat", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DESCRIPTION_APPROVAL##-->", get_Apps_DocumentField("descripcion_aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##BENEFICIARY_NAME##-->", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DOCUMENT_DESCRIPTION##-->", get_Apps_DocumentField("descripcion_doc", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'strMess = strMess.Replace("<!--##CODE_PROCESS##-->", get_Apps_DocumentField("codigo_SAP_APP", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            strMess = strMess.Replace("<!--##APPROVAL_CODE##-->", get_Apps_DocumentField("codigo_Approval", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##USER_NAME##-->", get_Apps_DocumentField("nombre_usuario_app", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##NEXT_FASE##-->", get_Apps_DocumentField("nextFase", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento)), "f")
            'strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'dateUtil.set_DateFormat(CDate( ), "f")
            strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", vFecha)

            strMess = strMess.Replace("<!--##ID_DOC##-->", id_documento)
            strMess = strMess.Replace("<!--##COMMENTS##-->", get_Apps_DocumentField("observacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            strMess = strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

            Dim strTablePath As String = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1) 'getting the second Element to fill
            Dim strALL_Rows As String = ""
            Dim strRow As String = ""

            Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>

            set_Ta_RutaSeguimiento() 'getting the complete route

            For Each dtRow In Tbl_Ta_RutaSeguimiento.Rows

                strRow = get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 1)

                strRow = strRow.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                strRow = strRow.Replace("<!--##EMPLOYEE_NAME##-->", dtRow("nombre_empleado"))
                strRow = strRow.Replace("<!--##STATE##-->", dtRow("descripcion_estado"))
                strRow = strRow.Replace("<!--##DATE_RECEIPT##-->", dtRow("fecha_aprobacion"))
                strRow = strRow.Replace("<!--##ALERT_TYPE##-->", dtRow("Alerta"))

                'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                If id_AppDocumento = dtRow("id_app_documento") Then
                    strRow = strRow.Replace("<tr>", strStyle)
                End If

                strALL_Rows &= strRow

            Next

            strTablePath = strTablePath.Replace("<!--##CONTENT##-->", strALL_Rows)

            strMess = strMess.Replace("<!--##STEP_TABLE##-->", strTablePath) 'this would be by template
            '**************At this part we neeed to start to replace the Document info for this email*******************
            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            '********************Find the LogProgram if doesn´t exist nto include**************************

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
            End If

            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            '********************Find the LogProgram if doesn´t exist nto include**************************

            ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_APPROVAL_STEP = True
            Else
                Emailing_APPROVAL_STEP = False
            End If

        End Function



        Public Function Emailing_PRODUCTS_ACTIONS(ByVal id_entregable As Integer, ByVal asunto As String, ByVal email_to As String, ByVal email_to2 As String) As Boolean
            Try


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
                set_Apps_Document() 'Set all AppDoc

                strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

                '*****************ADDING THE SUBJECTS**********************

                Add_emailsContratos(email_to, email_to2, asunto) 'Adding the respective email according the state of the approval proccess

                set_t_programa()
                set_t_entregable(id_entregable)
                set_T_RutaEntregable(id_entregable)
                set_T_ComentariosEntregable(id_entregable)


                Dim strSubject As String = asunto

                'ObJemail.AddSubject(strSubject) 'Subject


                '**************At this part we neeed to start to replace the Documetn info for this email*******************

                '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
                strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_t_programaFIELDS("nombre_programa", "id_programa", id_programa))



                strMess = strMess.Replace("<!--##CONTRATISTA##-->", get_t_productsFIELDS("usuario_responsable", "id_contrato_entregable", id_entregable))
                strMess = strMess.Replace("<!--##NUMERO_CONTRATO##-->", get_t_productsFIELDS("numero_contrato", "id_contrato_entregable", id_entregable))
                strMess = strMess.Replace("<!--##FECHA_INICIO##-->", get_t_productsFIELDS("fecha_inicio", "id_contrato_entregable", id_entregable))
                strMess = strMess.Replace("<!--##FECHA_FINALIZACION##-->", get_t_productsFIELDS("fecha_finalizacion", "id_contrato_entregable", id_entregable))


                strMess = strMess.Replace("<!--##FECHA_SUPERVISOR##-->", get_t_productsFIELDS("usuario_supervisor", "id_contrato_entregable", id_entregable))
                strMess = strMess.Replace("<!--##NUMERO##-->", get_t_productsFIELDS("numero_entregable", "id_contrato_entregable", id_entregable))
                strMess = strMess.Replace("<!--##ENTREGABLE##-->", get_t_productsFIELDS("nombre_entregable", "id_contrato_entregable", id_entregable))
                strMess = strMess.Replace("<!--##FECHA_ESPERADA_ENTREGA##-->", get_t_productsFIELDS("fecha_esperada_entrega", "id_contrato_entregable", id_entregable))
                strMess = strMess.Replace("<!--##PRODUCTOS##-->", get_t_productsFIELDS("productos", "id_contrato_entregable", id_entregable))
                strMess = strMess.Replace("<!--##VALOR##-->", get_t_productsFIELDS("valor_entregable", "id_contrato_entregable", id_entregable))
                strMess = strMess.Replace("<!--##URLLINK##-->", "<a href ='https://sime.justiciainclusiva.org/frm_login?idsgtoHito=" & id_entregable & "' style=' text-decoration:none; font-family:Verdana, Arial;'> Ver en el SIME <a />")


                strMess = strMess.Replace("<!--##ALERTAPRO##-->", get_t_productsFIELDS("texto_alerta", "id_contrato_entregable", id_entregable))
                Dim strTableContent As String = ""
                Dim strTableContentALL As String = ""



                For Each dtRow In tbl_t_ruta_entregable.Rows
                    strTableContent = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1)
                    strTableContent = strTableContent.Replace("<!--##RESPONSABLE##-->", dtRow("responsable"))
                    strTableContent = strTableContent.Replace("<!--##USURESP##-->", dtRow("usuario"))
                    strTableContent = strTableContent.Replace("<!--##FECHAACC##-->", dtRow("fecha_envio"))
                    strTableContent = strTableContent.Replace("<!--##ESTADOPR##-->", dtRow("estado"))
                    strTableContentALL &= strTableContent
                Next
                strMess = strMess.Replace("<!--##RUTAAPR##-->", strTableContentALL)
                strTableContent = ""
                strTableContentALL = ""
                For Each dtRow In tbl_t_comentarios_entregable.Rows
                    strTableContent = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2)
                    strTableContent = strTableContent.Replace("<!--##FECHACOM##-->", dtRow("fecha_envio_text"))
                    strTableContent = strTableContent.Replace("<!--##USUARIOCOM##-->", dtRow("usuario"))
                    strTableContent = strTableContent.Replace("<!--##COMENTARIO##-->", dtRow("comentarios"))
                    strTableContentALL &= strTableContent
                Next

                strMess = strMess.Replace("<!--##COMENTAPRO##-->", strTableContentALL)


                'strMess = strMess.Replace("<!--##STEP_TABLE##-->", strTablePath) 'this would be by template
                '**************At this part we neeed to start to replace the Document info for this email*******************
                strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
                strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
                '************************* THE message it supposed Is here, need to proceed to test*****************
                '*****************building the email from the templates objects**************************************

                ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

                '********************Find the LogProgram if doesn´t exist nto include**************************

                Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                If Not String.IsNullOrEmpty(strProgramIMG) Then
                    ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
                Else
                    ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
                End If

                ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                '********************Find the LogProgram if doesn´t exist nto include**************************

                ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
                ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
                ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
                ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

                ObJemail.SetPriority(MailPriority.High)

                If ObJemail.SendEmail(id_notification) Then
                    Emailing_PRODUCTS_ACTIONS = True
                Else
                    Emailing_PRODUCTS_ACTIONS = False
                End If
            Catch ex As Exception
                Return False
            End Try


        End Function

        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************

        Public Function Emailing_APPROVAL_Anticipo(Optional id_AppDoc As Integer = 0, Optional id_anticipo As Integer = 0) As Boolean

            Dim strMess As String
            id_AppDocumento = IIf(id_AppDoc = 0, id_AppDocumento, id_AppDoc)

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
            set_Apps_Document() 'Set all AppDoc

            set_Apps_anticipo(id_anticipo)
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            '*****************ADDING THE SUBJECTS**********************


            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->

            'Change this part temporaly
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)
            'strSubject = strSubject.Replace("<!--##COD_VIAJE##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", get_Apps_DocumentField("descripcion_estado", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'ObJemail.AddSubject(strSubject) Move to Email find

            '*****************FINDING the destinataries **************************************
            'ObJemail.AddTo("grivera@colombiaresponde-ns.org") 'Just for testing

            Dim idTool As Integer = get_Apps_DocumentField("id_approval_tool", "id_documento", id_documento, "id_App_documento", id_AppDocumento)

            If idTool <= 1 Then
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 2 Then 'TimeSheet or Tool
                Add_emailsTS(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), idTool, strSubject) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 5 Then 'Travel or Tool
                Add_emailsPAR(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), idTool, 1) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 6 Then
                Add_emailsAnticipos(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_anticipo, 1) 'Adding the respective email according the state of the approval proccess
            Else
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            End If



            'Add_emailsPAR(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_par, 1) 'Adding the respective email according the state of the approval proccess
            'ObJemail.AddBcc("grivera@colombiaresponde-ns.org") 'Just for testing
            '*****************FINDING the destinataries **************************************

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




            '**************At this part we neeed to start to replace the Documetn info for this email*******************


            '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_Apps_DocumentField("nombre_programa", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##USUARIO_SOLICITA##-->", get_Apps_AnticipoField("usuario_solicita", "id_anticipo", id_anticipo))
            strMess = strMess.Replace("<!--##NRO_DOC_UDUARIO##-->", get_Apps_AnticipoField("numero_documento", "id_anticipo", id_anticipo))
            strMess = strMess.Replace("<!--##CARGO_USUARIO##-->", get_Apps_AnticipoField("cargo_usuario", "id_anticipo", id_anticipo))
            strMess = strMess.Replace("<!--##REGIONAL##-->", get_Apps_AnticipoField("sub_region", "id_anticipo", id_anticipo))
            strMess = strMess.Replace("<!--##TIPO_ANTICIPO##-->", get_Apps_AnticipoField("tipo_par", "id_anticipo", id_anticipo))
            strMess = strMess.Replace("<!--##CODIGO_ANTICIPO##-->", get_Apps_AnticipoField("codigo_anticipo", "id_anticipo", id_anticipo))
            strMess = strMess.Replace("<!--##CODIGO_PAR##-->", get_Apps_AnticipoField("codigo_par", "id_anticipo", id_anticipo))
            strMess = strMess.Replace("<!--##COSTO_GIRO##-->", get_Apps_AnticipoField("costo_total_giro", "id_anticipo", id_anticipo))
            strMess = strMess.Replace("<!--##MOTIVO_ANTICIPO##-->", get_Apps_AnticipoField("motivo", "id_anticipo", id_anticipo))
            'Dim xxx = get_Apps_TravelField("fecha_crea", "id_viaje", id_par)
            'Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_AnticipoField("fecha_solicitud", "id_anticipo", id_anticipo)), "f")

            'strMess = strMess.Replace("<!--##FECHA_SOLICITA##-->", get_Apps_TravelField("fecha_crea", "id_viaje", id_par))


            strMess = strMess.Replace("<!--##FECHA_ENVIO##-->", get_Apps_AnticipoField("fecha_crea", "id_anticipo", id_anticipo))


            'Dim vFecha2 As String = dateUtil.set_DateFormat(CDate(get_Apps_ParField("fecha_requiere_servicio", "id_par", id_par)), "f")
            strMess = strMess.Replace("<!--##FECHA_ANTICIPO##-->", get_Apps_AnticipoField("fecha_anticipo", "id_anticipo", id_anticipo))


            strMess = strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

            'Dim strTablePath As String = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1) 'getting the second Element to fill
            Dim strALL_Rows As String = ""
            Dim strRow As String = ""

            Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>

            Dim idTipoPar = Convert.ToInt32(get_Apps_AnticipoField("id_tipo_par", "id_anticipo", id_anticipo))

            Dim strALL_Rows_anticipo_compras As String = ""
            Dim strRow_compras As String = ""
            set_Ta_detalle_anticipo_compras(id_anticipo)
            set_Ta_detalle_anticipo_eventos(id_anticipo)
            If idTipoPar = 1 Then
                Dim templateCompras = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1)
                strMess = strMess.Replace("<!--##CONTENT_ANT_TIPO_COMPRAS##-->", templateCompras)

                For Each dtRow In Tbl_Ta_anticipoDetalle_compras.Rows

                    strRow_compras = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 3)

                    strRow_compras = strRow_compras.Replace("<!--##CANTIDAD##-->", dtRow("cantidad"))
                    strRow_compras = strRow_compras.Replace("<!--##DESCRIPCION##-->", dtRow("descripcion"))
                    strRow_compras = strRow_compras.Replace("<!--##VALOR_UNITARIO##-->", dtRow("precio_unitario"))
                    strRow_compras = strRow_compras.Replace("<!--##VALOR_TOTAL##-->", dtRow("valor_total"))

                    strALL_Rows_anticipo_compras &= strRow_compras

                Next


            ElseIf idTipoPar = 2 Then
                Dim templateCompras = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2)
                strMess = strMess.Replace("<!--##CONTENT_ANT_TIPO_EVENTOS##-->", templateCompras)


            End If

            set_Ta_RutaSeguimiento() 'getting the complete route






            Dim strALL_Rows_hotel As String = ""
            Dim strRow_hotel As String = ""

            'For Each dtRow In Tbl_Ta_ViajeHotel.Rows

            '    strRow_hotel = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2)

            '    strRow_hotel = strRow_hotel.Replace("<!--##FECHA_LLEGADA##-->", dtRow("fecha_llegada"))
            '    strRow_hotel = strRow_hotel.Replace("<!--##FECHA_SALIDA##-->", dtRow("fecha_salida"))
            '    strRow_hotel = strRow_hotel.Replace("<!--##CIUDAD##-->", dtRow("ciudad"))
            '    strRow_hotel = strRow_hotel.Replace("<!--##HOTEL##-->", dtRow("hotel"))

            '    ''If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
            '    'If id_AppDocumento = dtRow("id_app_documento") Then
            '    '    strRow = strRow.Replace("<tr>", strStyle)
            '    'End If

            '    strALL_Rows_hotel &= strRow_hotel

            'Next



            'For Each dtRow In Tbl_Ta_RutaSeguimiento.Rows

            '    strRow = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 5)

            '    strRow = strRow.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
            '    strRow = strRow.Replace("<!--##EMPLOYEE_NAME##-->", dtRow("nombre_empleado"))
            '    strRow = strRow.Replace("<!--##STATE##-->", dtRow("descripcion_estado"))
            '    strRow = strRow.Replace("<!--##DATE_RECEIPT##-->", dtRow("fecha_aprobacion"))
            '    strALL_Rows &= strRow

            'Next

            strMess = strMess.Replace("<!--##CONTENT_ANT_COMPRAS##-->", strALL_Rows_anticipo_compras)
            'strMess = strMess.Replace("<!--##CONTENT_HOTEL##-->", strALL_Rows_hotel)
            strMess = strMess.Replace("<!--##CONTENT_RUTA_APROBACION##-->", strALL_Rows)


            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            '********************Find the LogProgram if doesn´t exist nto include**************************

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
            End If

            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            '********************Find the LogProgram if doesn´t exist nto include**************************

            'ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_APPROVAL_Anticipo = True
            Else
                Emailing_APPROVAL_Anticipo = False
            End If

        End Function


        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************

        Public Function Emailing_APPROVAL_PAR(Optional id_AppDoc As Integer = 0, Optional id_par As Integer = 0) As Boolean

            Dim strMess As String
            id_AppDocumento = IIf(id_AppDoc = 0, id_AppDocumento, id_AppDoc)

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
            set_Apps_Document() 'Set all AppDoc

            set_Apps_par(id_par)
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            '*****************ADDING THE SUBJECTS**********************


            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->

            'Change this part temporaly
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)
            'strSubject = strSubject.Replace("<!--##COD_VIAJE##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", get_Apps_DocumentField("descripcion_estado", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'ObJemail.AddSubject(strSubject) Move to Email find

            '*****************FINDING the destinataries **************************************
            'ObJemail.AddTo("grivera@colombiaresponde-ns.org") 'Just for testing

            Dim idTool As Integer = get_Apps_DocumentField("id_approval_tool", "id_documento", id_documento, "id_App_documento", id_AppDocumento)

            If idTool <= 1 Then
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 2 Then 'TimeSheet or Tool
                Add_emailsTS(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), idTool, strSubject) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 5 Then 'Travel or Tool
                Add_emailsPAR(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_par, 1) 'Adding the respective email according the state of the approval proccess
            Else
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            End If



            'Add_emailsPAR(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_par, 1) 'Adding the respective email according the state of the approval proccess
            'ObJemail.AddBcc("grivera@colombiaresponde-ns.org") 'Just for testing
            '*****************FINDING the destinataries **************************************

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




            '**************At this part we neeed to start to replace the Documetn info for this email*******************


            '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_Apps_DocumentField("nombre_programa", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##USUARIO_SOLICITA##-->", get_Apps_ParField("usuario_solicita", "id_par", id_par))
            strMess = strMess.Replace("<!--##NRO_DOC_UDUARIO##-->", get_Apps_ParField("numero_documento", "id_par", id_par))
            strMess = strMess.Replace("<!--##CARGO_USUARIO##-->", get_Apps_ParField("cargo_usuario", "id_par", id_par))
            strMess = strMess.Replace("<!--##REGIONAL##-->", get_Apps_ParField("sub_region", "id_par", id_par))
            strMess = strMess.Replace("<!--##CIUDAD_ENTREGA##-->", get_Apps_ParField("ciudad_entrega", "id_par", id_par))
            strMess = strMess.Replace("<!--##TIPO_PAR##-->", get_Apps_ParField("tipo_par", "id_par", id_par))
            strMess = strMess.Replace("<!--##CODIGO_PAR##-->", get_Apps_ParField("codigo_par", "id_par", id_par))
            strMess = strMess.Replace("<!--##CODIG_PT##-->", get_Apps_ParField("codigo_pt", "id_par", id_par))
            strMess = strMess.Replace("<!--##PROPOSITO_PAR##-->", get_Apps_ParField("proposito", "id_par", id_par))
            'Dim xxx = get_Apps_TravelField("fecha_crea", "id_viaje", id_par)
            Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_ParField("fecha_solicitud", "id_par", id_par)), "f")

            'strMess = strMess.Replace("<!--##FECHA_SOLICITA##-->", get_Apps_TravelField("fecha_crea", "id_viaje", id_par))


            strMess = strMess.Replace("<!--##FECHA_ENVIO##-->", get_Apps_ParField("fecha_solicitud", "id_par", id_par))


            'Dim vFecha2 As String = dateUtil.set_DateFormat(CDate(get_Apps_ParField("fecha_requiere_servicio", "id_par", id_par)), "f")
            strMess = strMess.Replace("<!--##FECHA_SERVICIOS##-->", get_Apps_ParField("fecha_requiere_servicio", "id_par", id_par))


            'strMess = strMess.Replace("<!--##REGIONAL##-->", get_Apps_ParField("region_solicita", "id_par", id_par))

            ''strMess = strMess.Replace("<!--##CODE_PROCESS##-->", get_Apps_DocumentField("codigo_SAP_APP", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'strMess = strMess.Replace("<!--##APPROVAL_CODE##-->", get_Apps_DocumentField("codigo_Approval", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strMess = strMess.Replace("<!--##USER_NAME##-->", get_Apps_DocumentField("nombre_usuario_app", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strMess = strMess.Replace("<!--##NEXT_FASE##-->", get_Apps_DocumentField("nextFase", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento)), "f")
            ''strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            ''dateUtil.set_DateFormat(CDate( ), "f")
            'strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", vFecha)

            strMess = strMess.Replace("<!--##ID_PAR##-->", id_par)
            'strMess = strMess.Replace("<!--##COMMENTS##-->", get_Apps_DocumentField("observacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            strMess = strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

            'Dim strTablePath As String = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1) 'getting the second Element to fill
            Dim strALL_Rows As String = ""
            Dim strRow As String = ""

            Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>

            set_Ta_RutaSeguimiento() 'getting the complete route

            set_Ta_detalle_par(id_par)

            'set_Ta_Viaje_Hotel(id_par)

            '    Public Function set_Ta_Viajje_Itinerario(ByVal id_viaje As Integer) As DataTable

            '    Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_itinerario where id_viaje = {} ", id_viaje)

            '    Tbl_Ta_ViajeItinerario = cl_utl.setObjeto("vw_tme_solicitud_viaje_itinerario", "id_viaje", id_viaje, Sql)
            '    set_Ta_Viajje_Itinerario = Tbl_Ta_ViajeItinerario

            'End Function

            'Public Function set_Ta_Viajje_Hotel(ByVal id_viaje As Integer) As DataTable

            '    Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_hotel where id_viaje = {} ", id_viaje)

            '    Tbl_Ta_ViajeHotel = cl_utl.setObjeto("vw_tme_solicitud_viaje_hotel", "id_viaje", id_viaje, Sql)
            '    set_Ta_Viajje_Hotel = Tbl_Ta_ViajeHotel

            'End Function

            Dim strALL_Rows_par_detalle As String = ""
            Dim strRow_detalle As String = ""

            For Each dtRow In Tbl_Ta_parDetalle.Rows

                strRow_detalle = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1)

                strRow_detalle = strRow_detalle.Replace("<!--##CANTIDAD##-->", dtRow("cantidad"))
                strRow_detalle = strRow_detalle.Replace("<!--##DESCRIPCION##-->", dtRow("descripcion"))
                strRow_detalle = strRow_detalle.Replace("<!--##UNIDAD_MEDIDA##-->", dtRow("unidad_medida"))
                strRow_detalle = strRow_detalle.Replace("<!--##VALOR_UNITARIO##-->", dtRow("precio_unitario"))
                strRow_detalle = strRow_detalle.Replace("<!--##VALOR_TOTAL##-->", dtRow("valor_total"))

                ''If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                'If id_AppDocumento = dtRow("id_app_documento") Then
                '    strRow = strRow.Replace("<tr>", strStyle)
                'End If

                strALL_Rows_par_detalle &= strRow_detalle

            Next

            Dim strALL_Rows_hotel As String = ""
            Dim strRow_hotel As String = ""

            'For Each dtRow In Tbl_Ta_ViajeHotel.Rows

            '    strRow_hotel = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2)

            '    strRow_hotel = strRow_hotel.Replace("<!--##FECHA_LLEGADA##-->", dtRow("fecha_llegada"))
            '    strRow_hotel = strRow_hotel.Replace("<!--##FECHA_SALIDA##-->", dtRow("fecha_salida"))
            '    strRow_hotel = strRow_hotel.Replace("<!--##CIUDAD##-->", dtRow("ciudad"))
            '    strRow_hotel = strRow_hotel.Replace("<!--##HOTEL##-->", dtRow("hotel"))

            '    ''If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
            '    'If id_AppDocumento = dtRow("id_app_documento") Then
            '    '    strRow = strRow.Replace("<tr>", strStyle)
            '    'End If

            '    strALL_Rows_hotel &= strRow_hotel

            'Next



            For Each dtRow In Tbl_Ta_RutaSeguimiento.Rows

                strRow = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2)

                strRow = strRow.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                strRow = strRow.Replace("<!--##EMPLOYEE_NAME##-->", dtRow("nombre_empleado"))
                strRow = strRow.Replace("<!--##STATE##-->", dtRow("descripcion_estado"))
                strRow = strRow.Replace("<!--##DATE_RECEIPT##-->", dtRow("fecha_aprobacion"))
                strALL_Rows &= strRow

            Next

            strMess = strMess.Replace("<!--##CONTENT_PASAJES##-->", strALL_Rows_par_detalle)
            'strMess = strMess.Replace("<!--##CONTENT_HOTEL##-->", strALL_Rows_hotel)
            strMess = strMess.Replace("<!--##CONTENT_RUTA_APROBACION##-->", strALL_Rows)


            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            '********************Find the LogProgram if doesn´t exist nto include**************************

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
            End If

            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            '********************Find the LogProgram if doesn´t exist nto include**************************

            'ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_APPROVAL_PAR = True
            Else
                Emailing_APPROVAL_PAR = False
            End If

        End Function

        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************


        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************

        Public Function Emailing_APPROVAL_TRAVEL(Optional id_AppDoc As Integer = 0, Optional id_viaje As Integer = 0, Optional anulado As Boolean = False, Optional tool As Integer = 0) As Boolean

            Dim strMess As String
            id_AppDocumento = IIf(id_AppDoc = 0, id_AppDocumento, id_AppDoc)

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
            set_Apps_Document() 'Set all AppDoc

            set_Apps_travel(id_viaje)
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            '*****************ADDING THE SUBJECTS**********************


            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->

            'Change this part temporaly
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)
            'strSubject = strSubject.Replace("<!--##COD_VIAJE##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", get_Apps_DocumentField("descripcion_estado", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'ObJemail.AddSubject(strSubject) Move to Email find

            '*****************FINDING the destinataries **************************************
            Dim idTool As Integer = 0
            If tool = 0 Then
                idTool = get_Apps_DocumentField("id_approval_tool", "id_documento", id_documento, "id_App_documento", id_AppDocumento)
            Else
                idTool = tool
            End If

            If idTool <= 1 Then
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 2 Then 'TimeSheet or Tool
                Add_emailsTS(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), idTool, strSubject) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 4 Then 'Travel or Tool
                If anulado = False Then
                    Add_emailsTravel(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_viaje, 1) 'Adding the respective email according the state of the approval proccess
                Else
                    Dim codViaje = get_Apps_TravelField("codigo_solicitud_viaje", "id_viaje", id_viaje)
                    '(ByVal idEstado As Integer, ByVal vOrden As Integer, Optional id_viaje As Integer = 0, Optional tipo As Integer = 0, Optional anulado = False)
                    Add_emailsTravel(0, 0, id_viaje, 0, anulado, codViaje) 'Adding the respective email according the state of the approval proccess
                End If
            Else
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            End If


            'ObJemail.AddTo("grivera@colombiaresponde-ns.org") 'Just for testing
            'Add_emailsTravel(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_par, 1) 'Adding the respective email according the state of the approval proccess
            'ObJemail.AddBcc("grivera@colombiaresponde-ns.org") 'Just for testing
            '*****************FINDING the destinataries **************************************

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




            '**************At this part we neeed to start to replace the Documetn info for this email*******************


            '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_Apps_DocumentField("nombre_programa", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##USUARIO_SOLICITA##-->", get_Apps_TravelField("nombre_usuario", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##NRO_DOC_UDUARIO##-->", get_Apps_TravelField("numero_documento", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##CARGO_USUARIO##-->", get_Apps_TravelField("cargo", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##MOTIVO_VIAJE##-->", get_Apps_TravelField("motivo_viaje", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##CORREO##-->", get_Apps_TravelField("email_usuario", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##TELEFONO##-->", get_Apps_TravelField("numero_contacto", "id_viaje", id_viaje))
            Dim xxx = get_Apps_TravelField("fecha_crea", "id_viaje", id_viaje)
            'Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_TravelField("fecha_crea", "id_viaje", id_par)), "f")
            strMess = strMess.Replace("<!--##FECHA_SOLICITA##-->", get_Apps_TravelField("fecha_crea", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##REGIONAL##-->", get_Apps_TravelField("region_solicita", "id_viaje", id_viaje))

            ''strMess = strMess.Replace("<!--##CODE_PROCESS##-->", get_Apps_DocumentField("codigo_SAP_APP", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'strMess = strMess.Replace("<!--##APPROVAL_CODE##-->", get_Apps_DocumentField("codigo_Approval", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strMess = strMess.Replace("<!--##USER_NAME##-->", get_Apps_DocumentField("nombre_usuario_app", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strMess = strMess.Replace("<!--##NEXT_FASE##-->", get_Apps_DocumentField("nextFase", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento)), "f")
            ''strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            ''dateUtil.set_DateFormat(CDate( ), "f")
            'strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", vFecha)

            strMess = strMess.Replace("<!--##ID_VIAJE##-->", id_viaje)
            'strMess = strMess.Replace("<!--##COMMENTS##-->", get_Apps_DocumentField("observacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            strMess = strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

            'Dim strTablePath As String = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1) 'getting the second Element to fill
            Dim strALL_Rows As String = ""
            Dim strRow As String = ""

            Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>

            set_Ta_RutaSeguimiento() 'getting the complete route

            set_Ta_Viaje_Itinerario(id_viaje)

            set_Ta_Viaje_Hotel(id_viaje)

            '    Public Function set_Ta_Viajje_Itinerario(ByVal id_viaje As Integer) As DataTable

            '    Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_itinerario where id_viaje = {} ", id_viaje)

            '    Tbl_Ta_ViajeItinerario = cl_utl.setObjeto("vw_tme_solicitud_viaje_itinerario", "id_viaje", id_viaje, Sql)
            '    set_Ta_Viajje_Itinerario = Tbl_Ta_ViajeItinerario

            'End Function

            'Public Function set_Ta_Viajje_Hotel(ByVal id_viaje As Integer) As DataTable

            '    Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_hotel where id_viaje = {} ", id_viaje)

            '    Tbl_Ta_ViajeHotel = cl_utl.setObjeto("vw_tme_solicitud_viaje_hotel", "id_viaje", id_viaje, Sql)
            '    set_Ta_Viajje_Hotel = Tbl_Ta_ViajeHotel

            'End Function

            Dim strALL_Rows_itinerario As String = ""
            Dim strRow_itinerario As String = ""

            If Tbl_Ta_ViajeItinerario.Rows.Count() > 0 Then
                For Each dtRow In Tbl_Ta_ViajeItinerario.Rows
                    If dtRow("id_viaje") > 0 Then
                        strRow_itinerario = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1)

                        strRow_itinerario = strRow_itinerario.Replace("<!--##FECHA_VUELO##-->", dtRow("fecha_viaje"))
                        strRow_itinerario = strRow_itinerario.Replace("<!--##HORA_VUELO##-->", dtRow("hora_salida"))
                        strRow_itinerario = strRow_itinerario.Replace("<!--##CIUDAD_ORIGEN##-->", dtRow("origen"))
                        strRow_itinerario = strRow_itinerario.Replace("<!--##CIUDAD_DESTINO##-->", dtRow("destino"))
                        strRow_itinerario = strRow_itinerario.Replace("<!--##TRANSPORTE_AEREO##-->", dtRow("requiere_transporte_aereo"))
                        strRow_itinerario = strRow_itinerario.Replace("<!--##SERVICIO_PUBLICO##-->", dtRow("requiere_servicio_publico"))
                        strRow_itinerario = strRow_itinerario.Replace("<!--##TRANSPORTE_FLUVIAL##-->", dtRow("requiere_transporte_fluvial"))
                        strRow_itinerario = strRow_itinerario.Replace("<!--##VEHICULO_PROYECTO##-->", dtRow("requiere_vehiculo_proyecto"))

                        ''If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                        'If id_AppDocumento = dtRow("id_app_documento") Then
                        '    strRow = strRow.Replace("<tr>", strStyle)
                        'End If

                        strALL_Rows_itinerario &= strRow_itinerario
                    End If


                Next
            End If


            Dim strALL_Rows_hotel As String = ""
            Dim strRow_hotel As String = ""

            If Tbl_Ta_ViajeHotel.Rows.Count() > 0 Then
                For Each dtRow In Tbl_Ta_ViajeHotel.Rows
                    If dtRow("id_viaje") > 0 Then
                        strRow_hotel = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2)

                        strRow_hotel = strRow_hotel.Replace("<!--##FECHA_LLEGADA##-->", dtRow("fecha_llegada"))
                        strRow_hotel = strRow_hotel.Replace("<!--##FECHA_SALIDA##-->", dtRow("fecha_salida"))
                        strRow_hotel = strRow_hotel.Replace("<!--##CIUDAD##-->", dtRow("ciudad"))
                        strRow_hotel = strRow_hotel.Replace("<!--##HOTEL##-->", dtRow("hotel"))

                        ''If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                        'If id_AppDocumento = dtRow("id_app_documento") Then
                        '    strRow = strRow.Replace("<tr>", strStyle)
                        'End If

                        strALL_Rows_hotel &= strRow_hotel
                    End If


                Next
            End If




            For Each dtRow In Tbl_Ta_RutaSeguimiento.Rows

                strRow = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 3)

                strRow = strRow.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                strRow = strRow.Replace("<!--##EMPLOYEE_NAME##-->", dtRow("nombre_empleado"))
                strRow = strRow.Replace("<!--##STATE##-->", dtRow("descripcion_estado"))
                strRow = strRow.Replace("<!--##DATE_RECEIPT##-->", dtRow("fecha_aprobacion"))
                strALL_Rows &= strRow

            Next

            strMess = strMess.Replace("<!--##CONTENT_ITINERARIO##-->", strALL_Rows_itinerario)
            strMess = strMess.Replace("<!--##CONTENT_HOTEL##-->", strALL_Rows_hotel)
            strMess = strMess.Replace("<!--##CONTENT_RUTA_APROBACION##-->", strALL_Rows)


            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            '********************Find the LogProgram if doesn´t exist nto include**************************

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
            End If

            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            '********************Find the LogProgram if doesn´t exist nto include**************************

            'ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_APPROVAL_TRAVEL = True
            Else
                Emailing_APPROVAL_TRAVEL = False
            End If

        End Function

        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************


        '*****************************************************************************************************************
        '********************************************RADICADOS************************************************************
        '*****************************************************************************************************************

        Public Function Emailing_RADICADOS(ByVal radicado As vw_tme_radicados) As Boolean

            Dim strMess As String
            'id_AppDocumento = IIf(id_AppDoc = 0, id_AppDocumento, id_AppDoc)

            '************************************SYSTEM INFO********************************************
            Dim cProgram As New RMS.cls_Program
            cProgram.get_Sys(0, True)
            cProgram.get_Programs(id_programa, True)
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))

            Dim nombrePrograma = cProgram.getprogramField("nombre_programa", "id_programa", id_programa)
            dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
            '************************************SYSTEM INFO********************************************

            '*****************Creating the email Object**************************************
            ObJemail = New UTILS.cls_email(id_programa)
            '*****************Creating the email Object**************************************


            '*****************building the email from the templates objects**************************************

            set_t_notification(id_notification) 'Create the Noti_Object
            set_t_Template() 'Set Template object from the notification id
            'set_Apps_Document() 'Set all AppDoc

            'set_Apps_travel(id_viaje)
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            '*****************ADDING THE SUBJECTS**********************


            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->

            'Change this part temporaly
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)
            'strSubject = strSubject.Replace("<!--##COD_VIAJE##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", get_Apps_DocumentField("descripcion_estado", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'ObJemail.AddSubject(strSubject) Move to Email find

            '*****************FINDING the destinataries **************************************
            Dim idTool As Integer = 0
            'If tool = 0 Then
            '    idTool = get_Apps_DocumentField("id_approval_tool", "id_documento", id_documento, "id_App_documento", id_AppDocumento)
            'Else
            '    idTool = tool
            'End If

            'If idTool <= 1 Then
            '    Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            'ElseIf idTool = 2 Then 'TimeSheet or Tool
            '    Add_emailsTS(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), idTool, strSubject) 'Adding the respective email according the state of the approval proccess
            'ElseIf idTool = 4 Then 'Travel or Tool
            '    If anulado = False Then
            '        Add_emailsTravel(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_viaje, 1) 'Adding the respective email according the state of the approval proccess
            '    Else
            '        Dim codViaje = get_Apps_TravelField("codigo_solicitud_viaje", "id_viaje", id_viaje)
            '        '(ByVal idEstado As Integer, ByVal vOrden As Integer, Optional id_viaje As Integer = 0, Optional tipo As Integer = 0, Optional anulado = False)
            '        Add_emailsTravel(0, 0, id_viaje, 0, anulado, codViaje) 'Adding the respective email according the state of the approval proccess
            '    End If
            'Else
            '    Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            'End If


            'ObJemail.AddTo("grivera@colombiaresponde-ns.org") 'Just for testing
            'Add_emailsTravel(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_par, 1) 'Adding the respective email according the state of the approval proccess
            'ObJemail.AddBcc("grivera@colombiaresponde-ns.org") 'Just for testing
            '*****************FINDING the destinataries **************************************

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

            Add_emailsRadicado(radicado.email_usuario, radicado.email_gerente_oficina, radicado.codigo)


            '**************At this part we neeed to start to replace the Documetn info for this email*******************


            '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", nombrePrograma)
            strMess = strMess.Replace("<!--##USUARIO_SOLICITA##-->", radicado.nombre_responsable)
            strMess = strMess.Replace("<!--##CARGO_USUARIO##-->", radicado.job)
            strMess = strMess.Replace("<!--##FECHA_RADICADO##-->", radicado.fecha_radicado)
            strMess = strMess.Replace("<!--##CORREO##-->", radicado.email_usuario)
            strMess = strMess.Replace("<!--##REGIONAL##-->", radicado.nombre_subregion)
            strMess = strMess.Replace("<!--##CODIGO_RADICADO##-->", radicado.codigo)
            strMess = strMess.Replace("<!--##TIPO_DOCUMENTO##-->", radicado.tipo_documento)
            strMess = strMess.Replace("<!--##TERCERO##-->", radicado.identificacion_tercero_a_pagar & " - " & radicado.tercero_a_pagar)
            strMess = strMess.Replace("<!--##VALOR##-->", radicado.valor_factura)
            strMess = strMess.Replace("<!--##CARACTER##-->", radicado.caracter)
            strMess = strMess.Replace("<!--##DOCUMENTO##-->", radicado.detalle)
            strMess = strMess.Replace("<!--##MOTIVO_RECHAZO##-->", radicado.motivo_rechazo)



            'Dim strTablePath As String = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1) 'getting the second Element to fill
            Dim strALL_Rows As String = ""
            Dim strRow As String = ""

            Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>

            set_Ta_RutaSeguimiento() 'getting the complete route


            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            '********************Find the LogProgram if doesn´t exist nto include**************************

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
            End If

            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            '********************Find the LogProgram if doesn´t exist nto include**************************

            'ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_RADICADOS = True
            Else
                Emailing_RADICADOS = False
            End If

        End Function

        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************


        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************

        Public Function Emailing_APPROVAL_TRAVEL_LEGALIZATION(Optional id_AppDoc As Integer = 0, Optional id_viaje As Integer = 0) As Boolean

            Dim strMess As String
            id_AppDocumento = IIf(id_AppDoc = 0, id_AppDocumento, id_AppDoc)

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
            set_Apps_Document() 'Set all AppDoc

            set_Apps_travel(id_viaje)
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            '*****************ADDING THE SUBJECTS**********************


            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->

            'Change this part temporaly
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)
            'strSubject = strSubject.Replace("<!--##COD_VIAJE##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", get_Apps_DocumentField("descripcion_estado", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'ObJemail.AddSubject(strSubject) Move to Email find

            '*****************FINDING the destinataries **************************************
            'ObJemail.AddTo("grivera@colombiaresponde-ns.org") 'Just for testing
            'Add_emailsTravel(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_viaje, 2) 'Adding the respective email according the state of the approval proccess

            Dim idTool As Integer = get_Apps_DocumentField("id_approval_tool", "id_documento", id_documento, "id_App_documento", id_AppDocumento)

            If idTool <= 1 Then
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 2 Then 'TimeSheet or Tool
                Add_emailsTS(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), idTool, strSubject) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 4 Then 'Travel or Tool
                Add_emailsTravel(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_viaje, 2) 'Adding the respective email according the state of the approval proccess
            Else
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            End If


            'ObJemail.AddBcc("grivera@colombiaresponde-ns.org") 'Just for testing
            '*****************FINDING the destinataries **************************************

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




            '**************At this part we neeed to start to replace the Documetn info for this email*******************


            '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_Apps_DocumentField("nombre_programa", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##USUARIO_SOLICITA##-->", get_Apps_TravelField("nombre_usuario", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##NRO_DOC_UDUARIO##-->", get_Apps_TravelField("numero_documento", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##CARGO_USUARIO##-->", get_Apps_TravelField("cargo", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##MOTIVO_VIAJE##-->", get_Apps_TravelField("motivo_viaje", "id_viaje", id_viaje))
            Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_TravelField("fecha_legalizacion", "id_viaje", id_viaje)), "f")
            strMess = strMess.Replace("<!--##FECHA_ENVIO##-->", vFecha)
            strMess = strMess.Replace("<!--##CODIGO_VIAJE##-->", get_Apps_TravelField("codigo_solicitud_viaje", "id_viaje", id_viaje))
            vFecha = dateUtil.set_DateFormat(CDate(get_Apps_TravelField("fecha_tasa_ser", "id_viaje", id_viaje)), "f")
            strMess = strMess.Replace("<!--##FECHA_TASA_SER##-->", vFecha)
            strMess = strMess.Replace("<!--##TASA_SER##-->", get_Apps_TravelField("tasa_ser", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##REGIONAL##-->", get_Apps_TravelField("region_solicita", "id_viaje", id_viaje))

            strMess = strMess.Replace("<!--##CORREO##-->", get_Apps_TravelField("email_usuario", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##TELEFONO##-->", get_Apps_TravelField("numero_contacto", "id_viaje", id_viaje))

            ''strMess = strMess.Replace("<!--##CODE_PROCESS##-->", get_Apps_DocumentField("codigo_SAP_APP", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'strMess = strMess.Replace("<!--##APPROVAL_CODE##-->", get_Apps_DocumentField("codigo_Approval", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strMess = strMess.Replace("<!--##USER_NAME##-->", get_Apps_DocumentField("nombre_usuario_app", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strMess = strMess.Replace("<!--##NEXT_FASE##-->", get_Apps_DocumentField("nextFase", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento)), "f")
            ''strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            ''dateUtil.set_DateFormat(CDate( ), "f")
            'strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", vFecha)

            strMess = strMess.Replace("<!--##ID_VIAJE##-->", id_viaje)
            'strMess = strMess.Replace("<!--##COMMENTS##-->", get_Apps_DocumentField("observacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            strMess = strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

            'Dim strTablePath As String = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1) 'getting the second Element to fill
            Dim strALL_Rows As String = ""
            Dim strRow As String = ""

            Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>

            set_Ta_RutaSeguimiento() 'getting the complete route

            set_Ta_Viaje_Pasajes(id_viaje)

            '    Public Function set_Ta_Viajje_Itinerario(ByVal id_viaje As Integer) As DataTable

            '    Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_itinerario where id_viaje = {} ", id_viaje)

            '    Tbl_Ta_ViajeItinerario = cl_utl.setObjeto("vw_tme_solicitud_viaje_itinerario", "id_viaje", id_viaje, Sql)
            '    set_Ta_Viajje_Itinerario = Tbl_Ta_ViajeItinerario

            'End Function

            'Public Function set_Ta_Viajje_Hotel(ByVal id_viaje As Integer) As DataTable

            '    Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_hotel where id_viaje = {} ", id_viaje)

            '    Tbl_Ta_ViajeHotel = cl_utl.setObjeto("vw_tme_solicitud_viaje_hotel", "id_viaje", id_viaje, Sql)
            '    set_Ta_Viajje_Hotel = Tbl_Ta_ViajeHotel

            'End Function

            Dim strALL_Rows_pasajes As String = ""
            Dim strRow_pasajes As String = ""

            For Each dtRow In Tbl_Ta_ViajePasaje.Rows
                If dtRow("id_viaje") > 0 Then
                    strRow_pasajes = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1)

                    strRow_pasajes = strRow_pasajes.Replace("<!--##FECHA_SERVICIO##-->", dtRow("fecha"))
                    strRow_pasajes = strRow_pasajes.Replace("<!--##CODIGO_FACTURACION##-->", dtRow("codigo_facturacion"))
                    strRow_pasajes = strRow_pasajes.Replace("<!--##NRO_REC##-->", dtRow("nro_rec"))
                    strRow_pasajes = strRow_pasajes.Replace("<!--##DESCRIPCION_GASTO##-->", dtRow("descripcion_gasto"))
                    strRow_pasajes = strRow_pasajes.Replace("<!--##CODE_PASAJES##-->", dtRow("codigo_legalizacion_pasajes"))
                    strRow_pasajes = strRow_pasajes.Replace("<!--##MONTO_PASAJES##-->", dtRow("monto_pasajes"))

                    ''If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                    'If id_AppDocumento = dtRow("id_app_documento") Then
                    '    strRow = strRow.Replace("<tr>", strStyle)
                    'End If

                    strALL_Rows_pasajes &= strRow_pasajes
                End If
            Next

            Dim strALL_Rows_reuniones As String = ""
            Dim strRow_reuniones As String = ""

            For Each dtRow In Tbl_Ta_ViajeReuniones.Rows

                If dtRow("id_viaje") > 0 Then
                    strRow_reuniones = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 2)

                    strRow_reuniones = strRow_reuniones.Replace("<!--##FECHA_SERVICIO##-->", dtRow("fecha"))
                    strRow_reuniones = strRow_reuniones.Replace("<!--##CODIGO_FACTURACION##-->", dtRow("codigo_facturacion"))
                    strRow_reuniones = strRow_reuniones.Replace("<!--##NRO_REC##-->", dtRow("nro_rec"))
                    strRow_reuniones = strRow_reuniones.Replace("<!--##ENT_MTG##-->", dtRow("ent_mtg"))
                    strRow_reuniones = strRow_reuniones.Replace("<!--##PARTICIPANTES##-->", dtRow("nro_participantes"))
                    strRow_reuniones = strRow_reuniones.Replace("<!--##DESCRIPCION_GASTO##-->", dtRow("descripcion_gasto"))
                    strRow_reuniones = strRow_reuniones.Replace("<!--##MONTO##-->", dtRow("monto_total"))

                    ''If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                    'If id_AppDocumento = dtRow("id_app_documento") Then
                    '    strRow = strRow.Replace("<tr>", strStyle)
                    'End If

                    strALL_Rows_reuniones &= strRow_reuniones

                End If

            Next

            Dim strALL_Rows_miscelaneos As String = ""
            Dim strRow_miscelaneos As String = ""

            For Each dtRow In Tbl_Ta_ViajeMiscelaneos.Rows
                If dtRow("id_viaje") > 0 Then
                    strRow_miscelaneos = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 3)

                    strRow_miscelaneos = strRow_miscelaneos.Replace("<!--##FECHA_SERVICIO##-->", dtRow("fecha"))
                    strRow_miscelaneos = strRow_miscelaneos.Replace("<!--##CODIGO_FACTURACION##-->", dtRow("codigo_facturacion"))
                    strRow_miscelaneos = strRow_miscelaneos.Replace("<!--##NRO_REC##-->", dtRow("nro_rec"))
                    strRow_miscelaneos = strRow_miscelaneos.Replace("<!--##DESCRIPCION_GASTO##-->", dtRow("descripcion_gasto"))
                    strRow_miscelaneos = strRow_miscelaneos.Replace("<!--##MONTO##-->", dtRow("monto_total"))

                    ''If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                    'If id_AppDocumento = dtRow("id_app_documento") Then
                    '    strRow = strRow.Replace("<tr>", strStyle)
                    'End If

                    strALL_Rows_miscelaneos &= strRow_miscelaneos
                End If


            Next

            Dim strALL_Rows_alojamiento As String = ""
            Dim strRow_alojamiento As String = ""

            For Each dtRow In Tbl_Ta_ViajeAlonamiento.Rows

                If dtRow("id_viaje") > 0 Then
                    strRow_alojamiento = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 4)

                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##FECHA_SERVICIO##-->", dtRow("fecha"))
                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##CODIGO_FACTURACION##-->", dtRow("codigo_facturacion"))
                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##NRO_REC##-->", dtRow("nro_rec"))
                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##LUGAR##-->", dtRow("ubicacion_alojamiento"))
                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##DESCRIPCION_GASTO##-->", dtRow("descripcion_gasto"))
                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##PORC_VIATICO##-->", dtRow("porcentaje_perdiem"))
                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##MYE_RATE##-->", dtRow("valor_perdiem"))
                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##NRO_DIAS##-->", dtRow("numero_dias"))
                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##DESC_ALIME##-->", dtRow("descuento_alimentacion"))
                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##MONTO_ALIME##-->", dtRow("valor_total_alimentacion"))
                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##MONTO_ALOJA##-->", dtRow("monto_alojamiento"))
                    strRow_alojamiento = strRow_alojamiento.Replace("<!--##MONTO##-->", dtRow("monto_total"))

                    ''If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                    'If id_AppDocumento = dtRow("id_app_documento") Then
                    '    strRow = strRow.Replace("<tr>", strStyle)
                    'End If

                    strALL_Rows_alojamiento &= strRow_alojamiento
                End If


            Next



            For Each dtRow In Tbl_Ta_RutaSeguimiento.Rows

                strRow = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 5)

                strRow = strRow.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                strRow = strRow.Replace("<!--##EMPLOYEE_NAME##-->", dtRow("nombre_empleado"))
                strRow = strRow.Replace("<!--##STATE##-->", dtRow("descripcion_estado"))
                strRow = strRow.Replace("<!--##DATE_RECEIPT##-->", dtRow("fecha_aprobacion"))
                strALL_Rows &= strRow

            Next

            strMess = strMess.Replace("<!--##CONTENT_PASAJES##-->", strALL_Rows_pasajes)
            strMess = strMess.Replace("<!--##CONTENT_REUNIONE##-->", strALL_Rows_reuniones)
            strMess = strMess.Replace("<!--##CONTENT_MISCELANEOS##-->", strALL_Rows_miscelaneos)
            strMess = strMess.Replace("<!--##CONTENT_ALOJAMIENTO##-->", strALL_Rows_alojamiento)
            strMess = strMess.Replace("<!--##CONTENT_RUTA_APROBACION##-->", strALL_Rows)


            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            '********************Find the LogProgram if doesn´t exist nto include**************************

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
            End If

            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            '********************Find the LogProgram if doesn´t exist nto include**************************

            'ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_APPROVAL_TRAVEL_LEGALIZATION = True
            Else
                Emailing_APPROVAL_TRAVEL_LEGALIZATION = False
            End If

        End Function

        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************


        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************

        Public Function Emailing_APPROVAL_TRAVEL_INFORME(Optional id_AppDoc As Integer = 0, Optional id_viaje As Integer = 0) As Boolean

            Dim strMess As String
            id_AppDocumento = IIf(id_AppDoc = 0, id_AppDocumento, id_AppDoc)

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
            set_Apps_Document() 'Set all AppDoc

            set_Apps_travel(id_viaje)
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            '*****************ADDING THE SUBJECTS**********************


            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->

            'Change this part temporaly
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)
            'strSubject = strSubject.Replace("<!--##COD_VIAJE##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", get_Apps_DocumentField("descripcion_estado", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'ObJemail.AddSubject(strSubject) Move to Email find

            '*****************FINDING the destinataries **************************************
            'ObJemail.AddTo("grivera@colombiaresponde-ns.org") 'Just for testing


            Dim idTool As Integer = get_Apps_DocumentField("id_approval_tool", "id_documento", id_documento, "id_App_documento", id_AppDocumento)

            If idTool <= 1 Then
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 2 Then 'TimeSheet or Tool
                Add_emailsTS(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), idTool, strSubject) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 4 Then 'Travel or Tool
                Add_emailsTravel(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_viaje, 3) 'Adding the respective email according the state of the approval proccess
            Else
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            End If



            'Add_emailsTravel(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), id_viaje, 3) 'Adding the respective email according the state of the approval proccess
            'ObJemail.AddBcc("grivera@colombiaresponde-ns.org") 'Just for testing
            '*****************FINDING the destinataries **************************************

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




            '**************At this part we neeed to start to replace the Documetn info for this email*******************


            '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_Apps_DocumentField("nombre_programa", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##USUARIO_SOLICITA##-->", get_Apps_TravelField("nombre_usuario", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##NRO_DOC_UDUARIO##-->", get_Apps_TravelField("numero_documento", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##CARGO_USUARIO##-->", get_Apps_TravelField("cargo", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##MOTIVO_VIAJE##-->", get_Apps_TravelField("motivo_viaje", "id_viaje", id_viaje))
            Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_TravelField("fecha_registro_informe", "id_viaje", id_viaje)), "f")
            strMess = strMess.Replace("<!--##FECHA_ENVIO##-->", vFecha)
            strMess = strMess.Replace("<!--##REGIONAL##-->", get_Apps_TravelField("region_solicita", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##CODIGO_VIAJE##-->", get_Apps_TravelField("codigo_solicitud_viaje", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##INFORME_RESULTADO##-->", get_Apps_TravelField("informe_resultado", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##INFORME_ACTIVIDADES##-->", get_Apps_TravelField("informe_compromiso", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##INFORME_LUGARES_ENTIDADES_PERSONAS##-->", get_Apps_TravelField("lugares_entidades_personas", "id_viaje", id_viaje))

            strMess = strMess.Replace("<!--##CORREO##-->", get_Apps_TravelField("email_usuario", "id_viaje", id_viaje))
            strMess = strMess.Replace("<!--##TELEFONO##-->", get_Apps_TravelField("numero_contacto", "id_viaje", id_viaje))

            ''strMess = strMess.Replace("<!--##CODE_PROCESS##-->", get_Apps_DocumentField("codigo_SAP_APP", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'strMess = strMess.Replace("<!--##APPROVAL_CODE##-->", get_Apps_DocumentField("codigo_Approval", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strMess = strMess.Replace("<!--##USER_NAME##-->", get_Apps_DocumentField("nombre_usuario_app", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'strMess = strMess.Replace("<!--##NEXT_FASE##-->", get_Apps_DocumentField("nextFase", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento)), "f")
            ''strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            ''dateUtil.set_DateFormat(CDate( ), "f")
            'strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", vFecha)

            strMess = strMess.Replace("<!--##ID_VIAJE##-->", id_viaje)
            'strMess = strMess.Replace("<!--##COMMENTS##-->", get_Apps_DocumentField("observacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            strMess = strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

            'Dim strTablePath As String = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1) 'getting the second Element to fill
            Dim strALL_Rows As String = ""
            Dim strRow As String = ""

            Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>

            set_Ta_RutaSeguimiento() 'getting the complete route

            set_Ta_Viaje_Pasajes(id_viaje)

            '    Public Function set_Ta_Viajje_Itinerario(ByVal id_viaje As Integer) As DataTable

            '    Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_itinerario where id_viaje = {} ", id_viaje)

            '    Tbl_Ta_ViajeItinerario = cl_utl.setObjeto("vw_tme_solicitud_viaje_itinerario", "id_viaje", id_viaje, Sql)
            '    set_Ta_Viajje_Itinerario = Tbl_Ta_ViajeItinerario

            'End Function

            'Public Function set_Ta_Viajje_Hotel(ByVal id_viaje As Integer) As DataTable

            '    Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_hotel where id_viaje = {} ", id_viaje)

            '    Tbl_Ta_ViajeHotel = cl_utl.setObjeto("vw_tme_solicitud_viaje_hotel", "id_viaje", id_viaje, Sql)
            '    set_Ta_Viajje_Hotel = Tbl_Ta_ViajeHotel

            'End Function




            For Each dtRow In Tbl_Ta_RutaSeguimiento.Rows

                strRow = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1)

                strRow = strRow.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                strRow = strRow.Replace("<!--##EMPLOYEE_NAME##-->", dtRow("nombre_empleado"))
                strRow = strRow.Replace("<!--##STATE##-->", dtRow("descripcion_estado"))
                strRow = strRow.Replace("<!--##DATE_RECEIPT##-->", dtRow("fecha_aprobacion"))
                strALL_Rows &= strRow

            Next


            strMess = strMess.Replace("<!--##CONTENT_RUTA_APROBACION##-->", strALL_Rows)


            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            '********************Find the LogProgram if doesn´t exist nto include**************************

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
            End If

            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            '********************Find the LogProgram if doesn´t exist nto include**************************

            'ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_APPROVAL_TRAVEL_INFORME = True
            Else
                Emailing_APPROVAL_TRAVEL_INFORME = False
            End If

        End Function

        '*****************************************************************************************************************
        '********************************************APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************


        Public Function set_t_entregableHito(ByVal id_ficha_entre As Integer) As DataTable

            id_ficha_entHito = IIf(id_ficha_entre > 0, id_ficha_entre, 0)
            Dim strSql As String = String.Format("select * from  vw_ficha_proyecto_hitos  where id_hito = {0} ", id_ficha_entHito)

            tbl_t_entregableHito = cl_utl.setObjeto("vw_ficha_proyecto_hitos", "id_hito", id_ficha_entHito, strSql)
            set_t_entregableHito = tbl_t_entregable

        End Function

        Public Function set_T_RutaHito(ByVal id_ficha_entre As Integer) As DataTable

            id_ficha_ent = IIf(id_ficha_entre > 0, id_ficha_entre, 0)
            Dim strSql As String = String.Format("select * from vw_ruta_aprobacion_hito_history where id_hito = {0} ", id_ficha_ent)

            tbl_t_ruta_entregable = cl_utl.setObjeto("vw_ruta_aprobacion_hito_history", "id_hito", id_ficha_ent, strSql)
            set_T_RutaHito = tbl_t_ruta_entregable

        End Function
        Public Function set_T_ComentariosHito(ByVal id_ficha_entre As Integer) As DataTable

            id_ficha_ent = IIf(id_ficha_entre > 0, id_ficha_entre, 0)
            Dim strSql As String = String.Format("select id_hito, id_ruta_hito, isnull(Convert(varchar(40),fecha_envio),'--') as fecha_envio_text, usuario_aprueba, 
                isnull(comentarios,'') comentarios,  isnull(soporte,'') soporte from vw_comentario_hitos where id_hito = {0}   order by fecha_envio asc", id_ficha_ent)

            tbl_t_comentarios_entregableHito = cl_utl.setObjeto("vw_comentario_hitos", "id_hito", id_ficha_ent, strSql)
            set_T_ComentariosHito = tbl_t_comentarios_entregableHito

        End Function
        Public Function set_t_t_entregables_hito(ByVal id_hito As Integer) As DataTable

            id_ficha_ent = IIf(id_hito > 0, id_hito, 0)
            Dim strSql As String = String.Format("select id_entregable_hito, nro_entregable, descripcion_entregable, observaciones_entregable from tme_hitos_entregables where id_hito = {0}   order by nro_entregable asc", id_hito)

            tbl_t_entregables_hito = cl_utl.setObjeto("tme_hitos_entregables", "id_hito", id_hito, strSql)
            set_t_t_entregables_hito = tbl_t_entregables_hito

        End Function

        Public Function get_t_productsEntregableFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_t_entregableHito, campoSearch, campo, valorSearch)

        End Function
        Public Function Emailing_ENTREGABLES_ACTIONS(ByVal id_hito As Integer, ByVal asunto As String, ByVal email_to As String, ByVal id_estado_aprobacion_hito As Integer, ByVal id_estado_ruta As Integer, ByVal tiene_ruta As Integer) As Boolean
            Try


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
                set_Apps_Document() 'Set all AppDoc

                strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

                '*****************ADDING THE SUBJECTS**********************

                Add_emailsDonaciones(email_to, "", asunto) 'Adding the respective email according the state of the approval proccess

                set_t_programa()
                set_t_entregableHito(id_hito)
                set_T_RutaHito(id_hito)
                set_T_ComentariosHito(id_hito)
                set_t_t_entregables_hito(id_hito)

                Dim strSubject As String = asunto

                'ObJemail.AddSubject(strSubject) 'Subject


                '**************At this part we neeed to start to replace the Documetn info for this email*******************

                '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
                strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_t_programaFIELDS("nombre_programa", "id_programa", id_programa))


                strMess = strMess.Replace("<!--##CODIGO##-->", get_t_productsEntregableFIELDS("codigo_RFA", "id_hito", id_hito))
                strMess = strMess.Replace("<!--##ACTIVIDAD##-->", get_t_productsEntregableFIELDS("nombre_proyecto", "id_hito", id_hito))
                strMess = strMess.Replace("<!--##EJECUTOR##-->", get_t_productsEntregableFIELDS("nombre_ejecutor", "id_hito", id_hito))


                strMess = strMess.Replace("<!--##NUMERO##-->", get_t_productsEntregableFIELDS("nro_hito", "id_hito", id_hito))
                strMess = strMess.Replace("<!--##PRODUCTO##-->", get_t_productsEntregableFIELDS("descripcion_hito", "id_hito", id_hito))
                strMess = strMess.Replace("<!--##FECHAENTREGA##-->", get_t_productsEntregableFIELDS("fecha_entrega", "id_hito", id_hito))
                strMess = strMess.Replace("<!--##DIASFALTANTES##-->", get_t_productsEntregableFIELDS("dias_restantes", "id_hito", id_hito))
                strMess = strMess.Replace("<!--##URLLINK##-->", "<a href ='http://sime.justiciainclusiva.org/frm_login?idsgtoHito=" & id_hito & "' style=' text-decoration:none; font-family:Verdana, Arial;'> Ver en el SIME <a />")
                strMess = strMess.Replace("<!--##ALERTAPRO##-->", get_t_productsEntregableFIELDS("texto_alerta", "id_hito", id_hito))
                Dim strTableContent As String = ""
                Dim strTableContentALL As String = ""

                For Each dtRow In tbl_t_ruta_entregable.Rows
                    strTableContent = get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 1)
                    strTableContent = strTableContent.Replace("<!--##AREAENC##-->", dtRow("responsable"))
                    strTableContent = strTableContent.Replace("<!--##USURESP##-->", dtRow("usuario_aprueba"))
                    strTableContent = strTableContent.Replace("<!--##FECHAACC##-->", dtRow("fecha_envio"))
                    strTableContent = strTableContent.Replace("<!--##ESTADOPR##-->", dtRow("estado"))
                    strTableContentALL &= strTableContent
                Next


                strMess = strMess.Replace("<!--##RUTAAPR##-->", strTableContentALL)
                strTableContent = ""
                strTableContentALL = ""

                For Each dtRow In tbl_t_comentarios_entregableHito.Rows
                    strTableContent = get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 2)
                    strTableContent = strTableContent.Replace("<!--##FECHACOM##-->", dtRow("fecha_envio_text"))
                    strTableContent = strTableContent.Replace("<!--##USUARIOCOM##-->", dtRow("usuario_aprueba"))
                    strTableContent = strTableContent.Replace("<!--##COMENTARIO##-->", dtRow("comentarios"))
                    strTableContentALL &= strTableContent
                Next

                strMess = strMess.Replace("<!--##COMENTAPRO##-->", strTableContentALL)



                strTableContent = ""
                strTableContentALL = ""
                For Each dtRow In tbl_t_entregables_hito.Rows
                    strTableContent = get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 3)
                    strTableContent = strTableContent.Replace("<!--##NROENTREGABLE##-->", dtRow("nro_entregable"))
                    strTableContent = strTableContent.Replace("<!--##DESCRIPCION##-->", dtRow("descripcion_entregable"))
                    strTableContent = strTableContent.Replace("<!--##OBSERVACIONESENTREGABLE##-->", dtRow("observaciones_entregable"))
                    strTableContentALL &= strTableContent
                Next

                strMess = strMess.Replace("<!--##ENTREGABLESHITO##-->", strTableContentALL)


                'strMess = strMess.Replace("<!--##STEP_TABLE##-->", strTablePath) 'this would be by template
                '**************At this part we neeed to start to replace the Document info for this email*******************
                strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
                strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
                '************************* THE message it supposed Is here, need to proceed to test*****************
                '*****************building the email from the templates objects**************************************

                ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

                '********************Find the LogProgram if doesn´t exist nto include**************************

                Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
                Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

                If Not String.IsNullOrEmpty(strProgramIMG) Then
                    ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
                Else
                    ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
                End If

                ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
                '********************Find the LogProgram if doesn´t exist nto include**************************

                ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
                ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
                ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
                ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

                ObJemail.SetPriority(MailPriority.High)

                If ObJemail.SendEmail(id_notification) Then
                    Emailing_ENTREGABLES_ACTIONS = True
                Else
                    Emailing_ENTREGABLES_ACTIONS = False
                End If
            Catch ex As Exception
                Return False
            End Try


        End Function






        Public Sub Add_emailsContratos(ByVal email1 As String, ByVal email2 As String, ByVal asunto As String) 'According the state

            Set_Emails_List() 'SE the Email List
            Dim strSubject As String = asunto

            Add_Emails_List("TO", email1) 'Actual, Originator 0
            Add_Emails_List("TO", email2) 'Actual, Originator 0
            'Add_Emails_List("CC", "ybohorquez@justiciainclusiva.org") 'Actual step Group 0




            If Not ObJemail.SilentMode Then

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



            Else
                ObJemail.AddTo("ing.ychacon@gmail.com")
                strSubject &=  " (Silent MODE) "
            End If

            'Adding the subjects according the state
            ObJemail.AddSubject(strSubject)


        End Sub

        Public Sub Add_emailsDonaciones(ByVal email1 As String, ByVal email2 As String, ByVal asunto As String) 'According the state

            Set_Emails_List() 'SE the Email List
            Dim strSubject As String = asunto

            Add_Emails_List("TO", email1) 'Actual, Originator 0
            If email2.Length > 4 Then
                Add_Emails_List("TO", email2) 'Actual, Originator 0
            End If
            Add_Emails_List("CC", "ybohorquez@justiciainclusiva.org") 'Actual step Group 0




            If Not ObJemail.SilentMode Then

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



            Else
                ObJemail.AddTo("ing.ychacon@gmail.com")
                strSubject &= " (Silent MODE) "
            End If

            'Adding the subjects according the state
            ObJemail.AddSubject(strSubject)


        End Sub
        Public Sub Add_emailsTravel(ByVal idEstado As Integer, ByVal vOrden As Integer, Optional id_viaje As Integer = 0, Optional tipo As Integer = 0, Optional anulado As Boolean = False, Optional codigo_viaje As String = "") 'According the state

            Set_Emails_List() 'SE the Email List
            Dim strSubject As String = ""

            Dim tipoApp = ""
            Dim campo = ""
            Dim campo2 = ""
            Dim campo3 = ""
            If tipo = 1 Then
                tipoApp = "Solicitud de viaje"
                campo = "id_usuario_app"
                campo2 = "id_usuario_radica"
                campo3 = "id_usuario_notificacion_tiquete"
            ElseIf tipo = 2 Then
                tipoApp = "Legalización de viaje"
                campo = "id_usuario_app_legalizacion"
                campo2 = "id_usuario_radica"
                campo3 = "id_usuario_notificacion_tiquete"
            ElseIf tipo = 3 Then
                tipoApp = "Informe de viaje"
                campo = "id_usuario_app_informe"
                campo2 = "id_usuario_radica"
                campo3 = "id_usuario_notificacion_tiquete"
            ElseIf anulado Then
                tipoApp = "El viaje"
                campo = "id_gerente_oficina"
                campo2 = "id_usuario_radica"
            End If
            If anulado = False Then
                Select Case idEstado

                    Case cOPEN

                        'strSubject = strSubject.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is Open ")

                        strSubject = String.Format("Approvals System: {0} número {1} enviado por aprobación.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))


                        Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual, Originator 0
                        Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step


                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual, Originator 0
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step


                        'Add_Emails_List("TO", set_ta_travel_roles_emails(vOrden, id_viaje, campo)) 'Actual, Originator 0
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual, Originator Group 0
                        Add_Emails_List("TO", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group

                    Case cPENDING


                        'Approvals System: Notification of documents pending approval
                        'Approvals System: APPROVAL Process XXXX Is pending approval
                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is pending for approval ")
                        strSubject = String.Format("Approvals System: {0} número {1} enviado por aprobación.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                        'Add_Emails_List("TO", set_ta_travel_roles_emails(vOrden, id_viaje, campo)) 'Actual, Originator 0
                        Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                        Add_Emails_List("TO", set_ta_roles_emails(vOrden))
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual
                        Add_Emails_List("TO", set_ta_GrupoRoles_emails(vOrden)) 'Actual step Group 0

                    Case cAPPROVED


                        'Add a for loop to send email to all participated before!
                        Dim maxOrder As Integer = 0
                        Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                        If vOrden > 0 Then 'Normal approval, the Approved status just come for order hier than 0
                            Add_Emails_List("TO", set_ta_travel_roles_emails(vOrden, id_viaje, campo))
                            'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                            'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step
                            Add_Emails_List("TO", set_ta_travel_roles_emails((vOrden + 1), id_viaje, campo))
                        Else 'StandBy Step, because order 0 genereta a approval status

                            'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step we dont need to add againg to the  Originator
                            'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step, instead of that we need to add to the last order already created, to reconfirm the notification 
                            Dim clss_approval As APPROVAL.clss_approval
                            clss_approval = New APPROVAL.clss_approval(id_programa)
                            maxOrder = clss_approval.get_ta_AppDocumentoOrden_MAX(id_documento).Rows().Item(0).Item("orden") ' To get the Max ORder values to make the same step again
                            Add_Emails_List("TO", set_ta_roles_emails(maxOrder)) 'The last step who did the standby
                            Add_Emails_List("TO", set_ta_travel_roles_emails(maxOrder, id_viaje, campo))
                        End If

                        Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator
                        If vOrden > 0 Then 'Normal
                            Add_Emails_List("TO", set_ta_travel_roles_emails(vOrden, id_viaje, campo))
                            ' Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                            'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group
                            Add_Emails_List("TO", set_ta_travel_roles_emails((vOrden + 1), id_viaje, campo))
                            'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is approved for next phase ")
                            strSubject = String.Format("Approvals System: {0} {1} siguiente fase.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                        Else 'StandBy Step

                            'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " Document approval for next phase. Observations completed ")
                            Add_Emails_List("TO", set_ta_travel_roles_emails(maxOrder, id_viaje, campo))
                            'Add_Emails_List("CC", set_ta_GrupoRoles_emails(maxOrder)) 'The last step group of whom did the standby
                            'Add_Emails_List("CC", set_ta_GrupoRoles_emails(maxOrder + 1)) 'Next Step Group of last step group of whom did the standby
                            Add_Emails_List("TO", set_ta_travel_roles_emails((maxOrder + 1), id_viaje, campo))
                            strSubject = String.Format("Approvals System: {0} {1} siguiente fase. Observaciones completadas.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                        End If



                    Case cnotAPPROVED

                        Add_Emails_List("TO", set_ta_travel_roles_emails(vOrden, id_viaje, campo))
                        Add_Emails_List("TO", set_ta_travel_roles_emails(vOrden, id_viaje, campo2))
                        'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups
                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is not approved, the approval document has finished ")
                        strSubject = String.Format("Approvals System: {0} {1} no aprobado.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    Case cCANCELLED

                        Add_Emails_List("TO", set_ta_travel_roles_emails(vOrden, id_viaje, campo))
                        Add_Emails_List("TO", set_ta_travel_roles_emails(vOrden, id_viaje, campo2))
                        'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is not Cancelled, the approval document has finished ")
                        strSubject = String.Format("Approvals System: {0} {1} cancelado", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    Case cSTANDby


                        'Add a for loop to send email to all participated before!

                        Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                        'Add_Emails_List("CC", get_t_notification_emails("RHHMNG")) 'Infom to the Manager


                        'Add_Emails_List("TO", set_ta_travel_roles_emails(vOrden, id_viaje, campo))

                        'Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator
                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group

                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is classified as Stand By ")
                        strSubject = String.Format("Approvals System: {0} {1} solicitud de ajustes.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))


                    Case cCOMPLETED

                        'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " has been completed ")
                        Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                        Add_Emails_List("TO", set_ta_travel_roles_emails(vOrden, id_viaje, "0", campo2, campo3))
                        strSubject = String.Format("Approvals System: {0} {1} aprobado.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))



                        'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups
                        'Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                        'Add_Emails_List("CC", get_t_notification_emails_end_app(id_documento)) 'Infom to the Manager

                        ' strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} has been completed.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                        'strSubject = toolSubject.Replace("<!--##STATE_STEP##-->", "has been completed.")

                End Select
            Else

                'strSubject = strSubject.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is Open ")

                strSubject = String.Format("Approvals System: {0} número {1} ha sido anulado.", tipoApp, codigo_viaje)

                'Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                Add_Emails_List("TO", set_ta_travel_roles_emails(vOrden, id_viaje, "0", campo, campo2))
            End If


            If Not ObJemail.SilentMode Then

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
                set_ta_DocumentosINFO()
                Dim email As String() = get_ta_DocumentosINFO("email", "id_documento", id_documento).Split(";")
                Dim boolExist As Boolean = False

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
            Else
                strSubject &= " (Silent MODE) "
            End If

            'Adding the subjects according the state
            ObJemail.AddSubject(strSubject)


        End Sub



        Public Sub Add_emailsRadicado(ByVal email As String, ByVal emailGerente As String, Optional codigo_radicado As String = "") 'According the state
            Set_Emails_List()
            Dim strSubject As String = ""
            Dim tipoApp = "Radicado "
            strSubject = String.Format("Approvals System: Radicado {0} rechazado.", codigo_radicado)
            Add_Emails_List("TO", email) 'Actual, Originator 0
            Add_Emails_List("TO", emailGerente) 'Next Step




            If Not ObJemail.SilentMode Then

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

            Else
                strSubject &= " (Silent MODE) "
            End If

            'Adding the subjects according the state
            ObJemail.AddSubject(strSubject)


        End Sub


        Public Sub Add_emailsAnticipos(ByVal idEstado As Integer, ByVal vOrden As Integer, Optional id_anticipo As Integer = 0, Optional tipo As Integer = 0) 'According the state

            Set_Emails_List() 'SE the Email List
            Dim strSubject As String = ""

            Dim tipoApp = ""
            Dim campo = ""
            Dim campo2 = ""
            Dim campo3 = ""
            If tipo = 1 Then
                tipoApp = "Solicitud de anticipo"
                campo = "id_usuario_app"
                campo2 = "id_usuario_radica"
                campo3 = "id_usuario_notificacion"
            ElseIf tipo = 2 Then
                tipoApp = "Dispersión de los fondos"
                campo = "id_usuario_app_legalizacion"
                campo2 = "id_usuario_radica"
                campo3 = "id_usuario_notificacion"
            ElseIf tipo = 3 Then
                tipoApp = "Legalización anticipo"
                campo = "id_usuario_app_informe"
                campo2 = "id_usuario_radica"
                campo3 = "id_usuario_notificacion"
            End If
            Select Case idEstado

                Case cOPEN

                    'strSubject = strSubject.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is Open ")

                    strSubject = String.Format("Approvals System: {0} número {1} enviado por aprobación.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual, Originator 0
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step


                    'Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_anticipo, campo))

                    'Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_viaje, campo)) 'Actual, Originator 0
                    'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual, Originator Group 0
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group
                    Add_Emails_List("TO", set_ta_GrupoRoles_emails(vOrden + 1)) 'Actual step Group 0

                Case cPENDING


                    'Approvals System: Notification of documents pending approval
                    'Approvals System: APPROVAL Process XXXX Is pending approval
                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is pending for approval ")
                    strSubject = String.Format("Approvals System: {0} número {1} enviado por aprobación.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    'Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_viaje, campo)) 'Actual, Originator 0
                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden))
                    'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step Group 0

                Case cAPPROVED


                    'Add a for loop to send email to all participated before!
                    Dim maxOrder As Integer = 0
                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    If vOrden > 0 Then 'Normal approval, the Approved status just come for order hier than 0
                        'Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_anticipo, campo))
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step
                        'Add_Emails_List("TO", set_ta_par_roles_emails((vOrden + 1), id_anticipo, campo))
                    Else 'StandBy Step, because order 0 genereta a approval status

                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step we dont need to add againg to the  Originator
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step, instead of that we need to add to the last order already created, to reconfirm the notification 
                        Dim clss_approval As APPROVAL.clss_approval
                        clss_approval = New APPROVAL.clss_approval(id_programa)
                        maxOrder = clss_approval.get_ta_AppDocumentoOrden_MAX(id_documento).Rows().Item(0).Item("orden") ' To get the Max ORder values to make the same step again
                        Add_Emails_List("TO", set_ta_roles_emails(maxOrder)) 'The last step who did the standby
                        'Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_anticipo, campo))
                        'Add_Emails_List("TO", set_ta_par_roles_emails(maxOrder, id_anticipo, campo))
                    End If

                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator
                    If vOrden > 0 Then 'Normal
                        'Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_anticipo, campo))
                        '' Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                        ''Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group
                        'Add_Emails_List("TO", set_ta_par_roles_emails((vOrden + 1), id_anticipo, campo))
                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is approved for next phase ")
                        strSubject = String.Format("Approvals System: Approval process {0} for next phase.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    Else 'StandBy Step

                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " Document approval for next phase. Observations completed ")
                        'Add_Emails_List("TO", set_ta_par_roles_emails(maxOrder, id_anticipo, campo))
                        ''Add_Emails_List("CC", set_ta_GrupoRoles_emails(maxOrder)) 'The last step group of whom did the standby
                        ''Add_Emails_List("CC", set_ta_GrupoRoles_emails(maxOrder + 1)) 'Next Step Group of last step group of whom did the standby
                        'Add_Emails_List("TO", set_ta_par_roles_emails((maxOrder + 1), id_anticipo, campo))
                        strSubject = String.Format("Approvals System: Approval process {0} for next phase. Observations completed.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    End If



                Case cnotAPPROVED
                    'Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_anticipo, campo))
                    'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups
                    Add_Emails_List("TO", set_ta_roles_emails(0))
                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is not approved, the approval document has finished ")
                    strSubject = String.Format("Approvals System: Approval process {0} has not been approved.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                Case cCANCELLED
                    Add_Emails_List("TO", set_ta_roles_emails(0))
                    'Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_anticipo, campo))
                    'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is not Cancelled, the approval document has finished ")
                    strSubject = String.Format("Approvals System: Approval process {0} has been cancelled", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                Case cSTANDby


                    'Add a for loop to send email to all participated before!
                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator

                    'Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                    'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is classified as Stand By ")
                    strSubject = String.Format("Approvals System: Approval process {0} has been classified as Stand By.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))


                Case cCOMPLETED

                    'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " has been completed ")
                    strSubject = String.Format("Approvals System: {0} {1} aprobado.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    'Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_anticipo, "0", campo2, campo3))


            End Select

            If Not ObJemail.SilentMode Then

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
                set_ta_DocumentosINFO()
                Dim email As String() = get_ta_DocumentosINFO("email", "id_documento", id_documento).Split(";")
                Dim boolExist As Boolean = False

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
            Else
                strSubject &= " (Silent MODE) "
            End If

            'Adding the subjects according the state
            ObJemail.AddSubject(strSubject)


        End Sub

        Public Sub Add_emailsPAR(ByVal idEstado As Integer, ByVal vOrden As Integer, Optional id_viaje As Integer = 0, Optional tipo As Integer = 0) 'According the state

            Set_Emails_List() 'SE the Email List
            Dim strSubject As String = ""

            Dim tipoApp = ""
            Dim campo = ""
            Dim campo2 = ""
            Dim campo3 = ""
            If tipo = 1 Then
                tipoApp = "Solicitud de par"
                campo = "id_usuario_app"
                campo2 = "id_usuario_radica"
                campo3 = "id_usuario_notificacion"
            ElseIf tipo = 2 Then
                tipoApp = "Facturación del PAR"
                campo = "id_usuario_app_legalizacion"
                campo2 = "id_usuario_radica"
                campo3 = "id_usuario_notificacion"
            ElseIf tipo = 3 Then
                tipoApp = "Informe de viaje"
                campo = "id_usuario_app_informe"
                campo2 = "id_usuario_radica"
                campo3 = "id_usuario_notificacion"
            End If
            Select Case idEstado

                Case cOPEN

                    'strSubject = strSubject.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is Open ")

                    strSubject = String.Format("Approvals System: {0} número {1} enviado por aprobación.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual, Originator 0
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step


                    Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_viaje, campo))

                    'Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_viaje, campo)) 'Actual, Originator 0
                    'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual, Originator Group 0
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group
                    Add_Emails_List("TO", set_ta_GrupoRoles_emails(vOrden + 1)) 'Actual step Group 0

                Case cPENDING


                    'Approvals System: Notification of documents pending approval
                    'Approvals System: APPROVAL Process XXXX Is pending approval
                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is pending for approval ")
                    strSubject = String.Format("Approvals System: {0} número {1} enviado por aprobación.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    'Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_viaje, campo)) 'Actual, Originator 0
                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden))
                    'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step Group 0

                Case cAPPROVED


                    'Add a for loop to send email to all participated before!
                    Dim maxOrder As Integer = 0
                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    If vOrden > 0 Then 'Normal approval, the Approved status just come for order hier than 0
                        Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_viaje, campo))
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step
                        Add_Emails_List("TO", set_ta_par_roles_emails((vOrden + 1), id_viaje, campo))
                    Else 'StandBy Step, because order 0 genereta a approval status

                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step we dont need to add againg to the  Originator
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step, instead of that we need to add to the last order already created, to reconfirm the notification 
                        Dim clss_approval As APPROVAL.clss_approval
                        clss_approval = New APPROVAL.clss_approval(id_programa)
                        maxOrder = clss_approval.get_ta_AppDocumentoOrden_MAX(id_documento).Rows().Item(0).Item("orden") ' To get the Max ORder values to make the same step again
                        Add_Emails_List("TO", set_ta_roles_emails(maxOrder)) 'The last step who did the standby
                        Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_viaje, campo))
                        Add_Emails_List("TO", set_ta_par_roles_emails(maxOrder, id_viaje, campo))
                    End If

                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator
                    If vOrden > 0 Then 'Normal
                        Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_viaje, campo))
                        ' Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group
                        Add_Emails_List("TO", set_ta_par_roles_emails((vOrden + 1), id_viaje, campo))
                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is approved for next phase ")
                        strSubject = String.Format("Approvals System: Approval process {0} for next phase.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    Else 'StandBy Step

                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " Document approval for next phase. Observations completed ")
                        Add_Emails_List("TO", set_ta_par_roles_emails(maxOrder, id_viaje, campo))
                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(maxOrder)) 'The last step group of whom did the standby
                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(maxOrder + 1)) 'Next Step Group of last step group of whom did the standby
                        Add_Emails_List("TO", set_ta_par_roles_emails((maxOrder + 1), id_viaje, campo))
                        strSubject = String.Format("Approvals System: Approval process {0} for next phase. Observations completed.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    End If



                Case cnotAPPROVED
                    Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_viaje, campo))
                    'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is not approved, the approval document has finished ")
                    strSubject = String.Format("Approvals System: Approval process {0} has not been approved.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                Case cCANCELLED
                    Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_viaje, campo))
                    'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is not Cancelled, the approval document has finished ")
                    strSubject = String.Format("Approvals System: Approval process {0} has been cancelled", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                Case cSTANDby


                    'Add a for loop to send email to all participated before!
                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator

                    'Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                    'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is classified as Stand By ")
                    strSubject = String.Format("Approvals System: Approval process {0} has been classified as Stand By.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))


                Case cCOMPLETED

                    'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " has been completed ")
                    strSubject = String.Format("Approvals System: {0} {1} aprobado.", tipoApp, get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    Add_Emails_List("TO", set_ta_par_roles_emails(vOrden, id_viaje, "0", campo2, campo3))


            End Select

            If Not ObJemail.SilentMode Then

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
                set_ta_DocumentosINFO()
                Dim email As String() = get_ta_DocumentosINFO("email", "id_documento", id_documento).Split(";")
                Dim boolExist As Boolean = False

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
            Else
                strSubject &= " (Silent MODE) "
            End If

            'Adding the subjects according the state
            ObJemail.AddSubject(strSubject)


        End Sub
        Public Sub Add_emails(ByVal idEstado As Integer, ByVal vOrden As Integer) 'According the state

            Set_Emails_List() 'SE the Email List
            Dim strSubject As String = ""


            Select Case idEstado

                Case cOPEN

                    'strSubject = strSubject.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is Open ")

                    strSubject = String.Format("Approvals System: Opening the approval process {0}.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual, Originator 0
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual, Originator Group 0
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group

                Case cPENDING


                    'Approvals System: Notification of documents pending approval
                    'Approvals System: APPROVAL Process XXXX Is pending approval
                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is pending for approval ")
                    strSubject = String.Format("Approvals System: APPROVAL Process {0} is pending for approval.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step Group 0

                Case cAPPROVED


                    'Add a for loop to send email to all participated before!
                    Dim maxOrder As Integer = 0
                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    If vOrden > 0 Then 'Normal approval, the Approved status just come for order hier than 0
                        Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                        Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step
                    Else 'StandBy Step, because order 0 genereta a approval status

                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step we dont need to add againg to the  Originator
                        'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step, instead of that we need to add to the last order already created, to reconfirm the notification 
                        Dim clss_approval As APPROVAL.clss_approval
                        clss_approval = New APPROVAL.clss_approval(id_programa)
                        maxOrder = clss_approval.get_ta_AppDocumentoOrden_MAX(id_documento).Rows().Item(0).Item("orden") ' To get the Max ORder values to make the same step again
                        Add_Emails_List("TO", set_ta_roles_emails(maxOrder)) 'The last step who did the standby

                    End If

                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator
                    If vOrden > 0 Then 'Normal

                        Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                        Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group
                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is approved for next phase ")
                        strSubject = String.Format("Approvals System: Approval process {0} for next phase.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    Else 'StandBy Step

                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " Document approval for next phase. Observations completed ")
                        Add_Emails_List("CC", set_ta_GrupoRoles_emails(maxOrder)) 'The last step group of whom did the standby
                        Add_Emails_List("CC", set_ta_GrupoRoles_emails(maxOrder + 1)) 'Next Step Group of last step group of whom did the standby
                        strSubject = String.Format("Approvals System: Approval process {0} for next phase. Observations completed.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    End If



                Case cnotAPPROVED

                    Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is not approved, the approval document has finished ")
                    strSubject = String.Format("Approvals System: Approval process {0} has not been approved.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                Case cCANCELLED

                    Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is not Cancelled, the approval document has finished ")
                    strSubject = String.Format("Approvals System: Approval process {0} has been cancelled", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                Case cSTANDby


                    'Add a for loop to send email to all participated before!

                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is classified as Stand By ")
                    strSubject = String.Format("Approvals System: Approval process {0} has been classified as Stand By.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))


                Case cCOMPLETED

                    Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " has been completed ")
                    strSubject = String.Format("Approvals System: Approval process {0} has been completed.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            End Select

            If Not ObJemail.SilentMode Then

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
                set_ta_DocumentosINFO()
                Dim email As String() = get_ta_DocumentosINFO("email", "id_documento", id_documento).Split(";")
                Dim boolExist As Boolean = False

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
            Else
                strSubject &= " (Silent MODE) "
            End If

            'Adding the subjects according the state
            ObJemail.AddSubject(strSubject)


        End Sub


        Public Sub Add_emailsTS(ByVal idEstado As Integer, ByVal vOrden As Integer, ByVal idTools As Integer, ByVal toolSubject As String) 'According the state

            Set_Emails_List() 'SE the Email List
            Dim strSubject As String = ""


            Select Case idEstado

                Case cOPEN

                    ' strSubject = String.Format("Time Sheet Approval Open For {0}, document {1}.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    strSubject = toolSubject.Replace("<!--##STATE_STEP##-->", "is Open.")

                    Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual, Originator 0
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual, Originator Group 0
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group

                Case cPENDING

                    'Approvals System: Notification of documents pending approval
                    'Approvals System: APPROVAL Process XXXX Is pending approval
                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is pending for approval ")
                    'strSubject = String.Format("Approvals System: APPROVAL Process {0} is pending for approval.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    '  strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} is pending for approval.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    strSubject = toolSubject.Replace("<!--##STATE_STEP##-->", "is pending for approval.")
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual
                   ' Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step Group 0

                Case cAPPROVED

                    'Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator take off, the originator
                    'If vOrden > 0 Then 'StandBy Step
                    '    Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                    'End If

                    Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator

                    If vOrden > 0 Then 'Normal
                        'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                        '  strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} for next phase.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                        strSubject = toolSubject.Replace("<!--##STATE_STEP##-->", "for next phase.")
                    Else 'StandBy Step
                        ' strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} for next phase. Observations completed.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                        strSubject = toolSubject.Replace("<!--##STATE_STEP##-->", "for next phase. Observations completed.")
                    End If

                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group

                Case cnotAPPROVED

                    'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    Add_Emails_List("CC", get_t_notification_emails("RHHMNG")) 'Infom to the Manager

                    ' strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} has not been approved.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    strSubject = toolSubject.Replace("<!--##STATE_STEP##-->", "has not been approved.")
                Case cCANCELLED

                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    Add_Emails_List("CC", get_t_notification_emails("RHHMNG")) 'Infom to the Manager

                    'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    '  strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} has been cancelled.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    strSubject = toolSubject.Replace("<!--##STATE_STEP##-->", "has been cancelled.")
                Case cSTANDby

                    'Add a for loop to send email to all participated before!
                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    Add_Emails_List("CC", get_t_notification_emails("RHHMNG")) 'Infom to the Manager

                    'Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                    'Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group

                    ' strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} has been classified as Stand By.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    strSubject = toolSubject.Replace("<!--##STATE_STEP##-->", "has been classified as Stand By.")
                Case cCOMPLETED

                    'Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    'Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups
                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    Add_Emails_List("CC", get_t_notification_emails("RHHMNG")) 'Infom to the Manager

                    ' strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} has been completed.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    strSubject = toolSubject.Replace("<!--##STATE_STEP##-->", "has been completed.")
            End Select

            If Not ObJemail.SilentMode Then

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
                set_ta_DocumentosINFO()
                Dim email As String() = get_ta_DocumentosINFO("email", "id_documento", id_documento).Split(";")
                Dim boolExist As Boolean = False

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
            Else
                strSubject &= " (Silent MODE) "
            End If

            'Adding the subjects according the state
            ObJemail.AddSubject(strSubject)


        End Sub



        Public Function Add_emailsDELV(ByVal idEstado As Integer, ByVal vOrden As Integer, ByVal toolSubject As String) As String

            Set_Emails_List() 'SE the Email List
            'Dim strSubject As String = ""


            Select Case idEstado

                Case cOPEN

                    'strSubject = strSubject.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is Open ")
                    'strSubject = String.Format("Time Sheet Approval Open For {0}, document {1}.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    toolSubject &= ", is opening the approval process."

                    Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual, Originator 0
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual, Originator Group 0
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group

                Case cPENDING

                    'Approvals System: Notification of documents pending approval
                    'Approvals System: APPROVAL Process XXXX Is pending approval
                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is pending for approval ")
                    'strSubject = String.Format("Approvals System: APPROVAL Process {0} is pending for approval.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    'strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} is pending for approval.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    toolSubject &= ", is pending for approval."

                    Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step Group 0

                Case cAPPROVED

                    'Add a for loop to send email to all participated before!

                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    If vOrden > 0 Then 'StandBy Step
                        Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                    End If
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator

                    If vOrden > 0 Then 'Normal

                        Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is approved for next phase ")
                        'strSubject = String.Format("Approvals System: Approval process {0} for next phase.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                        'strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} for next phase.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                        toolSubject &= ", for next phase."

                    Else 'StandBy Step

                        'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " Document approval for next phase. Observations completed ")
                        'strSubject = String.Format("Approvals System: Approval process {0} for next phase. Observations completed.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                        ' strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} for next phase. Observations completed.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                        toolSubject &= ", for next phase. Observations completed."

                    End If

                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group
                '

                Case cnotAPPROVED

                    Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is not approved, the approval document has finished ")
                    'strSubject = String.Format("Approvals System: Approval process {0} has not been approved.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    'strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} has not been approved.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    toolSubject &= ", has not been approved."


                Case cCANCELLED

                    Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is not Cancelled, the approval document has finished ")
                    'strSubject = String.Format("Approvals System: Approval process {0} has been cancelled", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    ' strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} has been cancelled.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    toolSubject &= ", has been cancelled."


                Case cSTANDby

                    'Add a for loop to send email to all participated before!

                    Add_Emails_List("TO", set_ta_roles_emails(0)) 'Originator
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'Actual step
                    Add_Emails_List("TO", set_ta_roles_emails(vOrden + 1)) 'Next Step

                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(0)) 'Originator
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'Actual step
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden + 1)) 'Next Step Group

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " is classified as Stand By ")
                    'strSubject = String.Format("Approvals System: Approval process {0} has been classified as Stand By.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    ' strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} has been classified as Stand By.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    toolSubject &= ", has been classified as Stand By."

                Case cCOMPLETED

                    Add_Emails_List("TO", set_ta_roles_emails(-1)) 'included all
                    Add_Emails_List("CC", set_ta_GrupoRoles_emails(-1)) 'included all groups

                    'strSubject = strSubject.Replace("<!--##STATE_STEP##-->", " has been completed ")
                    'strSubject = String.Format("Approvals System: Approval process {0} has been completed.", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                    'strSubject = String.Format("Time Sheet Approval For {0}, Approval process {1} has been completed.", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

                    toolSubject &= ", has been completed."

            End Select

            If Not ObJemail.SilentMode Then

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
                set_ta_DocumentosINFO()
                Dim email As String() = get_ta_DocumentosINFO("email", "id_documento", id_documento).Split(";")
                Dim boolExist As Boolean = False

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


                Add_emailsDELV = ""

            Else

                Dim strTO As String = ""
                Dim strCC As String = ""

                '**************ADDING THE EMAILS FROM THE UNIQUE LIST*********************
                For Each dtRow In tbl_emails.Rows

                    Select Case dtRow("Tipo")
                        Case "TO"
                            strTO &= String.Format("{0};", dtRow("email").ToString.Trim)
                        Case "CC"
                            strCC &= String.Format("{0};", dtRow("email").ToString.Trim)
                            'Case "BCC"
                            '    ObJemail.AddBcc(dtRow("email").ToString.Trim)
                    End Select

                Next
                '**************ADDING THE EMAILS FROM THE UNIQUE LIST*********************



                '**************************************Independent Email list By Document ***************************************
                set_ta_DocumentosINFO()
                Dim email As String() = get_ta_DocumentosINFO("email", "id_documento", id_documento).Split(";")
                Dim boolExist As Boolean = False

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
                                strCC &= String.Format("{0};", dtEmail.Trim)
                            Else
                                boolExist = False
                            End If


                        End If
                    End If

                Next
                '**************************************Independent Email list By Document***************************************

                Add_emailsDELV = "TO: " & strTO & "<br/>" & "CC: " & strCC
                toolSubject &= " (Silent MODE) "

            End If

            'Adding the subjects according the state
            ObJemail.AddSubject(toolSubject)


        End Function


        '*****************************************************************************************************************
        '********************************************TIME SHEET MESSAGE **************************************************
        '*****************************************************************************************************************

        Public Function Emailing_TIME_SHEET_APPROVAL(Optional id_AppDoc As Integer = 0) As Boolean

            Dim strMess As String
            id_AppDocumento = IIf(id_AppDoc = 0, id_AppDocumento, id_AppDoc)

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
            set_Apps_Document() 'Set all AppDoc
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            '*****************ADDING THE SUBJECTS**********************

            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->

            'Change this part temporaly
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

            strSubject = strSubject.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            ' strSubject = strSubject.Replace("<!--##STATE_STEP##-->", get_Apps_DocumentField("descripcion_estado", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strSubject = strSubject.Replace("<!--##USER_NAME##-->", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'ObJemail.AddSubject(strSubject) Move to Email find

            Dim idTool As Integer = get_Apps_DocumentField("id_approval_tool", "id_documento", id_documento, "id_App_documento", id_AppDocumento)


            ' --*****************************************************************************************************************************************************

            Using db As New dbRMS_JIEntities

                Dim idDoc As Integer = get_Apps_DocumentField("id_documento", "id_documento", id_documento, "id_App_documento", id_AppDocumento)
                Dim idTimeSheet As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(get_Apps_DocumentField("id_programa", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
                Dim timesheet As vw_ta_timesheet = cls_TimeSheet.getTimeSheet(idTimeSheet)

                Dim DaysInMonth As Integer = Date.DaysInMonth(timesheet.anio, timesheet.mes)

                Dim strLeave As String = ""
                If timesheet.ts_leave_update Then

                    strSubject = strSubject.Replace("Time Sheet", "Leave")
                    'strLeave = cls_TimeSheet.annual_leave_table(timesheet.id_usuario)

                End If

                Dim strTimeSheetTable As String = cls_TimeSheet.get_TimeSheetTableHTML(timesheet.anio, timesheet.mes, idTimeSheet, timesheet.id_employee_type, timesheet.ts_leave_update) 'The Original Template
                strMess = strMess.Replace("<!--##TIMESHEET_TABLE##-->", strLeave & "<br>" & strTimeSheetTable) 'this is the template

            End Using

            ' --*****************************************************************************************************************************************************


            '*****************FINDING the destinataries **************************************
            'ObJemail.AddTo("grivera@colombiaresponde-ns.org") 'Just for testing

            If idTool <= 1 Then
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            ElseIf idTool = 2 Then 'TimeSheet or Tool
                Add_emailsTS(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), idTool, strSubject) 'Adding the respective email according the state of the approval proccess
            Else
                Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            End If

            'ObJemail.AddBcc("grivera@colombiaresponde-ns.org") 'Just for testing
            '*****************FINDING the destinataries **************************************

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


            '**************At this part we neeed to start to replace the Documetn info for this email*******************

            '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_Apps_DocumentField("nombre_programa", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DESCRIPTION_CAT##-->", get_Apps_DocumentField("descripcion_cat", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DESCRIPTION_APPROVAL##-->", get_Apps_DocumentField("descripcion_aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##BENEFICIARY_NAME##-->", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DOCUMENT_DESCRIPTION##-->", get_Apps_DocumentField("descripcion_doc", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'strMess = strMess.Replace("<!--##CODE_PROCESS##-->", get_Apps_DocumentField("codigo_SAP_APP", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            strMess = strMess.Replace("<!--##APPROVAL_CODE##-->", get_Apps_DocumentField("codigo_Approval", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##USER_NAME##-->", get_Apps_DocumentField("nombre_usuario_app", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##NEXT_FASE##-->", get_Apps_DocumentField("nextFase", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento)), "f")
            'strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'dateUtil.set_DateFormat(CDate( ), "f")
            strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", vFecha)

            strMess = strMess.Replace("<!--##ID_DOC##-->", id_documento)
            strMess = strMess.Replace("<!--##COMMENTS##-->", get_Apps_DocumentField("observacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            strMess = strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

            Dim strTablePath As String = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1) 'getting the second Element to fill
            Dim strALL_Rows As String = ""
            Dim strRow As String = ""

            Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>

            set_Ta_RutaSeguimiento() 'getting the complete route

            For Each dtRow In Tbl_Ta_RutaSeguimiento.Rows

                strRow = get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 1)

                strRow = strRow.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                strRow = strRow.Replace("<!--##EMPLOYEE_NAME##-->", dtRow("nombre_empleado"))
                strRow = strRow.Replace("<!--##STATE##-->", dtRow("descripcion_estado"))
                strRow = strRow.Replace("<!--##DATE_RECEIPT##-->", dtRow("fecha_aprobacion"))
                strRow = strRow.Replace("<!--##ALERT_TYPE##-->", dtRow("Alerta"))

                'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                If id_AppDocumento = dtRow("id_app_documento") Then
                    strRow = strRow.Replace("<tr>", strStyle)
                End If

                strALL_Rows &= strRow

            Next

            strTablePath = strTablePath.Replace("<!--##CONTENT##-->", strALL_Rows)

            strMess = strMess.Replace("<!--##STEP_TABLE##-->", strTablePath) 'this would be by template
            '**************At this part we neeed to start to replace the Document info for this email*******************
            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************



            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            '********************Find the LogProgram if doesn´t exist nto include**************************

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
            End If

            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            '********************Find the LogProgram if doesn´t exist nto include**************************

            ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_TIME_SHEET_APPROVAL = True
            Else
                Emailing_TIME_SHEET_APPROVAL = False
            End If

        End Function



        Public Function get_t_notification_emails(ByVal strCode As String) As DataTable

            Dim tbl_emails As DataTable
            Dim strSql As String = String.Format("select a.id_notification_emails,
			                                             a.noti_email_code,
			                                             b.email_usuario as email,
			                                             b.id_usuario from t_notification_emails  a
	                                               inner join t_usuarios b on (a.id_usuario = b.id_usuario)
	                                               where a.id_programa = {1} and a.noti_email_code = '{0}'", strCode, id_programa)

            tbl_emails = cl_utl.setObjeto("t_notification_emails", "id_notification_emails", 0, strSql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (tbl_emails.Rows.Count = 1 And tbl_emails.Rows.Item(0).Item("id_notification_emails") = 0) Then
                tbl_emails.Rows.Remove(tbl_emails.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_t_notification_emails = tbl_emails

        End Function

        Public Function get_t_notification_emails_end_app(ByVal id_documento As Integer) As DataTable

            Dim tbl_emails As DataTable
            Dim strSql As String = String.Format("select email from ta_tipoDocumento a
                                            inner join ta_documento b on a.id_tipoDocumento = b.id_tipoDocumento
                                            where b.id_documento = {0}", id_documento)

            tbl_emails = cl_utl.setObjeto("t_notification_emails", "id_notification_emails", 0, strSql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            'If (tbl_emails.Rows.Count = 1 And tbl_emails.Rows.Item(0).Item("email") = 0) Then
            '    tbl_emails.Rows.Remove(tbl_emails.Rows.Item(0))
            'End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

            get_t_notification_emails_end_app = tbl_emails

        End Function


        '*****************************************************************************************************************
        '********************************************DELIVERABLE MESSAGE **************************************************
        '*****************************************************************************************************************

        Public Function Emailing_DELIVERABLE_APPROVAL(Optional id_AppDoc As Integer = 0) As Boolean

            Dim strMess As String
            id_AppDocumento = IIf(id_AppDoc = 0, id_AppDocumento, id_AppDoc)

            '************************************SYSTEM INFO********************************************
            Dim cProgram As New RMS.cls_Program
            cProgram.get_Sys(0, True)
            cProgram.get_Programs(id_programa, True)
            timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", id_programa))
            dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
            '************************************SYSTEM INFO********************************************

            Dim regionalizacionCulture = New CultureInfo(cProgram.getprogramField("codigo_regionalizacion", "id_programa", id_programa))
            Dim strLocalCurrency As String = ""
            If Not String.IsNullOrEmpty(cProgram.getprogramField("currency_symbol", "id_programa", id_programa)) Then
                strLocalCurrency = cProgram.getprogramField("currency_symbol", "id_programa", id_programa)
            Else
                strLocalCurrency = regionalizacionCulture.NumberFormat.CurrencySymbol
            End If

            '*****************Creating the email Object**************************************
            ObJemail = New UTILS.cls_email(id_programa)
            '*****************Creating the email Object**************************************

            '*****************building the email from the templates objects**************************************

            set_t_notification(id_notification) 'Create the Noti_Object
            set_t_Template() 'Set Template object from the notification id
            set_Apps_Document() 'Set all AppDoc
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            '*************************************************************************************************************************************************
            '********************************************************DELIVERABLE SIDE*************************************************************************
            '*************************************************************************************************************************************************
            Dim cls_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

            Dim tbl_Deliverable As DataTable = cls_Deliverable.Deliv_Document(0, id_documento)
            Dim tbl_Deliverable_detail As DataTable = cls_Deliverable.Deliv_Document_detail(tbl_Deliverable.Rows.Item(0).Item("id_deliverable"))

            '*************************************************************************************************************************************************
            '********************************************************DELIVERABLE SIDE*************************************************************************
            '*************************************************************************************************************************************************

            '*****************ADDING THE SUBJECTS**********************
            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->

            'Change this part temporaly
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

            'Deliverable #<!--##DELIVERABLE_NUMBER##--> <!--##DELIVERABLE_IMPLEMENTER##--> (<!--##DELIVERABLE_ACTIVITY-->), Due Date: <!--##DELIVERABLE_DUE_DATE##-->

            strSubject = strSubject.Replace("<!--##DELIVERABLE_NUMBER##-->", tbl_Deliverable_detail.Rows.Item(0).Item("numero_entregable"))
            strSubject = strSubject.Replace("<!--##DELIVERABLE_IMPLEMENTER##-->", tbl_Deliverable_detail.Rows.Item(0).Item("implementer"))
            strSubject = strSubject.Replace("<!--##DELIVERABLE_ACTIVITY-->", tbl_Deliverable_detail.Rows.Item(0).Item("codigo_SAPME"))
            strSubject = strSubject.Replace("<!--##DELIVERABLE_DUE_DATE##-->", String.Format("{0:m}", tbl_Deliverable_detail.Rows.Item(0).Item("DueDate")))

            'ObJemail.AddSubject(strSubject) Move to Email find


            'Dim idTool As Integer = get_Apps_DocumentField("id_approval_tool", "id_documento", id_documento, "id_App_documento", id_AppDocumento)
            '*****************FINDING the destinataries **************************************
            'ObJemail.AddTo("grivera@colombiaresponde-ns.org") 'Just for testing
            'If idTool <= 1 Then
            'Add_emails(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento)) 'Adding the respective email according the state of the approval proccess
            'ElseIf idTool = 2 Then 'TimeSheet or Tool

            Dim strEmail As String = Add_emailsDELV(get_Apps_DocumentField("id_estadoDoc", "id_documento", id_documento, "id_App_documento", id_AppDocumento), get_Apps_DocumentField("orden", "id_documento", id_documento, "id_App_documento", id_AppDocumento), strSubject) 'Adding the respective email according the state of the approval proccess
            'End If

            'ObJemail.AddBcc("grivera@colombiaresponde-ns.org") 'Just for testing
            '*****************FINDING the destinataries **************************************

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


            '**************At this part we neeed to start to replace the Documetn info for this email*******************

            '**************At this part we neeed to start to replace the Document info for this email*******************
            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
            '************************* THE message it supposed Is here, need to proceed to test*****************


            '<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##CODE_PROCESS##-->;<!--##APPROVAL_CODE##-->;<!--##USER_NAME##-->;<!--##NEXT_FASE##-->;<!--##DATE_RECEIPT##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_Apps_DocumentField("nombre_programa", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DESCRIPTION_CAT##-->", get_Apps_DocumentField("descripcion_cat", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DESCRIPTION_APPROVAL##-->", get_Apps_DocumentField("descripcion_aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##NUMBER_INSTRUMENT##-->", get_Apps_DocumentField("numero_instrumento", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##BENEFICIARY_NAME##-->", get_Apps_DocumentField("nom_beneficiario", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##DOCUMENT_DESCRIPTION##-->", get_Apps_DocumentField("descripcion_doc", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            'strMess = strMess.Replace("<!--##CODE_PROCESS##-->", get_Apps_DocumentField("codigo_SAP_APP", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            strMess = strMess.Replace("<!--##APPROVAL_CODE##-->", get_Apps_DocumentField("codigo_Approval", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##USER_NAME##-->", get_Apps_DocumentField("nombre_usuario_app", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##NEXT_FASE##-->", get_Apps_DocumentField("nextFase", "id_documento", id_documento, "id_App_documento", id_AppDocumento))

            Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento)), "f")
            'strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            'dateUtil.set_DateFormat(CDate( ), "f")
            strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", vFecha)

            strMess = strMess.Replace("<!--##ID_DOC##-->", id_documento)
            strMess = strMess.Replace("<!--##COMMENTS##-->", get_Apps_DocumentField("observacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

            strMess = strMess.Replace("<!--##TABLE_STATUS##-->", get_StatusTable(1)) 'this would be by template

            '***********************************************************************************************
            '********************************DELIVERABLE RESULT TABLE***************************************
            '***********************************************************************************************

            strMess = strMess.Replace("<!--##DELIVERABLE_RESULT_TABLE-->", get_ResultTable(tbl_Deliverable.Rows.Item(0).Item("id_deliverable"), 2)) 'this would be by template

            '***********************************************************************************************
            '********************************DELIVERABLE RESULT TABLE***************************************
            '***********************************************************************************************


            '***********************************************************************************************
            '********************************DELIVERABLE DETAIL*********************************************
            '***********************************************************************************************

            '<!--##DELIVERABLE_LAPSED_TIME##-->
            '<!--##DELIVERABLE_TIME_ALERT##-->

            'Func_Unit(Eval("D_Days")) form delayed time
            'Func_Alert(Eval("porc_Days"), Eval("porc_EDays"), 1) Alert label
            'Func_Alert(Eval("porc_Days"), Eval("porc_EDays"), 2) Alert type 2

            strMess = strMess.Replace("<!--##DELIVERABLE_NUMBER##-->", tbl_Deliverable_detail.Rows.Item(0).Item("numero_entregable"))
            strMess = strMess.Replace("<!--##DELIVERABLE_AMOUNT##-->", String.Format("{0:N2} {1}", tbl_Deliverable_detail.Rows.Item(0).Item("valor"), strLocalCurrency))
            strMess = strMess.Replace("<!--##DELIVERABLE_AMOUNT_USD##-->", String.Format("{0:N2} USD", Math.Round((tbl_Deliverable_detail.Rows.Item(0).Item("valor") / tbl_Deliverable_detail.Rows.Item(0).Item("tasa_cambio")), 2, MidpointRounding.AwayFromZero)))
            strMess = strMess.Replace("<!--##DELIVERABLE_PORCENT##-->", String.Format("{0:N2}", tbl_Deliverable_detail.Rows.Item(0).Item("porcentaje")))
            strMess = strMess.Replace("<!--##DELIVERABLE_DUE_DATE##-->", String.Format("{0:m}", tbl_Deliverable_detail.Rows.Item(0).Item("DueDate")))

            strMess = strMess.Replace("<!--##DELIVERABLE_TIME_ALERT##-->", Func_Alert(tbl_Deliverable_detail.Rows.Item(0).Item("porc_Days"), tbl_Deliverable_detail.Rows.Item(0).Item("porc_EDays"), 1))
            strMess = strMess.Replace("<!--##DELIVERABLE_STATUS##-->", Func_Alert(tbl_Deliverable_detail.Rows.Item(0).Item("porc_Days"), tbl_Deliverable_detail.Rows.Item(0).Item("porc_EDays"), 2))

            strMess = strMess.Replace("<!--##DELIVERABLE_LAPSED_TIME##-->", Func_Unit(tbl_Deliverable_detail.Rows.Item(0).Item("D_Days")))
            strMess = strMess.Replace("<!--##DELIVERABLE_DESCRIPTION-->", tbl_Deliverable_detail.Rows.Item(0).Item("descripcion_entregable"))
            strMess = strMess.Replace("<!--##DELIVERABLE_APP_DESCRIPTION-->", tbl_Deliverable_detail.Rows.Item(0).Item("description"))
            strMess = strMess.Replace("<!--##DELIVERABLE_APP_NOTES-->", tbl_Deliverable_detail.Rows.Item(0).Item("notes"))


            '***********************************************************************************************
            '********************************DELIVERABLE DETAIL*********************************************
            '***********************************************************************************************


            '***********************************************************************************************
            '********************************DELIVERABLE SUPPORT DOCS***************************************
            '***********************************************************************************************

            strMess = strEmail & strMess.Replace("<!--##DELIVERABLE_SUPPORT_DOCS-->", get_Docs_Table(tbl_Deliverable.Rows.Item(0).Item("id_deliverable"), 3)) 'this would be by template

            '***********************************************************************************************
            '********************************DELIVERABLE SUPPORT DOCS***************************************
            '***********************************************************************************************

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            '********************Find the LogProgram if doesn´t exist nto include**************************

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
            End If

            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            '********************Find the LogProgram if doesn´t exist nto include**************************

            'ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Red_Time_3.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Blue_Time_3.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Orange_Time_3.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Cyan_Time_3.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_DELIVERABLE_APPROVAL = True
            Else
                Emailing_DELIVERABLE_APPROVAL = False
            End If

        End Function


        Function Func_Alert(ByVal porcDays As Double, ByVal porcEDays As Double, ByVal alertType As Integer) As String

            Dim Dif_Porce As Double = porcDays - porcEDays
            Dim porc_Progress As Double = If(porcEDays <> 0, (Dif_Porce / porcEDays), 0)

            Const c_label_danger As String = "#dd4b39 !important" '"label-danger"
            Const c_label_warning As String = "#f39c12 !important" '"label-warning"
            Const c_label_primary As String = "#3c8dbc !important" '"label-primary"
            Const c_label_success As String = "#00a65a !important" '"label-Success"

            'Const c_progress_bar_warning = "progress-bar-warning"
            'Const c_progress_bar_primary = "progress-bar-primary"
            'Const c_progress_bar_danger = "progress-bar-danger"

            Dim strResult As String = ""
            Dim strStatus As String = ""
            Dim strAlertBar1 As String = ""
            Dim strAlertBar2 As String = ""

            If porc_Progress >= 0 Then

                'Inverter number
                If ((1 - porc_Progress) * 100) >= 90 Then
                    strResult = c_label_danger
                ElseIf ((1 - porc_Progress) * 100) >= 60 And ((1 - porc_Progress) * 100) < 90 Then
                    strResult = c_label_warning
                ElseIf ((1 - porc_Progress) * 100) >= 30 And ((1 - porc_Progress) * 100) < 60 Then
                    strResult = c_label_primary
                Else
                    strResult = c_label_success
                End If

                strStatus = "Pending"
                'strAlertBar2 = c_progress_bar_primary

            Else 'Expired
                strResult = c_label_danger
                strStatus = "Expired"
                'strAlertBar2 = c_progress_bar_danger
            End If


            'If porc_Progress >= 0 Then

            '    'Inverter number
            '    If ((1 - porc_Progress) * 100) >= 90 Then
            '        strResult = c_label_danger
            '    ElseIf ((1 - porc_Progress) * 100) >= 60 And ((1 - porc_Progress) * 100) < 90 Then
            '        strResult = c_label_warning
            '    ElseIf ((1 - porc_Progress) * 100) >= 30 And ((1 - porc_Progress) * 100) < 60 Then
            '        strResult = c_label_primary
            '    Else
            '        strResult = c_label_success
            '    End If

            '    strStatus = "Pending"
            '    strAlertBar2 = c_progress_bar_primary

            'Else 'Expired
            '    strResult = c_label_danger
            '    strStatus = "Expired"
            '    strAlertBar2 = c_progress_bar_danger
            'End If

            'strAlertBar1 = c_progress_bar_warning
            'If alertType = 1 Then
            '    Func_Alert = strResult
            'ElseIf alertType = 2 Then
            'Func_Alert = strStatus
            'ElseIf alertType = 3 Then
            '    Func_Alert = strAlertBar1
            'Else
            '    Func_Alert = strAlertBar2
            'End If

            If alertType = 1 Then
                Func_Alert = strResult
            ElseIf alertType = 2 Then
                Func_Alert = strStatus
            End If

        End Function


        Function Func_Unit(ByVal Ndays As String) As String

            Dim vDays As Double
            Dim vWeeks As Double
            Dim vMonths As Double
            Dim vYear As Double

            Dim strUnit As String
            Dim vUnit As Double

            vDays = Ndays
            vWeeks = vDays / 7
            vMonths = vDays / 30
            vYear = vDays / 365

            If vWeeks < 1 Then

                vUnit = Math.Round(vDays, 2, MidpointRounding.AwayFromZero)

                If vDays > 1 Then
                    strUnit = " days"
                Else
                    strUnit = " day"
                End If

            ElseIf vMonths < 1 Then

                vUnit = Math.Round(vWeeks, 2, MidpointRounding.AwayFromZero)

                If vWeeks > 1 Then
                    strUnit = " weeks"
                Else
                    strUnit = " week"
                End If

            ElseIf vYear < 1 Then

                vUnit = Math.Round(vMonths, 2, MidpointRounding.AwayFromZero)

                If vMonths > 1 Then
                    strUnit = " months"
                Else
                    strUnit = " month"
                End If

            Else

                vUnit = Math.Round(vYear, 2, MidpointRounding.AwayFromZero)

                If vYear > 1 Then
                    strUnit = " years"
                Else
                    strUnit = " year"
                End If

            End If

            Func_Unit = String.Format("{0}&nbsp;{1}", vUnit, strUnit)

        End Function

        Public Function get_StatusTable(vOrder As Integer) As String


            '**************************************************************************************************************************
            '************************************STATUS TABLE**************************************************************************
            '**************************************************************************************************************************

            'We need to get the current ID  ruta

            '*****************<!--##TABLE_STATUS##-->'*****************
            set_Ta_RutaSeguimiento() 'getting the complete route Of the current Document
            Dim tbl_document_rep As DataTable = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_documento", id_documento)

            Dim NDays As Decimal
            Dim NHours As Decimal
            Dim SHours As Decimal

            Dim strStyle As String = "<tr style='background-color:#ee7108;border:1 dotted #FF0000; ' >" 'Putting inside <tr>
            'style="background-color:#ee7108;border:1 dotted #FF0000;"
            Dim strTableContent As StringBuilder = New StringBuilder()
            Dim strTable_Cont As StringBuilder = New StringBuilder()
            'Dim strTableContentALL As StringBuilder = New StringBuilder()
            Dim strTable As StringBuilder = New StringBuilder()

            '<!--##CONTENT##-->;<!--##ROLE_NAME##-->;<!--##EMPLOYEE_NAME##-->;<!--##STATE##-->;<!--##DATE_RECEIPT##-->;<!--##ALERT_TYPE##-->;<!--##STRONG_OPEN##-->;<!--##STRONG_OPEN##-->;
            strTable.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", vOrder))

            SHours = 0
            For Each seg_dtRow In Tbl_Ta_RutaSeguimiento.Rows

                strTable_Cont.Clear()
                strTable_Cont.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", vOrder))

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
                    '  strTable_Cont.Replace("background-color:rgba(0, 0, 0, 0);", "background-color:rgb(238, 113, 8);")

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

            '**************************************************************************************************************************
            '************************************STATUS TABLE**************************************************************************
            '**************************************************************************************************************************

            get_StatusTable = strTable.ToString


        End Function



        Public Function get_ResultTable(ByVal idDeliverable As Integer, ByVal vOrder As Integer) As String

            '***********************************************************************************************
            '********************************DELIVERABLE RESULT TABLE***************************************
            '***********************************************************************************************

            '*****************<!--##DELIVERABLE_RESULT_CONTENT-->'*****************
            Dim cls_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
            Dim tbl_reported As DataTable = cls_Deliverable.get_Deliverable_Reported(idDeliverable)

            Dim strTableContent As StringBuilder = New StringBuilder()
            Dim strTable_Cont As StringBuilder = New StringBuilder()
            Dim strTable As StringBuilder = New StringBuilder()

            '<!--##DELIVERABLE_RESULT_CONTENT-->

            '<!--##DELIVERABLE_RESULT_INDICATOR_CODE-->
            '<!--##DELIVERABLE_RESULT_INDICATOR_NAME-->
            '<!--##DELIVERABLE_RESULT_INDICATOR_TARGUET-->
            '<!--##DELIVERABLE_RESULT_INDICATOR_REPORTED-->
            '<!--##DELIVERABLE_RESULT_REPORTED-->
            '<!--##DELIVERABLE_RESULT_INDICATOR_REPORTED_PORC-->
            '<!--##DELIVERABLE_RESULT_INDICATOR_REPORTED_ALERT-->

            '<!--##DELIVERABLE_RESULT_REPORTED_PROC-->
            '<!--##DELIVERABLE_RESULT_REPORTED_ALERT-->

            strTable.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", vOrder))

            For Each dtRow In tbl_reported.Rows

                strTable_Cont.Clear()
                strTable_Cont.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", vOrder))

                strTable_Cont.Replace("<!--##DELIVERABLE_RESULT_INDICATOR_CODE-->", dtRow("codigo_indicador"))
                strTable_Cont.Replace("<!--##DELIVERABLE_RESULT_INDICATOR_NAME-->", dtRow("nombre_indicador_LB"))
                strTable_Cont.Replace("<!--##DELIVERABLE_RESULT_INDICATOR_TARGUET-->", dtRow("meta_total"))
                strTable_Cont.Replace("<!--##DELIVERABLE_RESULT_INDICATOR_REPORTED-->", dtRow("valor_avance"))
                strTable_Cont.Replace("<!--##DELIVERABLE_RESULT_REPORTED-->", dtRow("del_res_value"))
                strTable_Cont.Replace("<!--##DELIVERABLE_RESULT_INDICATOR_REPORTED_PORC-->", dtRow("porc_INDreported"))
                strTable_Cont.Replace("<!--##DELIVERABLE_RESULT_INDICATOR_REPORTED_ALERT-->", get_alert_ind(dtRow("porc_INDreported"))) 'Just for getting the color alert selected

                strTable_Cont.Append(get_t_templateField("tp_nwline", "id_notification", id_notification, "tp_orden", vOrder))

                strTable_Cont.Replace("<!--##DELIVERABLE_RESULT_REPORTED_PROC-->", dtRow("porc_DELIVreported"))
                strTable_Cont.Replace("<!--##DELIVERABLE_RESULT_REPORTED_ALERT-->", get_alert_ind(dtRow("porc_DELIVreported"))) 'Just for getting the color alert selected

                strTableContent.Append(strTable_Cont.ToString)

            Next

            strTable.Replace("<!--##DELIVERABLE_RESULT_CONTENT-->", strTableContent.ToString)

            '***********************************************************************************************
            '********************************DELIVERABLE RESULT TABLE***************************************
            '***********************************************************************************************

            get_ResultTable = strTable.ToString

        End Function


        '<!--##DELIVERABLE_SUPPORT_DOCS_CONTENT-->

        Public Function get_Docs_Table(ByVal idDeliverable As Integer, ByVal vOrder As Integer) As String

            '***********************************************************************************************
            '********************************DELIVERABLE SUPPORT DOCS***************************************
            '***********************************************************************************************

            '************************************SYSTEM INFO********************************************
            Dim cProgram As New RMS.cls_Program
            cProgram.get_Sys(0, True)
            cProgram.get_Programs(id_programa, True)
            '************************************SYSTEM INFO********************************************


            '*****************<!--##DELIVERABLE_RESULT_CONTENT-->'*****************
            Dim cls_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
            Dim tbl_Suppor_docs As DataTable = cls_Deliverable.Deliv_Support_Documents_detail(idDeliverable)

            Dim strTableContent As StringBuilder = New StringBuilder()
            Dim strTable_Cont As StringBuilder = New StringBuilder()
            Dim strTable As StringBuilder = New StringBuilder()

            '<!--##DELIVERABLE_SUPPORT_DOCS_NUMBER-->
            '<!--##DELIVERABLE_SUPPORT_DOCS_TYPE-->
            '<!--##SYS_PATH##-->
            '<!--##ID_DOC##-->
            '<!--##DELIVERABLE_SUPPORT_DOCS_FILE-->
            '<!--##DELIVERABLE_SUPPORT_DOCS_VER-->

            strTable.Append(get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", vOrder))

            For Each dtRow In tbl_Suppor_docs.Rows

                strTable_Cont.Clear()
                strTable_Cont.Append(get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", vOrder))

                strTable_Cont.Replace("<!--##DELIVERABLE_SUPPORT_DOCS_NUMBER-->", dtRow("no"))
                strTable_Cont.Replace("<!--##DELIVERABLE_SUPPORT_DOCS_TYPE-->", dtRow("nombre_documento"))
                strTable_Cont.Replace("<!--##ID_DOC##-->", id_documento)
                strTable_Cont.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys))

                strTable_Cont.Replace("<!--##DELIVERABLE_SUPPORT_DOCS_FILE-->", dtRow("archivo"))
                strTable_Cont.Replace("<!--##DELIVERABLE_SUPPORT_DOCS_VER-->", dtRow("ver")) 'Just for getting the color alert selected

                strTableContent.Append(strTable_Cont.ToString)

            Next

            strTable.Replace("<!--##DELIVERABLE_SUPPORT_DOCS_CONTENT-->", strTableContent.ToString)

            '***********************************************************************************************
            '********************************DELIVERABLE RESULT TABLE***************************************
            '***********************************************************************************************

            get_Docs_Table = strTable.ToString

        End Function




        Public Function get_alert_ind(ByVal vValue As Double) As String

            '.progress-bar - yellow,
            '.progress - bar - warning {
            '    background-color:   #f39c12;
            '}
            ' background-color: #f39c12 !important;

            '.progress-bar - light - blue,
            '.progress - bar - primary {
            '    background-color:   #3c8dbc;
            '}

            '.progress-bar - green,
            '.progress - bar - success {
            '    background-color:   #00a65a;
            '}

            Dim strAlert_color As String = "#d2d6de"

            If (vValue >= 0 And vValue <= 33.33) Then
                strAlert_color = " #f39c12"
            ElseIf (vValue > 33.33 And vValue <= 66.66) Then
                strAlert_color = "#3c8dbc"
            Else '(vValue > 66.66 ) Then
                strAlert_color = "#00a65a"
            End If

            get_alert_ind = strAlert_color

        End Function

        Public Sub Add_emails(ByVal vOrden As Integer, ByVal booInitialized As Boolean, ByVal boolADDIING As Boolean) 'According the state

            If vOrden >= 0 Then 'Just a valid Order

                If booInitialized Then 'set the tbl_emails
                    Set_Emails_List() 'SE the Email List
                End If

                Add_Emails_List("TO", set_ta_roles_emails(vOrden)) 'included all
                Add_Emails_List("CC", set_ta_GrupoRoles_emails(vOrden)) 'included all groups

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
                    set_ta_DocumentosINFO()
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


        Public Sub Add_emails(ByVal vRolesCODE As String, ByVal booInitialized As Boolean, ByVal boolADDIING As Boolean, ByVal strTYPE As String) 'According the role, created for Environmental Approvals

            If booInitialized Then 'set the tbl_emails
                Set_Emails_List() 'SE the Email List
            End If


            If strTYPE = "ROLE" Then

                Add_Emails_List("TO", set_vw_ta_roles_user_all(vRolesCODE)) 'included all
                Add_Emails_List("CC", set_vw_ta_roles_user_all_Grupo(vRolesCODE)) 'included all groups

            ElseIf strTYPE = "EMAIL" Then

                Add_Emails_List("CC", vRolesCODE)

            End If


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
                'set_ta_DocumentosINFO()
                'Dim email As String() = get_ta_DocumentosINFO("email", "id_documento", id_documento).Split(";")

                'For Each dtEmail As String In email

                '    If Not String.IsNullOrEmpty(dtEmail) Then
                '        If dtEmail.IndexOf("@") > 1 And dtEmail.IndexOf(".") Then

                '            For Each dtRow As DataRow In tbl_emails.Rows

                '                If dtEmail.Trim = dtRow("email").ToString.Trim Then
                '                    boolExist = True
                '                    Exit For
                '                End If

                '            Next

                '            If boolExist = False Then 'Add The Email
                '                ObJemail.AddCC(dtEmail.Trim)
                '            Else
                '                boolExist = False
                '            End If


                '        End If
                '    End If

                'Next
                '**************************************Independent Email list By Document***************************************
            End If
            '**************************************ADDING TO THE EMAIL***************************************


        End Sub



        Public Sub Set_Emails_List()

            Sql = String.Format(" Select 'AAAA' as Tipo,  email_usuario as email from vw_t_usuarios  where id_usuario = 0")
            tbl_emails = cl_utl.setObjeto("vw_ta_roles_emails", "id_Documento", id_documento, Sql)

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

                If strEmail.ToString.Trim = dtRow("email").ToString.Trim Then
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

        '********************************************APP APPROVAL DOCUMENT**************************************************


        Public Function set_ta_roles_emails(ByVal Orden As Integer) As DataTable

            Dim strOrden As String = " "
            If Orden > -1 Then
                strOrden = String.Format(" and orden = {0} ", Orden)
            End If

            Sql = String.Format("select * from vw_ta_roles_emails where id_documento = {0} {1} ", id_documento, strOrden)
            Tbl_ta_roles_emails = cl_utl.setObjeto("vw_ta_AppDocumento", "id_Documento", id_documento, Sql)


            If (Tbl_ta_roles_emails.Rows.Count = 1 And Tbl_ta_roles_emails.Rows.Item(0).Item("id_rol") = 0) Then
                Tbl_ta_roles_emails.Rows.Remove(Tbl_ta_roles_emails.Rows.Item(0))
            End If

            set_ta_roles_emails = Tbl_ta_roles_emails

        End Function

        Public Function set_ta_travel_roles_emails(ByVal Orden As Integer, Optional id_viaje As Integer = 0, Optional campo As String = "0", Optional campo2 As String = "0", Optional campo3 As String = "0") As DataTable

            Dim strOrden As String = " "
            'If Orden > -1 Then
            '    strOrden = String.Format(" and orden = {0} ", Orden)
            'End If
            Sql = String.Format("select id_usuario, email_usuario email from t_usuarios where id_usuario in 
                                (select * from dbo.ConvertDelimitedListIntoTable((select {0} from vw_tme_solicitud_viaje where id_viaje = {1}) ,',')) or 
                                id_usuario in (select * from dbo.ConvertDelimitedListIntoTable((select {2} from vw_tme_solicitud_viaje where id_viaje = {1}) ,',')) or
                                id_usuario in (select * from dbo.ConvertDelimitedListIntoTable((select {3} from vw_tme_solicitud_viaje where id_viaje = {1}) ,',')) or
                                id_usuario = (select id_usuario from vw_tme_solicitud_viaje where id_viaje = {1})", campo, id_viaje, campo2, campo3)
            Tbl_ta_viajes_email = cl_utl.setObjeto("t_usuarios", "id_viaje", id_viaje, Sql)

            'Sql = String.Format("select id_usuario, email_usuario email from t_usuarios where id_usuario in 
            '                    (select * from dbo.ConvertDelimitedListIntoTable((select {0} from vw_tme_solicitud_viaje where id_viaje = {1}) ,',')) or 
            '                    id_usuario in (select * from dbo.ConvertDelimitedListIntoTable((select {2} from vw_tme_solicitud_viaje where id_viaje = {1}) ,',')) or
            '                    id_usuario = (select id_usuario from vw_tme_solicitud_viaje where id_viaje = {1})", campo, id_viaje, campo3)
            'Tbl_ta_viajes_email = cl_utl.setObjeto("t_usuarios", "id_viaje", id_viaje, Sql)


            'If (Tbl_ta_viajes_email.Rows.Count = 1 And Tbl_ta_viajes_email.Rows.Item(0).Item("id_rol") = 0) Then
            '    Tbl_ta_viajes_email.Rows.Remove(Tbl_ta_viajes_email.Rows.Item(0))
            'End If

            set_ta_travel_roles_emails = Tbl_ta_viajes_email

        End Function


        Public Function set_ta_par_roles_emails(ByVal Orden As Integer, Optional id_viaje As Integer = 0, Optional campo As String = "", Optional campo2 As String = "0", Optional campo3 As String = "0") As DataTable

            Dim strOrden As String = " "
            'If Orden > -1 Then
            '    strOrden = String.Format(" and orden = {0} ", Orden)
            'End If
            Sql = String.Format("select id_usuario, email_usuario email from t_usuarios where id_usuario in 
                                (select * from dbo.ConvertDelimitedListIntoTable((select isnull({3},'-1') from vw_tme_par where id_par = {0}) ,',')) or 
                                id_usuario in (select * from dbo.ConvertDelimitedListIntoTable((select {1} from vw_tme_par where id_par = {0}) ,',')) or
                                id_usuario = (select id_usuario from vw_tme_par where id_par = {0}) or
                                id_usuario = (select isnull(case when id_tipo_par = 1 and id_subregion = 54 then 2280 else 0 end,0) id_usuario_compras from vw_tme_par where id_par = {0}) or
                                id_usuario in (select * from dbo.ConvertDelimitedListIntoTable((select {2} from vw_tme_par where id_par = {0}) ,','))", id_viaje, campo2, campo3, campo)

            'Sql = String.Format("select id_usuario, email_usuario email from t_usuarios where id_usuario in 
            '                    (select * from dbo.ConvertDelimitedListIntoTable((select isnull({3},'-1') from vw_tme_par where id_par = {0}) ,',')) or 
            '                    id_usuario in (select * from dbo.ConvertDelimitedListIntoTable((select {1} from vw_tme_par where id_par = {0}) ,',')) or
            '                    id_usuario = (select id_usuario from vw_tme_par where id_par = {0}) or
            '                    id_usuario = (select isnull(case when id_tipo_par = 1 then 2280 else 0 end,0) id_usuario_compras from vw_tme_par where id_par = {0}) or
            '                    id_usuario = (select {2} from vw_tme_par where id_par = {0})", id_viaje, campo2, campo3, campo)
            Tbl_ta_par_email = cl_utl.setObjeto("t_usuarios", "id_par", id_viaje, Sql)


            'If (Tbl_ta_viajes_email.Rows.Count = 1 And Tbl_ta_viajes_email.Rows.Item(0).Item("id_rol") = 0) Then
            '    Tbl_ta_viajes_email.Rows.Remove(Tbl_ta_viajes_email.Rows.Item(0))
            'End If

            set_ta_par_roles_emails = Tbl_ta_par_email

        End Function



        Public Function set_vw_ta_roles_user_all(ByVal vROL As String) As DataTable


            Sql = String.Format("select     a.id_rol,
		                                    a.nombre_rol,
		                                    a.descripcion_rol,
		                                    a.id_usuario,
		                                    a.id_programa,
		                                    a.id_type_role,
		                                    a.nombre_usuario,
		                                    a.usuario,
		                                    a.email_usuario as email 
                                      from vw_ta_roles_user_all a where a.id_programa = {0} and a.nombre_rol = '{1}'", id_programa, vROL)

            Tbl_ta_roles_emails = cl_utl.setObjeto("vw_ta_roles_user_all", "id_programa", id_programa, Sql)

            If (Tbl_ta_roles_emails.Rows.Count = 1 And Tbl_ta_roles_emails.Rows.Item(0).Item("id_rol") = 0) Then
                Tbl_ta_roles_emails.Rows.Remove(Tbl_ta_roles_emails.Rows.Item(0))
            End If

            set_vw_ta_roles_user_all = Tbl_ta_roles_emails

        End Function


        Public Function set_ta_GrupoRoles_emails(ByVal Orden As Integer) As DataTable

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


        Public Function set_vw_ta_roles_user_all_Grupo(ByVal vROL As String) As DataTable


            Sql = String.Format("select a.id_rol,
		                                a.nombre_rol,
		                                a.descripcion_rol,
		                                b.id_usuario,
		                                a.id_programa,
		                                a.id_type_role,
		                                c.nombre_usuario,
		                                c.usuario,
		                                c.email_usuario as email 
                                    from vw_ta_roles_user_all a
                                   inner join ta_gruposRoles b on (a.id_rol = b.id_rol)
                                  inner join vw_t_usuarios c on (b.id_usuario = c.id_usuario and a.id_programa = c.id_programa)
                                where a.id_programa = {0} and a.nombre_rol = '{1}'", id_programa, vROL)

            Tbl_ta_roles_emails = cl_utl.setObjeto("vw_ta_roles_user_all", "id_programa", id_programa, Sql)

            If (Tbl_ta_roles_emails.Rows.Count = 1 And Tbl_ta_roles_emails.Rows.Item(0).Item("id_rol") = 0) Then
                Tbl_ta_roles_emails.Rows.Remove(Tbl_ta_roles_emails.Rows.Item(0))
            End If

            set_vw_ta_roles_user_all_Grupo = Tbl_ta_roles_emails

        End Function


        Public Function set_Apps_Document(Optional ByVal idR As Integer = 0) As DataTable

            Dim strOpt As String = " "

            If idR > 0 Then

                strOpt = String.Format(" and  id_ruta= {0} ", idR)

            End If

            Sql = String.Format("SELECT * FROM vw_ta_AppDocumento WHERE id_Documento= {0} {1} ", id_documento, strOpt)
            Tbl_ta_AppDoumento = cl_utl.setObjeto("vw_ta_AppDocumento", "id_Documento", id_documento, Sql)

            set_Apps_Document = Tbl_ta_AppDoumento

        End Function

        Public Function set_Apps_travel(Optional ByVal id_viaje As Integer = 0) As DataTable



            Sql = String.Format("SELECT * FROM vw_tme_solicitud_viaje where id_viaje = {0}", id_viaje)
            Tbl_ta_viaje = cl_utl.setObjeto("vw_tme_solicitud_viaje", "id_viaje", id_viaje, Sql)

            set_Apps_travel = Tbl_ta_viaje

        End Function

        Public Function set_Apps_par(Optional ByVal id_par As Integer = 0) As DataTable



            Sql = String.Format("SELECT * FROM vw_tme_par where id_par = {0}", id_par)
            Tbl_ta_par = cl_utl.setObjeto("vw_tme_par", "id_par", id_par, Sql)

            set_Apps_par = Tbl_ta_par

        End Function

        Public Function set_Apps_anticipo(Optional ByVal id_anticipo As Integer = 0) As DataTable



            Sql = String.Format("SELECT * FROM vw_tme_anticipos where id_anticipo = {0}", id_anticipo)
            Tbl_ta_anticipo = cl_utl.setObjeto("vw_tme_anticipos", "id_anticipo", id_anticipo, Sql)

            set_Apps_anticipo = Tbl_ta_anticipo

        End Function

        Public Function get_Apps_DocumentField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(Tbl_ta_AppDoumento, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function

        Public Function get_Apps_TravelField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(Tbl_ta_viaje, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function


        Public Function get_Apps_ParField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(Tbl_ta_par, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function

        Public Function get_Apps_AnticipoField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(Tbl_ta_anticipo, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function

        Public Function Apps_Document_Count() As Integer

            Apps_Document_Count = Tbl_ta_AppDoumento.Rows.Count

        End Function

        Public Function set_Document() As DataTable

            Sql = String.Format("SELECT * FROM VW_GR_TA_DOCUMENTOS WHERE id_Documento= {0} ", id_documento)
            Tbl_Document = cl_utl.setObjeto("VW_GR_TA_DOCUMENTOS", "id_Documento", id_documento, Sql)
            set_Document = Tbl_Document

        End Function

        Public Function get_Document_Comments() As DataTable
            Sql = String.Format("SELECT ROW_NUMBER() OVER(order by id_app_documento ASC) as Number,* FROM vw_ta_comentariosDoc WHERE id_documento = {0}	ORDER BY id_app_documento ASC", id_documento)
            get_Document_Comments = cl_utl.setObjeto("vw_ta_comentariosDoc", "id_Documento", id_documento, Sql)
        End Function


        Public Function get_DocumentField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(Tbl_Document, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function


        Public Function set_ta_DocumentosINFO() As DataTable

            Sql = String.Format("Select * FROM vw_ta_documentos WHERE id_documento = {0} ", id_documento)

            tbl_ta_DocumentosINFO = cl_utl.setObjeto("ta_docs_soporte", "id_doc_soporte", id_documento, Sql)

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


        Public Function set_Ta_RutaSeguimiento() As DataTable

            Sql = String.Format(" SELECT * FROM dbo.FN_Ta_RutaSeguimiento({0}) order by id_App_Documento ", id_documento)

            Tbl_Ta_RutaSeguimiento = cl_utl.setObjeto("FN_Ta_RutaSeguimiento", "id_documento", id_documento, Sql)
            set_Ta_RutaSeguimiento = Tbl_Ta_RutaSeguimiento

        End Function

        Public Function set_Ta_Viaje_Itinerario(ByVal id_viaje As Integer) As DataTable

            Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_itinerario where id_viaje = {0} ", id_viaje)

            Tbl_Ta_ViajeItinerario = cl_utl.setObjeto("vw_tme_solicitud_viaje_itinerario", "id_viaje", id_viaje, Sql)
            set_Ta_Viaje_Itinerario = Tbl_Ta_ViajeItinerario

        End Function

        Public Function set_Ta_Viaje_Pasajes(ByVal id_viaje As Integer) As DataTable

            Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_legalizacion where id_viaje = {0} and id_tipo_legalizacion = 1", id_viaje)
            Tbl_Ta_ViajePasaje = cl_utl.setObjeto("vw_tme_solicitud_viaje_itinerario", "id_viaje", id_viaje, Sql)

            Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_legalizacion where id_viaje = {0} and id_tipo_legalizacion = 2", id_viaje)
            Tbl_Ta_ViajeReuniones = cl_utl.setObjeto("vw_tme_solicitud_viaje_itinerario", "id_viaje", id_viaje, Sql)

            Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_legalizacion where id_viaje = {0} and id_tipo_legalizacion = 3", id_viaje)
            Tbl_Ta_ViajeMiscelaneos = cl_utl.setObjeto("vw_tme_solicitud_viaje_itinerario", "id_viaje", id_viaje, Sql)

            Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_legalizacion where id_viaje = {0} and id_tipo_legalizacion = 4", id_viaje)
            Tbl_Ta_ViajeAlonamiento = cl_utl.setObjeto("vw_tme_solicitud_viaje_itinerario", "id_viaje", id_viaje, Sql)


            set_Ta_Viaje_Pasajes = Tbl_Ta_ViajeAlonamiento

        End Function

        Public Function set_Ta_detalle_anticipo_compras(ByVal id_anticipo As Integer) As DataTable

            Sql = String.Format(" select * from vw_tme_anticipo_compras where id_anticipo = {0} ", id_anticipo)

            Tbl_Ta_anticipoDetalle_compras = cl_utl.setObjeto("vw_tme_anticipo_compras", "id_anticipo", id_anticipo, Sql)
            set_Ta_detalle_anticipo_compras = Tbl_Ta_anticipoDetalle_compras

        End Function
        Public Function set_Ta_detalle_anticipo_eventos(ByVal id_anticipo As Integer) As DataTable

            Sql = String.Format(" select * from vw_tme_anticipo_eventos where id_anticipo = {0} ", id_anticipo)

            Tbl_Ta_anticipoDetalle_eventos = cl_utl.setObjeto("vw_tme_anticipo_eventos", "id_anticipo", id_anticipo, Sql)
            set_Ta_detalle_anticipo_eventos = Tbl_Ta_anticipoDetalle_eventos

        End Function

        Public Function set_Ta_detalle_par(ByVal id_par As Integer) As DataTable

            Sql = String.Format(" select * from vw_tme_par_detalle where id_par = {0} ", id_par)

            Tbl_Ta_parDetalle = cl_utl.setObjeto("vw_tme_par_detalle", "id_par", id_par, Sql)
            set_Ta_detalle_par = Tbl_Ta_parDetalle

        End Function

        Public Function set_Ta_Viaje_Hotel(ByVal id_viaje As Integer) As DataTable

            Sql = String.Format(" SELECT * FROM vw_tme_solicitud_viaje_hotel where id_viaje = {0} ", id_viaje)

            Tbl_Ta_ViajeHotel = cl_utl.setObjeto("vw_tme_solicitud_viaje_hotel", "id_viaje", id_viaje, Sql)
            set_Ta_Viaje_Hotel = Tbl_Ta_ViajeHotel

        End Function

        Public Function set_Comment(ByVal idComm As Integer) As DataTable

            Sql = String.Format("SELECT * FROM vw_ta_comentariosDoc where id_comment= {0} ", id_comment)
            Tbl_ta_comentariosDoc = cl_utl.setObjeto("vw_ta_comentariosDoc", "id_comment", id_comment, Sql)

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (Tbl_ta_comentariosDoc.Rows.Count = 1 And Tbl_ta_comentariosDoc.Rows.Item(0).Item("id_comment") = 0) Then
                Tbl_ta_comentariosDoc.Rows.Remove(Tbl_ta_comentariosDoc.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            set_Comment = Tbl_ta_comentariosDoc

        End Function

        Public Function get_CommentField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(Tbl_ta_comentariosDoc, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function

        '********************************************APP APPROVAL DOCUMENT**************************************************

        Public Function get_t_programaFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_t_programa, campoSearch, campo, valorSearch)

        End Function


        '*****************************************************************************************************************
        '********************************************ENVIRONMENTAL APPROVAL MESSAGE**************************************************
        '*****************************************************************************************************************

        Public Function Emailing_ENVIRONMENTAL_APPROVAL(ByVal id_EnvDoc As Integer, ByVal strEstate As String) As Boolean

            Dim strMess As String
            id_documento_ambiental = id_EnvDoc

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
            'set_Apps_Document() 'Set all AppDoc
            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the first template

            '*****************ADDING THE SUBJECTS**********************

            set_ta_DocumentosINFO()
            'get_ta_DocumentosINFO("email", "id_documento", id_documento)

            Dim cl_envir_app As cl_environmentalAPP = New APPROVAL.cl_environmentalAPP(id_programa, id_documento_ambiental)
            cl_envir_app.get_vw_t_documento_ambiental(id_documento_ambiental)

            Dim strEnvironmentalCODE As String = String.Format("{0}-{1}", "ENV", Strings.Right("000" & id_documento_ambiental, 4))
            strEnvironmentalCODE = String.Format("{0}-{1}", strEnvironmentalCODE.Trim, get_ta_DocumentosINFO("codigo_AID", "id_documento", id_documento))

            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->

            'Change this part temporaly
            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

            strSubject = strSubject.Replace("<!--##NUMBER_INSTRUMENT##-->", strEnvironmentalCODE)
            strSubject = strSubject.Replace("<!--##STATE_STEP##-->", strEstate)
            ObJemail.AddSubject(strSubject)

            '*****************FINDING the destinataries **************************************
            '*********************************INITIALIZING************************************
            Add_emails("NONE", True, False, "NONE") 'Adding the respective email according the state of the approval proccess
            '*********************************INITIALIZING************************************

            Dim strTO As String() = get_t_notificationFIELDS("nt_TO", "id_notification", id_notification).Split(";")
            Dim strRole As String

            For Each dtEmail As String In strTO

                If Not String.IsNullOrEmpty(dtEmail) Then

                    If dtEmail.IndexOf(">>") > 1 Then ' if is a ROLE <<AF-MGR>>;<<M&E/EMS>>;raveri17@hotmail.com;
                        strRole = Strings.Mid(dtEmail, 3, dtEmail.Length - 4)
                        Add_emails(strRole, False, False, "ROLE") 'Adding the respective email according the ROL selected
                    ElseIf dtEmail.IndexOf("@") > 1 And dtEmail.IndexOf(".") Then
                        Add_emails(dtEmail.Trim, False, False, "EMAIL") 'Adding the respective email according the ROL selected
                    End If

                End If

            Next

            '*************ADDING ALL EMAILS***********
            Add_emails("NONE", False, True, "NONE")
            '*************ADDING ALL EMAILS***********
            '*****************FINDING the destinataries **************************************

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

            '**************At this part we neeed to start to replace the Documetn info for this email*******************

            '<!--##IMG_CID1##-->;<!--##IMG_CID2##-->;<!--##PROJECT_NAME##-->;<!--##DESCRIPTION_CAT##-->;<!--##DESCRIPTION_APPROVAL##-->;<!--##NUMBER_INSTRUMENT##-->;<!--##BENEFICIARY_NAME##-->;<!--##DOCUMENT_DESCRIPTION##-->;<!--##USER_NAME##-->;<!--##DATE_COMPLETED##-->;<!--##STEP_TABLE##-->;<!--##ID_DOC##-->;<!--##SYS_PATH##-->;<!--##ENVIRONMENTAL_CODE##-->;<!--##ENVIRONMENTAL_ESTATUS##-->; <!--##ENVIRONMENTAL_APPROVER##-->;<!--##ENVIRONMENTAL_APPROVED_DATE##-->; <!--##REVIEW_TYPE##-->;<!--##OBSERVATION##-->;
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", cProgram.getprogramField("nombre_programa", "id_programa", id_programa))
            strMess = strMess.Replace("<!--##DESCRIPTION_CAT##-->", get_ta_DocumentosINFO("descripcion_cat", "id_documento", id_documento))
            strMess = strMess.Replace("<!--##DESCRIPTION_APPROVAL##-->", get_ta_DocumentosINFO("descripcion_aprobacion", "id_documento", id_documento))
            strMess = strMess.Replace("<!--##NUMBER_INSTRUMENT##-->", get_ta_DocumentosINFO("numero_instrumento", "id_documento", id_documento))
            strMess = strMess.Replace("<!--##BENEFICIARY_NAME##-->", get_ta_DocumentosINFO("nom_beneficiario", "id_documento", id_documento))
            strMess = strMess.Replace("<!--##DOCUMENT_DESCRIPTION##-->", get_ta_DocumentosINFO("descripcion_doc", "id_documento", id_documento))

            strMess = strMess.Replace("<!--##USER_NAME##-->", cl_envir_app.get_vw_t_documento_ambientalFIELDS("usuario_creo", "id_documento_ambiental", id_documento_ambiental))
            'clDate.set_DateFormat(dateIN, strFormat, timezoneUTC, boolUTC)
            strMess = strMess.Replace("<!--##DATE_COMPLETED##-->", dateUtil.set_DateFormat(CDate(cl_envir_app.get_vw_t_documento_ambientalFIELDS("fecha_creado", "id_documento_ambiental", id_documento_ambiental)), "f", timezoneUTC, True))



            Dim strTablePath As String = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 1) 'getting the second Element to fill
            Dim strALL_Rows As String = ""
            Dim strRow As String = ""

            Dim strStyle As String = "<tr style='background-color:#ED7620;border:1 dotted #FF0000; ' >" 'Putting inside <tr>

            set_Ta_RutaSeguimiento() 'getting the complete route

            For Each dtRow In Tbl_Ta_RutaSeguimiento.Rows

                strRow = get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 1)

                strRow = strRow.Replace("<!--##ROLE_NAME##-->", dtRow("nombre_rol"))
                strRow = strRow.Replace("<!--##EMPLOYEE_NAME##-->", dtRow("nombre_empleado"))
                strRow = strRow.Replace("<!--##STATE##-->", dtRow("descripcion_estado"))
                strRow = strRow.Replace("<!--##DATE_RECEIPT##-->", dtRow("fecha_aprobacion"))
                strRow = strRow.Replace("<!--##ALERT_TYPE##-->", dtRow("Alerta"))

                'If idAppDoc = dtR.Item("id_App_Documento") And Not (dtR.Item("id_estadoDOC") Is DBNull.Value) 
                If id_AppDocumento = dtRow("id_app_documento") Then
                    strRow = strRow.Replace("<tr>", strStyle)
                End If

                strALL_Rows &= strRow

            Next

            strTablePath = strTablePath.Replace("<!--##CONTENT##-->", strALL_Rows)
            strMess = strMess.Replace("<!--##STEP_TABLE##-->", strTablePath) 'this would be by template

            strMess = strMess.Replace("<!--##ID_DOC##-->", id_documento_ambiental)
            'strMess = strMess.Replace("<!--##COMMENTS##-->", get_Apps_DocumentField("observacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento))
            strMess = strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System

            strMess = strMess.Replace("<!--##ENVIRONMENTAL_CODE##-->", strEnvironmentalCODE)
            strMess = strMess.Replace("<!--##ENVIRONMENTAL_ESTATUS##-->", cl_envir_app.get_vw_t_documento_ambientalFIELDS("estado_ambiental", "id_documento_ambiental", id_documento_ambiental))

            strMess = strMess.Replace("<!--##ENVIRONMENTAL_ACTION##-->", Strings.UCase(strEstate))
            strMess = strMess.Replace("<!--##ENVIRONMENTAL_APPROVER##-->", cl_envir_app.get_vw_t_documento_ambientalFIELDS("usuario_upd", "id_documento_ambiental", id_documento_ambiental))
            strMess = strMess.Replace("<!--##ENVIRONMENTAL_APPROVED_DATE##-->", dateUtil.set_DateFormat(CDate(cl_envir_app.get_vw_t_documento_ambientalFIELDS("fecha_upd", "id_documento_ambiental", id_documento_ambiental)), "f", timezoneUTC, True))
            strMess = strMess.Replace("<!--##REVIEW_TYPE##-->", cl_envir_app.get_vw_t_documento_ambientalFIELDS("tipo_revision", "id_documento_ambiental", id_documento_ambiental))
            strMess = strMess.Replace("<!--##OBSERVATION##-->", cl_envir_app.get_vw_t_documento_ambientalFIELDS("observacion", "id_documento_ambiental", id_documento_ambiental))


            '**************At this part we neeed to start to replace the Document info for this email*******************
            strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo
            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            '********************Find the LogProgram if doesn´t exist nto include**************************

            Dim Tbl_Config As DataTable = cl_utl.setObjeto("t_programa_settings", "id_programa", id_programa).Copy
            Dim strProgramIMG As String = ObJemail.getConfigField("img_report_program", "id_programa", id_programa).ToString.Trim

            If Not String.IsNullOrEmpty(strProgramIMG) Then
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & strProgramIMG, "image/png")
            Else
                ObJemail.Add_LinkResources("LogProgram", Server.MapPath("~") & "\Images\Activities\accent.png", "image/png")
            End If

            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\accent.png", "image/png")

            '********************Find the LogProgram if doesn´t exist nto include**************************

            'ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Circle_Red.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Circle_Green.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Circle_Yellow.png", "image/png")
            'ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Circle_Gray.png", "image/png")

            ObJemail.Add_LinkResources("EmmB_Red", Server.MapPath("~") + "\Imagenes\iconos\Red_Time_2.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Green", Server.MapPath("~") + "\Imagenes\iconos\Blue_Time_2.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Yellow", Server.MapPath("~") + "\Imagenes\iconos\Orange_Time_2.png", "image/png")
            ObJemail.Add_LinkResources("EmmB_Gray", Server.MapPath("~") + "\Imagenes\iconos\Cyan_Time.png_2", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_ENVIRONMENTAL_APPROVAL = True
            Else
                Emailing_ENVIRONMENTAL_APPROVAL = True
            End If

        End Function

        '*****************************************************************************************************************
        '********************************************ENVIRONMENTAL APPROVAL MESSAGE, APPROVAL STEP**************************************************
        '*****************************************************************************************************************




#Region "Examples to implement"



        '*****************************************************************************************************************
        '********************************************ORGANIZATION DETAIL**************************************************
        '*****************************************************************************************************************

        Public Function BuildOrganizationEmail(ByVal id_proy As Integer) As Boolean

            id_proyecto = id_proy

            '**********************************Esto va configurado dependiendo de la necesidad*********************
            'Tbl_EquipoCentral = utlCORE.setObjeto("VW_GR_EQUIPO_ME", "id_proyecto", id_programa).Copy

            '****************************************TEMPORAL**************************************************
            Tbl_EquipoCentral = cl_utl.setObjeto("VW_GR_EQUIPO_ME", "id_usr", 92).Copy
            '****************************************TEMPORAL**************************************************

            '**********************************Esto va configurado dependiendo de la necesidad*********************
            Tbl_EquipoRegional = cl_utl.setObjeto("VW_GR_EQUIPO_ME", "id_proyecto", id_proyecto).Copy
            tbl_t_notification = cl_utl.setObjeto("t_notification", "nt_code", 0, , "NOT_BENEFRG").Copy
            'Tbl_EquipoRegional = utlCORE.setObjeto("VW_GR_ORGANIZACION_ME", "id_proyecto", id_proyecto).Copy
            '******************************************CREAMOS LA INSTANCIA*********************************************

            ObJemail = New UTILS.cls_email(id_programa)

            'If Tbl_EquipoRegional.Rows.Count > 0 Then 'Agregamos a la regional y copiamos a la central
            '    ObJemail.AddTo(Tbl_EquipoRegional)
            'End If
            If Tbl_EquipoCentral.Rows.Count > 0 Then
                ObJemail.AddTo(Tbl_EquipoCentral)
            End If

            ObJemail.AddSubject(tbl_t_notification.Rows.Item(0).Item("nt_subject"))

            '******************************************************************************************************
            Dim Mensaje = BuildOrganizationTemplate(tbl_t_notification.Rows.Item(0).Item("id_notification"))
            'Mensaje,
            If Not ObJemail.SendEmail(tbl_t_notification.Rows.Item(0).Item("id_notification")) Then
                BuildOrganizationEmail = False
            Else
                BuildOrganizationEmail = True
            End If

            '**********************************Esto va configurado dependiendo de la necesidad*********************

        End Function


        Function BuildOrganizationTemplate(ByVal id_notification As Integer, Optional ByVal id_Org As Integer = 0, Optional ByVal id_proy As Integer = 0) As String
            '***********construye la información de una organizaciones con su información pendiente**************

            Dim strMess As String
            Dim tbl_organization As DataTable
            Dim strSQL As String

            If id_proy <> 0 Then
                id_proyecto = id_proy
            End If

            '<!--##CONTENT##-->;<!--##REGION##-->
            Tbl_t_template = cl_utl.setObjeto("t_template", "id_notification", id_notification).Copy

            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)
            'strMess = Tbl_Template.Rows.Item(0).Item("tp_script")
            Dim strPicAdd As String = "~\Imagenes\logos\CoResCeliNegropantalla.png"

            Dim tbl_proy As DataTable = cl_utl.setObjeto("t_proyecto", "id_proyecto", id_proyecto).Copy
            strMess = strMess.Replace("<!--##REGION##-->", tbl_proy.Rows.Item(0).Item("nombre_proyecto"))
            strMess = strMess.Replace("<!--##IMG_SRC##-->", strPicAdd)

            '<!--##CONTENT##-->

            If id_Org = 0 Then

                strSQL = String.Format("select distinct region, id_ejecutor, nombre_ejecutor, COUNT(*) as N  " &
                                       " from vw_tme_Beneficiario_porc_registro_ficha " &
                                       "   where porc < 1 " &
                                       "   and id_proyecto = {0} " &
                                       "    and id_tipo_beneficiario = 1 " &
                                       "     and id_estado_beneficiario =    1 " &
                                       "   group by region, id_ejecutor, nombre_ejecutor" &
                                       "  order by COUNT(*) DESC ", id_proyecto)
            Else

                strSQL = String.Format("select distinct id_ejecutor, nombre_ejecutor " &
                                        "    from vw_tme_Beneficiario_porc_registro_ficha   " &
                                        "        where porc < 1  " &
                                        "         and id_proyecto = {0}  " &
                                        "           and id_tipo_beneficiario = 1 " &
                                        "               and id_estado_beneficiario = 1  " &
                                        "                and id_ejecutor = {1}          ", id_proyecto, id_Org)

            End If

            tbl_organization = cl_utl.setObjeto("vw_tme_Beneficiario_porc_registro_ficha", "id_proyecto", id_proyecto, strSQL).Copy

            Dim strOrganization As String = ""
            For Each dt As DataRow In tbl_organization.Rows
                '<!--##ORGANIZATION##-->
                strOrganization &= get_t_templateField("tp_content", "id_notification", 1, "tp_orden", 0).Replace("<!--##ORGANIZATION##-->", BuildOrganizationDETAIL(dt.Item("id_ejecutor"), dt.Item("nombre_ejecutor"), 1))
                'strOrganization &= get_t_templateField("tp_content", "id_notification", 1, "tp_orden", 0).Replace("<!--##ORGANIZATION##-->", BuildOrganizationDETAIL(tbl_organization.Rows.Item(0).Item("id_ejecutor"), tbl_organization.Rows.Item(0).Item("nombre_ejecutor"), 1))
                strOrganization &= get_t_templateField("tp_nwline", "id_notification", 1, "tp_orden", 0)
            Next

            strMess = strMess.Replace("<!--##CONTENT##-->", strOrganization)
            BuildOrganizationTemplate = strMess

        End Function



        '****************************MUESTRA DETALLE DE BENEFICIARIOS PARA UNA ORGANIZACION*************************************************
        Function BuildOrganizationDETAIL(ByVal id_ejecutor As Integer, ByVal strNombre As String, ByVal tp_orden As Integer)
            '******************construye una organización individual con toda su información por notificar********************

            ' <!--##CONTENT##-->;<!--##NOMBRE_ORGANIZACION##-->;<!--##CONVENIO##-->
            Dim strOrganization As String = get_t_templateField("tp_script", "id_notification", 1, "tp_orden", tp_orden)
            Dim tbl_Convenios As DataTable
            Dim strconvenios As String = ""

            Dim strSQL = String.Format("select distinct id_ejecutor, id_ficha_proyecto, codigo_SAPME " &
                                       "    from vw_tme_Beneficiario_porc_registro_ficha " &
                                       "     where porc < 1 " &
                                       "       and id_ejecutor = {0} " &
                                       "        and id_tipo_beneficiario = 1 " &
                                       "          and id_estado_beneficiario = 1 " &
                                       "         order by codigo_SAPME ", id_ejecutor)

            strOrganization = strOrganization.Replace("<!--##NOMBRE_ORGANIZACION##-->", strNombre)
            strOrganization = strOrganization.Replace("<!--##ID_EJEC##-->", id_ejecutor)
            strOrganization = strOrganization.Replace("<!--##ID_PROY##-->", id_proyecto)
            strOrganization = strOrganization.Replace("<!--##DETAIL##-->", "")

            '*********************************************ESTE GENERA  UN DETALLE GENERAL CON TODOS LOS BENEFICIARIOS********************************************
            tbl_Convenios = cl_utl.setObjeto("vw_tme_Beneficiario_porc_registro_ficha", "id_ejecutor", id_ejecutor, strSQL).Copy
            For Each dt As DataRow In tbl_Convenios.Rows
                strconvenios &= get_t_templateField("tp_content", "id_notification", 1, "tp_orden", tp_orden).Replace("<!--##CONVENIO##-->", BuildConvenio(dt.Item("id_ficha_proyecto"), 2))
                strconvenios &= get_t_templateField("tp_nwline", "id_notification", 1, "tp_orden", tp_orden)
            Next
            '*********************************************ESTE GENERA  UN DETALLE GENERAL CON TODOS LOS BENEFICIARIOS********************************************

            strOrganization = strOrganization.Replace("<!--##CONTENT##-->", strconvenios)
            BuildOrganizationDETAIL = strOrganization


        End Function


        Function BuildConvenio(ByVal id_ficha As Integer, ByVal tp_orden As Integer)
            'tiene la estrucutura principal de como irá notificado cada convenio

            Dim strConvenio As String = get_t_templateField("tp_script", "id_notification", 1, "tp_orden", tp_orden)
            strConvenio = strConvenio.Replace("<!--##CONVENIO##-->", BuildConvenioInfo(id_ficha, 1, 3))
            strConvenio = strConvenio.Replace("<!--##BENEFICIARIO##--> ", BuildBeneficiarioInfo(id_ficha, 4))
            BuildConvenio = strConvenio

        End Function

        Function BuildConvenioInfo(ByVal id_ficha As Integer, ByVal id_notification As Integer, ByVal tp_orden As Integer)

            Dim tbl_convenio As DataTable
            Dim strSQL As String
            Dim strConvenio As String = ""

            strSQL = String.Format(" select id_ficha_proyecto, " &
                                   "   codigo_SAPME, " &
                                   "   nombre_proyecto, " &
                                   "   fecha_inicio_proyecto, " &
                                   "   fecha_fin_proyecto " &
                                   "      from vw_tme_fichaProyecto " &
                                   "   where id_ficha_proyecto = {0} ", id_ficha)

            tbl_convenio = cl_utl.setObjeto("vw_tme_fichaProyecto", "id_ficha_proyecto", id_ficha, strSQL).Copy

            strConvenio = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", tp_orden)
            strConvenio = strConvenio.Replace("<!--##CODIGO##-->", tbl_convenio.Rows.Item(0).Item("codigo_SAPME"))
            strConvenio = strConvenio.Replace("<!--##NOMBRE##-->", tbl_convenio.Rows.Item(0).Item("nombre_proyecto"))
            strConvenio = strConvenio.Replace("<!--##FECH_INI##-->", String.Format("'{0:dd/MM/yyyy}'", tbl_convenio.Rows.Item(0).Item("fecha_inicio_proyecto")))
            strConvenio = strConvenio.Replace("<!--##FECH_FIN##-->", String.Format("'{0:dd/MM/yyyy}'", tbl_convenio.Rows.Item(0).Item("fecha_fin_proyecto")))


            BuildConvenioInfo = strConvenio

        End Function


        Function BuildBeneficiarioInfo(ByVal id_ficha As Integer, ByVal tp_orden As Integer)

            Dim tbl_beneficiario As DataTable
            Dim strBeneficiario As String
            Dim strBeneficiarioContent As String
            Dim strBeneficiarioLine As String
            Dim strSQL As String = String.Format("select * from  vw_tme_Beneficiario_porc_registro_ficha " &
                                                  "  where id_ficha_proyecto = {0}  " &
                                                  "   and porc < 1 " &
                                                  "    and id_tipo_beneficiario = 1 " &
                                                  "     and id_estado_beneficiario = 1 ", id_ficha)

            tbl_beneficiario = cl_utl.setObjeto("vw_tme_Beneficiario_porc_registro_ficha", "id_ficha_proyecto", id_ficha, strSQL).Copy
            strBeneficiario = get_t_templateField("tp_script", "id_notification", 1, "tp_orden", tp_orden)

            strBeneficiarioContent = ""

            For Each dt As DataRow In tbl_beneficiario.Rows
                strBeneficiarioLine = get_t_templateField("tp_content", "id_notification", 1, "tp_orden", tp_orden)
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##CEDULA##-->", dt.Item("ID"))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##NOMBRE##-->", dt.Item("nombre"))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##MUNICIPIO##-->", dt.Item("municipio"))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##DEPARTAMENTO##-->", dt.Item("departamento"))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##PORC##-->", String.Format("{0:0.00%}", dt.Item("Porc")))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##CREADO##-->", String.Format("'{0:dd/MM/yyyy HH:mm:ss}'", dt.Item("creado")))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##MODIFICADO##-->", String.Format("'{0:dd/MM/yyyy HH:mm:ss}'", dt.Item("modificado")))
                strBeneficiarioContent &= strBeneficiarioLine
            Next

            strBeneficiario = strBeneficiario.Replace("<!--##CONTENT##-->", strBeneficiarioContent)

            BuildBeneficiarioInfo = strBeneficiario

        End Function



        '*****************************************************************************************************************
        '********************************************ORGANIZATION DETAIL**************************************************
        '*****************************************************************************************************************

        '**********************************************************************************************************
        '********************************************BENEFICIARIOS*************************************************
        '**********************************************************************************************************


        Public Function BuildBeneficiariosEmail(ByVal id_proy As Integer) As Boolean

            id_proyecto = id_proy

            '**********************************Esto va configurado dependiendo de la necesidad*********************
            Tbl_EquipoCentral = cl_utl.setObjeto("VW_GR_EQUIPO_ME", "id_proyecto", id_programa).Copy
            '****************************************TEMPORAL**************************************************
            'Tbl_EquipoCentral = utlCORE.setObjeto("VW_GR_EQUIPO_ME", "id_usr", 92).Copy
            '****************************************TEMPORAL**************************************************

            '**********************************Esto va configurado dependiendo de la necesidad*********************
            Tbl_EquipoRegional = cl_utl.setObjeto("VW_GR_EQUIPO_ME", "id_proyecto", id_proyecto).Copy
            tbl_t_notification = cl_utl.setObjeto("t_notification", "nt_code", 0, , "NOT_BENEFRG").Copy
            'Tbl_EquipoRegional = utlCORE.setObjeto("VW_GR_ORGANIZACION_ME", "id_proyecto", id_proyecto).Copy
            '******************************************CREAMOS LA INSTANCIA*********************************************

            ObJemail = New UTILS.cls_email(id_programa)

            If Tbl_EquipoRegional.Rows.Count > 0 Then 'Agregamos a la regional y copiamos a la central
                ObJemail.AddTo(Tbl_EquipoRegional)
            End If

            If Tbl_EquipoCentral.Rows.Count > 0 Then
                ObJemail.AddCC(Tbl_EquipoCentral)
            End If

            ObJemail.AddSubject(tbl_t_notification.Rows.Item(0).Item("nt_subject"))

            '******************************************************************************************************
            Dim Mensaje = BuildBeneficiariosTemplate(tbl_t_notification.Rows.Item(0).Item("id_notification"))
            If Mensaje <> "" Then
                'Mensaje,
                If Not ObJemail.SendEmail(tbl_t_notification.Rows.Item(0).Item("id_notification")) Then
                    BuildBeneficiariosEmail = False
                Else
                    BuildBeneficiariosEmail = True
                End If
            End If
            '**********************************Esto va configurado dependiendo de la necesidad*********************

        End Function


        Function BuildBeneficiariosTemplate(ByVal id_notification As Integer) As String
            '***********construye la información de todas las organizaciones con su información pendiente**************

            Dim strMess As String
            Dim tbl_organization As DataTable
            Dim strSQL As String

            '<!--##CONTENT##-->;<!--##REGION##-->
            Tbl_t_template = cl_utl.setObjeto("t_template", "id_notification", id_notification).Copy

            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)
            'strMess = Tbl_Template.Rows.Item(0).Item("tp_script")

            Dim tbl_proy As DataTable = cl_utl.setObjeto("t_proyecto", "id_proyecto", id_proyecto).Copy
            strMess = strMess.Replace("<!--##REGION##-->", tbl_proy.Rows.Item(0).Item("nombre_proyecto"))
            strMess = strMess.Replace("<!--##IMG_SRC##-->", "cid:LogCELI")

            '<!--##CONTENT##-->
            strSQL = String.Format("select distinct region, id_ejecutor, nombre_ejecutor, COUNT(*) as N  " &
                                   " from vw_tme_Beneficiario_porc_registro_ficha " &
                                   "   where porc < 1 " &
                                   "   and id_proyecto = {0} " &
                                   "    and id_tipo_beneficiario = 1 " &
                                   "      and id_estado_beneficiario =    1 " &
                                   "   group by region, id_ejecutor, nombre_ejecutor" &
                                   "  order by COUNT(*) DESC ", id_proyecto)

            tbl_organization = cl_utl.setObjeto("vw_tme_Beneficiario_porc_registro_ficha", "id_proyecto", id_proyecto, strSQL).Copy

            Dim strOrganization As String = ""

            If tbl_organization.Rows.Item(0).Item("region").ToString.Trim.Length > 0 Then

                For Each dt As DataRow In tbl_organization.Rows

                    '<!--##ORGANIZATION##-->
                    strOrganization &= get_t_templateField("tp_content", "id_notification", 1, "tp_orden", 0).Replace("<!--##ORGANIZATION##-->", BuildOrganization(dt.Item("id_ejecutor"), dt.Item("nombre_ejecutor"), 1))
                    strOrganization &= get_t_templateField("tp_nwline", "id_notification", 1, "tp_orden", 0)

                Next
                strMess = strMess.Replace("<!--##CONTENT##-->", strOrganization)
                BuildBeneficiariosTemplate = strMess

            Else

                BuildBeneficiariosTemplate = ""

            End If

        End Function



        Function BuildOrganization(ByVal id_ejecutor As Integer, ByVal strNombre As String, ByVal tp_orden As Integer)
            '******************construye una organización individual con toda su información por notificar********************

            ' <!--##CONTENT##-->;<!--##NOMBRE_ORGANIZACION##-->;<!--##CONVENIO##-->
            Dim strOrganization As String = get_t_templateField("tp_script", "id_notification", 1, "tp_orden", tp_orden)
            Dim tbl_Convenios As DataTable
            Dim strconvenios As String = ""

            Dim strSQL = String.Format("select distinct id_ejecutor, id_ficha_proyecto, codigo_SAPME " &
                                       "    from vw_tme_Beneficiario_porc_registro_ficha " &
                                       "     where porc < 1 " &
                                       "       and id_ejecutor = {0} " &
                                       "        and id_tipo_beneficiario = 1 " &
                                       "          and id_estado_beneficiario = 1 " &
                                       "         order by codigo_SAPME ", id_ejecutor)

            strOrganization = strOrganization.Replace("<!--##NOMBRE_ORGANIZACION##-->", strNombre)
            strOrganization = strOrganization.Replace("<!--##ID_EJEC##-->", id_ejecutor)
            strOrganization = strOrganization.Replace("<!--##ID_PROY##-->", id_proyecto)
            strOrganization = strOrganization.Replace("<!--##DETAIL##-->", "Ver Detalle")

            '*********************************************ESTE GENERA  UN DETALLE GENERAL CON TODOS LOS BENEFICIARIOS********************************************
            'tbl_Convenios = utlCORE.setObjeto("vw_tme_Beneficiario_porc_registro_ficha", "id_ejecutor", id_ejecutor, strSQL).Copy
            'For Each dt As DataRow In tbl_Convenios.Rows
            'strconvenios &= get_t_templateField("tp_content", "id_notification", 1, "tp_orden", tp_orden).Replace("<!--##CONVENIO##-->", BuildConvenio(dt.Item("id_ficha_proyecto"), 2))
            'strconvenios &= get_t_templateField("tp_nwline", "id_notification", 1, "tp_orden", tp_orden)
            'Next
            '*********************************************ESTE GENERA  UN DETALLE GENERAL CON TODOS LOS BENEFICIARIOS********************************************

            strconvenios &= get_t_templateField("tp_content", "id_notification", 1, "tp_orden", tp_orden).Replace("<!--##CONVENIO##-->", BuildConvenioRES(id_ejecutor, 5))
            strconvenios &= get_t_templateField("tp_nwline", "id_notification", 1, "tp_orden", tp_orden)

            strOrganization = strOrganization.Replace("<!--##CONTENT##-->", strconvenios)
            BuildOrganization = strOrganization

        End Function


        '****************************MUESTRA DETALLE DE BENEFICIARIOS PARA UNA ORGANIZACION*************************************************

        Function BuildConvenioRES(ByVal id_Ejecutor As Integer, ByVal tp_orden As Integer)
            'tiene la estrucutura principal de como irá notificado el resumen de convenios

            Dim tbl_Convenios As DataTable
            Dim strConvenios_RES As String = get_t_templateField("tp_script", "id_notification", 1, "tp_orden", tp_orden)
            Dim strConvenioLines As String = ""

            Dim strSQL As String = String.Format("select table1.codigo_SAPME, " &
                                                       "table1.nombre_componente, " &
                                                          "table1.nombre_proyecto, " &
                                                          "table1.estado, " &
                                                       "table1.fecha_inicio_proyecto, " &
                                                       "table1.fecha_fin_proyecto, " &
                                                          "table1.B_PEND, " &
                                                          "table1.B_COMPL, " &
                                                          "round( (convert(decimal(15,3),(convert(decimal(15,3),table1.B_PEND)/(convert(decimal(15,3),table1.B_PEND) + convert(decimal(15,3),table1.B_COMPL))))),4) as Porc " &
                                                 "from  " &
                                           "   (select " &
                                               "      TAB.codigo_SAPME,  " &
                                               "      TAB.nombre_componente,  " &
                                               "      TAB.nombre_proyecto,  " &
                                               "      TAB.estado, " &
                                               "      TAB.fecha_inicio_proyecto, " &
                                               "      TAB.fecha_fin_proyecto, " &
                                               "      sum(B_PEND) as B_PEND, " &
                                               "      sum(B_COMPL) as B_COMPL " &
                                                     "  FROM  " &
                                                     " (select codigo_SAPME,  " &
                                                      "   nombre_componente,  " &
                                                      "   nombre_proyecto,  " &
                                                      "   nombre_estado_ficha as estado, " &
                                                      "   fecha_inicio_proyecto, " &
                                                      "   fecha_fin_proyecto, " &
                                                      "   COUNT(*) as B_PEND, " &
                                                      "   0 as B_COMPL " &
                                                           " from vw_tme_Beneficiario_porc_registro_ficha " &
                                                           "        where porc < 1 " &
                                                  "              and id_proyecto = {0}   " &
                                                  "              and id_tipo_beneficiario = 1  " &
                                                  "              and id_estado_beneficiario =  1 " &
                                                  "              and id_ejecutor = {1}  " &
                                                     "group by codigo_SAPME,  " &
                                                           "          nombre_componente, " &
                                                           "          nombre_proyecto, " &
                                                           "          nombre_estado_ficha, " &
                                                           "          fecha_inicio_proyecto, " &
                                                           "          fecha_fin_proyecto  " &
                                          " UNION ALL " &
                                             " select codigo_SAPME,  " &
                                          "        nombre_componente,  " &
                                          "        nombre_proyecto,  " &
                                          "        nombre_estado_ficha as estado, " &
                                          "        fecha_inicio_proyecto, " &
                                          "        fecha_fin_proyecto, " &
                                          "        0 as B_PEND, " &
                                          "        COUNT(*) as B_COMPL " &
                                                   " from vw_tme_Beneficiario_porc_registro_ficha  " &
                                                   "   where porc >= 1 " &
                                          "      and id_proyecto =  {0}  " &
                                          "      and id_tipo_beneficiario = 1  " &
                                          "      and id_estado_beneficiario =  1  " &
                                          "      and id_ejecutor = {1}   " &
                                             " group by codigo_SAPME,  " &
                                       "          nombre_componente,  " &
                                       "          nombre_proyecto,  " &
                                       "          nombre_estado_ficha,  " &
                                       "          fecha_inicio_proyecto, " &
                                       "          fecha_fin_proyecto)  as TAB " &
                                          "     group by TAB.codigo_SAPME,  " &
                                       "          TAB.nombre_componente,  " &
                                       "          TAB.nombre_proyecto,  " &
                                       "          TAB.estado, " &
                                       "          TAB.fecha_inicio_proyecto, " &
                                       "          TAB.fecha_fin_proyecto) as table1    " &
                                              " where table1.B_PEND > 0 ", id_proyecto, id_Ejecutor)

            tbl_Convenios = cl_utl.setObjeto("vw_tme_Beneficiario_porc_registro_ficha", "id_ejecutor", id_Ejecutor, strSQL).Copy

            For Each dt As DataRow In tbl_Convenios.Rows

                strConvenioLines &= get_t_templateField("tp_content", "id_notification", 1, "tp_orden", tp_orden)
                strConvenioLines = strConvenioLines.Replace("<!--##CODIGO##-->", dt.Item("codigo_SAPME"))
                strConvenioLines = strConvenioLines.Replace("<!--##COMPONENTE##-->", dt.Item("nombre_componente"))
                strConvenioLines = strConvenioLines.Replace("<!--##PROYECTO##-->", dt.Item("nombre_proyecto"))
                strConvenioLines = strConvenioLines.Replace("<!--##ESTADO##-->", dt.Item("estado"))
                strConvenioLines = strConvenioLines.Replace("<!--##INICIO##-->", String.Format("'{0:dd/MM/yyyy}'", dt.Item("fecha_inicio_proyecto")))
                strConvenioLines = strConvenioLines.Replace("<!--##FIN##-->", String.Format("'{0:dd/MM/yyyy}'", dt.Item("fecha_fin_proyecto")))
                strConvenioLines = strConvenioLines.Replace("<!--##PENDIENTE##-->", dt.Item("B_PEND"))
                strConvenioLines = strConvenioLines.Replace("<!--##COMPLETOS##-->", dt.Item("B_COMPL"))
                strConvenioLines = strConvenioLines.Replace("<!--##PORC##-->", String.Format("{0:0.00%}", dt.Item("Porc")))

            Next

            strConvenios_RES = strConvenios_RES.Replace("<!--##CONTENT##-->", strConvenioLines)

            BuildConvenioRES = strConvenios_RES

        End Function

        '**********************************************************************************************************
        '********************************************BENEFICIARIOS*************************************************
        '**********************************************************************************************************


        '**********************************************************************************************************
        '********************************************ORGANIZATION*************************************************
        '**********************************************************************************************************


        Public Function BuildOrganizationUniqueEmail(ByVal id_proy As Integer) As Boolean

            Dim tbl_organization As DataTable
            Dim strSQL As String
            id_proyecto = id_proy

            '**********************************Esto va configurado dependiendo de la necesidad*********************
            'Tbl_EquipoCentral = utlCORE.setObjeto("VW_GR_EQUIPO_ME", "id_proyecto", id_programa).Copy
            '****************************************TEMPORAL**************************************************
            Tbl_EquipoCentral = cl_utl.setObjeto("VW_GR_EQUIPO_ME", "id_usr", 92).Copy
            '****************************************TEMPORAL**************************************************

            '**********************************Esto va configurado dependiendo de la necesidad*********************
            'Tbl_EquipoRegional = utlCORE.setObjeto("VW_GR_EQUIPO_ME", "id_proyecto", id_proyecto).Copy
            tbl_t_notification = cl_utl.setObjeto("t_notification", "nt_code", 0, , "NOT_BENEFRG").Copy
            'Tbl_Organizacion = utlCORE.setObjeto("VW_GR_ORGANIZACION_ME", "id_proyecto", id_proyecto).Copy
            '******************************************CREAMOS LA INSTANCIA*********************************************

            strSQL = String.Format("select distinct region, id_ejecutor, nombre_ejecutor, COUNT(*) as N  " &
                                   " from vw_tme_Beneficiario_porc_registro_ficha " &
                                   "   where porc < 1 " &
                                   "   and id_proyecto = {0} " &
                                   "    and id_tipo_beneficiario = 1 " &
                                   "      and id_estado_beneficiario =    1 " &
                                   "   group by region, id_ejecutor, nombre_ejecutor" &
                                   "  order by COUNT(*) DESC ", id_proyecto)




            tbl_organization = cl_utl.setObjeto("vw_tme_Beneficiario_porc_registro_ficha", "id_proyecto", id_proyecto, strSQL).Copy

            If tbl_organization.Rows.Item(0).Item("region").ToString.Trim.Length > 0 Then

                For Each dt As DataRow In tbl_organization.Rows 'Mandar un correo para Cada Organización

                    ObJemail = New UTILS.cls_email(id_programa)

                    Tbl_EquipoOrganizacion = cl_utl.setObjeto("VW_GR_ORGANIZACION_ME", "id_ejecutor", dt.Item("id_ejecutor")).Copy

                    If Tbl_EquipoOrganizacion.Rows.Count > 0 Then 'Agregamos a la regional y copiamos a la central
                        ObJemail.AddTo(Tbl_EquipoOrganizacion)
                    End If

                    'If Tbl_EquipoCentral.Rows.Count > 0 Then
                    '    ObJemail.AddCC(Tbl_EquipoCentral)
                    'End If

                    ObJemail.AddSubject(tbl_t_notification.Rows.Item(0).Item("nt_subject"))

                    '******************************************************************************************************
                    Dim Mensaje = BuildOrganizationUNTemplate(tbl_t_notification.Rows.Item(0).Item("id_notification"), dt.Item("id_ejecutor"), dt.Item("nombre_ejecutor"))
                    'Mensaje,
                    If Not ObJemail.SendEmail(tbl_t_notification.Rows.Item(0).Item("id_notification")) Then
                        BuildOrganizationUniqueEmail = False
                    Else
                        BuildOrganizationUniqueEmail = True
                    End If

                Next

            End If
            '**********************************Esto va configurado dependiendo de la necesidad*********************

        End Function


        Function BuildOrganizationUNTemplate(ByVal id_notification As Integer, ByVal id_ejecutor As Integer, ByVal ejecut_nombre As String) As String
            '***********construye la información de todas las organizaciones con su información pendiente**************

            Dim strMess As String
            Dim strSQL As String

            '<!--##CONTENT##-->;<!--##REGION##-->
            Tbl_t_template = cl_utl.setObjeto("t_template", "id_notification", id_notification).Copy

            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)
            'strMess = Tbl_Template.Rows.Item(0).Item("tp_script")

            Dim tbl_proy As DataTable = cl_utl.setObjeto("t_proyecto", "id_proyecto", id_proyecto).Copy
            strMess = strMess.Replace("<!--##REGION##-->", tbl_proy.Rows.Item(0).Item("nombre_proyecto"))
            strMess = strMess.Replace("<!--##IMG_SRC##-->", "cid:LogCELI")

            '<!--##CONTENT##-->
            'strSQL = String.Format("select distinct region, id_ejecutor, nombre_ejecutor, COUNT(*) as N  " & _
            '                       " from vw_tme_Beneficiario_porc_registro_ficha " & _
            '                       "   where porc < 1 " & _
            '                       "   and id_proyecto = {0} " & _
            '                       "    and id_tipo_beneficiario = 1 " & _
            '                       "      and id_estado_beneficiario =    1 " & _
            '                       "   group by region, id_ejecutor, nombre_ejecutor" & _
            '                       "  order by COUNT(*) DESC ", id_proyecto)

            'tbl_organization = utlCORE.setObjeto("vw_tme_Beneficiario_porc_registro_ficha", "id_proyecto", id_proyecto, strSQL).Copy

            Dim strOrganization As String = ""
            'For Each dt As DataRow In tbl_organization.Rows
            '<!--##ORGANIZATION##-->
            strOrganization &= get_t_templateField("tp_content", "id_notification", 1, "tp_orden", 0).Replace("<!--##ORGANIZATION##-->", BuildOrganization(id_ejecutor, ejecut_nombre, 1))
            strOrganization &= get_t_templateField("tp_nwline", "id_notification", 1, "tp_orden", 0)
            'Next

            strMess = strMess.Replace("<!--##CONTENT##-->", strOrganization)
            BuildOrganizationUNTemplate = strMess

        End Function


        '**********************************************************************************************************
        '********************************************ORGANIZATION*************************************************
        '**********************************************************************************************************



        '***********************************************************************************************************************
        '********************************************VEREDAS CORREGIMIENTOS NOT*************************************************
        '***********************************************************************************************************************


        Public Function BuildVerCorrEmail(ByVal id_proy As Integer) As Boolean

            id_proyecto = id_proy

            '**********************************Esto va configurado dependiendo de la necesidad*********************
            'Tbl_EquipoCentral = utlCORE.setObjeto("VW_GR_EQUIPO_ME", "id_proyecto", id_programa).Copy
            '****************************************TEMPORAL**************************************************
            Tbl_EquipoCentral = cl_utl.setObjeto("VW_GR_EQUIPO_ME", "id_usr", 92).Copy
            '****************************************TEMPORAL**************************************************

            '**********************************Esto va configurado dependiendo de la necesidad*********************
            Tbl_EquipoRegional = cl_utl.setObjeto("VW_GR_EQUIPO_ME", "id_proyecto", id_proyecto).Copy
            tbl_t_notification = cl_utl.setObjeto("t_notification", "nt_code", 0, , "NOT_VERCORR").Copy
            'Tbl_EquipoRegional = utlCORE.setObjeto("VW_GR_ORGANIZACION_ME", "id_proyecto", id_proyecto).Copy
            '******************************************CREAMOS LA INSTANCIA*********************************************

            ObJemail = New UTILS.cls_email(id_programa)

            If Tbl_EquipoRegional.Rows.Count > 0 Then 'Agregamos a la regional y copiamos a la central
                ObJemail.AddTo(Tbl_EquipoRegional)
            End If

            If Tbl_EquipoCentral.Rows.Count > 0 Then
                ObJemail.AddCC(Tbl_EquipoCentral)
                'ObJemail.AddTo(Tbl_EquipoCentral)
            End If

            ObJemail.AddSubject(tbl_t_notification.Rows.Item(0).Item("nt_subject"))

            '******************************************************************************************************
            Dim Mensaje = BuildVerCorrTemplate(tbl_t_notification.Rows.Item(0).Item("id_notification"))
            'Mensaje, 
            If Mensaje <> "" Then
                If Not ObJemail.SendEmail(tbl_t_notification.Rows.Item(0).Item("id_notification")) Then
                    BuildVerCorrEmail = False
                Else
                    BuildVerCorrEmail = True
                End If
            End If
            '**********************************Esto va configurado dependiendo de la necesidad*********************

        End Function



        Function BuildVerCorrTemplate(ByVal id_notification As Integer) As String
            '***********construye la información de todas las organizaciones con su información pendiente**************

            Dim strMess As String
            Dim tbl_organization As DataTable
            Dim strSQL As String

            '<!--##CONTENT##-->;<!--##REGION##-->
            Tbl_t_template = cl_utl.setObjeto("t_template", "id_notification", id_notification).Copy

            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0)
            'strMess = Tbl_Template.Rows.Item(0).Item("tp_script")

            Dim tbl_proy As DataTable = cl_utl.setObjeto("t_proyecto", "id_proyecto", id_proyecto).Copy
            strMess = strMess.Replace("<!--##REGION##-->", tbl_proy.Rows.Item(0).Item("nombre_proyecto"))
            strMess = strMess.Replace("<!--##IMG_SRC##-->", "cid:LogCELI")

            '<!--##CONTENT##-->

            strSQL = String.Format(" select distinct region, id_ejecutor, nombre_ejecutor, COUNT(*) as N  " &
                                   "   from vw_ver_corr_solicitud    " &
                                   "     where id_proyecto = {0}     " &
                                   "  group by region, id_ejecutor, nombre_ejecutor " &
                                   "   order by COUNT(*) DESC ", id_proyecto)


            tbl_organization = cl_utl.setObjeto("vw_ver_corr_solicitud", "id_proyecto", id_proyecto, strSQL).Copy

            Dim strOrganization As String = ""

            If tbl_organization.Rows.Item(0).Item("region").ToString.Trim.Length > 0 Then

                For Each dt As DataRow In tbl_organization.Rows

                    '<!--##ORGANIZATION##-->
                    strOrganization &= get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", 0).Replace("<!--##ORGANIZATION##-->", BuildOrganizationVERCORR(dt.Item("id_ejecutor"), dt.Item("nombre_ejecutor"), id_notification, 1))
                    strOrganization &= get_t_templateField("tp_nwline", "id_notification", id_notification, "tp_orden", 0)

                Next

                strMess = strMess.Replace("<!--##CONTENT##-->", strOrganization)
                BuildVerCorrTemplate = strMess

            Else

                BuildVerCorrTemplate = ""

            End If


        End Function


        Function BuildOrganizationVERCORR(ByVal id_ejecutor As Integer, ByVal strNombre As String, ByVal id_notificacion As Integer, ByVal tp_orden As Integer)
            '******************construye una organización individual con toda su información por notificar********************

            ' <!--##CONTENT##-->;<!--##NOMBRE_ORGANIZACION##-->;<!--##CONVENIO##-->
            Dim strOrganization As String = get_t_templateField("tp_script", "id_notification", id_notificacion, "tp_orden", tp_orden)
            Dim tbl_Convenios As DataTable
            Dim strconvenios As String = ""


            Dim strSQL = String.Format(" select distinct id_ejecutor, id_ficha_proyecto, codigo_SAPME  " &
                                       "    from vw_ver_corr_solicitud     " &
                                       "       where id_ejecutor = {0}  " &
                                       "    order by codigo_SAPME ", id_ejecutor)

            strOrganization = strOrganization.Replace("<!--##NOMBRE_ORGANIZACION##-->", strNombre)
            strOrganization = strOrganization.Replace("<!--##ID_EJEC##-->", id_ejecutor)
            strOrganization = strOrganization.Replace("<!--##ID_PROY##-->", id_proyecto)

            '*********************************************ESTE GENERA  UN DETALLE GENERAL CON TODOS LOS BENEFICIARIOS********************************************
            tbl_Convenios = cl_utl.setObjeto("vw_ver_corr_solicitud", "id_ejecutor", id_ejecutor, strSQL).Copy
            For Each dt As DataRow In tbl_Convenios.Rows
                strconvenios &= get_t_templateField("tp_content", "id_notification", id_notificacion, "tp_orden", tp_orden).Replace("<!--##CONVENIO##-->", BuildConvenioVERCORR(dt.Item("id_ficha_proyecto"), id_notificacion, 2))
                strconvenios &= get_t_templateField("tp_nwline", "id_notification", id_notificacion, "tp_orden", tp_orden)
            Next
            '*********************************************ESTE GENERA  UN DETALLE GENERAL CON TODOS LOS BENEFICIARIOS********************************************


            strOrganization = strOrganization.Replace("<!--##CONTENT##-->", strconvenios)
            BuildOrganizationVERCORR = strOrganization

        End Function



        Function BuildConvenioVERCORR(ByVal id_ficha As Integer, ByVal id_notification As Integer, ByVal tp_orden As Integer)
            'tiene la estrucutura principal de como irá notificado cada convenio

            Dim strConvenio As String = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", tp_orden)
            strConvenio = strConvenio.Replace("<!--##CONVENIO##-->", BuildConvenioInfo(id_ficha, id_notification, 3))
            strConvenio = strConvenio.Replace("<!--##BENEFICIARIO##--> ", BuildBeneficiarioInfoVERCORR(id_ficha, id_notification, 4))
            BuildConvenioVERCORR = strConvenio

        End Function

        Function BuildBeneficiarioInfoVERCORR(ByVal id_ficha As Integer, ByVal id_notification As Integer, ByVal tp_orden As Integer)

            Dim tbl_beneficiario As DataTable
            Dim strBeneficiario As String
            Dim strBeneficiarioContent As String
            Dim strBeneficiarioLine As String
            Dim strSQL As String = String.Format(" select * from vw_ver_corr_solicitud " &
                                                  "  where id_ficha_proyecto = {0}  ", id_ficha)

            tbl_beneficiario = cl_utl.setObjeto("vw_ver_corr_solicitud", "id_ficha_proyecto", id_ficha, strSQL).Copy
            strBeneficiario = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", tp_orden)

            strBeneficiarioContent = ""

            For Each dt As DataRow In tbl_beneficiario.Rows
                strBeneficiarioLine = get_t_templateField("tp_content", "id_notification", id_notification, "tp_orden", tp_orden)
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##CEDULA##-->", dt.Item("ID"))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##NOMBRE##-->", dt.Item("nombre"))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##MUNICIPIO##-->", dt.Item("municipio"))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##DEPARTAMENTO##-->", dt.Item("departamento"))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##CREADO##-->", String.Format("'{0:dd/MM/yyyy HH:mm:ss}'", dt.Item("creado")))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##MODIFICADO##-->", String.Format("'{0:dd/MM/yyyy HH:mm:ss}'", dt.Item("modificado")))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##CORREGIMIENTO##-->", dt.Item("corr_solicitud"))
                strBeneficiarioLine = strBeneficiarioLine.Replace("<!--##VEREDA##-->", dt.Item("ver_solicitud"))
                strBeneficiarioContent &= strBeneficiarioLine
            Next

            strBeneficiario = strBeneficiario.Replace("<!--##CONTENT##-->", strBeneficiarioContent)

            BuildBeneficiarioInfoVERCORR = strBeneficiario

        End Function



        '***********************************************************************************************************************
        '********************************************VEREDAS CORREGIMIENTOS NOT*************************************************
        '***********************************************************************************************************************




        '*****************************************************************************************************************
        '********************************************USER MESSAGE CREATED, UPDATED**************************************************
        '*****************************************************************************************************************

        Public Function Emailing_USER_UPD(ByVal id_User As Integer, ByVal strPassword As String) As Boolean

            Dim strMess As String
            id_usuario = id_User

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
            'set_Apps_Document() 'Set all AppDoc
            set_t_usuarios(id_User)

            strMess = get_t_templateField("tp_script", "id_notification", id_notification, "tp_orden", 0) 'getting the last

            '*****************ADDING THE SUBJECTS**********************


            'Testing
            'Approval System: The document <!--##NUMBER_INSTRUMENT##--> <!--##STATE_STEP##-->


            Dim strSubject As String = get_t_notificationFIELDS("nt_subject", "id_notification", id_notification)

            ObJemail.AddSubject(strSubject) 'Subject

            '*****************FINDING the destinataries **************************************
            ObJemail.AddTo(get_t_usuariosFIELDS("email_usuario", "id_usuario", id_usuario)) 'email
            ' ObJemail.AddBcc("grivera@colombiaresponde-ns.org") 'Just for testing
            '*****************FINDING the destinataries **************************************

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

            '**************At this part we neeed to start to replace the Documetn info for this email*******************

            ' <!--##USER_NAME##-->;<!--##JOB_TITTLE##-->;<!--##USER##-->;<!--##PASSWORD##-->;<!--##SUPERVISOR##--> ;<!--##SYS_PATH##-->;<!--##SYS_NAME##-->
            strMess = strMess.Replace("<!--##PROJECT_NAME##-->", get_t_usuariosFIELDS("nombre_programa", "id_usuario", id_usuario))
            strMess = strMess.Replace("<!--##USER_NAME##-->", get_t_usuariosFIELDS("nombre_usuario", "id_usuario", id_usuario))
            strMess = strMess.Replace("<!--##JOB_TITTLE##-->", get_t_usuariosFIELDS("job", "id_usuario", id_usuario))

            strMess = strMess.Replace("<!--##USER##-->", get_t_usuariosFIELDS("usuario", "id_usuario", id_usuario))
            strMess = strMess.Replace("<!--##PASSWORD##-->", strPassword)

            'Dim vFecha As String = dateUtil.set_DateFormat(CDate(get_Apps_DocumentField("fecha_Aprobacion", "id_documento", id_documento, "id_App_documento", id_AppDocumento)), "f")
            'strMess = strMess.Replace("<!--##DATE_RECEIPT##-->", vFecha)

            strMess = strMess.Replace("<!--##SYS_PATH##-->", cProgram.getSysField("sys_url", "id_sys", cAPP_sys)) 'Get INFO For Approval System
            strMess = strMess.Replace("<!--##SYS_NAME##-->", cProgram.getSysField("prefix_n", "id_sys", cAPP_sys)) 'Get INFO For Approval System

            '************************* THE message it supposed Is here, need to proceed to test*****************
            '*****************building the email from the templates objects**************************************

            'strMess = strMess.Replace("<!--##IMG_CID1##-->", "LogProgram") 'Name of the resource the Program Logo
            strMess = strMess.Replace("<!--##IMG_CID2##-->", "LogChemo") 'Name of the resource the Company logo

            Dim id_usuarioPadre = get_t_usuariosFIELDS("id_usuario_padre", "id_usuario", id_usuario)

            If id_usuarioPadre = 0 Then

                strMess = strMess.Replace("<!--##SUPERVISOR##-->", "     ----     ")

            Else

                set_t_usuarios(id_usuarioPadre)
                strMess = strMess.Replace("<!--##SUPERVISOR##-->", get_t_usuariosFIELDS("nombre_usuario", "id_usuario", id_usuario) & " (" & get_t_usuariosFIELDS("job", "id_usuario", id_usuario) & ")")

            End If

            ObJemail.setAlternativeVIEW(strMess) 'Setting alternative View

            'ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\logo_Chemonics_nw.png", "image/png")
            ObJemail.Add_LinkResources("LogChemo", Server.MapPath("~") + "\Images\Activities\LIF_logo_OPT1.png", "image/png")

            ObJemail.SetPriority(MailPriority.High)

            If ObJemail.SendEmail(id_notification) Then
                Emailing_USER_UPD = True
            Else
                Emailing_USER_UPD = False
            End If

        End Function


        '*****************************************************************************************************************
        '********************************************USER MESSAGE CREATED, UPDATED**************************************************
        '*****************************************************************************************************************





#End Region



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




        '********************************************************* t_template ENTITY ************************************************************************************
        '********************************************************* t_template  ENTITY ************************************************************************************

        Public Function set_t_template(ByVal id_templ As Integer) As DataTable

            id_template = IIf(id_templ > 0, id_templ, 0)
            tbl_t_template = cl_utl.setObjeto("t_template", "id_template", id_template)
            set_t_template = tbl_t_template

        End Function

        Public Function get_t_templateFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_t_template, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_t_templateFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_t_template = cl_utl.setDTval(tbl_t_template, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_t_template() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("t_template", tbl_t_template, "id_template", id_template)

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



        '********************************************************* t_usuarios ENTITY ************************************************************************************
        '********************************************************* t_usuarios  ENTITY ************************************************************************************

        Public Function set_t_usuarios(ByVal id_usr As Integer) As DataTable

            id_usuario = IIf(id_usr > 0, id_usr, 0)
            Dim strSql As String = String.Format("select * from  vw_t_usuarios  where id_usuario = {0} And id_programa = {1} ", id_usuario, id_programa)

            tbl_t_usuarios = cl_utl.setObjeto("t_usuarios", "id_usuario", id_usuario, strSql)
            set_t_usuarios = tbl_t_usuarios

        End Function

        Public Function get_t_usuariosFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_t_usuarios, campoSearch, campo, valorSearch)

        End Function

        Public Sub set_t_usuariosFIELDS(ByVal campo As String, ByVal valor As String, campoSearch As String, valorSearch As String)

            tbl_t_usuarios = cl_utl.setDTval(tbl_t_usuarios, campoSearch, campo, valorSearch, valor)

        End Sub


        Public Function save_t_usuarios() As Integer

            Dim RES As Integer
            RES = cl_utl.SaveObjeto("t_usuarios", tbl_t_usuarios, "id_usuario", id_usuario)

            If RES <> -1 Then
                set_t_usuariosFIELDS("id_usuario", RES, "id_usuario", id_usuario)
                id_usuario = RES
                save_t_usuarios = RES
            Else
                save_t_usuarios = RES
            End If

        End Function

        Public Function del_t_usuarios(ByVal idTemp As Integer) As Boolean

            If idTemp > 0 Then

                Sql = String.Format("DELETE FROM t_usuarios WHERE (id_usuario = {0} ) ", idTemp)

                Try

                    cnnSAP.Open()
                    Dim dm As New SqlCommand(Sql, cnnSAP)
                    dm.ExecuteNonQuery()
                    cnnSAP.Close()

                    del_t_usuarios = True

                Catch ex As Exception
                    del_t_usuarios = False
                End Try

            Else

                del_t_usuarios = False

            End If

        End Function



        Public Function get_t_usuariosField(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String, Optional ByVal campoSearchAux As String = "", Optional ByVal valorSearchAux As String = "") As String

            Return cl_utl.getDTval(Tbl_t_usuarios, campoSearch, campo, valorSearch, campoSearchAux, valorSearchAux)

        End Function

        Public Function set_t_usuarios() As DataTable

            Tbl_t_usuarios = cl_utl.setObjeto("t_usuarios", "id_notification", id_notification).Copy

            set_t_usuarios = Tbl_t_usuarios

            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************
            If (set_t_usuarios.Rows.Count = 1 And set_t_usuarios.Rows.Item(0).Item("id_usuario") = 0) Then
                set_t_usuarios.Rows.Remove(set_t_usuarios.Rows.Item(0))
            End If
            '****************************************PATCH TO RETURN AN EMPTY TABLE******************************************

        End Function



        '********************************************************* t_usuarios  ENTITY ************************************************************************************
        '********************************************************* t_usuarios  ENTITY ************************************************************************************


        Public Function set_t_programa() As DataTable

            tbl_t_programa = cl_utl.setObjeto("vw_t_programas", "id_programa", id_programa).Copy

            set_t_programa = tbl_t_programa


        End Function

        Public Function get_t_productsFIELDS(ByVal campo As String, ByVal campoSearch As String, ByVal valorSearch As String) As String

            Return cl_utl.getDTval(tbl_t_entregable, campoSearch, campo, valorSearch)

        End Function

        Public Function set_t_entregable(ByVal id_entregable As Integer) As DataTable

            Dim strSql As String = String.Format("select * from  vw_contratos_entregables  where id_contrato_entregable = {0} ", id_entregable)

            tbl_t_entregable = cl_utl.setObjeto("vw_contratos_entregables", "id_hito", id_entregable, strSql)
            set_t_entregable = tbl_t_entregable

        End Function

        Public Function set_T_RutaEntregable(ByVal id_entregable As Integer) As DataTable

            Dim strSql As String = String.Format("select * from vw_ruta_aprobacion_entregable where id_contrato_entregable = {0} ", id_entregable)

            tbl_t_ruta_entregable = cl_utl.setObjeto("vw_ruta_aprobacion_entregable", "id_contrato_entregable", id_entregable, strSql)
            set_T_RutaEntregable = tbl_t_ruta_entregable

        End Function


        Public Function set_T_ComentariosEntregable(ByVal id_entregable As Integer) As DataTable

            Dim strSql As String = String.Format("select id_contrato_entregable, id_ruta_aprobacion_entregable, isnull(Convert(varchar(40),fecha_envio),'--') as fecha_envio_text, usuario, 
                isnull(comentarios,'') comentarios,  '' soporte from vw_ruta_aprobacion_entregable where id_contrato_entregable = {0}   order by fecha_envio asc", id_entregable)

            tbl_t_comentarios_entregable = cl_utl.setObjeto("vw_ruta_aprobacion_entregable", "id_contrato_entregable", id_entregable, strSql)
            set_T_ComentariosEntregable = tbl_t_comentarios_entregable

        End Function

    End Class

End Namespace
