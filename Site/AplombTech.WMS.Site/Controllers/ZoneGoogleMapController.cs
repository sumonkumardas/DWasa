using AplombTech.WMS.QueryModel.Reports;
using AplombTech.WMS.QueryModel.Repositories;
using NakedObjects;
using NakedObjects.Facade;
using NakedObjects.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Sensors;

namespace AplombTech.WMS.Site.Controllers
{
  [Authorize]
  public class ZoneGoogleMapController : SystemControllerImpl
  {
    #region Injected Services
    public ReportRepository _reportRepository { set; protected get; }
    #endregion

    public ZoneGoogleMapController(IFrameworkFacade facade, IIdHelper idHelper) : base(facade, idHelper) { }

    // Uncomment this constructor if you wish to have an IDomainObjectContainer and/or domain services injected.
    // You will also need to ensure you have NakedObjects.Core package installed & add using NakedObjects;
    public ZoneGoogleMapController(IFrameworkFacade facade, IIdHelper idHelper, INakedObjectsFramework nakedObjectsFramework)
        : base(facade, idHelper)
    {
      nakedObjectsFramework.DomainObjectInjector.InjectInto(this);
    }

    public JsonResult GetSingleSensorStatus(int sensorId)
    {
      Sensor sensor = _reportRepository.GetPumpSingleSensor(sensorId);
      string unit = GetSensorUnit(sensor);
      return Json(new { Value = sensor.CurrentValue, PumpStationId = sensor.PumpStation.AreaId, Unit = unit, IsSuccess = true }, JsonRequestBehavior.AllowGet);
    }

    public JsonResult GetOverViewDataOfPumpStation(int pumpStationId)
    {
      Dictionary<string, string> values = _reportRepository.GetPumpStationOverView(pumpStationId);
      return Json(new { Data = values, IsSuccess = true }, JsonRequestBehavior.AllowGet);
    }

    private string GetSensorUnit(Sensor sensor)
    {
      //if (sensor is PressureSensor)
      //{
      return sensor.UnitName != null ? sensor.UnitName : string.Empty;
      //}

      //else if (sensor is FlowSensor)
      //{
      //    return ((FlowSensor)sensor).Unit != null?((FlowSensor)sensor).Unit.Name:string.Empty;
      //}

      //else if (sensor is LevelSensor)
      //{
      //    return ((LevelSensor)sensor).Unit != null?((LevelSensor)sensor).Unit.Name:string.Empty;
      //}

      //else if (sensor is EnergySensor)
      //{
      //    return ((EnergySensor)sensor).Unit != null ? ((EnergySensor)sensor).Unit.Name:string.Empty;
      //}
      //else
      //{
      //    return string.Empty;
      //}



    }

    public JsonResult GetZoneGoogleMap(int zoneId)
    {
      List<MapLocation> locations = new List<MapLocation>();
      ZoneGoogleMap model = _reportRepository.GetSingleAreaGoogleMap(zoneId);
      foreach (var zone in model.Zones)
      {
        locations.Add(new MapLocation(zone.Name, zone.Location, zone.AreaId));
        foreach (var dma in zone.DMAs)
        {
          locations.Add(new MapLocation(dma.Name, dma.Location, dma.AreaId));
          locations.AddRange(dma.PumpStations.Select(pumpStation => new MapLocation(pumpStation.Name, pumpStation.Location, pumpStation.AreaId)));
        }
      }
      return Json(new { Data = locations, IsSuccess = true }, JsonRequestBehavior.AllowGet);
    }
  }
}
