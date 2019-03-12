using ModuleLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.ViewModels
{
	public interface IViewModel: IModule, INotifyPropertyChanged, IDisposable
	{
	}
}
