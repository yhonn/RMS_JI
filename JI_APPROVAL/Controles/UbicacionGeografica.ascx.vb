Imports Telerik.Web.UI
Imports ly_SIME

Public Class UbicacionGeografica
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

    'Public Property value_cmb_corregimiento() As RadComboBox
    '    Get
    '        Return cmb_corregimiento
    '    End Get
    '    Set(ByVal value As RadComboBox)
    '        cmb_corregimiento = value
    '    End Set
    'End Property

    'Public Property value_cmb_vereda() As RadComboBox
    '    Get
    '        Return cmb_vereda
    '    End Get
    '    Set(ByVal value As RadComboBox)
    '        cmb_vereda = value
    '    End Set
    'End Property

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

                'Dim aaa = Me.cmb_municipio.SelectedValue
                Dim departametos = dbEntities.t_departamentos.Where(Function(p) p.id_programa = id_programa).OrderBy(Function(p) p.nombre_departamento).ToList()
                Dim municipios = New List(Of t_municipios)
                Dim corregimientos = New List(Of t_corregimientos)
                Dim veredas = New List(Of t_veredas)
                If Me.cmb_departamento.SelectedValue <> "" Then
                    Dim deptoID = Convert.ToInt32(Me.cmb_departamento.SelectedValue)
                    municipios = dbEntities.t_municipios.Where(Function(p) p.id_departamento = deptoID).OrderBy(Function(p) p.nombre_municipio).ToList()
                Else
                    municipios = departametos.FirstOrDefault().t_municipios.OrderBy(Function(p) p.nombre_municipio).ToList()
                End If

                If Me.cmb_municipio.SelectedValue <> "" Then
                    Dim mpioID = Convert.ToInt32(Me.cmb_municipio.SelectedValue)
                    corregimientos = dbEntities.t_corregimientos.Where(Function(p) p.id_municipio = mpioID).OrderBy(Function(p) p.corregimiento).ToList()
                    veredas = dbEntities.t_veredas.Where(Function(p) p.id_municipio = mpioID).OrderBy(Function(p) p.vereda).ToList()
                Else
                    corregimientos = municipios.FirstOrDefault().t_corregimientos.OrderBy(Function(p) p.corregimiento).ToList()
                    veredas = municipios.FirstOrDefault().t_veredas.OrderBy(Function(p) p.vereda).ToList()
                End If

                If Not String.IsNullOrEmpty(ListadoDepartamentos) Then
                    Dim listado = ListadoDepartamentos.Split(",")
                    departametos = departametos.Where(Function(p) listado.Contains(p.id_departamento)).ToList()
                    'municipios = departametos.FirstOrDefault().t_municipios.ToList()
                End If

                If Not String.IsNullOrEmpty(listadoMunicipios) Then
                    Dim listado = listadoMunicipios.Split(",")
                    municipios = municipios.Where(Function(p) listado.Contains(p.id_municipio)).ToList()
                End If

                If municipioDeptoId > 0 Then
                    municipios = municipios.Where(Function(p) p.id_departamento = municipioDeptoId).ToList()
                End If

                Me.cmb_departamento.DataSource = departametos
                Me.cmb_departamento.DataTextField = "nombre_departamento"
                Me.cmb_departamento.DataValueField = "id_departamento"
                Me.cmb_departamento.DataBind()

                Me.cmb_municipio.DataSource = municipios
                Me.cmb_municipio.DataTextField = "nombre_municipio"
                Me.cmb_municipio.DataValueField = "id_municipio"
                Me.cmb_municipio.DataBind()

                'Me.cmb_corregimiento.DataSource = corregimientos
                'Me.cmb_corregimiento.DataTextField = "corregimiento"
                'Me.cmb_corregimiento.DataValueField = "id_corregimiento"
                'Me.cmb_corregimiento.DataBind()
                'Me.cmb_corregimiento.Text = ""

                'Me.cmb_vereda.DataSource = veredas
                'Me.cmb_vereda.DataTextField = "vereda"
                'Me.cmb_vereda.DataValueField = "id_vereda"
                'Me.cmb_vereda.DataBind()
                'Me.cmb_vereda.Text = ""
            End Using

            LoadList()
        End If
    End Sub

    'Sub LoadList(Optional i_departamento = "", Optional id_subcounty = "", Optional id_parish = "")
    Sub LoadList(Optional i_departamento = "", Optional i_municipio = "", Optional i_corregimiento = "")
        If Not String.IsNullOrEmpty(i_departamento) Then
            Me.cmb_municipio.DataSource = clListados.get_t_municipios(i_departamento)
            Me.cmb_municipio.DataTextField = "nombre_municipio"
            Me.cmb_municipio.DataValueField = "id_municipio"
            Me.cmb_municipio.DataBind()
            Me.cmb_municipio.Text = ""
            'Me.cmb_vereda.Text = ""
            'Me.cmb_parish.Text = ""
            'Me.cmb_village.Text = ""
            LoadList()
        End If
        If Not String.IsNullOrEmpty(i_municipio) Then
            If Me.cmb_municipio.SelectedValue.Length > 0 Then
                'Me.cmb_corregimiento.DataSource = clListados.get_t_corregimientos(Me.cmb_municipio.SelectedValue)
                'Me.cmb_corregimiento.DataTextField = "corregimiento"
                'Me.cmb_corregimiento.DataValueField = "id_corregimiento"
                'Me.cmb_corregimiento.DataBind()
                'Me.cmb_corregimiento.Text = ""

                'Me.cmb_vereda.DataSource = clListados.get_t_veredas(Me.cmb_municipio.SelectedValue)
                'Me.cmb_vereda.DataTextField = "vereda"
                'Me.cmb_vereda.DataValueField = "id_vereda"
                'Me.cmb_vereda.DataBind()
                'Me.cmb_vereda.Text = ""
            End If
        End If

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
                Using dbEntities As New dbRMS_JIEntities
                    Dim municipiosList = dbEntities.tme_ficha_municipio.Where(Function(p) p.id_ficha_proyecto = id_ficha And p.t_municipios.id_departamento = Me.cmb_departamento.SelectedValue).Select(Function(p) p.id_municipio).ToList()
                    Dim municipios = dbEntities.t_municipios.Where(Function(p) municipiosList.Contains(p.id_municipio)).ToList()
                    Me.cmb_municipio.DataSource = municipios
                    Me.cmb_municipio.DataTextField = "nombre_municipio"
                    Me.cmb_municipio.DataValueField = "id_municipio"
                    Me.cmb_municipio.DataBind()
                End Using
            Else
                LoadList(i_departamento:=Me.cmb_departamento.SelectedValue)
            End If
        Else
            LoadList(i_departamento:=Me.cmb_departamento.SelectedValue)
        End If



    End Sub

    Protected Sub cmb_municipio_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_municipio.SelectedIndexChanged
        LoadList(i_municipio:=Me.cmb_municipio.SelectedValue)
    End Sub
    'Protected Sub cmb_corregimiento_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_corregimiento.SelectedIndexChanged
    '    LoadList(i_corregimiento:=Me.cmb_corregimiento.SelectedValue)
    'End Sub
End Class