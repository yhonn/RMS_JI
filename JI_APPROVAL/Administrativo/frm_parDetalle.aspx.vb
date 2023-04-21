Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Drawing
Imports ly_APPROVAL
Public Class frm_parDetalle
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_VIAJE_PRINT"
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
                Dim id_par = Convert.ToInt32(Me.Request.QueryString("id"))
                'Me.idFactura.Value = id_Factura
                LoadData(id_par)

                'Dim viaje = dbEntities.tme_solicitud_viaje.Find(id_viaje)
                'loadDataItinerario(viaje)
                'loadDataAlojamiento(viaje)
                'fillGridItinerario()
                'fillGridAlojamiento()
                'fillGridLegalizacion(id_viaje)
                'fillGridArchivos(id_viaje)

            End If
        End Using

    End Sub
    Sub LoadData(ByVal id_par As Integer)
        Dim indx As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
        Using dbEntities As New dbRMS_JIEntities
            Dim par = dbEntities.vw_tme_par.Where(Function(p) p.id_par = id_par).FirstOrDefault()
            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_solicitud)
            Me.lbl_fecha_entrega.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_requiere_servicio)

            Me.lbl_fecha_inicio_evento.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_inicio_evento)
            Me.lbl_fech_finalizacion_evento.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_fin_evento)


            Me.lbl_firmaSolicitante.Text = par.usuario_solicita
            Me.lbl_cargofirmaSolicitante.Text = par.cargo_usuario
            Me.lbl_fechaSolicitante.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_solicitud)

            Me.lbl_cargofirmaSolicitante.Text = par.cargo_usuario
            Me.lbl_fechaSolicitante.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_solicitud)


            Me.lbl_firmaAprobacion.Text = par.usuario_aprueba
            Me.lbl_cargofirmaAprobacion.Text = par.cargo_usuario_aprueba
            Me.lbl_fecha_aprobacion.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_aprobacion)


            If par.id_tipo_par = 2 Then
                Me.lbl_tipo_evento.Text = par.tipo_evento
                Me.lbl_numero_horas.Text = par.numero_horas.Value
                Me.lbl_nombre_evento.Text = par.nombre_evento
                Me.lbl_entidad_acargo.Text = par.entidad_responsable_evento
                Me.lbl_evento_recursos_apalancados.Text = If(par.asociado_recursos_apalancados = True, "SI", "NO")

                Me.info_evento.Visible = True
            End If

            Dim parComponentes = dbEntities.tme_par_marco_logico.Where(Function(p) p.id_par = id_par).Select(Function(p) New With {Key .sub_objetivo = p.tme_estructura_marcologico.tme_estructura_marcologico2.codigo & " " & p.tme_estructura_marcologico.tme_estructura_marcologico2.descripcion_logica,
                .objetivo = p.tme_estructura_marcologico.tme_estructura_marcologico2.tme_estructura_marcologico2.codigo & " " & p.tme_estructura_marcologico.tme_estructura_marcologico2.tme_estructura_marcologico2.descripcion_logica,
                                                                                                                      .componente = p.tme_estructura_marcologico.codigo & " " & p.tme_estructura_marcologico.descripcion_logica,
                                                                                                                      .id_par_componente = p.id_par_marco_logico}).ToList()
            Me.RadGrid1.DataSource = parComponentes.ToList()
            Me.RadGrid1.DataBind()


            Me.lbl_solicitado.Text = par.usuario_solicita
            Me.lbl_aprobado.Text = par.nombre_departamento & " - " & par.nombre_municipio
            Me.lbl_cargo.Text = par.cargo_usuario
            Me.lbl_codigo_rfa.Text = If(par.asociado_actividad = True, par.subactividad, "")
            Me.lbl_departamento.Text = par.nombre_region

            Me.rbn_tipo_solicitud.DataSource = dbEntities.tme_tipo_solicitud_par.ToList()
            Me.rbn_tipo_solicitud.DataValueField = "id_tipo_solicitud"
            Me.rbn_tipo_solicitud.DataTextField = "tipo_solicitud"
            Me.rbn_tipo_solicitud.DataBind()
            Me.rbn_tipo_solicitud.SelectedValue = par.id_tipo_solicitud

            Me.lbl_tipo_par.Text = par.tipo_par

            Me.rbn_cargo_a.DataSource = dbEntities.tme_cargo_pares.ToList()
            Me.rbn_cargo_a.DataValueField = "id_cargo_a_par"
            Me.rbn_cargo_a.DataTextField = "cargo"
            Me.rbn_cargo_a.DataBind()
            Me.rbn_cargo_a.SelectedValue = par.id_cargo_a_par

            Me.grd_servicios_requeridos.DataSource = dbEntities.tme_par_detalle.Where(Function(p) p.id_par = id_par).Select(Function(p) New With {Key p.descripcion,
                                                                                                                                p.cantidad,
                                                                                                                                p.precio_unitario,
                                                                                                                                p.valor_total,
                                                                                                                                .unidad_medida = p.tme_unidad_medida_par.unidad_medida,
                                                                                                                                p.id_par_detalle}).ToList()
            Me.grd_servicios_requeridos.DataBind()

            Me.lbl_descripcion.Text = par.proposito
            Me.lbl_observaciones.Text = "Si el valor total estimado es inferior a US$3,500, cumple criterio de micro-compra (no competencia requerida) Valor total estimado en dólares 
                                        = $" & String.Format("{0:N}", par.valor_total / par.tasa_ser_cotizacion) & ", Costo total estimado/SER = $" & String.Format("{0:N}", par.tasa_ser_cotizacion) & " <b> Es Microcompra? " & If(par.valor_total / par.tasa_ser_cotizacion < 3500, "SÍ", "NO") & "</b>"

            Me.rbn_adjuntos_par.DataSource = dbEntities.tme_tipo_adjunto_par.ToList()
            Me.rbn_adjuntos_par.DataValueField = "id_tipo_adjunto_par"
            Me.rbn_adjuntos_par.DataTextField = "tipo_adjunto"
            Me.rbn_adjuntos_par.DataBind()
            Me.rbn_adjuntos_par.SelectedValue = par.id_tipo_adjunto_par

            Me.lbl_adjuntos.Text = par.observaciones_adicionales
            Me.lbl_codigo_facturacion.Text = If(par.id_tipo_par = 2, par.codigo_pt, par.codigo_facturación)
            Me.tasa_ser.Text = "SER $" & String.Format("{0:N}", par.tasa_ser_cotizacion)

            Me.lbl_codigo_par.Text = par.codigo_par
            Me.lbl_date_received.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_solicitud)
            Me.lbl_asociado_comunicaciones.Text = par.asociado_comunicaciones_text
            'Dim legalizacion = dbEntities.vw_tme_solicitud_viaje.Where(Function(p) p.id_viaje = id_viaje).FirstOrDefault()

            'Me.lbl_usuario.Text = viaje.nombre_usuario
            'Me.lbl_numero_documento.Text = viaje.numero_documento
            'Me.lbl_cargo.Text = viaje.cargo
            'Me.lbl_codigo_usuario.Text = viaje.codigo_usuario
            'Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_crea)
            'Me.lbl_fecha_indicio_viaje.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_inicio_viaje)
            'Me.lbl_fecha_fin_viaje.Text = String.Format("{0:MM/dd/yyyy}", viaje.fecha_fin_viaje)
            'Me.lbl_motivo.Text = viaje.motivo_viaje

            'Me.lblt_resultados.Text = viaje.informe_resultado
            'Me.lblt_compromisos.Text = viaje.informe_compromiso


            'fillGridRutaAprobacion(viaje.id_documento)
            'fillGridRutaAprobacionLegalizacion(viaje.id_documento_legalizacion)
            'fillGridRutaAprobacionInforme(viaje.id_documento_informe)
        End Using
    End Sub
End Class