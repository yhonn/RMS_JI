Imports System
Imports Telerik.Web.UI

' The upload configuration object is passed from the page to the custom handler.
' You can customize it to include custom properties by extending the AsyncUploadConfiguration class.
' In this case we send the ID of the currently logged-in user to be stored in the database as the auther of the image.
Public Class AppFileConfiguration
    Inherits AsyncUploadConfiguration

    Private m_iddocument As Integer
    Private m_iduser As Integer
    Private m_FinalPath As String

    Public Property IDuser() As Integer
        Get
            Return m_iduser
        End Get
        Set(ByVal value As Integer)
            m_iduser = value
        End Set
    End Property


    Public Property IDdocument() As Integer
        Get
            Return m_iddocument
        End Get
        Set(ByVal value As Integer)
            m_iddocument = value
        End Set
    End Property

    Public Property finalPath() As String
        Get
            Return m_FinalPath
        End Get
        Set(ByVal value As String)
            m_FinalPath = value
        End Set
    End Property


End Class
