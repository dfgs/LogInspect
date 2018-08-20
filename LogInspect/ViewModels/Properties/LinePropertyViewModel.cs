using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.ViewModels.Columns;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class LinePropertyViewModel : PropertyViewModel
	{
		public int Value
		{
			get;
			private set;
		}

		public LinePropertyViewModel(ILogger Logger, ColumnViewModel Column,EventViewModel Event) : base(Logger, Column)
		{
			this.Value = Event.LineIndex + 1;
		}
	}
}
