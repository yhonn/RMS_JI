
Imports System
Imports System.Web
Imports Telerik.Web.UI
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Data.SqlClient
Imports System.IO

Public Class AppFileHandler
    Inherits AsyncUploadHandler
    Implements System.Web.SessionState.IRequiresSessionState

    Protected Overrides Function Process(ByVal file As UploadedFile, ByVal context As HttpContext, ByVal configuration As IAsyncUploadConfiguration, ByVal tempFileName As String) As IAsyncUploadResult
        ' Call the base Process method to save the file to the temporary folder
        ' base.Process(file, context, configuration, tempFileName);

        'Populate the default (base) result into an object of type SampleAsyncUploadResult
        Dim result As AppFileResult = CreateDefaultUploadResult(Of AppFileResult)(File)

        Dim idUser As Integer = -1
        Dim idDoc As Integer = -1
        Dim sFileDir As String = context.Server.MapPath("~") & "\Temp\"

        Try

            ' You can obtain any custom information passed from the page via casting the configuration parameter to your custom class
            Dim FileConfiguration As AppFileConfiguration = TryCast(configuration, AppFileConfiguration)
            If FileConfiguration IsNot Nothing Then
                idUser = FileConfiguration.IDuser
                idDoc = FileConfiguration.IDdocument
                sFileDir = FileConfiguration.finalPath
            End If

            ' Populate any additional fields into the upload result.
            ' The upload result is available both on the client and on the server
            '*************Result.ImageID = InsertImage(file, userID)
            '*************************************************************************MY HANDDLER*******************************************************

            Dim dmyhm As String = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))
            Dim extension As String = System.IO.Path.GetExtension(tempFileName)
            Dim fileNameWE = System.IO.Path.GetFileNameWithoutExtension(tempFileName)
            Dim newFile As String
            newFile = String.Format("doc{0}_0{1}_{2}_{3}{4}{5}", idDoc, idUser, dmyhm, fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-"), "_v1.1", extension)

            file.SaveAs(sFileDir & newFile)

            result.fileNameResult = newFile
            result.UploadedStatus = 1
            result.exceptionMSG = ""
            '*************************************************************************MY HANDDLER*******************************************************

        Catch ex As Exception

            result.fileNameResult = "--none--"
            result.UploadedStatus = -1
            result.exceptionMSG = ex.Message

        End Try

        Return result

    End Function

    Public Function InsertImage(ByVal file As UploadedFile, ByVal userID As Integer) As Integer
        Dim connectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("TelerikConnectionString35").ConnectionString

        Using conn As New SqlConnection(connectionString)
            Dim cmdText As String = "INSERT INTO AsyncUploadImages VALUES(@ImageData, @ImageName, @UserID) SET @Identity = SCOPE_IDENTITY()"
            Dim cmd As New SqlCommand(cmdText, conn)

            Dim imageData As Byte() = GetImageBytes(file.InputStream)

            Dim identityParam As New SqlParameter("@Identity", SqlDbType.Int, 0, "ImageID")
            identityParam.Direction = ParameterDirection.Output

            cmd.Parameters.AddWithValue("@ImageData", imageData)
            cmd.Parameters.AddWithValue("@ImageName", file.GetName())
            cmd.Parameters.AddWithValue("@UserID", userID)

            cmd.Parameters.Add(identityParam)

            conn.Open()
            cmd.ExecuteNonQuery()

            Return CInt(identityParam.Value)
        End Using
    End Function

    Public Function GetImageBytes(ByVal stream As Stream) As Byte()
        Dim buffer As Byte()

        Using image As Bitmap = ResizeImage(stream)
            Using ms As New MemoryStream()
                image.Save(ms, ImageFormat.Jpeg)

                'return the current position in the stream at the beginning
                ms.Position = 0

                buffer = New Byte(ms.Length - 1) {}
                ms.Read(buffer, 0, CInt(ms.Length))
                Return buffer
            End Using
        End Using
    End Function

    Public Function ResizeImage(ByVal stream As Stream) As Bitmap
        Dim originalImage As Image = Bitmap.FromStream(stream)

        Dim height As Integer = 500
        Dim width As Integer = 500

        Dim ratio As Double = Math.Min(originalImage.Width, originalImage.Height) / CDbl(Math.Max(originalImage.Width, originalImage.Height))

        If originalImage.Width > originalImage.Height Then
            height = Convert.ToInt32(height * ratio)
        Else
            width = Convert.ToInt32(width * ratio)
        End If

        Dim scaledImage As New Bitmap(width, height)

        Using g As Graphics = Graphics.FromImage(scaledImage)
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
            g.DrawImage(originalImage, 0, 0, width, height)

            Return scaledImage
        End Using

    End Function

End Class