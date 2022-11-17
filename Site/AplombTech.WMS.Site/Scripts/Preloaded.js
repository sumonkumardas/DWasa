var nofModule = (function (window, undefined) {
    function showPreloadedScada() {

        var zoneId = $('#selectedZone').text();
        var dmaId = $('#selectedDma').text();
        var pumpStationId = $('#selectedPumpStation').text();
        if (dmaId && pumpStationId) {
            $("#SelectedZoneId").val("1");
            scadModule.LoadDma(zoneId);
            $("#SelectedDmaId").val("2");
            scadModule.LoadPumpStation(dmaId);
            $("#SelectedPumpStationId").val("3");
            scadModule.ShowScada();
        }
    }

    return {
            ShowPreloadedScada: showPreloadedScada
        };
    
})(window);
