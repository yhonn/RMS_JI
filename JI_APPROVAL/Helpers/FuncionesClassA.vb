Imports System
Imports System.Web
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Threading

Public Class FuncionesClassA
    Public Function create(ByVal sql As String) As Boolean
        Dim result As Boolean
        result = True
        Return True
    End Function
    Public Function delete(ByVal sql As String) As Boolean
        Dim result As Boolean
        Try
            Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
            cnn.Open()
            Dim dm As New SqlDataAdapter(sql, cnn)
            dm.SelectCommand.CommandText = sql
            dm.SelectCommand.ExecuteNonQuery()
            cnn.Close()
            cnn.Dispose()
            result = True
        Catch ex As Exception
            result = False
        End Try
        Return result
    End Function

    Public Function selectdb(ByVal sql As String) As DataTable
        Dim result As Boolean
        Dim dt As New DataTable
        Try
            Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
            cnn.Open()
            Dim dm As New SqlDataAdapter(sql, cnn)
            dm.SelectCommand.CommandText = sql
            dm.SelectCommand.ExecuteNonQuery()
            dm.Fill(dt)
            cnn.Close()
            cnn.Dispose()
        Catch ex As Exception
            result = False
        End Try
        Return dt
    End Function
End Class
