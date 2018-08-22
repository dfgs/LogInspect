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



		private static void SelectedItemIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((LogFileViewModel)d).OnSelectedItemIndexChanged();
		}
		protected virtual void OnSelectedItemIndexChanged()
		{
			//Dispatcher.Invoke(() =>
			//{
				if ((SelectedItemIndex < 0) || (SelectedItemIndex >= EventIndexer.IndexedEventsCount)) SelectedItem = null;
				else SelectedItem = GetEvent(SelectedItemIndex);
			//});
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
			EventViewModel vm;

			eventIndex = Page.Index * pageSize + Page.LastFilledIndex+1;
			
			for (t = Page.LastFilledIndex+1; (t < pageSize) && (eventIndex < EventIndexer.IndexedEventsCount); t++, eventIndex++)
			{
				fileIndex = eventIndexerModule[eventIndex];
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

				vm= new EventViewModel(Logger, Columns, pageEventReader.FormatHandler.ColoringRules, ev, eventIndex, fileIndex.LineIndex);
				vm.IsBookMarked = fileIndex.IsBookMarked;
				Page[t] = vm;
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
		object IVirtualCollection.GetItem(int Index)
		{
			return GetEvent(Index);
		}

		public EventViewModel GetEvent(int Index)
		{
			Page page;

			page = GetPage(Index / pageSize);
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

		#region find severities
		public async Task FindPreviousAsync(object Severity)
		{
			int index=0;
			bool result;

			index = SelectedItemIndex;  // cannot access SelectedItemIndex inside Task.Run (Thread sync)
			BeginWork();
			result = await Task.FromResult(false);// await Task.Run<bool>(()=> { return eventIndexerModule.FindPrevious(ref index, (item) => item.Severity == Severity); } );
			
			if (result) SelectedItemIndex = index;
			EndWork();
		}
		public async Task FindNextAsync(object Severity)
		{
			int index = 0;
			bool result;

			index = SelectedItemIndex;	// cannot access SelectedItemIndex inside Task.Run (Thread sync)
			BeginWork();
			result = await Task.FromResult(false);//await Task.Run<bool>(() => { return eventIndexerModule.FindNext(ref index, (item) => item.Severity == Severity); });
			if (result) SelectedItemIndex = index;
			EndWork();
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
		/*public bool HasBookMarks()
		{
			return bookMarks.Count > 0;
		}*/

		public async Task FindPreviousBookMarkAsync()
		{
			int index = 0;
			bool result;

			index = SelectedItemIndex;  // cannot access SelectedItemIndex inside Task.Run (Thread sync)
			BeginWork();
			result = await Task.Run<bool>(() => { return eventIndexerModule.FindPrevious(ref index, (item) => item.IsBookMarked); });
			if (result) SelectedItemIndex = index;
			EndWork();
		}
		public async Task FindNextBookMarkAsync()
		{
			int index = 0;
			bool result;

			index = SelectedItemIndex;  // cannot access SelectedItemIndex inside Task.Run (Thread sync)
			BeginWork();
			result = await Task.Run<bool>(() => { return eventIndexerModule.FindNext(ref index, (item) => item.IsBookMarked); });
			if (result) SelectedItemIndex = index;
			EndWork();
		}

		#endregion







	}
}
