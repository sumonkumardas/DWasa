using AplombTech.WMS.Domain.Shared;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Sensors
{
    [Table("FlowSensors")]
    public class FlowSensor : Sensor
    {
        #region Life Cycle Methods
        // This region should contain any of the 'life cycle' convention methods (such as
        // Created(), Persisted() etc) called by the framework at specified stages in the lifecycle.

        public override void Persisting()
        {
            AuditFields.InsertedBy = Container.Principal.Identity.Name;
            AuditFields.InsertedDateTime = DateTime.Now;
            AuditFields.LastUpdatedBy = Container.Principal.Identity.Name;
            AuditFields.LastUpdatedDateTime = DateTime.Now;
            this.CurrentValue = 0;
            this.CumulativeValue = 0;
        }

        #endregion

        [MemberOrder(30), Required, Disabled]
        [DisplayName("Total")]
        public virtual decimal CumulativeValue { get; set; }
        public virtual decimal LitrePerMinuteValue { get; set; }
        public virtual decimal? MeterCubePerHourValue { get; set; }
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Flow Sensor";

            t.Append(title);

            return t.ToString();
        }
    }
}
