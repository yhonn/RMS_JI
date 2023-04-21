Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class frm_ActualizarEntregable
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_STO_HITOS"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clListados As New ly_SIME.CORE.cls_listados

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
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_JIEntities
                Dim idEntregable = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Me.idEntregable.Value = idEntregable
                Dim entregable = dbEntities.tme_hitos_entregables.FirstOrDefault(Function(p) p.id_entregable_hito = idEntregable)
                Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = entregable.tme_ficha_proyecto_hitos.id_ficha_proyecto)
                Me.lbl_cod_acti.Text = proyecto.codigo_RFA
                Me.lbl_actividad.Text = proyecto.nombre_proyecto
                Me.lbl_nombre_hito.Text = entregable.tme_ficha_proyecto_hitos.descripcion_hito
                Me.lbl_num_hito.Text = entregable.tme_ficha_proyecto_hitos.nro_hito
                Me.lbl_nombre_hito.Text = entregable.tme_ficha_proyecto_hitos.descripcion_hito
                Me.lbl_entregable.Text = entregable.descripcion_entregable
                Me.lbl_num_entregable.Text = entregable.nro_entregable
                'Me.SqlDts_comentarios.SelectCommand = "select * from vw_productos where id_ficha_entregable = " & idEntregable
                'Me.grd_aportes.DataBind()
                loadData(idEntregable)
            End Using
        End If
    End Sub
    Sub loadData(ByVal idEntregable As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim Entregable = dbEntities.tme_hitos_entregables.Find(idEntregable)
            Me.txt_url.Text = Entregable.soporte
            Me.txt_observaciones_entregable.Text = Entregable.observaciones_entregable
        End Using
    End Sub
    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click

        Using dbEntities As New dbRMS_JIEntities

            Dim idEntregable = Convert.ToInt32(Me.idEntregable.Value)
            Dim Entregable = dbEntities.tme_hitos_entregables.Find(idEntregable)

            Entregable.soporte = Me.txt_url.Text
            Entregable.observaciones_entregable = Me.txt_observaciones_entregable.Text
            dbEntities.Entry(Entregable).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            'Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectosEntregables?id=" & Me.lbl_id_ficha.Text
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_sgtoEntregables?id=" & Entregable.id_hito
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
        'guardarAportes()

    End Sub
End Class