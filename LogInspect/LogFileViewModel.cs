using LogInspect.Models;
using LogInspect.Modules;
using LogInspectLib;
using LogInspectLib.Readers;
using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect
{
	public class LogFileViewModel:ViewModel,IVirtualCollection
	{

		public string FileName
		{
			get;
			private set;
		}
		public string Name
		{
			get;
			private set;
		}

		private int count;
		public int Count
		{
			get { return count; }
			private set { count = value; OnPropertyChanged(); }
		}

		
		private AppViewModel appViewModel;
		private EventIndexerModule eventIndexerModule;
		private EventReader eventReader;



		public LogFileViewModel(ILogger Logger, AppViewModel AppViewModel, string FileName,int BufferSize):base(Logger)
		{
			EventReader eventReader;
			
			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);
			this.appViewModel = AppViewModel;

			//Count = 0;
			Count = 0;
			//pageSize = 10;
			this.eventReader = AppViewModel.CreateEventReader(FileName,BufferSize);

			eventReader = AppViewModel.CreateEventReader(FileName,BufferSize);
			if (eventReader != null)
			{
				eventIndexerModule = new EventIndexerModule(Logger, eventReader);
				eventIndexerModule.EventIndexed += EventIndexerModule_EventIndexed;
				Log(LogLevels.Debug, "Starting EventIndexer");
				eventIndexerModule.Start();
			}
			else eventIndexerModule = null;

			//LoadItems(0);
		}


		public override void Dispose()
		{
			if (eventIndexerModule != null)
			{
				Log(LogLevels.Debug, "Stopping EventIndexer");
				eventIndexerModule.Stop();
			}
			
		}
		/*private void LoadPage(int Page)
		{
			long pos;
			Event ev;

			pos = eventIndexerModule.GetStreamPos(Page * pageSize);
			if (pos==-1)
			{
				items = null;
				return;
			}
			logReader.BaseStream.Seek(pos, SeekOrigin.Begin);
			items = new Event[pageSize];
			for(int t=0;t<pageSize;t++)
			{
				ev = logReader.ReadEvent();
				items[t] = ev;
			}
			currentPage = Page;
		}*/

		/*private void LoadItems(int Position)
		{
			long pos;
			Event ev;

			pos = eventIndexerModule?.GetStreamPos(Position)??-1;
			if (pos==-1)
			{
				Log(LogLevels.Error, $"Failed to seek to position {Position}");
				items = null;
				return;
			}
			logReader.BaseStream.Seek(pos, SeekOrigin.Begin);
			items = new Event[pageSize];
			for(int t=0;t<pageSize;t++)
			{
				ev = logReader.ReadEvent();
				items[t] = ev;
			}
			this.position=Position;
		}*/
		public IEnumerable<Event> GetEvents(int StartIndex, int Count)
		{
			long pos;
			Event ev;

			pos = eventIndexerModule?.GetStreamPos(StartIndex) ?? -1;
			if (pos == -1)
			{
				Log(LogLevels.Error, $"Failed to seek to position {StartIndex}");
				yield break;
			}
			eventReader.Seek(pos);
			for (int t = 0; t < Count; t++)
			{
				ev = eventReader.Read();
				if (ev == null) yield break;
				yield return ev;
			}
		}


		private void EventIndexerModule_EventIndexed(Event Ev, int Index)
		{
			Dispatcher.Invoke(() => 
			{
				Count++;
			});
		}


		/*protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
		}
		public IEnumerator<Event> GetEnumerator()
		{
			if (items == null) yield break;
			foreach (Event item in items) yield return item;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}//*/
	}
}
