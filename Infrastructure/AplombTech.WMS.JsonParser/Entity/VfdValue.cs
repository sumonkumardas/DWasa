using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.Entity
{
	public class VfdValue
	{
		public string VfdUid { get; set; }
		public decimal Current { get; set; }
		public decimal Energy { get; set; }
		public decimal Frequency { get; set; }
		public decimal Power { get; set; }
		public decimal Voltage { get; set; }
		public decimal OperatingHour { get; set; }
		public decimal RunningHour { get; set; }
	}
}
