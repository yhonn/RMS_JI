Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Public Class frm_contratosMod
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
            Dim modificaciones = dbEntities.tme_contrato_modificaciones.where(Function(p) p.id_contrato = id_contrato).toList()
            If modificaciones.Count() = 0 Then
                Me.modificacionesList.Visible = False
            End If
        End Using
    End Sub
    Sub fillGrid()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_contrato = Convert.ToInt32(Me.idContrato.Value)

            Me.grd_entregables.DataSource = dbEntities.tme_contrato_modificaciones.Where(Function(p) p.id_contrato = id_contrato).Select(Function(p) New With
                                                                                                                                         {Key .id_modificacion_contrato = p.id_modificacion_contrato,
                                                                                                                                          Key .fecha_modificacion = p.fecha_modificacion,
                                                                                                                                          Key .objetivo_modificacion = p.objetivo_modificacion,
                                                                                                                                          Key .tipo_modificacion = p.tme_tipo_modificacion_contrato.tipo_modificacion
                                                                                                                                         }).ToList()
            Me.grd_entregables.DataBind()

            'Dim id_contrato = Convert.ToInt32(Me.idContrato.Value)
            'Me.grd_entregables.DataSource = dbEntities.vw_contratos_entregables.Where(Function(p) p.id_contrato = id_contrato).ToList()
            'Me.grd_entregables.DataBind()
        End Using
    End Sub

    Private Sub btn_registrar_modifiacion_Click(sender As Object, e As EventArgs) Handles btn_registrar_modifiacion.Click
        Dim id_contrato = Convert.ToInt32(Me.idContrato.Value)
        Me.Response.Redirect("~/contratistas/frm_contratosModAd?id=" & id_contrato)
    End Sub
End Class