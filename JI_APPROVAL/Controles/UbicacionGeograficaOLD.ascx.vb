Imports Telerik.Web.UI

Public Class UbicacionGeograficaOLD
    Inherits System.Web.UI.UserControl

    Dim clListados As New ly_SIME.CORE.cls_listados


    Public Property value_cmb_county() As RadComboBox
        Get
            Return cmb_county
        End Get
        Set(ByVal value As RadComboBox)
            cmb_county = value
        End Set
    End Property

    Public Property value_cmb_subcounty() As RadComboBox
        Get
            Return cmb_subcounty
        End Get
        Set(ByVal value As RadComboBox)
            cmb_subcounty = value
        End Set
    End Property

    Public Property value_cmb_parish() As RadComboBox
        Get
            Return cmb_parish
        End Get
        Set(ByVal value As RadComboBox)
            cmb_parish = value
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




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Me.cmb_county.DataSource = clListados.get_t_counties()
            Me.cmb_county.DataTextField = "nombre_county"
            Me.cmb_county.DataValueField = "id_county"
            Me.cmb_county.DataBind()
            LoadList()

        End If

    End Sub

    Sub LoadList(Optional id_subcounty = 0, Optional id_parish = 0)

        If Me.cmb_county.SelectedValue.Length > 0 Then
            Me.cmb_subcounty.DataSource = clListados.get_t_subcounties(Me.cmb_county.SelectedValue)
            Me.cmb_subcounty.DataTextField = "nombre_subcounty"
            Me.cmb_subcounty.DataValueField = "id_subcounty"
            Me.cmb_subcounty.DataBind()
            If id_subcounty > 0 Then
                Me.cmb_subcounty.SelectedValue = id_subcounty
            End If

            If Me.cmb_subcounty.SelectedValue.Length > 0 Then
                Me.cmb_parish.DataSourceID = ""
                Me.cmb_parish.DataSource = clListados.get_t_parishes(Me.cmb_subcounty.SelectedValue)
                Me.cmb_parish.DataTextField = "nombre_parish"
                Me.cmb_parish.DataValueField = "id_parish"
                Me.cmb_parish.DataBind()

                If id_parish > 0 Then
                    Me.cmb_parish.SelectedValue = id_parish
                End If

                If Me.cmb_parish.SelectedValue.Length > 0 Then
                    Me.cmb_village.DataSourceID = ""
                    Me.cmb_village.DataSource = clListados.get_t_villages(Me.cmb_parish.SelectedValue)
                    Me.cmb_village.DataTextField = "nombre_village"
                    Me.cmb_village.DataValueField = "id_village"
                    Me.cmb_village.DataBind()
                End If
            End If
        End If
    End Sub

    Protected Sub cmb_county_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_county.SelectedIndexChanged
        LoadList()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseTwo'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub

    Protected Sub cmb_subcounty_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_subcounty.SelectedIndexChanged
        LoadList(id_subcounty:=Me.cmb_subcounty.SelectedValue)
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseTwo'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub

    Protected Sub cmb_parish_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_parish.SelectedIndexChanged
        LoadList(id_subcounty:=Me.cmb_subcounty.SelectedValue, id_parish:=Me.cmb_parish.SelectedValue)
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseTwo'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub
End Class