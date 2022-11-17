using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AplombTech.WMS.Domain.Vfds;
using AplombTech.WMS.Site.Models;

namespace AplombTech.WMS.Site.Extensions
{
    public static class WMSExtensions
    {
        public static VariableFrequencyDriveViewModel MapVDFViewModel(this VariableFrequencyDrive vfdData)
        {
            VariableFrequencyDriveViewModel vfdDataModel = new VariableFrequencyDriveViewModel();
            if (vfdData != null)
            {
                vfdDataModel.Current = vfdData.Current;
                vfdDataModel.Energy = vfdData.Energy;
                vfdDataModel.Frequency = vfdData.Frequency;
                vfdDataModel.Power = vfdData.Power;
                vfdDataModel.Voltage = vfdData.Voltage;
                vfdDataModel.OperatingHour = vfdData.OperatingHour;
                vfdDataModel.RunningHour = vfdData.RunningHour;
            }
           
            return vfdDataModel;
        }
    }
}