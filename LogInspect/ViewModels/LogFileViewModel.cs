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
using System.Windows.Data;

namespace LogInspect.ViewModels
{
	public class LogFileViewModel:ViewModel//,IEnumerable<EventViewModel>//,INotifyCollectionChanged
	{

		public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(int), typeof(LogFileViewModel));
		public int Position
		{
			get { return (int)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}

		public List<ColumnViewModel> Columns
		{
			get;
			private set;
		}


		public int Count
		{
			get;
			private set;
		}

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


		private int pageSize;
		//private int pageCount;
		private Cache<int,Page> pages;

		private EventIndexerModule eventIndexerModule;
		private EventReader pageEventReader;

		private bool isDisposing;   // prevent hangs when closing application


		public LogFileViewModel(ILogger Logger,string FileName, EventReader PageEventReader,EventReader IndexerEventReader,int PageSize,int PageCount ):base(Logger)
		{

			isDisposing = false;
			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);

			this.pageSize = PageSize;
			//this.pageCount = PageCount;
			pages = new Cache<int, Page>(PageCount);

			this.pageEventReader = PageEventReader;

			Columns = new List<ColumnViewModel>();
			
			if (pageEventReader.FormatHandler.Rules.Count > 0)
			{
				foreach(string property in pageEventReader.FormatHandler.Rules[0].GetColumns())
				{
					Columns.Add(new ColumnViewModel(Logger, property));
				}
			}

			eventIndexerModule = new EventIndexerModule(Logger, IndexerEventReader);
			eventIndexerModule.Updated += EventIndexerModule_Updated;


			Log(LogLevels.Debug, "Starting EventIndexer");
			eventIndexerModule.Start();
			
		}


		public override void Dispose()
		{
			isDisposing = true;
			if (eventIndexerModule != null)
			{
				Log(LogLevels.Debug, "Stopping EventIndexer");
				eventIndexerModule.Stop();
			}
			pages.Clear();
			pages = null;
		}

		private Page GetPage(int PageIndex)
		{
			Page page;

			if (!pages.TryGetValue(PageIndex,out page))
			{
				page = new Page(PageIndex, pageSize);
				if (!LoadPage(page)) return null;
				pages.Add(PageIndex, page);
			}
			else
			{
				if (!page.IsComplete) LoadPage(page);
			}
			return page;
		}
		private bool LoadPage(Page Page)
		{
			Event ev;
			int eventIndex;
			FileIndex fileIndex;

			eventIndex = Page.Index * pageSize;

			if (!eventIndexerModule.GetFileIndex(eventIndex,out fileIndex))
			{
				Log(LogLevels.Error, $"Failed to seek to position {eventIndex}");
				return false;
			}
			pageEventReader.Seek(fileIndex.Position);

			for (int t = 0; (t < pageSize) && (!pageEventReader.EndOfStream); t++)
			{
				try
				{
					ev = pageEventReader.Read();
				}
				catch (Exception ex)
				{
					Log(ex);
					return false;
				}
				Page[t] = new EventViewModel(Logger, ev, fileIndex.EventIndex, fileIndex.LineIndex);
			}
			Page.IsComplete = true;
			return true;
		}

		public IEnumerable<EventViewModel> GetEvents(int StartIndex, int Count)
		{
			Page page;
			EventViewModel ev;
			
			for(int t=StartIndex;t<StartIndex+Count;t++)
			{
				page = GetPage(t/pageSize );
				if (page == null) yield break;
				ev= page[t % pageSize];
				if (ev == null) yield break;
				yield return ev;
			}

		}

		private void EventIndexerModule_Updated(object sender,EventArgs e)
		{
			if (!isDisposing)	// prevent hangs when closing application
			{
				Dispatcher.Invoke(() =>
				{
					Count=eventIndexerModule.Count;
					OnPropertyChanged("Count");
				});
			}
		}



	}
}
