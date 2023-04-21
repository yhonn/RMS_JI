Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Drawing

Public Class frm_parAnular
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_PAR_ANULAR"
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
    Sub fillGridRutaAprobacion(ByVal id_documento As Integer)
        Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(id_documento.ToString())
        Me.grd_cate.DataBind()
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

            'If parDetalle IsNot Nothing Then
            '    Dim intOwnerLegalizacion As String() = parDetalle.id_usuario_app.ToString.Split(",")
            '    If intOwnerLegalizacion.Where(Function(p) p.Contains(indx)).Count() = 0 Then
            '        Me.Response.Redirect("~/administrativo/frm_parDetalle?id=" & id_par)
            '    End If
            'End If

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

            Dim tbl_AppOrden As New DataTable
            tbl_AppOrden = clss_approval.get_ta_AppDocumentoAPP_MAX(Me.HiddenField1.Value) 'To get the info on the max step (id_app_doc

            'If parDetalle IsNot Nothing Then
            '    Dim intOwnerLegalizacion As String() = parDetalle.id_usuario_app.ToString.Split(",")
            '    If intOwnerLegalizacion.Where(Function(p) p.Contains(indx)).Count() = 0 Then
            '        Me.Response.Redirect("~/administrativo/frm_parDetalle?id=" & id_par)
            '    End If
            'End If

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
    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/administrativo/frm_par")
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_par = Convert.ToInt32(Me.idPar.Value)
                Dim par = dbEntities.tme_pares.Find(id_par)
                par.id_usuario_anula = Me.Session("E_IdUser").ToString()
                par.fecha_anula = DateTime.Now
                par.anulado = True
                par.motivo_anula = Me.txt_motivo_anular.Text
                dbEntities.Entry(par).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

                If par.ta_documento_par.Where(Function(p) p.reversado Is Nothing).Count() > 0 Then
                    Dim idDoc = par.ta_documento_par.Where(Function(p) p.reversado Is Nothing).FirstOrDefault().id_documento
                    Dim documeto = dbEntities.ta_AppDocumento.Where(Function(p) p.id_documento = idDoc).OrderByDescending(Function(p) p.id_App_Documento).ToList().FirstOrDefault()
                    documeto.id_estadoDoc = 4
                    documeto.datecreated = DateTime.Now
                    documeto.observacion = Me.txt_motivo_anular.Text

                    dbEntities.Entry(documeto).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()

                End If

            End Using

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/administrativo/frm_par"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub
End Class