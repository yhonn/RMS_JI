Imports ly_SIME
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports CuteWebUI
Imports System.IO
Imports System.Configuration.ConfigurationManager

Public Class frm_proyectosEdit
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_listados As New ly_SIME.CORE.cls_listados
    Dim frmCODE As String = "AP_EDIT_PROY"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim valorSuma As Decimal = 0
    Dim cero = False

    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
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
            Me.cmb_programa.Enabled = False

            'Me.lbl_id_sesion_temp.Text = cl_listados.CodigoRandom()
            Using dbEntities As New dbRMS_JIEntities
                Dim id = Convert.ToInt32(Me.Request.QueryString("Id").ToString)
                Me.lbl_id_ficha.Text = id
                Dim proyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = id)

                Me.lbl_informacionProyecto.Text = "(" + proyecto.codigo_ficha_AID + ")" + " " + proyecto.nombre_proyecto
                loadListas(idPrograma, proyecto)

                'LoadPErsonalTecnico()
                LoadData(id)
                'Me.alink_regionbeneficiada.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosRegion?Id=" & id.ToString()))
                'Me.alink_value_chain.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosValueChain?Id=" & id.ToString()))
                'Me.alink_areas.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_actividad_areas?Id=" & id.ToString()))
                'Me.alink_indicadores.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosIndicadores?Id=" & id.ToString()))
                'Me.alink_documentos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosDocumentos?Id=" & id.ToString()))
                'Me.alink_predios.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosPredios?Id=" & id.ToString()))
                'Me.alink_waiver.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosWaiver?Id=" & id.ToString()))
                Me.alink_aportes.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosAportes?Id=" & id.ToString()))
                Me.link_ruta.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyecto_ruta_aprobacion?Id=" & id.ToString()))
                'If proyecto.tasa_cambio_trimestre IsNot Nothing Then
                '    Me.alink_aportes.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosAportes?Id=" & id.ToString()))
                'Else
                '    Me.alink_aportes.Attributes.Add("href", "#modalTasaCambio")
                '    Me.alink_aportes.Attributes.Add("data-toggle", "modal")
                'End If

                'Me.alink_entregables.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosEntregables?Id=" & id.ToString()))

                'Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString()))

                Dim oPro = dbEntities.tme_Ficha_Proyecto.Find(proyecto.id_ficha_proyecto)
                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo <> "IQS" Then
                '    Me.alink_stos.Attributes.Add("style", "display:none;")
                'End If
                'Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString()))

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************

                Dim oSubFather As Object = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_father = oPro.tme_sub_mecanismo.id_sub_mecanismo).ToList()

                'Me.alink_stos.Attributes.Add("style", "display:none;")
                'Me.alink_po.Attributes.Add("style", "display:none;")
                'Me.alink_Ik.Attributes.Add("style", "display:none;")

                'Me.alink_stos.Attributes.Add("href", "#")
                'Me.alink_po.Attributes.Add("href", "#")
                'Me.alink_Ik.Attributes.Add("href", "#")

                Dim i = 0
                If oSubFather.count > 0 Then

                    'For Each item In oSubFather

                    '    If i = 0 Then
                    '        Me.alink_stos.InnerText = item.perfijo_sub_mecanismo
                    '        Me.alink_stos.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString() & "&tp=" & item.perfijo_sub_mecanismo))
                    '        Me.alink_stos.Attributes.Add("style", "display:block;")
                    '    ElseIf i = 1 Then
                    '        Me.alink_po.InnerText = item.perfijo_sub_mecanismo
                    '        Me.alink_po.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString() & "&tp=" & item.perfijo_sub_mecanismo))
                    '        Me.alink_po.Attributes.Add("style", "display:block;")
                    '    Else
                    '        Me.alink_Ik.InnerText = item.perfijo_sub_mecanismo
                    '        Me.alink_Ik.Attributes.Add("href", ResolveUrl("~/Proyectos/frm_proyectosSTO?Id=" & id.ToString() & "&tp=IK"))
                    '        Me.alink_Ik.Attributes.Add("style", "display:block;")
                    '        Me.alink_Ik.Attributes.Add("style", "display:block;")
                    '    End If

                    '    i += 1

                    'Next

                End If

                'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo = "PO" Then
                '    Me.alink_areas.Attributes.Add("href", "#")
                '    Me.alink_areas.Attributes.Add("style", "display:none;")
                '    Me.alink_indicadores.Attributes.Add("href", "#")
                '    Me.alink_indicadores.Attributes.Add("style", "display:none;")
                '    Me.alink_waiver.Attributes.Add("href", "#")
                '    Me.alink_waiver.Attributes.Add("style", "display:none;")
                'End If

                '****************************************************************************************************************************
                '****************************************************************************************************************************
                '****************************************************************************************************************************


            End Using



        End If
    End Sub

    Sub loadListas(ByVal idPrograma As Integer, ByVal oProyecto As vw_tme_ficha_proyecto)
        Using dbEntities As New dbRMS_JIEntities


            Me.txt_codigo_SAPME.Text = oProyecto.codigo_SAPME
            Me.lbl_mensaje.Text = oProyecto.codigo_SAPME

            Me.txt_codigoproyecto.Text = oProyecto.codigo_ficha_AID
            Me.divCodigo.Visible = True
            Me.lbl_mensaje.Visible = True

            Me.txt_nombreproyecto.Text = oProyecto.nombre_proyecto
            Me.txt_codigoMonitor.Text = oProyecto.codigo_MONITOR
            Me.txt_descripcion.Text = oProyecto.area_intervencion
            Me.dt_fecha_inicio.SelectedDate = oProyecto.fecha_inicio_proyecto
            Me.dt_fecha_fin.SelectedDate = oProyecto.fecha_fin_proyecto
            Me.txt_codigoRFA.Text = oProyecto.codigo_RFA

            'Me.txt_key_issues.Text = oProyecto.keyissues

            Dim entryPoint = From u In db.t_usuarios Join q In db.t_usuario_programa On u.id_usuario Equals q.id_usuario
                             Where q.id_programa = idPrograma And u.id_estado_usr = 1 And u.id_tipo_usuario = 1
                             Select New With {Key .id_usuario = u.id_usuario, Key .nombre = u.nombre_usuario & " " & u.apellidos_usuario}


            Me.cmb_persona_encargada.DataSourceID = ""
            Me.cmb_persona_encargada.DataSource = entryPoint.ToList()
            Me.cmb_persona_encargada.DataTextField = "nombre"
            Me.cmb_persona_encargada.DataValueField = "id_usuario"
            Me.cmb_persona_encargada.DataBind()
            Me.cmb_persona_encargada.SelectedValue = oProyecto.id_usuario_responsable

            'Me.cmb_area_intervension.DataSourceID = ""
            'Me.cmb_area_intervension.DataSource = dbEntities.tme_area_intervencion_programa.ToList()
            'Me.cmb_area_intervension.DataTextField = "nombre_area"
            'Me.cmb_area_intervension.DataValueField = "id_area_intervencion"
            'Me.cmb_area_intervension.DataBind()

            'Me.cmb_responsable_componente.DataSourceID = ""
            'Me.cmb_responsable_componente.DataSource = entryPoint.Where(Function(p) p.busqueda_actividad = True).ToList()
            'Me.cmb_responsable_componente.DataTextField = "nombre"
            'Me.cmb_responsable_componente.DataValueField = "id_usuario"
            'Me.cmb_responsable_componente.DataBind()

            'Me.cmb_personal_tecnico.DataSourceID = ""
            'Me.cmb_personal_tecnico.DataSource = entryPoint.Where(Function(p) p.busqueda_actividad = True).ToList()
            'Me.cmb_personal_tecnico.DataTextField = "nombre"
            'Me.cmb_personal_tecnico.DataValueField = "id_usuario"
            'Me.cmb_personal_tecnico.DataBind()

            'Me.cmb_grupo_asociados.DataSourceID = ""
            'Me.cmb_grupo_asociados.DataSource = db.tme_grupo_aprobacion_hitos.ToList()
            'Me.cmb_grupo_asociados.DataTextField = "grupo_aprobacion"
            'Me.cmb_grupo_asociados.DataValueField = "id_grupo_aprobacion_hitos"
            'Me.cmb_grupo_asociados.DataBind()

            'grupo_asociado.Visible = False
            'Dim idSubR = oProyecto.id_subregion.Split(",").ToList().ConvertAll(Function(x) Convert.ToInt32(x))
            'If idSubR.Contains(2) Then
            '    grupo_asociado.Visible = False
            'End If

            'If idSubR.Contains(1) Then
            '    grupo_asociado.Visible = True
            'End If

            'If oProyecto.id_subregion <> 1 Then
            '    grupo_asociado.Visible = False
            'Else
            '    grupo_asociado.Visible = True
            'End If

            'If oProyecto.id_usuario_responsable > 0 Then
            '    Me.cmb_persona_encargada.SelectedValue = oProyecto.id_usuario_responsable
            'End If

            'If oProyecto.id_responsable_componente > 0 Then
            '    Me.cmb_responsable_componente.SelectedValue = oProyecto.id_responsable_componente
            'End If

            'If oProyecto.id_personal_tecnico > 0 Then
            '    Me.cmb_personal_tecnico.SelectedValue = oProyecto.id_personal_tecnico
            'End If
            Me.cmb_ejecutor.DataSourceID = ""
            Me.cmb_ejecutor.DataSource = cl_listados.get_t_ejecutores(idPrograma)
            Me.cmb_ejecutor.DataTextField = "nombre_ejecutor"
            Me.cmb_ejecutor.DataValueField = "id_ejecutor"
            Me.cmb_ejecutor.DataBind()
            Me.cmb_ejecutor.SelectedValue = oProyecto.id_ejecutor

            Me.cmb_region.DataSourceID = ""
            Me.cmb_region.DataSource = cl_listados.get_t_regiones(Convert.ToInt32(Me.cmb_programa.SelectedValue))
            Me.cmb_region.DataTextField = "nombre_region"
            Me.cmb_region.DataValueField = "id_region"
            Me.cmb_region.DataBind()
            'Me.cmb_region.SelectedValue = oProyecto.id_region
            Me.grd_subregion.DataSource = ""
            Me.grd_subregion.DataSource = db.vw_t_subregiones.Where(Function(p) p.id_programa = Me.cmb_programa.SelectedValue).ToList()
            Me.grd_subregion.DataBind()

            If Not oProyecto.id_region.Contains(",") Then
                Me.cmb_region.SelectedValue = Convert.ToInt32(oProyecto.id_region)
                Me.cmb_subregion.DataSourceID = ""
                Me.cmb_subregion.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
                Me.cmb_subregion.DataTextField = "nombre_subregion"
                Me.cmb_subregion.DataValueField = "id_subregion"
                Me.cmb_subregion.DataBind()
                Me.cmb_subregion.SelectedValue = Convert.ToInt32(oProyecto.id_subregion)
            Else
                Me.cmb_subregion.DataSourceID = ""
                Me.cmb_subregion.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
                Me.cmb_subregion.DataTextField = "nombre_subregion"
                Me.cmb_subregion.DataValueField = "id_subregion"
                Me.cmb_subregion.DataBind()
                Me.cmb_subregion.SelectedValue = Convert.ToInt32(oProyecto.id_subregion.Split(",")(0))

                Me.chk_todos.Checked = True
                Me.grd_subregion.Visible = True
                Me.cmb_subregion.Visible = False

                For Each item In db.tme_ficha_subregion.Where(Function(p) p.id_ficha_proyecto = oProyecto.id_ficha_proyecto)
                    For Each row In Me.grd_subregion.Items
                        If TypeOf row Is GridDataItem Then
                            Dim dataItem As GridDataItem = CType(row, GridDataItem)
                            Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                            Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
                            Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
                            If item.id_subregion = idSubregion Then
                                subR.Checked = True
                                nivel_cobertura.Value = item.nivel_cobertura

                                If item.nivel_cobertura > 0 Then
                                    nivel_cobertura.Enabled = True
                                End If
                            End If
                        End If
                    Next
                Next

            End If

            Dim subregiones = oProyecto.id_subregion.Replace(" ", "").Split(",")
            Me.grd_district.DataSource = ""
            Me.grd_district.DataSource = db.vw_tme_municipios.Where(Function(p) subregiones.Contains(p.id_subregion.ToString())).ToList()
            Me.grd_district.DataBind()

            For Each item In db.tme_ficha_municipio.Where(Function(p) p.id_ficha_proyecto = oProyecto.id_ficha_proyecto)
                For Each row In Me.grd_district.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_municipio"), CheckBox)
                        Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
                        Dim id_municipio As Integer = dataItem.GetDataKeyValue("id_municipio")
                        If item.id_municipio = id_municipio Then
                            subR.Checked = True
                            nivel_cobertura.Value = item.nivel_cobertura

                            If item.nivel_cobertura > 0 Then
                                nivel_cobertura.Enabled = True
                            End If
                        End If
                    End If
                Next
            Next

            'Me.cmb_componente.DataSourceID = ""
            'Me.cmb_componente.DataSource = cl_listados.get_tme_componente_programa(Convert.ToInt32(Me.cmb_programa.SelectedValue))
            'Me.cmb_componente.DataTextField = "nombre_componente"
            'Me.cmb_componente.DataValueField = "id_componente"
            'Me.cmb_componente.DataBind()

            'If oProyecto.id_componente IsNot Nothing Then
            '    Me.cmb_componente.SelectedValue = oProyecto.id_componente
            'End If

            Me.cmb_periodo.DataSourceID = ""
            Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregion.SelectedValue))
            Me.cmb_periodo.DataTextField = "nombre_periodo"
            Me.cmb_periodo.DataValueField = "id_periodo"
            Me.cmb_periodo.DataBind()
            Me.cmb_periodo.SelectedValue = oProyecto.id_periodo


            'If oProyecto.id_area_intervencion IsNot Nothing Then
            '    Me.cmb_area_intervension.SelectedValue = oProyecto.id_area_intervencion
            'End If


            'Dim id_tmp = Convert.ToInt64(Me.lbl_id_sesion_temp.Text)
            Dim oPro = dbEntities.tme_Ficha_Proyecto.Find(oProyecto.id_ficha_proyecto)

            'If oProyecto.isprivatepublic.HasValue Then
            '    If oProyecto.isprivatepublic.Value Then
            '        Me.rbn_private_public.SelectedValue = 1
            '        Me.grd_partners.Enabled = True

            '        'listPartners = oPro.tme_ficha_partner.ToList()
            '        'grd_partners.Rebind()
            '    Else
            '        Me.rbn_private_public.SelectedValue = 2
            '    End If
            'Else
            '    Me.rbn_private_public.SelectedValue = 2
            'End If

            Me.cmb_estado.DataSourceID = ""
            Me.cmb_estado.DataSource = cl_listados.get_tme_FichaEstado()
            Me.cmb_estado.DataTextField = "nombre_estado_ficha"
            Me.cmb_estado.DataValueField = "id_ficha_estado"
            Me.cmb_estado.DataBind()
            Me.cmb_estado.SelectedValue = oPro.id_ficha_estado

            Me.cmb_mecanismo_contratacion.DataSourceID = ""
            Me.cmb_mecanismo_contratacion.DataSource = dbEntities.tme_mecanismo_contratacion.Where(Function(p) p.id_programa = Me.cmb_programa.SelectedValue).ToList()
            Me.cmb_mecanismo_contratacion.DataTextField = "nombre_mecanismo_contratacion"
            Me.cmb_mecanismo_contratacion.DataValueField = "id_mecanismo_contratacion"
            Me.cmb_mecanismo_contratacion.DataBind()
            Me.cmb_mecanismo_contratacion.SelectedValue = oPro.tme_sub_mecanismo.id_mecanismo_contratacion

            Me.cmb_sub_mecanismo_contratacion.DataSourceID = ""
            Me.cmb_sub_mecanismo_contratacion.DataSource = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_mecanismo_contratacion = Me.cmb_mecanismo_contratacion.SelectedValue).ToList()
            Me.cmb_sub_mecanismo_contratacion.DataTextField = "nombre_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion.DataValueField = "id_sub_mecanismo"
            Me.cmb_sub_mecanismo_contratacion.DataBind()
            Me.cmb_sub_mecanismo_contratacion.SelectedValue = oPro.tme_sub_mecanismo.id_sub_mecanismo

            'If oPro.tme_sub_mecanismo.perfijo_sub_mecanismo <> "IQS" Then
            '    Me.alink_stos.Attributes.Add("style", "display:none;")
            'End If

            Dim idSub_Father = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = oPro.tme_sub_mecanismo.id_sub_mecanismo).FirstOrDefault.id_sub_father

            If idSub_Father IsNot Nothing Then 'It´s linking to a respective sub contract

                Dim cls_util As New ly_SIME.CORE.cls_util
                Me.cmb_activity_father.DataSourceID = ""
                Me.cmb_activity_father.DataSource = cls_util.ConvertToDataTable(dbEntities.vw_tme_ficha_proyecto.Where(Function(p) p.id_sub_mecanismo = idSub_Father).ToList())
                'Me.cmb_activity_father.DataTextField = "nombre_sub_mecanismo"
                'Me.cmb_actividad_father.DataValueField = "id_sub_mecanismo"
                Me.lblt_actividad_padre.Text = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = idSub_Father).FirstOrDefault.nombre_sub_mecanismo
                Me.cmb_activity_father.DataBind()
                Me.ly_activity.Visible = True

                If Val(oPro.id_ficha_padre) > 0 Then
                    Me.cmb_activity_father.SelectedValue = oPro.id_ficha_padre
                End If

            Else
                Me.ly_activity.Visible = False
            End If

        End Using

    End Sub
    Sub LoadData(ByVal id As Integer)
        'Me.txt_codigoproyecto.Text = clListados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, Me.cmb_componente.SelectedValue)

        Using dbEntities As New dbRMS_JIEntities
            Dim oProyecto = dbEntities.tme_Ficha_Proyecto.Find(id)
            If oProyecto.id_ficha_estado > 1 Then
                'Me.lbl_alerta.Text = "El proyecto esta en ejecución o finalizado, no lo puede editar."
                'Me.btn_guardar.Enabled = False
            End If

        End Using
    End Sub
    Protected Sub cmb_region_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_region.SelectedIndexChanged
        Me.cmb_subregion.DataSourceID = ""
        Me.cmb_subregion.DataSource = cl_listados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
        Me.cmb_subregion.DataTextField = "nombre_subregion"
        Me.cmb_subregion.DataValueField = "id_subregion"
        Me.cmb_subregion.DataBind()
        If Not chk_todos.Checked Then
            Me.grd_district.DataSource = ""
            Me.grd_district.DataSource = db.vw_tme_municipios.Where(Function(p) p.id_subregion = Me.cmb_subregion.SelectedValue).ToList()
            Me.grd_district.DataBind()
        End If
    End Sub

    'Protected Sub lnk_sugerir_codigo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_sugerir_codigo.Click
    '    Me.txt_codigoproyecto.Text = cl_listados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, Me.cmb_componente.SelectedValue)
    '    Me.lbl_mensaje.Text = cl_listados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, Me.cmb_componente.SelectedValue)
    'End Sub

    Protected Sub cmb_subregion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_subregion.SelectedIndexChanged
        Dim sql = ""
        Me.cmb_periodo.DataSourceID = ""
        Me.cmb_periodo.DataSource = cl_listados.get_t_periodo(Convert.ToInt32(Me.cmb_subregion.SelectedValue))
        Me.cmb_periodo.DataBind()
        If Not chk_todos.Checked Then
            Me.grd_district.DataSource = ""
            Me.grd_district.DataSource = db.vw_tme_municipios.Where(Function(p) p.id_subregion = Me.cmb_subregion.SelectedValue).ToList()
            Me.grd_district.DataBind()
        End If
    End Sub
    'Protected Sub btn_add_personal_clcik(sender As Object, e As EventArgs) Handles btn_add_personal.Click
    '    Using dbEntities As New dbRMS_JIEntities
    '        Dim id_ficha = Convert.ToInt32(Me.lbl_id_ficha.Text)
    '        Dim personalTecnico = New tme_ficha_proyecto_personal_tecnico
    '        personalTecnico.id_usuario = Me.cmb_personal_tecnico.SelectedValue
    '        personalTecnico.id_ficha_proyecto = id_ficha
    '        personalTecnico.es_grupo = False
    '        dbEntities.tme_ficha_proyecto_personal_tecnico.Add(personalTecnico)
    '        dbEntities.SaveChanges()
    '        LoadPErsonalTecnico()
    '    End Using
    'End Sub

    'Protected Sub btn_add_grupo_clcik(sender As Object, e As EventArgs) Handles btn_add_grupo.Click
    '    Using dbEntities As New dbRMS_JIEntities
    '        Dim id_ficha = Convert.ToInt32(Me.lbl_id_ficha.Text)
    '        Dim personalTecnico = New tme_ficha_proyecto_personal_tecnico
    '        personalTecnico.id_grupo_arpobacion_hito = Me.cmb_grupo_asociados.SelectedValue
    '        personalTecnico.id_ficha_proyecto = id_ficha
    '        personalTecnico.es_grupo = True
    '        dbEntities.tme_ficha_proyecto_personal_tecnico.Add(personalTecnico)
    '        dbEntities.SaveChanges()
    '        LoadPErsonalTecnico()
    '    End Using
    'End Sub
    'Sub LoadPErsonalTecnico()
    '    Dim id_ficha = Convert.ToInt32(Me.lbl_id_ficha.Text)
    '    Me.sqlDTS_personal_tecnico.SelectCommand = "select case when isnull(a.es_grupo,0) = 0 then CONCAT(b.nombre_usuario, b.apellidos_usuario) else c.grupo_aprobacion end personal_tecnico, a.id_personal_tecnico from tme_ficha_proyecto_personal_tecnico a 
    '                                                left join t_usuarios b on a.id_usuario = b.id_usuario 
    '                                                left join tme_grupo_aprobacion_hitos c on a.id_grupo_arpobacion_hito = c.id_grupo_aprobacion_hitos 
    '                                                where id_ficha_proyecto = '" & id_ficha & "'"
    '    Me.grd_acciones.DataBind()
    'End Sub


    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Dim id_ficha = Convert.ToInt32(Me.lbl_id_ficha.Text)
        Try

            'sumarLOE()

            valorSuma = 100

            'If (valorSuma <> 100 And chk_todos.Checked) Or (chk_todos.Checked And cero = True) Then
            '    Me.div_mensaje.Visible = True
            '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncHide()", True)
            'Else

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




                    Me.lbl_Activity_error.Visible = False

                    Dim oFicha = dbEntities.tme_Ficha_Proyecto.Find(id_ficha)
                    oFicha.codigo_SAPME = Me.txt_codigoproyecto.Text
                    oFicha.codigo_ficha_AID = Me.txt_codigoproyecto.Text
                    oFicha.nombre_proyecto = Me.txt_nombreproyecto.Text
                    oFicha.area_intervencion = Me.txt_descripcion.Text
                    'oFicha.id_area_intervencion = Me.cmb_area_intervension.SelectedValue

                    'oFicha.id_mecanismo_contratacion = Me.cmb_mecanismo_contratacion.SelectedValue
                    oFicha.id_sub_mecanismo = Me.cmb_sub_mecanismo_contratacion.SelectedValue

                    oFicha.id_ejecutor = Me.cmb_ejecutor.SelectedValue
                    oFicha.id_subregion = Me.cmb_subregion.SelectedValue
                    'If Me.cmb_componente.SelectedValue <> "" Then
                    '    oFicha.id_componente = Me.cmb_componente.SelectedValue
                    'End If
                    oFicha.fecha_inicio_proyecto = Me.dt_fecha_inicio.SelectedDate
                    oFicha.fecha_fin_proyecto = Me.dt_fecha_fin.SelectedDate
                    oFicha.codigo_RFA = Me.txt_codigoRFA.Text
                    oFicha.codigo_MONITOR = Me.txt_codigoMonitor.Text
                    oFicha.id_periodo = Me.cmb_periodo.SelectedValue
                    If Me.cmb_persona_encargada.SelectedValue IsNot Nothing Then
                        oFicha.id_usuario_responsable = Me.cmb_persona_encargada.SelectedValue
                    End If

                    'If Me.cmb_responsable_componente.SelectedValue IsNot Nothing Then
                    '    oFicha.id_responsable_componente = Me.cmb_responsable_componente.SelectedValue
                    'End If
                    'If Me.cmb_personal_tecnico.SelectedValue IsNot Nothing Then
                    '    oFicha.id_personal_tecnico = Me.cmb_personal_tecnico.SelectedValue
                    'End If
                    oFicha.id_ficha_estado = Me.cmb_estado.SelectedValue
                    'oFicha.id_ficha_estado = 1
                    Dim fechaReg = dt_fecha_inicio.SelectedDate
                    Dim fehcaTasaCambio = ""
                    If Month(fechaReg) > 9 Then
                        fehcaTasaCambio = (Year(fechaReg) + 1) & "-" & Month(fechaReg) & "-" & Day(fechaReg)
                        fechaReg = Convert.ToDateTime(fehcaTasaCambio)
                    End If
                    Dim oTasaCambio = dbEntities.t_trimestre_tasa_cambio.Where(Function(p) p.mes = Month(fechaReg) And p.t_trimestre.anio = Year(fechaReg)).ToList()
                    If oTasaCambio.Count() > 0 Then
                        oFicha.tasa_cambio = oTasaCambio.FirstOrDefault().tasa_cambio
                    End If
                    oFicha.id_usuario_update = Me.Session("E_IdUser").ToString()
                    oFicha.dateUpdate = Date.UtcNow

                    If Not (Me.cmb_activity_father.SelectedValue Is Nothing) Then
                        If Val(Me.cmb_activity_father.SelectedValue) > 0 Then
                            oFicha.id_ficha_padre = Convert.ToInt32(Me.cmb_activity_father.SelectedValue)
                        Else
                            oFicha.id_ficha_padre = Nothing
                        End If
                    End If


                    dbEntities.Database.ExecuteSqlCommand("DELETE FROM tme_ficha_subregion where id_ficha_proyecto = " & id_ficha)
                    'dbEntities.Database.ExecuteSqlCommand("DELETE FROM tme_ficha_municipio where id_ficha_proyecto = " & id_ficha)

                    Dim boolAdded As Boolean = False

                    If chk_todos.Checked Then
                        For Each row In Me.grd_subregion.Items
                            If TypeOf row Is GridDataItem Then
                                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                                Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                                Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
                                If subR.Checked = True Then
                                    Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
                                    Dim oSubregion = New tme_ficha_subregion
                                    oSubregion.id_subregion = idSubregion
                                    oSubregion.id_ficha_proyecto = oFicha.id_ficha_proyecto
                                    oSubregion.nivel_cobertura = nivel_cobertura.Value
                                    oFicha.tme_ficha_subregion.Add(oSubregion)

                                    boolAdded = True

                                End If
                            End If
                        Next

                    End If

                    If Not boolAdded Then

                        Dim oSubregion = New tme_ficha_subregion
                        oSubregion.id_subregion = Me.cmb_subregion.SelectedValue
                        oSubregion.id_ficha_proyecto = oFicha.id_ficha_proyecto
                        oSubregion.nivel_cobertura = 100
                        oFicha.tme_ficha_subregion.Add(oSubregion)

                    End If

                    'For Each row In Me.grd_district.Items
                    '    If TypeOf row Is GridDataItem Then
                    '        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    '        Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_municipio"), CheckBox)
                    '        Dim nivel_cobertura As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)
                    '        If subR.Checked = True Then
                    '            Dim id_municipio As Integer = dataItem.GetDataKeyValue("id_municipio")
                    '            Dim oDistrict = New tme_ficha_municipio
                    '            oDistrict.id_municipio = id_municipio
                    '            oDistrict.id_ficha_proyecto = oFicha.id_ficha_proyecto
                    '            oDistrict.nivel_cobertura = nivel_cobertura.Value
                    '            oFicha.tme_ficha_municipio.Add(oDistrict)
                    '        End If
                    '    End If
                    'Next

                    'If Me.rbn_private_public.SelectedValue = 1 Then
                    '    oFicha.isprivatepublic = True
                    '    For Each row In Me.grd_partners.Items
                    '        If TypeOf row Is GridDataItem Then
                    '            Dim dataItem As GridDataItem = CType(row, GridDataItem)

                    '            Dim nombre_partner As TextBox = CType(row.Cells(0).FindControl("txt_nombre_partner"), TextBox)
                    '            Dim partner_type As RadComboBox = CType(row.Cells(0).FindControl("cmb_partner_type"), RadComboBox)
                    '            Dim partnership_focus As RadComboBox = CType(row.Cells(0).FindControl("cmb_partnership_focus"), RadComboBox)


                    '            oFicha.tme_ficha_partner.Add(New tme_ficha_partner() _
                    '                                 With {.id_partner_type = partner_type.SelectedValue, _
                    '                                       .id_partnership_focus = partnership_focus.SelectedValue, _
                    '                                       .nombre_partner = nombre_partner.Text
                    '                                     })

                    '        End If
                    '    Next
                    'Else
                    '    oFicha.isprivatepublic = False
                    'End If

                    For Each file As UploadedFile In AsyncUpload1.UploadedFiles
                        Dim nombreArchivo = cl_listados.getNewName(file, Me.Session("E_IdUser").ToString())
                        Dim oImagen = New tme_FichaProyectoImagen
                        oImagen.id_ficha_proyecto = oFicha.id_ficha_proyecto
                        oImagen.nombre_archivo_proyecto = nombreArchivo
                        oImagen.id_tipo_proyecto_imagen = 1

                        dbEntities.tme_FichaProyectoImagen.Add(oImagen)
                        Dim Path As String
                        Path = Server.MapPath(dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = Me.cmb_programa.SelectedValue).images_folder)
                        file.SaveAs(Path + nombreArchivo)
                    Next

                    dbEntities.Entry(oFicha).State = Entity.EntityState.Modified
                    dbEntities.SaveChanges()

                    Dim usuAct = db.t_usuario_ficha_proyecto.Where(Function(p) p.id_usuario = oFicha.id_usuario_responsable And p.id_ficha_proyecto = oFicha.id_ficha_proyecto).ToList()
                    If usuAct.Count() = 0 Then
                        Dim oUsuaAct = New t_usuario_ficha_proyecto
                        oUsuaAct.id_usuario = Me.cmb_persona_encargada.SelectedValue
                        oUsuaAct.id_ficha_proyecto = oFicha.id_ficha_proyecto
                        oUsuaAct.fecha_crea = DateTime.Now
                        oUsuaAct.acc_act = True
                        oUsuaAct.id_usuario_crea = oFicha.id_usuario_creo
                        dbEntities.t_usuario_ficha_proyecto.Add(oUsuaAct)
                        dbEntities.SaveChanges()
                    End If

                    'Dim usuAct2 = db.t_usuario_ficha_proyecto.Where(Function(p) p.id_usuario = oFicha.id_responsable_componente And p.id_ficha_proyecto = oFicha.id_ficha_proyecto).ToList()
                    'If usuAct2.Count() = 0 Then
                    '    Dim oUsuaAct = New t_usuario_ficha_proyecto
                    '    oUsuaAct.id_usuario = Me.cmb_responsable_componente.SelectedValue
                    '    oUsuaAct.id_ficha_proyecto = oFicha.id_ficha_proyecto
                    '    oUsuaAct.fecha_crea = DateTime.Now
                    '    oUsuaAct.acc_act = True
                    '    oUsuaAct.id_usuario_crea = oFicha.id_usuario_creo
                    '    dbEntities.t_usuario_ficha_proyecto.Add(oUsuaAct)
                    '    dbEntities.SaveChanges()
                    'End If

                    'If Me.cmb_personal_tecnico.SelectedValue IsNot Nothing Then
                    '    Dim idPersonalTecnico = Convert.ToInt32(Me.cmb_personal_tecnico.SelectedValue)
                    '    Dim oUsuaAct = dbEntities.t_usuario_ficha_proyecto.Where(Function(p) p.id_usuario = idPersonalTecnico And p.id_ficha_proyecto = oFicha.id_ficha_proyecto).FirstOrDefault()
                    '    If oUsuaAct Is Nothing Then
                    '        oUsuaAct = New t_usuario_ficha_proyecto
                    '        oUsuaAct.id_usuario = Me.cmb_personal_tecnico.SelectedValue
                    '        oUsuaAct.id_ficha_proyecto = oFicha.id_ficha_proyecto
                    '        oUsuaAct.fecha_crea = DateTime.Now
                    '        oUsuaAct.id_usuario_crea = oFicha.id_usuario_creo
                    '        oUsuaAct.acc_act = True
                    '        dbEntities.t_usuario_ficha_proyecto.Add(oUsuaAct)
                    '    Else
                    '        oUsuaAct.acc_act = True
                    '        dbEntities.Entry(oUsuaAct).State = Entity.EntityState.Modified
                    '    End If
                    'End If

                    Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                    Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectos"
                    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

                Else

                    Me.lbl_Activity_error.Visible = True

                End If

                'End If

            End Using

        Catch ex As Exception
            Me.MsgGuardar.NuevoMensaje = "Error al guardar" & ex.Source
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectosEdit?id=" & id_ficha
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Try

    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/Proyectos/frm_proyectos")
    End Sub

    'Protected Sub rbn_private_public_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbn_private_public.SelectedIndexChanged
    '    If rbn_private_public.SelectedValue = 1 Then
    '        Me.grd_partners.Enabled = True
    '        'Dim gridCo = CType(Me.placeHolder_grid.FindControl("gridAvance"), RadGrid)
    '        'gridCo.Enabled = True
    '    Else
    '        Me.grd_partners.Enabled = False
    '        'Dim gridCo = CType(Me.placeHolder_grid.FindControl("gridAvance"), RadGrid)
    '        'gridCo.Enabled = False
    '    End If
    'End Sub

    Protected Sub chk_todos_CheckedChanged(sender As Object, e As EventArgs) Handles chk_todos.CheckedChanged
        If chk_todos.Checked Then
            Me.grd_subregion.Visible = True
            Me.cmb_subregion.Visible = False
        Else
            Me.grd_subregion.Visible = False
            Me.cmb_subregion.Visible = True
        End If

    End Sub

    Protected Sub txt_nivel_cobertura_TextChanged(sender As Object, e As EventArgs)
        'sumarLOE()
    End Sub

    Sub sumarLOE()
        valorSuma = 0

        For Each row In Me.grd_district.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                'Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_aporteFicha")
                Dim selected As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_municipio"), CheckBox)
                Dim txt_loe As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)

                If selected.Checked Then
                    txt_loe.Enabled = True
                    valorSuma += txt_loe.Value
                    If txt_loe.Value = 0 Then
                        cero = True
                    End If
                Else
                    txt_loe.Enabled = False
                End If
                Dim valor = 100 - valorSuma
                If valor <= 0.4 And valor >= -0.4 Then
                    valorSuma = 100
                End If
                If valorSuma <> 100 Then
                    Me.div_mensaje.Visible = True
                    Me.lbl_errorLOE.Visible = True
                Else
                    Me.div_mensaje.Visible = False
                    Me.lbl_errorLOE.Visible = False
                End If

            End If
        Next

        If cero = True Then
            Me.lbl_errorLOECero.Visible = True
            Me.div_mensaje.Visible = True
        Else
            Me.lbl_errorLOECero.Visible = False
        End If
    End Sub

    Protected Sub ctrl_id_CheckedChanged(sender As Object, e As EventArgs)
        Dim id_subregiones = ""
        For Each row In Me.grd_subregion.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim selected As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                Dim txt_loe As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_nivel_cobertura"), RadNumericTextBox)

                If selected.Checked Then
                    Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
                    id_subregiones &= idSubregion.ToString() & ","
                End If
            End If
        Next
        id_subregiones = id_subregiones.TrimEnd(",")
        Dim ids = id_subregiones.Split(",")
        Me.grd_district.DataSource = ""
        Me.grd_district.DataSource = db.vw_tme_municipios.Where(Function(p) ids.Contains(p.id_subregion)).ToList()
        Me.grd_district.DataBind()
        'sumarLOE()
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
        ' sumarLOE()
    End Sub

    Protected Sub cmb_mecanismo_contratacion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_mecanismo_contratacion.SelectedIndexChanged


        'Me.txt_codigoproyecto.Text = cl_listados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, e.Value)
        'Me.lbl_mensaje.Text = cl_listados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, e.Value)

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

                Dim idSub_Father = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = e.Value).FirstOrDefault.id_sub_father

                If idSub_Father IsNot Nothing Then 'It´s linking to a respective sub contract

                    Dim cls_util As New ly_SIME.CORE.cls_util
                    Me.cmb_activity_father.DataSourceID = ""
                    Me.cmb_activity_father.DataSource = cls_util.ConvertToDataTable(dbEntities.vw_tme_ficha_proyecto.Where(Function(p) p.id_sub_mecanismo = idSub_Father).ToList())
                    Me.lblt_actividad_padre.Text = dbEntities.tme_sub_mecanismo.Where(Function(p) p.id_sub_mecanismo = idSub_Father).FirstOrDefault.nombre_sub_mecanismo
                    Me.cmb_activity_father.DataBind()
                    Me.ly_activity.Visible = True

                Else

                    Me.ly_activity.Visible = False

                End If


            End If

        End Using

        LoadData()
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
        e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("id_ficha_proyecto").ToString()

    End Sub

    Protected Sub cmb_activity_father_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        'Handles cmb_activity_father.SelectedIndexChanged

        If e.Value IsNot Nothing Then
            LoadData()
        End If

    End Sub

    Sub LoadData()

        Me.txt_codigoproyecto.Text = cl_listados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, If(chk_todos.Checked, -1, Me.cmb_subregion.SelectedValue), Me.cmb_mecanismo_contratacion.SelectedValue, If(Me.cmb_sub_mecanismo_contratacion.SelectedValue IsNot Nothing, Me.cmb_sub_mecanismo_contratacion.SelectedValue, -1), If(Me.cmb_activity_father.SelectedValue IsNot Nothing, If(Val(Me.cmb_activity_father.SelectedValue) = 0, -1, Me.cmb_activity_father.SelectedValue), -1))

        Me.divCodigo.Visible = True
        Me.lbl_mensaje.Visible = True
        Me.lbl_mensaje.Text = Me.txt_codigoproyecto.Text

    End Sub

    'Protected Sub grd_partners_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
    '    grd_partners.DataSource = listPartners
    'End Sub

    'Protected Sub grd_partners_ItemCommand(sender As Object, e As GridCommandEventArgs)
    '    If e.CommandName = RadGrid.InitInsertCommandName Then
    '        saveAllData()
    '        Dim num = CInt(Rnd() * (Date.UtcNow.Millisecond * 1000) + Date.UtcNow.Millisecond)
    '        listPartners.Insert(Me.grd_partners.Items.Count, New tme_ficha_partner() With {.nombre_partner = "", .id_ficha_partner = num, .id_partner_type = 1, .id_partnership_focus = 1})
    '        e.Canceled = True
    '        grd_partners.Rebind()
    '    End If
    'End Sub



    'Protected Sub saveAllData()
    '    'Update Session
    '    For Each item As GridDataItem In grd_partners.MasterTableView.Items
    '        Dim nombre_partner As TextBox = CType(item.FindControl("txt_nombre_partner"), TextBox)
    '        Dim cmb_partnership_focus As RadComboBox = CType(item.FindControl("cmb_partnership_focus"), RadComboBox)
    '        Dim cmb_partner_type As RadComboBox = CType(item.FindControl("cmb_partner_type"), RadComboBox)
    '        Dim UniqueID = Convert.ToInt32(item.GetDataKeyValue("id_ficha_partner").ToString())
    '        Dim emp As tme_ficha_partner = listPartners.Where(Function(i) i.id_ficha_partner = UniqueID).First()
    '        emp.nombre_partner = nombre_partner.Text
    '        emp.id_partnership_focus = cmb_partnership_focus.SelectedValue
    '        emp.id_partner_type = cmb_partner_type.SelectedValue
    '    Next
    'End Sub


    'Protected Sub grd_partners_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_partners.ItemDataBound
    '    If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

    '        Dim cmb_partner_type As RadComboBox = CType(e.Item.FindControl("cmb_partner_type"), RadComboBox)
    '        cmb_partner_type.DataSource = db.tme_partner_type.ToList()
    '        cmb_partner_type.DataTextField = "partner_type_name"
    '        cmb_partner_type.DataValueField = "id_partner_type"
    '        cmb_partner_type.DataBind()

    '        cmb_partner_type.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_partner_type").ToString()

    '        Dim cmb_partnership_focus As RadComboBox = CType(e.Item.FindControl("cmb_partnership_focus"), RadComboBox)
    '        cmb_partnership_focus.DataSource = db.tme_partnership_focus.ToList()
    '        cmb_partnership_focus.DataTextField = "partnership_focus_name"
    '        cmb_partnership_focus.DataValueField = "id_partnership_focus"
    '        cmb_partnership_focus.DataBind()

    '        cmb_partnership_focus.SelectedValue = DataBinder.Eval(e.Item.DataItem, "id_partnership_focus").ToString()


    '        Dim hlnkDelete As LinkButton = New LinkButton
    '        hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
    '        hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_ficha_partner").ToString())
    '        hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_ficha_partner").ToString())

    '    End If
    'End Sub


    'Protected Sub esp_ctrl_btn_eliminar_Click(sender As Object, e As EventArgs) Handles esp_ctrl_btn_eliminar.Click
    '    Using db As New ly_SIME.dbRMS_JIEntities
    '        Try
    '            'db.Database.ExecuteSqlCommand("DELETE FROM ins_beneficiaries WHERE id_beneficiary = " + Me.identity.Text)
    '            listPartners = listPartners.Where(Function(p) p.id_ficha_partner <> Me.identity.Text).ToList()
    '            grd_partners.Rebind()
    '            'saveAllData()
    '            Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
    '        Catch ex As SqlException
    '            Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
    '            Me.MsgGuardar.TituMensaje = "Error al eliminar"
    '        End Try
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "hideDeleteModal()", True)
    '    End Using
    'End Sub

    'Protected Sub grd_acciones_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_acciones.ItemDataBound
    '    If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
    '        Dim id_personal_tecnico = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_personal_tecnico").ToString())
    '        Dim col_hlk_eliminar As LinkButton = New LinkButton
    '        col_hlk_eliminar = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
    '        col_hlk_eliminar.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_personal_tecnico").ToString())
    '        col_hlk_eliminar.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_personal_tecnico").ToString())
    '        col_hlk_eliminar.ToolTip = controles.iconosGrid("col_hlk_eliminar")

    '    End If
    'End Sub

    'Protected Sub Eliminar_Click(sender As Object, e As EventArgs)

    '    Using db As New ly_SIME.dbRMS_JIEntities
    '        Dim a = CType(sender, LinkButton)
    '        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
    '        db.Database.ExecuteSqlCommand("DELETE FROM tme_ficha_proyecto_personal_tecnico where id_personal_tecnico = " + Me.identity.Text)
    '        LoadPErsonalTecnico()
    '    End Using
    'End Sub
    Protected Sub btn_registrar_tc_Click(sender As Object, e As EventArgs) Handles btn_registrar_tc.Click
        Me.Response.Redirect("~/Administracion/frm_tasas_cambio")
    End Sub
    'Private Sub cmb_componente_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_componente.SelectedIndexChanged
    '    Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
    '    Dim id_componente = Me.cmb_componente.SelectedValue
    '    'Dim entryPoint = From u In db.t_usuarios Join q In db.t_usuario_programa On u.id_usuario Equals q.id_usuario
    '    '                 Where q.id_programa = idPrograma And u.asociado_componente = True And u.id_componente = id_componente
    '    '                 Select New With {Key .id_usuario = u.id_usuario, Key .nombre = u.nombre_usuario & " " & u.apellidos_usuario,
    '    '                               Key .busqueda_actividad = u.t_usuario_programa.FirstOrDefault().busqueda_actividad}

    '    Me.cmb_persona_encargada.Text = String.Empty
    '    Me.cmb_persona_encargada.ClearSelection()

    '    Me.cmb_responsable_componente.Text = String.Empty
    '    Me.cmb_responsable_componente.ClearSelection()


    '    'Me.cmb_persona_encargada.DataSourceID = ""
    '    'Me.cmb_persona_encargada.DataSource = entryPoint.Where(Function(p) p.busqueda_actividad = True).OrderBy(Function(p) p.nombre).ToList()
    '    'Me.cmb_persona_encargada.DataBind()

    '    'Me.cmb_responsable_componente.DataSourceID = ""
    '    'Me.cmb_responsable_componente.DataSource = entryPoint.Where(Function(p) p.busqueda_actividad = True).OrderBy(Function(p) p.nombre).ToList()
    '    'Me.cmb_responsable_componente.DataBind()
    'End Sub
    Protected Sub dt_fecha_inicio_SelectedDateChanged(sender As Object, e As Calendar.SelectedDateChangedEventArgs) Handles dt_fecha_inicio.SelectedDateChanged
        Using dbEntities As New dbRMS_JIEntities
            Dim fechaReg = dt_fecha_inicio.SelectedDate
            Dim fehcaTasaCambio = ""
            If Month(fechaReg) > 9 Then
                fehcaTasaCambio = (Year(fechaReg) + 1) & "-" & Month(fechaReg) & "-" & Day(fechaReg)
                fechaReg = Convert.ToDateTime(fehcaTasaCambio)
            End If

            Dim oTasaCambio = dbEntities.t_trimestre_tasa_cambio.Where(Function(p) p.mes = Month(fechaReg) And p.t_trimestre.anio = Year(fechaReg)).ToList()
            If oTasaCambio.Count() > 0 Then
                Me.btn_guardar.Enabled = True
            Else
                Dim funcion = "FuncModatTrim()"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "Pop", funcion, True)
                Me.btn_guardar.Enabled = False
            End If
        End Using
    End Sub

    'Public Property listPartners() As List(Of tme_ficha_partner)
    '    Get
    '        If Session("listPartners") IsNot Nothing Then
    '            Return DirectCast(Session("listPartners"), List(Of tme_ficha_partner))
    '        Else
    '            Return New List(Of tme_ficha_partner)()
    '        End If
    '    End Get
    '    Set(value As List(Of tme_ficha_partner))
    '        Session("listPartners") = value
    '    End Set
    'End Property
End Class