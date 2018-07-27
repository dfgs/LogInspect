using LogInspectLib;
using LogInspectLib.Readers;
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

	public class EventIndexerModule : ThreadModule
	{
		private Dictionary<int,long> dictionary;
		private EventReader eventReader;

		public event EventIndexedHandler EventIndexed;

		public int Count
		{
			get { return dictionary.Count; }
		}

		public EventIndexerModule(ILogger Logger, EventReader EventReader) : base("EventIndexer",Logger)
		{
			this.eventReader = EventReader;
			dictionary = new Dictionary<int, long>();
		}

		protected override void ThreadLoop()
		{
			Event ev ;
			int index;
			long previousTicks,newTicks;

			previousTicks=Environment.TickCount;
			index = 0;
			while(State==ModuleStates.Started)
			{
				while ((State == ModuleStates.Started) && (!eventReader.EndOfStream))
				{
					
					try
					{
						ev = eventReader.Read();
					}
					catch (Exception ex)
					{
						Log(ex);
						return;
					}
					lock (dictionary)
					{
						dictionary.Add(index, ev.Position);
					}
					newTicks = Environment.TickCount;
					if (newTicks - previousTicks >= 500)	// prevent UI hangs because of too many updates
					{
						EventIndexed?.Invoke(ev, index);
						previousTicks = newTicks;
					}
					index++;
					Thread.Sleep(1); // limit cpu usage
				}
				if (State == ModuleStates.Started) WaitHandles(1000, QuitEvent);
			}
		}

		public long GetStreamPos(int Index)
		{
			long result;

			if (Index == 0) return 0; // we return valid result even if thread is not yet started
			lock(dictionary)
			{
				try
				{
					result = dictionary[Index];
				}
				catch(Exception ex)
				{
					Log(ex);
					return -1;
				}
			}
			return result;
		}




	}
}
