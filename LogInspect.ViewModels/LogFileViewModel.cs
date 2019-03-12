using LogInspect.Models;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogLib;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LogInspect.ViewModels
{
	public class LogFileViewModel:ViewModel
	{
		private LogFile logFile;

		public string FileName
		{
			get { return logFile.FileName; }
		}
		public string Name
		{
			get;
			private set;
		}

		public string FormatHandlerName
		{
			get;
			private set;
		}


		public FindOptions FindOptions
		{
			get;
			set;
		}

		public Statuses Status
		{
			get;
			set;
		}

		public ColumnsViewModel Columns
		{
			get;
			private set;
		}

		private FilterItemSourcesViewModel filterItemSourcesViewModel;
		public SeveritiesViewModel Severities
		{
			get;
			private set;
		}

		private EventCollectionViewModel events;
		public FilteredEventsViewModel FilteredEvents
		{
			get;
			private set;
		}

		public MarkersViewModel Markers
		{
			get;
			private set;
		}

		

		

		public LogFileViewModel(ILogger Logger,LogFile LogFile, IInlineParserBuilderModule InlineParserBuilderModule, IColorProviderModule ColorProviderModule) :base(Logger)
		{
			AssertParameterNotNull("LogFile", LogFile);
			AssertParameterNotNull("InlineParserBuilderModule", InlineParserBuilderModule);
			AssertParameterNotNull("ColorProviderModule", ColorProviderModule);

			this.logFile = LogFile;
			

			this.Name = Path.GetFileName(LogFile.FileName);
			FindOptions = new FindOptions();
			FindOptions.Column = LogFile.FormatHandler.DefaultColumn;
			FormatHandlerName = LogFile.FormatHandler.Name;

			// loaded on opening
			filterItemSourcesViewModel = new FilterItemSourcesViewModel(Logger, LogFile.FormatHandler.Columns);
			Columns = new ColumnsViewModel(Logger, LogFile.FormatHandler, filterItemSourcesViewModel, InlineParserBuilderModule, ColorProviderModule);
			events = new EventCollectionViewModel(Logger, ColorProviderModule, Columns);

			// loaded on refresh
			this.FilteredEvents = new FilteredEventsViewModel(Logger);
			Severities = new SeveritiesViewModel(Logger, LogFile.FormatHandler.SeverityColumn);
			Markers = new MarkersViewModel(Logger, LogFile.FormatHandler.SeverityColumn);
		}





		#region filter events
		public async Task Load()
		{
			await filterItemSourcesViewModel.Load(logFile.Events);
			await Columns.LoadModels(logFile.FormatHandler.Columns);
			await events.LoadModels(logFile.Events);

			await Refresh();
		}

		public async Task ReloadEvents()
		{
			await events.LoadModels(logFile.Events);

			await Refresh();
		}


		public async Task Refresh()
		{
			FilteredEvents.Filters= Columns.Where(item => item.Filter != null).Select(item => item.Filter).ToArray();
			await FilteredEvents.LoadModels(events);
			await Severities.LoadModels(FilteredEvents);
			await Markers.LoadModels(FilteredEvents);
		}

		#endregion

		#region generic search methods
		public async Task<int> FindPreviousAsync(int Index, Func<EventViewModel, bool> Predicate)
		{
			int result;
			EventViewModel ev;
			Status = Statuses.Searching;
			result= await Task.Run<int>(() =>
			{
				while (Index > 0)
				{
					Index--;
					ev = FilteredEvents[Index];
					if (Predicate(ev)) return Index;
				}
				return -1;
			});
			Status = Statuses.Idle;
			return result;
		}
		public async Task<int> FindNextAsync(int Index, Func<EventViewModel, bool> Predicate)
		{
			int result;
			EventViewModel ev;
			Status = Statuses.Searching;
			result = await Task.Run<int>(() =>
			{
				while (Index < FilteredEvents.Count- 1)
				{
					Index++;
					ev = FilteredEvents[Index];
					if (Predicate(ev)) return Index;
				}
				return -1;
			});
			Status = Statuses.Idle;
			return result;
		}
		#endregion

		#region find severities
		public async Task<int> FindPreviousSeverityAsync(string Severity,int StartIndex)
		{
			int index;

			index =  await  FindPreviousAsync(StartIndex, (item) => Severity == item[logFile.FormatHandler.SeverityColumn].ToString());
			return index;
		}
		public async Task<int> FindNextSeverityAsync(string Severity, int StartIndex)
		{
			int index ;

			index = await  FindNextAsync(StartIndex, (item) => Severity == item[logFile.FormatHandler.SeverityColumn].ToString());
			return index;
		}
		#endregion

		#region bookmark
		public void ToogleBookMark()
		{
			if (FilteredEvents.SelectedItem == null) return;
			FilteredEvents.SelectedItem["BookMarked"].Value = !(bool)FilteredEvents.SelectedItem["BookMarked"].Value;
		}

		public async Task<int> FindPreviousBookMarkAsync(int StartIndex)
		{
			int index;

			index = await FindPreviousAsync(StartIndex, (item) => (bool)item["BookMarked"].Value);
			return index;
		}
		public async Task<int> FindNextBookMarkAsync(int StartIndex)
		{
			int index;

			index = await FindNextAsync(StartIndex, (item) => (bool)item["BookMarked"].Value );
			return index;
		}

		#endregion

		#region timeline
		public async Task<int> DecMinutesAsync(int StartIndex)
		{
			int index;
			DateTime newTime;

			if (StartIndex < 0) newTime = DateTime.MinValue;
			else newTime = FilteredEvents[StartIndex].TimeStamp.AddMinutes(-1);

			index = await FindPreviousAsync(StartIndex, (item) => item.TimeStamp<=newTime);
			return index;
		}
		public async Task<int> IncMinutesAsync(int StartIndex)
		{
			int index;
			DateTime newTime;

			if (StartIndex < 0) newTime = DateTime.MinValue;
			else newTime = FilteredEvents[StartIndex].TimeStamp.AddMinutes(1);

			index = await FindNextAsync(StartIndex, (item) => item.TimeStamp>=newTime);
			return index;
		}
		public async Task<int> DecHoursAsync(int StartIndex)
		{
			int index;
			DateTime newTime;

			if (StartIndex < 0) newTime = DateTime.MinValue;
			else newTime = FilteredEvents[StartIndex].TimeStamp.AddHours(-1);

			index = await FindPreviousAsync(StartIndex, (item) => item.TimeStamp <= newTime);
			return index;
		}
		public async Task<int> IncHoursAsync(int StartIndex)
		{
			int index;
			DateTime newTime;

			if (StartIndex < 0) newTime = DateTime.MinValue;
			else newTime = FilteredEvents[StartIndex].TimeStamp.AddHours(1);

			index = await FindNextAsync(StartIndex, (item) => item.TimeStamp >= newTime);
			return index;
		}
		#endregion






	}
}
