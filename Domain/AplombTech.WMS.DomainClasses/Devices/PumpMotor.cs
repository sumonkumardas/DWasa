using AplombTech.WMS.Domain.Features;
using NakedObjects;
using NakedObjects.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Devices
{
    [Table("PumpMotors")]
    public class PumpMotor : Motor
    {
        [Title]
        [MemberOrder(20)]
        [StringLength(50)]
        public virtual string ModelNo { get; set; }
        public virtual decimal Capacity { get; set; }
        public virtual int StaticWaterLevel { get; set; }
        [StringLength(250)]
        public virtual string RemoveRemarks { get; set; }
        public bool HideRemoveRemarks()
        {
            if (IsRemoved)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public virtual bool IsRemoved { get; set; }
        public bool HideIsRemoved()
        {
            return true;
        }

        public string DisablePropertyDefault()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.EditPump
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
            {
                return "You do not have permission to Edit";
            }

            return null;
        }

        #region RemovePump (Action)
        [DisplayName("Remove Pump")]
        public void RemovePump(string remarks)
        {
            this.RemoveRemarks = remarks;
            this.IsRemoved = true;
        }
        public bool RemovePump()
        {
            if (this.IsRemoved)
                return true;

            return false;
        }
        #endregion

        #region Menu
        public static void Menu(IMenu menu)
        {
            menu.AddAction("RemovePump");           
        }
        #endregion
    }
}
