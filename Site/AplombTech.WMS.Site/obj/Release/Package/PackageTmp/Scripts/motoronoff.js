

$("#showUT").click(function (e) {
    e.preventDefault();
    $('#results').empty();
    if (!validate())
        return;

    var model = setModel();
    $.ajax({
        type: 'POST',
        url: $("#motorOnOffReportUrl").val(),
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
        FromDateTime: $('#motorOnOffStartDate').val(),
        EndDateTime: $('#motorOnOffEndDate').val(),
        SelectedPumpStationId: $('#SelectedPumpStationId').val()
    }

    return model;
}


$("#SelectedPumpStationId").change(function () {
    validate();
});
$("#motorOnOffStartDate").change(function () {
    validate();
});

$("#motorOnOffEndDate").change(function () {
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

    var startDate = new Date($('#motorOnOffStartDate').val());
    var endDate = new Date($('#motorOnOffEndDate'));

    if (endDate > startDate) {
        $("#motorOnOffStartDate").addClass("input-validation-error");
        $("#startdatevalid").text("End date is bigger than start date.");
    }
    else {
        $("#motorOnOffStartDate").addClass("valid");
        $("#startdatevalid").text("");
    }
    return valid;
}

$('#exp').click(function(e) {
    if (!validate())
        e.preventDefault(); 
});



