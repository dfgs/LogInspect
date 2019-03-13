using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
using LogInspect.Models;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class TextPropertyViewModel : PropertyViewModel
	{
		
		

		public TextPropertyViewModel(ILogger Logger, string Name,  string Value) : base(Logger, Name,Value)
		{
		}


	}
}
