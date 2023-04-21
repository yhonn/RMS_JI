Imports System
Imports System.Data
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Telerik.Web.UI
Imports ly_APPROVAL

Partial Class frm_doc_support

    Inherits System.Web.UI.Page
    Dim cnn As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cnnSAP As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)

    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "AP_TYPE_DOC_ADD"
    Dim controles As New ly_SIME.CORE.cls_controles

    Dim cl_Doc_supp As APPROVAL.cl_Doc_support

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
            End If
            controles.code_mod = frmCODE
            For Each Control As Control In Page.Controls
                controles.checkControls(Control, cl_user.id_idioma, cl_user)
            Next
        End If

        If Not IsPostBack Then

            'If HttpContext.Current.Session.Item("cl_Doc_supp") IsNot Nothing Then
            '    cl_Doc_supp = Session.Item("cl_Doc_supp")
            'Else
            cl_Doc_supp = New APPROVAL.cl_Doc_support(Me.Session("E_IDPrograma"))
            'End If

            Me.lbl_oldFile.Value = "--none--"
            HttpContext.Current.Session.Add("cl_Doc_supp", cl_Doc_supp)

        Else

            If HttpContext.Current.Session.Item("cl_Doc_supp") IsNot Nothing Then
                cl_Doc_supp = Session.Item("cl_Doc_supp")
            End If

        End If


    End Sub

    Protected Sub btn_save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_save.Click

        Dim err As Boolean = False
        Dim ext As String = ""
        For i = 0 To Me.chk_ext.Items.Count - 1
            If Me.chk_ext.Items(i).Selected = True Then
                ext &= Me.chk_ext.Items(i).Text.ToString & ", "
            End If
        Next
        ext &= ">!"
        If ext = ">!" Then
            err = True
            Me.lblt_errExt.Visible = True
        Else
            ext = ext.Replace(", >!", "").Replace(".", "").ToString
        End If

        If err = False Then

            Dim sFileDir As String = Server.MapPath("~\FileUploads\Templates\")
            Dim sFileDirTemp As String = Server.MapPath("~\Temp\")
            Dim dmyhm As String = String.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", DatePart(DateInterval.Year, Date.UtcNow), DatePart(DateInterval.Month, Date.UtcNow), DatePart(DateInterval.Day, Date.UtcNow), DatePart(DateInterval.Hour, Date.UtcNow), DatePart(DateInterval.Minute, Date.UtcNow), DatePart(DateInterval.Second, Date.UtcNow))

            Dim fileName As String = "--none--"
            Dim NewFname As String = ""
            Dim fuLLfiLeName As String = ""

            Dim fileNameWE As String
            Dim extension As String
            For Each f As UploadedFile In uploadFile.UploadedFiles
                fileName = f.GetName()
                fileNameWE = System.IO.Path.GetFileNameWithoutExtension(fileName)
                extension = System.IO.Path.GetExtension(fileName)
                NewFname = dmyhm & fileNameWE.Replace("'", "").Replace("`", "").Replace("´", "").Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-").Replace("'", "").Replace("""", "").Replace("#", "") & extension
                fuLLfiLeName = System.IO.Path.Combine(sFileDir, NewFname)
            Next


            cl_Doc_supp.set_ta_docs_soporte(0)
            cl_Doc_supp.set_ta_docs_soporteFIELDS("nombre_documento", Me.txt_cat.Text, "id_doc_soporte", 0)
            cl_Doc_supp.set_ta_docs_soporteFIELDS("extension", ext, "id_doc_soporte", 0)
            cl_Doc_supp.set_ta_docs_soporteFIELDS("id_programa", Me.Session("E_IDPrograma"), "id_doc_soporte", 0)
            cl_Doc_supp.set_ta_docs_soporteFIELDS("template", NewFname, "id_doc_soporte", 0)
            cl_Doc_supp.set_ta_docs_soporteFIELDS("environmental", IIf(chk_environmetal.Checked = True, 1, 0), "id_doc_soporte", 0)
            cl_Doc_supp.set_ta_docs_soporteFIELDS("deliverable", IIf(chk_deliverable.Checked = True, 1, 0), "id_doc_soporte", 0)
            cl_Doc_supp.set_ta_docs_soporteFIELDS("max_size", Me.txt_max_size.Value, "id_doc_soporte", 0)

            If cl_Doc_supp.save_ta_docs_soporte() <> -1 Then 'Save the document
                For Each f As UploadedFile In uploadFile.UploadedFiles
                    'f.SaveAs(fuLLfiLeName)
                    CopyFile(System.IO.Path.Combine(sFileDirTemp, f.GetName()), fuLLfiLeName)
                Next

            Else 'Error
                lblt_Error_Save.Text = "Error Saving the document type"
                lblt_Error_Save.Visible = True
            End If

            Session.Remove("cl_Doc_supp") 'Remove the session
            Me.Response.Redirect("~/Approvals/frm_consultadoc_support.aspx")

        End If


    End Sub


    Sub CopyFile(ByVal sFile_oldName As String, ByVal File_NewName As String)

        Dim file_info As New IO.FileInfo(sFile_oldName)

        Try

            If file_info.Exists() Then
                file_info.CopyTo(File_NewName)
                file_info.Delete()
            End If


        Catch ex As Exception

        End Try

    End Sub


    Protected Sub btn_cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_cancel.Click
        Me.Response.Redirect("~/Approvals/frm_consultadoc_support.aspx")
    End Sub
End Class
