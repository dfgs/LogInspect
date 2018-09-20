using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class TimeStampPropertyViewModel : PropertyViewModel
	{
		

		public TimeStampPropertyViewModel(ILogger Logger, string Name,TextAlignment Alignment,DateTime Value) : base(Logger, Name,Alignment)
		{
			this.Value = Value;
		}
	}
}
