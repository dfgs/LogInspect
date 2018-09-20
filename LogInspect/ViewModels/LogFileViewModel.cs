using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspect.ViewModels.Loaders;
using LogInspectLib;
using LogInspectLib.Loaders;
using LogInspectLib.Parsers;
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

		private IEventLoader eventLoader;
		private ILogLoader logLoader;
		private ILineLoader lineLoader;

		private ILineLoaderModule lineLoaderModule;
		public ILoaderViewModel LineLoader
		{
			get;
			private set;
		}
		private ILogLoaderModule logLoaderModule;
		public ILoaderViewModel LogLoader
		{
			get;
			private set;
		}
		private IEventLoaderModule eventLoaderModule;
		public ILoaderViewModel EventLoader
		{
			get;
			private set;
		}

		public StreamViewModel Stream
		{
			get;
			private set;
		}

		public LogFileViewModel(ILogger Logger,string FileName,FormatHandler FormatHandler,IRegexBuilder RegexBuilder, int LoaderModuleLookupRetryDelay,int ViewModelRefreshInterval) :base(Logger,-1)
		{
			Stream stream;
			IStringMatcher discardLineMatcher;
			IStringMatcher appendLineToPreviousMatcher;
			IStringMatcher appendLineToNextMatcher;
			ILogParser logParser;

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
			
			lineLoader = new LineLoader(stream, Encoding.Default, discardLineMatcher);
			logLoader = new LogLoader(lineLoader, appendLineToPreviousMatcher, appendLineToNextMatcher);
			eventLoader = new EventLoader(logLoader, logParser);

			lineLoaderModule = new LineLoaderModule(Logger, lineLoader, null,LoaderModuleLookupRetryDelay);
			logLoaderModule = new LogLoaderModule(Logger, logLoader, lineLoaderModule.ProceededEvent, LoaderModuleLookupRetryDelay);
			eventLoaderModule = new EventLoaderModule(Logger, eventLoader,logLoaderModule.ProceededEvent, LoaderModuleLookupRetryDelay);

			Log(LogLevels.Information, "Creating viewmodels");

			FindOptions = new FindOptions();
			FindOptions.Column = FormatHandler.DefaultColumn;

			Stream = new StreamViewModel(Logger, ViewModelRefreshInterval, stream);

			filterItemSourcesViewModel =  new FilterItemSourcesViewModel(Logger, ViewModelRefreshInterval, eventLoader, FormatHandler.Columns);
			Severities = new SeveritiesViewModel(Logger, ViewModelRefreshInterval, FormatHandler.SeverityColumn, filterItemSourcesViewModel);

			Columns = new ColumnsViewModel(Logger, FormatHandler,filterItemSourcesViewModel);

			Events = new EventsViewModel(Logger, ViewModelRefreshInterval, eventLoader,Columns,FormatHandler.EventColoringRules);
			Markers = null;//new MarkersViewModel(Logger, indexerBufferModule, FormatHandler.EventColoringRules, FormatHandler.SeverityColumn);


			LineLoader = new LoaderViewModel(Logger, ViewModelRefreshInterval, lineLoaderModule);
			LogLoader = new LoaderViewModel(Logger, ViewModelRefreshInterval, logLoaderModule);
			EventLoader = new LoaderViewModel(Logger, ViewModelRefreshInterval, eventLoaderModule);


			Log(LogLevels.Information, "Starting modules");
			lineLoaderModule.Start();
			logLoaderModule.Start();
			eventLoaderModule.Start();

		}


		public override void Dispose()
		{

			Log(LogLevels.Information, "Stopping modules");
			eventLoaderModule.Stop();
			logLoaderModule.Stop();
			lineLoaderModule.Stop();

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

			//SelectedItemIndex = -1;
			filters= Columns.Where(item => item.Filter != null).Select(item => item.Filter).ToArray();
			//eventIndexerModule.SetFilters(filters  );
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
					//if (Dispatcher.Invoke<bool>(()=> Predicate(ev))) return Index;
					if (Predicate(ev)) return Index;
				}
				return -1;
			});
		}
		public async Task<int> FindNextAsync(int Index, Func<EventViewModel, bool> Predicate)
		{
			EventViewModel ev;

			return await Task.Run<int>(() =>
			{
				while (Index < Events.Count- 1)
				{
					Index++;
					ev = Events[Index];
					//if (Dispatcher.Invoke<bool>(() => Predicate(ev))) return Index;
					if (Predicate(ev)) return Index;
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
			index =  await  FindPreviousAsync(StartIndex, (item) => Severity.Equals( item[severityColumn]));
			Status = Statuses.Idle;
			return index;
		}
		public async Task<int> FindNextSeverityAsync(object Severity, int StartIndex)
		{
			int index ;

			Status = Statuses.Searching;
			index = await  FindNextAsync(StartIndex, (item) =>  Severity.Equals(item[severityColumn]));
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
