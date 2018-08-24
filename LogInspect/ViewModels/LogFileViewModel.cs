using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
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
	public class LogFileViewModel:ViewModel
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


		private int pageSize;
		private Cache<int,Page> pages;

		private EventReader pageEventReader;


		public static readonly DependencyProperty IsWorkingProperty = DependencyProperty.Register("IsWorking", typeof(bool), typeof(LogFileViewModel),new PropertyMetadata(false));
		public bool IsWorking
		{
			get { return (bool)GetValue(IsWorkingProperty); }
			set { SetValue(IsWorkingProperty, value); }
		}


		public ColumnsViewModel Columns
		{
			get;
			private set;
		}

		private EventIndexerModule eventIndexerModule;
		public IndexerModuleViewModel<EventIndexerModule> EventIndexer
		{
			get;
			private set;
		}
		

		private FilterChoicesViewModel filterChoices;
		public SeveritiesViewModel Severities
		{
			get;
			private set;
		}

		public EventsViewModel Events
		{
			get;
			private set;
		}

		public LogFileViewModel(ILogger Logger,string FileName,EventReader PageEventReader,EventReader IndexerEventReader,int PageSize,int PageCount, int IndexerLookupRetryDelay, int FiltererLookupRetryDelay) :base(Logger)
		{

			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);

			this.pageSize = PageSize;
			pages = new Cache<int, Page>(PageCount);

			this.pageEventReader = PageEventReader;

			eventIndexerModule = new EventIndexerModule(Logger, IndexerEventReader,IndexerLookupRetryDelay);
			EventIndexer = new IndexerModuleViewModel<EventIndexerModule>(Logger, eventIndexerModule, 300);

			filterChoices = new FilterChoicesViewModel(Logger, eventIndexerModule, pageEventReader.FormatHandler.Columns);
			Severities = new SeveritiesViewModel(Logger, PageEventReader.FormatHandler.SeverityColumn, filterChoices);

			Columns = new ColumnsViewModel(Logger, PageEventReader.FormatHandler,filterChoices);
			Columns.FilterChanged += Columns_FilterChanged;

			Events = new EventsViewModel(Logger, eventIndexerModule,Columns,pageEventReader.FormatHandler.ColoringRules);

			Log(LogLevels.Information, "Starting EventIndexer");
			eventIndexerModule.Start();
		}


		public override void Dispose()
		{

			Log(LogLevels.Information, "Stopping EventIndexer");
			eventIndexerModule.Stop();

			EventIndexer.Dispose();
			filterChoices.Dispose();

			pages.Clear();
			pages = null;
		}

		

		private void BeginWork()
		{
			IsWorking = true;
		}
		private void EndWork()
		{
			IsWorking = false;
		}

		#region filter events
		private void Columns_FilterChanged(object sender, EventArgs e)
		{
			Refresh();
		}
		public void Refresh()
		{
			IEnumerable<Filter> filters;

			BeginWork();
			pages.Clear();
			//SelectedItemIndex = -1;
			filters= Columns.Where(item => item.Filter != null).Select(item => item.Filter);
			eventIndexerModule.SetFilters(filters  );
			EndWork();
		}

		#endregion

		#region generic search methods
		public async Task<int> FindPreviousAsync(int Index, Func<EventViewModel, bool> Predicate)
		{
			EventViewModel ev;
			return await Task.Run<int>(() =>
			{
				while (Index > 0)
				{
					Index--;
					ev = Events[Index];
					if (Dispatcher.Invoke<bool>(()=> Predicate(ev))) return Index;
				}
				return -1;
			});
		}
		public async Task<int> FindNextAsync(int Index, Func<EventViewModel, bool> Predicate)
		{
			EventViewModel ev;

			return await Task.Run<int>(() =>
			{
				while (Index < eventIndexerModule.IndexedEventsCount - 1)
				{
					Index++;
					ev = Events[Index];
					if (Dispatcher.Invoke<bool>(() => Predicate(ev))) return Index;
				}
				return -1;
			});

		}
		#endregion

		#region find severities
		public async Task<int> FindPreviousSeverityAsync(object Severity,int StartIndex)
		{
			int index;

			BeginWork();
			index =  await  FindPreviousAsync(StartIndex, (item) => Severity.Equals( item.GetPropertyValue(pageEventReader.FormatHandler.SeverityColumn))); 
			EndWork();
			return index;
		}
		public async Task<int> FindNextSeverityAsync(object Severity, int StartIndex)
		{
			int index ;

			BeginWork();
			index = await  FindNextAsync(StartIndex, (item) =>  Severity.Equals(item.GetPropertyValue(pageEventReader.FormatHandler.SeverityColumn)));
			EndWork();
			return index;
		}
		#endregion

		#region bookmark
		public async Task ToogleBookMarkAsync()
		{
			if (Events.SelectedItem == null) return;
			BeginWork();
			Events.SelectedItem.IsBookMarked = !Events.SelectedItem.IsBookMarked;
			await Task.Yield();
			EndWork();
		}

		public async Task<int> FindPreviousBookMarkAsync(int StartIndex)
		{
			int index;

			BeginWork();
			index = await FindPreviousAsync(StartIndex, (item) => item.IsBookMarked );
			EndWork();
			return index;
		}
		public async Task<int> FindNextBookMarkAsync(int StartIndex)
		{
			int index;

			BeginWork();
			index = await FindNextAsync(StartIndex, (item) => item.IsBookMarked );
			EndWork();
			return index;
		}

		#endregion







	}
}
