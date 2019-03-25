using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Models
{
	public interface IEventList
	{
		void Add(Event Event);
	}
}
