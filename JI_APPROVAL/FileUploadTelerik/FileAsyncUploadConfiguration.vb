Imports Telerik.Web.UI

Public Class FileAsyncUploadConfiguration
    Inherits AsyncUploadConfiguration
    Private m_userID As Integer
    Public Property UserID() As Integer
        Get
            Return m_userID
        End Get

        Set(ByVal value As Integer)
            m_userID = value
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
