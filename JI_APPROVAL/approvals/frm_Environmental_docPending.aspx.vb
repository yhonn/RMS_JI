Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI

Imports ly_APPROVAL



Partial Class frm_environmental_docsPending
    Inherits System.Web.UI.Page
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim varUSer As String
    Dim IDstatus As Integer
    Dim IDproceso As Integer
    Private Const ItemsPerRequest As Integer = 10

    Dim clss_approval As APPROVAL.clss_approval

    Const cPENDING = 1
    Const cAPPROVED = 2


    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_ENV_DOCS_PEN"
    Dim controles As New ly_SIME.CORE.cls_controles

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login.aspx")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                cl_user.chk_Rights(Page.Controls, 14, frmCODE, 0)
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            'If Me.Session("E_IdPerfil") = 5 Or Me.Session("E_IdPerfil") = 1 Then
            'Me.grd_cate.Columns(0).Visible = True
            'Else
            '    Me.grd_cate.Columns(0).Visible = False
            'End If


            Dim Sql As String = String.Format("SELECT nombre_usuario as nombre_empleado, usuario, job as codigo FROM vw_t_usuarios " &
                                                  "   WHERE id_usuario={0} and id_programa ={1} ", Me.Session("E_IdUser"), Me.Session("E_IDPrograma"))
            '"SELECT edita_informes, dbo.INITCAP(nombre_empleado) as nombre_empleado, usuario, codigo FROM vw_empleados WHERE id_empleado=" & Me.Session("E_IdUser")

            Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            Dim ds As New DataSet("permisos")
            dm.Fill(ds, "permisos")
            Dim verTodo = ds.Tables("permisos").Rows(0).Item(0)

            varUSer = ds.Tables("permisos").Rows(0).Item("nombre_empleado")

            IDstatus = 0
            IDproceso = 0
            Me.Session("E_bnd_var_01") = IDstatus
            Me.Session("E_bnd_var_02") = IDproceso
            Me.Session("E_bnd_var_03") = varUSer


            Dim currentdate As Date = Date.Today

            'Date Formulas: Find First Day of Previous Month
            'DateAdd("m", -1, DateSerial(Year(Today()), Month(Today()), 1))
            'Find Last Day of Previous Month
            'DateSerial(Year(Today()), Month(Today()), 0)
            'Find First Day of Current Month
            'DateSerial(Year(Today()), Month(Today()), 1)
            'Find Last Day of Current Month
            'DateSerial(Year(Today()), Month(DateAdd("m", 1, Today())), 0)

            Me.txt_finicio.SelectedDate = DateSerial(Year(Today()), Month(Today()), 1) '"01/" & currentdate.Month & "/" & currentdate.Year
            'Dim diaMax = Date.DaysInMonth(currentdate.Year, currentdate.Month)
            Me.txt_ffin.SelectedDate = DateSerial(Year(Today()), Month(DateAdd("m", 1, Today())), 0) 'diaMax.ToString & "/" & currentdate.Month & "/" & currentdate.Year

            grd_cate.PagerStyle.Position = GridPagerPosition.Top
            'fillGrid()

        Else

            IDstatus = Me.Session("E_bnd_var_01")
            IDproceso = Me.Session("E_bnd_var_02")
            varUSer = Me.Session("E_bnd_var_03")

        End If

    End Sub


    Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource

        Me.grd_cate.DataSource = getData(IDstatus, IDproceso)

    End Sub


    Public Function getData(Optional ByVal idEstatus As Integer = 0, Optional ByVal idProcess As Integer = 0) As DataTable

        'Dim sql As String = "SELECT ROW_NUMBER() OVER (ORDER BY Fecha_Recepcion) AS [No], * FROM VW_GR_TA_DOCUMENTOS " &
        '                  " WHERE id_programa=" & Me.Session("E_IDPrograma")

        Dim sql As String = String.Format("select ROW_NUMBER() OVER (ORDER BY fecha_creado) AS [No], * from vw_t_documento_ambiental where id_estado = {0} and id_programa = {1} ", cPENDING, Convert.ToInt32(Me.Session("E_IDPrograma")))


        Dim strSearch As String = ""

        'If Not String.IsNullOrEmpty(Trim(Me.txt_doc.Text)) Then
        '    sql &= String.Format(" AND ((descripcion_doc LIKE '%{0}%') OR (nom_beneficiario LIKE '%{0}%') OR (numero_instrumento LIKE '%{0}%')) ", Trim(Me.txt_doc.Text))
        'End If

        If Not String.IsNullOrEmpty(Trim(Me.RadComboBox3.Text)) Then

            If idProcess = 0 Then
                sql &= String.Format(" AND ((observacion LIKE '%{0}%') OR (descripcion_aprobacion LIKE '%{0}%') OR (numero_instrumento LIKE '%{0}%')  OR (nom_beneficiario LIKE '%{0}%') ) ", Trim(Me.RadComboBox3.Text))
                strSearch &= String.Format(" Contains the words = ""{0}{2}"" {1}", Strings.Left(Trim(RadComboBox3.Text), 45), "<br />", "...")
            Else
                sql &= String.Format(" AND ( id_documento_ambiental = {0} )", idProcess)
                strSearch &= String.Format(" Proccess = ""{0}{2}"" {1}", Strings.Left(Trim(RadComboBox3.Text), 45), "<br />", "...")
            End If

        End If

        Dim datefilter As String = ""
        If Me.chkfilterDate.Checked = True Then
            datefilter = " AND ( fecha_creado BETWEEN CONVERT(DATETIME, '" & Me.txt_finicio.SelectedDate & "') AND CONVERT(DATETIME, '" & Me.txt_ffin.SelectedDate & "') ) "
            strSearch &= String.Format(" Created between ""{0:d}"" and ""{1:d}"" {2}", Me.txt_finicio.SelectedDate, Me.txt_ffin.SelectedDate, "<br />")
        End If

        'If Me.RadioButton1.Checked = True Then

        '    sql &= String.Format("  AND ( " &
        '          "  (     (( {0} = IdOriginador) OR ( IdRolOriginator in ({1}))) AND id_estadoDoc = 6 ) " &
        '          "      OR ({0} IN (select * from dbo.SDF_SplitString(idUserOwner,',')) AND id_estadoDoc = 1)  " &
        '          "      OR ( rol_owner in ({2}) AND id_estadoDoc = 1)  )  ", Me.Session("E_IdUser"), lbl_GroupRolID.Text, lbl_ALL_SIMPLE_RolID.Text)


        '    strSearch &= String.Format("Approval processes that is pending action by {0}{1}", varUSer, "<br />")


        'ElseIf Me.RadioButton2.Checked = True Then

        '    sql &= String.Format(" AND ( (id_estadoDoc in (1,6) ) " &
        '             " AND (   ( {0} = IdOriginador) OR ( IdRolOriginator in ({1}) )   ) ) ", Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text)

        '    strSearch &= String.Format("Approvals processes that has been initiated by {0}{1}", varUSer, "<br />")

        'ElseIf Me.RadioButton3.Checked = True Then

        '    sql &= String.Format("  And (   
        '                                ( {0} = IdOriginador And id_estadoDoc = 1 ) 
        '                                Or (  
        '                           ( id_estadoDoc = 6 Or id_estadoDoc = 1 )  And ( ({0}) in (select * from dbo.SDF_SplitString(IdUserPArticipate,',')) )     						    
        '                          )
        '                                Or (
        '                            ( id_estadoDoc = 6 ) And (  (select * from dbo.SDF_SplitString(IdRolePArticipate,',')) in  (select * from dbo.SDF_SplitString('{1}',',')) )
        '                           ) 
        '                                 Or (
        '                            ( id_estadoDoc = 1 ) And (  (select * from dbo.SDF_SplitString(IdRolePArticipate,',')) in  (select * from dbo.SDF_SplitString('{2}',',')) )
        '                           ) 

        '                         )  ", Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text)


        '    strSearch &= String.Format(" {0} has participated into the approval process{1}", varUSer, "<br />")

        'Else

        '    sql &= " AND ( id_estadoDoc = 1 OR id_estadoDoc = 6)"
        '    strSearch &= String.Format("All pending approval processes ", varUSer, "<br />")

        'End If

        sql &= datefilter
        sql &= " ORDER BY fecha_creado "

        Dim ds As New DataSet("TotalRegistros")
        Dim dm As New SqlDataAdapter(sql, cnnSAP)
        dm.Fill(ds, "TotalRegistros")
        Me.lbltotal.Text = "[ " & ds.Tables("TotalRegistros").Rows.Count & " ]" & If(ds.Tables("TotalRegistros").Rows.Count = 0, " approval process", " approval processes")


        If Strings.Len(strSearch) > 0 Then

            strSearch = String.Format("...Searching by {0}", "<br />") & strSearch

        Else

            strSearch = String.Format("...Searching all pending Environmental approvals  {0}", "<br />") & strSearch

        End If

        lblt_Search.Text = strSearch

        Return ds.Tables("TotalRegistros")

    End Function

    Private Function GetData(ByVal text As String) As DataTable

        Dim sql As String
        Dim ds As New DataSet("registeres")

        'sql = String.Format("select a.id_documento, a.descripcion_doc, a.numero_instrumento, a.nom_beneficiario " &
        '                    "    from VW_GR_TA_DOCUMENTOS a " &
        '                    "        where  a.id_programa={1} " &
        '                    "  AND                               (a.descripcion_doc like '%{0}%' " &
        '                    "                                      or a.numero_instrumento like '%{0}%' " &
        '                    "                                        or a.nom_beneficiario like '%{0}%') ", Trim(text), Me.Session("E_IDPrograma"))

        sql = String.Format("select * from vw_t_documento_ambiental where id_estado = {0} and id_programa = {1}  " &
                                           "  AND ((observacion LIKE '%{2}%') OR (descripcion_aprobacion LIKE '%{2}%') OR (numero_instrumento LIKE '%{2}%') OR (nom_beneficiario LIKE '%{2}%'))  ", cPENDING, Convert.ToInt32(Me.Session("E_IDPrograma")), Trim(text))


        Dim datefilter As String = ""
        If Me.chkfilterDate.Checked = True Then
            datefilter = " And ( fecha_creado BETWEEN CONVERT(DATETIME, '" & Me.txt_finicio.SelectedDate & "') AND CONVERT(DATETIME, '" & Me.txt_ffin.SelectedDate & "') ) "
        End If


        'If Me.RadioButton1.Checked = True Then


        '    sql &= String.Format("  AND ( " &
        '          "  (     (( {0} = IdOriginador) OR ( IdRolOriginator in ({1}))) AND id_estadoDoc = 6 ) " &
        '          "      OR ({0} IN (select * from dbo.SDF_SplitString(idUserOwner,',')) AND id_estadoDoc = 1)  " &
        '          "      OR ( row_owner in ({2}) AND id_estadoDoc = 1)  )  ", Me.Session("E_IdUser"), lbl_GroupRolID.Text, lbl_ALL_SIMPLE_RolID.Text)


        'ElseIf Me.RadioButton2.Checked = True Then  'Initiated by


        '    sql &= String.Format(" AND ( (id_estadoDoc in (1,6) ) " &
        '             " AND (   ( {0} = IdOriginador) OR ( IdRolOriginator in ({1}) )   ) ) ", Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text)



        'ElseIf Me.RadioButton3.Checked = True Then


        '    sql &= String.Format("  And (   
        '                                ( {0} = IdOriginador And id_estadoDoc = 1 ) 
        '                                Or (  
        '                           ( id_estadoDoc = 6 Or id_estadoDoc = 1 )  And ( ({0}) in (select * from dbo.SDF_SplitString(IdUserPArticipate,',')) )     						    
        '                          )
        '                                Or (
        '                            ( id_estadoDoc = 6 ) And (  (select * from dbo.SDF_SplitString(IdRolePArticipate,',')) in  (select * from dbo.SDF_SplitString('{1}',',')) )
        '                           ) 
        '                                Or (
        '                            ( id_estadoDoc = 1 ) And (  (select * from dbo.SDF_SplitString(IdRolePArticipate,',')) in  (select * from dbo.SDF_SplitString('{2}',',')) )
        '                           ) 
        '                         )  ", Me.Session("E_IdUser"), lbl_ALL_SIMPLE_RolID.Text, lbl_ALL_RolID.Text)



        'Else

        '    sql &= " AND ( id_estadoDoc = 1 OR id_estadoDoc = 6)"

        'End If

        sql &= datefilter

        Dim adapter As New SqlDataAdapter(sql, cnnSAP)
        Dim data As New DataTable()
        adapter.Fill(data)

        Return data

    End Function

    Sub fillGrid()
        Me.grd_cate.Rebind()

    End Sub
    Protected Sub btn_buscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_buscar.Click
        fillGrid()
    End Sub



    Protected Sub RadComboBox3_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs) 'Handles RadComboBox3.SelectedIndexChanged

        If Val(RadComboBox3.SelectedValue) > 0 Then
            IDproceso = Val(RadComboBox3.SelectedValue)
            Me.Session("E_bnd_var_02") = IDproceso
            fillGrid()
        Else
            IDproceso = 0
            Me.Session("E_bnd_var_02") = IDproceso
            fillGrid()
        End If

    End Sub

    Protected Sub RadComboBox3_ItemsRequested(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs) Handles RadComboBox3.ItemsRequested

        Dim data As DataTable = GetData(e.Text)

        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count)
        e.EndOfItems = endOffset = data.Rows.Count

        RadComboBox3.DataSource = data
        RadComboBox3.DataBind()

        If Strings.Len(Trim(e.Text)) = 0 Then
            IDproceso = 0
            Me.Session("E_bnd_var_02") = IDproceso
        End If

        'For i As Integer = itemOffset To endOffset - 1
        '    RadComboBox3.Items.Add(New RadComboBoxItem(data.Rows(i)("id_documento").ToString(), data.Rows(i)("descripcion_doc").ToString()))
        'Next
        ' e.Message = GetStatusMessage(endOffset, data.Rows.Count)
        ' SessionDataSource1.SelectCommand = sql
        'RadComboBox1.DataBind()

    End Sub

    Protected Sub RadComboBox3_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
        'set the Text and Value property of every item
        'here you can set any other properties like Enabled, ToolTip, Visible, etc.
        e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("observacion").ToString()
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_documento_ambiental").ToString()

    End Sub


    Protected Sub RadComboBox3_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        'set the initial footer label
        CType(RadComboBox3.Footer.FindControl("RadComboItemsCount"), Literal).Text = Convert.ToString(RadComboBox3.Items.Count)
    End Sub

    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim estado As New Image
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            Dim hlnkPrint As HyperLink = New HyperLink
            hlnkPrint = CType(e.Item.FindControl("Imprimir"), HyperLink)
            hlnkPrint.NavigateUrl = "frm_seguimientoAprobacionEnvironmental.aspx?IdDoc=" & itemD("id_documento_ambiental").Text

            Dim aprobar As New HyperLink
            aprobar = CType(itemD("colm_approvals").FindControl("aprobar"), HyperLink)
            aprobar.NavigateUrl = "~/approvals/frm_EnvironmentalApprovals.aspx?IdDoc=" & itemD("id_documento_ambiental").Text
            aprobar.ImageUrl = "~/Imagenes/Iconos/accept.png"
            aprobar.Target = "_self"

        End If
    End Sub

    Protected Sub chkfilterDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkfilterDate.CheckedChanged
        fillGrid()
    End Sub


End Class
