Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL

Partial Class frm_aprobaciones_ruta
    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_APPROVAL_PATH"
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim cls_approval_route As APPROVAL.clss_approval_route

    Dim dtTipoAPP As DataTable
    Dim dtDeliverableStage As DataTable


    'Sub llenarGrid(ByVal id_tipoDoc As Integer)
    '    Dim Sql = " INSERT INTO ta_rutaTipoDoc_temp SELECT '" & Me.lbl_id_sesion_temp.Text & "' as id_sesion_temp, id_tipoDocumento, id_rol, orden, duracion FROM ta_rutaTipoDoc WHERE id_tipoDocumento = " & Me.HiddenField1.Value
    '    Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
    '    Dim ds1 As New DataSet("proyecto")
    '    dm1.Fill(ds1, "proyecto")
    '    Me.grd_cate.DataBind()

    'End Sub

    Sub ActualizaDatos()

        Dim row As GridItem
        Dim i = 0
        Dim sql = ""
        cnnSAP.Open()

        For Each row In Me.grd_cate.Items
            Dim txt_orden As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_orden"), RadNumericTextBox)
            Dim txt_duracion As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_duracion"), RadNumericTextBox)
            Dim origen As Image = CType(row.Cells(0).FindControl("imgStart"), Image)
            sql = "UPDATE ta_rutaTipoDoc SET orden=" & txt_orden.Text & ", duracion=" & txt_duracion.Text & " WHERE id_ruta=" & row.Cells(3).Text

            Dim dm As New SqlDataAdapter(sql, cnnSAP)
            Dim ds As New DataSet("IdPlan")
            dm.Fill(ds, "IdPlan")

            Me.grd_cate.DataBind()

            i += 1
        Next
        cnnSAP.Close()
    End Sub


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
                cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            Dim Random As New Random()
            Dim Aleatorio As Double = Random.Next(1, 999)
            Me.hd_dtTipoAPP.Value = String.Format("dtTipoAPP{0}_{1}", Me.Session("E_IdUser"), Aleatorio)
            Me.hd_dtDeliverableStage.Value = String.Format("dtDeliverableStage{0}_{1}", Me.Session("E_IdUser"), Aleatorio)



            Me.HiddenField1.Value = Me.Request.QueryString("IdType")
            cls_approval_route = New APPROVAL.clss_approval_route(Me.Session("E_IDPrograma"), Me.HiddenField1.Value)
            cls_approval_route.set_Approval()


            dtTipoAPP = cls_approval_route.get_Approval_tipo()
            dtDeliverableStage = cls_approval_route.get_Approval_Deliverable_stage()


            Session(Me.hd_dtTipoAPP.Value) = dtTipoAPP
            Session(Me.hd_dtDeliverableStage.Value) = dtDeliverableStage



            'Dim Sql = "SELECT * FROM vw_aprobaciones WHERE id_tipoDocumento=" & Me.HiddenField1.Value
            'Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds1 As New DataSet("proyecto")
            'dm1.Fill(ds1, "proyecto")
            'Me.lbl_category.Text = ds1.Tables("proyecto").Rows(0).Item("descripcion_cat").ToString
            'Me.lbl_approval.Text = ds1.Tables("proyecto").Rows(0).Item("descripcion_aprobacion").ToString

            Me.lbl_category.Text = cls_approval_route.get_Approval_dtFIELDS("descripcion_cat", "id_tipoDocumento", Me.HiddenField1.Value)
            Me.lbl_approval.Text = cls_approval_route.get_Approval_dtFIELDS("descripcion_aprobacion", "id_tipoDocumento", Me.HiddenField1.Value)



            Try
                Dim rnd As New Random()
                Dim fecha As DateTime = Date.Now
                Dim textfecha = fecha.ToString.Replace("/", "").Replace(" ", "").Replace("a", "").Replace(".", "").Replace("m", "").Replace(":", "").Replace(";", "").Replace("p", "")
                Me.lbl_id_sesion_temp.Text = rnd.Next(1, 999999).ToString & textfecha.ToString
            Catch ex As Exception
                Me.lbl_id_sesion_temp.Text = "-1"
            End Try


            Dim email_items() As String
            email_items = cls_approval_route.get_Approval_dtFIELDS("email", "id_tipoDocumento", Me.HiddenField1.Value).ToString.Split(";")
            For i = 0 To email_items.Count - 1
                If email_items(i).ToString <> "" Then
                    Me.lst_email.Items.Add(email_items(i).ToString)
                End If
            Next

            Me.grd_cate.DataSource = cls_approval_route.get_RutaAprobacion()
            Me.grd_cate.DataBind()

            Me.cmb_rol.DataSource = cls_approval_route.get_RolesUser(Me.HiddenField1.Value)
            Me.cmb_rol.DataBind()

            cmb_emailList.DataSource = cls_approval_route.get_UserEmail()
            cmb_emailList.DataBind()


            HttpContext.Current.Session.Add("cls_approval_route", cls_approval_route)

            '*******Check  the Open ProccesseS*******

            'Sql = "SELECT * FROM vw_ta_AppDocumento WHERE id_tipoDocumento=" & Me.HiddenField1.Value
            'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds As New DataSet("AppDocumento")
            'dm.Fill(ds, "AppDocumento")
            'If ds.Tables("AppDocumento").Rows.Count() > 0 Then

            If cls_approval_route.get_OPEN_Proccess() > 0 Then
                Me.grd_cate.Columns(0).Visible = False
                Me.btn_doc.Visible = False
                Me.cmb_rol.Enabled = False
                Me.lblt_msj_step_error.Visible = True
            End If
        Else

            dtTipoAPP = Session(Me.hd_dtTipoAPP.Value)
            dtDeliverableStage = Session(Me.hd_dtDeliverableStage.Value)

            If HttpContext.Current.Session.Item("cls_approval_route") IsNot Nothing Then
                cls_approval_route = Me.Session.Item("cls_approval_route")
            End If

        End If


    End Sub

    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click

        'Dim cont As Integer = 0
        'Dim sum_orden As Integer = 0
        Dim err As Boolean = False


        'Dim Sql = "SELECT count(*) as cantidad FROM ta_rutaTipoDoc WHERE id_tipoDocumento=" & Me.HiddenField1.Value
        'cnnSAP.Open()
        'Dim dm1 As New SqlDataAdapter(Sql, cnnSAP)
        'Dim ds1 As New DataSet("IdPlan")
        'dm1.Fill(ds1, "IdPlan")

        'If ds1.Tables("IdPlan").Rows(0).Item(0) <= 1 Then

        If cls_approval_route.get_ApprovalProC_Order <= 1 Then
            err = True
            Me.lbl_msjItems.Text = "The approval process has to have at least 2 steps."
        End If

        'Sql = "SELECT orden FROM ta_rutaTipoDoc WHERE id_tipoDocumento=" & Me.HiddenField1.Value
        'ds1.Tables.Add("rutaTipoDoc_temp")
        'dm1.SelectCommand.CommandText = Sql
        'dm1.SelectCommand.ExecuteNonQuery()
        'dm1.Fill(ds1, "rutaTipoDoc_temp")
        ''**********************PRIMERA VALIDACION POR SUMATORIAS********************
        'For i = 0 To ds1.Tables("rutaTipoDoc_temp").Rows.Count - 1
        '    sum_orden += ds1.Tables("rutaTipoDoc_temp").Rows(i).Item(0)
        '    cont += i
        'Next
        'If sum_orden <> cont Then
        '    err = True
        '    Me.lbl_msjItems.Text = "Error 1.." & cont & " " & sum_orden
        'End If
        '***************************************************************************

        ''******************************SEGUNDA VALIDACION POR GROUP BY **************************
        'Sql = "SELECT orden, COUNT(*) AS cantidad FROM ta_rutaTipoDoc WHERE id_tipoDocumento=" & Me.HiddenField1.Value & " GROUP BY orden HAVING (COUNT(*) > 1)"
        'ds1.Tables.Add("agrupamiento")
        'dm1.SelectCommand.CommandText = Sql
        'dm1.SelectCommand.ExecuteNonQuery()
        'dm1.Fill(ds1, "agrupamiento")
        'If ds1.Tables("agrupamiento").Rows.Count() > 0 Then
        '    err = True
        '    Me.lbl_msjItems.Text = "The approval process has duplicated items.."
        'End If
        '*****************************************************************************************

        '******************************TERCERA VALIDACION VALORES **************************
        'Sql = "SELECT orden FROM ta_rutaTipoDoc WHERE  (id_tipoDocumento = " & Me.HiddenField1.Value & ")"
        'Sql &= " AND (orden >  (SELECT COUNT(*) - 1 AS cantidad  FROM  ta_rutaTipoDoc AS ta_rutaTipoDoc_temp_1"
        'Sql &= " WHERE id_tipoDocumento = " & Me.HiddenField1.Value & "))"
        'ds1.Tables.Add("valores")
        'dm1.SelectCommand.CommandText = Sql
        'dm1.SelectCommand.ExecuteNonQuery()
        'dm1.Fill(ds1, "valores")
        'If ds1.Tables("valores").Rows.Count() > 0 Then
        '    err = True
        '    Me.lbl_msjItems.Text = "Index out of interval .."
        'End If
        '****************************************************************************************

        Dim email As String = ""
        For i = 0 To Me.lst_email.Items.Count - 1
            email &= Me.lst_email.Items(i).Text.ToString & "; "
        Next

        'email &= ">!"
        'If email = ">!" Then
        '    email = ""
        '    'err = True
        '    'Me.lbl_msjItems.Visible = True
        'Else
        '    email = email.Replace("; >!", "").ToString
        'End If

        If err = False Then

            For Each rowD As GridDataItem In Me.grd_cate.Items

                Dim txt_duracion As RadNumericTextBox = CType(rowD("Duracion").FindControl("txt_duracion"), RadNumericTextBox)
                Dim chk_TriggerTool As CheckBox = CType(rowD("colm_trigger_tool").FindControl("chk_trigger"), CheckBox)
                Dim bnd_Trigger As Integer = 0

                If chk_TriggerTool.Checked Then
                    bnd_Trigger = 1
                End If


                Dim cmb_tp As RadComboBox = CType(rowD("colm_app_tp").FindControl("cmb_app_tp"), RadComboBox)
                Dim idTIPO As Integer = cmb_tp.SelectedValue

                Dim cmb_Deliv As RadComboBox = CType(rowD("colm_deliverable_stage").FindControl("cmb_deliv_stage"), RadComboBox)
                Dim idDeliverableStage As Integer = cmb_Deliv.SelectedValue


                cls_approval_route.set_ta_rutaTipoDoc(rowD("id_ruta").Text)
                cls_approval_route.set_ta_rutaTipoDocFIELDS("duracion", txt_duracion.Value, "id_ruta", rowD("id_ruta").Text)
                cls_approval_route.set_ta_rutaTipoDocFIELDS("trigger_tool", bnd_Trigger, "id_ruta", rowD("id_ruta").Text)
                cls_approval_route.set_ta_rutaTipoDocFIELDS("id_estadoTipo", idTIPO, "id_ruta", rowD("id_ruta").Text)
                cls_approval_route.set_ta_rutaTipoDocFIELDS("id_deliverable_stage", idDeliverableStage, "id_ruta", rowD("id_ruta").Text)

                cls_approval_route.save_ta_rutaTipoDoc()

            Next



            Me.lbl_msjItems.Visible = False

            cls_approval_route.set_ta_tipoDocumento() 'SEt de Tipo Documento at the beginning
            cls_approval_route.set_ta_tipoDocumentoFIELDS("ruta_completa", "SI", "id_tipoDocumento", cls_approval_route.id_tipoDocumento)
            cls_approval_route.set_ta_tipoDocumentoFIELDS("email", email, "id_tipoDocumento", cls_approval_route.id_tipoDocumento)

            'Sql = " UPDATE ta_tipoDocumento SET ruta_completa='SI', email='" & email & "' WHERE  id_tipoDocumento=" & Me.HiddenField1.Value
            'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds As New DataSet("IdPlan")
            'dm.Fill(ds, "IdPlan")

            If cls_approval_route.save_ta_tipoDocumento() Then

                Session.Remove(Me.hd_dtTipoAPP.Value)
                Session.Remove(Me.hd_dtDeliverableStage.Value)

                Me.Response.Redirect("~/Approvals/frm_aprobaciones.aspx")

            Else

                Me.lbl_msjItems.Text = "Error Saving the Route.."
                Me.lbl_msjItems.Visible = True

            End If


        Else
            Me.lbl_msjItems.Visible = True
        End If


    End Sub

    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/Approvals/frm_aprobaciones.aspx")
    End Sub

    Protected Sub btn_doc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_doc.Click


        Dim i = 0
        Dim orden = 0

        If Val(cmb_rol.SelectedValue) > 0 Then

            'cnnSAP.Open()
            'Dim Sql = "SELECT COUNT(*) AS nfilas FROM ta_rutaTipoDoc WHERE id_tipoDocumento=" & Me.HiddenField1.Value
            'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds As New DataSet("IdPlan")
            'dm.Fill(ds, "IdPlan")
            'orden = ds.Tables("IdPlan").Rows(0).Item(0)

            orden = cls_approval_route.get_ApprovalProC_Order()
            Dim duracion = 5 'Look where change it
            Dim trigger As Integer = 0
            Dim id_APP_tipo As Integer = 1 'VBO by default

            cls_approval_route.set_ta_rutaTipoDoc(0) 'Set to New Record

            cls_approval_route.set_ta_rutaTipoDocFIELDS("id_tipoDocumento", Me.HiddenField1.Value, "id_ruta", 0)
            cls_approval_route.set_ta_rutaTipoDocFIELDS("id_rol", Me.cmb_rol.SelectedValue, "id_ruta", 0)
            cls_approval_route.set_ta_rutaTipoDocFIELDS("orden", orden, "id_ruta", 0)
            cls_approval_route.set_ta_rutaTipoDocFIELDS("duracion", duracion, "id_ruta", 0)
            cls_approval_route.set_ta_rutaTipoDocFIELDS("trigger_tool", trigger, "id_ruta", 0)
            cls_approval_route.set_ta_rutaTipoDocFIELDS("id_estadoTipo", id_APP_tipo, "id_ruta", 0)


            'Sql = " INSERT INTO ta_rutaTipoDoc ( id_tipoDocumento, id_rol, orden, duracion ) VALUES (" & Me.HiddenField1.Value & ", " & Me.cmb_rol.SelectedValue & ", " & orden & ", " & duracion & ") "
            'dm.SelectCommand.CommandText = Sql
            'dm.SelectCommand.ExecuteNonQuery()
            'cnnSAP.Close()

            If cls_approval_route.save_ta_rutaTipoDoc() <> -1 Then

                Me.grd_cate.DataSource = cls_approval_route.get_RutaAprobacion()
                Me.grd_cate.DataBind()
                Me.cmb_rol.DataSource = cls_approval_route.get_RolesUser(Me.HiddenField1.Value)
                Me.cmb_rol.DataBind()
                cmb_rol.SelectedIndex = -1
                cmb_rol.Text = ""

            End If


            'btn_doc.Enabled = True
            'btn_doc.Text = "Add Role to Approval"

        End If

    End Sub

    Protected Sub grd_cate_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand

        Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_ruta").ToString()
        'Me.SqlDataSource4.DeleteCommand = "DELETE FROM ta_rutaTipoDoc WHERE (id_ruta = " & id_temp & ")"
        cls_approval_route.del_RutaAprobacion(id_temp)
        cls_approval_route.Approval_Reorder()

        Me.grd_cate.DataSource = cls_approval_route.get_RutaAprobacion()
        Me.grd_cate.DataBind()

        Me.cmb_rol.DataSource = cls_approval_route.get_RolesUser(Me.HiddenField1.Value)
        Me.cmb_rol.DataBind()

    End Sub


    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            Dim txt_orden As RadNumericTextBox = CType(itemD("Orden").FindControl("txt_orden"), RadNumericTextBox) 'CType(e.Item.FindControl("txt_orden"), RadNumericTextBox)
            Dim txt_duracion As RadNumericTextBox = CType(itemD("Duracion").FindControl("txt_duracion"), RadNumericTextBox)  'CType(e.Item.FindControl("txt_duracion"), RadNumericTextBox)

            Dim origen As Image = CType(itemD("Orden").FindControl("imgStart"), Image) 'CType(e.Item.FindControl("imgStart"), Image)

            txt_orden.Text = itemD("ordentxt").Text 'e.Item.Cells(8).Text.ToString
            txt_duracion.Text = itemD("duraciontxt").Text ' e.Item.Cells(9).Text

            If txt_orden.Text = "0" Then
                origen.Visible = True
            End If

            Dim chk_CheckTrigger As CheckBox = CType(itemD("colm_trigger_tool").FindControl("chk_trigger"), CheckBox)  'CType(e.Item.FindControl("txt_duracion"), RadNumericTextBox)

            If itemD("trigger_tool").Text = 1 Then
                chk_CheckTrigger.Checked = True
            End If

            Dim cmb_tp As RadComboBox = CType(itemD("colm_app_tp").FindControl("cmb_app_tp"), RadComboBox)
            cmb_tp.DataSource = dtTipoAPP
            cmb_tp.DataTextField = "estado_tipo_prefijo"
            cmb_tp.DataValueField = "id_estadoTipo"
            cmb_tp.DataBind()

            cmb_tp.SelectedValue = itemD("id_estadoTipo").Text

            Dim cmb_deliv As RadComboBox = CType(itemD("colm_deliverable_stage").FindControl("cmb_deliv_stage"), RadComboBox)
            cmb_deliv.DataSource = dtDeliverableStage
            cmb_deliv.DataTextField = "deliverable_stage"
            cmb_deliv.DataValueField = "id_deliverable_stage"
            cmb_deliv.DataBind()
            cmb_deliv.SelectedValue = itemD("id_deliverable_stage").Text

            If itemD("id_approval_tool").Text <> 3 Then 'Deliverable Tool assigment
                cmb_deliv.Visible = False
            Else
                cmb_deliv.Visible = True
            End If


            'Dim Sql = "SELECT * FROM vw_ta_AppDocumento WHERE id_tipoDocumento=" & Me.HiddenField1.Value
            'Dim dm As New SqlDataAdapter(Sql, cnnSAP)
            'Dim ds As New DataSet("AppDocumento")
            'dm.Fill(ds, "AppDocumento")
            'If ds.Tables("AppDocumento").Rows.Count() > 0 Then
            '    txt_orden.ReadOnly = True
            'End If

        End If
    End Sub

  
    Protected Sub imgUpdateCmb_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUpdateCmb.Click

        Me.cmb_rol.DataSource = cls_approval_route.get_RolesUser(Me.HiddenField1.Value)
        Me.cmb_rol.DataBind()

    End Sub

    'Protected Sub txt_orden_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    ActualizaDatos()
    'End Sub

    'Protected Sub txt_duracion_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    ActualizaDatos()
    'End Sub

    Protected Sub btn_add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_add.Click
        Dim list As New ListItem
        list.Value = Me.txtemail.Text
        list.Text = Me.txtemail.Text
        Me.lst_email.Items.Add(list)
        Me.txtemail.Text = ""
    End Sub

    Protected Sub btn_remove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_remove.Click
        Me.lst_email.Items.Remove(Me.lst_email.SelectedItem)
    End Sub


    Public Sub chkDown_CheckedChanged()

        Dim tbl_UserRol As New DataTable

        Dim minOrder As Integer = cls_approval_route.get_MinOrder()
        Dim minBack As Integer
        'Dim maxOrder As Integer

        For Each rowD As GridDataItem In Me.grd_cate.Items

            Dim chkDown As CheckBox = CType(rowD("colm_reOrderDW").FindControl("chkDown"), CheckBox)

            If chkDown.Checked = True Then 'less

                If minOrder < rowD("ordentxt").Text Then 'Yes Reorder
                    minBack = cls_approval_route.get_MinOrder(rowD("ordentxt").Text)
                    cls_approval_route.route_Reorder(CType(rowD("ordentxt").Text, Integer), minBack)
                End If

            End If

        Next

        Me.grd_cate.DataSource = cls_approval_route.get_RutaAprobacion()
        Me.grd_cate.DataBind()

    End Sub

    Public Sub chkUP_CheckedChanged()


        Dim tbl_UserRol As New DataTable

        Dim MaxOrder As Integer = cls_approval_route.get_MaxOrder()
        Dim maxNext As Integer
        'Dim maxOrder As Integer

        For Each rowD As GridDataItem In Me.grd_cate.Items

            Dim chkUP As CheckBox = CType(rowD("colm_reOrderUP").FindControl("chkUP"), CheckBox)

            If chkUP.Checked = True Then 'less

                If MaxOrder > rowD("ordentxt").Text Then 'Yes Reorder
                    maxNext = cls_approval_route.get_MaxOrder(rowD("ordentxt").Text)
                    cls_approval_route.route_Reorder(CType(rowD("ordentxt").Text, Integer), maxNext)
                End If

            End If

        Next

        Me.grd_cate.DataSource = cls_approval_route.get_RutaAprobacion()
        Me.grd_cate.DataBind()


    End Sub

    Public Sub chk_trigger_CheckedChanged()


        For Each rowD As GridDataItem In Me.grd_cate.Items

            Dim chkUP As CheckBox = CType(rowD("colm_trigger_tool").FindControl("chk_trigger"), CheckBox)
            Dim bnd_trigger As Integer = 0
            If chkUP.Checked = True Then
                bnd_trigger = 1
            End If

            cls_approval_route.set_ta_rutaTipoDoc(rowD("id_ruta").Text)
            cls_approval_route.set_ta_rutaTipoDocFIELDS("trigger_tool", bnd_Trigger, "id_ruta", rowD("id_ruta").Text)
            cls_approval_route.save_ta_rutaTipoDoc()

        Next

        Me.grd_cate.DataSource = cls_approval_route.get_RutaAprobacion()
        Me.grd_cate.DataBind()

    End Sub

    Protected Sub btn_addEmail2_Click(sender As Object, e As EventArgs) Handles btn_addEmail2.Click
        If Val(cmb_emailList.SelectedValue) > 0 Then

            Dim list As New ListItem
            list.Value = Me.cmb_emailList.SelectedItem.Text
            list.Text = Me.cmb_emailList.SelectedItem.Text
            Me.lst_email.Items.Add(list)
            cmb_emailList.SelectedIndex = -1
            cmb_emailList.Text = ""

        End If
    End Sub


End Class

