Imports ly_SIME
Imports DotNet.Highcharts
Imports DotNet.Highcharts.Options
Imports DotNet.Highcharts.Enums

Public Class grp_GraphAvanceEjecucionInd
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        get_chart(Convert.ToInt32(Me.Request.QueryString("id").ToString))
    End Sub

    Function get_chart(ByVal id_indicador As Integer) As String

        Using dbEntities As New dbRMS_HNEntities
            Dim abc = dbEntities.vw_tme_ficha_meta_indicador.Where(Function(p) p.id_ficha_proyecto = id_indicador) _
                      .Select(Function(p) _
                                          New With {Key .titulo = p.definicion_indicador, _
                                                    Key .valor = p.meta_total}).ToList()

            Dim elementos(abc.Count - 1) As Series
            Dim i = 0

            For Each item In abc
                Dim series As Series = New Series()
                series.Name = item.titulo
                series.Data = New Helpers.Data(New Object() {item.valor})
                elementos(i) = series
                i = i + 1
            Next

            Dim chart1 As DotNet.Highcharts.Highcharts = New DotNet.Highcharts.Highcharts("chart")

            chart1.InitChart(New Chart() With { _
                             .DefaultSeriesType = ChartTypes.Bar, _
                             .Height = 200
                         }).SetTitle(New Title() With { _
                                     .Text = "Avance de Indicadores" _
                                 }) _
                         .SetXAxis(New XAxis() With { _
                                   .Categories = New String() {"Meta", "Ejecutado"} _
                               }) _
                       .SetYAxis(New YAxis() With { _
                                   .Title = New YAxisTitle() With {.Text = "Meta Indicador"} _
                               }) _
                       .SetSeries(elementos)

            Me.ltrChart.Text = chart1.ToHtmlString()
            Return chart1.ChartScriptHtmlString().ToString()
        End Using
    End Function

End Class