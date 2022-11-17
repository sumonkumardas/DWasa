using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Shared;
using NakedObjects.Menu;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Features;

namespace AplombTech.WMS.Domain.Areas
{
    public class DMA : Area
    {
        #region Injected Services
        public LoggedInUserInfoDomainRepository LoggedInUserInfoDomainRepository { set; protected get; }
        #endregion
        public override string Name { get; set; }
        public string DisablePropertyDefault()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.EditDma
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

            DMA dma = (from obj in Container.Instances<DMA>()
                       where obj.Name == areaName
                       select obj).FirstOrDefault();

            if (dma != null)
            {
                if (this.AreaId != dma.AreaId)
                {
                    rb.AppendOnCondition(true, "Duplicate DMA Name");
                }
            }
            return rb.Reason;
        }
        #endregion

        #region Show PumpStation
        [MemberOrder(20), NotMapped]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [DisplayName("Pump Station")]
        [TableView(true, "Name")]
        public IList<PumpStation> PumpStations
        {
            get
            {
                IList<PumpStation> stations = (from station in Container.Instances<PumpStation>()
                                               where station.Parent.AreaId == this.AreaId
                                               select station).ToList();
                return stations;
            }
        }
        #endregion

        #region AddPumpStation (Action)

        [DisplayName("Add PumpStation")]
        public void AddPumpStation(string name)
        {
            PumpStation station = Container.NewTransientInstance<PumpStation>();
            station.Name = name;

            station.Parent = this;

            Container.Persist(ref station);
        }
        #region Validations
        public string ValidateAddPumpStation(string name)
        {
            var rb = new ReasonBuilder();

            PumpStation station = (from obj in Container.Instances<PumpStation>()
                                   where obj.Name == name
                                   select obj).FirstOrDefault();

            if (station != null)
            {
                rb.AppendOnCondition(true, "Duplicate DMA Name");
            }
            return rb.Reason;
        }
        #endregion
        public bool HideAddPumpStation()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.AddPumpStation
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
                return true;

            return false;
        }
        #endregion

        #region SetAddress (Action)
        [DisplayName("Set Address")]
        public void SetAddress([Required]string street1, string street2, string zipCode, string zone, string city)
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
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.SetPumpStationAddress
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
                return true;

            if (this.Address != null)
                return true;

            return false;
        }
        #endregion

        #region Menu

        public static void Menu(IMenu menu)
        {
            menu.AddAction("AddPumpStation");
            menu.AddAction("SetAddress");
        }

        #endregion

        public override Area Parent { get; set; }
        [PageSize(10)]
        public IQueryable<Zone> AutoCompleteParent([MinLength(3)] string name)
        {
            IQueryable<Zone> zones = (from z in Container.Instances<Zone>()
                                      where z.Name.Contains(name)
                                      select z).OrderBy(o => o.Name);

            return zones;
        }
    }
}
