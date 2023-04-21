
Imports ly_APPROVAL

Public Class frm_send_email
    Inherits System.Web.UI.Page


    Dim clss_approval As APPROVAL.clss_approval
    Dim id_documento As Integer = 0
    Dim id_programa As Integer = 0
    Dim id_notificacion As Integer = 0
    Dim id_app_documento As Integer = 0
    Dim err As Boolean
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

        err = False

        If Not String.IsNullOrEmpty(Me.Request.QueryString("IdDoc")) Then
            id_documento = Me.Request.QueryString("IdDoc").ToString
        Else
            err = True
        End If

        If Not String.IsNullOrEmpty(Me.Request.QueryString("IdProg")) Then
            id_programa = Me.Request.QueryString("IdProg").ToString
        Else
            err = True
        End If


        If Not String.IsNullOrEmpty(Me.Request.QueryString("IdNoti")) Then
            id_notificacion = Me.Request.QueryString("IdNoti").ToString
        Else
            err = True
        End If


        If Not String.IsNullOrEmpty(Me.Request.QueryString("IdApp")) Then
            id_app_documento = Me.Request.QueryString("IdApp").ToString
        Else
            err = True
        End If



        Try


            If Not err Then


                If id_notificacion = 5 Then 'Normal step notification

                    'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"),1047, 5, cl_user.regionalizacionCulture, 1179)
                    Dim objEmail As New APPROVAL.cls_notification(id_programa, id_documento, id_notificacion, cl_user.regionalizacionCulture, id_app_documento)
                    If (objEmail.Emailing_APPROVAL_STEP(id_app_documento)) Then
                        ' Response.Write("Error Sending Doc")

                        Me.lblerr_user.Text = "Email Sent  for id step:" & id_app_documento.ToString

                    Else 'Error mandando Email
                        'Response.Write("Email Sended")
                        Me.lblerr_user.Text = "Error on sending the email"

                    End If

                ElseIf id_notificacion = 1009 Then 'Deliverables Notification


                    '*********************************APPROVED****************************************
                    Dim objEmail As New APPROVAL.cls_notification(id_programa, id_documento, id_notificacion, cl_user.regionalizacionCulture, id_app_documento)
                    If (objEmail.Emailing_DELIVERABLE_APPROVAL(id_app_documento)) Then

                        Me.lblerr_user.Text = "Email Sent for id step:" & id_app_documento.ToString

                    Else 'Error mandando Email

                        Me.lblerr_user.Text = "Error on sending the email"

                    End If
                    '*********************************APPROVED****************************************



                End If



            Else
                Response.Write("Error Vars")
            End If

        Catch ex As Exception

            Response.Write(ex.Message)

        End Try

    End Sub

End Class