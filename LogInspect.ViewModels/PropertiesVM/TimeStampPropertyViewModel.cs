using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
using LogInspect.Models;
using LogInspect.Models.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class TimeStampPropertyViewModel : PropertyViewModel
	{
		

		public TimeStampPropertyViewModel(ILogger Logger, string Name, DateTime Value) : base(Logger, Name,Value)
		{
		}

	}
}
