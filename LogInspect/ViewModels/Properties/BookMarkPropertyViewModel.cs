using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
using LogInspectLib.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class BookMarkPropertyViewModel : PropertyViewModel
	{
		

		public BookMarkPropertyViewModel(ILogger Logger, string Name) : base(Logger,Name,false)
		{
		}

	}
}
