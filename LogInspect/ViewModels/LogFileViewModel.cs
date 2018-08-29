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




		public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(Statuses), typeof(LogFileViewModel),new PropertyMetadata(Statuses.Idle));
		public Statuses Status
		{
			get { return (Statuses)GetValue(StatusProperty); }
			private set { SetValue(StatusProperty, value); }
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
		private EventIndexerBufferModule indexerBufferModule;

		private FilterItemSourcesViewModel filterItemSourcesViewModel;
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

		public MarkersViewModel Markers
		{
			get;
			private set;
		}

		private EventReader EventReader;

		public LogFileViewModel(ILogger Logger,string FileName,EventReader EventReader, int IndexerLookupRetryDelay, int IndexerBufferLookupRetryDelay,int IndexerProgressRefreshDelay) :base(Logger)
		{

			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);
			this.EventReader = EventReader;


			eventIndexerModule = new EventIndexerModule(Logger, EventReader,IndexerLookupRetryDelay);
			EventIndexer = new IndexerModuleViewModel<EventIndexerModule>(Logger, eventIndexerModule, IndexerProgressRefreshDelay);

			filterItemSourcesViewModel = new FilterItemSourcesViewModel(Logger, eventIndexerModule, EventReader.FormatHandler.Columns);
			Severities = new SeveritiesViewModel(Logger, EventReader.FormatHandler.SeverityColumn, filterItemSourcesViewModel);

			Columns = new ColumnsViewModel(Logger, EventReader.FormatHandler,filterItemSourcesViewModel);
			//Columns.FilterChanged += Columns_FilterChanged;

			indexerBufferModule = new EventIndexerBufferModule(Logger, eventIndexerModule, IndexerLookupRetryDelay);

			Events = new EventsViewModel(Logger, indexerBufferModule,Columns,EventReader.FormatHandler.EventColoringRules);
			Markers = new MarkersViewModel(Logger, indexerBufferModule, EventReader.FormatHandler.EventColoringRules, EventReader.FormatHandler.SeverityColumn);

			Log(LogLevels.Information, "Starting EventIndexerBufferModule");
			indexerBufferModule.Start();
			Log(LogLevels.Information, "Starting EventIndexerModule");
			eventIndexerModule.Start();
		}


		public override void Dispose()
		{

			Log(LogLevels.Information, "Stopping EventIndexerBufferModule");
			indexerBufferModule.Stop();
			Log(LogLevels.Information, "Stopping EventIndexerModule");
			eventIndexerModule.Stop();

			EventIndexer.Dispose();
			filterItemSourcesViewModel.Dispose();

		}

		

		

		#region filter events
		/*private void Columns_FilterChanged(object sender, EventArgs e)
		{
			Refresh();
		}*/
		public void Refresh()
		{
			Filter[] filters;

			//SelectedItemIndex = -1;
			filters= Columns.Where(item => item.Filter != null).Select(item => item.Filter).ToArray();
			eventIndexerModule.SetFilters(filters  );
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
					//Thread.Sleep(1000);
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
					//Thread.Sleep(1000);
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

			Status = Statuses.Searching;
			index =  await  FindPreviousAsync(StartIndex, (item) => Severity.Equals( item.GetPropertyValue(EventReader.FormatHandler.SeverityColumn)));
			Status = Statuses.Idle;
			return index;
		}
		public async Task<int> FindNextSeverityAsync(object Severity, int StartIndex)
		{
			int index ;

			Status = Statuses.Searching;
			index = await  FindNextAsync(StartIndex, (item) =>  Severity.Equals(item.GetPropertyValue(EventReader.FormatHandler.SeverityColumn)));
			Status = Statuses.Idle;
			return index;
		}
		#endregion

		#region bookmark
		public void ToogleBookMark()
		{
			if (Events.SelectedItem == null) return;
			Events.SelectedItem.IsBookMarked = !Events.SelectedItem.IsBookMarked;
		}

		public async Task<int> FindPreviousBookMarkAsync(int StartIndex)
		{
			int index;

			Status = Statuses.Searching;
			index = await FindPreviousAsync(StartIndex, (item) => item.IsBookMarked );
			Status = Statuses.Idle;
			return index;
		}
		public async Task<int> FindNextBookMarkAsync(int StartIndex)
		{
			int index;

			Status = Statuses.Searching;
			index = await FindNextAsync(StartIndex, (item) => item.IsBookMarked );
			Status = Statuses.Idle;
			return index;
		}

		#endregion







	}
}
