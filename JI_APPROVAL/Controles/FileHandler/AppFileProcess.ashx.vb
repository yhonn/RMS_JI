Imports System.Web
Imports System.Web.Services
Imports System.Web.SessionState
Imports System.Web.Script.Serialization


Public Class AppFileProcess
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest



        Dim myArray(1) As Object
        Dim serializer As New JavaScriptSerializer()
        Dim jsonITEMS As String = "[]"

        Try

            context.Response.ContentType = "text/plain" '"text/json"
            Dim id As Integer = -1
            'Dim isNumber As Boolean = Integer.TryParse(context.Request.QueryString("imageID"), id)
            Dim FileName As String = context.Request.Form("FileName")
            Dim SourceAddr As String = context.Request.Form("SourceAddr")
            Dim FinalAddr As String = context.Request.QueryString("FinalAddr")
            Dim TypeProcess As String = context.Request.QueryString("TypeProcess") 'Move, Delete

            Dim idEstatus As Integer = -1
            Dim msgEstatus As String = ""

            Dim boolResult As Boolean = True
            Dim flagERR As Boolean = True

            If (Not IsNothing(TypeProcess)) Then
                If TypeProcess = "Move" Then
                    If (IsNothing(SourceAddr) Or IsNothing(FinalAddr)) Then
                        flagERR = False
                        idEstatus = -1
                        msgEstatus &= "--Addresses are not valid--"
                    End If
                ElseIf TypeProcess = "Delete" Then
                    If (IsNothing(SourceAddr)) Then
                        flagERR = False
                        idEstatus = -1
                        msgEstatus &= "--The source address is not valid--"
                    End If
                Else
                    flagERR = False
                End If
            Else
                flagERR = False
                idEstatus = -1
                msgEstatus &= "--Uknowing Type of process--"
            End If


            If TypeProcess = "Move" Then
                If Not MoveFile(FileName, SourceAddr, FinalAddr, context) Then
                    flagERR = False
                    idEstatus = -1
                    msgEstatus &= "--Unabled to moved the file--"
                Else
                    idEstatus = 1
                    msgEstatus &= "--File has been moved successfully--"
                End If
            ElseIf TypeProcess = "Delete" Then
                If Not DeleteFile(FileName, SourceAddr, context) Then
                    flagERR = False
                    idEstatus = -1
                    msgEstatus &= "--Unabled to Delete the file--"
                Else
                    idEstatus = 1
                    msgEstatus &= "--File has been deleted successfully--"
                End If
            End If

            '******************Valid the variables*****************
            'If Not isNumber Then
            'context.Response.[End]()
            'End If
            '******************Valid the variables*****************
            'Dim imageData As Byte() = GetImage(id)
            'context.Response.ContentType = "image/jpeg"
            'context.Response.BinaryWrite(imageData)
            'context.Response.Flush()

            myArray(0) = New With {Key .Estado = idEstatus, .Mensaje = msgEstatus}
            jsonITEMS = serializer.Serialize(myArray)
            context.Response.Write(jsonITEMS)

            'If Not flagERR Then
            '    context.Response.Write(jsonITEMS)
            '    'context.Response.[End]()
            'Else
            '    context.Response.Write(boolResult.ToString())
            'End If


        Catch ex As Exception
            myArray(0) = New With {Key .Estado = -1, .Mensaje = ex.Message}
            jsonITEMS = serializer.Serialize(myArray)
            context.Response.Write(jsonITEMS)
        End Try

    End Sub

    Private Function MoveFile(ByVal fName As String, ByVal sAddress As String, ByVal dAddress As String, context As HttpContext) As Boolean

        Try
            Dim sFileDir As String = context.Server.MapPath("~") & sAddress
            Dim dFileDir As String = context.Server.MapPath("~") & dAddress
            Dim file_info As New IO.FileInfo(sFileDir + fName)
            If (file_info.Exists) Then
                file_info.CopyTo(dFileDir)
                MoveFile = True
            Else
                MoveFile = False
            End If

        Catch ex As Exception
            MoveFile = False
        End Try

    End Function


    Private Function DeleteFile(ByVal fName As String, ByVal sAddress As String, context As HttpContext) As Boolean

        Try

            Dim sFileDir As String = context.Server.MapPath("~") & sAddress
            Dim file_info As New IO.FileInfo(sFileDir + fName)
            If (file_info.Exists) Then
                file_info.Delete()
                DeleteFile = True
            Else
                DeleteFile = False
            End If

        Catch ex As Exception
            DeleteFile = False
        End Try

    End Function


    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class