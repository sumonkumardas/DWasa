using AplombTech.WMS.JsonParser.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.DeviceMessages
{
	public class VfdMessage : DeviceMessage
	{
		public VfdMessage ()
		{
			VfdList = new List<VfdValue>();
		}
		public int VfdDataComplete { get; set; }
		public int LogCount { get; set; }

		public IList<VfdValue> VfdList { get; set; }
	}
}
