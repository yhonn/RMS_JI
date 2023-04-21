Imports ly_SIME
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports CuteWebUI
Imports System.IO

Public Class frm_proyectoPrintInfoAttachImg
    Inherits System.Web.UI.Page
    Dim cnnME As New SqlConnection(ConnectionStrings("dbCI_SAPConnectionString").ConnectionString)
    Dim cl_user As ly_SIME.CORE.cls_user
    Dim frmCODE As String = "PROY_IMGS"
    Dim sFileDir As String = Server.MapPath("~") & "\FileUploads\"
    Dim sFileDirTemp As String = Server.MapPath("~") & "\Temp\"
    Dim controles As New ly_SIME.CORE.cls_controles

    Protected Overloads Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        AddHandler Uploader2.FileUploaded, AddressOf Uploader_FileUploadedImg
    End Sub
    Private Sub Uploader_FileUploadedImg(ByVal sender As Object, ByVal args As UploaderEventArgs)
        Dim uploader As Uploader = DirectCast(sender, Uploader)
        Try
            Dim Random As New Random()
            Dim Aleatorio As Double = Random.Next(1, 99999)
            Dim fileNameWE = Path.GetFileNameWithoutExtension(args.FileName)
            Dim extension As String = Path.GetExtension(args.FileName)
            Dim File As String = "MEImg_" & Me.Session("E_IdUser") & Aleatorio & fileNameWE.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", "_").Replace(",", "-").Replace("&", "-") & extension
            args.CopyTo(sFileDirTemp & File)
            Me.lblarchivoImg.Text = File
        Catch ex As Exception
            Me.imgEliminaImg.ImageUrl = "../imagenes/iconos/s_warn.png"
            Me.lblarchivoImg.Text = "Error.."
        End Try
        Me.Panel6.Visible = True
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
            Me.lbl_id_proyecto.Text = Me.Request.QueryString("Id").ToString
            fillGrid()
            LoadData()
        End If
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
            Me.lbl_nombre_subregion.Text = oProyecto.nombre_subregion
        End Using
    End Sub

    Protected Sub GrdArchivosImg_DeleteCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grd_cate.DeleteCommand
        Dim id_ficha_proyecto_imagen = TryCast(e.Item, GridDataItem).GetDataKeyValue("id_ficha_proyecto_imagen").ToString()
        cnnME.Open()
        Dim dm As New SqlCommand("DELETE FROM tme_FichaProyectoImagen WHERE (id_ficha_proyecto_imagen = " & id_ficha_proyecto_imagen & ")", cnnME)
        dm.ExecuteNonQuery()
        cnnME.Close()
        fillGrid()
    End Sub
    Protected Sub GrdArchivosImg_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grd_cate.ItemDataBound
        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
            Dim hlnk As HyperLink = New HyperLink
            hlnk = CType(e.Item.FindControl("AttachImg"), HyperLink)
            hlnk.Target = "_blank"
            hlnk.NavigateUrl = "../FileUploads/" & e.Item.Cells(4).Text.ToString

            Dim FileImg As Image = New Image
            FileImg = CType(e.Item.FindControl("FileImg"), Image)
            FileImg.ImageUrl = "../FileUploads/" & e.Item.Cells(4).Text.ToString
            FileImg.Width = "100"
            FileImg.Height = "100"
        End If
    End Sub
    Sub DelFileParam(ByVal archivo As String)
        Dim sFileName As String = System.IO.Path.GetFileName(archivo)

        Dim file_info As New IO.FileInfo(sFileDirTemp + sFileName)
        If (file_info.Exists) Then
            file_info.Delete()
        End If
    End Sub

    Sub CopyFileParam(ByVal file As String)
        Dim sFileName As String = System.IO.Path.GetFileNameWithoutExtension(file)
        Dim extension As String = System.IO.Path.GetExtension(file)
        Dim file_info As New IO.FileInfo(sFileDirTemp + sFileName + extension)
        Try
            file_info.CopyTo(sFileDir & sFileName.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace(" ", "_").Replace("%", "-").Replace(".", ".").Replace(",", "-") + extension)
        Catch ex As Exception
        End Try
        DelFileParam(file)
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnk_adjuntar.Click
        Me.UpdatePanel4.Visible = True
    End Sub

    Protected Sub ImageButton2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        Me.UpdatePanel4.Visible = False
    End Sub

    Protected Sub imgEliminaImg_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgEliminaImg.Click
        Me.Panel6.Visible = False
        DelFileParam(Me.lblarchivoImg.Text)
    End Sub

    Protected Sub imgEliminaImg0_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgEliminaImg0.Click
        cnnME.Open()
        Dim dmmCod As New SqlCommand("INSERT INTO tme_FichaProyectoImagen(id_ficha_proyecto,id_tipo_proyecto_imagen,nombre_archivo_proyecto, Descripcion_Imagen) VALUES (" & Me.lbl_id_proyecto.Text & "," & Me.RBEstados.SelectedValue.ToString & ",'" & Me.lblarchivoImg.Text & "','" & txtdescripcion.Text & "')", cnnME)
        dmmCod.ExecuteNonQuery()
        CopyFileParam(Me.lblarchivoImg.Text)
        Me.Panel6.Visible = False
        DelFileParam(Me.lblarchivoImg.Text)
        Me.UpdatePanel4.Visible = False
        fillGrid()
        cnnME.Close()
    End Sub

    Sub fillGrid()
        Dim sql As String = "SELECT ROW_NUMBER() OVER(ORDER BY id_ficha_proyecto_imagen DESC) as Number,nombre_archivo_proyecto, nombre_tipo_proyecto_imagen,id_ficha_proyecto_imagen"
        sql &= " FROM tme_FichaProyectoImagen INNER JOIN"
        sql &= " tme_FichaProyectoImageTipo ON tme_FichaProyectoImagen.id_tipo_proyecto_imagen = tme_FichaProyectoImageTipo.id_tipo_proyecto_imagen WHERE id_ficha_proyecto=" & Me.lbl_id_proyecto.Text
        Me.SqlDataSource8.SelectCommand = sql
        Me.grd_cate.DataBind()
    End Sub

End Class