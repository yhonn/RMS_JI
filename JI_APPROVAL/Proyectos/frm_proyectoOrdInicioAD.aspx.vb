Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Net.Mail
Imports System.Net
Imports ly_SIME
Imports Telerik.Web.UI

Public Class frm_proyectoOrdInicioAD
    Inherits System.Web.UI.Page
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "EDIT_ORDIN"
    Dim controles As New ly_SIME.CORE.cls_controles

    Function enviar_email() As String
        Dim fecha As DateTime = Date.Now
        Dim textClave = fecha.ToString.Substring(0, 10).Replace("/", "").Replace(" ", "")

        Dim sql = "SELECT * FROM t_config_email"
        Dim dm As New SqlDataAdapter(sql, cnnME)
        Dim ds As New DataSet("email")
        dm.Fill(ds, "email")

        sql = "SELECT TOP (1) * FROM vw_tme_UsuariosEjecutor WHERE id_nivel_usuario=11 AND id_ejecutor=" & Me.lbl_id_ejecutor.Text
        Dim dmEje As New SqlDataAdapter(sql, cnnME)
        Dim dsEje As New DataSet("EmailEjecutor")
        dmEje.Fill(dsEje, "EmailEjecutor")

        '*************** PARAMETROS DE CONFIGURACION PARA ENVIAR EL EMAIL ***************
        Dim destinatarioAdmin = ds.Tables("email").Rows(0).Item("email_admin").ToString 'DEBE SER A QUIEN INTERESE LA INFORMACION CONTENIDA EN LA FICHA
        Dim port = ds.Tables("email").Rows(0).Item("puerto_SMTP").ToString
        Dim account = ds.Tables("email").Rows(0).Item("email").ToString
        Dim pass = ds.Tables("email").Rows(0).Item("password").ToString
        Dim smtp = ds.Tables("email").Rows(0).Item("SMTP").ToString

        Dim SendFrom As New MailAddress(ds.Tables("email").Rows(0).Item("email_noreplay").ToString, "Sistema ME-Monitoreo y Evaluación CHEMONICS")
        Dim SendTo As New MailAddress(ds.Tables("email").Rows(0).Item("BCC").ToString)
        Dim MensajeSend As New MailMessage(SendFrom, SendTo)

        MensajeSend.To.Add(dsEje.Tables("EmailEjecutor").Rows(0).Item("email").ToString)

        'sql = "SELECT email FROM vw_empleados WHERE (id_proyecto IN (" & Me.lbl_id_proyecto_padre.Text & ", " & Me.lbl_id_proyecto.Text & ")) AND (email IS NOT NULL OR email <> '') AND (edita_registros = 'SI')"
        sql = "SELECT email FROM vw_empleados WHERE (id_proyecto IN (" & Me.lbl_id_proyecto.Text & ")) AND (email IS NOT NULL OR email <> '') AND (edita_registros = 'SI')"
        Dim dmME As New SqlDataAdapter(sql, cnnME)
        Dim dsME As New DataSet("EmailME")
        dmME.Fill(dsME, "EmailME")
        For jME As Integer = 0 To dsME.Tables("EmailME").Rows.Count - 1
            MensajeSend.To.Add(dsME.Tables("EmailME").Rows(jME).Item("email").ToString)
        Next

        Dim emails_cco() As String = ds.Tables("email").Rows(0).Item("BCO").ToString.Split(";")
        For i As Integer = 0 To emails_cco.Count() - 1
            If emails_cco(i).ToString <> "" Then
                MensajeSend.Bcc.Add(emails_cco(i).ToString)
            End If
        Next

        sql = "UPDATE tme_UsuariosEjecutor SET clave='" & textClave & "' WHERE id_usuario_ejecutor=" & dsEje.Tables("EmailEjecutor").Rows(0).Item("id_usuario_ejecutor").ToString
        Dim dmUser As New SqlCommand(sql, cnnME)
        dmUser.ExecuteNonQuery()


        MensajeSend.Subject = "Registro de Nuevo Proyecto"
        Dim Mensaje As String = "<html><body><table cellpadding='0' style='color: rgb(62, 62, 45); font-family: Helvetica;font-size: 11px; font-size: 9px; font-style: normal; font-variant: normal; font-weight: normal; "
        Mensaje &= "letter-spacing: normal; line-height: normal; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-size-adjust: auto; -webkit-text-stroke-width: 0px; "
        Mensaje &= "background-color: rgb(255, 255, 255); width: 680px; border-collapse: collapse; border: 1px solid rgb(154, 115, 40); '>"
        Mensaje &= "<tr> <td style='font-family: Helvetica;font-size: 12px;  font-weight: bold; text-align: left; background-color: rgb(194, 184, 139); '>"
        Mensaje &= "<br>&nbsp;&nbsp;&nbsp;Ficha de registro de proyecto<hr></td>"
        Mensaje &= "</tr><tr><td style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "<table border='0' style='width: 680px; '>"

        Mensaje &= "<tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>&nbsp;</td>"
        Mensaje &= "<td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "<br>Código de proyecto: <b>" & Me.lbl_codigo_ficha.Text & "</b></td></tr>"

        Mensaje &= "<tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "Nombre de proyecto: <b>" & Me.lbl_nombre_ficha.Text.Trim & "</b></td></tr>"

        Mensaje &= "<tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "Región: <b>" & Me.lbl_nombre_subregion.Text & "</b></td></tr>"

        Mensaje &= "<tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        'Mensaje &= "Componente: <b>" & Me.lbl_nombre_componente.Text & "</b></td></tr>"

        Mensaje &= "<tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "Ejecutor: <b>" & Me.lbl_nombre_ejecutor.Text & "</b></td></tr>"

        Mensaje &= "<tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>&nbsp;</td></tr><tr>"
        Mensaje &= "<td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "Para ingresar al sistema de Monitoreo y Evaluación se ha creado automáticamente el siguiente acceso</td>"
        Mensaje &= "</tr><tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "Usuario: <b>" & dsEje.Tables("EmailEjecutor").Rows(0).Item("usuario").ToString & "</b></td></tr><tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "Contraseña: <b>" & textClave.Trim & "</b></td></tr><tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "URL: <a href='http://www.colombiaresponde-ns.org/SIME'>www.colombiaresponde-ns.org/SIME</a></td></tr><tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "&nbsp;</td></tr><tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "&nbsp;</td></tr><tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "<img border='0' width=63 height=75 src='http://www.colombiaresponde-ns.org/SAP/Imagenes/image002.jpg' /></td></tr><tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 10px; text-align: left; '><b>Mensaje automático. Favor no responder.<span class='Apple-converted-space'>&nbsp;</span><br>Unidad de Administración Sistema SAP-Monitoreo y Evaluación.<br>"
        Mensaje &= "La información contenida en este correo electrónico es confidencial."
        Mensaje &= "</b></td></tr><tr><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; width: 27px; '>"
        Mensaje &= "&nbsp;</td><td align='right' style='font-family: Helvetica;font-size: 11px; text-align: left; '>"
        Mensaje &= "&nbsp;</td></tr></table></td><tr></table></body></html>"

        MensajeSend.IsBodyHtml = True
        MensajeSend.Body = Mensaje
        MensajeSend.Priority = MailPriority.High
        Dim CorreoSend As New SmtpClient(smtp, port)
        CorreoSend.Credentials = New NetworkCredential(account, pass)
        CorreoSend.EnableSsl = False
        Mensaje &= ""
        Try
            CorreoSend.Send(MensajeSend)
        Catch ex As Exception
            'MsgBox("ERROR: " & ex.ToString, MsgBoxStyle.Critical, "Error")
        End Try
        Return "Usuario: " & dsEje.Tables("EmailEjecutor").Rows(0).Item("usuario").ToString & "<br>Clave: " & textClave

    End Function

    Sub CargarDatos(ByVal IdProyecto As String)
        If IdProyecto = "" Then
            IdProyecto = "-1"
        End If
        Dim dm As New SqlDataAdapter("SELECT * FROM vw_tme_Ficha_Proyecto WHERE id_ficha_proyecto=" & IdProyecto, cnnME)
        Dim ds As New DataSet("DsFichaProyecto")
        dm.Fill(ds, "DsFichaProyecto")

        Me.lbl_id_componente.Text = ds.Tables("DsFichaProyecto")(0)("id_componente")
        'Me.lbl_id_proyecto.Text = ds.Tables("DsFichaProyecto")(0)("id_ficha_proyecto")
        'Me.lbl_id_ejecutor.Text = ds.Tables("DsFichaProyecto")(0)("id_ejecutor")

        'Me.lbl_id_proyecto.Text = ds.Tables("DsFichaProyecto")(0)("id_proyecto")
        'Me.lbl_id_proyecto_padre.Text = ds.Tables("DsFichaProyecto")(0)("id_proyecto_padre")

        Me.lbl_nombre_ficha.Text = ds.Tables("DsFichaProyecto")(0)("nombre_proyecto")
        'Me.lbl_region.Text = ds.Tables("DsFichaProyecto")(0)("Region")
        'Me.lbl_ejecutor.Text = ds.Tables("DsFichaProyecto")(0)("nombre_ejecutor")
        'Me.lbl_componente.Text = ds.Tables("DsFichaProyecto")(0)("nombre_componente")
        'Me.lbl_codigoAID.Text = ds.Tables("DsFichaProyecto")(0)("codigo_ficha_AID")
        'Me.lbl_codigoSAPME.Text = ds.Tables("DsFichaProyecto")(0)("codigo_SAPME")

        Dim Sql As String = "SELECT ROW_NUMBER() OVER(ORDER BY id_InstrumentoIndicador ASC) as Number, definicion_indicador, nombre_tipo_umedida, nombre_instrumento,id_instrumento FROM vw_tme_Instrumento_Componente_Indicador"
        Sql &= " WHERE id_indicador IN( SELECT id_indicador FROM tme_meta_indicador_ficha WHERE id_ficha_proyecto= " & IdProyecto & ")"

        Me.SqlDataSource11.SelectCommand = Sql
        Sql = "SELECT ROW_NUMBER() OVER(ORDER BY id_indicador ASC) as Number, id_indicador, definicion_indicador, nombre_tipo_umedida FROM vw_tme_ficha_meta_indicador WHERE id_ficha_proyecto= " & IdProyecto
        Me.SqlDataSource12.SelectCommand = Sql

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
        End If

        If Not IsPostBack Then
            LoadData()
            Me.dt_fecha_inicio.SelectedDate = Date.Now()
            'Dim dm As New SqlDataAdapter("SELECT empleado_nombre + ' ' + apellidos AS nombre, codigo_empleado FROM t_empleados WHERE id_empleado=" & Me.Session("E_IdUser"), cnnME)
            'Dim ds As New DataSet("DsUser")
            'dm.Fill(ds, "DsUser")
            Me.lbl_usuario.Text = Me.Session("E_Nombre")
            'Me.MsgGuarda.Visible = False
            CargarDatos(Me.Request.QueryString("Id").ToString)

            Dim rnd As New Random()
            Dim fecha As DateTime = Date.Now
            Dim textfecha = fecha.ToString.Replace("/", "").Replace(" ", "").Replace("a", "").Replace(".", "").Replace("m", "").Replace(":", "").Replace(";", "").Replace("p", "")
            Me.lbl_id_codigo.Text = rnd.Next(1, 999).ToString & textfecha.ToString
        End If
    End Sub

    Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_guardar.Click
        'Me.lbl_Error.Visible = False

        Dim sql As String = ""
        Try
            Using dbEntities As New dbRMS_HNEntities
                Dim idProyecto = Convert.ToInt32(Me.lbl_id_proyecto.Text)
                Dim idUsuario = Convert.ToInt32(Me.Session("E_IdUser").ToString)
                Dim oFichaOrdenInicio As New tme_FichaProyectoOrdenInicio
                oFichaOrdenInicio.id_ficha_proyecto = idProyecto
                oFichaOrdenInicio.fecha_ordInicio = Me.dt_fecha_inicio.SelectedDate
                oFichaOrdenInicio.id_usuario_creo = idUsuario
                oFichaOrdenInicio.datecreated = Date.UtcNow

                dbEntities.tme_FichaProyectoOrdenInicio.Add(oFichaOrdenInicio)

                dbEntities.SaveChanges()

                Dim oFichaProyectoMenu = dbEntities.tme_FichaProyectoMenu.Where(Function(p) p.id_ficha_proyecto = idProyecto).ToList()
                For Each row In Me.grd_Instrumentos1.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim id_instrumento As Integer = Convert.ToInt32(dataItem.GetDataKeyValue("id_instrumento"))

                        If oFichaProyectoMenu.Where(Function(p) p.id_instrumento = id_instrumento).Count() = 0 Then
                            dbEntities.Database.ExecuteSqlCommand("INSERT INTO tme_FichaProyectoMenu(id_ficha_proyecto, id_instrumento) VALUES(" & idProyecto & "," & id_instrumento & ")")
                        End If
                    End If
                Next
                For Each row In Me.grd_Instrumentos2.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim id_indicador As Integer = Convert.ToInt32(dataItem.GetDataKeyValue("id_indicador"))
                        Dim ChkHerramientas As CheckBox = CType(row.Cells(0).FindControl("ChkHerramientas"), CheckBox)
                        If ChkHerramientas.Checked = True Then
                            dbEntities.Database.ExecuteSqlCommand("INSERT INTO tme_FichaProyectoOrdenInicioIndicador(id_ficha_proyecto_ordInicio, id_indicador) VALUES(" & oFichaOrdenInicio.id_ficha_proyecto_ordInicio & "," & id_indicador & ")")
                        End If

                        Dim chkActivo As CheckBox = CType(row.Cells(0).FindControl("chkActivo"), CheckBox)
                        If chkActivo.Checked = True Then
                            dbEntities.Database.ExecuteSqlCommand("UPDATE tme_meta_indicador_ficha SET requiere_aprobacion='SI' WHERE id_indicador=" & id_indicador & " AND id_ficha_proyecto=" & idProyecto)
                        End If
                    End If
                Next
                For Each row In Me.grd_Instrumentos3.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim id_instrumento As Integer = Convert.ToInt32(dataItem.GetDataKeyValue("id_instrumento"))
                        Dim ChkHRSeguimiento As CheckBox = CType(row.Cells(0).FindControl("ChkHRSeguimiento"), CheckBox)
                        If ChkHRSeguimiento.Checked = True Then
                            dbEntities.Database.ExecuteSqlCommand("INSERT INTO tme_FichaProyectoMenu(id_ficha_proyecto, id_instrumento) VALUES(" & idProyecto & "," & id_instrumento & ")")
                        End If
                    End If
                Next
                For Each row In Me.grd_Instrumentos4.Items
                    If TypeOf row Is GridDataItem Then
                        Dim dataItem As GridDataItem = CType(row, GridDataItem)
                        Dim id_instrumento As Integer = Convert.ToInt32(dataItem.GetDataKeyValue("id_instrumento"))
                        dbEntities.Database.ExecuteSqlCommand("INSERT INTO tme_FichaProyectoMenu(id_ficha_proyecto, id_instrumento) VALUES(" & idProyecto & "," & id_instrumento & ")")
                    End If
                Next
                dbEntities.Database.ExecuteSqlCommand("UPDATE tme_Ficha_Proyecto SET id_ficha_estado=2 WHERE id_ficha_proyecto =" & idProyecto)
                dbEntities.Database.ExecuteSqlCommand("UPDATE tme_FichaProyectoOrdenInicio SET codigo_ficha_ordInicio='AI-" & Me.lbl_codigo_ficha.Text & "' WHERE id_ficha_proyecto_ordInicio=" & oFichaOrdenInicio.id_ficha_proyecto_ordInicio)

            End Using
            Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectosCuadroMando"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        Catch ex As Exception
            Me.MsgGuardar.NuevoMensaje = "Error"
            Me.MsgGuardar.Redireccion = "~/Proyectos/frm_proyectosCuadroMando"
            ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
        End Try
        'Me.lbl_usuarioyclave.Text = enviar_email() & "<br/> Datos enviados por email."
        ' Me.Response.Redirect("~/proyectos/frm_proyectoOrdInicio.aspx")

    End Sub

    Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
        Me.MsgReturn.Redireccion = "~/proyectos/frm_proyectosCuadroMando"
        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
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
            'Me.lbl_nombre_subregion.Text = oProyecto.nombre_subregion
        End Using
    End Sub

End Class