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
using System.Threading;
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


		public double TotalWidth
		{
			get { return Columns.Sum(item => item.Width); }
		}


		public List<ColumnViewModel> Columns
		{
			get;
			private set;
		}

		public ObservableCollection<SeverityViewModel> Severities
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
		private EventFiltererModule eventFiltererModule;
		private EventReader pageEventReader;


		private Timer timer;

		public LogFileViewModel(ILogger Logger,string FileName, EventReader PageEventReader,EventReader IndexerEventReader,int PageSize,int PageCount ):base(Logger)
		{
			ColumnViewModel column;

			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);

			this.pageSize = PageSize;
			pages = new Cache<int, Page>(PageCount);

			this.pageEventReader = PageEventReader;

			Severities = new ObservableCollection<SeverityViewModel>();

			#region create columns
			Columns = new List<ColumnViewModel>();

			column = new LineColumnViewModel(Logger, "#") { Width = 50};
			column.WidthChanged += Column_WidthChanged;
			Columns.Add(column);
			if (pageEventReader.FormatHandler.Rules.Count > 0)
			{
				foreach(Token property in pageEventReader.FormatHandler.Rules[0].Tokens.Where(item=>item.Name!=null))
				{
					column=new PropertyColumnViewModel(Logger, property.Name,property.Alignment) {Width=property.Width };
					column.WidthChanged += Column_WidthChanged;
					Columns.Add(column);
				}
			}
			#endregion

			eventIndexerModule = new EventIndexerModule(Logger, IndexerEventReader);
			eventIndexerModule.SeverityAdded += EventIndexerModule_SeverityAdded;
			Log(LogLevels.Information, "Starting EventIndexer");
			eventIndexerModule.Start();


			eventFiltererModule = new EventFiltererModule(Logger, eventIndexerModule);
			Log(LogLevels.Information, "Starting EventFilterer");
			eventFiltererModule.Start();

			timer = new Timer(timerCallBack, null, 0, 500);
		
		}


		public override void Dispose()
		{
			timer.Dispose();

			if (eventIndexerModule != null)
			{
				Log(LogLevels.Information, "Stopping EventFilterer");
				eventFiltererModule.Stop();
				Log(LogLevels.Information, "Stopping EventIndexer");
				eventIndexerModule.Stop();
			}
			pages.Clear();
			pages = null;
		}

		private void Column_WidthChanged(object sender, EventArgs e)
		{
			OnPropertyChanged("TotalWidth");
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
			int eventIndex,lineIndex;
			FileIndex fileIndex;

			eventIndex = Page.Index * pageSize;

			if (!eventIndexerModule.GetFileIndex(eventIndex,out fileIndex))
			{
				Log(LogLevels.Error, $"Failed to seek to position {eventIndex}");
				return false;
			}
			pageEventReader.Seek(fileIndex.Position);
			lineIndex = fileIndex.LineIndex;
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
				Page[t] = new EventViewModel(Logger, ev, t+eventIndex ,lineIndex);
				lineIndex += pageEventReader.GetReadLines();
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

		private void timerCallBack(object state)
		{
			Count = eventFiltererModule.Count;
			OnPropertyChanged("Count");
		}

		
		private void EventIndexerModule_SeverityAdded(object sender, SeverityAddedEventArgs e)
		{
			Dispatcher.Invoke(() =>
			{
				Severities.Add(new SeverityViewModel(Logger, e.Severity));
			});
		}



	}
}
