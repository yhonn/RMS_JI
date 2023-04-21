Imports System.Web
Imports System.Web.Services
Imports System.Data.SqlClient

Public Class StreamImage
    Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim id As Integer = -1
        Dim isNumber As Boolean = Integer.TryParse(context.Request.QueryString("imageID"), id)

        If Not isNumber Then
            context.Response.[End]()
        End If

        'Dim imageData As Byte() = GetImage(id)

        'context.Response.ContentType = "image/jpeg"
        'context.Response.BinaryWrite(imageData)
        'context.Response.Flush()
        'context.Response.Write("test")
    End Sub

    Private Function GetImage(ByVal id As Integer) As Byte()
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TelerikConnectionString35").ConnectionString

        Using conn As New SqlConnection(connectionString)
            Dim cmdText As String = "SELECT ImageData FROM AsyncUploadImages WHERE ImageID = @ImageID;"

            Dim cmd As New SqlCommand(cmdText, conn)
            Dim idParam As New SqlParameter("@ImageID", SqlDbType.Int)
            idParam.Value = id

            cmd.Parameters.Add(idParam)
            conn.Open()

            Using reader As SqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    Dim imageData As Byte() = DirectCast(reader("ImageData"), Byte())
                    Return imageData
                Else
                    Throw New ArgumentException("Invalid ID")
                End If
            End Using
        End Using
    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class