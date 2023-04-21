Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports System.Drawing
Imports System.Net.Mail
Imports System.Net
Imports ly_APPROVAL
Imports ly_RMS
Imports System.Globalization


Partial Class frm_seguimientoNotificacion
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim idEstadoDoc As Integer
    Dim objEmail As APPROVAL.cls_notification
    Dim clss_approval As APPROVAL.clss_approval


    Const cAction_ByProcess = 1
    Const cAction_ByMessage = 2


    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_APPROVAL_COMMENT"
    Dim controles As New ly_SIME.CORE.cls_controles

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

        If Not IsPostBack Then

            ' cnnSAP.Open()
            Me.lblIDocumento.Text = Me.Request.QueryString("IdDoc").ToString
            Me.lblIdRuta.Text = Me.Request.QueryString("IdRuta").ToString

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            'Dim Sql = "SELECT * FROM vw_ta_documentos WHERE id_documento=" & Me.lblIDocumento.Text
            'Dim Sql = "select * from VW_GR_TA_DOCUMENTOS WHERE id_documento=" & Me.lblIDocumento.Text
            'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds As New DataSet("proyecto")
            'dm.Fill(ds, "proyecto")

            Dim tbl_Doc As New DataTable
            tbl_Doc = clss_approval.get_DocumentINFO(Me.lblIDocumento.Text)

            Dim USerAllowed As String() = tbl_Doc.Rows.Item(0).Item("IdUserParticipate").ToString.Split(",")
            Dim indx As Integer = USerAllowed.ToList().IndexOf(Me.Session("E_IDUser"))

            Dim boolAcces As Boolean = Convert.ToBoolean(Val(Me.h_Filter.Value))

            If (indx = -1) Then '--The User is not Allowed
                If Not boolAcces Then
                    Me.Response.Redirect("~/Proyectos/no_access2_app")
                End If
            End If

            'Me.lbl_categoria.Text = ds.Tables("proyecto").Rows(0)("descripcion_cat").ToString
            'Me.lbl_aprobacion.Text = ds.Tables("proyecto").Rows(0)("descripcion_aprobacion").ToString
            'Me.lbl_nivelaprobacion.Text = ds.Tables("proyecto").Rows(0)("nivel_aprobacion").ToString
            'Me.lbl_condicion.Text = ds.Tables("proyecto").Rows(0)("condicion").ToString
            'Me.lbl_proceso.Text = ds.Tables("proyecto").Rows(0)("descripcion_doc").ToString
            'Me.lbl_codigo.Text = ds.Tables("proyecto").Rows(0)("codigo_AID").ToString
            'Me.lbl_instrumento.Text = ds.Tables("proyecto").Rows(0)("numero_instrumento").ToString
            'Me.lbl_beneficiario.Text = ds.Tables("proyecto").Rows(0)("nom_beneficiario").ToString

            'Me.lbl_status.Text = ds.Tables("proyecto").Rows(0)("descripcion_estado").ToString
            'Me.lbl_IdCodigoAPP.Text = ds.Tables("proyecto")(0)("codigo_Approval").ToString
            'Me.lbl_IdCodigoSAP.Text = ds.Tables("proyecto")(0)("codigo_SAP_APP").ToString

            Me.lbl_categoria.Text = tbl_Doc.Rows.Item(0).Item("descripcion_cat").ToString
            Me.lbl_aprobacion.Text = tbl_Doc.Rows.Item(0).Item("descripcion_aprobacion").ToString
            Me.lbl_nivelaprobacion.Text = tbl_Doc.Rows.Item(0).Item("nivel_aprobacion").ToString
            Me.lbl_condicion.Text = tbl_Doc.Rows.Item(0).Item("condicion").ToString
            Me.lbl_proceso.Text = tbl_Doc.Rows.Item(0).Item("descripcion_doc").ToString
            Me.lbl_instrumento.Text = tbl_Doc.Rows.Item(0).Item("numero_instrumento").ToString
            Me.lbl_beneficiario.Text = tbl_Doc.Rows.Item(0).Item("nom_beneficiario").ToString

            ' lblt_subtitulo_pantalla.Text = tbl_Doc.Rows.Item(0).Item("descripcion_doc").ToString
            Me.lbl_codigo.Text = tbl_Doc.Rows.Item(0).Item("codigo_AID").ToString

            Me.lbl_Comment.Text = tbl_Doc.Rows.Item(0).Item("comentarios").ToString '.Replace("''", "'")
            Me.lbl_datecreated.Text = getFecha(tbl_Doc.Rows.Item(0).Item("datecreated"), "f", True)
            'Me.lbl_IdCodigoAPP.Text = tbl_Doc.Rows.Item(0).Item("codigo_Approval").ToString
            'Me.lbl_IdCodigoSAP.Text = tbl_Doc.Rows.Item(0).Item("codigo_SAP_APP").ToString
            'Me.lbl_status.Text = tbl_Doc.Rows.Item(0).Item("descripcion_estado").ToString
            Me.lbl_region.Text = tbl_Doc.Rows.Item(0).Item("regional").ToString

            Me.lbl_tipoDocumento.Text = tbl_Doc.Rows.Item(0).Item("nombreTipoAprobacion").ToString


            Dim dValue As Decimal
            dValue = tbl_Doc.Rows.Item(0).Item("monto_ficha")
            Me.lbl_montoProyecto.Text = dValue.ToString("c2", cl_user.regionalizacionCulture)

            dValue = tbl_Doc.Rows.Item(0).Item("monto_total")
            Me.lbl_montoTotal.Text = dValue.ToString("c2", cl_user.regionalizacionCulture)

            Me.lbl_tasaCambio.Text = tbl_Doc.Rows.Item(0).Item("tasa_cambio").ToString
            Me.lbl_createdby.Text = tbl_Doc.Rows.Item(0).Item("Originador").ToString
            Me.lbl_approvedby.Text = tbl_Doc.Rows.Item(0).Item("propietario").ToString
            Me.lbl_dateapproved.Text = getFecha(tbl_Doc.Rows.Item(0).Item("fecha_aprobacion"), "f", True)

            If tbl_Doc.Rows.Item(0).Item("id_estadoDoc").ToString = "1" Then
                Me.lbl_ErrApprovedBy.Visible = True
            End If

            'Me.SqlDataSource2.SelectCommand = "SELECT * FROM dbo.FN_Ta_RutaSeguimiento(" & Me.lblIDocumento.Text & ") ORDER BY ORDEN"
            Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(Me.lblIDocumento.Text)
            Me.grd_cate.DataBind()

            Me.grd_Document.DataSource = clss_approval.get_Document(Me.lblIDocumento.Text)
            Me.grd_Document.DataBind()

            ' Me.SqlDataSource3.SelectCommand = "SELECT  ROW_NUMBER() OVER(ORDER BY id_archivo DESC) as No,* FROM vw_ta_archivos_documento WHERE id_documento=" & Me.lblIDocumento.Text & " ORDER BY step ASC, fecha_recepcion ASC" '& " AND id_ruta= " & Me.lblIdRuta.Text
            'Me.SqlDataSource3.SelectCommand = "SELECT * FROM VW_GR_TA_ARCHIVOS_DOCUMENTO WHERE id_documento=" & Me.lblIDocumento.Text & " ORDER BY step ASC, fecha_recepcion ASC" '& " AND id_ruta= " & Me.lblIdRuta.Text

            objEmail = New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.lblIDocumento.Text, 6, cl_user.regionalizacionCulture)
            objEmail.set_Apps_Document(Me.lblIdRuta.Text)

            'Sql = "SELECT id_App_Documento, id_estadoDoc, descripcion_estado FROM vw_ta_AppDocumento WHERE id_documento=" & Me.lblIDocumento.Text & " AND id_ruta= " & Me.lblIdRuta.Text
            'ds.Tables.Add("AppDocs")
            'dm.SelectCommand.CommandText = Sql
            'dm.SelectCommand.ExecuteNonQuery()
            'dm.Fill(ds, "AppDocs")
            'cnnSAP.Close()

            'ds.Tables("AppDocs").Rows.Count = 0
            If objEmail.Apps_Document_Count() = 0 Then
                Me.lbl_status2.Text = ""
                Me.lblAppIDocumento.Text = "0"
                idEstadoDoc = 0
            Else
                Me.lbl_status2.Text = objEmail.get_Apps_DocumentField("descripcion_estado", "id_documento", Me.lblIDocumento.Text) 'ds.Tables("AppDocs").Rows(0).Item("descripcion_estado")
                idEstadoDoc = objEmail.get_Apps_DocumentField("id_estadoDoc", "id_documento", Me.lblIDocumento.Text) 'ds.Tables("AppDocs").Rows(0).Item("id_estadoDoc")
                Me.lblAppIDocumento.Text = objEmail.get_Apps_DocumentField("id_App_Documento", "id_documento", Me.lblIDocumento.Text) 'ds.Tables("AppDocs").Rows(0)("id_App_Documento").ToString
            End If

            'Me.grd_cate.DataBind()

            'Me.SqlDataSource1.SelectCommand = "SELECT ROW_NUMBER() OVER(order by step ASC, fecha_comentario ASC) as Number,* FROM vw_ta_comentariosDoc WHERE id_documento=" & Me.lblIDocumento.Text & " ORDER BY step ASC, fecha_comentario ASC"    'id_App_Documento=" & Me.lblAppIDocumento.Text & " ORDER BY fecha_comentario DESC"
            grd_comment.DataSource = objEmail.get_Document_Comments()
            Me.grd_comment.DataBind()

            'If Me.grd_comment.Items.Count > 0 Then
            '    Me.grd_comment.Items(0).BackColor = Color.FromArgb(227, 132, 67)
            'End If


            Me.Session("E_bnd_var_01") = idEstadoDoc

            'If Me.lbl_status2.Text <> "Pending" Then
            '    Me.lblcomentarios.Visible = False
            '    Me.txtresp.Visible = False
            '    Me.btn_guardar.Visible = False
            'End If

            '******************************RAD EDITOR***********************
            '*****************ENABLE
            'RadEditor1.Enabled = True
            'RadEditor1.Style.Remove("overflow")
            '*****************DISABLED
            'RadEditor1.Enabled = False
            'RadEditor1.Style.Add("width", "680px")
            'RadEditor1.Style.Add("height", "515px")
            'RadEditor1.Style.Add("overflow", "auto")
            '*****************DEFAULT TOOLBAR
            'RadEditor1.ToolsFile = Nothing


            RadEditor1.CssFiles.Add("~/App_Themes/Default/CustomStyles.css")



            '***************************MODULES*****************************
            'RadEditor1.Modules.Clear()
            'Dim moduleStatistics As New EditorModule()
            'moduleStatistics.Name = "RadEditorStatistics"
            'RadEditor1.Modules.Add(moduleStatistics)
            'Dim moduleDomInspector As New EditorModule()
            'moduleDomInspector.Name = "RadEditorDomInspector"
            'RadEditor1.Modules.Add(moduleDomInspector)
            'Dim moduleNodeInspector As New EditorModule()
            'moduleNodeInspector.Name = "RadEditorNodeInspector"
            'RadEditor1.Modules.Add(moduleNodeInspector)
            '***************************MODULES*****************************
            '*****************EDIT MODE
            'RadEditor1.EditModes = RadEditor1.EditModes Xor Telerik.Web.UI.EditModes.Design
            'RadEditor1.EditModes = RadEditor1.EditModes Xor Telerik.Web.UI.EditModes.Html
            ' RadEditor1.EditModes = RadEditor1.EditModes Xor Telerik.Web.UI.EditModes.Preview

            'Dim transparency As Integer = 15
            'RadAjaxLoadingPanel1.Transparency = transparency

            Session.Item("cls_approval") = clss_approval
            Session.Item("cls_notification") = objEmail

        Else


            If HttpContext.Current.Session.Item("cls_approval") IsNot Nothing Then
                clss_approval = Session.Item("cls_approval")
                objEmail = Session.Item("cls_notification")
            End If
            idEstadoDoc = Me.Session("E_bnd_var_01")

        End If

    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnk As HyperLink = New HyperLink
            hlnk = CType(e.Item.FindControl("Completo"), HyperLink)
            hlnk.ToolTip = "Alert"
            hlnk.ImageUrl = "../Imagenes/Iconos/Circle_" & e.Item.Cells(7).Text.ToString & ".png"
            If e.Item.Cells(5).Text.ToString = "Pending" Then
                For i As Integer = 2 To 8
                    e.Item.Cells(i).BackColor = Color.FromArgb(227, 132, 67)
                Next
            End If

        End If
    End Sub

    Protected Sub grd_Document_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_Document.ItemDataBound


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnk As HyperLink = New HyperLink
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            '--6:  archivo 
            '--7:  AttachFile
            hlnk = CType(e.Item.FindControl("Attach"), HyperLink)
            hlnk.Target = "_blank"
            hlnk.NavigateUrl = "~/fileUploads/ApprovalProcc/" & itemD("archivo").Text  'e.Item.Cells(7).Text.ToString
            'e.Item.Cells(6).Text = "." & itemD("archivo"").Text.ToString.Substring(e.Item.Cells(7).Text.Length - 4, 4).Replace(".", "")  'e.Item.Cells(7).Text.ToString.Substring(e.Item.Cells(7).Text.Length - 4, 4).Replace(".", "")
            'e.Item.Cells(6).Text = "." & itemD("archivo"").Text.ToString.Substring(itemD("AttachFile").Text.Length - 4, 4).Replace(".", "")  'e.Item.Cells(7).Text.ToString.Substring(e.Item.Cells(7).Text.Length - 4, 4).Replace(".", "")
            itemD("extension").Text = "." & itemD("archivo").Text.ToString.Substring(itemD("archivo").Text.Length - 4, 4).Replace(".", "")  'e.Item.Cells(7).Text.ToString.Substring(e.Item.Cells(7).Text.Length - 4, 4).Replace(".", "")

        End If
    End Sub

    Protected Sub grd_comment_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grd_comment.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnk As HyperLink = New HyperLink
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            hlnk = CType(e.Item.FindControl("imgEvent"), HyperLink)
            hlnk.ToolTip = String.Format("{0} {1} ({2})", Trim(itemD("messAct").Text), Trim(itemD("empleado").Text), Trim(itemD("nombre_rol").Text))
            hlnk.ImageUrl = Trim(itemD("iconAct").Text)

            '"~/Imagenes/Iconos/flag_yellow.png"

            Dim hlnk2 As HyperLink = New HyperLink
            hlnk2 = CType(e.Item.FindControl("imgFlag"), HyperLink)
            hlnk2.ToolTip = "Indicator"
            hlnk2.ImageUrl = itemD("icon_msj").Text

        End If

    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click 'Handles btn_guardar.Click 'Handles btn_guardar.Click

        Try


            If Me.RadEditor1.Text.Trim <> "" Then

                'cnnSAP.Open()

                Dim strComm As String = Trim(RadEditor1.Content.Trim.Replace("  ", " ")) '.Replace("'", "''")
                RadEditor1.Content = ""

                'Dim SqlInsert As String = "INSERT INTO ta_comentariosDoc (id_App_Documento, id_estadoDoc, id_tipoAccion, id_usuario, comentario) 
                'VALUES (" & Me.lblAppIDocumento.Text & "," & idEstadoDoc & "," & 2 & "," & Me.Session("E_IdUser").ToString.Trim & ",'" & strComm & "') SELECT @@IDENTITY"
                'Dim dmt As New SqlDataAdapter(SqlInsert, cnnSAP)

                'Dim ds As New DataSet("IDComentario")
                'dmt.Fill(ds, "IDComentario")
                'cnnSAP.Close()

                clss_approval.set_ta_comentariosDoc(0) 'New Record
                clss_approval.set_ta_comentariosDocFIELDS("id_App_Documento", Me.lblAppIDocumento.Text, "id_comment", 0)
                clss_approval.set_ta_comentariosDocFIELDS("id_estadoDoc", idEstadoDoc, "id_comment", 0)
                clss_approval.set_ta_comentariosDocFIELDS("id_tipoAccion", cAction_ByMessage, "id_comment", 0)
                clss_approval.set_ta_comentariosDocFIELDS("id_usuario", Me.Session("E_IdUser"), "id_comment", 0)
                clss_approval.set_ta_comentariosDocFIELDS("fecha_comentario", Date.UtcNow, "id_comment", 0)
                clss_approval.set_ta_comentariosDocFIELDS("comentario", strComm, "id_comment", 0)

                Dim idComm As Integer = clss_approval.save_ta_comentariosDoc()

                If idComm = -1 Then
                    'Error do somenthing
                    lblError.Visible = True
                    lblError.Text = "An error ocurred executing the action, please contact to the System Administration: error saving the comment "
                Else

                    'Me.SqlDataSource1.SelectCommand = "SELECT ROW_NUMBER() OVER(order by step ASC, fecha_comentario ASC) as Number,* FROM vw_ta_comentariosDoc WHERE id_documento=" & Me.lblIDocumento.Text & " ORDER BY step ASC, fecha_comentario ASC"
                    Me.grd_comment.DataSource = objEmail.get_Document_Comments()
                    Me.grd_comment.DataBind()
                    'Me.grd_comment.Items(0).BackColor = Color.FromArgb(227, 132, 67)

                    ' If Not enviar_email(Me.lblIDocumento.Text, Me.lblAppIDocumento.Text, strComm.Replace("''", "'")) Then

                    If Not objEmail.Emailing_COMMENT_APPROVAL(strComm.Trim, Me.lblAppIDocumento.Text, idComm) Then

                        lblError.Visible = True
                        lblError.Text = "An error ocurred executing the action, please contact to the System Administration: error saving the comment "
                        RadEditor1.Content = strComm

                    End If


                End If

            End If

        Catch ex As Exception

            btn_guardar.Enabled = "false"
            lblError.Visible = True
            lblError.Text = "An error ocurred executing the action, please contact to the System Administration:  " & ex.Message

        End Try


    End Sub

    Function enviar_email(ByVal id_doc As Integer, ByVal id_appdoc As Integer, ByVal strComment As String) As Boolean

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

        sql = "select * from vw_empleados WHERE id_empleado=" & Me.Session("E_IdUser").ToString.Trim
        dmap.SelectCommand.CommandText = sql
        dsap.Tables.Add("Empleado")
        dmap.Fill(dsap, "Empleado")

        MensajeSend.Subject = String.Format("New comment added into the approval process {0} by {1} ({2})", dsap.Tables("appdoc").Rows(0).Item("numero_instrumento"), dsap.Tables("Empleado").Rows(0).Item("nombre_empleado"), dsap.Tables("Empleado").Rows(0).Item("nombre_rol"))


        '*****************************TABLA DE ESTADOS ******************************** 
        Dim dsP As New DataSet("dtPROCS")
        dsP.Tables.Add("dtPROCS")
        sql = "SELECT * FROM dbo.FN_Ta_RutaSeguimiento(" & id_doc & ") ORDER BY ORDEN"
        dm.SelectCommand.CommandText = sql
        dm.Fill(dsP, "dtPROCS")
        Dim strTable = makeTable(dsP.Tables("dtPROCS"), dsap.Tables("appdoc").Rows(0).Item("descripcion_estado"), id_appdoc)
        '*****************************TABLA DE ESTADOS ******************************** 

        Dim nullable As System.Text.Encoding
        Dim Mensaje = Make_Email(dsap.Tables("appdoc"), url_aplicacion, strTable, strComment, dsap.Tables("Empleado").Rows(0).Item("nombre_empleado"), dsap.Tables("Empleado").Rows(0).Item("nombre_rol"))
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

            Return True
        Catch ex As Exception
            MsgBox("ERROR: " & ex.ToString, MsgBoxStyle.Critical, "Error")
            Return False

        End Try

    End Function

    Public Function Make_Email(ByVal dtData As DataTable, ByVal url_aplicacion As String, ByVal strTABLE As String, ByVal strComment As String, ByVal strNombreEmpleado As String, ByVal strNombreRol As String) As String

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
                      "            This message has been sent from the Results Management System CHEMONICS <br /><br />" & _
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
                             strNombreEmpleado & "   </td>" & _
                               "  </tr>" & _
                               "   <tr>" & _
                               "      <td style='padding: 3px;border-color:#CECECE;background-color: #CECECE; font-weight: bold;font-family: Arial, Helvetica, sans-serif;font-size: small; width:20%;'>" & _
                               "         POSITION:</td>" & _
                               " <td style='border-color:#FFFFFF;padding: 5px; font-family:Verdana, Arial; font-size: x-small; border-bottom-color:#CECECE; border:1px dotted #ED7620;width:80%;'> " & _
                             strNombreRol & "   </td>" & _
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
                                  "      VIEW A FULL HISTORY:</td>" & _
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
                               Trim(strComment) & _
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



    Public Sub enviar_emailOLD(ByVal id_doc As Integer, ByVal id_appdoc As Integer, ByVal id_comentario As Integer)

        Dim sql = "SELECT * FROM t_config_email WHERE id_proyecto=" & Me.Session("E_IdProy")
        Dim dm As New SqlDataAdapter(sql, cnnSAP)
        Dim ds As New DataSet("email")
        dm.Fill(ds, "email")
        Dim i As Integer
        Dim destinatario = ""
        Dim destinatarioAdmin = ds.Tables("email").Rows(0).Item("email_admin").ToString
        Dim port = ds.Tables("email").Rows(0).Item("puerto_SMTP2").ToString
        Dim account = ds.Tables("email").Rows(0).Item("email2").ToString
        Dim pass = ds.Tables("email").Rows(0).Item("password2").ToString
        Dim smtp = ds.Tables("email").Rows(0).Item("SMTP2").ToString

        sql = "SELECT orden FROM  vw_ta_AppDocumento WHERE id_App_Documento=" & id_appdoc
        ds.Tables.Add("Orden")
        dm.SelectCommand.CommandText = sql
        dm.Fill(ds, "Orden")
        Dim Orden = ds.Tables("Orden").Rows(0).Item("orden")

        'sql = "SELECT email FROM vw_ta_roles_emails WHERE id_documento=" & id_doc & " ORDER BY orden "

        sql = "  SELECT email, nombre_rol, id_rol FROM vw_ta_roles_emails "
        sql &= " WHERE (id_documento = " & id_doc & ") AND (orden = 0 OR orden =(" & Orden & " - 1) OR  orden = " & Orden & ")"
        sql &= " UNION "
        sql &= " SELECT email, nombre_rol, id_rol FROM vw_ta_email_gruposRoles WHERE id_rol IN("
        sql &= " SELECT  id_rol FROM vw_ta_roles_emails AS A "
        sql &= " WHERE (id_documento = " & id_doc & ") AND (orden = 0 OR orden =(" & Orden & " - 1) OR  orden = " & Orden & "))"

        Dim dm1 As New SqlDataAdapter(sql, cnnSAP)
        Dim ds1 As New DataSet("instrumento")
        dm1.Fill(ds1, "instrumento")

        Dim SendFrom As New MailAddress(ds.Tables("email").Rows(0).Item("email_noreplay").ToString, "System SAP-Approvals CHEMONICS")
        Dim SendTo As New MailAddress(ds.Tables("email").Rows(0).Item("BCC").ToString)
        Dim MensajeSend As New MailMessage(SendFrom, SendTo)
        MensajeSend.Subject = "New Comment"

        Dim emails_cco() As String = ds.Tables("email").Rows(0).Item("BCO").ToString.Split(";")
        For i = 0 To emails_cco.Count() - 1
            If emails_cco(i).ToString <> "" Then
                MensajeSend.Bcc.Add(emails_cco(i).ToString)
            End If
        Next

        'MensajeSend.Bcc.Add(destinatarioAdmin)

        For i = 0 To ds1.Tables("instrumento").Rows.Count() - 1
            MensajeSend.To.Add(ds1.Tables("instrumento").Rows(i).Item("email"))
        Next

        sql = "SELECT * FROM vw_ta_comentariosDoc WHERE id_coment=" & id_comentario
        Dim dmapComent As New SqlDataAdapter(sql, cnnSAP)
        Dim dsapComent As New DataSet("dmapComent")
        dmapComent.Fill(dsapComent, "dmapComent")

        sql = "SELECT * FROM vw_ta_AppDocumento WHERE id_App_Documento=" & id_appdoc
        Dim dmap As New SqlDataAdapter(sql, cnnSAP)
        Dim dsap As New DataSet("appdoc")
        dmap.Fill(dsap, "appdoc")

        Dim Mensaje As String = "<html><body>"
        Mensaje &= "<style type='text/css'><!--.Estilo1 {color: #0000CC}.Estilo2 {color: #0000FF}--></style>"

        Mensaje &= "<table border='1' cellpadding='0' cellspacing='0' bordercolor='#000033'>"

        Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #FFFFFF' >"
        Mensaje &= "<div align='righ'><b>Proyect: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF' >" & dsap.Tables("appdoc").Rows(0).Item("nombre_proyecto") & "</th></tr>"

        Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #000000' >"
        Mensaje &= "<div align='righ'><b>Name of Category: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000' >" & dsap.Tables("appdoc").Rows(0).Item("descripcion_cat") & "</th></tr>"

        Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #FFFFFF' >"
        Mensaje &= "<div align='righ'><b>Approval: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF' >" & dsap.Tables("appdoc").Rows(0).Item("descripcion_aprobacion") & "</th></tr>"

        Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #000000' >"
        Mensaje &= "<div align='righ'><b>Instrument Number: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000' >" & dsap.Tables("appdoc").Rows(0).Item("numero_instrumento") & "</th></tr>"

        Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #FFFFFF'>"
        Mensaje &= "<div align='righ'><b>Beneficiary Name: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF' >" & dsap.Tables("appdoc").Rows(0).Item("nom_beneficiario") & "</th></tr>"

        Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #000000' >"
        Mensaje &= "<div align='righ'><b>Name of process: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000' >" & dsap.Tables("appdoc").Rows(0).Item("descripcion_doc") & "</th></tr>"

        Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #FFFFFF'>"
        Mensaje &= "<div align='righ'><b>Code of Process: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF' >" & dsap.Tables("appdoc").Rows(0).Item("codigo_AID") & "</th></tr>"

        Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif;color: #000000' >"
        Mensaje &= "<div align='righ'><b>Code SAP/APPROVAL: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000' >" & dsap.Tables("appdoc").Rows(0).Item("codigo_SAP_APP") & "</th></tr>"

        Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #FFFFFF' >"
        Mensaje &= "<div align='righ'><b>Report create by: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF' >" & dsapComent.Tables("dmapComent").Rows(0).Item("empleado") & "</th></tr>"

        Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #000000' >"
        Mensaje &= "<div align='righ'><b>Date of receipt: </b></div></th><th bordercolor='#FFFFFF' bgcolor='#FFFFFF' style='color: #000000' >" & dsapComent.Tables("dmapComent").Rows(0).Item("fecha_comentario") & "</th></tr>"

        Mensaje &= "<tr><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='font-size: 13px;font-family: Arial, Helvetica, sans-serif; color: #FFFFFF'>"
        Mensaje &= "<div align='righ'><b>Comments</b></div></th><th bordercolor='#FFFFFF' bgcolor='#A4A4A4' style='color: #FFFFFF' >" & dsapComent.Tables("dmapComent").Rows(0).Item("comentario") & "</th></tr></table>"

        Mensaje &= "<p><font face='Arial' style='font-size: 11px';font-weight: bold>This is an automatically generated email, please do not reply<br>"
        Mensaje &= "<em><strong>WARNING</strong>: <em>This message contains information legally protected. If you have received this message by mistake, please avoid checking, distributing, copying, reproduction or misuse of the information within it and reply to the sender informing about the misunderstanding.</em></p>"

        Mensaje &= "</body></html>"

        MensajeSend.IsBodyHtml = True
        MensajeSend.Body = Mensaje
        MensajeSend.Priority = MailPriority.High
        Dim CorreoSend As New SmtpClient(smtp, port)
        CorreoSend.Credentials = New NetworkCredential(account, pass)
        CorreoSend.EnableSsl = False

        Try
            CorreoSend.Send(MensajeSend)
        Catch ex As Exception
            Me.lblError.Visible = True
        End Try
    End Sub

    'Protected Sub bntlk_print_Click(sender As Object, e As EventArgs) Handles bntlk_printing.Click
    '    'Handles btn_print.Click
    '    'Handles bntlk_printing.Click

    '    'OnClientClick="javascript:window.open('frm_seguimientoAprobacionMessRep.aspx','_blank');"
    '    'Dim script As String = String.Format("<script type=""text/javascript"">window.open('frm_seguimientoAprobacionMessRep.aspx?IdDoc={0}&IdRuta={1}','_blank');</script>", Me.lblIDocumento.Text, Me.lblIdRuta.Text)

    '    Dim script As String = String.Format("window.open('frm_seguimientoAprobacionMessRep.aspx?IdDoc={0}&IdRuta={1}','_blank');", Me.lblIDocumento.Text, Me.lblIdRuta.Text)
    '    ScriptManager.RegisterStartupScript(Page, Page.GetType, "openWindow", script, True)

    'End Sub

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

    Private Sub btnlk_printing__Click(sender As Object, e As EventArgs) Handles btnlk_printing_.Click

        Dim URLrequest As String = String.Format("~/approvals/frm_seguimientoAprobacionMessRep.aspx?IdDoc={0}&IdRuta={1}", Me.lblIDocumento.Text, Me.lblIdRuta.Text)
        Me.Response.Redirect(URLrequest)

    End Sub
End Class
