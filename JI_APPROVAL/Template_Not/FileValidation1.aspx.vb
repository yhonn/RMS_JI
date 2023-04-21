Imports Telerik.Web.UI


Public Class FileValidation1

    Inherits System.Web.UI.Page

    Const MaxTotalBytes As Integer = 1048576   ' 1 MB
    Private totalBytes As Long

    Protected Sub Page_Load(sender As Object, e As EventArgs)
    End Sub

    Public Sub RadAsyncUpload1_FileUploaded(sender As Object, e As FileUploadedEventArgs)

        BtnSubmit.Visible = False
        RefreshButton.Visible = True
        RadAsyncUpload1.Visible = False

        Dim liItem = New HtmlGenericControl("li")
        liItem.InnerText = e.File.FileName


        If totalBytes < MaxTotalBytes Then
            ' Total bytes limit has not been reached, accept the file
            e.IsValid = True
            totalBytes += e.File.ContentLength
        Else
            ' Limit reached, discard the file
            e.IsValid = False
        End If

        If e.IsValid Then

            ValidFiles.Visible = True

            ValidFilesList.Controls.AddAt(0, liItem)
        Else

            InvalidFiles.Visible = True
            InValidFilesList.Controls.AddAt(0, liItem)
        End If
    End Sub

    Protected Sub RefreshButton_Click(sender As Object, e As EventArgs)
        Page.Response.Redirect(Request.RawUrl)
    End Sub

End Class