using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogInspectLib.Parsers;
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
using System.Windows.Threading;

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


		

		

		public LogFileViewModel(ILogger Logger,LogFile LogFile,IRegexBuilder RegexBuilder,IInlineColoringRuleDictionary InlineColoringRuleDictionary) :base(Logger)
		{
			AssertParameterNotNull("LogFile", LogFile);
			AssertParameterNotNull("RegexBuilder", RegexBuilder);
			AssertParameterNotNull("InlineColoringRuleDictionary", InlineColoringRuleDictionary);

			this.logFile = LogFile;


			this.Name = Path.GetFileName(LogFile.FileName);
			FindOptions = new FindOptions();
			FindOptions.Column = LogFile.FormatHandler.DefaultColumn;
			FormatHandlerName = LogFile.FormatHandler.Name;


			filterItemSourcesViewModel = new FilterItemSourcesViewModel(Logger,LogFile.Events, LogFile.FormatHandler.Columns);


			Columns = new ColumnsViewModel(Logger, LogFile.FormatHandler, filterItemSourcesViewModel, RegexBuilder, InlineColoringRuleDictionary);

			events = new EventCollectionViewModel(Logger,Columns,logFile.FormatHandler.EventColoringRules);

			this.FilteredEvents = new FilteredEventsViewModel(Logger);

			Severities = new SeveritiesViewModel(Logger, LogFile.FormatHandler.SeverityColumn);
			
			Markers = new MarkersViewModel(Logger,LogFile.FormatHandler.SeverityColumn);

		}





		#region filter events
		public async void Load()
		{
			await events.Load(logFile.Events);
			filterItemSourcesViewModel.Refresh();
			await Refresh();
		}

		public async Task Refresh()
		{
			FilteredEvents.Filters= Columns.Where(item => item.Filter != null).Select(item => item.Filter).ToArray();
			await FilteredEvents.Load(events);
			await Severities.Load(FilteredEvents);
			await Markers.Load(FilteredEvents);
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

			index =  await  FindPreviousAsync(StartIndex, (item) => Severity == item.GetEventValue(logFile.FormatHandler.SeverityColumn));
			return index;
		}
		public async Task<int> FindNextSeverityAsync(string Severity, int StartIndex)
		{
			int index ;

			index = await  FindNextAsync(StartIndex, (item) => Severity == item.GetEventValue(logFile.FormatHandler.SeverityColumn));
			return index;
		}
		#endregion

		#region bookmark
		public void ToogleBookMark()
		{
			if (FilteredEvents.SelectedItem == null) return;
			FilteredEvents.SelectedItem.IsBookMarked = !FilteredEvents.SelectedItem.IsBookMarked;
		}

		public async Task<int> FindPreviousBookMarkAsync(int StartIndex)
		{
			int index;

			index = await FindPreviousAsync(StartIndex, (item) => item.IsBookMarked );
			return index;
		}
		public async Task<int> FindNextBookMarkAsync(int StartIndex)
		{
			int index;

			index = await FindNextAsync(StartIndex, (item) => item.IsBookMarked );
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
