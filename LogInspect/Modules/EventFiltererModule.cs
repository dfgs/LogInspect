using LogInspect.Models;
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
	public class EventFiltererModule : ThreadModule
	{
		private Dictionary<int, FileIndex> dictionary;

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

		private int index;
		private EventIndexerModule indexerModule;

		public EventFiltererModule( ILogger Logger,EventIndexerModule IndexerModule) : base("EventFilterer", Logger)
		{
			index = 0;
			dictionary = new Dictionary<int, FileIndex>();
			this.indexerModule = IndexerModule;
		}

		protected override void ThreadLoop()
		{
			FileIndex fileIndex;

			while(State==ModuleStates.Started)
			{
				while(index<indexerModule.Count)
				{
					if (indexerModule.GetFileIndex(index,out fileIndex))
					{
						lock(dictionary)
						{
							dictionary.Add(index, fileIndex);
						}
						index++;
					}
					else
					{
						Log(LogLevels.Error, $"Failed to get file index {index}");
						WaitHandles(5000, QuitEvent);
					}
				}
				if (State==ModuleStates.Started) WaitHandles(1000, QuitEvent);
			}
		}


	}
}
