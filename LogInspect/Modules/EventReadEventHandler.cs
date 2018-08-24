using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public delegate void EventReadEventHandler(object sender, EventReadEventArgs e);

	public class EventReadEventArgs:EventArgs
	{
		public Event Event
		{
			get;
			private set;
		}

		public EventReadEventArgs(Event Event)
		{
			this.Event=Event;
		}
	}


}
