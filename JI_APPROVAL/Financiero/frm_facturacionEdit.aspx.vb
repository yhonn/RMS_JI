Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME

Public Class frm_facturacionEdit
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_FACT_EDIT"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtConceptos As New DataTable
    Dim ListItemsDeleteBD As New List(Of Integer)

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
                'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            Dim id_Factura = Convert.ToInt32(Me.Request.QueryString("id"))
            Me.idFactura.Value = id_Factura
            Session.Remove("dtConceptos")
            Session.Remove("ListItemsDeleteBD")
            LoadLista()
            LoadData(id_Factura)
            fillGrid()
        End If

    End Sub

    Sub LoadLista()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Dim departamentos = dbEntities.t_departamentos.Where(Function(p) p.id_programa = id).ToList()
            Me.cmb_departamento.DataSourceID = ""
            Me.cmb_departamento.DataSource = departamentos
            Me.cmb_departamento.DataTextField = "nombre_departamento"
            Me.cmb_departamento.DataValueField = "id_departamento"
            Me.cmb_departamento.DataBind()
        End Using
    End Sub
    Sub loadMunicipios()
        Using dbEntities As New dbRMS_JIEntities
            Dim idDepto = Convert.ToInt32(Me.cmb_departamento.SelectedValue)
            Dim municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = idDepto).ToList()

            Me.cmb_municipio.DataSourceID = ""
            Me.cmb_municipio.DataSource = municipios
            Me.cmb_municipio.DataTextField = "nombre_municipio"
            Me.cmb_municipio.DataValueField = "id_municipio"
            Me.cmb_municipio.DataBind()
        End Using
    End Sub
    Sub LoadData(ByVal id_Factura As Integer)
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        Using dbEntities As New dbRMS_JIEntities
            Dim factura = dbEntities.tme_facturacion.Find(id_Factura)
            Me.dt_fecha_factura.SelectedDate = factura.fecha_factura
            Me.txt_nombre.Text = factura.nombre
            Me.txt_direccion.Text = factura.direccion
            Me.cmb_departamento.SelectedValue = factura.t_municipios.id_departamento
            loadMunicipios()
            Me.cmb_municipio.SelectedValue = factura.id_municipio
            Me.txt_numero_identificacion.Value = factura.documento_identificacion
            Me.txt_telefono_.Text = factura.telefono
            Me.txt_celular_.Text = factura.celular
            Me.txt_correo.Text = factura.correo
            Me.txt_cantidad_productos.Value = factura.tme_facturacion_productos.Count()
            Me.txt_codigo_postal.Text = factura.codigo_postal
            For Each item In factura.tme_facturacion_productos.ToList()
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio & item.id_facturacion_producto

                dtConceptos.Rows.Add(idunique, item.cantidad, item.descripcion, item.valor, item.valor / item.cantidad, True, item.id_facturacion_producto)
            Next


            Dim usuario = dbEntities.t_usuarios.Find(factura.id_usuario_crea)
            Dim regionesUsuario = usuario.t_usuario_subregion.ToList()
            Me.cmb_sub_Region.DataSourceID = ""
            If regionesUsuario.Count() = 1 Then
                Dim subRegion = regionesUsuario.Select(Function(p) _
                                            New With {Key .nombre_subregion = p.t_subregiones.t_regiones.nombre_region & " - " & p.t_subregiones.nombre_subregion,
                                                      Key .id_subregion = p.id_subregion}).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
            ElseIf regionesUsuario.Count() > 0 Then
                Dim subRegion = regionesUsuario.Select(Function(p) _
                                             New With {Key .nombre_subregion = p.t_subregiones.t_regiones.nombre_region & " - " & p.t_subregiones.nombre_subregion,
                                                       Key .id_subregion = p.id_subregion}).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.subRegionVisible.Visible = True
            Else
                Dim subRegion = dbEntities.t_subregiones.Where(Function(p) p.t_regiones.id_programa = ID).Select(Function(p) _
                                             New With {Key .nombre_subregion = p.t_regiones.nombre_region & " - " & p.nombre_subregion,
                                                       Key .id_subregion = p.id_subregion}).ToList()
                Me.cmb_sub_Region.DataSource = subRegion
                Me.subRegionVisible.Visible = True
            End If

            Me.cmb_sub_Region.DataTextField = "nombre_subregion"
            Me.cmb_sub_Region.DataValueField = "id_subregion"
            Me.cmb_sub_Region.DataBind()
            Me.cmb_sub_Region.SelectedValue = factura.id_subregion

            Session("dtConceptos") = dtConceptos
        End Using
    End Sub
    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/financiero/frm_facturacion")
    End Sub
    Sub fillGrid()
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If
        Me.grd_conceptos.DataSource = dtConceptos
        Me.grd_conceptos.DataBind()
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        If Session("ListItemsDeleteBD") IsNot Nothing Then
            ListItemsDeleteBD = Session("ListItemsDeleteBD")
        End If
        Using dbEntities As New dbRMS_JIEntities
            Dim id_factura = Convert.ToInt32(Me.idFactura.Value)
            Dim oFactura = dbEntities.tme_facturacion.Find(id_factura)
            oFactura.fecha_edita = Date.Now
            oFactura.id_usuario_edita = Me.Session("E_IdUser").ToString()
            oFactura.fecha_factura = Me.dt_fecha_factura.SelectedDate
            oFactura.nombre = Me.txt_nombre.Text.ToUpper()
            oFactura.direccion = Me.txt_direccion.Text.ToUpper()
            'oFactura.ciudad = Me.txt_ciudad.Text
            oFactura.id_municipio = Convert.ToInt32(Me.cmb_municipio.SelectedValue)
            oFactura.telefono = Me.txt_telefono_.Text.ToUpper()
            oFactura.documento_identificacion = Me.txt_numero_identificacion.Value
            oFactura.celular = Me.txt_celular_.Text.ToUpper()
            oFactura.correo = Me.txt_correo.Text.ToUpper()
            oFactura.codigo_postal = Me.txt_codigo_postal.Text.ToUpper()

            oFactura.id_subregion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
            Dim idSubRegion = Convert.ToInt32(Me.cmb_sub_Region.SelectedValue)
            Dim subR = dbEntities.t_subregiones.Find(idSubRegion)
            oFactura.id_region = subR.id_region

            dbEntities.Entry(oFactura).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()

            Dim cantidad As Decimal
            Dim descripcion As String
            Dim valor As Decimal
            Dim estaBD As Boolean

            For Each item In ListItemsDeleteBD
                Dim oFactura_conceptos = dbEntities.tme_facturacion_productos.Find(item)
                dbEntities.Entry(oFactura_conceptos).State = Entity.EntityState.Deleted
                dbEntities.SaveChanges()
            Next

            For Each row As DataRow In dtConceptos.Rows
                Dim oFactura_conceptos As New tme_facturacion_productos
                cantidad = row("cantidad")
                descripcion = row("descripcion")
                valor = row("valor")
                estaBD = row("esta_bd")

                If estaBD = False Then
                    oFactura_conceptos.id_facturacion = oFactura.id_facturacion
                    oFactura_conceptos.cantidad = cantidad
                    oFactura_conceptos.descripcion = descripcion
                    oFactura_conceptos.valor = valor

                    dbEntities.tme_facturacion_productos.Add(oFactura_conceptos)
                    dbEntities.SaveChanges()
                End If

            Next
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/financiero/frm_facturacion"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub
    Protected Sub btn_agregar_concepto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_agregar_concepto.Click
        Try
            If Session("dtConceptos") IsNot Nothing Then
                dtConceptos = Session("dtConceptos")
            Else
                createdtcolums()
            End If

            Dim fecha = DateTime.Now
            Dim rnd As New Random()
            Dim aleatorio As String = ""
            Dim index = 1
            aleatorio = rnd.Next(index, 9999999).ToString()
            Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio
            Dim valor_total = Me.txt_valor.Value
            Dim cantidad = Me.txt_cantidad.Value
            Dim valor_unitario = valor_total / cantidad
            dtConceptos.Rows.Add(idunique, Me.txt_cantidad.Value, Me.txt_descripcion_concepto.Text.ToUpper(), Me.txt_valor.Value, valor_unitario, False, 0)
            Session("dtConceptos") = dtConceptos
            Me.txt_cantidad.Text = String.Empty
            Me.txt_valor.Text = String.Empty
            Me.txt_descripcion_concepto.Text = String.Empty
            Me.txt_cantidad_productos.Value = dtConceptos.Rows().Count()
            fillGrid()
        Catch ex As Exception
            Dim mensaje = ex.Message
        End Try

    End Sub

    Sub createdtcolums()
        If dtConceptos.Columns.Count = 0 Then
            dtConceptos.Columns.Add("id_facturacion_producto", GetType(String))
            dtConceptos.Columns.Add("cantidad", GetType(Double))
            dtConceptos.Columns.Add("descripcion", GetType(String))
            dtConceptos.Columns.Add("valor", GetType(Double))
            dtConceptos.Columns.Add("valor_unitario", GetType(Double))
            dtConceptos.Columns.Add("esta_bd", GetType(Boolean))
            dtConceptos.Columns.Add("id_facturacion_producto_bd", GetType(Integer))
        End If
    End Sub

    Protected Sub delete_concepto(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.id_concpeto.Value = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub grd_conceptos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_conceptos.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim id_facturacion_producto = DataBinder.Eval(e.Item.DataItem, "id_facturacion_producto").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_facturacion_producto").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_facturacion_producto").ToString())
            hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_eliminar")

        End If
    End Sub

    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        If Session("ListItemsDeleteBD") IsNot Nothing Then
            ListItemsDeleteBD = Session("ListItemsDeleteBD")
        End If

        Dim idConcepto = Convert.ToString(Me.id_concpeto.Value)

        Dim foundRow As DataRow() = dtConceptos.Select("id_facturacion_producto = '" & idConcepto.ToString() & "'")
        Dim estaBD As Boolean = foundRow(0)("esta_bd")
        Dim id_factura_detalle = foundRow(0)("id_facturacion_producto_bd")

        If estaBD Then
            ListItemsDeleteBD.Add(id_factura_detalle)
        End If
        If foundRow.Count > 0 Then
            dtConceptos.Rows.Remove(foundRow(0))
        End If
        Session("dtConceptos") = dtConceptos
        Session("ListItemsDeleteBD") = ListItemsDeleteBD
        fillGrid()
        Me.txt_cantidad_productos.Value = dtConceptos.Rows().Count()
        Me.MsgGuardar.NuevoMensaje = "Registro eliminado"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub
    Private Sub cmb_departamento_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento.SelectedIndexChanged
        loadMunicipios()
    End Sub
End Class