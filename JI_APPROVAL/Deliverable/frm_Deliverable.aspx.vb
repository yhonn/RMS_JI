Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports System.Globalization
Imports ly_APPROVAL
Imports ly_RMS
Imports ly_SIME

Partial Class frm_Deliverable
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_DELIVERABLE"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cls_TimeSheet As APPROVAL.clss_TimeSheet
    Dim cls_Deliverable As APPROVAL.clss_Deliverable
    Dim clss_approval As APPROVAL.clss_approval
    Dim booEXEC As Boolean = False

    Sub ActualizaDatos()
        Dim row As GridItem
        Dim visibleSI As String = "0"
        Dim visibleNO As String = "0"

        For Each rowD As GridDataItem In Me.grd_cate.Items

            Dim chkvisible As CheckBox = CType(rowD("colm_visible").FindControl("chkVisible"), CheckBox)
            If chkvisible.Checked = True Then
                visibleSI &= "," & rowD("id_tipoDocumento").Text
            Else
                visibleNO &= "," & rowD("id_tipoDocumento").Text
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


            Dim tbl_user_role As New DataTable
            Dim strRoles As String = ""
            '***********************Group Roles***********************************
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
            tbl_user_role = clss_approval.get_RolesUser(Me.Session("E_IdUser"), 2)
            strRoles = ""
            For Each dtRow In tbl_user_role.Rows
                strRoles &= dtRow("id_rol").ToString & ","
            Next
            If strRoles.Length > 0 Then
                strRoles = strRoles.Substring(0, strRoles.Length - 1)
            End If
            lbl_GroupRolID.Text = IIf(strRoles.Length = 0, "0", strRoles)
            '*********************** Roles Roles***********************************

            LoadGrid()
            'Me.btn_nuevo.Visible = True

        End If


    End Sub


    Public Sub LoadGrid()

        ' Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Convert.ToInt32(Me.Session("E_IDprograma")))
        cls_Deliverable = New APPROVAL.clss_Deliverable(Convert.ToInt32(Me.Session("E_IDprograma")))

        Dim strUsers As String = lbl_GroupRolID.Text.Trim
        Dim ArrayUsers As String() = strUsers.Split(New Char() {","c})
        Dim ArrayINT_Users As Integer() = Array.ConvertAll(ArrayUsers, Function(str) Int32.Parse(str))


        Dim tbl_user As DataTable = cls_Deliverable.get_t_usuarios_Implementer(Me.Session("E_IdUser"))
        Dim tbl_Deliverables As DataTable

        tbl_Deliverables = cls_Deliverable.get_Deliverables_Implementer(tbl_user.Rows.Item(0).Item("id_ejecutor"))

        'Me.grd_cate.DataSource = cls_TimeSheet.getTimeSheet(Convert.ToInt32(Me.Session("E_IDuser")), ArrayINT_Users)

        Me.grd_cate.DataSource = tbl_Deliverables
        Me.grd_cate.DataBind()


    End Sub

    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        'Dim sql = "SELECT id_tipoDocumento, id_categoria, descripcion_aprobacion, condicion, nivel_aprobacion, email_notificacion, descripcion_cat, visible FROM vw_aprobaciones WHERE id_programa=" & Me.Session("E_IDPrograma") & " AND ((descripcion_aprobacion LIKE '%" & Me.txt_doc.Text & "%') OR (descripcion_cat LIKE '%" & Me.txt_doc.Text & "%') OR (condicion LIKE '%" & Me.txt_doc.Text & "%'))"
        'Me.SqlDataSource2.SelectCommand = sql
        'Me.grd_cate.DataBind()
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound

        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
            Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
            item.DataCell.Text = ""
        End If

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim editar As New ImageButton
            Dim Preview_ As New HyperLink
            Dim Minute_ As New HyperLink

            editar = CType(itemD("colm_edit").FindControl("editar"), ImageButton)
            editar.PostBackUrl = "frm_DeliverableAdd.aspx?ID=" & itemD("id_deliverable").Text

            Preview_ = CType(itemD("colm_open").FindControl("hlk_deliverable"), HyperLink)
            Preview_.NavigateUrl = "frm_DeliverableFollowingREP.aspx?ID=" & itemD("id_deliverable").Text

            Minute_ = CType(itemD("colm_minute").FindControl("hlk_minute"), HyperLink)

            If Convert.ToInt32(itemD("id_deliverable_Estado").Text) = 3 Then

                Minute_.Visible = True
                If Convert.ToInt32(itemD("id_deliverable_minute").Text) > 0 Then

                    If Convert.ToBoolean(itemD("minute_close").Text) Then

                        Using dbEntities As New dbRMS_JIEntities

                            Dim idFichaProyecto = Convert.ToInt32(itemD("id_ficha_proyecto").Text)
                            Dim oFichaProyecto = dbEntities.vw_tme_ficha_proyecto.Where(Function(p) p.id_ficha_proyecto = idFichaProyecto).FirstOrDefault()

                            If oFichaProyecto.id_mecanismo_contratacion = 3 Then
                                Minute_.NavigateUrl = "frm_Deliverable_minutePrintG.aspx?ID=" & itemD("id_deliverable").Text
                            Else
                                Minute_.NavigateUrl = "frm_Deliverable_minutePrint.aspx?ID=" & itemD("id_deliverable").Text
                            End If


                            Minute_.ImageUrl = "~/Imagenes/iconos/document-hf-delete-footer.png"
                            Minute_.ToolTip = "Ver Acta"
                            Minute_.Target = "_blank"

                        End Using

                    Else
                        Minute_.NavigateUrl = "frm_Deliverable_minuteAdd.aspx?ID=" & itemD("id_deliverable").Text
                        Minute_.ImageUrl = "~/Imagenes/iconos/alacarte.png"
                        Minute_.ToolTip = "Editar Acta"
                    End If

                Else
                    Minute_.NavigateUrl = "frm_Deliverable_minuteAdd.aspx?ID=" & itemD("id_deliverable").Text
                End If

            Else
                Minute_.Visible = False
            End If

        End If

    End Sub

    Protected Sub btn_nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_nuevo.Click
        Me.Response.Redirect("~/Deliverable/frm_DeliverableAdd.aspx")
    End Sub

    Private Sub Buttom_testing_Click(sender As Object, e As EventArgs) Handles Buttom_testing.Click

        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), 8913, 1009, cl_user.regionalizacionCulture, 13843)
        If (objEmail.Emailing_DELIVERABLE_APPROVAL(13843)) Then
        Else 'Error mandando Email
        End If

    End Sub
End Class
