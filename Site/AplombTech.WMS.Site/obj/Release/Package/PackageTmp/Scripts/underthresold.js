

$("#showUT").click(function (e) {
    e.preventDefault();
    $('#results').empty();
    if (!validate())
        return;

    var model = setModel();
    $.ajax({
        type: 'POST',
        url: $("#underThresoldReportUrl").val(),
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ model: model }),
        dataType: "html",
        success: function (data) {
            $('#results').html(data);
        }
    });
});

function setModel() {
    var model = {
        ReportType: $('#ReportType').val(),
        Month: $('#Month').val(),
        Year: $('#Year').val(),
        Week: $('#Week').val(),
        Day: $('#Day').val(),
        Hour: $('#Hour').val(),
        SelectedPumpStationId: $('#SelectedPumpStationId').val(),
        TransmeType: $('#TransmeType').val()
    }

    return model;
}

$(function () {
    $('#inputPanel').hide();
});

$("#Month").change(function () {
    validate();
    var monthValue = $('#Month').val();
    if (monthValue == 4 || monthValue == 6 || monthValue == 9 || monthValue == 11) {
        $("#Day option[value='31']").hide();
    } else if (monthValue == 2) {
        $("#Day option[value='30']").hide();
        $("#Day option[value='31']").hide();

        if ($('#Year').val() > 0) {
            if (!leapYear($('#Year').val()))
                $("#Day option[value='29']").hide();
            else {
                $("#Day option[value='29']").show();
            }
        }
    } else {
        $("#Day option[value='29']").show();
        $("#Day option[value='30']").show();
        $("#Day option[value='31']").show();
    }
});

function leapYear(year) {
    return new Date(year, 1, 29).getMonth() == 1;
}

$("#ReportType").change(function () {
    validate();
    var reportType = $('#ReportType').val();
    $('#inputPanel').show();
    if (reportType == 1) {
        $('#Week').hide();
        $('#Year').show();
        $('#Month').show();
        $('#Day').show();
        $('#Hour').show();
        $("#TransmeType option[value='1']").show();
        $("#TransmeType option[value='2']").show();
        $("#TransmeType option[value='5']").show();
    }
    if (reportType == 2) {
        $('#Week').hide();
        $('#Hour').hide();
        $('#Year').show();
        $('#Month').show();
        $('#Day').show();
    }
    if (reportType == 3) {
        $('#Hour').hide();
        $('#Month').hide();
        $('#Day').hide();
        $('#Year').show();
        $('#Week').show();
    }
    if (reportType == 4) {
        $('#Week').hide();
        $('#Day').hide();
        $('#Hour').hide();
        $('#Year').show();
        $('#Month').show();
        $('#Day').hide();
    }
    $('#exp').show();
    if (reportType == 5) {
        $('#Month').hide();
        $('#Day').hide();
        $('#Hour').hide();
        $('#Year').show();
        $('#Month').show();
        $('#Day').hide();
        //$('#inputPanel').hide();
        //$("#TransmeType option[value='1']").show();
        //$("#TransmeType option[value='2']").show();
        //$("#TransmeType option[value='5']").show();
        $('#exp').hide();
    }

    if (reportType != 5 && reportType != 1) {
        //$("#TransmeType option[value='1']").hide();
        //$("#TransmeType option[value='2']").hide();
        //$("#TransmeType option[value='5']").hide();
    }
});

$("#SelectedPumpStationId").change(function () {
    validate();
});
$("#TransmeType").change(function () {
    validate();
});

$("#Year").change(function () {
    validate();
});

$("#Week").change(function () {
    validate();
});

$("#Day").change(function () {
    validate();
});

$("#Hour").change(function () {
    validate();
});

function validate() {
    var valid = true;
    if ($('#SelectedPumpStationId').val() <= 0) {
        $("#SelectedPumpStationId").addClass("input-validation-error");
        $("#pumpstationvalid").text("Pumpstation required");
        valid = false;

    } else {
        $("#SelectedPumpStationId").addClass("valid");
        $("#pumpstationvalid").text("");
    }

    if ($('#ReportType').val() <= 0) {
        $("#ReportType").addClass("input-validation-error");
        $("#reporttypevalid").text("Report Type required");
        valid = false;

    } else {
        $("#ReportType").addClass("valid");
        $("#reporttypevalid").text("");
    }

    if ($('#TransmeType').val() <= 0) {
        $("#TransmeType").addClass("input-validation-error");
        $("#sensorvalid").text("Sensor Type required");
        valid = false;

    } else {
        $("#TransmeType").addClass("valid");
        $("#sensorvalid").text("");
    }

    if ($('#Year').val() < 2016) {
        $("#Year").addClass("input-validation-error");
        $("#yearvalid").text("year required");
        valid = false;

    } else {
        $("#Year").addClass("valid");
        $("#yearvalid").text("");
    }

    if (($('#ReportType').val() == 1 || $('#ReportType').val() == 2 || $('#ReportType').val() == 4) && $('#Month').val() <= 0) {
        $("#Month").addClass("input-validation-error");
        $("#monthvalid").text("month required");
        valid = false;

    } else {
        $("#Month").addClass("valid");
        $("#monthvalid").text("");
    }

    if ($('#ReportType').val() == 3 && $('#Week').val() < 1) {
        $("#Week").addClass("input-validation-error");
        $("#weekvalid").text("Week required");
        valid = false;

    } else {
        $("#Week").addClass("valid");
        $("#weekvalid").text("");
    }

    if (($('#ReportType').val() == 1 || $('#ReportType').val() == 2) && $('#Day').val() < 1) {
        $("#Day").addClass("input-validation-error");
        $("#dayvalid").text("day required");
        valid = false;

    } else {
        $("#Day").addClass("valid");
        $("#dayvalid").text("");
    }

    if ($('#ReportType').val() == 1 && $('#Hour').val() < 1) {
        $("#Hour").addClass("input-validation-error");
        $("#hourvalid").text("Hour required");
        valid = false;

    } else {
        $("#Hour").addClass("valid");
        $("#hourvalid").text("");
    }

    return valid;
}

$('#exp').click(function(e) {
    if (!validate())
        e.preventDefault();
});



