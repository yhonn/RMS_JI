Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Public Class frm_contratosModAd
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_CONT_MOD"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtItinerario As New DataTable
    Dim dtAlojamiento As New DataTable
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
                'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_entregables)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            Me.idContrato.Value = Convert.ToInt32(Me.Request.QueryString("id"))
            loadData()
            fillGrid()
        End If

    End Sub
    Sub loadData()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_contrato = Convert.ToInt32(Me.idContrato.Value)
            Dim oContrato = dbEntities.vw_tme_contratos.Where(Function(p) p.id_contrato = id_contrato).FirstOrDefault()
            Me.lbl_cod_contrato.Text = oContrato.numero_contrato
            Me.lbl_contratista.Text = oContrato.contratista
            Me.lbl_fecha_inicio.Text = oContrato.fecha_inicio
            Me.lbl_fecha_fin.Text = oContrato.fecha_finalizacion
            Me.lbl_valor.Text = "$" & String.Format("{0:N}", oContrato.valor_contrato)
            Me.lbl_objeto.Text = oContrato.objeto_contrato
            Me.lbl_supervisor.Text = oContrato.supervisor

            Me.cmb_tipo_modificacion.DataSourceID = ""
            Me.cmb_tipo_modificacion.DataSource = dbEntities.tme_tipo_modificacion_contrato.ToList()
            Me.cmb_tipo_modificacion.DataTextField = "tipo_modificacion"
            Me.cmb_tipo_modificacion.DataValueField = "id_tipo_modificacion_contrato"
            Me.cmb_tipo_modificacion.DataBind()

        End Using
    End Sub
    Sub fillGrid()
        Using dbEntities As New dbRMS_JIEntities
            'Dim id_contrato = Convert.ToInt32(Me.idContrato.Value)
            'Me.grd_entregables.DataSource = dbEntities.vw_contratos_entregables.Where(Function(p) p.id_contrato = id_contrato).ToList()
            'Me.grd_entregables.DataBind()
        End Using
    End Sub

    Private Sub btn_guardar_modificacion_Click(sender As Object, e As EventArgs) Handles btn_guardar_modificacion.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim id_contrato = Convert.ToInt32(Me.idContrato.Value)
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim modificacion = New tme_contrato_modificaciones
            modificacion.id_contrato = id_contrato
            modificacion.id_tipo_modificacion_contrato = Convert.ToInt32(Me.cmb_tipo_modificacion.SelectedValue)
            modificacion.objetivo_modificacion = Me.txt_objetivo_modificacion.Text
            modificacion.fecha_modificacion = Me.dt_fecha_modificacion.SelectedDate
            modificacion.fecha_crea = DateTime.Now
            modificacion.id_usuario_crea = Me.Session("E_IdUser").ToString()
            modificacion.modificacion_finalizada = False
            For Each file As UploadedFile In soporte.UploadedFiles
                Dim fecha = DateTime.Now
                Dim exten = file.GetExtension()
                Dim nombreArchivo = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & "_" & Regex.Replace(file.GetNameWithoutExtension(), "[^A-Za-z0-9\-/]", "") & "" & exten
                Dim Path As String
                modificacion.soporte = nombreArchivo
                Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).documents_folder)
                file.SaveAs(Path + nombreArchivo)
            Next

            If modificacion.id_tipo_modificacion_contrato = 3 Then
                modificacion.tme_contratos.anulado = True
            End If

            dbEntities.tme_contrato_modificaciones.Add(modificacion)
            dbEntities.SaveChanges()

            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/contratistas/frm_contratosModIG?id=" & modificacion.id_modificacion_contrato
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)


        End Using
    End Sub
End Class