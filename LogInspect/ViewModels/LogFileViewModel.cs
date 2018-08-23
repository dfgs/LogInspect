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
using VirtualListBoxLib;

namespace LogInspect.ViewModels
{
	public class LogFileViewModel:ViewModel,IVirtualCollection //,IEnumerable<EventViewModel>//,INotifyCollectionChanged
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


		public static readonly DependencyProperty SelectedItemIndexProperty = DependencyProperty.Register("SelectedItemIndex", typeof(int), typeof(LogFileViewModel),new PropertyMetadata(SelectedItemIndexPropertyChanged));
		public int SelectedItemIndex
		{
			get { return (int)GetValue(SelectedItemIndexProperty); }
			set { SetValue(SelectedItemIndexProperty, value); }
		}


		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(EventViewModel), typeof(LogFileViewModel));
		public EventViewModel SelectedItem
		{
			get { return (EventViewModel)GetValue(SelectedItemProperty); }
			private set { SetValue(SelectedItemProperty, value); }
		}



		public ColumnsViewModel Columns
		{
			get;
			private set;
		}

		private EventIndexerModule eventIndexerModule;
		public IndexerModuleViewModel<EventIndexerModule,Event, FileIndex> EventIndexer
		{
			get;
			private set;
		}
		

		private SelectionFiltersIndexerModule selectionFiltersIndexerModule;
		public SeveritiesViewModel Severities
		{
			get;
			private set;
		}

		//public event EventHandler PagesCleared;

		public LogFileViewModel(ILogger Logger,string FileName,EventReader PageEventReader,EventReader IndexerEventReader,int PageSize,int PageCount,int IndexerLookupRetryDelay, int FiltererLookupRetryDelay) :base(Logger)
		{

			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);

			this.pageSize = PageSize;
			pages = new Cache<int, Page>(PageCount);

			this.pageEventReader = PageEventReader;

			eventIndexerModule = new EventIndexerModule(Logger, IndexerEventReader,IndexerLookupRetryDelay);
			EventIndexer = new IndexerModuleViewModel<EventIndexerModule,Event,FileIndex>(Logger, eventIndexerModule, 300);
			
			selectionFiltersIndexerModule = new SelectionFiltersIndexerModule(Logger, eventIndexerModule,pageEventReader.FormatHandler);
			Severities = new SeveritiesViewModel(Logger, PageEventReader.FormatHandler.SeverityColumn, selectionFiltersIndexerModule);

			Columns = new ColumnsViewModel(Logger, PageEventReader.FormatHandler,selectionFiltersIndexerModule);
			Columns.FilterChanged += Columns_FilterChanged;

			Log(LogLevels.Information, "Starting EventIndexer");
			eventIndexerModule.Start();
		}


		public override void Dispose()
		{

			Log(LogLevels.Information, "Stopping EventIndexer");
			eventIndexerModule.Stop();

			EventIndexer.Dispose();
			selectionFiltersIndexerModule.Dispose();

			pages.Clear();
			pages = null;
		}



		private static async void SelectedItemIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			await ((LogFileViewModel)d).OnSelectedItemIndexChanged();
		}
		private async Task OnSelectedItemIndexChanged()
		{
			if ((SelectedItemIndex < 0) || (SelectedItemIndex >= EventIndexer.IndexedEventsCount)) SelectedItem = null;
			else SelectedItem = await GetEventAsync(SelectedItemIndex);
		}




		private async Task<Page> GetPageAsync(int PageIndex)
		{
			Page page;

			if (!pages.TryGetValue(PageIndex,out page))
			{
				page = new Page(PageIndex, pageSize);
				if (!await LoadPageAsync(page)) return null;
				pages.Add(PageIndex, page);
			}
			else
			{
				if (!page.IsComplete) await LoadPageAsync(page);
			}
			return page;
		}
		private async Task<bool> LoadPageAsync(Page Page)
		{
			Event ev;
			FileIndex fileIndex;
			int eventIndex, t;
			EventViewModel vm;

			eventIndex = Page.Index * pageSize + Page.LastFilledIndex + 1;

			for (t = Page.LastFilledIndex + 1; (t < pageSize) && (eventIndex < EventIndexer.IndexedEventsCount); t++, eventIndex++)
			{
				fileIndex = eventIndexerModule[eventIndex];
				pageEventReader.Seek(fileIndex.Position);

				try
				{
					ev = await pageEventReader.ReadAsync();
				}
				catch (Exception ex)
				{
					Log(ex);
					return false;
				}

				vm = new EventViewModel(Logger, Columns, pageEventReader.FormatHandler.ColoringRules, ev, eventIndex, fileIndex.LineIndex);
				vm.IsBookMarked = fileIndex.IsBookMarked;
				Page[t] = vm;
				//if (pageEventReader.EndOfStream) break;
			}
			return true;
		}

		/*public async Task<IEnumerable<EventViewModel>> GetEventsAsync(int StartIndex, int Count)
		{
			Page page;
			EventViewModel ev;
			
			for (int t = StartIndex; t < StartIndex + Count; t++)
			{
				page = await GetPageAsync(t / pageSize);
				if (page == null) yield break;
				ev = page[t % pageSize];
				if (ev == null) yield break;
				yield return ev;
			}

		}*/

		object IVirtualCollection.GetItem(int Index)
		{
			EventViewModel result;

			result = Task.Run<EventViewModel>( ()=>GetEventAsync(Index) ).Result;
			return result;
		}

		public async Task<EventViewModel> GetEventAsync(int Index)
		{
			Page page;

			page = await GetPageAsync(Index / pageSize);
			if (page == null) return null;
			return page[Index % pageSize];
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
			SelectedItemIndex = -1;
			filters= Columns.Where(item => item.Filter != null).Select(item => item.Filter);
			eventIndexerModule.SetFilters(filters  );
			EndWork();
		}

		#endregion

		#region generic search methods
		public async Task<int> FindPreviousAsync(int Index, Func<EventViewModel, bool> Predicate)
		{
			EventViewModel ev;
			while (Index > 0)
			{
				Index--;
				ev = await this.GetEventAsync(Index);
				if (Predicate(ev)) return Index;
			}
			return -1;
		}
		public async Task<int> FindNextAsync(int Index, Func<EventViewModel, bool> Predicate)
		{
			EventViewModel ev;

			while (Index < eventIndexerModule.IndexedEventsCount - 1)
			{
				Index++;
				ev = await this.GetEventAsync(Index);
				if (Predicate(ev)) return Index;
			}
			return -1;

		}
		#endregion

		#region find severities
		public async Task<int> FindPreviousSeverityAsync(object Severity)
		{
			int index;

			BeginWork();
			index =  await  FindPreviousAsync(SelectedItemIndex, (item) => Severity.Equals( item.GetPropertyValue(pageEventReader.FormatHandler.SeverityColumn))); 
			EndWork();
			return index;
		}
		public async Task<int> FindNextSeverityAsync(object Severity)
		{
			int index ;

			BeginWork();
			index = await  FindNextAsync(SelectedItemIndex, (item) =>  Severity.Equals(item.GetPropertyValue(pageEventReader.FormatHandler.SeverityColumn)));
			EndWork();
			return index;
		}
		#endregion

		#region bookmark
		public async Task ToogleBookMarkAsync()
		{
			FileIndex fileIndex;

			if (SelectedItem == null) return;
			BeginWork();
			SelectedItem.IsBookMarked = !SelectedItem.IsBookMarked;
			fileIndex = eventIndexerModule[SelectedItemIndex];
			fileIndex.IsBookMarked = SelectedItem.IsBookMarked;
			await Task.Yield();
			EndWork();
		}

		public async Task<int> FindPreviousBookMarkAsync()
		{
			int index;

			BeginWork();
			index = await FindPreviousAsync(SelectedItemIndex, (item) => item.IsBookMarked );
			EndWork();
			return index;
		}
		public async Task<int> FindNextBookMarkAsync()
		{
			int index;

			BeginWork();
			index = await FindNextAsync(SelectedItemIndex, (item) => item.IsBookMarked );
			EndWork();
			return index;
		}

		#endregion







	}
}
