Imports Telerik.Web.UI

Namespace APPROVAL

    Public Class HelperMethods

        Function SetValues(ByVal combo As RadComboBox, ByVal datasource As Object, ByVal textField As String, ByVal valueField As String)
            ClearValues(combo)
            combo.DataSource = datasource
            combo.DataTextField = textField
            combo.DataValueField = valueField
            combo.DataBind()
            Return True
        End Function

        Function ClearValues(ByVal combo As RadComboBox)
            combo.ClearSelection()
            combo.Text = ""
            combo.Items.Clear()
            Return True
        End Function
    End Class


End Namespace