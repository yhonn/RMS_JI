Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Configuration.ConfigurationManager
Imports ly_SIME
Imports DotNet.Highcharts
Imports DotNet.Highcharts.Options
Imports DotNet.Highcharts.Enums

Public Class frm_ProyectoAvanceMetas
    Inherits System.Web.UI.Page

    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "MGR_PROY01"


    Sub CargarDatos(ByVal IdProyecto As String)
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))
        Using dbentities As New dbRMS_HNEntities
            Dim oProyecto = dbentities.vw_tme_ficha_proyecto.FirstOrDefault(Function(p) p.id_ficha_proyecto = IdProyecto)
            Me.lbl_id_ficha_proyecto.Text = IdProyecto
            Me.lbl_proyecto.Text = oProyecto.nombre_proyecto
            Me.lbl_region.Text = oProyecto.nombre_subregion
            Me.lbl_ejecutor.Text = oProyecto.nombre_ejecutor
            Me.lbl_componente.Text = oProyecto.nombre_componente
            Me.lbl_codigoSAPME.Text = oProyecto.codigo_SAPME
            Me.lbl_estadoFicha.Text = oProyecto.nombre_estado_ficha
            Me.lbl_tasaCambio.Text = oProyecto.tasa_cambio.Value.ToString("C2", cl_user.regionalizacionCulture)
            Me.lbl_fechainicio.Text = oProyecto.fecha_inicio_proyecto
            Me.lbl_fechafin.Text = oProyecto.fecha_fin_proyecto

            Dim periodo = dbentities.vw_t_periodos.FirstOrDefault(Function(p) p.id_programa = id_programa And p.activo)
            Me.lbl_periodoActivo.Text = periodo.codigo_anio_fiscal & "-" & periodo.anio

            If periodo.bloqueado Then
                Me.btn_guardarSync.Visible = False
                Me.btn_guardar.Visible = False
                Me.lbl_periodoActivoEstatus.Text = "Periodo BLOQUEADO. No permite registrar datos."
                Me.img_periodoActivoEstatus.ImageUrl = "~/Imagenes/iconos/flag_red.png"
                Me.img_periodoActivoEstatus.ToolTip = "Periodo BLOQUEADO"
            End If

            Me.grd_cate.DataSource = dbentities.vw_tme_ficha_meta_indicador_avance.Where(Function(p) p.id_ficha_proyecto = IdProyecto).ToList()
            Me.grd_cate.DataBind()
        End Using
        cnnME.Open()
        Dim dm As New SqlDataAdapter("SELECT * FROM vw_tme_ficha_proyecto WHERE id_ficha_proyecto=" & IdProyecto, cnnME)
        Dim ds As New DataSet("DsFichaProyecto")
        dm.Fill(ds, "DsFichaProyecto")

        Dim sql As String = "SELECT tme_FichaProyectoOrdenInicioIndicador.id_indicador FROM tme_FichaProyectoOrdenInicioIndicador INNER JOIN"
        sql &= " tme_FichaProyectoOrdenInicio ON tme_FichaProyectoOrdenInicioIndicador.id_ficha_proyecto_ordInicio = tme_FichaProyectoOrdenInicio.id_ficha_proyecto_ordInicio"
        sql &= " WHERE id_ficha_proyecto =" & IdProyecto
        ds.Tables.Add("IdRepsimple")
        dm.SelectCommand.CommandText = sql
        dm.SelectCommand.ExecuteNonQuery()
        dm.Fill(ds, "IdRepsimple")

        'If ds.Tables("DsFichaProyecto")(0)("ActualizacionReciente") = "SI" Then
        '    Me.lbl_periodoActivoEstatusSync.Text = "Los datos requiere iniciar el proceso de sincronización para calcular/acumular datos a la Región."
        '    Me.img_periodoActivoEstatusSync.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
        '    Me.img_periodoActivoEstatusSync.ToolTip = "Actualización pendiente"
        'End If

        If ds.Tables("IdRepsimple").Rows.Count = 0 Then
            Me.btn_guardar.Visible = False
        End If

        'If Me.Session("E_NivelProy") = "2" Or Me.Session("E_NivelProy") = "-1" Then
        '    Me.btn_guardarSync.Visible = False
        'End If

        cnnME.Close()

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
            'If Not cl_user.chk_accessMOD(0, frmCODE) Then
            '    Me.Response.Redirect("~/Proyectos/no_access2.aspx")
            'Else
            '    cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            'End If
        End If


        If Not IsPostBack Then
            Dim id_ficha = Convert.ToInt32(Me.Session("E_IdFicha"))
            CargarDatos(id_ficha)
            get_chart(id_ficha)
            'creaNodosTreeViewTabla(Me.Request.QueryString("id"))
        End If
    End Sub

    Sub creaNodosTreeViewTabla(ByVal IdProyecto As String)

        cnnME.Open()
        Dim dsDetMetas As New DataSet("DetMetas")
        Dim dmDetMetas As New SqlDataAdapter("SELECT ROW_NUMBER() OVER(ORDER BY id_meta_ficha ASC) AS No, nombre_indicador_LB AS 'Indicador', nombre_tipo_umedida AS UM, '<script> document.write(formatNumber.new(' + REPLACE(CAST(meta_total_ficha AS Varchar(50)), ',', '.') + '));</script>' AS 'Meta Total', '<script> document.write(formatNumber.new(' + REPLACE(CAST(Ejecutado AS Varchar(50)), ',', '.') + '));</script>' AS Ejecutado, EjecutadoPERTXT '% Avance', dbo.FN_ME_DetAalertaAvance(EjecutadoPERTXT) As '&nbsp;' FROM vw_tme_MetaIndicadoresFicha WHERE id_ficha_proyecto =" & IdProyecto, cnnME)
        dmDetMetas.SelectCommand.ExecuteNonQuery()
        dmDetMetas.Fill(dsDetMetas, "DetMetas")

        Me.TreeViewMetas.DataSource = Me.SqlDataSource9
        Me.TreeViewMetas.DataBind()

        Dim WithTable As Integer = 850
        Dim PadreLB As New RadTreeNode
        PadreLB.Text = "Planificación y Resultados"
        Dim radTablaLB As New RadGrid()
        radTablaLB.DataSource = dsDetMetas
        radTablaLB.Skin = "Sunset"
        radTablaLB.Width = WithTable
        radTablaLB.MasterTableView.Width = WithTable
        radTablaLB.ClientSettings.EnableRowHoverStyle = True
        'radTablaLB.ClientSettings.Selecting.AllowRowSelect = True

        Dim HijoLB As RadTreeNode = New RadTreeNode
        HijoLB.Controls.Add(radTablaLB)

        Dim LblIndicadoresTrimTabLB1 As New Label()
        LblIndicadoresTrimTabLB1.Text = "<br />"
        HijoLB.Controls.Add(LblIndicadoresTrimTabLB1)
        Dim NumberRow As Integer = Val(dsDetMetas.Tables("DetMetas").Rows.Count())
        If NumberRow = 1 Then
            NumberRow = 2
        End If

        Dim TextIfram As String = "<iframe id=""iframeResumen"" runat=""server"" frameborder=""0"" name=""I1"" "
        TextIfram &= " scrolling = ""no"" Style = ""color: White; width:" & WithTable & "px; margin-right: 0px; height: " & (NumberRow * 100).ToString & "px;"""
        TextIfram &= " src=""grap/grp_GraphAvanceEjecucionInd.asp?IdRepProyME=" & IdProyecto & "&IdRepProyMELong= " & (NumberRow * 100).ToString & """> </iframe>"

        Dim IframeGrafico As New Panel
        IframeGrafico.Controls.Add(New LiteralControl(TextIfram))
        HijoLB.Controls.Add(IframeGrafico)

        'Dim hyperlink As New HyperLink
        'hyperlink.NavigateUrl = "ExportExcelDescripcionPMP.aspx?IdProy=" + IdProyecto
        'hyperlink.Target = "_blank"
        'hyperlink.ToolTip = "Exportar Datos"
        'hyperlink.Text = " >>Exportar Datos"
        'HijoLB.Controls.Add(hyperlink)

        PadreLB.Nodes.Add(HijoLB)
        Me.TreeViewMetas.Nodes.Add(PadreLB)

        Dim TxtActivo As String = ""
        Dim i As Integer
        Dim sql As String = "SELECT id_meta_ficha, id_indicador, definicion_indicador FROM vw_tme_MetaIndicadoresFicha WHERE id_ficha_proyecto =" & IdProyecto
        Dim dm As New SqlDataAdapter(sql, cnnME)
        Dim ds As New DataSet("IDFichaAvance")
        dm.Fill(ds, "IDFichaAvance")

        For i = 0 To ds.Tables("IDFichaAvance").Rows.Count() - 1

            Dim Title As String = ds.Tables("IDFichaAvance").Rows(i).Item("definicion_indicador").ToString
            If ds.Tables("IDFichaAvance").Rows(i).Item("definicion_indicador").ToString.Length > 150 Then
                Title = ds.Tables("IDFichaAvance").Rows(i).Item("definicion_indicador").ToString.Substring(0, 150) & "..."
            End If

            Dim Padre As New RadTreeNode
            Padre.Text = Title
            Dim Hijo As New RadTreeNode
            Dim radTabla As New RadGrid()
            sql = "SELECT  TOP (10) CAST(anio AS varchar(10)) + ' - Q' + ciclo AS Periodo, datecreated AS 'Fecha repote', '<script> document.write(formatNumber.new(' + REPLACE(CAST(ejecutado_ficha AS Varchar(50)), ',', '.') + '));</script>' AS 'Ejecutado', nombre_indicador_avance AS 'Origen',   TAG_img_Print AS Ficha, TAG_img_validacion As 'Estado' FROM vw_tme_MetaIndicadoresFichaAvance WHERE id_ficha_proyecto=" & IdProyecto & " AND id_meta_ficha=" & ds.Tables("IDFichaAvance").Rows(i).Item("id_meta_ficha").ToString & " ORDER BY 2"
            Dim dmCat As New SqlDataAdapter(sql, cnnME)
            Dim dsCat As New DataSet("Catologo")
            dmCat.Fill(dsCat, "Catologo")
            radTabla.DataSource = dsCat
            radTabla.Width = WithTable
            radTabla.MasterTableView.Width = WithTable
            radTabla.ClientSettings.EnableRowHoverStyle = True
            Hijo.Controls.Add(radTabla)

            Dim LblIndicadoresTrimAlert As New Label()
            LblIndicadoresTrimAlert.Font.Size = 8
            LblIndicadoresTrimAlert.Font.Italic = True
            LblIndicadoresTrimAlert.Text = "Sólo se muestran las últimas 10 actualizaciones. Imprimir reporte completo: "
            Hijo.Controls.Add(LblIndicadoresTrimAlert)

            Dim HlnkTool As New HyperLink
            HlnkTool.Target = "_blank"
            HlnkTool.ToolTip = "Mostrar reporte completo"
            HlnkTool.NavigateUrl = "frm_ProyectoAvanceMetasRSPrintAll.aspx?IdProyME=" & IdProyecto & "&IdArIND=" & ds.Tables("IDFichaAvance").Rows(i).Item("id_indicador").ToString
            HlnkTool.ImageUrl = "~/Imagenes/iconos/printer_off.png"
            Hijo.Controls.Add(HlnkTool)


            Dim LblIndicadoresTrim As New Label()
            LblIndicadoresTrim.Font.Size = 8
            LblIndicadoresTrim.Text = "<br /><br />"
            Hijo.Controls.Add(LblIndicadoresTrim)

            TextIfram = "<iframe id=""iframeResumen"" runat=""server"" frameborder=""0"" name=""I2"" "
            TextIfram &= " scrolling = ""no"" Style = ""color: White; width:" & WithTable & "px; margin-right: 0px; height: 200px;"""
            TextIfram &= " src=""grap/grp_GraphAvanceEjecucionIndDet.asp?IdRepAr=" & ds.Tables("IDFichaAvance").Rows(i).Item("id_meta_ficha").ToString & "&IdRepProyME=" & IdProyecto & """> </iframe>"

            Dim IframeGraficoDet As New Panel
            IframeGraficoDet.Controls.Add(New LiteralControl(TextIfram))
            Hijo.Controls.Add(IframeGraficoDet)
            Padre.Nodes.Add(Hijo)
            Me.TreeViewMetas.Nodes.Add(Padre)
        Next

        Me.TreeViewMetas.Nodes(0).Expanded = True
        'Me.TreeViewMetas.ExpandAllNodes()
        cnnME.Close()

    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click
        Me.Response.Redirect("frm_ProyectoAvanceMetasRSAD.aspx")
    End Sub

    Protected Sub btn_guardar0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardarSync.Click
        Me.Response.Redirect("frm_ProyectoAvanceMetasRSADSynProyecto.aspx?IdRepProyME=" & Me.lbl_id_ficha_proyecto.Text)
    End Sub

    Function get_chart(ByVal id_proyecto As Integer) As String
        Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Using dbEntities As New dbRMS_HNEntities
            Dim indicadoresProyecto = dbEntities.vw_tme_ficha_meta_indicador.Where(Function(p) p.id_ficha_proyecto = id_proyecto) _
                      .Select(Function(p) p.id_indicador).ToList()

            Dim indicadores = dbEntities.vw_indicador_metas_anuales.Where(Function(p) p.id_programa = id_programa And p.anio = 2016 And indicadoresProyecto.Contains(p.id_indicador)) _
                              .GroupBy(Function(p) p.id_indicador) _
                              .Select(Function(p) p.FirstOrDefault()).ToList()

            Dim datos = New List(Of Series)()
            Dim proyectado = New List(Of Object)
            Dim ejecutado = New List(Of Object)
            Dim aaaaaaa = New Series()
            Dim categories = New List(Of String)
            For Each item In indicadores
                categories.Add(item.codigo_indicador)
                aaaaaaa.Data = New Helpers.Data({item.porcentaje})
                ejecutado.Add(item.porcentaje)
            Next
            datos.Add(New Series With {.Name = "Progress %", .Data = New Helpers.Data(ejecutado.ToArray())})
            Dim chart As Highcharts = New Highcharts("chart").InitChart(New Chart() With { _
                                .PlotShadow = False, .Type = ChartTypes.Bar _
                            }).SetTitle(New Title() With { _
                                        .Text = "Indicator Progress" _
                                    }).SetPlotOptions(New PlotOptions() With {.Column = New PlotOptionsColumn() With { _
                                                      .Stacking = Stackings.Percent _
                                                  }}).SetSeries(datos.ToArray()) _
                                                         .SetXAxis(New XAxis() With { _
                                                                   .Categories = categories.ToArray()})

            Me.ltrChart.Text = chart.ToHtmlString().Replace("[[", "[").Replace("]]", "]")



            Return chart.ChartScriptHtmlString().ToString().Replace("[[", "[").Replace("]]", "]")
        End Using



    End Function

End Class