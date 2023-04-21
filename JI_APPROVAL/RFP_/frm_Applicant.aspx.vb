Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_SIME

Partial Class frm_Applicant
    Inherits System.Web.UI.Page
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As New ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADMIN_APPLICANTS"
    Dim controles As New ly_SIME.CORE.cls_controles

    Sub fillGrid()

        Dim db As New dbRMS_JIEntities
        Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
        Me.grd_cate.DataSourceID = ""
        Me.grd_cate.DataSource = db.VW_TA_ORGANIZATION_APP.Where(Function(p) p.ID_PROGRAMA = id_programa And (p.ORGANIZATIONNAME.Contains(Me.txt_doc.Text) Or p.NAMEALIAS.Contains(Me.txt_doc.Text))).ToList()
        '' db.t_ejecutores.Where(Function(p) p.nombre_ejecutor.Contains(Me.txt_doc.Text) And p.id_programa = id_programa).ToList()
        Me.grd_cate.DataBind()
        'Me.lbltotal.Text = "Total de registros: [ " & Me.grd_cate.Items.Count & " ]"

    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/RFP_/frm_ApplicantAD")
    End Sub

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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            fillGrid()
        End If


    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnkEdit As HyperLink = New HyperLink
            hlnkEdit = CType(e.Item.FindControl("col_hlk_edit"), HyperLink)
            hlnkEdit.NavigateUrl = "frm_ApplicantAD.aspx?PPT=" & DataBinder.Eval(e.Item.DataItem, "ID_ORGANIZATION_APP").ToString()
            hlnkEdit.ToolTip = controles.iconosGrid("col_hlk_edit")

            'Dim hlnkPrint As HyperLink = New HyperLink
            'hlnkPrint = CType(e.Item.FindControl("col_hlk_Print"), HyperLink)
            'hlnkPrint.NavigateUrl = "frm_EjecutorPrint.aspx?IdEjecME=" & DataBinder.Eval(e.Item.DataItem, "id_ejecutor").ToString()
            ''hlnkPrint.ToolTip = controles.iconosGrid("col_hlk_Print")

            Dim hlnkActivo As New CheckBox
            hlnkActivo = CType(e.Item.FindControl("chkActivo"), CheckBox)

            If DataBinder.Eval(e.Item.DataItem, "ORGANIZATIONSTATUS").ToString() = "True" Then
                hlnkActivo.ToolTip = controles.iconosGrid("col_hlk_activo")
                hlnkActivo.Checked = True
            Else
                hlnkActivo.Checked = False
                hlnkActivo.ToolTip = controles.iconosGrid("col_hlk_inactivo")
            End If

            'Dim hlnkDelete As LinkButton = New LinkButton
            'hlnkDelete = CType(e.Item.FindControl("col_hlk_delete"), LinkButton)
            'hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_ejecutor").ToString())
            'hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_ejecutor").ToString())
            ''hlnkDelete.ToolTip = controles.iconosGrid("col_hlk_delete")

        End If


    End Sub

    Protected Sub chkVisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim row As GridItem
        Dim sql = ""
        Dim Activo As String = ""
        cnnME.Open()
        For Each row In Me.grd_cate.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim idEjecutor As Integer = Convert.ToInt32(dataItem.GetDataKeyValue("ID_ORGANIZATION_APP"))
                Dim chkvisible As CheckBox = CType(row.Cells(0).FindControl("chkActivo"), CheckBox)
                Activo = 1
                If chkvisible.Checked = False Then
                    Activo = 0
                End If
                sql = "UPDATE TA_ORGANIZATION_APP SET ORGANIZATIONSTATUS=" & Activo & " WHERE ID_ORGANIZATION_APP=" & idEjecutor
                Dim dm As New SqlDataAdapter(sql, cnnME)
                Dim ds As New DataSet("IdActivo")
                dm.Fill(ds, "IdActvo")
            End If
        Next
        fillGrid()
        cnnME.Close()
    End Sub

    Protected Sub grd_cate_PageIndexChanged(sender As Object, e As GridPageChangedEventArgs) Handles grd_cate.PageIndexChanged
        fillGrid()
    End Sub

    Protected Sub grd_cate_PageSizeChanged(sender As Object, e As GridPageSizeChangedEventArgs) Handles grd_cate.PageSizeChanged
        fillGrid()
    End Sub
    Protected Sub btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Try
                Dim Sql = "delete from t_ejecutores WHERE id_ejecutor = " & Me.identity.Text
                dbEntities.Database.ExecuteSqlCommand(Sql)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_Ejecutor"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    Protected Sub Elimina_Elemento(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

End Class
