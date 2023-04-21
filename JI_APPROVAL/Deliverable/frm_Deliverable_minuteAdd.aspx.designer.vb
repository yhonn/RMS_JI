'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class frm_Deliverable_minuteAdd
    
    '''<summary>
    '''Control MsgGuardar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents MsgGuardar As Global.RMS_APPROVAL.ModalConfirm
    
    '''<summary>
    '''Control lblt_titulo_pantalla.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_titulo_pantalla As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_subtitulo_pantalla.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_subtitulo_pantalla As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_activity_name.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_activity_name As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_activity_Code.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_activity_Code As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_period.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_period As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_totalACT.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_totalACT As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_totalACT_usd.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_totalACT_usd As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control hd_percent.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hd_percent As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control lbl_totalPerf.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_totalPerf As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_totalPerf_usd.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_totalPerf_usd As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control hd_id_deliverable.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hd_id_deliverable As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hd_id_ficha_entregable.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hd_id_ficha_entregable As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hd_performed.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hd_performed As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hd_tasa_cambio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hd_tasa_cambio As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control rep_DelivINFO.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rep_DelivINFO As Global.System.Web.UI.WebControls.Repeater
    
    '''<summary>
    '''Control lblt_Description.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_Description As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control dv_description.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents dv_description As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control lblt_Notes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_Notes As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control dv_notes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents dv_notes As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control chk_data_in.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chk_data_in As Global.System.Web.UI.HtmlControls.HtmlInputCheckBox
    
    '''<summary>
    '''Control hd_value_deliverable.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hd_value_deliverable As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control curr_local.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents curr_local As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control curr_International.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents curr_International As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control lblt_exchange_rate.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_exchange_rate As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_tasa_cambio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_tasa_cambio As Global.Telerik.Web.UI.RadNumericTextBox
    
    '''<summary>
    '''Control hd_exchange_Rate.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hd_exchange_Rate As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control lblt_minute_label.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_minute_label As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_approval_route.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_approval_route As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control hd_id_documento_deliverable.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hd_id_documento_deliverable As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hd_dtTipoAPP.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hd_dtTipoAPP As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control lblt_Beneficiario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_Beneficiario As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_beneficiario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_beneficiario As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_Activity.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Activity As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_Activity_Code_2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Activity_Code_2 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_pay_number.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_pay_number As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_total_Value.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_total_Value As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_OTR.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_OTR As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_OTR.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_OTR As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_accountable_info.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_accountable_info As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_accountability.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_accountability As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control RequiredFieldValidator1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator1 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control lbl_CLIN.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_CLIN As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lbl_GL.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_GL As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblt_Departamento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_Departamento As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_departamento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_departamento As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control lblt_municipio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_municipio As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_municipio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_municipio As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control RequiredFieldValidator3.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator3 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control hd_id_documento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hd_id_documento As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hd_id_deliverable_minute.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hd_id_deliverable_minute As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control lblt_Observation.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_Observation As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_notes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_notes As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control lblt_office.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_office As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control cmb_offices.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmb_offices As Global.Telerik.Web.UI.RadComboBox
    
    '''<summary>
    '''Control RequiredFieldValidator2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator2 As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control lblt_pay_beneficiary.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_pay_beneficiary As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_beneficiary.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_beneficiary As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control lblt_billing_info.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_billing_info As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_billing_info.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_billing_info As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control lblt_NIT_number.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_NIT_number As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txt_number_NIT.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_number_NIT As Global.Telerik.Web.UI.RadTextBox
    
    '''<summary>
    '''Control grd_cate.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grd_cate As Global.Telerik.Web.UI.RadGrid
    
    '''<summary>
    '''Control lblt_minute_code.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblt_minute_code As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control sp_code_minute.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents sp_code_minute As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control lyButtoms.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lyButtoms As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control btn_cancel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_cancel As Global.Telerik.Web.UI.RadButton
    
    '''<summary>
    '''Control btnlk_continue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnlk_continue As Global.System.Web.UI.WebControls.LinkButton
    
    '''<summary>
    '''Control btnlk_generate_code.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnlk_generate_code As Global.System.Web.UI.WebControls.LinkButton
    
    '''<summary>
    '''Control btnlk_print_preview.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnlk_print_preview As Global.System.Web.UI.WebControls.HyperLink
    
    '''<summary>
    '''Control lblerr_user.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblerr_user As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblError.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblError As Global.System.Web.UI.WebControls.Label
End Class
