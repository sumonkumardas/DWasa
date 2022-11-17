using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Utility
{
    public class PingBroker
    {
        public bool IsBrokerLive(string stringIpAddress)
        {
            bool isLive = false;

            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            IPAddress ipAdress = IPAddress.Parse(stringIpAddress);
            PingReply reply = pingSender.Send(ipAdress, timeout, buffer, options);
            
            if (reply.Status == IPStatus.Success)
            {
                isLive = true;
            }

            return isLive;
        }
    }
}
