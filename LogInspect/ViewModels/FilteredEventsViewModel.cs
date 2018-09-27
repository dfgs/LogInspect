using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LogInspect.ViewModels
{
	public class FilteredEventsViewModel:CollectionViewModel<EventViewModel>
	{
		private int position;
		private IEventListModule eventList;

		private IEnumerable<ColumnViewModel> columns;
		private IEnumerable<EventColoringRule> eventColoringRules;

		public static readonly DependencyProperty TailProperty = DependencyProperty.Register("Tail", typeof(bool), typeof(FilteredEventsViewModel));
		public bool Tail
		{
			get { return (bool)GetValue(TailProperty); }
			set { SetValue(TailProperty, value); }
		}

		private Filter[] filters;

		public FilteredEventsViewModel(ILogger Logger , int RefreshInterval, IEventListModule EventList, IEnumerable<ColumnViewModel> Columns, IEnumerable<EventColoringRule> EventColoringRules) : base(Logger, RefreshInterval)
		{
			this.eventList = EventList;
			this.columns = Columns;this.eventColoringRules = EventColoringRules;
		}

		protected override void OnRefresh()
		{
			int target;
			int index;
			EventViewModel vm;

			lock (this)
			{
				index = this.Count;
				List<EventViewModel> list = new List<EventViewModel>();
				if (eventList.Count-position > 100) target = position+100;		// smooth list loading
				else target = eventList.Count;

				for (int t = position; t < target; t++)
				{
					vm = new EventViewModel(Logger,columns,eventColoringRules,  eventList[t]);
					vm.EventIndex = index; ;
					if (MustDiscard(vm)) continue;
					list.Add(vm);
					index++;
				}
				position = target;
				AddRange(list);
				if (Tail) Select(Count - 1);

			}
		}
		private bool MustDiscard(EventViewModel Event)
		{
			if (filters == null) return false;
			foreach(Filter filter in filters)
			{
				if (filter.MustDiscard(Event)) return true;
			}
			return false;
		}
		public void SetFilters(Filter[] Filters)
		{
			lock (this)
			{
				this.filters = Filters;
				position = 0;
				Clear();
			}
		}


	}
}
