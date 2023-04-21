Imports Telerik.Web.UI
Imports ly_SIME
Imports ly_APPROVAL

Public Class UbicacionGeograficaBHA
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


    Private idVillage As Integer = 0
    Public Property AddVillage() As Integer
        Get
            Return idVillage
        End Get
        Set(ByVal value As Integer)
            idVillage = value
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
            Return lblt_District
        End Get
        Set(ByVal value As Label)
            lblt_District = value
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

    Public Property value_cmb_district() As RadComboBox
        Get
            Return cmb_district
        End Get
        Set(ByVal value As RadComboBox)
            cmb_district = value
        End Set
    End Property


    Public Property value_cmb_upazila() As RadComboBox
        Get
            Return cmb_upazila
        End Get
        Set(ByVal value As RadComboBox)
            cmb_upazila = value
        End Set
    End Property

    Public Property value_cmb_union() As RadComboBox
        Get
            Return cmb_union
        End Get
        Set(ByVal value As RadComboBox)
            cmb_union = value
        End Set
    End Property

    Public Property value_cmb_village() As RadComboBox
        Get
            Return cmb_village
        End Get
        Set(ByVal value As RadComboBox)
            cmb_village = value
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

            Using dbEntities As New dbRMS_JIEntities

                Dim villageID As Integer = 0
                Dim countryID As Integer = 0
                Dim zoneID As Integer = 0
                Dim divisionID As Integer = 0

                Dim oVillage As List(Of vw_t_village)

                If idVillage <> 0 Or Me.cmb_village.SelectedValue <> "" Then
                    villageID = idVillage
                    oVillage = dbEntities.vw_t_village.Where(Function(p) p.id_parish = villageID).ToList()
                Else
                    oVillage = Nothing
                End If

                Dim countriesList = dbEntities.t_districts.Where(Function(p) p.id_programa = id_programa) _
                                      .Select(Function(p) p.id_pais).Distinct().ToList()


                ''Dim ListCountry As Integer() = countriesList.ToArray()
                Dim countries = dbEntities.t_pais.Where(Function(p) countriesList.Contains(p.id_pais)).OrderBy(Function(p) p.nombre_pais) _
                                            .Select(Function(p) _
                                                    New With {Key .id_pais = p.id_pais,
                                                               Key .nombre_pais = p.nombre_pais}).ToList()

                Dim zones
                Dim zoneList As List(Of Integer) '= zones.Select(Function(p) p.id_region).Distinct().ToList()

                If Me.cmb_country.SelectedValue <> "" Or villageID <> 0 Then

                    countryID = If(Me.cmb_country.SelectedValue <> "", Convert.ToInt32(Me.cmb_country.SelectedValue), oVillage.FirstOrDefault.id_pais)

                    zones = dbEntities.t_regiones.Where(Function(p) p.id_pais = countryID).OrderBy(Function(p) p.nombre_region) _
                                            .Select(Function(p) _
                                                    New With {Key .id_region = p.id_region,
                                                               Key .nombre_region = p.nombre_region}).ToList()

                    zoneList = dbEntities.t_regiones.Where(Function(p) p.id_pais = countryID).OrderBy(Function(p) p.nombre_region) _
                                            .Select(Function(p) p.id_region).Distinct.ToList()

                Else

                    zones = dbEntities.t_regiones.Where(Function(p) countriesList.Contains(p.id_pais)).OrderBy(Function(p) p.nombre_region) _
                                             .Select(Function(p) _
                                                                New With {Key .id_region = p.id_region,
                                                                          Key .nombre_region = p.nombre_region}).ToList()

                    zoneList = dbEntities.t_regiones.Where(Function(p) countriesList.Contains(p.id_pais)).OrderBy(Function(p) p.nombre_region) _
                                              .Select(Function(p) p.id_region).Distinct.ToList()

                End If


                Dim division As Object

                If Me.cmb_zone.SelectedValue <> "" Or villageID <> 0 Then

                    zoneID = If(Me.cmb_zone.SelectedValue <> "", Convert.ToInt32(Me.cmb_zone.SelectedValue), oVillage.FirstOrDefault.id_region)

                    division = dbEntities.t_subregiones.Where(Function(p) p.id_region = zoneID).OrderBy(Function(p) p.nombre_subregion) _
                                         .Select(Function(p) _
                                                 New With {Key .id_subregion = p.id_subregion,
                                                            Key .nombre_subregion = p.nombre_subregion}).ToList()

                    divisionID = dbEntities.t_subregiones.Where(Function(p) p.id_region = zoneID).FirstOrDefault().id_subregion

                Else

                    division = dbEntities.t_subregiones.Where(Function(p) zoneList.Contains(p.id_region)).OrderBy(Function(p) p.nombre_subregion) _
                                           .Select(Function(p) _
                                                   New With {Key .id_subregion = p.id_subregion,
                                                              Key .nombre_subregion = p.nombre_subregion}).ToList()


                    divisionID = dbEntities.t_subregiones.Where(Function(p) zoneList.Contains(p.id_region)).FirstOrDefault().id_subregion

                End If




                Dim oDistrict As List(Of t_districts)
                Dim DistrictList As List(Of Integer)



                If Me.cmb_division.SelectedValue <> "" Or villageID <> 0 Then

                    divisionID = If(Me.cmb_division.SelectedValue <> "", Convert.ToInt32(Me.cmb_division.SelectedValue), oVillage.FirstOrDefault.id_subregion)
                    'departametos = dbEntities.t_districts.Where(Function(p) p.id_programa = id_programa And p.id_pais = countryID).OrderBy(Function(p) p.nombre_district).ToList()

                    DistrictList = (From div In dbEntities.tme_subregion_nivel_avance
                                    Join dist In dbEntities.t_districts On div.id_relacion Equals dist.id_district
                                    Where (dist.id_programa = id_programa And div.id_subregion = divisionID)
                                    Select dist.id_district).ToList()



                Else

                    ' countryID = countries.FirstOrDefault.id_pais
                    'departametos = dbEntities.t_districts.Where(Function(p) p.id_programa = id_programa And p.id_pais = countryID).OrderBy(Function(p) p.nombre_district).ToList()

                    DistrictList = (From div In dbEntities.tme_subregion_nivel_avance
                                    Join dist In dbEntities.t_districts On div.id_relacion Equals dist.id_district
                                    Where (dist.id_programa = id_programa And div.id_subregion = divisionID)
                                    Select dist.id_district).ToList()

                End If


                oDistrict = dbEntities.t_districts.Where(Function(p) p.id_programa = id_programa And DistrictList.Contains(p.id_district)).OrderBy(Function(p) p.nombre_district).ToList()


                '' Dim oCounties = New List(Of t_municipios)
                Dim oCounties = New List(Of t_counties)
                'Dim corregimientos = New List(Of t_corregimientos)
                Dim oSubCounties = New List(Of t_subcounties)

                '' Dim veredas = New List(Of t_veredas)
                Dim oParishes = New List(Of t_parishes)


                If Me.cmb_district.SelectedValue <> "" Or villageID <> 0 Then

                    Dim districtID = If(Me.cmb_district.SelectedValue <> "", Convert.ToInt32(Me.cmb_district.SelectedValue), oVillage.FirstOrDefault.id_district)
                    oCounties = dbEntities.t_counties.Where(Function(p) p.id_district = districtID).OrderBy(Function(p) p.nombre_county).ToList()

                Else

                    oCounties = oDistrict.FirstOrDefault().t_counties.OrderBy(Function(p) p.nombre_county).ToList()

                End If

                If Me.cmb_upazila.SelectedValue <> "" Or villageID <> 0 Then

                    Dim countyID = If(Me.cmb_upazila.SelectedValue <> "", Convert.ToInt32(Me.cmb_upazila.SelectedValue), oVillage.FirstOrDefault.id_county)
                    oSubCounties = dbEntities.t_subcounties.Where(Function(p) p.id_county = countyID).OrderBy(Function(p) p.nombre_subcounty).ToList()

                Else

                    oSubCounties = oCounties.FirstOrDefault().t_subcounties.OrderBy(Function(p) p.nombre_subcounty).ToList()

                End If

                If Me.cmb_union.SelectedValue <> "" Or villageID <> 0 Then
                    Dim subcountyID = If(Me.cmb_union.SelectedValue <> "", Convert.ToInt32(Me.cmb_union.SelectedValue), oVillage.FirstOrDefault.id_subcounty)
                    oParishes = dbEntities.t_parishes.Where(Function(p) p.id_subcounty = subcountyID).OrderBy(Function(p) p.nombre_parish).ToList()
                Else
                    oParishes = oSubCounties.FirstOrDefault().t_parishes.OrderBy(Function(p) p.nombre_parish).ToList()
                End If

                If Not String.IsNullOrEmpty(District_list) Then
                    Dim listado = District_list.Split(",")
                    oDistrict = oDistrict.Where(Function(p) listado.Contains(p.id_district)).ToList()
                End If

                'If Not String.IsNullOrEmpty(ListadoDepartamentos) Then
                '    Dim listado = ListadoDepartamentos.Split(",")
                '    departametos = departametos.Where(Function(p) listado.Contains(p.id_departamento)).ToList()
                '    'oCounties = departametos.FirstOrDefault().t_municipios.ToList()
                'End If

                'If Not String.IsNullOrEmpty(listadoMunicipios) Then
                '    Dim listado = listadoMunicipios.Split(",")
                '    oCounties = oCounties.Where(Function(p) listado.Contains(p.id_municipio)).ToList()
                'End If

                'If municipioDeptoId > 0 Then
                '    oCounties = oCounties.Where(Function(p) p.id_departamento = municipioDeptoId).ToList()
                'End If

                'If municipioDeptoId > 0 Then
                '    oCounties = oCounties.Where(Function(p) p.id_county = municipioDeptoId).ToList()
                'End If

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

                Me.cmb_district.DataSource = oDistrict
                Me.cmb_district.DataTextField = "nombre_district"
                Me.cmb_district.DataValueField = "id_district"
                Me.cmb_district.DataBind()
                Me.cmb_district.Text = ""

                Me.cmb_upazila.DataSource = oCounties
                Me.cmb_upazila.DataTextField = "nombre_county"
                Me.cmb_upazila.DataValueField = "id_county"
                Me.cmb_upazila.DataBind()
                Me.cmb_upazila.Text = ""

                Me.cmb_union.DataSource = oSubCounties
                Me.cmb_union.DataTextField = "nombre_subcounty"
                Me.cmb_union.DataValueField = "id_subcounty"
                Me.cmb_union.DataBind()
                Me.cmb_union.Text = ""

                Me.cmb_village.DataSource = oParishes
                Me.cmb_village.DataTextField = "nombre_parish"
                Me.cmb_village.DataValueField = "id_parish"
                Me.cmb_village.DataBind()
                Me.cmb_village.Text = ""

                If Me.cmb_country.SelectedValue = "" And villageID <> 0 Then
                    Me.cmb_country.SelectedValue = oVillage.FirstOrDefault.id_pais
                End If

                If Me.cmb_zone.SelectedValue = "" And villageID <> 0 Then
                    Me.cmb_zone.SelectedValue = oVillage.FirstOrDefault.id_region
                End If

                If Me.cmb_division.SelectedValue = "" And villageID <> 0 Then
                    Me.cmb_division.SelectedValue = oVillage.FirstOrDefault.id_subregion
                End If

                If Me.cmb_district.SelectedValue = "" And villageID <> 0 Then
                    Me.cmb_district.SelectedValue = oVillage.FirstOrDefault.id_district
                End If

                If Me.cmb_upazila.SelectedValue = "" And villageID <> 0 Then
                    Me.cmb_upazila.SelectedValue = oVillage.FirstOrDefault.id_county
                End If

                If Me.cmb_union.SelectedValue = "" And villageID <> 0 Then
                    Me.cmb_union.SelectedValue = oVillage.FirstOrDefault.id_subcounty
                End If

                If Me.cmb_village.SelectedValue = "" And villageID <> 0 Then
                    Me.cmb_village.SelectedValue = oVillage.FirstOrDefault.id_parish
                End If

            End Using

            LoadList()

        End If
        End Sub


    Sub LoadList(Optional i_pais = "", Optional i_zone = "", Optional i_division = "", Optional i_district = "", Optional i_county = "", Optional i_subcounty = "", Optional i_parish = "")

        If Not String.IsNullOrEmpty(i_pais) Then

            Me.cmb_district.DataSource = clListados.get_t_district(i_pais)
            Me.cmb_district.DataTextField = "nombre_district"
            Me.cmb_district.DataValueField = "id_district"
            Me.cmb_district.DataBind()
            Me.cmb_district.Text = ""

            ClearValues(Me.cmb_district)
            ClearValues(Me.cmb_division)
            ClearValues(Me.cmb_district)
            ClearValues(Me.cmb_upazila)
            ClearValues(Me.cmb_union)
            ClearValues(Me.cmb_village)

        End If

        If Not String.IsNullOrEmpty(i_zone) Then

                Me.cmb_division.DataSource = clListados.get_t_subregiones(i_zone) 'id_region
                Me.cmb_division.DataTextField = "nombre_subregion"
                Me.cmb_division.DataValueField = "id_subregion"
                Me.cmb_division.DataBind()
                Me.cmb_division.Text = ""

            'ClearValues(Me.cmb_division)
            ClearValues(Me.cmb_district)
            ClearValues(Me.cmb_upazila)
            ClearValues(Me.cmb_union)
            ClearValues(Me.cmb_village)


        End If

        If Not String.IsNullOrEmpty(i_division) Then

                Me.cmb_district.DataSource = clListados.get_t_district2(i_division)
                Me.cmb_district.DataTextField = "nombre_district"
                Me.cmb_district.DataValueField = "id_district"
                Me.cmb_district.DataBind()
                Me.cmb_district.Text = ""

            ' ClearValues(Me.cmb_district)
            ClearValues(Me.cmb_upazila)
            ClearValues(Me.cmb_union)
            ClearValues(Me.cmb_village)

        End If


        If Not String.IsNullOrEmpty(i_district) Then

            Me.cmb_upazila.DataSource = clListados.get_t_counties(i_district)
            Me.cmb_upazila.DataTextField = "nombre_county"
            Me.cmb_upazila.DataValueField = "id_county"
            Me.cmb_upazila.DataBind()
            Me.cmb_upazila.Text = ""

            'ClearValues(Me.cmb_upazila)
            ClearValues(Me.cmb_union)
            ClearValues(Me.cmb_village)

        End If

        If Not String.IsNullOrEmpty(i_county) Then

            Me.cmb_union.DataSource = clListados.get_t_subcounties(i_county)
            Me.cmb_union.DataTextField = "nombre_subcounty"
            Me.cmb_union.DataValueField = "id_subcounty"
            Me.cmb_union.DataBind()
            Me.cmb_union.Text = ""

            ' ClearValues(Me.cmb_union)
            ClearValues(Me.cmb_village)

        End If

        If Not String.IsNullOrEmpty(i_subcounty) Then

            Me.cmb_village.DataSource = clListados.get_t_parish(i_subcounty)
            Me.cmb_village.DataTextField = "nombre_parish"
            Me.cmb_village.DataValueField = "id_parish"
            Me.cmb_village.DataBind()
            Me.cmb_village.Text = ""

            'ClearValues(Me.cmb_village)

        End If

        'If Not String.IsNullOrEmpty(i_pais) Then
        'ElseIf Not String.IsNullOrEmpty(i_zone) Then
        'ElseIf Not String.IsNullOrEmpty(i_division) Then
        'ElseIf Not String.IsNullOrEmpty(i_district) Then
        'ElseIf Not String.IsNullOrEmpty(i_county) Then
        'ElseIf Not String.IsNullOrEmpty(i_subcounty) Then
        'End If

    End Sub


    'Protected Sub cmb_subcounty_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_village.SelectedIndexChanged
    '    LoadList(id_subcounty:=Me.cmb_village.SelectedValue)
    '    'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseTwo'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    'End Sub

    'Protected Sub cmb_parish_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_parish.SelectedIndexChanged
    '    LoadList(id_subcounty:=Me.cmb_village.SelectedValue, id_parish:=Me.cmb_parish.SelectedValue)
    '    'ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseTwo'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    'End Sub

    Protected Sub cmb_district_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_district.SelectedIndexChanged

            Dim fullPath = Request.Url.AbsolutePath
            Dim fileName = System.IO.Path.GetFileName(fullPath)
            If fileName = "frm_proyectosRegion" Or fileName = "frm_proyectosRegion.aspx" Then
                If Not Me.Request.QueryString("Id") Is Nothing Then
                    Dim id_ficha = Me.Request.QueryString("Id")
                    Using dbEntities As New dbRMS_JIEntities
                        Dim municipiosList = dbEntities.tme_ficha_municipio.Where(Function(p) p.id_ficha_proyecto = id_ficha And p.t_municipios.id_departamento = Me.cmb_district.SelectedValue).Select(Function(p) p.id_municipio).ToList()
                        Dim municipios = dbEntities.t_municipios.Where(Function(p) municipiosList.Contains(p.id_municipio)).ToList()
                        Me.cmb_upazila.DataSource = municipios
                        Me.cmb_upazila.DataTextField = "nombre_municipio"
                        Me.cmb_upazila.DataValueField = "id_municipio"
                        Me.cmb_upazila.DataBind()
                    End Using
                Else
                    LoadList(i_district:=Me.cmb_district.SelectedValue)
                End If
            Else
                LoadList(i_district:=Me.cmb_district.SelectedValue)
            End If


        End Sub

        Protected Sub cmb_upazila_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_upazila.SelectedIndexChanged
            LoadList(i_county:=Me.cmb_upazila.SelectedValue)
        End Sub
        Protected Sub cmb_union_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_union.SelectedIndexChanged
            LoadList(i_subcounty:=Me.cmb_union.SelectedValue)
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


    Function SetValues(ByVal combo As RadComboBox, ByVal datasource As Object, ByVal textField As String, ByVal valueField As String) As Boolean
        ClearValues(combo)
        combo.DataSource = datasource
        combo.DataTextField = textField
        combo.DataValueField = valueField
        combo.DataBind()
        Return True
    End Function

    Function ClearValues(ByVal combo As RadComboBox) As Boolean
        combo.ClearSelection()
        combo.Text = ""
        combo.Items.Clear()
        Return True
    End Function


End Class