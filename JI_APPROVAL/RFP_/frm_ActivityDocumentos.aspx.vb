Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.IO
Imports CuteWebUI
Imports System.Configuration.ConfigurationManager

Public Class frm_ActivityDocumentos
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ACTIVITY_DOC"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_listados As New ly_SIME.CORE.cls_listados
    Dim valorSuma As Decimal = 0
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Public Property document_folder As String = ""
    Public Property award_folder As String = ""
    Public Property deliverable_folder As String = ""



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next

            Me.btn_eliminarDocumento.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
        End If



        Using dbEntities As New dbRMS_JIEntities

            If Not Me.IsPostBack Then

                Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
                Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Me.lbl_id_ficha.Text = id
                Dim proyecto = dbEntities.VW_TA_ACTIVITY.FirstOrDefault(Function(p) p.id_activity = id)
                Me.lbl_informacionproyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto
                'loadListas(idPrograma, proyecto)

                'LoadData_code(id)
                loadLists(idPrograma)

                Dim id_aw As Integer = 0

                If Not IsNothing(Me.Request.QueryString("Id_AW")) Then
                    id_aw = Convert.ToInt32(Val(Me.Request.QueryString("Id_AW").ToString))
                End If

                Me.lbl_id_ficha_aw.Text = id_aw

                Me.alink_definicion.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityE?Id=" & id.ToString()))
                ''Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDocumentos?Id=" & id.ToString()))
                Me.alink_solicitation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivitySolicitation?Id=" & id.ToString()))
                Me.alink_prescreening.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityPrescreening?Id=" & id.ToString()))
                Me.alink_submission.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityApply?Id=" & id.ToString()))
                Me.alink_evaluation.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityEvaluation?Id=" & id.ToString()))
                Me.alink_awarded.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityAW?Id=" & id.ToString() & "&Id_AW=" & id_aw.ToString()))

                'If proyecto.ID_ACTIVITY_STATUS >= 5 Then
                Dim oTA_ACTIVITY_STATUS = dbEntities.TA_ACTIVITY_STATUS.Find(proyecto.ID_ACTIVITY_STATUS)

                If ((oTA_ACTIVITY_STATUS.ORDER = 4 And oTA_ACTIVITY_STATUS.ORDERbool = True) Or oTA_ACTIVITY_STATUS.ORDER > 4) Then

                    Me.alink_funding.Attributes.Add("style", "display:block;")
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:block;")
                    Me.alink_INDICATORS.Attributes.Add("style", "display:block;")

                Else
                    Me.alink_funding.Attributes.Add("style", "display:none;")
                    Me.alink_DELIVERABLES.Attributes.Add("style", "display:none;")
                    Me.alink_INDICATORS.Attributes.Add("style", "display:none;")
                End If

                Dim oPro = dbEntities.TA_ACTIVITY.Find(proyecto.id_activity)

                Me.timeline_activity.ID_ACTIVITY = proyecto.id_activity

                Dim oVW_TA_AWARDED_APP = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = Me.lbl_id_ficha.Text).FirstOrDefault()

                Dim proyectoAW As New VW_TA_AWARDED_ACTIVITY

                If Not IsNothing(oVW_TA_AWARDED_APP) Then

                    ' Dim proyectoAW = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = oVW_TA_AWARDED_APP.ID_AWARDED_APP).FirstOrDefault()
                    If id_aw > 0 Then
                        proyectoAW = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = oVW_TA_AWARDED_APP.ID_AWARDED_APP).FirstOrDefault()
                    Else
                        proyectoAW = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = oVW_TA_AWARDED_APP.ID_AWARDED_APP).FirstOrDefault()
                    End If

                    Me.lbl_informacionproyecto.Text = "(" + proyectoAW.codigo_ficha_AID + ")" + " " + proyectoAW.nombre_proyecto

                End If

                ' Me.cmb_awards.SelectedValue = oVW_TA_AWARDED_APP.ID_AWARDED_ACTIVITY
                Me.cmb_awards.SelectedValue = proyectoAW.ID_AWARDED_ACTIVITY

                loadAWARD(oVW_TA_AWARDED_APP.ID_AWARDED_APP)

                '  End If


                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo <> "IQS" Then
                '    Me.alink_stos.Attributes.Add("style", "display:none;")
                'End If
                'Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString()))

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************

                Dim oSubFather As Object = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_father = oPro.tme_sub_mecanismo.id_sub_mecanismo).ToList()

                'Me.alink_stos.Attributes.Add("style", "display:none;")
                'Me.alink_po.Attributes.Add("style", "display:none;")
                'Me.alink_Ik.Attributes.Add("style", "display:none;")

                'Me.alink_stos.Attributes.Add("href", "#")
                'Me.alink_po.Attributes.Add("href", "#")
                'Me.alink_Ik.Attributes.Add("href", "#")


                'Dim i = 0
                'If oSubFather.count > 0 Then

                '    For Each item In oSubFather

                '        If i = 0 Then
                '            Me.alink_stos.InnerText = item.perfijo_sub_mecanismo
                '            Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString() & "&tp=" & item.perfijo_sub_mecanismo))
                '            Me.alink_stos.Attributes.Add("style", "display:block;")
                '        ElseIf i = 1 Then
                '            Me.alink_po.InnerText = item.perfijo_sub_mecanismo
                '            Me.alink_po.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString() & "&tp=" & item.perfijo_sub_mecanismo))
                '            Me.alink_po.Attributes.Add("style", "display:block;")
                '        Else
                '            Me.alink_Ik.InnerText = item.perfijo_sub_mecanismo
                '            Me.alink_Ik.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString() & "&tp=IK"))
                '            Me.alink_Ik.Attributes.Add("style", "display:block;")
                '            Me.alink_Ik.Attributes.Add("style", "display:block;")
                '        End If

                '        i += 1

                '    Next

                'End If

                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo = "PO" Then
                '    Me.alink_areas.Attributes.Add("href", "#")
                '    Me.alink_areas.Attributes.Add("style", "display:none;")
                '    Me.alink_indicadores.Attributes.Add("href", "#")
                '    Me.alink_indicadores.Attributes.Add("style", "display:none;")
                '    Me.alink_waiver.Attributes.Add("href", "#")
                '    Me.alink_waiver.Attributes.Add("style", "display:none;")
                'End If

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************


                'And p.DOCUMENTROLE = "ACTIVITY_ANNEX"


            Else

                Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
                document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
                award_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).awarded_documents_path
                'deliverable_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).documents_folder
                deliverable_folder = "\fileUploads\ApprovalProcc\"


            End If


        End Using

    End Sub


    Sub set_links(ByVal idACT As Integer, ByVal id_aw As Integer)

        Me.alink_funding.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityF?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))
        Me.alink_DELIVERABLES.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDeliv?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))
        Me.alink_INDICATORS.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityInd?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))

        Me.alink_awarded.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityAW?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))
        Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/RFP_/frm_ActivityDocumentos?Id=" & idACT.ToString() & "&Id_AW=" & id_aw.ToString()))

    End Sub



    Protected Sub EliminarAporte_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub




    Protected Sub grd_archivos_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_archivos.DeleteCommand

        Using dbEntities As New dbRMS_JIEntities

            Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("ID_ACTIVITY_AW_DOCUMENTS").ToString()
            cnnME.Open()
            Dim dm As New SqlCommand("DELETE FROM TA_ACTIVITY_AW_DOCUMENTS WHERE (ID_ACTIVITY_AW_DOCUMENTS = " & id_temp & ")", cnnME)
            dm.ExecuteNonQuery()
            cnnME.Close()
            Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)

            'Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            ' Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY
            'Me.grd_archivos.DataSource = dbEntities.tme_Anexos_ficha.Where(Function(p) p.id_ficha_proyecto = id And p.visible.Value).ToList()
            'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_AW_DOCUMENTS.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT).ToList()
            'Me.grd_archivos.DataBind()

            LoadDOCS_Grid(True)

        End Using

        'DelFileParam(e.Item.Cells(4).Text.ToString)
    End Sub
    Protected Sub grd_archivos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_archivos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim ImageDownload As New HyperLink
            ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)

            If (DataBinder.Eval(e.Item.DataItem, "DOCUMENTROLE").ToString().IndexOf("ACTIVITY_DELIV") <> -1) Then

                ImageDownload.NavigateUrl = deliverable_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString() ''e.Item.Cells(4).Text.ToString

            ElseIf (DataBinder.Eval(e.Item.DataItem, "DOCUMENTROLE").ToString().IndexOf("AWARD_ANNEX") <> -1) Then

                ImageDownload.NavigateUrl = award_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString()

            ElseIf (DataBinder.Eval(e.Item.DataItem, "DOCUMENTROLE").ToString().IndexOf("ACTIVITY_ANNEX") <> -1) Then

                ImageDownload.NavigateUrl = document_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString()

            Else
                ImageDownload.NavigateUrl = document_folder & DataBinder.Eval(e.Item.DataItem, "DOCUMENT_NAME").ToString() ''e.Item.Cells(4).Text.ToString
            End If

            ImageDownload.Target = "_blank"

        End If



    End Sub



    Public Sub LoadDOCS_Grid(ByVal bndRebind As Boolean)


        Using dbEntities As New dbRMS_JIEntities

            Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)
            Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY

            Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_AW_DOCUMENTS.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT).ToList()

            If bndRebind Then
                Me.grd_archivos.DataBind()
            End If

        End Using

    End Sub


    Public Sub RadAsyncUpload1_FileUploaded(sender As Object, e As FileUploadedEventArgs)
        'Dim Path As String
        'Path = Server.MapPath("~/FileUploads/")
        'e.File.SaveAs(Path + getNewName(e.File))
    End Sub

    Protected Sub btn_agregar_Click(sender As Object, e As EventArgs) Handles btn_agregar.Click

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)
        Dim idAW = Convert.ToInt32(Me.LBL_ID_AWARD.Text)

        Using dbEntities As New dbRMS_JIEntities


            'Dim id_activity_AW As Integer = Convert.ToInt32(Me.lbl_id_ficha_aw.Text)
            'Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = idAW).FirstOrDefault().ID_AWARDED_ACTIVITY
            'Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_ACTIVITY = id_activity_AW).FirstOrDefault()
            Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue

            For Each file As UploadedFile In AsyncUpload1.UploadedFiles
                Dim exten = file.GetExtension()
                Dim nombreArchivo = Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten

                Dim anexo As New TA_ACTIVITY_AW_DOCUMENTS
                anexo.DOCUMENT_TITLE = Me.txt_document_tittle.Text
                anexo.DOCUMENT_NAME = nombreArchivo
                anexo.DOCUMENTROLE = "ACTIVITY_ANNEX"
                anexo.id_doc_soporte = cmb_type_of_document.SelectedValue
                anexo.ID_AWARDED_ACTIVITY = idAW_ACT
                anexo.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                anexo.fecha_crea = Date.UtcNow
                anexo.visible = True
                anexo.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)


                dbEntities.TA_ACTIVITY_AW_DOCUMENTS.Add(anexo)
                Dim Path As String
                Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path)
                file.SaveAs(Path + nombreArchivo)
            Next
            dbEntities.SaveChanges()

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivityDocumentos?id=" & id & "&Id_AW=" & idAW_ACT.ToString()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
            award_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).awarded_documents_path
            'deliverable_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).documents_folder
            deliverable_folder = "\fileUploads\ApprovalProcc\"

            'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_AW_DOCUMENTS.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW).ToList()

        End Using

        Me.grd_archivos.DataBind()

    End Sub

    Function getNewName(ByVal file As UploadedFile) As String
        Dim rand As New Random()
        Dim Aleatorio As Double = rand.Next(1, 99999)
        Dim extension As String = System.IO.Path.GetExtension(file.GetExtension())
        Dim newName As String = "doc_" & Me.Session("E_IdUser") & Date.UtcNow.ToShortDateString().Replace("/", "-") & Aleatorio & file.GetNameWithoutExtension().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension
        Return newName
    End Function

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue
        Me.Response.Redirect("~/RFP_/frm_ActivityAW?Id=" & Me.lbl_id_ficha.Text & "&Id_AW=" & idAW_ACT.ToString())
        'Me.MsgReturn.Redireccion = "~/Proyectos/frm_Proyectos"
        'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub btn_continue_Click(sender As Object, e As EventArgs) Handles btn_continue.Click
        Dim id = Convert.ToInt32(Me.lbl_id_ficha.Text)
        Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue
        Me.Response.Redirect("~/RFP_/frm_ActivityF?Id=" & Me.lbl_id_ficha.Text & "&Id_AW=" & idAW_ACT.ToString())
    End Sub
    Protected Sub btn_registrar_tc_Click(sender As Object, e As EventArgs) Handles btn_registrar_tc.Click
        Me.Response.Redirect("~/Administracion/frm_tasas_cambio")
    End Sub


    Sub loadLists(ByVal idPrograma As Integer)


        Using dbEntities As New dbRMS_JIEntities

            Me.cmb_type_of_document.DataSourceID = ""
            Me.cmb_type_of_document.DataSource = cl_listados.get_ta_docs_soporte(idPrograma)
            Me.cmb_type_of_document.DataTextField = "nombre_documento"
            Me.cmb_type_of_document.DataValueField = "id_doc_soporte"
            Me.cmb_type_of_document.DataBind()

            Me.cmb_awards.DataSourceID = ""
            Me.cmb_awards.DataSource = dbEntities.VW_TA_AWARDED_APP.Where(Function(p) p.id_programa = idPrograma And p.ID_ACTIVITY = Me.lbl_id_ficha.Text).ToList()
            Me.cmb_awards.DataTextField = "AWARD_CODE"
            'Me.cmb_awards.DataValueField = "ID_AWARDED_APP"
            Me.cmb_awards.DataValueField = "ID_AWARDED_ACTIVITY"
            Me.cmb_awards.DataBind()


        End Using


    End Sub



    Sub loadAWARD(ByVal id_awarded_app As Integer)

        Using dbEntities As New dbRMS_JIEntities

            Dim oTA_AWARDED_APP = dbEntities.TA_AWARDED_APP.Find(id_awarded_app)
            Dim idACT As Integer = Convert.ToInt32(Me.lbl_id_ficha.Text)
            Dim oTA_AWARDED_APP_all = dbEntities.TA_AWARDED_APP.Where(Function(p) p.ID_ACTIVITY = idACT).ToList()
            Dim PercentProgress As Double

            If Not IsNothing(oTA_AWARDED_APP) Then

                Me.LBL_ID_AWARD.Text = oTA_AWARDED_APP.ID_AWARDED_APP

                Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)

                'Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_ACTIVITY.Where(Function(p) p.id_activity = idActivity).FirstOrDefault()
                Dim oVW_TA_ACTIVITY = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = id_awarded_app).FirstOrDefault()

                Dim oTA_ACTIVITY = dbEntities.TA_ACTIVITY.Find(idACT)

                Me.lbl_implementer.Text = oVW_TA_ACTIVITY.ORGANIZATIONNAME
                Me.lbl_activity_name.Text = oVW_TA_ACTIVITY.nombre_proyecto
                Me.lbl_activity_Code.Text = oTA_AWARDED_APP.AWARD_CODE

                Me.lbl_last_Deliverable.Text = oTA_AWARDED_APP.TA_AWARD_STATUS.AWARD_STATUS


                Me.lbl_totalACT2.Text = String.Format("{0:N2} USD", oTA_ACTIVITY.costo_total_proyecto)
                Me.lbl_totalACT2_usd.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", oTA_ACTIVITY.costo_total_proyecto_LOC, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)
                'proyectos.Sum(Function(p) p.tme_AportesFicha.Sum(Function(q) q.monto_aporte_obligado))
                PercentProgress = If(oTA_ACTIVITY.costo_total_proyecto > 0, (oTA_AWARDED_APP_all.Sum(Function(p) p.TOTAL_AMOUNT) / oTA_ACTIVITY.costo_total_proyecto), 0) * 100

                Me.lbl_totalPerf2.Text = String.Format("{0:N2} USD", oTA_AWARDED_APP_all.Sum(Function(p) p.TOTAL_AMOUNT))
                Me.lbl_totalPerf2_usd.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", oTA_AWARDED_APP_all.Sum(Function(p) p.TOTAL_AMOUNT_LOC), sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol)


                Me.lbl_period.Text = String.Format("{0:dd/MM/yyyy} to {1:dd/MM/yyyy}", oVW_TA_ACTIVITY.fecha_inicio_proyecto, oVW_TA_ACTIVITY.fecha_fin_proyecto)

                Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

                document_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).activity_documents_path
                award_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).awarded_documents_path
                'deliverable_folder = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).documents_folder
                deliverable_folder = "\fileUploads\ApprovalProcc\"


                'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_DOCUMENTS.Where(Function(p) p.id_activity = id And p.visible.Value).OrderBy(Function(o) o.DOCUMENTROLE).ThenBy(Function(o) o.DOCUMENT_NAME).ToList()

                ' Dim idAW_ACT As Integer = dbEntities.TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = id_awarded_app).FirstOrDefault().ID_AWARDED_ACTIVITY
                Dim idAW_ACT As Integer = Me.cmb_awards.SelectedValue
                set_links(idACT, idAW_ACT)


                'Me.grd_archivos.DataSource = dbEntities.VW_TA_ACTIVITY_AW_DOCUMENTS.Where(Function(p) p.ID_AWARDED_ACTIVITY = idAW_ACT).OrderBy(Function(o) o.DOCUMENTROLE).ThenBy(Function(o) o.DOCUMENT_NAME).ToList()
                'Me.grd_archivos.DataBind()
                LoadDOCS_Grid(True)

            End If


            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "set_Chart_Progress(" & Math.Round(Convert.ToDouble(PercentProgress), 2, MidpointRounding.AwayFromZero).ToString & ",'" & " " & "');", True)


        End Using


    End Sub


    Protected Sub cmb_awards_DataBound(sender As Object, e As EventArgs) Handles cmb_awards.DataBound

        CType(cmb_awards.Footer.FindControl("RadComboItemsCount_award"), Literal).Text = Convert.ToString(cmb_awards.Items.Count)

    End Sub


    Private Sub cmb_awards_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_awards.SelectedIndexChanged

        Using dbEntities As New dbRMS_JIEntities

            If e.Value IsNot Nothing Then

                'Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
                'Dim proyecto = dbEntities.VW_TA_AWARDED_ACTIVITY.Where(Function(p) p.ID_AWARDED_APP = e.Value).FirstOrDefault()
                loadAWARD(e.Value)

            End If

        End Using

    End Sub


    Private Sub cmb_awards_ItemDataBound(sender As Object, e As RadComboBoxItemEventArgs) Handles cmb_awards.ItemDataBound


        Dim DateINI As Date = CType(DataBinder.Eval(e.Item.DataItem, "fecha_inicio_proyecto"), Date)
        Dim DateFIN As Date = CType(DataBinder.Eval(e.Item.DataItem, "fecha_fin_proyecto"), Date)


        e.Item.Text = String.Format(" {0} ==>> {1} ==>> {2:d} ==>> {3:d} ", DataBinder.Eval(e.Item.DataItem, "AWARD_CODE").ToString(), DataBinder.Eval(e.Item.DataItem, "nombre_proyecto").ToString(), DateINI, DateFIN)
        e.Item.Value = DataBinder.Eval(e.Item.DataItem, "ID_AWARDED_APP").ToString


    End Sub

    Private Sub grd_archivos_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_archivos.NeedDataSource

        LoadDOCS_Grid(False)

    End Sub
End Class