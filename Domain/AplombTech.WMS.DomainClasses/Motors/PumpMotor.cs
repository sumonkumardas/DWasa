using AplombTech.WMS.Domain.Features;
using NakedObjects;
using NakedObjects.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AplombTech.WMS.Domain.Motors
{
    [Table("PumpMotors")]
    public class PumpMotor : Motor
    {
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Pump Motor";

            t.Append(title);

            return t.ToString();
        }
        [MemberOrder(20)]
        [StringLength(50)]
        public virtual string ModelNo { get; set; }
        [MemberOrder(30)]
        public virtual decimal Capacity { get; set; }
        [MemberOrder(40)]
        public virtual int StaticWaterLevel { get; set; }        

        public string DisablePropertyDefault()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.EditMotor
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
