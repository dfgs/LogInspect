using LogInspect.Models;
using LogInspectLib;
using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public class EventIndexerBufferModule: ThreadModule
	{
		

		private int lookupRetryDelay;
		private List<BufferItem> buffer;

		public event EventsBufferedEventHandler EventsBuffered;
		public event EventHandler Reseted;

		public EventIndexerBufferModule( ILogger Logger,  EventIndexerModule IndexerModule, int LookupRetryDelay) : base("IndexerBuffer", Logger,ThreadPriority.Lowest)
		{
			buffer = new List<BufferItem>();
			this.lookupRetryDelay = LookupRetryDelay;
			IndexerModule.Indexed += IndexerModule_Indexed;
			IndexerModule.Reseted += IndexerModule_Reseted;
		}

		private void IndexerModule_Reseted(object sender, EventArgs e)
		{
			lock(buffer)
			{
				buffer.Clear();
				Reseted?.Invoke(this, EventArgs.Empty);
			}
		}

		private void IndexerModule_Indexed(object sender, EventIndexedEventArgs e)
		{
			lock (buffer)
			{
				buffer.Add(new BufferItem() { EventIndex=e.EventIndex, LineIndex = e.LineIndex, Event = e.Event });
			}
		}

		protected override sealed void ThreadLoop()
		{
			BufferItem[] items;

			while(State == ModuleStates.Started)
			{
				WaitHandles(lookupRetryDelay, QuitEvent);
				lock (buffer)
				{
					items = buffer.ToArray();
					buffer.Clear();
				}
				if ((State == ModuleStates.Started) && (items.Length > 0)) EventsBuffered?.Invoke(this, new EventsBufferedEventArgs(items));
			}
		}
		
		


	}
}
