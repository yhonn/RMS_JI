﻿Public Class Global_asax
    Inherits HttpApplication

    Dim cl_user As ly_SIME.CORE.cls_user

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        AuthConfig.RegisterOpenAuth()
        RouteConfig.RegisterRoutes(System.Web.Routing.RouteTable.Routes)
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)

        ' Fires at the beginning of each request

    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub
End Class