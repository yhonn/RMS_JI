Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Public Class frm_entregableSgmt
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_CONT_SGMT_ENTR"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtItinerario As New DataTable
    Dim dtAlojamiento As New DataTable
    Dim clss_approval As APPROVAL.clss_approval
    Const cPENDING = 1
    Const cAPPROVED = 2
    Const cnotAPPROVED = 3
    Const cCANCELLED = 4
    Const cOPEN = 5
    Const cSTANDby = 6
    Const cCOMPLETED = 7

    Const cAction_ByProcess = 1
    Const cAction_ByMessage = 2


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
                'cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_entregables)
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            Me.idEntregable.Value = Convert.ToInt32(Me.Request.QueryString("id"))
            loadData()
            fillGrid()
            fillGridComentarios()
        End If

    End Sub
    Sub loadData()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_entregable = Convert.ToInt32(Me.idEntregable.Value)
            Dim oEntregable = dbEntities.tme_contrato_entregables.Where(Function(p) p.id_contrato_entregable = id_entregable).FirstOrDefault()
            Dim oContrato = dbEntities.vw_tme_contratos.Where(Function(p) p.id_contrato = oEntregable.id_contrato).FirstOrDefault()
            Me.lbl_cod_contrato.Text = oContrato.numero_contrato
            Me.lbl_contratista.Text = oContrato.contratista
            Me.lbl_nro_entregable.Text = oEntregable.numero_entregable
            Me.lbl_fecha_entrega.Text = oEntregable.fecha_esperada_entrega
            Me.lbl_valor.Text = "$" & String.Format("{0:N}", oEntregable.valor_entregable)
            Me.lbl_nom_entregable.Text = oEntregable.nombre_entregable
            Me.lbl_num_producto.Text = oEntregable.productos
            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))

            Dim ruta = dbEntities.tme_ruta_aprobacion_entregable.Where(Function(p) p.id_entregable = id_entregable).ToList().LastOrDefault()


            If oEntregable.url IsNot Nothing Then
                Me.docs_admon.Visible = True
                Me.txt_url_docs_admin.Text = oEntregable.url
                Me.docs_admon.NavigateUrl = oEntregable.url
            Else
                Me.docs_admon.Visible = False
            End If
            If (oEntregable.id_estado_aprobacion_entregable = 1 Or oEntregable.id_estado_aprobacion_entregable = 4) And oEntregable.tme_contratos.id_supervisor = idUser Then
                Me.grd_ruta.Visible = False
                Me.comentarios.Visible = False
                Me.btn_ajustes.Visible = False
                Me.btn_no_aprobar.Visible = False
                Me.btn_aprueba.Text = "Guardar y finalizar"
                Me.addComentario.Visible = True
                Me.soporteURL.Visible = True

            ElseIf oEntregable.id_estado_aprobacion_entregable = 2 Then
                Me.soporteURL.Visible = False
                Me.docs_admon.NavigateUrl = oEntregable.url
                If oEntregable.tme_contratos.id_supervisor = idUser Then
                    Me.addComentario.Visible = True
                Else
                    Me.addComentario.Visible = False
                    Me.btn_ajustes.Visible = False
                    Me.btn_aprueba.Visible = False
                End If
            ElseIf oEntregable.id_estado_aprobacion_entregable = 3 Then
                Me.addComentario.Visible = False
                Me.btn_ajustes.Visible = False
                Me.btn_aprueba.Visible = False
            Else
                Me.addComentario.Visible = False
                Me.btn_ajustes.Visible = False
                Me.btn_aprueba.Visible = False
            End If



        End Using
    End Sub

    Sub fillGrid()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_entregable = Convert.ToInt32(Me.idEntregable.Value)
            Me.grd_ruta.DataSource = dbEntities.vw_ruta_aprobacion_entregable.Where(Function(p) p.id_contrato_entregable = id_entregable).ToList()
            Me.grd_ruta.DataBind()
        End Using
    End Sub

    Sub fillGridComentarios()
        Using dbEntities As New dbRMS_JIEntities
            Dim id_entregable = Convert.ToInt32(Me.idEntregable.Value)
            Dim comentariosList = dbEntities.vw_ruta_aprobacion_entregable.Where(Function(p) p.id_contrato_entregable = id_entregable And p.comentarios IsNot Nothing).ToList()
            If comentariosList.Count() > 0 Then
                Me.comentarios.Visible = True
            End If
            Me.grd_coment.DataSource = dbEntities.vw_ruta_aprobacion_entregable.Where(Function(p) p.id_contrato_entregable = id_entregable And p.comentarios IsNot Nothing).ToList()
            Me.grd_coment.DataBind()
        End Using
    End Sub

    Protected Sub btn_aprueba_pro_Click(sender As Object, e As EventArgs) Handles btn_aprueba.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim sopExterno As Boolean = False
            Dim existeSoporte As Boolean = False
            Dim soporte As String = ""
            Dim comentarios As String = ""
            'Dim idRutaEntre = Convert.ToInt32(Me.id_ruta_hito.Value)
            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            comentarios = Me.txtcoments.Text

            Dim id_entregable = Convert.ToInt32(Me.idEntregable.Value)
            Dim oEntregable = dbEntities.tme_contrato_entregables.Where(Function(p) p.id_contrato_entregable = id_entregable).FirstOrDefault()
            Dim strEmail = ""
            Dim id_notificacion = 1024


            If oEntregable.id_estado_aprobacion_entregable = 1 Then

                Dim oEntregableRuta = New tme_ruta_aprobacion_entregable
                oEntregableRuta.id_entregable = id_entregable
                oEntregableRuta.id_usuario_ruta = idUser
                oEntregableRuta.comentarios = Me.txtcoments.Text
                oEntregableRuta.id_estado_ruta_entregable = 4
                oEntregableRuta.fecha_crea = DateTime.Now
                oEntregableRuta.fecha_envio = DateTime.Now
                oEntregableRuta.id_responsable_aprueba_entregable = 2

                oEntregable.id_estado_aprobacion_entregable = 3
                oEntregable.fecha_entrega = DateTime.Now
                oEntregable.url = Me.txt_url_docs_admin.Text
                dbEntities.Entry(oEntregable).State = Entity.EntityState.Modified
                dbEntities.tme_ruta_aprobacion_entregable.Add(oEntregableRuta)
                dbEntities.SaveChanges()

                'oEntregableRuta = New tme_ruta_aprobacion_entregable
                'oEntregableRuta.id_entregable = id_entregable
                'oEntregableRuta.id_usuario_ruta = oEntregable.tme_contratos.id_supervisor
                'oEntregableRuta.id_estado_ruta_entregable = 1
                'oEntregableRuta.fecha_crea = DateTime.Now
                'oEntregableRuta.id_responsable_aprueba_entregable = 2

                'dbEntities.tme_ruta_aprobacion_entregable.Add(oEntregableRuta)
                'dbEntities.SaveChanges()

                strEmail = oEntregable.tme_contratos.numero_contrato & " Entregable  " & oEntregable.numero_entregable & " aprobado "
            ElseIf oEntregable.id_estado_aprobacion_entregable = 2 Then
                oEntregable.id_estado_aprobacion_entregable = 3
                oEntregable.fecha_aprobacion = DateTime.Now
                dbEntities.Entry(oEntregable).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

                Dim ruta = dbEntities.tme_ruta_aprobacion_entregable.Where(Function(p) p.id_entregable = id_entregable).ToList().LastOrDefault()
                ruta.comentarios = Me.txtcoments.Text
                ruta.fecha_envio = DateTime.Now
                ruta.id_estado_ruta_entregable = 4

                dbEntities.Entry(ruta).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

                strEmail = oEntregable.tme_contratos.numero_contrato & " Entregable " & oEntregable.numero_entregable & " aprobado"
            ElseIf oEntregable.id_estado_aprobacion_entregable = 4 Then
                Dim ruta = dbEntities.tme_ruta_aprobacion_entregable.Where(Function(p) p.id_entregable = id_entregable).ToList().LastOrDefault()
                ruta.comentarios = Me.txtcoments.Text
                ruta.fecha_envio = DateTime.Now
                ruta.id_estado_ruta_entregable = 2

                dbEntities.Entry(ruta).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()

                Dim oEntregableRuta = New tme_ruta_aprobacion_entregable
                oEntregableRuta.id_entregable = id_entregable
                oEntregableRuta.id_usuario_ruta = oEntregable.tme_contratos.id_supervisor
                oEntregableRuta.id_estado_ruta_entregable = 1
                oEntregableRuta.fecha_crea = DateTime.Now
                oEntregableRuta.id_responsable_aprueba_entregable = 2

                dbEntities.tme_ruta_aprobacion_entregable.Add(oEntregableRuta)
                dbEntities.SaveChanges()


                oEntregable.id_estado_aprobacion_entregable = 2
                oEntregable.url = Me.txt_url_docs_admin.Text
                dbEntities.Entry(oEntregable).State = Entity.EntityState.Modified
                dbEntities.SaveChanges()
                strEmail = oEntregable.tme_contratos.numero_contrato & " Entregable " & oEntregable.numero_entregable & " ajustes realizados"
            End If



            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), 0, id_notificacion, cl_user.regionalizacionCulture, 0)
            If (objEmail.Emailing_PRODUCTS_ACTIONS(oEntregable.id_contrato_entregable, strEmail, "aarismendy@justiciainclusiva.org", "lruiz@justiciainclusiva.org")) Then
            Else 'Error mandando Email
            End If


        End Using
        'apruebaProducto(idRutaEntre, comentarios, soporte, True, True)
        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
        Me.MsgGuardar.Redireccion = "frm_contratos_consultores"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

    End Sub

    Protected Sub btn_ajustes_Click(sender As Object, e As EventArgs) Handles btn_ajustes.Click
        Using dbEntities As New dbRMS_JIEntities
            Dim sopExterno As Boolean = False
            Dim existeSoporte As Boolean = False
            Dim soporte As String = ""
            Dim comentarios As String = ""
            'Dim idRutaEntre = Convert.ToInt32(Me.id_ruta_hito.Value)
            Dim idUser As Integer = Convert.ToInt32(Me.Session("E_IDUser"))
            comentarios = Me.txtcoments.Text
            Dim id_notificacion = 1024

            Dim id_entregable = Convert.ToInt32(Me.idEntregable.Value)
            Dim oEntregable = dbEntities.tme_contrato_entregables.Where(Function(p) p.id_contrato_entregable = id_entregable).FirstOrDefault()
            Dim ruta = dbEntities.tme_ruta_aprobacion_entregable.Where(Function(p) p.id_entregable = id_entregable).ToList().LastOrDefault()

            oEntregable.id_estado_aprobacion_entregable = 4
            oEntregable.fecha_aprobacion = DateTime.Now
            dbEntities.Entry(oEntregable).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()

            ruta.comentarios = Me.txtcoments.Text
            ruta.fecha_envio = DateTime.Now
            ruta.id_estado_ruta_entregable = 3

            dbEntities.Entry(ruta).State = Entity.EntityState.Modified
            dbEntities.SaveChanges()


            Dim oEntregableRuta = New tme_ruta_aprobacion_entregable
            oEntregableRuta.id_entregable = id_entregable
            oEntregableRuta.id_usuario_ruta = oEntregable.tme_contratos.id_contratista
            oEntregableRuta.id_estado_ruta_entregable = 1
            oEntregableRuta.fecha_crea = DateTime.Now
            oEntregableRuta.id_responsable_aprueba_entregable = 1

            dbEntities.tme_ruta_aprobacion_entregable.Add(oEntregableRuta)
            dbEntities.SaveChanges()

            Dim strEmail = oEntregable.tme_contratos.numero_contrato & " Entregable " & oEntregable.numero_entregable & " solicitud de ajustes"
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "frm_contratos_consultores"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)

        End Using

    End Sub


End Class