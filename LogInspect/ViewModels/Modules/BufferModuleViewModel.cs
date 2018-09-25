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
	public class BufferModuleViewModel<T> : ModuleViewModel,IBufferModuleViewModel
	{


		



		public BufferModuleViewModel(ILogger Logger, int RefreshInterval, IBufferModule<T> Module) : base(Logger, RefreshInterval,Module)
		{
		}

		


	}
}
