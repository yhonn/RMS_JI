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


Partial Class frm_seguimientoAprobacionRep
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim clss_approval As APPROVAL.clss_approval
    Public strtitulo As String
    Dim cProgram As New RMS.cls_Program
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_APP_COMMENT_PR"
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

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
            cProgram.get_Programs(CType(Me.Session("E_IDPrograma").ToString(), Integer), True)

            Me.lblIDocumento.Text = Me.Request.QueryString("IdDoc").ToString()
            Me.lblIdRuta.Text = Me.Request.QueryString("IdRuta").ToString()

            Dim tbl_Doc As New DataTable
            Dim id_doc = CType(Me.Request.QueryString("IdDoc").ToString(), Integer)
            tbl_Doc = clss_approval.get_DocumentINFO(id_doc)

            'Dim strIMG As String = ResolveUrl("../" & cProgram.getprogramField("imgName", "id_programa", CType(Me.Session("E_IDPrograma"), Integer)))
            'If String.IsNullOrEmpty(strIMG) Then
            '    strIMG = "../"
            'End If
            'imgProgram.ImageUrl = strIMG

            Dim USerAllowed As String() = tbl_Doc.Rows.Item(0).Item("IdUserParticipate").ToString.Split(",")
            Dim indx As Integer = USerAllowed.ToList().IndexOf(Me.Session("E_IDUser"))

            Dim boolAcces As Boolean = Convert.ToBoolean(Val(Me.h_Filter.Value))

            If (indx = -1) Then '--The User is not Allowed
                If Not boolAcces Then
                    Me.Response.Redirect("~/Proyectos/no_access2_app")
                End If
            End If


            Me.lbl_categoria.Text = tbl_Doc.Rows.Item(0).Item("descripcion_cat").ToString
            Me.lbl_aprobacion.Text = tbl_Doc.Rows.Item(0).Item("descripcion_aprobacion").ToString
            Me.lbl_nivelaprobacion.Text = tbl_Doc.Rows.Item(0).Item("nivel_aprobacion").ToString
            Me.lbl_condicion.Text = tbl_Doc.Rows.Item(0).Item("condicion").ToString
            Me.lbl_proceso.Text = tbl_Doc.Rows.Item(0).Item("descripcion_doc").ToString

            lbl_nameProc.Text = tbl_Doc.Rows.Item(0).Item("descripcion_doc").ToString

            ' lblt_subtitulo_pantalla.Text = tbl_Doc.Rows.Item(0).Item("descripcion_doc").ToString
            ' lblt_subtitulo_pantalla.Text = "Approval Process Report"

            Me.lbl_codigo.Text = tbl_Doc.Rows.Item(0).Item("codigo_AID").ToString

            Me.lbl_instrumento.Text = tbl_Doc.Rows.Item(0).Item("numero_instrumento").ToString
            Me.lbl_beneficiario.Text = tbl_Doc.Rows.Item(0).Item("nom_beneficiario").ToString
            Me.lbl_Comment.Text = tbl_Doc.Rows.Item(0).Item("comentarios").ToString '.Replace("''", "'")

            Me.lbl_datecreated.Text = getFecha(tbl_Doc.Rows.Item(0).Item("datecreated"), "f", True)

            'Me.lbl_IdCodigoAPP.Text = tbl_Doc.Rows.Item(0).Item("codigo_Approval").ToString
            'Me.lbl_IdCodigoSAP.Text = tbl_Doc.Rows.Item(0).Item("codigo_SAP_APP").ToString
            Me.lbl_status.Text = tbl_Doc.Rows.Item(0).Item("descripcion_estado").ToString
            Me.lbl_region.Text = tbl_Doc.Rows.Item(0).Item("regional").ToString

            Me.lbl_tipoDocumento.Text = tbl_Doc.Rows.Item(0).Item("nombreTipoAprobacion").ToString

            'Me.lbl_montoProyecto.Text = tbl_Doc.Rows.Item(0).Item("monto_ficha").ToString
            'Me.lbl_montoTotal.Text = tbl_Doc.Rows.Item(0).Item("monto_total").ToString

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

            'Me.SqlDataSource3.SelectCommand = "SELECT  ROW_NUMBER() OVER(ORDER BY id_archivo DESC) as Number,* FROM vw_ta_archivos_documento WHERE id_documento=" & Me.lblIDocumento.Text & " AND id_ruta= " & Me.lblIdRuta.Text
            Me.grd_Document.DataSource = clss_approval.get_Document(Me.lblIDocumento.Text)
            Me.grd_Document.DataBind()

            Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(Me.lblIDocumento.Text)
            Me.grd_cate.DataBind()

        End If
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


End Class
