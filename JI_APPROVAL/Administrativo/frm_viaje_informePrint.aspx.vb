Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Drawing
Imports ly_APPROVAL
Public Class frm_viaje_informePrint
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJE_INF_PRINT"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtItinerario As New DataTable
    Dim dtAlojamiento As New DataTable
    Dim clss_approval As APPROVAL.clss_approval
    Dim PathArchivos = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Using dbEntities As New dbRMS_JIEntities
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
                    'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                    cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                End If
                controles.code_mod = frmCODE
                For Each Control As Control In Page.Controls
                    controles.checkControls(Control, cl_user.id_idioma, cl_user)
                Next
            End If
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            PathArchivos = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
            If Not Me.IsPostBack Then
                clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
                Session.Remove("dtConceptos")
                Session.Remove("dtItinerario")
                Session.Remove("dtAlojamiento")
                Dim id_viaje = Convert.ToInt32(Me.Request.QueryString("id"))
                'Me.idFactura.Value = id_Factura
                'LoadData(id_viaje)
                LoadData(id_viaje)
                'loadDataItinerario(viaje)
                'loadDataAlojamiento(viaje)
                'fillGridItinerario()
                'fillGridAlojamiento()
                'fillGridLegalizacion(id_viaje)
                'fillGridArchivos(id_viaje)

            End If
        End Using
    End Sub

    Sub LoadData(ByVal id_viaje As Integer)

        Using dbEntities As New dbRMS_JIEntities
            Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
            Dim viajeDetail = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).FirstOrDefault()

            Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(viajeDetail.id_documento_informe.ToString())
            Me.grd_cate.DataBind()
            Dim id_usuario_app = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).FirstOrDefault().id_usuario_app

            Dim indx As Integer = Convert.ToInt32(Me.Session("E_IDUser"))

            Me.lbl_nombres.Text = viaje.t_usuarios.nombre_usuario & " " & viaje.t_usuarios.apellidos_usuario
            Me.lbl_numero_documento.Text = viaje.t_usuarios.numero_documento
            Me.lbl_cargo.Text = viaje.t_job_title.job
            If viaje.fecha_registro_informe IsNot Nothing Then
                Me.lbl_fecha_solicitud.Text = viaje.fecha_registro_informe
            End If
            Me.lbl_motivo.Text = viaje.motivo_viaje
            Me.lbl_regional.Text = viajeDetail.region_solicita
            Me.lbl_codigo_viaje.Text = viaje.codigo_solicitud_viaje

            Me.lbl_Tasa_ser.Text = viaje.tasa_ser
            Me.lbl_fecha_radicacion.Text = viaje.fecha_tasa_ser

            Me.lbl_resultados.Text = viaje.informe_resultado
            Me.lbl_actividades.Text = viaje.informe_compromiso
            Me.lbl_lugares_visitados.Text = viaje.lugares_entidades_personas

        End Using
    End Sub

End Class