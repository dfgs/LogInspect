using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Models
{
	public interface IVirtualCollection
	{
		/*Event this[int Index]
		{
			get;
		}*/
		IEnumerable<Event> GetEvents(int StartIndex, int Count);

	}
}
