Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Globalization
Imports System.Data.Entity.Infrastructure
Imports ly_SIME


Public Class frm_regiones
    Inherits System.Web.UI.Page
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "AP_ADMIN_REGI"
    Dim db As New dbRMS_HNEntities
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clListados As New ly_SIME.CORE.cls_listados

    Sub fillGrid()
        Me.grd_cate.DataBind()
        Me.grd_cate.DataSourceID = ""
        Me.grd_cate.DataSource = db.t_regiones.Where(Function(p) p.nombre_region.Contains(Me.txt_doc.Text) And p.id_programa = Me.cmb_programa.SelectedValue).ToList()
        'Me.SqlDataSource2.SelectCommand = "SELECT * FROM t_programas WHERE nombre_programa LIKE '%" & Me.txt_doc.Text.Trim.Replace("'", "") & "%'"
        Me.grd_cate.DataBind()

        Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                cl_user.chk_Rights(Page.Controls, 8, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Me.cmb_programa.DataSource = clListados.get_t_programas()
            Me.cmb_programa.DataTextField = "nombre_programa"
            Me.cmb_programa.DataValueField = "id_programa"
            Me.cmb_programa.DataBind()
            Me.cmb_programa.SelectedValue = idPrograma
            Me.cmb_programa.Enabled = False
            fillGrid()
        End If

        If cl_user.codigo_nivel_usuario = "SYS_ADMIN" Then
            Me.cmb_programa.Enabled = True
        End If

    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("hlk_Edit"), HyperLink)
            hlnkEdit.NavigateUrl = "frm_regionesEdit?Id=" & DataBinder.Eval(e.Item.DataItem, "id_region").ToString()

            Dim hlnkSubRegiones As HyperLink = New HyperLink
            hlnkSubRegiones = CType(e.Item.FindControl("hlk_SubRegiones"), HyperLink)
            hlnkSubRegiones.NavigateUrl = "frm_subregiones?Id=" & DataBinder.Eval(e.Item.DataItem, "id_region").ToString()

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("lnk_eliminar"), LinkButton)
            'hlnkDelete.Text = DataBinder.Eval(e.Item.DataItem, "id_region").ToString()
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_region").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_region").ToString())

            'hlnkDelete.NavigateUrl = "frm_regionesEdit.aspx?Id=" & DataBinder.Eval(e.Item.DataItem, "id_region").ToString()

        End If
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid()
    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/proyectos/frm_regionesAD")
    End Sub
    Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand
        Dim id_region = Convert.ToInt32(TryCast(e.Item, GridDataItem).GetDataKeyValue("id_region").ToString())

        Using dbRMS As New dbRMS_HNEntities
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_regiones"
            Try
                Dim oregion = dbRMS.t_regiones.Find(id_region)
                dbRMS.t_regiones.Remove(oregion)
                dbRMS.SaveChanges()
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Catch ex As DbUpdateException
                Me.MsgGuardar.NuevoMensaje = "Existen registros asociados a la región."
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using db As New ly_SIME.dbRMS_HNEntities
            Try
                db.Database.ExecuteSqlCommand("DELETE FROM t_regiones WHERE id_region = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    Protected Sub Eliminar_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub grd_cate_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource
        Me.grd_cate.DataSource = db.t_regiones.Where(Function(p) p.nombre_region.Contains(Me.txt_doc.Text)).ToList()
    End Sub

    Protected Sub cmb_programa_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_programa.SelectedIndexChanged
        fillGrid()
    End Sub
End Class