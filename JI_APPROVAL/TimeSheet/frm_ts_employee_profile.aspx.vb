Imports CuteWebUI
Imports System.Data.SqlClient
Imports ly_SIME
Imports ly_APPROVAL
Imports ly_RMS
Imports Telerik.Web.UI
Imports System.Globalization
Imports System.Configuration.ConfigurationManager

Public Class frm_ts_employee_profile
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim clListado As New ly_SIME.CORE.cls_listados
    Dim frmCODE As String = "TS_PROF_EMPLOYEE"
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim cl_files As New ly_SIME.CORE.cls_files

    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\Usuarios\"
    Dim sFileDirTemp As String = Server.MapPath("~") & "\Temp\"
    Dim sFileDirUbicacion As String = "~/FileUploads/Usuarios/"
    Dim extension, File As String
    Public str_TABLE As String = ""
    Public str_TABLE_CAS As String = ""
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim totalC As Integer()

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
            Me.lbl_id_usuario.Text = Convert.ToInt32(Me.Session("E_IdUser"))
            'Me.Request.QueryString("Id").ToString
            FillData(Me.lbl_id_usuario.Text)
        End If



    End Sub

    'Protected Sub btn_guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_guardar.Click

    '    Dim Sql As String = ""
    '    Dim err As Boolean = True
    '    Dim errSave As Boolean = False


    '    'If Val(Me.cmb_job_title.SelectedValue) = 0 Then
    '    '    Me.lbl_JobErr.Visible = True
    '    '    errSave = True
    '    'Else
    '    '    Me.lbl_JobErr.Visible = False
    '    '    errSave = False
    '    'End If


    '    If chk_SupervisorNA.Checked = False Then
    '        If Val(Me.cmb_supervisor.SelectedValue) = 0 Then
    '            Me.lbl_supervisorErr.Visible = True
    '            errSave = True
    '        Else
    '            Me.lbl_supervisorErr.Visible = False
    '            errSave = False
    '        End If
    '    End If

    '    Dim fileName = Me.lbl_oldFile.Value

    '    If lbl_hasFiles.Value = "true" Then
    '        fileName = Me.lbl_archivo_uploaded.Value
    '    End If

    '    Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma").ToString())

    '    'If chk_PasswordChange.Checked Then
    '    '    Me.pass_validator.Validate()
    '    '    If Not Me.pass_validator.IsValid Then
    '    '        errSave = True
    '    '    End If
    '    'End If

    '    If errSave = False Then

    '        Using dbEntities As New dbRMS_JIEntities

    '            Dim oUsuario As New t_usuarios
    '            Dim query = ""

    '            Dim parameters() As SqlParameter = New SqlParameter() _
    '                    {
    '                        New SqlParameter("@nombre_usuario", SqlDbType.VarChar) With {.Value = Me.txt_nombreUsuario.Text},
    '                        New SqlParameter("@apellidos_usuario", SqlDbType.VarChar) With {.Value = Me.txt_apellidos.Text},
    '                        New SqlParameter("@email_usuario", SqlDbType.VarChar) With {.Value = Me.txt_email_usuario.Text},
    '                        New SqlParameter("@id_job_title", SqlDbType.Int) With {.Value = Me.cmb_job_title.SelectedValue},
    '                        New SqlParameter("@fecha_contrato", SqlDbType.Date) With {.Value = Me.dt_fecha_contrato.SelectedDate},
    '                        New SqlParameter("@id_usuario_upd", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.Session("E_IdUser").ToString())},
    '                        New SqlParameter("@id_usuario_padre", SqlDbType.Int) With {.Value = If(Not Me.chk_SupervisorNA.Checked, Convert.ToInt32(Me.cmb_supervisor.SelectedValue), 0)},
    '                        New SqlParameter("@upddate", SqlDbType.DateTime) With {.Value = Date.UtcNow},
    '                        New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = Me.lbl_id_usuario.Text}
    '                    }

    '            'New SqlParameter("@imagen", SqlDbType.VarChar) With {.Value = fileName},

    '            query = "UPDATE t_usuarios SET nombre_usuario = @nombre_usuario, apellidos_usuario = @apellidos_usuario, email_usuario = @email_usuario, " &
    '                        "id_job_title = @id_job_title, " &
    '                        "fecha_contrato = @fecha_contrato, id_usuario_upd = @id_usuario_upd, id_usuario_padre = @id_usuario_padre, upddate = @upddate" &
    '                        " WHERE id_usuario = @id_usuario"
    '            dbEntities.Database.ExecuteSqlCommand(query, parameters)

    '            If lbl_hasFiles.Value = "true" Then

    '                Dim user_route = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).url_user_images
    '                Dim path As String = user_route
    '                path = user_route.Substring(0, path.Trim.Length - 1)
    '                Dim idx1 As Integer = path.LastIndexOf("/") + 1
    '                Dim account As String = path.Substring(idx1, path.Length - idx1)
    '                cl_files.SaveFileCloud(account, Server.MapPath("~") & "\Temp\", Me.lbl_archivo_uploaded.Value)
    '                Dim user_uri As Uri
    '                Uri.TryCreate(user_route & Me.lbl_archivo_uploaded.Value, UriKind.Absolute, user_uri)

    '            End If

    '            Dim parameters3() As SqlParameter = New SqlParameter() _
    '                    {
    '                        New SqlParameter("@id_usuario", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.lbl_id_usuario.Text)},
    '                        New SqlParameter("@id_programa", SqlDbType.Int) With {.Value = id_programa},
    '                        New SqlParameter("@id_job_title", SqlDbType.Int) With {.Value = Convert.ToInt32(Me.cmb_job_title.SelectedValue)}
    '                    }

    '            Dim query3 = "UPDATE t_usuario_programa SET id_job_title = @id_job_title WHERE id_usuario = @id_usuario and id_programa = @id_programa"
    '            dbEntities.Database.ExecuteSqlCommand(query3, parameters3)



    '            Dim id_usuario = CType(Me.lbl_id_usuario.Text, Integer)
    '            Dim id_employee As Integer = If(dbEntities.rh_employee.Where(Function(p) p.id_usuario = id_usuario).Count > 0, dbEntities.rh_employee.Where(Function(p) p.id_usuario = id_usuario).FirstOrDefault.id_rh_employee, 0)
    '            Dim oEmployee As rh_employee = New rh_employee

    '            If id_employee > 0 Then
    '                oEmployee = dbEntities.rh_employee.Find(id_employee)
    '            End If


    '            oEmployee.id = Me.txt_documento.Text
    '            oEmployee.id_rh_office = Me.cmb_office.SelectedValue
    '            oEmployee.id_sex_type = Me.cmb_gender.SelectedValue
    '            oEmployee.contract_date = Me.dt_fecha_contrato.SelectedDate
    '            oEmployee.expired_date = Me.dt_fecha_contrato_vence.SelectedDate
    '            oEmployee.birth_date = Me.dt_birth_day.SelectedDate
    '            oEmployee.notes = Me.txt_note1.Text
    '            oEmployee.note1 = Me.txt_note2.Text
    '            oEmployee.worked_count = Me.txt_contract_days.Value
    '            oEmployee.worked_days = Me.txt_worked_days.Value
    '            oEmployee.salary = Me.txt_salary.Value
    '            oEmployee.address = Me.txt_direccion.Text
    '            oEmployee.telephone = Me.txt_telefono.Text
    '            oEmployee.mobile = Me.txt_mobile.Text
    '            oEmployee.img_employee = fileName
    '            oEmployee.id_usuario = CType(Me.lbl_id_usuario.Text, Integer)

    '            If id_employee = 0 Then
    '                oEmployee.date_created = Date.UtcNow
    '                oEmployee.id_user_created = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '            Else
    '                oEmployee.date_updated = Date.UtcNow
    '                oEmployee.id_user_updated = Convert.ToInt32(Me.Session("E_IdUser").ToString())
    '            End If

    '            If id_employee = 0 Then
    '                dbEntities.rh_employee.Add(oEmployee)
    '            Else
    '                dbEntities.Entry(oEmployee).State = Entity.EntityState.Modified
    '            End If

    '            dbEntities.SaveChanges()

    '            'If chk_Notify.Checked Then
    '            '    '*********************************OPEN****************************************
    '            '    Dim objEmail As New SIMEly.cls_notification(Me.Session("E_IDPrograma"), 7, cl_user.regionalizacionCulture, cl_user.idSys)

    '            '    If (objEmail.Emailing_USER_UPD(Me.lbl_id_usuario.Text, If(chk_PasswordChange.Checked, Me.txt_clave_usuario.Text, ""))) Then
    '            '    Else 'Error mandando Email
    '            '    End If
    '            '    '*********************************OPEN****************************************
    '            'End If

    '        End Using

    '        Me.MsgGuardar.NuevoMensaje = cl_user.controles_otros.FirstOrDefault(Function(p) p.control_code = "GUARDADO").texto
    '        Me.MsgGuardar.Redireccion = "~/HRM/frm_employees"
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "Func()", True)
    '    End If

    'End Sub

    'Protected Sub btn_salir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_salir.Click
    '    Me.MsgReturn.Redireccion = "~/frm_employees"
    '    ScriptManager.RegisterStartupScript(Me.Page, Page.[GetType](), "text", "FuncCancel()", True)
    'End Sub

    Sub FillData(ByVal idUsuario As String)

        Dim id_usuario = Convert.ToInt32(idUsuario)
        Using dbEntities As New dbRMS_JIEntities

            Dim oUsuario = dbEntities.t_usuarios.Find(id_usuario)

            Dim oEmployee = dbEntities.vw_rh_employee.Where(Function(p) p.id_usuario = id_usuario)

            Dim id_employee As Integer = CType(oEmployee.FirstOrDefault.id_rh_employee, Integer)

            Dim id_programa = Convert.ToInt32(Me.Session("E_IDPrograma"))

            Dim user_uri As Uri
            Dim user_route = dbEntities.t_programa_settings.FirstOrDefault(Function(p) p.id_programa = id_programa).url_user_images

            Dim strImagen As String = If(id_employee = 0, oUsuario.imagen, oEmployee.FirstOrDefault.img_employee)

            Uri.TryCreate(user_route & strImagen, UriKind.Absolute, user_uri)
            Me.imgUser.ImageUrl = user_uri.AbsoluteUri.ToString()
            Me.lbl_oldFile.Value = strImagen

            Me.cmb_job_title.DataSource = clListado.get_t_job_title(id_programa)
            Me.cmb_job_title.DataTextField = "job"
            Me.cmb_job_title.DataValueField = "id_job_title"
            Me.cmb_job_title.DataBind()

            Me.cmb_office.DataSource = dbEntities.rh_offices.ToList()
            Me.cmb_office.DataTextField = "off_name"
            Me.cmb_office.DataValueField = "id_rh_office"
            Me.cmb_office.DataBind()

            Me.cmb_gender.DataSource = dbEntities.tme_sex_type.ToList()
            Me.cmb_gender.DataTextField = "sex_type"
            Me.cmb_gender.DataValueField = "id_sex_type"
            Me.cmb_gender.DataBind()

            Me.cmb_supervisor.DataSourceID = ""
            Me.cmb_supervisor.DataSource = clListado.get_t_usuarios()
            Me.cmb_supervisor.DataTextField = "nombre_usuario"
            Me.cmb_supervisor.DataValueField = "id_usuario"
            Me.cmb_supervisor.DataBind()
            If (oUsuario.id_usuario_padre.HasValue) Then
                Me.cmb_supervisor.SelectedValue = oUsuario.id_usuario_padre
            End If

            Me.hd_tp.Value = 1

            If id_employee = 0 Then

                Dim oConfigUsuario = dbEntities.t_usuario_programa.FirstOrDefault(Function(p) p.id_programa = id_programa And p.id_usuario = id_usuario)
                Me.cmb_job_title.SelectedValue = oConfigUsuario.id_job_title 'Watch out this
                Me.cmb_office.SelectedValue = ""
                Me.cmb_gender.SelectedValue = ""

                Me.txt_nombreUsuario.Text = oUsuario.nombre_usuario
                Me.txt_apellidos.Text = oUsuario.apellidos_usuario
                Me.txt_documento.Text = ""
                Me.txt_email_usuario.Text = oUsuario.email_usuario

                Dim fd As Date = Date.Now

                Me.dt_fecha_contrato.SelectedDate = oUsuario.fecha_contrato
                Me.dt_fecha_contrato_vence.SelectedDate = fd.Date
                Me.dt_birth_day.SelectedDate = fd.Date

                Me.txt_telefono.Text = ""
                Me.txt_mobile.Text = ""
                Me.txt_direccion.Text = ""

                Me.txt_salary.Value = 0
                Me.txt_contract_days.Value = 0
                Me.txt_worked_days.Value = 0

                Me.lbl_createdBy.Text = "--"
                Me.lbl_created_date.Text = "--"

                Me.lbl_updatedBy.Text = "--"
                Me.lbl_updated_date.Text = "--"

                'Me.txt_note1.Text = ""
                'Me.txt_note2.Text = ""

            Else

                Me.cmb_job_title.SelectedValue = oEmployee.FirstOrDefault.id_job_title 'Watch out this
                Me.cmb_office.SelectedValue = oEmployee.FirstOrDefault.id_rh_office
                Me.cmb_gender.SelectedValue = oEmployee.FirstOrDefault.id_sex_type

                Me.txt_nombreUsuario.Text = oEmployee.FirstOrDefault.nombre_usuario
                Me.txt_apellidos.Text = oEmployee.FirstOrDefault.apellidos_usuario
                Me.txt_documento.Text = oEmployee.FirstOrDefault.id
                Me.txt_email_usuario.Text = oUsuario.email_usuario

                Me.dt_fecha_contrato.SelectedDate = oEmployee.FirstOrDefault.contract_date
                Me.dt_fecha_contrato_vence.SelectedDate = oEmployee.FirstOrDefault.expired_date
                Me.dt_birth_day.SelectedDate = oEmployee.FirstOrDefault.birth_date

                Me.txt_telefono.Text = oEmployee.FirstOrDefault.telephone
                Me.txt_mobile.Text = oEmployee.FirstOrDefault.mobile
                Me.txt_direccion.Text = oEmployee.FirstOrDefault.address

                Me.txt_salary.Value = oEmployee.FirstOrDefault.salary
                Me.txt_contract_days.Value = oEmployee.FirstOrDefault.worked_count
                Me.txt_worked_days.Value = oEmployee.FirstOrDefault.worked_days

                Me.lbl_createdBy.Text = If(IsNothing(oEmployee.FirstOrDefault.user_created), "--", oEmployee.FirstOrDefault.user_created)
                Me.lbl_created_date.Text = If(oEmployee.FirstOrDefault.date_created.HasValue, getFecha(oEmployee.FirstOrDefault.date_created, "f", True), "--")

                Me.lbl_updatedBy.Text = If(IsNothing(oEmployee.FirstOrDefault.user_updated), "--", oEmployee.FirstOrDefault.user_updated)
                Me.lbl_updated_date.Text = If(oEmployee.FirstOrDefault.date_updated.HasValue, getFecha(oEmployee.FirstOrDefault.date_updated, "f", True), "--")

                'Me.txt_note1.Text = oEmployee.FirstOrDefault.notes
                'Me.txt_note2.Text = oEmployee.FirstOrDefault.note1

            End If


            fill_Grid(True, id_usuario)

            LoadTimeSheets(True, id_usuario)
            'Me.txt_usuarioSistema.Text = oUsuario.usuario
            'Me.txt_clave_usuario.Text = "ThiIsmYPassWoR"
            'Me.txt_clave_usuario.Enabled = False
            'Me.chk_PasswordChange.Checked = False

            fill_table(id_usuario)

            fill_tableCASUAL(id_usuario)


            ' Me.rbn_actividades_habilitadas.SelectedValue = If(oConfigUsuario.busqueda_actividad, 1, 2)


            'Me.lbl_date_upd.Text = If(oUsuario.upddate.HasValue, CDate(oUsuario.upddate).ToShortDateString(), "")
            'Me.lbl_id_usrupdate.Text = If(IsDBNull(oUsuario.id_usuario_upd), "", dbEntities.t_usuarios.FirstOrDefault(Function(p) p.id_usuario_upd = oUsuario.id_usuario_upd).usuario)


            'Me.cmb_tipo_nivel.DataSourceID = ""
            'Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa)
            'Me.cmb_tipo_nivel.DataTextField = "codigo_nivel_usuario"
            'Me.cmb_tipo_nivel.DataValueField = "id_tipo_nivel_usuario"
            'Me.cmb_tipo_nivel.DataBind()
            'Me.cmb_tipo_nivel.SelectedValue = oUsuario.id_tipo_nivel_usuario

            'Me.cmb_tipo_usuario.DataSourceID = ""
            'Me.cmb_tipo_usuario.DataSource = clListado.get_t_tipo_usuario()
            'Me.cmb_tipo_usuario.DataTextField = "nombre_tipo_usuario"
            'Me.cmb_tipo_usuario.DataValueField = "id_tipo_usuario"
            'Me.cmb_tipo_usuario.DataBind()
            'Me.cmb_tipo_usuario.SelectedValue = oUsuario.id_tipo_usuario

            'Me.cmb_ejecutor.DataSourceID = ""
            'Me.cmb_ejecutor.DataSource = clListado.get_t_ejecutores(id_programa)
            'Me.cmb_ejecutor.DataTextField = "nombre_ejecutor"
            'Me.cmb_ejecutor.DataValueField = "id_ejecutor"
            'Me.cmb_ejecutor.DataBind()

            'Me.rb_estado.DataSource = clListado.get_t_estado_usuario()
            'Me.rb_estado.DataTextField = "nombre_estado_usuario"
            'Me.rb_estado.DataValueField = "id_estado_usuario"
            'Me.rb_estado.RepeatColumns = 2
            'Me.rb_estado.DataBind()
            'Me.rb_estado.SelectedValue = oUsuario.id_estado_usr

            'Me.cmb_idioma.DataSource = dbEntities.vw_t_programa_idiomas.Where(Function(p) p.id_programa = id_programa).ToList()
            'Me.cmb_idioma.DataTextField = "descripcion_idioma"
            'Me.cmb_idioma.DataValueField = "id_idioma"
            'Me.cmb_idioma.DataBind()
            'Me.cmb_idioma.SelectedValue = oConfigUsuario.id_idioma


            'Dim oUsuarioActual = dbEntities.t_usuarios.Find(cl_user.id_usr)
            'Dim nivel = oUsuarioActual.t_tipo_nivel_usuario
            'Dim t_levels_user = dbEntities.t_tipo_nivel_usuario.Where(Function(P) P.id_programa = id_programa).ToList()

            'If nivel.codigo_nivel_usuario = "SYS_USER" Then

            'Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa, "SYS_USER")
            'Me.cmb_tipo_nivel.DataTextField = "codigo_nivel_usuario"
            'Me.cmb_tipo_nivel.DataValueField = "id_tipo_nivel_usuario"
            'Me.cmb_tipo_nivel.DataBind()

            'Me.cmb_tipo_nivel.SelectedValue = oUsuario.id_tipo_nivel_usuario
            'Me.cmb_tipo_nivel.Enabled = False
            'Me.div_nivel_usuario.Visible = False

            'If oUsuario.t_tipo_nivel_usuario.codigo_nivel_usuario = "SYS_ADMIN" Or oUsuario.t_tipo_nivel_usuario.codigo_nivel_usuario = "SYS_P_ADM" Then
            '    Me.btn_guardar.Enabled = False
            '    Me.lbl_mensajeError.Text = "Access denied"
            'End If

            ' Else

            'If nivel.codigo_nivel_usuario = "SYS_P_ADM" Then
            '    Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa, "SYS_USER,SYS_P_ADM")
            '    If oUsuario.t_tipo_nivel_usuario.codigo_nivel_usuario = "SYS_ADMIN" Then
            '        Me.btn_guardar.Enabled = False
            '        Me.lbl_mensajeError.Text = "Access denied"
            '    End If
            'Else
            '    Me.cmb_tipo_nivel.DataSource = clListado.get_t_tipo_nivel_usuario(id_programa, "SYS_USER,SYS_P_ADM,SYS_ADMIN")
            'End If

            'Me.cmb_tipo_nivel.DataTextField = "codigo_nivel_usuario"
            'Me.cmb_tipo_nivel.DataValueField = "id_tipo_nivel_usuario"
            'Me.cmb_tipo_nivel.DataBind()
            'Me.cmb_tipo_nivel.SelectedValue = oUsuario.id_tipo_nivel_usuario
            'Me.div_nivel_usuario.Visible = True
            'Me.cmb_tipo_nivel.Enabled = True

            ' End If

            'If oUsuario.id_tipo_usuario = 2 Then
            '    Me.divEjecutor.Visible = True
            '    Me.cmb_ejecutor.SelectedValue = dbEntities.t_ejecutor_usuario.FirstOrDefault(Function(p) p.id_usuario = oUsuario.id_usuario).id_ejecutor
            'End If

            'Me.grd_ejecutores.DataSource = ""
            'Me.grd_ejecutores.DataSource = dbEntities.t_ejecutores.Where(Function(p) p.id_programa = id_programa) _
            '    .Select(Function(p) _
            '                New With {Key .nombre_ejecutor = p.nombre_ejecutor, _
            '                          Key .id_ejecutor = p.id_ejecutor}).ToList()
            'Me.grd_ejecutores.DataBind()

            'For Each item In dbEntities.t_ejecutor_usuario.Where(Function(p) p.id_usuario = id_usuario)
            '    For Each row In Me.grd_ejecutores.Items
            '        If TypeOf row Is GridDataItem Then
            '            Dim dataItem As GridDataItem = CType(row, GridDataItem)
            '            Dim EjeS As CheckBox = CType(row.Cells(0).FindControl("ctrl_id_ejecutor"), CheckBox)
            '            Dim id_ejecutor As Integer = dataItem.GetDataKeyValue("id_ejecutor")
            '            If item.id_ejecutor = id_ejecutor Then
            '                EjeS.Checked = True
            '            End If
            '        End If
            '    Next
            'Next



        End Using
    End Sub


    'Public Sub fill_table(ByVal idUser As Integer)

    '    Dim ds As New DataSet
    '    Dim adapter As SqlDataAdapter

    '    cnn.Open()

    '    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("SP_HR_BILLABLE_TIME", cnn)
    '    cmd.CommandType = CommandType.StoredProcedure
    '    'cmd.Parameters.Add("@tp_view", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
    '    cmd.Parameters.AddWithValue("@tp_view", 4)
    '    cmd.Parameters.AddWithValue("@_id_usuario", idUser)
    '    cmd.Parameters.AddWithValue("@_anio", 0)

    '    adapter = New SqlDataAdapter(cmd)
    '    adapter.Fill(ds)

    '    If Not IsNothing(ds.Tables(0)) Then



    '        ' grd_cate.DataSource = ds.Tables(0)
    '        'Dim str_TABLE As String = ""
    '        Dim str_tbl_OPEN As String = "<table class='table table-striped'>"
    '        Dim str_tbl_CLOSE As String = "</table>"

    '        Dim str_tbl_HEADER As String = "<tr>"
    '        Dim str_tbl_row1 As String = "<tr>"
    '        Dim str_tbl_row2 As String = "<tr><td colspan='{0}'>{1}</td></tr>"

    '        Dim cols As Integer = 0
    '        Dim colsTot As Integer = ds.Tables(0).Columns.Count - 1

    '        str_TABLE &= str_tbl_OPEN

    '        For j = 0 To ds.Tables(0).Columns.Count - 1

    '            If j = 4 Then

    '                str_tbl_HEADER &= String.Format("<th>{0}</th>", "YEAR")
    '                cols += 1

    '            ElseIf j = 6 Then

    '                str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper) ' ds.Tables(0).Columns(j).ColumnName.ToString
    '                cols += 1

    '            ElseIf j = 7 Then

    '                str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
    '                cols += 1

    '            ElseIf j > 7 And j <= (colsTot - 2) Then

    '                str_tbl_HEADER &= String.Format("<th>{0}</th>", Replace(ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper, "VACATION_", ""))
    '                cols += 1

    '            ElseIf j > (colsTot - 2) Then

    '                str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
    '                cols += 1

    '            End If


    '        Next

    '        str_tbl_HEADER &= String.Format("<th></th>")
    '        cols += 1
    '        str_tbl_HEADER &= "</tr>"

    '        str_TABLE &= str_tbl_HEADER

    '        str_tbl_row1 = ""
    '        Dim k As Integer = 0
    '        Dim percentVAC As Decimal = 0
    '        Dim TotWorked As Decimal = 0
    '        Dim TotYearly As Decimal = 0
    '        Dim TotVACATION As Decimal = 0
    '        Dim TotPEND As Decimal = 0

    '        For Each dtRow In ds.Tables(0).Rows

    '            str_tbl_row1 &= "<tr>"
    '            For k = 0 To ds.Tables(0).Columns.Count - 1

    '                If k = 4 Then
    '                    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
    '                ElseIf k = 6 Then
    '                    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
    '                    TotWorked += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
    '                ElseIf k = 7 Then
    '                    TotYearly += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
    '                    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
    '                ElseIf k > 7 And k < (cols + 2) Then
    '                    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
    '                ElseIf k = (cols + 2) Then
    '                    TotVACATION += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
    '                    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
    '                ElseIf k = (cols + 3) Then
    '                    TotPEND += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
    '                    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
    '                End If

    '                'If k > 6 Then
    '                'End If

    '            Next

    '            If dtRow("VACATION_YEARLY") = 0 Then
    '                percentVAC = 0.0
    '            Else
    '                percentVAC = Math.Round(dtRow("VACATION_TOT") / dtRow("VACATION_YEARLY"), 2) * 100
    '            End If

    '            str_tbl_row1 &= String.Format("<td><span class='badge {0}'> {1}% </span></td>", get_Tag(1, percentVAC), percentVAC.ToString)
    '            str_tbl_row1 &= "</tr>"

    '            str_tbl_row1 &= String.Format("<tr><td colspan='{0}'>
    '                                         <div class='progress progress-xs'>
    '                                              <div class='progress-bar {1}' style='width:{2}%'></div>
    '                                          </div>
    '                                        </td></tr>", cols, get_Tag(2, percentVAC), percentVAC)

    '        Next

    '        str_TABLE &= str_tbl_row1

    '        str_tbl_row2 = " <tr style='border-bottom:1px solid lightgray;'> "

    '        'For k = 0 To ds.Tables(0).Columns.Count - 1
    '        str_tbl_row2 &= "<th></th>"
    '        ' If k = 4 Then
    '        str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotWorked.ToString() & "</th>"
    '        ' ElseIf k = 5 Then
    '        '  ElseIf k = 7 Then
    '        str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotYearly.ToString() & "</th><th colspan=" & (cols - 6) & " ></th>"
    '        'ElseIf k = (cols + 4) Then
    '        str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotVACATION.ToString() & "</th>"
    '        '    ElseIf k = (cols + 5) Then

    '        'str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotPEND.ToString() & "</th>"
    '        Dim percentTOT As Decimal = 0
    '        If TotYearly = 0 Then
    '            percentTOT = 100
    '        Else
    '            percentTOT = (1 - Math.Round((TotPEND / TotYearly), 2)) * 100
    '        End If

    '        str_tbl_row2 &= String.Format("<th style='border-top:1px solid lightgray;' ><span class='badge {0}'> {1} </span></th>", get_Tag(1, percentTOT), TotPEND)
    '        'str_tbl_row1 &= String.Format("<td><span class='badge {0}'> {1}% </span></td>", get_Tag(1, percentVAC), percentVAC.ToString)
    '        '     End If

    '        '  Next
    '        str_tbl_row2 &= "<th></th></tr>"

    '        str_TABLE &= str_tbl_row2

    '        str_TABLE &= str_tbl_CLOSE

    '    Else
    '        str_TABLE = ""
    '    End If


    'End Sub



    Public Sub fill_table(ByVal idUser As Integer)

        Dim ds As New DataSet
        Dim adapter As SqlDataAdapter

        cnn.Open()

        Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("SP_HR_BILLABLE_TIME", cnn)
        cmd.CommandType = CommandType.StoredProcedure
        'cmd.Parameters.Add("@tp_view", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
        cmd.Parameters.AddWithValue("@tp_view", 4)
        cmd.Parameters.AddWithValue("@_id_usuario", idUser)
        cmd.Parameters.AddWithValue("@_anio", 0)

        adapter = New SqlDataAdapter(cmd)
        adapter.Fill(ds)

        If Not IsNothing(ds.Tables(0)) Then



            ' grd_cate.DataSource = ds.Tables(0)
            'Dim str_TABLE As String = ""
            Dim str_tbl_OPEN As String = "<table class='table table-striped'>"
            Dim str_tbl_CLOSE As String = "</table>"

            Dim str_tbl_HEADER As String = "<tr>"
            Dim str_tbl_row1 As String = "<tr>"
            Dim str_tbl_row2 As String = "<tr><td colspan='{0}'>{1}</td></tr>"

            Dim cols As Integer = 0
            Dim colsTot As Integer = ds.Tables(0).Columns.Count - 1

            str_TABLE &= str_tbl_OPEN

            For j = 0 To ds.Tables(0).Columns.Count - 1

                If j = 5 Then

                    str_tbl_HEADER &= String.Format("<th>{0}</th>", "YEAR")
                    cols += 1

                ElseIf j = 7 Then

                    str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper) ' ds.Tables(0).Columns(j).ColumnName.ToString
                    cols += 1

                ElseIf j = 8 Then

                    'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                    str_tbl_HEADER &= String.Format("<th>{0}</th>", "ANNUAL_LEAVE")
                    cols += 1

                ElseIf j > 8 And j <= (colsTot - 2) Then

                    str_tbl_HEADER &= String.Format("<th>{0}</th>", Replace(ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper, "ANNUAL_LEAVE", "AL"))
                    cols += 1

                    'ElseIf j > (colsTot - 2) Then

                    '    str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                    '    cols += 1

                ElseIf j = (colsTot - 1) Then

                    'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                    str_tbl_HEADER &= String.Format("<th>{0}</th>", "AL_TAKEN")
                    cols += 1

                ElseIf j = (colsTot) Then

                    'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                    str_tbl_HEADER &= String.Format("<th>{0}</th>", "AL_BALANCE")
                    cols += 1

                End If


            Next

            str_tbl_HEADER &= String.Format("<th></th>")
            cols += 1
            str_tbl_HEADER &= "</tr>"

            str_TABLE &= str_tbl_HEADER

            str_tbl_row1 = ""
            Dim k As Integer = 0
            Dim percentVAC As Decimal = 0
            Dim TotWorked As Decimal = 0
            Dim TotYearly As Decimal = 0
            Dim TotVACATION As Decimal = 0
            Dim TotPEND As Decimal = 0

            For Each dtRow In ds.Tables(0).Rows

                str_tbl_row1 &= "<tr>"
                For k = 0 To ds.Tables(0).Columns.Count - 1

                    If k = 5 Then
                        str_tbl_row1 &= "<th style ='border:1px solid lightgray;' >" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</th>"
                    ElseIf k = 7 Then
                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                        TotWorked += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                    ElseIf k = 8 Then
                        TotYearly += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                    ElseIf k > 8 And k <= (colsTot - 2) Then
                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                    ElseIf k = (colsTot - 1) Then
                        TotVACATION += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                    ElseIf k = (colsTot) Then
                        TotPEND += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"

                        'ElseIf k > 8 And k < (cols + 2) Then
                        '    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                        'ElseIf k = (cols + 2) Then
                        '    TotVACATION += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        '    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                        'ElseIf k = (cols + 3) Then
                        '    TotPEND += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        '    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"


                    End If

                    'If k > 6 Then
                    'End If

                Next

                If dtRow("VACATION_YEARLY") = 0 Then
                    percentVAC = 0.0
                Else
                    percentVAC = Math.Round(dtRow("VACATION_TOT") / dtRow("VACATION_YEARLY"), 2) * 100
                End If

                str_tbl_row1 &= String.Format("<td><span class='badge {0}'> {1}% </span></td>", get_Tag(1, percentVAC), percentVAC.ToString)
                str_tbl_row1 &= "</tr>"

                str_tbl_row1 &= String.Format("<tr><td colspan='1' >AL %</td><td colspan='{0}'>
                                             <div class='progress progress-xs'>
                                                  <div class='progress-bar {1}' style='width:{2}%'></div>
                                              </div>
                                            </td></tr>", cols - 1, get_Tag(2, percentVAC), percentVAC)

            Next

            str_TABLE &= str_tbl_row1

            str_tbl_row2 = " <tr style='border-bottom:1px solid lightgray;'> "

            'For k = 0 To ds.Tables(0).Columns.Count - 1
            str_tbl_row2 &= "<th></th>"
            ' If k = 4 Then
            str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotWorked.ToString() & "</th>"
            ' ElseIf k = 5 Then
            '  ElseIf k = 7 Then
            str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotYearly.ToString() & "</th><th colspan=" & (cols - 6) & " ></th>"
            'ElseIf k = (cols + 4) Then
            str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotVACATION.ToString() & "</th>"
            '    ElseIf k = (cols + 5) Then

            'str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotPEND.ToString() & "</th>"
            Dim percentTOT As Decimal = 0
            If TotYearly = 0 Then
                percentTOT = 100
            Else
                percentTOT = (1 - Math.Round((TotPEND / TotYearly), 2)) * 100
            End If

            str_tbl_row2 &= String.Format("<th style='border-top:1px solid lightgray;' ><span class='badge {0}'> {1} </span></th>", get_Tag(1, percentTOT), TotPEND)
            'str_tbl_row1 &= String.Format("<td><span class='badge {0}'> {1}% </span></td>", get_Tag(1, percentVAC), percentVAC.ToString)
            '     End If

            '  Next
            str_tbl_row2 &= "<th></th></tr>"

            str_TABLE &= str_tbl_row2

            str_TABLE &= str_tbl_CLOSE

        Else
            str_TABLE = ""
        End If


        cnn.Close()

    End Sub



    Public Sub fill_tableCASUAL(ByVal idUser As Integer)

        Dim ds As New DataSet
        Dim adapter As SqlDataAdapter

        cnn.Open()

        Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("SP_HR_BILLABLE_TIME", cnn)
        cmd.CommandType = CommandType.StoredProcedure
        'cmd.Parameters.Add("@tp_view", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
        cmd.Parameters.AddWithValue("@tp_view", 6)
        cmd.Parameters.AddWithValue("@_id_usuario", idUser)
        cmd.Parameters.AddWithValue("@_anio", 0)

        adapter = New SqlDataAdapter(cmd)
        adapter.Fill(ds)

        If Not IsNothing(ds.Tables(0)) Then

            ' grd_cate.DataSource = ds.Tables(0)
            'Dim str_TABLE_CAS As String = ""
            Dim str_tbl_OPEN As String = "<table class='table table-striped'>"
            Dim str_tbl_CLOSE As String = "</table>"

            Dim str_tbl_HEADER As String = "<tr>"
            Dim str_tbl_row1 As String = "<tr>"
            Dim str_tbl_row2 As String = "<tr><td colspan='{0}'>{1}</td></tr>"

            Dim cols As Integer = 0
            Dim colsTot As Integer = ds.Tables(0).Columns.Count - 1

            Dim gYEAR As Integer = DatePart(DateInterval.Year, Date.UtcNow)

            str_TABLE_CAS &= str_tbl_OPEN

            For j = 0 To ds.Tables(0).Columns.Count - 1

                If j = 5 Then

                    str_tbl_HEADER &= String.Format("<th>{0}</th>", "YEAR")
                    cols += 1

                ElseIf j = 7 Then

                    str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper) ' ds.Tables(0).Columns(j).ColumnName.ToString
                    cols += 1

                ElseIf j = 8 Then

                    'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                    str_tbl_HEADER &= String.Format("<th>{0}</th>", "CASUAL_LEAVE")
                    cols += 1

                ElseIf j = 9 Then

                    'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                    str_tbl_HEADER &= String.Format("<th>{0}</th>", "SICK_LEAVE")
                    cols += 1

                ElseIf j > 9 And j <= (colsTot - 4) Then


                    If InStr(ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper, "CASUAL_LEAVE") > 0 Then
                        str_tbl_HEADER &= String.Format("<th>{0}</th>", Replace(ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper, "CASUAL_LEAVE", "CL"))
                    ElseIf InStr(ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper, "SICK_LEAVE") > 0 Then
                        str_tbl_HEADER &= String.Format("<th>{0}</th>", Replace(ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper, "SICK_LEAVE", "SL"))
                    End If
                    cols += 1

                    'ElseIf j > (colsTot - 2) Then
                    '    str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                    '    cols += 1

                ElseIf j = (colsTot - 3) Then

                    'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                    str_tbl_HEADER &= String.Format("<th>{0}</th>", "CL_TAKEN")
                    cols += 1

                ElseIf j = (colsTot - 2) Then

                    'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                    str_tbl_HEADER &= String.Format("<th>{0}</th>", "CL_BALANCE")
                    cols += 1

                    str_tbl_HEADER &= String.Format("<th></th>")
                    cols += 1

                ElseIf j = (colsTot - 1) Then

                    'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                    str_tbl_HEADER &= String.Format("<th>{0}</th>", "SL_TAKEN")
                    cols += 1


                ElseIf j = (colsTot) Then

                    'str_tbl_HEADER &= String.Format("<th>{0}</th>", ds.Tables(0).Columns(j).ColumnName.ToString.ToUpper)
                    str_tbl_HEADER &= String.Format("<th>{0}</th>", "SL_BALANCE")
                    cols += 1

                    str_tbl_HEADER &= String.Format("<th></th>")
                    cols += 1

                End If


            Next


            str_tbl_HEADER &= "</tr>"

            str_TABLE_CAS &= str_tbl_HEADER

            str_tbl_row1 = ""
            Dim k As Integer = 0
            Dim percentVAC As Decimal = 0
            Dim percentSICK As Decimal = 0
            Dim TotWorked As Decimal = 0
            Dim TotYearly As Decimal = 0
            Dim SickYearly As Decimal = 0
            Dim TotVACATION As Decimal = 0
            Dim TotSICK As Decimal = 0
            Dim TotPEND As Decimal = 0
            Dim TotsickPEND As Decimal = 0
            Dim ThisYEAR As Boolean = False

            For Each dtRow In ds.Tables(0).Rows

                ThisYEAR = If(gYEAR = dtRow(ds.Tables(0).Columns(5).ColumnName.ToString), True, False)

                str_tbl_row1 &= "<tr>"
                For k = 0 To ds.Tables(0).Columns.Count - 1

                    If k = 5 Then
                        'str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                        str_tbl_row1 &= "<th style ='border:1px solid lightgray;' >" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</th>"
                    ElseIf k = 7 Then
                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"

                        If ThisYEAR And TotWorked = 0 Then
                            TotWorked = dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        End If

                    ElseIf k = 8 Then

                        If ThisYEAR And TotYearly = 0 Then
                            TotYearly = dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        End If

                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                    ElseIf k = 9 Then


                        If ThisYEAR And SickYearly = 0 Then
                            SickYearly = dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        End If

                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                    ElseIf k > 9 And k <= (colsTot - 4) Then
                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"

                    ElseIf k = (colsTot - 3) Then


                        If ThisYEAR And TotVACATION = 0 Then
                            TotVACATION = dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        End If

                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"

                        If ThisYEAR And TotPEND = 0 Then
                            TotPEND = dtRow(ds.Tables(0).Columns(k + 2).ColumnName.ToString)
                        End If

                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k + 2).ColumnName.ToString) & "</td>"

                        If dtRow("VACATION_YEARLY") = 0 Then
                            percentVAC = 0.0
                        Else
                            percentVAC = Math.Round(dtRow("VACATION_TOT") / dtRow("VACATION_YEARLY"), 2) * 100
                        End If

                        str_tbl_row1 &= String.Format("<td><span class='badge {0}'> {1}% </span></td>", get_Tag(1, percentVAC), percentVAC.ToString)


                    ElseIf k = (colsTot - 2) Then


                        If ThisYEAR And TotSICK = 0 Then
                            TotSICK = dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        End If

                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"

                        If ThisYEAR And TotsickPEND = 0 Then
                            TotsickPEND = dtRow(ds.Tables(0).Columns(k + 2).ColumnName.ToString)
                        End If

                        str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k + 2).ColumnName.ToString) & "</td>"

                        If dtRow("SICK_YEARLY") = 0 Then
                            percentSICK = 0.0
                        Else
                            percentSICK = Math.Round(dtRow("SICK_TOT") / dtRow("SICK_YEARLY"), 2) * 100
                        End If

                        str_tbl_row1 &= String.Format("<td><span class='badge {0}'> {1}% </span></td>", get_Tag(1, percentSICK), percentSICK.ToString)


                        ' ElseIf k = (colsTot - 1) Then

                        'ElseIf k = (colsTot) Then

                        'ElseIf k > 8 And k < (cols + 2) Then
                        '    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                        'ElseIf k = (cols + 2) Then
                        '    TotVACATION += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        '    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"
                        'ElseIf k = (cols + 3) Then
                        '    TotPEND += dtRow(ds.Tables(0).Columns(k).ColumnName.ToString)
                        '    str_tbl_row1 &= "<td>" & dtRow(ds.Tables(0).Columns(k).ColumnName.ToString) & "</td>"

                    End If

                    'If k > 6 Then
                    'End If

                Next



                str_tbl_row1 &= "</tr>"

                str_tbl_row1 &= String.Format("<tr><td colspan='1' >CL %</td><td colspan='{0}'>
                                             <div class='progress progress-xs'>
                                                  <div class='progress-bar {1}' style='width:{2}%'></div>
                                              </div>
                                            </td></tr>", cols - 1, get_Tag(2, percentVAC), percentVAC)

                str_tbl_row1 &= String.Format("<tr><td colspan='1' >SL %</td><td colspan='{0}'>
                                             <div class='progress progress-xs'>
                                                  <div class='progress-bar {1}' style='width:{2}%'></div>
                                              </div>
                                            </td></tr>", cols - 1, get_Tag(2, percentSICK), percentSICK)

            Next

            str_TABLE_CAS &= str_tbl_row1

            str_tbl_row2 = " <tr style='border-bottom:1px solid lightgray;'> "

            'For k = 0 To ds.Tables(0).Columns.Count - 1
            str_tbl_row2 &= "<th></th>"
            ' If k = 4 Then
            str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotWorked.ToString() & "</th>"
            ' ElseIf k = 5 Then
            '  ElseIf k = 7 Then
            str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotYearly.ToString() & "</th>"
            str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & SickYearly.ToString() & "</th>"
            str_tbl_row2 &= "<th colspan=" & (cols - 10) & " ></th>"
            'ElseIf k = (cols + 4) Then
            str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotVACATION.ToString() & "</th>"
            '    ElseIf k = (cols + 5) Then

            'str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotPEND.ToString() & "</th>"
            Dim percentTOT As Decimal = 0
            If TotYearly = 0 Then
                percentTOT = 100
            Else
                percentTOT = (1 - Math.Round((TotPEND / TotYearly), 2)) * 100
            End If

            str_tbl_row2 &= String.Format("<th style='border-top:1px solid lightgray;' ><span class='badge {0}'> {1} </span></th><th></th>", get_Tag(1, percentTOT), TotPEND)
            'str_tbl_row1 &= String.Format("<td><span class='badge {0}'> {1}% </span></td>", get_Tag(1, percentVAC), percentVAC.ToString)
            '     End If

            '  Next

            str_tbl_row2 &= "<th style='border-top:1px solid lightgray;' >" & TotSICK.ToString() & "</th>"

            Dim percentTOT2 As Decimal = 0
            If SickYearly = 0 Then
                percentTOT2 = 100
            Else
                percentTOT2 = (1 - Math.Round((TotsickPEND / SickYearly), 2)) * 100
            End If

            str_tbl_row2 &= String.Format("<th style='border-top:1px solid lightgray;' ><span class='badge {0}'> {1} </span></th>", get_Tag(1, percentTOT2), TotsickPEND)


            str_tbl_row2 &= "</tr>"

            str_TABLE_CAS &= str_tbl_row2

            str_TABLE_CAS &= str_tbl_CLOSE

        Else
            str_TABLE_CAS = ""
        End If

        cnn.Close()

    End Sub

    Public Function get_Tag(ByVal idT As Integer, ByVal PercentV As Decimal) As String

        Dim strOutPut_bar As String = ""
        Dim strOutPut_color As String = ""

        If PercentV >= 0 And PercentV <= 25 Then
            strOutPut_bar = "progress-bar-yellow"
            strOutPut_color = "bg-yellow"
        ElseIf PercentV > 25 And PercentV <= 50 Then
            strOutPut_bar = "progress-bar-primary"
            strOutPut_color = "bg-light-blue"
        ElseIf PercentV > 50 And PercentV <= 75 Then
            strOutPut_bar = "progress-bar-success"
            strOutPut_color = "bg-green"
        Else
            strOutPut_bar = "progress-bar-red"
            strOutPut_color = "bg-red"
        End If

        If idT = 1 Then
            get_Tag = strOutPut_bar
        Else
            get_Tag = strOutPut_color
        End If

    End Function



    Public Sub fill_Grid(ByVal booREbind As Boolean, ByVal idUser As Integer)

        Dim ds As New DataSet
        Dim adapter As SqlDataAdapter


        cnn.Open()

        'Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("SP_HR_BILLABLE_TIME", cnn)
        'cmd.CommandType = CommandType.StoredProcedure
        ''cmd.Parameters.Add("@tp_view", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
        'cmd.Parameters.AddWithValue("@tp_view", Me.hd_tp.Value)
        'cmd.Parameters.AddWithValue("@_id_usuario", idUser)
        'cmd.Parameters.AddWithValue("@_anio", 0)

        'adapter = New SqlDataAdapter(cmd)
        'adapter.Fill(ds)


        'grd_cate.DataSource = ds.Tables(0)

        'If booREbind Then
        '    grd_cate.Rebind()
        'End If

        ''For i = 0 To ds.Tables(0).Rows.Count - 1
        ''    MsgBox(ds.Tables(0).Rows(i).Item(0))
        ''Next
        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("id_usuario"))) Then
        '    hideColumn("id_usuario")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("id"))) Then
        '    hideColumn("id")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("nombre_usuario"))) Then
        '    hideColumn("nombre_usuario")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("apellidos_usuario"))) Then
        '    hideColumn("apellidos_usuario")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("worked_count"))) Then
        '    hideColumn("worked_count")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("salary"))) Then
        '    hideColumn("salary")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("id_billable_time"))) Then
        '    hideColumn("id_billable_time")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("payable"))) Then
        '    hideColumn("payable")
        'End If

        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("billable_time_category"))) Then
        '    hideColumn("billable_time_category")
        'End If



        Dim cmd2 As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("SP_HR_BILLABLE_TIME", cnn)
        'cmd2.CommandText = "SP_HR_BILLABLE_TIME"
        cmd2.CommandType = CommandType.StoredProcedure
        'cmd.Parameters.Add("@tp_view", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
        cmd2.Parameters.AddWithValue("@tp_view", 7)
        cmd2.Parameters.AddWithValue("@_id_usuario", idUser)
        cmd2.Parameters.AddWithValue("@_anio", 0)

        adapter = New SqlDataAdapter(cmd2)
        adapter.Fill(ds)

        'Dim dataT As DataTable = ds.Tables(0)
        radgrid_time_reported.DataSource = ds.Tables(0)

        If booREbind Then
            radgrid_time_reported.Rebind()
        End If

        cnn.Close()


        'Dim expression As GridGroupByExpression = New GridGroupByExpression
        'Dim gridGroupByField As GridGroupByField = New GridGroupByField
        'If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("billable_time_category"))) Then

        '    gridGroupByField = New GridGroupByField
        '    gridGroupByField.FieldName = "billable_time_category"
        '    gridGroupByField.HeaderText = "billable_time_category"
        '    'gridGroupByField.HeaderValueSeparator = ""
        '    'gridGroupByField.FormatString = "<strong>{0:0}</strong>"
        '    'gridGroupByField.Aggregate = GridAggregateFunction.Sum
        '    expression.SelectFields.Add(gridGroupByField)

        '    'gridGroupByField = New GridGroupByField
        '    'gridGroupByField.FieldName = "ContactTitle"
        '    'expression.GroupByFields.Add(gridGroupByField)
        '    grd_cate.MasterTableView.GroupByExpressions.Add(expression)

        'End If

        Me.btnlk_Export.PostBackUrl = "~/HRM/frm_TemplateReport.aspx?idTR=1&vUs=" & Me.lbl_id_usuario.Text 'Report Detail Tools


    End Sub

    Public Function getFecha(dateIN As DateTime, strFormat As String, boolUTC As Boolean) As String

        Dim clDate As APPROVAL.cls_dUtil
        '************************************SYSTEM INFO********************************************
        Dim cProgram As New RMS.cls_Program
        cProgram.get_Sys(0, True)
        cProgram.get_Programs(cl_user.Id_Cprogram, True)
        Dim userCulture As CultureInfo
        Dim timezoneUTC As Integer
        userCulture = cl_user.regionalizacionCulture
        timezoneUTC = Val(cProgram.getprogramField("huso_horario", "id_programa", cl_user.Id_Cprogram))
        clDate = New APPROVAL.cls_dUtil(userCulture, timezoneUTC)
        '************************************SYSTEM INFO********************************************
        Return clDate.set_DateFormat(dateIN, strFormat, timezoneUTC, boolUTC)
        'Return dateIN.ToShortDateString

    End Function

    'Private Sub grd_cate_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grd_cate.NeedDataSource
    '    fill_Grid(False, Convert.ToInt32(Me.lbl_id_usuario.Text))

    'End Sub

    'Dim totalC(check_field().Rows.Count) As Integer
    'Dim totalC As Integer()
    'ReDim Preserve totalC(check_field().rows.count)

    'Private Sub grd_cate_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_cate.ItemDataBound


    '    If TypeOf e.Item Is GridGroupHeaderItem Then
    '        Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
    '        Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
    '        'item.DataCell.Text = item.DataCell.Text.Split(":")(1).ToString
    '        item.DataCell.Text = "--> " & item.DataCell.Text.Split(":")(1).ToString
    '    End If


    '    'If IsNothing(totalC) Then
    '    '    ReDim Preserve totalC(check_field().Rows.Count)
    '    'End If


    '    'If (TypeOf e.Item Is GridDataItem) Then

    '    '    Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
    '    '    Dim i = 0
    '    '    For Each dtRow As DataRow In check_field().Rows
    '    '        Dim fieldValue As Integer = Integer.Parse(dataItem(dtRow("Period")).Text)
    '    '        'ReDim Preserve totalC(i + 1)
    '    '        totalC(i) += fieldValue
    '    '        i += 1
    '    '    Next

    '    '    'Dim fieldValue As Integer = Integer.Parse(dataItem("Quantity").Text)
    '    '    'totalC = (totalC + fieldValue)

    '    'End If

    '    'If (TypeOf e.Item Is GridFooterItem) Then

    '    '    Dim footerItem As GridFooterItem = CType(e.Item, GridFooterItem)
    '    '    Dim i = 0
    '    '    For Each dtRow As DataRow In check_field().Rows
    '    '        'ReDim Preserve totalC(i + 1)
    '    '        footerItem(dtRow("Period")).Text = "total: " + totalC(i).ToString()
    '    '        i += 1
    '    '    Next

    '    '    ' footerItem("Quantity").Text = "total: " + totalC.ToString()

    '    'End If

    '    If (TypeOf e.Item Is GridGroupFooterItem) Then
    '        Dim GroupfooterItem As GridGroupFooterItem = CType(e.Item, GridGroupFooterItem)
    '        For Each dtRow As DataRow In check_field().Rows

    '            Dim strArrContent As String() = GroupfooterItem(dtRow("Period")).Text.Split(":")

    '            If strArrContent.Length > 1 Then

    '                GroupfooterItem(dtRow("Period")).Text = "Total: " + GroupfooterItem(dtRow("Period")).Text.ToString().Split(":")(1).ToString()

    '                If Val(strArrContent(1)) > 0 Then
    '                    GroupfooterItem(dtRow("Period")).Style.Add("font-weight", "bold")
    '                End If

    '            End If
    '        Next
    '    End If

    '    If (TypeOf e.Item Is GridFooterItem) Then

    '        Dim footerItem As GridFooterItem = CType(e.Item, GridFooterItem)
    '        For Each dtRow As DataRow In check_field().Rows

    '            Dim strArrContent As String() = footerItem(dtRow("Period")).Text.Split(":")

    '            If strArrContent.Length > 1 Then

    '                footerItem(dtRow("Period")).Text = dtRow("Period") + " " + footerItem(dtRow("Period")).Text.ToString().Split(":")(1).ToString()
    '                If Val(strArrContent(1)) > 0 Then
    '                    footerItem(dtRow("Period")).Style.add("font-weight", "bold")
    '                End If

    '            End If


    '        Next

    '    End If

    'End Sub

    Public Function check_field(Optional tp As Integer = 0) As DataTable

        Dim id_type = If(tp = 0, Me.hd_tp.Value, tp)
        Dim cls_rh_employee As APPROVAL.cls_rh_employee = New APPROVAL.cls_rh_employee(Convert.ToInt32(Me.Session("E_IDprograma")))
        Dim tbl_fields As DataTable = cls_rh_employee.get_fields(id_type)
        check_field = tbl_fields

    End Function



    'Public Sub hideColumn(strColumn)

    '    Me.grd_cate.MasterTableView.GetColumn(strColumn).Display = True
    '    Me.grd_cate.MasterTableView.GetColumn(strColumn).Visible = False

    'End Sub
    'Protected Sub cmb_tipo_usuario_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmb_tipo_usuario.SelectedIndexChanged
    '    If cmb_tipo_usuario.SelectedValue = 2 Then
    '        Me.divEjecutor.Visible = True
    '    Else
    '        Me.divEjecutor.Visible = False
    '    End If

    'End Sub

    'Private Sub chk_PasswordChange_CheckedChanged(sender As Object, e As EventArgs) Handles chk_PasswordChange.CheckedChanged

    '    If Me.chk_PasswordChange.Checked = True Then
    '        Me.txt_clave_usuario.Text = ""
    '        Me.txt_clave_usuario.Enabled = True
    '    Else
    '        Me.txt_clave_usuario.Enabled = False
    '    End If

    'End Sub

    Public Sub LoadTimeSheets(ByVal bndRebind As Boolean, ByVal idUser As Integer)

        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Convert.ToInt32(Me.Session("E_IDprograma")))

        Me.grd_timesheet.DataSource = cls_TimeSheet.getTimeSheetUSR(idUser)

        If bndRebind Then
            Me.grd_timesheet.DataBind()
        End If


    End Sub

    Private Sub grd_timesheet_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grd_timesheet.ItemDataBound


        If TypeOf e.Item Is GridGroupHeaderItem Then
            Dim item As GridGroupHeaderItem = CType(e.Item, GridGroupHeaderItem)
            Dim groupDataRow As DataRowView = CType(e.Item.DataItem, DataRowView)
            'item.DataCell.Text = item.DataCell.Text.Split(":")(1).ToString
            item.DataCell.Text = ""
        End If


        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            Dim editar As New ImageButton
            'HyperLink ID="hlk_timesheet"
            Dim Preview_ As New HyperLink
            '    Dim ruta As New ImageButton
            '    Dim visible As New CheckBox
            editar = CType(itemD("colm_edit").FindControl("editar"), ImageButton)

            Preview_ = CType(itemD("colm_open").FindControl("hlk_timesheet"), HyperLink)
            Preview_.NavigateUrl = "~/TimeSheet/frm_TimeSheetFollowingREP.aspx?ID=" & itemD("id_timesheet").Text

            'Dim idUsuario As Integer = Convert.ToInt32(Me.Session("E_IdUser"))
            'If itemD("id_usuario").Text = idUsuario Or itemD("usuario_creo").Text = idUsuario Then
            editar.PostBackUrl = "~/TimeSheet/frm_TimeSheetAdd.aspx?ID=" & itemD("id_timesheet").Text
            'Else
            '    editar.Visible = False
            'End If

        End If



    End Sub

    'Private Sub grd_cate_PreRender(sender As Object, e As EventArgs) Handles grd_cate.PreRender

    '    If grd_cate.MasterTableView.GroupByExpressions.Count > 0 Then

    '        For i = 0 To grd_cate.MasterTableView.GroupByExpressions.Count

    '            If Not IsNothing(grd_cate.MasterTableView.GroupByExpressions(i).GroupByFields) Then
    '                Dim strField As String = grd_cate.MasterTableView.GroupByExpressions(i).GroupByFields(0).FieldName
    '                If strField = "billable_time_category" Then
    '                    grd_cate.MasterTableView.GroupByExpressions.RemoveAt(i)
    '                End If
    '            End If
    '            i += 1
    '        Next

    '    End If

    '    Dim expression As GridGroupByExpression = New GridGroupByExpression
    '    Dim gridGroupByField As GridGroupByField = New GridGroupByField
    '    If (Not IsNothing(grd_cate.MasterTableView.GetColumnSafe("billable_time_category"))) Then

    '        gridGroupByField = New GridGroupByField
    '        gridGroupByField.FieldName = "billable_time_category"
    '        gridGroupByField.HeaderText = "billable_time_category"
    '        'gridGroupByField.HeaderValueSeparator = ""
    '        'gridGroupByField.FormatString = "<strong>{0:0}</strong>"
    '        'gridGroupByField.Aggregate = GridAggregateFunction.Sum
    '        expression.GroupByFields.Add(gridGroupByField)
    '        expression.SelectFields.Add(gridGroupByField)

    '        'gridGroupByField = New GridGroupByField
    '        'gridGroupByField.FieldName = "ContactTitle"
    '        'expression.GroupByFields.Add(gridGroupByField)
    '        grd_cate.MasterTableView.GroupByExpressions.Add(expression)
    '        grd_cate.Rebind()

    '    End If

    'End Sub

    'Private Sub grd_cate_ColumnCreated(sender As Object, e As GridColumnCreatedEventArgs) Handles grd_cate.ColumnCreated

    '    Dim boundColumn As New GridBoundColumn

    '    If e.Column.UniqueName = "billable_time" Then
    '        boundColumn = e.Column
    '        boundColumn.ItemStyle.Wrap = True
    '        boundColumn.ItemStyle.Width = Unit.Pixel(200)
    '        boundColumn.HeaderStyle.Width = Unit.Pixel(200)
    '    End If

    '    For Each dtRow As DataRow In check_field().Rows

    '        If e.Column.UniqueName = dtRow("Period") Then

    '            boundColumn = e.Column
    '            boundColumn.Aggregate = GridAggregateFunction.Sum

    '        End If

    '    Next

    'End Sub
End Class
