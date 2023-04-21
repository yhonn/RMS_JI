
Imports ly_SIME
Imports System.Globalization


Namespace APPROVAL


    Public Class cls_dUtil

        Public Property VARcultureINFO As CultureInfo
        Public Property offSET As Integer


        Public Sub New(ByVal cINFO As CultureInfo, Optional ByVal oSet As Integer = 0)

            VARcultureINFO = cINFO
            offSET = oSet

        End Sub

        Public Function set_DateFormat(ByVal dateINPUT As Date, ByVal formatSPECIFIER As String, Optional ByVal offsetINPUT As Integer = 0, Optional ByVal Print_UTC As Boolean = False) As String

            'dateUtil.set_DateFormat(Date,"G",offset)
            '  formatSPECIFIER 
            '  "d" short date, 04/10/2008
            '  "D" Long date,  Thursday, April 10, 2008 
            '  "f" full date short time, Thursday, April 10, 2008 6:30 AM
            '  "F" full date Long time,  Thursday, April 10, 2008 6:30:00 AM   
            '  "g" General date Short time,  04/10/2008 06:30    
            '  "G" General date Long time,  04/10/2008 06:30:00    
            '  "m" Month/day pattern. June 15 

            Dim vDate As Date
            Dim strUTC As String = ""
            Dim strSign As String = ""

            If offsetINPUT > 0 Then
                vDate = DateAdd(DateInterval.Hour, offsetINPUT, dateINPUT)
            Else
                vDate = DateAdd(DateInterval.Hour, offSET, dateINPUT)
            End If

            If Print_UTC Then

                If offSET > 0 Then
                    strSign = "+"
                Else
                    strSign = ""
                End If

                strUTC = String.Format(" UTC {0}{1}", strSign, offSET)

            End If

            set_DateFormat = vDate.ToString(formatSPECIFIER, VARcultureINFO) & strUTC


        End Function


        Public Function set_TimeFormat(dateINPUT As DateTime, Optional ByVal offsetINPUT As Integer = 0, Optional ByVal Print_UTC As Boolean = False) As String

            Dim vDate As Date
            Dim strUTC As String = ""
            Dim strSign As String = ""

            If offsetINPUT > 0 Then
                vDate = DateAdd(DateInterval.Hour, offsetINPUT, dateINPUT)
            Else
                vDate = DateAdd(DateInterval.Hour, offSET, dateINPUT)
            End If

            If Print_UTC Then

                If offSET > 0 Then
                    strSign = "+"
                Else
                    strSign = ""
                End If

                strUTC = String.Format(" UTC {0}{1}", strSign, offSET)

            End If

            'set_DateFormat = vDate.ToString(formatSPECIFIER, VARcultureINFO) & strUTC
            set_TimeFormat = vDate.ToShortTimeString & strUTC

        End Function


    End Class


End Namespace

