using AplombTech.WMS.Persistence.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Vfds;

namespace AplombTech.WMS.Persistence.Repositories
{
	public class VfdDataProcessorRepository
	{
		private readonly WMSDBContext _wmsdbcontext;

		public VfdDataProcessorRepository (WMSDBContext wmsdbcontext)
		{
			_wmsdbcontext = wmsdbcontext;
		}
		public VariableFrequencyDrive GetVfdByUuid (string uuid)
		{
			return (from c in _wmsdbcontext.Vfds where c.UUID == uuid select c).Single();
		}
	}
}
