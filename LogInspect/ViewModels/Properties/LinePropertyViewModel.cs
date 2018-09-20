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
	public class LinePropertyViewModel : PropertyViewModel
	{
		

		public LinePropertyViewModel(ILogger Logger, string Name, TextAlignment Alignment, int Index) : base(Logger, Name,Alignment)
		{
			this.Value = Index;
		}
	}
}
