


var map;
var dmaPolygon;
var myLatLng = { lat: 23.751284, lng: 90.393570 };
var marker;
var z2marker;
var z3marker;
var z4marker;
var zone1;
var markers = {};
var pumpStations = $('*[id^="pump_"]');
var markers = {};

//var zone1PolyGonCoords = [
//    { lat: 23.784850, lng: 90.389999 },
//    { lat: 23.784676, lng: 90.393992 },
//    { lat: 23.784421, lng: 90.394196 },
//    { lat: 23.785295, lng: 90.397071 },
//    { lat: 23.785108, lng: 90.398809 },
//    { lat: 23.785108, lng: 90.398809 },
//    { lat: 23.775901, lng: 90.393132 },
//    { lat: 23.775371, lng: 90.389860 },
//    { lat: 23.781862, lng: 90.389291 },
//    { lat: 23.784850, lng: 90.389999 }
//];

var infowindow;

function initMap() {
    map = new window.google.maps.Map(document.getElementById('map'), {
        center: myLatLng,
        zoom: 13
    });

    var legend = document.getElementById('legend');

    map.controls[window.google.maps.ControlPosition.RIGHT_TOP].push(legend);
}

function ShowInfoPopUp(marker) {
    //alert(marker.id + marker.title + marker.getPosition().lat());
    $('#pumpname').text(marker.title);
    $('#myModal').modal('show');
}

$("#modalSave").click(function () {
    
    if ($('#ServiceType').val() > 0) {
        $('#myModal').modal('hide');
        $("#servicevalid").text("");
        if ($('#ServiceType').val() == 1) {
            window.location = scadaUrl + '?pumpStationId=3';
        } 
        if ($('#ServiceType').val() == 2) {
            window.location = drillDownUrl;
        }

        if ($('#ServiceType').val() == 3) {
            window.location = underThresoldUrl;
        }
    } else {
        $("#servicevalid").text("Select a Service");
    }
    //$('#body').load(url, { pumpStationId: 3 });

});

function drawDmaAndPumpStation(zoneId) {
    markers = [];
    $.ajax({
        type: "POST",
        url: $("#getZoneGooleMapUrl").val(),
        data: JSON.stringify({ zoneId: zoneId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success:
            function (data) {
                for (var key in data.Data) {
                    if (data.Data.hasOwnProperty(key)) {
                        if (data.Data[key].Locations) {
                            var location = data.Data[key].Locations;
                            var name = data.Data[key].Name;
                            var deviceId = data.Data[key].DeviceId;
                            var polyCoords = [];
                            if (location.length > 0) {
                                for (var k in location) {
                                    if (location.hasOwnProperty(k)) {
                                        polyCoords.push({ lat: location[k].Latitude, lng: location[k].Longitude });
                                    }
                                }
                                // Construct the polygon.

                                dmaPolygon = new window.google.maps.Polygon({
                                    paths: polyCoords,
                                    strokeColor: 'green',
                                    strokeOpacity: 0.8,
                                    strokeWeight: 2,
                                    fillColor: 'green',
                                    fillOpacity: 0.35
                                });

                                dmaPolygon.setMap(map);
                            }
                            if (location.length == 1) {
                                for (var k in location) {
                                    if (location.hasOwnProperty(k)) {
                                        marker = new window.google.maps.Marker({
                                            position: { lat: location[k].Latitude, lng: location[k].Longitude },
                                            map: map,
                                            label: "P",
                                            title: name,
                                            icon: '../Images/Icons/pump.png',
                                            animation: google.maps.Animation.DROP,
                                            id: 'marker_' + deviceId
                                        });
                                        marker.addListener('click', function () {
                                            ShowInfoPopUp(marker);
                                        });
                                        markers['marker_' + deviceId] = marker;
                                        map.setZoom(15);
                                        map.panTo(marker.position);
                                    }
                                }

                            }
                        }

                    }
                }
            },
        error: function () { }
    });
}


function highlightPolyGon(zone1) {
    zone1.setOptions({ strokeColor: 'black' });
}

