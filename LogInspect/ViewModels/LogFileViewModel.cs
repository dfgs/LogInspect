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

namespace LogInspect.ViewModels
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

		public ColumnViewModel[] Columns
		{
			get;
			private set;
		}
		
		private AppViewModel appViewModel;
		private EventIndexerModule eventIndexerModule;
		private EventReader eventReader;

		private bool isDisposing;   // prevent hangs when closing application

		public LogFileViewModel(ILogger Logger, AppViewModel AppViewModel, string FileName,int BufferSize):base(Logger)
		{
			EventReader indexerReader;
			Rule rule;

			isDisposing = false;
			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);
			this.appViewModel = AppViewModel;

			Count = 0;
			this.eventReader = AppViewModel.CreateEventReader(FileName,BufferSize);

			if (this.eventReader.FormatHandler.Rules.Count == 0) Columns = new ColumnViewModel[0];
			else Columns=this.eventReader.FormatHandler.Rules.First().GetColumns().Select(item=> new ColumnViewModel(Logger,item,150)).ToArray();

			indexerReader = AppViewModel.CreateEventReader(FileName,BufferSize);
			if (indexerReader != null)
			{
				eventIndexerModule = new EventIndexerModule(Logger, indexerReader);
				eventIndexerModule.EventIndexed += EventIndexerModule_EventIndexed;
				Log(LogLevels.Debug, "Starting EventIndexer");
				eventIndexerModule.Start();
			}
			else eventIndexerModule = null;

			//LoadItems(0);
		}


		public override void Dispose()
		{
			isDisposing = true;
			if (eventIndexerModule != null)
			{
				Log(LogLevels.Debug, "Stopping EventIndexer");
				eventIndexerModule.Stop();
			}
			
		}

	

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
				try
				{
					ev = eventReader.Read();
				}
				catch(Exception ex)
				{
					Log(ex);
					yield break;
				}
				yield return ev;
			}
		}


		private void EventIndexerModule_EventIndexed(Event Ev, int Index)
		{
			if (!isDisposing)	// prevent hangs when closing application
			{
				Dispatcher.Invoke(() =>
				{
					Count = eventIndexerModule.Count;
				});
			}
		}


	

	}
}
