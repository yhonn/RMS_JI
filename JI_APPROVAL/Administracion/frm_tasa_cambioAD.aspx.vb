Imports ly_SIME
Imports Telerik.Web.UI

Public Class frm_tasa_cambioAD
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "APP_AD_TACA"
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
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
                'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next

            'Me.btn_eliminar.Text = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "btn_eliminar").texto
        End If

        If Not IsPostBack Then
            Try
                Using dbEntities As New dbRMS_JIEntities
                    Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
                    Dim oPeriodos = dbEntities.vw_t_periodos.Where(Function(p) p.id_programa = id).ToList()
                    Dim oPeriodo = oPeriodos.Where(Function(p) p.activo.Value)
                    LoadList()
                    LoadTrimestre(oPeriodos, Me.cmb_year.SelectedValue)
                End Using
            Catch ex As Exception
                Dim a = ex
            End Try
        End If
    End Sub
    Public Function Meses(ByVal idTrimestre As Integer) As List(Of meses)
        Dim oTrimestre
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
        Dim ListMeses As New List(Of meses)
        Using dbEntities As New dbRMS_JIEntities
            oTrimestre = dbEntities.t_trimestre.Find(idTrimestre)
            Dim tasasRegistradas = dbEntities.t_trimestre_tasa_cambio.Where(Function(p) p.id_trimestre = idTrimestre And p.id_programa = id).ToList()
            idTrimestre = oTrimestre.Codigo_anio_calendario
            Dim trimestre = 1
            Dim nombreMes = ""
            Dim idmes = 1
            For imes = 1 To 12
                Select Case imes
                    Case 1 To 3
                        trimestre = 1
                    Case 4 To 6
                        trimestre = 2
                    Case 7 To 9
                        trimestre = 3
                    Case 10 To 12
                        trimestre = 4
                End Select
                nombreMes = MonthName(imes, True)
                idmes = imes
                If tasasRegistradas.Where(Function(p) p.mes = idmes).Count = 0 Then
                    ListMeses.Add(New meses() With { _
                             .mes = imes,
                             .nombre_mes = nombreMes,
                             .id_trimestre = trimestre
                        })
                End If

                
            Next
            
        End Using
        ListMeses = ListMeses.Where(Function(p) p.id_trimestre = idTrimestre).ToList()
        Return ListMeses
    End Function
    Sub LoadList()
        Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())


        Using dbEntities As New dbRMS_JIEntities
            Dim programa = dbEntities.t_programas.Find(id)

            Me.cmb_year.DataSourceID = ""
            Me.cmb_year.DataSource = dbEntities.t_trimestre.Where(Function(p) p.anio >= programa.fecha_inicio.Value.Year And p.anio <= (programa.fecha_inicio.Value.Year + programa.numero_anios)) _
                .GroupBy(Function(p) p.anio) _
                .Select(Function(p) p.FirstOrDefault()) _
                .Select(Function(p) _
                            New With {Key .anio = p.anio}).ToList()
        End Using


        Me.cmb_year.DataTextField = "anio"
        Me.cmb_year.DataValueField = "anio"
        Me.cmb_year.DataBind()

    End Sub
    Protected Sub cmb_year_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_year.SelectedIndexChanged
        Using dbEntities As New dbRMS_JIEntities
            Dim id = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())
            Dim oPeriodos = dbEntities.vw_t_periodos.Where(Function(p) p.id_programa = id).ToList()
            LoadTrimestre(oPeriodos, Me.cmb_year.SelectedValue)
        End Using
    End Sub
    Sub LoadTrimestre(ByVal oPeriodos As List(Of vw_t_periodos), ByVal anio As Integer)
        Dim oTrimestres = oPeriodos.Where(Function(p) p.anio = anio) _
                             .GroupBy(Function(p) p.id_trimestre) _
                             .Select(Function(p) p.FirstOrDefault()) _
                             .Select(Function(p) _
                                         New With {Key .nombre_trimestre = "FY" & p.ciclo.Value.ToString() & "-" & p.codigo_anio_fiscal, _
                                                   Key .id_trimestre = p.id_trimestre}).ToList()

        Me.cmb_periodo.DataSource = oTrimestres
        Me.cmb_periodo.DataTextField = "nombre_trimestre"
        Me.cmb_periodo.DataValueField = "id_trimestre"
        Me.cmb_periodo.DataBind()

        Me.cmb_mes.DataSource = Meses(cmb_periodo.SelectedValue)
        Me.cmb_mes.DataTextField = "nombre_mes"
        Me.cmb_mes.DataValueField = "mes"
        Me.cmb_mes.DataBind()
    End Sub
    
    Protected Sub cmb_periodo_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_periodo.SelectedIndexChanged
        Me.cmb_mes.DataSource = Meses(cmb_periodo.SelectedValue)
        Me.cmb_mes.DataTextField = "nombre_mes"
        Me.cmb_mes.DataValueField = "mes"
        Me.cmb_mes.DataBind()
    End Sub

    Protected Sub btn_guardar_Click(sender As Object, e As EventArgs) Handles btn_guardar.Click
        Try
            Using dbEntities As New dbRMS_JIEntities
                Dim id_programa = Convert.ToInt32(Session("E_IdPrograma"))
                Dim oTasaCambio As New t_trimestre_tasa_cambio
                oTasaCambio.id_trimestre = Me.cmb_periodo.SelectedValue
                oTasaCambio.tasa_cambio = Me.txt_tasa_cambio.Value
                oTasaCambio.mes = Me.cmb_mes.SelectedValue
                oTasaCambio.id_usuario_crea = Convert.ToInt32(Me.Session("E_IdUser").ToString())
                oTasaCambio.fecha_crea = Date.Now
                oTasaCambio.id_programa = id_programa
                dbEntities.t_trimestre_tasa_cambio.Add(oTasaCambio)
                dbEntities.SaveChanges()
                Dim trimestre = dbEntities.t_trimestre.Find(oTasaCambio.id_trimestre)
                trimestre.tasa_cambio = trimestre.t_trimestre_tasa_cambio.Average(Function(p) p.tasa_cambio)
                dbEntities.Entry(trimestre).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
                Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
                Me.MsgGuardar.Redireccion = "~/Administracion/frm_tasas_cambio"
                ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
            End Using
        Catch
        End Try
    End Sub
    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/Administracion/frm_tasa_cambio")
    End Sub
End Class