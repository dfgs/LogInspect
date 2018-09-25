using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.ViewModels.Modules
{
	public interface IModuleViewModel:IViewModel
	{
		int Rate
		{
			get;
		}
		int Count
		{
			get;
		}
	}
}
