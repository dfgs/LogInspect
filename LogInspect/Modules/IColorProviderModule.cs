using LogInspect.ViewModels;
using LogInspectLib;
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
		Brush GetBackground(Event Event);
		Brush GetBackground(EventViewModel Event);
		//Brush GetBackground(object Severity);
	}
}
