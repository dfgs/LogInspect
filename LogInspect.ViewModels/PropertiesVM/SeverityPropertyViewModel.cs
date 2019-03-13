using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class SeverityPropertyViewModel : PropertyViewModel
	{
		public string Background
		{
			get;
			private set;
		}
		

		public SeverityPropertyViewModel(ILogger Logger, string Name,   string Value,string Background) : base(Logger, Name,Value)
		{
			this.Background = Background;
		}
	}
}
