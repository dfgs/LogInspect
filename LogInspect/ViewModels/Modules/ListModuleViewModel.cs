using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Modules;
using LogLib;

namespace LogInspect.ViewModels.Modules
{
	public class ListModuleViewModel<T> : ModuleViewModel,IListModuleViewModel
	{

		

		public ListModuleViewModel(ILogger Logger, int RefreshInterval, IListModule<T> Module) : base(Logger, RefreshInterval,Module)
		{
		}

		
	}
}
