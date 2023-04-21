Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Web
Imports System.IO
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Net
Imports System.Math
Imports Telerik.Web.UI
Imports System.Net.Mail
Imports ly_SIME
Public Class frm_EjecutorPrint
    Inherits System.Web.UI.Page

    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login")
        End Try
        If Not IsPostBack Then
            Me.lbl_id_ejecutor.Text = Me.Request.QueryString("IdEjecME").ToString
            FillData(Convert.ToInt32(Me.lbl_id_ejecutor.Text))
        End If
    End Sub

    Sub FillData(ByVal IdEjecutor As Integer)

        Using dbRMS As New dbRMS_JIEntities
            Dim oEjecutor = dbRMS.t_ejecutores.Find(IdEjecutor)
            Me.lbl_codigo_ejecutor.Text = oEjecutor.codigo_ejecutor
            Me.txt_nombreEjecutor.Text = oEjecutor.nombre_ejecutor
            Me.txt_NombreCorto.Text = oEjecutor.nombre_corto
            Me.txt_nit.Text = oEjecutor.nit
            Me.txt_telefono_ejecutor.Text = oEjecutor.telefono_ejecutor
            Me.txt_email.Text = oEjecutor.email_ejecutor
            Me.txt_representante.Text = oEjecutor.representante_legal
            Me.txt_cedula.Text = oEjecutor.cedula_representante
            Me.txt_telefono_representante.Text = oEjecutor.telefono_representante
            Me.txt_numsocios.Text = oEjecutor.numero_socios
            Me.dt_fecha_inicio.Text = oEjecutor.fecha_constitucion
            Me.lbl_estado.Text = oEjecutor.t_ejecutor_estado.nombre_estado

        End Using
    End Sub

End Class