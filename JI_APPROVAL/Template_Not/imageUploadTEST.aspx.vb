
Imports System
Imports System.Linq
Imports System.IO
Imports Telerik.Web.UI
Imports Telerik.Web.UI.ImageEditor

Partial Public Class imageUploadTEST
    'Inherits Telerik.Web.QuickStart.QsfPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        'PreventOverwriteProvider.EnableImageProvider(Me, RadImageEditor1)
    End Sub

    Protected Sub AsyncUpload1_FileUploaded(sender As Object, e As FileUploadedEventArgs)
        'Clear changes and remove uploaded image from Cache
        RadImageEditor1.ResetChanges()
        HttpContext.Current.Cache.Remove(HttpContext.Current.Session.SessionID + "UploadedFile")
        'Context.Cache.Remove(Session.SessionID + "UploadedFile")
        Using stream As Stream = e.File.InputStream
            Dim imgData As Byte() = New Byte(stream.Length) {}
            stream.Read(imgData, 0, imgData.Length)
            Dim ms As New MemoryStream()
            ms.Write(imgData, 0, imgData.Length)
            HttpContext.Current.Cache.Insert(HttpContext.Current.Session.SessionID + "UploadedFile", ms, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
            'Context.Cache.Insert(Session.SessionID + "UploadedFile", ms, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        End Using
    End Sub

    Protected Sub RadImageEditor1_ImageLoading(sender As Object, args As ImageEditorLoadingEventArgs)
        'Handle Uploaded images

        If [Object].Equals(HttpContext.Current.Cache.[Get](HttpContext.Current.Session.SessionID + "UploadedFile"), Nothing) Then
            Using image As New EditableImage(DirectCast(HttpContext.Current.Cache.[Remove](HttpContext.Current.Session.SessionID + "UploadedFile"), MemoryStream))
                args.Image = image.Clone()
                args.Cancel = True
            End Using
        End If

        'If Not [Object].Equals(Context.Cache.[Get](Session.SessionID + "UploadedFile"), Nothing) Then
        '    Using image As New EditableImage(DirectCast(Context.Cache.[Remove](Session.SessionID + "UploadedFile"), MemoryStream))
        '        args.Image = image.Clone()
        '        args.Cancel = True
        '    End Using
        'End If
    End Sub
End Class
