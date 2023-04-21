Imports ly_SIME
Imports Telerik.Web.UI
Imports System.Linq.Dynamic
Imports System.Web.Script.Serialization

Public Class frm_measurement_dashboard
    Inherits System.Web.UI.Page

    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim cl_user As ly_SIME.CORE.cls_user
    'Dim frmCODE As String = "SKILLS_DASH"
    Dim frmCODE As String = "REP_PROY"
    Dim controles As New ly_SIME.CORE.cls_controles
    Public chart As String = ""
    Public chartEndLine As String = ""
    Public showGraphic As String = ""
    Public people As String = ""
    Public men As Integer = 0
    Public women As Integer = 0
    Public menData As String = ""
    Public womenData As String = ""
    Public classLevelData As String = ""
    Dim helper As New ly_APPROVAL.APPROVAL.HelperMethods

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
        If Not Page.IsPostBack Then

            LoadLista()


            'filtroStar()

            'Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?rAll=1&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue
            'Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue


        End If
    End Sub

    Sub LoadLista()

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))

        helper.SetValues(Me.cmb_region, clListados.get_t_regiones(idPrograma), "nombre_region", "id_region")

        helper.SetValues(Me.cmb_subregion, clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue)), "nombre_subregion", "id_subregion")

        Using dbEntities As New dbRMS_LAC_Entities

            Dim id_districts = dbEntities.vw_ins_measurement_valuepertheme.Where(Function(p) p.id_region = Me.cmb_region.SelectedValue).Select(Function(p) p.id_district).Distinct().ToList()
            helper.SetValues(Me.cmb_district, dbEntities.vw_tme_districts.Where(Function(p) id_districts.Contains(p.id_district) And p.id_programa = 2).ToList(), "nombre_district", "id_district")

            Dim id_orgs = dbEntities.vw_ins_measurement_valuepertheme.Where(Function(p) p.id_district = Me.cmb_district.SelectedValue).Select(Function(p) p.id_organization).Distinct()

            Dim id_types = dbEntities.vw_ins_organization.Where(Function(p) id_orgs.Contains(p.id_organization)).Select(Function(p) p.id_organization_type).Distinct()
            helper.SetValues(Me.cmb_typeOrg, dbEntities.tme_organization_type.Where(Function(p) id_types.Contains(p.id_organization_type)).ToList(), "organization_type", "id_organization_type")

            Dim Schools = dbEntities.vw_ins_measurement_valuepertheme.Where(Function(p) p.id_organization_type = Me.cmb_typeOrg.SelectedValue).Select(Function(p) New With {
                                                                                 Key .name = p.name,
                                                                                 Key .id_organization = p.id_organization}).Distinct().OrderBy(Function(p) p.name).ToList()

            helper.SetValues(Me.cmb_school, Schools, "name", "id_organization")

            helper.SetValues(Me.cmb_TypeSurvey, dbEntities.ins_measurement_survey.Where(Function(P) P.id_programa = idPrograma).ToList(), "survey_name", "id_measurement_survey")

        End Using

    End Sub

    'Protected Sub cmb_region_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_region.SelectedIndexChanged

    '    helper.SetValues(Me.cmb_subregion, clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue)), "nombre_subregion", "id_subregion")

    '    Using dbEntities As New dbRMS_LAC_Entities

    '        Dim id_districts = dbEntities.vw_ins_measurement_valuepertheme.Where(Function(p) p.id_region = Me.cmb_region.SelectedValue).Select(Function(p) p.id_district).Distinct().ToList()
    '        helper.SetValues(Me.cmb_district, dbEntities.vw_tme_districts.Where(Function(p) id_districts.Contains(p.id_district) And p.id_programa = 2).ToList(), "nombre_district", "id_district")

    '        Dim id_orgs = dbEntities.vw_ins_measurement_valuepertheme.Where(Function(p) p.id_district = Me.cmb_district.SelectedValue).Select(Function(p) p.id_organization).Distinct()
    '        'Dim id_types = {1005, 1006, 1007}
    '        Dim id_types = dbEntities.vw_ins_organization.Where(Function(p) id_orgs.Contains(p.id_organization)).Select(Function(p) p.id_organization_type).Distinct()
    '        helper.SetValues(Me.cmb_typeOrg, dbEntities.tme_organization_type.Where(Function(p) id_types.Contains(p.id_organization_type)).ToList(), "organization_type", "id_organization_type")
    '        helper.SetValues(Me.cmb_school, dbEntities.vw_ins_organization.Where(Function(p) id_types.Contains(p.id_organization_type) And id_orgs.Contains(p.id_organization)).OrderBy(Function(p) p.name).ToList(), "name", "id_organization")

    '        Dim id_org = Convert.ToInt32(Me.cmb_subregion.SelectedValue)

    '        filtroStar()

    '        '    Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString
    '        '    Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString

    '    End Using
    'End Sub

    'Protected Sub cmb_district_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_district.SelectedIndexChanged

    '    Using dbEntities As New dbRMS_LAC_Entities

    '        'Dim id_types = {1005, 1006, 1007}
    '        Dim id_orgs = dbEntities.vw_ins_measurement_valuepertheme.Where(Function(p) p.id_district = Me.cmb_district.SelectedValue).Select(Function(p) p.id_organization).Distinct()

    '        Dim id_types = dbEntities.vw_ins_organization.Where(Function(p) id_orgs.Contains(p.id_organization)).Select(Function(p) p.id_organization_type).Distinct()
    '        helper.SetValues(Me.cmb_typeOrg, dbEntities.tme_organization_type.Where(Function(p) id_types.Contains(p.id_organization_type)).ToList(), "organization_type", "id_organization_type")
    '        helper.SetValues(Me.cmb_school, dbEntities.vw_ins_organization.Where(Function(p) id_types.Contains(p.id_organization_type) And id_orgs.Contains(p.id_organization)).OrderBy(Function(p) p.name).ToList(), "name", "id_organization")

    '        Dim id_org = Convert.ToInt32(Me.cmb_subregion.SelectedValue)

    '        filtroStar()

    '        'Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString
    '        'Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString

    '    End Using

    'End Sub

    'Protected Sub ckh_district_CheckedChanged(sender As Object, e As EventArgs) Handles chk_district.CheckedChanged
    '    Using dbEntities As New dbRMS_LAC_Entities

    '        If Me.chk_district.Checked Then
    '            chk_typeOrg.Checked = True
    '            chk_school.Checked = True
    '        Else
    '            chk_typeOrg.Checked = False
    '            chk_school.Checked = False
    '        End If

    '        filtroStar()

    '        'Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString
    '        'Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString

    '    End Using
    'End Sub

    'Protected Sub chk_TodosSub_CheckedChanged(sender As Object, e As EventArgs) Handles chk_TodosSub.CheckedChanged

    '    Using dbEntities As New dbRMS_LAC_Entities

    '        If chk_TodosSub.Checked Then
    '            chk_district.Checked = True
    '            chk_typeOrg.Checked = True
    '            chk_school.Checked = True
    '        Else
    '            chk_district.Checked = False
    '            chk_typeOrg.Checked = False
    '            chk_school.Checked = False
    '        End If

    '        filtroStar()

    '        'Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString
    '        'Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString

    '    End Using


    'End Sub

    'Protected Sub chk_school_CheckedChanged(sender As Object, e As EventArgs) Handles chk_school.CheckedChanged
    '    Using dbEntities As New dbRMS_LAC_Entities
    '        Dim id_org = Convert.ToInt32(Me.cmb_subregion.SelectedValue)

    '        filtroStar()
    '        ' Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?oAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString
    '    End Using
    'End Sub

    'Protected Sub cmb_school_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_school.SelectedIndexChanged
    '    Using dbEntities As New dbRMS_LAC_Entities
    '        Dim id_org = Convert.ToInt32(Me.cmb_school.SelectedValue)

    '        filtroStar()

    '        'Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?id=" & Me.cmb_region.SelectedValue & "&idOrg=" & Me.cmb_school.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString
    '        'Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?id=" & Me.cmb_region.SelectedValue & "&idOrg=" & Me.cmb_school.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString

    '    End Using
    'End Sub


    'Protected Sub chk_TodosR_CheckedChanged(sender As Object, e As EventArgs) Handles chk_TodosR.CheckedChanged
    '    Using dbEntities As New dbRMS_LAC_Entities

    '        Dim id_org = Convert.ToInt32(Me.cmb_subregion.SelectedValue)

    '        If chk_TodosR.Checked Then
    '            chk_district.Checked = True
    '            chk_TodosSub.Checked = True
    '            chk_typeOrg.Checked = True
    '            chk_school.Checked = True
    '        Else
    '            chk_district.Checked = False
    '            chk_TodosSub.Checked = False
    '            chk_typeOrg.Checked = False
    '            chk_school.Checked = False
    '        End If

    '        filtroStar()

    '        'Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?rAll=1&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue
    '        'Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue

    '    End Using
    'End Sub

    'Function filtroStar() As String

    '    Dim filtro = " 1 = 1 "
    '    Dim filtroPeople = " 1 = 1 "
    '    Dim ReturnCarr As String = "<br />"
    '    Dim strFilter = "Showing assessment result For: " & ReturnCarr

    '    'If Me.chk_TodosR.Checked Then

    '    '    filtro = " 1 = 1 "
    '    '    filtroPeople = "1 = 1 "
    '    '    strFilter &= " All Regions " & ReturnCarr
    '    '    strFilter &= " All Districts " & ReturnCarr
    '    '    strFilter &= " All kind of School " & ReturnCarr
    '    '    strFilter &= " All Schools included " & ReturnCarr


    '    'Else


    '    '    If Not Me.chk_district.Checked Then

    '    '        If Not Me.chk_typeOrg.Checked Then

    '    '            If Not Me.chk_school.Checked Then
    '    '                filtro = filtro & " AND id_organization = " & Me.cmb_school.SelectedValue
    '    '                filtroPeople = filtroPeople & " AND id_organization = " & Me.cmb_school.SelectedValue
    '    '                If Not String.IsNullOrEmpty(Me.cmb_school.SelectedItem.Text.Trim) Then
    '    '                    strFilter &= " The School: " & Me.cmb_school.SelectedItem.Text.Trim & ReturnCarr
    '    '                End If
    '    '            Else
    '    '                filtro = filtro & " AND id_region = " & Me.cmb_region.SelectedValue
    '    '                filtroPeople = filtroPeople & " AND id_region = " & Me.cmb_region.SelectedValue
    '    '                If Not String.IsNullOrEmpty(Me.cmb_region.SelectedItem.Text.Trim) Then
    '    '                    strFilter &= " Region: " & Me.cmb_region.SelectedItem.Text.Trim & ReturnCarr
    '    '                End If
    '    '            End If

    '    '        Else  'type Organization

    '    '            filtro = filtro & " AND id_organization_type = " & Me.cmb_typeOrg.SelectedValue
    '    '            filtroPeople = filtroPeople & " AND id_organization_type = " & Me.cmb_typeOrg.SelectedValue
    '    '            If Not String.IsNullOrEmpty(Me.cmb_typeOrg.SelectedItem.Text.Trim) Then
    '    '                strFilter &= " School Type: " & Me.cmb_typeOrg.SelectedItem.Text.Trim & ReturnCarr
    '    '            End If

    '    '        End If

    '    '    Else

    '    '        'District
    '    '        filtro = filtro & " AND id_district = " & Me.cmb_district.SelectedValue
    '    '        filtroPeople = filtroPeople & " AND id_district = " & Me.cmb_district.SelectedValue
    '    '        If Not String.IsNullOrEmpty(Me.cmb_district.SelectedItem.Text.Trim) Then
    '    '            strFilter &= " District: " & Me.cmb_district.SelectedItem.Text.Trim & ReturnCarr
    '    '        End If

    '    '    End If


    '    'End If


    '    'strFilter &= Me.cmb_TypeSurvey.SelectedItem.Text.Trim & "/" & rd_TypeAssessmemt.Text.Trim & ReturnCarr

    '    If Not Me.chk_TodosR.Checked Then
    '        filtro = filtro & " AND id_region = " & Me.cmb_region.SelectedValue
    '        filtroPeople = filtroPeople & " AND id_region = " & Me.cmb_region.SelectedValue
    '        If Not String.IsNullOrEmpty(Me.cmb_region.SelectedItem.Text.Trim) Then
    '            strFilter &= " Region: " & Me.cmb_region.SelectedItem.Text.Trim & ReturnCarr
    '        End If
    '    Else
    '        strFilter &= " All Regions " & ReturnCarr
    '    End If


    '    If Not Me.chk_district.Checked Then

    '        filtro = filtro & " AND id_district = " & Me.cmb_district.SelectedValue
    '        filtroPeople = filtroPeople & " AND id_district = " & Me.cmb_district.SelectedValue

    '        If Not String.IsNullOrEmpty(Me.cmb_district.SelectedItem.Text.Trim) Then
    '            strFilter &= " District: " & Me.cmb_district.SelectedItem.Text.Trim & ReturnCarr
    '        End If

    '    Else
    '        strFilter &= " All Districts " & ReturnCarr
    '    End If


    '    If Not Me.chk_typeOrg.Checked Then

    '        filtro = filtro & " AND id_organization_type = " & Me.cmb_typeOrg.SelectedValue
    '        filtroPeople = filtroPeople & " AND id_organization_type = " & Me.cmb_typeOrg.SelectedValue

    '        If Not String.IsNullOrEmpty(Me.cmb_typeOrg.SelectedItem.Text.Trim) Then
    '            strFilter &= " School Type: " & Me.cmb_typeOrg.SelectedItem.Text.Trim & ReturnCarr
    '        End If

    '    Else

    '        strFilter &= " All kind of School " & ReturnCarr

    '    End If


    '    If Not Me.chk_school.Checked Then

    '        filtro = filtro & " AND id_organization = " & Me.cmb_school.SelectedValue
    '        filtroPeople = filtroPeople & " AND id_organization = " & Me.cmb_school.SelectedValue

    '        If Not String.IsNullOrEmpty(Me.cmb_school.SelectedItem.Text.Trim) Then
    '            strFilter &= " The School: " & Me.cmb_school.SelectedItem.Text.Trim & ReturnCarr
    '        End If

    '    Else

    '        strFilter &= " All Schools included " & ReturnCarr

    '    End If

    '    ' lblFilter.Text = strFilter

    '    Using dbEntities As New dbRMS_LAC_Entities

    '        'rd_TypeAssessmemt.SelectedValue
    '        filtro = filtro & String.Format(" AND  baseline = {0} AND id_measurement_survey = {1} or id_measurement = 0  ", 1, Me.cmb_TypeSurvey.SelectedValue)
    '        'Dim lista = .ToList()
    '        Dim queryStar =
    '            From el In dbEntities.vw_ins_measurement_valuepertheme_school.Where(filtro) _
    '                .OrderBy(Function(p) p.id_measurement_theme).ThenBy(Function(p) p.order_theme)
    '            Group el By Key = New With {Key el.id_measurement_theme, Key el.theme_name, Key el.order_theme} Into Group
    '            Select New With {.theme = Key.theme_name,
    '                             .Value = Group.Average(Function(p) p.ValuePerTheme),
    '                             .id_measurement_theme = Key.id_measurement_theme,
    '                             .order_theme = Key.order_theme}

    '        If queryStar.Count > 5 Then



    '            For Each item In queryStar.OrderBy(Function(p) p.id_measurement_theme).ThenBy(Function(p) p.order_theme)
    '                Dim total = Convert.ToString(Math.Round(Convert.ToDouble(item.Value), 2))
    '                chart = chart + "{name:'" + item.theme + "', y: " + total + "}, "
    '            Next
    '            hdnStarInfo.Value = chart

    '            Dim numberParticipants = From a In dbEntities.vw_ins_measurement_detail.Where(filtroPeople)
    '                                     Group a By Key = a.id_sex_type Into Group
    '                                     Where Group.Count() > 1
    '                                     Select artnr = Key, numbersCount = Group.Count()

    '            men = numberParticipants.FirstOrDefault(Function(p) p.artnr = 2).numbersCount
    '            women = numberParticipants.FirstOrDefault(Function(p) p.artnr = 1).numbersCount

    '            people = "{""name"": 'Male', ""y"": " & men & "}, {""name"": 'Female', ""y"": " & women & "}"
    '            hdnPeople.Value = people

    '            Dim query =
    '                    From el In dbEntities.vw_ins_measurement_valuepertheme.Where(filtroPeople).OrderBy(Function(p) p.id_measurement_theme)
    '                    Group el By Key = New With {Key el.id_measurement_theme, Key el.theme_name, Key el.sex_type, Key el.id_sex_type} Into Group
    '                    Select New With {.key = Key.theme_name,
    '                                     .theme = Group.Average(Function(p) p.ValuePerTheme),
    '                                     .sex_type = Key.sex_type,
    '                                     .id_sex_type = Key.id_sex_type,
    '                                     .id_measurement_theme = Key.id_measurement_theme}

    '            For Each item In query.Where(Function(p) p.id_sex_type = 2).OrderBy(Function(p) p.id_measurement_theme)
    '                Dim total = Convert.ToString(item.key)
    '                menData = menData + Math.Round(item.theme.Value, 2).ToString() + ", "
    '            Next

    '            For Each item In query.Where(Function(p) p.id_sex_type = 1).OrderBy(Function(p) p.id_measurement_theme)
    '                Dim total = Convert.ToString(item.key)
    '                womenData = womenData + Math.Round(item.theme.Value, 2).ToString() + ", "
    '            Next

    '            Me.hdnManInfo.Value = menData
    '            Me.hdnWomanInfo.Value = womenData


    '            Dim queryClassLevel =
    '                    From el In dbEntities.vw_ins_measurement_valuepertheme.Where(filtroPeople).OrderBy(Function(p) p.id_class_level)
    '                    Group el By Key = New With {Key el.id_class_level, Key el.class_level_name, Key el.theme_name, Key el.id_measurement_theme} Into Group
    '                    Select New With {.theme_name = Key.theme_name,
    '                                     .ValuePerTheme = Group.Average(Function(p) p.ValuePerTheme),
    '                                     .class_level_name = Key.class_level_name,
    '                                     .id_class_level = Key.id_class_level,
    '                                     .id_measurement_theme = Key.id_measurement_theme}

    '            For Each item In queryClassLevel.OrderBy(Function(p) p.id_measurement_theme).GroupBy(Function(p) p.id_measurement_theme)
    '                classLevelData = classLevelData + "{ ""type"" : 'column', ""name"": '" + item.FirstOrDefault().theme_name + "', ""data"":["

    '                For Each detail In item.GroupBy(Function(p) p.id_class_level)
    '                    classLevelData = classLevelData + Math.Round(detail.Average(Function(p) p.ValuePerTheme).Value, 2).ToString() + ","
    '                Next
    '                classLevelData = classLevelData.TrimEnd(",")
    '                classLevelData = classLevelData + "]},"

    '            Next
    '            classLevelData = classLevelData.TrimEnd(",")
    '            Me.hdnClassLevel.Value = classLevelData

    '        Else

    '            Me.hdnStarInfo.Value = ""
    '            Me.hdnManInfo.Value = ""
    '            Me.hdnWomanInfo.Value = ""
    '            Me.hdnClassLevel.Value = ""
    '            Me.hdnPeople.Value = ""
    '        End If

    '    End Using


    '    Return filtro
    'End Function


    <Web.Services.WebMethod()>
    Public Shared Function web_filtroStar(ByVal IDgraph As Integer, ByVal idTypeSurvey As Integer, ByVal BaseLine As Integer, ByVal IdRegion As Integer, ByVal RegionAll As Boolean, ByVal idDistrict As Integer, ByVal DistrictALL As Boolean, ByVal idTypeOrg As Integer, ByVal TypeOrganizationALL As Boolean, ByVal idSchool As Integer, ByVal TypeSchoolALL As Boolean) As Object

        Dim filtro = " 1 = 1 "
        Dim filtroPeople = " 1 = 1 "
        'Dim ReturnCarr As String = "<br />"
        'Dim strFilter = "Showing assessment result For: " & ReturnCarr
        'strFilter &= Me.cmb_TypeSurvey.SelectedItem.Text.Trim & "/" & rd_TypeAssessmemt.Text.Trim & ReturnCarr

        Dim menData As String = ""
        Dim womenData As String = ""
        Dim chart As String = ""
        Dim people As String = ""
        Dim classLevelData As String = ""
        Dim men As Integer = 0
        Dim women As Integer = 0
        Dim serializer As New JavaScriptSerializer()
        Dim jsonGraphValue As String = "[]"

        If Not RegionAll Then
            filtro = filtro & " AND id_region = " & IdRegion
            filtroPeople = filtroPeople & " AND id_region = " & IdRegion
            'If Not String.IsNullOrEmpty(Me.cmb_region.SelectedItem.Text.Trim) Then
            'strFilter &= " Region: " & Me.cmb_region.SelectedItem.Text.Trim & ReturnCarr
            'End If
            'Else
            'strFilter &= " All Regions " & ReturnCarr
        End If


        If Not DistrictALL Then

            filtro = filtro & " AND id_district = " & idDistrict
            filtroPeople = filtroPeople & " AND id_district = " & idDistrict
            'If Not String.IsNullOrEmpty(Me.cmb_district.SelectedItem.Text.Trim) Then
            '    strFilter &= " District: " & Me.cmb_district.SelectedItem.Text.Trim & ReturnCarr
            'End If
            'Else
            'strFilter &= " All Districts " & ReturnCarr
        End If


        If Not TypeOrganizationALL Then

            filtro = filtro & " AND id_organization_type = " & idTypeOrg
            filtroPeople = filtroPeople & " AND id_organization_type = " & idTypeOrg

            'If Not String.IsNullOrEmpty(Me.cmb_typeOrg.SelectedItem.Text.Trim) Then
            '    strFilter &= " School Type: " & Me.cmb_typeOrg.SelectedItem.Text.Trim & ReturnCarr
            'End If

            'Else

            '    strFilter &= " All kind of School " & ReturnCarr

        End If


        If Not TypeSchoolALL Then

            filtro = filtro & " AND id_organization = " & idSchool
            filtroPeople = filtroPeople & " AND id_organization = " & idSchool

            '    If Not String.IsNullOrEmpty(Me.cmb_school.SelectedItem.Text.Trim) Then
            '        strFilter &= " The School: " & Me.cmb_school.SelectedItem.Text.Trim & ReturnCarr
            '    End If

            'Else

            '    strFilter &= " All Schools included " & ReturnCarr

        End If

        'lblFilter.Text = strFilter

        Using dbEntities As New dbRMS_LAC_Entities

            'BaseLine 1,2,3
            'AND  baseline = {0} 
            Dim filtroBaseLine As String = ""
            If BaseLine <> 3 Then 'Not data
                filtroBaseLine &= String.Format(" AND (baseline = {0} ) ", BaseLine)
            End If

            filtro &= String.Format(" {0} AND ( id_measurement_survey = {1} ) OR (id_measurement = 0 AND id_measurement_survey = {1} ) ", filtroBaseLine, idTypeSurvey)
            filtroPeople &= String.Format(" {0} AND ( id_measurement_survey = {1} ) ", filtroBaseLine, idTypeSurvey)

            'Dim lista = .ToList()
            Dim queryStar = From el In dbEntities.vw_ins_measurement_valuepertheme_school.Where(filtro) _
                    .OrderBy(Function(p) p.order_theme)
                            Group el By Key = New With {Key el.id_measurement_theme, Key el.theme_name, Key el.order_theme, Key el.baseline} Into Group
                            Select New With {.theme = Key.theme_name,
                                 .Value = Group.Average(Function(p) p.ValuePerTheme),
                                 .order_theme = Key.order_theme,
                                 .baseline = Key.baseline}

            'Dim queryStar =
            '    From el In dbEntities.vw_ins_measurement_valuepertheme_school.Where(filtro) _
            '        .OrderBy(Function(p) p.id_measurement_theme).ThenBy(Function(p) p.order_theme)
            '    Group el By Key = New With {Key el.id_measurement_theme, Key el.theme_name, Key el.order_theme} Into Group
            '    Select New With {.name = Key.theme_name,
            '                     .y = Group.Average(Function(p) p.ValuePerTheme)}

            If queryStar.Count > 5 Then

                'For Each item In queryStar.OrderBy(Function(p) p.id_measurement_theme).ThenBy(Function(p) p.order_theme)
                '    Dim total = Convert.ToString(Math.Round(Convert.ToDouble(item.Value), 2))
                '    chart = chart + "{name:'" + item.theme + "', y: " + total + "}, "
                'Next
                'hdnStarInfo.Value = chart
                'jsonStar = chart


                'If IDgraph = 1 Then 'Star 

                Dim StarGraphValues As Object
                Dim StarGraphBaseLine As Object
                Dim StarGraphEndLine As Object

                If BaseLine = 3 Then 'Base line, End line
                    'filtroBaseLine &= String.Format(" AND (baseline = {0} ) ", BaseLine)

                    StarGraphBaseLine = queryStar.Where(Function(p) p.baseline = 1 Or p.baseline = 0).OrderBy(Function(p) p.order_theme) _
                                            .Select(Function(p) New With {Key .name = p.theme,
                                                                              .y = p.Value}).ToList()

                    If StarGraphBaseLine.Count > 5 Then
                        jsonGraphValue = serializer.Serialize(StarGraphBaseLine)
                    Else
                        jsonGraphValue = "[]"
                    End If

                    StarGraphEndLine = queryStar.Where(Function(p) p.baseline = 2 Or p.baseline = 0).OrderBy(Function(p) p.order_theme) _
                                    .Select(Function(p) New With {Key .name = p.theme,
                                                                      .y = p.Value}).ToList()

                    If StarGraphEndLine.Count > 5 Then
                        jsonGraphValue &= "||" & serializer.Serialize(StarGraphEndLine)
                    Else
                        jsonGraphValue &= "||[]"
                    End If

                Else

                    StarGraphValues = queryStar.Where(Function(p) p.baseline = BaseLine Or p.baseline = 0).OrderBy(Function(p) p.order_theme) _
                               .Select(Function(p) New With {Key .name = p.theme,
                                                                 .y = p.Value}).ToList()

                    If BaseLine = 1 Then
                        jsonGraphValue = serializer.Serialize(StarGraphValues) & "||[]"
                    Else
                        jsonGraphValue = "[]||" & serializer.Serialize(StarGraphValues)
                    End If

                    'jsonGraphValue = serializer.Serialize(StarGraphValues) & "||[]"

                End If


                'End If


                'If IDgraph = 2 Then 'People

                Dim numberParticipants = From a In dbEntities.vw_ins_measurement_detail.Where(filtroPeople)
                                         Group a By Key = a.id_sex_type Into Group
                                         Where Group.Count() > 1
                                         Select artnr = Key, numbersCount = Group.Count()

                men = numberParticipants.FirstOrDefault(Function(p) p.artnr = 2).numbersCount
                women = numberParticipants.FirstOrDefault(Function(p) p.artnr = 1).numbersCount

                people = "{""name"": 'Male', ""y"": " & men & "}, {""name"": 'Female', ""y"": " & women & "}"

                'jsonGraphValue = serializer.Serialize(People)
                jsonGraphValue &= "||" & people


                ' End If


                'If IDgraph = 3 Then 'Gender

                'Dim query =
                '            From el In dbEntities.vw_ins_measurement_valuepertheme.Where(filtroPeople).OrderBy(Function(p) p.id_measurement_theme)
                '            Group el By Key = New With {Key el.order_theme, Key el.theme_name, Key el.sex_type, Key el.id_sex_type} Into Group
                '            Select New With {.key = Key.theme_name,
                '                             .theme = Group.Average(Function(p) p.ValuePerTheme),
                '                             .sex_type = Key.sex_type,
                '                             .id_sex_type = Key.id_sex_type,
                '                             .order_theme = Key.order_theme}

                'For Each item In query.Where(Function(p) p.id_sex_type = 2).OrderBy(Function(p) p.order_theme)
                '    Dim total = Convert.ToString(item.key)
                '    menData = menData + Math.Round(item.theme.Value, 2).ToString() + ", "
                'Next

                'For Each item In query.Where(Function(p) p.id_sex_type = 1).OrderBy(Function(p) p.order_theme)
                '    Dim total = Convert.ToString(item.key)
                '    womenData = womenData + Math.Round(item.theme.Value, 2).ToString() + ", "
                'Next

                'jsonGraphValue &= "||" & menData & "||" & womenData
                ''Me.hdnManInfo.Value = menData
                ''Me.hdnWomanInfo.Value = womenData

                Dim id_programa = 2
                Dim skill_assessment As CORE.cl_skill_assessment = New CORE.cl_skill_assessment(id_programa)

                Dim tbl_scoringBy_gender = skill_assessment.get_resultBy_gender(filtroPeople)

                menData = ""
                womenData = ""
                For Each item As DataRow In tbl_scoringBy_gender.Rows

                    If item("id_sex_type") = 1 Then
                        womenData &= Math.Round(item("ValuePerTheme"), 2).ToString() + ", "
                    Else
                        menData &= Math.Round(item("ValuePerTheme"), 2).ToString() + ", "
                    End If

                Next

                jsonGraphValue &= "||" & menData & "||" & womenData

                'End If

                'If IDgraph = 4 Then 'Class Level


                'Dim queryclasslevel = dbEntities.vw_ins_measurement_valuepertheme.Where(filtroPeople) _
                '                                  .GroupBy(Function(g) New With {g.id_class_level, g.class_level_name, g.id_measurement_theme, g.order_theme, g.theme_name}) _
                '                                  .Select(Function(p) New With {Key .id_class_level = p.FirstOrDefault.id_class_level,
                '                                                                    .class_level_name = p.FirstOrDefault.class_level_name,
                '                                                                    .id_measurement_theme = p.FirstOrDefault.id_measurement_theme,
                '                                                                    .order_theme = p.FirstOrDefault.order_theme,
                '                                                                    .theme_name = p.FirstOrDefault.theme_name,
                '                                                                    .value = p.Average(Function(k) k.ValuePerTheme)}) _
                '                                 .OrderBy(Function(o) o.order_theme).ThenBy(Function(t) t.id_class_level).ToList()


                'Dim ClassLevels = queryclasslevel.OrderBy(Function(o) o.id_class_level).Select(Function(p) New With {Key .class_name = p.class_level_name}).Distinct()

                ''Dim strClassLevels As String = "[{""name"":""Class Level"", ""data"": ["
                'Dim strClassLevels As String = "["

                'For Each ClassNames In ClassLevels
                '    strClassLevels &= """" & ClassNames.class_name & ""","
                'Next
                ''strClassLevels = strClassLevels.TrimEnd(",") & "]}]"
                'strClassLevels = strClassLevels.TrimEnd(",") & "]"

                'Dim idTheme As Integer = 0
                'classLevelData = ""
                'For Each itemClassLEvel In queryclasslevel

                '    If idTheme = 0 Then

                '        idTheme = itemClassLEvel.id_measurement_theme
                '        classLevelData = classLevelData + "{ ""type"" : 'column', ""name"": '" + itemClassLEvel.theme_name + "', ""data"":["
                '        classLevelData = classLevelData + Math.Round(Convert.ToDouble(itemClassLEvel.value), 2).ToString() + ","

                '    ElseIf idTheme <> itemClassLEvel.id_measurement_theme Then

                '        classLevelData = classLevelData.TrimEnd(",")
                '        classLevelData = classLevelData + "]},"
                '        idTheme = itemClassLEvel.id_measurement_theme
                '        classLevelData = classLevelData + "{ ""type"" : 'column', ""name"": '" + itemClassLEvel.theme_name + "', ""data"":["
                '        classLevelData = classLevelData + Math.Round(Convert.ToDouble(itemClassLEvel.value), 2).ToString() + ","

                '    Else

                '        classLevelData = classLevelData + Math.Round(Convert.ToDouble(itemClassLEvel.value), 2).ToString() + ","

                '    End If

                'Next

                'classLevelData = classLevelData.TrimEnd(",")
                'classLevelData = classLevelData + "]}"

                'jsonGraphValue &= "||" & classLevelData



                Dim tbl_scoringBy_Class = skill_assessment.get_resultBy_Class(filtroPeople)

                'Dim ClassLevels = From row In tbl_scoringBy_Class.AsEnumerable()
                '                  Select row.Field(Of String)("class_level_name") Distinct

                Dim ClassLevels = tbl_scoringBy_Class.DefaultView.ToTable(True, "class_level_name")

                ' Dim ClassLevels = ClassT.OrderBy(Function(o) o.id_class_level).Select(Function(p) New With {Key .class_name = p.class_level_name}).Distinct()

                'Dim strClassLevels As String = "[{""name"":""Class Level"", ""data"": ["
                Dim strClassLevels As String = "["

                For Each ClassNames As DataRow In ClassLevels.Rows
                    strClassLevels &= """" & ClassNames("class_level_name") & ""","
                Next
                'strClassLevels = strClassLevels.TrimEnd(",") & "]}]"
                strClassLevels = strClassLevels.TrimEnd(",") & "]"

                Dim idTheme As Integer = 0
                classLevelData = ""
                For Each itemClassLEvel In tbl_scoringBy_Class.Rows

                    If idTheme = 0 Then

                        idTheme = itemClassLEvel("id_measurement_theme")
                        classLevelData = classLevelData + "{ ""type"" : 'column', ""name"": '" + itemClassLEvel("theme_name") + "', ""data"":["
                        classLevelData = classLevelData + Math.Round(Convert.ToDouble(itemClassLEvel("ValuePerTheme")), 2).ToString() + ","

                    ElseIf idTheme <> itemClassLEvel("id_measurement_theme") Then

                        classLevelData = classLevelData.TrimEnd(",")
                        classLevelData = classLevelData + "]},"
                        idTheme = itemClassLEvel("id_measurement_theme")
                        classLevelData = classLevelData + "{ ""type"" : 'column', ""name"": '" + itemClassLEvel("theme_name") + "', ""data"":["
                        classLevelData = classLevelData + Math.Round(Convert.ToDouble(itemClassLEvel("ValuePerTheme")), 2).ToString() + ","

                    Else

                        classLevelData = classLevelData + Math.Round(Convert.ToDouble(itemClassLEvel("ValuePerTheme")), 2).ToString() + ","

                    End If

                Next

                classLevelData = classLevelData.TrimEnd(",")
                classLevelData = classLevelData + "]}"

                jsonGraphValue &= "||" & classLevelData



                Dim ValuesFilter = filtroPeople & " AND baseline <> 0 "
                Dim SchoolsNumber = dbEntities.vw_ins_measurement_valuepertheme_school.Where(ValuesFilter).Select(Function(p) New With {Key .id_organization = p.id_organization}).Distinct().Count()
                Dim AssessmentNumber = dbEntities.vw_ins_measurement_valuepertheme.Where(ValuesFilter).Select(Function(p) New With {Key .id_measurement_detail = p.id_measurement_detail}).Distinct().Count()
                Dim AssessmentBaseLine = dbEntities.vw_ins_measurement_valuepertheme.Where(ValuesFilter).Select(Function(p) New With {Key .id_measurement_detail = p.id_measurement_detail,
                                                                                                                                                 .BaseLine = p.baseline}).Where(Function(p) p.BaseLine = 1).Distinct().Count()
                Dim AssessmentEndLine = dbEntities.vw_ins_measurement_valuepertheme.Where(ValuesFilter).Select(Function(p) New With {Key .id_measurement_detail = p.id_measurement_detail,
                                                                                                                                                 .BaseLine = p.baseline}).Where(Function(p) p.BaseLine = 2).Distinct().Count()

                jsonGraphValue &= "||" & SchoolsNumber & "||" & AssessmentNumber & "||" & AssessmentBaseLine & "||" & AssessmentEndLine

                'End If


                Dim queryStarOptimal = dbEntities.vw_ins_measurement_valuepertheme_MAX.Where(Function(p) p.id_measurement_survey = idTypeSurvey) _
                                         .OrderBy(Function(p) p.order_theme).Select(Function(p) New With {Key .name = p.theme_name,
                                                                                                          .y = p.max_value}).ToList()

                jsonGraphValue &= "||" & serializer.Serialize(queryStarOptimal)

                jsonGraphValue &= "||" & strClassLevels

            End If

        End Using

        Return jsonGraphValue

    End Function

    <Web.Services.WebMethod()>
    Public Shared Function get_DataINFO(ByVal IDLevel As Integer, ByVal IdRegion As Integer, ByVal idDistrict As Integer, ByVal idTypeOrg As Integer, ByVal idTypeSurvey As Integer) As Object

        Using dbEntities As New dbRMS_LAC_Entities

            Dim serializer As New JavaScriptSerializer()
            Dim clListados As New ly_SIME.CORE.cls_listados

            Dim JsonResult As String
            'helper.SetValues(Me.cmb_subregion, clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue)), "nombre_subregion", "id_subregion")

            Dim JsonSubRegion As Object
            Dim JsonDistrict As Object
            Dim JsonOrgTypes As Object
            Dim JsonSchools As Object

            If IDLevel <= 1 Then
                Dim ListValues As List(Of t_subregiones) = dbEntities.t_subregiones.Where(Function(p) p.id_region = IdRegion).ToList()
                JsonSubRegion = ListValues.Select(Function(p) New With {
                                                            Key .Text = p.nombre_subregion,
                                                                .Value = p.id_subregion}).ToList()

            Else
                JsonSubRegion = "[]"
            End If

            If IDLevel <= 2 Then
                Dim IDdistricts = dbEntities.vw_ins_measurement_valuepertheme.Where(Function(p) p.id_region = IdRegion And p.id_measurement_survey = idTypeSurvey).Select(Function(p) p.id_district).Distinct().ToList()
                Dim ListDistrict As List(Of vw_tme_districts) = dbEntities.vw_tme_districts.Where(Function(p) IDdistricts.Contains(p.id_district) And p.id_programa = 2).ToList()
                JsonDistrict = ListDistrict.OrderBy(Function(p) p.id_district).Select(Function(p) New With {
                                                                        Key .Text = p.nombre_district,
                                                                             .Value = p.id_district}).ToList()

                idDistrict = ListDistrict.OrderBy(Function(p) p.id_district).FirstOrDefault().id_district

            Else
                JsonDistrict = "[]"
            End If


            If IDLevel <= 3 Then
                Dim IDorgs = dbEntities.vw_ins_measurement_valuepertheme.Where(Function(p) p.id_district = idDistrict And p.id_measurement_survey = idTypeSurvey).Select(Function(p) p.id_organization).Distinct()
                Dim IDtypes = dbEntities.vw_ins_organization.Where(Function(p) IDorgs.Contains(p.id_organization)).Select(Function(p) p.id_organization_type).Distinct()

                Dim ListOrgTypes As List(Of tme_organization_type) = dbEntities.tme_organization_type.Where(Function(p) IDtypes.Contains(p.id_organization_type)).OrderBy(Function(p) p.id_organization_type).ToList()
                JsonOrgTypes = ListOrgTypes.OrderBy(Function(p) p.id_organization_type).Select(Function(p) New With {
                                                            Key .Text = p.organization_type,
                                                                .Value = p.id_organization_type}).ToList()

                idTypeOrg = ListOrgTypes.OrderBy(Function(p) p.id_organization_type).FirstOrDefault().id_organization_type

            Else
                JsonOrgTypes = "[]"
            End If

            If IDLevel <= 4 Then

                JsonSchools = dbEntities.vw_ins_measurement_valuepertheme.Where(Function(p) p.id_organization_type = idTypeOrg And p.id_district = idDistrict).Select(Function(p) New With {
                                                                                 Key .Text = p.name,
                                                                                 Key .Value = p.id_organization}).Distinct().OrderBy(Function(p) p.Text).ToList()
            Else

                JsonSchools = "[]"

            End If

            'JsonSubRegion
            'JsonDistrict
            'JsonOrgTypes
            'JsonSchools

            JsonResult = serializer.Serialize(JsonSubRegion) & "||" & serializer.Serialize(JsonDistrict) & "||" & serializer.Serialize(JsonOrgTypes) & "||" & serializer.Serialize(JsonSchools)
            'filtroStar()

            '    Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString
            '    Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString

            Return JsonResult

        End Using



    End Function

    'Private Sub cmb_typeOrg_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_typeOrg.SelectedIndexChanged

    '    Using dbEntities As New dbRMS_LAC_Entities

    '        Dim id_type = Convert.ToInt32(cmb_typeOrg.SelectedValue)

    '        'Dim id_types = {1005, 1006, 1007}
    '        Dim id_orgs = dbEntities.vw_ins_measurement_valuepertheme.Where(Function(p) p.id_district = Me.cmb_district.SelectedValue And p.id_organization_type = id_type).Select(Function(p) p.id_organization)

    '        'Dim id_types = dbEntities.vw_ins_organization.Where(Function(p) id_orgs.Contains(p.id_organization)).Select(Function(p) p.id_organization_type).Distinct()
    '        'helper.SetValues(Me.cmb_typeOrg, dbEntities.tme_organization_type.Where(Function(p) id_types.Contains(p.id_organization_type)).ToList(), "organization_type", "id_organization_type")
    '        helper.SetValues(Me.cmb_school, dbEntities.vw_ins_organization.Where(Function(p) p.id_organization_type = id_type And id_orgs.Contains(p.id_organization)).OrderBy(Function(p) p.name).ToList(), "name", "id_organization")

    '        Dim id_org = Convert.ToInt32(Me.cmb_subregion.SelectedValue)
    '        filtroStar()

    '        'Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString
    '        'Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString

    '    End Using


    'End Sub

    'Private Sub chk_typeOrg_CheckedChanged(sender As Object, e As EventArgs) Handles chk_typeOrg.CheckedChanged
    '    Using dbEntities As New dbRMS_LAC_Entities
    '        Dim id_org = Convert.ToInt32(Me.cmb_subregion.SelectedValue)

    '        If chk_typeOrg.Checked Then
    '            chk_school.Checked = True
    '        Else
    '            chk_school.Checked = False
    '        End If

    '        filtroStar()
    '        'Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?oAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString
    '        'Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&idSubR=" & Me.cmb_subregion.SelectedValue & "&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue.ToString

    '    End Using
    'End Sub

    'Private Sub btn_filter_Click(sender As Object, e As EventArgs) Handles btn_filter.Click
    '    filtroStar()
    'End Sub

    '************************************************************************************************************************************************************
    '************************************************************************************************************************************************************
    'Private Sub cmb_TypeSurvey_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_TypeSurvey.SelectedIndexChanged
    '    filtroStar()
    '    Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?rAll=1&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue
    '    Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue
    'End Sub
    '************************************************************************************************************************************************************
    '************************************************************************************************************************************************************

    '************************************************************************************************************************************************************
    '************************************************************************************************************************************************************
    'Private Sub rd_TypeAssessmemt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rd_TypeAssessmemt.SelectedIndexChanged

    '    filtroStar()

    '    Me.btn_export.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesment?rAll=1&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue
    '    Me.btn_detail.NavigateUrl = "~/Reportes/Instrumentos/frm_reporteSkillsAssesmentDetail?rAll=1&flgTR=" & rd_TypeAssessmemt.SelectedValue.ToString & "&flgTA=" & Me.cmb_TypeSurvey.SelectedValue
    'End Sub
    '************************************************************************************************************************************************************
    '************************************************************************************************************************************************************


End Class