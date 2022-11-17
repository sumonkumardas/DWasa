using System.Collections.Generic;
using NakedObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using AplombTech.WMS.Domain.Areas;


namespace AplombTech.WMS.QueryModel.Reports
{
    [NotMapped]
    public class Summary : IViewModel
    {
        public IDomainObjectContainer Container { set; protected get; }  //Injected service
        public virtual IList<Zone> Zones { get; set; }
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Summary";

            t.Append(title);

            return t.ToString();
        }

        public string[] DeriveKeys()
        {
            string[] ids = Zones.Select(s => s.AreaId.ToString()).ToArray();
            return ids;
        }

        public void PopulateUsingKeys(string[] keys)
        {
            IList<string> ids = keys.ToList();
            Zones = Container.Instances<Zone>().Where(w => ids.Contains(w.AreaId.ToString())).ToList();

        }
    }
}
