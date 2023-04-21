Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports ly_APPROVAL

Public Class frm_radicadosEdit
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_RADICADO_EDIT"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles


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
                cl_user.chk_Rights(Page.Controls, 5, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            LoadLista()
            Dim id_radicado = Convert.ToInt32(Me.Request.QueryString("id"))
            Dim anularRadicado = 0
            If Me.Request.QueryString("a") IsNot Nothing Then
                anularRadicado = 1
            End If
            Me.anularRadicadoV.Value = anularRadicado
            Me.idRadicado.Value = id_radicado

            If anularRadicado = 1 Then
                Me.anulaR.Visible = True
            Else
                Me.cambioEstado.Visible = True
            End If
            loadData(id_radicado)
        End If

    End Sub
    Sub loadData(ByVal id_radicado As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim oRadicado = dbEntities.tme_radicados.Find(id_radicado)

            Dim editarR = Convert.ToString(Me.h_Filter_editar_radicado.Value)
            If Convert.ToInt32(Me.anularRadicadoV.Value) = 1 Then
                If oRadicado.id_usuario_crea <> Convert.ToInt32(Me.Session("E_IdUser").ToString()) Then
                    Me.Response.Redirect("~/financiero/frm_radicados")
                End If
            ElseIf editarR = "" Then
                Me.Response.Redirect("~/financiero/frm_radicados")
            End If


            Me.cmb_sub_Region.SelectedValue = oRadicado.id_subregion
            Me.cmb_caracter.SelectedValue = oRadicado.id_caracter
            Me.txt_valor_factura.Value = oRadicado.valor_factura
            Me.txt_identificacion_tercero_a_pagar_.Value = oRadicado.identificacion_tercero_a_pagar
            Me.txt_tercero_a_pagar.Text = oRadicado.tercero_a_pagar
            Me.txt_observaciones.Text = oRadicado.observaciones
            Me.txt_documento.Text = oRadicado.detalle
            Me.cmb_estado.SelectedValue = oRadicado.id_estado
            If oRadicado.id_tipo_documento IsNot Nothing Then
                Me.cmb_tipo_documento.SelectedValue = oRadicado.id_tipo_documento
            End If
        End Using
    End Sub
    Sub LoadLista()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities


            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            Dim usuario = dbEntities.t_usuarios.Find(idUser)

            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy hh:mm}", DateTime.Now)

            Dim regionesUsuario = usuario.t_usuario_subregion.ToList()
            Me.cmb_sub_Region.DataSourceID = ""
            If regionesUsuario.Count() = 1 Then
                Dim subRegion = regionesUsuario.Select(Function(p) _
                                            New With {Key .nombre_subregion = p.t_subregiones.t_regiones.nombre_region & " - " & p.t_subregiones.nombre_subregion,
                                                      Key .id_subregion = p.id_subregion}).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.cmb_sub_Region.SelectedValue = subRegion.FirstOrDefault().id_subregion
            ElseIf regionesUsuario.Count() > 0 Then
                Dim subRegion = regionesUsuario.Select(Function(p) _
                                             New With {Key .nombre_subregion = p.t_subregiones.t_regiones.nombre_region & " - " & p.t_subregiones.nombre_subregion,
                                                       Key .id_subregion = p.id_subregion}).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.subRegionVisible.Visible = True
            Else
                Dim subRegion = dbEntities.t_subregiones.Where(Function(p) p.t_regiones.id_programa = id).Select(Function(p) _
                                             New With {Key .nombre_subregion = p.t_regiones.nombre_region & " - " & p.nombre_subregion,
                                                       Key .id_subregion = p.id_subregion}).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.subRegionVisible.Visible = True
            End If


            Me.cmb_sub_Region.DataTextField = "nombre_subregion"
            Me.cmb_sub_Region.DataValueField = "id_subregion"
            Me.cmb_sub_Region.DataBind()

            Me.cmb_caracter.DataSource = dbEntities.tme_caracter_radicados.Where(Function(p) p.id_programa = id).ToList()
            Me.cmb_caracter.DataTextField = "caracter"
            Me.cmb_caracter.DataValueField = "id_caracter"
            Me.cmb_caracter.DataBind()

            Me.cmb_estado.DataSource = dbEntities.tme_estado_radicados.Where(Function(p) p.id_programa = id).ToList()
            Me.cmb_estado.DataTextField = "estado_radicado"
            Me.cmb_estado.DataValueField = "id_estado_radicado"
            Me.cmb_estado.DataBind()

            Me.cmb_tipo_documento.DataSource = dbEntities.tme_tipo_documento_radicados.Where(Function(p) p.id_programa = id).ToList()
            Me.cmb_tipo_documento.DataTextField = "tipo_documento"
            Me.cmb_tipo_documento.DataValueField = "id_tipo_documento"
            Me.cmb_tipo_documento.DataBind()
        End Using
    End Sub

    Private Sub cmb_estado_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_estado.SelectedIndexChanged
        Me.codigo_gj.Visible = False
        Me.rechazo.Visible = False
        If (Convert.ToInt32(Me.cmb_estado.SelectedValue) = 2) Then
            Me.codigo_gj.Visible = True
        ElseIf (Convert.ToInt32(Me.cmb_estado.SelectedValue) = 3) Then
            Me.rechazo.Visible = True
        End If
    End Sub
    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_radicado = Convert.ToInt32(Me.idRadicado.Value)
                Dim oRadicado = dbEntities.tme_radicados.Find(id_radicado)
                Dim anula = Convert.ToInt32(Me.anularRadicadoV.Value)
                If anula = 0 Then
                    oRadicado.id_usuario_aprobo = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    oRadicado.fecha_aprobacion = DateTime.Now
                    oRadicado.id_estado = Me.cmb_estado.SelectedValue
                    oRadicado.codigo_GJ = Me.txt_codigo_gj.Text.ToUpper()
                    oRadicado.motivo_rechazo = Me.txt_motivo_rechazo.Text.ToUpper()
                Else
                    oRadicado.fecha_anula = DateTime.Now
                    oRadicado.id_usuario_anula = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    oRadicado.motivo_anula = Me.txt_motivo_anula.Text.ToUpper()
                    oRadicado.anulado = True
                End If

                dbEntities.Entry(oRadicado).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

                Dim radicadoDetalle = dbEntities.vw_tme_radicados.Where(Function(p) p.id_radicado = oRadicado.id_radicado).FirstOrDefault()
                If oRadicado.id_estado = 3 Then
                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), -1, 1026, cl_user.regionalizacionCulture, -1)
                    If (objEmail.Emailing_RADICADOS(radicadoDetalle)) Then
                    Else 'Error mandando Email
                    End If
                End If

                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/financiero/frm_radicados"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End Using
        Catch ex As Exception

        End Try

    End Sub
End Class