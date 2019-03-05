using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LogInspect.ViewModels
{
	public abstract class ViewModel:Module,IViewModel
	{

		public event PropertyChangedEventHandler PropertyChanged;


		public ViewModel(ILogger Logger):base(Logger)
		{
			
			
		}

		

		protected virtual void OnPropertyChanged([CallerMemberName]string PropertyName=null)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
		}

		


	}
}
