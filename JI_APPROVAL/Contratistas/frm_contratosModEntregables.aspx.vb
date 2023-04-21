Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Public Class frm_contratosModEntregables
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
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            Me.idModificacion.Value = Convert.ToInt32(Me.Request.QueryString("id"))
            Me.anchorInformation.Attributes.Add("class", "btn btn-success btn-circle")
            Me.anchorInformation.Attributes.Add("href", "frm_contratosModIG.aspx?Id=" & Me.idModificacion.Value)

            Me.anchorMod.Attributes.Add("class", "btn btn-success btn-circle")
            Me.anchorMod.Attributes.Add("href", "frm_contratosModAd.aspx?Id=" & Me.idModificacion.Value)



            fillGrid()
            loadData()
        End If

    End Sub
    Sub fillGrid()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_modificacion = Convert.ToInt32(Me.idModificacion.Value)

            Dim modificacion = dbEntities.tme_contrato_modificaciones.Find(id_modificacion)

            Me.grd_entregables.DataSource = dbEntities.vw_contratos_entregables.Where(Function(p) p.id_contrato = modificacion.id_contrato).ToList()
            Me.grd_entregables.DataBind()
        End Using
    End Sub
    Sub loadData()
        Using dbEntities As New dbRMS_JIEntities


            Dim id_modificacion = Convert.ToInt32(Me.idModificacion.Value)

            Dim modificacion = dbEntities.tme_contrato_modificaciones.Find(id_modificacion)
            Dim contrato = modificacion.tme_contratos


            Me.lbl_total.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", contrato.valor_contrato)
            Me.lbl_sumatoria.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", contrato.tme_contrato_entregables.Sum(Function(p) p.valor_entregable))
            Me.lbl_diferencia.Text = String.Format(cl_user.regionalizacionCulture, "{0:C2}", contrato.valor_contrato - contrato.tme_contrato_entregables.Sum(Function(p) p.valor_entregable))

            If contrato.valor_contrato = contrato.tme_contrato_entregables.Sum(Function(p) p.valor_entregable) Then
                Me.errorTotalContrato.Visible = False
            End If
        End Using
    End Sub

    Private Sub btn_agregar_entregable_Click(sender As Object, e As EventArgs) Handles btn_agregar_entregable.Click
        Try
            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Using dbEntities As New dbRMS_JIEntities
                Dim oContratoEntregable = New tme_contrato_entregables
                Dim idEntregable = Convert.ToInt32(Me.identity.Text)

                Dim id_modificacion = Convert.ToInt32(Me.idModificacion.Value)

                Dim modificacion = dbEntities.tme_contrato_modificaciones.Find(id_modificacion)
                Dim contrato = modificacion.tme_contratos




                If (idEntregable <> 0) Then
                    oContratoEntregable = dbEntities.tme_contrato_entregables.Find(idEntregable)
                Else
                    oContratoEntregable.id_contrato = contrato.id_contrato
                    oContratoEntregable.fecha_crea = Date.Now
                    oContratoEntregable.id_estado_aprobacion_entregable = 1
                    oContratoEntregable.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                    oContratoEntregable.aprobado = False
                End If

                oContratoEntregable.fecha_esperada_entrega = Me.dt_fecha_entrega.SelectedDate
                oContratoEntregable.numero_entregable = Me.txt_numero_entregable.Value
                oContratoEntregable.nombre_entregable = Me.txt_nombre_entregble.Text
                oContratoEntregable.valor_entregable = Me.txt_valor_entregable.Value
                oContratoEntregable.productos = Me.txt_productos.Text

                If (idEntregable <> 0) Then
                    dbEntities.Entry(oContratoEntregable).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()
                Else
                    dbEntities.tme_contrato_entregables.Add(oContratoEntregable)
                    dbEntities.SaveChanges()
                End If
                fillGrid()

                If contrato.valor_contrato = contrato.tme_contrato_entregables.Sum(Function(p) p.valor_entregable) Then
                    Me.errorTotalContrato.Visible = False
                End If

                Me.dt_fecha_entrega.Clear()
                Me.txt_numero_entregable.Text = String.Empty
                Me.txt_nombre_entregble.Text = String.Empty
                Me.txt_valor_entregable.Text = String.Empty
                Me.txt_productos.Text = String.Empty
                Me.identity.Text = 0

                loadData()

                Me.MsgGuardar.NuevoMensaje = "Registro guardado"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

            End Using
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try
    End Sub

    Private Sub grd_entregables_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_entregables.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim EstadoEntregable = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_estado_aprobacion_entregable").ToString())
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)

            Dim hlnkEdit As LinkButton = New LinkButton
            hlnkEdit = CType(e.Item.FindControl("col_hlk_editar"), LinkButton)
            If EstadoEntregable = 1 Then
                hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString())
                hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString())

                hlnkEdit.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString())
                hlnkEdit.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString())
            Else
                hlnkDelete.Visible = False
                hlnkEdit.Visible = False
            End If

            'Dim hlnkActivar As HyperLink = New HyperLink
            'hlnkActivar = CType(e.Item.FindControl("hlk_activar"), HyperLink)
            'hlnkActivar.NavigateUrl = "~/contratistas/frm_entregableProducto?id=" & DataBinder.Eval(e.Item.DataItem, "id_contrato_entregable").ToString()

        End If
    End Sub

    Protected Sub Editar_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Using dbEntities As New dbRMS_JIEntities
            Dim idEntregable = Convert.ToInt32(a.Attributes.Item("data-identity").ToString())
            Me.identity.Text = idEntregable
            Dim oEntregable = dbEntities.tme_contrato_entregables.Find(idEntregable)

            Me.txt_nombre_entregble.Text = oEntregable.nombre_entregable
            Me.dt_fecha_entrega.SelectedDate = oEntregable.fecha_esperada_entrega
            Me.txt_valor_entregable.Value = oEntregable.valor_entregable
            Me.txt_numero_entregable.Value = oEntregable.numero_entregable
            Me.txt_productos.Text = oEntregable.productos
        End Using
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/contratistas/frm_contratos"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub

    Private Sub btn_guardar2_Click(sender As Object, e As EventArgs) Handles btn_guardar2.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim id_modificacion = Convert.ToInt32(Me.idModificacion.Value)
            Dim modificacion = dbEntities.tme_contrato_modificaciones.Find(id_modificacion)


            modificacion.modificacion_finalizada = True
            dbEntities.Entry(modificacion).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/contratistas/frm_contratos"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using

    End Sub

    Private Sub btn_salir_Click(sender As Object, e As EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/contratistas/frm_contratos")
    End Sub
End Class