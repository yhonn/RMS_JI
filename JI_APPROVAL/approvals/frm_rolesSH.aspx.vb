Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL


Partial Class frm_rolesSH

    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Const CONST_TYPE_SIMPLE = 1
    Const CONST_TYPE_SHARED = 2

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ROLES_SH_ADD"
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim dtRoles As New DataTable
    Dim cl_RolUser As APPROVAL.clss_RolUSER

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
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            Dim Random As New Random()
            Dim Aleatorio As Double = Random.Next(1, 999)
            Me.hd_dtRoles.Value = String.Format("dtRoles{0}_{1}", Me.Session("E_IdUser"), Aleatorio)

            'If HttpContext.Current.Session.Item("cl_RolUser") IsNot Nothing Then
            '    cl_RolUser = Session.Item("cl_RolUser")
            'Else
            cl_RolUser = New APPROVAL.clss_RolUSER(Me.Session("E_IDPrograma"))
            'End If

            Dim tbl_UserRol As New DataTable
            tbl_UserRol = cl_RolUser.setRol_USER(False)

            dtRoles = cl_RolUser.Get_User_ROLE() 'Roles

            If tbl_UserRol.Rows.Count = 0 Then

                lblt_filerequeried.Visible = "true"
            Else

                grd_cate.DataSource = dtRoles
                grd_cate.DataBind()

                cmb_usu.DataSource = tbl_UserRol
                cmb_usu.DataBind()
                cmb_usu.SelectedIndex = -1
                cmb_usu.Text = ""

                If tbl_UserRol.Rows.Count = 0 Then
                    btnlk_addEmployeed.Enabled = False
                    lblt_filerequeried.Visible = "true"
                Else
                    btnlk_addEmployeed.Enabled = True
                    lblt_filerequeried.Visible = "false"
                End If

            End If

            'HttpContext.Current.Session.Add("cl_RolUser", cl_RolUser)
            Session(Me.hd_dtRoles.Value) = dtRoles

        Else

            dtRoles = Session(Me.hd_dtRoles.Value)
            cl_RolUser = New APPROVAL.clss_RolUSER(Me.Session("E_IDPrograma"))

            'If HttpContext.Current.Session.Item("cl_RolUser") IsNot Nothing Then
            '    cl_RolUser = Session.Item("cl_RolUser")
            'End If

        End If



    End Sub

    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click
        Dim err As Boolean = False
        Dim ext As String = ""
        Dim bnd_ERROR As Boolean = False


        If dtRoles.Rows.Count > 0 Then 'SHARED ROLE TYPE

            lblt_requeired.Visible = False
            'Me.lblt_filerequeried.Text = ext.ToString

            cl_RolUser.set_ta_roles(0) 'Set a Blank Record
            cl_RolUser.set_ta_rolesFIELDS("nombre_rol", Me.txt_cat.Text.Trim, "id_rol", 0)
            cl_RolUser.set_ta_rolesFIELDS("descripcion_rol", Me.txt_des.Text.Trim, "id_rol", 0)
            cl_RolUser.set_ta_rolesFIELDS("id_programa", Me.Session("E_IDPrograma"), "id_rol", 0)
            cl_RolUser.set_ta_rolesFIELDS("id_type_role", CONST_TYPE_SHARED, "id_rol", 0)

            Dim id_rol As Integer = cl_RolUser.save_ta_roles
            Dim id_rol_user As Integer = 0

            If id_rol <> -1 Then 'Save the user role respective

                'Dim dtRow As DataRow
                'For i = 0 To cl_RolUser.CountUsers - 1

                For Each dtRow As DataRow In dtRoles.Rows

                    'dtRow = cl_RolUser.get_UserRol_ROW(i)

                    cl_RolUser.set_ta_role_user(0) 'Set a black record
                    cl_RolUser.set_ta_role_userFIELDS("id_rol", id_rol, "id_rol_user", 0)
                    cl_RolUser.set_ta_role_userFIELDS("id_usuario", dtRow("id_usuario"), "id_rol_user", 0)

                    id_rol_user = cl_RolUser.save_ta_role_user()

                    If id_rol_user <> -1 Then
                        dtRow("id_rol_user") = id_rol_user
                    Else 'Error do somenthing
                        bnd_ERROR = True
                    End If

                Next

            Else 'Error Saving
                bnd_ERROR = True
            End If

            If Not bnd_ERROR Then
                'Session.Remove("cl_RolUser") 'Remove the session
                Session.Remove(Me.hd_dtRoles.Value)
                Me.Response.Redirect("~/Approvals/frm_consulta_roles.aspx")
            Else
                lblt_requeired.Text = "Error Saving the Share Role, please contact to the system administration."
                lblt_requeired.Visible = True
            End If


        Else

            lblt_requeired.Visible = True

        End If


    End Sub

    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click

        Me.Response.Redirect("~/Approvals/frm_consulta_roles.aspx")

    End Sub


    Private Sub grd_cate_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_cate.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then


            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)
            Dim state = CType(itemD("state").FindControl("state_user"), ImageButton)

            If (itemD("estado").Text = "ACTIVE" Or itemD("estado").Text = "ACTIVO") Then
                state.ImageUrl = "~/Imagenes/iconos/drop-yes.gif"
                state.ToolTip = "ACTIVE"
            Else
                state.ToolTip = "INACTIVE"
                state.ImageUrl = "~/Imagenes/iconos/icon-warningAlert.png"
            End If

            'editar.PostBackUrl = "frm_groups_edit.aspx?IdType=" & itemD("id_grupo").Text 'e.Item.Cells(4).Text.ToString


        End If
    End Sub


    Protected Sub chkRemove_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim tbl_UserRol As New DataTable

        For Each Irow As GridDataItem In Me.grd_cate.Items

            Dim chkRemove As CheckBox = CType(Irow("remove").FindControl("chkRemove"), CheckBox)

            If chkRemove.Checked = True Then 'Remove the User of the table

                delMemeber_toRole(Irow("id_usuario").Text)

                grd_cate.DataSource = dtRoles 'cl_RolUser.Remove_User_Role(Irow("id_usuario").Text)
                Me.grd_cate.DataBind()

                tbl_UserRol = cl_RolUser.setRol_USER(True, dtRoles)
                cmb_usu.DataSource = tbl_UserRol
                cmb_usu.DataBind()
                cmb_usu.SelectedIndex = -1
                cmb_usu.Text = ""
                If tbl_UserRol.Rows.Count = 0 Then
                    btnlk_addEmployeed.Enabled = False
                    lblt_filerequeried.Visible = "true"
                Else
                    btnlk_addEmployeed.Enabled = True
                    lblt_filerequeried.Visible = "false"
                End If
            End If
        Next

    End Sub

    Public Sub addMemeber_toRole(ByVal idUs As Integer)

        Dim tblAdd As DataTable = New DataTable
        tblAdd = cl_RolUser.Add_User_ROLE_one(idUs)

        For Each drR As DataRow In tblAdd.Rows
            dtRoles.ImportRow(drR)
        Next

        Session(Me.hd_dtRoles.Value) = dtRoles

    End Sub

    Public Sub delMemeber_toRole(ByVal idUs As Integer)

        Dim tbl_tmp As DataTable = New DataTable
        tbl_tmp = dtRoles.Copy

        Dim i As Integer = 0
        For Each drR As DataRow In tbl_tmp.Rows

            If idUs = drR("id_usuario") Then
                dtRoles.Rows.Remove(dtRoles.Rows.Item(i))
            End If

            i += 1
        Next

        Session(Me.hd_dtRoles.Value) = dtRoles

    End Sub

    Protected Sub btnlk_addEmployeed_Click(sender As Object, e As EventArgs) Handles btnlk_addEmployeed.Click

        If Val(cmb_usu.SelectedValue) > 0 Then

            addMemeber_toRole(Val(cmb_usu.SelectedValue))

            grd_cate.DataSource = dtRoles ' cl_RolUser.Add_User_ROLE(cmb_usu.SelectedValue)
            grd_cate.Rebind()

            Dim tbl_UserRol As New DataTable
            tbl_UserRol = cl_RolUser.setRol_USER(True, dtRoles)

            cmb_usu.DataSource = tbl_UserRol
            cmb_usu.DataBind()
            cmb_usu.SelectedIndex = -1
            cmb_usu.Text = ""

            If tbl_UserRol.Rows.Count = 0 Then
                btnlk_addEmployeed.Enabled = False
                lblt_filerequeried.Visible = "true"
            Else
                btnlk_addEmployeed.Enabled = True
                lblt_filerequeried.Visible = "false"
            End If


        End If

    End Sub
End Class
