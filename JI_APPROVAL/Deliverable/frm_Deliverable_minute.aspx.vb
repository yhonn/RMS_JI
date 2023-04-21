Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports ly_SIME
Imports System.Web.Services
Imports ly_RMS
Imports System.Globalization
Imports System.Web.Script.Serialization


Public Class frm_Deliverable_minute
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_DELIV_MINUTE"

    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clss_approval As APPROVAL.clss_approval


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
                ' cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            If Not IsNothing(Me.Request.QueryString("ID")) Then
                Me.hd_id_deliverable.Value = Convert.ToInt32(Me.Request.QueryString("ID"))
            Else
                Me.hd_id_deliverable.Value = 0
            End If

            LoadData(hd_id_deliverable.Value)

        End If

    End Sub

    Public Sub LoadData(ByVal idDeliverable As Integer)



    End Sub

End Class