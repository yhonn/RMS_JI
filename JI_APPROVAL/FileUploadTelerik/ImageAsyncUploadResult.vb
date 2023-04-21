Imports Telerik.Web.UI

Public Class ImageAsyncUploadResult
    Inherits AsyncUploadResult
    Private m_imageID As Integer

    Public Property ImageID() As Integer
        Get
            Return m_imageID
        End Get
        Set(ByVal value As Integer)
            m_imageID = value
        End Set
    End Property

    Private fileNameValue As String
    Public Property FileNameResult() As String
        Get
            Return fileNameValue
        End Get
        Set(ByVal value As String)
            fileNameValue = value
        End Set
    End Property


End Class
