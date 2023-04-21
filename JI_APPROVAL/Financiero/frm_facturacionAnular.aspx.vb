Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME

Public Class frm_facturacionAnular
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_FACT_ANULAR"
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
            Session.Remove("dtConceptos")
            Dim id_Factura = Convert.ToInt32(Me.Request.QueryString("id"))
            Me.idFactura.Value = id_Factura
            LoadData(id_Factura)
            fillGrid()
        End If

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
            Me.txt_ciudad.Text = factura.ciudad
            Me.txt_numero_identificacion.Value = factura.documento_identificacion
            Me.txt_telefono_.Text = factura.telefono
            Me.txt_celular_.Text = factura.celular
            Me.txt_correo.Text = factura.correo
            Me.txt_cantidad_productos.Value = factura.tme_facturacion_productos.Count()
            For Each item In factura.tme_facturacion_productos.ToList()
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio & item.id_facturacion_producto

                dtConceptos.Rows.Add(idunique, item.cantidad, item.descripcion, item.valor, item.valor / item.cantidad, True, item.id_facturacion_producto)
            Next
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

        Using dbEntities As New dbRMS_JIEntities
            Dim id_factura = Convert.ToInt32(Me.idFactura.Value)
            Dim oFactura = dbEntities.tme_facturacion.Find(id_factura)
            Dim anularFactura = Me.rbn_anular_factura.SelectedValue

            If anularFactura = 1 Then
                oFactura.fecha_anulacion = Date.Now
                oFactura.id_usuario_anula = Me.Session("E_IdUser").ToString()
                oFactura.anulada = True
                oFactura.motivoanulacion = Me.txt_motivo_anular.Text
                dbEntities.Entry(oFactura).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
            End If



            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/financiero/frm_facturacion"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
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
End Class