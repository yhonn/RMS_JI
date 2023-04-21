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


Public Class frm_EjecutorEdit
    Inherits System.Web.UI.Page

    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "EDIT_EJECU"
    Dim db As New dbRMS_JIEntities
    Dim controles As New ly_SIME.CORE.cls_controles

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
            Me.lbl_id_ejecutor.Text = Me.Request.QueryString("IdEjecME").ToString
            FillData(Me.lbl_id_ejecutor.Text)

        End If
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click
        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False

        If errSave = False Then

            Using dbEntities As New dbRMS_JIEntities
                Dim id_ejecutor = Convert.ToInt32(Me.lbl_id_ejecutor.Text)
                Dim oEjecutor = dbEntities.t_ejecutores.Find(id_ejecutor)

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
                oEjecutor.numero_socios = Me.txt_numsocios.Value
                oEjecutor.id_municipio = Me.ctrl_ubicacionGeografica.value_cmb_municipio.SelectedValue
                oEjecutor.billing_info = Me.txt_billin_info.Text

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


                oEjecutor.dateupd = Date.Now
                oEjecutor.id_usuario_upd = Me.Session("E_IdUser").ToString()
                oEjecutor.id_programa = Me.Session("E_IdPrograma").ToString()
                oEjecutor.id_estado_EJ = 1

                dbEntities.Entry(oEjecutor).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_ejecutor"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If

    End Sub

    Sub FillData(ByVal IdEjecutor As String)

        Dim id_ejecutor = Convert.ToInt32(IdEjecutor)
        Dim oEjecutor = db.t_ejecutores.Find(id_ejecutor)

        Me.lbl_codigo_ejecutor.Text = oEjecutor.codigo_ejecutor
        Me.txt_nombreEjecutor.Text = oEjecutor.nombre_ejecutor
        Me.txt_NombreCorto.Text = oEjecutor.nombre_corto
        Me.txt_nit.Text = oEjecutor.nit
        Me.txt_telefono_ejecutor.Text = oEjecutor.telefono_ejecutor
        Me.txt_email.Text = oEjecutor.email_ejecutor
        Me.txt_representante.Text = oEjecutor.representante_legal
        Me.txt_cedula.Text = oEjecutor.cedula_representante
        Me.txt_telefono_representante.Text = oEjecutor.telefono_representante
        Me.txt_direccion.Text = oEjecutor.address
        'Me.txt_numsocios.Text = oEjecutor.numero_socios
        Me.dt_fecha_inicio.SelectedDate = oEjecutor.fecha_constitucion
        Me.txt_billin_info.Text = oEjecutor.billing_info


        Dim id_programa = Convert.ToInt32(Me.Session("E_IdPrograma").ToString())
        Using dbEntities As New dbRMS_JIEntities
            Me.cmb_tipo_organizacion.DataSource = dbEntities.tme_organization_type.Where(Function(p) p.id_programa = id_programa).ToList()
            Me.cmb_tipo_organizacion.DataValueField = "id_organization_type"
            Me.cmb_tipo_organizacion.DataTextField = "organization_type"
            Me.cmb_tipo_organizacion.DataBind()
            Me.cmb_tipo_organizacion.SelectedValue = oEjecutor.id_organization_type

            'Dim location = dbEntities.vw_location_all.FirstOrDefault(Function(p) (p.id_municipio = oEjecutor.id_municipio Or Not oEjecutor.id_municipio.HasValue))

            Me.ctrl_ubicacionGeografica.value_cmb_departamento.SelectedValue = oEjecutor.t_municipios.id_departamento
            Me.ctrl_ubicacionGeografica.value_cmb_municipio.SelectedValue = oEjecutor.id_municipio
            'If oEjecutor.id_vereda > 0 Then
            '    Me.ctrl_ubicacionGeografica.value_cmb_vereda.SelectedValue = oEjecutor.id_vereda
            'End If
            'If oEjecutor.id_corregimiento > 0 Then
            '    Me.ctrl_ubicacionGeografica.value_cmb_corregimiento.SelectedValue = oEjecutor.id_corregimiento
            'End If
            'Me.ctrl_ubicacionGeografica.value_cmb_vereda.SelectedValue = location.id_subcounty

            'If oEjecutor.id_parish.HasValue And Not oEjecutor.id_municipio.HasValue Then
            '    'Me.ctrl_ubicacionGeografica.value_cmb_parish.SelectedValue = oEjecutor.id_parish
            'End If

            If oEjecutor.id_municipio.HasValue Then
                'Me.ctrl_ubicacionGeografica.value_cmb_village.SelectedValue = oEjecutor.id_municipio
            End If


        End Using


    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/proyectos/frm_Ejecutor")
    End Sub
End Class