Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports System.Drawing
Imports ly_APPROVAL
Imports ly_RMS
Imports System.Globalization


Partial Class frm_seguimientoAprobacionEnvironmental
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim clss_approval As APPROVAL.clss_approval
    Dim cl_envir_app As APPROVAL.cl_environmentalAPP
    Dim cl_user As ly_SIME.CORE.cls_user

    Public strtitulo As String
    Dim cProgram As New RMS.cls_Program


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login.aspx")
        End Try

        If Not IsPostBack Then

            If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
                cl_user = Session.Item("clUser")
                'If Not cl_user.chk_accessMOD(0, frmCODE) Then
                '    Me.Response.Redirect("~/Proyectos/no_access2")
                'Else
                '    cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                '    cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_archivos)
                'End If
                'controles.code_mod = frmCODE
                'For Each Control As Control In Page.Controls
                '    controles.checkControls(Control, cl_user.id_idioma, cl_user)
                'Next
            End If

            hd_Id_doc.Value = Convert.ToInt32(Me.Request.QueryString("IdDoc").ToString)

            'cl_Doc_supp = New APPROVAL.cl_Doc_support(Me.Session("E_IDPrograma"))
            cl_envir_app = New APPROVAL.cl_environmentalAPP(Me.Session("E_IDPrograma"), hd_Id_doc.Value)

            LoadData()

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            cProgram.get_Programs(CType(Me.Session("E_IDPrograma").ToString(), Integer), True)

            'Me.lblIDocumento.Text = Me.Request.QueryString("IdDoc").ToString

            Dim tbl_Doc As New DataTable

            tbl_Doc = clss_approval.get_DocumentINFO(Me.lblIDocumento.Text)

            Dim strIMG As String = ResolveUrl("../" & cProgram.getprogramField("imgName", "id_programa", CType(Me.Session("E_IDPrograma"), Integer)))

            If String.IsNullOrEmpty(strIMG) Then
                strIMG = "../"
            End If
            'imgProgram.ImageUrl = strIMG


            Me.lbl_categoria.Text = tbl_Doc.Rows.Item(0).Item("descripcion_cat").ToString
            Me.lbl_aprobacion.Text = tbl_Doc.Rows.Item(0).Item("descripcion_aprobacion").ToString
            Me.lbl_nivelaprobacion.Text = tbl_Doc.Rows.Item(0).Item("nivel_aprobacion").ToString
            Me.lbl_condicion.Text = tbl_Doc.Rows.Item(0).Item("condicion").ToString
            Me.lbl_proceso.Text = tbl_Doc.Rows.Item(0).Item("descripcion_doc").ToString

            'lblt_subtitulo_pantalla.Text = ""

            Me.lbl_codigo.Text = tbl_Doc.Rows.Item(0).Item("codigo_AID").ToString
            lblt_environmental_code.Text = String.Format("{0}-{1}", lblt_environmental_code.Text.Trim, tbl_Doc.Rows.Item(0).Item("codigo_AID").ToString)

            Me.lbl_instrumento.Text = tbl_Doc.Rows.Item(0).Item("numero_instrumento").ToString
            Me.lbl_beneficiario.Text = tbl_Doc.Rows.Item(0).Item("nom_beneficiario").ToString

            'Me.lbl_IdCodigoAPP.Text = tbl_Doc.Rows.Item(0).Item("codigo_Approval").ToString
            'Me.lbl_IdCodigoSAP.Text = tbl_Doc.Rows.Item(0).Item("codigo_SAP_APP").ToString
            'Me.lbl_region.Text = tbl_Doc.Rows.Item(0).Item("regional").ToString
            'Me.lbl_montoProyecto.Text = tbl_Doc.Rows.Item(0).Item("monto_ficha").ToString
            'Me.lbl_montoTotal.Text = tbl_Doc.Rows.Item(0).Item("monto_total").ToString
            'Me.lbl_tasaCambio.Text = tbl_Doc.Rows.Item(0).Item("tasa_cambio").ToString
            'Me.lbl_datecreated.Text = tbl_Doc.Rows.Item(0).Item("datecreated").ToString '.Format("{0:dd/MM/yyyy hh:mm:ss}")
            'Me.lbl_status.Text = tbl_Doc.Rows.Item(0).Item("descripcion_estado").ToString
            'Me.lbl_createdby.Text = tbl_Doc.Rows.Item(0).Item("Originador").ToString
            'Me.lbl_approvedby.Text = tbl_Doc.Rows.Item(0).Item("propietario").ToString
            'Me.lbl_Comment.Text = tbl_Doc.Rows.Item(0).Item("comentarios").ToString

            Me.lbl_dateapproved.Text = getFecha(tbl_Doc.Rows.Item(0).Item("fecha_aprobacion"), "f", True)

            'If tbl_Doc.Rows.Item(0).Item("id_estadoDoc").ToString = "1" Then
            '    Me.lbl_ErrApprovedBy.Visible = True
            'End If

            'Me.SqlDataSource3.SelectCommand = "SELECT  ROW_NUMBER() OVER(ORDER BY id_archivo DESC) as Number,* FROM vw_ta_archivos_documento WHERE id_documento=" & Me.lblIDocumento.Text & " AND id_ruta= " & Me.lblIdRuta.Text
            Me.grd_Document.DataSource = clss_approval.get_Document(Me.lblIDocumento.Text)
            Me.grd_Document.DataBind()

            Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(Me.lblIDocumento.Text)
            Me.grd_cate.DataBind()

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



    Public Sub LoadData()

        cl_envir_app.get_vw_t_documento_ambiental(hd_Id_doc.Value)

        lbl_instrumento.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("numero_instrumento", "id_documento_ambiental", hd_Id_doc.Value)
        lbl_proceso.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("descripcion_aprobacion", "id_documento_ambiental", hd_Id_doc.Value)
        'lblt_environmental_code.Text = String.Format("{0}-{1:D4}", "ENV", cl_envir_app.get_vw_t_documento_ambientalFIELDS("id_documento_ambiental", "id_documento_ambiental", hd_Id_doc.Value))
        lblt_environmental_code.Text = String.Format("{0}-{1}", "ENV", Strings.Right("000" & hd_Id_doc.Value, 4))

        'lbl_beneficiaryN.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("nom_beneficiario", "id_documento_ambiental", hd_Id_doc.Value)
        'lbl_created_user.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("usuario_creo", "id_documento_ambiental", hd_Id_doc.Value)

        Me.lblIDocumento.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("id_documento", "id_documento_ambiental", hd_Id_doc.Value)

        Dim v_date As DateTime = Convert.ToDateTime(cl_envir_app.get_vw_t_documento_ambientalFIELDS("fecha_creado", "id_documento_ambiental", hd_Id_doc.Value))

        '************************************SYSTEM DATE FORMAT********************************************
        'Dim timezoneUTC As Integer
        'Dim dateUtil As APPROVAL.cls_dUtil
        'Dim cProgram As New RMS.cls_Program
        'Dim userCulture As CultureInfo = cl_user.regionalizacionCulture
        'cProgram.get_Programs(2, True)
        'timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", 2))
        'dateUtil = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
        '************************************SYSTEM INFO********************************************

        lbl_observation.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("observacion", "id_documento_ambiental", hd_Id_doc.Value)
        lbl_review_type.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("tipo_revision", "id_documento_ambiental", hd_Id_doc.Value)

        'lbl_created_Date.Text = dateUtil.set_DateFormat(v_date, "g", , True)

        Dim strEstado As String
        If cl_envir_app.get_vw_t_documento_ambientalFIELDS("id_Estado", "id_documento_ambiental", hd_Id_doc.Value) = 1 Then 'Pending
            strEstado = String.Format("{0} &nbsp;<i class='fa fa-clock-o'></i>&nbsp;{1} days", cl_envir_app.get_vw_t_documento_ambientalFIELDS("estado_ambiental", "id_documento_ambiental", hd_Id_doc.Value), cl_envir_app.get_vw_t_documento_ambientalFIELDS("elapsed", "id_documento_ambiental", hd_Id_doc.Value))
            spn_state.InnerHtml = strEstado
            spn_state.Visible = True
            spn_state_approved.Visible = False

            v_date = Convert.ToDateTime(cl_envir_app.get_vw_t_documento_ambientalFIELDS("fecha_upd", "id_documento_ambiental", hd_Id_doc.Value))
            lbl_dateUpdated.Text = "Last Updated"
            If cl_envir_app.get_vw_t_documento_ambientalFIELDS("id_usuario_upd", "id_documento_ambiental", hd_Id_doc.Value) > 0 Then '--Not registered
                lbl_updated_user.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("usuario_upd", "id_documento_ambiental", hd_Id_doc.Value)
                lbl_updated_Date.Text = getFecha(v_date, "g", True)
            Else
                lbl_updated_user.Text = "--"
                lbl_updated_Date.Text = " "
            End If

        Else

            strEstado = String.Format("{0} &nbsp;<i class='fa fa-check-circle-o'></i>&nbsp;", cl_envir_app.get_vw_t_documento_ambientalFIELDS("estado_ambiental", "id_documento_ambiental", hd_Id_doc.Value))
            spn_state_approved.InnerHtml = strEstado
            spn_state.Visible = False
            spn_state_approved.Visible = True

            v_date = Convert.ToDateTime(cl_envir_app.get_vw_t_documento_ambientalFIELDS("fecha_aprobado", "id_documento_ambiental", hd_Id_doc.Value))
            lbl_dateUpdated.Text = "Approved Date"
            lbl_updated_user.Text = cl_envir_app.get_vw_t_documento_ambientalFIELDS("usuario_upd", "id_documento_ambiental", hd_Id_doc.Value)
            lbl_updated_Date.Text = getFecha(v_date, "g", True)  'dateUtil.set_DateFormat(v_date, "g", , True)

        End If

        grd_archivos.DataSource = cl_envir_app.get_vw_enviromental_document(hd_Id_doc.Value)
        grd_archivos.DataBind()


    End Sub

    Protected Sub RadGrid1_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_Document.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnk As HyperLink = New HyperLink
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            hlnk = CType(itemD("AttachFile").FindControl("Attach"), HyperLink)
            hlnk.Target = "_blank"
            hlnk.NavigateUrl = "~/FileUploads/ApprovalProcc/" & itemD("archivo").Text 'e.Item.Cells(7).Text.ToString
            itemD("extension").Text = "." & itemD("archivo").Text.ToString.Substring(itemD("archivo").Text.Length - 4, 4).Replace(".", "")
        End If

    End Sub


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

    Protected Sub grd_archivos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_archivos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim ImageDownload As New HyperLink
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            ImageDownload = CType(e.Item.FindControl("ImageDownload"), HyperLink)
            ImageDownload.ImageUrl = "~/Imagenes/Iconos/adjunto.png"
            ImageDownload.NavigateUrl = "~/FileUploads/EnvironmentalApproval/" & itemD("colm_archivos").Text
            ImageDownload.Target = "_blank"

        End If
    End Sub


End Class
