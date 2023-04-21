Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Public Class frm_admincatalogo
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADMIN_CATA"
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim db As New dbRMS_JIEntities
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles

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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_catalogos)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If
        If Not IsPostBack Then
            fillGrid()
            LoadCatalogo()
        End If
        Me.hlk_export.Target = "_blank"
        Me.hlk_export.NavigateUrl = "~/Administracion/ExportExcelListaCatalogos?Id=" & Me.cmb_catalogo.SelectedValue
    End Sub

    Protected Sub cmb_catalogo_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_catalogo.SelectedIndexChanged
        LoadCatalogo()
    End Sub

    Protected Sub grd_catalogos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_catalogos.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnkEdit As ImageButton = CType(e.Item.FindControl("col_hlk_edit"), ImageButton)
            hlnkEdit.PostBackUrl = "~/Administracion/frm_AdminCatalogosEdit?IdCat=" & Me.cmb_catalogo.SelectedValue & "&ID=" & DataBinder.Eval(e.Item.DataItem, "ID").ToString() _
                & "&IdTipo=" & DataBinder.Eval(e.Item.DataItem, "tipo").ToString()

            'hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")
        End If
    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/Administracion/frm_AdminCatalogosAD")
    End Sub

    Protected Sub btn_asoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_asoc.Click
        Me.Response.Redirect("~/Administracion/frm_AdminCatalogosAsoc")
    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Sub fillGrid()

        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Me.cmb_catalogo.DataSource = ""
        Me.cmb_catalogo.DataSource = clListados.get_t_admincatalogos(id_programa)
        Me.cmb_catalogo.DataValueField = "id_CatalogoMaster"
        Me.cmb_catalogo.DataTextField = "TipoCatalogo"
        Me.cmb_catalogo.DataBind()

    End Sub

    Sub LoadCatalogo()
        Using dbEntites As New dbRMS_JIEntities
            Dim tipo = ""
            Dim oCatalogo = db.t_AdminCatalogos.FirstOrDefault(Function(p) p.id_CatalogoMaster = Me.cmb_catalogo.SelectedValue)
            Dim strTabla = oCatalogo.NombreTabla
            Dim campoID = oCatalogo.CampoID
            Dim campoTipo = oCatalogo.CampoTipo

            If campoTipo = "-1" Then

            Else

            End If

            Dim campoNombre = oCatalogo.CampoDescripcion
            Dim sql As String = ""
            Dim nombreCampo = ""
            Dim join = ""

            If oCatalogo.id_tabla_padre.HasValue Then
                Dim tablaPadre = oCatalogo.t_AdminCatalogos2.NombreTabla
                nombreCampo = ", " & tablaPadre & "." & oCatalogo.t_AdminCatalogos2.CampoDescripcion & " AS CampoPadre "
                join = " INNER JOIN " & tablaPadre & " ON " & strTabla & "." & oCatalogo.CampoTipo & "=" & tablaPadre & "." & oCatalogo.CampoTipo
                tipo = ", " & tablaPadre & "." & campoTipo & " AS tipo "
                grd_catalogos.MasterTableView.GetColumn("colm_CampoPadre").Visible = True
            Else
                nombreCampo = ", '' as CampoPadre "
                tipo = ", -1 as tipo"
                grd_catalogos.MasterTableView.GetColumn("colm_CampoPadre").Visible = False
            End If

            sql = " SELECT " & campoID & " AS id, " & campoNombre & " AS nombre " & tipo & nombreCampo & " FROM " & strTabla & join & " WHERE " & strTabla & ".id_programa = " & Me.Session("E_IDPrograma")

            Me.SqlDataSource3.SelectCommand = sql
        End Using
        Me.grd_catalogos.DataBind()
    End Sub

End Class