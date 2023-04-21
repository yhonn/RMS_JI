Imports ly_RMS
Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Drawing
Imports Telerik.Web.UI.Calendar
Imports System.Globalization

Public Class frm_par_permisos
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_PAR_PERM"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtConceptos As New DataTable
    Dim clss_approval As APPROVAL.clss_approval

    Const cPENDING = 1
    Const cAPPROVED = 2
    Const cnotAPPROVED = 3
    Const cCANCELLED = 4
    Const cOPEN = 5
    Const cSTANDby = 6
    Const cCOMPLETED = 7

    Const cAction_ByProcess = 1
    Const cAction_ByMessage = 2

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
        End If

        If Not Me.IsPostBack Then
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            Dim id_par = Convert.ToInt32(Me.Request.QueryString("id"))
            Me.idPar.Value = id_par



            Session.Remove("dtConceptos")
            LoadData(id_par)
            fillGrid()
            fillGridHistorial()
        End If
    End Sub
    Sub LoadData(ByVal id_par As Integer)
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        Using dbEntities As New dbRMS_JIEntities
            Dim par = dbEntities.tme_pares.Find(id_par)
            Dim parDetalle = dbEntities.vw_tme_par.Where(Function(p) p.id_par = id_par).FirstOrDefault()
            Dim id_usuario_appStr = parDetalle.id_usuario_app
            Dim id_usuario_app = id_usuario_appStr.Split(",")
            Dim indx As Integer = Convert.ToInt32(Me.Session("E_IDUser"))



            Me.cmb_tipo_permiso.DataSourceID = ""
            Me.cmb_tipo_permiso.DataSource = dbEntities.tme_tipo_permiso_par.ToList()
            Me.cmb_tipo_permiso.DataTextField = "tipo_permiso"
            Me.cmb_tipo_permiso.DataValueField = "id_tipo_permiso_par"
            Me.cmb_tipo_permiso.DataBind()


            'Me.lbl_categoria.Text = par.ta_documento_par.FirstOrDefault().ta_documento.ta_tipoDocumento.descripcion_aprobacion
            'Me.lbl_codigo.Text = par.codigo_par
            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_crea)
            Me.lbl_fecha_entrega.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_requiere_servicio)
            Me.lbl_solicitado.Text = parDetalle.usuario_solicita
            Me.lbl_aprobado.Text = parDetalle.nombre_departamento & " - " & parDetalle.nombre_municipio
            Me.lbl_cargo.Text = parDetalle.cargo_usuario
            Me.lbl_codigo_rfa.Text = If(parDetalle.asociado_actividad = True, parDetalle.subactividad, "")
            Me.lbl_proposito_par.Text = parDetalle.tipo_solicitud
            Me.lbl_asociado_a_par.Text = parDetalle.asociado_a
            'Me.lbl_usuario_solicitud.Text = parDetalle.usuario_cargo_solicita & " (" & parDetalle.cargo_usuario & ")"
            'Me.lbl_tasa_ser.Text = parDetalle.tasa_ser_cotizacion
            'Me.lbl_codigo_pt.Text = par.codigo_pt
            Me.lbl_departamento.Text = parDetalle.nombre_region
            Me.lbl_tipo_par.Text = parDetalle.tipo_par
            'Me.lbl_regional.Text = par.t_subregiones.t_regiones.nombre_region
            'Me.lbl_sub_region.Text = par.t_subregiones.nombre_subregion
            'Me.lbl_proposito_par.Text = par.proposito
            'Me.lbl_municipio_entrega.Text = par.t_municipios.t_departamentos.nombre_departamento & " " & par.t_municipios.nombre_municipio
            Me.lbl_tipo_par.Text = par.tme_tipo_par.tipo_par

            For Each item In par.tme_par_detalle.ToList()
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio & item.id_par_detalle

                dtConceptos.Rows.Add(idunique, item.cantidad, item.descripcion, item.precio_unitario, item.valor_total, item.tme_unidad_medida_par.unidad_medida, True, item.id_par_detalle)
            Next


            Me.grd_servicios_requeridos.DataSource = dbEntities.tme_par_detalle.Where(Function(p) p.id_par = id_par).Select(Function(p) New With {Key p.descripcion,
                                                                                                                                p.cantidad,
                                                                                                                                p.precio_unitario,
                                                                                                                                p.valor_total,
                                                                                                                                .unidad_medida = p.tme_unidad_medida_par.unidad_medida,
                                                                                                                                p.id_par_detalle}).ToList()
            Me.grd_servicios_requeridos.DataBind()

            Session("dtConceptos") = dtConceptos
            If par.ta_documento_par.Count() > 0 Then
                Me.HiddenField1.Value = par.ta_documento_par.LastOrDefault().id_documento
                fillGridRutaAprobacion(par.ta_documento_par.LastOrDefault().id_documento)
            End If

        End Using
    End Sub
    Sub fillGrid()
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If
        'Me.grd_conceptos.DataSource = dtConceptos
        'Me.grd_conceptos.DataBind()
    End Sub
    Sub createdtcolums()
        If dtConceptos.Columns.Count = 0 Then
            dtConceptos.Columns.Add("id_par_detalle", GetType(String))
            dtConceptos.Columns.Add("cantidad", GetType(Double))
            dtConceptos.Columns.Add("descripcion", GetType(String))
            dtConceptos.Columns.Add("valor_unitario", GetType(Double))
            dtConceptos.Columns.Add("valor", GetType(Double))
            dtConceptos.Columns.Add("unidad_medida", GetType(String))
            dtConceptos.Columns.Add("esta_bd", GetType(Boolean))
            dtConceptos.Columns.Add("id_par_detalle_bd", GetType(Integer))
        End If
    End Sub
    Sub fillGridRutaAprobacion(ByVal id_documento As Integer)
        Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(id_documento.ToString())
        Me.grd_cate.DataBind()
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_par = Convert.ToInt32(Me.idPar.Value)

                Dim par = dbEntities.tme_pares.Find(id_par)
                Dim oPermisosPar = dbEntities.tme_par_permisos.Where(Function(p) p.id_par = id_par).ToList().FirstOrDefault()
                Dim historial = New tme_par_historial_permisos
                Dim esnuevo = False
                If oPermisosPar Is Nothing Then
                    oPermisosPar = New tme_par_permisos
                    esnuevo = True
                End If
                Dim tipoPermiso = Convert.ToInt32(Me.cmb_tipo_permiso.SelectedValue)
                If tipoPermiso = 1 Then
                    oPermisosPar.editar_par = True
                ElseIf tipoPermiso = 2 Then
                    oPermisosPar.habilitar_facturacion = True
                ElseIf tipoPermiso = 3 Then

                    Dim documento = dbEntities.ta_documento_par.Where(Function(p) p.id_par = id_par And p.reversado Is Nothing).ToList().FirstOrDefault()
                    If documento IsNot Nothing Then
                        documento.reversado = True
                        documento.id_usuario_reverso = Me.Session("E_IdUser").ToString()
                        documento.fecha_reversion = DateTime.Now
                        documento.motivo_reversion = Me.txt_motivo.Text
                        dbEntities.Entry(documento).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()


                        Dim idDoc = par.ta_documento_par.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                        Dim appDocumeto = dbEntities.ta_AppDocumento.Where(Function(p) p.id_documento = idDoc).OrderByDescending(Function(p) p.id_App_Documento).ToList().FirstOrDefault()
                        appDocumeto.id_estadoDoc = 4
                        appDocumeto.datecreated = DateTime.Now
                        appDocumeto.observacion = Me.txt_motivo.Text & " By " & Me.txt_usuario_solicita.Text
                        appDocumeto.id_usuario_app = Convert.ToInt32(Me.Session("E_IDUser"))

                        dbEntities.Entry(appDocumeto).State = Entity.EntityState.Modified
                        dbEntities.SaveChanges()
                    End If

                End If

                historial.id_tipo_permiso_par = tipoPermiso
                historial.id_par = id_par
                historial.motivo = Me.txt_motivo.Text
                historial.usuario_solicito = Me.txt_usuario_solicita.Text
                historial.fecha = DateTime.Now

                dbEntities.tme_par_historial_permisos.Add(historial)

                If esnuevo Then
                    oPermisosPar.id_par = id_par
                    dbEntities.tme_par_permisos.Add(oPermisosPar)
                Else
                    dbEntities.Entry(oPermisosPar).State = Entity.EntityState.Modified
                End If

                dbEntities.SaveChanges()

                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/administrativo/frm_par"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End Using
        Catch ex As Exception
            Dim aa = ex.Message
        End Try
    End Sub
    Sub fillGridHistorial()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_par = Convert.ToInt32(Me.idPar.Value)
            Dim viajeHistorial = dbEntities.tme_par_historial_permisos.Where(Function(p) p.id_par = id_par).Select(Function(p) _
                                             New With {Key p.fecha,
                                                       Key p.usuario_solicito,
                                                       Key p.motivo,
                                                       Key p.id_par_historial_permisos,
                                                       Key p.tme_tipo_permiso_par.tipo_permiso}).ToList()
            Me.grd_historial_reversiones.DataSource = viajeHistorial
            Me.grd_historial_reversiones.DataBind()
        End Using
    End Sub
End Class