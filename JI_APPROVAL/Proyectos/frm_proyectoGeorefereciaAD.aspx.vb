Imports ly_SIME
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Public Class frm_proyectoGeorefereciaAD
    Inherits System.Web.UI.Page

    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "NEW_GIS000"
    Dim controles As New ly_SIME.CORE.cls_controles

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

        If Not IsPostBack Then
            Me.lbl_id_proyecto.Text = Me.Request.QueryString("Id").ToString
            LoadData()
            CargarDatos(Me.lbl_id_proyecto.Text)
            Me.rbn_norte.Attributes.Add("OnClick", "return ActualizarGO(document.aspnetForm,'" & Me.rbn_norte.ClientID.Replace(Me.rbn_norte.ID, "").Replace("_", "$") & "')")
            Me.rbn_sur.Attributes.Add("OnClick", "return ActualizarGO(document.aspnetForm,'" & Me.rbn_sur.ClientID.Replace(Me.rbn_sur.ID, "").Replace("_", "$") & "')")
            Me.rbn_este.Attributes.Add("OnClick", "return ActualizarGO(document.aspnetForm,'" & Me.rbn_este.ClientID.Replace(Me.rbn_este.ID, "").Replace("_", "$") & "')")
            Me.rbn_oeste.Attributes.Add("OnClick", "return ActualizarGO(document.aspnetForm,'" & Me.rbn_oeste.ClientID.Replace(Me.rbn_oeste.ID, "").Replace("_", "$") & "')")
            Me.txt_gLatitud.Attributes.Add("onchange", "return ActualizarGO(document.aspnetForm,'" & Me.txt_gLatitud.ClientID.Replace(Me.txt_gLatitud.ID, "").Replace("_", "$") & "')")
            Me.txt_mLatitud.Attributes.Add("onchange", "return ActualizarGO(document.aspnetForm,'" & Me.txt_mLatitud.ClientID.Replace(Me.txt_mLatitud.ID, "").Replace("_", "$") & "')")
            Me.txt_sLatitud.Attributes.Add("onchange", "return ActualizarGO(document.aspnetForm,'" & Me.txt_sLatitud.ClientID.Replace(Me.txt_sLatitud.ID, "").Replace("_", "$") & "')")
            Me.txt_gLongitud.Attributes.Add("onchange", "return ActualizarGO(document.aspnetForm,'" & Me.txt_gLongitud.ClientID.Replace(Me.txt_gLongitud.ID, "").Replace("_", "$") & "')")
            Me.txt_mLongitud.Attributes.Add("onchange", "return ActualizarGO(document.aspnetForm,'" & Me.txt_mLongitud.ClientID.Replace(Me.txt_mLongitud.ID, "").Replace("_", "$") & "')")
            Me.txt_sLongitud.Attributes.Add("onchange", "return ActualizarGO(document.aspnetForm,'" & Me.txt_sLongitud.ClientID.Replace(Me.txt_sLongitud.ID, "").Replace("_", "$") & "')")
        Else
            ComprobarPuntosGO(3)
        End If
    End Sub

    Sub LoadData()
        Using dbEntities As New dbRMS_HNEntities
            Dim id = Convert.ToInt32(Me.Request.QueryString("Id"))
            Dim oProyecto = dbEntities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = id)

            Me.lbl_id_proyecto.Text = id
            Me.lbl_codigo_ficha.Text = oProyecto.codigo_SAPME
            Me.lbl_nombre_ficha.Text = oProyecto.nombre_proyecto
            Me.lbl_nombre_ejecutor.Text = oProyecto.nombre_ejecutor
            Me.lbl_estado.Text = oProyecto.nombre_estado_ficha
            Me.lbl_nombre_subregion.Text = oProyecto.nombre_subregion
        End Using
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click
        'Me.RadWindowManager.Windows(0).NavigateUrl = "frm_proyectoMapa.aspx?lat=" & Me.lblPuntoLat.Text & "&Long=" & Me.lblPuntoLong.Text
        ComprobarPuntosGO(1)
        Dim Err As Boolean = ComprobarMapa(1)
        Dim sql As String = ""

        If Err = True Or (Err = False) Then

            Dim LatproyeccionNS As String = "N"
            Dim LatproyeccionEO As String = "E"

            If Me.rbn_sur.Checked = True Then
                LatproyeccionNS = "S"
            End If
            If Me.rbn_oeste.Checked = True Then
                LatproyeccionEO = "O"
            End If

            Try
                If lbl_guardado.Text = "NO" Then
                    sql = "INSERT INTO tme_FichaUbicacionProyecto(id_ficha_proyecto,descripcion_ubicacion, latitud, longitud, lat_grd, lat_min, lat_seg, long_grd, long_min, long_seg, "
                    sql &= " LatproyeccionNS, LongproyeccionEW, id_tipo_precisionFicha, id_tipo_ubicacioncodigo, MSNM, Punto )VALUES("
                    sql &= Me.lbl_id_proyecto.Text & ",'" & Me.txt_ubicacion.Text.Trim.Replace("'", "") & "'," & Me.lblPuntoLat.Text.Trim.Replace(",", ".") & ","
                    sql &= Me.lblPuntoLong.Text.Trim.Replace(",", ".") & "," & Me.txt_gLatitud.Value.ToString.Replace(",", ".") & "," & Me.txt_mLatitud.Value.ToString.Replace(",", ".")
                    sql &= "," & Me.txt_sLatitud.Value.ToString.Replace(",", ".") & "," & Me.txt_gLongitud.Value.ToString.Replace(",", ".") & ","
                    sql &= Me.txt_mLongitud.Value.ToString.Replace(",", ".") & "," & Me.txt_sLongitud.Value.ToString.Replace(",", ".") & ",'" & LatproyeccionNS & "','"
                    sql &= LatproyeccionEO & "'," & Me.cmb_precision.SelectedValue & "," & Me.cmb_codigo.SelectedValue & " ," & Me.txt_msnm.Value.ToString.Replace(",", ".")
                    sql &= ", geometry::Point(" & Me.lblPuntoLong.Text.Trim.Replace(",", ".") & ", " & Me.lblPuntoLat.Text.Trim.Replace(",", ".") & ", 4326))"
                Else
                    sql = "UPDATE tme_FichaUbicacionProyecto SET descripcion_ubicacion='" & Me.txt_ubicacion.Text.Trim.Replace(" '", "") & "', latitud=" & Me.lblPuntoLat.Text.Trim.Replace(",", ".") & ", longitud=" & Me.lblPuntoLong.Text.Trim.Replace(",", ".") & ", lat_grd=" & Me.txt_gLatitud.Value.ToString.Replace(",", ".") & ", "
                    sql &= "lat_min=" & Me.txt_mLatitud.Value.ToString.Replace(",", ".") & ", lat_seg=" & Val(Me.txt_sLatitud.Value).ToString.Replace(",", ".") & ", long_grd=" & Me.txt_gLongitud.Value.ToString.Replace(",", ".") & ", long_min=" & Me.txt_mLongitud.Value.ToString.Replace(",", ".") & ", long_seg=" & Val(Me.txt_sLongitud.Value).ToString.Replace(",", ".") & ", LatproyeccionNS='" & LatproyeccionNS & "', LongproyeccionEW='" & LatproyeccionEO & "'"
                    sql &= ",id_tipo_precisionFicha=" & Me.cmb_precision.SelectedValue & ",id_tipo_ubicacioncodigo=" & Me.cmb_codigo.SelectedValue & ",MSNM=" & Me.txt_msnm.Value.ToString.Replace(",", ".")
                    sql &= ",Punto= geometry::Point(" & Me.lblPuntoLong.Text.Trim.Replace(",", ".") & ", " & Me.lblPuntoLat.Text.Trim.Replace(",", ".") & ", 4326)"
                    sql &= " WHERE id_ficha_proyecto=" & Me.lbl_id_proyecto.Text
                End If

                cnnME.Open()
                Dim dm As New SqlCommand(sql, cnnME)
                dm.ExecuteNonQuery()
                dm.CommandText = "UPDATE tme_Ficha_Proyecto SET georeferencia_completa='SI' WHERE id_ficha_proyecto=" & Me.lbl_id_proyecto.Text
                dm.ExecuteNonQuery()
                cnnME.Close()

                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectosCuadroMando"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            Catch ex As Exception
                'Me.lbl_ErrorMapa.Visible = True
                'Me.lbl_ErrorMapa.Text = Me.lbl_ErrorMapa.Text & ": " & ex.Message.Trim & ":" & sql

            End Try

        End If

    End Sub
    Sub CargarDatos(ByVal id_proyecto As String)

        Dim dm As New SqlDataAdapter("SELECT * FROM vw_tme_Ficha_Proyecto WHERE id_ficha_proyecto=" & id_proyecto, cnnME)
        Dim ds As New DataSet("DsFichaProyecto")
        dm.Fill(ds, "DsFichaProyecto")
        Me.lbl_id_proyecto.Text = id_proyecto
        'Me.lbl_codigo_proyecto.Text = ds.Tables("DsFichaProyecto")(0)("codigo_SAPME")
        'Me.lbl_proyecto.Text = ds.Tables("DsFichaProyecto")(0)("nombre_proyecto")
        'Me.lbl_ejecutor.Text = ds.Tables("DsFichaProyecto")(0)("nombre_ejecutor")
        'Me.lbl_componente.Text = ds.Tables("DsFichaProyecto")(0)("nombre_componente")
        'Me.lbl_region.Text = ds.Tables("DsFichaProyecto")(0)("Region")
        'Me.lbl_estado.Text = ds.Tables("DsFichaProyecto")(0)("nombre_estado_ficha")
        If ds.Tables("DsFichaProyecto")(0)("ficha_requiereGIS").ToString = "NO" Then
            Me.btn_guardar.Visible = False
        End If
        cnnME.Open()

        ds.Tables.Add("DsGeoreferencia")
        dm.SelectCommand.CommandText = "SELECT * FROM tme_FichaUbicacionProyecto WHERE id_ficha_proyecto=" & id_proyecto
        dm.SelectCommand.ExecuteNonQuery()
        dm.Fill(ds, "DsGeoreferencia")
        cnnME.Close()

        If ds.Tables("DsGeoreferencia").Rows.Count > 0 Then
            Me.txt_ubicacion.Text = ds.Tables("DsGeoreferencia")(0)("descripcion_ubicacion")
            Me.txt_gLatitud.Text = ds.Tables("DsGeoreferencia")(0)("lat_grd")
            Me.txt_mLatitud.Text = ds.Tables("DsGeoreferencia")(0)("lat_min")
            Me.txt_sLatitud.Value = Val(ds.Tables("DsGeoreferencia")(0)("lat_seg"))
            Me.lblPuntoLat.Text = ds.Tables("DsGeoreferencia")(0)("latitud")

            Me.txt_gLongitud.Text = ds.Tables("DsGeoreferencia")(0)("long_grd")
            Me.txt_mLongitud.Text = ds.Tables("DsGeoreferencia")(0)("long_min")
            Me.txt_sLongitud.Value = Val(ds.Tables("DsGeoreferencia")(0)("long_seg"))
            Me.lblPuntoLong.Text = ds.Tables("DsGeoreferencia")(0)("longitud")
            Me.txt_msnm.Text = ds.Tables("DsGeoreferencia")(0)("MSNM")
            Me.cmb_codigo.DataBind()
            Me.cmb_codigo.SelectedValue = ds.Tables("DsGeoreferencia")(0)("id_tipo_ubicacioncodigo")
            Me.cmb_precision.DataBind()
            Me.cmb_precision.SelectedValue = ds.Tables("DsGeoreferencia")(0)("id_tipo_precisionFicha")

            If ds.Tables("DsGeoreferencia")(0)("LatproyeccionNS") = "S" Then
                Me.rbn_norte.Checked = False
                Me.rbn_sur.Checked = True
            End If

            If ds.Tables("DsGeoreferencia")(0)("LongproyeccionEW") = "E" Then
                Me.rbn_este.Checked = True
                Me.rbn_oeste.Checked = False
            End If
            Me.lbl_guardado.Text = "SI"
        Else
            lbl_guardado.Text = "NO"
        End If

    End Sub
    Protected Sub btn_guardar3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar3.Click
        ComprobarPuntosGO(2)
        ComprobarMapa(2)
        'Me.RadWindowManager.VisibleOnPageLoad = True
        'Me.RadWindowManager.Windows(0).NavigateUrl = "frm_proyectoMapa.aspx?lat=" & Me.lblPuntoLat.Text & "&Long=" & Me.lblPuntoLong.Text
        'Me.RadWindowManager.Visible = True
    End Sub

    Function ComprobarMapa(ByVal tipo As Integer) As Boolean
        'Me.lbl_ErrorMapa.Visible = False
        'Me.lbl_MapaOK.Visible = False
        'Me.chkConfirmacion.Visible = False
        'If Me.lblPuntoLat.Text = 0 Or Me.lblPuntoLong.Text = 0 Then
        '    lbl_ErrorMapa.Text = "[ Latitud y Longitud requerida ]"
        '    lbl_ErrorMapa.Visible = True
        '    Return False
        'Else
        '    cnnME.Open()
        '    Dim dm As New SqlDataAdapter("SELECT NOMBRE_DPT, NOMBRE_MPI FROM tme_MuniColombiaGIS WHERE (geom.STContains(GEOMETRY::STGeomFromText('POINT (" & Me.lblPuntoLong.Text.Replace(",", ".") & " " & Me.lblPuntoLat.Text.Replace(",", ".") & ")', 4326)) = 1)", cnnME)
        '    Dim ds As New DataSet("dsMapMunicipios")
        '    dm.Fill(ds, "dsMapMunicipios")
        '    cnnME.Close()
        '    If ds.Tables("dsMapMunicipios").Rows.Count = 0 Then
        '        lbl_ErrorMapa.Text = "[ La coordenada está fuera de la área geográfica ] "
        '        lbl_ErrorMapa.Visible = True
        '        If tipo = 1 Then
        '            lbl_ErrorMapa.Text &= "Requiere confirmación."
        '            Me.chkConfirmacion.Visible = True
        '        End If
        '        Return False
        '    Else
        '        Me.lbl_MapaOK.Visible = True
        '        Return True
        '    End If
        'End If
        Return True
    End Function

    Sub ComprobarPuntosGO(ByVal tipo As Integer)
        Me.lblPuntoLat.Text = FormatNumber((Val(Me.txt_gLatitud.Value) + Val(Me.txt_mLatitud.Value) / 60 + Val(Me.txt_sLatitud.Value) / 3600), 8, , TriState.True, TriState.True).ToString
        If rbn_sur.Checked = True Then
            Me.lblPuntoLat.Text = "-" & Me.lblPuntoLat.Text
        End If
        Me.lblPuntoLong.Text = FormatNumber((Val(Me.txt_gLongitud.Value) + Val(Me.txt_mLongitud.Value) / 60 + Val(Me.txt_sLongitud.Value) / 3600), 8, , TriState.True, TriState.True).ToString
        If rbn_oeste.Checked = True Then
            Me.lblPuntoLong.Text = "-" & Me.lblPuntoLong.Text
        End If
        If tipo = 2 Then
            'Me.chkConfirmacion.Checked = False
        End If
    End Sub
    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/proyectos/frm_proyectoGeoreferecia"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

End Class