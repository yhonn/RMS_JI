Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Globalization
Imports ly_RMS

Partial Class frm_EnvironmentalApprovals

    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ENV_DOCS_EDIT"
    Dim controles As New ly_SIME.CORE.cls_controles

    Const cPENDING = 1
    Const cAPPROVED = 2

    ' Dim cl_Doc_supp As APPROVAL.cl_Doc_support
    Dim cl_envir_app As APPROVAL.cl_environmentalAPP

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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_archivos)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            'If HttpContext.Current.Session.Item("cl_Doc_supp") IsNot Nothing Then
            '   cl_Doc_supp = Session.Item("cl_Doc_supp")
            'Else
            'End If

            hd_Id_doc.Value = Convert.ToInt32(Me.Request.QueryString("IdDoc").ToString)

            'cl_Doc_supp = New APPROVAL.cl_Doc_support(Me.Session("E_IDPrograma"))
            cl_envir_app = New APPROVAL.cl_environmentalAPP(Me.Session("E_IDPrograma"), hd_Id_doc.Value)

            LoadData()

            Me.lbl_oldFile.Value = "--none--"
            HttpContext.Current.Session.Add("cl_envir_app", cl_envir_app)

        Else

            If HttpContext.Current.Session.Item("cl_envir_app") IsNot Nothing Then
                cl_envir_app = Session.Item("cl_envir_app")
            End If

        End If


    End Sub


    Public Sub LoadData()

        cl_envir_app.get_vw_t_documento_ambiental(hd_Id_doc.Value)
        lbl_InstrumentNumber.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("numero_instrumento", "id_documento_ambiental", hd_Id_doc.Value)
        lbl_approvalNameD.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("descripcion_aprobacion", "id_documento_ambiental", hd_Id_doc.Value)
        lbl_beneficiaryN.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("nom_beneficiario", "id_documento_ambiental", hd_Id_doc.Value)
        lbl_created_user.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("usuario_creo", "id_documento_ambiental", hd_Id_doc.Value)

        Me.HiddenField1.Value = cl_envir_app.get_vw_t_documento_ambientalFIELDS("id_documento", "id_documento_ambiental", hd_Id_doc.Value)


        'Dim v_date As DateTime = Convert.ToDateTime(cl_envir_app.get_vw_t_documento_ambientalFIELDS("fecha_creado", "id_documento_ambiental", hd_Id_doc.Value))
        'Me.txt_finicio.SelectedDate = DateSerial(Year(v_date), Month(v_date), Day(v_date))
        '************************************SYSTEM DATE FORMAT********************************************
        'Dim timezoneUTC As Integer
        'Dim dateUtil As APPROVAL.cls_dUtil
        'Dim cProgram As New RMS.cls_Program
        'Dim userCulture As CultureInfo = cl_user.regionalizacionCulture
        'cProgram.get_Programs(2, True)
        'timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", 2))
        'dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
        '************************************SYSTEM INFO********************************************
        'lbl_created_Date.Text = dateUtil.set_DateFormat(v_date, "g", , True)

        lbl_created_Date.Text = getFecha(cl_envir_app.get_vw_t_documento_ambientalFIELDS("fecha_creado", "id_documento_ambiental", hd_Id_doc.Value), "g", True)

        'v_date = Convert.ToDateTime(cl_envir_app.get_vw_t_documento_ambientalFIELDS("fecha_upd", "id_documento_ambiental", hd_Id_doc.Value))
        If cl_envir_app.get_vw_t_documento_ambientalFIELDS("id_Estado", "id_documento_ambiental", hd_Id_doc.Value) = 1 Then

            If cl_envir_app.get_vw_t_documento_ambientalFIELDS("id_usuario_upd", "id_documento_ambiental", hd_Id_doc.Value) > 0 Then '--Not registered

                lbl_updated_user.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("usuario_upd", "id_documento_ambiental", hd_Id_doc.Value)
                ' lbl_updated_Date.Text = dateUtil.set_DateFormat(v_date, "g", , True)
                '
                lbl_updated_Date.Text = getFecha(cl_envir_app.get_vw_t_documento_ambientalFIELDS("fecha_upd", "id_documento_ambiental", hd_Id_doc.Value), "g", True)
                Me.txt_observation.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("observacion", "id_documento_ambiental", hd_Id_doc.Value)
                cmb_rev_type.SelectedValue = cl_envir_app.get_vw_t_documento_ambientalFIELDS("id_tipoApp_Environmental", "id_documento_ambiental", hd_Id_doc.Value)

            Else

                lbl_updated_user.Text = "--"
                lbl_updated_Date.Text = " "
                Me.txt_observation.EmptyMessage = cl_envir_app.get_vw_t_documento_ambientalFIELDS("observacion", "id_documento_ambiental", hd_Id_doc.Value)
                cmb_rev_type.SelectedIndex = -1

            End If

        Else


            '****************HIDE CONTROLS**************************
            btn_save.Visible = False
            btn_cancel.Visible = False
            btn_Approved.Visible = False
            grd_archivos.MasterTableView.GetColumn("colm_ELIMINAR").Visible = False
            btn_addDoc.Visible = False
            uploadFile.Visible = False
            grd_documentos.Visible = False
            '****************HIDE CONTROLS**************************

            lbl_dateUpdated.Text = "Approved"
            lbl_updated_user.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("usuario_upd", "id_documento_ambiental", hd_Id_doc.Value)
            lbl_updated_Date.Text = getFecha(cl_envir_app.get_vw_t_documento_ambientalFIELDS("fecha_upd", "id_documento_ambiental", hd_Id_doc.Value), "g", True)
            Me.txt_observation.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("observacion", "id_documento_ambiental", hd_Id_doc.Value)
            cmb_rev_type.SelectedValue = cl_envir_app.get_vw_t_documento_ambientalFIELDS("id_tipoApp_Environmental", "id_documento_ambiental", hd_Id_doc.Value)

        End If

        'Dim strEstado As String
        'If cl_envir_app.get_vw_t_documento_ambientalFIELDS("id_Estado", "id_documento_ambiental", hd_Id_doc.Value) = 1 Then 'Pending
        '    strEstado = String.Format("{0} &nbsp;<i class='fa fa-clock-o'></i>&nbsp;{1} days", cl_envir_app.get_vw_t_documento_ambientalFIELDS("estado_ambiental", "id_documento_ambiental", hd_Id_doc.Value), cl_envir_app.get_vw_t_documento_ambientalFIELDS("elapsed", "id_documento_ambiental", hd_Id_doc.Value))
        '    spn_state.InnerHtml = strEstado
        'Else
        '    strEstado = String.Format("{0} &nbsp;<i class='fa fa-lock'></i>&nbsp;", cl_envir_app.get_vw_t_documento_ambientalFIELDS("estado_ambiental", "id_documento_ambiental", hd_Id_doc.Value), cl_envir_app.get_vw_t_documento_ambientalFIELDS("elapsed", "id_documento_ambiental", hd_Id_doc.Value))
        '    spn_state.InnerHtml = strEstado
        'End If

        Dim strEstado As String
        If cl_envir_app.get_vw_t_documento_ambientalFIELDS("id_Estado", "id_documento_ambiental", hd_Id_doc.Value) = 1 Then 'Pending
            strEstado = String.Format("{0} &nbsp;<i class='fa fa-clock-o'></i>&nbsp;{1} days", cl_envir_app.get_vw_t_documento_ambientalFIELDS("estado_ambiental", "id_documento_ambiental", hd_Id_doc.Value), cl_envir_app.get_vw_t_documento_ambientalFIELDS("elapsed", "id_documento_ambiental", hd_Id_doc.Value))
            spn_state.InnerHtml = strEstado
            spn_state.Visible = True
            spn_state_approved.Visible = False
        Else
            strEstado = String.Format("{0} &nbsp;<i class='fa fa-check-circle-o'></i>&nbsp;", cl_envir_app.get_vw_t_documento_ambientalFIELDS("estado_ambiental", "id_documento_ambiental", hd_Id_doc.Value))
            spn_state_approved.InnerHtml = strEstado
            spn_state.Visible = False
            spn_state_approved.Visible = True
        End If

        grd_documentos.DataSource = cl_envir_app.get_environmental_docs_type()
        grd_documentos.DataBind()

        grd_archivos.DataSource = cl_envir_app.get_vw_enviromental_document(hd_Id_doc.Value)
        grd_archivos.DataBind()


    End Sub


    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click

        Dim err As Boolean = False


        txt_observation.Text = txt_observation.Text.Trim.Replace("---Not applicable---", "")

        If Me.cmb_rev_type.SelectedValue = 0 Then 'If is Not Applicable
            txt_observation.Text = String.Format("---{1}---{2} {0} {2}---{1}---", txt_observation.Text.Trim, Me.cmb_rev_type.Text, vbNewLine)
        End If

        If err = False Then

            cl_envir_app.set_ta_documento_ambiental(hd_Id_doc.Value)
            'cl_envir_app.set_ta_documento_ambientalFIELDS("id_estado", cAPPROVED, "id_documento_ambiental", cl_envir_app.id_documento_ambiental)
            cl_envir_app.set_ta_documento_ambientalFIELDS("observacion", txt_observation.Text, "id_documento_ambiental", cl_envir_app.id_documento_ambiental)
            cl_envir_app.set_ta_documento_ambientalFIELDS("fecha_upd", Date.UtcNow, "id_documento_ambiental", cl_envir_app.id_documento_ambiental)
            cl_envir_app.set_ta_documento_ambientalFIELDS("id_usuario_upd", Me.Session("E_IdUser"), "id_documento_ambiental", cl_envir_app.id_documento_ambiental)
            cl_envir_app.set_ta_documento_ambientalFIELDS("id_tipoApp_Environmental", cmb_rev_type.SelectedValue, "id_documento_ambiental", cl_envir_app.id_documento_ambiental)

            If cl_envir_app.save_ta_documento_ambiental() <> -1 Then

                '********************************COMPLETED DOCUMENT*********************************************************
                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 9, cl_user.regionalizacionCulture)

                If (objEmail.Emailing_ENVIRONMENTAL_APPROVAL(hd_Id_doc.Value, "Updated")) Then

                Else 'Error mandando Email

                End If
                ' ********************************COMPLETED DOCUMENT*********************************************************

                Session.Remove("cl_envir_app") 'Remove the session
                Me.Response.Redirect("~/approvals/frm_Environmental_docPending.aspx")

            End If


        End If


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


    Protected Sub btn_Approved_Click(sender As Object, e As EventArgs) Handles btn_Approved.Click



        Dim err As Boolean = False

        If String.IsNullOrEmpty(Me.cmb_rev_type.SelectedValue) Then
            err = True
            lblt_Error_Save.Text = "Error Saving the environmental approval, select a Review type"
            lblt_Error_Save.Visible = True
        End If

        If err = False Then

            If grd_archivos.Items.Count = 0 Then

                If Me.cmb_rev_type.SelectedValue > 0 Then
                    err = True
                    lblt_Error_Save.Text = "Error Saving the environmental approval, at least a file is requered"
                    lblt_Error_Save.Visible = True
                End If

            End If

        End If

        If err = False Then

            cl_envir_app.set_ta_documento_ambiental(hd_Id_doc.Value)
            cl_envir_app.set_ta_documento_ambientalFIELDS("id_estado", cAPPROVED, "id_documento_ambiental", cl_envir_app.id_documento_ambiental)
            cl_envir_app.set_ta_documento_ambientalFIELDS("observacion", txt_observation.Text, "id_documento_ambiental", cl_envir_app.id_documento_ambiental)
            cl_envir_app.set_ta_documento_ambientalFIELDS("fecha_upd", Date.UtcNow, "id_documento_ambiental", cl_envir_app.id_documento_ambiental)
            cl_envir_app.set_ta_documento_ambientalFIELDS("fecha_aprobado", Date.UtcNow, "id_documento_ambiental", cl_envir_app.id_documento_ambiental)
            cl_envir_app.set_ta_documento_ambientalFIELDS("id_usuario_upd", Me.Session("E_IdUser"), "id_documento_ambiental", cl_envir_app.id_documento_ambiental)
            cl_envir_app.set_ta_documento_ambientalFIELDS("id_tipoApp_Environmental", cmb_rev_type.SelectedValue, "id_documento_ambiental", cl_envir_app.id_documento_ambiental)

            If cl_envir_app.save_ta_documento_ambiental() <> -1 Then

                '********************************COMPLETED DOCUMENT*********************************************************
                Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 9, cl_user.regionalizacionCulture)

                If (objEmail.Emailing_ENVIRONMENTAL_APPROVAL(hd_Id_doc.Value, "Approved")) Then

                Else 'Error mandando Email

                End If
                ' ********************************COMPLETED DOCUMENT*********************************************************


                Session.Remove("cl_envir_app") 'Remove the session
                Me.Response.Redirect("~/approvals/frm_Environmental_docPending.aspx")

            End If


        End If

    End Sub




    Protected Sub file_uploaded_change(ByVal sender As Object, ByVal e As FileUploadedEventArgs)

        Dim uploader As RadAsyncUpload = DirectCast(sender, RadAsyncUpload)

        Dim fullPath As String

        Dim fuLLfiLeName As String
        Dim nAmEuPd As String

        Try

            If hd_id_tp_doc.Value > 0 Then

                fullPath = Server.MapPath("~\FileUploads\EnvironmentalApproval\")

                    Dim dmyhm As String = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
                    Dim fileNameWE '= System.IO.Path.GetFileNameWithoutExtension(fileName)
                    Dim extension As String '= System.IO.Path.GetExtension(fileName)

                    Dim version As Integer = 1

                    nAmEuPd = uploadFile.UploadedFiles.Item(0).FileName.ToString

                    If Trim(nAmEuPd) = Trim(e.File.GetName) Then 'Si es el mismo archivo que disparo el evento

                        fileNameWE = System.IO.Path.GetFileNameWithoutExtension(nAmEuPd)
                        extension = System.IO.Path.GetExtension(nAmEuPd)
                        fileNameWE = String.Format("doc{0}_0{1}_{2}_{3}_V1.{4}{5}", hd_Id_doc.Value, Me.Session("E_IdUser"), dmyhm, fileNameWE, version, extension)

                        fuLLfiLeName = System.IO.Path.Combine(fullPath, fileNameWE)

                        e.File.SaveAs(fuLLfiLeName)

                        cl_envir_app.set_ta_documento_ambiental_archivos(0)
                        cl_envir_app.set_ta_documento_ambiental_archivosFIELDS("id_documento_ambiental", hd_Id_doc.Value, "id_archivo_ambiental", cl_envir_app.id_archivo_ambiental)
                        cl_envir_app.set_ta_documento_ambiental_archivosFIELDS("id_doc_soporte", hd_id_tp_doc.Value, "id_archivo_ambiental", cl_envir_app.id_archivo_ambiental)
                        cl_envir_app.set_ta_documento_ambiental_archivosFIELDS("archivo", fileNameWE, "id_archivo_ambiental", cl_envir_app.id_archivo_ambiental)
                        cl_envir_app.set_ta_documento_ambiental_archivosFIELDS("ver", version, "id_archivo_ambiental", cl_envir_app.id_archivo_ambiental)
                        cl_envir_app.set_ta_documento_ambiental_archivosFIELDS("id_usuario", Me.Session("E_IdUser"), "id_archivo_ambiental", cl_envir_app.id_archivo_ambiental)
                        cl_envir_app.set_ta_documento_ambiental_archivosFIELDS("fecha_creado", Date.UtcNow, "id_archivo_ambiental", cl_envir_app.id_archivo_ambiental)

                        Dim idArch = cl_envir_app.save_ta_documento_ambiental_archivos()

                    If idArch = -1 Then 'Erro Happenned

                        lblt_Error_Save.Text = "Error Saving the environmental document"
                        lblt_Error_Save.Visible = True

                        lblt_ErrTypeDocFile.Text = "Error Saving the environmental document"
                        lblt_ErrTypeDocFile.Visible = True

                        btn_save.Enabled = False

                    Else '******************SAVING*******************
                        btn_addDoc.Enabled = True
                        grd_archivos.DataSource = cl_envir_app.get_vw_enviromental_document(hd_Id_doc.Value)
                        grd_archivos.DataBind()
                        grd_documentos.DataSource = cl_envir_app.get_environmental_docs_type()
                        grd_documentos.DataBind()
                        hd_id_tp_doc.Value = 0
                        lblt_ErrTypeDocFile.Visible = False
                        lblt_Error_Save.Visible = False
                    End If

                    End If



                Else

                    lblt_ErrTypeDocFile.Text = "A document type is required"
                lblt_ErrTypeDocFile.Visible = True
            End If

        Catch ex As Exception
            lblt_Error_Save.Text = ex.Message
            lblt_Error_Save.Visible = True
        End Try

    End Sub


    Sub CopyFile()

        Dim sFileName As String = Me.lbl_archivo_uploaded.Value
        Dim sFileDirTemp As String = Server.MapPath("~") & "\Temp\"
        Dim file_info As New IO.FileInfo(sFileDirTemp + sFileName)
        Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Templates\" 'This we have to get from DB

        Try

            file_info.CopyTo(sFileDir & sFileName.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_"))

        Catch ex As Exception
        End Try

    End Sub


    Sub CopyFileParam(ByVal file As String, ByVal nw_NameFile As String)

        Dim sFileDirTemp As String = Server.MapPath("~") & "\Temp\"
        Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\EnvironmentalApproval\"
        Dim file_info As New IO.FileInfo(sFileDirTemp + file)
        Try
            file_info.CopyTo(sFileDir & nw_NameFile)
        Catch ex As Exception

        End Try

        ' DelFileParam(file)

    End Sub


    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/Approvals/frm_Environmental_docPending.aspx")
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
        hd_id_tp_doc.Value = Convert.ToInt32(chkSelect.InputAttributes.Item("value"))

        ActualizaDatosDOCS()

    End Sub

    Sub ActualizaDatosDOCS()

        For Each Irow As GridDataItem In Me.grd_documentos.Items

            Dim chkvisible As CheckBox = CType(Irow("colm_select").FindControl("chkSelect"), CheckBox)

            If chkvisible.Checked = True Then
                If Irow("id_doc_soporte").Text = hd_id_tp_doc.Value Then
                    'uploadFile.AllowedFileExtensions = Irow("extension").Text
                Else
                    chkvisible.Checked = False
                End If
            End If

        Next

    End Sub

    Protected Sub grd_archivos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_archivos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim ImageDownload As New HyperLink
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            'Visible = itemD("colm_select").FindControl("chkSelect")


            ''Dim j = e.Item.Cells(6).Text.ToString
            'Dim arregloExtension() As String = itemD("extension").Text.Split(",")
            ''Dim strPrBvar As String = CType(e.Item, GridEditableItem).KeyValues

            'Dim uploader As RadAsyncUpload = CType(itemD("fileControl").FindControl("RadAsyncUpload1"), RadAsyncUpload)
            ''uploader.AllowedFileExtensions = arregloExtension
            ''uploader.RenderMode = AsyncUpload.UploadedFilesRendering.BelowFileInput
            ''uploader.MaxFileInputsCount = 1
            ''uploader.MultipleFileSelection = AsyncUpload.MultipleFileSelection.Disabled

            ImageDownload = CType(e.Item.FindControl("ImageDownload"), HyperLink)
            ImageDownload.ImageUrl = "~/Imagenes/Iconos/adjunto.png"
            ImageDownload.NavigateUrl = "~/FileUploads/EnvironmentalApproval/" & itemD("colm_archivos").Text
            ImageDownload.Target = "_blank"

        End If
    End Sub

    Private Sub btn_addDoc_Click(sender As Object, e As EventArgs) Handles btn_addDoc.Click

        If uploadFile.UploadedFiles.Count = 0 Then
            lblt_ErrTypeDocFile.Text = "Select a document to upload"
            lblt_ErrTypeDocFile.Visible = True
            'Else
            '    lblt_ErrTypeDocFile.Visible = False
        End If
    End Sub

    Private Sub grd_archivos_DeleteCommand(sender As Object, e As GridCommandEventArgs) Handles grd_archivos.DeleteCommand

        Dim id_archivo_ambiental = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_archivo_ambiental")
        Dim nombre_archivo As String = ""

        Dim iTemD As GridDataItem

        For Each iTemD In Me.grd_archivos.Items

            If iTemD("id_archivo_ambiental").Text = id_archivo_ambiental Then

                nombre_archivo = iTemD("colm_archivos").Text

            End If

        Next

        If cl_envir_app.del_ta_documento_ambiental_archivos(id_archivo_ambiental) Then
            If DelFile(nombre_archivo) Then
                grd_archivos.DataSource = cl_envir_app.get_vw_enviromental_document(hd_Id_doc.Value)
                grd_archivos.DataBind()
            Else
                lblt_ErrTypeDocFile.Text = "An error accurred when deleted the selected file"
                lblt_ErrTypeDocFile.Visible = True
            End If
        Else
            lblt_ErrTypeDocFile.Text = "An error accurred when deleted the selected register"
            lblt_ErrTypeDocFile.Visible = True
        End If

    End Sub


    Function DelFile(ByVal sFileName As String) As Boolean
        Dim sFileDir As String = Server.MapPath("~\FileUploads\EnvironmentalApproval\")
        Dim file_info As New IO.FileInfo(sFileDir + sFileName)
        If (file_info.Exists) Then
            file_info.Delete()
            DelFile = True
        Else
            DelFile = False
        End If
    End Function

End Class
