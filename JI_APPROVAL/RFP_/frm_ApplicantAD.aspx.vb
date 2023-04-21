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


Public Class frm_ApplicantAD
    Inherits System.Web.UI.Page

    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "ADD_APPLICANT"
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
            '  Me.lbl_codigo_ejecutor.Text = CrearCodigo() adding in another step to coding APP + YEAR + COUNTRY + CORRELATIVE (APP2020BR001)
            Dim id_programa = Convert.ToInt32(Me.Session("E_IdPrograma").ToString())
            Using dbEntities As New dbRMS_JIEntities
                Me.cmb_tipo_organizacion.DataSource = dbEntities.tme_organization_type.Where(Function(p) p.id_programa = id_programa).OrderBy(Function(p) p.organization_type).ToList()
                Me.cmb_tipo_organizacion.DataValueField = "id_organization_type"
                Me.cmb_tipo_organizacion.DataTextField = "organization_type"
                Me.cmb_tipo_organizacion.DataBind()
            End Using

            If Not Me.Request.QueryString("PPT") Is Nothing Then

                Dim idApplicant As Integer = Convert.ToInt32(Me.Request.QueryString("PPT"))
                Me.lbl_IDapplicant.Text = idApplicant
                LoadApplicant(idApplicant)

            End If

        End If
    End Sub


    Sub LoadApplicant(ByVal idApplicant As Integer)

        Using dbEntities As New dbRMS_JIEntities


            Dim oApplicant As VW_TA_ORGANIZATION_APP = dbEntities.VW_TA_ORGANIZATION_APP.Where(Function(p) p.ID_ORGANIZATION_APP = idApplicant).FirstOrDefault


            cmb_tipo_organizacion.SelectedValue = oApplicant.ID_ORGANIZATION_TYPE
            Me.txt_nombreEjecutor.Text = oApplicant.ORGANIZATIONNAME
            Me.txt_NombreCorto.Text = oApplicant.NAMEALIAS
            Me.txt_nit.Text = oApplicant.ORGANIZATIONNUMBER
            Me.txt_telefono_ejecutor.Text = oApplicant.ORGANIZATIONPHONE
            Me.txt_email.Text = oApplicant.ORGANIZATIONEMAIL

            Me.txt_contact_name.Text = oApplicant.PERSONFIRSTNAME
            Me.txt_contact_lastname.Text = oApplicant.PERSONLASTNAME
            Me.txt_cedula.Text = oApplicant.PERSONID
            Me.txt_email2.Text = oApplicant.PRIMARYCONTACTEMAIL
            Me.txt_telefono_representante.Text = oApplicant.PRIMARYCONTACTPHONE
            Me.dt_fecha_inicio.SelectedDate = oApplicant.ORGANIZATIONREGDATE


            Dim oVillage = dbEntities.vw_t_village.Where(Function(p) p.id_village = oApplicant.ID_COUNTY).ToList()

            Me.txt_direccion.Text = oApplicant.ADDRESSSTREET

            If oVillage.Count > 0 Then

                Me.ctrl_ubicacionGeografica.AddVillage = oApplicant.ID_COUNTY

                'Me.ctrl_ubicacionGeografica.value_cmb_country.SelectedValue = oVillage.FirstOrDefault.id_pais
                'Me.ctrl_ubicacionGeografica.value_cmb_zone.SelectedValue = oVillage.FirstOrDefault.id_region
                'Me.ctrl_ubicacionGeografica.value_cmb_division.SelectedValue = oVillage.FirstOrDefault.id_subregion
                'Me.ctrl_ubicacionGeografica.value_cmb_district.SelectedValue = oVillage.FirstOrDefault.id_district
                'Me.ctrl_ubicacionGeografica.value_cmb_upazila.SelectedValue = oVillage.FirstOrDefault.id_county
                'Me.ctrl_ubicacionGeografica.value_cmb_union.SelectedValue = oVillage.FirstOrDefault.id_subcounty
                'Me.ctrl_ubicacionGeografica.value_cmb_village.SelectedValue = oVillage.FirstOrDefault.id_parish

                'Me.ctrl_ubicacionGeografica.value_cmb_district.Text = oApplicant.ADDRESSSTATE
                'Me.ctrl_ubicacionGeografica.value_cmb_upazila.Text = oApplicant.ADDRESSDISTRICTNAME
                'Me.ctrl_ubicacionGeografica.value_cmb_union.Text = oApplicant.ADDRESSCOUNTY
                'Me.ctrl_ubicacionGeografica.value_cmb_village.Text = oApplicant.ADDRESSDESCRIPTION

            End If

            Me.txt_cicty.Text = oApplicant.ADDRESSCITY

            'oApplicant.ADDRESSSTREET = Me.txt_direccion.Text
            'oApplicant.ADDRESSCOUNTRYREGIONID = Me.ctrl_ubicacionGeografica.value_cmb_country.Text
            'oApplicant.ADDRESSSTATE = Me.ctrl_ubicacionGeografica.value_cmb_district.Text
            'oApplicant.ADDRESSDISTRICTNAME = Me.ctrl_ubicacionGeografica.value_cmb_upazila.Text
            'oApplicant.ADDRESSCOUNTY = Me.ctrl_ubicacionGeografica.value_cmb_union.Text

            'oApplicant.ADDRESSDESCRIPTION = Me.ctrl_ubicacionGeografica.value_cmb_village.Text
            'oApplicant.ADDRESSCITY = Me.txt_cicty.Text

            chk_Active.Checked = If(oApplicant.ORGANIZATIONSTATUS, True, False)


        End Using

    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click

        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False

        If errSave = False Then
            'cnnME.Open()

            Using dbEntities As New dbRMS_JIEntities
                Dim oApplicant As New TA_ORGANIZATION_APP
                Dim idApplicant As Integer = Val(Me.lbl_IDapplicant.Text.Trim)

                If idApplicant > 0 Then
                    oApplicant = dbEntities.TA_ORGANIZATION_APP.Find(idApplicant)
                End If

                oApplicant.ID_PROGRAMA = Me.Session("E_IdPrograma").ToString()
                oApplicant.ID_ORGANIZATION_TYPE = cmb_tipo_organizacion.SelectedValue
                oApplicant.ORGANIZATIONNAME = Me.txt_nombreEjecutor.Text
                oApplicant.NAMEALIAS = Me.txt_NombreCorto.Text
                oApplicant.ORGANIZATIONNUMBER = Me.txt_nit.Text
                oApplicant.ORGANIZATIONPHONE = Me.txt_telefono_ejecutor.Text
                oApplicant.ORGANIZATIONEMAIL = Me.txt_email.Text

                oApplicant.PERSONFIRSTNAME = Me.txt_contact_name.Text
                oApplicant.PERSONLASTNAME = Me.txt_contact_lastname.Text
                oApplicant.PERSONID = Me.txt_cedula.Text
                oApplicant.PRIMARYCONTACTEMAIL = Me.txt_email2.Text
                oApplicant.PRIMARYCONTACTPHONE = Me.txt_telefono_representante.Text
                oApplicant.ORGANIZATIONREGDATE = Me.dt_fecha_inicio.SelectedDate

                oApplicant.ADDRESSSTREET = Me.txt_direccion.Text
                oApplicant.ADDRESSCOUNTRYREGIONID = Me.ctrl_ubicacionGeografica.value_cmb_country.Text
                'oApplicant.ADDRESSSTATE = Me.ctrl_ubicacionGeografica.value_cmb_district.Text
                'oApplicant.ADDRESSDISTRICTNAME = Me.ctrl_ubicacionGeografica.value_cmb_upazila.Text
                'oApplicant.ADDRESSCOUNTY = Me.ctrl_ubicacionGeografica.value_cmb_union.Text
                'oApplicant.ADDRESSDESCRIPTION = Me.ctrl_ubicacionGeografica.value_cmb_village.Text
                oApplicant.ADDRESSCITY = Me.txt_cicty.Text
                'oApplicant.ID_COUNTY = Me.ctrl_ubicacionGeografica.value_cmb_village.SelectedValue

                ''oEjecutor.billing_info = Me.txt_billin_info.Text

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

                If idApplicant > 0 Then
                    oApplicant.DATEUPDATED = Date.UtcNow
                    oApplicant.ID_USER_UPDATED = Me.Session("E_IdUser").ToString()
                Else
                    oApplicant.DATECREATED = Date.UtcNow
                    oApplicant.ID_USER_CREATED = Me.Session("E_IdUser").ToString()
                End If

                oApplicant.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)
                oApplicant.ORGANIZATIONSTATUS = If(chk_Active.Checked, True, False) 'Active

                If idApplicant > 0 Then
                    dbEntities.Entry(oApplicant).State = Entity.EntityState.Modified
                Else
                    dbEntities.TA_ORGANIZATION_APP.Add(oApplicant)
                End If

                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/RFP_/frm_Applicant"
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
        Me.Response.Redirect("~/RFP_/frm_Applicant")
    End Sub

End Class