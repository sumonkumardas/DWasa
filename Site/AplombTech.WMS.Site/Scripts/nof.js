var scadachart;
function LoadMapRelatedScript() {
    $(function () { $('#legend').jstree({ "themes": { "stripes": true } }); });
    $('#legend').on("changed.jstree", function (e, data) {

        //zone1.setOptions({ strokeColor: 'green' });

        if ($('.jstree-clicked').text().trim()) {

            var stationName = $('.jstree-clicked').text();
            var depth = data.node.parents.length;
            var node_id = data.node.id;
            if (depth == 1) {
                drawDmaAndPumpStation(node_id.split("_")[1]);
            }
            if (depth == 3) {
                node_id = node_id.split("_")[1];
                
                $.ajax({
                    type: "POST",
                    url: $("#pumpStationOverViewUrl").val(),
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ pumpStationId: node_id }),
                    dataType: "json",
                    success: function (model) {
                        
                        if (model.IsSuccess) {
                            var contentString = '<h2>Overview</h2><hr/> <div >';
                            //drawChart(z2marker, model.data, stationName);
                            for (var key in model.Data) {
                                if (model.Data.hasOwnProperty(key)) {
                                    var val = model.Data[key];
                                    if (val !== null) {
                                        contentString += '<h4>' + key + '</h4>' + ' <strong>' + model.Data[key] + ' </strong>';
                                    }

                                }
                            }
                            
                            drawChart(markers['marker_' + node_id], contentString + '</div>');
                        }

                    },
                    error: function () { }
                });
            }

            if (depth == 4) {
                node_id = node_id.split("_")[1];


                $.ajax({
                    type: "POST",
                    url: $("#getSingleSensorStatusUrl").val(),
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ sensorId: node_id }),
                    dataType: "json",
                    success: function (model) {
                        if (model.IsSuccess) {

                            //drawChart(z2marker, model.data, stationName);

                            if (stationName.includes("CPD")) {
                                var contentString;
                                if (model.Value > 0)
                                    contentString = '<h2>Cholorination On</h2>';
                                else {
                                    contentString = '<h2>Cholorination Off</h2>';
                                }
                                drawChart(markers['marker_' + model.PumpStationId], contentString);
                            }
                            else if (stationName.includes("ACP")) {
                                var contentString;
                                if (model.Value > 0)
                                    contentString = '<h2>ACP On</h2>';
                                else {
                                    contentString = '<h2>ACP Off</h2>';
                                }
                                drawChart(markers['marker_' + model.PumpStationId], contentString);
                            } else {
                                contentString = '<div><h2>' + stationName.trim() + '</h2><hr>' + '<p>Current value = ' + model.Value + ' <strong>' + model.Unit + '</strong></p></div>';
                                //drawSensorData(z2marker, stationName.trim(), model.Value, model.Unit);
                                drawChart(markers['marker_' + model.PumpStationId], contentString);
                            }
                        }

                    },
                    error: function () { }
                });
            }

        }
    });

    function drawChart(marker, content) {
        
        var infowindow2 = new google.maps.InfoWindow({
            content: content
        });

        infowindow2.open(marker.getMap(), marker);
    }
}
function showRealChartScada(data2, sensorId) {
    $('#chartModal').modal('show');
    Highcharts.setOptions({
        global: {
            useUTC: false
        }
    });
    scadachart = new Highcharts.Chart({
        chart: {
            renderTo: 'chart_div',
            defaultSeriesType: 'spline',
            events: {
                load: function () {
                    var series = this.series[0];

                    graphInterval = setInterval(function () {
                        //var shift = series.data.length > 20; // shift if the series is longer than 20

                        $.ajax({
                            type: "POST",
                            url: $("#getScadaSensorDataUrl").val(),
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify({ sensorId: sensorId }),
                            dataType: "json",
                            success:
                                function (data) {
                                    if (data.IsSuccess) {
                                        var x = (new Date()).getTime(), // current time
                                            y = data.Value;
                                        series.addPoint([x, y], true, series.data.length > 20);

                                    }

                                },
                            error: function (e) { }
                        });


                    }, 1000);
                }
            }
        },
        global: {
            useUTC: false
        },
        title: {
            text: 'Real time Chart'
        },
        xAxis: {
            type: 'datetime',
            title: {
                text: 'time',
                margin:20
            },
            tickPixelInterval: 150,
            maxZoom: 20 * 1000,
            gridLineWidth: 1
        },
        yAxis: {
            minPadding: 0.2,
            maxPadding: 0.2,
            title: {
                text: data2.Unit,
                margin: 40
            },
            lineWidth: 0,
            gridLineWidth: 0,
            lineColor: 'transparent'
        },
        credits: {
            text: 'Aplombtech BD',
            href: 'http://www.aplombtechbd.com'
        },
        tooltip: {
            valueSuffix: ' ' + data2.Unit
        },
        series: [{
            name: data2.Name,
            data: []
        }],
        exporting: {
            enabled: true
        }
    });

    scadachart.setOptions({
        global: {
            useUTC: false
        }
    });


}

function setFrequency() {
    $('#frequencyShowModal').modal('show');
    var slider = $("#vfdFrequencyRange");
    $("#frequencyValue").val(slider.val());
}

function showNewFrequencyValueViaSlider() {
    var slider = $("#vfdFrequencyRange");
    $("#frequencyValue").val(slider.val());
}

function showNewFrequencyValueViaTextBox() {
    var slider = $("#frequencyValue");
    $("#vfdFrequencyRange").val(slider.val());
}

function publishVfdFrequency() {
    $('#setFrequencyError').text('');
    $.ajax({
        type: "POST",
        url: $("#publishVfdFrequency").val(),
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ frequencyValue: $("#vfdFrequencyRange").val(), pumpStationId: $('#SelectedPumpStationId').val() }),
        dataType: "json",
        success: function (model) {

            if (model.IsSuccess) {
                $('#setFrequencyError').text('Saved');
            } else {
                $('#setFrequencyError').text('Error');
            }
        },
        error: function () { }
    });
}

