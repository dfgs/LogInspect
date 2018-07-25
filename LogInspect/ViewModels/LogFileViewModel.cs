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
	public class LogFileViewModel:VirtualCollection<Event>
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

		

		public ColumnViewModel[] Columns
		{
			get;
			private set;
		}
		
		private AppViewModel appViewModel;
		private EventIndexerModule eventIndexerModule;
		private EventReader eventReader;

		private bool isDisposing;   // prevent hangs when closing application

		public LogFileViewModel(ILogger Logger, AppViewModel AppViewModel, string FileName,int BufferSize):base(Logger,3,100)
		{
			EventReader indexerReader;

			isDisposing = false;
			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);
			this.appViewModel = AppViewModel;

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


		protected override IEnumerable<Event> OnLoadPage(int PageIndex,int PageSize)
		{
			long pos;
			Event ev;
			int startIndex;

			startIndex = PageIndex * PageSize;

			pos = eventIndexerModule?.GetStreamPos(startIndex) ?? -1;
			if (pos == -1)
			{
				Log(LogLevels.Error, $"Failed to seek to position {startIndex}");
				yield break;
			}
			eventReader.Seek(pos);
			for (int t = 0; t < PageSize; t++)
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
					SetCount(eventIndexerModule.Count);
					OnPropertyChanged("Count");
				});
			}
		}

		
	}
}
