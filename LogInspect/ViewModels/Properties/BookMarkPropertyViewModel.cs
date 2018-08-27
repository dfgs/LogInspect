using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.ViewModels.Columns;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class BookMarkPropertyViewModel : PropertyViewModel
	{
		public EventViewModel Event
		{
			get;
			private set;
		}

		public bool Value
		{
			get { return Event.IsBookMarked; }
		}
		public BookMarkPropertyViewModel(ILogger Logger, ColumnViewModel Column,EventViewModel Event) : base(Logger, Column)
		{
			this.Event = Event;
		}
	}
}
