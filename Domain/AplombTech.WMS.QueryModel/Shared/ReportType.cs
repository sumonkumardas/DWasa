using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Shared
{
    public enum ReportType
    {
        Hourly = 1,
        Daily = 2,
        Weekly = 3,
        Monthly = 4,
        Realtime = 5
    }

    public enum ServiceType
    {
        Scada = 1,
        Drill_Down = 2
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}
