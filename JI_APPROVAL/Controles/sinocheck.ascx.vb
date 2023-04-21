Imports Telerik.Web.UI

Public Class sinocheck
    Inherits System.Web.UI.UserControl

    Private checkeds As RadButton
    Public Property CheckedSI() As RadButton
        Get
            Return btnToggleSI
        End Get
        Set(ByVal value As RadButton)
            checkeds = value
        End Set
    End Property

    Private checkedn As RadButton
    Public Property CheckedNO() As RadButton
        Get
            Return btnToggleNO
        End Get
        Set(ByVal value As RadButton)
            checkedn = value
        End Set
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim state1 As New RadButtonToggleState("Yes")
        state1.Text = "Yes"
        state1.PrimaryIconCssClass = "rbToggleRadioChecked"
        Dim state2 As New RadButtonToggleState("Yes")
        state2.Text = "Yes"
        state2.PrimaryIconCssClass = "rbToggleRadio"

        Me.btnToggleSI.ToggleStates.Add(state1)
        Me.btnToggleSI.ToggleStates.Add(state2)

        Dim state3 As New RadButtonToggleState("No")
        state3.Text = "No"
        state3.PrimaryIconCssClass = "rbToggleRadioChecked"
        Dim state4 As New RadButtonToggleState("No")
        state4.Text = "No"
        state4.PrimaryIconCssClass = "rbToggleRadio"

        Me.btnToggleNO.ToggleStates.Add(state3)
        Me.btnToggleNO.ToggleStates.Add(state4)

    End Sub

    Protected Sub CustomValidator1_ServerValidate(source As Object, args As ServerValidateEventArgs)
        args.IsValid = btnToggleSI.Checked OrElse btnToggleNO.Checked
    End Sub

End Class