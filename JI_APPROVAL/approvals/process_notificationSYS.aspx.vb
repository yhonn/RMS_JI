
Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports CuteWebUI
Imports System.Net.Mail
Imports System.Net
Imports ly_APPROVAL

Public Class process_notificationSYS
    Inherits System.Web.UI.Page

    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim strResult As String
    Dim cl_user As ly_SIME.CORE.cls_user


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login.aspx")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
        End If

        Try

            Dim cl_Noti_Process As New APPROVAL.notification_proccess(Me.Session("E_IDPrograma"), 1007, cl_user.regionalizacionCulture)
            Dim tbl_UserPending As DataTable

            '--StandBy for Share Roles, send to the originator 
            tbl_UserPending = cl_Noti_Process.get_notificationSharedRoles("StandBy")

            For Each dtR As DataRow In tbl_UserPending.Rows
                cl_Noti_Process.Notify_App_ByOriginator(dtR("ID"))
            Next

            '--Pending for Share Roles, send to the originator 
            'Dim tbl_UserPending As DataTable = cl_Noti_Process.get_notificationSharedRoles("Pending")
            tbl_UserPending = cl_Noti_Process.get_notificationSharedRoles("Pending")

            For Each dtR As DataRow In tbl_UserPending.Rows
                cl_Noti_Process.Notify_App_ByRolOwner(dtR("ID"), True) 'Shared Roles
            Next

            '--Pending for Share Roles, send to the originator 
            'Dim tbl_UserPending As DataTable = cl_Noti_Process.get_notificationPending()

            tbl_UserPending = cl_Noti_Process.get_notificationPending()

            For Each dtR As DataRow In tbl_UserPending.Rows
                cl_Noti_Process.Notify_App_ByRolOwner(dtR("ID"), False) 'Simple Roles
            Next

            strResult &= "<br /> Finished proccess"

        Catch ex As Exception

            strResult &= String.Format("{0}{0}{0}  {1}!!Error Generado Correos!!{2}: {3}", "<br />", "<strong>", "</strong>", ex.Message)

        End Try

        Dim dvResult As System.Web.UI.HtmlControls.HtmlGenericControl = New System.Web.UI.HtmlControls.HtmlGenericControl
        dvResult.InnerHtml = strResult
        pnResult.Controls.Add(dvResult)


    End Sub

End Class