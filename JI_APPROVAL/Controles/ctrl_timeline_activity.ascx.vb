
Imports ly_SIME

Public Class ctrl_timeline_activity
    Inherits System.Web.UI.UserControl


    Public cl_utl As New CORE.cls_util

    Private val_ID_ACTIVITY As Integer
    Public Property ID_ACTIVITY() As Integer
        Get
            Return val_ID_ACTIVITY
        End Get
        Set(ByVal value As Integer)
            val_ID_ACTIVITY = value
        End Set
    End Property
    Dim tbl_res As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Using dbEntities As New dbRMS_JIEntities

            Dim result As Object


            If val_ID_ACTIVITY <> 0 Then

                result = dbEntities.PROC_GET_TIMELINE(val_ID_ACTIVITY).ToList()
                tbl_res = cl_utl.ConvertToDataTable(result)
                Dim strTEXT As String = ""
                Dim strURL As String = ""

                For Each dtRow As DataRow In tbl_res.Rows

                    strURL = String.Format(dtRow("TIMELINE_URL"), val_ID_ACTIVITY.ToString)
                    dtRow("TIMELINE_URL") = strURL

                    strTEXT = String.Format(dtRow("TIMELINE_TEXT"), dtRow("TIMELINE_DATA1"), dtRow("TIMELINE_DATA2"), dtRow("TIMELINE_DATA3"), dtRow("TIMELINE_DATA4"))
                    dtRow("TIMELINE_TEXT") = strTEXT

                Next

                Dim tbl_ As DataTable = tbl_res.Copy

                Dim i As Integer = 0
                Dim dtR As DataRow

                For i = tbl_.Rows.Count - 1 To 0 Step -1

                    dtR = tbl_.Rows(i)
                    If ((dtR("ID_TIMELINE_STAGES_SNAP") Mod 2) = 0) Then
                        dtR.Delete()
                    End If

                Next
                tbl_.AcceptChanges()

                'For Each dtRow As DataRow In tbl_.Rows
                '    If ((dtRow("ID_TIMELINE_STAGES_SNAP") Mod 2) <> 0) Then
                '        dtRow.Delete()
                '    End If
                'Next
                'tbl_.AcceptChanges()

                rpt_timeline_odd.DataSource = tbl_
                rpt_timeline_odd.DataBind()

            End If

        End Using



    End Sub

    Private Sub rpt_timeline_odd_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpt_timeline_odd.ItemDataBound


        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ItemD As RepeaterItem
            ItemD = CType(e.Item, RepeaterItem)
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Dim rept_Messages As Repeater = ItemD.FindControl("rpt_timeline_whole")

            'Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
            'Dim query = dt.AsEnumerable
            '.Where(Function(dr) dr("column name").ToString = "something")
            '.GroupBy(Function(dr) dr("column name"))

            'Dim hd_id As HiddenField = ItemD.FindControl("hd_ID_TIME")
            Dim id_time As Integer = Convert.ToInt32(DataBinder.Eval(ItemD.DataItem, "ID_TIMELINE_STAGES_SNAP").ToString()) + 1

            Dim tbl_whole As New DataTable
            Dim strCond As String = String.Format("ID_TIMELINE_STAGES_SNAP={0}", id_time.ToString)


            If tbl_res.Select(strCond).Count > 0 Then
                tbl_whole = tbl_res.Select(strCond).CopyToDataTable()
            Else
                tbl_whole = Nothing
            End If


            'For Each dr As DataRow In tbl_res.Rows

            '    If dr("ID_TIMELINE_STAGES_SNAP") = id_time Then
            '        tbl_whole = dr.CopyToDataTable()
            '        Exit For
            '    End If

            'Next


            If Not IsNothing(tbl_whole) Then
                rept_Messages.DataSource = tbl_whole
                rept_Messages.DataBind()
            End If


        End If


    End Sub

    Public Shared Function Func_Unit(ByVal StartDate As DateTime, ByVal EndDate As DateTime) As String

        Dim vSeconds As Double
        Dim vMinutes As Double
        Dim vHours As Double
        Dim vDays As Double
        Dim vWeeks As Double
        Dim vMonths As Double
        Dim vYear As Double

        Dim strUnit As String
        Dim vUnit As Double


        vSeconds = DateDiff(DateInterval.Second, StartDate, EndDate)
        vMinutes = DateDiff(DateInterval.Minute, StartDate, EndDate)
        'vHours = DateDiff(DateInterval.Hour, StartDate, EndDate)
        'vDays = DateDiff(DateInterval.Day, StartDate, EndDate)

        vHours = vMinutes / 60
        vDays = vHours / 24
        vWeeks = vDays / 7
        vMonths = vDays / 30
        vYear = vDays / 365


        If vSeconds < 60 Then

            strUnit = " seconds"
            vUnit = Math.Round(vSeconds, 0, MidpointRounding.AwayFromZero)

        ElseIf vMinutes >= 1 And vMinutes < 60 Then

            If vMinutes > 1 Then
                strUnit = " minutes"
            Else
                strUnit = " minute"
            End If

            vUnit = Math.Round(vMinutes, 2, MidpointRounding.AwayFromZero)

        ElseIf vHours >= 1 And vHours < 24 Then

            If vHours > 1 Then
                strUnit = " hours"
            Else
                strUnit = " hour"
            End If

            vUnit = Math.Round(vHours, 2, MidpointRounding.AwayFromZero)

        ElseIf vWeeks < 1 Then

            vUnit = Math.Round(vDays, 2, MidpointRounding.AwayFromZero)

            If vDays > 1 Then
                strUnit = " days"
            Else
                strUnit = " day"
            End If

        ElseIf vMonths < 1 Then

            vUnit = Math.Round(vWeeks, 2, MidpointRounding.AwayFromZero)

            If vWeeks > 1 Then
                strUnit = " weeks"
            Else
                strUnit = " week"
            End If

        ElseIf vYear < 1 Then

            vUnit = Math.Round(vMonths, 2, MidpointRounding.AwayFromZero)

            If vMonths > 1 Then
                strUnit = " months"
            Else
                strUnit = " month"
            End If

        Else

            vUnit = Math.Round(vYear, 2, MidpointRounding.AwayFromZero)

            If vYear > 1 Then
                strUnit = " years"
            Else
                strUnit = " year"
            End If

        End If

        Func_Unit = String.Format("{0}&nbsp;{1}", vUnit, strUnit)

    End Function


    Public Function getFecha(dateIN As DateTime, strFormat As String, boolUTC As Boolean) As String

        Dim cl_user As ly_SIME.CORE.cls_user
        cl_user = Session.Item("clUser")

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
        Return cls_Solicitation.getFecha(dateIN, strFormat, boolUTC)

    End Function



    Public Function getHora(dateIN As DateTime) As String

        Dim cl_user As ly_SIME.CORE.cls_user
        cl_user = Session.Item("clUser")

        Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Dim cls_Solicitation As ly_APPROVAL.APPROVAL.cls_solicitations = New ly_APPROVAL.APPROVAL.cls_solicitations(idPrograma, cl_user)
        Return cls_Solicitation.getHora(dateIN)

    End Function

End Class