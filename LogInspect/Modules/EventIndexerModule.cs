using LogInspect.Models;
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
		private Dictionary<int,FileIndex> dictionary;
		private EventReader eventReader;

		public event EventHandler Updated;

		public int Count
		{
			get { return dictionary.Count; }
		}

		public EventIndexerModule(ILogger Logger, EventReader EventReader) : base("EventIndexer",Logger)
		{
			this.eventReader = EventReader;
			dictionary = new Dictionary<int, FileIndex >();
		}

		protected override void ThreadLoop()
		{
			Event ev ;
			int eventIndex;
			int lineIndex;
			long previousTicks,newTicks;

			previousTicks=Environment.TickCount;
			eventIndex = 0;lineIndex = 0;
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
						dictionary.Add(eventIndex, new FileIndex(eventReader.Position,lineIndex, eventIndex));
					}
					newTicks = Environment.TickCount;
					if (newTicks - previousTicks >= 500)	// prevent UI hangs because of too many updates
					{
						Updated?.Invoke(this, EventArgs.Empty);
						previousTicks = newTicks;
					}
					eventIndex++;lineIndex += ev.Log.Lines.Length;
					Thread.Sleep(1); // limit cpu usage
				}
				if (State == ModuleStates.Started)
				{
					Updated?.Invoke(this,EventArgs.Empty);
					WaitHandles(1000, QuitEvent);
				}
			}


		}

		public bool GetFileIndex(int EventIndex,out FileIndex FileIndex)
		{
			if (EventIndex == 0)
			{
				// we return valid result even if thread is not yet started
				FileIndex = new FileIndex(0, 0, 0);
				return true;
			}
			lock(dictionary)
			{
				return dictionary.TryGetValue(EventIndex, out FileIndex);
			}
		}
		



	}
}
