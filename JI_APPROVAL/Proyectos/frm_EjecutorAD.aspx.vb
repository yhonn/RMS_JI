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


Public Class frm_EjecutorAD
    Inherits System.Web.UI.Page

    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "NEW_EJECUT"
    Dim controles As New ly_SIME.CORE.cls_controles
    'Dim db As New dbRMS_JIEntities

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then
            Me.lbl_codigo_ejecutor.Text = CrearCodigo()
            Dim id_programa = Convert.ToInt32(Me.Session("E_IdPrograma").ToString())
            Using dbEntities As New dbRMS_JIEntities
                Me.cmb_tipo_organizacion.DataSource = dbEntities.tme_organization_type.Where(Function(p) p.id_programa = id_programa).ToList()
                Me.cmb_tipo_organizacion.DataValueField = "id_organization_type"
                Me.cmb_tipo_organizacion.DataTextField = "organization_type"
                Me.cmb_tipo_organizacion.DataBind()
            End Using

        End If
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click

        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False

        If errSave = False Then
            'cnnME.Open()
            Using dbEntities As New dbRMS_JIEntities
                Dim oEjecutor As New t_ejecutores

                oEjecutor.nombre_ejecutor = Me.txt_nombreEjecutor.Text
                oEjecutor.nombre_corto = Me.txt_NombreCorto.Text
                oEjecutor.codigo_ejecutor = Me.lbl_codigo_ejecutor.Text
                oEjecutor.nit = Me.txt_nit.Text
                oEjecutor.telefono_ejecutor = Me.txt_telefono_ejecutor.Text
                oEjecutor.email_ejecutor = Me.txt_email.Text
                oEjecutor.representante_legal = Me.txt_representante.Text
                oEjecutor.cedula_representante = Me.txt_cedula.Text
                oEjecutor.telefono_representante = Me.txt_telefono_representante.Text
                oEjecutor.fecha_constitucion = Me.dt_fecha_inicio.SelectedDate
                'oEjecutor.numero_socios = Me.txt_numsocios.Value
                oEjecutor.billing_info = Me.txt_billin_info.Text

                oEjecutor.id_municipio = Me.ctrl_ubicacionGeografica.value_cmb_municipio.SelectedValue
                'If Me.ctrl_ubicacionGeografica.value_cmb_vereda.SelectedItem IsNot Nothing Then
                '    oEjecutor.id_vereda = Me.ctrl_ubicacionGeografica.value_cmb_vereda.SelectedValue
                'End If
                'If Me.ctrl_ubicacionGeografica.value_cmb_corregimiento.SelectedItem IsNot Nothing Then
                '    oEjecutor.id_corregimiento = Me.ctrl_ubicacionGeografica.value_cmb_corregimiento.SelectedValue
                'End If
                'If Me.ctrl_ubicacionGeografica.value_cmb_parish.SelectedItem IsNot Nothing Then
                '    oEjecutor.id_parish = Me.ctrl_ubicacionGeografica.value_cmb_parish.SelectedValue
                'End If
                'If Me.ctrl_ubicacionGeografica.value_cmb_village.SelectedItem IsNot Nothing Then
                '    oEjecutor.id_municipio = Me.ctrl_ubicacionGeografica.value_cmb_village.SelectedValue
                'End If

                oEjecutor.id_organization_type = Me.cmb_tipo_organizacion.SelectedValue
                oEjecutor.address = Me.txt_direccion.Text


                oEjecutor.datecreated = Date.Now
                oEjecutor.id_usuario_creo = Me.Session("E_IdUser").ToString()
                oEjecutor.id_programa = Me.Session("E_IdPrograma").ToString()
                oEjecutor.id_estado_EJ = 1

                dbEntities.t_ejecutores.Add(oEjecutor)
                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_ejecutor"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If


    End Sub

    Function CrearCodigo() As String
        Dim txtCodigo As String = ""
        Dim fecha As DateTime = Date.Now
        Dim textfecha = fecha.ToString.Replace("/", "").Replace(" ", "").Replace("a", "").Replace(".", "").Replace("m", "").Replace(":", "").Replace(";", "").Replace("p", "")
        Dim CorrrelativoStr As String = ""
        Dim dm As New SqlDataAdapter("SELECT { fn IFNULL(MAX(id_ejecutor), 0) } + 1 AS Correlativo FROM t_ejecutores", cnnME)
        Dim ds As New DataSet("Codigos")
        dm.Fill(ds, "Codigos")
        Dim Corrrelativo As Integer = (Val(ds.Tables("Codigos")(0)(0)).ToString)
        If Corrrelativo < 10 Then
            CorrrelativoStr = "000"
        ElseIf Corrrelativo < 100 Then
            CorrrelativoStr = "00"
        ElseIf Corrrelativo < 1000 Then
            CorrrelativoStr = "0"
        End If

        txtCodigo = "-" & textfecha.Substring(0, 8) & "-" & CorrrelativoStr & Corrrelativo
        CorrrelativoStr = ""
        Corrrelativo = Val(Me.Session("E_IdProy"))
        If Corrrelativo < 10 Then
            CorrrelativoStr = "00"
        ElseIf Corrrelativo < 100 Then
            CorrrelativoStr = "0"
        End If

        txtCodigo = CorrrelativoStr & Corrrelativo & txtCodigo

        'Me.lbl_codigo_proyecto.Text = txtCodigo
        Return (txtCodigo)
    End Function
    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/proyectos/frm_Ejecutor")
    End Sub
    
End Class