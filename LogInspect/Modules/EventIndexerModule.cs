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

	public class EventIndexerModule : ThreadModule
	{
		private Dictionary<int,long> dictionary;
		private LogReader logReader;

		public event EventIndexedHandler EventIndexed;

		public int Count
		{
			get { return dictionary.Count; }
		}

		public EventIndexerModule(ILogger Logger, LogReader LogReader) : base("EventIndexer",Logger)
		{
			this.logReader = LogReader;
			dictionary = new Dictionary<int, long>();
		}

		protected override void ThreadLoop()
		{
			Event ev ;
			int index;

			index = 0;
			while(State==ModuleStates.Started)
			{
				while ((State == ModuleStates.Started) && (!logReader.EndOfStream))
				{
					try
					{
						//pos = logReader.BaseStream.Position;
						ev = logReader.ReadEvent();
					}
					catch (Exception ex)
					{
						Log(ex);
						return;
					}
					lock (dictionary)
					{
						dictionary.Add(index, ev.Log.Position);
					}
					EventIndexed?.Invoke(ev, index);
					index++;
					Thread.Sleep(1000);
				}

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
