Imports ly_SIME
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports System.Drawing

Public Class frm_parSgmt
    Inherits System.Web.UI.Page
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim cl_error As New ly_SIME.CORE.ErrorHandler
    Dim frmCODE As String = "ADM_PAR_SGMT"
    Dim clListados As New ly_SIME.CORE.cls_listados
    Dim controles As New ly_SIME.CORE.cls_controles
    Dim dtConceptos As New DataTable
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
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not Me.IsPostBack Then
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))

            Dim id_par = Convert.ToInt32(Me.Request.QueryString("id"))
            Me.idPar.Value = id_par



            Session.Remove("dtConceptos")
            LoadData(id_par)
            fillGrid()
        End If
    End Sub
    Protected Sub grd_cate_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim hlnk As HyperLink = New HyperLink
            Dim itemD As GridDataItem = DirectCast(e.Item, GridDataItem)

            hlnk = CType(itemD("completoC").FindControl("Completo"), HyperLink)
            hlnk.ToolTip = "Alert"

            hlnk.ImageUrl = "../Imagenes/Iconos/Circle_" & itemD("Alerta").Text & ".png" ' e.Item.Cells(7).Text.ToString
            If itemD("descripcion_estado").Text = "Pending" Then
                For i As Integer = 2 To 8
                    e.Item.Cells(i).BackColor = Color.FromArgb(227, 132, 67)
                Next
            End If

        End If
    End Sub
    Sub fillGridRutaAprobacion(ByVal id_documento As Integer)
        Me.grd_cate.DataSource = clss_approval.get_ta_RutaSeguimiento(id_documento.ToString())
        Me.grd_cate.DataBind()
    End Sub
    Sub LoadData(ByVal id_par As Integer)
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If

        Using dbEntities As New dbRMS_JIEntities
            Dim par = dbEntities.tme_pares.Find(id_par)
            Dim parDetalle = dbEntities.vw_tme_par.Where(Function(p) p.id_par = id_par).FirstOrDefault()
            Dim id_usuario_appStr = parDetalle.id_usuario_app
            Dim id_usuario_app = id_usuario_appStr.Split(",")
            Dim indx As Integer = Convert.ToInt32(Me.Session("E_IDUser"))

            If parDetalle IsNot Nothing Then
                Dim intOwnerLegalizacion As String() = parDetalle.id_usuario_app.ToString.Split(",")
                If intOwnerLegalizacion.Where(Function(p) p.Contains(indx)).Count() = 0 Then
                    Me.Response.Redirect("~/administrativo/frm_parDetalle?id=" & id_par)
                End If
            End If

            'Me.lbl_categoria.Text = par.ta_documento_par.FirstOrDefault().ta_documento.ta_tipoDocumento.descripcion_aprobacion
            'Me.lbl_codigo.Text = par.codigo_par
            Me.lbl_fecha_solicitud.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_crea)
            Me.lbl_fecha_entrega.Text = String.Format("{0:MM/dd/yyyy}", par.fecha_requiere_servicio)
            Me.lbl_solicitado.Text = parDetalle.usuario_solicita
            Me.lbl_aprobado.Text = parDetalle.nombre_departamento & " - " & parDetalle.nombre_municipio
            Me.lbl_cargo.Text = parDetalle.cargo_usuario
            Me.lbl_codigo_rfa.Text = If(parDetalle.asociado_actividad = True, parDetalle.subactividad, "")
            Me.lbl_proposito_par.Text = parDetalle.tipo_solicitud
            Me.lbl_asociado_a_par.Text = parDetalle.asociado_a
            'Me.lbl_usuario_solicitud.Text = parDetalle.usuario_cargo_solicita & " (" & parDetalle.cargo_usuario & ")"
            'Me.lbl_tasa_ser.Text = parDetalle.tasa_ser_cotizacion
            'Me.lbl_codigo_pt.Text = par.codigo_pt
            Me.lbl_departamento.Text = parDetalle.nombre_region
            Me.lbl_tipo_par.Text = parDetalle.tipo_par
            'Me.lbl_regional.Text = par.t_subregiones.t_regiones.nombre_region
            'Me.lbl_sub_region.Text = par.t_subregiones.nombre_subregion
            'Me.lbl_proposito_par.Text = par.proposito
            'Me.lbl_municipio_entrega.Text = par.t_municipios.t_departamentos.nombre_departamento & " " & par.t_municipios.nombre_municipio
            Me.lbl_tipo_par.Text = par.tme_tipo_par.tipo_par

            For Each item In par.tme_par_detalle.ToList()
                Dim fecha = DateTime.Now
                Dim rnd As New Random()
                Dim aleatorio As String = ""
                Dim index = 1
                aleatorio = rnd.Next(index, 9999999).ToString()
                Dim idunique = fecha.Year & fecha.Month & fecha.Day & fecha.Hour & fecha.Minute & fecha.Second & fecha.Millisecond & aleatorio & item.id_par_detalle

                dtConceptos.Rows.Add(idunique, item.cantidad, item.descripcion, item.precio_unitario, item.valor_total, item.tme_unidad_medida_par.unidad_medida, True, item.id_par_detalle)
            Next


            Me.grd_servicios_requeridos.DataSource = dbEntities.tme_par_detalle.Where(Function(p) p.id_par = id_par).Select(Function(p) New With {Key p.descripcion,
                                                                                                                                p.cantidad,
                                                                                                                                p.precio_unitario,
                                                                                                                                p.valor_total,
                                                                                                                                .unidad_medida = p.tme_unidad_medida_par.unidad_medida,
                                                                                                                                p.id_par_detalle}).ToList()
            Me.grd_servicios_requeridos.DataBind()

            Session("dtConceptos") = dtConceptos
            If par.ta_documento_par.Where(Function(p) p.reversado Is Nothing).Count() > 0 Then
                Me.HiddenField1.Value = par.ta_documento_par.Where(Function(p) p.reversado Is Nothing).LastOrDefault().id_documento
                fillGridRutaAprobacion(par.ta_documento_par.Where(Function(p) p.reversado Is Nothing).LastOrDefault().id_documento)
            End If

            Dim tbl_AppOrden As New DataTable
            tbl_AppOrden = clss_approval.get_ta_AppDocumentoAPP_MAX(Me.HiddenField1.Value) 'To get the info on the max step (id_app_doc

            If parDetalle IsNot Nothing Then
                Dim intOwnerLegalizacion As String() = parDetalle.id_usuario_app.ToString.Split(",")
                If intOwnerLegalizacion.Where(Function(p) p.Contains(indx)).Count() = 0 Then
                    Me.Response.Redirect("~/administrativo/frm_parDetalle?id=" & id_par)
                End If
            End If

            If tbl_AppOrden.Rows(0)("id_ruta_next").ToString <> "-1" Then
                btn_Completed.Visible = False
                btn_Approved.Visible = True
            Else
                btn_Completed.Visible = True
                btn_Approved.Visible = False
            End If
        End Using
    End Sub
    Sub fillGrid()
        If Session("dtConceptos") IsNot Nothing Then
            dtConceptos = Session("dtConceptos")
        Else
            createdtcolums()
        End If
        'Me.grd_conceptos.DataSource = dtConceptos
        'Me.grd_conceptos.DataBind()
    End Sub
    Sub createdtcolums()
        If dtConceptos.Columns.Count = 0 Then
            dtConceptos.Columns.Add("id_par_detalle", GetType(String))
            dtConceptos.Columns.Add("cantidad", GetType(Double))
            dtConceptos.Columns.Add("descripcion", GetType(String))
            dtConceptos.Columns.Add("valor_unitario", GetType(Double))
            dtConceptos.Columns.Add("valor", GetType(Double))
            dtConceptos.Columns.Add("unidad_medida", GetType(String))
            dtConceptos.Columns.Add("esta_bd", GetType(Boolean))
            dtConceptos.Columns.Add("id_par_detalle_bd", GetType(Integer))
        End If
    End Sub
    Private Function calculaDiaHabil(ByVal nDias As Integer, ByVal fechaPost As Date) As Date
        Dim hoy As Date = Date.UtcNow
        Dim weekend As Integer = 0
        Dim fecha_limite As Date
        Select Case fechaPost.DayOfWeek
            Case DayOfWeek.Sunday
                If nDias < 6 Then
                    weekend = 0
                ElseIf nDias < 11 Then
                    weekend = 2
                ElseIf nDias < 16 Then
                    weekend = 4
                End If
            Case DayOfWeek.Monday
                If nDias < 5 Then
                    weekend = 0
                ElseIf nDias < 10 Then
                    weekend = 2
                ElseIf nDias < 15 Then
                    weekend = 4
                End If
            Case DayOfWeek.Tuesday
                If nDias < 4 Then
                    weekend = 0
                ElseIf nDias < 9 Then
                    weekend = 2
                ElseIf nDias < 14 Then
                    weekend = 4
                End If
            Case DayOfWeek.Wednesday
                If nDias < 3 Then
                    weekend = 0
                ElseIf nDias < 8 Then
                    weekend = 2
                ElseIf nDias < 13 Then
                    weekend = 4
                End If
            Case DayOfWeek.Thursday
                If nDias < 2 Then
                    weekend = 0
                ElseIf nDias < 7 Then
                    weekend = 2
                ElseIf nDias < 12 Then
                    weekend = 4
                End If
            Case DayOfWeek.Friday
                If nDias < 1 Then
                    weekend = 0
                ElseIf nDias < 6 Then
                    weekend = 2
                ElseIf nDias < 11 Then
                    weekend = 4
                End If
            Case DayOfWeek.Saturday
                If nDias < 6 Then
                    weekend = 2
                ElseIf nDias < 10 Then
                    weekend = 4
                End If
        End Select
        Dim totaldias = weekend + nDias
        fecha_limite = DateAdd(DateInterval.DayOfYear, totaldias, fechaPost)
        Return fecha_limite
    End Function
    Protected Sub btn_Approved_Click(sender As Object, e As EventArgs) Handles btn_Approved.Click

        'If check_exchange_Rate() Then
        'EXECUTE_ACTION(cAPPROVED)
        'btn_Approved.Enabled = True
        'End If

        'Dim IsTool As Int16 = Convert.ToInt16(Me.hd_is_tool.Value)
        'Dim boolIS_deliverable As Boolean = Convert.ToBoolean(IsTool)

        'If boolIS_deliverable Then
        '    check_allocated_amount()  'This is for confirming the allocated amount for Deliverable
        'Else
        EXECUTE_ACTION(cAPPROVED)
        'End If

    End Sub

    Protected Sub btn_Completed_Click(sender As Object, e As EventArgs) Handles btn_Completed.Click


        'Dim IsTool As Int16 = Convert.ToInt16(Me.hd_is_tool.Value)
        'Dim boolIS_deliverable As Boolean = Convert.ToBoolean(IsTool)

        'If boolIS_deliverable Then
        '    check_allocated_amount()  'This is for confirming the allocated amount for Deliverable
        'Else
        EXECUTE_ACTION(cAPPROVED)
        'End If




    End Sub

    Protected Sub btn_STandBy_Click(sender As Object, e As EventArgs) Handles btn_STandBy.Click
        EXECUTE_ACTION(cSTANDby)
    End Sub

    Protected Sub btn_NotApproved_Click(sender As Object, e As EventArgs) Handles btn_NotApproved.Click
        EXECUTE_ACTION(cnotAPPROVED)
    End Sub

    Protected Sub btn_Cancelled_Click(sender As Object, e As EventArgs) Handles btn_Cancelled.Click
        EXECUTE_ACTION(cCANCELLED)
    End Sub
    Protected Sub btn_salir_Click(sender As Object, e As EventArgs) Handles btn_salir.Click
        Me.Response.Redirect("~/administrativo/frm_par")
    End Sub

    Protected Sub EXECUTE_ACTION(ByVal vEsTatE As Integer)
        Using dbEntities As New dbRMS_JIEntities

            'Dim RadAsyncUpload1 As New RadAsyncUpload

            Dim duracion = 0
            Dim Err As Boolean = False

            Dim tbl_rutas_tipo_doc As New DataTable
            Dim strComment As String = ""

            Dim id_par = Convert.ToInt32(Me.idPar.Value)
            Try

                Dim tbl_AppOrden As New DataTable
                clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
                tbl_AppOrden = clss_approval.get_ta_AppDocumentoAPP_MAX(Me.HiddenField1.Value) 'To get the info on the max step (id_app_doc)
                'Check_NewFile_FileUploaded() 'Uploading New Files here

                'FileUploaded_UpdatedFiles() 'Uploading Files here

                '******This has to change for Stand BY app*********************
                Dim idRuta = tbl_AppOrden.Rows(0)("id_ruta_next").ToString
                Dim idNextRol = tbl_AppOrden.Rows(0)("id_role_next").ToString
                Dim idNextUserID = tbl_AppOrden.Rows(0)("id_usuario_next").ToString
                Dim idRoute As Integer = 0
                idRoute = clss_approval.get_ta_AppDocumentoOrden_MAX(Me.HiddenField1.Value).Rows(0).Item("id_ruta").ToString 'current route
                Dim id_tipoDoc = tbl_AppOrden.Rows(0).Item("id_tipoDocumento")
                Dim id_app_documento = tbl_AppOrden.Rows(0).Item("id_app_Documento")
                lblerr_user.Text = ""

                If idRuta = -1 Then 'Just for the last step

                    '*****************SET DOCUMENT REQUIRED BY every approval**************************
                    Dim tbl_Doc As DataTable
                    Dim Bool_pendingDoc As Boolean = False
                    Dim str_pendingDoc As String = ""
                    Dim i As Integer = 0
                    Dim strPart1 As String
                    Dim strPart2 As String
                    Dim Bool_find As Boolean = False

                    tbl_Doc = clss_approval.get_Doc_support_Route_Pending(CType(id_tipoDoc, Integer), Me.HiddenField1.Value)

                    For Each dtRow As DataRow In tbl_Doc.Rows

                        If dtRow("requeridoFin") = "SI" Then


                            'For Each item As RadListBoxItem In rdListBox_files.Items

                            '    If CType(item.Value, Integer) = dtRow("id_doc_soporte") Then
                            '        Bool_find = True
                            '    End If

                            'Next

                            'For i = 0 To Me.ListBox_file.Items.Count - 1
                            '    If CType(Me.ListBox_file.Items(i).Value, Integer) = dtRow("id_doc_soporte") Then
                            '        Bool_find = True
                            '    End If
                            'Next

                            If Not Bool_find Then
                                Bool_pendingDoc = True
                                str_pendingDoc = """" & dtRow("nombre_documento") & " """", "
                                i += 1
                            End If

                            Bool_find = False

                        End If

                    Next

                    If Bool_pendingDoc Then
                        If i > 1 Then
                            strPart1 = "s"
                            strPart2 = "are"
                        Else
                            strPart1 = ""
                            strPart2 = "is"
                        End If
                        str_pendingDoc = str_pendingDoc.Substring(0, str_pendingDoc.Trim.Length - 1)
                        lblerr_user.Text = String.Format("The document{0}: {1} {2} required. Please attached it before completed this approval proccess", strPart1, str_pendingDoc, strPart2)
                        Me.lblerr_user.Visible = True
                        Err = True
                    End If

                    '*****************SET DOCUMENT REQUIRED BY every approval**************************
                End If

                Dim Tool_code As String = clss_approval.get_ta_DocumentosInfoFIELDS("tool_code", "id_documento", Me.HiddenField1.Value)


                'vEsTatE = 1000

                If Err = False Then


                    Select Case vEsTatE


                        Case cAPPROVED '*****************DOCUMENTO APROBADO***************************
                            '****************************OBTENIENDO LA PROXIMA RUTA***********
                            clss_approval.get_ta_DocumentosINFO(Me.HiddenField1.Value)
                            Dim tbl_AppOrderO As New DataTable

                            'If came from StandBy State, We need to retur n to the ROL originator if is required**********************
                            If clss_approval.get_ta_DocumentosInfoFIELDS("id_estadoDoc", "id_documento", Me.HiddenField1.Value) = cSTANDby Then

                                tbl_AppOrderO = clss_approval.get_ta_AppDocumentoOrden_MAX(Me.HiddenField1.Value) ' To get the Max ORder values to make the same step again

                                If tbl_AppOrderO.Rows.Count > 0 Then

                                    '*******************************************check this one************************************************
                                    '****************Getting the values of the Max Order Again to return the approve**************************
                                    idRuta = tbl_AppOrderO.Rows(0).Item("id_ruta").ToString
                                    idNextRol = tbl_AppOrderO.Rows(0).Item("id_rol").ToString
                                    idNextUserID = tbl_AppOrderO.Rows(0).Item("id_usuario_app").ToString 'Return to de user that put the document in estand by status
                                    '*****************************Getting the new values of the Max Order Again*******************************

                                End If

                            End If


                            tbl_rutas_tipo_doc = clss_approval.get_Route_By_DocumentType(idRuta)

                            If tbl_rutas_tipo_doc.Rows.Count > 0 Then
                                duracion = tbl_rutas_tipo_doc.Rows(0).Item("duracion")
                            End If

                            Dim docState As Integer
                            If idRuta = -1 Then ' there is not more steps
                                docState = cCOMPLETED
                            Else
                                docState = cAPPROVED
                            End If

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(id_app_documento, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", docState, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            'clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", , "id_app_documento", clss_approval.id_App_Documento) 'Add the Actual Role it is necesary?

                            If clss_approval.save_ta_AppDocumento() <> -1 Then

                                'Save_NewFiles(CType(Me.lblIDocumento.Text, Integer), Tool_code)

                                SaveComment(id_app_documento, docState, Me.txtcoments.Text.Trim) '.Replace("'", "''")

                                If idRuta = -1 Then ' there is not more steps
                                    'If you have to do something, do here....

                                    clss_approval.set_ta_documento(Me.HiddenField1.Value)
                                    clss_approval.set_ta_documentoFIELDS("completo", "SI", "id_documento", clss_approval.id_Documento)

                                    If clss_approval.save_ta_documento() <> -1 Then

                                        '***************Ver la categoria y ver si aplica registro ambiental**********************
                                        If clss_approval.get_enviromentalDoc(Me.HiddenField1.Value) = 1 Then

                                            clss_approval.set_ta_documento_ambiental(0)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_documento", Me.HiddenField1.Value, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_estado", cPENDING, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("observacion", clss_approval.get_ta_DocumentosInfoFIELDS("descripcion_doc", "id_documento", Me.HiddenField1.Value), "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            'clss_approval.set_ta_documento_ambientalFIELDS("fecha_aprobado", Date.UtcNow, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_usuario_creo", Me.Session("E_IdUser"), "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("fecha_creado", Date.UtcNow, "id_documento_ambiental", clss_approval.id_documento_ambiental)
                                            clss_approval.set_ta_documento_ambientalFIELDS("id_tipoApp_Environmental", 0, "id_documento_ambiental", clss_approval.id_documento_ambiental)

                                            If clss_approval.save_ta_documento_ambiental() <> -1 Then
                                            End If

                                        End If

                                        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)


                                        'If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                        '    '**************  Estatus 3 *********** Approved***********************************************************
                                        '    Dim result = clss_approval.TimeSheet_Update_Status(3, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                        '    '**************  Estatus 3 *********** Approved***********************************************************

                                        '    If result <> -1 Then
                                        '        '*********************************OPEN****************************************
                                        '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                        '        If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                        '        Else 'Error mandando Email
                                        '        End If
                                        '        '*********************************OPEN****************************************
                                        '    End If

                                        'ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                        '    '**************  Estatus 3 *********** Approved***********************************************************
                                        '    Dim result = clss_approval.Deliverable_Update_Status(3, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                        '    '**************  Estatus 3 *********** Approved***********************************************************

                                        '    If result <> -1 Then

                                        '        '*********************************APPROVED****************************************
                                        '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                        '        If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                        '        Else 'Error mandando Email
                                        '        End If
                                        '        '*********************************APPROVED****************************************

                                        '    End If



                                        'Else 'No tool related to this approval

                                        '***************Ver la categoria y ver si aplica registro ambiental*****************************************
                                        '********************************COMPLETED DOCUMENT*********************************************************
                                        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                        If (objEmail.Emailing_APPROVAL_PAR(id_app_documento, id_par)) Then
                                        Else 'Error mandando Email
                                        End If

                                        'Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                        'If (objEmail.Emailing_APPROVAL_STEP(CType(id_app_documento, Integer))) Then
                                        'Else 'Error mandando Email
                                        'End If
                                        ' ********************************COMPLETED DOCUMENT*********************************************************

                                        'End If



                                    Else 'Error Saving Docs


                                    End If


                                Else 'Yes there is more steps


                                    Dim fecha_recep As DateTime = Date.UtcNow 'DateAdd(DateInterval.Hour, 8, Date.UtcNow)
                                    Dim fecha_limit As DateTime = calculaDiaHabil(duracion, fecha_recep)

                                    strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                                    clss_approval.set_ta_AppDocumento(0) 'New Record
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_documento", Me.HiddenField1.Value, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_recep, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cPENDING, "id_app_documento", 0) 'Pending Step
                                    clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_app_documento", 0) 'Pending Step .Replace("'", "''")
                                    'clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_recep, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", idNextUserID, "id_app_documento", 0)
                                    clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) 'Add the next Role

                                    Dim id_appdocumento = clss_approval.save_ta_AppDocumento()

                                    If id_appdocumento <> -1 Then
                                        '********************************************************************************
                                        '***********************COPIANDO LOS ARCHIVOS A LA NUEVA VERSION***************
                                        '*************************NEW version change, just we goint to save the files required by the user***************************
                                        'sql = "INSERT INTO ta_archivos_documento SELECT " & id_appdocumento & " as id_App_Documento, archivo, id_doc_soporte FROM ta_archivos_documento WHERE id_App_Documento= " & Me.lblIDocumento.Text
                                        'dm.SelectCommand.CommandText = sql
                                        'dm.SelectCommand.ExecuteNonQuery()
                                    Else 'Error Saving
                                    End If


                                    '' 2 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then

                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))
                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 2 'In Approved Process

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                    '        If result <> -1 Then
                                    '            '*********************************OPEN****************************************
                                    '            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '            Else 'Error mandando Email
                                    '            End If
                                    '            '*********************************OPEN****************************************
                                    '        End If

                                    '    End Using

                                    'Else

                                    '    '*********************************APPROVED NEXT STEP****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************APPROVED NEXT STEP****************************************

                                    'End If
                                    '' 2 - *******************************TOOLS TIME SHEET**********************************

                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    'If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                    '    '**************  Estatus 2 *********** Approved In Process ***********************************************************
                                    '    Dim result = clss_approval.TimeSheet_Update_Status(2, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                    '    '**************  Estatus 2 ***********  Approved In Process***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************NEXT STEP APP****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************NEXT STEP APP****************************************
                                    '    End If

                                    'ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                    '    '**************  Estatus 2 *********** Approval in Process ***********************************************************
                                    '    Dim result = clss_approval.Deliverable_Update_Status(2, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                    '    '**************  Estatus 2 *********** Approval in Process ***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************APPROVED****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************APPROVED****************************************

                                    '    End If



                                    'Else 'No tool related to this approval

                                    '*********************************APPROVED NEXT STEP****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_PAR(id_app_documento, id_par)) Then
                                    Else 'Error mandando Email
                                    End If

                                    '*********************************APPROVED NEXT STEP****************************************

                                    'End If




                                End If

                            Else 'Error saving the estep



                            End If 'clss_approval.save_ta_AppDocumento()

                 '****************************************END APPROVED***************************************************

           '****************NO APROBADO ahí se termina el proceso**************************
           '**************************ACTUALIZAR EL REGISTRO QUE SE ESTA EDITANDO***********

                        Case cnotAPPROVED

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(id_app_documento, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cnotAPPROVED, "id_App_documento", clss_approval.id_App_Documento)

                            If clss_approval.save_ta_AppDocumento() <> -1 Then

                                clss_approval.set_ta_documento(Me.HiddenField1.Value)
                                clss_approval.set_ta_documentoFIELDS("completo", "SI", "id_documento", clss_approval.id_Documento)

                                If clss_approval.save_ta_documento() <> -1 Then

                                    SaveComment(id_app_documento, cnotAPPROVED, Me.txtcoments.Text.Trim) '.Replace("'", "''")

                                    'Save_NewFiles(CType(Me.lblIDocumento.Text, Integer), Tool_code)


                                    ' 3 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then

                                    '    'Update Here the Time Sheet Status
                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 4 ' Not Approved

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                    '        If result <> -1 Then

                                    '            '*********************************OPEN****************************************
                                    '            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '            Else 'Error mandando Email
                                    '            End If
                                    '            '*********************************OPEN****************************************

                                    '        End If

                                    '    End Using

                                    'Else

                                    '    '*********************************NOT APPROVED ****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************NOT APPROVED ****************************************
                                    'End If
                                    ' 3 - *******************************TOOLS TIME SHEET**********************************

                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                        '**************  Estatus 4 *********** NOT Approved***********************************************************
                                        Dim result = clss_approval.TimeSheet_Update_Status(4, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                        '**************  Estatus 4 *********** NOT Approved***********************************************************

                                        If result <> -1 Then
                                            '*********************************NEXT STEP APP****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(id_app_documento, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************NEXT STEP APP****************************************
                                        End If

                                    ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                        '**************  Estatus 4 *********** Not Approved***********************************************************
                                        Dim result = clss_approval.Deliverable_Update_Status(4, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                        '**************  Estatus 4 *********** Not Approved***********************************************************

                                        If result <> -1 Then
                                            '*********************************APPROVED****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                            If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(id_app_documento, Integer))) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************APPROVED****************************************

                                        End If

                                    ElseIf Tool_code = "PAR-RMS01" Then '--Deliverable Tools

                                        '**************  Estatus 4 *********** Not Approved***********************************************************
                                        Dim result = clss_approval.Deliverable_Update_Status(4, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                        '**************  Estatus 4 *********** Not Approved***********************************************************

                                        If result <> -1 Then
                                            '*********************************APPROVED****************************************
                                            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                            If (objEmail.Emailing_APPROVAL_PAR(id_app_documento, id_par)) Then
                                            Else 'Error mandando Email
                                            End If
                                            '*********************************APPROVED****************************************

                                        End If

                                    Else 'No tool related to this approval

                                        '*********************************APPROVED NEXT STEP****************************************
                                        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                        If (objEmail.Emailing_APPROVAL_PAR(id_app_documento, id_par)) Then
                                        Else 'Error mandando Email
                                        End If
                                        '*********************************APPROVED NEXT STEP****************************************

                                    End If


                                Else 'Error

                                End If


                            Else 'Error happened
                            End If

                        '****************************************FIN***************************************************

                         '********************CANCELADO FIN DEL DOCUMENTO (ACTUALIZAR campo complet a SI EN ta_documentos )
                         '****************************ACTUALIZAR EL REGISTRO QUE SE ESTA EDITANDO***********
                        Case cCANCELLED

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(id_app_documento, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cCANCELLED, "id_App_documento", clss_approval.id_App_Documento)

                            If clss_approval.save_ta_AppDocumento() <> -1 Then

                                clss_approval.set_ta_documento(Me.HiddenField1.Value)
                                clss_approval.set_ta_documentoFIELDS("completo", "SI", "id_documento", clss_approval.id_Documento)

                                If clss_approval.save_ta_documento() <> -1 Then

                                    SaveComment(id_app_documento, cCANCELLED, Me.txtcoments.Text.Trim) '.Replace("'", "''")
                                    'Save_NewFiles(CType(id_app_documento, Integer), Tool_code)


                                    '' 4 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then
                                    '    'Update Here the Time Sheet Status

                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 4 ' Not Approved

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))

                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))

                                    '        If result <> -1 Then

                                    '            '*********************************OPEN****************************************
                                    '            Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '            If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '            Else 'Error mandando Email
                                    '            End If
                                    '            '*********************************OPEN****************************************

                                    '        End If

                                    '    End Using

                                    'Else

                                    '    '*********************************CANCELLED ****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************CANCELLED****************************************
                                    'End If

                                    '' 4 - *******************************TOOLS TIME SHEET**********************************


                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    'If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                    '    '**************  Estatus 6 *********** Cancelled  ***********************************************************
                                    '    Dim result = clss_approval.TimeSheet_Update_Status(6, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                    '    '**************  Estatus 6 ***********  Cancelled ***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************NEXT STEP APP****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    '        If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(id_app_documento, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************NEXT STEP APP****************************************
                                    '    End If

                                    'ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                    '    '**************  Estatus 6 *********** Cancelled ***********************************************************
                                    '    Dim result = clss_approval.Deliverable_Update_Status(6, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                    '    '**************  Estatus 6 *********** Cancelled ***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************APPROVED****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************APPROVED****************************************

                                    '    End If



                                    'Else 'No tool related to this approval

                                    '*********************************APPROVED NEXT STEP****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_PAR(id_app_documento, id_par)) Then
                                    Else 'Error mandando Email
                                    End If
                                    '*********************************APPROVED NEXT STEP****************************************

                                    'End If


                                Else 'Error
                                End If


                            Else  'Error
                            End If


                        Case cSTANDby
                            '*********************************************************************************

                            strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                            clss_approval.set_ta_AppDocumento(CType(id_app_documento, Integer))
                            clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", Date.UtcNow, "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", Me.Session("E_IdUser"), "id_App_documento", clss_approval.id_App_Documento)
                            clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_App_documento", clss_approval.id_App_Documento) '.Replace("'", "''")
                            clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cSTANDby, "id_App_documento", clss_approval.id_App_Documento)

                            If clss_approval.save_ta_AppDocumento() <> -1 Then


                                SaveComment(id_app_documento, cSTANDby, Me.txtcoments.Text.Trim) '.Replace("'", "''")
                                'Save_NewFiles(CType(id_app_documento, Integer), Tool_code)


                                Dim tbl_AppOrderO As New DataTable
                                'tbl_AppOrderO = clss_approval.get_ta_AppDocumento_byOrden(Me.HiddenField1.Value, 0) ' To get the Rol originator Problem when repeat
                                tbl_AppOrderO = clss_approval.get_ta_AppDocumentoOrden_MIN(Me.HiddenField1.Value) 'To get the info on the min step
                                'To Create a New APP to the originator in Stand by state

                                '****************Getting the new values of the originator**************************
                                idRuta = tbl_AppOrderO.Rows(0).Item("id_ruta").ToString
                                idNextRol = tbl_AppOrderO.Rows(0).Item("id_rol").ToString
                                idNextUserID = tbl_AppOrderO.Rows(0).Item("id_usuario_app").ToString 'The user who applied as originator from this Approval procc
                                '****************Getting the new values of the originator**************************

                                tbl_rutas_tipo_doc = clss_approval.get_Route_By_DocumentType(idRuta)

                                If tbl_rutas_tipo_doc.Rows.Count > 0 Then
                                    duracion = tbl_rutas_tipo_doc.Rows(0).Item("duracion")
                                End If

                                Dim fecha_recep As DateTime = Date.UtcNow 'DateAdd(DateInterval.Hour, 8, Date.UtcNow)
                                Dim fecha_limit As DateTime = calculaDiaHabil(duracion, fecha_recep)

                                strComment = If(Me.txtcoments.Text.Trim.Length = 0, "--no comments--", Me.txtcoments.Text.Trim)

                                clss_approval.set_ta_AppDocumento(0) 'New Record in stanb By
                                clss_approval.set_ta_AppDocumentoFIELDS("id_documento", Me.HiddenField1.Value, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("id_ruta", idRuta, "id_app_documento", 0) 'IdRutaOriginator
                                clss_approval.set_ta_AppDocumentoFIELDS("fecha_limite", fecha_limit, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("fecha_recepcion", fecha_recep, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("id_estadoDoc", cSTANDby, "id_app_documento", 0) 'Pending Step
                                clss_approval.set_ta_AppDocumentoFIELDS("observacion", strComment, "id_app_documento", 0) 'Pending Step '.Replace("'", "''")
                                'clss_approval.set_ta_AppDocumentoFIELDS("fecha_aprobacion", fecha_recep, "id_app_documento", 0)
                                clss_approval.set_ta_AppDocumentoFIELDS("id_usuario_app", idNextUserID, "id_app_documento", 0) 'IdUSerORiginator
                                clss_approval.set_ta_AppDocumentoFIELDS("id_role_app", idNextRol, "id_app_documento", 0) ' idrolORiginator

                                Dim id_appdocumento = clss_approval.save_ta_AppDocumento()

                                If id_appdocumento <> -1 Then

                                    '' 5 - *******************************TOOLS TIME SHEET**********************************
                                    'If clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then


                                    '    'Update Here the Time Sheet Status
                                    '    Using db As New dbRMS_JIEntities

                                    '        Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)
                                    '        Dim idTS As Integer = db.ta_documento_timesheets.Where(Function(p) p.id_documento = idDoc).FirstOrDefault().id_timesheet

                                    '        Dim TimeSheet As New ta_timesheet

                                    '        TimeSheet = db.ta_timesheet.Find(Convert.ToInt32(idTS))

                                    '        TimeSheet.fecha_upd = Date.UtcNow
                                    '        TimeSheet.usuario_upd = Convert.ToInt32(Me.Session("E_IdUser"))
                                    '        TimeSheet.id_timesheet_estado = 5 'Observation Pending

                                    '        Dim cls_TimeSheet As APPROVAL.clss_TimeSheet = New APPROVAL.clss_TimeSheet(Me.Session("E_IDPrograma"))
                                    '        Dim result = cls_TimeSheet.SaveTimeSheet(TimeSheet, Convert.ToInt32(idTS))


                                    '        'Update Here the Time Sheet Status
                                    '        '*********************************OPEN****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************OPEN****************************************

                                    '    End Using


                                    'Else

                                    '    '*********************************STAND BY****************************************
                                    '    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 5, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '    If (objEmail.Emailing_APPROVAL_STEP(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '    Else 'Error mandando Email
                                    '    End If
                                    '    '*********************************STAND BY****************************************

                                    'End If


                                    '' 5 - *******************************TOOLS TIME SHEET**********************************

                                    Dim idDoc As Integer = Convert.ToInt32(Me.HiddenField1.Value)

                                    'If Tool_code = "TM-SHEET01" And clss_approval.get_TimeSheetDoc(Me.HiddenField1.Value) > 0 Then '--Time Tools

                                    '    '**************  Estatus 5 *********** 'Observation Pending ***********************************************************
                                    '    Dim result = clss_approval.TimeSheet_Update_Status(5, idDoc, Convert.ToInt32(Me.Session("E_IdUser")))
                                    '    '**************  Estatus 5 *********** 'Observation Pending ***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************NEXT STEP APP****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 10, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_TIME_SHEET_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************NEXT STEP APP****************************************
                                    '    End If

                                    'ElseIf Tool_code = "DELIV-RMS01" Then '--Deliverable Tools

                                    '    '**************  Estatus 5 *********** 'Observation Pending ***********************************************************
                                    '    Dim result = clss_approval.Deliverable_Update_Status(5, idDoc, Convert.ToInt32(Me.Session("E_IdUser")), idRoute)
                                    '    '**************  Estatus 5 *********** 'Observation Pending ***********************************************************

                                    '    If result <> -1 Then
                                    '        '*********************************APPROVED****************************************
                                    '        Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), idDoc, 1009, cl_user.regionalizacionCulture, CType(Me.lblIDocumento.Text, Integer))
                                    '        If (objEmail.Emailing_DELIVERABLE_APPROVAL(CType(Me.lblIDocumento.Text, Integer))) Then
                                    '        Else 'Error mandando Email
                                    '        End If
                                    '        '*********************************APPROVED****************************************

                                    '    End If



                                    'Else 'No tool related to this approval

                                    '*********************************APPROVED NEXT STEP****************************************
                                    Dim objEmail As New APPROVAL.cls_notification(Me.Session("E_IDPrograma"), Me.HiddenField1.Value, 1023, cl_user.regionalizacionCulture, CType(id_app_documento, Integer))
                                    If (objEmail.Emailing_APPROVAL_PAR(id_app_documento, id_par)) Then
                                    Else 'Error mandando Email
                                    End If
                                    '*********************************APPROVED NEXT STEP****************************************

                                    'End If

                                End If

                            Else 'Error

                            End If


                    End Select


                    Me.Response.Redirect("~/administrativo/frm_par")


                End If

            Catch ex As Exception
                lblerr_user.Text = String.Format("An error was found in the action: {0} ", ex.Message)
                Me.lblerr_user.Visible = True
                Me.btn_Approved.Enabled = False
                Me.btn_NotApproved.Enabled = False
                Me.btn_Cancelled.Enabled = False
                Me.btn_STandBy.Enabled = False
                Me.btn_Completed.Enabled = False

            End Try
        End Using

    End Sub
    Public Sub SaveComment(ByVal idApp As Integer, ByVal idEstadoDoc As Integer, ByVal Comment As String)

        Dim strComment As String
        If Trim(Comment).Length = 0 Then
            strComment = "--No Comments--"
        Else
            strComment = Comment
        End If

        clss_approval.set_ta_comentariosDoc(0) 'New Record
        clss_approval.set_ta_comentariosDocFIELDS("id_App_Documento", idApp, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("id_estadoDoc", idEstadoDoc, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("id_tipoAccion", cAction_ByProcess, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("id_usuario", Me.Session("E_IdUser"), "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("fecha_comentario", Date.UtcNow, "id_comment", 0)
        clss_approval.set_ta_comentariosDocFIELDS("comentario", strComment.Trim.Trim, "id_comment", 0) '.Replace("  ", "")

        If clss_approval.save_ta_comentariosDoc() = -1 Then
            'Error do somenthing

        End If


    End Sub
End Class