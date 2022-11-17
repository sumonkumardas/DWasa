using AplombTech.WMS.Domain.Features;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Devices
{
    [Table("Routers")]
    public class Router : Device
    {
        [MemberOrder(20)]
        [StringLength(50)]
        public virtual string MACAddress { get; set; }
        [MemberOrder(30), Title]
        [StringLength(20)]
        public virtual string IP { get; set; }
        [MemberOrder(40)]
        public virtual int Port { get; set; }

        public string DisablePropertyDefault()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.EditRouter
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
            {
                return "You do not have permission to Edit";
            }

            return null;
        }
    }
}
