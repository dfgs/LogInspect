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
		//TODO download value tuple
		private Dictionary<int,Tuple<long, int>> dictionary;
		private EventReader eventReader;

		public event EventHandler Updated;

		public int Count
		{
			get { return dictionary.Count; }
		}

		public EventIndexerModule(ILogger Logger, EventReader EventReader) : base("EventIndexer",Logger)
		{
			this.eventReader = EventReader;
			dictionary = new Dictionary<int, Tuple<long,int> >();
		}

		protected override void ThreadLoop()
		{
			Event ev ;
			int index;
			int lineIndex;
			long previousTicks,newTicks;

			previousTicks=Environment.TickCount;
			index = 0;lineIndex = 0;
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
						dictionary.Add(index, new Tuple<long, int>( ev.Position, lineIndex));
					}
					newTicks = Environment.TickCount;
					if (newTicks - previousTicks >= 500)	// prevent UI hangs because of too many updates
					{
						Updated?.Invoke(this, EventArgs.Empty);
						previousTicks = newTicks;
					}
					index++;lineIndex += ev.Log.Lines.Length;
					Thread.Sleep(1); // limit cpu usage
				}
				if (State == ModuleStates.Started)
				{
					Updated?.Invoke(this,EventArgs.Empty);
					WaitHandles(1000, QuitEvent);
				}
			}


		}

		public long GetStreamPos(int EventIndex)
		{
			long result;

			if (EventIndex == 0) return 0; // we return valid result even if thread is not yet started
			lock(dictionary)
			{
				try
				{
					result = dictionary[EventIndex].Item1;
				}
				catch(Exception ex)
				{
					Log(ex);
					return -1;
				}
			}
			return result;
		}
		public int GetLineIndex(int EventIndex)
		{
			int result;

			if (EventIndex == 0) return 0; // we return valid result even if thread is not yet started
			lock (dictionary)
			{
				try
				{
					result = dictionary[EventIndex].Item2;
				}
				catch (Exception ex)
				{
					Log(ex);
					return -1;
				}
			}
			return result;
		}



	}
}
