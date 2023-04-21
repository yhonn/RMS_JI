

function Deliverable_Allocated_chart(jsonDeliverable, DelivTittle) {




    Highcharts.chart('Deliverable_Activity-chart', {
        chart: {
            type: 'areaspline'
        },
        title: {
            text: 'Disbursed Deliverables' + ' (' + DelivTittle + ')'
        },
        //legend: {
        //    layout: 'vertical',
        //    align: 'left',
        //    verticalAlign: 'top',
        //    x: 150,
        //    y: 100,
        //    floating: true,
        //    borderWidth: 1,
        //    backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
        //}
        legend: {
            align: 'right',
            x: -50,
            verticalAlign: 'top',
            layout: 'vertical',
            y: 0,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false
        },
        plotOptions: {
            areaspline: {
                fillOpacity: 0.5
            },
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y:,.2f} USD',
                    formatter: function () {
                        //console.log(this);
                        var val = this.y;
                        if (val < 1) {
                            return '';
                        }
                        return val;
                    }
                }
            }
        },
        labels: {
            items: [{
                html: 'Obligated vs Disbursed Funding (USD)',
                style: {
                    left: '50px',
                    top: '0px',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                }
            }]
        },
        xAxis: {
            type: 'category',
            labels: {
                rotation: 25
            },
            plotBands: [{ // visualize the weekend
                from: 3.5,
                to: 6.5,
                color: 'rgba(68, 170, 213, .2)'
            }]
        },
        yAxis: {
            title: {
                text: 'Total amount'
            }
        },
        //tooltip: {
        //    shared: true,
        //    valueSuffix: ' USD'
        //},
        tooltip: {
            headerFormat: '<span style="color:{point.color};font-size:11px">{series.name}</span><br>',
            //********************************pointFormat: '{series.name}: <b>{point.y}</b> ({point.percentage:.1f}%)<br/>'
            // pointFormat: '<span>{point.name}</span>: <b>{point.y:2f} USD</b>'
            pointFormat: '<span>{point.name}</span>: <b>{point.y:,.2f} USD</b>'
        },
        credits: {
            enabled: false
        },
        //series: [{
        //    name: 'Obligated',
        //    data: [3000, 5000, 3568,0, 4745, 10000, 12000,6874,25415,16845,7845,15965]
        //}, {
        //    name: 'Allocated',
        //    data: [2500, 5874, 0, 3789, 3654, 5254, 4541,7845,7451,9475,18954,21544]
        //}]
        //series: [{
        //    name: 'Obligated Funds USD',
        //    data: [{ name: '1-2018-Mayo', y: 17712.37 }, { name: '1-2019-Marzo', y: 0 }, { name: '2-2018-Junio', y: 61356.53 }, { name: '2-2019-Abril', y: 0 }, { name: '3-2018-Julio', y: 17908.44 }, { name: '3-2019-Abril', y: 0 }, { name: '4-2018-Octubre', y: 52835.56 }, { name: '4-2019-Abril', y: 0 }, { name: '5-2019-Enero', y: 54707.12 }, { name: '6-2019-Marzo', y: 27425.61 }]
        //}, {
        //    name: 'Allocated Funds USD',
        //    data: [{ name: '1-2018-Mayo', y: 0 }, { name: '1-2019-Marzo', y: 0 }, { name: '2-2018-Junio', y: 0 }, { name: '2-2019-Abril', y: 62000 }, { name: '3-2018-Julio', y: 0 }, { name: '3-2019-Abril', y: 18000 }, { name: '4-2018-Octubre', y: 0 }, { name: '4-2019-Abril', y: 55000 }, { name: '5-2019-Enero', y: 0 }, { name: '6-2019-Marzo', y: 0 }]
        //}]
        series: jsonDeliverable
    });


    //Deliverable_Activity - chart.Highcharts.setOptions({
    //    lang: {
    //        thousandsSep: ','
    //    }
    //});



}


//**********************************************************************************
//******************************FOR TESTING PURPOSES********************************
//**********************************************************************************
function Deliverable_chart_test() {




    Highcharts.chart('Deliverable_Activity-chart', {
        chart: {
            type: 'areaspline'
        },
        title: {
            text: 'Allocated Deliverables'
        },
        //legend: {
        //    layout: 'vertical',
        //    align: 'left',
        //    verticalAlign: 'top',
        //    x: 150,
        //    y: 100,
        //    floating: true,
        //    borderWidth: 1,
        //    backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
        //}
        legend: {
            align: 'right',
            x: -150,
            verticalAlign: 'top',
            layout: 'horizontal',
            y: -5,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false
        },
        plotOptions: {
            areaspline: {
                fillOpacity: 0.5
            },
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y:,.2f} USD',                    
                    formatter: function () {
                        //console.log(this);
                        var val = this.y;
                        if (val < 1) {
                            return '';
                        }
                        return val;
                    }
                }
            }
        },
        labels: {
            items: [{
                html: 'Comparing on Allocated Funding (USD)',
                style: {
                    left: '50px',
                    top: '0px',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                }
            }]
        },

        xAxis: {
            type: 'category',
            labels: {
                rotation: 25
            },
            plotBands: [{ // visualize the weekend
                from: 3.5,
                to: 6.5,
                color: 'rgba(68, 170, 213, .2)'
            }]
        },
        yAxis: {
            title: {
                text: 'Total amount'
            }
        },
        //tooltip: {
        //    shared: true,
        //    valueSuffix: ' USD'
        //},
        tooltip: {
            headerFormat: '<span style="color:{point.color};font-size:11px">{series.name}</span><br>',
            //********************************pointFormat: '{series.name}: <b>{point.y}</b> ({point.percentage:.1f}%)<br/>'
            // pointFormat: '<span>{point.name}</span>: <b>{point.y:2f} USD</b>'
            pointFormat: '<span>{point.name}</span>: <b>{point.y:,.2f} USD</b>'
        },
        credits: {
            enabled: false
        },       
        //series: [{
        //    name: 'Obligated',
        //    data: [3000, 5000, 3568,0, 4745, 10000, 12000,6874,25415,16845,7845,15965]
        //}, {
        //    name: 'Allocated',
        //    data: [2500, 5874, 0, 3789, 3654, 5254, 4541,7845,7451,9475,18954,21544]
        //}]
        series: [{
                     name: 'Obligated Funds USD',
                     data: [{ name: '1-2018-Mayo', y: 17712.37 }, { name: '1-2019-Marzo', y: 0 }, { name: '2-2018-Junio', y: 61356.53 }, { name: '2-2019-Abril', y: 0 }, { name: '3-2018-Julio', y: 17908.44 }, { name: '3-2019-Abril', y: 0 }, { name: '4-2018-Octubre', y: 52835.56 }, { name: '4-2019-Abril', y: 0 }, { name: '5-2019-Enero', y: 54707.12 }, { name: '6-2019-Marzo', y: 27425.61 }]
        }, {
            name: 'Allocated Funds USD',
            data: [{ name: '1-2018-Mayo', y: 0 }, { name: '1-2019-Marzo', y: 0 }, { name: '2-2018-Junio', y: 0 }, { name: '2-2019-Abril', y: 62000 }, { name: '3-2018-Julio', y: 0 }, { name: '3-2019-Abril', y: 18000 }, { name: '4-2018-Octubre', y: 0 }, { name: '4-2019-Abril', y: 55000 }, { name: '5-2019-Enero', y: 0 }, { name: '6-2019-Marzo', y: 0 }]
        }]            
    });

    
    //Deliverable_Activity-chart.Highcharts.setOptions({
    //    lang: {
    //        thousandsSep: ','
    //    }
    //});



}