Imports Telerik.Web.UI

Public Class BeneficiarySearchTool
    Inherits System.Web.UI.UserControl

    Public Property value_txt_surname() As RadTextBox
        Get
            Return txt_surname
        End Get
        Set(ByVal value As RadTextBox)
            txt_surname = value
        End Set
    End Property

    Public Property value_txt_first_name() As RadTextBox
        Get
            Return txt_first_name
        End Get
        Set(ByVal value As RadTextBox)
            txt_first_name = value
        End Set
    End Property

    Public Property value_txt_mother_surname() As RadTextBox
        Get
            Return txt_mother_surname
        End Get
        Set(ByVal value As RadTextBox)
            txt_mother_surname = value
        End Set
    End Property

    Public Property value_txt_mother_firts_name() As RadTextBox
        Get
            Return txt_mother_firts_name
        End Get
        Set(ByVal value As RadTextBox)
            txt_mother_firts_name = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


End Class