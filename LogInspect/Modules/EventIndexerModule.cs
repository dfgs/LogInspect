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
		private List<string> severities;

		public event SeverityAddedEventHandler SeverityAdded;

		public int Count
		{
			get
			{
				lock (dictionary)
				{
					return dictionary.Count;
				}
			}
		}

		public EventIndexerModule(ILogger Logger, EventReader EventReader) : base("EventIndexer",Logger)
		{
			this.eventReader = EventReader;
			dictionary = new Dictionary<int, FileIndex >();
			severities = new List<string>();
		}

		protected override void ThreadLoop()
		{
			Event ev ;
			int eventIndex;
			int lineIndex;
			long pos;
			string severity;

			eventIndex = 0;lineIndex = 0;
			while(State==ModuleStates.Started)
			{
				while ((State == ModuleStates.Started) && (!eventReader.EndOfStream))
				{
					try
					{
						pos = eventReader.Position;
						ev = eventReader.Read();
					}
					catch (Exception ex)
					{
						Log(ex);
						return;
					}
					severity = ev.GetValue("Severity")?.ToString();

					lock (dictionary)
					{
						dictionary.Add(eventIndex, new FileIndex(pos,lineIndex, eventIndex,severity));
					}
					lock(severities)
					{
						if (!severities.Contains(severity))
						{
							severities.Add(severity);
							SeverityAdded?.Invoke(this, new SeverityAddedEventArgs(severity));
						}
					}
					eventIndex++;lineIndex += eventReader.GetReadLines();
					Thread.Sleep(1); // limit cpu usage
				}
				if (State == ModuleStates.Started) WaitHandles(1000, QuitEvent);
			}


		}

		public bool GetFileIndex(int EventIndex,out FileIndex FileIndex)
		{
			/*if (EventIndex == 0)
			{
				// we return valid result even if thread is not yet started
				FileIndex = new FileIndex(0, 0, 0);
				return true;
			}*/
			lock(dictionary)
			{
				return dictionary.TryGetValue(EventIndex, out FileIndex);
			}
		}
		



	}
}
