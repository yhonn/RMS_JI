Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL

Partial Class frm_aprobaciones
    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_APPROVAL_ADMIN"
    Dim controles As New ly_SIME.CORE.cls_controles


    Sub ActualizaDatos()
        Dim row As GridItem
        Dim visibleSI As String = "0"
        Dim visibleNO As String = "0"

        For Each rowD As GridDataItem In Me.grd_cate.Items

            Dim chkvisible As CheckBox = CType(rowD("colm_visible").FindControl("chkVisible"), CheckBox) 'CType(row.Cells(0).FindControl("chkVisible"), CheckBox)
            If chkvisible.Checked = True Then
                visibleSI &= "," & rowD("id_tipoDocumento").Text 'row.Cells.Item(4).Text
            Else
                visibleNO &= "," & rowD("id_tipoDocumento").Text 'row.Cells.Item(4).Text
            End If
        Next

        cnnSAP.Open()
        Dim dm As New SqlCommand("UPDATE ta_tipoDocumento SET visible='SI' WHERE id_tipoDocumento IN(" & visibleSI & ")", cnnSAP)
        dm.ExecuteNonQuery()
        dm.CommandText = "UPDATE ta_tipoDocumento SET visible='NO' WHERE id_tipoDocumento IN(" & visibleNO & ")"
        dm.ExecuteNonQuery()
        cnnSAP.Close()
        Me.grd_cate.DataBind()

        cnnSAP.Close()
    End Sub
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
            Me.btn_nuevo.Visible = True
            'Else
            '    Me.grd_cate.Columns(0).Visible = False
            '    Me.grd_cate.Columns(4).Visible = False
            '    Me.btn_nuevo.Visible = False
            'End If

        End If


    End Sub


    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        Dim sql = "SELECT id_tipoDocumento, id_categoria, descripcion_aprobacion, condicion, nivel_aprobacion, email_notificacion, descripcion_cat, visible FROM vw_aprobaciones WHERE id_programa=" & Me.Session("E_IDPrograma") & " AND ((descripcion_aprobacion LIKE '%" & Me.txt_doc.Text & "%') OR (descripcion_cat LIKE '%" & Me.txt_doc.Text & "%') OR (condicion LIKE '%" & Me.txt_doc.Text & "%'))"
        Me.SqlDataSource2.SelectCommand = sql
        Me.grd_cate.DataBind()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            Dim editar As New ImageButton
            Dim ruta As New ImageButton
            Dim visible As New CheckBox

            editar = CType(itemD("colm_edit").FindControl("editar"), ImageButton) ' CType(e.Item.FindControl("editar"), ImageButton)
            editar.PostBackUrl = "frm_aprobaciones_edit.aspx?IdType=" & itemD("id_tipoDocumento").Text 'e.Item.Cells(4).Text.ToString
            visible = CType(itemD("colm_visible").FindControl("chkVisible"), CheckBox) ' CType(e.Item.FindControl("chkVisible"), CheckBox)

            Dim Sql = "SELECT ruta_completa FROM ta_tipoDocumento WHERE id_tipoDocumento=" & itemD("id_tipoDocumento").Text 'e.Item.Cells(4).Text.ToString
            Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds1 As New DataSet("ruta")
            dm1.Fill(ds1, "ruta")

            ruta = CType(itemD("colm_path").FindControl("ruta"), ImageButton) 'CType(e.Item.FindControl("ruta"), ImageButton)

            If ds1.Tables("ruta").Rows(0).Item(0).ToString = "SI" Then
                ruta.ImageUrl = "~/Imagenes/iconos/accept.png"
            Else
                ruta.ImageUrl = "~/Imagenes/iconos/alerta.png"
            End If
            ruta.PostBackUrl = "frm_aprobaciones_ruta.aspx?IdType=" & itemD("id_tipoDocumento").Text 'e.Item.Cells(4).Text.ToString
            'Dim J = e.Item.Cells(9).Text.ToString

            If itemD("visibleBound").Text = "SI" Then 'e.Item.Cells(9).Text.ToString = "SI" Then
                visible.ToolTip = "Visible"
                visible.Checked = True
            Else
                visible.Checked = False
                visible.ToolTip = "Hidden"
            End If

            ''If Me.Session("E_IdPerfil") <> 5 And Me.Session("E_IdPerfil") <> 1 Then
            'visible.Enabled = False
            'End If
        End If
    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/Approvals/frm_aprobacionesAD.aspx")
    End Sub

    Protected Sub chkVisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ActualizaDatos()
    End Sub

    'Private Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click

    '    'Dim objEmail As New APPROVAL.cls_notification(2, 10, 5)
    '    'objEmail.Emailing_APPROVAL_STEP(37)

    '    Dim objEmail = New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), 10, 6)
    '    objEmail.Emailing_COMMENT_APPROVAL("Este es mi comentario de propuesta........", 37)

    'End Sub

End Class
