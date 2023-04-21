Imports Telerik.Web.UI
Imports ly_SIME
Imports ly_APPROVAL
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Web.Script.Serialization

Public Class frm_DeliverableRes
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "DELIVERABLE_RES"
    Dim db As New dbRMS_JIEntities
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim totalC As Integer()

    Dim dtAPPROVALS As DataTable


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
                'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate_employee)
                'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_users)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then

            fill_Grid(True)
            Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)
            Me.curr_local.Value = sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol
            Me.curr_International.Value = "USD"

            Me.hd_show_this_tab.Value = 0

            Dim id_programa = Convert.ToInt32(Me.Session("E_IDprograma"))

            Using dbEntities As New dbRMS_JIEntities

                Me.cmb_mecanism.DataSourceID = ""
                Me.cmb_mecanism.DataSource = dbEntities.tme_mecanismo_contratacion.Where(Function(p) p.id_programa = id_programa).ToList()
                Me.cmb_mecanism.DataTextField = "nombre_mecanismo_contratacion"
                Me.cmb_mecanism.DataValueField = "id_mecanismo_contratacion"
                Me.cmb_mecanism.DataBind()
                'Me.cmb_mecanism.SelectedIndex = 1
                Me.cmb_mecanism.SelectedValue = 8 'GRANT

                Me.cmb_Sub_mecanism.DataSourceID = ""
                Me.cmb_Sub_mecanism.DataSource = (From SubM In dbEntities.tme_sub_mecanismo
                                                  Where SubM.id_mecanismo_contratacion = Me.cmb_mecanism.SelectedValue
                                                  Select New With {.Value = SubM.id_sub_mecanismo,
                                                                   .Text = SubM.nombre_sub_mecanismo}).ToList()

                Me.cmb_Sub_mecanism.DataTextField = "Text"
                Me.cmb_Sub_mecanism.DataValueField = "Value"
                Me.cmb_Sub_mecanism.DataBind()

            End Using

        End If
    End Sub


    Public Sub fill_Grid(ByVal booREbind As Boolean)

        Dim cls_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))

        Dim valMEC As Integer
        Dim valSubMec As Integer

        If Val(Me.cmb_mecanism.SelectedValue) > 0 Then
            valMEC = Me.cmb_mecanism.SelectedValue
        Else
            valMEC = -1
        End If

        If chk_allMecanism.Checked Then
            valMEC = -1
        End If

        If Val(Me.cmb_Sub_mecanism.SelectedValue) > 0 Then
            valSubMec = cmb_Sub_mecanism.SelectedValue
        Else
            valSubMec = -1
        End If

        If chk_allSubmecanims.Checked Then
            valSubMec = -1
        End If

        grd_cate.DataSource = cls_Deliverable.get_Deliverable_PEND(valMEC, valSubMec)


        If booREbind Then
            grd_cate.Rebind()
        End If


        If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("Organization"))) Then
            hideColumn("Organization")
        End If


        If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("id_deliverable_estado"))) Then
            hideColumn("id_deliverable_estado")
        End If

        If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("Activity"))) Then
            hideColumn("Activity")
        End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("id"))) Then
        '    hideColumn("id")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("nombre_usuario"))) Then
        '    hideColumn("nombre_usuario")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("apellidos_usuario"))) Then
        '    hideColumn("apellidos_usuario")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("worked_count"))) Then
        '    hideColumn("worked_count")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("salary"))) Then
        '    hideColumn("salary")
        'End If

        If Me.hd_show_this_tab.Value = 1 Then

            Me.hd_show_this_tab.Value = 0
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadTab()", True)

        End If

        Me.btnlk_Export.PostBackUrl = "frm_TemplateReportDeliv.aspx?idTR=1&vUs=" & Me.Session("E_IdUser").ToString & "&vMec=" & valMEC.ToString() & "&vSubMec=" & valSubMec.ToString()

    End Sub

    Public Sub hideColumn(strColumn)

        Me.grd_cate.MasterTableView.GetColumn(strColumn).Display = True
        Me.grd_cate.MasterTableView.GetColumn(strColumn).Visible = False

    End Sub

    'Public Function check_field(Optional tp As Integer = 0) As DataTable

    '    Dim id_type = If(tp = 0, Me.hd_tp.Value, tp)
    '    Dim cls_rh_employee As APPROVAL.cls_rh_employee = New APPROVAL.cls_rh_employee(Convert.ToInt32(Me.Session("E_IDprograma")))
    '    Dim tbl_fields As DataTable = cls_rh_employee.get_fields(id_type)
    '    check_field = tbl_fields

    'End Function
    <Web.Services.WebMethod()>
    Public Shared Function get_Grid_Data(ByVal idMec As Integer, ByVal idSubMec As Integer) As Object

        Dim JsonResult As String
        Dim JsonGridData As Object
        Dim serializer As New JavaScriptSerializer()
        Dim tbl_Data As DataTable

        Dim contextVar As HttpContext = HttpContext.Current
        Dim idPrograma As Integer = contextVar.Session.Item("E_IDPrograma")

        Dim cls_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(idPrograma)

        tbl_Data = cls_Deliverable.get_Deliverable_PEND(idMec, idSubMec)


        JsonGridData = (From Deliv In tbl_Data.AsEnumerable()
                        Select (New With {Key .Organizacion = Deliv.Field(Of String)("Organizacion"),
                                           Key .Actividad = Deliv.Field(Of String)("Actividad"),
                                           Key .No = Deliv.Field(Of Int32)("No"),
                                           Key .Entregable = Deliv.Field(Of String)("Entregable"),
                                           Key .fecha = Deliv.Field(Of DateTime)("fecha"),
                                           Key .id_deliverable_estado = Deliv.Field(Of Int32)("id_deliverable_estado"),
                                           Key .Estado = Deliv.Field(Of String)("Estado"),
                                           Key .D_Days = Deliv.Field(Of Int32)("D_Days"),
                                           Key .valor = Deliv.Field(Of Decimal)("valor"),
                                           Key .valorUSD = Deliv.Field(Of Decimal)("valorUSD")})).ToList()

        JsonResult = serializer.Serialize(JsonGridData)

        Return JsonResult
        'btnlk_Export.PostBackUrl = "~/HRM/frm_TemplateReport.aspx?idTR=2&vUs=0"

    End Function

    <Web.Services.WebMethod()>
    Public Shared Function get_subMEC(ByVal idMec As Integer) As Object

        Dim JsonResult As String
        Dim JsonSubMecanismo As Object
        Dim serializer As New JavaScriptSerializer()

        Using dbEntities As New dbRMS_JIEntities

            If Val(idMec) > 0 Then

                JsonSubMecanismo = (From SubM In dbEntities.tme_sub_mecanismo
                                    Where SubM.id_mecanismo_contratacion = idMec
                                    Select New With {.Value = SubM.id_sub_mecanismo,
                                                    .Text = SubM.nombre_sub_mecanismo}).ToList()

            Else

                JsonSubMecanismo = "[]"

            End If

            JsonResult = serializer.Serialize(JsonSubMecanismo)

        End Using

        Return JsonResult

    End Function



    <Web.Services.WebMethod()>
    Public Shared Function get_DeliverableID(ByVal vActivity As String, ByVal vDeliverableN As Integer) As Object

        Dim contextVar As HttpContext = HttpContext.Current
        Dim sesUser As ly_SIME.CORE.cls_user = CType(contextVar.Session.Item("clUser"), ly_SIME.CORE.cls_user)
        Dim idPrograma As Integer = contextVar.Session.Item("E_IDPrograma")

        Dim cl_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(idPrograma, sesUser) 'Just for YLA
        Dim JsonResult As String

        'JsonResult = String.Format("[{{""y"":""{0}""}}]", cl_Deliverable.get_DeliverableID(vActivity, vDeliverableN))
        JsonResult = cl_Deliverable.get_DeliverableID(vActivity, vDeliverableN).ToString()

        get_DeliverableID = JsonResult

    End Function

    <Web.Services.WebMethod()>
    Public Shared Function get_DataGraph(ByVal idPrograma As Integer, ByVal vCurrency As Integer, ByVal strCurrency As String, ByVal vMecanismo As Integer, ByVal vSubMecanismo As Integer) As Object

        Dim contextVar As HttpContext = HttpContext.Current
        Dim sesUser As ly_SIME.CORE.cls_user = CType(contextVar.Session.Item("clUser"), ly_SIME.CORE.cls_user)
        'Dim idPrograma As Integer = contextVar.Session.Item("E_IDPrograma")
        Dim cl_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(idPrograma, sesUser) 'Just for YLA
        Dim JsonResult As String
        Dim strColorA As String = "rgb(126,86,134)"
        Dim strColorB As String = "rgb(165,170,217)"
        Dim strColor As String = ""

        Dim tableResult = cl_Deliverable.get_Deliverables_values(1, "", "N", "", vMecanismo, vSubMecanismo)

        '{  type "column",
        '   name: "Non Govermental Organisation (NGO)",
        '   data: [{ "name": "FY1", "y": 37763.78, "drilldown": "FY1NGO" }, { "name": "FY2", "y": 0, "drilldown": "FY2NGO" }, { "name": "FY3", "y": 280351.21, "drilldown": "FY3NGO" }]
        '}

        Dim strSerie As String = "["
        Dim strDrillDown As String = "["
        Dim idType As String = "-1"
        Dim strDrill As String = ""
        Dim strSerie_name As String = ""
        Dim strValueField As String = If(vCurrency = 0, "valor", "valorUSD")
        Dim YearCounter As Integer = 0

        For Each dtRow As DataRow In tableResult.Rows

            strSerie_name = If(dtRow("allocated") = 0, "Obligated", "Disbursed")
            strColor = If(dtRow("allocated") = 0, strColorA, strColorB)

            If idType = "-1" Then

                idType = dtRow("allocated")
                strSerie &= String.Format(" {{ ""type"": ""column"", 
                                               ""name"":""{0}"", 
                                               ""color"":""{4}"", 
                                               ""data"": [ {{ ""name"": ""{1}"", ""y"": {2}, ""drilldown"": ""{3}"" }}, ", strSerie_name, dtRow("FY_2"), dtRow(strValueField), dtRow("FY_2") & "_" & strSerie_name, strColor)
                'strDrill &= det_DrillDown(2, dtRow("FY"), idType)

            Else
                If idType = dtRow("allocated") Then
                    strSerie &= String.Format(" {{ ""name"": ""{0}"", ""y"": {1}, ""drilldown"": ""{2}"" }}, ", dtRow("FY_2"), dtRow(strValueField), dtRow("FY_2") & "_" & strSerie_name)
                    'strDrill &= det_DrillDown(2, dtRow("FY"), idType)
                Else
                    strSerie = strSerie.Trim.TrimEnd(",") & "] }, " 'Closing the serie
                    strSerie &= String.Format(" {{ ""type"": ""column"", 
                                                  ""name"":""{0}"", 
                                                  ""color"":""{4}"", 
                                                  ""data"": [ {{ ""name"": ""{1}"", ""y"": {2}, ""drilldown"": ""{3}"" }}, ", strSerie_name, dtRow("FY_2"), dtRow(strValueField), dtRow("FY_2") & "_" & strSerie_name, strColor)
                    idType = dtRow("allocated")

                End If

                If dtRow("allocated") = 0 Then

                    YearCounter += 1
                End If

            End If

            If strDrill.Length > 1 Then
                strDrillDown &= strDrill '& ", "
            End If
            strDrill = ""

            'strDrill &= det_DrillDown(2, dtRow("FY"), idType, vCurrency, vMecanismo, vSubMecanismo)
            strDrill &= det_DrillDown(2, dtRow("FY_2"), idType, vCurrency, vMecanismo, vSubMecanismo)

        Next

        'strDrill &= det_DrillDown(2, idType) 'The LAst One
        If strDrill.Length > 1 Then
            strDrillDown &= strDrill '& ", "
        End If
        strDrill = ""

        strSerie = strSerie.Trim.TrimEnd(",")

        If Not strSerie.Trim = "[" Then
            strSerie &= "] }, "
        End If

        strDrillDown = strDrillDown.Trim.TrimEnd(",") & " ] "

        '{
        'type: 'spline',
        'name: 'YLA Contribution',
        'data: [91567.92, 315323.50, 1331890.74],
        'marker: {
        '   lineWidth: 2,
        '   lineColor: Highcharts.GetOptions().colors[3],
        '   fillColor: 'white'
        '  }
        '}

        '--***********************************************************************************************************************************
        '--****************************************************PROJECT FOUNDING***************************************************************
        '--***********************************************************************************************************************************

        Dim tbl_Budget As DataTable = cl_Deliverable.get_Budget_Apportes(idPrograma)

        tableResult = Nothing

        Dim strFundName As String = "Project Founding"
        If Not IsNothing(tbl_Budget) Then
            strFundName = tbl_Budget.Rows.Item(0).Item("nombre_aporte")
        End If

        tableResult = cl_Deliverable.get_Tot_Funding(vMecanismo, vSubMecanismo)
        Dim strTittle As String = String.Format(" {1} ({0})", strCurrency, strFundName)

        strSerie &= String.Format("{{ ""type"": ""spline"", 
                                      ""name"": ""{0}"",   
                                      ""marker"": {{
                                                      ""lineWidth"": 2,                                                       
                                                      ""fillColor"": ""white""
                                                    }}, 
                                      ""color"": ""#FC472B"",                                   
                                      ""data"": [ ", strTittle)

        '""lineColor"": Highcharts.getOptions().colors[3],

        Dim str_ProjVal As String = ""

        Dim i = 0

        If tableResult.Rows.Count > 0 Then
            For i = 0 To YearCounter
                str_ProjVal &= tableResult.Rows.Item(i).Item(strValueField) & ", "
            Next
        Else
            For Each dtRow As DataRow In tableResult.Rows
                str_ProjVal &= dtRow(strValueField) & ", "
            Next
        End If
        'For Each dtRow As DataRow In tableResult.Rows
        '    str_ProjVal &= dtRow(strValueField) & ", "
        'Next
        str_ProjVal = str_ProjVal.Trim.TrimEnd(",")

        strSerie &= str_ProjVal.Trim & "] },"

        '--***********************************************************************************************************************************
        '--****************************************************SOURCE OF FOUNDING***************************************************************
        '--***********************************************************************************************************************************

        tableResult = Nothing
        tableResult = cl_Deliverable.get_Tot_Deliverables(vMecanismo, vSubMecanismo)

        strSerie &= String.Format("{{  ""type"": ""pie"", 
                                       ""name"":""Executed Funds ({0})"", 
                                       ""center"": [100, 60],
                                       ""size"": 110,
                                       ""showInLegend"": false,
                                       ""dataLabels"": {{
                                          ""enabled"": false
                                            }},
                                        ""data"": [ ", strCurrency)

        '{
        ' type: 'pie',
        ' name: 'Total Funding',
        ' data: [{  name: 'Private Funding', y: 13906564.21},
        '        { name: 'Project Funding',  y: 1738782.16 }, 
        '        { name: 'Public Funding',  y: 119.47 }],
        'center: [100, 60],
        'size: 120,
        'showInLegend: false,
        'dataLabels: {
        '  enabled: false
        ' }

        '}

        Dim str_SourceData As String = ""

        Dim TotDisbursed As Double = 0.0
        Dim TotObligated As Double = 0.0
        Dim TotBudget As Double = 0.0
        Dim PivotValue As Double = 0.0

        If tableResult.Rows.Count > 0 Then

            For Each dtRow As DataRow In tableResult.Rows

                If dtRow("allocated") = 0 Then
                    TotObligated = dtRow(strValueField)
                Else
                    TotDisbursed = dtRow(strValueField)
                End If

            Next

            TotObligated = TotObligated - TotDisbursed

            For Each dtRow As DataRow In tableResult.Rows
                strSerie_name = If(dtRow("allocated") = 0, "Obligated", "Disbursed")
                PivotValue = If(dtRow("allocated") = 0, TotObligated, TotDisbursed)
                str_SourceData &= String.Format("{{ ""name"": ""{0}"", ""y"": {1} }}, ", strSerie_name, PivotValue)
            Next

            'If (vCurrency = 0, "valor", "valorUSD") 
            If (vCurrency = 0) Then 'local currency
                If Not IsNothing(tbl_Budget) Then
                    'strFundName = tbl_Budget.Rows.Item(0).Item("nombre_aporte")
                    TotBudget = Math.Round(tbl_Budget.Rows.Item(0).Item("bud_amount") * tbl_Budget.Rows.Item(0).Item("exchange_rate"), 0, MidpointRounding.AwayFromZero)
                End If
            Else
                If Not IsNothing(tbl_Budget) Then
                    TotBudget = Math.Round(tbl_Budget.Rows.Item(0).Item("bud_amount"), 0, MidpointRounding.AwayFromZero)
                End If
            End If

            TotBudget = TotBudget - (TotDisbursed + TotObligated)
            str_SourceData &= String.Format("{{ ""name"": ""{0}"", ""y"": {1} }}, ", strFundName, TotBudget)
        Else

            strSerie_name = "None"

            If Not IsNothing(tbl_Budget) Then
                strSerie_name = strFundName
                If (vCurrency = 0) Then 'local currency
                    If Not IsNothing(tbl_Budget) Then
                        'strFundName = tbl_Budget.Rows.Item(0).Item("nombre_aporte")
                        TotBudget = Math.Round(tbl_Budget.Rows.Item(0).Item("bud_amount") * tbl_Budget.Rows.Item(0).Item("exchange_rate"), 0, MidpointRounding.AwayFromZero)
                    End If
                Else
                    If Not IsNothing(tbl_Budget) Then
                        TotBudget = Math.Round(tbl_Budget.Rows.Item(0).Item("bud_amount"), 0, MidpointRounding.AwayFromZero)
                    End If
                End If

                str_SourceData &= String.Format("{{ ""name"": ""{0}"", ""y"": {1} }}, ", strFundName, TotBudget)

            Else

                str_SourceData &= String.Format("{{ ""name"": ""{0}"", ""y"": {1} }}, ", strSerie_name, 0.0)

            End If


        End If



        str_SourceData = str_SourceData.Trim.TrimEnd(",")
        str_SourceData = str_SourceData.Trim & "] }" '******************Close Serie

        strSerie &= str_SourceData.Trim & "] " '*************The total serie*******************

        '****************************************************************************************************************************************
        '****************************************************************************************************************************************



        '****************************************************************************************************************************************
        '****************************************************************************************************************************************

        JsonResult = strSerie & "||" & strDrillDown
        'JsonResult = strDrillDown

        Return JsonResult

    End Function


    Public Shared Function det_DrillDown(ByVal idType_ As Integer, ByVal strFY As String, ByVal strSerie As String, ByVal vCurrency As Integer, ByVal varMec As Integer, ByVal varSubM As Integer) As String

        Dim contextVar As HttpContext = HttpContext.Current
        Dim sesUser As ly_SIME.CORE.cls_user = CType(contextVar.Session.Item("clUser"), ly_SIME.CORE.cls_user)
        Dim idPrograma As Integer = CType(contextVar.Session.Item("E_IDPrograma"), Integer)
        Dim cl_Deliverable As APPROVAL.clss_Deliverable = New APPROVAL.clss_Deliverable(idPrograma, sesUser) 'Just for YLA

        Dim JsonResult As String
        Dim tableResult = cl_Deliverable.get_Deliverables_values(idType_, strFY, "N", strSerie, varMec, varSubM)
        Dim strDrillDown As String = ""
        Dim strDrillDownPIV As String = ""
        Dim strData As String = ""

        Dim idRecord As String = "-1"
        Dim idRecordPIV As String = "0"
        Dim strSerie_name As String = ""
        Dim strValueField As String = If(vCurrency = 0, "valor", "valorUSD")
        Dim strColorA As String = "rgb(126,86,134)"
        Dim strColorB As String = "rgb(165,170,217)"

        '{                               
        'name: 'FY1-Non Govermental Organisation (NGO)',
        'id: 'FY1NGO',
        'type: 'column',
        'data: [
        '                                            ['Caïo Shea Butter', 0],                                            
        '                                            ['Mbale District Local Government', 3646.95],
        '                                            ['KadAfrica Ltd', 280123.15]                                           

        '                                      ]
        '                            }

        '{  type "column",
        '   name: "Non Govermental Organisation (NGO)",
        '   data: [{ "name": "FY1", "y": 37763.78, "drilldown": "FY1NGO" }, { "name": "FY2", "y": 0, "drilldown": "FY2NGO" }, { "name": "FY3", "y": 280351.21, "drilldown": "FY3NGO" }]
        '}

        If tableResult.Rows.Count = 0 Then
            JsonResult = ""
        Else

            If idType_ = 2 Then 'Month

                For Each dtRow As DataRow In tableResult.Rows

                    strSerie_name = If(dtRow("allocated") = 0, "Obligated", "Disbursed")

                    If idRecord = "-1" Then

                        idRecord = dtRow("allocated")
                        strDrillDown &= String.Format(" {{ ""type"": ""column"", 
                                                            ""name"":""{0}"", 
                                                            ""id"": ""{4}"",
                                                            ""data"": [ {{ ""name"": ""{1}"", ""y"": {2}, ""drilldown"": ""{3}"" }}, ", strSerie_name, dtRow("M"), dtRow(strValueField), dtRow("M") & "_" & strSerie_name, dtRow("FY_2") & "_" & strSerie_name)

                    Else

                        If idRecord = dtRow("allocated") Then
                            strDrillDown &= String.Format(" {{ ""name"": ""{0}"", ""y"": {1}, ""drilldown"": ""{2}"" }}, ", dtRow("M"), dtRow(strValueField), dtRow("M") & "_" & strSerie_name)
                        Else

                            strDrillDown = strDrillDown.Trim.TrimEnd(",") & "]}, " 'Closing the serie
                            strDrillDown &= String.Format(" {{ ""type"": ""column"", 
                                                            ""name"":""{0}"", 
                                                            ""id"": ""{4}"",
                                                            ""data"": [ {{ ""name"": ""{1}"", ""y"": {2}, ""drilldown"": ""{3}"" }}, ", strSerie_name, dtRow("M"), dtRow(strValueField), dtRow("M") & "_" & strSerie_name, dtRow("FY_2") & "_" & strSerie_name)
                            idRecord = dtRow("allocated")

                        End If

                    End If

                    strDrillDownPIV &= det_DrillDown(3, dtRow("M"), idRecord, vCurrency, varMec, varSubM) ', get the Drilldown

                Next

                strDrillDown = strDrillDown.Trim.TrimEnd(",") & "]}, " 'Closing the serie

            ElseIf idType_ = 3 Then 'Implementer

                For Each dtRow As DataRow In tableResult.Rows

                    strSerie_name = If(dtRow("allocated") = 0, "Obligated", "Disbursed")

                    If idRecord = "-1" Then

                        'dtRow("id_ejecutor")
                        idRecord = dtRow("allocated")
                        strDrillDown &= String.Format(" {{ ""type"": ""column"", 
                                                            ""name"":""{0}"", 
                                                            ""id"": ""{4}"",
                                                            ""data"": [ {{ ""name"": ""{1}"", ""y"": {2}, ""drilldown"": ""{3}"" }}, ", strSerie_name, dtRow("nombre_ejecutor"), dtRow(strValueField), "EJEC_" & dtRow("id_ejecutor") & "_" & strSerie_name, dtRow("M") & "_" & strSerie_name)
                    Else
                        If idRecord = dtRow("allocated") Then
                            strDrillDown &= String.Format(" {{ ""name"": ""{0}"", ""y"": {1}, ""drilldown"": ""{2}"" }}, ", dtRow("nombre_ejecutor"), dtRow(strValueField), "EJEC_" & dtRow("id_ejecutor") & "_" & strSerie_name)
                        Else

                            'strDrillDownPIV &= det_DrillDown(4,  idRecord)

                            strDrillDown = strDrillDown.Trim.TrimEnd(",") & "]}, " 'Closing the serie
                            strDrillDown &= String.Format(" {{ ""type"": ""column"", 
                                                            ""name"":""{0}"", 
                                                            ""id"": ""{4}"",
                                                            ""data"": [ {{ ""name"": ""{1}"", ""y"": {2}, ""drilldown"": ""{3}"" }}, ", strSerie_name, dtRow("nombre_ejecutor"), dtRow(strValueField), "EJEC_" & dtRow("id_ejecutor") & "_" & strSerie_name, dtRow("M") & "_" & strSerie_name)
                            idRecord = dtRow("allocated")

                        End If
                    End If

                    strDrillDownPIV &= det_DrillDown(4, dtRow("id_ejecutor"), idRecord, vCurrency, varMec, varSubM) ', get the Drilldown

                Next

                strDrillDown = strDrillDown.Trim.TrimEnd(",") & "]}, " 'Closing the serie


            ElseIf idType_ = 4 Then 'Activity

                For Each dtRow As DataRow In tableResult.Rows

                    strSerie_name = If(dtRow("allocated") = 0, "Obligated", "Disbursed")

                    If idRecord = "-1" Then

                        'dtRow("id_ejecutor")
                        idRecord = dtRow("allocated")

                        strDrillDown &= String.Format(" {{ ""type"": ""column"", 
                                                            ""name"":""{0}"", 
                                                            ""id"": ""{4}"",
                                                            ""data"": [ {{ ""name"": ""{1}"", ""y"": {2}, ""drilldown"": ""{3}"" }}, ", strSerie_name, dtRow("codigo_SAPME"), dtRow(strValueField), "PROY_" & dtRow("id_ficha_proyecto") & "_" & strSerie_name, "EJEC_" & dtRow("id_ejecutor") & "_" & strSerie_name)
                    Else
                        If idRecord = dtRow("allocated") Then
                            strDrillDown &= String.Format(" {{ ""name"": ""{0}"", ""y"": {1}, ""drilldown"": ""{2}"" }}, ", dtRow("codigo_SAPME"), dtRow(strValueField), "PROY_" & dtRow("id_ficha_proyecto") & "_" & strSerie_name)
                        Else

                            '**********strDrillDownPIV &= det_DrillDown(4, idRecord)

                            strDrillDown = strDrillDown.Trim.TrimEnd(",") & "]}, " 'Closing the serie
                            strDrillDown &= String.Format(" {{ ""type"": ""column"", 
                                                            ""name"":""{0}"", 
                                                            ""id"": ""{4}"",
                                                            ""data"": [ {{ ""name"": ""{1}"", ""y"": {2}, ""drilldown"": ""{3}"" }}, ", strSerie_name, dtRow("codigo_SAPME"), dtRow(strValueField), "PROY_" & dtRow("id_ficha_proyecto") & "_" & strSerie_name, "EJEC_" & dtRow("id_ejecutor") & "_" & strSerie_name)
                            idRecord = dtRow("allocated")

                        End If
                    End If

                    strDrillDownPIV &= det_DrillDown(5, dtRow("id_ficha_proyecto"), idRecord, vCurrency, varMec, varSubM) ', get the Drilldown

                Next

                '*******************strDrillDownPIV &= det_DrillDown(4, idRecord) 'The Last One

                strDrillDown = strDrillDown.Trim.TrimEnd(",") & "]}, " 'Closing the serie

            ElseIf idType_ = 5 Then 'Deliverable

                For Each dtRow As DataRow In tableResult.Rows

                    strSerie_name = If(dtRow("allocated") = 0, "Obligated", "Disbursed")

                    If idRecord = "-1" Then

                        'dtRow("id_ejecutor")
                        idRecord = dtRow("allocated")

                        strDrillDown &= String.Format(" {{ ""type"": ""areaspline"", 
                                                            ""name"":""{0}"", 
                                                            ""id"": ""{4}"",
                                                            ""data"": [ {{ ""name"": ""{1}"", ""y"": {2} }}, ", strSerie_name, dtRow("MonthYear"), dtRow(strValueField), "true", "PROY_" & dtRow("id_ficha_proyecto") & "_" & strSerie_name)
                    Else
                        If idRecord = dtRow("allocated") Then
                            strDrillDown &= String.Format(" {{ ""name"": ""{0}"", ""y"": {1} }}, ", dtRow("MonthYear"), dtRow(strValueField), "true")
                        Else

                            strDrillDown = strDrillDown.Trim.TrimEnd(",") & "]}, " 'Closing the serie
                            strDrillDown &= String.Format(" {{ ""type"": ""column"", 
                                                            ""name"":""{0}"", 
                                                            ""id"": ""{4}"",
                                                            ""data"": [ {{ ""name"": ""{1}"", ""y"": {2} }}, ", strSerie_name, dtRow("MonthYear"), dtRow(strValueField), "true", "PROY_" & dtRow("id_ficha_proyecto") & "_" & strSerie_name)
                            idRecord = dtRow("allocated")

                        End If
                    End If

                    'strDrillDownPIV &= det_DrillDown(5, dtRow("id_ficha_proyecto"), idRecord) ', what else

                Next

                '*******************strDrillDownPIV &= det_DrillDown(4, idRecord) 'The Last One

                strDrillDown = strDrillDown.Trim.TrimEnd(",") & "]}, " 'Closing the serie

            End If

        End If

        strDrillDown &= strDrillDownPIV

        JsonResult = strDrillDown
        'strDrillDown &= strData.Trim & " ]}" ??? the last One
        det_DrillDown = JsonResult

    End Function



    Private Sub grd_cate_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_cate.ItemDataBound


        If (TypeOf e.Item Is GridGroupFooterItem) Then
            Dim GroupfooterItem As GridGroupFooterItem = CType(e.Item, GridGroupFooterItem)
            'For Each dtRow As DataRow In check_field().Rows

            Dim strArrContent As String() = GroupfooterItem("Amount").Text.Split(":")

            If strArrContent.Length > 1 Then

                GroupfooterItem("Amount").Text = "Total: " + GroupfooterItem("Amount").Text.ToString().Split(":")(1).ToString("N2")
                GroupfooterItem("Amount").Style.Add("font-weight", "bold")

                GroupfooterItem("AmountUSD").Text = "TotalUSD: " + GroupfooterItem("AmountUSD").Text.ToString().Split(":")(1).ToString("N2")
                GroupfooterItem("AmountUSD").Style.Add("font-weight", "bold")


            End If

            'Next
        End If

        If (TypeOf e.Item Is GridFooterItem) Then

            Dim footerItem As GridFooterItem = CType(e.Item, GridFooterItem)
            ' For Each dtRow As DataRow In check_field().Rows
            Dim strArrContent As String() = footerItem("Amount").Text.Split(":")

            If strArrContent.Length > 1 Then

                footerItem("Amount").Text = footerItem("Amount").Text.ToString().Split(":")(1).ToString()
                footerItem("Amount").Style.Add("font-weight", "bold")

                footerItem("AmountUSD").Text = "TotalUSD: " + footerItem("AmountUSD").Text.ToString().Split(":")(1).ToString("N2")
                footerItem("AmountUSD").Style.Add("font-weight", "bold")

            End If


            'Next

        End If

    End Sub

    Private Sub grd_cate_PreRender(sender As Object, e As EventArgs) Handles grd_cate.PreRender

        If grd_cate.MasterTableView.GroupByExpressions.Count > 0 Then

            For i = 0 To grd_cate.MasterTableView.GroupByExpressions.Count

                If Not IsNothing(grd_cate.MasterTableView.GroupByExpressions(i).GroupByFields) Then
                    Dim strField As String = grd_cate.MasterTableView.GroupByExpressions(i).GroupByFields(0).FieldName
                    If strField = "Implementer" Then
                        grd_cate.MasterTableView.GroupByExpressions.RemoveAt(i)
                    End If
                End If
                i += 1
            Next

        End If

        Dim expression As GridGroupByExpression = New GridGroupByExpression
        Dim gridGroupByField As GridGroupByField = New GridGroupByField

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("anio"))) Then

        gridGroupByField = New GridGroupByField
        gridGroupByField.FieldName = "Organization"
        gridGroupByField.HeaderText = "Organization"
        'gridGroupByField.FieldAlias = "anio_1"

        'gridGroupByField.HeaderValueSeparator = "**"
        'gridGroupByField.FormatString = "<strong>{0:0}</strong>"
        'gridGroupByField.Aggregate = GridAggregateFunction.Sum

        expression.GroupByFields.Add(gridGroupByField)
        expression.SelectFields.Add(gridGroupByField)

        Dim gridGroupByField_2 As GridGroupByField = New GridGroupByField

        gridGroupByField_2 = New GridGroupByField
        gridGroupByField_2.FieldName = "Activity"
        gridGroupByField_2.HeaderText = "Activity"

        expression.GroupByFields.Add(gridGroupByField_2)
        expression.SelectFields.Add(gridGroupByField_2)

        'gridGroupByField = New GridGroupByField
        'gridGroupByField.FieldName = "ContactTitle"
        'expression.GroupByFields.Add(gridGroupByField)
        grd_cate.MasterTableView.GroupByExpressions.Add(expression)
        grd_cate.Rebind()

    End Sub

    Private Sub grd_cate_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource

        Dim a = Me.hd_show_this_tab.Value
        Me.hd_show_this_tab.Value = 1
        fill_Grid(False)

    End Sub

    Private Sub grd_cate_ColumnCreated(sender As Object, e As GridColumnCreatedEventArgs) Handles grd_cate.ColumnCreated

        ' For Each dtRow As DataRow In check_field().Rows
        Dim boundColumn As New GridBoundColumn
        If e.Column.UniqueName = "Amount" Or e.Column.UniqueName = "AmountUSD" Then
            boundColumn = e.Column
            boundColumn.Aggregate = GridAggregateFunction.Sum
            boundColumn.DataFormatString = "{0:N2}"

        End If

        If e.Column.UniqueName = "Date" Then
            boundColumn = e.Column
            boundColumn.DataFormatString = "{0:dd/MM/yyyy}"
        End If

        If e.Column.UniqueName = "Entregable" Then
            BoundColumn = e.Column
            BoundColumn.ItemStyle.Wrap = True
            boundColumn.ItemStyle.Width = Unit.Pixel(250)
            boundColumn.HeaderStyle.Width = Unit.Pixel(250)
        End If

        '  Next

    End Sub



    'Private Sub cmb_mecanism_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_mecanism.SelectedIndexChanged

    '    Using dbEntities As New dbRMS_JIEntities

    '        If Val(e.Value) > 0 Then
    '            Me.cmb_Sub_mecanism.DataSourceID = ""
    '            Me.cmb_Sub_mecanism.DataSource = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_mecanismo_contratacion = Me.cmb_mecanism.SelectedValue).ToList()
    '            Me.cmb_Sub_mecanism.DataTextField = "nombre_sub_mecanismo"
    '            Me.cmb_Sub_mecanism.DataValueField = "id_sub_mecanismo"
    '            Me.cmb_Sub_mecanism.DataBind()
    '            ' fillGrid(True)
    '        End If

    '    End Using


    'End Sub


End Class