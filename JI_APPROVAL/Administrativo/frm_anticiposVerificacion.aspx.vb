Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Configuration.ConfigurationManager
Imports ly_APPROVAL
Public Class frm_anticiposVerificacion
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_ANTICIPOS_AD"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtRutas As New DataTable
    Dim dtCompras As New DataTable
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
            Dim idAnticipo = Convert.ToInt32(Me.Request.QueryString("id"))
            Me.id_anticipo.Value = idAnticipo
            loadLista()
            loadData(idAnticipo)
            loadRutas(idAnticipo)
        End If
    End Sub
    Sub loadRutas(idAnticipo As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim rutas = dbEntities.tme_anticipos.Find(idAnticipo)
        End Using
    End Sub
    Sub loadData(idAnticipo As Integer)
        Using dbEntities As New dbRMS_JIEntities
            Dim anticipo = dbEntities.tme_anticipos.Find(idAnticipo)
            Me.cmb_sub_Region.SelectedValue = anticipo.id_subregion
            Me.dt_fecha_anticipo.SelectedDate = anticipo.fecha_anticipo
            Me.cmb_medio_pago.SelectedValue = anticipo.id_medio_pago
            Me.txt_detalle_medio_pago.Text = anticipo.observaciones_medio_pago
            Me.cmb_tipo_anticipo.SelectedValue = anticipo.id_tipo_anticipo
            loadPar()
            Me.cmb_par.SelectedValue = anticipo.id_par
            Me.txt_motivo_anticipo.Text = anticipo.motivo
        End Using
    End Sub
    Sub loadPar()
        Using dbEntities As New dbRMS_JIEntities
            'Me.anticipo_fondos.Visible = False
            'Me.info_par.Visible = False
            Me.cmb_par.ClearSelection()
            Me.cmb_par.DataSourceID = ""
            If Me.cmb_tipo_anticipo.SelectedValue = "2" And Me.cmb_sub_Region.SelectedValue <> "" Then
                'Me.info_par.Visible = True
                'Me.anticipo_fondos.Visible = True
                Dim idSubRegion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                Me.cmb_par.DataSource = dbEntities.tme_pares.Where(Function(p) p.id_subregion = idSubRegion And p.id_tipo_par = 2).OrderByDescending(Function(p) p.id_par).ToList()
            ElseIf Me.cmb_tipo_anticipo.SelectedValue = "1" And Me.cmb_sub_Region.SelectedValue <> "" Then
                Dim idSubRegion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
                Me.cmb_par.DataSource = dbEntities.tme_pares.Where(Function(p) p.id_subregion = idSubRegion And p.id_tipo_par = 1).OrderByDescending(Function(p) p.id_par).ToList()
            End If

            Me.cmb_par.DataTextField = "codigo_par"
            Me.cmb_par.DataValueField = "id_par"
            Me.cmb_par.DataBind()
        End Using
    End Sub
    Sub loadLista()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            Dim usuario = dbEntities.t_usuarios.Find(idUser)
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

            Me.cmb_medio_pago.DataSourceID = ""
            Me.cmb_medio_pago.DataSource = dbEntities.tme_medio_pago.ToList()
            Me.cmb_medio_pago.DataTextField = "medio_pago"
            Me.cmb_medio_pago.DataValueField = "id_medio_pago"
            Me.cmb_medio_pago.DataBind()

            Me.cmb_tipo_anticipo.DataSourceID = ""
            Me.cmb_tipo_anticipo.DataSource = dbEntities.tme_tipo_par.ToList()
            Me.cmb_tipo_anticipo.DataTextField = "tipo_par"
            Me.cmb_tipo_anticipo.DataValueField = "id_tipo_par"
            Me.cmb_tipo_anticipo.DataBind()
        End Using

    End Sub
End Class