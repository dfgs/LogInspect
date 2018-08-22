using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.ViewModels.Columns;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class TimeStampPropertyViewModel : PropertyViewModel
	{
		public string Value
		{
			get;
			private set;
		}

		public TimeStampPropertyViewModel(ILogger Logger, ColumnViewModel Column,EventViewModel Event) : base(Logger, Column)
		{
			this.Value = Event.GetPropertyValue(Column.Name)?.ToString();
		}
	}
}
