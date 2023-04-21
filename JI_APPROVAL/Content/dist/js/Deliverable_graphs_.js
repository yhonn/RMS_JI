

$(function () {
    Highcharts.setOptions({
        lang: {
            thousandsSep: ','
        }
    });
});



    function Get_Code(strActivityCode, DeliverableN) {

        console.log('Activity Cede:' + strActivityCode + ', Deliverable#: ' + DeliverableN);

        $.ajax({
            type: "POST",
            url: "frm_DeliverableREs.aspx/get_DeliverableID",
            data: "{vActivity:'" + strActivityCode + "', vDeliverableN:" + DeliverableN + " }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                jsonResult = data.d;                   
                //console.log("data: " + JSON.stringify(data));
                //console.log("d: " + jsonResult);
                UseRadWindow(jsonResult);

            },
            failure: function (response) {
                alert('Error getting code: ' + response.d);
                //$("#LoadScreen").hide();
            }

        });

    }

function Load_Leveraging_chart(jsonSerie, jsonDrillDown, CurrencySymbol) {

    var tittleLABELS = ['By Fiscal Year'];
    var level = 0;
    var ran = false;
    var Activity_Code = '';
    var strCurrentLevel = '';

    Highcharts.setOptions({
        lang: {
            thousandsSep: ',',
            drillUpText: '<< ' + tittleLABELS[level]
        }
    });


    $('#Deliverables_chart').highcharts({

        title: {
            text: 'DELIVERABLES & PAYMENTS',
            align: 'center',
            x: 10
        },
        subtitle: {
            text: tittleLABELS[0]
        },
        xAxis: {
            type: 'category',
            labels: {
                rotation: 25
            }
        },
        chart: {
            events: {
                drilldown: function (e) {

                    if (ran) {
                        ran = false;
                        //newTotals[0] = e.seriesOptions.data.reduce(getSum, 0);
                        //totals.push(newTotals);
                        //this.series[2].setData(newTotals, true);
                        //newTotals = [];
                    } else {
                        
                        ran = true;
                        chart = this;
                        chart.showLoading('Loading chart for ' + e.point.name);
                        strCurrentLevel = e.point.name;    
                        level++;
                        tittleLABELS.push(e.point.name);
                        chart.setTitle(null, {
                            text: tittleLABELS.join('->')
                        });
                        if (level === 4)
                            Activity_Code = e.point.name;

                        
                        setTimeout(function () {                                                      
                            
                       //console.log('Point Name: ' + e.point.name + ' Level: ' + level); //+ ' Serie Name: ' + e.series.name                           
                        //chart.addSingleSeriesAsDrilldown(e.point, series);  
                            chart.hideLoading();
                           //chart.applyDrilldown();                            
                        }, 1000);


                     

                      
                    }
                },
                //--********************************--//
                drillup: function (e) {

                    if (ran) {
                        ran = false;
                    } else {
                        
                        ran = true;
                        chart = this;
                        //console.log('Level: ' + level);                       
                        //if (level != 2)
                          //  chart.drillUpButton = chart.drillUpButton.destroy();
                        tittleLABELS.pop();
                        chart.showLoading('Loading chart for ' + tittleLABELS.join('<-'));

                        setTimeout(function () {         
                            level--;                           
                            //totals.pop();
                            //this.series[2].setData(totals[totals.length - 1], true);
                            chart.setTitle(null, {
                                text: tittleLABELS.join('->')
                            }); 

                            //if (level != 2)
                                //chart.showDrillUpButton();

                            chart.hideLoading();
                            
                        }, 1000);

                    }
                }
                //--********************************--//
            }
        }
        //, events: {
        //    drilldown: function (e) {
        //        //var chart = this,
        //        //    drilldowns = chart.userOptions.drilldown.series,
        //        //    series = [];

        //        //alert(e.point.name);
        //        console.log(e.point.name);

        //        //e.preventDefault();
        //        //Highcharts.each(drilldowns, function (p, i) {
        //        //    if (p.id.includes(e.point.name)) {
        //        //        chart.addSingleSeriesAsDrilldown(e.point, p);
        //        //    }
        //        //});
        //        //chart.applyDrilldown();
        //    }
        //}
        , labels: {
            items: [{
                html: 'Executed Funds (' + CurrencySymbol +')',
                style: {
                    left: '50px',
                    top: '0px',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                }
            }]
        },
        legend: {
            align: 'right',
            x: -50,
            verticalAlign: 'top',
            layout: 'vertical',
            y: -5,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y:,.2f} (' + CurrencySymbol + ')',
                    formatter: function () {
                        console.log(this);
                        var val = this.y;
                        if (val < 1) {
                            return '';
                        }
                        return val;
                    },
                    rotation: 0,
                    align: 'left'
                },
                allowPointSelect: true,
                point: {
                    events: {
                        select: function (e) {

                            // alert('Value1:' + this.category + ', Value2: ' + this.y + ', Value:' + this.series.name)
                            //console.log('Value1:' + this.category + ', Value2: ' + this.y + ', Value2.1:' + this.name + ', Value:' + this.series.name);
                            var DeliverableN = this.name;
                            var strA = DeliverableN.indexOf('[') + 1;
                            var strB = DeliverableN.indexOf(']');
                            var strLength = (strB - strA) - 1;
                            //DeliverableN.substr(strA + 1, strLength)
                            var NumberDELIV = parseInt(DeliverableN.substr(strA + 1, strLength));
                            //console.log('Activity Cede:' + Activity_Code + ', Deliverable#: '+ NumberDELIV);

                            var idDeliv = Get_Code(Activity_Code, parseInt(DeliverableN.substr(strA + 1, strLength)));

                        }
                    }
                }
            }
        },
        tooltip: {
            headerFormat: '<span style="color:{point.color};font-size:11px">{series.name}</span><br>',
            //pointFormat: '{series.name}: <b>{point.y}</b> ({point.percentage:.1f}%)<br/>'
            //pointFormat: '<span>{point.name}</span>: <b>{point.y:2f} USD</b>'
            pointFormat: '<span>{point.name}</span>: <b>{point.y} (' + CurrencySymbol +')</b>'
        },

        series: jsonSerie,

        drilldown: {
            drillUpButton: {
                relativeTo: "plotBox",
                position: {
                    align: 'left',
                    verticalAlign: 'bottom',
                    y: -30,
                    x:0
                },
            },//****************************************************************************************************                
            series: jsonDrillDown
        } //****************************************************************************************************


    });

}



//**********************************************************************************
//******************************FOR TESTING PURPOSES********************************
//**********************************************************************************


function DELIV_SAMPLE_CHART() {
        
      $('#Deliverables_chart').highcharts({
                        title: {
                            text: 'YLA Leveraged Funds',
                            align: 'left',
                            x: 350
                        },
                        xAxis: {
                            type: 'category',
                            labels: {
                                rotation: 25
                            }
                        }
                       , events: {
                           drilldown: function (e) {
                               var chart = this,
                                   drilldowns = chart.userOptions.drilldown.series,
                                   series = [];
                               //alert(e.point.name);
                               e.preventDefault();
                               Highcharts.each(drilldowns, function (p, i) {
                                   if (p.id.includes(e.point.name)) {
                                       chart.addSingleSeriesAsDrilldown(e.point, p);
                                   }
                               });
                               chart.applyDrilldown();
                           }
                       }
                         , labels: {
                             items: [{
                                 html: 'By Source of Funding (USD)',
                                 style: {
                                     left: '50px',
                                     top: '0px',
                                     color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                                 }
                             }]
                         },
                        legend: {
                            align: 'right',
                            x: -250,
                            verticalAlign: 'top',
                            layout: 'vertical',
                            y: -5,
                            floating: true,
                            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                            borderColor: '#CCC',
                            borderWidth: 1,
                            shadow: false
                        },
                        plotOptions: {
                            series: {
                                borderWidth: 0,
                                dataLabels: {
                                    enabled: true,
                                    format: '{point.y:2f} USD',
                                    formatter: function () {
                                        console.log(this);
                                        var val = this.y;
                                        if (val < 1) {
                                            return '';
                                        }
                                        return val;
                                    }
                                }
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="color:{point.color};font-size:11px">{series.name}</span><br>',
                            //pointFormat: '{series.name}: <b>{point.y}</b> ({point.percentage:.1f}%)<br/>'
                            //pointFormat: '<span>{point.name}</span>: <b>{point.y:2f} USD</b>'
                            pointFormat: '<span>{point.name}</span>: <b>{point.y} USD</b>'
                        },
                        series: [{
                            "type": "column",
                            "name": "Do not apply",
                            "data": [{ "name": "FY1", "y": 15000, "drilldown": "FY1_2" }, { "name": "FY2", "y": 0, "drilldown": "FY2_2" }, { "name": "FY3", "y": 283770.1, "drilldown": "FY3_2" }]
                        }, {
                            "type": "column",
                            "name": "Local Governments",
                            "data": [{ "name": "FY1", "y": 5000, "drilldown": "FY1_8" }, { "name": "FY2", "y": 44.8, "drilldown": "FY2_8" }, { "name": "FY3", "y": 74.67, "drilldown": "FY3_8" }]
                        }, {
                            "type": "column",
                            "name": "Non Govermental Organisation (NGO)",
                            "data": [{ "name": "FY1", "y": 37763.78, "drilldown": "FY1_12" }, { "name": "FY2", "y": 0, "drilldown": "FY2_12" }, { "name": "FY3", "y": 280351.21, "drilldown": "FY3_12" }]
                        }, {
                            "type": "column",
                            "name": "Private Enterprises and Industry Associations",
                            "data": [{ "name": "FY1", "y": 26127.59, "drilldown": "FY1_1" }, { "name": "FY2", "y": 10572047.14, "drilldown": "FY2_1" }, { "name": "FY3", "y": 2681310.24, "drilldown": "FY3_1" }]
                        }, {
                            "type": "column",
                            "name": "Producer Associations",
                            "data": [{ "name": "FY1", "y": 6000, "drilldown": "FY1_3" }, { "name": "FY2", "y": 0, "drilldown": "FY2_3" }, { "name": "FY3", "y": 25194.15, "drilldown": "FY3_3" }]
                        }, {
                            "type": "spline",
                            "name": "YLA Funding",
                            "marker": {
                                "lineWidth": 2,
                                "fillColor": "white"
                            },
                            "data": [91567.92, 315323.5, 1331890.74]
                        }, {
                            "type": "pie",
                            "name": "Total Funding",
                            "center": [100, 60],
                            "size": 120,
                            "showInLegend": false,
                            "dataLabels": {
                                "enabled": false
                            },
                            "data": [{ "name": "Private Funding", "y": 13906564.21 }, { "name": "Project Funding", "y": 1738782.16 }, { "name": "Public Funding", "y": 119.47 }]
                        }],
                        drilldown: {
                            series: [{
                                "name": "FY3-Do not apply",
                                "id": "FY3_2",
                                "type": "column",
                                "data": [["Implementer Funding", 283770.1]]
                            }, {
                                "name": "FY2-Local Governments",
                                "id": "FY2_8",
                                "type": "column",
                                "data": [["Local Funding", 44.8]]
                            }, {
                                "name": "FY3-Local Governments",
                                "id": "FY3_8",
                                "type": "column",
                                "data": [["Local Funding", 74.67]]
                            }, {
                                "name": "FY1-Non Govermental Organisation (NGO)",
                                "id": "FY1_12",
                                "type": "column",                                 
                                "data": [{ "name": "Ciao", "y": 25471, "drilldown": "FY1_JAN" }, { "name": "Akronio", "y": 37763.78, "drilldown": "FY1_MARCH" }]
                            }, {
                                "name": "FY3-Non Govermental Organisation (NGO)",
                                "id": "FY3_12",
                                "type": "column",
                                "data": [["Kiima Foods", 8682.3], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 168149.2], ["Sing With Me Happily", 103519.71]]
                            }, {
                                "name": "FY1-Private Enterprises and Industry Associations",
                                "id": "FY1_1",
                                "type": "column",
                                "data": [["Akorion Company Limited", 26127.59]]
                            }, {
                                "name": "FY2-Private Enterprises and Industry Associations",
                                "id": "FY2_1",
                                "type": "column",
                                "data": [["African Innovations Institute (AfrII)", 103972.52], ["Agromart Agricultural Company. (U) Ltd.", 7287.93], ["Brilliant Youth Organization (BYO)", 48261.65], ["Consult Agri-Query Solutions Ltd (Agri-Query)", 3945.04], ["Mugalex Agro Enterprise Ltd", 896.06], ["Responsible Suppliers Limited", 29495.16], ["Smart Money", 102995.53], ["TOTCO Uganda Limited", 9735320.19], ["West Nile Holdings Limited", 539873.06]]
                            }, {
                                "name": "FY3-Private Enterprises and Industry Associations",
                                "id": "FY3_1",
                                "type": "column",
                                "data": [["Agrinet Uganda Ltd (AgriNet)", 100546.59], ["Ankole Coffee Processors Limited ", 43710.87], ["Aponye Uganda Limited", 50059.74], ["Byeffe  Foods Company Limited", 24401.08], ["East African Seeds Company Limited", 8643.97], ["Ensibuuko Technologies Limited", 268894.82], ["Equator Seeds Ltd", 95020.31], ["Faith Agro Inputs Limited", 11275.39], ["Grain Trade Development Services", 27081.24], ["Kulika Uganda", 19005.38], ["MobiPay AgroSys Limited", 55428.61], ["Ngetta Tropical Holdings", 1168706.69], ["Sebei Farmer SACCO", 477166.67], ["Sunshine Agro Products", 331368.88]]
                            }, {
                                "name": "FY3-Producer Associations",
                                "id": "FY3_3",
                                "type": "column",
                                "data": [["Green Growers Training and Demonstration Farm ", 25194.15]]
                                }, {
                                    "name": "FY1-JAN-CIAO",
                                    "id": "FY1-JAN",
                                    "type": "column",
                                    "data": [["DELIV1", 25194.15]]
                                }, {
                                    "name": "FY1-MARCH-CIAO",
                                    "id": "FY1_MARCH",
                                    "type": "column",
                                    "data": [["DELIV2", 35194.15]]
                                }]

                        }

                    });



}






