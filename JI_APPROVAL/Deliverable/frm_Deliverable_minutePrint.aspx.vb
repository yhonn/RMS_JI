Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports ly_SIME
Imports System.Web.Services
Imports ly_RMS
Imports System.Globalization
Imports System.Web.Script.Serialization


Public Class frm_Deliverable_minutePrint
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_DELIV_MIN_PRINT"

    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clss_approval As APPROVAL.clss_approval
    Dim cls_Deliverable As APPROVAL.clss_Deliverable

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
                ' cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            If Not IsNothing(Me.Request.QueryString("ID")) Then
                Me.hd_id_deliverable.Value = Convert.ToInt32(Me.Request.QueryString("ID"))
            Else
                Me.hd_id_deliverable.Value = 0
            End If

            LoadData(hd_id_deliverable.Value)

        End If

    End Sub

    Public Sub LoadData(ByVal idDeliverable As Integer)

        Using dbEntities As New dbRMS_JIEntities


            Dim id_programa As Integer = CType(Me.Session("E_IDPrograma"), Integer)
            cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
            Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)


            Dim Tbl_deliverable As DataTable = cls_Deliverable.get_Deliverables(idDeliverable)

            Dim idFichaProyecto As Integer = Convert.ToInt32(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"))
            Dim oFichaProyecto = dbEntities.vw_tme_ficha_proyecto.Where(Function(p) p.id_ficha_proyecto = idFichaProyecto).FirstOrDefault()
            Dim oEjecutores = dbEntities.t_ejecutores.Where(Function(p) p.id_ejecutor = oFichaProyecto.id_ejecutor).FirstOrDefault()
            Dim oUsuario_Responsable = dbEntities.vw_t_usuarios.Where(Function(p) p.id_programa = id_programa And p.id_usuario = oFichaProyecto.id_usuario_responsable).FirstOrDefault()

            Me.hd_id_deliverable_minute.Value = Tbl_deliverable.Rows.Item(0).Item("id_deliverable_minute")

            Me.lbl_Beneficiario.Text = oFichaProyecto.nombre_ejecutor
            Me.lbl_activity_code.Text = String.Format("{0} No.", oFichaProyecto.nombre_mecanismo_contratacion)
            Me.lbl_activity.Text = oFichaProyecto.codigo_ficha_AID

            Dim idM As Integer = Convert.ToInt32(Me.hd_id_deliverable_minute.Value)
            Dim oMinute As ta_deliverable_minute = dbEntities.ta_deliverable_minute.Find(idM)

            Dim Delive_Value As Double = If(oMinute.local_currency = True, Tbl_deliverable.Rows.Item(0).Item("valor_final"), (Tbl_deliverable.Rows.Item(0).Item("valor_final") / Tbl_deliverable.Rows.Item(0).Item("tasa_cambio_final")))
            Dim strCurrSymbol As String = If(oMinute.local_currency = True, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol, "USD")

            'Just Local Currency
            Me.lbl_pago_descricion.Text = String.Format("Pago No. {0} / Valor Total {1} ", Tbl_deliverable.Rows.Item(0).Item("numero_entregable"), String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", Delive_Value, strCurrSymbol))
            Me.lbl_otr_name.Text = String.Format("{0} ({1})", oUsuario_Responsable.nombre_usuario, oUsuario_Responsable.job)

            Me.lblt_product_name.Text = String.Format("Producto {0}. {1} <br /><br />", Tbl_deliverable.Rows.Item(0).Item("numero_entregable"), Tbl_deliverable.Rows.Item(0).Item("descripcion_entregable"))

            Me.product_descrip.InnerHtml = Tbl_deliverable.Rows.Item(0).Item("verification_mile").ToString.Replace(vbLf, "<br />").Replace(vbCrLf, "<br />")


            If oMinute.minute_close = True Then
                Me.lbl_acta_no.Text = oMinute.minute_code
            Else
                Me.lbl_acta_no.Text = "--------"
            End If

            Dim oClin = dbEntities.ta_clin_codes.Where(Function(p) p.id_clin_code = oMinute.id_clin_code).FirstOrDefault()
            Me.lbl_CLIN.Text = String.Format("CLIN {0}", oClin.clin_code)
            Me.lbl_GL.Text = String.Format("GL {0}", oClin.GL_code)

            Dim municipios = dbEntities.t_municipios.Where(Function(p) p.id_municipio = oMinute.id_municipio).FirstOrDefault
            Me.lbl_municipio.Text = municipios.nombre_municipio
            Me.lbl_comment.Text = oMinute.minute_comment  '.ToString.Replace(Environment.NewLine, "<br />")
            Me.lbl_soporte_ubicacion.Text = dbEntities.ta_offices.Where(Function(p) p.id_office = oMinute.id_office).FirstOrDefault.office_name
            Me.lbl_beneficiario_pago.Text = oEjecutores.nombre_ejecutor
            Me.div_bank.InnerHtml = If(oEjecutores.billing_info Is Nothing, "", oEjecutores.billing_info.ToString.Replace(vbLf, "<br />").Replace(vbCrLf, "<br />"))
            Me.lbl_identificacion_trib.Text = oEjecutores.nit

            Dim tbl_result As DataTable = cls_Deliverable.Deliv_Document(idDeliverable)
            Me.hd_id_documento.Value = tbl_result.Rows.Item(0).Item("id_documento")
            Dim tbl_App As DataTable = clss_approval.get_DocumentINFO(Convert.ToInt32(Me.hd_id_documento.Value))

            Me.lbl_autorizacion_comment.Text = String.Format("Por medio del presente documento los abajo participantes del proceso de aprobación {0}, autorizan el pago de los productos mencionados anteriormente.  <br /><br />", tbl_App.Rows.Item(0).Item("numero_instrumento"))

            Dim strRows As String = get_ApprovedTable()

            Me.App_users.InnerHtml = String.Format("<table class=""table table-bordered table-responsive"">  {0} </table>", strRows)

        End Using

    End Sub


    Function get_ApprovedTable() As String

        Using dbEntities As New dbRMS_JIEntities

            Dim strROWS As String = ""

            Dim tbl_route As DataTable = clss_approval.get_ta_RutaSeguimiento(Me.hd_id_documento.Value)
            Dim idMin = Convert.ToInt32(Me.hd_id_deliverable_minute.Value)
            Dim dtTipoAPP As DataTable = cls_Deliverable.get_ApprovalEstado_tipo()
            Dim id_programa As Integer = CType(Me.Session("E_IDPrograma"), Integer)

            Dim strROW As String = "<tr> {0} </tr>"
            Dim strCOL As String = ""
            Dim i As Integer = 0

            For Each dtRow As DataRow In tbl_route.Rows

                Dim idApp = Convert.ToInt32(dtRow("id_app_documento"))

                If (dbEntities.ta_deliverable_minute_app.Where(Function(p) p.id_deliverable_minute = idMin And p.id_App_documento = idApp).Count() > 0) Then

                    Dim tbl_app_doc As DataTable = clss_approval.get_vw_ta_AppDocumento(0, idApp)

                    'Dim id_usuario = tbl_app_doc.Rows.Item(0).Item("id_usuario_app")
                    'Dim idRuta = dtRow("id_ruta")
                    'Dim idEstadoTipo = dbEntities.ta_rutaTipoDoc.Where(Function(p) p.id_ruta = idRuta).FirstOrDefault.id_estadoTipo
                    'Dim oUsuario_Responsable = dbEntities.vw_t_usuarios.Where(Function(p) p.id_programa = id_programa And p.id_usuario = id_usuario).FirstOrDefault()
                    'tbl_app_doc.Rows.Item(0).Item("fecha_aprobacion")

                    strCOL &= String.Format("<td>
                                                <span style=""font-weight:600;"" class= ""text-left""> {0} </span><br />
                                                <span style=""font-size:medium;"">{1} {2} PARAMOS Y BOSQUES </span><br />
                                                <span style=""font-size:medium;"">Fecha {3} {4}</span><br />
                                            </td>", tbl_app_doc.Rows.Item(0).Item("nombre_usuario"), tbl_app_doc.Rows.Item(0).Item("estado_tipo_prefijo"), tbl_app_doc.Rows.Item(0).Item("job"), tbl_app_doc.Rows.Item(0).Item("estado_tipo_prefijo"), getFecha(tbl_app_doc.Rows.Item(0).Item("fecha_aprobacion"), "f", True))


                    i += 1
                    If i = 2 Then
                        strROWS &= String.Format(strROW, strCOL)
                        strCOL = ""
                        i = 0
                    End If

                End If

            Next

            If i = 1 Then
                strCOL &= "<td>&nbsp;</td>"
                strROWS &= String.Format(strROW, strCOL)
                strCOL = ""
                i = 0
            End If

            get_ApprovedTable = strROWS

        End Using

    End Function


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