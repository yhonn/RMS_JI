Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI

Partial Class frm_consultadoc_support
    Inherits System.Web.UI.Page

    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADMIN_TYPE_DOC"
    Dim controles As New ly_SIME.CORE.cls_controles

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login.aspx")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            'Dim Sql = "SELECT nombre_proyecto FROM vw_proyectos WHERE id_proyecto=" & Me.Session("E_IdProy")
            'Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds1 As New DataSet("proyecto")
            'dm1.Fill(ds1, "proyecto")
            'Me.lbl_proyecto.Text = ds1.Tables("proyecto").Rows(0).Item(0).ToString
            'If Me.Session("E_IdPerfil") = 5 Or Me.Session("E_IdPerfil") = 1 Then
            Me.grd_cate.Columns(2).Visible = True
            Me.btn_nuevo.Visible = True
            'Else
            '    Me.grd_cate.Columns(0).Visible = False
            '    Me.btn_nuevo.Visible = False
            'End If

        End If


    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        Dim sql = "SELECT id_doc_soporte, nombre_documento, extension, Template, environmental, deliverable, max_size FROM ta_docs_soporte WHERE id_programa=" & Me.Session("E_IDPrograma") & " AND ((nombre_documento LIKE '%" & Me.txt_doc.Text & "%') OR (extension LIKE '%" & Me.txt_doc.Text & "%'))"
        Me.SqlDataSource2.SelectCommand = sql
        Me.grd_cate.DataBind()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim editar As New ImageButton
            Dim hlk_ref As New HyperLink

            Dim chkENVIR As New CheckBox
            Dim chkDELIV As New CheckBox

            'editar = CType(e.Item.FindControl("editar"), ImageButton)
            editar = itemD("colm_edit").FindControl("editar")
            editar.PostBackUrl = "~/Approvals/frm_doc_support_edit.aspx?IdType=" & itemD("id_doc_soporte").Text 'e.Item.Cells(3).Text.ToString

            hlk_ref = itemD("colm_template").FindControl("hlk_template")

            If Not itemD("Template").Text.Contains("--none--") Then
                hlk_ref.Text = itemD("Template").Text
                hlk_ref.NavigateUrl = "~/FileUploads/Templates/" & itemD("Template").Text
            Else
                hlk_ref.Text = itemD("Template").Text
                hlk_ref.NavigateUrl = "#"
            End If


            chkENVIR = itemD("colm_env").FindControl("chkENVIR")

            If CType(itemD("environmental").Text, Boolean) Then
                chkENVIR.Checked = True
            Else
                chkENVIR.Checked = False
            End If


            chkDELIV = itemD("colm_deliv").FindControl("chkDELIV")

            If CType(itemD("deliverable").Text, Boolean) Then
                chkDELIV.Checked = True
            Else
                chkDELIV.Checked = False
            End If


        End If
    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/Approvals/frm_doc_support.aspx")
    End Sub


End Class
