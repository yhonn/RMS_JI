Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI

Public Class frm_ProyectoAvanceMetasPrintInfo
    Inherits System.Web.UI.Page


    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "MGR_OPEN01"


    Sub CargarDatos(ByVal IdProyecto As String)
        cnnME.Open()
        Dim dm As New SqlDataAdapter("SELECT * FROM vw_tme_Ficha_Proyecto WHERE id_ficha_proyecto=" & IdProyecto, cnnME)
        Dim ds As New DataSet("DsFichaProyecto")
        dm.Fill(ds, "DsFichaProyecto")
        Me.lbl_id_ficha_proyecto.Text = IdProyecto
        Me.lbl_proyecto.Text = ds.Tables("DsFichaProyecto")(0)("nombre_proyecto")
        Me.lbl_region.Text = ds.Tables("DsFichaProyecto")(0)("nombre_region")
        Me.lbl_ejecutor.Text = ds.Tables("DsFichaProyecto")(0)("nombre_ejecutor")
        Me.lbl_componente.Text = ds.Tables("DsFichaProyecto")(0)("nombre_componente")
        Me.lbl_codigoAID.Text = ds.Tables("DsFichaProyecto")(0)("codigo_ficha_AID")
        Me.lbl_codigoSAPME.Text = ds.Tables("DsFichaProyecto")(0)("codigo_SAPME")
        Me.lbl_estadoFicha.Text = ds.Tables("DsFichaProyecto")(0)("nombre_estado_ficha")
        Me.lbl_tasaCambio.Text = FormatNumber(Val(ds.Tables("DsFichaProyecto")(0)("tasa_cambio")), 2).ToString
        Me.lbl_fechainicio.Text = ds.Tables("DsFichaProyecto")(0)("fecha_inicio_proyecto")
        Me.lbl_fechafin.Text = ds.Tables("DsFichaProyecto")(0)("fecha_fin_proyecto")
        'Me.lbl_codigoRFA.Text = ds.Tables("DsFichaProyecto")(0)("codigo_RFA")


        Dim sql As String = "SELECT tme_FichaProyectoOrdenInicioIndicador.id_indicador FROM tme_FichaProyectoOrdenInicioIndicador INNER JOIN"
        sql &= " tme_FichaProyectoOrdenInicio ON tme_FichaProyectoOrdenInicioIndicador.id_ficha_proyecto_ordInicio = tme_FichaProyectoOrdenInicio.id_ficha_proyecto_ordInicio"
        sql &= " WHERE id_ficha_proyecto =" & IdProyecto
        ds.Tables.Add("IdRepsimple")
        dm.SelectCommand.CommandText = sql
        dm.SelectCommand.ExecuteNonQuery()
        dm.Fill(ds, "IdRepsimple")

        Dim dmIns As New SqlDataAdapter("SELECT id_trimestre, CAST(anio AS varchar(50)) + ' - Q' + CAST(ciclo as varchar(50)) AS Periodo, bloqueado FROM vw_t_periodos WHERE (id_programa = " & ds.Tables("DsFichaProyecto")(0)("id_programa") & ") AND (activo = 1)", cnnME)
        Dim dsIns As New DataSet("dsMenu")
        dmIns.Fill(dsIns, "dsMenu")
        Me.lbl_periodoActivo.Text = dsIns.Tables("dsMenu")(0)("Periodo")
        If dsIns.Tables("dsMenu")(0)("bloqueado") = "1" Then
            Me.lbl_periodoActivoEstatus.Text = "Periodo BLOQUEADO. No permite registrar datos."
            Me.img_periodoActivoEstatus.ImageUrl = "~/Imagenes/iconos/flag_red.png"
            Me.img_periodoActivoEstatus.ToolTip = "Periodo BLOQUEADO"
        End If

        If ds.Tables("DsFichaProyecto")(0)("ActualizacionReciente") = "SI" Then
            Me.lbl_periodoActivoEstatusSync.Text = "Los datos requiere iniciar el proceso de sincronización para calcular/acumular datos a la Región."
            Me.img_periodoActivoEstatusSync.ImageUrl = "~/Imagenes/iconos/Informacion2.png"
            Me.img_periodoActivoEstatusSync.ToolTip = "Actualización pendiente"
        End If

        cnnME.Close()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try
        '    If Me.Session("E_IdUser").ToString = "" Then
        '    End If
        'Catch ex As Exception
        '    Me.Response.Redirect("~/frm_login")
        'End Try

        'If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
        '    cl_user = Session.Item("clUser")
        '    If Not cl_user.chk_accessMOD(0, frmCODE) Then
        '        Me.Response.Redirect("~/Proyectos/no_access2")
        '    End If
        'End If

        If Not IsPostBack Then
            CargarDatos(Me.Request.QueryString("id").ToString)
            creaNodosTreeViewTabla(Me.Request.QueryString("id").ToString)
        End If
    End Sub
    Sub CrearNodos(ByVal IdProyecto As String)

    End Sub

    Sub creaNodosTreeViewTabla(ByVal IdProyecto As String)

        cnnME.Open()
        Dim dsDetMetas As New DataSet("DetMetas")
        Dim dmDetMetas As New SqlDataAdapter("SELECT ROW_NUMBER() OVER(ORDER BY id_meta_indicador_ficha ASC) AS No, nombre_indicador_LB AS 'Indicador', nombre_tipo_umedida AS UM, '<script> document.write(formatNumber.new(' + REPLACE(CAST(meta_total AS Varchar(50)), ',', '.') + '));</script>' AS 'Meta Total', '<script> document.write(formatNumber.new(' + REPLACE(CAST(10 AS Varchar(50)), ',', '.') + '));</script>' AS Ejecutado, 10 '% Avance', dbo.DetAalertaAvance(0) As '&nbsp;' FROM vw_tme_ficha_meta_indicador WHERE id_ficha_proyecto =" & IdProyecto, cnnME)
        dmDetMetas.SelectCommand.ExecuteNonQuery()
        dmDetMetas.Fill(dsDetMetas, "DetMetas")

        Me.TreeViewMetas.DataSource = Me.SqlDataSource9
        Me.TreeViewMetas.DataBind()

        Dim WithTable As Integer = 850
        Dim PadreLB As New RadTreeNode
        PadreLB.Text = "Planificación y Resultados"
        Dim radTablaLB As New RadGrid()
        radTablaLB.DataSource = dsDetMetas
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
        TextIfram &= " scrolling = ""no"" Style = ""color: White; width:" & WithTable & "px; margin-right: 0px; height: " & (NumberRow * 150).ToString & "px;"""
        TextIfram &= " src=""grap/grp_GraphAvanceEjecucionInd?Id=" & IdProyecto & "&IdRepProyMELong=" & (NumberRow * 100).ToString & """> </iframe>"

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
        Dim sql As String = "SELECT id_meta_indicador_ficha, id_indicador, definicion_indicador FROM vw_tme_ficha_meta_indicador WHERE id_ficha_proyecto =" & IdProyecto
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
            sql = "SELECT  TOP (10) CAST(anio AS varchar(10)) + ' - Q' + ciclo AS Periodo, datecreated AS 'Fecha repote', " _
                & "'<script> document.write(formatNumber.new(' + REPLACE(CAST(ejecutado_ficha AS Varchar(50)), ',', '.') + '));</script>' AS 'Ejecutado', nombre_indicador_avance AS 'Origen', " _
                & "TAG_img_Print AS Ficha, TAG_img_validacion As 'Estado' FROM vw_tme_ficha_meta_indicadorAvance WHERE id_ficha_proyecto=" & IdProyecto & " AND id_meta_indicador_ficha=" & ds.Tables("IDFichaAvance").Rows(i).Item("id_meta_indicador_ficha").ToString & " ORDER BY 2"
            sql = "SELECT * from vw_tme_ficha_meta_indicador WHERE id_indicador = 100"
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
            HlnkTool.NavigateUrl = "frm_ProyectoAvanceMetasRSPrintAll.aspx?id=" & IdProyecto & "&IdArIND=" & ds.Tables("IDFichaAvance").Rows(i).Item("id_indicador").ToString
            HlnkTool.ImageUrl = "~/Imagenes/iconos/printer_off.png"
            Hijo.Controls.Add(HlnkTool)


            Dim LblIndicadoresTrim As New Label()
            LblIndicadoresTrim.Font.Size = 8
            LblIndicadoresTrim.Text = "<br /><br />"
            Hijo.Controls.Add(LblIndicadoresTrim)

            TextIfram = "<iframe id=""iframeResumen"" runat=""server"" frameborder=""0"" name=""I2"" "
            TextIfram &= " scrolling = ""no"" Style = ""color: White; width:" & WithTable & "px; margin-right: 0px; height: 200px;"""
            TextIfram &= " src=""grap/grp_GraphAvanceEjecucionIndDet.asp?IdRepAr=" & ds.Tables("IDFichaAvance").Rows(i).Item("id_meta_indicador_ficha").ToString & "&IdRepProyME=" & IdProyecto & """> </iframe>"

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

End Class