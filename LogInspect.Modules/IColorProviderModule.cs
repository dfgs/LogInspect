using LogInspect.Models;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LogInspect.Modules
{
	public interface IColorProviderModule:IModule
	{
		string GetBackground(Event Event);
	}
}
