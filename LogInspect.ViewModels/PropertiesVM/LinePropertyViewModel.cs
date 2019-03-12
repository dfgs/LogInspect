using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
using LogInspect.Models.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class LinePropertyViewModel : PropertyViewModel
	{

		public LinePropertyViewModel(ILogger Logger, string Name,  int Index) : base(Logger, Name,Index)
		{
		}
	}
}
