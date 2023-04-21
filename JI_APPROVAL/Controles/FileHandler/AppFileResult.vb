Imports System
Imports Telerik.Web.UI

' The result object is returned from the handler to the page.
' You can include custom fields in the result by extending the AsyncUploadResult class.
' In this case we return the ID of the image record.
Public Class AppFileResult
    Inherits AsyncUploadResult
    Private m_imageID As Integer
    Private m_fileName As String
    Private m_uploadedStatus As Integer
    Private m_ErrorMSG As String

    Public Property ImageID() As Integer
        Get
            Return m_imageID
        End Get
        Set(ByVal value As Integer)
            m_imageID = value
        End Set
    End Property

    Public Property fileNameResult() As String
        Get
            Return m_fileName
        End Get
        Set(ByVal value As String)
            m_fileName = value
        End Set
    End Property

    Public Property UploadedStatus() As Integer
        Get
            Return m_uploadedStatus
        End Get
        Set(ByVal value As Integer)
            m_uploadedStatus = value
        End Set
    End Property

    Public Property exceptionMSG() As String
        Get
            Return m_ErrorMSG
        End Get
        Set(ByVal value As String)
            m_ErrorMSG = value
        End Set
    End Property

End Class
