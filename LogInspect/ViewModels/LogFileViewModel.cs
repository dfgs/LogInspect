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

		/*public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(int), typeof(LogFileViewModel));
		public int Position
		{
			get { return (int)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}*/


		public double TotalWidth
		{
			get { return Columns.Sum(item => item.Width); }
		}


		public List<ColumnViewModel> Columns
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
		private Cache<int,Page> pages;

		private EventReader pageEventReader;


		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(EventViewModel), typeof(LogFileViewModel));
		public EventViewModel SelectedItem
		{
			get { return (EventViewModel)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}


		private EventIndexerModule eventIndexerModule;
		public IndexerModuleViewModel<EventIndexerModule,FileIndex> EventIndexer
		{
			get;
			private set;
		}
		private EventFiltererModule eventFiltererModule;
		public IndexerModuleViewModel<EventFiltererModule, FileIndex> EventFilterer
		{
			get;
			private set;
		}

		private SeverityIndexerModule severityIndexerModule;
		public SeverityIndexerViewModel Severities
		{
			get;
			private set;
		}

		public event EventHandler PagesCleared;

		public LogFileViewModel(ILogger Logger,string FileName,EventReader PageEventReader,EventReader IndexerEventReader,int PageSize,int PageCount,int IndexerLookupRetryDelay, int FiltererLookupRetryDelay) :base(Logger)
		{
			ColumnViewModel column;

			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);

			this.pageSize = PageSize;
			pages = new Cache<int, Page>(PageCount);

			this.pageEventReader = PageEventReader;

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

			eventIndexerModule = new EventIndexerModule(Logger, IndexerEventReader,IndexerLookupRetryDelay);
			Log(LogLevels.Information, "Starting EventIndexer");
			eventIndexerModule.Start();
			EventIndexer = new IndexerModuleViewModel<EventIndexerModule,FileIndex>(Logger, eventIndexerModule, 300);

			eventFiltererModule = new EventFiltererModule(Logger, eventIndexerModule,FiltererLookupRetryDelay);
			Log(LogLevels.Information, "Starting EventFilterer");
			eventFiltererModule.Start();
			EventFilterer = new IndexerModuleViewModel<EventFiltererModule, FileIndex>(Logger, eventFiltererModule, 300);

			severityIndexerModule = new SeverityIndexerModule(Logger, eventIndexerModule, FiltererLookupRetryDelay);
			Log(LogLevels.Information, "Starting SeverityIndexer");
			severityIndexerModule.Start();
			Severities = new SeverityIndexerViewModel(Logger, severityIndexerModule, 300);
			Severities.IsCheckedChanged += Severities_IsCheckedChanged;
		}


		public override void Dispose()
		{
			EventIndexer.Dispose();
			EventFilterer.Dispose();
			Severities.Dispose();

			Log(LogLevels.Information, "Stopping SeverityIndexer");
			severityIndexerModule.Stop();
			Log(LogLevels.Information, "Stopping EventFilterer");
			eventFiltererModule.Stop();
			Log(LogLevels.Information, "Stopping EventIndexer");
			eventIndexerModule.Stop();
			
			pages.Clear();
			pages = null;
		}

		private void Severities_IsCheckedChanged(object sender, EventArgs e)
		{
			pages.Clear();
			PagesCleared?.Invoke(this, EventArgs.Empty);
			eventFiltererModule.SetFilter(Severities.Where(item => !item.IsChecked).Select(item => item.Name).ToArray());
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
			FileIndex fileIndex;
			int eventIndex, t;
			
			eventIndex = Page.Index * pageSize + Page.LastFilledIndex+1;

			for (t = Page.LastFilledIndex+1; (t < pageSize) && (eventIndex < eventFiltererModule.IndexedEventsCount); t++, eventIndex++)
			{
				fileIndex = eventFiltererModule[eventIndex];
				pageEventReader.Seek(fileIndex.Position);

				try
				{
					ev = pageEventReader.Read();
				}
				catch (Exception ex)
				{
					Log(ex);
					return false;
				}
				Page[t] = new EventViewModel(Logger,pageEventReader.FormatHandler.SeverityMapping,  ev, eventIndex ,fileIndex.LineIndex);

				//if (pageEventReader.EndOfStream) break;
			}

			return true;
		}

		public IEnumerable<EventViewModel> GetEvents(int StartIndex, int Count)
		{
			Page page;
			EventViewModel ev;

			for (int t = StartIndex; t < StartIndex + Count; t++)
			{
				page = GetPage(t / pageSize);
				if (page == null) yield break;
				ev = page[t % pageSize];
				if (ev == null) yield break;
				yield return ev;
			}

		}
		public EventViewModel GetEvent(int Index)
		{
			Page page;

			page = GetPage(Index / pageSize);
			if (page == null) return null;
			return page[Index % pageSize];
		}






	}
}
