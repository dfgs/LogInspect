using LogInspect.Models;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface ILogFileLoaderModule:IModule
	{
		
		IEnumerable<Event> Load();
	}
}
