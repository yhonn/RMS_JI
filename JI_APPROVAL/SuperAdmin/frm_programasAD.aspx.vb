Imports Telerik.Web.UI
Imports ly_SIME
Public Class frm_programasAD
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_NEW_PROG"
    Dim clListado As New ly_SIME.CORE.cls_listados
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
            FillData()
        End If

    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click
        Dim Sql As String = ""
        Dim err As Boolean = True
        Dim errSave As Boolean = False

        If errSave = False Then

            Using dbEntities As New dbRMS_JIEntities
                Dim oprograma As New t_programas

                oprograma.nombre_programa = Me.txt_nombrePrograma.Text
                oprograma.descripcion_programa = Me.txt_descripcion.Text
                oprograma.codigo_programa = Me.txt_codigoprograma.Text
                oprograma.monto_programa = Me.txt_monto.Value
                oprograma.fecha_inicio = Me.dt_fecha_inicio.SelectedDate
                oprograma.fecha_fin = Me.dt_fecha_fin.SelectedDate
                oprograma.equivalente_dollar = Me.txt_montoDolares.Value
                oprograma.numero_anios = Me.txt_numeroanio.Value
                oprograma.id_pais = Me.cmb_pais.SelectedValue
                oprograma.id_cliente = Me.cmb_cliente.SelectedValue
                oprograma.id_rep_frecuencia_indicador = Me.cmb_frecuencia.SelectedValue
                oprograma.anio_inicio = Me.dt_fecha_inicio.SelectedDate.Value.Year
                oprograma.numero_contrato = Me.txt_numerocontrato.Text
                oprograma.tasa_cambio = Me.txt_tasa.Value
                oprograma.id_language = Me.cmb_idioma.SelectedValue
                oprograma.prefijo_actividades = Me.txt_codigoprograma.Text

                oprograma.datecreated = Date.Now
                oprograma.id_usuario_creo = Me.Session("E_IdUser").ToString()


                dbEntities.t_programas.Add(oprograma)
                dbEntities.SaveChanges()

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/SuperAdmin/frm_programas"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End If

    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/SuperAdmin/frm_programas"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    End Sub

    Sub FillData()
        Using db As New dbRMS_JIEntities
            Me.cmb_pais.DataSource = clListado.get_t_pais()
            Me.cmb_pais.DataTextField = "nombre_pais"
            Me.cmb_pais.DataValueField = "id_pais"
            Me.cmb_pais.DataBind()

            Me.cmb_cliente.DataSource = clListado.get_t_cliente()
            Me.cmb_cliente.DataTextField = "nombre_cliente"
            Me.cmb_cliente.DataValueField = "id_cliente"
            Me.cmb_cliente.DataBind()

            Me.cmb_frecuencia.DataSource = clListado.get_tme_rep_frecuencia_indicador()
            Me.cmb_frecuencia.DataTextField = "nombre_rep_frecuencia_indicador"
            Me.cmb_frecuencia.DataValueField = "id_rep_frecuencia_indicador"
            Me.cmb_frecuencia.DataBind()

            Me.cmb_idioma.DataSource = clListado.get_t_idiomas()
            Me.cmb_idioma.DataTextField = "descripcion_idioma"
            Me.cmb_idioma.DataValueField = "id_idioma"
            Me.cmb_idioma.DataBind()
        End Using
    End Sub

    'Protected Sub cmb_pais_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)

    '    cmb_pais.DataSource = clListado.get_t_pais(e.Text)
    '    cmb_pais.DataBind()

    'End Sub


End Class