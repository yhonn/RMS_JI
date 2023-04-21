Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL
Imports ly_SIME
Imports System.Web.Services
Imports ly_RMS
Imports System.Globalization
Imports System.Web.Script.Serialization


Public Class frm_Deliverable_minutePrintG
    Inherits System.Web.UI.Page

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_DELIV_MIN_PRINT"

    Dim controles As New ly_SIME.CORE.cls_controles
    Dim clss_approval As APPROVAL.clss_approval
    Dim cls_Deliverable As APPROVAL.clss_Deliverable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Me.Session("E_IdUser").ToString = "" Then
            End If
        Catch ex As Exception
            Me.Response.Redirect("~/frm_login.aspx")
        End Try

        If HttpContext.Current.Session.Item("clUser") IsNot Nothing Then
            cl_user = Session.Item("clUser")
            If Not cl_user.chk_accessMOD(0, frmCODE) Then
                Me.Response.Redirect("~/Proyectos/no_access2")
            Else
                cl_user.chk_Rights(Page.Controls, 4, frmCODE, 0)
                ' cl_user.chk_Rights(Page.Controls, 2, frmCODE, 0, grd_cate)
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            If Not IsNothing(Me.Request.QueryString("ID")) Then
                Me.hd_id_deliverable.Value = Convert.ToInt32(Me.Request.QueryString("ID"))
            Else
                Me.hd_id_deliverable.Value = 0
            End If

            LoadData(hd_id_deliverable.Value)

        End If

    End Sub

    Public Sub LoadData(ByVal idDeliverable As Integer)

        Using dbEntities As New dbRMS_JIEntities


            Dim id_programa As Integer = CType(Me.Session("E_IDPrograma"), Integer)
            cls_Deliverable = New APPROVAL.clss_Deliverable(Me.Session("E_IDPrograma"))
            clss_approval = New APPROVAL.clss_approval(Me.Session("E_IDPrograma"))
            Dim sesUser As ly_SIME.CORE.cls_user = CType(Session.Item("clUser"), ly_SIME.CORE.cls_user)


            Dim Tbl_deliverable As DataTable = cls_Deliverable.get_Deliverables(idDeliverable)

            Dim idFichaProyecto As Integer = Convert.ToInt32(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"))
            Dim oFichaProyecto = dbEntities.vw_tme_ficha_proyecto.Where(Function(p) p.id_ficha_proyecto = idFichaProyecto).FirstOrDefault()
            Dim oEjecutores = dbEntities.t_ejecutores.Where(Function(p) p.id_ejecutor = oFichaProyecto.id_ejecutor).FirstOrDefault()
            Dim oUsuario_Responsable = dbEntities.vw_t_usuarios.Where(Function(p) p.id_programa = id_programa And p.id_usuario = oFichaProyecto.id_usuario_responsable).FirstOrDefault()

            Me.hd_id_deliverable_minute.Value = Tbl_deliverable.Rows.Item(0).Item("id_deliverable_minute")

            Me.lbl_Beneficiario.Text = oFichaProyecto.nombre_ejecutor
            'Me.lbl_activity_code.Text = String.Format("{0} No.", oFichaProyecto.nombre_mecanismo_contratacion)
            Me.lbl_activity.Text = oFichaProyecto.codigo_ficha_AID
            Me.lbl_fecha.Text = String.Format("{0:dd/MM/yyyy}", Tbl_deliverable.Rows.Item(0).Item("fecha_entrego"))
            Me.lbl_fecha_entrega.Text = String.Format("{0:dd/MM/yyyy}", Tbl_deliverable.Rows.Item(0).Item("fecha"))
            Me.lbl_No.Text = Tbl_deliverable.Rows.Item(0).Item("numero_entregable")
            Me.lbl_descripcion_entregable.Text = Tbl_deliverable.Rows.Item(0).Item("descripcion_entregable")
            Me.lbl_medio_verificacion.Text = Tbl_deliverable.Rows.Item(0).Item("verification_mile")
            Me.lbl_percen.Text = Tbl_deliverable.Rows.Item(0).Item("porcentaje")
            Me.lbl_activity_name.Text = Tbl_deliverable.Rows.Item(0).Item("nombre_proyecto")

            Dim idM As Integer = Convert.ToInt32(Me.hd_id_deliverable_minute.Value)
            Dim oMinute As ta_deliverable_minute = dbEntities.ta_deliverable_minute.Find(idM)

            Dim Delive_Value As Double = If(oMinute.local_currency = True, Tbl_deliverable.Rows.Item(0).Item("valor_final"), (Tbl_deliverable.Rows.Item(0).Item("valor_final") / Tbl_deliverable.Rows.Item(0).Item("tasa_cambio_final")))
            Dim strCurrSymbol As String = If(oMinute.local_currency = True, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol, "USD")

            Me.currency_info.InnerHtml = strCurrSymbol
            Me.lbl_valor_PB.Text = String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", Delive_Value, strCurrSymbol)

            Me.lbl_nombre_rep.Text = oEjecutores.representante_legal
            Me.lbl_fecha_rep.Text = getFecha(Tbl_deliverable.Rows.Item(0).Item("fecha_entrego"), "f", True)

            'Me.lbl_otr_name.Text = String.Format("{0} ({1})", oUsuario_Responsable.nombre_usuario, oUsuario_Responsable.job)
            'Me.lblt_product_name.Text = String.Format("Producto {0}. {1} <br /><br />", Tbl_deliverable.Rows.Item(0).Item("numero_entregable"), Tbl_deliverable.Rows.Item(0).Item("descripcion_entregable"))
            'Me.product_descrip.InnerHtml = Tbl_deliverable.Rows.Item(0).Item("verification_mile").ToString.Replace(vbLf, "<br />").Replace(vbCrLf, "<br />")


            Me.lbl_supervisor_nombre.Text = oUsuario_Responsable.nombre_usuario
            Me.lbl_supervisor_fecha.Text = getFecha(Tbl_deliverable.Rows.Item(0).Item("fecha_entrego"), "f", True)

            Dim tbl_result As DataTable = cls_Deliverable.Deliv_Document(idDeliverable)
            Me.hd_id_documento.Value = tbl_result.Rows.Item(0).Item("id_documento")

            Dim tbl_route As DataTable = clss_approval.get_ta_RutaSeguimiento(Me.hd_id_documento.Value)

            '--****************************************** COP*************************
            For Each dtRow As DataRow In tbl_route.Rows

                If dtRow("nombre_rol") = "DCOP" Or dtRow("nombre_rol") = "COP" Then

                    Dim tbl_app_doc As DataTable = clss_approval.get_vw_ta_AppDocumento(0, dtRow("id_App_Documento"))
                    Me.lbl_COP_name.Text = tbl_app_doc.Rows.Item(0).Item("nombre_usuario")
                    Me.lbl_COP_Date.Text = getFecha(tbl_app_doc.Rows.Item(0).Item("fecha_aprobacion"), "f", True)
                    Exit For

                End If

            Next
            '--****************************************** COP*************************

            '--******************************************M&E*************************
            For Each dtRow As DataRow In tbl_route.Rows

                If dtRow("nombre_rol") = "M&E_MANAG" Then

                    Dim tbl_app_doc As DataTable = clss_approval.get_vw_ta_AppDocumento(0, dtRow("id_App_Documento"))
                    Me.lbl_ME_name.Text = tbl_app_doc.Rows.Item(0).Item("nombre_usuario")
                    Me.lbl_ME_Date.Text = getFecha(tbl_app_doc.Rows.Item(0).Item("fecha_aprobacion"), "f", True)
                    Exit For

                End If

            Next
            '--******************************************M&E*************************

            '--******************************************GRANT*************************
            For Each dtRow As DataRow In tbl_route.Rows

                If dtRow("nombre_rol") = "GRANT_MANAG" Then

                    Dim tbl_app_doc As DataTable = clss_approval.get_vw_ta_AppDocumento(0, dtRow("id_App_Documento"))
                    Me.lbl_GRANT_name.Text = tbl_app_doc.Rows.Item(0).Item("nombre_usuario")
                    Me.lbl_GRANT_Date.Text = getFecha(tbl_app_doc.Rows.Item(0).Item("fecha_aprobacion"), "f", True)
                    Exit For

                End If

            Next
            '--******************************************GRANT*************************

            Me.lbl_concepto_pago.Text = String.Format("Concepto: Pago del hito No. {0} del convenio {1}", Tbl_deliverable.Rows.Item(0).Item("numero_entregable"), oFichaProyecto.codigo_ficha_AID)

            Dim strArray As String() = GetDeliverable_(Tbl_deliverable.Rows.Item(0).Item("id_ficha_proyecto"), idDeliverable).Split("||")

            'Dim Delive_Value As Double = If(oMinute.local_currency = True, Tbl_deliverable.Rows.Item(0).Item("valor_final"), (Tbl_deliverable.Rows.Item(0).Item("valor_final") / Tbl_deliverable.Rows.Item(0).Item("tasa_cambio_final")))
            'Dim strCurrSymbol As String = If(oMinute.local_currency = True, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol, "USD")

            Me.lbl_total_usd.Text = String.Format("{0:N2} {1}", If(oMinute.local_currency = True, oFichaProyecto.c_Aportes, (oFichaProyecto.c_Aportes / oFichaProyecto.tasa_cambio_actividad)), strCurrSymbol)

            lbl_totalACT_usd.Text = If(oMinute.local_currency = True, strArray(4), strArray(2))
            'lbl_totalACT.Text = strArray(4)
            lbl_totalPend_usd.Text = If(oMinute.local_currency = True, strArray(8), strArray(6))
            'lbl_totalPend.Text = strArray(8)
            lbl_totalPerf_usd.Text = If(oMinute.local_currency = True, strArray(12), strArray(10))
            'lbl_totalPerf.Text = strArray(12)

            Me.lbl_org.Text = Tbl_deliverable.Rows.Item(0).Item("Implementer")

            If oMinute.minute_close = True Then
                Me.lbl_acta_no.Text = oMinute.minute_code
            Else
                Me.lbl_acta_no.Text = "--------"
            End If

            Dim oClin = dbEntities.ta_clin_codes.Where(Function(p) p.id_clin_code = oMinute.id_clin_code).FirstOrDefault()

            'Me.lbl_CLIN.Text = String.Format("CLIN {0}", oClin.clin_code)
            'Me.lbl_GL.Text = String.Format("GL {0}", oClin.GL_code)
            'Dim municipios = dbEntities.t_municipios.Where(Function(p) p.id_municipio = oMinute.id_municipio).FirstOrDefault
            'Me.lbl_municipio.Text = municipios.nombre_municipio
            'Me.lbl_comment.Text = oMinute.minute_comment  '.ToString.Replace(Environment.NewLine, "<br />")
            'Me.lbl_soporte_ubicacion.Text = dbEntities.ta_offices.Where(Function(p) p.id_office = oMinute.id_office).FirstOrDefault.office_name
            'Me.lbl_beneficiario_pago.Text = oEjecutores.nombre_ejecutor
            'Me.div_bank.InnerHtml = If(oEjecutores.billing_info Is Nothing, "", oEjecutores.billing_info.ToString.Replace(vbLf, "<br />").Replace(vbCrLf, "<br />"))
            'Me.lbl_identificacion_trib.Text = oEjecutores.nit
            'Dim tbl_App As DataTable = clss_approval.get_DocumentINFO(Convert.ToInt32(Me.hd_id_documento.Value))
            'Me.lbl_autorizacion_comment.Text = String.Format("Por medio del presente documento los abajo participantes del proceso de aprobación {0}, autorizan el pago de los productos mencionados anteriormente.  <br /><br />", tbl_App.Rows.Item(0).Item("numero_instrumento"))
            'Dim strRows As String = get_ApprovedTable()
            'Me.App_users.InnerHtml = String.Format("<table class=""table table-bordered table-responsive"">  {0} </table>", strRows)

        End Using

    End Sub


    Function get_ApprovedTable() As String

        Using dbEntities As New dbRMS_JIEntities

            Dim strROWS As String = ""

            Dim tbl_route As DataTable = clss_approval.get_ta_RutaSeguimiento(Me.hd_id_documento.Value)
            Dim idMin = Convert.ToInt32(Me.hd_id_deliverable_minute.Value)
            Dim dtTipoAPP As DataTable = cls_Deliverable.get_ApprovalEstado_tipo()
            Dim id_programa As Integer = CType(Me.Session("E_IDPrograma"), Integer)

            Dim strROW As String = "<tr> {0} </tr>"
            Dim strCOL As String = ""
            Dim i As Integer = 0

            For Each dtRow As DataRow In tbl_route.Rows

                Dim idApp = Convert.ToInt32(dtRow("id_app_documento"))

                If (dbEntities.ta_deliverable_minute_app.Where(Function(p) p.id_deliverable_minute = idMin And p.id_App_documento = idApp).Count() > 0) Then

                    Dim tbl_app_doc As DataTable = clss_approval.get_vw_ta_AppDocumento(0, idApp)

                    'Dim id_usuario = tbl_app_doc.Rows.Item(0).Item("id_usuario_app")
                    'Dim idRuta = dtRow("id_ruta")
                    'Dim idEstadoTipo = dbEntities.ta_rutaTipoDoc.Where(Function(p) p.id_ruta = idRuta).FirstOrDefault.id_estadoTipo
                    'Dim oUsuario_Responsable = dbEntities.vw_t_usuarios.Where(Function(p) p.id_programa = id_programa And p.id_usuario = id_usuario).FirstOrDefault()
                    'tbl_app_doc.Rows.Item(0).Item("fecha_aprobacion")

                    strCOL &= String.Format("<td>
                                                <span style=""font-weight:600;"" class= ""text-left""> {0} </span><br />
                                                <span style=""font-size:medium;"">{1} {2} PARAMOS Y BOSQUES </span><br />
                                                <span style=""font-size:medium;"">Fecha {3} {4}</span><br />
                                            </td>", tbl_app_doc.Rows.Item(0).Item("nombre_usuario"), tbl_app_doc.Rows.Item(0).Item("estado_tipo_prefijo"), tbl_app_doc.Rows.Item(0).Item("job"), tbl_app_doc.Rows.Item(0).Item("estado_tipo_prefijo"), getFecha(tbl_app_doc.Rows.Item(0).Item("fecha_aprobacion"), "f", True))


                    i += 1
                    If i = 2 Then
                        strROWS &= String.Format(strROW, strCOL)
                        strCOL = ""
                        i = 0
                    End If

                End If

            Next

            If i = 1 Then
                strCOL &= "<td>&nbsp;</td>"
                strROWS &= String.Format(strROW, strCOL)
                strCOL = ""
                i = 0
            End If

            get_ApprovedTable = strROWS

        End Using

    End Function


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

    Private Function GetDeliverable_(ByVal idFichaProyecto As Integer, Optional id_deliverable As Integer = 0) As String

        Dim cls_Deliverable = New APPROVAL.clss_Deliverable(Convert.ToInt32(Me.Session("E_IDPrograma")))

        Dim contextVar As HttpContext = HttpContext.Current
        Dim sesUser As ly_SIME.CORE.cls_user = CType(contextVar.Session.Item("clUser"), ly_SIME.CORE.cls_user)
        Dim ExchangeRate As Double = cls_Deliverable.get_ExchangeRate()

        Dim tbl_Deliverables As DataTable = cls_Deliverable.get_Deliverable_Activity(idFichaProyecto, 0)

        Dim strRowsTOT As String = ""
        Dim strRows As String = "<tr>
                                   <td  rowspan='4'><div class='tools'><a href='/RMS_APPROVAL/Deliverable/frm_DeliverableFollowingRep.aspx?ID={10}' target='_blank' ><i class='fa fa-search' ></i></a></div>  </td>
                                   <td rowspan='4'>{0}</td>
                                   <td rowspan='4'>
                                      <div style='overflow-y:auto; text-align:left; max-width:100%; max-height:300px;'>
                                          {1}
                                      </div>
                                   </td>
                                   <td rowspan='4'>
                                      <div style='overflow-y:auto; text-align:left; max-width:100%; max-height:300px;'>
                                         {2}
                                      </div>
                                   </td>
                                   <td>Due Date</td>
                                   <td>{3:d}</td>
                                   <td rowspan='4'>{5:P2}</td>
                                   <td rowspan='4'>{6}</td>
                                   <td rowspan='4'>                                                                       
                                     <span class='label {9}'>{7}&nbsp;<i class='fa fa-clock-o'></i>&nbsp;{8}</span>
                                   </td>
                                </tr>"

        Dim strRowsFourth As String = "<tr>
                                       <td>Delivered Date</td>
                                       <td>{0:d}</td>
                                      </tr><tr>
                                       <td>Approved Date</td>
                                       <td>{1:d}</td>
                                      </tr><tr>
                                       <td>Disbursed Date</td>
                                       <td>{2:d}</td>
                                      </tr>"


        Dim strRows2 As String = " <tr>
                                       <td colspan='9'>                                                                     
                                          <div class='progress'>
                                              <div class='progress-bar {0} progress-bar-striped' role='progressbar' aria-valuenow='{1}' aria-valuemin='0' aria-valuemax='100' style='width: {1}%'>
                                                   <span >{1}% </span>
                                              </div> 
                                              <div class='progress-bar {2} progress-bar-striped' role='progressbar' aria-valuenow='{3}' aria-valuemin='0' aria-valuemax='100' style='width: {3}%'>
                                                    <span>{4}% </span>
                                              </div>                                                                            
                                          </div>
                                        </td>  
                                    </tr> "

        Dim strTableDEL As String = "<table class='table table-hover'>
                                                                <tr>
                                                                  <th>Deliverable #</th>
                                                                  <td><span class='badge bg-primary'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{0}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span></td>                                                                 
                                                                </tr>
                                                                <tr>                                                                  
                                                                  <td colspan='2'>
                                                                     <div style='text-align:left; max-width:100%;'>
                                                                         {1}
                                                                     </div>
                                                                   </td>                                                                  
                                                                </tr>
                                                                <tr>
                                                                  <th>Due Date</th>
                                                                  <td>{2:d}</td>   
                                                                </tr>     
                                                                <tr>
                                                                  <th>Status</th>
                                                                  <td><span class='label {5}'>{3}&nbsp;<i class='fa fa-clock-o'></i>&nbsp;{4}</span></td>   
                                                                </tr>  
                                                                 <tr>
                                                                  <th>Porcent</th>
                                                                  <td>{6:P2}</td>   
                                                                </tr>   
                                                                <tr>
                                                                  <th>Amount</th>
                                                                  <td> {7} / {8:N2} USD</td>   
                                                                </tr>                                                               
                                                              </table>"

        'Dim strTable_nextDEL As String = ""
        Dim id_ficha_entregable As Integer = 0

        Dim strStatus As String = ""
        Dim strTime As String = ""
        Dim strAlert As String = ""

        Dim strAlert2 As String = ""
        Dim strAlert3 As String = ""
        Dim vDiferences As Double = 0
        Dim vDiferences_Adj As Double = 0

        Dim vDays As Double = 0

        Dim YLAfundingUSD As Double = 0
        Dim PerformedFundingUSD As Double = 0
        Dim PendingFundingUSD As Double = 0

        Dim YLAfunding As Double = 0
        Dim PerformedFunding As Double = 0
        Dim PendingFunding As Double = 0
        Dim PorcenPerformed As Double = 0

        'Dim IdMaxDeliverable As Integer = cls_Deliverable.get_Last_Deliverable(idFichaProyecto, 1).Rows.Item(0).Item("id_deliverable")

        For Each dtRow As DataRow In tbl_Deliverables.Rows

            Dim rDays As Double

            YLAfunding = dtRow("monto_aporte")

            'If (dtRow("id_deliverable_estado") >= 0 And dtRow("id_deliverable_estado") <= 2) Then
            '    PendingFunding += dtRow("valor")
            'ElseIf dtRow("id_deliverable_estado") = 3 Then
            '    PerformedFunding += dtRow("valor")
            'End If

            If dtRow("id_deliverable_estado") = 3 Then
                PerformedFunding += dtRow("valor")
                PerformedFundingUSD += (dtRow("valor") / dtRow("tasa_cambio_final"))
            Else
                PendingFunding += dtRow("valor") 'Take in account when the value allocated it is not the planned
                PendingFundingUSD += (dtRow("valor") / dtRow("tasa_Cambio"))
            End If



            If dtRow("D_Days") <= 0 Then 'its not time 

                rDays = dtRow("D_Days") * -1

                If dtRow("id_deliverable_estado") = 0 Then 'Pending status

                    strTime = Func_Unit(rDays)
                    strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)
                    strStatus = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 2)

                Else 'finish processes

                    strStatus = dtRow("deliverable_estado")
                    strTime = Func_Unit(rDays)
                    strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)

                End If


                vDiferences = Math.Round((dtRow("porc_Days") - dtRow("porc_EDays")) * 100, 2, MidpointRounding.AwayFromZero)
                vDays = (dtRow("porc_Days") * 100) - vDiferences
                'vDiferences_Adj = If(vDays + vDiferences > 100, (vDiferences - ((vDays + vDiferences) - 100)), vDiferences)
                vDiferences_Adj = vDiferences

            Else 'Delayed time

                If dtRow("id_deliverable_estado") = 0 Then 'Pending status

                    strTime = Func_Unit(dtRow("D_Days"))
                    strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)
                    strStatus = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 2)

                Else 'finish processes

                    strStatus = dtRow("deliverable_estado")
                    strTime = Func_Unit(dtRow("D_Days"))
                    strAlert = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 1)

                End If

                vDays = dtRow("porc_Days") * 100
                vDiferences = Math.Round((dtRow("porc_EDays") - dtRow("porc_Days")) * 100, 2, MidpointRounding.AwayFromZero)
                vDiferences_Adj = If(vDays + vDiferences > 100, (vDiferences - ((vDays + vDiferences) - 100)), vDiferences)

            End If

            strAlert2 = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 3)
            strAlert3 = Func_Alert(dtRow("porc_Days"), dtRow("porc_EDays"), 4)

            'strRowsTOT &= String.Format(strRows, dtRow("numero_entregable"), dtRow("descripcion_entregable"), dtRow("verification_mile"), dtRow("fecha"), dtRow("delivered_date"), (dtRow("porcentaje") / 100), String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", dtRow("valor"), sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol), strStatus, strTime, strAlert, dtRow("id_deliverable"))
            strRowsTOT &= String.Format(strRows, dtRow("numero_entregable"), dtRow("descripcion_entregable"), dtRow("verification_mile"), dtRow("fecha"), dtRow("delivered_date"), (dtRow("porcentaje") / 100), String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", dtRow("valor"), sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol), strStatus, strTime, strAlert, dtRow("id_deliverable"))
            strRowsTOT &= String.Format(strRowsFourth, If(IsDBNull(dtRow("fecha_entrego")), "--", dtRow("fecha_entrego")), If(IsDBNull(dtRow("fecha_aprobo")), "--", dtRow("fecha_aprobo")), If(IsDBNull(dtRow("delivered_date")), "--", dtRow("delivered_date")))

            strRowsTOT &= String.Format(strRows2, strAlert2, vDays, strAlert3, vDiferences_Adj, vDiferences)


            If dtRow("id_deliverable_estado") = 0 And id_ficha_entregable = 0 Then

                id_ficha_entregable = dtRow("id_ficha_entregable")
                'strTable_nextDEL = String.Format(strTableDEL, dtRow("numero_entregable"), dtRow("descripcion_entregable").ToString.Trim, dtRow("fecha"), strStatus, strTime, strAlert, (dtRow("porcentaje") / 100), dtRow("valor"), Math.Round((dtRow("valor") / 3348), 2, MidpointRounding.AwayFromZero))

            ElseIf dtRow("id_deliverable") = id_deliverable Then

                id_ficha_entregable = dtRow("id_ficha_entregable")
                'strTable_nextDEL = String.Format(strTableDEL, dtRow("numero_entregable"), dtRow("descripcion_entregable").ToString.Trim, dtRow("fecha"), strStatus, strTime, strAlert, (dtRow("porcentaje") / 100), dtRow("valor"), Math.Round((dtRow("valor") / 3348), 2, MidpointRounding.AwayFromZero))

            End If

        Next



        Dim strTable = "<table class='table no-margin'>
                                  <thead>
                                      <tr>
                                        <th style='width:2%;'></th>                                   
                                        <th style='width:3%;'>#</th>                    
                                        <th style='width:25%;'>Milestone</th>
                                        <th style='width:35%;'>Verification</th>
                                        <th style='width:8%;'>Due Date</th>
                                        <th style='width:8%;'>Delivered Date</th>
                                        <th style='width:3%;'>%</th>
                                        <th style='width:8%;'>Amount</th>
                                        <th style='width:8%;'>Status</th>
                                      </tr>
                                  </thead>
                                  <tbody>   
                                       {0}
                                 </tbody>
                        </table>"

        YLAfundingUSD = Math.Round((YLAfunding / ExchangeRate), 2, MidpointRounding.AwayFromZero)
        'PendingFundingUSD = Math.Round((PendingFunding / ExchangeRate), 2, MidpointRounding.AwayFromZero)
        'PerformedFundingUSD = Math.Round((PerformedFunding / ExchangeRate), 2, MidpointRounding.AwayFromZero)

        PorcenPerformed = Math.Round(((PerformedFunding / YLAfunding) * 100), 2, MidpointRounding.AwayFromZero)

        GetDeliverable_ = String.Format(strTable, strRowsTOT.Trim) & "||" &
                         String.Format("{0:N2} USD", YLAfundingUSD) & "||" &
                         String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", YLAfunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                         String.Format("{0:N2} USD", Math.Round(PendingFundingUSD, 2, MidpointRounding.AwayFromZero)) & "||" &
                         String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", PendingFunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                         String.Format("{0:N2} USD", Math.Round(PerformedFundingUSD, 2, MidpointRounding.AwayFromZero)) & "||" &
                         String.Format(sesUser.regionalizacionCulture, "{0:N2} {1}", PerformedFunding, sesUser.regionalizacionCulture.NumberFormat.CurrencySymbol) & "||" &
                         String.Format("{0:N2}", PorcenPerformed) & "||" &
                         "none" & "||" &
                         id_ficha_entregable



    End Function


    Public Shared Function Func_Alert(ByVal porcDays As Double, ByVal porcEDays As Double, ByVal alertType As Integer) As String


        Dim Dif_Porce As Double = porcDays - porcEDays
        Dim porc_Progress As Double = If(porcEDays <> 0, (Dif_Porce / porcEDays), 0)

        Const c_label_danger As String = "label-danger"
        Const c_label_warning As String = "label-warning"
        Const c_label_primary As String = "label-primary"
        Const c_label_success As String = "label-success"

        Const c_progress_bar_warning = "progress-bar-warning"
        Const c_progress_bar_primary = "progress-bar-primary"
        Const c_progress_bar_danger = "progress-bar-danger"


        Dim strResult As String = ""
        Dim strStatus As String = ""
        Dim strAlertBar1 As String = ""
        Dim strAlertBar2 As String = ""


        If porc_Progress >= 0 Then

            'Inverter number
            If ((1 - porc_Progress) * 100) >= 90 Then
                strResult = c_label_danger
            ElseIf ((1 - porc_Progress) * 100) >= 60 And ((1 - porc_Progress) * 100) < 90 Then
                strResult = c_label_warning
            ElseIf ((1 - porc_Progress) * 100) >= 30 And ((1 - porc_Progress) * 100) < 60 Then
                strResult = c_label_primary
            Else
                strResult = c_label_success
            End If

            strStatus = "Pending"
            strAlertBar2 = c_progress_bar_primary

        Else 'Expired
            strResult = c_label_danger
            strStatus = "Expired"
            strAlertBar2 = c_progress_bar_danger
        End If

        strAlertBar1 = c_progress_bar_warning

        If alertType = 1 Then
            Func_Alert = strResult
        ElseIf alertType = 2 Then
            Func_Alert = strStatus
        ElseIf alertType = 3 Then
            Func_Alert = strAlertBar1
        Else
            Func_Alert = strAlertBar2
        End If


    End Function


    Public Shared Function Func_Unit(ByVal Ndays As String) As String


        Dim vDays As Double
        Dim vWeeks As Double
        Dim vMonths As Double
        Dim vYear As Double

        Dim strUnit As String
        Dim vUnit As Double

        vDays = Ndays
        vWeeks = vDays / 7
        vMonths = vDays / 30
        vYear = vDays / 365


        If vWeeks < 1 Then

            vUnit = Math.Round(vDays, 2, MidpointRounding.AwayFromZero)

            If vDays > 1 Then
                strUnit = " days"
            Else
                strUnit = " day"
            End If

        ElseIf vMonths < 1 Then

            vUnit = Math.Round(vWeeks, 2, MidpointRounding.AwayFromZero)

            If vWeeks > 1 Then
                strUnit = " weeks"
            Else
                strUnit = " week"
            End If

        ElseIf vYear < 1 Then

            vUnit = Math.Round(vMonths, 2, MidpointRounding.AwayFromZero)

            If vMonths > 1 Then
                strUnit = " months"
            Else
                strUnit = " month"
            End If

        Else

            vUnit = Math.Round(vYear, 2, MidpointRounding.AwayFromZero)

            If vYear > 1 Then
                strUnit = " years"
            Else
                strUnit = " year"
            End If

        End If


        Func_Unit = String.Format("{0}&nbsp;{1}", vUnit, strUnit)

    End Function

End Class