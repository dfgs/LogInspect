using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels
{
	public class PropertyViewModel : ViewModel
	{


		public ColumnViewModel Column
		{
			get;
			private set;
		}

		public EventViewModel Event
		{
			get;
			private set;
		}

		public object Value
		{
			get;
			private set;
		}
		public PropertyViewModel(ILogger Logger,ColumnViewModel Column,EventViewModel Event) : base(Logger)
		{
			this.Column = Column;
			this.Event = Event;
			this.Value = Column.GetValue(Event);
		}


	}
}
