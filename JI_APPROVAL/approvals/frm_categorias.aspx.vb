Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Globalization

Partial Class frm_categorias
    Inherits System.Web.UI.Page

    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ADMIN_TYPE_APP"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_Categories As APPROVAL.cl_categorie

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
            'Me.grd_cate.Columns(0).Visible = True
            'Me.btn_save.Visible = True
            'Else
            '    Me.grd_cate.Columns(0).Visible = False
            '    Me.btn_save.Visible = False
            'End If

        End If


    End Sub

    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        'Dim Sql = " INSERT INTO ta_categoria (descripcion_cat, id_programa) VALUES ('" & Me.txt_cat.Text & "', " & Me.Session("E_IDPrograma") & ") "
        'cnnSAP.Open()
        'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
        'Dim ds As New DataSet("IdPlan")
        'dm.Fill(ds, "IdPlan")
        'cnnSAP.Close()

        cl_Categories = New APPROVAL.cl_categorie(Me.Session("E_IDPrograma"))
        cl_Categories.set_ta_categoria(0) 'New

        cl_Categories.set_ta_categoriaFIELDS("descripcion_cat", Me.txt_cat.Text, "id_categoria", 0)
        cl_Categories.set_ta_categoriaFIELDS("id_programa", Me.Session("E_IDPrograma"), "id_categoria", 0)

        If cl_Categories.save_ta_categoria() <> -1 Then 'Save the categorie
            Me.grd_cate.DataBind()
            Me.txt_cat.Text = ""
        Else 'Error
            lbl_Error.Visible = True
        End If


    End Sub

    Protected Sub chkVisible_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        ActualizaDatos()

    End Sub

    Sub ActualizaDatos()

        Dim visibleSI As String = "0"
        Dim visibleNO As String = "0"

        For Each Irow As GridDataItem In Me.grd_cate.Items

            'Dim chkvisible As CheckBox = CType(row.Cells(0).FindControl("chkVisible"), CheckBox)
            Dim chkvisible As CheckBox = CType(Irow("colm_visible").FindControl("chkVisible"), CheckBox)

            If chkvisible.Checked = True Then
                visibleSI &= "," & Irow("id_categoria").Text
            Else
                visibleNO &= "," & Irow("id_categoria").Text 'row.Cells.Item(3).Text
            End If
        Next

        cnnSAP.Open()
        Dim dm As New SqlCommand("UPDATE ta_categoria SET visible='SI' WHERE id_categoria IN(" & visibleSI & ")", cnnSAP)
        dm.ExecuteNonQuery()
        dm.CommandText = "UPDATE ta_categoria SET visible='NO' WHERE id_categoria IN(" & visibleNO & ")"
        dm.ExecuteNonQuery()
        cnnSAP.Close()

        Me.grd_cate.DataBind()

    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim visible As New CheckBox
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            'visible = CType(e.Item.FindControl("chkVisible"), CheckBox)
            visible = itemD("colm_visible").FindControl("chkVisible")
            'If e.Item.Cells(6).Text.ToString = "SI" Then
            If itemD("Cvisible").Text = "SI" Then
                visible.ToolTip = "Visible"
                visible.Checked = True
            Else
                visible.Checked = False
                visible.ToolTip = "Hidden"
            End If
            'If Me.Session("E_IdPerfil") <> 5 And Me.Session("E_IdPerfil") <> 1 Then
            ' visible.Enabled = False
            'End If
        End If
    End Sub

    Private Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click

        '*********************************APPROVED NEXT STEP****************************************

        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), 2216, 5, cl_user.regionalizacionCulture, 2683)
        'If (objEmail.Emailing_APPROVAL_STEP(2683)) Then
        'Else 'Error mandando Email
        'End If

        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), 2216, 6, cl_user.regionalizacionCulture, 2683)
        'If (objEmail.Emailing_COMMENT_APPROVAL("this is just for testing purposes...", 2683)) Then
        'Else 'Error mandando Email
        'End If


        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), 2216, 7, cl_user.regionalizacionCulture, 2683)
        'If (objEmail.Emailing_USER_UPD(1, "TUMBLINBLI")) Then
        'Else 'Error mandando Email
        'End If
        '*********************************APPROVED NEXT STEP****************************************



        '*********************************APPROVED****************************************
        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), 283, 1009, cl_user.regionalizacionCulture, 1051)
        'If (objEmail.Emailing_DELIVERABLE_APPROVAL(1051)) Then
        'Else 'Error mandando Email
        'End If
        '*********************************APPROVED****************************************

        ''*********************************SENT EMAIL OF PRESCREENING****************************************
        'Dim idPrograma As Integer = CType(Me.Session("E_IDPrograma"), Int32)
        'Dim Id_solicitation_app As Integer = 1042
        ''**********************************SCREENNNIG SENT*******************************************************
        'Dim regionalizacionCulture As CultureInfo = New CultureInfo(cl_user.regionalizacion)
        'Dim cl_Noti_Process As New ly_APPROVAL.APPROVAL.notification_proccess(idPrograma, 1015, regionalizacionCulture)
        ''**********************************SCREENNNIG SENT*******************************************************
        'cl_Noti_Process.NOTIFIYING_SOLICITATION_SCREENING(Id_solicitation_app)


        ''*********************************SENT DELIVERABLE EMAIL OF PRESCREENING****************************************
        Dim id_documento As Integer = 327
        Dim id_appdocumento As Integer = 1308


        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), id_documento, 1009, cl_user.regionalizacionCulture, id_appdocumento)
        If (objEmail.Emailing_DELIVERABLE_APPROVAL(id_appdocumento)) Then
        Else 'Error mandando Email
        End If


    End Sub
End Class
