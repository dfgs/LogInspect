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

		/*public string FormatHandlerName
		{
			get;
			private set;
		}*/

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

		private List<Event> events;
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


		

		

		public LogFileViewModel(ILogger Logger,LogFile LogFile) :base(Logger)
		{
			
			this.logFile = LogFile;



			this.Name = Path.GetFileName(LogFile.FileName);
			//this.severityColumn = FormatHandler.SeverityColumn;

			Log(LogLevels.Information, "Creating viewmodels");

			//FindOptions = new FindOptions();
			//FindOptions.Column = FormatHandler.DefaultColumn;


			/*Severities = new SeveritiesViewModel(Logger, FormatHandler.SeverityColumn, filterItemSourcesViewModel);

			Columns = new ColumnsViewModel(Logger, FormatHandler,filterItemSourcesViewModel,RegexBuilder,InlineColoringRuleDictionary);

			Markers = new MarkersViewModel(Logger,   Events, FormatHandler.EventColoringRules, FormatHandler.SeverityColumn);*/




		}


		public override void Dispose()
		{

			//filterItemSourcesViewModel.Dispose();//*/

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
