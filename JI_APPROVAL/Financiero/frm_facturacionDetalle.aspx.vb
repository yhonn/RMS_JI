Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports ly_SIME

Public Class frm_facturacionDetalle
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_FACT_PRINT"
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
            'Me.idFactura.Value = id_Factura
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
            Me.lbl_dia.Text = factura.fecha_factura.Day
            Me.lbl_mes.Text = factura.fecha_factura.Month
            Me.lbl_anio.Text = factura.fecha_factura.Year
            Me.resolucion.Text = factura.resolucion
            'Me.lbl_valor_total.Text = factura.tme_facturacion_productos.Sum(Function(p) p.valor).ToString("c2", cl_user.regionalizacionCulture)
            Me.lbl_nombre.Text = factura.nombre
            Me.lbl_direccion.Text = factura.direccion
            Me.lbl_departamento.Text = factura.t_municipios.t_departamentos.nombre_departamento & " - " & factura.t_municipios.nombre_municipio
            'Me.lbl_municipio.Text = factura.t_municipios.nombre_municipio
            ''Me.lbl_ciudad.Text = factura.ciudad
            Me.lbl_numero_documento.Text = factura.documento_identificacion
            Me.lbl_telefono.Text = factura.telefono
            Me.lbl_celular.Text = factura.celular
            Me.lbl_codigo_postal.Text = factura.codigo_postal
            Me.lbl_correo.Text = factura.correo
            Me.lbl_numero_factura.Text = factura.numero_factura

            'If factura.anulada IsNot Nothing And factura.anulada = True Then
            '    Me.lbl_factura_anulada.Text = "SI"
            '    Me.lbl_fecha_anulacion.Text = Format(factura.fecha_anulacion, "dd/mm/yyyy")
            '    Me.lbl_motivo_anulacion.Text = factura.motivoanulacion
            'Else
            '    Me.lbl_factura_anulada.Text = "NO"
            '    'Me.factura_anulada_visible.Visible = False
            '    Me.factura_anulada_visible2.Visible = False
            '    Me.factura_anulada_visible3.Visible = False
            'End If


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

    Sub fillGrid()
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If
        Me.grd_conceptos.DataSource = dtConceptos
        Me.grd_conceptos.DataBind()
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