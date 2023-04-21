Imports Telerik.Web.UI
Imports ly_SIME
Imports System.Data.SqlClient
Imports CuteWebUI
Imports System.IO
Imports System.Configuration.ConfigurationManager

Public Class frm_proyectosADOldACS
    Inherits System.Web.UI.Page

    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim frmCODE As String = "NEW_PROY"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Proyectos\"
    Dim sFileDirTemp As String = Server.MapPath("~") & "\Temp\"
    Dim sFileDirUbicacion As String = "~/FileUploads/Proyectos/"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim valorSuma As Decimal = 0

    Protected Overloads Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        AddHandler UploaderProyecto.FileUploaded, AddressOf Uploader_FileUploaded
        AddHandler Uploader2.FileUploaded, AddressOf Uploader_FileUploadedImg
    End Sub

    Private Sub Uploader_FileUploadedImg(ByVal sender As Object, ByVal args As UploaderEventArgs)
        Dim uploader As Uploader = DirectCast(sender, Uploader)
        Try
            Dim Random As New Random()
            Dim Aleatorio As Double = Random.Next(1, 99999)
            Dim fileNameWE = Path.GetFileNameWithoutExtension(args.FileName)
            Dim extension As String = Path.GetExtension(args.FileName)
            Dim File As String = "MEImg_" & Me.Session("E_IdUser") & Aleatorio & fileNameWE.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension
            args.CopyTo(sFileDirTemp & File)
            Me.lblarchivoImg.Text = File
            Me.imgUser.ImageUrl = "~/Temp/" & File
            Me.HLKFoto.NavigateUrl = "../Temp/" & File
        Catch ex As Exception
            Me.imgEliminaImg.ImageUrl = "../imagenes/iconos/s_warn.png"
            Me.lblarchivoImg.Text = "Error.."
        End Try
        Me.Panel6.Visible = True
        Me.HLKFoto.Visible = True
    End Sub

    Private Sub Uploader_FileUploaded(ByVal sender As Object, ByVal args As UploaderEventArgs)
        Dim uploader As Uploader = DirectCast(sender, Uploader)
        Try
            Dim Random As New Random()
            Dim Aleatorio As Double = Random.Next(1, 99999)
            Dim fileNameWE = Path.GetFileNameWithoutExtension(args.FileName)
            Dim extension As String = Path.GetExtension(args.FileName)
            Dim File As String = "doc_" & Me.Session("E_IdUser") & Aleatorio & fileNameWE.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension

            args.CopyTo(sFileDirTemp & File)
            Dim sql = "INSERT INTO tme_Anexos_ficha_temp(id_sesion_temp, archivo) VALUES('" & Me.lbl_id_sesion_temp.Text & "','" & File & "')"
            Dim dm1 As New SqlDataAdapter(sql, cnnME)
            Dim ds1 As New DataSet("Archivos")
            dm1.Fill(ds1, "Archivos")
            Me.grd_archivos.DataBind()
            Me.Panel1_firma.Visible = False
            Dim j = 0
        Catch ex As Exception
            Me.Panel1_firma.Visible = True
            Me.img_btn_borrar_temp.ImageUrl = "../imagenes/iconos/s_warn.png"
            Me.lblarchivo.Text = "Error.."
        End Try
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseFive'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub
    Sub DelFileParam(ByVal archivo As String)
        Dim sFileName As String = System.IO.Path.GetFileName(archivo)

        Dim file_info As New IO.FileInfo(sFileDirTemp + sFileName)
        If (file_info.Exists) Then
            file_info.Delete()
        End If
        Me.lblarchivo.Text = ""
        Me.Panel1_firma.Visible = False
    End Sub

    Sub CopyFileParam(ByVal file As String)
        Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(file)
        Dim extension As String = System.IO.Path.GetExtension(file)
        Dim file_info As New IO.FileInfo(sFileDirTemp + sFileName + extension)
        Dim fileName = ""
        Try
            fileName = sFileDir & sFileName.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", ".").Replace(",", "-") + extension
            file_info.CopyTo(fileName)
        Catch ex As Exception
        End Try
        DelFileParam(file)
        Me.Panel1_firma.Visible = False

    End Sub



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

            Me.btn_eliminarAporte.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
            Me.btn_eliminarIndicador.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
            Me.btn_eliminarMunicipio.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
        End If

        If Not Me.IsPostBack Then
            Dim idPrograma = Convert.ToInt32(Me.Session("E_IDPrograma"))
            Me.cmb_programa.DataSourceID = ""
            Me.cmb_programa.DataSource = clListados.get_t_programas(idPrograma)
            Me.cmb_programa.DataTextField = "nombre_programa"
            Me.cmb_programa.DataValueField = "id_programa"
            Me.cmb_programa.DataBind()
            Me.cmb_programa.Enabled = False

            Me.lbl_id_sesion_temp.Text = clListados.CodigoRandom()
            loadListas(idPrograma)
            LoadData()
        End If
    End Sub

    Sub loadListas(ByVal idPrograma As Integer)
        Me.cmb_ejecutor.DataSourceID = ""
        Me.cmb_ejecutor.DataSource = clListados.get_t_ejecutores(idPrograma)
        Me.cmb_ejecutor.DataTextField = "nombre_ejecutor"
        Me.cmb_ejecutor.DataValueField = "id_ejecutor"
        Me.cmb_ejecutor.DataBind()

        Me.cmb_region.DataSourceID = ""
        Me.cmb_region.DataSource = clListados.get_t_regiones(Convert.ToInt32(Me.cmb_programa.SelectedValue))
        Me.cmb_region.DataTextField = "nombre_region"
        Me.cmb_region.DataValueField = "id_region"
        Me.cmb_region.DataBind()

        Me.cmb_subregion.DataSourceID = ""
        Me.cmb_subregion.DataSource = clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
        Me.cmb_subregion.DataTextField = "nombre_subregion"
        Me.cmb_subregion.DataValueField = "id_subregion"
        Me.cmb_subregion.DataBind()

        Me.cmb_componente.DataSourceID = ""
        Me.cmb_componente.DataSource = clListados.get_tme_componente_programa(Convert.ToInt32(Me.cmb_programa.SelectedValue))
        Me.cmb_componente.DataTextField = "nombre_componente"
        Me.cmb_componente.DataValueField = "id_componente"
        Me.cmb_componente.DataBind()

        Me.cmb_periodo.DataSourceID = ""
        Me.cmb_periodo.DataSource = clListados.get_t_periodo(Convert.ToInt32(Me.cmb_subregion.SelectedValue))
        Me.cmb_periodo.DataTextField = "nombre_periodo"
        Me.cmb_periodo.DataValueField = "id_periodo"
        Me.cmb_periodo.DataBind()



        Using dbEntities As New dbRMS_HNEntities

            Dim id_tmp = Convert.ToInt64(Me.lbl_id_sesion_temp.Text)


            Dim actuales = dbEntities.tmp_meta_indicador_ficha.Where(Function(p) p.id_ficha_proyecto_temp = id_tmp) _
                           .Select(Function(p) p.id_indicador.Value).ToList()

            Dim aaa = dbEntities.tme_indicador _
                .Where(Function(p) Not actuales.Contains(p.id_indicador)).ToList()

            Me.cmb_indicador.DataSource = ""
            Me.cmb_indicador.DataSource = dbEntities.vw_indicador_marcologico _
                .Where(Function(p) Not actuales.Contains(p.id_indicador) And p.id_programa = idPrograma).ToList()
            Me.cmb_indicador.DataTextField = "definicion_indicador"
            Me.cmb_indicador.DataValueField = "id_indicador"
            Me.cmb_indicador.DataBind()


            Me.grd_cobertura.DataSource = dbEntities.tmp_ficha_municipio.Where(Function(p) p.id_ficha_proyecto_tmp = id_tmp).ToList()
            Me.grd_cobertura.DataBind()

            Me.grd_indicadores.DataSource = dbEntities.tmp_meta_indicador_ficha.Where(Function(p) p.id_ficha_proyecto_temp = id_tmp).ToList()
            Me.grd_indicadores.DataBind()


            Dim nivel = dbEntities.t_nivel_avance.FirstOrDefault(Function(p) p.id_programa = idPrograma)
            Dim sql = "SELECT " & nivel.campo_nombre_tabla & ", " & nivel.campo_id_tabla & " FROM " & nivel.nombre_tabla & " WHERE " _
                      & nivel.campo_id_tabla & " IN (SELECT id_relacion FROM tme_subregion_nivel_avance WHERE id_subregion =" & Me.cmb_subregion.SelectedValue & ") ORDER BY " & nivel.campo_nombre_tabla

            Me.SqlDataSource2.SelectCommand = sql

            Me.cmb_departamento.DataValueField = nivel.campo_id_tabla
            Me.cmb_departamento.DataTextField = nivel.campo_nombre_tabla
            Me.cmb_departamento.DataBind()

            Dim actualesMu = dbEntities.tmp_ficha_municipio.Where(Function(p) p.id_ficha_proyecto_tmp = Me.lbl_id_sesion_temp.Text) _
                           .Select(Function(p) p.id_municipio.Value).ToList()

            Me.cmb_municipio.DataSource = ""
            If Me.cmb_programa.SelectedValue = 1 Then
                Me.cmb_municipio.DataSource = dbEntities.t_municipios _
                .Where(Function(p) Not actualesMu.Contains(p.id_municipio) And p.id_departamento = Me.cmb_departamento.SelectedValue).ToList()
                Me.cmb_municipio.DataValueField = "id_municipio"
                Me.cmb_municipio.DataTextField = "nombre_municipio"
            Else
                Me.cmb_municipio.DataSource = dbEntities.tme_counties _
                .Where(Function(p) Not actualesMu.Contains(p.id_county) And p.id_district = Me.cmb_departamento.SelectedValue).ToList()
                Me.cmb_municipio.DataValueField = "id_county"
                Me.cmb_municipio.DataTextField = "nombre_county"
            End If

            Me.cmb_municipio.DataBind()


            Me.grd_Sectores.DataSource = ""
            Me.grd_Sectores.DataSource = dbEntities.vw_tme_sectores.Where(Function(p) p.id_programa = idPrograma).ToList()
            Me.grd_Sectores.DataBind()

            Me.grd_subregion.DataSource = ""
            Me.grd_subregion.DataSource = dbEntities.vw_t_subregiones.Where(Function(p) p.id_programa = Me.cmb_programa.SelectedValue).ToList()
            Me.grd_subregion.DataBind()


            Me.cmb_fuente_aporte.DataSource = ""
            Me.cmb_fuente_aporte.DataSource = dbEntities.tme_Aportes.ToList()
            Me.cmb_fuente_aporte.DataTextField = "nombre_aporte"
            Me.cmb_fuente_aporte.DataValueField = "id_aporte"
            Me.cmb_fuente_aporte.DataBind()

            Me.cmb_estado.DataSourceID = ""
            Me.cmb_estado.DataSource = clListados.get_tme_FichaEstado()
            Me.cmb_estado.DataTextField = "nombre_estado_ficha"
            Me.cmb_estado.DataValueField = "id_ficha_estado"
            Me.cmb_estado.DataBind()


        End Using

    End Sub
    Sub LoadData()
        Me.txt_codigoproyecto.Text = clListados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, Me.cmb_componente.SelectedValue)

        Using dbEntities As New dbRMS_HNEntities
        End Using
    End Sub
    Protected Sub cmb_region_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_region.SelectedIndexChanged
        Me.cmb_subregion.DataSourceID = ""
        Me.cmb_subregion.DataSource = clListados.get_t_subregiones(Convert.ToInt32(Me.cmb_region.SelectedValue))
        Me.cmb_subregion.DataTextField = "nombre_subregion"
        Me.cmb_subregion.DataValueField = "id_subregion"
        Me.cmb_subregion.DataBind()
        LoadData()
    End Sub

    Protected Sub lnk_sugerir_codigo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_sugerir_codigo.Click
        Me.txt_codigoproyecto.Text = clListados.CrearCodigoFicha(Me.cmb_programa.SelectedValue, Me.cmb_subregion.SelectedValue, Me.cmb_componente.SelectedValue)
    End Sub

    Protected Sub cmb_subregion_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_subregion.SelectedIndexChanged
        Dim sql = ""


        LoadData()
    End Sub

    Protected Sub cmb_departamento_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_departamento.SelectedIndexChanged

        Using dbEntities As New dbRMS_HNEntities
            Dim actualesMu = dbEntities.tmp_ficha_municipio.Where(Function(p) p.id_ficha_proyecto_tmp = Me.lbl_id_sesion_temp.Text) _
                           .Select(Function(p) p.id_municipio.Value).ToList()

            Me.cmb_municipio.DataSource = ""
            If Me.cmb_programa.SelectedValue = 1 Then
                Me.cmb_municipio.DataSource = dbEntities.t_municipios _
                .Where(Function(p) Not actualesMu.Contains(p.id_municipio) And p.id_departamento = Me.cmb_departamento.SelectedValue).ToList()
                Me.cmb_municipio.DataValueField = "id_municipio"
                Me.cmb_municipio.DataTextField = "nombre_municipio"
            Else
                Me.cmb_municipio.DataSource = dbEntities.tme_counties _
                .Where(Function(p) Not actualesMu.Contains(p.id_county) And p.id_district = Me.cmb_departamento.SelectedValue).ToList()
                Me.cmb_municipio.DataValueField = "id_county"
                Me.cmb_municipio.DataTextField = "nombre_county"
            End If
            Me.cmb_municipio.DataBind()
        End Using


        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseTwo'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Using dbEntities As New dbRMS_HNEntities
            Dim oFicha = New tme_Ficha_Proyecto
            oFicha.codigo_SAPME = Me.txt_codigoproyecto.Text
            oFicha.codigo_ficha_AID = Me.txt_codigoproyecto.Text
            oFicha.nombre_proyecto = Me.txt_nombreproyecto.Text
            oFicha.area_intervencion = Me.txt_descripcion.Text
            oFicha.id_ejecutor = Me.cmb_ejecutor.SelectedValue
            oFicha.id_componente = Me.cmb_componente.SelectedValue
            oFicha.fecha_inicio_proyecto = Me.dt_fecha_inicio.SelectedDate
            oFicha.fecha_fin_proyecto = Me.dt_fecha_fin.SelectedDate
            oFicha.georeferencia_completa = "NO"
            oFicha.aportes_actualizados = "NO"
            oFicha.ActualizacionReciente = "NO"
            'Dim id_trimestre = Convert.ToInt32(Me.cmb_periodo.SelectedValue)
            oFicha.id_periodo = Me.cmb_periodo.SelectedValue
            oFicha.id_ficha_estado = 1

            Dim idPrograma = Convert.ToInt32(Me.cmb_programa.SelectedValue)
            oFicha.tasa_cambio = dbEntities.t_programas.Find(idPrograma).tasa_cambio

            oFicha.id_usuario_creo = Me.Session("E_IdUser").ToString()
            oFicha.id_usuario_update = Me.Session("E_IdUser").ToString()
            oFicha.datecreated = Date.UtcNow
            oFicha.dateUpdate = Date.UtcNow

            oFicha.tme_ficha_historico_estado.Add(New tme_ficha_historico_estado() _
                                                  With {.id_ficha_estado = 1, _
                                                        .id_usuario = oFicha.id_usuario_creo,
                                                        .fecha = Date.UtcNow
                                                        })


            dbEntities.tme_Ficha_Proyecto.Add(oFicha)
            dbEntities.SaveChanges()

            If chk_todos.Checked Then
                For Each row In Me.grd_subregion.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim subR As CheckBox = CType(row.Cells(0).FindControl("ctrl_id"), CheckBox)
                        If subR.Checked = True Then
                            Dim idSubregion As Integer = dataItem.GetDataKeyValue("id_subregion")
                            Dim oSubregion = New tme_ficha_subregion
                            oSubregion.id_subregion = idSubregion
                            oSubregion.id_ficha_proyecto = oFicha.id_ficha_proyecto
                            oFicha.tme_ficha_subregion.Add(oSubregion)
                        End If
                    End If
                Next
            Else
                Dim oSubregion = New tme_ficha_subregion
                oSubregion.id_subregion = Me.cmb_subregion.SelectedValue
                oSubregion.id_ficha_proyecto = oFicha.id_ficha_proyecto

                oFicha.tme_ficha_subregion.Add(oSubregion)
            End If





            Dim municipios_tmp = dbEntities.tmp_ficha_municipio.Where(Function(p) p.id_ficha_proyecto_tmp = Me.lbl_id_sesion_temp.Text).ToList()
            For Each item In municipios_tmp
                Dim oMunicipio = New tme_ficha_municipio
                oMunicipio.id_ficha_proyecto = oFicha.id_ficha_proyecto
                oMunicipio.id_municipio = item.id_municipio
                oFicha.tme_ficha_municipio.Add(oMunicipio)
            Next

            For Each row In Me.grd_Sectores.Items
                If TypeOf row Is GridDataItem Then
                    Dim dataItem As GridDataItem = CType(row, GridDataItem)
                    Dim IdSectorME As Integer = Convert.ToInt32(dataItem.GetDataKeyValue("id_sector_me"))
                    Dim IdSector As CheckBox = CType(row.Cells(0).FindControl("IdSector"), CheckBox)
                    If IdSector.Checked = True Then
                        Dim oSector = New tme_FichaSector
                        oSector.id_ficha_proyecto = oFicha.id_ficha_proyecto
                        oSector.id_sector_me = IdSectorME
                        oFicha.tme_FichaSector.Add(oSector)
                    End If
                End If
            Next

            guardarIndicadores()
            Dim indicadores_tmp = dbEntities.tmp_meta_indicador_ficha.Where(Function(p) p.id_ficha_proyecto_temp = Me.lbl_id_sesion_temp.Text).ToList()
            For Each item In indicadores_tmp
                Dim oMetaIndicador = New tme_meta_indicador_ficha
                oMetaIndicador.id_ficha_proyecto = oFicha.id_ficha_proyecto
                oMetaIndicador.meta_total = item.meta_total_ficha
                oMetaIndicador.id_indicador = item.id_indicador
                oMetaIndicador.fecha_creo = Date.UtcNow
                oMetaIndicador.id_usuario_creo = Me.Session("E_IdUser").ToString()
                oFicha.tme_meta_indicador_ficha.Add(oMetaIndicador)
            Next


            guardarAportes()
            Dim aportes_tmp = dbEntities.tmp_aportes_ficha.Where(Function(p) p.id_ficha_proyecto_tmp = Me.lbl_id_sesion_temp.Text).ToList()
            For Each item In aportes_tmp
                Dim oAporte = New tme_AportesFicha
                oAporte.id_ficha_proyecto = oFicha.id_ficha_proyecto
                oAporte.monto_aporte = item.monto_aporte
                oAporte.id_indicador = 0
                oAporte.monto_aporte_obligado = item.monto_aporte
                oAporte.id_aporte = item.id_aporte
                oFicha.tme_AportesFicha.Add(oAporte)
            Next

            oFicha.costo_total_proyecto = aportes_tmp.Sum(Function(p) p.monto_aporte)

            Dim componente = dbEntities.tme_componente_programa.Find(oFicha.id_componente)
            oFicha.tme_componente_programa.correlativos = oFicha.tme_componente_programa.correlativos + 1


            '********* Documentos *********
            Me.grd_archivos.DataBind()
            Dim archivos_tmp = dbEntities.tme_Anexos_ficha_temp.Where(Function(p) p.id_sesion_temp = Me.lbl_id_sesion_temp.Text).ToList()
            For Each item In archivos_tmp
                Dim oArchivo = New tme_Anexos_ficha
                oArchivo.id_ficha_proyecto = oFicha.id_ficha_proyecto
                oArchivo.nombre_documento = item.archivo

                oFicha.tme_Anexos_ficha.Add(oArchivo)

                'For Each row In Me.grd_archivos.Items
                '    Dim file = item.archivo
                '    CopyFileParam(file)
                'Next
            Next

            If Me.Panel6.Visible = True Then
                Dim oImagen = New tme_FichaProyectoImagen
                oImagen.id_ficha_proyecto = oFicha.id_ficha_proyecto
                oImagen.nombre_archivo_proyecto = Me.lblarchivoImg.Text
                oImagen.id_tipo_proyecto_imagen = 2
                dbEntities.tme_FichaProyectoImagen.Add(oImagen)
                CopyFileParam(Me.lblarchivoImg.Text)
            End If

            dbEntities.Entry(oFicha).State = Entity.EntityState.Modified
            dbEntities.Entry(componente).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()

        End Using

        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectos"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    End Sub

    Protected Sub btn_guardar1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar1.Click
        Using dbEntities As New dbRMS_HNEntities
            Dim municipio_temp = New tmp_ficha_municipio
            municipio_temp.id_ficha_proyecto_tmp = Me.lbl_id_sesion_temp.Text
            municipio_temp.id_municipio = Me.cmb_municipio.SelectedValue
            municipio_temp.nombre_municipio = Me.cmb_municipio.Text
            municipio_temp.nombre_departamento = Me.cmb_departamento.Text

            dbEntities.tmp_ficha_municipio.Add(municipio_temp)
            dbEntities.SaveChanges()

            Me.grd_cobertura.DataSource = dbEntities.tmp_ficha_municipio.Where(Function(p) p.id_ficha_proyecto_tmp = Me.lbl_id_sesion_temp.Text).ToList()

            Dim actualesMu = dbEntities.tmp_ficha_municipio.Where(Function(p) p.id_ficha_proyecto_tmp = Me.lbl_id_sesion_temp.Text) _
                           .Select(Function(p) p.id_municipio.Value).ToList()

            Me.cmb_municipio.DataSource = ""
            If Me.cmb_programa.SelectedValue = 1 Then
                Me.cmb_municipio.DataSource = dbEntities.t_municipios _
                .Where(Function(p) Not actualesMu.Contains(p.id_municipio) And p.id_departamento = Me.cmb_departamento.SelectedValue).ToList()
                Me.cmb_municipio.DataValueField = "id_municipio"
                Me.cmb_municipio.DataTextField = "nombre_municipio"
            Else
                Me.cmb_municipio.DataSource = dbEntities.tme_counties _
                .Where(Function(p) Not actualesMu.Contains(p.id_county) And p.id_district = Me.cmb_departamento.SelectedValue).ToList()
                Me.cmb_municipio.DataValueField = "id_county"
                Me.cmb_municipio.DataTextField = "nombre_county"
            End If
            Me.cmb_municipio.DataBind()
        End Using

        Me.grd_cobertura.DataBind()

        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseTwo'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub

    Protected Sub btn_guardarIndicador_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardarIndicador.Click
        guardarIndicadores()
        Using dbEntities As New dbRMS_HNEntities
            Dim oMetaIndicador = New tmp_meta_indicador_ficha
            oMetaIndicador.id_ficha_proyecto_temp = Me.lbl_id_sesion_temp.Text
            oMetaIndicador.meta_total_ficha = 0
            oMetaIndicador.id_indicador = Me.cmb_indicador.SelectedValue
            oMetaIndicador.definicion_indicador = Me.cmb_indicador.Text

            dbEntities.tmp_meta_indicador_ficha.Add(oMetaIndicador)
            dbEntities.SaveChanges()

            listaIndicadores()
        End Using

        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseSix'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub

    Sub guardarIndicadores()
        Dim sql As String = ""
        For Each row In Me.grd_indicadores.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_meta_ficha")
                Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_TotalIndicador"), RadNumericTextBox)
                sql = "UPDATE tmp_meta_indicador_ficha SET meta_total_ficha=" & TotalIndicador.Text.Trim
                sql &= " WHERE id_meta_ficha= " & IDInstrumentoID.ToString

                Using dbEntities As New dbRMS_HNEntities
                    dbEntities.Database.ExecuteSqlCommand(sql)
                End Using
            End If
            'Dim dm As New SqlCommand(sql, cnnME)
            'dm.ExecuteNonQuery()
        Next
    End Sub

    Sub listaIndicadores()
        Using dbEntities As New dbRMS_HNEntities
            Me.grd_indicadores.DataSource = dbEntities.tmp_meta_indicador_ficha.Where(Function(p) p.id_ficha_proyecto_temp = Me.lbl_id_sesion_temp.Text).ToList()
            Dim actuales = dbEntities.tmp_meta_indicador_ficha.Where(Function(p) p.id_ficha_proyecto_temp = Me.lbl_id_sesion_temp.Text) _
                           .Select(Function(p) p.id_indicador.Value).ToList()

            Me.cmb_indicador.DataSource = ""
            Me.cmb_indicador.DataSource = dbEntities.vw_indicador_marcologico _
                .Where(Function(p) Not actuales.Contains(p.id_indicador) And p.id_programa = Me.cmb_programa.SelectedValue).ToList()
            Me.cmb_indicador.DataTextField = "definicion_indicador"
            Me.cmb_indicador.DataValueField = "id_indicador"
            Me.cmb_indicador.DataBind()
        End Using

        Me.grd_indicadores.DataBind()
    End Sub

    Protected Sub grd_indicadores_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_indicadores.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim TotalIndicador As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_TotalIndicador"), RadNumericTextBox)
            TotalIndicador.Text = DataBinder.Eval(e.Item.DataItem, "meta_total_ficha").ToString()
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            'hlnkDelete.Text = DataBinder.Eval(e.Item.DataItem, "id_region").ToString()
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_meta_ficha").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_meta_ficha").ToString())
        End If
    End Sub

    Protected Sub EliminarIndicador_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        Me.btn_eliminarMunicipio.Visible = False
        Me.btn_eliminarAporte.Visible = False
        Me.btn_eliminarIndicador.Visible = True
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseSix'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub

    Protected Sub btn_eliminarIndicador_Click(sender As Object, e As EventArgs) Handles btn_eliminarIndicador.Click
        Using db As New dbRMS_HNEntities
            Try
                db.Database.ExecuteSqlCommand("DELETE FROM tmp_meta_indicador_ficha WHERE id_meta_ficha = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"

            End Try
            listaIndicadores()
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseSix'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "hideDeleteModal()", True)

        End Using
    End Sub

    Protected Sub btn_guardarAporte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardarAporte.Click
        guardarAportes()
        Using dbEntities As New dbRMS_HNEntities
            Dim oAporte = New tmp_aportes_ficha
            oAporte.id_ficha_proyecto_tmp = Me.lbl_id_sesion_temp.Text
            oAporte.monto_aporte = 0
            oAporte.id_aporte = Me.cmb_fuente_aporte.SelectedValue
            oAporte.nombre_aporte = Me.cmb_fuente_aporte.Text

            dbEntities.tmp_aportes_ficha.Add(oAporte)
            dbEntities.SaveChanges()

            Dim aportesFicha = dbEntities.tmp_aportes_ficha.Where(Function(p) p.id_ficha_proyecto_tmp = Me.lbl_id_sesion_temp.Text).ToList()
            Me.grd_aportes.DataSource = aportesFicha

            valorSuma = aportesFicha.Sum(Function(p) p.monto_aporte)

            sumaAportes()

            Dim actuales = dbEntities.tmp_aportes_ficha.Where(Function(p) p.id_ficha_proyecto_tmp = Me.lbl_id_sesion_temp.Text) _
                           .Select(Function(p) p.id_aporte.Value).ToList()

            Me.cmb_fuente_aporte.DataSource = ""
            Me.cmb_fuente_aporte.DataSource = dbEntities.tme_Aportes.Where(Function(p) Not actuales.Contains(p.id_aporte)).ToList()
            Me.cmb_fuente_aporte.DataTextField = "nombre_aporte"
            Me.cmb_fuente_aporte.DataValueField = "id_aporte"
            Me.cmb_fuente_aporte.DataBind()

        End Using

        Me.grd_aportes.DataBind()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseFour'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub

    Protected Sub grd_cobertura_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cobertura.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            'hlnkDelete.Text = DataBinder.Eval(e.Item.DataItem, "id_region").ToString()
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_ficha_municipio_tmp").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_ficha_municipio_tmp").ToString())
        End If
    End Sub
    Protected Sub grd_aportes_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_aportes.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim TotalIndicador As RadNumericTextBox = CType(e.Item.Cells(0).FindControl("txt_total_aporte"), RadNumericTextBox)
            TotalIndicador.Text = DataBinder.Eval(e.Item.DataItem, "monto_aporte").ToString()
            Dim hlnkDelete As LinkButton = New LinkButton
            hlnkDelete = CType(e.Item.FindControl("col_hlk_eliminar"), LinkButton)
            'hlnkDelete.Text = DataBinder.Eval(e.Item.DataItem, "id_region").ToString()
            hlnkDelete.Attributes.Add("data-href", DataBinder.Eval(e.Item.DataItem, "id_aporte_ficha").ToString())
            hlnkDelete.Attributes.Add("data-identity", DataBinder.Eval(e.Item.DataItem, "id_aporte_ficha").ToString())
        End If
    End Sub

    Sub guardarAportes()
        valorSuma = 0
        Dim sql As String = ""
        For Each row In Me.grd_aportes.Items
            If TypeOf row Is GridDataItem Then
                Dim dataItem As GridDataItem = CType(row, GridDataItem)
                Dim IDInstrumentoID As Integer = dataItem.GetDataKeyValue("id_aporte_ficha")
                Dim TotalIndicador As RadNumericTextBox = CType(row.Cells(0).FindControl("txt_total_aporte"), RadNumericTextBox)
                sql = "UPDATE tmp_Aportes_ficha SET monto_aporte=" & TotalIndicador.Value
                sql &= " WHERE id_aporte_ficha= " & IDInstrumentoID.ToString
                valorSuma += TotalIndicador.Value
                Using dbEntities As New dbRMS_HNEntities
                    dbEntities.Database.ExecuteSqlCommand(sql)
                End Using
            End If
            'Dim dm As New SqlCommand(sql, cnnME)
            'dm.ExecuteNonQuery()
        Next
        sumaAportes()
    End Sub

    Sub sumaAportes()
        If valorSuma = 0 Then
            Me.lbl_total.Text = 0.ToString("c2", cl_user.regionalizacionCulture)
            Me.lbl_totalUSD.Text = 0.ToString("c2", cl_user.regionalizacionCulture)
        Else
            Me.lbl_total.Text = valorSuma.ToString("c2", cl_user.regionalizacionCulture)
            Me.lbl_totalUSD.Text = (valorSuma / 1000).ToString("c2", cl_user.regionalizacionCulture)
        End If
    End Sub

    'Sub sumaAportes()
    '    Me.lbl_total.Text = oProyecto.costo_total_proyecto.Value.ToString("c2", cl_user.regionalizacionCulture)
    '    Me.lbl_totalUSD.Text = (oProyecto.costo_total_proyecto / oProyecto.tasa_cambio).Value.ToString("c2", cl_user.regionalizacionCulture)
    'End Sub



    Protected Sub EliminarMunicipio_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        Me.btn_eliminarMunicipio.Visible = True
        Me.btn_eliminarAporte.Visible = False
        Me.btn_eliminarIndicador.Visible = False
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub



    Protected Sub EliminarAporte_Click(sender As Object, e As EventArgs)
        Dim a = CType(sender, LinkButton)
        Me.identity.Text = a.Attributes.Item("data-identity").ToString()
        Me.btn_eliminarAporte.Visible = True
        Me.btn_eliminarMunicipio.Visible = False
        Me.btn_eliminarIndicador.Visible = False
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "loadDeleteModal()", True)
    End Sub


    Protected Sub btn_eliminarMunicipio_Click(sender As Object, e As EventArgs) Handles btn_eliminarMunicipio.Click
        Using db As New dbRMS_HNEntities
            Try
                db.Database.ExecuteSqlCommand("DELETE FROM tmp_ficha_municipio WHERE id_ficha_municipio_tmp = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseTwo'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "hide", "hideDeleteModal()", True)
        End Using
    End Sub
    Protected Sub btn_eliminarAporte_Click(sender As Object, e As EventArgs) Handles btn_eliminarAporte.Click
        Using db As New dbRMS_HNEntities
            Try
                db.Database.ExecuteSqlCommand("DELETE FROM tmp_aportes_ficha WHERE id_aporte_ficha = " + Me.identity.Text)
                Me.MsgGuardar.NuevoMensaje = "Eliminado Correctamente"
            Catch ex As SqlException
                Me.MsgGuardar.NuevoMensaje = cl_error.MensajeError(ex.Number, ex.StackTrace.ToString())
                Me.MsgGuardar.TituMensaje = "Error al eliminar"
            End Try
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseFour'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Using
    End Sub

    Protected Sub cmb_fuente_aporte_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_fuente_aporte.SelectedIndexChanged
        guardarAportes()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseFour'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub

    Protected Sub cmb_fuente_aporte_TextChanged(sender As Object, e As EventArgs) Handles cmb_fuente_aporte.TextChanged
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseFour'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub

    Protected Sub txt_meta_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        guardarAportes()
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseFour'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub

    Protected Sub grd_archivos_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_archivos.DeleteCommand
        Dim id_temp = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_archivo_temp").ToString()
        cnnME.Open()
        Dim dm As New SqlCommand("DELETE FROM tme_Anexos_ficha_temp WHERE (id_archivo_temp = " & id_temp & ")", cnnME)
        dm.ExecuteNonQuery()
        cnnME.Close()
        Me.grd_archivos.DataBind()
        DelFileParam(e.Item.Cells(4).Text.ToString)
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Cookies.set('activeAccordionGroup', 'collapseFive'); if (Cookies.get('activeAccordionGroup') != null) { $('#accordion .panel-collapse').removeClass('in'); $('#' + Cookies.get('activeAccordionGroup')).addClass('in');}", True)
    End Sub
    Protected Sub grd_archivos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_archivos.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim ImageDownload As New HyperLink
            ImageDownload = CType(e.Item.FindControl("hlk_ImageDownload"), HyperLink)
            ImageDownload.NavigateUrl = "~/Temp/" & e.Item.Cells(4).Text.ToString
            ImageDownload.Target = "_blank"
        End If

    End Sub
    Protected Sub imgEliminaImg_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgEliminaImg.Click
        Me.Panel6.Visible = False
        Me.imgUser.ImageUrl = "~/Imagenes/iconos/photo64X64.png"
        Me.HLKFoto.Visible = False
        DelFileParam(Me.lblarchivoImg.Text)
    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/Proyectos/frm_proyectosCuadroMando"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Protected Sub chk_todos_CheckedChanged(sender As Object, e As EventArgs) Handles chk_todos.CheckedChanged
        If chk_todos.Checked Then
            Me.grd_subregion.Visible = True
            Me.cmb_subregion.Visible = False
        Else
            Me.grd_subregion.Visible = False
            Me.cmb_subregion.Visible = True
        End If

    End Sub
End Class