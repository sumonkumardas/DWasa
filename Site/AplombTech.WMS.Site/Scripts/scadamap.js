
var scadModule = (function (window, undefined) {
    //#region Variables
    var isLoadScadaRunning = false;
    var scadaInterval;
    //#endregion 

    //#region Private Methods
    function clearRecursiveCall() {
        if (scadaInterval != null)
            //clearInterval(scadaInterval);
            clearTimeout(scadaInterval);
    }

    function clearScada() {
        $('#searchResults').empty();
    }

    function refreshScada() {
        var pumpStationId = $('#SelectedPumpStationId').val();
        if (pumpStationId > 0) {
            clearRecursiveCall();
            scadaInterval = setTimeout(function tick() {
                $.ajax({
                    type: "POST",
                    url: $("#getScadaDataUrl").val(),
                    data: JSON.stringify({ pumpStationId: pumpStationId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.IsSuccess == true) {

                            //Power Information.
                            $('#Vfd_current').text(data.VfdData.Current);
                            $('#Vfd_frequency').text(data.VfdData.Frequency);
                            $('#Vfd_power').text(data.VfdData.Power);
                            $('#Vfd_voltage').text(data.VfdData.Voltage);
                            $('#Vfd_energy').text(data.VfdData.Energy);
                            $('#Vfd_oh').text(data.VfdData.OperatingHour);
                            $('#Vfd_rh').text(data.VfdData.RunningHour);


                            //Motor Information.
                            //$('#connectionStatus').val('Online');
                            for (var i = 0; i < data.MotorList.length; i++) {

                                //if the name is what we are looking for return it
                                if (i % 2 == 0) {
                                    $('#pmotorStatus').val(data.MotorList[i].MotorStatus);
                                    $('#pmotorCommandTime').val(data.MotorList[i].LastCommandTime);
                                    if (data.MotorList[i].Auto == true) {
                                        $('#motorswitch *').prop('disabled', false);
                                        $("#pmotorAuto").text('Auto');
                                    } else {
                                        $("#pmotorAuto").text('Manual');
                                        $('#pautoarea').show();
                                    }
                                    if ($('#motorswitchStatus').text() == 'Processing...') {
                                        if (data.MotorList[i].MotorStatus != $('#motorswitch').text()) {
                                            if (data.MotorList[i].MotorStatus == 'ON') {
                                                $("#motorswitch").removeClass("label-danger");
                                                $("#motorswitch").addClass("label-success");

                                            }

                                            if (data.MotorList[i].MotorStatus == 'OFF') {
                                                $("#motorswitch").addClass("label-danger");
                                                $("#motorswitch").removeClass("label-success");
                                            }
                                            $('#motorswitch').text(data.MotorList[i].MotorStatus);
                                            $('#motorswitchStatus').text('');
                                            $('#pmotorCommandSent').text('');
                                        }

                                        if (data.MotorList[i].MotorStatus == $('#motorswitch').text()) {
                                            //after pumpmotor command send data is pulled after 30 seconds
                                            //check if the time elapsed or not
                                            var d1 = new Date($('#pmotorCommandSent').text());
                                            var d2 = new Date(d1);
                                            d2.setSeconds(d1.getSeconds() + 28);
                                            if (new Date() > d2) {
                                                if (data.MotorList[i].MotorStatus == 'ON') {
                                                    $("#motorswitch").removeClass("label-danger");
                                                    $("#motorswitch").addClass("label-success");
                                                }

                                                if (data.MotorList[i].MotorStatus == 'OFF') {
                                                    $("#motorswitch").addClass("label-danger");
                                                    $("#motorswitch").removeClass("label-success");
                                                }
                                                $('#motorswitch').text(data.MotorList[i].MotorStatus);
                                                $('#motorswitchStatus').text('');
                                                $('#pmotorCommandSent').text('');
                                            }
                                        }
                                    }


                                }
                                if (i % 2 == 1) {
                                    $('#cmotorStatus').val(data.MotorList[i].MotorStatus);
                                    $('#cmotorCommandTime').val(data.MotorList[i].LastCommandTime);

                                    if (data.MotorList[i].MotorStatus == 'ON') {
                                        $("#cmotorStatus").removeClass("label-danger");
                                        $("#cmotorStatus").addClass("label-success");

                                    }

                                    if (data.MotorList[i].MotorStatus == 'OFF') {
                                        $("#cmotorStatus").addClass("label-danger");
                                        $("#cmotorStatus").removeClass("label-success");
                                    }


                                    $('#connectionStatusTime').text(data.LastDataRecived);

                                }

                            }
                            var res = JSON.parse(data.SensorList);
                            console.log(data.SensorList);
                            for (var key in res) {
                                if (res.hasOwnProperty(key)) {
                                    $('#' + key).text(res[key]);


                                }
                            }


                        }
                        //pulling scada data every 1 min 
                        //
                        var d3 = new Date($('#connectionStatusTime').text());
                        var d4 = new Date(d3);

                        d4.setMinutes(d3.getMinutes() + 1);

                        if (d4 > new Date()) {
                            $('#connectionStatus').text('Online');
                            $("#connectionStatus").removeClass("label-danger");
                            $("#connectionStatus").addClass("label-success");
                        } else {
                            $('#connectionStatus').text('Offline');
                            $("#connectionStatus").addClass("label-danger");
                            $("#connectionStatus").removeClass("label-success");
                        }
                    },
                    failure: function (response) {
                    }
                });
                scadaInterval = setTimeout(tick, 30000); // (*)
            }, 1000);
        }
    }
    //#endregion 

    //#region Public Methods

    function loadDma(zoneId) {
        clearScada();
        clearRecursiveCall();

        if (!zoneId) return;//zoneId = 0;
        var dmaDownList = $('#SelectedDmaId');
        $.ajax({
            type: "POST",
            url: $("#getDmaUrl").val(),
            data: JSON.stringify({ zoneId: zoneId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                var res = JSON.parse(data.Data);
                dmaDownList.empty();
                dmaDownList.append($('<option/>', {
                    value: 0,
                    text: "Select Dma"
                }));
                for (var key in res) {
                    if (res.hasOwnProperty(key)) {

                        dmaDownList.append($('<option/>', {
                            value: key,
                            text: res[key]
                        }));
                    }
                }
            },
            failure: function (response) {
            }
        });
    }

    function loadPumpStation(dmaId) {
        clearScada();
        clearRecursiveCall();
        if (!dmaId) return;// dmaId = 0;
        var dmaDownList = $('#SelectedPumpStationId');
        $.ajax({
            type: "POST",
            url: $("#getPumpStationUrl").val(),
            data: JSON.stringify({ dmaId: dmaId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                var res = JSON.parse(data.Data);
                dmaDownList.empty();
                dmaDownList.append($('<option/>', {
                    value: 0,
                    text: "Select Pumpstation"
                }));
                for (var key in res) {
                    if (res.hasOwnProperty(key)) {

                        dmaDownList.append($('<option/>', {
                            value: key,
                            text: res[key]
                        }));
                    }
                }
            },
            failure: function (response) {
            }
        });
    }

    function showScada() {


        $('#searchResults').html('<img src="' + $("#imageUrl").val() + '/ajax-loader.gif">').delay(50000).fadeIn(4000);

        var url = $("#showScadaUrl").val();
        var pumpStationId = $('#SelectedPumpStationId').val();
        if (pumpStationId > 0) {
            $('#searchResults').load(url, { pumpStationId: pumpStationId });
            refreshScada();
        } else {
            $('#searchResults').html('<p style="color:red"> Please Select Pumpstation </p>')
        }
       
    }

    function clearAllScada() {
        clearScada();
        clearRecursiveCall();
    };

    function motorSwitchFunction() {
        if ($('#pmotorAuto').text() == "Manual") {
            return;
        }
        if ($('#motorswitchStatus').text() == "Processing...") {
            return;
        }
        var state;
        if ($('#motorswitch').text() == 'ON') {
            state = "OFF";
        } else
            state = "ON";
        $('#motorswitchStatus').text('Processing...');
        $('#pmotorCommandSent').text(new Date().toLocaleString());

        $.ajax({
            type: "POST",
            url: $("#publishCommandUrl").val(),
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ state: state, pumpStationId: $('#SelectedPumpStationId').val() }),
            dataType: "json",
            success:
                function (data) {
                    if (data.IsSuccess) {
                        if (data.Data == 'Success') {
                            $('#motorswitchStatus').text('');
                            $('#motorswitch').text(state);
                            if (state === 'OFF') {
                                $('#motorswitch').removeClass('label-success').addClass('label-danger');
                            }
                            else {
                                $('#motorswitch').removeClass('label-danger').addClass('label-success');
                            }
                            //setTimeout(refreshScada, 10000);
                        }
                        else {
                        }

                    } else {

                    }
                },
            error: function (e) {
            }
        });

    }

    function cmotorSwitchFunction() {
        
        var state;
        if ($('#cmotorswitch').text() == 'ON') {
            state = "OFF";
        } else
            state = "ON";

        $.ajax({
            type: "POST",
            url: $("#publishCholorineOnOffCommandUrl").val(),
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ state: state, pumpStationId: $('#SelectedPumpStationId').val() }),
            dataType: "json",
            success:
                function (data) {
                    if (data.IsSuccess) {
                        if (data.Data == 'Success') {
                            
                            $('#cmotorswitch').text(state);
                            if (state === 'OFF') {
                                $('#cmotorswitch').removeClass('label-success').addClass('label-danger');
                            }
                            else {
                                $('#cmotorswitch').removeClass('label-danger').addClass('label-success');
                            }
                            
                        }
                        else {
                        }

                    } else {

                    }
                },
            error: function (e) {
            }
        });

    }

    function scadaSpanClick() {
        var sensorId = $.trim(this.id.split("__")[1]);
        if (sensorId == null) return;
        if (graphInterval != null)
            clearInterval(graphInterval);
        $.ajax({
            type: "POST",
            url: $("#getScadaSensorDataUrl").val(),
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ sensorId: sensorId }),
            dataType: "json",
            success:
                function (data) {
                    if (data.IsSuccess) {

                        showRealChartScada(data, sensorId);

                    }

                },
            error: function () { }
        });

    }

    var bindFunctions = function () {
        $("#motorswitch").on("click", motorSwitchFunction);
        $("#cmotorswitch").on("click", cmotorSwitchFunction);
        $('span').on('click', scadaSpanClick);
    };

    var init = function () {
        bindFunctions();
    };

    //#endregion 

    return {
        LoadDma: loadDma,
        LoadPumpStation: loadPumpStation,
        ShowScada: showScada,
        ClearAllScada: clearAllScada,
        Init: init
    };
    
})(window);

