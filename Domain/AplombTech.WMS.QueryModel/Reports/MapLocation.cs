using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Reports
{
  public class MapLocation
  {
    public string Name { get; set; }
    public List<GeoLocation> Locations { get; set; }
    public int DeviceId { get; set; }

    public MapLocation(string name, string location, int deviceId)
    {
      this.Name = name;
      this.DeviceId = deviceId;
      if (location != null)
        Locations = ConvertLocationStringToPoints(location);
    }

    private List<GeoLocation> ConvertLocationStringToPoints(string location)
    {
      string[] points = location.Split('|');

      return (from point in points select point.Split(',') into values where values.Length == 2 select new GeoLocation() { Latitude = Convert.ToDecimal(values[1]), Longitude = Convert.ToDecimal(values[0]) }).ToList();

    }
  }
}
