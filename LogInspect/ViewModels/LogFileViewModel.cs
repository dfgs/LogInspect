﻿using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspect.ViewModels.Modules;
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

		public string FormatHandlerName
		{
			get;
			private set;
		}

		private string severityColumn;

		public static readonly DependencyProperty FindOptionsProperty = DependencyProperty.Register("FindOptions", typeof(FindOptions), typeof(LogFileViewModel));
		public FindOptions FindOptions
		{
			get { return (FindOptions)GetValue(FindOptionsProperty); }
			set { SetValue(FindOptionsProperty, value); }
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

		private FilterItemSourcesViewModel filterItemSourcesViewModel;
		public SeveritiesViewModel Severities
		{
			get;
			private set;
		}

		public FilteredEventsViewModel Events
		{
			get;
			private set;
		}

		public MarkersViewModel Markers
		{
			get;
			private set;
		}


		private ILogBufferModule logBufferModule;
		public IBufferModuleViewModel LogBuffer
		{
			get;
			private set;
		}

		private EventListModule eventListModule;
		public IListModuleViewModel EventList
		{
			get;
			private set;
		}

		public StreamViewModel Stream
		{
			get;
			private set;
		}

		public LogFileViewModel(ILogger Logger,string FileName,FormatHandler FormatHandler,IRegexBuilder RegexBuilder, IInlineColoringRuleDictionary InlineColoringRuleDictionary, 
			int LoaderModuleLookupRetryDelay, int ViewModelRefreshInterval, int EventsViewModelRefreshInterval, int MaxEventsViewModelChunkSize) :base(Logger,-1)
		{
			Stream stream;
			IStringMatcher discardLineMatcher;
			IStringMatcher appendLineToPreviousMatcher;
			IStringMatcher appendLineToNextMatcher;
			ILogParser logParser;
			ILineReader lineReader;
			ILogReader logReader;

			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);
			this.FormatHandlerName = FormatHandler.Name;
			this.severityColumn = FormatHandler.SeverityColumn;

			Log(LogLevels.Information, "Creating modules");

			stream = new FileStream(FileName, FileMode.Open,FileAccess.Read,FileShare.ReadWrite);

			discardLineMatcher = FormatHandler.CreateDiscardLinesMatcher(RegexBuilder);
			appendLineToPreviousMatcher = FormatHandler.CreateAppendLineToPreviousMatcher(RegexBuilder);
			appendLineToNextMatcher = FormatHandler.CreateAppendLineToNextMatcher(RegexBuilder) ;

			logParser = FormatHandler.CreateLogParser(RegexBuilder);
			
			lineReader = new LineReader(stream, Encoding.Default, discardLineMatcher);
			logReader = new LogReader(lineReader, appendLineToPreviousMatcher, appendLineToNextMatcher);

			logBufferModule = new LogBufferModule(Logger,  LoaderModuleLookupRetryDelay, logReader);
			eventListModule = new EventListModule(Logger, LoaderModuleLookupRetryDelay, logBufferModule,logParser);

			Log(LogLevels.Information, "Creating viewmodels");

			FindOptions = new FindOptions();
			FindOptions.Column = FormatHandler.DefaultColumn;

			Stream = new StreamViewModel(Logger, ViewModelRefreshInterval, stream);

			filterItemSourcesViewModel =  new FilterItemSourcesViewModel(Logger, ViewModelRefreshInterval, eventListModule, FormatHandler.Columns);
			Severities = new SeveritiesViewModel(Logger, ViewModelRefreshInterval, FormatHandler.SeverityColumn, filterItemSourcesViewModel);

			Columns = new ColumnsViewModel(Logger, FormatHandler,filterItemSourcesViewModel,RegexBuilder,InlineColoringRuleDictionary);

			Events = new FilteredEventsViewModel(Logger, EventsViewModelRefreshInterval, eventListModule,Columns,FormatHandler.EventColoringRules,MaxEventsViewModelChunkSize);
			Markers = new MarkersViewModel(Logger, EventsViewModelRefreshInterval,  Events, FormatHandler.EventColoringRules, FormatHandler.SeverityColumn);


			LogBuffer = new BufferModuleViewModel<Log>(Logger, ViewModelRefreshInterval, logBufferModule);
			EventList = new ListModuleViewModel<Event>(Logger, ViewModelRefreshInterval, eventListModule);


			Log(LogLevels.Information, "Starting modules");
			logBufferModule.Start();
			eventListModule.Start();
		}


		public override void Dispose()
		{

			Log(LogLevels.Information, "Stopping modules");
			eventListModule.Stop();
			logBufferModule.Stop();

			filterItemSourcesViewModel.Dispose();//*/

		}

		



		#region filter events
		/*private void Columns_FilterChanged(object sender, EventArgs e)
		{
			Refresh();
		}*/
		public void Refresh()
		{
			Filter[] filters;
			
			filters= Columns.Where(item => item.Filter != null).Select(item => item.Filter).ToArray();
			Events.SetFilters(filters);
			Markers.Clear();
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
					ev = Events[Index];
					if (Dispatcher.Invoke<bool>(()=> Predicate(ev))) return Index;
					//if (Predicate(ev)) return Index;
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
				while (Index < Events.Count- 1)
				{
					Index++;
					ev = Events[Index];
					if (Dispatcher.Invoke<bool>(() => Predicate(ev))) return Index;
					//if (Predicate(ev)) return Index;
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

			index =  await  FindPreviousAsync(StartIndex, (item) => Severity == item.GetEventValue(severityColumn));
			return index;
		}
		public async Task<int> FindNextSeverityAsync(string Severity, int StartIndex)
		{
			int index ;

			index = await  FindNextAsync(StartIndex, (item) => Severity == item.GetEventValue(severityColumn));
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
			else newTime = Events[StartIndex].TimeStamp.AddMinutes(-1);

			index = await FindPreviousAsync(StartIndex, (item) => item.TimeStamp<=newTime);
			return index;
		}
		public async Task<int> IncMinutesAsync(int StartIndex)
		{
			int index;
			DateTime newTime;

			if (StartIndex < 0) newTime = DateTime.MinValue;
			else newTime = Events[StartIndex].TimeStamp.AddMinutes(1);

			index = await FindNextAsync(StartIndex, (item) => item.TimeStamp>=newTime);
			return index;
		}
		public async Task<int> DecHoursAsync(int StartIndex)
		{
			int index;
			DateTime newTime;

			if (StartIndex < 0) newTime = DateTime.MinValue;
			else newTime = Events[StartIndex].TimeStamp.AddHours(-1);

			index = await FindPreviousAsync(StartIndex, (item) => item.TimeStamp <= newTime);
			return index;
		}
		public async Task<int> IncHoursAsync(int StartIndex)
		{
			int index;
			DateTime newTime;

			if (StartIndex < 0) newTime = DateTime.MinValue;
			else newTime = Events[StartIndex].TimeStamp.AddHours(1);

			index = await FindNextAsync(StartIndex, (item) => item.TimeStamp >= newTime);
			return index;
		}
		#endregion






	}
}
