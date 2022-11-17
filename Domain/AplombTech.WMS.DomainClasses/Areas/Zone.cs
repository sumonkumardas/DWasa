using AplombTech.WMS.Domain.Shared;
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
using NakedObjects.Value;
using System.Windows.Forms;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Features;

namespace AplombTech.WMS.Domain.Areas
{   
    public class Zone : Area
    {
        #region Injected Services
        public LoggedInUserInfoDomainRepository LoggedInUserInfoDomainRepository { set; protected get; }
        #endregion
        public override string Name { get; set; }

        public string DisablePropertyDefault()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.EditZone
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
            {
                return "You do not have permission to Edit";
            }
                
            return null;
        }

        #region Validations
        public string ValidateName(string areaName)
        {
            var rb = new ReasonBuilder();

            Zone zone = (from obj in Container.Instances<Zone>()
                         where obj.Name == areaName
                         select obj).FirstOrDefault();

            if (zone != null)
            {
                if (this.AreaId != zone.AreaId)
                {
                    rb.AppendOnCondition(true, "Duplicate Zone Name");
                }
            }
            return rb.Reason;
        }
        #endregion

        #region Show DMA
        [MemberOrder(20), NotMapped]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("DMA")]
        [TableView(true, "Name")]
        public IList<DMA> DMAs
        {
            get
            {
                IList<DMA> dmas = (from dma in Container.Instances<DMA>()
                                   where dma.Parent.AreaId == this.AreaId
                                   select dma).ToList();
                return dmas;
            }
        }
        #endregion

        #region AddDMA (Action)
        [DisplayName("Add DMA")]
        public void AddDMA(string name)
        {
            DMA dma = Container.NewTransientInstance<DMA>();
            dma.Name = name;

            dma.Parent = this;

            Container.Persist(ref dma);
        }
        public bool HideAddDMA()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.AddDma
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        #region Validations
        public string ValidateAddDMA(string name)
        {
            var rb = new ReasonBuilder();

            DMA dma = (from obj in Container.Instances<DMA>()
                       where obj.Name == name
                       select obj).FirstOrDefault();

            if (dma != null)
            {
                rb.AppendOnCondition(true, "Duplicate DMA Name");
            }
            return rb.Reason;
        }
        #endregion
        #endregion

        #region SetAddress (Action)
        [DisplayName("Set Address")]
        public void SetAddress(string street1, [Optionally] string street2, string zipCode, string zone, string city)
        {
            Address address = Container.NewTransientInstance<Address>();
            address.Street1 = street1;
            address.Street2 = street2;
            address.ZipCode = zipCode;
            address.Zone = zone;
            address.City = city;

            Container.Persist(ref address);
            this.Address = address;
        }       
        public bool HideSetAddress()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.SetZoneAddress
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
                return true;

            if (this.Address != null)
                return true;

            return false;
        }
        #endregion

        public string RemoveZone()
        {
            string message = "<b>Zone is not deleted, it may be in use. </b>";
            string alertMessage = "Are you sure you want to remove this item?";
            string caption = "Remove Item?";
            string isConform = GetAlertMessage(caption, alertMessage);
            if (isConform == "Yes")
            {
                message = "<b>Zone is deleted successfully</b>";
            }
            
            return message;
        }
        public string GetAlertMessage(string caption, string message)
        {

            DialogResult r1 = MessageBox.Show(message,
                                   caption, MessageBoxButtons.YesNo);

            return r1.ToString();
        }

        #region Menu
        public static void Menu(IMenu menu)
        {
            menu.AddAction("AddDMA");
            menu.AddAction("SetAddress");
            //menu.AddAction("RemoveZone");
        }
        #endregion
    }
}
