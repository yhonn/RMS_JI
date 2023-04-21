Imports Telerik.Web.UI
Imports ly_SIME

Public Class UbicacionGeograficaLAC_OlD
    Inherits System.Web.UI.UserControl

    Dim clListados As New ly_SIME.CORE.cls_listados

    Private listadosDepartamentos As String
    Public Property ListadoDepartamentos() As String
        Get
            Return listadosDepartamentos
        End Get
        Set(ByVal value As String)
            listadosDepartamentos = value
        End Set
    End Property


    Private District_list As String
    Public Property list_OfDistrict() As String
        Get
            Return District_list
        End Get
        Set(ByVal value As String)
            District_list = value
        End Set
    End Property

    Private listadosMunicipios As String
    Public Property listadoMunicipios() As String
        Get
            Return listadosMunicipios
        End Get
        Set(ByVal value As String)
            listadosMunicipios = value
        End Set
    End Property

    Private municipiosDeptoId As Integer
    Public Property municipioDeptoId() As Integer
        Get
            Return municipiosDeptoId
        End Get
        Set(ByVal value As Integer)
            municipiosDeptoId = value
        End Set
    End Property

    Private listadosCorregimientos As String
    Public Property listadoCorregimientos() As String
        Get
            Return listadosCorregimientos
        End Get
        Set(ByVal value As String)
            listadosCorregimientos = value
        End Set
    End Property

    Public Property value_lbl_departamento() As Label
        Get
            Return lblt_departamento
        End Get
        Set(ByVal value As Label)
            lblt_departamento = value
        End Set
    End Property

    Public Property value_cmb_country() As RadComboBox
        Get
            Return cmb_country
        End Get
        Set(ByVal value As RadComboBox)
            cmb_country = value
        End Set
    End Property

    Public Property value_cmb_departamento() As RadComboBox
        Get
            Return cmb_departamento
        End Get
        Set(ByVal value As RadComboBox)
            cmb_departamento = value
        End Set
    End Property


    Public Property value_cmb_municipio() As RadComboBox
        Get
            Return cmb_municipio
        End Get
        Set(ByVal value As RadComboBox)
            cmb_municipio = value
        End Set
    End Property

    Public Property value_cmb_corregimiento() As RadComboBox
        Get
            Return cmb_corregimiento
        End Get
        Set(ByVal value As RadComboBox)
            cmb_corregimiento = value
        End Set
    End Property

    Public Property value_cmb_vereda() As RadComboBox
        Get
            Return cmb_vereda
        End Get
        Set(ByVal value As RadComboBox)
            cmb_vereda = value
        End Set
    End Property


    Public Property value_cmb_zone() As RadComboBox
        Get
            Return cmb_zone
        End Get
        Set(ByVal value As RadComboBox)
            cmb_zone = value
        End Set
    End Property



    Public Property value_cmb_division() As RadComboBox
        Get
            Return cmb_division
        End Get
        Set(ByVal value As RadComboBox)
            cmb_division = value
        End Set
    End Property

    'Public Property value_cmb_parish() As RadComboBox
    '    Get
    '        Return cmb_parish
    '    End Get
    '    Set(ByVal value As RadComboBox)
    '        cmb_parish = value
    '    End Set
    'End Property

    'Public Property value_cmb_village() As RadComboBox
    '    Get
    '        Return cmb_village
    '    End Get
    '    Set(ByVal value As RadComboBox)
    '        cmb_village = value
    '    End Set
    'End Property


    'Public Property grupo_div_ParishVillage() As HtmlGenericControl
    '    Get
    '        Return divParishVillage
    '    End Get
    '    Set(ByVal value As HtmlGenericControl)
    '        divParishVillage = value
    '    End Set
    'End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim id_programa = Convert.ToInt64(Me.Session("E_IDPrograma"))

            Using dbEntities As New dbRMS_LAC_Entities


                Dim countriesList = dbEntities.t_districts.Where(Function(p) p.id_programa = id_programa) _
                                         .Select(Function(p) p.id_pais).Distinct().ToList()

                ''Dim ListCountry As Integer() = countriesList.ToArray()

                Dim countries = dbEntities.t_pais.Where(Function(p) countriesList.Contains(p.id_pais)).OrderBy(Function(p) p.nombre_pais) _
                                            .Select(Function(p) _
                                                    New With {Key .id_pais = p.id_pais,
                                                               Key .nombre_pais = p.nombre_pais}).ToList()


                Dim zones = dbEntities.t_regiones.Where(Function(p) countriesList.Contains(p.id_pais)).OrderBy(Function(p) p.nombre_region) _
                                            .Select(Function(p) _
                                                    New With {Key .id_region = p.id_region,
                                                               Key .nombre_region = p.nombre_region}).ToList()

                Dim zoneList = zones.Select(Function(p) p.id_region).Distinct().ToList()


                Dim division = dbEntities.t_subregiones.Where(Function(p) zoneList.Contains(p.id_region)).OrderBy(Function(p) p.nombre_subregion) _
                                            .Select(Function(p) _
                                                    New With {Key .id_subregion = p.id_subregion,
                                                               Key .nombre_subregion = p.nombre_subregion}).ToList()


                'Dim countries = dbEntities.t_districts.Where(Function(p) p.id_programa = id_programa).Distinct().OrderBy(Function(p) p.t_pais.nombre_pais) _
                '                            .Select(Function(p) _
                '                                    New With {Key .id_pais = p.id_pais,
                '                                               Key .nombre_pais = p.t_pais.nombre_pais}).ToList()

                'Dim aaa = Me.cmb_municipio.SelectedValue
                'Dim departametos = dbEntities.t_departamentos.Where(Function(p) p.id_programa = id_programa).OrderBy(Function(p) p.nombre_departamento).ToList()

                'Dim departametos = New List(Of t_districts)
                'If Me.cmb_country.SelectedValue <> "" Then
                '    Dim countryID = Convert.ToInt32(Me.cmb_country.SelectedValue)
                '    departametos = dbEntities.t_districts.Where(Function(p) p.id_programa = id_programa And p.id_pais = countryID).OrderBy(Function(p) p.nombre_district).ToList()

                'Else
                '    Dim countryID = countries.FirstOrDefault.id_pais
                '    departametos = dbEntities.t_districts.Where(Function(p) p.id_programa = id_programa And p.id_pais = countryID).OrderBy(Function(p) p.nombre_district).ToList()
                'End If

                Dim departamentos As List(Of t_districts)
                Dim departamentoList As List(Of Integer)
                Dim divisionID As Integer
                Dim countryID As Integer


                If Me.cmb_division.SelectedValue <> "" Then

                    divisionID = Convert.ToInt32(Me.cmb_division.SelectedValue)
                    'departametos = dbEntities.t_districts.Where(Function(p) p.id_programa = id_programa And p.id_pais = countryID).OrderBy(Function(p) p.nombre_district).ToList()

                    departamentoList = (From div In dbEntities.tme_subregion_nivel_avance
                                        Join dist In dbEntities.t_districts On div.id_relacion Equals dist.id_district
                                        Where (dist.id_programa = id_programa And div.id_subregion = divisionID)
                                        Select dist.id_district).ToList()



                Else

                    countryID = countries.FirstOrDefault.id_pais
                    'departametos = dbEntities.t_districts.Where(Function(p) p.id_programa = id_programa And p.id_pais = countryID).OrderBy(Function(p) p.nombre_district).ToList()
                    divisionID = division.FirstOrDefault().id_subregion
                    departamentoList = (From div In dbEntities.tme_subregion_nivel_avance
                                        Join dist In dbEntities.t_districts On div.id_relacion Equals dist.id_district
                                        Where (dist.id_programa = id_programa And div.id_subregion = divisionID)
                                        Select dist.id_district).ToList()

                End If

                departamentos = dbEntities.t_districts.Where(Function(p) p.id_programa = id_programa And departamentoList.Contains(p.id_district)).OrderBy(Function(p) p.nombre_district).ToList()


                '' Dim municipios = New List(Of t_municipios)
                Dim municipios = New List(Of t_counties)
                'Dim corregimientos = New List(Of t_corregimientos)
                Dim corregimientos = New List(Of t_subcounties)
                '' Dim veredas = New List(Of t_veredas)
                Dim veredas = New List(Of t_parishes)

                If Me.cmb_departamento.SelectedValue <> "" Then
                    Dim deptoID = Convert.ToInt32(Me.cmb_departamento.SelectedValue)
                    ' municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = deptoID).OrderBy(Function(p) p.nombre_municipio).ToList()
                    municipios = dbEntities.t_counties.Where(Function(p) p.id_district = deptoID).OrderBy(Function(p) p.nombre_county).ToList()
                Else
                    ''municipios =  departametos.FirstOrDefault().t_municipios.OrderBy(Function(p) p.nombre_municipio).ToList()
                    municipios = departamentos.FirstOrDefault().t_counties.OrderBy(Function(p) p.nombre_county).ToList()
                End If

                If Me.cmb_municipio.SelectedValue <> "" Then
                    Dim mpioID = Convert.ToInt32(Me.cmb_municipio.SelectedValue)
                    ''corregimientos = dbEntities.t_corregimientos.Where(Function(p) p.id_municipio = mpioID).OrderBy(Function(p) p.corregimiento).ToList()
                    corregimientos = dbEntities.t_subcounties.Where(Function(p) p.id_subcounty = mpioID).OrderBy(Function(p) p.nombre_subcounty).ToList()
                Else
                    '' corregimientos = municipios.FirstOrDefault().t_corregimientos.OrderBy(Function(p) p.corregimiento).ToList()
                    corregimientos = municipios.FirstOrDefault().t_subcounties.OrderBy(Function(p) p.nombre_subcounty).ToList()
                End If

                If Me.cmb_corregimiento.SelectedValue <> "" Then
                    Dim corregimientoID = Convert.ToInt32(Me.cmb_corregimiento.SelectedValue)
                    veredas = dbEntities.t_parishes.Where(Function(p) p.id_subcounty = corregimientoID).OrderBy(Function(p) p.nombre_parish).ToList()
                Else
                    veredas = corregimientos.FirstOrDefault().t_parishes.OrderBy(Function(p) p.nombre_parish).ToList()
                End If

                If Not String.IsNullOrEmpty(District_list) Then
                    Dim listado = District_list.Split(",")
                    departamentos = departamentos.Where(Function(p) listado.Contains(p.id_district)).ToList()
                End If


                'If Not String.IsNullOrEmpty(ListadoDepartamentos) Then
                '    Dim listado = ListadoDepartamentos.Split(",")
                '    departametos = departametos.Where(Function(p) listado.Contains(p.id_departamento)).ToList()
                '    'municipios = departametos.FirstOrDefault().t_municipios.ToList()
                'End If

                'If Not String.IsNullOrEmpty(listadoMunicipios) Then
                '    Dim listado = listadoMunicipios.Split(",")
                '    municipios = municipios.Where(Function(p) listado.Contains(p.id_municipio)).ToList()
                'End If

                'If municipioDeptoId > 0 Then
                '    municipios = municipios.Where(Function(p) p.id_departamento = municipioDeptoId).ToList()
                'End If

                If municipioDeptoId > 0 Then
                    municipios = municipios.Where(Function(p) p.id_county = municipioDeptoId).ToList()
                End If

                Me.cmb_country.DataSource = countries
                Me.cmb_country.DataTextField = "nombre_pais"
                Me.cmb_country.DataValueField = "id_pais"
                Me.cmb_country.DataBind()

                Me.cmb_zone.DataSource = zones
                Me.cmb_zone.DataTextField = "nombre_region"
                Me.cmb_zone.DataValueField = "id_region"
                Me.cmb_zone.DataBind()

                Me.cmb_division.DataSource = division
                Me.cmb_division.DataTextField = "nombre_subregion"
                Me.cmb_division.DataValueField = "id_subregion"
                Me.cmb_division.DataBind()

                Me.cmb_departamento.DataSource = departamentos
                Me.cmb_departamento.DataTextField = "nombre_district"
                Me.cmb_departamento.DataValueField = "id_district"
                Me.cmb_departamento.DataBind()

                Me.cmb_municipio.DataSource = municipios
                Me.cmb_municipio.DataTextField = "nombre_county"
                Me.cmb_municipio.DataValueField = "id_county"
                Me.cmb_municipio.DataBind()

                Me.cmb_corregimiento.DataSource = corregimientos
                Me.cmb_corregimiento.DataTextField = "nombre_subcounty"
                Me.cmb_corregimiento.DataValueField = "id_subcounty"
                Me.cmb_corregimiento.DataBind()
                Me.cmb_corregimiento.Text = ""

                Me.cmb_vereda.DataSource = veredas
                Me.cmb_vereda.DataTextField = "nombre_parish"
                Me.cmb_vereda.DataValueField = "id_parish"
                Me.cmb_vereda.DataBind()
                Me.cmb_vereda.Text = ""
            End Using

            LoadList()
        End If
    End Sub

    'Sub LoadList(Optional i_departamento = "", Optional id_subcounty = "", Optional id_parish = "")
    Sub LoadList(Optional i_pais = "", Optional i_zone = "", Optional i_division = "", Optional i_district = "", Optional i_county = "", Optional i_subcounty = "", Optional i_parish = "")

        If Not String.IsNullOrEmpty(i_pais) Then

            Me.cmb_departamento.DataSource = clListados.get_t_district(i_pais)
            Me.cmb_departamento.DataTextField = "nombre_district"
            Me.cmb_departamento.DataValueField = "id_district"
            Me.cmb_departamento.DataBind()
            Me.cmb_departamento.Text = ""
            'Me.cmb_vereda.Text = ""
            'Me.cmb_parish.Text = ""
            'Me.cmb_village.Text = ""
            ''LoadList()

        End If

        If Not String.IsNullOrEmpty(i_zone) Then

            Me.cmb_division.DataSource = clListados.get_t_subregiones(i_zone) 'id_region
            Me.cmb_division.DataTextField = "nombre_region"
            Me.cmb_division.DataValueField = "id_region"
            Me.cmb_division.DataBind()
            Me.cmb_division.Text = ""
            'Me.cmb_vereda.Text = ""
            'Me.cmb_parish.Text = ""
            'Me.cmb_village.Text = ""
            ''LoadList()

        End If

        If Not String.IsNullOrEmpty(i_division) Then

            Me.cmb_departamento.DataSource = clListados.get_t_district2(i_division)
            Me.cmb_departamento.DataTextField = "nombre_district"
            Me.cmb_departamento.DataValueField = "id_district"
            Me.cmb_departamento.DataBind()
            Me.cmb_departamento.Text = ""
            'Me.cmb_vereda.Text = ""
            'Me.cmb_parish.Text = ""
            'Me.cmb_village.Text = ""
            ''LoadList()

        End If


        If Not String.IsNullOrEmpty(i_district) Then

            '' If Me.cmb_country.SelectedValue.Length > 0 Then
            Me.cmb_municipio.DataSource = clListados.get_t_counties(i_district)
            Me.cmb_municipio.DataTextField = "nombre_county"
            Me.cmb_municipio.DataValueField = "id_county"
            Me.cmb_municipio.DataBind()
            Me.cmb_municipio.Text = ""
            ''End If

        End If

        If Not String.IsNullOrEmpty(i_county) Then

            '' If Me.cmb_country.SelectedValue.Length > 0 Then

            '' Me.cmb_corregimiento.DataSource = clListados.get_t_subcounties(Me.cmb_municipio.SelectedValue)
            Me.cmb_corregimiento.DataSource = clListados.get_t_subcounties(i_county)
            Me.cmb_corregimiento.DataTextField = "nombre_subcounty"
            Me.cmb_corregimiento.DataValueField = "id_subcounty"
            Me.cmb_corregimiento.DataBind()
            Me.cmb_corregimiento.Text = ""

            ''End If

        End If

        If Not String.IsNullOrEmpty(i_subcounty) Then

            ''If Me.cmb_corregimiento.SelectedValue.Length > 0 Then

            ''Me.cmb_vereda.DataSource = clListados.get_t_parish(Me.cmb_corregimiento.SelectedValue)
            Me.cmb_vereda.DataSource = clListados.get_t_parish(i_subcounty)
            Me.cmb_vereda.DataTextField = "nombre_parish"
            Me.cmb_vereda.DataValueField = "id_parish"
            Me.cmb_vereda.DataBind()
            Me.cmb_vereda.Text = ""

            ''End If

        End If

        'If Not String.IsNullOrEmpty(i_departamento) Then
        '    Me.cmb_municipio.DataSource = clListados.get_t_municipios(i_departamento)
        '    Me.cmb_municipio.DataTextField = "nombre_municipio"
        '    Me.cmb_municipio.DataValueField = "id_municipio"
        '    Me.cmb_municipio.DataBind()
        '    Me.cmb_municipio.Text = ""
        '    'Me.cmb_vereda.Text = ""
        '    'Me.cmb_parish.Text = ""
        '    'Me.cmb_village.Text = ""
        '    LoadList()
        'End If
        'If Not String.IsNullOrEmpty(i_municipio) Then
        '    If Me.cmb_municipio.SelectedValue.Length > 0 Then
        '        Me.cmb_corregimiento.DataSource = clListados.get_t_corregimientos(Me.cmb_municipio.SelectedValue)
        '        Me.cmb_corregimiento.DataTextField = "corregimiento"
        '        Me.cmb_corregimiento.DataValueField = "id_corregimiento"
        '        Me.cmb_corregimiento.DataBind()
        '        Me.cmb_corregimiento.Text = ""

        '        Me.cmb_vereda.DataSource = clListados.get_t_veredas(Me.cmb_municipio.SelectedValue)
        '        Me.cmb_vereda.DataTextField = "vereda"
        '        Me.cmb_vereda.DataValueField = "id_vereda"
        '        Me.cmb_vereda.DataBind()
        '        Me.cmb_vereda.Text = ""
        '    End If
        'End If

        'If Me.cmb_corregimiento.SelectedValue.Length > 0 Then
        '    Me.cmb_vereda.DataSource = clListados.get_t_veredas(Me.cmb_corregimiento.SelectedValue)
        '    Me.cmb_vereda.DataTextField = "vereda"
        '    Me.cmb_vereda.DataValueField = "id_vereda"
        '    Me.cmb_vereda.DataBind()
        '    Me.cmb_vereda.Text = ""
        'End If
    End Sub





    'Protected Sub cmb_subcounty_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_vereda.SelectedIndexChanged
    '    LoadList(id_subcounty:=Me.cmb_vereda.SelectedValue)
    '    'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseTwo'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    'End Sub

    'Protected Sub cmb_parish_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_parish.SelectedIndexChanged
    '    LoadList(id_subcounty:=Me.cmb_vereda.SelectedValue, id_parish:=Me.cmb_parish.SelectedValue)
    '    'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseTwo'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    'End Sub

    Protected Sub cmb_departamento_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento.SelectedIndexChanged

        Dim fullPath = Request.Url.AbsolutePath
        Dim fileName = System.IO.Path.GetFileName(fullPath)
        If fileName = "frm_proyectosRegion" Or fileName = "frm_proyectosRegion.aspx" Then
            If Not Me.Request.QueryString("Id") Is Nothing Then
                Dim id_ficha = Me.Request.QueryString("Id")
                Using dbEntities As New dbRMS_LAC_Entities
                    Dim municipiosList = dbEntities.tme_ficha_municipio.Where(Function(p) p.id_ficha_proyecto = id_ficha And p.t_municipios.id_departamento = Me.cmb_departamento.SelectedValue).Select(Function(p) p.id_municipio).ToList()
                    Dim municipios = dbEntities.t_municipios.Where(Function(p) municipiosList.Contains(p.id_municipio)).ToList()
                    Me.cmb_municipio.DataSource = municipios
                    Me.cmb_municipio.DataTextField = "nombre_municipio"
                    Me.cmb_municipio.DataValueField = "id_municipio"
                    Me.cmb_municipio.DataBind()
                End Using
            Else
                LoadList(i_district:=Me.cmb_departamento.SelectedValue)
            End If
        Else
            LoadList(i_district:=Me.cmb_departamento.SelectedValue)
        End If


    End Sub

    Protected Sub cmb_municipio_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_municipio.SelectedIndexChanged
        LoadList(i_county:=Me.cmb_municipio.SelectedValue)
    End Sub
    Protected Sub cmb_corregimiento_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_corregimiento.SelectedIndexChanged
        LoadList(i_subcounty:=Me.cmb_corregimiento.SelectedValue)
    End Sub

    Private Sub cmb_country_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_country.SelectedIndexChanged
        LoadList(i_pais:=Me.cmb_country.SelectedValue)
    End Sub

    Private Sub cmb_zone_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_zone.SelectedIndexChanged
        LoadList(i_zone:=Me.cmb_zone.SelectedValue)
    End Sub

    Private Sub cmb_division_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_division.SelectedIndexChanged
        LoadList(i_division:=Me.cmb_division.SelectedValue)
    End Sub

End Class