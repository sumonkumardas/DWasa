using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.WindowsService
{
  public static class Log
  {
    public static void WriteLog(string message)
    {
      StreamWriter sw = null;
      try
      {
        string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt";
        if (!File.Exists(filePath))
        {
          File.Create(filePath).Dispose();
        }
        sw = new StreamWriter(filePath,true);
        sw.WriteLine(DateTime.Now.ToString()+" : "+message);
        sw.Flush();
        sw.Close();
      }
      catch (Exception ex)
      {

      }

    }
  }
}
