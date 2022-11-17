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

namespace AplombTech.WMS.Domain.Features
{
    [Bounded]
    public class FeatureType
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        #endregion

        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int FeatureTypeId { get; set; }
        [Title, Required]
        [MemberOrder(10)]
        [StringLength(50)]
        public virtual string FeatureTypeName { get; set; }
        #endregion

        #region Get Properties
        #region Features
        [MemberOrder(50), NotMapped]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Features")]
        [TableView(false, "FeatureName")]
        public IList<Feature> Features
        {
            get
            {
                IList<Feature> features = (from r in Container.Instances<Feature>()
                                           where r.FeatureType.FeatureTypeId == this.FeatureTypeId
                                           select r).ToList();
                return features;
            }
        }
        #endregion
        #endregion

        #region FeatureType
        public enum FeatureTypeEnums
        {
            Area = 1,
            Alert = 2,
            UserAccount = 3,
            Report = 4            
        }
        #endregion
    }
}
