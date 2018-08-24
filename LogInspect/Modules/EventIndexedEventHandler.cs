using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public delegate void EventIndexedEventHandler(object sender, EventIndexedEventArgs e);

	public class EventIndexedEventArgs:EventArgs
	{
		public Event Event
		{
			get;
			private set;
		}

		public int EventIndex
		{
			get;
			private set;
		}

		public int LineIndex
		{
			get;
			private set;
		}

		public EventIndexedEventArgs(Event Event,int EventIndex,int LineIndex)
		{
			this.Event=Event;this.EventIndex = EventIndex;this.LineIndex = LineIndex;
		}
	}


}
