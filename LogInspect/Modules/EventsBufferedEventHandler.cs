using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public delegate void EventsBufferedEventHandler(object sender, EventsBufferedEventArgs e);

	public class EventsBufferedEventArgs:EventArgs
	{
		public BufferItem[] Items
		{
			get;
			private set;
		}

		

		public EventsBufferedEventArgs(BufferItem[] Items)
		{
			this.Items = Items;
		}
	}


}
