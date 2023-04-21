Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Data.SqlClient
Imports CuteWebUI
Imports System.IO
Imports System.Configuration.ConfigurationManager

Public Class frm_ActivityAD_
    Inherits System.Web.UI.Page

    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_listados As New ly_SIME.CORE.cls_listados
    Dim frmCODE As String = "AP_NEW_ACTIVITY"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim valorSuma As Decimal = 0
    Dim cero = False
    Dim db As New ly_SIME.dbRMS_JIEntities

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
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then

            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Me.cmb_programa.DataSourceID = ""
            Me.cmb_programa.DataSource = cl_listados.get_t_programas(idPrograma)
            Me.cmb_programa.DataTextField = "nombre_programa"
            Me.cmb_programa.DataValueField = "id_programa"
            Me.cmb_programa.DataBind()
            'Me.cmb_programa.SelectedValue = idPrograma
            Me.cmb_programa.Enabled = False
            Me.cmb_programa.Items.Item(0).Selected = True

            Me.lbl_id_sesion_temp.Text = cl_listados.CodigoRandom()

            loadListas(idPrograma) '***TMP***
            'LoadData_code() ***TMP***

            'listPartners = New List(Of tme_ficha_partner)
            ''grd_partners.Rebind()
            Me.id_documento.Value = 0

        Else


        End If

    End Sub

    'Protected Sub cmb_region_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_region.SelectedIndexChanged

    '    Me.cmb_subregion.DataSourceID = ""
    '    Me.cmb_subregion.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
    '    Me.cmb_subregion.DataTextField = "nombre_subregion"
    '    Me.cmb_subregion.DataValueField = "id_subregion"
    '    Me.cmb_subregion.DataBind()
    '    If Not chk_todos.Checked Then
    '        Me.grd_district.DataSource = ""
    '        Me.grd_district.DataSource = db.vw_t_village.Where(Function(p) p.id_subregion = Me.cmb_subregion.SelectedValue).ToList()
    '        Me.grd_district.DataBind()
    '    End If

    '    Me.cmb_periodo.DataSourceID = ""
    '    Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregion.SelectedValue))
    '    Me.cmb_periodo.DataTextField = "nombre_periodo"
    '    Me.cmb_periodo.DataValueField = "id_periodo"
    '    Me.cmb_periodo.DataBind()
    '    LoadData_code()

    'End Sub



    Protected Sub cmb_regionII_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_regionII.SelectedIndexChanged

        Me.cmb_subregionII.DataSourceID = ""
        Me.cmb_subregionII.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_regionII.SelectedValue))
        Me.cmb_subregionII.DataTextField = "nombre_subregion"
        Me.cmb_subregionII.DataValueField = "id_subregion"
        Me.cmb_subregionII.DataBind()

        'If Not chk_todosII.Checked Then
        '    Me.grd_districtII.DataSource = ""
        '    Me.grd_districtII.DataSource = db.vw_t_village.Where(Function(p) p.id_subregion = Me.cmb_subregion.SelectedValue).ToList()
        '    Me.grd_districtII.DataBind()
        'End If

        Me.cmb_periodo.DataSourceID = ""
        Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregionII.SelectedValue))
        Me.cmb_periodo.DataTextField = "nombre_periodo"
        Me.cmb_periodo.DataValueField = "id_periodo"
        Me.cmb_periodo.DataBind()
        LoadData()

    End Sub

    Protected Sub cmb_subregionII_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_subregionII.SelectedIndexChanged
        Dim sql = ""
        Me.cmb_periodo.DataSourceID = ""
        Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregionII.SelectedValue))
        Me.cmb_periodo.DataTextField = "nombre_periodo"
        Me.cmb_periodo.DataValueField = "id_periodo"
        Me.cmb_periodo.DataBind()

        LoadData()
    End Sub



    'Protected Sub cmb_subregion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_subregion.SelectedIndexChanged
    '    Dim sql = ""
    '    Me.cmb_periodo.DataSourceID = ""
    '    Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregion.SelectedValue))
    '    Me.cmb_periodo.DataTextField = "nombre_periodo"
    '    Me.cmb_periodo.DataValueField = "id_periodo"
    '    Me.cmb_periodo.DataBind()

    '    LoadData_code()
    'End Sub



    Sub loadListas(ByVal idPrograma As Integer)

        Dim entryPoint = From u In db.t_usuarios Join q In db.t_usuario_programa On u.id_usuario Equals q.id_usuario
                         Where q.id_programa = idPrograma
                         Select New With {Key .id_usuario = u.id_usuario, Key .nombre = u.nombre_usuario & " " & u.apellidos_usuario,
                                       Key .busqueda_actividad = u.t_usuario_programa.FirstOrDefault().busqueda_actividad}


        Me.cmb_persona_encargada.DataSourceID = ""
        'Me.cmb_persona_encargada.DataSource = entryPoint.Where(Function(p) p.busqueda_actividad = True).ToList()
        Me.cmb_persona_encargada.DataSource = entryPoint.ToList()
        Me.cmb_persona_encargada.DataTextField = "nombre"
        Me.cmb_persona_encargada.DataValueField = "id_usuario"
        Me.cmb_persona_encargada.DataBind()
        Me.cmb_persona_encargada.Items.Item(0).Selected = True
        'Dim objRegion = cl_listados.get_t_regiones(idPrograma)
        Dim objRegion = cl_listados.get_t_regiones(Convert.ToInt32(Me.cmb_programa.SelectedValue))



        'Me.cmb_region.DataSourceID = ""
        'Me.cmb_region.DataSource = objRegion
        'Me.cmb_region.DataTextField = "nombre_region"
        'Me.cmb_region.DataValueField = "id_region"
        'Me.cmb_region.DataBind()

        'Me.cmb_subregion.DataSourceID = ""
        'Me.cmb_subregion.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
        'Me.cmb_subregion.DataTextField = "nombre_subregion"
        'Me.cmb_subregion.DataValueField = "id_subregion"
        'Me.cmb_subregion.DataBind()

        Me.cmb_regionII.DataSourceID = ""
        Me.cmb_regionII.DataSource = objRegion
        Me.cmb_regionII.DataTextField = "nombre_region"
        Me.cmb_regionII.DataValueField = "id_region"
        Me.cmb_regionII.DataBind()
        Me.cmb_regionII.Items.Item(0).Selected = True

        Me.cmb_subregionII.DataSourceID = ""
        Me.cmb_subregionII.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_regionII.SelectedValue))
        Me.cmb_subregionII.DataTextField = "nombre_subregion"
        Me.cmb_subregionII.DataValueField = "id_subregion"
        Me.cmb_subregionII.DataBind()
        Me.cmb_subregionII.Items.Item(0).Selected = True

        'Me.cmb_componente.DataSourceID = ""
        'Me.cmb_componente.DataSource = cl_listados.get_tme_componente_programa(Convert.ToInt32(Me.cmb_programa.SelectedValue))
        'Me.cmb_componente.DataTextField = "nombre_componente"
        'Me.cmb_componente.DataValueField = "id_componente"
        'Me.cmb_componente.DataBind()

        Me.cmb_periodo.DataSourceID = ""
        Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregionII.SelectedValue))
        Me.cmb_periodo.DataTextField = "nombre_periodo"
        Me.cmb_periodo.DataValueField = "id_periodo"
        Me.cmb_periodo.DataBind()
        Me.cmb_periodo.Items.Item(0).Selected = True

        Using dbEntities As New dbRMS_JIEntities

            Dim id_tmp = Convert.ToInt64(Me.lbl_id_sesion_temp.Text)

            Me.grd_subregionII.DataSource = ""
            Me.grd_subregionII.DataSource = dbEntities.vw_t_subregiones.Where(Function(p) p.id_programa = Me.cmb_programa.SelectedValue).OrderBy(Function(p) p.nombre_region).ThenBy(Function(p) p.nombre_subregion).ToList()
            Me.grd_subregionII.DataBind()

            '    Me.grd_district.DataSource = ""
            '    Me.grd_district.DataSource = db.vw_t_village.Where(Function(p) p.id_subregion = Me.cmb_subregion.SelectedValue).ToList()
            '    Me.grd_district.DataBind()

            Me.cmb_estado.DataSourceID = ""
            Me.cmb_estado.DataSource = cl_listados.get_TA_ACTIVITY_STATUS(idPrograma)
            Me.cmb_estado.DataTextField = "STATUS"
            Me.cmb_estado.DataValueField = "ID_ACTIVITY_STATUS"
            Me.cmb_estado.DataBind()

            Me.cmb_mecanismo_contratacion.DataSourceID = ""
            Me.cmb_mecanismo_contratacion.DataSource = dbEntities.tme_mecanismo_contratacion.Where(Function(p) p.id_programa = Me.cmb_programa.SelectedValue).ToList()
            Me.cmb_mecanismo_contratacion.DataTextField = "nombre_mecanismo_contratacion"
            Me.cmb_mecanismo_contratacion.DataValueField = "id_mecanismo_contratacion"
            Me.cmb_mecanismo_contratacion.DataBind()
            Me.cmb_mecanismo_contratacion.Items.Item(0).Selected = True

            Me.cmb_sub_mecanismo_contratacion.DataSourceID = ""
            Me.cmb_sub_mecanismo_contratacion.DataSource = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_mecanismo_contratacion = Me.cmb_mecanismo_contratacion.SelectedValue).ToList()
            Me.cmb_sub_mecanismo_contratacion.DataTextField = "nombre_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion.DataValueField = "id_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion.DataBind()
            Me.cmb_sub_mecanismo_contratacion.Items.Item(0).Selected = True

            ''************************************************************************************

            Dim fechaReg = Date.Now
            Dim fehcaTasaCambio = ""
            If Month(fechaReg) > 9 Then
                fehcaTasaCambio = (Year(fechaReg) + 1) & "-" & Month(fechaReg) & "-" & Day(fechaReg)
                fechaReg = Convert.ToDateTime(fehcaTasaCambio)
            End If

            Dim oTasaCambio = dbEntities.t_trimestre_tasa_cambio.Where(Function(p) p.mes = Month(fechaReg) And p.t_trimestre.anio = Year(fechaReg) And p.id_programa = idPrograma).ToList()
            If oTasaCambio.Count() > 0 Then
                Me.txt_exchange_rate.Value = oTasaCambio.FirstOrDefault.tasa_cambio
                Me.txt_exchange_rate.EmptyMessage = oTasaCambio.FirstOrDefault.tasa_cambio
            End If


        End Using

    End Sub


    Sub LoadData()

        '***TMP***
        Me.txt_codigoproyecto.Text = cl_listados.CrearCodigoFichaACT(Me.cmb_programa.SelectedValue, If(chk_todosII.Checked, -1, Me.cmb_subregionII.SelectedValue), Me.cmb_mecanismo_contratacion.SelectedValue, If(Val(Me.cmb_sub_mecanismo_contratacion.SelectedValue) > 0, Me.cmb_sub_mecanismo_contratacion.SelectedValue, -1), If(Me.cmb_activity_father.SelectedValue IsNot Nothing, If(Val(Me.cmb_activity_father.SelectedValue) = 0, -1, Me.cmb_activity_father.SelectedValue), -1))

        Me.divCodigo.Visible = True
        Me.lbl_mensaje.Visible = True
        Me.lbl_mensaje.Text = Me.txt_codigoproyecto.Text

    End Sub

    Protected Sub Eliminar_Click(sender As Object, e As EventArgs)
        Dim eliminar = CType(sender, LinkButton)
        Me.identity.Text = eliminar.Attributes.Item("data-identity").ToString()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub

    Protected Sub ctrl_id_municipio_CheckedChanged(sender As Object, e As EventArgs)
        For Each row In Me.grd_district.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim selected As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_municipio"), CheckBox)
                Dim txt_loe As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)

                If selected.Checked Then
                    If txt_loe.Enabled = False Then
                        txt_loe.Value = 0
                    End If
                    txt_loe.Enabled = True
                Else
                    txt_loe.Enabled = False
                    txt_loe.Value = 0
                End If
            End If
        Next
        'sumarLOE()
    End Sub


    Protected Sub txt_nivel_cobertura_TextChanged(sender As Object, e As EventArgs)
        'sumarLOE()
    End Sub

    Protected Sub ctrl_id_CheckedChanged(sender As Object, e As EventArgs)

        Dim id_subregiones = ""

        'For Each row In Me.grd_subregionII.Items
        '    If TypeOf row Is GridDataItem Then
        '        Dim dataItem As GridDataItem = CType(row, GridDataItem)
        '        Dim selected As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
        '        Dim txt_loe As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)

        '        If selected.Checked Then
        '            Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
        '            id_subregiones &= idSubregion.ToString() & ","
        '        End If
        '    End If
        'Next

        'id_subregiones = id_subregiones.TrimEnd(",")

        'Dim ids = id_subregiones.Split(",")
        'Me.grd_district.DataSource = ""
        'Me.grd_district.DataSource = db.vw_t_village.Where(Function(p) ids.Contains(p.id_subregion)).ToList()
        'Me.grd_district.DataBind()
        ''sumarLOE()
    End Sub


    Protected Sub cmb_activity_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        'set the initial footer label
        CType(cmb_activity_father.Footer.FindControl("RadComboItemsCount"), Literal).Text = Convert.ToString(cmb_activity_father.Items.Count)

    End Sub

    Protected Sub cmb_activity_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
        'set the Text and Value property of every item
        'here you can set any other properties like Enabled, ToolTip, Visible, etc.

        Dim DateINI As Date = CType((DirectCast(e.Item.DataItem, DataRowView))("fecha_inicio_proyecto").ToString(), Date)
        Dim DateFIN As Date = CType((DirectCast(e.Item.DataItem, DataRowView))("fecha_fin_proyecto").ToString(), Date)

        'codigo_SAPME
        'fecha_inicio_proyecto
        'fecha_fin_proyecto
        'id_ficha_proyecto
        'nombre_proyecto

        e.Item.Text = String.Format(" {0} ==>> {1} ==>> {2:d} ==>> {3:d} ", (DirectCast(e.Item.DataItem, DataRowView))("codigo_SAPME").ToString(), (DirectCast(e.Item.DataItem, DataRowView))("nombre_proyecto").ToString(), String.Format("{0:d}", DateINI), String.Format("{0:d}", DateFIN))
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_activity").ToString()

    End Sub

    Protected Sub cmb_activity_father_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        'Handles cmb_activity_father.SelectedIndexChanged

        If e.Value IsNot Nothing Then
            LoadData()
        End If

    End Sub


    Protected Sub chk_todosII_CheckedChanged(sender As Object, e As EventArgs) Handles chk_todosII.CheckedChanged
        If chk_todosII.Checked Then
            Me.grd_subregionII.Visible = True
            Me.cmb_subregionII.Visible = False
        Else
            Me.grd_subregionII.Visible = False
            Me.cmb_subregionII.Visible = True
        End If
        LoadData()
    End Sub

    Protected Sub cmb_mecanismo_contratacion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_mecanismo_contratacion.SelectedIndexChanged


        Using dbEntities As New dbRMS_JIEntities

            If e.Value IsNot Nothing Then

                Me.cmb_sub_mecanismo_contratacion.DataSourceID = ""
                Me.cmb_sub_mecanismo_contratacion.DataSource = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_mecanismo_contratacion = e.Value).ToList()
                Me.cmb_sub_mecanismo_contratacion.DataTextField = "nombre_sub_mecanismo"
                Me.cmb_sub_mecanismo_contratacion.DataValueField = "id_sub_mecanismo"
                Me.cmb_sub_mecanismo_contratacion.DataBind()
                LoadData()

            Else
                LoadData()
            End If



        End Using

    End Sub


    Private Sub cmb_sub_mecanismo_contratacion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_sub_mecanismo_contratacion.SelectedIndexChanged

        Using dbEntities As New dbRMS_JIEntities


            If e.Value IsNot Nothing Then

                'Dim idSub_Father = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = e.Value) _
                '    .Select(Function(s) s.id_sub_father).ToList()

                Dim idSub_Father = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = e.Value).FirstOrDefault.id_sub_father


                If idSub_Father IsNot Nothing Then 'It´s linking to a respective sub contract

                    '******************TMP*********************************
                    Dim cls_util As New ly_SIME.CORE.cls_util
                    Me.cmb_activity_father.DataSourceID = ""
                    ''Me.cmb_activity_father.DataSource = cls_util.ConvertToDataTable(dbEntities.vw_tme_ficha_proyecto.Where(Function(p) idSub_Father.Contains(p.id_sub_mecanismo)).OrderBy(Function(o) o.perfijo_sub_mecanismo).ToList())
                    Me.cmb_activity_father.DataSource = cls_util.ConvertToDataTable(dbEntities.VW_TA_ACTIVITY.Where(Function(p) p.id_sub_mecanismo = idSub_Father).ToList())
                    'Me.lblt_actividad_padre.Text = dbEntities.tme_sub_mecanismo.Where(Function(p) idSub_Father.Contains(p.id_sub_mecanismo)).FirstOrDefault.nombre_sub_mecanismo
                    Me.lblt_actividad_padre.Text = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = idSub_Father).FirstOrDefault.nombre_sub_mecanismo
                    Me.cmb_activity_father.DataBind()
                    Me.ly_activity.Visible = True
                    '******************TMP*********************************
                Else

                    Me.ly_activity.Visible = False

                End If


            End If

        End Using

        LoadData()
    End Sub

    Private Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click

        Dim idFicha = 0

        'sumarLOE()

        'valorSuma = 100
        'If (valorSuma <> 100 And chk_todos.Checked) Or (chk_todos.Checked And cero = True) Then
        '    Me.div_mensaje.Visible = True
        '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncHide()", True)
        'Else

        Try



            Using dbEntities As New dbRMS_JIEntities

                Dim bndError As Boolean = False
                Dim idSub_Father = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = Me.cmb_sub_mecanismo_contratacion.SelectedValue).FirstOrDefault.id_sub_father

                If idSub_Father IsNot Nothing Then 'It´s linking to a respective sub contract

                    If Me.cmb_activity_father.SelectedValue IsNot Nothing Then
                        If Val(Me.cmb_activity_father.SelectedValue) = 0 Then
                            bndError = True
                        Else
                            bndError = False
                        End If
                    Else
                        bndError = True
                    End If

                End If






                If Not bndError Then


                    Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
                    Me.lbl_Activity_error.Visible = False
                    Dim oFicha = New TA_ACTIVITY

                    oFicha.codigo_SAPME = Me.txt_codigoproyecto.Text
                    oFicha.codigo_ficha_AID = Me.txt_codigoproyecto.Text
                    oFicha.nombre_proyecto = Me.txt_nombreproyecto.Text
                    oFicha.area_intervencion = Me.txt_descripcion.Text
                    'oFicha.id_mecanismo_contratacion = Me.cmb_mecanismo_contratacion.SelectedValue
                    oFicha.id_sub_mecanismo = Me.cmb_sub_mecanismo_contratacion.SelectedValue

                    ''oFicha.id_ejecutor = Me.cmb_ejecutor.SelectedValue
                    oFicha.id_usuario_responsable = Me.cmb_persona_encargada.SelectedValue

                    'If Me.cmb_componente.SelectedValue <> "" Then
                    '    oFicha.id_componente = Me.cmb_componente.SelectedValue
                    'End If

                    oFicha.fecha_inicio_proyecto = Me.dt_fecha_inicio.SelectedDate
                    oFicha.fecha_fin_proyecto = Me.dt_fecha_fin.SelectedDate
                    oFicha.codigo_RFA = Me.txt_codigoRFA.Text
                    oFicha.codigo_MONITOR = Me.txt_codigoMonitor.Text

                    'If Me.id_documento.Value <> 0 Then
                    '    oFicha.id_documento = Me.id_documento.Value
                    'End If

                    'oFicha.id_district = Me.cmb_district.SelectedValue

                    'oFicha.aportes_actualizados = "NO"
                    'oFicha.ActualizacionReciente = "NO"
                    oFicha.id_periodo = Me.cmb_periodo.SelectedValue
                    oFicha.ID_ACTIVITY_STATUS = 1
                    oFicha.id_programa = id_programa

                    Dim idPrograma = Convert.ToInt32(Me.cmb_programa.SelectedValue)
                    Dim fechaReg = Date.Now
                    Dim fehcaTasaCambio = ""
                    If Month(fechaReg) > 9 Then
                        fehcaTasaCambio = (Year(fechaReg) + 1) & "-" & Month(fechaReg) & "-" & Day(fechaReg)
                        fechaReg = Convert.ToDateTime(fehcaTasaCambio)
                    End If

                    'String.Format(cl_user.regionalizacionCulture, "{1} {0:N2}",dtRow("valor"),cl_user.regionalizacionCulture.NumberFormat.CurrencySymbol)


                    Dim oTasaCambio = dbEntities.t_trimestre_tasa_cambio.Where(Function(p) p.mes = Month(fechaReg) And p.t_trimestre.anio = Year(fechaReg)).ToList()
                    If txt_exchange_rate.Value > 0 Then
                        oFicha.tasa_cambio = txt_exchange_rate.Value
                    Else
                        If oTasaCambio.Count > 0 Then
                            oFicha.tasa_cambio = oTasaCambio.FirstOrDefault().tasa_cambio
                        Else
                            oFicha.tasa_cambio = 0
                        End If
                    End If

                    ' oFicha.OBLIGATED_AMOUNT = Me.txt_tot_amount.Text
                    ' oFicha.OBLIGATED_AMOUNT_LOC = Me.txt_tot_amount.Text

                    oFicha.costo_total_proyecto = Me.txt_tot_amount.Value
                    oFicha.costo_total_proyecto_LOC = Me.txt_tot_amount_Local.Value


                    oFicha.id_usuario_creo = Me.Session("E_IdUser").ToString()
                    oFicha.id_usuario_update = Me.Session("E_IdUser").ToString()
                    oFicha.datecreated = Date.UtcNow
                    oFicha.dateUpdate = Date.UtcNow
                    oFicha.id_subregion = Me.cmb_subregionII.SelectedValue
                    oFicha.USER_TOKEN_UPDATE = Guid.Parse(Me.Session("idGuiToken").ToString)

                    If Not (Me.cmb_activity_father.SelectedValue Is Nothing) Then
                        If Val(Me.cmb_activity_father.SelectedValue) > 0 Then
                            oFicha.id_ficha_padre = Convert.ToInt32(Me.cmb_activity_father.SelectedValue)
                        Else
                            oFicha.id_ficha_padre = Nothing
                        End If
                    End If


                    ''******************THINKING ABOUT***************************
                    '' oFicha.tme_ficha_historico_estado.Add(cl_listados.createFichaHistorico(1, cl_user.id_usr))


                    dbEntities.TA_ACTIVITY.Add(oFicha)
                    dbEntities.SaveChanges()

                    Dim boolAdded As Boolean = False

                    If chk_todosII.Checked Then
                        For Each row In Me.grd_subregionII.Items
                            If TypeOf row Is GridDataItem Then
                                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                                Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                                Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
                                If subR.Checked = True Then
                                    Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
                                    Dim oSubregion = New TA_ACTIVITY_SUBREGION
                                    oSubregion.id_subregion = idSubregion
                                    oSubregion.id_activity = oFicha.id_activity
                                    oSubregion.nivel_cobertura = nivel_cobertura.Value
                                    oFicha.TA_ACTIVITY_SUBREGION.Add(oSubregion)

                                    boolAdded = True

                                End If
                            End If
                        Next
                    End If

                    If Not boolAdded Then

                        Dim oSubregion = New TA_ACTIVITY_SUBREGION
                        oSubregion.id_subregion = Me.cmb_subregionII.SelectedValue
                        oSubregion.id_activity = oFicha.id_activity
                        oSubregion.nivel_cobertura = 100
                        oFicha.TA_ACTIVITY_SUBREGION.Add(oSubregion)

                    End If

                    ''''For Each row In Me.grd_district.Items
                    ''''    If TypeOf row Is GridDataItem Then
                    ''''        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    ''''        Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_municipio"), CheckBox)
                    ''''        Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
                    ''''        If subR.Checked = True Then
                    ''''            Dim id_municipio As Integer = dataItem.GetDataKeyValue("id_municipio")
                    ''''            Dim oDistrict = New tme_ficha_municipio
                    ''''            oDistrict.id_municipio = id_municipio
                    ''''            oDistrict.id_ficha_proyecto = oFicha.id_ficha_proyecto
                    ''''            oDistrict.nivel_cobertura = nivel_cobertura.Value
                    ''''            oFicha.tme_ficha_municipio.Add(oDistrict)
                    ''''        End If
                    ''''    End If
                    ''''Next

                    'For Each file As UploadedFile In AsyncUpload1.UploadedFiles
                    '    Dim nombreArchivo = cl_listados.getNewName(file, Me.Session("E_IdUser").ToString())
                    '    Dim oImagen = New tme_FichaProyectoImagen
                    '    oImagen.id_ficha_proyecto = oFicha.id_ficha_proyecto
                    '    oImagen.nombre_archivo_proyecto = nombreArchivo
                    '    oImagen.id_tipo_proyecto_imagen = 1

                    '    dbEntities.tme_FichaProyectoImagen.Add(oImagen)
                    '    Dim Path As String
                    '    Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = idPrograma).images_folder)
                    '    file.SaveAs(Path + nombreArchivo)
                    'Next

                    'dbEntities.Entry(oFicha).State = Entity.EntityState.Modified
                    'dbEntities.SaveChanges()
                    'idFicha = oFicha.id_ficha_proyecto
                    'Dim oUsuaAct = New t_usuario_ficha_proyecto
                    'oUsuaAct.id_usuario = Me.cmb_persona_encargada.SelectedValue
                    'oUsuaAct.id_ficha_proyecto = idFicha
                    'oUsuaAct.fecha_crea = DateTime.Now
                    'oUsuaAct.id_usuario_crea = oFicha.id_usuario_creo
                    'oUsuaAct.acc_act = True
                    'dbEntities.t_usuario_ficha_proyecto.Add(oUsuaAct)
                    'dbEntities.SaveChanges()

                    dbEntities.SaveChanges()

                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    Me.MsgGuardar.Redireccion = "~/RFP_/frm_ActivitySolicitation?id=" & oFicha.id_activity
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                Else
                    Me.lbl_Activity_error.Visible = True
                End If

            End Using


        Catch ex As Exception

            Me.MsgGuardar.NuevoMensaje = ex.Message & Chr(13) & "   " & ex.InnerException.Message
            Me.MsgGuardar.Redireccion = ""
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End Try

    End Sub

    Private Sub btn_salir_Click(sender As Object, e As EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/RFP_/frm_Activities")
    End Sub
End Class