using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.JsonParser.DeviceMessages.Exceptions;

namespace AplombTech.WMS.JsonParser.DeviceMessages.Parsing
{
	public class VfdMessageParser : IDeviceMessageParser<VfdMessage>
	{
		public VfdMessage ParseMessage (string messageString)
		{
			try
			{
				VfdMessage messageObject = JsonManager.GetVfdObject(messageString);
				return messageObject;
			}
			catch (Exception)
			{
				throw new SensorMessageException(); ;
			}

		}
	}
}