function DELIV_SAMPLE_CHART_II() {

    var tittleLABELS = ['By Fiscal Year'];
    var level = 0;
    var ran = false;
    var Activity_Code = '';

    Highcharts.setOptions({
        lang: {
            thousandsSep: ',' ,
            drillUpText: '<< ' + tittleLABELS[level]
        }
    });
           
    $('#Deliverables_chart').highcharts({
        
        title: {
            text: 'BHA DELIVERABLE CHART',
            align: 'center',
            x: 10
        },
        subtitle: {
            text: tittleLABELS[0]
        },
        xAxis: {
            type: 'category',
            labels: {
                rotation: 25
            }
        },
        chart: {            
            events: {
                drilldown: function (e) {
                                  
                    if (ran) {
                        ran = false;
                        //newTotals[0] = e.seriesOptions.data.reduce(getSum, 0);
                        //totals.push(newTotals);
                        //this.series[2].setData(newTotals, true);
                        //newTotals = [];
                    } else {
                        ran = true;
                        level++;
                        tittleLABELS.push(e.point.name);
                        //newTotals[1] = e.seriesOptions.data.reduce(getSum, 0);

                        this.setTitle(null, {
                            text: tittleLABELS.join('->')
                        });                       

                        console.log('Point Name: ' + e.point.name + ' Level: ' + level); //+ ' Serie Name: ' + e.series.name

                        if (level === 4)
                            Activity_Code = e.point.name;
                    }
                },
            //--********************************--//
                drillup: function (e) {

                    if (ran) {
                        ran = false;
                    } else {
                        ran = true;
                        level--;
                        tittleLABELS.pop();
                        //totals.pop();
                        //this.series[2].setData(totals[totals.length - 1], true);
                        this.setTitle(null, {
                            text: tittleLABELS.join('->')
                        });
                    }
                }
            //--********************************--//
            }
        }
        //, events: {
        //    drilldown: function (e) {
        //        //var chart = this,
        //        //    drilldowns = chart.userOptions.drilldown.series,
        //        //    series = [];

        //        //alert(e.point.name);
        //        console.log(e.point.name);

        //        //e.preventDefault();
        //        //Highcharts.each(drilldowns, function (p, i) {
        //        //    if (p.id.includes(e.point.name)) {
        //        //        chart.addSingleSeriesAsDrilldown(e.point, p);
        //        //    }
        //        //});
        //        //chart.applyDrilldown();
        //    }
        //}
        , labels: {
            items: [{
                html: 'By Source of Funding (USD)',
                style: {
                    left: '50px',
                    top: '0px',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                }
            }]
        },
        legend: {
            align: 'right',
            x: -50,
            verticalAlign: 'top',
            layout: 'vertical',
            y: -5,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y:,.2f} USD',
                    formatter: function () {
                        console.log(this);
                        var val = this.y;
                        if (val < 1) {
                            return '';
                        }
                        return val;
                    },
                    rotation: 0,
                    align: 'left'
                },
                allowPointSelect: true,
                point: {
                    events: {
                        select: function (e) {

                            // alert('Value1:' + this.category + ', Value2: ' + this.y + ', Value:' + this.series.name)
                            //console.log('Value1:' + this.category + ', Value2: ' + this.y + ', Value2.1:' + this.name + ', Value:' + this.series.name);
                            var DeliverableN = this.name;
                            var strA = DeliverableN.indexOf('[') + 1; 
                            var strB = DeliverableN.indexOf(']');
                            var strLength = (strB - strA) - 1;
                            //DeliverableN.substr(strA + 1, strLength)
                            var NumberDELIV = parseInt(DeliverableN.substr(strA + 1, strLength));
                            //console.log('Activity Cede:' + Activity_Code + ', Deliverable#: '+ NumberDELIV);

                            var idDeliv = Get_Code(Activity_Code, parseInt(DeliverableN.substr(strA + 1, strLength)));
                                                                               
                        }
                    }
                }
            }
        },
        tooltip: {
            headerFormat: '<span style="color:{point.color};font-size:11px">{series.name}</span><br>',
            //pointFormat: '{series.name}: <b>{point.y}</b> ({point.percentage:.1f}%)<br/>'
            //pointFormat: '<span>{point.name}</span>: <b>{point.y:2f} USD</b>'
            pointFormat: '<span>{point.name}</span>: <b>{point.y} USD</b>'
        },
        series: [{
            "type": "column",
            "name": "Obligated",
            "color": "rgb(126,86,134)",
            "data": [{ "name": "FY1", "y": 237333.87, "drilldown": "FY1_Obligated" }, { "name": "FY2", "y": 618899.95, "drilldown": "FY2_Obligated" }, { "name": "FY3", "y": 851004.54, "drilldown": "FY3_Obligated" }]
        }, {
            "type": "column",
            "name": "Disbursed",
            "color": "rgb(165,170,217)",
            "data": [{ "name": "FY1", "y": 175833.81, "drilldown": "FY1_Disbursed" }, { "name": "FY2", "y": 398272.68, "drilldown": "FY2_Disbursed" }, { "name": "FY3", "y": 0, "drilldown": "FY3_Disbursed" }]
        }, {
            "type": "spline",
            "name": "BHA Founding (USD)",
            "marker": {
                "lineWidth": 2,
                "fillColor": "white"
            },
            "color": "#FC472B",
            "data": [1926546.00, 3112501.81, 0.00]
        }, {
            "type": "pie",
            "name": "BHA Deliverables (USD) ",
            "center": [100, 60],
            "size": 110,
            "showInLegend": false,
            "dataLabels": {
                "enabled": false
            },
            "data": [{ "name": "Obligated", "y": 1707238.36 }, { "name": "Disbursed", "y": 574106.49 }]
        }],
        drilldown: {
            drillUpButton: {
                relativeTo: "spacingBox",
                position: {
                    align: 'left',
                    verticalAlign: 'bottom',
                    y: -20
                },
            },//****************************************************************************************************                
            series: [{
                "type": "column",
                "name": "Obligated",
                "id": "FY1_Obligated",
                "data": [{ "name": "June-2018", "y": 83790.36, "drilldown": "June-2018_Obligated" }, { "name": "July-2018", "y": 40659.5, "drilldown": "July-2018_Obligated" }, { "name": "August-2018", "y": 58527.82, "drilldown": "August-2018_Obligated" }, { "name": "September-2018", "y": 54356.19, "drilldown": "September-2018_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "June-2018_Obligated",
                "data": [{ "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 83790.36, "drilldown": "EJEC_11_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_11_Obligated",
                "data": [{ "name": "BHA-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Obligated" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1011_Obligated",
                "data": [{ "name": "[#1]-2018-June", "y": 12774.2 }, { "name": "[#2]-2018-June", "y": 21716.14 }, { "name": "[#3]-2018-June", "y": 16606.46 }, { "name": "[#4]-2018-June", "y": 23751.62 }, { "name": "[#5]-2018-June", "y": 8941.94 }, { "name": "[#6]-2018-July", "y": 30658.08 }, { "name": "[#1]-2018-August", "y": 0 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#7]-2018-August", "y": 28523.56 }, { "name": "[#3]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#6]-2018-September", "y": 0 }, { "name": "[#7]-2018-September", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1013_Obligated",
                "data": [{ "name": "[#1]-2018-September", "y": 8635.42 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 17778.8 }, { "name": "[#4]-2018-October", "y": 37081.49 }, { "name": "[#1]-2018-November", "y": 0 }, { "name": "[#5]-2018-November", "y": 9143.38 }, { "name": "[#6]-2018-November", "y": 26414.21 }, { "name": "[#7]-2018-November", "y": 16762.87 }, { "name": "[#4]-2018-December", "y": 0 }, { "name": "[#5]-2018-December", "y": 0 }, { "name": "[#8]-2018-December", "y": 18794.72 }, { "name": "[#8]-2019-January", "y": 0 }, { "name": "[#3]-2019-March", "y": 0 }, { "name": "[#7]-2019-March", "y": 0 }, { "name": "[#6]-2019-April", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "July-2018_Obligated",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 10001.42, "drilldown": "EJEC_14_Obligated" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 30658.08, "drilldown": "EJEC_11_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_14_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Obligated" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1008_Obligated",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 22860.39 }, { "name": "[#3]-2018-August", "y": 7143.87 }, { "name": "[#2]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 31433.03 }, { "name": "[#5]-2018-September", "y": 14287.74 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#5]-2018-November", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1009_Obligated",
                "data": [{ "name": "[#1]-2018-November", "y": 21914.42 }, { "name": "[#1]-2018-December", "y": 0 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 21914.42 }, { "name": "[#3]-2019-February", "y": 0 }, { "name": "[#4]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-March", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_11_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Obligated" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1011_Obligated",
                "data": [{ "name": "[#1]-2018-June", "y": 12774.2 }, { "name": "[#2]-2018-June", "y": 21716.14 }, { "name": "[#3]-2018-June", "y": 16606.46 }, { "name": "[#4]-2018-June", "y": 23751.62 }, { "name": "[#5]-2018-June", "y": 8941.94 }, { "name": "[#6]-2018-July", "y": 30658.08 }, { "name": "[#1]-2018-August", "y": 0 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#7]-2018-August", "y": 28523.56 }, { "name": "[#3]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#6]-2018-September", "y": 0 }, { "name": "[#7]-2018-September", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1013_Obligated",
                "data": [{ "name": "[#1]-2018-September", "y": 8635.42 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 17778.8 }, { "name": "[#4]-2018-October", "y": 37081.49 }, { "name": "[#1]-2018-November", "y": 0 }, { "name": "[#5]-2018-November", "y": 9143.38 }, { "name": "[#6]-2018-November", "y": 26414.21 }, { "name": "[#7]-2018-November", "y": 16762.87 }, { "name": "[#4]-2018-December", "y": 0 }, { "name": "[#5]-2018-December", "y": 0 }, { "name": "[#8]-2018-December", "y": 18794.72 }, { "name": "[#8]-2019-January", "y": 0 }, { "name": "[#3]-2019-March", "y": 0 }, { "name": "[#7]-2019-March", "y": 0 }, { "name": "[#6]-2019-April", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "August-2018_Obligated",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 30004.26, "drilldown": "EJEC_14_Obligated" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 28523.56, "drilldown": "EJEC_11_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_14_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Obligated" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1008_Obligated",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 22860.39 }, { "name": "[#3]-2018-August", "y": 7143.87 }, { "name": "[#2]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 31433.03 }, { "name": "[#5]-2018-September", "y": 14287.74 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#5]-2018-November", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1009_Obligated",
                "data": [{ "name": "[#1]-2018-November", "y": 21914.42 }, { "name": "[#1]-2018-December", "y": 0 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 21914.42 }, { "name": "[#3]-2019-February", "y": 0 }, { "name": "[#4]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-March", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_11_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Obligated" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1011_Obligated",
                "data": [{ "name": "[#1]-2018-June", "y": 12774.2 }, { "name": "[#2]-2018-June", "y": 21716.14 }, { "name": "[#3]-2018-June", "y": 16606.46 }, { "name": "[#4]-2018-June", "y": 23751.62 }, { "name": "[#5]-2018-June", "y": 8941.94 }, { "name": "[#6]-2018-July", "y": 30658.08 }, { "name": "[#1]-2018-August", "y": 0 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#7]-2018-August", "y": 28523.56 }, { "name": "[#3]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#6]-2018-September", "y": 0 }, { "name": "[#7]-2018-September", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1013_Obligated",
                "data": [{ "name": "[#1]-2018-September", "y": 8635.42 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 17778.8 }, { "name": "[#4]-2018-October", "y": 37081.49 }, { "name": "[#1]-2018-November", "y": 0 }, { "name": "[#5]-2018-November", "y": 9143.38 }, { "name": "[#6]-2018-November", "y": 26414.21 }, { "name": "[#7]-2018-November", "y": 16762.87 }, { "name": "[#4]-2018-December", "y": 0 }, { "name": "[#5]-2018-December", "y": 0 }, { "name": "[#8]-2018-December", "y": 18794.72 }, { "name": "[#8]-2019-January", "y": 0 }, { "name": "[#3]-2019-March", "y": 0 }, { "name": "[#7]-2019-March", "y": 0 }, { "name": "[#6]-2019-April", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "September-2018_Obligated",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 45720.77, "drilldown": "EJEC_14_Obligated" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 8635.42, "drilldown": "EJEC_11_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_14_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Obligated" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1008_Obligated",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 22860.39 }, { "name": "[#3]-2018-August", "y": 7143.87 }, { "name": "[#2]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 31433.03 }, { "name": "[#5]-2018-September", "y": 14287.74 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#5]-2018-November", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1009_Obligated",
                "data": [{ "name": "[#1]-2018-November", "y": 21914.42 }, { "name": "[#1]-2018-December", "y": 0 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 21914.42 }, { "name": "[#3]-2019-February", "y": 0 }, { "name": "[#4]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-March", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_11_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Obligated" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1011_Obligated",
                "data": [{ "name": "[#1]-2018-June", "y": 12774.2 }, { "name": "[#2]-2018-June", "y": 21716.14 }, { "name": "[#3]-2018-June", "y": 16606.46 }, { "name": "[#4]-2018-June", "y": 23751.62 }, { "name": "[#5]-2018-June", "y": 8941.94 }, { "name": "[#6]-2018-July", "y": 30658.08 }, { "name": "[#1]-2018-August", "y": 0 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#7]-2018-August", "y": 28523.56 }, { "name": "[#3]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#6]-2018-September", "y": 0 }, { "name": "[#7]-2018-September", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1013_Obligated",
                "data": [{ "name": "[#1]-2018-September", "y": 8635.42 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 17778.8 }, { "name": "[#4]-2018-October", "y": 37081.49 }, { "name": "[#1]-2018-November", "y": 0 }, { "name": "[#5]-2018-November", "y": 9143.38 }, { "name": "[#6]-2018-November", "y": 26414.21 }, { "name": "[#7]-2018-November", "y": 16762.87 }, { "name": "[#4]-2018-December", "y": 0 }, { "name": "[#5]-2018-December", "y": 0 }, { "name": "[#8]-2018-December", "y": 18794.72 }, { "name": "[#8]-2019-January", "y": 0 }, { "name": "[#3]-2019-March", "y": 0 }, { "name": "[#7]-2019-March", "y": 0 }, { "name": "[#6]-2019-April", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "FY2_Obligated",
                "data": [{ "name": "October-2018", "y": 82798.4, "drilldown": "October-2018_Obligated" }, { "name": "November-2018", "y": 74234.88, "drilldown": "November-2018_Obligated" }, { "name": "December-2018", "y": 40709.14, "drilldown": "December-2018_Obligated" }, { "name": "January-2019", "y": 21914.42, "drilldown": "January-2019_Obligated" }, { "name": "February-2019", "y": 40096.12, "drilldown": "February-2019_Obligated" }, { "name": "March-2019", "y": 39352.05, "drilldown": "March-2019_Obligated" }, { "name": "April-2019", "y": 6435.43, "drilldown": "April-2019_Obligated" }, { "name": "May-2019", "y": 26375.57, "drilldown": "May-2019_Obligated" }, { "name": "June-2019", "y": 7722.51, "drilldown": "June-2019_Obligated" }, { "name": "July-2019", "y": 108950.92, "drilldown": "July-2019_Obligated" }, { "name": "August-2019", "y": 41953.49, "drilldown": "August-2019_Obligated" }, { "name": "September-2019", "y": 128357.02, "drilldown": "September-2019_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "October-2018_Obligated",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 0, "drilldown": "EJEC_14_Obligated" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 82798.4, "drilldown": "EJEC_11_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_14_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Obligated" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1008_Obligated",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 22860.39 }, { "name": "[#3]-2018-August", "y": 7143.87 }, { "name": "[#2]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 31433.03 }, { "name": "[#5]-2018-September", "y": 14287.74 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#5]-2018-November", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1009_Obligated",
                "data": [{ "name": "[#1]-2018-November", "y": 21914.42 }, { "name": "[#1]-2018-December", "y": 0 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 21914.42 }, { "name": "[#3]-2019-February", "y": 0 }, { "name": "[#4]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-March", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_11_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Obligated" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1011_Obligated",
                "data": [{ "name": "[#1]-2018-June", "y": 12774.2 }, { "name": "[#2]-2018-June", "y": 21716.14 }, { "name": "[#3]-2018-June", "y": 16606.46 }, { "name": "[#4]-2018-June", "y": 23751.62 }, { "name": "[#5]-2018-June", "y": 8941.94 }, { "name": "[#6]-2018-July", "y": 30658.08 }, { "name": "[#1]-2018-August", "y": 0 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#7]-2018-August", "y": 28523.56 }, { "name": "[#3]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#6]-2018-September", "y": 0 }, { "name": "[#7]-2018-September", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1013_Obligated",
                "data": [{ "name": "[#1]-2018-September", "y": 8635.42 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 17778.8 }, { "name": "[#4]-2018-October", "y": 37081.49 }, { "name": "[#1]-2018-November", "y": 0 }, { "name": "[#5]-2018-November", "y": 9143.38 }, { "name": "[#6]-2018-November", "y": 26414.21 }, { "name": "[#7]-2018-November", "y": 16762.87 }, { "name": "[#4]-2018-December", "y": 0 }, { "name": "[#5]-2018-December", "y": 0 }, { "name": "[#8]-2018-December", "y": 18794.72 }, { "name": "[#8]-2019-January", "y": 0 }, { "name": "[#3]-2019-March", "y": 0 }, { "name": "[#7]-2019-March", "y": 0 }, { "name": "[#6]-2019-April", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "November-2018_Obligated",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 21914.42, "drilldown": "EJEC_14_Obligated" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 52320.46, "drilldown": "EJEC_11_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_14_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Obligated" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1008_Obligated",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 22860.39 }, { "name": "[#3]-2018-August", "y": 7143.87 }, { "name": "[#2]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 31433.03 }, { "name": "[#5]-2018-September", "y": 14287.74 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#5]-2018-November", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1009_Obligated",
                "data": [{ "name": "[#1]-2018-November", "y": 21914.42 }, { "name": "[#1]-2018-December", "y": 0 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 21914.42 }, { "name": "[#3]-2019-February", "y": 0 }, { "name": "[#4]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-March", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_11_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Obligated" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1011_Obligated",
                "data": [{ "name": "[#1]-2018-June", "y": 12774.2 }, { "name": "[#2]-2018-June", "y": 21716.14 }, { "name": "[#3]-2018-June", "y": 16606.46 }, { "name": "[#4]-2018-June", "y": 23751.62 }, { "name": "[#5]-2018-June", "y": 8941.94 }, { "name": "[#6]-2018-July", "y": 30658.08 }, { "name": "[#1]-2018-August", "y": 0 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#7]-2018-August", "y": 28523.56 }, { "name": "[#3]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#6]-2018-September", "y": 0 }, { "name": "[#7]-2018-September", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1013_Obligated",
                "data": [{ "name": "[#1]-2018-September", "y": 8635.42 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 17778.8 }, { "name": "[#4]-2018-October", "y": 37081.49 }, { "name": "[#1]-2018-November", "y": 0 }, { "name": "[#5]-2018-November", "y": 9143.38 }, { "name": "[#6]-2018-November", "y": 26414.21 }, { "name": "[#7]-2018-November", "y": 16762.87 }, { "name": "[#4]-2018-December", "y": 0 }, { "name": "[#5]-2018-December", "y": 0 }, { "name": "[#8]-2018-December", "y": 18794.72 }, { "name": "[#8]-2019-January", "y": 0 }, { "name": "[#3]-2019-March", "y": 0 }, { "name": "[#7]-2019-March", "y": 0 }, { "name": "[#6]-2019-April", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "December-2018_Obligated",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 21914.42, "drilldown": "EJEC_14_Obligated" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 18794.72, "drilldown": "EJEC_11_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_14_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Obligated" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1008_Obligated",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 22860.39 }, { "name": "[#3]-2018-August", "y": 7143.87 }, { "name": "[#2]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 31433.03 }, { "name": "[#5]-2018-September", "y": 14287.74 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#5]-2018-November", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1009_Obligated",
                "data": [{ "name": "[#1]-2018-November", "y": 21914.42 }, { "name": "[#1]-2018-December", "y": 0 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 21914.42 }, { "name": "[#3]-2019-February", "y": 0 }, { "name": "[#4]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-March", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_11_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Obligated" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1011_Obligated",
                "data": [{ "name": "[#1]-2018-June", "y": 12774.2 }, { "name": "[#2]-2018-June", "y": 21716.14 }, { "name": "[#3]-2018-June", "y": 16606.46 }, { "name": "[#4]-2018-June", "y": 23751.62 }, { "name": "[#5]-2018-June", "y": 8941.94 }, { "name": "[#6]-2018-July", "y": 30658.08 }, { "name": "[#1]-2018-August", "y": 0 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#7]-2018-August", "y": 28523.56 }, { "name": "[#3]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#6]-2018-September", "y": 0 }, { "name": "[#7]-2018-September", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1013_Obligated",
                "data": [{ "name": "[#1]-2018-September", "y": 8635.42 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 17778.8 }, { "name": "[#4]-2018-October", "y": 37081.49 }, { "name": "[#1]-2018-November", "y": 0 }, { "name": "[#5]-2018-November", "y": 9143.38 }, { "name": "[#6]-2018-November", "y": 26414.21 }, { "name": "[#7]-2018-November", "y": 16762.87 }, { "name": "[#4]-2018-December", "y": 0 }, { "name": "[#5]-2018-December", "y": 0 }, { "name": "[#8]-2018-December", "y": 18794.72 }, { "name": "[#8]-2019-January", "y": 0 }, { "name": "[#3]-2019-March", "y": 0 }, { "name": "[#7]-2019-March", "y": 0 }, { "name": "[#6]-2019-April", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "January-2019_Obligated",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 21914.42, "drilldown": "EJEC_14_Obligated" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 0, "drilldown": "EJEC_11_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_14_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Obligated" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1008_Obligated",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 22860.39 }, { "name": "[#3]-2018-August", "y": 7143.87 }, { "name": "[#2]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 31433.03 }, { "name": "[#5]-2018-September", "y": 14287.74 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#5]-2018-November", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1009_Obligated",
                "data": [{ "name": "[#1]-2018-November", "y": 21914.42 }, { "name": "[#1]-2018-December", "y": 0 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 21914.42 }, { "name": "[#3]-2019-February", "y": 0 }, { "name": "[#4]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-March", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_11_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Obligated" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1011_Obligated",
                "data": [{ "name": "[#1]-2018-June", "y": 12774.2 }, { "name": "[#2]-2018-June", "y": 21716.14 }, { "name": "[#3]-2018-June", "y": 16606.46 }, { "name": "[#4]-2018-June", "y": 23751.62 }, { "name": "[#5]-2018-June", "y": 8941.94 }, { "name": "[#6]-2018-July", "y": 30658.08 }, { "name": "[#1]-2018-August", "y": 0 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#7]-2018-August", "y": 28523.56 }, { "name": "[#3]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#6]-2018-September", "y": 0 }, { "name": "[#7]-2018-September", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1013_Obligated",
                "data": [{ "name": "[#1]-2018-September", "y": 8635.42 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 17778.8 }, { "name": "[#4]-2018-October", "y": 37081.49 }, { "name": "[#1]-2018-November", "y": 0 }, { "name": "[#5]-2018-November", "y": 9143.38 }, { "name": "[#6]-2018-November", "y": 26414.21 }, { "name": "[#7]-2018-November", "y": 16762.87 }, { "name": "[#4]-2018-December", "y": 0 }, { "name": "[#5]-2018-December", "y": 0 }, { "name": "[#8]-2018-December", "y": 18794.72 }, { "name": "[#8]-2019-January", "y": 0 }, { "name": "[#3]-2019-March", "y": 0 }, { "name": "[#7]-2019-March", "y": 0 }, { "name": "[#6]-2019-April", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "February-2019_Obligated",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 21914.42, "drilldown": "EJEC_14_Obligated" }, { "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 18181.7, "drilldown": "EJEC_7_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_14_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Obligated" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1008_Obligated",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 22860.39 }, { "name": "[#3]-2018-August", "y": 7143.87 }, { "name": "[#2]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 31433.03 }, { "name": "[#5]-2018-September", "y": 14287.74 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#5]-2018-November", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1009_Obligated",
                "data": [{ "name": "[#1]-2018-November", "y": 21914.42 }, { "name": "[#1]-2018-December", "y": 0 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 21914.42 }, { "name": "[#3]-2019-February", "y": 0 }, { "name": "[#4]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-March", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_7_Obligated",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 119941.95, "drilldown": "PROY_2_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_2_Obligated",
                "data": [{ "name": "[#1]-2019-February", "y": 18181.7 }, { "name": "[#2]-2019-March", "y": 30342.45 }, { "name": "[#1]-2019-April", "y": 0 }, { "name": "[#2]-2019-April", "y": 0 }, { "name": "[#3]-2019-May", "y": 23801.4 }, { "name": "[#4]-2019-July", "y": 23801.4 }, { "name": "[#5]-2019-October", "y": 11907.5 }, { "name": "[#6]-2019-December", "y": 11907.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "March-2019_Obligated",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 0, "drilldown": "EJEC_14_Obligated" }, { "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 30342.45, "drilldown": "EJEC_7_Obligated" }, { "name": "CORPORACIÓN PARA LA GESTIÓN AMBIENTAL BIODIVERSA", "y": 9009.6, "drilldown": "EJEC_8_Obligated" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 0, "drilldown": "EJEC_11_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_14_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Obligated" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1008_Obligated",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 22860.39 }, { "name": "[#3]-2018-August", "y": 7143.87 }, { "name": "[#2]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 31433.03 }, { "name": "[#5]-2018-September", "y": 14287.74 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#5]-2018-November", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1009_Obligated",
                "data": [{ "name": "[#1]-2018-November", "y": 21914.42 }, { "name": "[#1]-2018-December", "y": 0 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 21914.42 }, { "name": "[#3]-2019-February", "y": 0 }, { "name": "[#4]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-March", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_7_Obligated",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 119941.95, "drilldown": "PROY_2_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_2_Obligated",
                "data": [{ "name": "[#1]-2019-February", "y": 18181.7 }, { "name": "[#2]-2019-March", "y": 30342.45 }, { "name": "[#1]-2019-April", "y": 0 }, { "name": "[#2]-2019-April", "y": 0 }, { "name": "[#3]-2019-May", "y": 23801.4 }, { "name": "[#4]-2019-July", "y": 23801.4 }, { "name": "[#5]-2019-October", "y": 11907.5 }, { "name": "[#6]-2019-December", "y": 11907.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_8_Obligated",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-001", "y": 25741.71, "drilldown": "PROY_1003_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1003_Obligated",
                "data": [{ "name": "[#1]-2019-March", "y": 2574.17 }, { "name": "[#2]-2019-March", "y": 6435.43 }, { "name": "[#3]-2019-April", "y": 6435.43 }, { "name": "[#4]-2019-May", "y": 2574.17 }, { "name": "[#5]-2019-June", "y": 7722.51 }, { "name": "[#2]-2019-September", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_11_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Obligated" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1011_Obligated",
                "data": [{ "name": "[#1]-2018-June", "y": 12774.2 }, { "name": "[#2]-2018-June", "y": 21716.14 }, { "name": "[#3]-2018-June", "y": 16606.46 }, { "name": "[#4]-2018-June", "y": 23751.62 }, { "name": "[#5]-2018-June", "y": 8941.94 }, { "name": "[#6]-2018-July", "y": 30658.08 }, { "name": "[#1]-2018-August", "y": 0 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#7]-2018-August", "y": 28523.56 }, { "name": "[#3]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#6]-2018-September", "y": 0 }, { "name": "[#7]-2018-September", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1013_Obligated",
                "data": [{ "name": "[#1]-2018-September", "y": 8635.42 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 17778.8 }, { "name": "[#4]-2018-October", "y": 37081.49 }, { "name": "[#1]-2018-November", "y": 0 }, { "name": "[#5]-2018-November", "y": 9143.38 }, { "name": "[#6]-2018-November", "y": 26414.21 }, { "name": "[#7]-2018-November", "y": 16762.87 }, { "name": "[#4]-2018-December", "y": 0 }, { "name": "[#5]-2018-December", "y": 0 }, { "name": "[#8]-2018-December", "y": 18794.72 }, { "name": "[#8]-2019-January", "y": 0 }, { "name": "[#3]-2019-March", "y": 0 }, { "name": "[#7]-2019-March", "y": 0 }, { "name": "[#6]-2019-April", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "April-2019_Obligated",
                "data": [{ "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 0, "drilldown": "EJEC_7_Obligated" }, { "name": "CORPORACIÓN PARA LA GESTIÓN AMBIENTAL BIODIVERSA", "y": 6435.43, "drilldown": "EJEC_8_Obligated" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 0, "drilldown": "EJEC_11_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_7_Obligated",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 119941.95, "drilldown": "PROY_2_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_2_Obligated",
                "data": [{ "name": "[#1]-2019-February", "y": 18181.7 }, { "name": "[#2]-2019-March", "y": 30342.45 }, { "name": "[#1]-2019-April", "y": 0 }, { "name": "[#2]-2019-April", "y": 0 }, { "name": "[#3]-2019-May", "y": 23801.4 }, { "name": "[#4]-2019-July", "y": 23801.4 }, { "name": "[#5]-2019-October", "y": 11907.5 }, { "name": "[#6]-2019-December", "y": 11907.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_8_Obligated",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-001", "y": 25741.71, "drilldown": "PROY_1003_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1003_Obligated",
                "data": [{ "name": "[#1]-2019-March", "y": 2574.17 }, { "name": "[#2]-2019-March", "y": 6435.43 }, { "name": "[#3]-2019-April", "y": 6435.43 }, { "name": "[#4]-2019-May", "y": 2574.17 }, { "name": "[#5]-2019-June", "y": 7722.51 }, { "name": "[#2]-2019-September", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_11_Obligated",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Obligated" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1011_Obligated",
                "data": [{ "name": "[#1]-2018-June", "y": 12774.2 }, { "name": "[#2]-2018-June", "y": 21716.14 }, { "name": "[#3]-2018-June", "y": 16606.46 }, { "name": "[#4]-2018-June", "y": 23751.62 }, { "name": "[#5]-2018-June", "y": 8941.94 }, { "name": "[#6]-2018-July", "y": 30658.08 }, { "name": "[#1]-2018-August", "y": 0 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#7]-2018-August", "y": 28523.56 }, { "name": "[#3]-2018-September", "y": 0 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#6]-2018-September", "y": 0 }, { "name": "[#7]-2018-September", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1013_Obligated",
                "data": [{ "name": "[#1]-2018-September", "y": 8635.42 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 17778.8 }, { "name": "[#4]-2018-October", "y": 37081.49 }, { "name": "[#1]-2018-November", "y": 0 }, { "name": "[#5]-2018-November", "y": 9143.38 }, { "name": "[#6]-2018-November", "y": 26414.21 }, { "name": "[#7]-2018-November", "y": 16762.87 }, { "name": "[#4]-2018-December", "y": 0 }, { "name": "[#5]-2018-December", "y": 0 }, { "name": "[#8]-2018-December", "y": 18794.72 }, { "name": "[#8]-2019-January", "y": 0 }, { "name": "[#3]-2019-March", "y": 0 }, { "name": "[#7]-2019-March", "y": 0 }, { "name": "[#6]-2019-April", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "May-2019_Obligated",
                "data": [{ "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 23801.4, "drilldown": "EJEC_7_Obligated" }, { "name": "CORPORACIÓN PARA LA GESTIÓN AMBIENTAL BIODIVERSA", "y": 2574.17, "drilldown": "EJEC_8_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_7_Obligated",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 119941.95, "drilldown": "PROY_2_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_2_Obligated",
                "data": [{ "name": "[#1]-2019-February", "y": 18181.7 }, { "name": "[#2]-2019-March", "y": 30342.45 }, { "name": "[#1]-2019-April", "y": 0 }, { "name": "[#2]-2019-April", "y": 0 }, { "name": "[#3]-2019-May", "y": 23801.4 }, { "name": "[#4]-2019-July", "y": 23801.4 }, { "name": "[#5]-2019-October", "y": 11907.5 }, { "name": "[#6]-2019-December", "y": 11907.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_8_Obligated",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-001", "y": 25741.71, "drilldown": "PROY_1003_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1003_Obligated",
                "data": [{ "name": "[#1]-2019-March", "y": 2574.17 }, { "name": "[#2]-2019-March", "y": 6435.43 }, { "name": "[#3]-2019-April", "y": 6435.43 }, { "name": "[#4]-2019-May", "y": 2574.17 }, { "name": "[#5]-2019-June", "y": 7722.51 }, { "name": "[#2]-2019-September", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "June-2019_Obligated",
                "data": [{ "name": "CORPORACIÓN PARA LA GESTIÓN AMBIENTAL BIODIVERSA", "y": 7722.51, "drilldown": "EJEC_8_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_8_Obligated",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-001", "y": 25741.71, "drilldown": "PROY_1003_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1003_Obligated",
                "data": [{ "name": "[#1]-2019-March", "y": 2574.17 }, { "name": "[#2]-2019-March", "y": 6435.43 }, { "name": "[#3]-2019-April", "y": 6435.43 }, { "name": "[#4]-2019-May", "y": 2574.17 }, { "name": "[#5]-2019-June", "y": 7722.51 }, { "name": "[#2]-2019-September", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "July-2019_Obligated",
                "data": [{ "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 23801.4, "drilldown": "EJEC_7_Obligated" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 33427.77, "drilldown": "EJEC_13_Obligated" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 11494.75, "drilldown": "EJEC_12_Obligated" }, { "name": "CORPORACIÓN SEMILLAS DE AGUA ", "y": 40227, "drilldown": "EJEC_9_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_7_Obligated",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 119941.95, "drilldown": "PROY_2_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_2_Obligated",
                "data": [{ "name": "[#1]-2019-February", "y": 18181.7 }, { "name": "[#2]-2019-March", "y": 30342.45 }, { "name": "[#1]-2019-April", "y": 0 }, { "name": "[#2]-2019-April", "y": 0 }, { "name": "[#3]-2019-May", "y": 23801.4 }, { "name": "[#4]-2019-July", "y": 23801.4 }, { "name": "[#5]-2019-October", "y": 11907.5 }, { "name": "[#6]-2019-December", "y": 11907.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_13_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 167157.46, "drilldown": "PROY_1006_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1006_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 33427.77 }, { "name": "[#2]-2019-September", "y": 33427.77 }, { "name": "[#3]-2019-December", "y": 33427.77 }, { "name": "[#4]-2020-February", "y": 30098.02 }, { "name": "[#5]-2020-April", "y": 20052.94 }, { "name": "[#6]-2020-June", "y": 16723.19 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_12_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 114642.16, "drilldown": "PROY_1005_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1005_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 11494.75 }, { "name": "[#2]-2019-August", "y": 34499.5 }, { "name": "[#3]-2019-October", "y": 19554.81 }, { "name": "[#4]-2020-January", "y": 19188.44 }, { "name": "[#5]-2020-March", "y": 18409.91 }, { "name": "[#6]-2020-June", "y": 11494.75 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_9_Obligated",
                "data": [{ "name": "PF-AP-2019-GNT-001", "y": 201135, "drilldown": "PROY_1004_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1004_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 40227 }, { "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-September", "y": 40227 }, { "name": "[#3]-2019-December", "y": 50283.75 }, { "name": "[#4]-2020-March", "y": 50283.75 }, { "name": "[#5]-2020-June", "y": 20113.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "August-2019_Obligated",
                "data": [{ "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 34499.5, "drilldown": "EJEC_12_Obligated" }, { "name": "PONTIFICIA UNIVERSIDAD JAVERIANA", "y": 7453.99, "drilldown": "EJEC_15_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_12_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 114642.16, "drilldown": "PROY_1005_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1005_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 11494.75 }, { "name": "[#2]-2019-August", "y": 34499.5 }, { "name": "[#3]-2019-October", "y": 19554.81 }, { "name": "[#4]-2020-January", "y": 19188.44 }, { "name": "[#5]-2020-March", "y": 18409.91 }, { "name": "[#6]-2020-June", "y": 11494.75 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_15_Obligated",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-002", "y": 74539.88, "drilldown": "PROY_1012_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1012_Obligated",
                "data": [{ "name": "[#1]-2019-August", "y": 7453.99 }, { "name": "[#2]-2019-September", "y": 7453.99 }, { "name": "[#3]-2019-October", "y": 11180.98 }, { "name": "[#4]-2019-November", "y": 16771.47 }, { "name": "[#5]-2019-November", "y": 16771.47 }, { "name": "[#6]-2019-November", "y": 14907.98 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "September-2019_Obligated",
                "data": [{ "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 22959.36, "drilldown": "EJEC_16_Obligated" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 33427.77, "drilldown": "EJEC_13_Obligated" }, { "name": "CORPORACIÓN PARA LA GESTIÓN AMBIENTAL BIODIVERSA", "y": 0, "drilldown": "EJEC_8_Obligated" }, { "name": "CORPORACIÓN SEMILLAS DE AGUA ", "y": 40227, "drilldown": "EJEC_9_Obligated" }, { "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 24288.9, "drilldown": "EJEC_17_Obligated" }, { "name": "PONTIFICIA UNIVERSIDAD JAVERIANA", "y": 7453.99, "drilldown": "EJEC_15_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_16_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 184069.44, "drilldown": "PROY_1014_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1014_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 22959.36 }, { "name": "[#2]-2019-October", "y": 54411.33 }, { "name": "[#3]-2019-December", "y": 46748.49 }, { "name": "[#4]-2020-February", "y": 25237.66 }, { "name": "[#5]-2020-May", "y": 20797.88 }, { "name": "[#6]-2020-August", "y": 13914.72 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_13_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 167157.46, "drilldown": "PROY_1006_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1006_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 33427.77 }, { "name": "[#2]-2019-September", "y": 33427.77 }, { "name": "[#3]-2019-December", "y": 33427.77 }, { "name": "[#4]-2020-February", "y": 30098.02 }, { "name": "[#5]-2020-April", "y": 20052.94 }, { "name": "[#6]-2020-June", "y": 16723.19 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_8_Obligated",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-001", "y": 25741.71, "drilldown": "PROY_1003_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1003_Obligated",
                "data": [{ "name": "[#1]-2019-March", "y": 2574.17 }, { "name": "[#2]-2019-March", "y": 6435.43 }, { "name": "[#3]-2019-April", "y": 6435.43 }, { "name": "[#4]-2019-May", "y": 2574.17 }, { "name": "[#5]-2019-June", "y": 7722.51 }, { "name": "[#2]-2019-September", "y": 0 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_9_Obligated",
                "data": [{ "name": "PF-AP-2019-GNT-001", "y": 201135, "drilldown": "PROY_1004_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1004_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 40227 }, { "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-September", "y": 40227 }, { "name": "[#3]-2019-December", "y": 50283.75 }, { "name": "[#4]-2020-March", "y": 50283.75 }, { "name": "[#5]-2020-June", "y": 20113.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_17_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 132941.05, "drilldown": "PROY_1015_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1015_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 24288.9 }, { "name": "[#2]-2019-November", "y": 28195.9 }, { "name": "[#3]-2020-February", "y": 38748.8 }, { "name": "[#4]-2020-May", "y": 26515.62 }, { "name": "[#5]-2020-July", "y": 13071.58 }, { "name": "[#6]-2020-August", "y": 2120.25 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_15_Obligated",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-002", "y": 74539.88, "drilldown": "PROY_1012_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1012_Obligated",
                "data": [{ "name": "[#1]-2019-August", "y": 7453.99 }, { "name": "[#2]-2019-September", "y": 7453.99 }, { "name": "[#3]-2019-October", "y": 11180.98 }, { "name": "[#4]-2019-November", "y": 16771.47 }, { "name": "[#5]-2019-November", "y": 16771.47 }, { "name": "[#6]-2019-November", "y": 14907.98 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "FY3_Obligated",
                "data": [{ "name": "October-2019", "y": 181584.03, "drilldown": "October-2019_Obligated" }, { "name": "November-2019", "y": 140250.89, "drilldown": "November-2019_Obligated" }, { "name": "December-2019", "y": 169503.87, "drilldown": "December-2019_Obligated" }, { "name": "January-2020", "y": 19188.44, "drilldown": "January-2020_Obligated" }, { "name": "February-2020", "y": 94084.48, "drilldown": "February-2020_Obligated" }, { "name": "March-2020", "y": 101588.4, "drilldown": "March-2020_Obligated" }, { "name": "April-2020", "y": 20052.94, "drilldown": "April-2020_Obligated" }, { "name": "May-2020", "y": 47313.5, "drilldown": "May-2020_Obligated" }, { "name": "June-2020", "y": 48331.44, "drilldown": "June-2020_Obligated" }, { "name": "July-2020", "y": 13071.58, "drilldown": "July-2020_Obligated" }, { "name": "August-2020", "y": 16034.97, "drilldown": "August-2020_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "October-2019_Obligated",
                "data": [{ "name": "AENOR INTERNATIONAL  S.A.U.", "y": 11000, "drilldown": "EJEC_18_Obligated" }, { "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 54411.33, "drilldown": "EJEC_16_Obligated" }, { "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 11907.5, "drilldown": "EJEC_7_Obligated" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 19554.81, "drilldown": "EJEC_12_Obligated" }, { "name": "PÁRAMOS Y BOSQUES (CHEMONICS INTERNATIONAL INC.)", "y": 73529.41, "drilldown": "EJEC_5_Obligated" }, { "name": "PONTIFICIA UNIVERSIDAD JAVERIANA", "y": 11180.98, "drilldown": "EJEC_15_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_18_Obligated",
                "data": [{ "name": "LIF-SUB-FFP-BP-2020-001", "y": 11000, "drilldown": "PROY_1016_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1016_Obligated",
                "data": [{ "name": "[#1]-2019-October", "y": 11000 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_16_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 184069.44, "drilldown": "PROY_1014_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1014_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 22959.36 }, { "name": "[#2]-2019-October", "y": 54411.33 }, { "name": "[#3]-2019-December", "y": 46748.49 }, { "name": "[#4]-2020-February", "y": 25237.66 }, { "name": "[#5]-2020-May", "y": 20797.88 }, { "name": "[#6]-2020-August", "y": 13914.72 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_7_Obligated",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 119941.95, "drilldown": "PROY_2_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_2_Obligated",
                "data": [{ "name": "[#1]-2019-February", "y": 18181.7 }, { "name": "[#2]-2019-March", "y": 30342.45 }, { "name": "[#1]-2019-April", "y": 0 }, { "name": "[#2]-2019-April", "y": 0 }, { "name": "[#3]-2019-May", "y": 23801.4 }, { "name": "[#4]-2019-July", "y": 23801.4 }, { "name": "[#5]-2019-October", "y": 11907.5 }, { "name": "[#6]-2019-December", "y": 11907.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_12_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 114642.16, "drilldown": "PROY_1005_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1005_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 11494.75 }, { "name": "[#2]-2019-August", "y": 34499.5 }, { "name": "[#3]-2019-October", "y": 19554.81 }, { "name": "[#4]-2020-January", "y": 19188.44 }, { "name": "[#5]-2020-March", "y": 18409.91 }, { "name": "[#6]-2020-June", "y": 11494.75 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_5_Obligated",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001-FP-001-POU-001", "y": 73529.41, "drilldown": "PROY_1018_Obligated" }, { "name": "LIF-GRA-FP-BP-2020-001-FP-001-INK-002", "y": 32894.74, "drilldown": "PROY_1019_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1018_Obligated",
                "data": [{ "name": "[#1]-2019-October", "y": 73529.41 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1019_Obligated",
                "data": [{ "name": "[#1]-2020-March", "y": 32894.74 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_15_Obligated",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-002", "y": 74539.88, "drilldown": "PROY_1012_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1012_Obligated",
                "data": [{ "name": "[#1]-2019-August", "y": 7453.99 }, { "name": "[#2]-2019-September", "y": 7453.99 }, { "name": "[#3]-2019-October", "y": 11180.98 }, { "name": "[#4]-2019-November", "y": 16771.47 }, { "name": "[#5]-2019-November", "y": 16771.47 }, { "name": "[#6]-2019-November", "y": 14907.98 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "November-2019_Obligated",
                "data": [{ "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 28195.9, "drilldown": "EJEC_17_Obligated" }, { "name": "FUNDACIÓN PROCUENCA RÍO LAS PIEDRAS", "y": 63604.07, "drilldown": "EJEC_19_Obligated" }, { "name": "PONTIFICIA UNIVERSIDAD JAVERIANA", "y": 48450.92, "drilldown": "EJEC_15_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_17_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 132941.05, "drilldown": "PROY_1015_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1015_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 24288.9 }, { "name": "[#2]-2019-November", "y": 28195.9 }, { "name": "[#3]-2020-February", "y": 38748.8 }, { "name": "[#4]-2020-May", "y": 26515.62 }, { "name": "[#5]-2020-July", "y": 13071.58 }, { "name": "[#6]-2020-August", "y": 2120.25 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_19_Obligated",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001-FP-001", "y": 90740.43, "drilldown": "PROY_1017_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1017_Obligated",
                "data": [{ "name": "[#1]-2019-November", "y": 25725.28 }, { "name": "[#3]-2019-November", "y": 37878.79 }, { "name": "[#2]-2019-December", "y": 13636.36 }, { "name": "[#4]-2019-December", "y": 13500 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_15_Obligated",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-002", "y": 74539.88, "drilldown": "PROY_1012_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1012_Obligated",
                "data": [{ "name": "[#1]-2019-August", "y": 7453.99 }, { "name": "[#2]-2019-September", "y": 7453.99 }, { "name": "[#3]-2019-October", "y": 11180.98 }, { "name": "[#4]-2019-November", "y": 16771.47 }, { "name": "[#5]-2019-November", "y": 16771.47 }, { "name": "[#6]-2019-November", "y": 14907.98 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "December-2019_Obligated",
                "data": [{ "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 46748.49, "drilldown": "EJEC_16_Obligated" }, { "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 11907.5, "drilldown": "EJEC_7_Obligated" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 33427.77, "drilldown": "EJEC_13_Obligated" }, { "name": "CORPORACIÓN SEMILLAS DE AGUA ", "y": 50283.75, "drilldown": "EJEC_9_Obligated" }, { "name": "FUNDACIÓN PROCUENCA RÍO LAS PIEDRAS", "y": 27136.36, "drilldown": "EJEC_19_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_16_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 184069.44, "drilldown": "PROY_1014_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1014_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 22959.36 }, { "name": "[#2]-2019-October", "y": 54411.33 }, { "name": "[#3]-2019-December", "y": 46748.49 }, { "name": "[#4]-2020-February", "y": 25237.66 }, { "name": "[#5]-2020-May", "y": 20797.88 }, { "name": "[#6]-2020-August", "y": 13914.72 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_7_Obligated",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 119941.95, "drilldown": "PROY_2_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_2_Obligated",
                "data": [{ "name": "[#1]-2019-February", "y": 18181.7 }, { "name": "[#2]-2019-March", "y": 30342.45 }, { "name": "[#1]-2019-April", "y": 0 }, { "name": "[#2]-2019-April", "y": 0 }, { "name": "[#3]-2019-May", "y": 23801.4 }, { "name": "[#4]-2019-July", "y": 23801.4 }, { "name": "[#5]-2019-October", "y": 11907.5 }, { "name": "[#6]-2019-December", "y": 11907.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_13_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 167157.46, "drilldown": "PROY_1006_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1006_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 33427.77 }, { "name": "[#2]-2019-September", "y": 33427.77 }, { "name": "[#3]-2019-December", "y": 33427.77 }, { "name": "[#4]-2020-February", "y": 30098.02 }, { "name": "[#5]-2020-April", "y": 20052.94 }, { "name": "[#6]-2020-June", "y": 16723.19 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_9_Obligated",
                "data": [{ "name": "PF-AP-2019-GNT-001", "y": 201135, "drilldown": "PROY_1004_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1004_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 40227 }, { "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-September", "y": 40227 }, { "name": "[#3]-2019-December", "y": 50283.75 }, { "name": "[#4]-2020-March", "y": 50283.75 }, { "name": "[#5]-2020-June", "y": 20113.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_19_Obligated",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001-FP-001", "y": 90740.43, "drilldown": "PROY_1017_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1017_Obligated",
                "data": [{ "name": "[#1]-2019-November", "y": 25725.28 }, { "name": "[#3]-2019-November", "y": 37878.79 }, { "name": "[#2]-2019-December", "y": 13636.36 }, { "name": "[#4]-2019-December", "y": 13500 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "January-2020_Obligated",
                "data": [{ "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 19188.44, "drilldown": "EJEC_12_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_12_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 114642.16, "drilldown": "PROY_1005_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1005_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 11494.75 }, { "name": "[#2]-2019-August", "y": 34499.5 }, { "name": "[#3]-2019-October", "y": 19554.81 }, { "name": "[#4]-2020-January", "y": 19188.44 }, { "name": "[#5]-2020-March", "y": 18409.91 }, { "name": "[#6]-2020-June", "y": 11494.75 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "February-2020_Obligated",
                "data": [{ "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 25237.66, "drilldown": "EJEC_16_Obligated" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 30098.02, "drilldown": "EJEC_13_Obligated" }, { "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 38748.8, "drilldown": "EJEC_17_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_16_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 184069.44, "drilldown": "PROY_1014_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1014_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 22959.36 }, { "name": "[#2]-2019-October", "y": 54411.33 }, { "name": "[#3]-2019-December", "y": 46748.49 }, { "name": "[#4]-2020-February", "y": 25237.66 }, { "name": "[#5]-2020-May", "y": 20797.88 }, { "name": "[#6]-2020-August", "y": 13914.72 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_13_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 167157.46, "drilldown": "PROY_1006_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1006_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 33427.77 }, { "name": "[#2]-2019-September", "y": 33427.77 }, { "name": "[#3]-2019-December", "y": 33427.77 }, { "name": "[#4]-2020-February", "y": 30098.02 }, { "name": "[#5]-2020-April", "y": 20052.94 }, { "name": "[#6]-2020-June", "y": 16723.19 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_17_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 132941.05, "drilldown": "PROY_1015_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1015_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 24288.9 }, { "name": "[#2]-2019-November", "y": 28195.9 }, { "name": "[#3]-2020-February", "y": 38748.8 }, { "name": "[#4]-2020-May", "y": 26515.62 }, { "name": "[#5]-2020-July", "y": 13071.58 }, { "name": "[#6]-2020-August", "y": 2120.25 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "March-2020_Obligated",
                "data": [{ "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 18409.91, "drilldown": "EJEC_12_Obligated" }, { "name": "CORPORACIÓN SEMILLAS DE AGUA ", "y": 50283.75, "drilldown": "EJEC_9_Obligated" }, { "name": "PÁRAMOS Y BOSQUES (CHEMONICS INTERNATIONAL INC.)", "y": 32894.74, "drilldown": "EJEC_5_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_12_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 114642.16, "drilldown": "PROY_1005_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1005_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 11494.75 }, { "name": "[#2]-2019-August", "y": 34499.5 }, { "name": "[#3]-2019-October", "y": 19554.81 }, { "name": "[#4]-2020-January", "y": 19188.44 }, { "name": "[#5]-2020-March", "y": 18409.91 }, { "name": "[#6]-2020-June", "y": 11494.75 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_9_Obligated",
                "data": [{ "name": "PF-AP-2019-GNT-001", "y": 201135, "drilldown": "PROY_1004_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1004_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 40227 }, { "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-September", "y": 40227 }, { "name": "[#3]-2019-December", "y": 50283.75 }, { "name": "[#4]-2020-March", "y": 50283.75 }, { "name": "[#5]-2020-June", "y": 20113.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_5_Obligated",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001-FP-001-POU-001", "y": 73529.41, "drilldown": "PROY_1018_Obligated" }, { "name": "LIF-GRA-FP-BP-2020-001-FP-001-INK-002", "y": 32894.74, "drilldown": "PROY_1019_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1018_Obligated",
                "data": [{ "name": "[#1]-2019-October", "y": 73529.41 }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1019_Obligated",
                "data": [{ "name": "[#1]-2020-March", "y": 32894.74 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "April-2020_Obligated",
                "data": [{ "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 20052.94, "drilldown": "EJEC_13_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_13_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 167157.46, "drilldown": "PROY_1006_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1006_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 33427.77 }, { "name": "[#2]-2019-September", "y": 33427.77 }, { "name": "[#3]-2019-December", "y": 33427.77 }, { "name": "[#4]-2020-February", "y": 30098.02 }, { "name": "[#5]-2020-April", "y": 20052.94 }, { "name": "[#6]-2020-June", "y": 16723.19 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "May-2020_Obligated",
                "data": [{ "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 20797.88, "drilldown": "EJEC_16_Obligated" }, { "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 26515.62, "drilldown": "EJEC_17_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_16_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 184069.44, "drilldown": "PROY_1014_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1014_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 22959.36 }, { "name": "[#2]-2019-October", "y": 54411.33 }, { "name": "[#3]-2019-December", "y": 46748.49 }, { "name": "[#4]-2020-February", "y": 25237.66 }, { "name": "[#5]-2020-May", "y": 20797.88 }, { "name": "[#6]-2020-August", "y": 13914.72 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_17_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 132941.05, "drilldown": "PROY_1015_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1015_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 24288.9 }, { "name": "[#2]-2019-November", "y": 28195.9 }, { "name": "[#3]-2020-February", "y": 38748.8 }, { "name": "[#4]-2020-May", "y": 26515.62 }, { "name": "[#5]-2020-July", "y": 13071.58 }, { "name": "[#6]-2020-August", "y": 2120.25 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "June-2020_Obligated",
                "data": [{ "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 16723.19, "drilldown": "EJEC_13_Obligated" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 11494.75, "drilldown": "EJEC_12_Obligated" }, { "name": "CORPORACIÓN SEMILLAS DE AGUA ", "y": 20113.5, "drilldown": "EJEC_9_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_13_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 167157.46, "drilldown": "PROY_1006_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1006_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 33427.77 }, { "name": "[#2]-2019-September", "y": 33427.77 }, { "name": "[#3]-2019-December", "y": 33427.77 }, { "name": "[#4]-2020-February", "y": 30098.02 }, { "name": "[#5]-2020-April", "y": 20052.94 }, { "name": "[#6]-2020-June", "y": 16723.19 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_12_Obligated",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 114642.16, "drilldown": "PROY_1005_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1005_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 11494.75 }, { "name": "[#2]-2019-August", "y": 34499.5 }, { "name": "[#3]-2019-October", "y": 19554.81 }, { "name": "[#4]-2020-January", "y": 19188.44 }, { "name": "[#5]-2020-March", "y": 18409.91 }, { "name": "[#6]-2020-June", "y": 11494.75 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_9_Obligated",
                "data": [{ "name": "PF-AP-2019-GNT-001", "y": 201135, "drilldown": "PROY_1004_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1004_Obligated",
                "data": [{ "name": "[#1]-2019-July", "y": 40227 }, { "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-September", "y": 40227 }, { "name": "[#3]-2019-December", "y": 50283.75 }, { "name": "[#4]-2020-March", "y": 50283.75 }, { "name": "[#5]-2020-June", "y": 20113.5 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "July-2020_Obligated",
                "data": [{ "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 13071.58, "drilldown": "EJEC_17_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_17_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 132941.05, "drilldown": "PROY_1015_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1015_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 24288.9 }, { "name": "[#2]-2019-November", "y": 28195.9 }, { "name": "[#3]-2020-February", "y": 38748.8 }, { "name": "[#4]-2020-May", "y": 26515.62 }, { "name": "[#5]-2020-July", "y": 13071.58 }, { "name": "[#6]-2020-August", "y": 2120.25 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "August-2020_Obligated",
                "data": [{ "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 13914.72, "drilldown": "EJEC_16_Obligated" }, { "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 2120.25, "drilldown": "EJEC_17_Obligated" }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_16_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 184069.44, "drilldown": "PROY_1014_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1014_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 22959.36 }, { "name": "[#2]-2019-October", "y": 54411.33 }, { "name": "[#3]-2019-December", "y": 46748.49 }, { "name": "[#4]-2020-February", "y": 25237.66 }, { "name": "[#5]-2020-May", "y": 20797.88 }, { "name": "[#6]-2020-August", "y": 13914.72 }]
            }, {
                "type": "column",
                "name": "Obligated",
                "id": "EJEC_17_Obligated",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 132941.05, "drilldown": "PROY_1015_Obligated" }]
            }, {
                "type": "areaspline",
                "name": "Obligated",
                "id": "PROY_1015_Obligated",
                "data": [{ "name": "[#1]-2019-September", "y": 24288.9 }, { "name": "[#2]-2019-November", "y": 28195.9 }, { "name": "[#3]-2020-February", "y": 38748.8 }, { "name": "[#4]-2020-May", "y": 26515.62 }, { "name": "[#5]-2020-July", "y": 13071.58 }, { "name": "[#6]-2020-August", "y": 2120.25 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "FY1_Disbursed",
                "data": [{ "name": "June-2018", "y": 0, "drilldown": "June-2018_Disbursed" }, { "name": "July-2018", "y": 10001.42, "drilldown": "July-2018_Disbursed" }, { "name": "August-2018", "y": 34490.34, "drilldown": "August-2018_Disbursed" }, { "name": "September-2018", "y": 131342.05, "drilldown": "September-2018_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "June-2018_Disbursed",
                "data": [{ "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 0, "drilldown": "EJEC_11_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_11_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Disbursed" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1011_Disbursed",
                "data": [{ "name": "[#1]-2018-June", "y": 0 }, { "name": "[#2]-2018-June", "y": 0 }, { "name": "[#3]-2018-June", "y": 0 }, { "name": "[#4]-2018-June", "y": 0 }, { "name": "[#5]-2018-June", "y": 0 }, { "name": "[#6]-2018-July", "y": 0 }, { "name": "[#1]-2018-August", "y": 12774.2 }, { "name": "[#2]-2018-August", "y": 21716.14 }, { "name": "[#7]-2018-August", "y": 0 }, { "name": "[#3]-2018-September", "y": 16606.46 }, { "name": "[#4]-2018-September", "y": 23751.62 }, { "name": "[#5]-2018-September", "y": 8941.94 }, { "name": "[#6]-2018-September", "y": 30658.08 }, { "name": "[#7]-2018-September", "y": 28523.56 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1013_Disbursed",
                "data": [{ "name": "[#1]-2018-September", "y": 0 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#1]-2018-November", "y": 8635.42 }, { "name": "[#5]-2018-November", "y": 0 }, { "name": "[#6]-2018-November", "y": 0 }, { "name": "[#7]-2018-November", "y": 0 }, { "name": "[#4]-2018-December", "y": 37081.49 }, { "name": "[#5]-2018-December", "y": 9143.38 }, { "name": "[#8]-2018-December", "y": 0 }, { "name": "[#8]-2019-January", "y": 18794.72 }, { "name": "[#3]-2019-March", "y": 17778.8 }, { "name": "[#7]-2019-March", "y": 16762.87 }, { "name": "[#6]-2019-April", "y": 26414.21 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "July-2018_Disbursed",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 10001.42, "drilldown": "EJEC_14_Disbursed" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 0, "drilldown": "EJEC_11_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_14_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Disbursed" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1008_Disbursed",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#3]-2018-August", "y": 0 }, { "name": "[#2]-2018-September", "y": 22860.39 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#3]-2018-October", "y": 7143.87 }, { "name": "[#4]-2018-October", "y": 31433.03 }, { "name": "[#5]-2018-November", "y": 14287.74 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1009_Disbursed",
                "data": [{ "name": "[#1]-2018-November", "y": 0 }, { "name": "[#1]-2018-December", "y": 21914.42 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 0 }, { "name": "[#3]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-February", "y": 0 }, { "name": "[#4]-2019-March", "y": 21914.42 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_11_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Disbursed" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1011_Disbursed",
                "data": [{ "name": "[#1]-2018-June", "y": 0 }, { "name": "[#2]-2018-June", "y": 0 }, { "name": "[#3]-2018-June", "y": 0 }, { "name": "[#4]-2018-June", "y": 0 }, { "name": "[#5]-2018-June", "y": 0 }, { "name": "[#6]-2018-July", "y": 0 }, { "name": "[#1]-2018-August", "y": 12774.2 }, { "name": "[#2]-2018-August", "y": 21716.14 }, { "name": "[#7]-2018-August", "y": 0 }, { "name": "[#3]-2018-September", "y": 16606.46 }, { "name": "[#4]-2018-September", "y": 23751.62 }, { "name": "[#5]-2018-September", "y": 8941.94 }, { "name": "[#6]-2018-September", "y": 30658.08 }, { "name": "[#7]-2018-September", "y": 28523.56 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1013_Disbursed",
                "data": [{ "name": "[#1]-2018-September", "y": 0 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#1]-2018-November", "y": 8635.42 }, { "name": "[#5]-2018-November", "y": 0 }, { "name": "[#6]-2018-November", "y": 0 }, { "name": "[#7]-2018-November", "y": 0 }, { "name": "[#4]-2018-December", "y": 37081.49 }, { "name": "[#5]-2018-December", "y": 9143.38 }, { "name": "[#8]-2018-December", "y": 0 }, { "name": "[#8]-2019-January", "y": 18794.72 }, { "name": "[#3]-2019-March", "y": 17778.8 }, { "name": "[#7]-2019-March", "y": 16762.87 }, { "name": "[#6]-2019-April", "y": 26414.21 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "August-2018_Disbursed",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 0, "drilldown": "EJEC_14_Disbursed" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 34490.34, "drilldown": "EJEC_11_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_14_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Disbursed" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1008_Disbursed",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#3]-2018-August", "y": 0 }, { "name": "[#2]-2018-September", "y": 22860.39 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#3]-2018-October", "y": 7143.87 }, { "name": "[#4]-2018-October", "y": 31433.03 }, { "name": "[#5]-2018-November", "y": 14287.74 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1009_Disbursed",
                "data": [{ "name": "[#1]-2018-November", "y": 0 }, { "name": "[#1]-2018-December", "y": 21914.42 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 0 }, { "name": "[#3]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-February", "y": 0 }, { "name": "[#4]-2019-March", "y": 21914.42 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_11_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Disbursed" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1011_Disbursed",
                "data": [{ "name": "[#1]-2018-June", "y": 0 }, { "name": "[#2]-2018-June", "y": 0 }, { "name": "[#3]-2018-June", "y": 0 }, { "name": "[#4]-2018-June", "y": 0 }, { "name": "[#5]-2018-June", "y": 0 }, { "name": "[#6]-2018-July", "y": 0 }, { "name": "[#1]-2018-August", "y": 12774.2 }, { "name": "[#2]-2018-August", "y": 21716.14 }, { "name": "[#7]-2018-August", "y": 0 }, { "name": "[#3]-2018-September", "y": 16606.46 }, { "name": "[#4]-2018-September", "y": 23751.62 }, { "name": "[#5]-2018-September", "y": 8941.94 }, { "name": "[#6]-2018-September", "y": 30658.08 }, { "name": "[#7]-2018-September", "y": 28523.56 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1013_Disbursed",
                "data": [{ "name": "[#1]-2018-September", "y": 0 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#1]-2018-November", "y": 8635.42 }, { "name": "[#5]-2018-November", "y": 0 }, { "name": "[#6]-2018-November", "y": 0 }, { "name": "[#7]-2018-November", "y": 0 }, { "name": "[#4]-2018-December", "y": 37081.49 }, { "name": "[#5]-2018-December", "y": 9143.38 }, { "name": "[#8]-2018-December", "y": 0 }, { "name": "[#8]-2019-January", "y": 18794.72 }, { "name": "[#3]-2019-March", "y": 17778.8 }, { "name": "[#7]-2019-March", "y": 16762.87 }, { "name": "[#6]-2019-April", "y": 26414.21 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "September-2018_Disbursed",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 22860.39, "drilldown": "EJEC_14_Disbursed" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 108481.66, "drilldown": "EJEC_11_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_14_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Disbursed" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1008_Disbursed",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#3]-2018-August", "y": 0 }, { "name": "[#2]-2018-September", "y": 22860.39 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#3]-2018-October", "y": 7143.87 }, { "name": "[#4]-2018-October", "y": 31433.03 }, { "name": "[#5]-2018-November", "y": 14287.74 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1009_Disbursed",
                "data": [{ "name": "[#1]-2018-November", "y": 0 }, { "name": "[#1]-2018-December", "y": 21914.42 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 0 }, { "name": "[#3]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-February", "y": 0 }, { "name": "[#4]-2019-March", "y": 21914.42 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_11_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Disbursed" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1011_Disbursed",
                "data": [{ "name": "[#1]-2018-June", "y": 0 }, { "name": "[#2]-2018-June", "y": 0 }, { "name": "[#3]-2018-June", "y": 0 }, { "name": "[#4]-2018-June", "y": 0 }, { "name": "[#5]-2018-June", "y": 0 }, { "name": "[#6]-2018-July", "y": 0 }, { "name": "[#1]-2018-August", "y": 12774.2 }, { "name": "[#2]-2018-August", "y": 21716.14 }, { "name": "[#7]-2018-August", "y": 0 }, { "name": "[#3]-2018-September", "y": 16606.46 }, { "name": "[#4]-2018-September", "y": 23751.62 }, { "name": "[#5]-2018-September", "y": 8941.94 }, { "name": "[#6]-2018-September", "y": 30658.08 }, { "name": "[#7]-2018-September", "y": 28523.56 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1013_Disbursed",
                "data": [{ "name": "[#1]-2018-September", "y": 0 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#1]-2018-November", "y": 8635.42 }, { "name": "[#5]-2018-November", "y": 0 }, { "name": "[#6]-2018-November", "y": 0 }, { "name": "[#7]-2018-November", "y": 0 }, { "name": "[#4]-2018-December", "y": 37081.49 }, { "name": "[#5]-2018-December", "y": 9143.38 }, { "name": "[#8]-2018-December", "y": 0 }, { "name": "[#8]-2019-January", "y": 18794.72 }, { "name": "[#3]-2019-March", "y": 17778.8 }, { "name": "[#7]-2019-March", "y": 16762.87 }, { "name": "[#6]-2019-April", "y": 26414.21 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "FY2_Disbursed",
                "data": [{ "name": "October-2018", "y": 66515.01, "drilldown": "October-2018_Disbursed" }, { "name": "November-2018", "y": 22923.16, "drilldown": "November-2018_Disbursed" }, { "name": "December-2018", "y": 90053.71, "drilldown": "December-2018_Disbursed" }, { "name": "January-2019", "y": 18794.72, "drilldown": "January-2019_Disbursed" }, { "name": "February-2019", "y": 21914.42, "drilldown": "February-2019_Disbursed" }, { "name": "March-2019", "y": 56456.09, "drilldown": "March-2019_Disbursed" }, { "name": "April-2019", "y": 74595.91, "drilldown": "April-2019_Disbursed" }, { "name": "May-2019", "y": 0, "drilldown": "May-2019_Disbursed" }, { "name": "June-2019", "y": 0, "drilldown": "June-2019_Disbursed" }, { "name": "July-2019", "y": 0, "drilldown": "July-2019_Disbursed" }, { "name": "August-2019", "y": 0, "drilldown": "August-2019_Disbursed" }, { "name": "September-2019", "y": 47019.66, "drilldown": "September-2019_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "October-2018_Disbursed",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 38576.9, "drilldown": "EJEC_14_Disbursed" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 27938.11, "drilldown": "EJEC_11_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_14_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Disbursed" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1008_Disbursed",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#3]-2018-August", "y": 0 }, { "name": "[#2]-2018-September", "y": 22860.39 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#3]-2018-October", "y": 7143.87 }, { "name": "[#4]-2018-October", "y": 31433.03 }, { "name": "[#5]-2018-November", "y": 14287.74 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1009_Disbursed",
                "data": [{ "name": "[#1]-2018-November", "y": 0 }, { "name": "[#1]-2018-December", "y": 21914.42 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 0 }, { "name": "[#3]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-February", "y": 0 }, { "name": "[#4]-2019-March", "y": 21914.42 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_11_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Disbursed" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1011_Disbursed",
                "data": [{ "name": "[#1]-2018-June", "y": 0 }, { "name": "[#2]-2018-June", "y": 0 }, { "name": "[#3]-2018-June", "y": 0 }, { "name": "[#4]-2018-June", "y": 0 }, { "name": "[#5]-2018-June", "y": 0 }, { "name": "[#6]-2018-July", "y": 0 }, { "name": "[#1]-2018-August", "y": 12774.2 }, { "name": "[#2]-2018-August", "y": 21716.14 }, { "name": "[#7]-2018-August", "y": 0 }, { "name": "[#3]-2018-September", "y": 16606.46 }, { "name": "[#4]-2018-September", "y": 23751.62 }, { "name": "[#5]-2018-September", "y": 8941.94 }, { "name": "[#6]-2018-September", "y": 30658.08 }, { "name": "[#7]-2018-September", "y": 28523.56 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1013_Disbursed",
                "data": [{ "name": "[#1]-2018-September", "y": 0 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#1]-2018-November", "y": 8635.42 }, { "name": "[#5]-2018-November", "y": 0 }, { "name": "[#6]-2018-November", "y": 0 }, { "name": "[#7]-2018-November", "y": 0 }, { "name": "[#4]-2018-December", "y": 37081.49 }, { "name": "[#5]-2018-December", "y": 9143.38 }, { "name": "[#8]-2018-December", "y": 0 }, { "name": "[#8]-2019-January", "y": 18794.72 }, { "name": "[#3]-2019-March", "y": 17778.8 }, { "name": "[#7]-2019-March", "y": 16762.87 }, { "name": "[#6]-2019-April", "y": 26414.21 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "November-2018_Disbursed",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 14287.74, "drilldown": "EJEC_14_Disbursed" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 8635.42, "drilldown": "EJEC_11_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_14_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Disbursed" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1008_Disbursed",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#3]-2018-August", "y": 0 }, { "name": "[#2]-2018-September", "y": 22860.39 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#3]-2018-October", "y": 7143.87 }, { "name": "[#4]-2018-October", "y": 31433.03 }, { "name": "[#5]-2018-November", "y": 14287.74 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1009_Disbursed",
                "data": [{ "name": "[#1]-2018-November", "y": 0 }, { "name": "[#1]-2018-December", "y": 21914.42 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 0 }, { "name": "[#3]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-February", "y": 0 }, { "name": "[#4]-2019-March", "y": 21914.42 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_11_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Disbursed" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1011_Disbursed",
                "data": [{ "name": "[#1]-2018-June", "y": 0 }, { "name": "[#2]-2018-June", "y": 0 }, { "name": "[#3]-2018-June", "y": 0 }, { "name": "[#4]-2018-June", "y": 0 }, { "name": "[#5]-2018-June", "y": 0 }, { "name": "[#6]-2018-July", "y": 0 }, { "name": "[#1]-2018-August", "y": 12774.2 }, { "name": "[#2]-2018-August", "y": 21716.14 }, { "name": "[#7]-2018-August", "y": 0 }, { "name": "[#3]-2018-September", "y": 16606.46 }, { "name": "[#4]-2018-September", "y": 23751.62 }, { "name": "[#5]-2018-September", "y": 8941.94 }, { "name": "[#6]-2018-September", "y": 30658.08 }, { "name": "[#7]-2018-September", "y": 28523.56 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1013_Disbursed",
                "data": [{ "name": "[#1]-2018-September", "y": 0 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#1]-2018-November", "y": 8635.42 }, { "name": "[#5]-2018-November", "y": 0 }, { "name": "[#6]-2018-November", "y": 0 }, { "name": "[#7]-2018-November", "y": 0 }, { "name": "[#4]-2018-December", "y": 37081.49 }, { "name": "[#5]-2018-December", "y": 9143.38 }, { "name": "[#8]-2018-December", "y": 0 }, { "name": "[#8]-2019-January", "y": 18794.72 }, { "name": "[#3]-2019-March", "y": 17778.8 }, { "name": "[#7]-2019-March", "y": 16762.87 }, { "name": "[#6]-2019-April", "y": 26414.21 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "December-2018_Disbursed",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 43828.84, "drilldown": "EJEC_14_Disbursed" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 46224.87, "drilldown": "EJEC_11_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_14_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Disbursed" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1008_Disbursed",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#3]-2018-August", "y": 0 }, { "name": "[#2]-2018-September", "y": 22860.39 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#3]-2018-October", "y": 7143.87 }, { "name": "[#4]-2018-October", "y": 31433.03 }, { "name": "[#5]-2018-November", "y": 14287.74 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1009_Disbursed",
                "data": [{ "name": "[#1]-2018-November", "y": 0 }, { "name": "[#1]-2018-December", "y": 21914.42 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 0 }, { "name": "[#3]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-February", "y": 0 }, { "name": "[#4]-2019-March", "y": 21914.42 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_11_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Disbursed" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1011_Disbursed",
                "data": [{ "name": "[#1]-2018-June", "y": 0 }, { "name": "[#2]-2018-June", "y": 0 }, { "name": "[#3]-2018-June", "y": 0 }, { "name": "[#4]-2018-June", "y": 0 }, { "name": "[#5]-2018-June", "y": 0 }, { "name": "[#6]-2018-July", "y": 0 }, { "name": "[#1]-2018-August", "y": 12774.2 }, { "name": "[#2]-2018-August", "y": 21716.14 }, { "name": "[#7]-2018-August", "y": 0 }, { "name": "[#3]-2018-September", "y": 16606.46 }, { "name": "[#4]-2018-September", "y": 23751.62 }, { "name": "[#5]-2018-September", "y": 8941.94 }, { "name": "[#6]-2018-September", "y": 30658.08 }, { "name": "[#7]-2018-September", "y": 28523.56 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1013_Disbursed",
                "data": [{ "name": "[#1]-2018-September", "y": 0 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#1]-2018-November", "y": 8635.42 }, { "name": "[#5]-2018-November", "y": 0 }, { "name": "[#6]-2018-November", "y": 0 }, { "name": "[#7]-2018-November", "y": 0 }, { "name": "[#4]-2018-December", "y": 37081.49 }, { "name": "[#5]-2018-December", "y": 9143.38 }, { "name": "[#8]-2018-December", "y": 0 }, { "name": "[#8]-2019-January", "y": 18794.72 }, { "name": "[#3]-2019-March", "y": 17778.8 }, { "name": "[#7]-2019-March", "y": 16762.87 }, { "name": "[#6]-2019-April", "y": 26414.21 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "January-2019_Disbursed",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 0, "drilldown": "EJEC_14_Disbursed" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 18794.72, "drilldown": "EJEC_11_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_14_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Disbursed" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1008_Disbursed",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#3]-2018-August", "y": 0 }, { "name": "[#2]-2018-September", "y": 22860.39 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#3]-2018-October", "y": 7143.87 }, { "name": "[#4]-2018-October", "y": 31433.03 }, { "name": "[#5]-2018-November", "y": 14287.74 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1009_Disbursed",
                "data": [{ "name": "[#1]-2018-November", "y": 0 }, { "name": "[#1]-2018-December", "y": 21914.42 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 0 }, { "name": "[#3]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-February", "y": 0 }, { "name": "[#4]-2019-March", "y": 21914.42 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_11_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Disbursed" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1011_Disbursed",
                "data": [{ "name": "[#1]-2018-June", "y": 0 }, { "name": "[#2]-2018-June", "y": 0 }, { "name": "[#3]-2018-June", "y": 0 }, { "name": "[#4]-2018-June", "y": 0 }, { "name": "[#5]-2018-June", "y": 0 }, { "name": "[#6]-2018-July", "y": 0 }, { "name": "[#1]-2018-August", "y": 12774.2 }, { "name": "[#2]-2018-August", "y": 21716.14 }, { "name": "[#7]-2018-August", "y": 0 }, { "name": "[#3]-2018-September", "y": 16606.46 }, { "name": "[#4]-2018-September", "y": 23751.62 }, { "name": "[#5]-2018-September", "y": 8941.94 }, { "name": "[#6]-2018-September", "y": 30658.08 }, { "name": "[#7]-2018-September", "y": 28523.56 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1013_Disbursed",
                "data": [{ "name": "[#1]-2018-September", "y": 0 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#1]-2018-November", "y": 8635.42 }, { "name": "[#5]-2018-November", "y": 0 }, { "name": "[#6]-2018-November", "y": 0 }, { "name": "[#7]-2018-November", "y": 0 }, { "name": "[#4]-2018-December", "y": 37081.49 }, { "name": "[#5]-2018-December", "y": 9143.38 }, { "name": "[#8]-2018-December", "y": 0 }, { "name": "[#8]-2019-January", "y": 18794.72 }, { "name": "[#3]-2019-March", "y": 17778.8 }, { "name": "[#7]-2019-March", "y": 16762.87 }, { "name": "[#6]-2019-April", "y": 26414.21 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "February-2019_Disbursed",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 21914.42, "drilldown": "EJEC_14_Disbursed" }, { "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 0, "drilldown": "EJEC_7_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_14_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Disbursed" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1008_Disbursed",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#3]-2018-August", "y": 0 }, { "name": "[#2]-2018-September", "y": 22860.39 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#3]-2018-October", "y": 7143.87 }, { "name": "[#4]-2018-October", "y": 31433.03 }, { "name": "[#5]-2018-November", "y": 14287.74 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1009_Disbursed",
                "data": [{ "name": "[#1]-2018-November", "y": 0 }, { "name": "[#1]-2018-December", "y": 21914.42 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 0 }, { "name": "[#3]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-February", "y": 0 }, { "name": "[#4]-2019-March", "y": 21914.42 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_7_Disbursed",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 48181.7, "drilldown": "PROY_2_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_2_Disbursed",
                "data": [{ "name": "[#1]-2019-February", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#1]-2019-April", "y": 18181.7 }, { "name": "[#2]-2019-April", "y": 30000 }, { "name": "[#3]-2019-May", "y": 0 }, { "name": "[#4]-2019-July", "y": 0 }, { "name": "[#5]-2019-October", "y": 0 }, { "name": "[#6]-2019-December", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "March-2019_Disbursed",
                "data": [{ "name": "2M CONSULTORES EN ESTRATEGIA Y DESARROLLO S.A.S. ", "y": 21914.42, "drilldown": "EJEC_14_Disbursed" }, { "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 0, "drilldown": "EJEC_7_Disbursed" }, { "name": "CORPORACIÓN PARA LA GESTIÓN AMBIENTAL BIODIVERSA", "y": 0, "drilldown": "EJEC_8_Disbursed" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 34541.67, "drilldown": "EJEC_11_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_14_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-BP-2018-001-STO-001", "y": 85726.45, "drilldown": "PROY_1008_Disbursed" }, { "name": "LIF-SUB-IQS-BP-2018-001-STO-002", "y": 87657.68, "drilldown": "PROY_1009_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1008_Disbursed",
                "data": [{ "name": "[#1]-2018-July", "y": 10001.42 }, { "name": "[#2]-2018-August", "y": 0 }, { "name": "[#3]-2018-August", "y": 0 }, { "name": "[#2]-2018-September", "y": 22860.39 }, { "name": "[#4]-2018-September", "y": 0 }, { "name": "[#5]-2018-September", "y": 0 }, { "name": "[#3]-2018-October", "y": 7143.87 }, { "name": "[#4]-2018-October", "y": 31433.03 }, { "name": "[#5]-2018-November", "y": 14287.74 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1009_Disbursed",
                "data": [{ "name": "[#1]-2018-November", "y": 0 }, { "name": "[#1]-2018-December", "y": 21914.42 }, { "name": "[#2]-2018-December", "y": 21914.42 }, { "name": "[#3]-2019-January", "y": 0 }, { "name": "[#3]-2019-February", "y": 21914.42 }, { "name": "[#4]-2019-February", "y": 0 }, { "name": "[#4]-2019-March", "y": 21914.42 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_7_Disbursed",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 48181.7, "drilldown": "PROY_2_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_2_Disbursed",
                "data": [{ "name": "[#1]-2019-February", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#1]-2019-April", "y": 18181.7 }, { "name": "[#2]-2019-April", "y": 30000 }, { "name": "[#3]-2019-May", "y": 0 }, { "name": "[#4]-2019-July", "y": 0 }, { "name": "[#5]-2019-October", "y": 0 }, { "name": "[#6]-2019-December", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_8_Disbursed",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-001", "y": 6488.03, "drilldown": "PROY_1003_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1003_Disbursed",
                "data": [{ "name": "[#1]-2019-March", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#3]-2019-April", "y": 0 }, { "name": "[#4]-2019-May", "y": 0 }, { "name": "[#5]-2019-June", "y": 0 }, { "name": "[#2]-2019-September", "y": 6488.03 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_11_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Disbursed" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1011_Disbursed",
                "data": [{ "name": "[#1]-2018-June", "y": 0 }, { "name": "[#2]-2018-June", "y": 0 }, { "name": "[#3]-2018-June", "y": 0 }, { "name": "[#4]-2018-June", "y": 0 }, { "name": "[#5]-2018-June", "y": 0 }, { "name": "[#6]-2018-July", "y": 0 }, { "name": "[#1]-2018-August", "y": 12774.2 }, { "name": "[#2]-2018-August", "y": 21716.14 }, { "name": "[#7]-2018-August", "y": 0 }, { "name": "[#3]-2018-September", "y": 16606.46 }, { "name": "[#4]-2018-September", "y": 23751.62 }, { "name": "[#5]-2018-September", "y": 8941.94 }, { "name": "[#6]-2018-September", "y": 30658.08 }, { "name": "[#7]-2018-September", "y": 28523.56 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1013_Disbursed",
                "data": [{ "name": "[#1]-2018-September", "y": 0 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#1]-2018-November", "y": 8635.42 }, { "name": "[#5]-2018-November", "y": 0 }, { "name": "[#6]-2018-November", "y": 0 }, { "name": "[#7]-2018-November", "y": 0 }, { "name": "[#4]-2018-December", "y": 37081.49 }, { "name": "[#5]-2018-December", "y": 9143.38 }, { "name": "[#8]-2018-December", "y": 0 }, { "name": "[#8]-2019-January", "y": 18794.72 }, { "name": "[#3]-2019-March", "y": 17778.8 }, { "name": "[#7]-2019-March", "y": 16762.87 }, { "name": "[#6]-2019-April", "y": 26414.21 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "April-2019_Disbursed",
                "data": [{ "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 48181.7, "drilldown": "EJEC_7_Disbursed" }, { "name": "CORPORACIÓN PARA LA GESTIÓN AMBIENTAL BIODIVERSA", "y": 0, "drilldown": "EJEC_8_Disbursed" }, { "name": "ECOLOGICAL CARBON OFFSET PARTNERS", "y": 26414.21, "drilldown": "EJEC_11_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_7_Disbursed",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 48181.7, "drilldown": "PROY_2_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_2_Disbursed",
                "data": [{ "name": "[#1]-2019-February", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#1]-2019-April", "y": 18181.7 }, { "name": "[#2]-2019-April", "y": 30000 }, { "name": "[#3]-2019-May", "y": 0 }, { "name": "[#4]-2019-July", "y": 0 }, { "name": "[#5]-2019-October", "y": 0 }, { "name": "[#6]-2019-December", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_8_Disbursed",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-001", "y": 6488.03, "drilldown": "PROY_1003_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1003_Disbursed",
                "data": [{ "name": "[#1]-2019-March", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#3]-2019-April", "y": 0 }, { "name": "[#4]-2019-May", "y": 0 }, { "name": "[#5]-2019-June", "y": 0 }, { "name": "[#2]-2019-September", "y": 6488.03 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_11_Disbursed",
                "data": [{ "name": "LIF-SUB-IQS-2018-002-STO-001", "y": 142972, "drilldown": "PROY_1011_Disbursed" }, { "name": "LIF-SUB-IQS-2018-002-STO-002", "y": 162549, "drilldown": "PROY_1013_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1011_Disbursed",
                "data": [{ "name": "[#1]-2018-June", "y": 0 }, { "name": "[#2]-2018-June", "y": 0 }, { "name": "[#3]-2018-June", "y": 0 }, { "name": "[#4]-2018-June", "y": 0 }, { "name": "[#5]-2018-June", "y": 0 }, { "name": "[#6]-2018-July", "y": 0 }, { "name": "[#1]-2018-August", "y": 12774.2 }, { "name": "[#2]-2018-August", "y": 21716.14 }, { "name": "[#7]-2018-August", "y": 0 }, { "name": "[#3]-2018-September", "y": 16606.46 }, { "name": "[#4]-2018-September", "y": 23751.62 }, { "name": "[#5]-2018-September", "y": 8941.94 }, { "name": "[#6]-2018-September", "y": 30658.08 }, { "name": "[#7]-2018-September", "y": 28523.56 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1013_Disbursed",
                "data": [{ "name": "[#1]-2018-September", "y": 0 }, { "name": "[#2]-2018-October", "y": 27938.11 }, { "name": "[#3]-2018-October", "y": 0 }, { "name": "[#4]-2018-October", "y": 0 }, { "name": "[#1]-2018-November", "y": 8635.42 }, { "name": "[#5]-2018-November", "y": 0 }, { "name": "[#6]-2018-November", "y": 0 }, { "name": "[#7]-2018-November", "y": 0 }, { "name": "[#4]-2018-December", "y": 37081.49 }, { "name": "[#5]-2018-December", "y": 9143.38 }, { "name": "[#8]-2018-December", "y": 0 }, { "name": "[#8]-2019-January", "y": 18794.72 }, { "name": "[#3]-2019-March", "y": 17778.8 }, { "name": "[#7]-2019-March", "y": 16762.87 }, { "name": "[#6]-2019-April", "y": 26414.21 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "May-2019_Disbursed",
                "data": [{ "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 0, "drilldown": "EJEC_7_Disbursed" }, { "name": "CORPORACIÓN PARA LA GESTIÓN AMBIENTAL BIODIVERSA", "y": 0, "drilldown": "EJEC_8_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_7_Disbursed",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 48181.7, "drilldown": "PROY_2_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_2_Disbursed",
                "data": [{ "name": "[#1]-2019-February", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#1]-2019-April", "y": 18181.7 }, { "name": "[#2]-2019-April", "y": 30000 }, { "name": "[#3]-2019-May", "y": 0 }, { "name": "[#4]-2019-July", "y": 0 }, { "name": "[#5]-2019-October", "y": 0 }, { "name": "[#6]-2019-December", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_8_Disbursed",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-001", "y": 6488.03, "drilldown": "PROY_1003_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1003_Disbursed",
                "data": [{ "name": "[#1]-2019-March", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#3]-2019-April", "y": 0 }, { "name": "[#4]-2019-May", "y": 0 }, { "name": "[#5]-2019-June", "y": 0 }, { "name": "[#2]-2019-September", "y": 6488.03 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "June-2019_Disbursed",
                "data": [{ "name": "CORPORACIÓN PARA LA GESTIÓN AMBIENTAL BIODIVERSA", "y": 0, "drilldown": "EJEC_8_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_8_Disbursed",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-001", "y": 6488.03, "drilldown": "PROY_1003_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1003_Disbursed",
                "data": [{ "name": "[#1]-2019-March", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#3]-2019-April", "y": 0 }, { "name": "[#4]-2019-May", "y": 0 }, { "name": "[#5]-2019-June", "y": 0 }, { "name": "[#2]-2019-September", "y": 6488.03 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "July-2019_Disbursed",
                "data": [{ "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 0, "drilldown": "EJEC_7_Disbursed" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 0, "drilldown": "EJEC_13_Disbursed" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 0, "drilldown": "EJEC_12_Disbursed" }, { "name": "CORPORACIÓN SEMILLAS DE AGUA ", "y": 0, "drilldown": "EJEC_9_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_7_Disbursed",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 48181.7, "drilldown": "PROY_2_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_2_Disbursed",
                "data": [{ "name": "[#1]-2019-February", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#1]-2019-April", "y": 18181.7 }, { "name": "[#2]-2019-April", "y": 30000 }, { "name": "[#3]-2019-May", "y": 0 }, { "name": "[#4]-2019-July", "y": 0 }, { "name": "[#5]-2019-October", "y": 0 }, { "name": "[#6]-2019-December", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_13_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 0, "drilldown": "PROY_1006_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1006_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-April", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_12_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 0, "drilldown": "PROY_1005_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1005_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-August", "y": 0 }, { "name": "[#3]-2019-October", "y": 0 }, { "name": "[#4]-2020-January", "y": 0 }, { "name": "[#5]-2020-March", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_9_Disbursed",
                "data": [{ "name": "PF-AP-2019-GNT-001", "y": 40531.63, "drilldown": "PROY_1004_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1004_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#1]-2019-September", "y": 40531.63 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-March", "y": 0 }, { "name": "[#5]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "August-2019_Disbursed",
                "data": [{ "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 0, "drilldown": "EJEC_12_Disbursed" }, { "name": "PONTIFICIA UNIVERSIDAD JAVERIANA", "y": 0, "drilldown": "EJEC_15_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_12_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 0, "drilldown": "PROY_1005_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1005_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-August", "y": 0 }, { "name": "[#3]-2019-October", "y": 0 }, { "name": "[#4]-2020-January", "y": 0 }, { "name": "[#5]-2020-March", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_15_Disbursed",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-002", "y": 0, "drilldown": "PROY_1012_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1012_Disbursed",
                "data": [{ "name": "[#1]-2019-August", "y": 0 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-October", "y": 0 }, { "name": "[#4]-2019-November", "y": 0 }, { "name": "[#5]-2019-November", "y": 0 }, { "name": "[#6]-2019-November", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "September-2019_Disbursed",
                "data": [{ "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 0, "drilldown": "EJEC_16_Disbursed" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 0, "drilldown": "EJEC_13_Disbursed" }, { "name": "CORPORACIÓN PARA LA GESTIÓN AMBIENTAL BIODIVERSA", "y": 6488.03, "drilldown": "EJEC_8_Disbursed" }, { "name": "CORPORACIÓN SEMILLAS DE AGUA ", "y": 40531.63, "drilldown": "EJEC_9_Disbursed" }, { "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 0, "drilldown": "EJEC_17_Disbursed" }, { "name": "PONTIFICIA UNIVERSIDAD JAVERIANA", "y": 0, "drilldown": "EJEC_15_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_16_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 0, "drilldown": "PROY_1014_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1014_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-October", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-May", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_13_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 0, "drilldown": "PROY_1006_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1006_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-April", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_8_Disbursed",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-001", "y": 6488.03, "drilldown": "PROY_1003_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1003_Disbursed",
                "data": [{ "name": "[#1]-2019-March", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#3]-2019-April", "y": 0 }, { "name": "[#4]-2019-May", "y": 0 }, { "name": "[#5]-2019-June", "y": 0 }, { "name": "[#2]-2019-September", "y": 6488.03 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_9_Disbursed",
                "data": [{ "name": "PF-AP-2019-GNT-001", "y": 40531.63, "drilldown": "PROY_1004_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1004_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#1]-2019-September", "y": 40531.63 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-March", "y": 0 }, { "name": "[#5]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_17_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 0, "drilldown": "PROY_1015_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1015_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-November", "y": 0 }, { "name": "[#3]-2020-February", "y": 0 }, { "name": "[#4]-2020-May", "y": 0 }, { "name": "[#5]-2020-July", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_15_Disbursed",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-002", "y": 0, "drilldown": "PROY_1012_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1012_Disbursed",
                "data": [{ "name": "[#1]-2019-August", "y": 0 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-October", "y": 0 }, { "name": "[#4]-2019-November", "y": 0 }, { "name": "[#5]-2019-November", "y": 0 }, { "name": "[#6]-2019-November", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "FY3_Disbursed",
                "data": [{ "name": "October-2019", "y": 0, "drilldown": "October-2019_Disbursed" }, { "name": "November-2019", "y": 0, "drilldown": "November-2019_Disbursed" }, { "name": "December-2019", "y": 0, "drilldown": "December-2019_Disbursed" }, { "name": "January-2020", "y": 0, "drilldown": "January-2020_Disbursed" }, { "name": "February-2020", "y": 0, "drilldown": "February-2020_Disbursed" }, { "name": "March-2020", "y": 0, "drilldown": "March-2020_Disbursed" }, { "name": "April-2020", "y": 0, "drilldown": "April-2020_Disbursed" }, { "name": "May-2020", "y": 0, "drilldown": "May-2020_Disbursed" }, { "name": "June-2020", "y": 0, "drilldown": "June-2020_Disbursed" }, { "name": "July-2020", "y": 0, "drilldown": "July-2020_Disbursed" }, { "name": "August-2020", "y": 0, "drilldown": "August-2020_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "October-2019_Disbursed",
                "data": [{ "name": "AENOR INTERNATIONAL  S.A.U.", "y": 0, "drilldown": "EJEC_18_Disbursed" }, { "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 0, "drilldown": "EJEC_16_Disbursed" }, { "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 0, "drilldown": "EJEC_7_Disbursed" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 0, "drilldown": "EJEC_12_Disbursed" }, { "name": "PÁRAMOS Y BOSQUES (CHEMONICS INTERNATIONAL INC.)", "y": 0, "drilldown": "EJEC_5_Disbursed" }, { "name": "PONTIFICIA UNIVERSIDAD JAVERIANA", "y": 0, "drilldown": "EJEC_15_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_18_Disbursed",
                "data": [{ "name": "LIF-SUB-FFP-BP-2020-001", "y": 0, "drilldown": "PROY_1016_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1016_Disbursed",
                "data": [{ "name": "[#1]-2019-October", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_16_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 0, "drilldown": "PROY_1014_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1014_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-October", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-May", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_7_Disbursed",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 48181.7, "drilldown": "PROY_2_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_2_Disbursed",
                "data": [{ "name": "[#1]-2019-February", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#1]-2019-April", "y": 18181.7 }, { "name": "[#2]-2019-April", "y": 30000 }, { "name": "[#3]-2019-May", "y": 0 }, { "name": "[#4]-2019-July", "y": 0 }, { "name": "[#5]-2019-October", "y": 0 }, { "name": "[#6]-2019-December", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_12_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 0, "drilldown": "PROY_1005_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1005_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-August", "y": 0 }, { "name": "[#3]-2019-October", "y": 0 }, { "name": "[#4]-2020-January", "y": 0 }, { "name": "[#5]-2020-March", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_5_Disbursed",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001-FP-001-POU-001", "y": 0, "drilldown": "PROY_1018_Disbursed" }, { "name": "LIF-GRA-FP-BP-2020-001-FP-001-INK-002", "y": 0, "drilldown": "PROY_1019_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1018_Disbursed",
                "data": [{ "name": "[#1]-2019-October", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1019_Disbursed",
                "data": [{ "name": "[#1]-2020-March", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_15_Disbursed",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-002", "y": 0, "drilldown": "PROY_1012_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1012_Disbursed",
                "data": [{ "name": "[#1]-2019-August", "y": 0 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-October", "y": 0 }, { "name": "[#4]-2019-November", "y": 0 }, { "name": "[#5]-2019-November", "y": 0 }, { "name": "[#6]-2019-November", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "November-2019_Disbursed",
                "data": [{ "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 0, "drilldown": "EJEC_17_Disbursed" }, { "name": "FUNDACIÓN PROCUENCA RÍO LAS PIEDRAS", "y": 0, "drilldown": "EJEC_19_Disbursed" }, { "name": "PONTIFICIA UNIVERSIDAD JAVERIANA", "y": 0, "drilldown": "EJEC_15_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_17_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 0, "drilldown": "PROY_1015_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1015_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-November", "y": 0 }, { "name": "[#3]-2020-February", "y": 0 }, { "name": "[#4]-2020-May", "y": 0 }, { "name": "[#5]-2020-July", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_19_Disbursed",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001-FP-001", "y": 0, "drilldown": "PROY_1017_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1017_Disbursed",
                "data": [{ "name": "[#1]-2019-November", "y": 0 }, { "name": "[#3]-2019-November", "y": 0 }, { "name": "[#2]-2019-December", "y": 0 }, { "name": "[#4]-2019-December", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_15_Disbursed",
                "data": [{ "name": "LIF-SUB-FFP-EAAP-2019-002", "y": 0, "drilldown": "PROY_1012_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1012_Disbursed",
                "data": [{ "name": "[#1]-2019-August", "y": 0 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-October", "y": 0 }, { "name": "[#4]-2019-November", "y": 0 }, { "name": "[#5]-2019-November", "y": 0 }, { "name": "[#6]-2019-November", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "December-2019_Disbursed",
                "data": [{ "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 0, "drilldown": "EJEC_16_Disbursed" }, { "name": "CABILDO MAYOR INDÍGENA DE MUTATÁ", "y": 0, "drilldown": "EJEC_7_Disbursed" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 0, "drilldown": "EJEC_13_Disbursed" }, { "name": "CORPORACIÓN SEMILLAS DE AGUA ", "y": 0, "drilldown": "EJEC_9_Disbursed" }, { "name": "FUNDACIÓN PROCUENCA RÍO LAS PIEDRAS", "y": 0, "drilldown": "EJEC_19_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_16_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 0, "drilldown": "PROY_1014_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1014_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-October", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-May", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_7_Disbursed",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001", "y": 48181.7, "drilldown": "PROY_2_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_2_Disbursed",
                "data": [{ "name": "[#1]-2019-February", "y": 0 }, { "name": "[#2]-2019-March", "y": 0 }, { "name": "[#1]-2019-April", "y": 18181.7 }, { "name": "[#2]-2019-April", "y": 30000 }, { "name": "[#3]-2019-May", "y": 0 }, { "name": "[#4]-2019-July", "y": 0 }, { "name": "[#5]-2019-October", "y": 0 }, { "name": "[#6]-2019-December", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_13_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 0, "drilldown": "PROY_1006_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1006_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-April", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_9_Disbursed",
                "data": [{ "name": "PF-AP-2019-GNT-001", "y": 40531.63, "drilldown": "PROY_1004_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1004_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#1]-2019-September", "y": 40531.63 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-March", "y": 0 }, { "name": "[#5]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_19_Disbursed",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001-FP-001", "y": 0, "drilldown": "PROY_1017_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1017_Disbursed",
                "data": [{ "name": "[#1]-2019-November", "y": 0 }, { "name": "[#3]-2019-November", "y": 0 }, { "name": "[#2]-2019-December", "y": 0 }, { "name": "[#4]-2019-December", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "January-2020_Disbursed",
                "data": [{ "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 0, "drilldown": "EJEC_12_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_12_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 0, "drilldown": "PROY_1005_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1005_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-August", "y": 0 }, { "name": "[#3]-2019-October", "y": 0 }, { "name": "[#4]-2020-January", "y": 0 }, { "name": "[#5]-2020-March", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "February-2020_Disbursed",
                "data": [{ "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 0, "drilldown": "EJEC_16_Disbursed" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 0, "drilldown": "EJEC_13_Disbursed" }, { "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 0, "drilldown": "EJEC_17_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_16_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 0, "drilldown": "PROY_1014_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1014_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-October", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-May", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_13_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 0, "drilldown": "PROY_1006_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1006_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-April", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_17_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 0, "drilldown": "PROY_1015_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1015_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-November", "y": 0 }, { "name": "[#3]-2020-February", "y": 0 }, { "name": "[#4]-2020-May", "y": 0 }, { "name": "[#5]-2020-July", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "March-2020_Disbursed",
                "data": [{ "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 0, "drilldown": "EJEC_12_Disbursed" }, { "name": "CORPORACIÓN SEMILLAS DE AGUA ", "y": 0, "drilldown": "EJEC_9_Disbursed" }, { "name": "PÁRAMOS Y BOSQUES (CHEMONICS INTERNATIONAL INC.)", "y": 0, "drilldown": "EJEC_5_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_12_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 0, "drilldown": "PROY_1005_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1005_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-August", "y": 0 }, { "name": "[#3]-2019-October", "y": 0 }, { "name": "[#4]-2020-January", "y": 0 }, { "name": "[#5]-2020-March", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_9_Disbursed",
                "data": [{ "name": "PF-AP-2019-GNT-001", "y": 40531.63, "drilldown": "PROY_1004_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1004_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#1]-2019-September", "y": 40531.63 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-March", "y": 0 }, { "name": "[#5]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_5_Disbursed",
                "data": [{ "name": "LIF-GRA-FP-BP-2020-001-FP-001-POU-001", "y": 0, "drilldown": "PROY_1018_Disbursed" }, { "name": "LIF-GRA-FP-BP-2020-001-FP-001-INK-002", "y": 0, "drilldown": "PROY_1019_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1018_Disbursed",
                "data": [{ "name": "[#1]-2019-October", "y": 0 }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1019_Disbursed",
                "data": [{ "name": "[#1]-2020-March", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "April-2020_Disbursed",
                "data": [{ "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 0, "drilldown": "EJEC_13_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_13_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 0, "drilldown": "PROY_1006_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1006_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-April", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "May-2020_Disbursed",
                "data": [{ "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 0, "drilldown": "EJEC_16_Disbursed" }, { "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 0, "drilldown": "EJEC_17_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_16_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 0, "drilldown": "PROY_1014_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1014_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-October", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-May", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_17_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 0, "drilldown": "PROY_1015_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1015_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-November", "y": 0 }, { "name": "[#3]-2020-February", "y": 0 }, { "name": "[#4]-2020-May", "y": 0 }, { "name": "[#5]-2020-July", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "June-2020_Disbursed",
                "data": [{ "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA CUENCA BAJA DEL RÍO CALIMA", "y": 0, "drilldown": "EJEC_13_Disbursed" }, { "name": "CONSEJO COMUNITARIO DE LA COMUNIDAD NEGRA DE LA PLATA BAHÍA MÁLAGA", "y": 0, "drilldown": "EJEC_12_Disbursed" }, { "name": "CORPORACIÓN SEMILLAS DE AGUA ", "y": 0, "drilldown": "EJEC_9_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_13_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-003", "y": 0, "drilldown": "PROY_1006_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1006_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-April", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_12_Disbursed",
                "data": [{ "name": "PF-BP-2019-GNT-002", "y": 0, "drilldown": "PROY_1005_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1005_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#2]-2019-August", "y": 0 }, { "name": "[#3]-2019-October", "y": 0 }, { "name": "[#4]-2020-January", "y": 0 }, { "name": "[#5]-2020-March", "y": 0 }, { "name": "[#6]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_9_Disbursed",
                "data": [{ "name": "PF-AP-2019-GNT-001", "y": 40531.63, "drilldown": "PROY_1004_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1004_Disbursed",
                "data": [{ "name": "[#1]-2019-July", "y": 0 }, { "name": "[#1]-2019-September", "y": 40531.63 }, { "name": "[#2]-2019-September", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-March", "y": 0 }, { "name": "[#5]-2020-June", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "July-2020_Disbursed",
                "data": [{ "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 0, "drilldown": "EJEC_17_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_17_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 0, "drilldown": "PROY_1015_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1015_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-November", "y": 0 }, { "name": "[#3]-2020-February", "y": 0 }, { "name": "[#4]-2020-May", "y": 0 }, { "name": "[#5]-2020-July", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "August-2020_Disbursed",
                "data": [{ "name": "ASOCIACIÓN TURISTICA PALMA DE CERA, PÁRAMOS Y PAISAJES", "y": 0, "drilldown": "EJEC_16_Disbursed" }, { "name": "FUNDACIÓN ECOLÓGICA LAS MELLIZAS", "y": 0, "drilldown": "EJEC_17_Disbursed" }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_16_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-002", "y": 0, "drilldown": "PROY_1014_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1014_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-October", "y": 0 }, { "name": "[#3]-2019-December", "y": 0 }, { "name": "[#4]-2020-February", "y": 0 }, { "name": "[#5]-2020-May", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }, {
                "type": "column",
                "name": "Disbursed",
                "id": "EJEC_17_Disbursed",
                "data": [{ "name": "LIF-GRA-EAAP-2019-003", "y": 0, "drilldown": "PROY_1015_Disbursed" }]
            }, {
                "type": "areaspline",
                "name": "Disbursed",
                "id": "PROY_1015_Disbursed",
                "data": [{ "name": "[#1]-2019-September", "y": 0 }, { "name": "[#2]-2019-November", "y": 0 }, { "name": "[#3]-2020-February", "y": 0 }, { "name": "[#4]-2020-May", "y": 0 }, { "name": "[#5]-2020-July", "y": 0 }, { "name": "[#6]-2020-August", "y": 0 }]
            }] 


        } //****************************************************************************************************
        

    });



}




function DELIV_CHART() {

    $('#Deliverables_chart').highcharts({
        title: {
            text: 'YLA Leveraged Funds',
            align: 'left',
            x: 350
        },
        xAxis: {
            type: 'category',
            labels: {
                rotation: 25
            }
        }
        , events: {
            drilldown: function (e) {
                var chart = this,
                    drilldowns = chart.userOptions.drilldown.series,
                    series = [];
                //alert(e.point.name);
                e.preventDefault();
                Highcharts.each(drilldowns, function (p, i) {
                    if (p.id.includes(e.point.name)) {
                        chart.addSingleSeriesAsDrilldown(e.point, p);
                    }
                });
                chart.applyDrilldown();
            }
        }
        , labels: {
            items: [{
                html: 'Deliverables (USD)',
                style: {
                    left: '50px',
                    top: '0px',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'black'
                }
            }]
        },
        legend: {
            align: 'right',
            x: -250,
            verticalAlign: 'top',
            layout: 'vertical',
            y: -5,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y:2f} USD',
                    formatter: function () {
                        console.log(this);
                        var val = this.y;
                        if (val < 1) {
                            return '';
                        }
                        return val;
                    }
                }
            }
        },
        tooltip: {
            headerFormat: '<span style="color:{point.color};font-size:11px">{series.name}</span><br>',
            //pointFormat: '{series.name}: <b>{point.y}</b> ({point.percentage:.1f}%)<br/>'
            //pointFormat: '<span>{point.name}</span>: <b>{point.y:2f} USD</b>'
            pointFormat: '<span>{point.name}</span>: <b>{point.y} USD</b>'
        },
        series: [{
            "type": "column",
            "name": "Do not apply",
            "data": [{ "name": "FY1", "y": 15000, "drilldown": "FY1_2" }, { "name": "FY2", "y": 0, "drilldown": "FY2_2" }, { "name": "FY3", "y": 283770.1, "drilldown": "FY3_2" }]
        }, {
            "type": "column",
            "name": "Local Governments",
            "data": [{ "name": "FY1", "y": 5000, "drilldown": "FY1_8" }, { "name": "FY2", "y": 44.8, "drilldown": "FY2_8" }, { "name": "FY3", "y": 74.67, "drilldown": "FY3_8" }]
        }, {
            "type": "column",
            "name": "Non Govermental Organisation (NGO)",
            "data": [{ "name": "FY1", "y": 37763.78, "drilldown": "FY1_12" }, { "name": "FY2", "y": 0, "drilldown": "FY2_12" }, { "name": "FY3", "y": 280351.21, "drilldown": "FY3_12" }]
        }, {
            "type": "column",
            "name": "Private Enterprises and Industry Associations",
            "data": [{ "name": "FY1", "y": 26127.59, "drilldown": "FY1_1" }, { "name": "FY2", "y": 10572047.14, "drilldown": "FY2_1" }, { "name": "FY3", "y": 2681310.24, "drilldown": "FY3_1" }]
        }, {
            "type": "column",
            "name": "Producer Associations",
            "data": [{ "name": "FY1", "y": 6000, "drilldown": "FY1_3" }, { "name": "FY2", "y": 0, "drilldown": "FY2_3" }, { "name": "FY3", "y": 25194.15, "drilldown": "FY3_3" }]
        }, {
            "type": "spline",
            "name": "YLA Funding",
            "marker": {
                "lineWidth": 2,
                "fillColor": "white"
            },
            "data": [91567.92, 315323.5, 1331890.74]
        }, {
            "type": "pie",
            "name": "Total Funding",
            "center": [100, 60],
            "size": 120,
            "showInLegend": false,
            "dataLabels": {
                "enabled": false
            },
            "data": [{ "name": "Private Funding", "y": 13906564.21 }, { "name": "Project Funding", "y": 1738782.16 }, { "name": "Public Funding", "y": 119.47 }]
        }],
        drilldown: {
            series: [{
                "name": "FY3-Do not apply",
                "id": "FY3_2",
                "type": "column",
                "data": [["Implementer Funding", 283770.1]]
            }, {
                "name": "FY2-Local Governments",
                "id": "FY2_8",
                "type": "column",
                "data": [["Local Funding", 44.8]]
            }, {
                "name": "FY3-Local Governments",
                "id": "FY3_8",
                "type": "column",
                "data": [["Local Funding", 74.67]]
            }, {
                "name": "FY1-Non Govermental Organisation (NGO)",
                "id": "FY1_12",
                "type": "column",
                "data": [{ "name": "Ciao", "y": 25471, "drilldown": "FY1_JAN" }, { "name": "Akronio", "y": 37763.78, "drilldown": "FY1_MARCH" }]
            }, {
                "name": "FY3-Non Govermental Organisation (NGO)",
                "id": "FY3_12",
                "type": "column",
                "data": [["Kiima Foods", 8682.3], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 168149.2], ["Sing With Me Happily", 103519.71]]
            }, {
                "name": "FY1-Private Enterprises and Industry Associations",
                "id": "FY1_1",
                "type": "column",
                "data": [["Akorion Company Limited", 26127.59]]
            }, {
                "name": "FY2-Private Enterprises and Industry Associations",
                "id": "FY2_1",
                "type": "column",
                "data": [["African Innovations Institute (AfrII)", 103972.52], ["Agromart Agricultural Company. (U) Ltd.", 7287.93], ["Brilliant Youth Organization (BYO)", 48261.65], ["Consult Agri-Query Solutions Ltd (Agri-Query)", 3945.04], ["Mugalex Agro Enterprise Ltd", 896.06], ["Responsible Suppliers Limited", 29495.16], ["Smart Money", 102995.53], ["TOTCO Uganda Limited", 9735320.19], ["West Nile Holdings Limited", 539873.06]]
            }, {
                "name": "FY3-Private Enterprises and Industry Associations",
                "id": "FY3_1",
                "type": "column",
                "data": [["Agrinet Uganda Ltd (AgriNet)", 100546.59], ["Ankole Coffee Processors Limited ", 43710.87], ["Aponye Uganda Limited", 50059.74], ["Byeffe  Foods Company Limited", 24401.08], ["East African Seeds Company Limited", 8643.97], ["Ensibuuko Technologies Limited", 268894.82], ["Equator Seeds Ltd", 95020.31], ["Faith Agro Inputs Limited", 11275.39], ["Grain Trade Development Services", 27081.24], ["Kulika Uganda", 19005.38], ["MobiPay AgroSys Limited", 55428.61], ["Ngetta Tropical Holdings", 1168706.69], ["Sebei Farmer SACCO", 477166.67], ["Sunshine Agro Products", 331368.88]]
            }, {
                "name": "FY3-Producer Associations",
                "id": "FY3_3",
                "type": "column",
                "data": [["Green Growers Training and Demonstration Farm ", 25194.15]]
            }, {
                "name": "FY1-JAN-CIAO",
                "id": "FY1-JAN",
                "type": "column",
                "data": [["DELIV1", 25194.15]]
            }, {
                "name": "FY1-MARCH-CIAO",
                "id": "FY1_MARCH",
                "type": "column",
                "data": [["DELIV2", 35194.15]]
            }]

        }

    });



}



//function FY_chart() {

//    // alert('Pending Graph');


//    //Highcharts.chart('FY-chart', {
//    //    chart: {
//    //        type: 'column'
//    //    },
//    //    title: {
//    //        text: 'FY - Progress Indicator'
//    //    },
//    //    xAxis: {
//    //        type: 'category'
//    //    },
//    //    legend: {
//    //        enabled: true
//    //    },
//    //    plotOptions: {
//    //        series: {
//    //            borderWidth: 0,
//    //            dataLabels: {
//    //                enabled: true,
//    //                style: {
//    //                    color: 'white',
//    //                    textShadow: '0 0 2px black, 0 0 2px black'
//    //                }
//    //            },
//    //            stacking: 'normal'
//    //        }
//    //    },
//    //    series: [{
//    //        "name": "Q4",
//    //        "data": [{ "name": "FY1", "y": 3691, "drilldown": "FY1Q4" }, { "name": "FY2", "y": 44365, "drilldown": "FY2Q4" }]
//    //    }, {
//    //        "name": "Q3",
//    //        "data": [{ "name": "FY1", "y": 2036, "drilldown": "FY1Q3" }, { "name": "FY2", "y": 20282, "drilldown": "FY2Q3" }]
//    //    }, {
//    //        "name": "Q2",
//    //        "data": [{ "name": "FY2", "y": 6741, "drilldown": "FY2Q2" }, { "name": "FY3", "y": 12845, "drilldown": "FY3Q2" }]
//    //    }, {
//    //        "name": "Q1",
//    //        "data": [{ "name": "FY2", "y": 638, "drilldown": "FY2Q1" }, { "name": "FY3", "y": 23423, "drilldown": "FY3Q1" }]
//    //    }],
//    //    drilldown: {
//    //        activeDataLabelStyle: {
//    //            color: 'white',
//    //            textShadow: '0 0 2px black, 0 0 2px black'
//    //        },
//    //        series: [{
//    //            "id": "FY1Q4",
//    //            "name": "FY1-Q4",
//    //            "data": [["Agasha", 2347], ["Equator Seeds Ltd", 563], ["K Mubende Farm Supplies", 25], ["Mugalex Agro Enterprise Ltd ", 316], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 159], ["YLA Eastern Team", 58], ["YLA Northern Team", 44], ["YLA Central team", 179]]
//    //        }, {
//    //            "id": "FY2Q4",
//    //            "name": "FY2-Q4",
//    //            "data": [["TOTCO Uganda Limited ", 42], ["West Nile Holdings Limited", 50], ["Sing With Me Happily", 31], ["SMART MONEY", 628], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 34159], ["Ngetta Tropical Holdings", 1409], ["East African seeds company Limited", 75], ["Kulika Uganda", 1851], ["KadAfrica Ltd", 320], ["African Innovations Institute (AfrII)", 3], ["Consult Agri-Query Solutions Ltd (Agri-Query)", 3], ["Equator Seeds Ltd", 241], ["Faith Agro Inputs Ltd (Faith Agro)", 1719], ["Agromart Agricultural Co. (U) Ltd. (Agromart)", 203], ["Akorion Company Limited ", 913], ["Balton Uganda Ltd", 15], ["Brilliant Youth Organization (BYO)", 885], ["Byeffe Foods  Company Ltd", 1747], ["Caïo Shea Butter", 71]]
//    //        }, {
//    //            "id": "FY1Q3",
//    //            "name": "FY1-Q3",
//    //            "data": [["Agasha", 251], ["Equator Seeds Ltd", 1785]]
//    //        }, {
//    //            "id": "FY2Q3",
//    //            "name": "FY2-Q3",
//    //            "data": [["Sing With Me Happily", 12], ["SMART MONEY", 2904], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 11764], ["Mugalex Agro Enterprise Ltd ", 554], ["Ngetta Tropical Holdings", 6], ["Kulika Uganda", 1570], ["Equator Seeds Ltd", 3342], ["Agromart Agricultural Co. (U) Ltd. (Agromart)", 18], ["Akorion Company Limited ", 84], ["Balton Uganda Ltd", 28]]
//    //        }, {
//    //            "id": "FY2Q2",
//    //            "name": "FY2-Q2",
//    //            "data": [["Sing With Me Happily", 11], ["SMART MONEY", 3666], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 504], ["Mugalex Agro Enterprise Ltd ", 287], ["East African seeds company Limited", 13], ["Equator Seeds Ltd", 926], ["Byeffe Foods  Company Ltd", 1334]]
//    //        }, {
//    //            "id": "FY3Q2",
//    //            "name": "FY3-Q2",
//    //            "data": [["Caïo Shea Butter", 477], ["Agrinet Uganda Ltd (AgriNet)", 1247], ["Green Growers Training and Demonstration Farm ", 123], ["Sebei Farmers Savings and Credit Cooperative Organization ", 1245], ["Sunshine Agro Products Limited", 31], ["TOTCO Uganda Limited ", 9722]]
//    //        }, {
//    //            "id": "FY2Q1",
//    //            "name": "FY2-Q1",
//    //            "data": [["YLA Central team", 20], ["YLA Northern Team", 10], ["YLA Western Team", 79], ["Simlaw seed company Limited", 113], ["Sing With Me Happily", 37], ["Mugalex Agro Enterprise Ltd ", 364], ["K Mubende Farm Supplies", 15]]
//    //        }, {
//    //            "id": "FY3Q1",
//    //            "name": "FY3-Q1",
//    //            "data": [["Caïo Shea Butter", 245], ["Aponye Uganda Limited", 107], ["Agrinet Uganda Ltd (AgriNet)", 37], ["Faith Agro Inputs Ltd (Faith Agro)", 563], ["Green Growers Training and Demonstration Farm ", 44], ["Consult Agri-Query Solutions Ltd (Agri-Query)", 118], ["African Innovations Institute (AfrII)", 725], ["KadAfrica Ltd", 100], ["Kulika Uganda", 465], ["East African seeds company Limited", 171], ["Ngetta Tropical Holdings", 5386], ["PRIVATE EDUCATION DEVELOPMENT NETWORK", 8410], ["Responsible Suppliers Ltd", 1331], ["Sebei Farmers Savings and Credit Cooperative Organization ", 20], ["West Nile Holdings Limited", 5701]]
//    //        }]
//    //    }
//    //})



//    //Create the chart


//    Highcharts.chart('Deliverables_chart', {
//        chart: {
//            type: 'column'
//        },
//        title: {
//            text: 'FY - Progress Indicator'
//        },
//        xAxis: {
//            type: 'category'
//        },

//        legend: {
//            enabled: true
//        },

//        plotOptions: {
//            series: {
//                borderWidth: 0,
//                dataLabels: {
//                    enabled: true,
//                    formatter: function () {
//                        if (this.y > 0)
//                            return this.y;
//                    },
//                    style: {
//                        color: 'white',
//                        textShadow: '0 0 2px black, 0 0 2px black'
//                    }
//                },
//                stacking: 'normal'
//            }
//        },
        
//        //series:[{name:'Q4', 
//        //        data:[{name:'FY1', y:3691, drilldown:'FY1Q4'},
//        //              {name:'FY2', y:44365, drilldown:'FY2Q4'}]
//        //        },
//        //        {name:'Q3', 
//        //        data:[{name:'FY1', y:2036, drilldown:'FY1Q3'},
//        //               {name:'FY2', y:20282, drilldown:'FY2Q3'}]
//        //        },
//        //        {name:'Q2', 
//        //         data:[{name:'FY2', y:6741, drilldown:'FY2Q2'},
//        //               {name:'FY3', y:12845, drilldown:'FY3Q2'}]
//        //        },
//        //        {name:'Q1', 
//        //        data:[ {name:'FY2', y:638, drilldown:'FY2Q1'},
//        //               {name:'FY3', y:23423, drilldown:'FY3Q1'}]
//        //        }],

//        //*****************************************************************************
//        //*****************************************************************************
//        //*****************************************************************************

//        series: [{
//            name: 'Q4',
//            data: [{
//                name: 'FY1',
//                y: 5000,
//                drilldown: 'FY1Q4'
//            }, {
//                name: 'FY2',
//                y: 1000,
//                drilldown: 'FY2Q4'
//            }, {
//                name: 'FY3',
//                y: 4500,
//                drilldown: 'FY3Q4'
//            }, {
//                name: 'FY4',
//                y: 3500,
//                drilldown: 'FY4Q4'
//            }]
//        }, {
//            name: 'Q3',
//            data: [{
//                name: 'FY1',
//                y: 5000,
//                drilldown: 'FY1Q3'
//            }, {
//                name: 'FY2',
//                y: 1000,
//                drilldown: 'FY2Q3'
//            }, {
//                name: 'FY3',
//                y: 4500,
//                drilldown: 'FY3Q3'
//            }, {
//                name: 'FY4',
//                y: 3500,
//                drilldown: 'FY4Q3'
//            }]
//        }, {
//            name: 'Q2',
//            data: [{
//                name: 'FY1',
//                y: 5000,
//                drilldown: 'FY1Q2'
//            }, {
//                name: 'FY2',
//                y: 1000,
//                drilldown: 'FY2Q2'
//            }, {
//                name: 'FY3',
//                y: 4500,
//                drilldown: 'FY3Q2'
//            }, {
//                name: 'FY4',
//                y: 3500,
//                drilldown: 'FY4Q2'
//            }]
//        }, {
//            name: 'Q1',
//            data: [{
//                name: 'FY1',
//                y: 15000,
//                drilldown: 'FY1Q1'
//            }, {
//                name: 'FY2',
//                y: 2000,
//                drilldown: 'FY2Q1'
//            }, {
//                name: 'FY3',
//                y: 4000,
//                drilldown: 'FY3Q1'
//            }, {
//                name: 'FY4',
//                y: 5000,
//                drilldown: 'FY4Q1'
//            }]
//        }],


//        drilldown: {
//            activeDataLabelStyle: {
//                color: 'white',
//                textShadow: '0 0 2px black, 0 0 2px black'
//            },


//            //***********************************************************************************************
//            //***********************************************************************************************
//            //***********************************************************************************************


//            //    series: [{
//            //        id: 'FY1Q4',
//            //        name: 'FY1-Q4',
//            //        data: [
//            //            ['Agasha', 2347],
//            //            ['Equator Seeds Ltd', 563],
//            //            ['K Mubende Farm Supplies', 25],
//            //            ['Mugalex Agro Enterprise Ltd ', 316],
//            //            ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 159],
//            //            ['YLA Eastern Team', 58],
//            //            ['YLA Northern Team', 44],
//            //            ['YLA Central team', 179]
//            //        ]
//            //    },
//            //        {
//            //            id: 'FY2Q4',
//            //            name: 'FY2-Q4',
//            //            data: [
//            //                ['TOTCO Uganda Limited ', 42],
//            //                ['West Nile Holdings Limited', 50],
//            //                ['Sing With Me Happily', 31],
//            //                ['SMART MONEY', 628],
//            //                ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 34159],
//            //                ['Ngetta Tropical Holdings', 1409],
//            //                ['East African seeds company Limited', 75],
//            //                ['Kulika Uganda', 1851],
//            //                ['KadAfrica Ltd', 320],
//            //                ['African Innovations Institute (AfrII)', 3],
//            //                ['Consult Agri-Query Solutions Ltd (Agri-Query)', 3],
//            //                ['Equator Seeds Ltd', 241],
//            //                ['Faith Agro Inputs Ltd (Faith Agro)', 1719],
//            //                ['Agromart Agricultural Co. (U) Ltd. (Agromart)', 203],
//            //                ['Akorion Company Limited ', 913],
//            //                ['Balton Uganda Ltd', 15],
//            //                ['Brilliant Youth Organization (BYO)', 885],
//            //                ['Byeffe Foods  Company Ltd', 1747],
//            //                ['Caïo Shea Butter', 71]
//            //            ]
//            //        }, {
//            //            id: 'FY1Q3',
//            //            name: 'FY1-Q3',
//            //            data: [
//            //                ['Agasha', 251],
//            //                ['Equator Seeds Ltd', 1785]
//            //            ]
//            //        }, {
//            //            id: 'FY2Q3',
//            //            name: 'FY2-Q3',
//            //            data: [
//            //                ['Sing With Me Happily', 12],
//            //                ['SMART MONEY', 2904],
//            //                ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 11764],
//            //                ['Mugalex Agro Enterprise Ltd ', 554],
//            //                ['Ngetta Tropical Holdings', 6],
//            //                ['Kulika Uganda', 1570],
//            //                ['Equator Seeds Ltd', 3342],
//            //                ['Agromart Agricultural Co. (U) Ltd. (Agromart)', 18],
//            //                ['Akorion Company Limited ', 84],
//            //                ['Balton Uganda Ltd', 28]
//            //            ]
//            //        }, {
//            //            id: 'FY2Q2',
//            //            name: 'FY2-Q2',
//            //            data: [
//            //                ['Sing With Me Happily', 11],
//            //                ['SMART MONEY', 3666],
//            //                ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 504],
//            //                ['Mugalex Agro Enterprise Ltd ', 287],
//            //                ['East African seeds company Limited', 13],
//            //                ['Equator Seeds Ltd', 926],
//            //                ['Byeffe Foods  Company Ltd', 1334]
//            //            ]
//            //        }, {
//            //            id: 'FY3Q2',
//            //            name: 'FY3-Q2',
//            //            data: [
//            //                ['Caïo Shea Butter', 477],
//            //                ['Agrinet Uganda Ltd (AgriNet)', 1247],
//            //                ['Green Growers Training and Demonstration Farm ', 123],
//            //                ['Sebei Farmers Savings and Credit Cooperative Organization ', 1245],
//            //                ['Sunshine Agro Products Limited', 31],
//            //                ['TOTCO Uganda Limited ', 9722]
//            //            ]
//            //        }, {
//            //            id: 'FY2Q1',
//            //            name: 'FY2-Q1',
//            //            data: [
//            //                ['YLA Central team', 20],
//            //                ['YLA Northern Team', 10],
//            //                ['YLA Western Team', 79],
//            //                ['Simlaw seed company Limited', 113],
//            //                ['Sing With Me Happily', 37],
//            //                ['Mugalex Agro Enterprise Ltd ', 364],
//            //                ['K Mubende Farm Supplies', 15]
//            //            ]
//            //        }, {
//            //            id: 'FY3Q1',
//            //            name: 'FY3-Q1',
//            //            data: [
//            //                ['Caïo Shea Butter', 245],
//            //                ['Aponye Uganda Limited', 107],
//            //                ['Agrinet Uganda Ltd (AgriNet)', 37],
//            //                ['Faith Agro Inputs Ltd (Faith Agro)', 563],
//            //                ['Green Growers Training and Demonstration Farm ', 44],
//            //                ['Consult Agri-Query Solutions Ltd (Agri-Query)', 118],
//            //                ['African Innovations Institute (AfrII)', 725],
//            //                ['KadAfrica Ltd', 100],
//            //                ['Kulika Uganda', 465],
//            //                ['East African seeds company Limited', 171],
//            //                ['Ngetta Tropical Holdings', 5386],
//            //                ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 8410],
//            //                ['Responsible Suppliers Ltd', 1331],
//            //                ['Sebei Farmers Savings and Credit Cooperative Organization ', 20],
//            //                ['West Nile Holdings Limited', 5701]
//            //            ]
//            //        }],


//            //***********************************************************************************************
//            //***********************************************************************************************
//            //***********************************************************************************************

//            series: [{
//                id: 'FY1Q1',
//                name: 'FY1-Q1',
//                data: [
//                    ['Equator Seeds Ltd', 5000],
//                    ['Byeffe Foods  Company Ltd', 2000],
//                    ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 1000],
//                    ['Akorion Company Limited ', 2000],
//                    ['Faith Agro Inputs Ltd (Faith Agro)', 5000]
//                ]
//            }, {
//                id: 'FY1Q2',
//                name: 'FY1-Q2',
//                data: [
//                    ['Equator Seeds Ltd', 500],
//                    ['Byeffe Foods  Company Ltd', 100],
//                    ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 1000],
//                    ['Akorion Company Limited ', 200],
//                    ['Faith Agro Inputs Ltd (Faith Agro)', 100],
//                    ['West Nile Holdings Limited', 100]
//                ]
//            }, {
//                id: 'FY1Q3',
//                name: 'FY1-Q3',
//                data: [
//                    ['Equator Seeds Ltd', 800],
//                    ['Byeffe Foods  Company Ltd', 800],
//                    ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 1000],
//                    ['Akorion Company Limited ', 400],
//                    ['Faith Agro Inputs Ltd (Faith Agro)', 500],
//                    ['West Nile Holdings Limited', 100],
//                    ['KadAfrica Ltd', 400]
//                ]
//            }, {
//                id: 'FY1Q4',
//                name: 'FY1-Q4',
//                data: [
//                    ['Equator Seeds Ltd', 500],
//                    ['Byeffe Foods  Company Ltd', 700],
//                    ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 800],
//                    ['Akorion Company Limited ', 2000],
//                    ['Faith Agro Inputs Ltd (Faith Agro)', 100],
//                    ['West Nile Holdings Limited', 500],
//                    ['Brilliant Youth Organization (BYO)', 400]
//                ]
//            }, {
//                id: 'FY2Q1',
//                name: 'FY2-Q1',
//                data: [
//                    ['Equator Seeds Ltd', 500],
//                    ['Byeffe Foods  Company Ltd', 600],
//                    ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 400],
//                    ['Akorion Company Limited ', 200],
//                    ['Faith Agro Inputs Ltd (Faith Agro)', 300],
//                    ['West Nile Holdings Limited', 1000],
//                    ['Brilliant Youth Organization (BYO)', 500],
//                    ['MobiPay Agrosys Limited', 1500]
//                ]
//            }, {
//                id: 'FY2Q2',
//                name: 'FY2-Q2',
//                data: [
//                    ['Equator Seeds Ltd', 100],
//                    ['Byeffe Foods  Company Ltd', 200],
//                    ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 100],
//                    ['Akorion Company Limited ', 2],
//                    ['Faith Agro Inputs Ltd (Faith Agro)', 100],
//                    ['West Nile Holdings Limited', 1],
//                    ['Brilliant Youth Organization (BYO)', 50],
//                    ['MobiPay Agrosys Limited', 50],
//                    ['Aponye Uganda Limited', 400]

//                ]
//            }, {
//                id: 'FY2Q3',
//                name: 'FY2-Q3',
//                data: [
//                    ['Equator Seeds Ltd', 500],
//                    ['Byeffe Foods  Company Ltd', 300],
//                    ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 300],
//                    ['Akorion Company Limited ', 200],
//                    ['Faith Agro Inputs Ltd (Faith Agro)', 500],
//                    ['West Nile Holdings Limited', 200],
//                    ['Brilliant Youth Organization (BYO)', 500],
//                    ['MobiPay Agrosys Limited', 1000],
//                    ['Aponye Uganda Limited', 1000]
//                ]
//            }, {
//                id: 'FY2Q4',
//                name: 'FY2-Q4',
//                data: [
//                    ['Equator Seeds Ltd', 100],
//                    ['Byeffe Foods  Company Ltd', 200],
//                    ['PRIVATE EDUCATION DEVELOPMENT NETWORK', 500],
//                    ['Akorion Company Limited ', 1000],
//                    ['Faith Agro Inputs Ltd (Faith Agro)', 500],
//                    ['West Nile Holdings Limited', 100],
//                    ['Brilliant Youth Organization (BYO)', 200],
//                    ['MobiPay Agrosys Limited', 700],
//                    ['Aponye Uganda Limited', 500]

//                ]
//            }]


//        }

//    })


//}


function Graph_sample_1() {
       
        // Create the chart
    //Highcharts.chart('Deliverables-chart', {
    $('#Deliverables_chart').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'Basic drilldown'
        },
        xAxis: {
            type: 'category'
        },

        legend: {
            enabled: false
        },

        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                }
            }
        },

        series: [{
            name: 'Things',
            colorByPoint: true,
            data: [{
                name: 'Animals',
                y: 5,
                drilldown: 'animals'
            }, {
                name: 'Fruits',
                y: 2,
                drilldown: 'fruits'
            }, {
                name: 'Cars',
                y: 4
            }]
        }],
        drilldown: {
            series: [{
                id: 'animals',
                data: [
                    ['Cats', 4, 'animals2'],
                    ['Dogs', 2, 'fruits2'],
                ],
                keys: ['name', 'y', 'drilldown']
            }, {
                id: 'fruits',
                data: [
                    ['Apples', 4]
                ]
            }, {
                id: 'animals2',
                data: [
                    ['Cats', 4],
                    ['Dogs', 2],
                    ['Cows', 1],
                    ['Sheep', 2],
                    ['Pigs', 1]
                ],
                point: {
                    events: {
                        click: function (e) {
                            hs.htmlExpand(null, {
                                pageOrigin: {
                                    x: e.pageX || e.clientX,
                                    y: e.pageY || e.clientY
                                },
                                headingText: this.series.name,
                                maincontentText: 'Here you can set up a list that should be displyed',
                                width: 200
                            });
                        }
                    }
                },
            }, {
                id: 'fruits2',
                data: [
                    ['Apples', 4],
                    ['Oranges', 2]
                ]
            }]
        }
    });
    



}


function Graph_sample_III() {

    /*
Issues
-------

1. Pie chart only updated on the first drilldown, never after
2. Have a hack for triggering drilldown/drillup only once, instead of for each series
3. Would be nice to alter the drillUp button text to point back to my "breadCrumbs". Or perhaps better, have the breadCrumbs be links instead of using the drillUp button at all
4. Would prefer that when clicking on the bar graph, it triggers a category drilldown instead of a series drilldown

*/
        
        var breadCrumbs = ['Overview'];
        var ran = false;
        var totals = [];
        var newTotals = [];
        var level = 0;

        function getSum(total, element) {
            return total + element.y;
        };

        Highcharts.setOptions({
            lang: {
                drillUpText: '<< ' + breadCrumbs[level]
            }
        });


    $('#Deliverables_chart').highcharts({

            chart: {
                type: 'column',
                events: {
                    // drilldown/up post-function to set the subtitle that tracks which level we are at (aka, "breadcrumbs")
                    drilldown: function (e) {
                        // not sure how to stop this function from running twice (once for each series included in the category); hack it with a global state variable

                        console.log(e.point.name);

                        if (ran) {
                            ran = false;
                            newTotals[0] = e.seriesOptions.data.reduce(getSum, 0);
                            totals.push(newTotals);
                            this.series[2].setData(newTotals, true);
                            newTotals = [];
                        } else {
                            ran = true;
                            level++;
                            breadCrumbs.push(e.point.name);
                            newTotals[1] = e.seriesOptions.data.reduce(getSum, 0);

                            this.setTitle(null, {
                                text: breadCrumbs.join('->')
                            });
                        }
                    },
                    drillup: function (e) {
                        if (ran) {
                            ran = false;
                        } else {
                            ran = true;
                            level--;
                            breadCrumbs.pop();

                            totals.pop();
                            this.series[2].setData(totals[totals.length - 1], true);

                            this.setTitle(null, {
                                text: breadCrumbs.join('->')
                            });
                        }
                    }
                }
            },

            title: {
                text: 'Multi-series multi-level drilldown'
            },
            subtitle: {
                text: breadCrumbs[0]
            },

            xAxis: {
                categories: true
            },

            plotOptions: {
                column: {
                    grouping: true,
                    shadow: false,
                    borderWidth: 0
                }
            },


            legend: {
                enabled: true,
                align: 'left',
                verticalAlign: 'top',
                x: '10%',
                y: '2%',
                floating: true,
                layout: "vertical",
                symbolHeight: 12,
                symbolWidth: 12,
                symbolRadius: 2
            },

            yAxis: {
                maxPadding: 0.4,
            },

            drilldown: {
                drillUpButton: {
                    relativeTo: "spacingBox",
                    position: {
                        align: 'left',
                        verticalAlign: 'bottom',
                        y: -20
                    },
                },
                series: [{
                    id: '1_one_drilldown',
                    name: 'Series 1',
                    color: 'rgb(126,86,134)',
                    zIndex: 2,
                    data: [{
                        name: 'one a',
                        y: 4,
                        drilldown: '1_one_a_drilldown'
                    }, {
                        name: 'one b',
                        y: 3,
                        drilldown: '1_one_b_drilldown'
                    }]
                }, {
                    id: '1_one_a_drilldown',
                    name: 'Series 1',
                    color: 'rgb(126,86,134)',
                    zIndex: 2,
                    data: [
                        ['one a i', 3],
                        ['one a ii', 1]
                    ]
                }, {
                    id: '1_one_b_drilldown',
                    name: 'Series 1',
                    color: 'rgb(126,86,134)',
                    zIndex: 2,
                    data: [
                        ['one b i', 3],
                        ['one b ii', 1]
                    ]
                }, {
                    id: '1_two_drilldown',
                    name: 'Series 1',
                    color: 'rgb(126,86,134)',
                    zIndex: 2,
                    data: [{
                        name: 'two a',
                        y: 4,
                        drilldown: '1_two_a_drilldown'
                    }, {
                        name: 'two b',
                        y: 3,
                        drilldown: '1_two_b_drilldown'
                    }]
                }, {
                    id: '1_two_a_drilldown',
                    name: 'Series 1',
                    color: 'rgb(126,86,134)',
                    zIndex: 2,
                    data: [
                        ['two a i', 3],
                        ['two a ii', 1]
                    ]
                }, {
                    id: '1_two_b_drilldown',
                    name: 'Series 1',
                    color: 'rgb(126,86,134)',
                    zIndex: 2,
                    data: [
                        ['two b i', 3],
                        ['two b ii', 1]
                    ]
                }, {
                    id: '2_one_drilldown',
                    name: 'Series 2',
                    color: 'rgb(165,170,217)',
                    borderColor: 'rgb(165,170,217)',
                    borderWidth: 6,
                    zIndex: 1,
                    data: [{
                        name: 'one a',
                        y: 6,
                        drilldown: '2_one_a_drilldown'
                    }, {
                        name: 'one b',
                        y: 0,
                        drilldown: '2_one_b_drilldown'
                    }]
                }, {
                    id: '2_one_a_drilldown',
                    name: 'Series 2',
                    color: 'rgb(165,170,217)',
                    borderColor: 'rgb(165,170,217)',
                    borderWidth: 6,
                    zIndex: 1,
                    data: [
                        ['one a i', 2],
                        ['one a ii', 5]
                    ]
                }, {
                    id: 'one_b_drilldown',
                    name: 'Series 2',
                    color: 'rgb(165,170,217)',
                    borderColor: 'rgb(165,170,217)',
                    borderWidth: 6,
                    zIndex: 1,
                    data: [
                        ['one b i', 5],
                        ['one b ii', 2]
                    ]
                }, {
                    id: '2_two_drilldown',
                    name: 'Series 2',
                    color: 'rgb(165,170,217)',
                    borderColor: 'rgb(165,170,217)',
                    borderWidth: 6,
                    zIndex: 1,
                    data: [{
                        name: 'two a',
                        y: 6,
                        drilldown: '2_two_a_drilldown'
                    }, {
                        name: 'two b',
                        y: 5,
                        drilldown: '2_two_b_drilldown'
                    }]
                }, {
                    id: '2_two_a_drilldown',
                    name: 'Series 2',
                    color: 'rgb(165,170,217)',
                    borderColor: 'rgb(165,170,217)',
                    borderWidth: 6,
                    zIndex: 1,
                    data: [
                        ['two a i', 1],
                        ['two a ii', 3]
                    ]
                }, {
                    id: '2_two_b_drilldown',
                    name: 'Series 2',
                    color: 'rgb(165,170,217)',
                    borderColor: 'rgb(165,170,217)',
                    borderWidth: 6,
                    zIndex: 1,
                    data: [
                        ['two b i', 2],
                        ['two b ii', 4]
                    ]
                }]
            },

            series: [{
                name: 'Series 1',
                color: 'rgb(126,86,134)',                
                data: [{
                    name: 'One',
                    y: 1,
                    drilldown: '1_one_drilldown'
                }, {
                    name: 'Two',
                    y: 2,
                    drilldown: '1_two_drilldown'
                }]
            }, {
                name: 'Series 2',
                color: 'rgb(165,170,217)',
                borderColor: 'rgb(165,170,217)',
                borderWidth: 6,             
                data: [{
                    name: 'One',
                    y: 2,
                    drilldown: '2_one_drilldown'
                }, {
                    name: 'Two',
                    y: 3,
                    drilldown: '2_two_drilldown'
                }]
            }, {
                type: 'pie',
                name: 'Comparison comparison',
                labels: {
                    items: [{
                        html: 'Totals',
                        style: {
                            top: '100px',
                            left: '100px',
                            'font-size': '15px',
                            'font-weight': 'bold',
                        }
                    }]
                },
                data: [{
                    name: 'Two',
                    y: 5,
                    color: 'rgba(165, 170, 217, 1)'
                }, {
                    name: 'One',
                    y: 3,
                    color: 'rgba(126, 86, 134, .9)'
                }],
                center: ['90%', 0],
                size: '30%',
                showInLegend: false,
                dataLabels: {
                    enabled: true,
                    distance: -20,
                    crop: false,
                    formatter: function () {
                        return this.point.y;
                    }
                }
            }]
        });
           
}


//**************************************************************************************************************************************







//**************************************************************************************************************************************

function Graph_sample_II() {

    var drilldownsAdded = 0;
    
    // Create the chart
    $('#Deliverables_chart').highcharts({
        chart: {
            type: 'column',
            events: {
                drilldown: function (e) {
                    if (!e.seriesOptions) {

                        console.log(
                            'point.name', e.point.name,
                            'series.name', e.point.series.name,
                            'byCategory', e.byCategory
                        );

                        var chart = this,
                            drilldowns = {
                                'California': [
                                    {
                                        name: 'Bill Clinton',
                                        data: [
                                            ['Santa Clara', 5, true],
                                            ['San Mateo', 2, true],
                                            ['San Francisco', 1, true],
                                        ],
                                        keys: ['name', 'y', 'drilldown']
                                    },
                                    {
                                        name: 'George W. Bush',
                                        data: [
                                            ['Santa Clara', 0,true],
                                            ['San Mateo', 1, true],
                                            ['San Francisco', 1, true],
                                        ],
                                        keys: ['name', 'y', 'drilldown']
                                    },
                                    {
                                        name: 'Barack Obama',
                                        data: [
                                            ['Santa Clara', 10, true],
                                            ['San Mateo', 5, true],
                                            ['San Francisco', 3, true],
                                        ],
                                        keys: ['name', 'y', 'drilldown']
                                    },
                                ],                                
                                'Santa Clara': [
                                    {
                                        name: 'Bill Clinton',
                                        data: [
                                            ['Santa 1', 7, true],
                                            ['Santa 2', 3, true],
                                            ['Santa 3',8, true],
                                        ],
                                        keys: ['name', 'y', 'drilldown']
                                    },
                                    {
                                        name: 'George W. Bush',
                                        data: [
                                            ['Santa 1', 3, true],
                                            ['Santa 2', 7, true],
                                            ['Santa 3', 5, true],
                                        ],
                                        keys: ['name', 'y', 'drilldown']
                                    },
                                    {
                                        name: 'Barack Obama',
                                        data: [
                                            ['Santa 1', 2, true],
                                            ['Santa 2', 4, true],
                                            ['Santa 3', 5, true],
                                        ],
                                        keys: ['name', 'y', 'drilldown']
                                    },
                                ],
                                'New York': [
                                    {
                                        name: 'Bill Clinton',
                                        data: [
                                            ['Albany', 3],
                                            ['New York', 5],
                                            ['Essex', 2],
                                        ]
                                    },
                                    {
                                        name: 'George W. Bush',
                                        data: [
                                            ['Albany', 1],
                                            ['New York', 0],
                                            ['Essex', 1],
                                        ]
                                    },
                                    {
                                        name: 'Barack Obama',
                                        data: [
                                            ['Albany', 5],
                                            ['New York', 6],
                                            ['Essex', 1],
                                        ]
                                    },
                                ],
                                'Texas': [
                                    {
                                        name: 'Bill Clinton',
                                        data: [
                                            ['Austin', 4],
                                            ['Dallas', 1],
                                            ['Houston', 1],
                                        ]
                                    },
                                    {
                                        name: 'George W. Bush',
                                        data: [
                                            ['Austin', 3],
                                            ['Dallas', 5],
                                            ['Houston', 7]
                                        ]
                                    },
                                    {
                                        name: 'Barack Obama',
                                        data: [
                                            ['Austin', 4],
                                            ['Dallas', 0],
                                            ['Houston', 0],
                                        ]
                                    },
                                ]
                            };

                        var stateSeries = drilldowns[e.point.name],
                            series;

                        for (var i = 0; i < stateSeries.length; i++) {
                            if (stateSeries[i].name === e.point.series.name) {
                                series = stateSeries[i];
                                break;
                            }
                        }

                        // Show the loading label
                        console.log(e.point);
                        chart.showLoading('Simulating Ajax for ' + e.point.name);

                        setTimeout(function () {
                            chart.addSingleSeriesAsDrilldown(e.point, series);
                            drilldownsAdded++;
                            if (drilldownsAdded === 3) {
                                drilldownsAdded = 0;
                                chart.hideLoading();
                                chart.applyDrilldown();
                            }
                        }, 1000);
                    }

                }
            }
        },
        title: {
            text: 'Async drilldown'
        },
        xAxis: {
            type: 'category',
        },

        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true
                }
            }
        },

        series: [{
            name: 'Bill Clinton',
            data: [{
                name: 'California',
                y: 8,
                drilldown: true
            }, {
                name: 'New York',
                y: 10,
                drilldown: true
            }, {
                name: 'Texas',
                y: 6,
                drilldown: true
            }]
        },
        {
            name: 'George W. Bush',
            data: [
                {
                    name: 'California',
                    y: 2,
                    drilldown: true
                },
                {
                    name: 'New York',
                    y: 2,
                    drilldown: true
                },
                {
                    name: 'Texas',
                    y: 15,
                    drilldown: true
                }
            ]
        },
        {
            name: 'Barack Obama',
            data: [
                {
                    name: 'California',
                    y: 18,
                    drilldown: true
                },
                {
                    name: 'New York',
                    y: 12,
                    drilldown: true
                },
                {
                    name: 'Texas',
                    y: 4,
                    drilldown: true
                }
            ]
        }],

        drilldown: {
            series: []
        },

        colors: ["#90BBE6", "#576B85", "#92BD8E"]
    });
}
