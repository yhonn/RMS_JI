Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Public Class frm_entregablePrint
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_CONT_SGMT_ENTR"
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
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            'fillGrid()
            loadData()
        End If

    End Sub

    'Sub fillGrid()
    '    Using dbEntities As New dbRMS_JIEntities
    '        Dim id_entregable = Convert.ToInt32(Me.Request.QueryString("id"))
    '        Me.grd_entregables.DataSource = dbEntities.vw_contratos_entregables.Where(Function(p) p.id_contrato = id_contrato).ToList()
    '        Me.grd_entregables.DataBind()
    '    End Using
    'End Sub
    Sub loadData()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_entregable = Convert.ToInt32(Me.Request.QueryString("id"))
            Dim entregable = dbEntities.tme_contrato_entregables.Find(id_entregable)
            Dim contrato = entregable.tme_contratos
            Me.lbl_codigo.Text = contrato.numero_contrato
            Me.lbl_valor_contrato.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", contrato.valor_contrato)
            Me.lbl_contratista.Text = contrato.t_usuarios.nombre_usuario & " " & contrato.t_usuarios.apellidos_usuario
            Me.lbl_supervisor.Text = contrato.t_usuarios1.nombre_usuario & " " & contrato.t_usuarios1.apellidos_usuario
            Me.lbl_fecha_inicio.Text = contrato.fecha_inicio.Year & "-" & contrato.fecha_inicio.Month & "-" & contrato.fecha_inicio.Day
            Me.lbl_fecha_fin.Text = contrato.fecha_finalizacion.Year & "-" & contrato.fecha_finalizacion.Month & "-" & contrato.fecha_finalizacion.Day

            Me.lbl_numero_entregable.Text = entregable.numero_entregable
            Me.lbl_valor_entregable.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", entregable.valor_entregable)
            Me.lbl_fecha_esperada_entrega.Text = entregable.fecha_esperada_entrega.Value.Year & "-" & entregable.fecha_esperada_entrega.Value.Month & "-" & entregable.fecha_esperada_entrega.Value.Day
            If entregable.fecha_entrega IsNot Nothing Then
                Me.lbl_fecha_entrega.Text = entregable.fecha_entrega.Value.Year & "-" & entregable.fecha_entrega.Value.Month & "-" & entregable.fecha_entrega.Value.Day
            End If
            Me.lbl_objeto.Text = contrato.objeto_contrato
            Me.lbl_producto.Text = entregable.productos
            Me.docs_admon.NavigateUrl = entregable.url
            If entregable.url IsNot Nothing Then
                Me.docs_admon.Text = "Soporte: " & entregable.url
            End If
        End Using
    End Sub

    'Private Sub btn_agregar_entregable_Click(sender As Object, e As EventArgs) Handles btn_agregar_entregable.Click
    '    Try
    '        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
    '        Using dbEntities As New dbRMS_JIEntities
    '            Dim oContratoEntregable = New tme_contrato_entregables
    '            Dim idEntregable = Convert.ToInt32(Me.identity.Text)
    '            Dim id_contrato = Convert.ToInt32(Me.idContrato.Value)

    '            If (idEntregable <> 0) Then
    '                oContratoEntregable = dbEntities.tme_contrato_entregables.Find(idEntregable)
    '            Else
    '                oContratoEntregable.id_contrato = id_contrato
    '                oContratoEntregable.fecha_crea = Date.Now
    '                oContratoEntregable.id_estado_aprobacion_entregable = 1
    '                oContratoEntregable.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '                oContratoEntregable.aprobado = False
    '            End If

    '            oContratoEntregable.fecha_esperada_entrega = Me.dt_fecha_entrega.SelectedDate
    '            oContratoEntregable.numero_entregable = Me.txt_numero_entregable.Value
    '            oContratoEntregable.nombre_entregable = Me.txt_nombre_entregble.Text
    '            oContratoEntregable.valor_entregable = Me.txt_valor_entregable.Value
    '            oContratoEntregable.productos = Me.txt_productos.Text

    '            If (idEntregable <> 0) Then
    '                dbEntities.Entry(oContratoEntregable).State = Entity.EntityState.Modified
    '                dbEntities.SaveChanges()
    '            Else
    '                dbEntities.tme_contrato_entregables.Add(oContratoEntregable)
    '                dbEntities.SaveChanges()
    '            End If
    '            fillGrid()

    '            Dim contrato = dbEntities.tme_contratos.Find(id_contrato)
    '            If contrato.valor_contrato = contrato.tme_contrato_entregables.Sum(Function(p) p.valor_entregable) Then
    '                Me.errorTotalContrato.Visible = False
    '            End If

    '            Me.dt_fecha_entrega.Clear()
    '            Me.txt_numero_entregable.Text = String.Empty
    '            Me.txt_nombre_entregble.Text = String.Empty
    '            Me.txt_valor_entregable.Text = String.Empty
    '            Me.txt_productos.Text = String.Empty

    '            loadData()

    '            Me.MsgGuardar.NuevoMensaje = "Registro guardado"
    '            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    '        End Using
    '    Catch ex As Exception
    '        Dim mensaje = ex.Message
    '    End Try
    'End Sub

    'Private Sub grd_entregables_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_entregables.ItemDataBound
    '    If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

    '        Dim EstadoEntregable = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estado_aprobacion_entregable").ToString())
    '        Dim hlnkDelete As LinkButton = New LinkButton
    '        hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)

    '        Dim hlnkEdit As LinkButton = New LinkButton
    '        hlnkEdit = CType(e.Item.FindControl("col_hlk_editar"), LinkButton)
    '        If EstadoEntregable = 1 Then
    '            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString())
    '            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString())

    '            hlnkEdit.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString())
    '            hlnkEdit.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString())
    '        Else
    '            hlnkDelete.Visible = False
    '            hlnkEdit.Visible = False
    '        End If

    '        'Dim hlnkActivar As HyperLink = New HyperLink
    '        'hlnkActivar = CType(e.Item.FindControl("hlk_activar"), HyperLink)
    '        'hlnkActivar.NavigateUrl = "~/contratistas/frm_entregableProducto?id=" & DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString()

    '    End If
    'End Sub

    'Protected Sub Editar_Click(sender As Object, e As EventArgs)
    '    Dim a = CType(sender, LinkButton)
    '    Using dbEntities As New dbRMS_JIEntities
    '        Dim idEntregable = Convert.ToInt32(a.Attributes.Item("data-identity").ToString())
    '        Me.identity.Text = idEntregable
    '        Dim oEntregable = dbEntities.tme_contrato_entregables.Find(idEntregable)

    '        Me.txt_nombre_entregble.Text = oEntregable.nombre_entregable
    '        Me.dt_fecha_entrega.SelectedDate = oEntregable.fecha_esperada_entrega
    '        Me.txt_valor_entregable.Value = oEntregable.valor_entregable
    '        Me.txt_numero_entregable.Value = oEntregable.numero_entregable
    '        Me.txt_productos.Text = oEntregable.productos
    '    End Using
    'End Sub

    'Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
    '    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
    '    Me.MsgGuardar.Redireccion = "~/contratistas/frm_contratos"
    '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    'End Sub

    'Private Sub btn_salir_Click(sender As Object, e As EventArgs) Handles btn_salir.Click
    '    Me.Response.Redirect("~/contratistas/frm_contratos")
    'End Sub
End Class