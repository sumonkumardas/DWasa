using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Features;
using NakedObjects.Menu;
using System.ComponentModel.DataAnnotations.Schema;

namespace AplombTech.WMS.Domain.Motors
{
    [Table("ChlorineMotors")]
    public class ChlorineMotor : Motor
    {
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Chlorine Motor";

            t.Append(title);

            return t.ToString();
        }
        
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

        #region Remove Chlorine Motor (Action)
        [DisplayName("Remove Chlorine Motor")]
        public void RemoveChlorineMotor(string remarks)
        {
            this.RemoveRemarks = remarks;
            this.IsRemoved = true;
        }
        public bool RemoveChlorineMotor()
        {
            if (this.IsRemoved)
                return true;

            return false;
        }
        #endregion

        #region Menu
        public static void Menu(IMenu menu)
        {
            menu.AddAction("RemoveChlorineMotor");
        }
        #endregion
    }
}
